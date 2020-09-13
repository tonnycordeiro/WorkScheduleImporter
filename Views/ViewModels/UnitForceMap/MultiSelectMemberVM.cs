using System.Collections.Generic;
using Sisgraph.Ips.Samu.AddIn.Models.UnitForceMap;
using Sisgraph.Ips.Samu.AddIn.Business.UnitForceMap;

namespace Sisgraph.Ips.Samu.AddIn.ViewModels.UnitForceMap
{
    public class MultiSelectMemberVM : ViewModelBase
    {
        #region Atributos
        private List<UnitCrewMember> _memberList;
        private UnitCrewMember _selectedMember;
        private string _parameter;
        private CrewMemberTypeEnum _crewMemberType;
        private string _title;
        #endregion

        #region Propriedades
        public List<UnitCrewMember> MemberList
        {
            get { return _memberList; }
            set
            {
                _memberList = value;
                OnPropertyChanged("MemberList");
            }
        }

        public UnitCrewMember SelectedMember
        {
            get { return _selectedMember; }
            set
            {
                _selectedMember = value;
                OnPropertyChanged("SelectedMember");
            }
        }

        public string Parameter
        {
            get { return _parameter; }
            set
            {
                _parameter = value;
                OnPropertyChanged("Parameter");
            }
        }

        public CrewMemberTypeEnum CrewMemberType
        {
            get { return _crewMemberType; }
            set
            {
                _crewMemberType = value;
                OnPropertyChanged("CrewMemberType");
            }
        }

        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                OnPropertyChanged("Title");
            }
        }

        public bool IsConfirmed { get; set; }

        public string UnitId { get; set; }
        #endregion

        #region Construtores
        public MultiSelectMemberVM(string parameter, CrewMemberTypeEnum crewMemberType, string unitId)
        {
            this.Parameter = parameter;
            this.CrewMemberType = crewMemberType;
            this.UnitId = unitId;
        }
        #endregion

        #region Métodos
        public void LoadData()
        {
            MemberList = UnitCrewMemberBusiness.GetByName(Parameter, CrewMemberType, UnitId);

            switch (CrewMemberType)
            {
                case CrewMemberTypeEnum.Driver:
                    Title = "Condutor";
                    break;
                case CrewMemberTypeEnum.Doctor:
                    Title = "Médico";
                    break;
                case CrewMemberTypeEnum.FirstAuxiliar:
                    Title = "Primeiro Auxiliar";
                    break;
                case CrewMemberTypeEnum.SecondAuxiliar:
                    Title = "Segundo Auxiliar";
                    break;
                case CrewMemberTypeEnum.ThirdAuxiliar:
                    Title = "Terceiro Auxiliar";
                    break;
                case CrewMemberTypeEnum.Nurse:
                    Title = "Enfermeiro";
                    break;
            }

            IsConfirmed = false;
        }

        public bool Confirm()
        {
            if (SelectedMember != null)
            {
                IsConfirmed = true;
                return true;
            }
            return false;
        }
        #endregion
    }
}
