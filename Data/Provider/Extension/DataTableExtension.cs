using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduleImporter.AddIn.Models.WorkSchedule;

namespace WorkScheduleImporter.AddIn.Data.Provider.Extension
{
    public static class DataTableExtension
    {
        public static IEnumerable<ImportableWorkScheduleUnitModel> ToUnitWorkScheduleModelList(this DataTable dt)
        {
            List<ImportableWorkScheduleUnitModel> workkShiftModelList = new List<ImportableWorkScheduleUnitModel>();
            int row = 2;

            foreach (DataRow dr in dt.Rows)
            {
                ImportableWorkScheduleUnitModel workSchedule = new ImportableWorkScheduleUnitModel();

                workSchedule.ShiftDateCellValue = GetCellValue(dr, ImportableWorkScheduleUnitModel.SHIFT_DATE_COL_NUM);
                if (String.IsNullOrEmpty(workSchedule.ShiftDateCellValue))
                    break;

                workSchedule.WorkshiftLabel = GetCellValue(dr, ImportableWorkScheduleUnitModel.WORKSHIF_LABEL_COL_NUM);
                workSchedule.UnitId = GetCellValue(dr, ImportableWorkScheduleUnitModel.UNIT_ID_COL_NUM);
                workSchedule.CellPhone = GetCellValue(dr, ImportableWorkScheduleUnitModel.CELL_PHONE_COL_NUM);
                workSchedule.StationName = GetCellValue(dr, ImportableWorkScheduleUnitModel.STATION_NAME_COL_NUM);
                workSchedule.StationId = GetCellValue(dr, ImportableWorkScheduleUnitModel.STATION_ID_COL_NUM);
                workSchedule.Remark = GetCellValue(dr, ImportableWorkScheduleUnitModel.REMARK_COL_NUM);
                workSchedule.Doctor = GetCellValue(dr, ImportableWorkScheduleUnitModel.DOCTOR_COL_NUM);
                workSchedule.Nurse = GetCellValue(dr, ImportableWorkScheduleUnitModel.NURSE_COL_NUM);
                workSchedule.FirstAuxiliar = GetCellValue(dr, ImportableWorkScheduleUnitModel.FIRST_AUXILIAR_COL_NUM);
                workSchedule.SecondAuxiliar = GetCellValue(dr, ImportableWorkScheduleUnitModel.SECOND_AUXILIAR_COL_NUM);
                workSchedule.ThirdAuxiliar = GetCellValue(dr, ImportableWorkScheduleUnitModel.THIRD_AUXILIAR_COL_NUM);
                workSchedule.Driver = GetCellValue(dr, ImportableWorkScheduleUnitModel.DRIVER_COL_NUM);
                workSchedule.IsURAM = GetCellValue(dr, ImportableWorkScheduleUnitModel.IS_URAM_COL_NUM);
                workSchedule.DateFrequence = GetCellValue(dr, ImportableWorkScheduleUnitModel.DATE_FREQUENCE_COL_NUM);
                workSchedule.Row = row++;

                workkShiftModelList.Add(workSchedule);
            }
            return workkShiftModelList;
        }

        public static WorkScheduleConfig ToWorkScheduleConfig(this DataTable dt)
        {
            WorkScheduleConfig workScheduleConfig = new WorkScheduleConfig();

            workScheduleConfig.IgnoreDate = GetCellValue(
                dt.Rows[WorkScheduleConfig.IGNORE_DATE_ROW_NUM-1],
                WorkScheduleConfig.IGNORE_DATE_COL_NUM).ToUpper() == 
                    Properties.Settings.Default.CONFIG_SHEET_FALSE_VALUE;

            workScheduleConfig.IgnoreWorkShift = GetCellValue(
                dt.Rows[WorkScheduleConfig.IGNORE_WORKSHIFT_ROW_NUM-1],
                WorkScheduleConfig.IGNORE_WORKSHIFT_COL_NUM).ToUpper() ==
                    Properties.Settings.Default.CONFIG_SHEET_TRUE_VALUE;

            return workScheduleConfig;
        }


        private static string GetCellValue(DataRow dr, int dateColNum)
        {
            if (dateColNum > 0)
                return dr[dateColNum - 1].ToString();
            return null;
        }
    }
}
