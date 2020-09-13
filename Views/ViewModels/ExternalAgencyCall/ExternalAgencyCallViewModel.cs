using log4net;
using Sisgraph.Ips.Samu.AddIn.Business.ExternalAgencyCall;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sisgraph.Ips.Samu.AddIn.ViewModels.ExternalAgencyCall
{
    public class ExternalAgencyCallViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Atributos
        private string _agencyEventId = null;
        private List<string> _externalAgencyList = null;
        private string _selectedExternalAgency = null;
        private string _externalAgencyContact = null;
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

        public List<string> ExternalAgencyList
        {
            get { return _externalAgencyList; }
            set 
            { 
                _externalAgencyList = value;
                OnPropertyChanged("ExternalAgencyList");
            }
        }

        public string SelectedExternalAgency
        {
            get { return _selectedExternalAgency; }
            set 
            { 
                _selectedExternalAgency = value;
                OnPropertyChanged("SelectedExternalAgency");
            }
        }

        public string ExternalAgencyContact
        {
            get { return _externalAgencyContact; }
            set { 
                _externalAgencyContact = value;
                OnPropertyChanged("ExternalAgencyContact");
            }
        }
        #endregion

        #region Construtores
        public ExternalAgencyCallViewModel(string AgencyEventId)
        {
            this.AgencyEventId = AgencyEventId;

            this.ExternalAgencyList = ExternalAgencyCallBusiness.GetExternalAgencyList();
        }
        #endregion

        public bool Confirm()
        {
            if (string.IsNullOrWhiteSpace(SelectedExternalAgency))
            {
                MessageBox.Show("Favor selecionar uma agência.", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Warning);

                return false;
            }

            if (!ExternalAgencyCallBusiness.InsertNewExternalAgencyCall(AgencyEventId, SelectedExternalAgency, ExternalAgencyContact))
                return false;

            return true;
        }
    }
}
