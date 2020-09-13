using System;
using System.Linq;
using System.Reflection;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using Government.EmergencyDepartment.AddIn.Models.WorkSchedule;
using Government.EmergencyDepartment.AddIn.Models;

namespace WorkScheduleImporter.AddIn.Models.WorkSchedule
{
    public class ImportableWorkScheduleUnitModel : Observable, ICloneable
    {
        public const int MAX_REMARK_LENGTH = 200; 
        public const int MAX_CELLPHONE_LENGTH = 100;

        public const int CELL_PHONE_COL_NUM = -1;
        public const int REMARK_COL_NUM = -2;
        public const int IS_URAM_COL_NUM = -3;
        
        public const int SHIFT_DATE_COL_NUM = 1; 
        public const int WORKSHIF_LABEL_COL_NUM = 2; 
        public const int UNIT_ID_COL_NUM = 4; 
        public const int STATION_NAME_COL_NUM = 5; 
        public const int DRIVER_COL_NUM = 6; 
        public const int DOCTOR_COL_NUM = 8; 
        public const int NURSE_COL_NUM = 10; 
        public const int FIRST_AUXILIAR_COL_NUM = 12; 
        public const int SECOND_AUXILIAR_COL_NUM = 14;
        public const int THIRD_AUXILIAR_COL_NUM = 16;
        public const int STATION_ID_COL_NUM = 27;
        public const int DATE_FREQUENCE_COL_NUM = 28;

        public static Dictionary<string, WorkScheduleDateFrequenceEnum> frequenceType = new Dictionary<string, WorkScheduleDateFrequenceEnum>
        { {Properties.Resources.FREQUENCE_TYPE_ONCE_DESCRIPTION, WorkScheduleDateFrequenceEnum.ONCE },
          {Properties.Resources.FREQUENCE_TYPE_DAILY_DESCRIPTION, WorkScheduleDateFrequenceEnum.DAILY },
          {Properties.Resources.FREQUENCE_TYPE_ALTERNABLE_DAY_DESCRIPTION, WorkScheduleDateFrequenceEnum.ALTERNABLE_DAY },
          {Properties.Resources.FREQUENCE_TYPE_WEEKLY_DESCRIPTION, WorkScheduleDateFrequenceEnum.WEEKLY },
          {Properties.Resources.FREQUENCE_TYPE_WORK_DAY_DESCRIPTION, WorkScheduleDateFrequenceEnum.WORK_DAY }
        };

        public static Dictionary<PersonType, string> PersonTypeToAlias = new Dictionary<PersonType, string>()
        {
            {PersonType.MEDIC, Properties.Resources.MEDIC_ALIAS},
            {PersonType.NURSE, Properties.Resources.NURSE_ALIAS},
            {PersonType.DRIVER, Properties.Resources.DRIVER_ALIAS},
            {PersonType.AUX_1, Properties.Resources.AUX_1_ALIAS},
            {PersonType.AUX_2, Properties.Resources.AUX_2_ALIAS},
            {PersonType.AUX_3, Properties.Resources.AUX_3_ALIAS}
        };

        public static Dictionary<int, string> colNames = new Dictionary<int, string>
        { {SHIFT_DATE_COL_NUM, Properties.Resources.SHIFT_DATE_COL_NAME },
          {WORKSHIF_LABEL_COL_NUM, Properties.Resources.WORKSHIF_LABEL_COL_NAME },
          {UNIT_ID_COL_NUM, Properties.Resources.UNIT_ID_COL_NAME },
          {CELL_PHONE_COL_NUM, Properties.Resources.CELL_PHONE_COL_NAME },
          {STATION_NAME_COL_NUM, Properties.Resources.STATION_NAME_COL_NAME },
          {REMARK_COL_NUM, Properties.Resources.REMARK_COL_NAME },
          {DOCTOR_COL_NUM, Properties.Resources.DOCTOR_COL_NAME},
          {NURSE_COL_NUM, Properties.Resources.NURSE_COL_NAME },
          {FIRST_AUXILIAR_COL_NUM, Properties.Resources.FIRST_AUXILIAR_COL_NAME },
          {SECOND_AUXILIAR_COL_NUM, Properties.Resources.SECOND_AUXILIAR_COL_NAME },
          {THIRD_AUXILIAR_COL_NUM, Properties.Resources.THIRD_AUXILIAR_COL_NAME },
          {DRIVER_COL_NUM, Properties.Resources.DRIVER_COL_NAME },
          {IS_URAM_COL_NUM, Properties.Resources.IS_URAM_COL_NAME },
          {STATION_ID_COL_NUM, Properties.Resources.STATION_ID_COL_NAME },
        };

        #region Attributes
        private DateTime? _shiftDate;
        private string _shiftDateCellValue;
        private string _workShiftLabel;
        private string _unitId;
        private string _cellPhone;
        private string _stationName;
        private string _stationId;
        private string _remark;
        private string _doctor;
        private string _nurse;
        private string _firstAuxiliar;
        private string _secondAuxiliar;
        private string _thirdAuxiliar;
        private string _driver;
        private string _isURAM;
        private string _unitType;
        private int _row;
        private List<ReportValidationItemModel> _validationTypeList;
        private string _dateFrequence;
        private bool _hasSelectedError;
        private bool _hasError;
        private bool _hasWarning;
        //private List<int> _requiredFieldsColNum;

        #endregion

        #region Constructors
        public ImportableWorkScheduleUnitModel()
        {
            _validationTypeList = new List<ReportValidationItemModel>();

            this.HasError = false;
            this.HasWarning = false;
            this.HasSelectedError = false;
        }
        #endregion

        #region Properties

        public List<ReportValidationItemModel> ValidationTypeList
        {
            get { return _validationTypeList; }
            set { _validationTypeList = value; }
        }

        private DateTime ConvertCellValueToDate(string value)
        {
            double dateExcelNumber;

            if (value == null)
                throw new ArgumentNullException();

            if (!Double.TryParse(value, out dateExcelNumber))
                throw new InvalidCastException();

            return DateTime.FromOADate(dateExcelNumber);
        }

        private string ConvertDateToCellValue(DateTime? date)
        {
            if (date.HasValue)
                return date.Value.ToOADate().ToString();
            else
                return null;
        }

        public DateTime? ShiftDate
        {
            get { return _shiftDate; }
            set { 
                _shiftDate = value;
                _shiftDateCellValue = ConvertDateToCellValue(_shiftDate).ToString();
            }
        }

        public string ShiftDateCellValue
        {
            get { return _shiftDateCellValue; }
            set
            {
                _shiftDateCellValue = value;

                try
                {
                    int dateValue;
                    if (Int32.TryParse(_shiftDateCellValue, out dateValue))
                        _shiftDate = ConvertCellValueToDate(value);
                    else
                        _shiftDate = new DateTime(Int32.Parse(value.Substring(6, 4)), Int32.Parse(value.Substring(3, 2)), Int32.Parse(value.Substring(0, 2)));
                }
                catch
                {
                    _shiftDate = null;
                }
            }
        }

        public string WorkshiftLabel
        {
            get { return _workShiftLabel; }
            set { _workShiftLabel = value; }
        }

        public string UnitId
        {
            get { return _unitId; }
            set { _unitId = value; }
        }

        public string CellPhone
        {
            get { return _cellPhone; }
            set { _cellPhone = value; }
        }

        public string StationName
        {
            get { return _stationName; }
            set { _stationName = value; }
        }

        public string StationId
        {
            get { return _stationId; }
            set { _stationId = value; }
        }

        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }

        public string Doctor
        {
            get { return _doctor; }
            set { _doctor = value; }
        }

        public string Nurse
        {
            get { return _nurse; }
            set { _nurse = value; }
        }

        public string FirstAuxiliar
        {
            get { return _firstAuxiliar; }
            set { _firstAuxiliar = value; }
        }

        public string SecondAuxiliar
        {
            get { return _secondAuxiliar; }
            set { _secondAuxiliar = value; }
        }

        public string ThirdAuxiliar
        {
            get { return _thirdAuxiliar; }
            set { _thirdAuxiliar = value; }
        }
        public string Driver
        {
            get { return _driver; }
            set { _driver = value; }
        }

        public string IsURAM
        {
            get { return _isURAM; }
            set { _isURAM = value; }
        }

        public string UnitType
        {
            get { return _unitType; }
            set { _unitType = value; }
        }

        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        public string DateFrequence
        {
            get { return _dateFrequence; }
            set { _dateFrequence = value; }
        }

        public bool HasSelectedError
        {
            get { return _hasSelectedError; }
            set { _hasSelectedError = value; }
        }

        public bool HasError
        {
            get { return _hasError; }
            set { _hasError = value; }
        }

        public bool HasWarning
        {
            get { return _hasWarning; }
            set { _hasWarning = value; }
        }

        public string ID
        {
            get {
                StringBuilder sb = new StringBuilder();
                sb.Append(this.ShiftDate.HasValue ? this.ShiftDate.Value.ToString("yyyyMMdd") : String.Empty);
                sb.Append(this.WorkshiftLabel);
                sb.Append(this.UnitId);
                return sb.ToString(); }
        }

        public int AuxiliarCount
        {
            get
            {
                return (String.IsNullOrEmpty(this.FirstAuxiliar) ? 0 : 1) +
                       (String.IsNullOrEmpty(this.SecondAuxiliar) ? 0 : 1) +
                       (String.IsNullOrEmpty(this.ThirdAuxiliar) ? 0 : 1);
            }
        }

        public string ConcatenatedCrewMembers
        {
            get
            {
                List<string> crewMembers = new List<string>();

                if (!String.IsNullOrEmpty(this._driver))
                    crewMembers.Add(String.Format("{0}({1})", this._driver, PersonTypeToAlias[PersonType.DRIVER]));
                if (!String.IsNullOrEmpty(this._doctor))
                    crewMembers.Add(String.Format("{0}({1})", this._doctor, PersonTypeToAlias[PersonType.MEDIC]));
                if (!String.IsNullOrEmpty(this._nurse))
                    crewMembers.Add(String.Format("{0}({1})", this._nurse, PersonTypeToAlias[PersonType.NURSE]));
                if (!String.IsNullOrEmpty(this._firstAuxiliar))
                    crewMembers.Add(String.Format("{0}({1})", this._firstAuxiliar, PersonTypeToAlias[PersonType.AUX_1]));
                if (!String.IsNullOrEmpty(this._secondAuxiliar))
                    crewMembers.Add(String.Format("{0}({1})", this._secondAuxiliar, PersonTypeToAlias[PersonType.AUX_2]));
                if (!String.IsNullOrEmpty(this._thirdAuxiliar))
                    crewMembers.Add(String.Format("{0}({1})", this._thirdAuxiliar, PersonTypeToAlias[PersonType.AUX_3]));

                return String.Join(",", crewMembers);
            }
        }
        #endregion

        #region Methods


        public bool HasRepeatedEmployeeId(out string messageInfo)
        {
            List<string> repeatedEmployeeIdList = new List<string>();
            messageInfo = null;

            List<string> employeeIdList = GetEmployeeIdList();

            employeeIdList = employeeIdList.OrderBy(e => e).ToList();
            for (int i = 1; i < employeeIdList.Count; i++)
            {
                if (employeeIdList[i].Equals(employeeIdList[i - 1]))
                {
                    if (!repeatedEmployeeIdList.Contains(employeeIdList[i]))
                        repeatedEmployeeIdList.Add(employeeIdList[i]);
                }
            }

            if (repeatedEmployeeIdList.Count > 0)
            {
                messageInfo = GetMessage("RF(s)", String.Join(",", repeatedEmployeeIdList));
                return true;
            }

            return false;
        }

        private List<string> GetEmployeeIdList()
        {
            List<string> employeeIdList = new List<string>();
            if (!String.IsNullOrEmpty(this.Doctor))
                employeeIdList.Add(this.Doctor);
            if (!String.IsNullOrEmpty(this.Nurse))
                employeeIdList.Add(this.Nurse);
            if (!String.IsNullOrEmpty(this.FirstAuxiliar))
                employeeIdList.Add(this.FirstAuxiliar);
            if (!String.IsNullOrEmpty(this.SecondAuxiliar))
                employeeIdList.Add(this.SecondAuxiliar);
            if (!String.IsNullOrEmpty(this.ThirdAuxiliar))
                employeeIdList.Add(this.ThirdAuxiliar);
            if (!String.IsNullOrEmpty(this.Driver))
                employeeIdList.Add(this.Driver);
            return employeeIdList;
        }

        private bool HasOverflow(string value, int maxLength, int indexOfColName, out string messageInfo)
        {
            messageInfo = null;
            if (String.IsNullOrEmpty(value) || value.Length <= maxLength)
                return false;

            messageInfo = GetMessage(null,colNames[indexOfColName] + " possui " + value.Length.ToString() + " caracteres (o máximo é " + maxLength + ")");
            return true;
        }

        public bool HasOverflowOfCellPhoneLength(out string messageInfo)
        {
            return HasOverflow(this.CellPhone, MAX_CELLPHONE_LENGTH, CELL_PHONE_COL_NUM, out messageInfo);
        }

        public bool HasOverflowOfRemarkLength(out string messageInfo)
        {
            return HasOverflow(this.Remark, MAX_REMARK_LENGTH, REMARK_COL_NUM, out messageInfo);
        }

        public bool IsPreviousDate(out string messageInfo)
        {
            messageInfo = null;

            if (this.ShiftDate.HasValue && this.ShiftDate.Value.Date.CompareTo(DateTime.Now.Date) < 0)
            {
                messageInfo = GetMessage(colNames[SHIFT_DATE_COL_NUM], ShiftDate.Value.Date.ToString("dd/MM/yyyy"));
                return true;
            }
            return false;
        }

        private bool IsNullOrNumeric(string value)
        {
            int result;
            return (String.IsNullOrEmpty(value) || Int32.TryParse(value, out result));
        }

        private bool IsNumericFormatValid(string value, int colIndex, out string messageInfo)
        {
            if (this.IsNullOrNumeric(value))
            {
                messageInfo = null;
                return true;
            }
            messageInfo = GetMessage(colNames[colIndex], value);
            return false;
        }

        public bool IsFormatValidOfDoctor(out string messageInfo)
        {
            return this.IsNumericFormatValid(this.Doctor, DOCTOR_COL_NUM, out messageInfo);
        }

        public bool IsFormatValidOfNurse(out string messageInfo)
        {
            return this.IsNumericFormatValid(this.Nurse, NURSE_COL_NUM, out messageInfo);
        }

        public bool IsFormatValidOfFirstAuxiliar(out string messageInfo)
        {
            return this.IsNumericFormatValid(this.FirstAuxiliar, FIRST_AUXILIAR_COL_NUM, out messageInfo);
        }

        public bool IsFormatValidOfSecondAuxiliar(out string messageInfo)
        {
            return this.IsNumericFormatValid(this.SecondAuxiliar, SECOND_AUXILIAR_COL_NUM, out messageInfo);
        }

        public bool IsFormatValidOfThirdAuxiliar(out string messageInfo)
        {
            return this.IsNumericFormatValid(this.ThirdAuxiliar, THIRD_AUXILIAR_COL_NUM, out messageInfo);
        }

        public bool IsFormatValidOfDriver(out string messageInfo)
        {
            return this.IsNumericFormatValid(this.Driver, DRIVER_COL_NUM, out messageInfo);
        }

        public bool HasFieldsShoudBeFilledButNotRequired(out string messageInfo)
        {
            List<string> colNamesNotFilled = new List<string>();
            messageInfo = null;

            if (String.IsNullOrEmpty(this.Driver))
                colNamesNotFilled.Add(colNames[ImportableWorkScheduleUnitModel.DRIVER_COL_NUM]);

            if (colNamesNotFilled.Count > 0)
            {
                messageInfo = GetMessage("colunas", String.Join(",", colNamesNotFilled));
                return true;
            }

            return false;
        }

        public bool HasRequiredFieldsNotFilled(out string messageInfo)
        {
            List<string> colNamesNotFilled = new List<string>();
            messageInfo = null;

            if (String.IsNullOrEmpty(this.ShiftDateCellValue))
                colNamesNotFilled.Add(colNames[ImportableWorkScheduleUnitModel.SHIFT_DATE_COL_NUM]);

            if (String.IsNullOrEmpty(this.StationId))
                colNamesNotFilled.Add(colNames[ImportableWorkScheduleUnitModel.STATION_ID_COL_NUM]);

            if (String.IsNullOrEmpty(this.UnitId))
                colNamesNotFilled.Add(colNames[ImportableWorkScheduleUnitModel.UNIT_ID_COL_NUM]);

            if (String.IsNullOrEmpty(this.WorkshiftLabel))
                colNamesNotFilled.Add(colNames[ImportableWorkScheduleUnitModel.WORKSHIF_LABEL_COL_NUM]);

            if (colNamesNotFilled.Count > 0)
            {
                messageInfo = GetMessage("colunas", String.Join(",", colNamesNotFilled));
                return true;
            }

            return false;
        }


        public bool IsInvalidDateFormat(out string messageInfo)
        {
            messageInfo = null;
            if (String.IsNullOrEmpty(this.ShiftDateCellValue) || (!String.IsNullOrEmpty(this.ShiftDateCellValue) && !this.ShiftDate.HasValue))
            {
                messageInfo = GetMessage(colNames[ImportableWorkScheduleUnitModel.SHIFT_DATE_COL_NUM], this.ShiftDateCellValue??String.Empty);
                return true;
            }
            return false;
        }

        public bool IsForwardShiftTime(List<WorkShiftModel> availableWorkShiftList, out string messageInfo)
        {
            bool isForward = true;
            WorkShiftModel workShiftSelected = availableWorkShiftList.Where(ws => ws.Label == this.WorkshiftLabel).FirstOrDefault();

            if (!this.ShiftDate.HasValue)
                throw new NullReferenceException();

            if (this.ShiftDate.Value.Date.CompareTo(DateTime.Now.Date) < 0)
            { 
                isForward = false;

            }

            if (this.ShiftDate.Value.Date.CompareTo(DateTime.Now.Date) == 0)
            {
                if(!workShiftSelected.IsPreviousTo(DateTime.Now.Hour, DateTime.Now.Minute))
                    isForward = false;
            }

            messageInfo = isForward ? null : GetMessage(
                    String.Format("{0} {1}", colNames[ImportableWorkScheduleUnitModel.SHIFT_DATE_COL_NUM], colNames[ImportableWorkScheduleUnitModel.WORKSHIF_LABEL_COL_NUM]),
                    String.Format("{0} {1}", (this.ShiftDate.HasValue ? this.ShiftDate.Value.ToString("dd/MM/yyyy") : this.ShiftDateCellValue), this.WorkshiftLabel));

            return isForward;

        }

        public bool IsInvalidWorkshift(List<WorkShiftModel> availableWorkshiftList, out string messageInfo)
        {
            List<string> availableWorkshiftLabels;

            if (availableWorkshiftList == null || availableWorkshiftList.Count == 0)
                throw new ArgumentException("lista de turnos nula", "availableWorkshifts");

            availableWorkshiftLabels = availableWorkshiftList.Select(ws => ws.Label).ToList<string>();

            messageInfo = null;
            if (!availableWorkshiftLabels.Contains(this.WorkshiftLabel))
            {
                messageInfo = GetMessage(colNames[ImportableWorkScheduleUnitModel.WORKSHIF_LABEL_COL_NUM], this.WorkshiftLabel);
                return true;
            }

            return false;
        }

        public void UpdateReportValidation(ref ReportValidationModel reportValidation, List<WorkShiftModel> availableWorkShiftList)
        {
            List<string> repeatedEmployeeIdList = new List<string>();
            List<string> workScheduleIdList = new List<string>() {this.ID };

            string messageInfo;


            if (this.HasRepeatedEmployeeId(out messageInfo))
            {
                reportValidation.AddReportItem(WorkScheduleValidationType.EMPLOYEE_DUPLICATED_ON_UNIT, messageInfo, workScheduleIdList);
            }

            if (this.HasOverflowOfCellPhoneLength(out messageInfo))
            {
                reportValidation.AddReportItem(WorkScheduleValidationType.DATA_LENGTH_OVERFLOW, messageInfo, workScheduleIdList);
            }

            if (this.HasOverflowOfRemarkLength(out messageInfo))
            {
                reportValidation.AddReportItem(WorkScheduleValidationType.DATA_LENGTH_OVERFLOW, messageInfo, workScheduleIdList);
            }

            if (!this.IsFormatValidOfDoctor(out messageInfo))
            {
                reportValidation.AddReportItem(WorkScheduleValidationType.INVALID_EMPLOYEE_ID, messageInfo, workScheduleIdList);
            }

            if (!this.IsFormatValidOfNurse(out messageInfo))
            {
                reportValidation.AddReportItem(WorkScheduleValidationType.INVALID_EMPLOYEE_ID, messageInfo, workScheduleIdList);
            }

            if (!this.IsFormatValidOfFirstAuxiliar(out messageInfo))
            {
                reportValidation.AddReportItem(WorkScheduleValidationType.INVALID_EMPLOYEE_ID, messageInfo, workScheduleIdList);
            }

            if (!this.IsFormatValidOfSecondAuxiliar(out messageInfo))
            {
                reportValidation.AddReportItem(WorkScheduleValidationType.INVALID_EMPLOYEE_ID, messageInfo, workScheduleIdList);
            }

            if (!this.IsFormatValidOfThirdAuxiliar(out messageInfo))
            {
                reportValidation.AddReportItem(WorkScheduleValidationType.INVALID_EMPLOYEE_ID, messageInfo, workScheduleIdList);
            }

            if (!this.IsFormatValidOfDriver(out messageInfo))
            {
                reportValidation.AddReportItem(WorkScheduleValidationType.INVALID_EMPLOYEE_ID, messageInfo, workScheduleIdList);
            }

            if(this.HasRequiredFieldsNotFilled(out messageInfo))
            {
                reportValidation.AddReportItem(WorkScheduleValidationType.MISSED_DATA, messageInfo, workScheduleIdList);
            }

            if (this.HasFieldsShoudBeFilledButNotRequired(out messageInfo)) 
            {
                reportValidation.AddReportItem(WorkScheduleValidationType.DATA_SHOULD_BE_FILLED, messageInfo, workScheduleIdList);
            }

            if (this.IsInvalidDateFormat(out messageInfo))
            {
                reportValidation.AddReportItem(WorkScheduleValidationType.WRONG_DATA_FORMAT, messageInfo, workScheduleIdList);
            }

            try
            {
                if (!this.IsForwardShiftTime(availableWorkShiftList, out messageInfo))
                {
                    reportValidation.AddReportItem(WorkScheduleValidationType.INVALID_DATE, messageInfo, workScheduleIdList);
                }
            }
            catch
            {
            }

            if(this.IsInvalidWorkshift(availableWorkShiftList, out messageInfo))
            {
                reportValidation.AddReportItem(WorkScheduleValidationType.INVALID_WORKSHIFT, messageInfo, workScheduleIdList);
            }

            if (this.IsURAMWithInvalidCrew(out messageInfo))
            {
                reportValidation.AddReportItem(WorkScheduleValidationType.INVALID_CREW, messageInfo, workScheduleIdList);
            }

            if (this.IsSAVWithInvalidCrew(out messageInfo))
            {
                reportValidation.AddReportItem(WorkScheduleValidationType.INVALID_CREW_FORMATION, messageInfo, workScheduleIdList);
            }
        }

        public bool Equals(ImportableWorkScheduleUnitModel other)
        {
            if (this != null && other != null)
            {
                var type = typeof(WorkScheduleUnitModel);
                var unequalProperties =
                    from propertyInfo in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    let thisValue = type.GetProperty(propertyInfo.Name).GetValue(this, null)
                    let otherValue = type.GetProperty(propertyInfo.Name).GetValue(other, null)
                    where thisValue != otherValue && (thisValue == null || !thisValue.Equals(otherValue))
                    select thisValue;
                return !unequalProperties.Any();
            }
            return this == other;
        }
            
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public string GetMessage(string colName, string colValue)
        {
            string message = string.Empty;
            if (string.IsNullOrEmpty(colName))
                return String.Format("(linha {0}) {1} [{2}]", this.Row, colName, colValue);
            else
                return String.Format("(linha {0}) {1}", this.Row, colValue); 
        }

        public bool IsRightCrewCategory()
        {
            WorkScheduleUnitModel workScheduleUnit = this.ToWorkScheduleUnitModel(null);
            
            return String.IsNullOrEmpty(this._unitType) || (workScheduleUnit.Category??String.Empty) == this._unitType;
        }

        private bool isCrewCategoryURAM()
        {
            return (!String.IsNullOrEmpty(this.Driver) &&
                    String.IsNullOrEmpty(this.Doctor) && String.IsNullOrEmpty(this.Nurse)
                    && String.IsNullOrEmpty(this.FirstAuxiliar) && String.IsNullOrEmpty(this.SecondAuxiliar)
                    && String.IsNullOrEmpty(this.ThirdAuxiliar));
        }

        public bool IsURAMWithInvalidCrew(out string messageInfo)
        {
            messageInfo = null;
            bool isInvalidCrew = (this._unitType ?? String.Empty) == WorkScheduleUnitModel.CATEGORY_URAM_NAME && !isCrewCategoryURAM();

            if (isInvalidCrew)
                messageInfo = GetMessage(null, WorkScheduleUnitModel.CATEGORY_URAM_NAME);

            return isInvalidCrew;
        }

        public bool IsSAVWithInvalidCrew(out string messageInfo)
        {
            messageInfo = null;
            bool isInvalidCrew = this.UnitType == WorkScheduleUnitModel.CATEGORY_SAV_NAME && !this.IsRightCrewCategory();

            if (isInvalidCrew)
                messageInfo = GetMessage(null, WorkScheduleUnitModel.CATEGORY_SAV_NAME);

            return isInvalidCrew;
        }

        public bool IsSAVWithMedicalMembers()
        {
            return (this.UnitType == WorkScheduleUnitModel.CATEGORY_SAV_NAME &&
                        (!String.IsNullOrEmpty(this.Doctor) || !String.IsNullOrEmpty(this.Nurse)));
        }

        public WorkScheduleUnitModel ToWorkScheduleUnitModel(List<WorkShiftModel> availableWorkShiftList) //, Dictionary<string, string> unitTypeByUnitId
        {
            WorkScheduleUnitModel newUnit = new WorkScheduleUnitModel();

            if (this.ShiftDate.HasValue)
                newUnit.ShiftDate = this.ShiftDate.Value;

            if(availableWorkShiftList != null)
                newUnit.CurrentWorkShift = availableWorkShiftList.Where(ws => ws.Label == this.WorkshiftLabel).FirstOrDefault();

            CadStationModel newCadStation = new CadStationModel();
            newCadStation.StationId = this.StationId;
            newUnit.Station = newCadStation;

            newUnit.Remarks = this.Remark;
            newUnit.CellPhone = this.CellPhone;
            newUnit.UnitId = this.UnitId;

            newUnit.Doctor = GetUnitCrewMember(this.Doctor);
            newUnit.Nurse = GetUnitCrewMember(this.Nurse);
            newUnit.FirstAuxiliar = GetUnitCrewMember(this.FirstAuxiliar);
            newUnit.SecondAuxiliar = GetUnitCrewMember(this.SecondAuxiliar);
            newUnit.ThirdAuxiliar = GetUnitCrewMember(this.ThirdAuxiliar);
            newUnit.Driver = GetUnitCrewMember(this.Driver);

            if (this.UnitType == WorkScheduleUnitModel.CATEGORY_URAM_NAME)
                newUnit.Category = WorkScheduleUnitModel.CATEGORY_URAM_NAME;
            else
                newUnit.Category =  newUnit.GenerateUnitType();
            
            return newUnit;
        }

        private UnitCrewMember GetUnitCrewMember(string employeeValue)
        {
            UnitCrewMember member = null;
            int employee;

            if (Int32.TryParse(employeeValue, out employee)) {
                member = new UnitCrewMember();
                member.EmployeeId = employee;
            }

            return member;
        }



        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(String.Format("{0:dd/MM/yyyy} {1} {2} {3} -> ", this._shiftDate, this._workShiftLabel, this.UnitId, this._stationName ?? String.Empty));
            sb.Append(String.Format("equipe: {0}", this.ConcatenatedCrewMembers));

            return sb.ToString();
        }

        public WorkScheduleDateFrequenceEnum GetFrequenceType(string frequenceDesc)
        {
            if (!frequenceType.ContainsKey(frequenceDesc))
                throw new Exception("Frequência de datas não reconhecida");

            return frequenceType[frequenceDesc];
        }

        public bool AddAuxiliarIfNotExists(string newAuxiliar)
        {
            if (String.IsNullOrEmpty(newAuxiliar))
                return true;

            if ((!String.IsNullOrEmpty(this.FirstAuxiliar) && newAuxiliar.CompareTo(this.FirstAuxiliar) == 0) ||
                (!String.IsNullOrEmpty(this.SecondAuxiliar) && newAuxiliar.CompareTo(this.SecondAuxiliar) == 0) ||
                (!String.IsNullOrEmpty(this.ThirdAuxiliar) && newAuxiliar.CompareTo(this.ThirdAuxiliar) == 0))
                return true;

            if (AuxiliarCount == 3)
                return false;

            if (String.IsNullOrEmpty(this.FirstAuxiliar))
                this.FirstAuxiliar = newAuxiliar;
            else
            {
                if (String.IsNullOrEmpty(this.SecondAuxiliar))
                    this.SecondAuxiliar = newAuxiliar;
                else
                    if (String.IsNullOrEmpty(this.ThirdAuxiliar))
                    this.ThirdAuxiliar = newAuxiliar;
            }
            return true;
        }

        private bool WasOtherCrewMemberIncludedBefore(string currentCrewMember, string candidateCrewMember)
        {
            return !String.IsNullOrEmpty(currentCrewMember) && 
                currentCrewMember.CompareTo(candidateCrewMember??String.Empty) != 0;
        }

        public bool AddCrewMemberIfOtherNotAlreadyIncluded(string crewMember, PersonType personType)
        {
            bool wasOtherCrewMemberIncludedBefore = false;
            if (String.IsNullOrEmpty(crewMember))
            {
                return true;
            }

            switch (personType)
            {
                case PersonType.DRIVER:
                    wasOtherCrewMemberIncludedBefore = WasOtherCrewMemberIncludedBefore(this.Driver, crewMember);
                    if (!wasOtherCrewMemberIncludedBefore)
                        this.Driver = crewMember;
                    break;
                case PersonType.MEDIC:
                    wasOtherCrewMemberIncludedBefore = WasOtherCrewMemberIncludedBefore(this.Doctor, crewMember);
                    if (!wasOtherCrewMemberIncludedBefore)
                        this.Doctor = crewMember;
                    break;
                case PersonType.NURSE:
                    wasOtherCrewMemberIncludedBefore = WasOtherCrewMemberIncludedBefore(this.Nurse, crewMember);
                    if (!wasOtherCrewMemberIncludedBefore)
                        this.Nurse = crewMember;
                    break;
                case PersonType.AUX_1:
                    wasOtherCrewMemberIncludedBefore = WasOtherCrewMemberIncludedBefore(this.FirstAuxiliar, crewMember);
                    if (!wasOtherCrewMemberIncludedBefore)
                        this.FirstAuxiliar = crewMember;
                    break;
                case PersonType.AUX_2:
                    wasOtherCrewMemberIncludedBefore = WasOtherCrewMemberIncludedBefore(this.SecondAuxiliar, crewMember);
                    if (!wasOtherCrewMemberIncludedBefore)
                        this.SecondAuxiliar = crewMember;
                    break;
                case PersonType.AUX_3:
                    wasOtherCrewMemberIncludedBefore = WasOtherCrewMemberIncludedBefore(this.ThirdAuxiliar, crewMember);
                    if (!wasOtherCrewMemberIncludedBefore)
                        this.ThirdAuxiliar = crewMember;
                    break;
                case PersonType.AUX:
                    wasOtherCrewMemberIncludedBefore =
                        WasOtherCrewMemberIncludedBefore(this.FirstAuxiliar, crewMember) &&
                        WasOtherCrewMemberIncludedBefore(this.SecondAuxiliar, crewMember) &&
                        WasOtherCrewMemberIncludedBefore(this.ThirdAuxiliar, crewMember);
                    if (!wasOtherCrewMemberIncludedBefore)
                        AddAuxiliarIfNotExists(crewMember);
                    break;
                default:
                    throw new Exception(String.Format("Tipo de pessoa não existente: {0}",crewMember));

            }

            return !wasOtherCrewMemberIncludedBefore;
        }

        #endregion
    }
}
