using System;
using System.Collections.Generic;
using System.Linq;
using Sisgraph.Ips.Samu.AddIn.Models.Submission;
using Sisgraph.Ips.Samu.AddIn.Business;
using Sisgraph.Ips.Samu.AddIn.Business.CustomCad;
using Sisgraph.Ips.Samu.AddIn.Models.CustomCad;
using System.Diagnostics;
using Sisgraph.Ips.Samu.AddIn.Business.Submission;
using System.Globalization;
using log4net;
using System.Reflection;
using Sisgraph.Ips.Samu.AddIn.Business.RetainedStretcher;
using Sisgraph.Ips.Samu.AddIn.Models;
using Sisgraph.Ips.Samu.AddIn.Models.RetainedStretcher;

namespace Sisgraph.Ips.Samu.AddIn.ViewModels.Submission
{
    public class SubmissionVM : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        #region Atributos
        private EventSituationEnum? _eventSituation;
        private string _unitId;
        private string _agencyEventId;
        private bool _asExpected = true;
        private IList<CustomHospital> _hospitalList;
        private IList<CustomHospital> _displayHospitalList;
        private CustomHospital _selectedHospital;
        private CustomHospital _selectedHospitalClone;
        private VictimModel _victimData;
        private string _remarks;
        private bool _isSubmitted;
        private IList<string> _hospitalTypeList;
        private string _selectedHospitalType;
        private IList<KeyValuePair<string, string>> _reasonList;
        private IList<KeyValuePair<string, string>> _situationFoundList;
        private KeyValuePair<string, string>? _selectedReason;
        private string _dispositionNotFoundTypeCode;
        private string _dispositionRemovedTypeCode;
        //private string _dispositionRetainedStretcher;
        private string _hospitalFilter;
        private bool _hospitalHasChanged;
        private bool _isNotUram;
        private bool _isRetainedStretcherSituation;
        private bool _hasNotRetainedStretcher;
        #endregion

        #region Construtores
        public SubmissionVM(string unitId, string agencyEventId, bool isSubmitted)
        {
            this.IsSubmitted = isSubmitted;
            this.UnitId = unitId;
            this.AgencyEventId = agencyEventId;
            this.HospitalHasChanged = false;
            this.HasNotRetainedStretcher = true;
        }

        //public SubmissionVM(string unitId, string agencyEventId, bool isSubmitted)
        //{
        //    this.IsSubmitted = isSubmitted;
        //    this.UnitId = unitId;
        //    this.AgencyEventId = agencyEventId;
        //    this.HospitalHasChanged = false;
        //}


        #endregion

        #region Propriedades
        public EventSituationEnum? EventSituation
        {
            get { return _eventSituation; }
            set
            {
                if (!_eventSituation.HasValue || _eventSituation.Value != value)
                {
                    _eventSituation = value;
                    OnPropertyChanged("EventSituation");
                    SelectedHospitalType = null;
                    SelectedHospital = null;
                    SelectedReason = null;
                }
            }
        }

        public bool AsExpected
        {
            get { return _asExpected; }
            set
            {
                _asExpected = value;
                OnPropertyChanged("AsExpected");
            }
        }

        public string SelectedHospitalType
        {
            get { return _selectedHospitalType; }
            set
            {
                _selectedHospitalType = value;
                OnPropertyChanged("SelectedHospitalType");
            }
        }

        public IList<CustomHospital> HospitalList
        {
            get { return _hospitalList; }
            set
            {
                _hospitalList = value;
                OnPropertyChanged("HospitalList");
            }
        }

        public IList<CustomHospital> DisplayHospitalList
        {
            get { return _displayHospitalList; }
            set
            {
                _displayHospitalList = value;
                OnPropertyChanged("DisplayHospitalList");
            }
        }

        public IList<string> HospitalTypeList
        {
            get { return _hospitalTypeList; }
            set
            {
                _hospitalTypeList = value;
                OnPropertyChanged("HospitalTypeList");
            }
        }

        public string HospitalFilter
        {
            get { return _hospitalFilter; }
            set
            {
                _hospitalFilter = value;
                OnPropertyChanged("HospitalFilter");
            }
        }

        public string UnitId
        {
            get { return _unitId; }
            set
            {
                _unitId = value;
                OnPropertyChanged("UnitId");
            }
        }

        public string AgencyEventId
        {
            get { return _agencyEventId; }
            set { _agencyEventId = value; OnPropertyChanged("AgencyEventId"); }
        }

        public CustomHospital SelectedHospital
        {
            get { return _selectedHospital; }
            set
            {
                _selectedHospital = value;
                OnPropertyChanged("SelectedHospital");
            }
        }

        public VictimModel VictimData
        {
            get { return _victimData; }
            set
            {
                _victimData = value;
                OnPropertyChanged("VictimData");
            }
        }

        public string Remarks
        {
            get { return _remarks; }
            set
            {
                _remarks = value;
                OnPropertyChanged("Remarks");
            }
        }

        public bool IsSubmitted
        {
            get { return _isSubmitted; }
            set
            {
                _isSubmitted = value;
                OnPropertyChanged("IsSubmitted");
            }
        }

        public IList<KeyValuePair<string,string>> ReasonList
        {
            get { return _reasonList; }
            set
            {
                _reasonList = value;
                OnPropertyChanged("ReasonList");
            }
        }

        public IList<KeyValuePair<string, string>> SituationFoundList
        {
            get { return _situationFoundList; }
            set
            {
                _situationFoundList = value;
                OnPropertyChanged("SituationFoundList");
            }
        }

        public KeyValuePair<string, string>? SelectedReason
        {
            get { return _selectedReason; }
            set
            {
                _selectedReason = value;
                OnPropertyChanged("SelectedReason");
            }
        }

        public bool IsRemovedSituationEnable
        {
            get
            {
                return IsNotUram && HasNotRetainedStretcher;
            }
        }

        public bool HospitalHasChanged
        {
            get { return _hospitalHasChanged; }
            set
            {
                if (value != _hospitalHasChanged)
                {
                    if (value)
                    {
                        if (SelectedHospital != null)
                            _selectedHospitalClone = (CustomHospital)SelectedHospital.Clone();
                    }
                    else
                    {
                        SelectedHospital = _selectedHospitalClone;
                    }

                    _hospitalHasChanged = value;
                    OnPropertyChanged("HospitalHasChanged");
                }
            }
        }

        public bool IsRetainedStretcherSituation
        {
            get 
            {
                //TODO: Temporário: Voltar para ativar recurso de Maca Retida
                return false;// _isRetainedStretcherSituation;
            }
            set
            {
                _isRetainedStretcherSituation = value;
                OnPropertyChanged("IsRetainedStretcherSituation");
            }
        }

        public bool HasNotRetainedStretcher
        {
            get
            {
                return _hasNotRetainedStretcher;
            }
            set
            {
                _hasNotRetainedStretcher = value;
                OnPropertyChanged("HasNotRetainedStretcher");
            }

        }

        public bool IsNotUram
        {
            get { return _isNotUram; }
            set
            {
                _isNotUram = value;
                OnPropertyChanged("IsNotUram");
            }
        }
        #endregion

        #region Métodos
        public bool LoadData()
        {
            try
            {
                //INICIALMENTE SETADO COMO NÃO ENCONTRADO, ABAIXO SE ENCONTRAR REGISTRO SETA PARA OUTRO.
                this.EventSituation = EventSituationEnum.NotFound;
                this.VictimData = new VictimModel();

                this.ReasonList = EventTypeBusiness.GetDispositionCodeList(true);

                if (this.ReasonList.Count == 0)
                    return false;
                
                this.SituationFoundList = EventTypeBusiness.GetEventTypeList();
                this.HospitalList = SamuBusiness.GetHospitalList();
                this.DisplayHospitalList = HospitalList;
                this.IsNotUram = (UnitBusiness.GetUnitType(this.UnitId) != "URAM");

                UnitWithRetainedStretcherModel unitWithRetainedStretcher = null;
                //TODO: Temporário: Voltar para ativar recurso de Maca Retida
                /*    RetainedStretcherBusiness.GetUnitsWithRetainedStretcher("SAMU", PersonBusiness.GetCurrentUser(), this.UnitId, isOnlyWithStatusRetainedStretcher: false)
                    .FirstOrDefault();*/

                this.HasNotRetainedStretcher = unitWithRetainedStretcher == null ? true : false; 

                HospitalTypeList = HospitalList.Select(p => p.HospitalGroup).Distinct().ToList();

                _dispositionNotFoundTypeCode = CadBusiness.GetCadParameterListItem("sisgraph", "SubmissionWindowParams", "DispositionNotFoundCode");
                _dispositionRemovedTypeCode = CadBusiness.GetCadParameterListItem("sisgraph", "SubmissionWindowParams", "DispositionRemovedCode");
                //_dispositionRetainedStretcher = CadBusiness.GetCadParameterListItem("sisgraph", "SubmissionWindowParams", "DispositionRetainedStretcher");

                if (_dispositionNotFoundTypeCode == null || _dispositionRemovedTypeCode == null) //|| _dispositionRetainedStretcher == null
                {
                    throw new Exception("Não foi possível carregar os parâmetros do ICAD.");
                }

                if (_isSubmitted)
                {
                    SubmissionModel loadedSubmissionModel = SubmissionBusiness.GetLoadedSubmission(UnitId, AgencyEventId);

                    if (loadedSubmissionModel == null)
                    {
                        //{
                        //    throw new Exception("Dados de finalização não encontrados.");
                        //}
                        loadedSubmissionModel = new SubmissionModel();

                        loadedSubmissionModel.DispositionCode = _dispositionRemovedTypeCode;

                        this.EventSituation = EventSituationEnum.Removed;

                        this.HospitalHasChanged = true;
                    }
                    else
                    {
                        //ToString: complete
                        if (loadedSubmissionModel.DispositionCode == _dispositionNotFoundTypeCode)
                            this.EventSituation = EventSituationEnum.NotFound;
                        else if (loadedSubmissionModel.DispositionCode == _dispositionRemovedTypeCode)
                            this.EventSituation = EventSituationEnum.Removed;
                        //else if (loadedSubmissionModel.DispositionCode == _dispositionRetainedStretcher)
                        //    this.EventSituation = EventSituationEnum.RetainedStretcher;
                        else
                            this.EventSituation = EventSituationEnum.ResolvedOnLocal;



                        this.SelectedReason = loadedSubmissionModel.SelectedReason;

                        this.AsExpected = loadedSubmissionModel.AsExpected;

                        this.VictimData = loadedSubmissionModel.Victim;

                        if (!string.IsNullOrEmpty(loadedSubmissionModel.Victim.SituationFound.Key))
                            this.VictimData.SituationFound = SituationFoundList.Where(p => p.Key == loadedSubmissionModel.Victim.SituationFound.Key).FirstOrDefault();

                        if (loadedSubmissionModel.SelectedHospital != null)
                        {
                            this.SelectedHospitalType = loadedSubmissionModel.SelectedHospital.HospitalGroup;

                            this.SelectedHospital = HospitalList.Where(p => p.Cnes == loadedSubmissionModel.SelectedHospital.Cnes).FirstOrDefault();
                        }

                        this.Remarks = loadedSubmissionModel.Remarks;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error("Ocorreu um erro carregando a ViewModel da janela de finalização de evento.", ex);

                throw new Exception("Falha ao carregar dados de finalização");
            }
        }

        public void RefreshHospitalList()
        {
            var filteredList = (from h in HospitalList
                                where (string.IsNullOrEmpty(SelectedHospitalType) || h.HospitalGroup.Equals(SelectedHospitalType))
                                        && (string.IsNullOrEmpty(HospitalFilter) 
                                                || CultureInfo.CurrentCulture.CompareInfo.IndexOf(h.Name, HospitalFilter, CompareOptions.IgnoreCase) >= 0) 
                                select h).ToList();

            DisplayHospitalList = filteredList;
        }

        public bool Confirm()
        {
            try
            {
                if (EventSituation == null)
                {
                    ShowMessage("Selecione a situação");
                }
                else
                {
                    DispositionCodeModel dispositionCode = GetDispositionCode();
                    if (dispositionCode != null && !string.IsNullOrEmpty(dispositionCode.Id))
                    {
                        if (!IsSubmitted)
                        {
                            if (EventSituation == EventSituationEnum.NotFound)
                                VictimData = new VictimModel();
                            else
                            {
                                if (AsExpected == false)
                                {
                                    if (string.IsNullOrEmpty(VictimData.SituationFound.Key))
                                    {
                                        ShowMessage("Favor selecionar a situação encontrada.");

                                        return false;
                                    }
                                }
                                else
                                {
                                    VictimData.SituationFound = new KeyValuePair<string, string>();
                                }
                            }

                            SubmissionBusiness.ExecuteCadUpdates(UnitId, AgencyEventId, EventSituation.Value, dispositionCode, Remarks, SelectedHospital, VictimData);
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(VictimData.Name.Trim()))
                            {
                                ShowMessage("Favor inserir o nome da vítima.");

                                return false;
                            }

                            if (VictimData.Age != null)
                            {
                                if (VictimData.AgeType == null || VictimData.AgeType == 0)
                                {
                                    ShowMessage("Favor selecionar um tipo de idade válido para a vítima.");

                                    return false;
                                }
                            }

                            if (VictimData.AgeType != null && VictimData.AgeType.Value > 0)
                            {
                                if (VictimData.Age == null)
                                {
                                    ShowMessage("Favor informar uma idade válida para a vítima.");

                                    return false;
                                }
                            }

                            if (AsExpected == false)
                            {
                                if (string.IsNullOrEmpty(VictimData.SituationFound.Key))
                                {
                                    ShowMessage("Favor selecionar a situação encontrada.");

                                    return false;
                                }
                            }
                            else
                                VictimData.SituationFound = new KeyValuePair<string, string>();

                            string selectedHospitalName = SelectedHospital != null ? SelectedHospital.Name : string.Empty;
                            SubmissionBusiness.ExecuteCadEndUpdates(UnitId, AgencyEventId, EventSituation.Value, dispositionCode, Remarks, selectedHospitalName, HospitalHasChanged, VictimData, IsRetainedStretcherSituation);
                        }

                        SubmissionBusiness.SaveSubmission(UnitId, AgencyEventId, dispositionCode, AsExpected, SelectedHospital, Remarks, VictimData);

                        if (!HasNotRetainedStretcher && !IsRetainedStretcherSituation) 
                        {
                            UserModel currentUser = PersonBusiness.GetCurrentUser();
                            RetainedStretcherBusiness.ComeBackUnitStatusToRetainedStretcher(UnitId, currentUser);
                        }

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("Add/In.: Falha na finalização, Submission.", ex);

                ShowMessage("Falha ao realizar solicitação.");
            }

            return false;
        }

        private DispositionCodeModel GetDispositionCode()
        {
            DispositionCodeModel dispositionCodeModel = new DispositionCodeModel();
            
            switch (EventSituation.Value)
            {
                case EventSituationEnum.NotFound:
                    dispositionCodeModel.Id = _dispositionNotFoundTypeCode;
                    break;
                case EventSituationEnum.ResolvedOnLocal:
                    if (SelectedReason == null)
                        ShowMessage("Favor selecionar o motivo da solução no local.");
                    else
                    {
                        dispositionCodeModel.Id = SelectedReason.Value.Key;
                        dispositionCodeModel.Description = SelectedReason.Value.Value;
                    }

                    break;
                case EventSituationEnum.Removed:
                    if (SelectedHospital == null)
                    {
                        ShowMessage("Favor selecionar o hospital da remoção.");

                        return null;
                    }
                    else
                        dispositionCodeModel.Id = _dispositionRemovedTypeCode;
                    break;
                /*case EventSituationEnum.RetainedStretcher:
                    if (SelectedHospital == null)
                    {
                        ShowMessage("Favor selecionar o hospital da maca retida.");
                        return null;
                    }
                    dispositionCodeModel.Id = _dispositionRetainedStretcher;
                    break;*/
            }

            return dispositionCodeModel;
        }
        #endregion
    }
}
