using System;
using System.Collections.Generic;
using System.Linq;
using Intergraph.IPS.Cad.Data;
using Intergraph.IPS.CADCore;
using WorkScheduleImporter.AddIn.Models.WorkSchedule;
using Government.EmergencyDepartment.AddIn.Models.WorkSchedule;
using Government.EmergencyDepartment.AddIn.Business.CustomCad;
using Government.EmergencyDepartment.AddIn.Business;
using Government.EmergencyDepartment.AddIn.Business.WorkSchedule;

namespace WorkScheduleImporter.AddIn.Business.WorkSchedule
{
    public class ReportValidationBusiness
    {
        public static Dictionary<WorkScheduleValidationType, WorkScheduleValidationTypeMessage> ValidationTypeByMessage;


        private static List<WorkScheduleValidationTypeMessage> WorkScheduleValidationTypeMessageList = new List<WorkScheduleValidationTypeMessage>()
            {
                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.CONFLICT_WITH_PREVIOUS_DATA,ValidationLevelType.WARNING,Properties.Resources.CONFLICT_WITH_PREVIOUS_DATA_MESSAGE),

                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.DATA_LENGTH_OVERFLOW,ValidationLevelType.ERROR,Properties.Resources.DATA_LENGTH_OVERFLOW_MESSAGE),

                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.EMPLOYEE_DUPLICATED_AT_DAY,ValidationLevelType.ERROR,Properties.Resources.EMPLOYEE_DUPLICATED_AT_DAY_MESSAGE),

                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.EMPLOYEE_DUPLICATED_ON_UNIT,ValidationLevelType.ERROR,Properties.Resources.EMPLOYEE_DUPLICATED_ON_UNIT_MESSAGE),

                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.INVALID_DATE,ValidationLevelType.ERROR,Properties.Resources.INVALID_DATE_MESSAGE),

                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.INVALID_EMPLOYEE_ID,ValidationLevelType.ERROR,Properties.Resources.INVALID_EMPLOYEE_ID_MESSAGE),

                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.INVALID_STATION,ValidationLevelType.ERROR,Properties.Resources.INVALID_STATION_MESSAGE),

                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.INVALID_UNIT,ValidationLevelType.ERROR,Properties.Resources.INVALID_UNIT_MESSAGE),

                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.INVALID_WORKSHIFT,ValidationLevelType.ERROR,Properties.Resources.INVALID_WORKSHIFT_MESSAGE),

                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.MISSED_DATA,ValidationLevelType.ERROR,Properties.Resources.MISSED_DATA_MESSAGE),

                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.UNIT_DUPLICATED_AT_DAY,ValidationLevelType.ERROR,Properties.Resources.UNIT_DUPLICATED_AT_DAY_MESSAGE),

                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.WRONG_DATA_FORMAT,ValidationLevelType.ERROR,Properties.Resources.WRONG_DATA_FORMAT_MESSAGE),

                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.SHEET_NAME_NOT_FOUND,ValidationLevelType.ERROR,Properties.Resources.SHEET_NAME_NOT_FOUND_MESSAGE),

                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.INVALID_CREW,ValidationLevelType.ERROR,Properties.Resources.INVALID_CREW_MESSAGE),

                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.DATA_SHOULD_BE_FILLED,ValidationLevelType.WARNING,Properties.Resources.DATA_SHOULD_BE_FILLED_MESSAGE),

                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.INVALID_CREW_FORMATION,ValidationLevelType.WARNING,Properties.Resources.INVALID_CREW_FORMATION_MESSAGE),

                new WorkScheduleValidationTypeMessage
                    (WorkScheduleValidationType.VALIDATED,ValidationLevelType.VALIDATED,Properties.Resources.VALIDATED_MESSAGE)

            };

        public static void LoadValidationsByType() //TODO: get by message file
        {
            ValidationTypeByMessage = new Dictionary<WorkScheduleValidationType, WorkScheduleValidationTypeMessage>();
            foreach (WorkScheduleValidationTypeMessage v in WorkScheduleValidationTypeMessageList)
            {
                ValidationTypeByMessage[v.ValidationType] = v;
            }
        }

        public static bool AreThereErrorMessages(ReportValidationModel reportValidationModel)
        {
            return (reportValidationModel.ReportValidationItemList.Where(r => ValidationTypeByMessage[r.WorkScheduleValidationType].Level == ValidationLevelType.ERROR).Count() > 0);
        }
           

        public static void UpdateReportValidation(WorkScheduleImportTemplateModel workScheduleTemplate, List<WorkShiftModel> availableWorkshiftList, string currentAgencyId, ReportValidationModel repotValidationModel)
        {
            string returnMessage;
            List<string> validUnitList = new List<string>();
            List<string> validStationIdList = new List<string>();
            List<int> validEmployeeIdNumList = new List<int>();
            List<string> validEmployeeIdList = new List<string>();

            List<WorkScheduleUnitModel> workScheduleUnitList = new List<WorkScheduleUnitModel>();

            validUnitList = UnitBusiness.GetActiveUnitsId(currentAgencyId);
            validStationIdList = CadBusiness.GetCadStationList(currentAgencyId).Select(cs => cs.StationId).ToList<string>();
            validEmployeeIdNumList = GetEmployeeIds(currentAgencyId);

            foreach (int emp in validEmployeeIdNumList)
            {
                validEmployeeIdList.Add(emp.ToString());
            }

            List<DateTime> dateTimeListFromWorkSchedule = workScheduleTemplate.WorkScheduleForUnitList.Where(ws => ws.ShiftDate.HasValue).Select(ws => ws.ShiftDate.Value).Distinct().ToList<DateTime>();
            List<string> workshifLabelListfromWorkSchedule = workScheduleTemplate.WorkScheduleForUnitList.Select(ws => ws.WorkshiftLabel).Distinct().ToList<string>();

            workScheduleTemplate.UpdateReportValidationBasedOnNewData(ref repotValidationModel, validStationIdList, validEmployeeIdList, validUnitList, availableWorkshiftList);

            foreach (DateTime date in dateTimeListFromWorkSchedule)
            {
                foreach(WorkShiftModel workShift in availableWorkshiftList.Where(ws => workshifLabelListfromWorkSchedule.Contains(ws.Label)))
                {
                    workScheduleUnitList = WorkScheduleBusiness.GetWorkScheduleList(null, date, workShift, WorkShiftModel.ShiftTime.Forward, (int)FilterTypeEnum.ALL ,out returnMessage)
                        .Where(uf => uf.Driver != null || uf.Doctor != null || uf.Nurse != null || uf.FirstAuxiliar != null || uf.SecondAuxiliar != null || uf.ThirdAuxiliar != null)
                        .ToList<WorkScheduleUnitModel>();
                    workScheduleTemplate.UpdateReportValidationBasedOnOldData(ref repotValidationModel, availableWorkshiftList, workScheduleUnitList);
                }
            }

            repotValidationModel.JoinIdsOfReportValidationList();

            UpdateWorkScheduleWithErrors(repotValidationModel, workScheduleTemplate);
 
            if(repotValidationModel.ReportValidationItemList.Count == 0)
                repotValidationModel.AddReportItem(WorkScheduleValidationType.VALIDATED, Properties.Resources.ALERT_MESSAGE_WHEN_NO_ERRORS);
        }

        private static void UpdateWorkScheduleWithErrors(ReportValidationModel reportValidationModel, WorkScheduleImportTemplateModel workScheduleTemplate)
        {
            List<string> idsWithWarnings = GetReportedWorkScheduleIds(reportValidationModel, ValidationLevelType.WARNING);
            List<string> idsWithErrors = GetReportedWorkScheduleIds(reportValidationModel, ValidationLevelType.ERROR);

            workScheduleTemplate.WorkScheduleForUnitList.ForEach(ws => ws.HasWarning = idsWithWarnings.Contains(ws.ID));
            workScheduleTemplate.WorkScheduleForUnitList.ForEach(ws => ws.HasError = idsWithErrors.Contains(ws.ID));
        }

        private static List<string> GetReportedWorkScheduleIds(ReportValidationModel reportValidationModel, ValidationLevelType validationType)
        {
            List<string> rowsWithErrors = reportValidationModel.GetWorkScheduleIds(
                ReportValidationBusiness.WorkScheduleValidationTypeMessageList.Where(
                    ws => ws.Level == validationType).Select(ws => ws.ValidationType).ToList<WorkScheduleValidationType>());
            return rowsWithErrors;
        }

        private static List<int> GetEmployeeIds(string currentAgencyId)
        {
            List<int> activeUnitIdList = null;

            using (CADTransaction cadTransaction = ((TransactionManager)CADSystem.CadContext).BeginTransaction())
            {
                try
                {
                    IpsCadEntities context = CADSystem.CadContext.CommandScope.CommandContext.GetObjectContext<IpsCadEntities>();
                    activeUnitIdList = context.DefinedEmployeeData.Where(e => e.AgencyId == currentAgencyId).Select(e => e.EmployeeId).ToList<int>();
                }
                catch 
                {
                    throw;
                }
            }

            return activeUnitIdList;
        }




    }
}
