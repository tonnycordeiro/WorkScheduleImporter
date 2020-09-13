using System;
using System.Windows;
using System.Windows.Controls;
using WorkScheduleImporter.AddIn.ViewModels.WorkSchedule;
using WorkScheduleImporter.AddIn.Models.WorkSchedule;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Government.EmergencyDepartment.AddIn.Models.WorkSchedule;

namespace WorkScheduleImporter.AddIn.Views.UnitForceMap
{
    public partial class ImportWorkScheduleWindow : Window
    {
        #region Constructors
        public ImportWorkScheduleWindow(List<WorkShiftModel> availableWorkshiftList)
        {
            InitializeComponent();
            ViewModel = new ImportWorkScheduleVM(availableWorkshiftList);
            GroupPeriodicWorkSchedule.Visibility = Visibility.Collapsed;

        }
        #endregion

        #region Properties
        public ImportWorkScheduleVM ViewModel
        {
            get { return this.DataContext as ImportWorkScheduleVM; }
            set { this.DataContext = value; }
        }
        #endregion

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.OpenFile();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.ImportWorkSchedule();
        }

        #region Events
        #endregion

        private void btnValidate_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.LoadValidation();
        }

        private void btnDownloadModel_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SaveWorkScheduleTemplate();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            GroupPeriodicWorkSchedule.Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            GroupPeriodicWorkSchedule.Visibility = Visibility.Collapsed;
            this.ViewModel.CleanWorkSchedulePeriodicFields();
        }

        private void dataGridReportValidation_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = sender as DataGrid;

            if (dataGrid.SelectedItems.Count > 0)
            {
                this.ViewModel.OrderWorkScheduleBySelectedError(dataGrid.SelectedItems.Cast < ReportValidationItemModel>().ToList());
                dataGridPeriodicWorkSchedule.ScrollIntoView(dataGridPeriodicWorkSchedule.Items[0]);
                dataGridPeriodicWorkSchedule.UpdateLayout();
            }
        }

    }
}
