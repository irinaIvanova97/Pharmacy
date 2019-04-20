using Pharmacy.Controls;
using Pharmacy.DataBase;
using Pharmacy.Utillities;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using static Pharmacy.Controls.BaseDialog;

namespace Pharmacy.PharmacyOrders
{
    /// <summary>
    /// Interaction logic for PharmacyOrdersDialog.xaml
    /// </summary>
    public partial class PharmacyOrdersDialog : BaseDialog
    {
        public PharmacyOrders pharmacyOrders { get; set; }
        //public List<Pharmacies.Pharmacies> pharmaciesList { get; set; }
        Pharmacies.Pharmacies pharmacy;
        //private BaseRecordObservableCollection itemsSource;

        public PharmacyOrdersDialog(PharmacyOrders _pharmacyOrders, Pharmacies.Pharmacies _pharmacy, DialogModes _dialogMode, DependencyObject parent = null) : base(_dialogMode, parent)
        {
            pharmacyOrders = _pharmacyOrders;
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
        }

        protected override void OnInitControls()
        {
            base.OnInitControls();

            switch (DialogMode)
            {
                case DialogModes.Add:
                    {
                        Title = "Добавяне на поръчка...";

                        ui_btnOK.Content = "Добави";
                        ui_btnCancel.Content = "Отказ";
                    }
                    break;
                case DialogModes.Edit:
                    {
                        Title = "Редактиране на поръчка...";

                        ui_btnOK.Content = "Редактирай";
                        ui_btnCancel.Content = "Отказ";
                    }
                    break;
                case DialogModes.Preview:
                    {
                        Title = "Преглед на поръчка...";


                        ui_edbDrugName.IsReadOnly = true;
                        ui_edbNumber.IsReadOnly = true;

                        ui_btnOK.Visibility = Visibility.Hidden;
                        ui_btnCancel.Content = "Затвори";
                    }
                    break;
            }
        }

        protected override void CopyDataToControls()
        {
            ui_edbDrugName.Text = pharmacyOrders.DrugName.ToString();
            ui_edbNumber.Text = pharmacyOrders.NumberOrders.ToString();
        }

        protected override void CopyControlsToData()
        {

            pharmacyOrders.DrugName = ui_edbDrugName.Text;
            pharmacyOrders.NumberOrders = int.Parse(ui_edbNumber.Text);
            pharmacyOrders.PharmacyID = pharmacy.ID;
        }

        protected override bool ValidateData()
        {
            string Message = "";
            bool focus = false;

            if (ui_edbDrugName.Text.Equals(""))
            {
                Message += "\n Моля попълнете задължителното поле: Име на лекарство";

                if (!focus)
                    focus = ui_edbDrugName.Focus();
            }

            if (ui_edbNumber.Text.Equals(""))
            {
                Message += "\n Моля попълнете задължителното поле: Брой";

                if (!focus)
                    focus = ui_edbNumber.Focus();
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
