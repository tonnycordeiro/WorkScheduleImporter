using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sisgraph.Ips.Samu.AddIn.ViewModels
{
    public class ShowMessageArgs : EventArgs
    {
        #region Atributos
        private string _message;
        #endregion

        #region Construtor
        public ShowMessageArgs() { }

        public ShowMessageArgs(string message)
        {
            this._message = message;
        }
        #endregion

        #region Propriedades
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        #endregion
    }
}
