using System.Collections.Generic;
using Sisgraph.Ips.Samu.AddIn.Models.CustomCad;
using Sisgraph.Ips.Samu.AddIn.Business.CustomCad;
using Sisgraph.Ips.Samu.AddIn.Business.UnitForceMap;

namespace Sisgraph.Ips.Samu.AddIn.ViewModels.CustomCad
{
    public class CustomUnitOutOfServiceVM : ViewModelBase
    {
        #region Atributos
        private string _unitId;
        private string _location;
        private string _mileage;
        private List<OutOfServiceTypeModel> _outTypeList;
        private OutOfServiceTypeModel _selectedOutType;
        private string _alarmTime;
        private string _remarks;
        #endregion

        #region Construtor
        public CustomUnitOutOfServiceVM(string UnitId)
        {
            List<OutOfServiceTypeModel.OutOfServiceSpecialType> SpecialTypes = new List<OutOfServiceTypeModel.OutOfServiceSpecialType>();
            SpecialTypes.Add(OutOfServiceTypeModel.OutOfServiceSpecialType.NORMAL);
            SpecialTypes.Add(OutOfServiceTypeModel.OutOfServiceSpecialType.TECH_STOP);
            SpecialTypes.Add(OutOfServiceTypeModel.OutOfServiceSpecialType.NO_OPERATIONAL_SELECTABLE);

            this.UnitId = UnitId;

            OutTypeList = EventTypeBusiness.GetOutTypeList(SpecialTypes);
        }
        #endregion

        #region Propriedades
        public string UnitId
        {
            get { return _unitId; }
            set { _unitId = value; OnPropertyChanged("UnitId"); }
        }

        public string Location
        {
            get { return _location; }
            set { _location = value; OnPropertyChanged("Location"); }
        }

        public string Mileage
        {
            get { return _mileage; }
            set { _mileage = value; OnPropertyChanged("Mileage"); }
        }

        public List<OutOfServiceTypeModel> OutTypeList
        {
            get { return _outTypeList; }
            set { _outTypeList = value; OnPropertyChanged("OutTypeList"); }
        }

        public OutOfServiceTypeModel SelectedOutType
        {
            get { return _selectedOutType; }
            set { _selectedOutType = value; OnPropertyChanged("SelectedOutType"); }
        }

        public string AlarmTime
        {
            get { return _alarmTime; }
            set 
            {
                _alarmTime = value;
                OnPropertyChanged("AlarmTime");
            }
        }

        public string Remarks
        {
            get { return _remarks; }
            set { _remarks = value; OnPropertyChanged("Remarks"); }
        }
        #endregion

        #region Métodos
        public bool Confirm()
        {
            try
            {
                //remove equipe caso tipo de Fora de Serviço seja não operacional, desde que não seja por equipe incompleta
                if (!string.IsNullOrEmpty(SelectedOutType.OutServiceTypeId) && 
                              SelectedOutType.OutServiceTypeId.StartsWith("NO") &&
                              !SelectedOutType.OutServiceTypeId.Equals("NO_EQI") && 
                              !SelectedOutType.OutServiceTypeId.Equals("NO_SEQ"))
                {
                    if (!ShowConfirmMessage("Colocar uma AM como 'Não Operacional' fará ela PERDER SUA EQUIPE. Deseja realmente continuar?"))
                    {
                        return false;
                    }

                    UnitBusiness.LogOutAllMembers(UnitId);
                    UnitForceMapBusiness.LogOutAllMembers(UnitId);
                    UnitForceMapBusiness.RemoveSubstituteUnit(UnitId);
                }

                int alarmTime = (string.IsNullOrEmpty(AlarmTime) ? 0 : int.Parse(AlarmTime));
                Location = (string.IsNullOrEmpty(Location) ? string.Empty : Location);
                double? mileage = (string.IsNullOrEmpty(Mileage) ? null : (double?)double.Parse(Mileage));

                UnitBusiness.UnitOutOfService(UnitId, SelectedOutType.OutServiceTypeId, Location, alarmTime, mileage, Remarks);

                return true;
            }
            catch
            {
                ShowMessage("Falha ao realizar solicitação");
            }

            return false;
        }
        #endregion
    }
}
