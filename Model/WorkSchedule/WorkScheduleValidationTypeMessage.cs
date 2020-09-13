using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkScheduleImporter.AddIn.Models.WorkSchedule;

namespace WorkScheduleImporter.AddIn.Models.WorkSchedule
{
    public class WorkScheduleValidationTypeMessage
    {
        private WorkScheduleValidationType _validationType;
        private ValidationLevelType _level;
        private string _message;

        public WorkScheduleValidationTypeMessage(WorkScheduleValidationType validationType, ValidationLevelType level, string message)
        {
            _validationType = validationType;
            _level = level;
            _message = message;
        }

        public WorkScheduleValidationType ValidationType { get {return _validationType;} set {_validationType = value;} }
        public ValidationLevelType Level { get {return _level;} set{_level = value;} }
        public string Message { get { return _message; } set { _message = value; } }

    }
}
