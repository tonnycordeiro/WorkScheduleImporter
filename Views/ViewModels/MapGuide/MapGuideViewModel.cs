using Intergraph.IPS.Cad;
using Intergraph.IPS.CADCore;
using log4net;
using Sisgraph.Ips.Samu.AddIn.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Sisgraph.Ips.Samu.AddIn.ViewModels.MapGuide
{
    public class MapGuideViewModel : ViewModelBase
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Atributos
        private string _streetName = null;
        private List<string> _mapGuideResultList = null;
        #endregion

        #region Propriedades
        public string StreetName
        {
            get { return _streetName; }
            set
            {
                _streetName = value;
                OnPropertyChanged("StreetName");
            }
        }


        public List<string> MapGuideResultList
        {
            get { return _mapGuideResultList; }
            set
            {
                _mapGuideResultList = value;
                OnPropertyChanged("MapGuideResultList");
            }
        }
        #endregion

        #region Construtores
        public MapGuideViewModel()
        {
            Location setLocation = CadServices.ActiveEventService.Location;

            if (!string.IsNullOrWhiteSpace(setLocation.Street.StreetName))
            {
                StreetName = setLocation.Street.StreetName;
            }
        }
        #endregion

        #region Eventos
        public void Clear()
        {
            this.StreetName = string.Empty;
            this.MapGuideResultList = new List<string>();
        }

        public void Search()
        {
            if (UtilsBusiness.VerifySqlInjection(this.StreetName))
            {
                MessageBox.Show("Caracter Inválido", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            if (string.IsNullOrWhiteSpace(this.StreetName))
            {
                MessageBox.Show("Favor digitar a rua", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Warning);

                return;
            }

            this.MapGuideResultList = MapGuideBusiness.SearchForStreet(this.StreetName);
        }
        #endregion
    }
}
