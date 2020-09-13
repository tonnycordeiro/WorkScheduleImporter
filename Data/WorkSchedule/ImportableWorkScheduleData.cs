using Intergraph.IPS.CADCore;
using log4net;
using WorkScheduleImporter.AddIn.Models;
using WorkScheduleImporter.AddIn.Models.WorkSchedule;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using WorkScheduleImporter.AddIn.Data.Provider;
using WorkScheduleImporter.AddIn.Data.Provider.Extension;
using WorkScheduleImporter.AddIn.Data.Provider.Interface;
using Government.EmergencyDepartment.AddIn.Data.WorkSchedule;

namespace WorkScheduleImporter.AddIn.Data.WorkSchedule
{
    public class ImportableWorkScheduleData
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void UpdateWorkScheduleSheets(string fullPathFile, string password)
        {
            try
            {
                ApplicationExcelDataProvider appXlsProvider = new ApplicationExcelDataProvider(fullPathFile, password);
                ImportableWorkScheduleData.UpdateSheets(appXlsProvider);
            }
            catch
            {
                throw;
            }
        }


        private static void UpdateSheets(IExcelDataProvider appXlsProvider)
        {
            List<string> sheetNamesList = new List<string>() { Properties.Settings.Default.SHEET_NAME_STATION,
                                                                   Properties.Settings.Default.SHEET_NAME_UNIT,
                                                                   Properties.Settings.Default.SHEET_NAME_EMPLOYEE};

            foreach (string sheet in sheetNamesList)
            {
                DataTable dt = WorkScheduleData.GetDataTableToFillWorkScheduleSheet(sheet);
                appXlsProvider.WriteSheet(sheet, dt);
            }
        }


        private static string GetCellValue(Excel.Range xlRange, int row, int col)
        {
            if (col > 0 && xlRange.Cells[row, col] != null && xlRange.Cells[row, col].Value2 != null)
            {
                return xlRange.Cells[row, col].Value2.ToString();
            }
            return null;
        }

        private static DateTime ConvertCellValueToDate(Excel.Range xlRange, int row, int col)
        {
            string value = GetCellValue(xlRange, row, col);
            double dateExcelNumber;

            if (value == null)
                throw new ArgumentNullException();

            if (!Double.TryParse(value, out dateExcelNumber))
                throw new InvalidCastException();

            return DateTime.FromOADate(dateExcelNumber);
        }

        public static IEnumerable<ImportableWorkScheduleUnitModel> ImportWorkSchedule(string fullPathToExcelFile)
        {
            try
            {
                DataTable unitWorkScheduleDataTable = GetSheetFromXls(fullPathToExcelFile, Properties.Settings.Default.SHEET_NAME_WORK_SCHEDULE_MAIN);
                return unitWorkScheduleDataTable.ToUnitWorkScheduleModelList();
            }
            catch
            {
                throw;
            }
        }

        public static WorkScheduleConfig ImportWorkScheduleConfig(string fullPathToExcelFile)
        {
            try
            {
                DataTable unitWorkScheduleDataTable = GetSheetFromXls(fullPathToExcelFile, Properties.Settings.Default.SHEET_NAME_CONFIG);
                return unitWorkScheduleDataTable.ToWorkScheduleConfig();
            }
            catch
            {
                throw;
            }
        }

        private static DataTable GetSheetFromXls(string fullPathToExcelFile, string sheetName)
        {
            DataTable dtOfWorkScheduleForUnit = new DataTable();
            try
            {
                OleDbExcelDataProvider oleXlsProvider = new OleDbExcelDataProvider(fullPathToExcelFile, null);
                dtOfWorkScheduleForUnit = oleXlsProvider.ReadSheet(sheetName);
            }
            catch (InvalidOperationException exception)
            {
                try
                {
                    ApplicationExcelDataProvider appXlsProvider = new ApplicationExcelDataProvider(fullPathToExcelFile, null);
                    dtOfWorkScheduleForUnit = appXlsProvider.ReadSheet(sheetName);
                }
                catch (Exception exceptionFromExcel)
                {
                    throw exceptionFromExcel;
                }
            }
            catch
            {
                throw;
            }

            return dtOfWorkScheduleForUnit;
        }

        public static string GetWorkScheduleSheetName()
        {
            return Properties.Settings.Default.SHEET_NAME_WORK_SCHEDULE_MAIN;
        }

        public static WorkScheduleConfig LoadWorkScheduleConfig(string fullPathToExcelFile)
        {
            DataTable dtOfWorkScheduleForUnit = new DataTable();
            try
            {
                OleDbExcelDataProvider oleXlsProvider = new OleDbExcelDataProvider(fullPathToExcelFile, null);
                dtOfWorkScheduleForUnit = oleXlsProvider.ReadSheet(Properties.Settings.Default.SHEET_NAME_WORK_SCHEDULE_MAIN);
            }
            catch (InvalidOperationException exception)
            {
                try
                {
                    ApplicationExcelDataProvider appXlsProvider = new ApplicationExcelDataProvider(fullPathToExcelFile, null);
                    dtOfWorkScheduleForUnit = appXlsProvider.ReadSheet(Properties.Settings.Default.SHEET_NAME_WORK_SCHEDULE_MAIN);
                }
                catch (Exception exceptionFromExcel)
                {
                    throw exceptionFromExcel;
                }
            }
            catch
            {
                throw;
            }
            return null;
        }
    }
}
