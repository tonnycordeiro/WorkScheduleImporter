using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduleImporter.AddIn.Models.WorkSchedule.WorkShiftPeriod
{
    public class WorkShiftPeriodBuilderWorkDay : WorkShiftPeriodBuilder
    {
        private List<DateTime> _holidayList;

        public WorkShiftPeriodBuilderWorkDay(DateTime startDate, DateTime endDate, List<DateTime> holidayList) : base(startDate, endDate)
        {
            _holidayList = holidayList;
        }

        protected override bool IsWorkScheduleFollowingTheRule(ImportableWorkScheduleUnitModel workSchedule)
        {
            return workSchedule.DateFrequence == Properties.Resources.FREQUENCE_TYPE_WORK_DAY_DESCRIPTION;
        }

        protected override bool IsWorkScheduleMatchingTheDate(ImportableWorkScheduleUnitModel workSchedule, DateTime date)
        {
            return
                (date.DayOfWeek != DayOfWeek.Saturday) &&
                (date.DayOfWeek != DayOfWeek.Sunday) &&
                !_holidayList.Contains(date);
        }

    }
}
