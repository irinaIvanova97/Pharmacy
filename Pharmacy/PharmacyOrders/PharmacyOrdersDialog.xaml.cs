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
        public List<Pharmacies.Pharmacies> pharmaciesList { get; set; }
        private BaseRecordObservableCollection itemsSource;

        public PharmacyOrdersDialog(PharmacyOrders _pharmacyOrders, List<Pharmacies.Pharmacies> _pharmaciesList, DialogModes _dialogMode, DependencyObject parent = null) : base(_dialogMode, parent)
        {
            pharmacyOrders = _pharmacyOrders;
            pharmaciesList = _pharmaciesList;

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

            itemsSource = new BaseRecordObservableCollection();
            pharmaciesList.ForEach(element => itemsSource.Add(element));
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

                        ui_cmbPharmacy.IsEnabled = false;
                        ui_edbDrugName.IsReadOnly = true;
                        ui_edbNumber.IsReadOnly = true;

                        ui_btnOK.Visibility = Visibility.Hidden;
                        ui_btnCancel.Content = "Затвори";
                    }
                    break;
            }

            ui_cmbPharmacy.ItemsSource = itemsSource;
            ui_cmbPharmacy.DisplayMemberPath = "Name";
            ui_cmbPharmacy.SelectedValuePath = "ID";
            ui_cmbPharmacy.SelectedIndex = -1;
        }

        protected override void CopyDataToControls()
        {
            Pharmacies.Pharmacies pharmacy = pharmaciesList.Find(element => element.ID == pharmacyOrders.PharmacyID);

            if (pharmacy != null)
            {
                ui_cmbPharmacy.Text = pharmacy.Name;
                ui_edbDrugName.Text = pharmacyOrders.DrugName.ToString();
                ui_edbNumber.Text = pharmacyOrders.NumberOrders.ToString();
            }
        }

        protected override void CopyControlsToData()
        {
            if (ui_cmbPharmacy.SelectedValue != null)
            {
                Pharmacies.Pharmacies pharmacy = pharmaciesList.Find(element => element.Name == ui_cmbPharmacy.Text);

                pharmacyOrders.DrugName = ui_edbDrugName.Text;
                pharmacyOrders.NumberOrders = int.Parse(ui_edbNumber.Text);
                pharmacyOrders.PharmacyID = pharmacy.ID;
            }
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
