using Pharmacy.Controls;
using Pharmacy.DrugsInfo;
using Pharmacy.Utillities;
using System.Windows;
using System.Windows.Controls;

namespace Pharmacy.Dealers
{
    /// <summary>
    /// Interaction logic for DealersDialog.xaml
    /// </summary>
    public partial class DealersDialog : BaseDialog
    {
        public DealerInfo dealerInfo { get; set; }
        private DrugsInfoView drugsInfoView;

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
    }
}
