using System;
using WorkScheduleImporter.AddIn.Models;

namespace WorkScheduleImporter.AddIn.ViewModels
{
    public class ViewModelBase : Observable
    {
        #region Events
        public event EventHandler<ShowMessageArgs> ShowMessageRequested;

        public delegate bool ShowConfirmMessageRequestedHandler(object sender, ShowMessageArgs e);
        public event ShowConfirmMessageRequestedHandler ShowConfirmMessageRequested;
        #endregion

        #region Methods
        protected void ShowMessage(string message)
        {
            var args = new ShowMessageArgs(message);
            ShowMessage(args);
        }

        protected void ShowMessage(ShowMessageArgs args)
        {
            if (ShowMessageRequested != null)
            {
                ShowMessageRequested(this, args);
            }
        }

        protected bool ShowConfirmMessage(string message)
        {
            var args = new ShowMessageArgs(message);
            return ShowConfirmMessage(args);
        }

        protected bool ShowConfirmMessage(ShowMessageArgs args)
        {
            if (ShowConfirmMessageRequested != null)
            {
                return ShowConfirmMessageRequested(this, args);
            }
            return false;
        }
        #endregion
    }
}
