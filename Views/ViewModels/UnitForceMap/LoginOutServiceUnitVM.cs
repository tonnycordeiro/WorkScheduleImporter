using System;
using System.Collections.Generic;
using Sisgraph.Ips.Samu.AddIn.Business.UnitForceMap;
using System.Windows;
using Sisgraph.Ips.Samu.AddIn.Models.UnitForceMap;
using Sisgraph.Ips.Samu.AddIn.Business;
using Sisgraph.Ips.Samu.AddIn.Models.CustomCad;
using Sisgraph.Ips.Samu.AddIn.Business.CustomCad;

namespace Sisgraph.Ips.Samu.AddIn.ViewModels.UnitForceMap
{
    public class LoginOutServiceUnitVM : ViewModelBase
    {
        #region Atributos
        private List<OutOfServiceTypeModel> _outServiceTypeList;
        private OutOfServiceTypeModel _selectedOutServiceType;
        private string _remarkText;
        private string _selectedTargetUnitId;
        private List<string> _reserveUnitList;
        #endregion

        #region Construtores
        public LoginOutServiceUnitVM()
        {
            LoadOutServiceTypeList();
        }
        #endregion

        #region Propriedades
        public List<OutOfServiceTypeModel> OutServiceTypeList
        {
            get { return _outServiceTypeList; }
            set
            {
                _outServiceTypeList = value;
                OnPropertyChanged("OutServiceTypeList");
            }
        }

        public OutOfServiceTypeModel SelectedOutServiceType
        {
            get { return _selectedOutServiceType; }
            set
            {
                _selectedOutServiceType = value;
                OnPropertyChanged("SelectedOutServiceType");
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

        public string SelectedTargetUnitId
        {
            get { return _selectedTargetUnitId; }
            set
            {
                _selectedTargetUnitId = value;
                OnPropertyChanged("SelectedTargetUnitId");
            }
        }

        public List<string> ReserveUnitList
        {
            get { return _reserveUnitList; }
            set
            {
                _reserveUnitList = value;
                OnPropertyChanged("ReserveUnitList");
            }
        }
        #endregion

        #region Métodos
        public void LoadReserveUnitList()
        {
            if (_selectedOutServiceType != null)
            {
                SpecialOutOfServiceType type = (SpecialOutOfServiceType)Enum.Parse(typeof(SpecialOutOfServiceType), _selectedOutServiceType.OutServiceTypeId);
                ReserveUnitList = UnitForceMapBusiness.GetSpecialOutOfServiceUnits(type);
            }
        }

        private void LoadOutServiceTypeList()
        {
            OutServiceTypeList = UnitForceMapBusiness.GetSpecialOutOfServiceList("SAMU", null);
        }

        public bool LoginOutServiceUnit()
        {
            try
            {
                if (string.IsNullOrEmpty(SelectedTargetUnitId))
                {
                    MessageBox.Show("Favor selecionar a AM que entrará em serviço.", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else
                {
                    //VERIFICAR SE A UNIDADE POSSUI CNES, NÃO É POSSÍVEL LOGAR UMA VIATURA QUE NÃO POSSUA CNES CADASTRADO.
                    string currentUnitCnes = UnitForceMapBusiness.GetUnitCnes(SelectedTargetUnitId);

                    if (string.IsNullOrEmpty(currentUnitCnes))
                    {
                        MessageBox.Show("Não foi possível logar a AM pois nao existe um CNES cadastrado para a mesma.", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Exclamation);

                        return false;
                    }

                    string substituteUnitId = UnitForceMapBusiness.GetSubstituteUnitId(SelectedTargetUnitId);
                    if (!string.IsNullOrEmpty(substituteUnitId))
                    {
                        //A UNIDADE SELECTIONADA POSSUI CNES, PORÉM ESTÁ SENDO SUBSTITUÍDA NO MOMENTO POR UMA OUTRA VIATURA,
                        //PRECISA VERIFICAR SE ESSA OUTRA VIATURA TAMBÉM POSSUI CNES ANTES DE PERGUNTAR SOBRE A RETIRADA DO VÍNCULO.
                        string substituteUnitCnes = UnitForceMapBusiness.GetUnitCnes(substituteUnitId);

                        if (string.IsNullOrEmpty(substituteUnitCnes))
                        {
                            MessageBox.Show(string.Format("A unidade {0} está atuando como reserva da selecionada, {1}." +
                                                                            Environment.NewLine + "Não é possível colocar a viatura {1} em operação, pois sua reserva {0} não possui um CNES cadastrado." + 
                                                                            Environment.NewLine + "Substitua a viatura reserva pela oficial caso deseje o retorno da viatura {1} de volta em operação.",
                                                                            substituteUnitId, SelectedTargetUnitId), "Viatura possui unidade reserva em operação", MessageBoxButton.OK);

                            return false;
                        }


                        if (UnitBusiness.IsAssigned(substituteUnitId))
                        {
                            MessageBox.Show(string.Format("A unidade {0}, que está substituindo a selecionada, está EMPENHADA." +
                                                                            Environment.NewLine + "Não é possível colocá-la em serviço.",
                                                                            substituteUnitId),"Viatura possui reserva empenhada", MessageBoxButton.OK);

                            return false;

                        }

                        

                        MessageBoxResult msgBoxResult = MessageBox.Show(string.Format("A unidade {0} está atuando como reserva da selecionada, {1}." +
                                                                            Environment.NewLine + "Deseja remover o vínculo entre elas e manter as duas em operação?",
                                                                            substituteUnitId, SelectedTargetUnitId), "Viatura possui unidade reserva em operação",
                                                                        MessageBoxButton.OKCancel);
                        if (msgBoxResult == MessageBoxResult.OK)
                        {//string _outServiceTypeId
                            UnitForceMapModel unitForceTarget = UnitForceMapBusiness.GetCurrentUnitForceMap(SelectedTargetUnitId);
                            UnitForceMapModel unitForceCurrent = UnitForceMapBusiness.GetCurrentUnitForceMap(substituteUnitId);
                            OutOfServiceTypeModel outOfServiceType = UnitForceMapBusiness.GetOutOfServiceType(OutOfServiceTypeModel.OUT_TYPE_RESERVA_TECNICA_ID, "SAMU");
                            UnitForceMapBusiness.ExchangeUnit(unitForceCurrent, unitForceTarget, outOfServiceType);


                            //UnitForceMapBusiness.RemoveSubstituteUnit(SelectedTargetUnitId);
                            //LoginSelectedUnit();
                            return true;
                        }
                    }
                    else
                    {
                        LoginSelectedUnit();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                ShowErrorMessage();
            }

            return false;
        }

        private void LoginSelectedUnit()
        {
            if (!CadBusiness.UnitInService(SelectedTargetUnitId, RemarkText))
            {
                throw new Exception();
            }

            UnitForceMapModel unit = UnitForceMapBusiness.GetCurrentUnitForceMap(SelectedTargetUnitId);

            if (UnitForceMapBusiness.UpdateUnitForceMap(unit) == null)
            {
                throw new Exception();
            }
        }

        private void ShowErrorMessage()
        {
            MessageBox.Show("Não foi possível logar a AM.", "Atenção!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        #endregion
    }
}
