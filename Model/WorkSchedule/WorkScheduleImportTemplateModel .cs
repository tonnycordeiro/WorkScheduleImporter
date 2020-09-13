using System;
using System.Linq;
using System.Reflection;
using System.Drawing;
using System.Collections.Generic;
using WorkScheduleImporter.AddIn.Models.WorkSchedule.WorkShiftPeriod;
using System.Text;
using Government.EmergencyDepartment.AddIn.Models.WorkSchedule;

namespace WorkScheduleImporter.AddIn.Models.WorkSchedule
{
    public class WorkScheduleImportTemplateModel : Observable
    {
        #region Attributes
        private List<ImportableWorkScheduleUnitModel> _workScheduleForUnitList;

        private WorkScheduleConfig _workScheduleConfig;

        #endregion

        #region Constructors
        public WorkScheduleImportTemplateModel()
        {
            _workScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>();
            _workScheduleConfig = new WorkScheduleConfig()
                                                        {
                                                            IgnoreDate = false,
                                                            IgnoreWorkShift = false
                                                        };
        }
        #endregion

        #region Properties

        public List<ImportableWorkScheduleUnitModel> WorkScheduleForUnitList
        {
            get { return _workScheduleForUnitList; }
            set { _workScheduleForUnitList = value;
                OnPropertyChanged("WorkScheduleForUnitList");
            }
        }

        public WorkScheduleConfig WorkScheduleConfig
        {
            get { return _workScheduleConfig; }
            set { _workScheduleConfig = value; }
        }

        #endregion

        #region Methods

        public void UpdateReportValidationWithRepeatedUnits(ref ReportValidationModel reportValidation)//out List<int> repeatedUnitRowList
        {
            List<ImportableWorkScheduleUnitModel> workScheduleForChecking;

            workScheduleForChecking = WorkScheduleForUnitList.Where(u => !String.IsNullOrEmpty(u.UnitId) && !String.IsNullOrEmpty(u.WorkshiftLabel) && u.ShiftDate.HasValue)
                                                                    .OrderBy(u => u.UnitId).ThenBy(u => u.ShiftDate.Value).ThenBy(u => u.WorkshiftLabel).ToList();
            for (int i = 1; i < workScheduleForChecking.Count; i++)
            {
                if (workScheduleForChecking[i].UnitId.Equals(workScheduleForChecking[i - 1].UnitId) &&
                   workScheduleForChecking[i].ShiftDate.Value.Equals(workScheduleForChecking[i - 1].ShiftDate.Value) &&
                   workScheduleForChecking[i].WorkshiftLabel.Equals(workScheduleForChecking[i - 1].WorkshiftLabel))
                {
                    reportValidation.AddReportItem(
                        WorkScheduleValidationType.UNIT_DUPLICATED_AT_DAY,
                        GetConflictingDataMessage(workScheduleForChecking[i - 1].Row, workScheduleForChecking[i].Row,
                        ImportableWorkScheduleUnitModel.colNames[ImportableWorkScheduleUnitModel.UNIT_ID_COL_NUM], workScheduleForChecking[i].UnitId),
                        new List<string>() { workScheduleForChecking[i - 1].ID, workScheduleForChecking[i].ID});
                    //repeatedUnitRowList.Add(workScheduleForChecking)
                }
            }
        }

        public void UpdateReportValidationWithRepeatedCrewMembers(ref ReportValidationModel reportValidation)
        {
            //repeatedCrewMemberRowList = new List<int>();
            List<ImportableWorkScheduleUnitModel> workScheduleAdaptedForOneMember = new List<ImportableWorkScheduleUnitModel>();

            foreach (ImportableWorkScheduleUnitModel workScheduleUnitOriginal in 
                WorkScheduleForUnitList.Where(u => !String.IsNullOrEmpty(u.WorkshiftLabel) && u.ShiftDate.HasValue))
            {
                CloneWorkScheduleWithDateValues(driverEmployeeId: workScheduleUnitOriginal.Doctor, workScheduleAdaptedForOneMember: workScheduleAdaptedForOneMember, workScheduleUnitOriginal: workScheduleUnitOriginal);
                CloneWorkScheduleWithDateValues(driverEmployeeId: workScheduleUnitOriginal.Nurse, workScheduleAdaptedForOneMember: workScheduleAdaptedForOneMember, workScheduleUnitOriginal: workScheduleUnitOriginal);
                CloneWorkScheduleWithDateValues(driverEmployeeId: workScheduleUnitOriginal.Driver, workScheduleAdaptedForOneMember: workScheduleAdaptedForOneMember, workScheduleUnitOriginal: workScheduleUnitOriginal);
                CloneWorkScheduleWithDateValues(driverEmployeeId: workScheduleUnitOriginal.FirstAuxiliar, workScheduleAdaptedForOneMember: workScheduleAdaptedForOneMember, workScheduleUnitOriginal: workScheduleUnitOriginal);
                CloneWorkScheduleWithDateValues(driverEmployeeId: workScheduleUnitOriginal.SecondAuxiliar, workScheduleAdaptedForOneMember: workScheduleAdaptedForOneMember, workScheduleUnitOriginal: workScheduleUnitOriginal);
                CloneWorkScheduleWithDateValues(driverEmployeeId: workScheduleUnitOriginal.ThirdAuxiliar, workScheduleAdaptedForOneMember: workScheduleAdaptedForOneMember, workScheduleUnitOriginal: workScheduleUnitOriginal);
            }

            workScheduleAdaptedForOneMember = workScheduleAdaptedForOneMember.Distinct().OrderBy(u => u.Driver).ThenBy(u => u.Row).ThenBy(u => u.WorkshiftLabel).ThenBy(u => u.ShiftDate.Value).ToList();

            for (int i = 1; i < workScheduleAdaptedForOneMember.Count; i++)
            {
                if (workScheduleAdaptedForOneMember[i].Driver.Equals(workScheduleAdaptedForOneMember[i - 1].Driver) &&
                   workScheduleAdaptedForOneMember[i].ShiftDate.Value.Equals(workScheduleAdaptedForOneMember[i - 1].ShiftDate.Value) &&
                   workScheduleAdaptedForOneMember[i].WorkshiftLabel.Equals(workScheduleAdaptedForOneMember[i - 1].WorkshiftLabel) &&
                   !workScheduleAdaptedForOneMember[i].Row.Equals(workScheduleAdaptedForOneMember[i - 1].Row))
                {
                    reportValidation.AddReportItem(
                        WorkScheduleValidationType.EMPLOYEE_DUPLICATED_AT_DAY,
                        GetConflictingDataMessage(workScheduleAdaptedForOneMember[i - 1].Row, workScheduleAdaptedForOneMember[i].Row,
                        Properties.Resources.EMPLOYEE_LABEL, workScheduleAdaptedForOneMember[i].Driver),
                        new List<string>() { workScheduleAdaptedForOneMember[i - 1].ID, workScheduleAdaptedForOneMember[i].ID });
                }
            }
            //return repeatedCrewMemberRowList.Count > 0;
        }

        public void UpdateReportValidationWithInvalidUnits(List<string> validUnitList, ref ReportValidationModel reportValidation)
        {
            List<string> invalidUnitList = WorkScheduleForUnitList.Select(wsU => wsU.UnitId).Except(validUnitList).ToList<string>();

            if (invalidUnitList.Count == 0)
                return;

            foreach (ImportableWorkScheduleUnitModel workScheduleWithInvalidUnit in
                WorkScheduleForUnitList.Where(u => invalidUnitList.Contains(u.UnitId)))
            {
                reportValidation.AddReportItem(
                        WorkScheduleValidationType.INVALID_UNIT,
                        workScheduleWithInvalidUnit.GetMessage(ImportableWorkScheduleUnitModel.colNames[ImportableWorkScheduleUnitModel.UNIT_ID_COL_NUM],
                                                               workScheduleWithInvalidUnit.UnitId), new List<string>() { workScheduleWithInvalidUnit.ID });
            }
        }

        public void UpdateReportValidationWithInvalidEmployees(List<string> validEmployeeIdList, ref ReportValidationModel reportValidation)
        {
            List<string> allEmployeeIdsOfWorkSchedule = WorkScheduleForUnitList.Select(wsU => wsU.Driver)
                                             .Union(WorkScheduleForUnitList.Select(wsU => wsU.ThirdAuxiliar)
                                                .Union(WorkScheduleForUnitList.Select(wsU => wsU.SecondAuxiliar)
                                                    .Union(WorkScheduleForUnitList.Select(wsU => wsU.FirstAuxiliar)
                                                        .Union(WorkScheduleForUnitList.Select(wsU => wsU.Nurse)
                                                            .Union(WorkScheduleForUnitList.Select(wsU => wsU.Doctor)
                                                ))))).ToList<string>();

            List<string> invalidEmployeeIdList = allEmployeeIdsOfWorkSchedule.Except(
                validEmployeeIdList.Union(new List<string>() { null, String.Empty})).ToList<string>();

            if (invalidEmployeeIdList.Count == 0)
                return;

            foreach (ImportableWorkScheduleUnitModel workScheduleWithInvalidEmployeeId in WorkScheduleForUnitList)
            {
                List<string> employeeInvalidListByUnit = new List<string>();

                if(invalidEmployeeIdList.Contains(workScheduleWithInvalidEmployeeId.Driver))
                    employeeInvalidListByUnit.Add(workScheduleWithInvalidEmployeeId.Driver);
                if (invalidEmployeeIdList.Contains(workScheduleWithInvalidEmployeeId.Doctor))
                    employeeInvalidListByUnit.Add(workScheduleWithInvalidEmployeeId.Doctor);
                if (invalidEmployeeIdList.Contains(workScheduleWithInvalidEmployeeId.Nurse))
                    employeeInvalidListByUnit.Add(workScheduleWithInvalidEmployeeId.Nurse);
                if (invalidEmployeeIdList.Contains(workScheduleWithInvalidEmployeeId.FirstAuxiliar))
                    employeeInvalidListByUnit.Add(workScheduleWithInvalidEmployeeId.FirstAuxiliar);
                if (invalidEmployeeIdList.Contains(workScheduleWithInvalidEmployeeId.SecondAuxiliar))
                    employeeInvalidListByUnit.Add(workScheduleWithInvalidEmployeeId.SecondAuxiliar);
                if (invalidEmployeeIdList.Contains(workScheduleWithInvalidEmployeeId.ThirdAuxiliar))
                    employeeInvalidListByUnit.Add(workScheduleWithInvalidEmployeeId.ThirdAuxiliar);

                if (employeeInvalidListByUnit.Count > 0)
                    reportValidation.AddReportItem(
                            WorkScheduleValidationType.INVALID_EMPLOYEE_ID,
                            workScheduleWithInvalidEmployeeId.GetMessage(Properties.Resources.EMPLOYEE_LABEL, String.Join(",", employeeInvalidListByUnit)),
                            new List<string>() { workScheduleWithInvalidEmployeeId.ID});
            }
        }

        public void UpdateReportValidationWithInvalidStations(List<string> validStationIdList, ref ReportValidationModel reportValidation)
        {
            List<string> invalidStationIdList = WorkScheduleForUnitList.Select(wsU => wsU.StationId).Except(validStationIdList).ToList<string>();

            if (invalidStationIdList.Count == 0)
                return;

            foreach (ImportableWorkScheduleUnitModel workScheduleWithInvalidUnit in
                WorkScheduleForUnitList.Where(u => invalidStationIdList.Contains(u.StationId)))
            {
                reportValidation.AddReportItem(
                        WorkScheduleValidationType.INVALID_STATION,
                        workScheduleWithInvalidUnit.GetMessage(ImportableWorkScheduleUnitModel.colNames[ImportableWorkScheduleUnitModel.STATION_NAME_COL_NUM],
                                                               workScheduleWithInvalidUnit.StationName ?? String.Empty),
                                                               new List<string>() { workScheduleWithInvalidUnit.ID });
            }
        }

        public void UpdateReportValidationAlertingConflictsWithPreviousData(List<WorkScheduleUnitModel> unitForceMapList, List<WorkShiftModel> availableWorkShiftList, ref ReportValidationModel reportValidation)
        {
            foreach (WorkScheduleUnitModel oldUnitForceMap in unitForceMapList)
            {
                ImportableWorkScheduleUnitModel workScheduleForUnit = WorkScheduleForUnitList
                    .Where(ws => ws.ShiftDate.HasValue &&
                                    ws.ShiftDate.Value.Date.CompareTo(oldUnitForceMap.ShiftDate.Date) == 0 &&
                                    ws.WorkshiftLabel == oldUnitForceMap.CurrentWorkShift.Label &&
                                    ws.UnitId == oldUnitForceMap.UnitId
                            ).FirstOrDefault();

                if (workScheduleForUnit == null)
                    continue;

                WorkScheduleUnitModel newUnitForceMap = workScheduleForUnit.ToWorkScheduleUnitModel(availableWorkShiftList);

                // necessary condition because the old cell phone takes the number registered in the Active Unit
                /*if(!String.IsNullOrEmpty(newUnitForceMap.CellPhone))
                    UpdateReportValidationWithReplacingData(oldUnitForceMap.CellPhone, newUnitForceMap.CellPhone, workScheduleForUnit,
                                                              WorkScheduleForUnitModel.colNames[WorkScheduleForUnitModel.CELL_PHONE_COL_NUM], ref reportValidation);

                UpdateReportValidationWithReplacingData(oldUnitForceMap.Remarks, newUnitForceMap.Remarks, workScheduleForUnit,
                                                              WorkScheduleForUnitModel.colNames[WorkScheduleForUnitModel.REMARK_COL_NUM], ref reportValidation);
                */
                UpdateReportValidationWithReplacingData(oldUnitForceMap.Station == null ? null : oldUnitForceMap.Station.StationId,
                                                        newUnitForceMap.Station == null ? null : newUnitForceMap.Station.StationId, workScheduleForUnit,
                                              ImportableWorkScheduleUnitModel.colNames[ImportableWorkScheduleUnitModel.STATION_NAME_COL_NUM], ref reportValidation);

                UpdateReportValidationWithReplacingData(oldUnitForceMap.Doctor, newUnitForceMap.Doctor, workScheduleForUnit, 
                                                              ImportableWorkScheduleUnitModel.colNames[ImportableWorkScheduleUnitModel.DOCTOR_COL_NUM], ref reportValidation);

                UpdateReportValidationWithReplacingData(oldUnitForceMap.Driver, newUnitForceMap.Driver, workScheduleForUnit,
                                                              ImportableWorkScheduleUnitModel.colNames[ImportableWorkScheduleUnitModel.DRIVER_COL_NUM], ref reportValidation);

                UpdateReportValidationWithReplacingData(oldUnitForceMap.Nurse, newUnitForceMap.Nurse, workScheduleForUnit,
                                                              ImportableWorkScheduleUnitModel.colNames[ImportableWorkScheduleUnitModel.NURSE_COL_NUM], ref reportValidation);

                UpdateReportValidationWithReplacingData(oldUnitForceMap.FirstAuxiliar, newUnitForceMap.FirstAuxiliar, workScheduleForUnit,
                                                              ImportableWorkScheduleUnitModel.colNames[ImportableWorkScheduleUnitModel.FIRST_AUXILIAR_COL_NUM], ref reportValidation);

                UpdateReportValidationWithReplacingData(oldUnitForceMap.SecondAuxiliar, newUnitForceMap.SecondAuxiliar, workScheduleForUnit,
                                                              ImportableWorkScheduleUnitModel.colNames[ImportableWorkScheduleUnitModel.SECOND_AUXILIAR_COL_NUM], ref reportValidation);

                UpdateReportValidationWithReplacingData(oldUnitForceMap.ThirdAuxiliar, newUnitForceMap.ThirdAuxiliar, workScheduleForUnit,
                                                              ImportableWorkScheduleUnitModel.colNames[ImportableWorkScheduleUnitModel.THIRD_AUXILIAR_COL_NUM], ref reportValidation);

                //TODO: UpdateReportValidationWithReplacingData (isURAM)




            }

        }

        private void UpdateReportValidationWithReplacingData(UnitCrewMember oldCrewMember, UnitCrewMember newCrewMember, ImportableWorkScheduleUnitModel workScheduleForUnit, string colLabel, ref ReportValidationModel reportValidation)
        {
            if ((oldCrewMember != null ? oldCrewMember.EmployeeId.ToString() : String.Empty) != (newCrewMember != null ? newCrewMember.EmployeeId.ToString() : String.Empty))
                reportValidation.AddReportItem(
                        WorkScheduleValidationType.CONFLICT_WITH_PREVIOUS_DATA,
                        GetReplacingDataMessage(workScheduleForUnit.Row, colLabel,
                                                (oldCrewMember != null ? oldCrewMember.EmployeeId.ToString() : String.Empty), (newCrewMember != null ? newCrewMember.EmployeeId.ToString() : String.Empty)),
                                                new List<string>() { workScheduleForUnit.ID });
        }

        private void UpdateReportValidationWithReplacingData(string oldValue, string newValue, ImportableWorkScheduleUnitModel workScheduleForUnit, string colLabel, ref ReportValidationModel reportValidation)
        {
            if ((oldValue ?? String.Empty) != (newValue ?? String.Empty))
                reportValidation.AddReportItem(
                        WorkScheduleValidationType.CONFLICT_WITH_PREVIOUS_DATA,
                        GetReplacingDataMessage(workScheduleForUnit.Row, colLabel,
                                                (oldValue ?? String.Empty), (newValue ?? String.Empty)),
                                                new List<string>() { workScheduleForUnit.ID });
        }

        public void UpdateReportValidationBasedOnOldData(ref ReportValidationModel reportValidation, List<WorkShiftModel> availableWorkshiftList, List<WorkScheduleUnitModel> unitForceMapList)
        {
            UpdateReportValidationAlertingConflictsWithPreviousData(unitForceMapList, availableWorkshiftList, ref reportValidation);
        }

        public void UpdateReportValidationBasedOnNewData(ref ReportValidationModel reportValidation, List<string> validStationIdList, List<string> validEmployeeIdList, List<string> validUnitList, List<WorkShiftModel> availableWorkshiftList)
        {
            UpdateReportValidationWithRepeatedUnits(ref reportValidation);
            UpdateReportValidationWithRepeatedCrewMembers(ref reportValidation);
            UpdateReportValidationWithInvalidUnits(validUnitList, ref reportValidation);
            UpdateReportValidationWithInvalidEmployees(validEmployeeIdList, ref reportValidation);
            UpdateReportValidationWithInvalidStations(validStationIdList, ref reportValidation);
            UpdateReportValidationByUnit(availableWorkshiftList, ref reportValidation);
        }

        private ReportValidationModel UpdateReportValidationByUnit(List<WorkShiftModel> availableWorkshiftList, ref ReportValidationModel reportValidation)
        {
            foreach (ImportableWorkScheduleUnitModel ws in WorkScheduleForUnitList)
            {
                ws.UpdateReportValidation(ref reportValidation, availableWorkshiftList);
            }

            return reportValidation;
        }

        private static void CloneWorkScheduleWithDateValues(string driverEmployeeId, List<ImportableWorkScheduleUnitModel> workScheduleAdaptedForOneMember, ImportableWorkScheduleUnitModel workScheduleUnitOriginal)
        {
            if (!String.IsNullOrEmpty(driverEmployeeId))
            {
                ImportableWorkScheduleUnitModel workScheduleUnitOneMember = new ImportableWorkScheduleUnitModel();
                workScheduleUnitOneMember.Driver = driverEmployeeId;
                workScheduleUnitOneMember.WorkshiftLabel = workScheduleUnitOriginal.WorkshiftLabel;
                workScheduleUnitOneMember.ShiftDateCellValue = workScheduleUnitOriginal.ShiftDateCellValue;
                workScheduleUnitOneMember.Row = workScheduleUnitOriginal.Row;

                workScheduleAdaptedForOneMember.Add(workScheduleUnitOneMember);
            }
        }

        private string GetConflictingDataMessage(int row1, int row2, string colName, string colValue)
        {
            return String.Format("(linhas {0} e {1}) {2} [{3}]", row1, row2, colName, colValue);
        }

        private string GetReplacingDataMessage(int row, string colName, string colOldValue, string colNewValue)
        {
            return String.Format("(linha {0}) {1} [{2}] será substituído por [{3}]", row, colName, colOldValue, colNewValue);
        }

        public void CompleteWithPeriodicWorkSchedule(List<ImportableWorkScheduleUnitModel> workScheduleList,
                                                    DateTime startDate, DateTime endDate, List<DateTime> dayOffDates,
                                                    bool mergingWorkSchedule)
        {
            this._workScheduleForUnitList = workScheduleList;

            CompleteWithPeriodicWorkSchedule(startDate, endDate, dayOffDates, mergingWorkSchedule);

            OnPropertyChanged("WorkScheduleForUnitList");
        }

        public bool AllWorkScheduleHasDateFrequence()
        {
            return this._workScheduleForUnitList.Where(ws => String.IsNullOrEmpty(ws.DateFrequence)).Count() == 0;
        }

        public void CompleteWithPeriodicWorkSchedule(DateTime startDate, DateTime endDate, List<DateTime> dayOffDates, bool mergingWorkSchedule)
        {
            List<WorkShiftPeriodBuilder> workShiftPeriodBuilderList = new List<WorkShiftPeriodBuilder>();

            try
            {
                workShiftPeriodBuilderList.Add(new WorkShiftPeriodBuilderAlternableDay(startDate, endDate));
                workShiftPeriodBuilderList.Add(new WorkShiftPeriodBuilderWeekly(startDate, endDate));
                workShiftPeriodBuilderList.Add(new WorkShiftPeriodBuilderWorkDay(startDate, endDate, dayOffDates));
                workShiftPeriodBuilderList.Add(new WorkShiftPeriodBuilderOnce(startDate, endDate));
                workShiftPeriodBuilderList.Add(new WorkShiftPeriodBuilderDaily(startDate, endDate));

                foreach (WorkShiftPeriodBuilder wsb in workShiftPeriodBuilderList)
                {
                    wsb.ExtendWorkScheduleWithPeriodicData(this);
                }

                if(mergingWorkSchedule)
                    MergeWorkScheduleList();
            }
            catch
            {
                throw;
            }
        }

        private string GetExceptionMessageToDateConflicted(ImportableWorkScheduleUnitModel ws1, ImportableWorkScheduleUnitModel ws2, int colIndex, List<int> conflictedRows)
        {
            return String.Format("Linhas [ {0} ] apresentaram conflito em {1:dd/MM/yyyy}, no campo: {2}",
                String.Join(",", conflictedRows),
                ws1.ShiftDate.Value,
                ImportableWorkScheduleUnitModel.colNames[colIndex]);
        }

        private ImportableWorkScheduleUnitModel MergeCrew(List<ImportableWorkScheduleUnitModel> workScheduleList)
        {
            //string message = "Linhas {0} e {1} apresentam confilto de equipe ao ser realizado o ";
            int conflictedColumn = -1;
            List<int> conflictedRows = new List<int>();
            ImportableWorkScheduleUnitModel mergedWorkSchedule = (ImportableWorkScheduleUnitModel)workScheduleList.First().Clone();


            foreach (ImportableWorkScheduleUnitModel ws in workScheduleList
                        .Except(new List<ImportableWorkScheduleUnitModel>() { mergedWorkSchedule }))
            {

                if(!mergedWorkSchedule.AddCrewMemberIfOtherNotAlreadyIncluded(ws.Doctor, PersonType.MEDIC))
                {
                    conflictedColumn = ImportableWorkScheduleUnitModel.DOCTOR_COL_NUM;
                }
                if (!mergedWorkSchedule.AddCrewMemberIfOtherNotAlreadyIncluded(ws.Nurse, PersonType.NURSE))
                {
                    conflictedColumn = ImportableWorkScheduleUnitModel.NURSE_COL_NUM;
                }
                if (!mergedWorkSchedule.AddCrewMemberIfOtherNotAlreadyIncluded(ws.Driver, PersonType.DRIVER))
                {
                    conflictedColumn = ImportableWorkScheduleUnitModel.DRIVER_COL_NUM;
                }
                if (!mergedWorkSchedule.AddCrewMemberIfOtherNotAlreadyIncluded(ws.FirstAuxiliar, PersonType.AUX) )
                {
                    conflictedColumn = ImportableWorkScheduleUnitModel.FIRST_AUXILIAR_COL_NUM;
                }
                if (!mergedWorkSchedule.AddCrewMemberIfOtherNotAlreadyIncluded(ws.SecondAuxiliar, PersonType.AUX))
                {
                    conflictedColumn = ImportableWorkScheduleUnitModel.SECOND_AUXILIAR_COL_NUM;
                }
                if (!mergedWorkSchedule.AddCrewMemberIfOtherNotAlreadyIncluded(ws.ThirdAuxiliar, PersonType.AUX))
                {
                    conflictedColumn = ImportableWorkScheduleUnitModel.THIRD_AUXILIAR_COL_NUM;
                }
                conflictedRows.Add(ws.Row);

                if(conflictedColumn > -1)
                {
                    throw new Exception(
                        GetExceptionMessageToDateConflicted(mergedWorkSchedule, ws, conflictedColumn, conflictedRows));
                }
            }
            return mergedWorkSchedule;
        }

        public void MergeWorkScheduleList()
        {
            List<ImportableWorkScheduleUnitModel> mergedWorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>();
            Dictionary<string, List<ImportableWorkScheduleUnitModel>> conflictedIds;
            try
            {
                conflictedIds = this.WorkScheduleForUnitList.GroupBy(ws => ws.ID).Where(g => g.Count() > 1).ToDictionary(g => g.Key, g => g.ToList());
                //newWorkScheduleForUnitList.AddRange(this.WorkScheduleForUnitList.Where(ws => !conflictedIds.Keys.Contains(ws.ID)));

                foreach (string id in conflictedIds.Keys)
                {
                    mergedWorkScheduleForUnitList.Add(MergeCrew(conflictedIds[id]));
                }
            }
            catch
            {
                throw;
            }

            this.WorkScheduleForUnitList.RemoveAll(ws => conflictedIds.Keys.Contains(ws.ID));
            this.WorkScheduleForUnitList.AddRange(mergedWorkScheduleForUnitList);
        }

        public string PrintWorkScheduleList()
        {
            StringBuilder sb = new StringBuilder();
            foreach(ImportableWorkScheduleUnitModel ws in this.WorkScheduleForUnitList)
            {
                sb.AppendLine(ws.ToString());
            }
            return sb.ToString();
        }

        #endregion
    }
}
