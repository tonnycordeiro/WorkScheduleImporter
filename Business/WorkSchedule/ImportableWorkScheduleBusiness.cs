using Intergraph.IPS.CADCore;
using log4net;
using WorkScheduleImporter.AddIn.Models.WorkSchedule;
using System;
using System.Collections.Generic;
using System.Reflection;
using Government.EmergencyDepartment.AddIn.Business.WorkSchedule;
using Government.EmergencyDepartment.AddIn.Data.WorkSchedule;
using WorkScheduleImporter.AddIn.Data.WorkSchedule;

namespace WorkScheduleImporter.AddIn.Business.WorkSchedule
{
    public class ImportableWorkScheduleBusiness
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static IEnumerable<ImportableWorkScheduleUnitModel> ExtractWorkSchedule(string fullPathFile)
        {
            try
            {
                IEnumerable<ImportableWorkScheduleUnitModel> workScheduleForUniList = Data.WorkSchedule.ImportableWorkScheduleData.ImportWorkSchedule(fullPathFile);
                UpdateWorkScheduleWithCrewCategoryFromUnitType(ref workScheduleForUniList);
                return workScheduleForUniList;
            }
            catch
            {
                throw;
            };
        }

        private static void UpdateWorkScheduleWithCrewCategoryFromUnitType(ref IEnumerable<ImportableWorkScheduleUnitModel> workScheduleList)
        {
            try
            {
                Dictionary<string, string> unitTypeByUnitIdList = WorkScheduleBusiness.GetUnitTypeByUnitIdDictionary();

                foreach (ImportableWorkScheduleUnitModel workScheduleForUnit in workScheduleList)
                {
                    if (!unitTypeByUnitIdList.ContainsKey(workScheduleForUnit.UnitId))
                        continue;

                    workScheduleForUnit.UnitType = unitTypeByUnitIdList[workScheduleForUnit.UnitId];
                }

            }
            catch
            {
                throw;
            };
        }

        public static string GetWorkScheduleSheetName()
        {
            return ImportableWorkScheduleData.GetWorkScheduleSheetName();
        }

        public static void SaveWorkScheduleTemplate(string targetFullPathFile)
        {
            try
            {
                WorkScheduleData.SaveWorkScheduleTemplate(targetFullPathFile);
                Data.WorkSchedule.ImportableWorkScheduleData.UpdateWorkScheduleSheets(targetFullPathFile, WorkScheduleBusiness.GetParameter(Properties.Settings.Default.PASSWORD_KEY_NAME));
            }
            catch (Exception exception)
            {
                logger.Error(exception);
                throw exception;
            }
        }
    }
}