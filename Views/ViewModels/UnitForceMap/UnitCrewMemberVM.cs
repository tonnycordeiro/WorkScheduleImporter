using System;
using Sisgraph.Ips.Samu.AddIn.Models.UnitForceMap;
using Sisgraph.Ips.Samu.AddIn.Business.UnitForceMap;
using System.ComponentModel;
using System.Collections.Generic;
using System.Linq;

namespace Sisgraph.Ips.Samu.AddIn.ViewModels.UnitForceMap
{
    public class UnitCrewMemberVM : ViewModelBase
    {
        #region Atributos
        private UnitCrewMemberStateEnum _result;
        private DateTime _shiftDate;
        private WorkShiftModel _workShift;
        private WorkShiftModel.ShiftTime _shiftTime;

        private string _message;
        private UnitCrewMember _crewMember;
        private string _label;
        private string _value;
        private CrewMemberTypeEnum _crewMemberType;
        private string _currentValue;
        private string _currentUnitId;
        #endregion

        #region Construtores
        public UnitCrewMemberVM(CrewMemberTypeEnum crewMemberType, UnitForceMapModel currentUnit, WorkShiftModel.ShiftTime currentShiftTime)
        {
            this._crewMemberType = crewMemberType;
            this._currentUnitId = currentUnit.UnitId;
            this._shiftTime = currentShiftTime;
            this._shiftDate = currentUnit.ShiftDate;
            this._workShift = currentUnit.CurrentWorkShift;
        }
        #endregion

        #region Propriedades
        public UnitCrewMemberStateEnum Result
        {
            get { return _result; }
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }

        public UnitCrewMember CrewMember
        {
            get { return _crewMember; }
            set
            {
                _crewMember = value;
                OnPropertyChanged("CrewMember");
                UnitCrewMemberSelected();
            }
        }

        public string Label
        {
            get { return _label; }
            set
            {
                _label = value;
                OnPropertyChanged("Label");
            }
        }

        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }

        public CrewMemberTypeEnum CrewMemberType
        {
            get { return _crewMemberType; }
        }

        public string CurrentUnitId
        {
            get { return _currentUnitId; }
        }
        #endregion

        #region Métodos
        public void Initialize()
        {
            SetLabel();
            SetEmptyValues();
        }

        private void SetLabel()
        {
            switch (_crewMemberType)
            {
                case CrewMemberTypeEnum.Doctor:
                    Label = "Médico";
                    break;
                case CrewMemberTypeEnum.Driver:
                    Label = "Condutor";
                    break;
                case CrewMemberTypeEnum.FirstAuxiliar:
                    Label = "Primeiro Auxiliar";
                    break;
                case CrewMemberTypeEnum.Nurse:
                    Label = "Enfermeiro";
                    break;
                case CrewMemberTypeEnum.SecondAuxiliar:
                    Label = "Segundo Auxiliar";
                    break;
                case CrewMemberTypeEnum.ThirdAuxiliar:
                    Label = "Terceiro Auxiliar";
                    break;
                default:
                    Label = "Inválido";
                    break;
            }
        }

        private void SetEmptyValues()
        {
            Result = UnitCrewMemberStateEnum.Ok;
            CrewMember = null;
            Message = string.Format("Nenhum {0} informado", Label.ToLower());
        }

        public void VerifyValue(bool onlyIfIsDifferent)
        {
            try
            {
                if (!onlyIfIsDifferent || 
                        (_currentValue == null ||
                            !Value.Equals(_currentValue, StringComparison.CurrentCultureIgnoreCase)))
                {
                    if (string.IsNullOrEmpty(Value))
                    {
                        _currentValue = Value;
                        SetEmptyValues();
                    }
                    else
                    {
                        _currentValue = Value.Trim();

                        long id;
                        if (long.TryParse(_currentValue, out id))
                        {
                            SearchByID(id);
                        }
                        else
                        {
                            SearchByName();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Result = UnitCrewMemberStateEnum.Error;
                CrewMember = null;
                Message = ex.Message;
            }
        }

        private void SearchByID(long id)
        {
            var bg = new BackgroundWorker();

            bg.DoWork += (s, e) =>
                {
                    try
                    {
                        Result = UnitCrewMemberStateEnum.Verifying;
                        Message = "Verificando";

                        UnitCrewMember crewMember = UnitCrewMemberBusiness.GetByID(id, CrewMemberType);

                        if (crewMember == null)
                        {
                            Result = UnitCrewMemberStateEnum.NotExists;
                            Message = "Não encontrado";
                            CrewMember = null;
                        }
                        else
                        {
                            VerifyAndSelectMember(crewMember);
                        }
                    }
                    catch (Exception ex)
                    {
                        Result = UnitCrewMemberStateEnum.Error;
                        Message = ex.Message;
                        CrewMember = null;
                    }
                };

            bg.RunWorkerCompleted += (s, e) =>
                {
                    bg.Dispose();
                };

            bg.RunWorkerAsync();
        }

        private void SearchByName()
        {
            var bg = new BackgroundWorker();

            bg.DoWork += (s, e) =>
            {
                try
                {
                    Result = UnitCrewMemberStateEnum.Verifying;
                    Message = "Verificando";

                    List<UnitCrewMember> list = UnitCrewMemberBusiness.GetByName(Value, CrewMemberType);

                    if (list == null || list.Count <= 0)
                    {
                        Result = UnitCrewMemberStateEnum.NotExists;
                        Message = "Não encontrado";
                        CrewMember = null;
                    }
                    else if (list.Count > 1)
                    {
                        Result = UnitCrewMemberStateEnum.Multiple;
                        Message = "Mais de um resultado encontrado";
                        CrewMember = null;
                    }
                    else //if (list.Count == 1)
                    {
                        VerifyAndSelectMember(list.First());
                    }
                }
                catch (Exception ex)
                {
                    Result = UnitCrewMemberStateEnum.Error;
                    Message = ex.Message;
                    CrewMember = null;
                }
            };

            bg.RunWorkerCompleted += (s, e) =>
            {
                bg.Dispose();
            };

            bg.RunWorkerAsync();
        }

        /// <summary>
        /// Seleciona membro, caso não esteja sendo usado em outro terminal.
        /// </summary>
        public void VerifyAndSelectMember(UnitCrewMember crewMember)
        {
            //caso seja verificação futura, atualiza informações da data e turno escolhidos
            if (_shiftTime == WorkShiftModel.ShiftTime.Forward)
            {
                crewMember.CheckFutureAllocation(_shiftDate, _workShift);
            }

            if (crewMember.IsLogged && !crewMember.Terminal.Equals(CurrentUnitId))
            {
                Result = UnitCrewMemberStateEnum.Error;
                Message = string.Format("Usuário já está sendo usado no terminal '{0}'", crewMember.Terminal);
                CrewMember = null;
            }
            else
            {
                Result = UnitCrewMemberStateEnum.Ok;
                CrewMember = crewMember;
                Message = CrewMember.ToString();
            }
        }

        private void UnitCrewMemberSelected()
        {
            if (OnUnitCrewMemberSelected != null)
            {
                OnUnitCrewMemberSelected(this, new EventArgs());
            }
        }
        #endregion

        #region Eventos
        public event EventHandler<EventArgs> OnUnitCrewMemberSelected;
        #endregion
    }
}
