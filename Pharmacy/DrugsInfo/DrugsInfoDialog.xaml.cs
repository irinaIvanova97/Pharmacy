using Pharmacy.Controls;
using Pharmacy.DataBase;
using Pharmacy.Utillities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Pharmacy.DrugsInfo
{
    /// <summary>
    /// Interaction logic for DrugsInfoDialog.xaml
    /// </summary>
    public partial class DrugsInfoDialog : BaseDialog
    {
        public DrugsInfo drugInfo { get; set; }
        public List<Drugs.Drugs> drugsList { get; set; }
        private BaseRecordObservableCollection itemsSource;

        public DrugsInfoDialog(DrugsInfo _drugInfo, List<Drugs.Drugs> _drugsList, DialogModes _dialogMode, DependencyObject parent = null) : base(_dialogMode, parent)
        {
            drugInfo = _drugInfo;
            drugsList = _drugsList;

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
            drugsList.ForEach(element => itemsSource.Add(element));
        }

        protected override void OnInitControls()
        {
            base.OnInitControls();

            switch (DialogMode)
            {
                case DialogModes.Add:
                    {
                        Title = "Добавяне на лекарство...";

                        ui_btnOK.Content = "Добави";
                        ui_btnCancel.Content = "Отказ";
                    }
                    break;
                case DialogModes.Edit:
                    {
                        Title = "Редактиране на лекарство...";

                        ui_btnOK.Content = "Редактирай";
                        ui_btnCancel.Content = "Отказ";
                    }
                    break;
                case DialogModes.Preview:
                    {
                        Title = "Преглед на лекарство...";

                        ui_cmbDrugName.IsEnabled = false;
                        ui_edbNumber.IsReadOnly = true;
                        ui_edbPrice.IsReadOnly = true;
                        ui_dpDate.IsEnabled = false;
                        ui_btnOK.Visibility = Visibility.Hidden;
                        ui_btnCancel.Content = "Затвори";
                    }
                    break;
            }

            ui_cmbDrugName.ItemsSource = itemsSource;
            ui_cmbDrugName.DisplayMemberPath = "Name";
            ui_cmbDrugName.SelectedValuePath = "ID";
            ui_cmbDrugName.SelectedIndex = -1;
        }

        protected override void CopyDataToControls()
        {
            
            Drugs.Drugs drug = drugsList.Find(element => element.ID == drugInfo.DrugID);
            if (drug != null)
            {
                //ui_cmbDrugName.SelectedValue = drug.ID == 0 ? -1 : drug.ID;
                ui_cmbDrugName.Text = drug.Name;
                ui_edbNumber.Text = drugInfo.Number.ToString();
                ui_edbPrice.Text = drugInfo.Price.ToString();
                ui_dpDate.Text = drugInfo.ExpiryDate.ToString();
                ui_image.Source = drug.image;
            }
            /*
               Drugs.Drugs drug = drugsList.Find(element => element.ID == drugInfo.DrugID);
            ui_cmbDrugName.Text = drug.Name;
            ui_edbNumber.Text = drugInfo.Number.ToString();
            ui_edbPrice.Text = drugInfo.Price.ToString();
            ui_dpDate.Text = drugInfo.ExpiryDate.ToString();
            ui_image.Source = drug.image;
             */
        }
        //////////////////////////////////
        protected override void CopyControlsToData()
        {
            //if (!DialogMode.Equals(DialogModes.Preview))
            //{
            //    Drugs.Drugs drug1 = drugsList.Find(element => element.ID == drugInfo.DrugID);
            //    if (drug1 != null)
            //    {
            //        drug1.Name = (string)ui_cmbDrugName.SelectedValue;
            //        /* drugInfo.Number = int.Parse(ui_edbNumber.Text);
            //         drugInfo.Price = double.Parse(ui_edbPrice.Text);
            //         drugInfo.ExpiryDate = DateTime.Parse(ui_dpDate.Text);*/
            //        drug1.image = ui_image.Source;
            //        drugInfo.DrugID = drug1.ID;
            //    }
            //}
            //else
            //{
            //    Drugs.Drugs drug = drugsList.Find(element => element.Name == ui_cmbDrugName.Text);
            //    if (drug != null)
            //        drugInfo.DrugID = drug.ID;
            //}

            //drugInfo.Number = int.Parse(ui_edbNumber.Text);
            //drugInfo.Price = double.Parse(ui_edbPrice.Text);
            //drugInfo.ExpiryDate = DateTime.Parse(ui_dpDate.Text);

            if (ui_cmbDrugName.SelectedValue != null)
            {
                Drugs.Drugs drug = drugsList.Find(element => element.Name == ui_cmbDrugName.Text);

                drugInfo.Number = int.Parse(ui_edbNumber.Text);
                drugInfo.Price = double.Parse(ui_edbPrice.Text);
                drugInfo.ExpiryDate = DateTime.Parse(ui_dpDate.Text);
                drugInfo.DrugID = drug.ID;
            }
        }

        protected override bool ValidateData()
        {
            string Message = "";
            bool focus = false;

            if (ui_cmbDrugName.Text.Equals(""))
            {
                Message += "\n Моля попълнете задължителното поле: Име";

                if (!focus)
                    focus = ui_cmbDrugName.Focus();
            }

            if (ui_edbNumber.Text.Equals(""))
            {
                Message += "\n Моля попълнете задължителното поле: Брой";

                if (!focus)
                    focus = ui_edbNumber.Focus();
            }

            if (ui_edbPrice.Text.Equals(""))
            {
                Message += "\n Моля попълнете задължителното поле: Цена";

                if (!focus)
                    focus = ui_edbPrice.Focus();
            }

            if (ui_dpDate.Text.Equals(""))
            {
                Message += "\n Моля попълнете задължителното поле: Дата на годност";

                if (!focus)
                    focus = ui_dpDate.Focus();
            }

            if (ui_image.Source == null)
            {
                Message += "\n Моля прикачете снимка";

                if (!focus)
                    focus = ui_image.Focus();
            }

            if (!Message.Equals(""))
            {
                MessageBoxes.ShowWarning(Message);
                return false;
            }

            return true;
        }

        private void ui_cmbDrugName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Drugs.Drugs drug = drugsList.Find(element => element.ID == (int)ui_cmbDrugName.SelectedValue);
            if (drug != null)
            {
                ui_image.Source = drug.image;
            }
        }
    }
}
