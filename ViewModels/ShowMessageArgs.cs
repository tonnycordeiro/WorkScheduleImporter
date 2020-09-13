using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorkScheduleImporter.AddIn.ViewModels
{
    public class ShowMessageArgs : EventArgs
    {
        #region Attributes
        private string _message;
        #endregion

        #region Constructors
        public ShowMessageArgs() { }

        public ShowMessageArgs(string message)
        {
            this._message = message;
        }
        #endregion

        #region Properties
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        #endregion
    }
}
