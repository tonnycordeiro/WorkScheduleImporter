using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkScheduleImporter.AddIn.Models.WorkSchedule
{
    public class WorkScheduleConfig
    {
        public const int IGNORE_DATE_ROW_NUM = 12;
        public const int IGNORE_WORKSHIFT_ROW_NUM = 13;
        public const int IGNORE_DATE_COL_NUM = 2;
        public const int IGNORE_WORKSHIFT_COL_NUM = 2;

        private bool _ignoreDate;
        private bool _ignoreWorkShift;

        public WorkScheduleConfig()
        {

        }

        public bool IgnoreDate
        {
            get { return _ignoreDate; }
            set { _ignoreDate = value; }
        }

        public bool IgnoreWorkShift
        {
            get { return _ignoreWorkShift; }
            set { _ignoreWorkShift = value; }
        }
    }
}
