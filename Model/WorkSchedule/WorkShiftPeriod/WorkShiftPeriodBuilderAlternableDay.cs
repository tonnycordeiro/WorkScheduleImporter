﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduleImporter.AddIn.Models.WorkSchedule.WorkShiftPeriod
{
    public class WorkShiftPeriodBuilderAlternableDay : WorkShiftPeriodBuilder
    {
        public WorkShiftPeriodBuilderAlternableDay(DateTime startDate, DateTime endDate) : base(startDate, endDate)
        {
        }

        protected override bool IsWorkScheduleFollowingTheRule(ImportableWorkScheduleUnitModel workSchedule)
        {
            return workSchedule.DateFrequence == Properties.Resources.FREQUENCE_TYPE_ALTERNABLE_DAY_DESCRIPTION;
        }

        protected override bool IsWorkScheduleMatchingTheDate(ImportableWorkScheduleUnitModel workSchedule, DateTime date)
        {
            int diffDays = Math.Abs((int)workSchedule.ShiftDate.Value.Subtract(date).TotalDays);
            bool isTheSameParity = (diffDays % 2) == 0;
            return isTheSameParity;
        }
    }
}
