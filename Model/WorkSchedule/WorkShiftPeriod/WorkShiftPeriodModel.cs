using System;

namespace WorkScheduleImporter.AddIn.Models.WorkSchedule.WorkShiftPeriod
{
    public class WorkShiftPeriodModel
    {
        private DateTime? _workShiftDate;
        private string _workShiftLabel;

        public WorkShiftPeriodModel(DateTime? workShiftDate, string workShiftLabel)
        {
            _workShiftDate = workShiftDate;
            _workShiftLabel = workShiftLabel;
        }

        public WorkShiftPeriodModel(DateTime? workShiftDate)
        {
            _workShiftDate = workShiftDate;
            _workShiftLabel = null;
        }

        public DateTime? WorkShiftDate
        {
            get { return _workShiftDate; }
            set { _workShiftDate = value; }
        }

        public string WorkShiftLabel {
            get { return _workShiftLabel; }
            set { _workShiftLabel = value; }
        }
    }
}