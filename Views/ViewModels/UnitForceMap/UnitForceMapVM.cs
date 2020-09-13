using System;
using System.Linq;
using System.Collections.Generic;
using Sisgraph.Ips.Samu.AddIn.Models.UnitForceMap;
using Sisgraph.Ips.Samu.AddIn.Business.UnitForceMap;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls;
using Intergraph.IPS.CADCore;
using Sisgraph.Ips.Samu.AddIn.Models.CustomCad;

namespace Sisgraph.Ips.Samu.AddIn.ViewModels.UnitForceMap
{
    public class UnitForceMapVM : ViewModelBase
    {
        #region Atributos
        private string _selectedRegion;
        private DateTime _selectedDate;
        private WorkShiftModel _selectedWorkShift;
        private FilterTypeEnum? _selectedFilterType;
        private UnitVM _selectedUnit;
        private string _alertMessage = string.Empty;

        private List<string> _regionList;
        private List<WorkShiftModel> _workShiftList;
        private ObservableCollection<UnitVM> _unitList;

        private WorkShiftModel _currentWorkShift;
        private string _currentActiveFilter;
        private DateTime _currentFilteredDate;
        private WorkShiftModel _currentFilteredWorkShift;
        private WorkShiftModel.ShiftTime _currentFilterTime;
        private string _filterUnitCount;

        private bool _isFilterShown;
        #endregion

        #region Propriedades
        public string SelectedRegion
        {
            get { return _selectedRegion; }
            set
            {
                _selectedRegion = value;
                OnPropertyChanged("SelectedRegion");
            }
        }

        public string AlertMessage
        {
            get { return _alertMessage; }
            set { _alertMessage = value; OnPropertyChanged("AlertMessage"); OnPropertyChanged("ShowAlertMessage"); }
        }

        public bool ShowAlertMessage
        {
            get { 
                return (string.IsNullOrWhiteSpace(_alertMessage) ? false : true); 
            }
        }

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set
            {
                _selectedDate = value;
                OnPropertyChanged("SelectedDate");
            }
        }

        public FilterTypeEnum? SelectedFilterType
        {
            get { return _selectedFilterType; }
            set { _selectedFilterType = value; OnPropertyChanged("SelectedFilterType"); }
        }

        public WorkShiftModel SelectedWorkShift
        {
            get { return _selectedWorkShift; }
            set
            {
                _selectedWorkShift = value;
                OnPropertyChanged("SelectedWorkShift");
            }
        }

        public List<string> RegionList
        {
            get { return _regionList; }
            set
            {
                _regionList = value;
                OnPropertyChanged("RegionList");
            }
        }

        public string FilterUnitCount
        {
            get { return _filterUnitCount; }
            set { _filterUnitCount = value; OnPropertyChanged("FilterUnitCount"); }
        }

        public string CurrentActiveFilter
        {
            get { return _currentActiveFilter; }
            set { _currentActiveFilter = value; OnPropertyChanged("CurrentActiveFilter"); }
        }

        public WorkShiftModel.ShiftTime CurrentFilterTime
        {
            get { return _currentFilterTime; }
        }

        public List<WorkShiftModel> WorkShiftList
        {
            get { return _workShiftList; }
            set
            {
                _workShiftList = value;
                OnPropertyChanged("WorkShiftList");
            }
        }

        public UnitVM SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                _selectedUnit = value;
                OnPropertyChanged("SelectedUnit");
            }
        }

        public WorkShiftModel CurrentWorkShift
        {
            get { return _currentWorkShift; }
            set { _currentWorkShift = value; OnPropertyChanged("CurrentWorkShift"); }
        }
        
        public ObservableCollection<UnitVM> UnitList
        {
            get { return _unitList; }
            set
            {
                _unitList = value;
                OnPropertyChanged("UnitList");
            }
        }

        public bool IsFilterShown
        {
            get { return _isFilterShown; }
            set { _isFilterShown = value; OnPropertyChanged("IsFilterShown"); }
        }
        #endregion

        #region Métodos
        public void LoadData()
        {
            string returnMessage;

            SelectedFilterType = FilterTypeEnum.ALL;

            AlertMessage = UnitForceMapBusiness.GetAlertMessage();

            UnitList = new ObservableCollection<UnitVM>();
            WorkShiftList = UnitForceMapBusiness.GetWorkShiftList(out returnMessage);
            CurrentWorkShift = UnitForceMapBusiness.GetCurrentWorkShift(WorkShiftList);

            CleanFilter();
            GetUnitList();
            SetRegionList();
        }

        public void GetUnitList()
        {
            try
            {
                string returnMessage;
                UnitList = new ObservableCollection<UnitVM>();

                _currentFilteredDate = SelectedDate;
                _currentFilteredWorkShift = SelectedWorkShift;

                _currentFilterTime = VerifyShiftTime();

                List<UnitForceMapModel> unitList = UnitForceMapBusiness.GetUnitForceMapList(SelectedRegion,
                                                        SelectedDate, SelectedWorkShift, CurrentFilterTime, (int)SelectedFilterType, out returnMessage);

                foreach (var u in unitList)
                {
                    var unitVM = new UnitVM(u, "Red");
                    UnitList.Add(unitVM);
                }
                SelectedUnit = null;

                FilterUnitCount = unitList.Count.ToString();

                CurrentActiveFilter = SelectedDate.ToString("dd/MM/yyyy") +
                    " das " + SelectedWorkShift.InicialTime.Hours.ToString().PadLeft(2, '0') + " às " + SelectedWorkShift.FinalTime.Hours.ToString().PadLeft(2, '0') +
                    " (" + (SelectedFilterType == FilterTypeEnum.ALL ? "Todas AMs" : string.Empty) + (SelectedFilterType == FilterTypeEnum.OUT_SERVICE ? "AMs fora de serviço" : string.Empty) + (SelectedFilterType == FilterTypeEnum.IN_SERVICE ? "AMs em serviço" : string.Empty) +
                    (string.IsNullOrEmpty(SelectedRegion) ? string.Empty : " da região " + SelectedRegion) + ")";
            }
            catch (Exception ex)
            {
                ShowMessage(ex.Message);
            }
        }

        public void CloseAlertMessage()
        {
            this.AlertMessage = string.Empty;
        }

        public void CleanFilter()
        {
            SelectedRegion = string.Empty;

            SelectedWorkShift = CurrentWorkShift;

            DateTime date = DateTime.Now;

            if (SelectedWorkShift.FinalTime < SelectedWorkShift.InicialTime && date.Hour < CurrentWorkShift.FinalTime.Hours)
                date = date.AddDays(-1);

            SelectedDate = date;
        }

        private void SetRegionList()
        {
            RegionList = new List<string>();
            RegionList.Add(string.Empty);
            if (UnitList.Count > 0)
            {
                RegionList.AddRange(UnitList.Select(a => a.UnitModel.Station.DispatchGroup).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().OrderBy(p=>p.ToString()).ToList());
            }
        }

        public Style GetDataGridColorStyle()
        {
            Style gridStyle = new Style();
            DataTrigger gridDataTrigger = null;
            gridStyle.TargetType = typeof(DataGridRow);

            List<KeyValuePair<string, string>> RegionColorList = UnitForceMapBusiness.GetRegionColorList();

            foreach (KeyValuePair<string, string> Item in RegionColorList)
            {
                gridDataTrigger = new DataTrigger()
                {
                    Binding = new Binding("UnitModel.Station.DispatchGroup"),
                    Value = Item.Key
                };

                gridDataTrigger.Setters.Add(new Setter()
                {
                    Property = Control.BackgroundProperty,
                    Value = (SolidColorBrush)(new BrushConverter().ConvertFrom(Item.Value))
                });

                gridStyle.Triggers.Add(gridDataTrigger);
            }

            gridDataTrigger = new DataTrigger()
            {
                Binding = new Binding("UnitModel.StatusId"),
                Value = "12"
            };

            gridDataTrigger.Setters.Add(new Setter()
            {
                Property = Control.BackgroundProperty,
                Value = (SolidColorBrush)(new SolidColorBrush(Colors.LightGray))
            });

            gridStyle.Triggers.Add(gridDataTrigger);

            gridDataTrigger = new DataTrigger()
            {
                Binding = new Binding("UnitModel.StatusId"),
                Value = "14"
            };

            gridDataTrigger.Setters.Add(new Setter()
            {
                Property = Control.BackgroundProperty,
                Value = (SolidColorBrush)(new SolidColorBrush(Colors.Thistle))
            });

            gridStyle.Triggers.Add(gridDataTrigger);

            gridDataTrigger = new DataTrigger()
            {
                Binding = new Binding("UnitModel.WorkShiftStarted"),
                Value = "F"
            };

            gridDataTrigger.Setters.Add(new Setter()
            {
                Property = Control.FontWeightProperty,
                Value = FontWeights.Bold
            });

            gridDataTrigger.Setters.Add(new Setter()
            {
                Property = Control.ToolTipProperty,
                Value = "Viatura aguardando liberação para atualização."
            });
            
            gridStyle.Triggers.Add(gridDataTrigger);

            return gridStyle;
        }

        public void VerifyShiftChange()
        {
            WorkShiftModel VerifyWorkShift = UnitForceMapBusiness.GetCurrentWorkShift(WorkShiftList);

            if (VerifyWorkShift.ToString() != CurrentWorkShift.ToString())
                CurrentWorkShift = VerifyWorkShift;

            _currentFilterTime = VerifyShiftTime();
        }

        private WorkShiftModel.ShiftTime VerifyShiftTime()
        {
            DateTime CurrentShiftDate = DateTime.Now;

            if (CurrentShiftDate.Hour >= 0 && CurrentShiftDate.Hour < 7)
                CurrentShiftDate = CurrentShiftDate.AddDays(-1);

            if (CurrentShiftDate.ToString("dd/MM/yyyy") == _currentFilteredDate.ToString("dd/MM/yyyy"))
            {
                if (_currentFilteredWorkShift.ToString() == CurrentWorkShift.ToString())
                    return WorkShiftModel.ShiftTime.Actual;
                else if (_currentFilteredWorkShift.InicialTime < CurrentWorkShift.InicialTime)
                    return WorkShiftModel.ShiftTime.Backwards;
                else
                    return WorkShiftModel.ShiftTime.Forward;
            }
            else if (int.Parse(_currentFilteredDate.ToString("yyyyMMdd")) < int.Parse(CurrentShiftDate.ToString("yyyyMMdd")))
                return WorkShiftModel.ShiftTime.Backwards;
            else
                return WorkShiftModel.ShiftTime.Forward;
        }

        public void UpdateUnitInForceMap(string unitId)
        {
            UnitVM selectedUnit = this.UnitList.Where(uvm => uvm.UnitModel.UnitId == unitId)
                                                          .FirstOrDefault();

            if(selectedUnit != null)
            {
                selectedUnit.UnitModel = UnitForceMapBusiness.GetCurrentUnitForceMap(unitId);
                UnitForceMapBusiness.UpdateUnitForceMap(selectedUnit.UnitModel);
            }
                
        }

        public void StartQAP()
        {
            if (this.SelectedUnit.UnitModel.OutType != OutOfServiceTypeModel.OUT_TYPE_AGUARDANDO_QAP)
            {
                MessageBox.Show("Apenas viaturas \"Fora de Serviço - Aguardando QAP\" podem ser selecionadas", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (MessageBox.Show("Confirma o início de QAP da AM '" + this.SelectedUnit.UnitModel.UnitId + "'?\n\n (Essa ação não pode ser revertida)", "Atenção!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        UnitForceMapBusiness.StartQAP(this.SelectedUnit.UnitModel);
                        UnitForceMapBusiness.UpdateUnitForceMap(this.SelectedUnit.UnitModel);

                        //SelectedUnit.UnitModel = UnitForceMapBusiness.GetUnitForceMapModel(this.SelectedUnit.UnitModel.UnitId, DateTime.Now, this.CurrentWorkShift);
                    }
                    catch (Exception exception)
                    {
                        throw exception;
                    }
                }
            }
        }

        #endregion
    }
}
