using System;
using System.Collections.Generic;
using System.Linq;
using WorkScheduleImporter.AddIn.Models;
using WorkScheduleImporter.AddIn.Models.WorkSchedule;
using WorkScheduleImporter.AddIn.Business.WorkSchedule;
using WorkScheduleImporter.AddIn.Business;
using System.Collections.ObjectModel;
using System.Windows;
using System.IO;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using WorkScheduleImporter.AddIn.Models.WorkSchedule.WorkShiftPeriod;
using Government.EmergencyDepartment.AddIn.Models.WorkSchedule;
using Government.EmergencyDepartment.AddIn.Business.WorkSchedule;

namespace WorkScheduleImporter.AddIn.ViewModels.WorkSchedule
{
    public class ImportWorkScheduleVM : ViewModelBase, IDisposable
    {
        #region Attributes
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
        #endregion

        #region Constructors
        public ImportWorkScheduleVM(List<WorkShiftModel> availableWorkshiftList)
        {
            _reportValidation = new ReportValidationModel();
            _workScheduleImported = new WorkScheduleImportTemplateModel();
            _workShiftList = availableWorkshiftList;
            _enableImporting = false;
            _loadingText = String.Empty;
            _isLoading = false;
            _workShiftPeriodBuilderList = new List<WorkShiftPeriodBuilder>();
            
            ReportValidationBusiness.LoadValidationsByType();

            IsCreatingWorkSchedule = true;
            IsUpdatingWorkSchedule = false;
            IsMergingWorkSchedule = false;
        }
        #endregion

        #region Properties

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

        #endregion

        #region Methods

        public void LoadValidation_DoWork(object sender, DoWorkEventArgs e)
        {
            if (IsLoading)
                return;

            try
            {
                EnableLoadingProgress(Properties.Resources.DIALOG_MESSAGE_VALIDATING_WORK_SCHEDULE);
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
                    
                List<ImportableWorkScheduleUnitModel> workScheduleList = ImportableWorkScheduleBusiness.ExtractWorkSchedule(this.FullPathFile).ToList<ImportableWorkScheduleUnitModel>();

                if (this.IsPeriodicWorkSchedule)
                {
                    bool hasError = false;

                    if (workScheduleList.Where(ws => String.IsNullOrEmpty(ws.DateFrequence)).Count() > 0)
                    {
                        DisableLoadingProgress();
                        MessageBox.Show(Properties.Resources.ALERT_MESSAGE_MISSED_DATE_FREQUENCE);
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
                ReportValidationBusiness.UpdateReportValidation(WorkScheduleImported, _workShiftList, Properties.Settings.Default.AGENCY_ID, ReportValidation);

                

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
                WorkScheduleImported.WorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>();
                ReportValidation = new ReportValidationModel();
                EnableImporting = false;

                ReportValidation.AddReportItem(WorkScheduleValidationType.SHEET_NAME_NOT_FOUND, ImportableWorkScheduleBusiness.GetWorkScheduleSheetName().ToUpper());
            }
            catch (Exception exception)
            {
                WorkScheduleImported.WorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>();
                ReportValidation = new ReportValidationModel();
                EnableImporting = false;

                MessageBox.Show(String.Format("{0}: {1}", Properties.Resources.ALERT_MESSAGE_ERROR_FILE_VALIDATING, exception.Message));
                return;
            }
            finally
            {
                DisableLoadingProgress();
            }

        }

        public void ImportWorkSchedule_DoWork(object sender, DoWorkEventArgs e)
        {
            List<WorkScheduleUnitModel> workScheduleUnitList = new List<WorkScheduleUnitModel>();

            if (IsLoading)
                return;

            try
            {
                EnableLoadingProgress(Properties.Resources.DIALOG_MESSAGE_IMPORTING_WORK_SCHEDULE);

                workScheduleUnitList.AddRange(WorkScheduleImported.WorkScheduleForUnitList.Select(ws => ws.ToWorkScheduleUnitModel(_workShiftList)));

                if (workScheduleUnitList.Where(uf => uf == null).Count() > 0)
                    throw new Exception(Properties.Resources.ALERT_MESSAGE_ERROR_WORK_SCHEDULE_UNIT_CONVERSION);

                foreach (WorkScheduleUnitModel uf in workScheduleUnitList)
                {
                    WorkScheduleBusiness.UpdateFutureUnitForceMap(uf, isCrewUpdating: IsUpdatingWorkSchedule);
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(String.Format("{0}: {1}", Properties.Resources.ALERT_MESSAGE_ERROR_IMPORTING, exception.Message));
                return;
            }
            finally
            {
                DisableLoadingProgress();
            }

            MessageBox.Show(Properties.Resources.ALERT_MESSAGE_SUCCESS_IMPORTING);

            CleanFields();

            //ShowMessage("Importação finalizada com sucesso.");

        }

        private void EnableLoadingProgress(string message)
        {
            IsLoading = true;
            LoadingText = String.Format("{0}. {1}",message, Properties.Resources.DIALOG_MESSAGE_WAITING);
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
            WorkScheduleImported.WorkScheduleForUnitList = new List<ImportableWorkScheduleUnitModel>();
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
            return String.Format("{0}{1}", Properties.Resources.OUT_FILE_PREFIX_NAME, DateTime.Now.ToString(Properties.Settings.Default.OUT_FILE_SUFFIX_DATE_FORMAT));
        }

        public void OpenFile()
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = String.Empty;
            dlg.DefaultExt = Properties.Settings.Default.OPEN_FILE_DIALOG_DEFAUL_EXTENSION;
            dlg.Filter = Properties.Settings.Default.OPEN_FILE_DIALOG_FILTER;
            dlg.Title = Properties.Settings.Default.OPEN_FILE_DIALOG_TITLE;
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                this.FullPathFile = dlg.FileName;
            }

        }

        public void SaveWorkScheduleTemplate_DoWork(object sender, DoWorkEventArgs e)
        {
            if (IsLoading)
                return;
            
            try{
                string file = (string)e.Argument; 
                EnableLoadingProgress(Properties.Resources.DIALOG_MESSAGE_GENERATING_FILE);
                ImportableWorkScheduleBusiness.SaveWorkScheduleTemplate(file);
                MessageBox.Show(Properties.Resources.ALERT_MESSAGE_SUCCESS_GENERATING_FILE);
            }
            catch (Exception exception)
            {
                MessageBox.Show(
                    String.Format("{0}: {1}", Properties.Resources.ALERT_MESSAGE_ERROR_GENERATING_FILE, exception.Message));

            }
            finally
            {
                DisableLoadingProgress();
            }

        }

        public void SaveWorkScheduleTemplate()
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = Properties.Settings.Default.SAVE_FILE_DIALOG_FILTER;
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.Title = Properties.Settings.Default.SAVE_FILE_DIALOG_TITLE;
            saveFileDialog1.DefaultExt = Properties.Settings.Default.SAVE_FILE_DIALOG_DEFAUL_EXTENSION;

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
                    String.Format("{0}: {1}", Properties.Resources.ALERT_MESSAGE_ERROR_GENERATING_FILE ,exception.Message));
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

        public void CompleteWithPeriodicWorkSchedule(List<ImportableWorkScheduleUnitModel> workScheduleList, ref bool hasError)
        {
            hasError = false;
            try
            {
                List<DateTime> UramDayOffDates = WorkScheduleBusiness.GetDayOffList(StartDate.Value, EndDate.Value, null);
                this.WorkScheduleImported.CompleteWithPeriodicWorkSchedule
                    (workScheduleList, StartDate.Value, EndDate.Value, UramDayOffDates, this.IsMergingWorkSchedule);
            }
            catch(Exception ex)
            {
                hasError = true;
                MessageBox.Show(String.Format("{0}: {1}", 
                    Properties.Resources.ALERT_MESSAGE_ERROR_GENERATING_PERIODIC_WORK_SCHEDULE ,ex.Message));
            }

        }

        public bool ValidatePeriodicWorkScheduleFilter()
        {
            if(!this.StartDate.HasValue || !this.EndDate.HasValue)
            {
                MessageBox.Show(Properties.Resources.ALERT_MESSAGE_VALIDATING_PERIODIC_WORK_SCHEDULE_REQUIRED_DATE);
                return false;
            }

            if (this.StartDate.Value.CompareTo(this.EndDate.Value) > 0)
            {
                MessageBox.Show(Properties.Resources.ALERT_MESSAGE_VALIDATING_PERIODIC_WORK_SCHEDULE_INVERTED_DATES);
                return false;
            }

            if (this.StartDate.Value.CompareTo(DateTime.Now) <= 0)
            {
                MessageBox.Show(Properties.Resources.ALERT_MESSAGE_VALIDATING_PERIODIC_WORK_SCHEDULE_PAST_DATE);
                return false;
            }

            if (!this.IsCreatingWorkSchedule && !this.IsUpdatingWorkSchedule && !this.IsMergingWorkSchedule)
            {
                MessageBox.Show(Properties.Resources.ALERT_MESSAGE_VALIDATING_PERIODIC_WORK_SCHEDULE_MISSED_IMPORTING_TYPE);
                return false;
            }

            return true;

        }

        public bool ValidateUpdatingWorkSchedule()
        {
            bool isValid = true;
            foreach (ImportableWorkScheduleUnitModel ws in WorkScheduleImported.WorkScheduleForUnitList)
            {
                if (!WorkScheduleBusiness.ExistsWorkSchedule(ws.UnitId, ws.ShiftDate.Value, ws.WorkshiftLabel))
                {
                    MessageBox.Show(String.Format("{0} {1} {2} {3} {4}",
                        Properties.Resources.ALERT_MESSAGE_ERROR_WORK_SCHEDULE_NOT_EXISTS_TO_DATE,
                        String.Format(Properties.Settings.Default.DATE_FORMAT, ws.ShiftDate.Value),
                        Properties.Resources.ALERT_MESSAGE_ERROR_WORK_SCHEDULE_NOT_EXISTS_TO_UNIT,
                        ws.WorkshiftLabel, ws.UnitId));
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
