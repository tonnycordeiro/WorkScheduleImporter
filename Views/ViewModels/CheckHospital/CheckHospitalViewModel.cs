using Intergraph.IPS.CADCore;
using log4net;
using Sisgraph.Ips.Samu.AddIn.Business;
using Sisgraph.Ips.Samu.AddIn.Business.CustomCad;
using Sisgraph.Ips.Samu.AddIn.Models.CustomCad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sisgraph.Ips.Samu.AddIn.ViewModels.CheckHospital
{
    public class CheckHospitalViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Atributos
        private List<CustomHospital> _hospitalList = null;
        private List<string> _hospitalTypesList = null;
        private string _selectedHospitalType;
        private CustomHospital _selectedHospital;
        private string _agencyEventId;
        private bool _showRoutedDistance;
        #endregion

        #region Propriedades
        public List<CustomHospital> HospitalList
        {
            get { return _hospitalList; }
            set { 
                _hospitalList = value;
                OnPropertyChanged("HospitalList");
            }
        }

        public List<string> HospitalTypesList
        {
            get { return _hospitalTypesList; }
            set
            {
                _hospitalTypesList = value;
                OnPropertyChanged("HospitalTypesList");
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

        public CustomHospital SelectedHospital
        {
            get { return _selectedHospital; }
            set
            {
                _selectedHospital = value;
            }
        }

        #endregion

        #region Construtores
        public CheckHospitalViewModel(string agencyEventId)
        {
            string outMessage = string.Empty;

            this._agencyEventId = agencyEventId;

            CADSystem.CadContext.Parameter.ClearCache(true);

            this._showRoutedDistance = (CADSystem.CadContext.Parameter.GetParameterListItem("sisgraph", "CheckHosp", "CalculateRoutedDistance") == "T" ? true : false);
            string HospitalTypesToShow = CADSystem.CadContext.Parameter.GetParameterListItem("sisgraph", "CheckHosp", "HospitalTypesToShow");

            this.HospitalTypesList = HospitalTypesToShow.Split(new char[] { ';' }).ToList<string>();
            HospitalTypesList.Insert(0, String.Empty);

            this.HospitalList = CustomHospitalBusiness.GetNearbyAgencyEventHospitalList(this._agencyEventId, this._showRoutedDistance, HospitalTypesToShow, out outMessage);

            if (!string.IsNullOrWhiteSpace(outMessage))
                MessageBox.Show(outMessage, "Atenção!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        #endregion


        #region Métodos
        public void UpdateHospitalList()
        {
            string outMessage = string.Empty;
            this.HospitalList = CustomHospitalBusiness.GetNearbyAgencyEventHospitalList(this._agencyEventId, this._showRoutedDistance, SelectedHospitalType, out outMessage);
            if (!string.IsNullOrWhiteSpace(outMessage))
                MessageBox.Show(outMessage, "Atenção!", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public void UpdateEventWithSelectedHospital()
        {
            try
            {
                string currentUserName = CadBusiness.GetCurrentFullUserName();
                string remark = String.Format("Deslocamento autorizado por Dr(a) {0}, em {1}, para hospital {2}",
                                                currentUserName, DateTime.Now.ToString("dd/MM/yyyy HH:mm"), SelectedHospital.Name);
                CadBusiness.AddEventRemark(this._agencyEventId, remark);
                return;
            }
            catch (Exception exception)
            {
                MessageBox.Show("Ocorreu um erro ao inserir o comentário na ocorrência", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        
        #endregion

    }
}
