using Pharmacy.Controls;
using Pharmacy.Utillities;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;

namespace Pharmacy.Pharmacies
{
    /// <summary>
    /// Interaction logic for PharmaciesDialog.xaml
    /// </summary>
    public partial class PharmaciesDialog : BaseDialog
    {
        public Pharmacies pharmacy { get; set; }
        private PharmacyOrders.PharmacyOrdersView pharmacyOrdersView;
        private Dealers.DealersData dealersData = new Dealers.DealersData();
        private Dealers.DealerInfo dealerInfo = new Dealers.DealerInfo();
        private DrugsInfo.DrugsInfoData drugsInfoData = new DrugsInfo.DrugsInfoData();
        private PharmacyOrders.PharmacyOrdersData pharmacyOrdersData = new PharmacyOrders.PharmacyOrdersData();

        private int numberDealers = 0;

        public PharmaciesDialog(Pharmacies _pharmacy, DialogModes _dialogMode, DependencyObject parent = null) : base(_dialogMode, parent)
        {
            pharmacy = _pharmacy;
            InitializeComponent();
        }

        protected override void GetOKButton(ref Button okButton)
        {
            okButton = ui_btnOK;
        }

        protected override void GetCancelButton(ref Button cancelButton)
        {
            cancelButton = ui_btnCancel;
        }

        protected override void LoadData()
        {
            base.LoadData();

            pharmacyOrdersView = new PharmacyOrders.PharmacyOrdersView(pharmacy);
        }

        protected override void OnInitControls()
        {
            base.OnInitControls();

            switch (DialogMode)
            {
                case DialogModes.Add:
                    {
                        Title = "Добавяне на аптека...";

                        ui_btnOK.Content = "Добави";
                        ui_btnCancel.Content = "Отказ";
                    }
                    break;
                case DialogModes.Edit:
                    {
                        Title = "Редактиране на аптека...";

                        ui_btnOK.Content = "Редактирай";
                        ui_btnCancel.Content = "Отказ";
                    }
                    break;
                case DialogModes.Preview:
                    {
                        Title = "Преглед на аптека...";

                        ui_edbName.IsReadOnly = true;
                        ui_btnOK.Visibility = Visibility.Hidden;
                        ui_btnCancel.Content = "Затвори";
                    }
                    break;
            }

            ui_gridPharmacyOrders.Children.Add(pharmacyOrdersView);
        }

        protected override void CopyDataToControls()
        {
            ui_edbName.Text = pharmacy.Name.ToString();
        }

        protected override void CopyControlsToData()
        {
            pharmacy.Name = ui_edbName.Text;
        }

        protected override bool ValidateData()
        {
            string Message = "";
            bool focus = false;

            if (ui_edbName.Text.Equals(""))
            {
                Message += "\n Моля попълнете задължителното поле: Име";

                if (!focus)
                    focus = ui_edbName.Focus();
            }

            if (!Message.Equals(""))
            {
                MessageBoxes.ShowWarning(Message);
                return false;
            }

            return true;
        }

        private void findDealer_Click(object sender, RoutedEventArgs e)
        {
            List<Dealers.DealerInfo> dealersInfoList = new List<Dealers.DealerInfo>();
            PharmacyOrders.PharmacyOrders pharmacyOrder = (PharmacyOrders.PharmacyOrders)pharmacyOrdersView.GetSelectedItem();
            if (pharmacyOrder == null)
            {
                MessageBoxes.ShowError("Моля изберете поръчка!");
                return;
            }

            List<Dealers.Dealers> dealersList = new List<Dealers.Dealers>();

            if (!dealersData.SelectAll(dealersList))
            {
                MessageBoxes.ShowError(MessageBoxes.LoadDataErrorMessage);
                return;
            }

            foreach (Dealers.Dealers dealer in dealersList)
            {
                Dealers.DealerInfo dealerInfo = new Dealers.DealerInfo();
                dealerInfo.dealer = dealer;

                List<DrugsInfo.DrugsInfo> drugsInfoList = new List<DrugsInfo.DrugsInfo>();

                if (!dealersData.SelectAllDrugsInfo(drugsInfoList, dealer.ID))
                {
                    MessageBoxes.ShowError(MessageBoxes.LoadDataErrorMessage);
                    return;
                }

                dealerInfo.drugsInfoList = drugsInfoList;

                List<Drugs.Drugs> drugsList = new List<Drugs.Drugs>();
                Drugs.Drugs drug = new Drugs.Drugs();
                foreach (DrugsInfo.DrugsInfo drugInfo in dealerInfo.drugsInfoList)
                {
                    if (!dealersData.SelectDrugWhereID(out drug, drugInfo.DrugID))
                    {
                        MessageBoxes.ShowError(MessageBoxes.LoadDataErrorMessage);
                        return;
                    }
                    drugsList.Add(drug);
                }

                dealerInfo.drugsList = drugsList;

                dealersInfoList.Add(dealerInfo);
            }

            findDealerMethod(pharmacyOrder, dealersInfoList);
        }

        private void findDealerMethod(PharmacyOrders.PharmacyOrders pharmacyOrder, List<Dealers.DealerInfo> dealersInfoList)
        {
            numberDealers = 0;
            List<Dealers.DealerInfo> dealersCandidates = new List<Dealers.DealerInfo>();
            foreach (Dealers.DealerInfo dealerInfo in dealersInfoList)
            {
                if (dealerInfo.ifAvailable(pharmacyOrder.DrugName, pharmacyOrder.NumberOrders))
                {
                    numberDealers++;
                    dealersCandidates.Add(dealerInfo);
                }
            }

            if (numberDealers == 0)
            {
                MessageBoxes.ShowInfo("Не е намерен нито един доставчик.");
                return;
            }
            else if (numberDealers == 1)
            {
                MessageBoxResult result = MessageBox.Show("Намерен е само един доставчик - " + dealersCandidates[0].dealer.Name + ". Желаете ли да преминете към поръчката?", "Поръчка", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    oneDealerOrder(dealersCandidates[0], pharmacyOrder);
                }
            }
            else if (numberDealers > 1)
            {
                MessageBoxResult result = MessageBox.Show("Намерен са няколко доставчика. Желаете ли да преминете към поръчката?", "Поръчка", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    multipleDealersOrder(dealersCandidates, pharmacyOrder);
                }
            }
        }

        private void oneDealerOrder(Dealers.DealerInfo dealersCandidates, PharmacyOrders.PharmacyOrders pharmacyOrder)
        {
            for (int i = 0; i < dealersCandidates.drugsList.Count; i++)
            {
                if (dealersCandidates.drugsList[i].Name == pharmacyOrder.DrugName)
                {
                    for (int j = 0; j < dealersCandidates.drugsInfoList.Count; j++)
                    {
                        if (dealersCandidates.drugsInfoList[j].DrugID == dealersCandidates.drugsList[i].ID)
                        {
                            double price = dealersCandidates.drugsInfoList[j].Price * pharmacyOrder.NumberOrders;
                            MessageBoxResult result = MessageBox.Show("Цена на поръчка - " + price + " лв. Желаете ли да изпълните поръчката?", "Поръчка", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                            if (result == MessageBoxResult.OK)
                            {
                                dealersCandidates.drugsInfoList[j].Number -= pharmacyOrder.NumberOrders;
                                MessageBoxes.ShowInfo("Поръчката е изпълнена успешно!");
                                if (dealersCandidates.drugsInfoList[j].Number == 0)
                                    drugsInfoData.DeleteWhereID(dealersCandidates.drugsInfoList[j].ID);
                                else
                                    dealersData.UpdateWhereID(dealersCandidates.dealer.ID, dealersCandidates);

                                if (!pharmacyOrdersData.DeleteWhereID(pharmacyOrder.ID))
                                {
                                    MessageBoxes.ShowError(MessageBoxes.DeleteErrorMessage);
                                    return;
                                }

                                pharmacyOrdersView.itemsSource.RemoveAt(pharmacyOrdersView.SelectedIndex);
                            }
                            return;
                        }
                    }
                }
            }
        }

        private void multipleDealersOrder(List<Dealers.DealerInfo> dealersCandidates, PharmacyOrders.PharmacyOrders pharmacyOrder)
        {
            double price = double.MaxValue;
            Dealers.DealerInfo dealersInfoMin = null;
            int drugInfoIndex = -1;

            foreach (Dealers.DealerInfo dealerInfo in dealersCandidates)
            {
                for (int i = 0; i < dealerInfo.drugsList.Count; i++)
                {
                    if (dealerInfo.drugsList[i].Name == pharmacyOrder.DrugName)
                    {
                        for (int j = 0; j < dealerInfo.drugsInfoList.Count; j++)
                        {
                            if (dealerInfo.drugsInfoList[j].DrugID == dealerInfo.drugsList[i].ID)
                            {
                                if (dealerInfo.drugsInfoList[j].Price <= price)
                                {
                                    price = dealerInfo.drugsInfoList[j].Price;
                                    dealersInfoMin = new Dealers.DealerInfo();
                                    dealersInfoMin = dealerInfo;
                                    drugInfoIndex = j;
                                }
                            }
                        }
                    }
                }
            }

            if (dealersInfoMin != null)
            {
                price = dealersInfoMin.drugsInfoList[drugInfoIndex].Price * pharmacyOrder.NumberOrders;
                MessageBoxResult result = MessageBox.Show("Минимална цена на поръчка - " + price + " лв. Желаете ли да изпълните поръчката?", "Поръчка", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                if (result == MessageBoxResult.OK)
                {
                    dealersInfoMin.drugsInfoList[drugInfoIndex].Number -= pharmacyOrder.NumberOrders;
                    MessageBoxes.ShowInfo("Поръчката е изпълнена успешно!");
                    if (dealersInfoMin.drugsInfoList[drugInfoIndex].Number == 0)
                        drugsInfoData.DeleteWhereID(dealersInfoMin.drugsInfoList[drugInfoIndex].ID);
                    else
                        dealersData.UpdateWhereID(dealersInfoMin.dealer.ID, dealersInfoMin);

                    if (!pharmacyOrdersData.DeleteWhereID(pharmacyOrder.ID))
                    {
                        MessageBoxes.ShowError(MessageBoxes.DeleteErrorMessage);
                        return;
                    }

                    pharmacyOrdersView.itemsSource.RemoveAt(pharmacyOrdersView.SelectedIndex);
                }
            }
        }
    }
}
