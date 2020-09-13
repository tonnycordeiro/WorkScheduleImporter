using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduleImporter.AddIn.Models.WorkSchedule.WorkShiftPeriod
{
    public class WorkShiftPeriodBuilderDaily : WorkShiftPeriodBuilder
    {
        public WorkShiftPeriodBuilderDaily(DateTime startDate, DateTime endDate) : base(startDate, endDate)
        {
        }

        protected override bool IsWorkScheduleFollowingTheRule(ImportableWorkScheduleUnitModel workSchedule)
        {
            return workSchedule.DateFrequence == Properties.Resources.FREQUENCE_TYPE_DAILY_DESCRIPTION;
        }

        protected override bool IsWorkScheduleMatchingTheDate(ImportableWorkScheduleUnitModel workSchedule, DateTime date)
        {
            return true;
        }
    }
}
