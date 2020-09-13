using log4net;
using Sisgraph.Ips.Samu.AddIn.Business.CustomCancelEvent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sisgraph.Ips.Samu.AddIn.ViewModels.CustomCancelEvent
{
    public class CustomCancelEventViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Atributos
        private string _agencyEventId = null;
        private List<KeyValuePair<string, string>> _cancelReasonList = null;
        private KeyValuePair<string, string>? _selectedCancelReason = null;
        private string _remarkText = null;
        #endregion

        #region Propriedades
        public string AgencyEventId
        {
            get { return _agencyEventId; }
            set
            {
                _agencyEventId = value;
                OnPropertyChanged("AgencyEventId");
            }
        }

        public List<KeyValuePair<string, string>> CancelReasonList
        {
            get { return _cancelReasonList; }
            set
            {
                _cancelReasonList = value;
                OnPropertyChanged("CancelReasonList");
            }
        }

        public KeyValuePair<string, string>? SelectedCancelReason
        {
            get { return _selectedCancelReason; }
            set
            {
                _selectedCancelReason = value;
                OnPropertyChanged("SelectedCancelReason");
            }
        }

        public string RemarkText
        {
            get { return _remarkText; }
            set
            {
                _remarkText = value; 
                OnPropertyChanged("RemarkText");
            }
        }
        #endregion

        #region Construtores
        public CustomCancelEventViewModel(string AgencyEventId)
        {
            this.AgencyEventId = AgencyEventId;

            this.CancelReasonList = CustomCancelEventBusiness.GetCancelReasonList();
        }
        #endregion

        public bool Confirm()
        {
            if (SelectedCancelReason == null)
            {
                MessageBox.Show("Favor selecionar um motivo de cancelamento.", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Warning);

                return false;
            }

            if (string.IsNullOrWhiteSpace(RemarkText))
            {
                MessageBox.Show("Favor informar a observação de cancelamento.", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Warning);

                return false;
            }

            if (!CustomCancelEventBusiness.InsertNewCancelEvent(AgencyEventId, SelectedCancelReason.Value, RemarkText))
                return false;

            return true;
        }
    }
}
