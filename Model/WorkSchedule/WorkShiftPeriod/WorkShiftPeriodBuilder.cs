using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduleImporter.AddIn.Models.WorkSchedule.WorkShiftPeriod
{
    public abstract class WorkShiftPeriodBuilder
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private List<WorkShiftPeriodModel> _workShiftPeriodList;

        public WorkShiftPeriodBuilder(DateTime startDate, DateTime endDate)
        {
            StartDate =
                startDate.Date.CompareTo(DateTime.Now.Date) > 0 ? startDate.Date : DateTime.Now.Date.AddDays(1);
            EndDate = endDate;

            WorkShiftPeriodList = new List<WorkShiftPeriodModel>();
            BuildWorkShiftPeriod();
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }

        public List<WorkShiftPeriodModel> WorkShiftPeriodList
        {
            get { return _workShiftPeriodList; }
            set { _workShiftPeriodList = value; }
        }

        public virtual void BuildWorkShiftPeriod()
        {

            int diffDays = (int)(EndDate.Subtract(StartDate)).TotalDays;

            for (int i = 0; i <= diffDays; i++)
            {
                this.WorkShiftPeriodList.Add(new WorkShiftPeriodModel(StartDate.AddDays(i)));
            }
        }

        public virtual bool Validate()
        {
            return StartDate.CompareTo(EndDate) <= 0;
        }

        protected abstract bool IsWorkScheduleFollowingTheRule(ImportableWorkScheduleUnitModel workSchedule);

        protected abstract bool IsWorkScheduleMatchingTheDate(ImportableWorkScheduleUnitModel workSchedule, DateTime date);

        public virtual List<ImportableWorkScheduleUnitModel> GetWorkScheduleInsidePeriod(List<ImportableWorkScheduleUnitModel> workScheduleList)
        {
            List<ImportableWorkScheduleUnitModel> workScheduleListModified = new List<ImportableWorkScheduleUnitModel>();

            foreach (WorkShiftPeriodModel workShift in
                this.WorkShiftPeriodList.Where(w => w.WorkShiftDate.Value.CompareTo(DateTime.Now.Date) >= 0))
            {
                foreach (ImportableWorkScheduleUnitModel workSchedule in workScheduleList.Where(ws => IsWorkScheduleFollowingTheRule(ws)))
                {
                    if (IsWorkScheduleMatchingTheDate(workSchedule, workShift.WorkShiftDate.Value))
                    {
                        ImportableWorkScheduleUnitModel newWorkSchedule = (ImportableWorkScheduleUnitModel)workSchedule.Clone();
                        newWorkSchedule.ShiftDate = workShift.WorkShiftDate;

                        workScheduleListModified.Add(newWorkSchedule);
                    }
                }
            }

            return workScheduleListModified;
        }

        public virtual void ExtendWorkScheduleWithPeriodicData(WorkScheduleImportTemplateModel workScheduleTemplate)
        {
            List<ImportableWorkScheduleUnitModel> workScheduleListModified = GetWorkScheduleInsidePeriod(workScheduleTemplate.WorkScheduleForUnitList);

            workScheduleTemplate.WorkScheduleForUnitList.RemoveAll(ws => IsWorkScheduleFollowingTheRule(ws));
            workScheduleTemplate.WorkScheduleForUnitList.AddRange(workScheduleListModified);
        }
    }
}
