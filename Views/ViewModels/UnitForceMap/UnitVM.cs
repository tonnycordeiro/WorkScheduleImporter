using Sisgraph.Ips.Samu.AddIn.Models.UnitForceMap;

namespace Sisgraph.Ips.Samu.AddIn.ViewModels.UnitForceMap
{
    public class UnitVM : ViewModelBase
    {
        #region Atributos
        private UnitForceMapModel _unitModel;
        private string _rowColor;
        #endregion

        #region Construtores
        public UnitVM() { }

        public UnitVM(UnitForceMapModel unitModel, string rowColor)
        {
            this.UnitModel = unitModel;
            this.RowColor = rowColor;
        }
        #endregion

        #region Propriedades
        public UnitForceMapModel UnitModel
        {
            get { return _unitModel; }
            set
            {
                _unitModel = value;
                OnPropertyChanged("UnitModel");
            }
        }

        public string RowColor
        {
            get { return _rowColor; }
            set
            {
                _rowColor = value;
                OnPropertyChanged("UnitModel");
            }
        }
        #endregion
    }
}
