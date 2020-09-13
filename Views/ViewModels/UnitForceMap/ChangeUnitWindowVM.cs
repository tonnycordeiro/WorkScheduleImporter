using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using Sisgraph.Ips.Samu.AddIn.Business.UnitForceMap;
using System.Windows;
using Sisgraph.Ips.Samu.AddIn.Models.UnitForceMap;
using Sisgraph.Ips.Samu.AddIn.Business;
using Sisgraph.Ips.Samu.AddIn.Models.CustomCad;
using Sisgraph.Ips.Samu.AddIn.Business.CustomCad;

namespace Sisgraph.Ips.Samu.AddIn.ViewModels.UnitForceMap
{
    public class ChangeUnitWindowVM : ViewModelBase
    {
        #region Atributos
        private UnitForceMapModel _currentUnitForceMap;
        private List<OutOfServiceTypeModel> _outServiceTypeList;
        private OutOfServiceTypeModel _selectedOutServiceType;
        private OutOfServiceTypeModel _selectedChangeReason;
        private string _selectedTargetUnitId;
        private List<string> _reserveUnitList;
        #endregion

        #region Construtores
        public ChangeUnitWindowVM(UnitForceMapModel UnitForceMap)
        {
            CurrentUnitForceMap = UnitForceMap;

            string agencyId = "SAMU";

            LoadOutServiceTypeList(agencyId);
            this.LoadReserveUnitList(agencyId);
        }
        #endregion

        #region Propriedades
        public UnitForceMapModel CurrentUnitForceMap
        {
            get { return _currentUnitForceMap; }
            set
            {
                _currentUnitForceMap = value;
                OnPropertyChanged("CurrentUnitForceMap");
            }
        }

        public List<OutOfServiceTypeModel> OutServiceTypeList
        {
            get { return _outServiceTypeList; }
            set
            {
                _outServiceTypeList = value;
                OnPropertyChanged("OutServiceTypeList");
            }
        }

        public OutOfServiceTypeModel SelectedOutServiceType
        {
            get { return _selectedOutServiceType; }
            set
            {
                _selectedOutServiceType = value;
                OnPropertyChanged("SelectedOutServiceType");
            }
        }

        public OutOfServiceTypeModel SelectedChangeReason
        {
            get { return _selectedChangeReason; }
            set { _selectedChangeReason = value; OnPropertyChanged("SelectedChangeReason"); }
        }

        public string SelectedTargetUnitId
        {
            get { return _selectedTargetUnitId; }
            set { _selectedTargetUnitId = value; OnPropertyChanged("SelectedTargetUnitId"); }
        }

        public List<string> ReserveUnitList
        {
            get { return _reserveUnitList; }
            set { _reserveUnitList = value; OnPropertyChanged("ReserveUnitList"); }
        }

        public UnitForceMapModel NewUnitForceMap { get; set; }

        public UnitForceMapModel TargetUnitForceMap { get; set; }
        #endregion

        #region Métodos
        public void LoadReserveUnitList(string agencyId)
        {
            List<string> specialOutTypeList = new List<string>();

            List<OutOfServiceTypeModel.OutOfServiceSpecialType> SpecialTypes = new List<OutOfServiceTypeModel.OutOfServiceSpecialType>()
            {
                OutOfServiceTypeModel.OutOfServiceSpecialType.NO_OPERATIONAL,
                OutOfServiceTypeModel.OutOfServiceSpecialType.TECH_STOP
            };
            
            //if (_selectedOutServiceType != null)
            ReserveUnitList = UnitBusiness.GetNotAssignedUnits(agencyId).OrderBy(u => Int32.Parse(u)).ToList<string>();
        }

        private void LoadOutServiceTypeList(string agencyId)
        {
            OutServiceTypeList = UnitForceMapBusiness.GetSpecialOutOfServiceList(agencyId, CurrentUnitForceMap.UnitId);
        }

        public bool ExecuteUnitChange()
        {
            if (_selectedChangeReason == null)
            {
                MessageBox.Show("Favor selecionar o motivo de saída de serviço da AM.", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else if (string.IsNullOrEmpty(_selectedTargetUnitId))
            {
                MessageBox.Show("Favor selecionar a AM que entrará em serviço.", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
            else
            {
                try
                {
                    TargetUnitForceMap = new UnitForceMapModel() { UnitId = _selectedTargetUnitId };

                    return UnitForceMapBusiness.ExchangeUnit(_currentUnitForceMap, TargetUnitForceMap, _selectedChangeReason);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                catch (Exception)
                {
                    MessageBox.Show("Não foi possível substituir a AM.", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }

            return false;
        }
        #endregion
    }
}
