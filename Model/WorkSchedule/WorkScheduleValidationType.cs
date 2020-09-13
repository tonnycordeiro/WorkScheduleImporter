using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkScheduleImporter.AddIn.Models.WorkSchedule
{
    public enum WorkScheduleValidationType
    {
        MISSED_DATA,
        SHEET_NAME_NOT_FOUND,
        WRONG_DATA_FORMAT,
        INVALID_DATE,
        INVALID_WORKSHIFT,
        DATA_LENGTH_OVERFLOW,
        INVALID_STATION,
        INVALID_UNIT,
        INVALID_EMPLOYEE_ID,
        EMPLOYEE_DUPLICATED_ON_UNIT,
        EMPLOYEE_DUPLICATED_AT_DAY,
        UNIT_DUPLICATED_AT_DAY,
        INVALID_CREW,
        CONFLICT_WITH_PREVIOUS_DATA,
        INVALID_CREW_FORMATION,
        DATA_SHOULD_BE_FILLED,
        VALIDATED
    }
}
