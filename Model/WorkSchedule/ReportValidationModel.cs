using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduleImporter.AddIn.Models.WorkSchedule
{
    public class ReportValidationModel
    {
        public List<ReportValidationItemModel> ReportValidationItemList;

        public ReportValidationModel() {
            ReportValidationItemList = new List<ReportValidationItemModel>();
        }

        public void AddReportItem(WorkScheduleValidationType validationType, string messageInfo)
        {
            ReportValidationItemModel reportItem = new ReportValidationItemModel(validationType, messageInfo);
            ReportValidationItemList.Add(reportItem);
        }

        public void AddReportItem(WorkScheduleValidationType validationType, string messageInfo, List<string> workScheduleIds)
        {
            ReportValidationItemModel reportItem = new ReportValidationItemModel(validationType, messageInfo, workScheduleIds);
            ReportValidationItemList.Add(reportItem);
        }

        public List<string> GetWorkScheduleIds(List<WorkScheduleValidationType> workScheduleValidationTypeList)
        {
            List<string> ids = new List<string>();
            foreach(ReportValidationItemModel report in 
                this.ReportValidationItemList.Where(r => workScheduleValidationTypeList.Contains(r.WorkScheduleValidationType)))
            {
                ids.AddRange(report.WorkScheduleIds);
            }

            return ids.Distinct().ToList<string>();
        }

        public void JoinIdsOfReportValidationList()
        {
            List<ReportValidationItemModel> reportListDistinct = new List<ReportValidationItemModel>();
            reportListDistinct = ReportValidationItemList.Select(r => new ReportValidationItemModel(r.WorkScheduleValidationType, r.DataAffected)).Distinct().ToList();

            foreach (ReportValidationItemModel report in reportListDistinct)
            {
                foreach (ReportValidationItemModel originalRpt in this.ReportValidationItemList.Where(ro => ro.MessageEquals(report)))
                {
                    report.WorkScheduleIds.AddRange(originalRpt.WorkScheduleIds);
                }
            }
            this.ReportValidationItemList = reportListDistinct;
        }


    }
}
