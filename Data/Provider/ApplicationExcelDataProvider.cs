using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduleImporter.AddIn.Data.Provider.Interface;
using Excel = Microsoft.Office.Interop.Excel;
using WorkScheduleImporter.AddIn.Models.WorkSchedule;

namespace WorkScheduleImporter.AddIn.Data.Provider
{
    public class ApplicationExcelDataProvider : IExcelDataProvider
    {
        string _fullPathFile;
        string _sheetPassword;

        public ApplicationExcelDataProvider(string fullPathFile, string sheetPassword) : base(fullPathFile, sheetPassword)
        {

        }

        public override DataTable ReadSheet(string sheetName)
        {
            DataTable dt = new DataTable(sheetName);

            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(this.FullPathFile);

            int workScheduleSheetIndex = GetSheeIndex(xlWorkbook, sheetName);

            if (workScheduleSheetIndex == 0)
            {
                throw new SheetNotFoundException();
            }

            Excel._Worksheet xlWorksheet = (Excel._Worksheet)xlWorkbook.Sheets[workScheduleSheetIndex];
            Excel.Range xlRange = xlWorksheet.UsedRange;

            int rowCount = xlRange.Rows.Count;
            int colCount = xlRange.Columns.Count;

            for (int row = 2; row <= rowCount; row++)
            {
                dt.Rows.Add(xlRange.Rows[row]);
            }

            for (int i = 0; i < colCount; i++)
            {
                dt.Columns[i].ColumnName = GetCellValue(xlRange, 1, i + 1);
            }

            return dt;
        }

        private static string GetCellValue(Excel.Range xlRange, int row, int col)
        {
            if (col > 0 && xlRange.Cells[row, col] != null && xlRange.Cells[row, col].Value2 != null)
            {
                return xlRange.Cells[row, col].Value2.ToString();
            }
            return null;
        }

        private int GetSheeIndex(Excel.Workbook xlWorkbook, string sheetName)
        {
            int workScheduleSheetIndex = 0;

            for (int i = 1; i < xlWorkbook.Sheets.Count; i++)
            {
                if (((Excel._Worksheet)xlWorkbook.Sheets[i]).Name.ToUpper() == sheetName.ToUpper())
                {
                    workScheduleSheetIndex = i;
                    break;
                }
            }
            return workScheduleSheetIndex;
        }

        private string[,] GetArrayOfStrings(DataTable dt)
        {
            string[,] array = new string[dt.Rows.Count, dt.Columns.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    array[i,j] = dt.Rows[i][j].ToString();
                }
            }

            return array;
        }

        public override void WriteSheet(string sheetName, DataTable dt)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(this.FullPathFile, ReadOnly: false);

            int workScheduleSheetIndex = GetSheeIndex(xlWorkbook, sheetName);

            if (workScheduleSheetIndex == 0)
            {
                throw new SheetNotFoundException();
            }

            Excel._Worksheet xlWorksheet = (Excel._Worksheet)xlWorkbook.Sheets[workScheduleSheetIndex];
            if (!String.IsNullOrEmpty(this.SheetPassword))
            {
                xlWorksheet.Unprotect(this.SheetPassword);
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    xlWorksheet.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
                }
            }

            if (!String.IsNullOrEmpty(this.SheetPassword))
            {
                xlWorksheet.Protect(this.SheetPassword);
            }

            xlWorkbook.Save();
            xlWorkbook.Close();
        }
    }
}
