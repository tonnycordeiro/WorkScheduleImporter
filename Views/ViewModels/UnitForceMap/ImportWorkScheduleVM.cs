using System;
using System.Collections.Generic;
using System.Linq;
using Sisgraph.Ips.Samu.AddIn.Models;
using Sisgraph.Ips.Samu.AddIn.Models.UnitForceMap;
using Sisgraph.Ips.Samu.AddIn.Business.UnitForceMap;
using Sisgraph.Ips.Samu.AddIn.Business;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using Sisgraph.Ips.Samu.AddIn.Models.UnitForceMap.WorkShiftPeriod;

namespace Sisgraph.Ips.Samu.AddIn.ViewModels.UnitForceMap
{
    public class ImportWorkScheduleVM : ViewModelBase, IDisposable
    {
        #region Atributos
        private string _fullPathFile;
        private WorkScheduleImportTemplateModel _workScheduleImported;
        private ReportValidationModel _reportValidation;
        private ObservableCollection<ReportValidationItemModel> _revortValidationList;
        private bool _enableImporting;
        private bool _isLoading;
        private string _loadingText;

        private DateTime? _startDate;
        private DateTime? _endDate;
        private bool _isCreatingWorkSchedule;
        private bool _isUpdatingWorkSchedule;
        private bool _isMergingWorkSchedule;
        private bool _isPeriodicWorkSchedule;
        private List<WorkShiftPeriodBuilder> _workShiftPeriodBuilderList;

        List<WorkShiftModel> _workShiftList;

        //Dictionary<WorkScheduleValidationType, WorkScheduleValidationTypeMessage> _validationsByType;

        #endregion

        #region Construtores
        public ImportWorkScheduleVM(List<WorkShiftModel> availableWorkshiftList)
        {
            _reportValidation = new ReportValidationModel();
            _workScheduleImported = new WorkScheduleImportTemplateModel();
            _workShiftList = availableWorkshiftList;
            //_validationsByType = 
            ReportValidationBusiness.LoadValidationsByType();
            _enableImporting = false;
            //ViewModel.ShowMessageRequested += ShowMessage;
            _loadingText = String.Empty;
            _isLoading = false;
            _workShiftPeriodBuilderList = new List<WorkShiftPeriodBuilder>();

            IsCreatingWorkSchedule = true;
            IsUpdatingWorkSchedule = false;
            IsMergingWorkSchedule = false;
        }
        #endregion

        #region Propriedades

        public string FullPathFile
        {
            get { return _fullPathFile; }
            set { _fullPathFile = value; OnPropertyChanged("FullPathFile"); }
        }

        public WorkScheduleImportTemplateModel WorkScheduleImported
        {
            get { return _workScheduleImported; }
            set
            {
                _workScheduleImported = value;
                OnPropertyChanged("WorkScheduleImported");
            }
        }

        public bool EnableImporting
        {
            get { return _enableImporting; }
            set
            {
                _enableImporting = value;
                OnPropertyChanged("EnableImporting");
                
            }
        }

        public ReportValidationModel ReportValidation
        {
            get { return _reportValidation; }
            set
            {
                _reportValidation = value;
                OnPropertyChanged("ReportValidation");
            }
        }

        public ObservableCollection<ReportValidationItemModel> RevortValidationList
        {
            get { return _revortValidationList; }
            set
            {
                _revortValidationList = value;
                OnPropertyChanged("RevortValidationList");
            }

        }
        
        public bool IsLoading
        {
            get { return _isLoading; }
            set
            {
                _isLoading = value;
                OnPropertyChanged("IsLoading");

            }
        }

        public string LoadingText
        {
            get { return _loadingText; }
            set
            {
                _loadingText = value;
                OnPropertyChanged("LoadingText");

            }
        }

        public bool IsCreatingWorkSchedule
        {
            get { return _isCreatingWorkSchedule; }
            set
            {
                if (this.EnableImporting && _isCreatingWorkSchedule != value)
                    CleanFields();

                _isCreatingWorkSchedule = value;


                OnPropertyChanged("IsCreatingWorkSchedule");            }
        }

        public bool IsUpdatingWorkSchedule
        {
            get { return _isUpdatingWorkSchedule; }
            set
            {
                if (this.EnableImporting && _isUpdatingWorkSchedule != value)
                    CleanFields();

                _isUpdatingWorkSchedule = value;

                OnPropertyChanged("IsUpdatingWorkSchedule");
            }
        }

        public bool IsMergingWorkSchedule
        {
            get { return _isMergingWorkSchedule; }
            set
            {
                if (this.EnableImporting && _isMergingWorkSchedule != value)
                    CleanFields();

                _isMergingWorkSchedule = value;

                OnPropertyChanged("IsMergingWorkSchedule");
            }
        }

        public bool IsPeriodicWorkSchedule
        {
            get { return _isPeriodicWorkSchedule; }
            set
            {
                if (this.EnableImporting && _isPeriodicWorkSchedule != value)
                    CleanFields();

                _isPeriodicWorkSchedule = value;


                OnPropertyChanged("IsPeriodicWorkSchedule");
            }
        }
        
        public DateTime? StartDate
        {
            get { return _startDate; }
            set
            {
                if (this.EnableImporting && 
                    (_startDate.HasValue?_startDate.Value:DateTime.MinValue).CompareTo(
                        (value.HasValue ? value.Value : DateTime.MinValue)) != 0)
                    CleanFields();

                _startDate = value;

                OnPropertyChanged("StartDate");
            }
        }

        public DateTime? EndDate
        {
            get { return _endDate; }
            set
            {
                if (this.EnableImporting &&
                    (_endDate.HasValue ? _endDate.Value : DateTime.MinValue).CompareTo(
                        (value.HasValue ? value.Value : DateTime.MinValue)) != 0)
                                    CleanFields();

                _endDate = value;

                OnPropertyChanged("EndDate");
            }
        }

        /*public Dictionary<WorkScheduleValidationType, WorkScheduleValidationTypeMessage> ValidationsByType {
            get { return _validationsByType; }
            set
            {
                _validationsByType = value;
                OnPropertyChanged("ValidationsByType");
            }
        }*/

        #endregion

        #region Métodos

        public void LoadValidation_DoWork(object sender, DoWorkEventArgs e)
        {
            if (IsLoading)
                return;

            try
            {
                EnableLoadingProgress("Validando escala");
                string agencyId = "SAMU";
                WorkScheduleImported = new WorkScheduleImportTemplateModel();
                RevortValidationList = new ObservableCollection<ReportValidationItemModel>();

                if (this.IsPeriodicWorkSchedule)
                {
                    if (!ValidatePeriodicWorkScheduleFilter())
                    {
                        DisableLoadingProgress();
                        return;
                    }
                }

                //WorkScheduleImported.WorkScheduleForUnitList 
                    
                List<WorkScheduleForUnitModel> workScheduleList = UnitForceMapBusiness.ExtractWorkSchedule(this.FullPathFile).ToList<WorkScheduleForUnitModel>();

                if (this.IsPeriodicWorkSchedule)
                {
                    bool hasError = false;

                    if (workScheduleList.Where(ws => String.IsNullOrEmpty(ws.DateFrequence)).Count() > 0)
                    {
                        DisableLoadingProgress();
                        MessageBox.Show("A frequência de datas deve ser informada em todas as escalas da planilha.\nFavor verificar a planilha de importação.");
                        return;
                    }

                    CompleteWithPeriodicWorkSchedule(workScheduleList, ref hasError);

                    if (hasError || (IsUpdatingWorkSchedule && !ValidateUpdatingWorkSchedule()))
                    {
                        DisableLoadingProgress();
                        return;
                    }


                }
                else
                {
                    WorkScheduleImported.WorkScheduleForUnitList = workScheduleList;
                }

                ReportValidation = new ReportValidationModel();
                ReportValidationBusiness.UpdateReportValidation(WorkScheduleImported, _workShiftList, agencyId, ReportValidation);

                

                if (IsUpdatingWorkSchedule)
                {
                    this.ReportValidation.ReportValidationItemList.RemoveAll(
                        r => r.WorkScheduleValidationType == WorkScheduleValidationType.CONFLICT_WITH_PREVIOUS_DATA ||
                            r.WorkScheduleValidationType == WorkScheduleValidationType.DATA_SHOULD_BE_FILLED ||
                            r.WorkScheduleValidationType == WorkScheduleValidationType.INVALID_CREW_FORMATION);
                }

                                


                EnableImporting = !ReportValidationBusiness.AreThereErrorMessages(this.ReportValidation);
            }
            catch (SheetNotFoundException exception)
            {
                WorkScheduleImported.WorkScheduleForUnitList = new List<WorkScheduleForUnitModel>();
                ReportValidation = new ReportValidationModel();
                EnableImporting = false;

                ReportValidation.AddReportItem(WorkScheduleValidationType.SHEET_NAME_NOT_FOUND, WorkScheduleImportTemplateModel.WORK_SCHEDULE_MAIN_SHEET_NAME.ToUpper());
            }
            catch (Exception exception)
            {
                WorkScheduleImported.WorkScheduleForUnitList = new List<WorkScheduleForUnitModel>();
                ReportValidation = new ReportValidationModel();
                EnableImporting = false;

                MessageBox.Show(String.Format("Erro durante a validação do arquivo: {0}", exception.Message));
                //ShowMessage(String.Format("Erro durante a validação do arquivo: {0}", exception.Message));

                return;
            }
            finally
            {
                DisableLoadingProgress();
            }

        }

        public void ImportWorkSchedule_DoWork(object sender, DoWorkEventArgs e)
        {
            List<UnitForceMapModel> unitForceMapList = new List<UnitForceMapModel>();

            if (IsLoading)
                return;

            try
            {
                EnableLoadingProgress("Importando escala");

                unitForceMapList.AddRange(WorkScheduleImported.WorkScheduleForUnitList.Select(ws => ws.ToUnitForceMapModel(_workShiftList)));

                if (unitForceMapList.Where(uf => uf == null).Count() > 0)
                    throw new Exception("Conversão para o mapa força de viatura falhou");

                foreach (UnitForceMapModel uf in unitForceMapList)
                {
                    UnitForceMapBusiness.UpdateFutureUnitForceMap(uf, isCrewUpdating: IsUpdatingWorkSchedule);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(String.Format("Erro na Importação: {0}", exception.Message));
                //ShowMessage(String.Format("Erro na Importação: {0}", exception.Message));
                return;
            }
            finally
            {
                DisableLoadingProgress();
            }

            MessageBox.Show("Importação finalizada com sucesso.");

            CleanFields();

            //ShowMessage("Importação finalizada com sucesso.");

        }

        private void EnableLoadingProgress(string message)
        {
            IsLoading = true;
            LoadingText = String.Format("{0}. Aguarde...",message);
        }

        private void DisableLoadingProgress()
        {
            IsLoading = false;
            LoadingText = String.Empty;
        }

        private void CleanFields()
        {
            ReportValidation = new ReportValidationModel();
            RevortValidationList = new ObservableCollection<ReportValidationItemModel>();
            WorkScheduleImported.WorkScheduleForUnitList = new List<WorkScheduleForUnitModel>();
            WorkScheduleImported = new WorkScheduleImportTemplateModel();
            EnableImporting = false;
            FullPathFile = String.Empty;
            CleanWorkSchedulePeriodicFields();
        }

        public void CleanWorkSchedulePeriodicFields()
        {
            this.StartDate = null;
            this.EndDate = null;
            this.IsCreatingWorkSchedule = false;
            this.IsUpdatingWorkSchedule = false;
            this.IsMergingWorkSchedule = false;
        }


        public void OrderWorkScheduleBySelectedError(List<ReportValidationItemModel> reportedItens)
        {
            List<string> selectedIds = new List<string>();

            foreach(ReportValidationItemModel report in reportedItens)
            {
                selectedIds.AddRange(report.WorkScheduleIds);
            }

            this.WorkScheduleImported.WorkScheduleForUnitList = this.WorkScheduleImported.WorkScheduleForUnitList.OrderByDescending(ws => selectedIds.Contains(ws.ID)).ToList();
        }

        private string GetFileNameSuggestion()
        {
            return String.Format("{0}{1}", "Escala_", DateTime.Now.ToString("yyyyMMdd"));
        }

        public void OpenFile()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = String.Empty;  // GetFileNameSuggestion(); // Default file name 
            dlg.DefaultExt = ".xlsm"; // Default file extension 
            dlg.Filter = "Arquivos do Excel|*.xls;*.xlsx;*.xlsm"; // Filter files by extension
            dlg.Title = "Importação de arquivo";
            // Show open file dialog box 
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results 
            if (result == true)
            {
                // Open document 
                this.FullPathFile = dlg.FileName;
                //this.LoadValidation();
            }

        }

        public void SaveWorkScheduleTemplate_DoWork(object sender, DoWorkEventArgs e)
        {
            if (IsLoading)
                return;
            
            try{
                string file = (string)e.Argument; 
                EnableLoadingProgress("Gerando arquivo modelo");
                UnitForceMapBusiness.SaveWorkScheduleTemplate(file);
                MessageBox.Show("Arquivo gerado com sucesso");
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    String.Format("Erro durante a geração do arquivo (obs: é necessário que o Excel esteja instalado): {0}",
                                    exception.Message));

            }
            finally
            {
                DisableLoadingProgress();
            }

        }

        public void SaveWorkScheduleTemplate()
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "Arquivos do Excel|*.xls;*.xlsx;*.xlsm";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.Title = "Salve como";
            saveFileDialog1.DefaultExt = ".xlsm";


            try
            {
                if (saveFileDialog1.ShowDialog().Value)// == DialogResult.OK
                {

                    BackgroundWorker worker = new BackgroundWorker();
                    worker.DoWork += SaveWorkScheduleTemplate_DoWork;
                    worker.RunWorkerAsync(saveFileDialog1.FileName);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    String.Format("Erro durante a geração do arquivo (obs: é necessário que o Excel esteja instalado): {0}",
                                    exception.Message));
                

            }
            finally
            {
                DisableLoadingProgress();
            }

            

            
        }

        public void ImportWorkSchedule()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += ImportWorkSchedule_DoWork;
            worker.RunWorkerAsync();
        }

        public void LoadValidation()
        {
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += LoadValidation_DoWork;
            worker.RunWorkerCompleted += delegate(object s, RunWorkerCompletedEventArgs args)
            {
                if (args.Error != null)
                {
                    DisableLoadingProgress();
                }
                foreach (ReportValidationItemModel r in ReportValidation.ReportValidationItemList)
                {
                    RevortValidationList.Add(r);
                }

            };

            worker.RunWorkerAsync();
        }

        public void CompleteWithPeriodicWorkSchedule(List<WorkScheduleForUnitModel> workScheduleList, ref bool hasError)
        {
            hasError = false;
            try
            {
                List<DateTime> UramDayOffDates = UnitForceMapBusiness.GetDayOffList(StartDate.Value, EndDate.Value, "URAM");
                this.WorkScheduleImported.CompleteWithPeriodicWorkSchedule
                    (workScheduleList, StartDate.Value, EndDate.Value, UramDayOffDates, this.IsMergingWorkSchedule);
            }
            catch(Exception ex)
            {
                hasError = true;
                MessageBox.Show(String.Format("Ocorreu um erro ao gerar as escalas periódicas: {0}", ex.Message));
            }

        }

        public bool ValidatePeriodicWorkScheduleFilter()
        {
            if(!this.StartDate.HasValue || !this.EndDate.HasValue)
            {
                MessageBox.Show("Para importação de escalas periódicas, é obrigatório informar o período de datas.");
                return false;
            }

            if (this.StartDate.Value.CompareTo(this.EndDate.Value) > 0)
            {
                MessageBox.Show("A data inicial de importação não pode ser maior que a data final.");
                return false;
            }

            if (this.StartDate.Value.CompareTo(DateTime.Now) <= 0)
            {
                MessageBox.Show("A data inicial de importação deve iniciar no mínimo a partir de amanhã.");
                return false;
            }

            if (!this.IsCreatingWorkSchedule && !this.IsUpdatingWorkSchedule && !this.IsMergingWorkSchedule)
            {
                MessageBox.Show("Para importação de escalas periódicas, é obrigatório informar o tipo de importação.");
                return false;
            }

            return true;

        }

        public bool ValidateUpdatingWorkSchedule()
        {
            bool isValid = true;
            foreach (WorkScheduleForUnitModel ws in WorkScheduleImported.WorkScheduleForUnitList)
            {
                if (!UnitForceMapBusiness.ExistsUnitForceMap(ws.UnitId, ws.ShiftDate.Value, ws.WorkshiftLabel))
                {
                    MessageBox.Show(String.Format("Não existe escala em {0:dd/MM/yyy} {1} para a AM {2}",
                        ws.ShiftDate.Value, ws.WorkshiftLabel, ws.UnitId));
                    isValid = false;
                    break;
                }
            }

            return isValid;

        }

        public void Dispose()
        {

        }
        #endregion
    }
}
