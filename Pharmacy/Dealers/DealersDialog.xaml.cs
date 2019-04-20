using Pharmacy.Controls;
using Pharmacy.DrugsInfo;
using Pharmacy.Utillities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Pharmacy.Dealers
{
    /// <summary>
    /// Interaction logic for DealersDialog.xaml
    /// </summary>
    public partial class DealersDialog : BaseDialog
    {
        public DealerInfo dealerInfo { get; set; }
        private DrugsInfoView drugsInfoView;
        private DrugsInfoData drugsInfoData = new DrugsInfoData();

        public DealersDialog(DealerInfo _dealerInfo, DialogModes _dialogMode, DependencyObject parent = null) : base(_dialogMode, parent)
        {
            dealerInfo = _dealerInfo;
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

            drugsInfoView = new DrugsInfoView(dealerInfo.dealer.ID, dealerInfo.drugsList, dealerInfo.drugsInfoList);
        }

        protected override void OnInitControls()
        {
            base.OnInitControls();

            switch (DialogMode)
            {
                case DialogModes.Add:
                    {
                        Title = "Добавяне на доставчик...";

                        ui_btnOK.Content = "Добави";
                        ui_btnCancel.Content = "Отказ";
                    }
                    break;
                case DialogModes.Edit:
                    {
                        Title = "Редактиране на доставчик...";

                        ui_btnOK.Content = "Редактирай";
                        ui_btnCancel.Content = "Отказ";
                    }
                    break;
                case DialogModes.Preview:
                    {
                        Title = "Преглед на доставчик...";

                        ui_edbName.IsReadOnly = true;
                        ui_edbDistrubutor.IsReadOnly = true;
                        ui_edbPhoneNumber.IsReadOnly = true;
                        ui_btnOK.Visibility = Visibility.Hidden;
                        ui_btnCancel.Content = "Затвори";
                    }
                    break;
            }

            ui_gridDrugs.Children.Add(drugsInfoView);
        }

        protected override void CopyDataToControls()
        {
            ui_edbName.Text = dealerInfo.dealer.Name.ToString();
            ui_edbDistrubutor.Text = dealerInfo.dealer.Distributor;
            ui_edbPhoneNumber.Text = dealerInfo.dealer.PhoneNumber;
        }

        protected override void CopyControlsToData()
        {
            dealerInfo.dealer.Name = ui_edbName.Text;
            dealerInfo.dealer.Distributor = ui_edbDistrubutor.Text;
            dealerInfo.dealer.PhoneNumber = ui_edbPhoneNumber.Text;
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

            if (ui_edbDistrubutor.Text.Equals(""))
            {
                Message += "\n Моля попълнете задължителното поле: Дистрибутор";

                if (!focus)
                    focus = ui_edbDistrubutor.Focus();
            }

            if (ui_edbPhoneNumber.Text.Equals(""))
            {
                Message += "\n Моля попълнете задължителното поле: Телефонен номер";

                if (!focus)
                    focus = ui_edbPhoneNumber.Focus();
            }

            if (!Message.Equals(""))
            {
                MessageBoxes.ShowWarning(Message);
                return false;
            }

            return true;
        }

        private void drugExpiryDate_Click(object sender, RoutedEventArgs e)
        {
            int numberNotWorthy = 0, numberDiscountedPrice = 0;

            List<DrugsInfo.DrugsInfo> drugsInfoListForSelect = new List<DrugsInfo.DrugsInfo>();
            if (!drugsInfoData.SelectAll(drugsInfoListForSelect, " WHERE DEALER_ID = " + dealerInfo.dealer.ID))
            {
                MessageBoxes.ShowError(MessageBoxes.EditErrorMessage);
                return;
            }

            dealerInfo.drugsInfoList = drugsInfoListForSelect;

            List<DrugsInfo.DrugsInfo> drugsInfoList = dealerInfo.ifDrugIsWorthy(out numberNotWorthy, out numberDiscountedPrice);

            string Message = "";

            if (numberNotWorthy != 0)
                Message += "Открити са " + numberNotWorthy + " негодни лекарства. Те ще бъдат премахнати.\n";

            if (numberDiscountedPrice != 0)
                Message += "Открити са " + numberDiscountedPrice + " лекарства, годността на които изтича след по - малко от месец. Цената им ще бъде намалена с 50%.";

            MessageBoxes.ShowWarning(Message);

            drugsInfoList.ForEach(delegate (DrugsInfo.DrugsInfo element)
            {
                if (element.Number == 0)
                {
                    if (!drugsInfoData.DeleteWhereID(element.ID))
                    {
                        MessageBoxes.ShowError(MessageBoxes.DeleteErrorMessage);
                        return;
                    }
                }
                else
                {
                    if (!drugsInfoData.UpdateWhereID(element.ID, element))
                    {
                        MessageBoxes.ShowError(MessageBoxes.EditErrorMessage);
                        return;
                    }
                }
            });

            drugsInfoView.Update();

            //ui_btndrugExpiryDate.IsEnabled = false;
        }

        private void SortByPrice_Click(object sender, RoutedEventArgs e)
        {
            Sort("Price");
        }

        private void SortByNumber_Click(object sender, RoutedEventArgs e)
        {
            Sort("Number");
        }

        private void Sort(string property)
        {
            ICollectionView dataView = CollectionViewSource.GetDefaultView(drugsInfoView.ItemsSource);
            dataView.SortDescriptions.Clear();
            dataView.SortDescriptions.Add(new SortDescription(property, ListSortDirection.Ascending));
            dataView.Refresh();
        }
    }
}
