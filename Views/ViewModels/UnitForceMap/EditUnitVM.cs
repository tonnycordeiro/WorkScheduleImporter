using System;
using System.Collections.Generic;
using System.Linq;
using Sisgraph.Ips.Samu.AddIn.Models;
using Sisgraph.Ips.Samu.AddIn.Models.UnitForceMap;
using Sisgraph.Ips.Samu.AddIn.Business.UnitForceMap;
using Sisgraph.Ips.Samu.AddIn.Business;

namespace Sisgraph.Ips.Samu.AddIn.ViewModels.UnitForceMap
{
    public class EditUnitVM : ViewModelBase, IDisposable
    {
        #region Atributos
        private string[] _headerText;

        private WorkShiftModel.ShiftTime _currentShiftTime;

        private UnitForceMapModel _selectedUnit;
        private UnitForceMapModel _backupUnit;

        private string _selectedDispatchGroup;

        private List<string> _groupList;
        private List<CadStationModel> _stationList;
        private List<CadStationModel> _cadStationList;

        private UnitCrewMemberVM _driverVM;
        private UnitCrewMemberVM _nurseVM;
        private UnitCrewMemberVM _firstAuxiliarVM;
        private UnitCrewMemberVM _secondAuxiliarVM;
        private UnitCrewMemberVM _thirdAuxiliarVM;
        private UnitCrewMemberVM _doctorVM;

        private bool _isURAM;
        #endregion

        #region Construtores
        public EditUnitVM(UnitForceMapModel unit, WorkShiftModel.ShiftTime currentShiftTime)
        {
            //UnitForceMapModel currentUnit = UnitForceMapBusiness.GetCurrentUnitForceMap(unit.UnitId);

            BackupUnit = CopyUnit(unit);// (UnitForceMapModel)unit.Clone();

            _currentShiftTime = currentShiftTime;

            HeaderText = new string[3];
            HeaderText[0] = "Você está editando as informações de uma AM em seu";
            HeaderText[1] = "TURNO " + (_currentShiftTime == WorkShiftModel.ShiftTime.Actual ? "ATUAL" : "FUTURO");
            HeaderText[2] = unit.ShiftDate.ToString("dd/MM/yyyy") + " das " + unit.CurrentWorkShift.ToString();

            LoadData();
        }
        #endregion

        #region Propriedades
        public UnitForceMapModel SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                _selectedUnit = value;
                OnPropertyChanged("SelectedUnit");
            }
        }

        public UnitForceMapModel BackupUnit
        {
            get { return _backupUnit; }
            set
            {
                _backupUnit = value; OnPropertyChanged("BackupUnit");
            }
        }

        public List<string> GroupList
        {
            get { return _groupList; }
            set
            {
                _groupList = value;
                OnPropertyChanged("GroupList");
            }
        }

        public string SelectedDispatchGroup
        {
            get { return _selectedDispatchGroup; }
            set { _selectedDispatchGroup = value; OnPropertyChanged("SelectedDispatchGroup"); }
        }

        public string[] HeaderText
        {
            get { return _headerText; }
            set { _headerText = value; OnPropertyChanged("HeaderText"); }
        }

        public List<CadStationModel> StationList
        {
            get { return _stationList; }
            set
            {
                _stationList = value;
                OnPropertyChanged("StationList");
            }
        }

        public bool Result { get; set; }

        public UnitCrewMemberVM DriverVM
        {
            get { return _driverVM; }
            set
            {
                _driverVM = value;
                OnPropertyChanged("DriverVM");
            }
        }

        public UnitCrewMemberVM NurseVM
        {
            get { return _nurseVM; }
            set
            {
                _nurseVM = value;
                OnPropertyChanged("NurseVM");
            }
        }

        public UnitCrewMemberVM FirstAuxiliarVM
        {
            get { return _firstAuxiliarVM; }
            set
            {
                _firstAuxiliarVM = value;
                OnPropertyChanged("FirstAuxiliarVM");
            }
        }

        public UnitCrewMemberVM SecondAuxiliarVM
        {
            get { return _secondAuxiliarVM; }
            set
            {
                _secondAuxiliarVM = value;
                OnPropertyChanged("SecondAuxiliarVM");
            }
        }

        public UnitCrewMemberVM ThirdAuxiliarVM
        {
            get { return _thirdAuxiliarVM; }
            set
            {
                _thirdAuxiliarVM = value;
                OnPropertyChanged("ThirdAuxiliarVM");
            }
        }

        public UnitCrewMemberVM DoctorVM
        {
            get { return _doctorVM; }
            set
            {
                _doctorVM = value;
                OnPropertyChanged("DoctorVM");
            }
        }

        public bool IsURAM
        {
            get { return _isURAM; }
            set
            {
                _isURAM = value;

                if (_isURAM)
                {
                    SelectedUnitAsURAM();
                }
                else
                {
                    UpdateUnitType();
                }

                OnPropertyChanged("IsURAM");
            }
        }
        #endregion

        #region Métodos
        public void LoadData()
        {
            _cadStationList = CadBusiness.GetCadStationList("SAMU");
            GroupList = _cadStationList.Select(s => s.DispatchGroup).Distinct().ToList();

            SelectedUnit = CopyUnit(BackupUnit);
            SelectedDispatchGroup = SelectedUnit.Station.DispatchGroup;

            LoadCrewMembersControl();

            LoadUnitInitialData();
        }

        private void LoadCrewMembersControl()
        {
            DriverVM = new UnitCrewMemberVM(CrewMemberTypeEnum.Driver, SelectedUnit, _currentShiftTime);
            DoctorVM = new UnitCrewMemberVM(CrewMemberTypeEnum.Doctor, SelectedUnit, _currentShiftTime);
            NurseVM = new UnitCrewMemberVM(CrewMemberTypeEnum.Nurse, SelectedUnit, _currentShiftTime);
            FirstAuxiliarVM = new UnitCrewMemberVM(CrewMemberTypeEnum.FirstAuxiliar, SelectedUnit, _currentShiftTime);
            SecondAuxiliarVM = new UnitCrewMemberVM(CrewMemberTypeEnum.SecondAuxiliar, SelectedUnit, _currentShiftTime);
            ThirdAuxiliarVM = new UnitCrewMemberVM(CrewMemberTypeEnum.ThirdAuxiliar, SelectedUnit, _currentShiftTime);

            DriverVM.Initialize();
            DoctorVM.Initialize();
            NurseVM.Initialize();
            FirstAuxiliarVM.Initialize();
            SecondAuxiliarVM.Initialize();
            ThirdAuxiliarVM.Initialize();

            DriverVM.OnUnitCrewMemberSelected += (s, e) =>
                {
                    SelectedUnit.Driver = DriverVM.CrewMember;
                    UpdateUnitType();
                };
            DoctorVM.OnUnitCrewMemberSelected += (s, e) =>
                {
                    SelectedUnit.Doctor = DoctorVM.CrewMember;
                    UpdateUnitType();
                };
            NurseVM.OnUnitCrewMemberSelected += (s, e) =>
                {
                    SelectedUnit.Nurse = NurseVM.CrewMember;
                    UpdateUnitType();
                };
            FirstAuxiliarVM.OnUnitCrewMemberSelected += (s, e) =>
                {
                    SelectedUnit.FirstAuxiliar = FirstAuxiliarVM.CrewMember;
                    UpdateUnitType();
                };
            SecondAuxiliarVM.OnUnitCrewMemberSelected += (s, e) =>
                {
                    SelectedUnit.SecondAuxiliar = SecondAuxiliarVM.CrewMember;
                    UpdateUnitType();
                };
            ThirdAuxiliarVM.OnUnitCrewMemberSelected += (s, e) =>
            {
                SelectedUnit.ThirdAuxiliar = ThirdAuxiliarVM.CrewMember;
                UpdateUnitType();
            };
        }

        private UnitForceMapModel CopyUnit(UnitForceMapModel unit)
        {
            var copy = new UnitForceMapModel();
            copy.Avl = unit.Avl;
            copy.Category = unit.Category;
            copy.CellPhone = unit.CellPhone;
            copy.CurrentWorkShift = unit.CurrentWorkShift;
            copy.DispatchedTimes = unit.DispatchedTimes;
            copy.Doctor = unit.Doctor;
            copy.Driver = unit.Driver;
            copy.FirstAuxiliar = unit.FirstAuxiliar;
            copy.HT = unit.HT;
            copy.Nurse = unit.Nurse;
            copy.Radio = unit.Radio;
            copy.Remarks = unit.Remarks;
            copy.SecondAuxiliar = unit.SecondAuxiliar;
            copy.ThirdAuxiliar = unit.ThirdAuxiliar;
            copy.ShiftDate = unit.ShiftDate;
            if (unit.Station != null)
            {
                copy.Station = new CadStationModel();
                copy.Station.StationId = unit.Station.StationId;
                copy.Station.Station = unit.Station.Station;
                copy.Station.DispatchGroup = unit.Station.DispatchGroup;
                copy.Station.Phone = unit.Station.Phone;
                copy.Station.Address = unit.Station.Address;
            }
            copy.Status = unit.Status;
            copy.UnitId = unit.UnitId;
            copy.WorkShiftStarted = unit.WorkShiftStarted;
            return copy;
        }

        /// <summary>
        /// Carrega dados já existentes da viatura.
        /// </summary>
        private void LoadUnitInitialData()
        {
            SelectedUnit.Station = _cadStationList
                .Where(p => p.Station == SelectedUnit.Station.Station).FirstOrDefault();

            if (SelectedUnit.Category.Equals("URAM"))
            {
                IsURAM = true;
            }

            DriverVM.CrewMember = SelectedUnit.Driver;
            if (DriverVM.CrewMember != null)
            {
                DriverVM.Value = SelectedUnit.Driver.EmployeeId.ToString();
                DriverVM.Message = SelectedUnit.Driver.ToString();
            }

            DoctorVM.CrewMember = SelectedUnit.Doctor;
            if (DoctorVM.CrewMember != null)
            {
                DoctorVM.Value = SelectedUnit.Doctor.EmployeeId.ToString();
                DoctorVM.Message = SelectedUnit.Doctor.ToString();
            }

            NurseVM.CrewMember = SelectedUnit.Nurse;
            if (NurseVM.CrewMember != null)
            {
                NurseVM.Value = SelectedUnit.Nurse.EmployeeId.ToString();
                NurseVM.Message = SelectedUnit.Nurse.ToString();
            }

            FirstAuxiliarVM.CrewMember = SelectedUnit.FirstAuxiliar;
            if (FirstAuxiliarVM.CrewMember != null)
            {
                FirstAuxiliarVM.Value = SelectedUnit.FirstAuxiliar.EmployeeId.ToString();
                FirstAuxiliarVM.Message = SelectedUnit.FirstAuxiliar.ToString();
            }

            SecondAuxiliarVM.CrewMember = SelectedUnit.SecondAuxiliar;
            if (SecondAuxiliarVM.CrewMember != null)
            {
                SecondAuxiliarVM.Value = SelectedUnit.SecondAuxiliar.EmployeeId.ToString();
                SecondAuxiliarVM.Message = SelectedUnit.SecondAuxiliar.ToString();
            }

            ThirdAuxiliarVM.CrewMember = SelectedUnit.ThirdAuxiliar;
            if (ThirdAuxiliarVM.CrewMember != null)
            {
                ThirdAuxiliarVM.Value = SelectedUnit.ThirdAuxiliar.EmployeeId.ToString();
                ThirdAuxiliarVM.Message = SelectedUnit.ThirdAuxiliar.ToString();
            }
        }

        private void UpdateUnitType()
        {
            if (!IsURAM)
            {
                SelectedUnit.Category = UnitForceMapBusiness.GetUnitTypeForCrew(SelectedUnit);
            }
            else if (SelectedUnit.Driver == null)
            {
                SelectedUnit.Category = string.Empty;
            }
        }

        public bool Confirm()
        {
            //try
            //{
                Result = false;

                VerifyRepeatedSelections();

                if (SelectedUnit.Station == null)
                {
                    ShowMessage("Selecione a base da viatura");
                }
                else
                {
                    if (HasConflicts())
                    {
                        ShowMessage("Resolva os conflitos indicados na tela");
                    }
                    else
                    {
                        switch (_currentShiftTime)
                        {
                            case WorkShiftModel.ShiftTime.Actual:
                                
                                SelectedUnit.WorkShiftStarted = "T";

                                Result = UnitForceMapBusiness.UpdateUnitForceMap(SelectedUnit);
                                break;
                            case WorkShiftModel.ShiftTime.Forward:
                                Result = UnitForceMapBusiness.UpdateFutureUnitForceMap(SelectedUnit);
                                break;
                        }

                        if (!Result)
                        {
                            ShowMessage("Falha ao confirmar edição da viatura");
                        }
                    }
                }
            /*}
            catch (UnauthorizedAccessException ex)
            {
                ShowMessage(ex.Message);
            }
            catch
            {
                ShowMessage("Falha ao confirmar edição da viatura");
            }*/

            return Result;
        }

        public void Cancel()
        {
            Result = false;
        }

        public void ReloadStationList()
        {
            StationList = null;

            if (!string.IsNullOrEmpty(SelectedDispatchGroup))
            {
                StationList = (from c in _cadStationList
                               where c.DispatchGroup.Equals(SelectedDispatchGroup)
                               select c).Distinct().ToList();
            }
        }

        private bool HasConflicts()
        {
            if (DriverVM.Result != UnitCrewMemberStateEnum.Ok ||
                DoctorVM.Result != UnitCrewMemberStateEnum.Ok ||
                NurseVM.Result != UnitCrewMemberStateEnum.Ok ||
                FirstAuxiliarVM.Result != UnitCrewMemberStateEnum.Ok ||
                SecondAuxiliarVM.Result != UnitCrewMemberStateEnum.Ok ||
                ThirdAuxiliarVM.Result != UnitCrewMemberStateEnum.Ok)
            {
                return true;
            }
            return false;
        }

        private void VerifyRepeatedSelections()
        {
            UnitCrewMember crewMember;

            if (DriverVM.CrewMember != null)
            {
                crewMember = DriverVM.CrewMember;

                if (DoctorVM.CrewMember != null && DoctorVM.CrewMember.Equals(crewMember))
                {
                    DoctorVM.Result = UnitCrewMemberStateEnum.Error;
                    DoctorVM.Message = "Usuário já utilizado como condutor";
                }
                if (NurseVM.CrewMember != null && NurseVM.CrewMember.Equals(crewMember))
                {
                    NurseVM.Result = UnitCrewMemberStateEnum.Error;
                    NurseVM.Message = "Usuário já utilizado como condutor";
                }
                if (FirstAuxiliarVM.CrewMember != null && FirstAuxiliarVM.CrewMember.Equals(crewMember))
                {
                    FirstAuxiliarVM.Result = UnitCrewMemberStateEnum.Error;
                    FirstAuxiliarVM.Message = "Usuário já utilizado como condutor";
                }
                if (SecondAuxiliarVM.CrewMember != null && SecondAuxiliarVM.CrewMember.Equals(crewMember))
                {
                    SecondAuxiliarVM.Result = UnitCrewMemberStateEnum.Error;
                    SecondAuxiliarVM.Message = "Usuário já utilizado como condutor";
                }
                if (ThirdAuxiliarVM.CrewMember != null && ThirdAuxiliarVM.CrewMember.Equals(crewMember))
                {
                    ThirdAuxiliarVM.Result = UnitCrewMemberStateEnum.Error;
                    ThirdAuxiliarVM.Message = "Usuário já utilizado como condutor";
                }
            }
            if (DoctorVM.CrewMember != null)
            {
                crewMember = DoctorVM.CrewMember;

                if (NurseVM.CrewMember != null && NurseVM.CrewMember.Equals(crewMember))
                {
                    NurseVM.Result = UnitCrewMemberStateEnum.Error;
                    NurseVM.Message = "Usuário já utilizado como médico";
                }
                if (FirstAuxiliarVM.CrewMember != null && FirstAuxiliarVM.CrewMember.Equals(crewMember))
                {
                    FirstAuxiliarVM.Result = UnitCrewMemberStateEnum.Error;
                    FirstAuxiliarVM.Message = "Usuário já utilizado como médico";
                }
                if (SecondAuxiliarVM.CrewMember != null && SecondAuxiliarVM.CrewMember.Equals(crewMember))
                {
                    SecondAuxiliarVM.Result = UnitCrewMemberStateEnum.Error;
                    SecondAuxiliarVM.Message = "Usuário já utilizado como médico";
                }
                if (ThirdAuxiliarVM.CrewMember != null && ThirdAuxiliarVM.CrewMember.Equals(crewMember))
                {
                    ThirdAuxiliarVM.Result = UnitCrewMemberStateEnum.Error;
                    ThirdAuxiliarVM.Message = "Usuário já utilizado como médico";
                }
            }
            if (NurseVM.CrewMember != null)
            {
                crewMember = NurseVM.CrewMember;

                if (FirstAuxiliarVM.CrewMember != null && FirstAuxiliarVM.CrewMember.Equals(crewMember))
                {
                    FirstAuxiliarVM.Result = UnitCrewMemberStateEnum.Error;
                    FirstAuxiliarVM.Message = "Usuário já utilizado como enfermeiro";
                }
                if (SecondAuxiliarVM.CrewMember != null && SecondAuxiliarVM.CrewMember.Equals(crewMember))
                {
                    SecondAuxiliarVM.Result = UnitCrewMemberStateEnum.Error;
                    SecondAuxiliarVM.Message = "Usuário já utilizado como enfermeiro";
                }
                if (ThirdAuxiliarVM.CrewMember != null && ThirdAuxiliarVM.CrewMember.Equals(crewMember))
                {
                    ThirdAuxiliarVM.Result = UnitCrewMemberStateEnum.Error;
                    ThirdAuxiliarVM.Message = "Usuário já utilizado como enfermeiro";
                }
            }
            if (FirstAuxiliarVM.CrewMember != null)
            {
                crewMember = FirstAuxiliarVM.CrewMember;

                if (SecondAuxiliarVM.CrewMember != null && SecondAuxiliarVM.CrewMember.Equals(crewMember))
                {
                    SecondAuxiliarVM.Result = UnitCrewMemberStateEnum.Error;
                    SecondAuxiliarVM.Message = "Usuário já utilizado como primeiro auxiliar";
                }
                if (ThirdAuxiliarVM.CrewMember != null && ThirdAuxiliarVM.CrewMember.Equals(crewMember))
                {
                    ThirdAuxiliarVM.Result = UnitCrewMemberStateEnum.Error;
                    ThirdAuxiliarVM.Message = "Usuário já utilizado como enfermeiro";
                }
            }

            if (SecondAuxiliarVM.CrewMember != null)
            {
                crewMember = SecondAuxiliarVM.CrewMember;

                if (ThirdAuxiliarVM.CrewMember != null && ThirdAuxiliarVM.CrewMember.Equals(crewMember))
                {
                    ThirdAuxiliarVM.Result = UnitCrewMemberStateEnum.Error;
                    ThirdAuxiliarVM.Message = "Usuário já utilizado como segundo auxiliar";
                }
            }
        }

        public void SelectedUnitAsURAM()
        {
            if (SelectedUnit != null)
            {
                DoctorVM.CrewMember = null;
                DoctorVM.Value = string.Empty;
                DoctorVM.Message = string.Empty;

                NurseVM.CrewMember = null;
                NurseVM.Value = string.Empty;
                NurseVM.Message = string.Empty;

                FirstAuxiliarVM.CrewMember = null;
                FirstAuxiliarVM.Value = string.Empty;
                FirstAuxiliarVM.Message = string.Empty;

                SecondAuxiliarVM.CrewMember = null;
                SecondAuxiliarVM.Value = string.Empty;
                SecondAuxiliarVM.Message = string.Empty;

                ThirdAuxiliarVM.CrewMember = null;
                ThirdAuxiliarVM.Value = string.Empty;
                ThirdAuxiliarVM.Message = string.Empty;

                SelectedUnit.Category = "URAM";
            }
        }

        public void Dispose()
        {

        }
        #endregion
    }
}
