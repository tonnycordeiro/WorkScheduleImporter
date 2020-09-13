using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduleImporter.AddIn.Models.WorkSchedule.WorkShiftPeriod
{
    public class WorkShiftPeriodBuilderEvenOddDayMonthly : WorkShiftPeriodBuilder
    {
        int _parity;
        public WorkShiftPeriodBuilderEvenOddDayMonthly(int month, int year, int parity) : 
            base(new DateTime(year, month, 1), 
                 new DateTime(year, month, 1).AddMonths(1).AddDays(-1))
        {
            this._parity = parity%2;
        }

        public override void BuildWorkShiftPeriod()
        {
            int diffDays = (int)(EndDate.Subtract(StartDate)).TotalDays;

            for (int i = this._parity; i <= diffDays; i += 2)
            {
                this.WorkShiftPeriodList.Add(new WorkShiftPeriodModel(this.StartDate.AddDays(i)));
            }
        }

        protected override bool IsWorkScheduleFollowingTheRule(ImportableWorkScheduleUnitModel workSchedule)
        {
            throw new NotImplementedException();
        }

        protected override bool IsWorkScheduleMatchingTheDate(ImportableWorkScheduleUnitModel workSchedule, DateTime date)
        {
            throw new NotImplementedException();
        }
    }
}
