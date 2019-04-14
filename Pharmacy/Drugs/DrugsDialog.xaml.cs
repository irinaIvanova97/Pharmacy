using Microsoft.Win32;
using Pharmacy.Controls;
using Pharmacy.Utillities;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Pharmacy.Drugs
{
    /// <summary>
    /// Interaction logic for DrugsDialog.xaml
    /// </summary>
    public partial class DrugsDialog : BaseDialog
    {
        public Drugs drug { get; set; }

        public DrugsDialog(Drugs _drug, DialogModes _dialogMode, DependencyObject parent = null) : base(_dialogMode, parent)
        {
            InitializeComponent();

            drug = _drug;
        }

        protected override void GetOKButton(ref Button okButton)
        {
            okButton = ui_btnOK;
        }

        protected override void GetCancelButton(ref Button cancelButton)
        {
            cancelButton = ui_btnCancel;
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

                        ui_edbName.IsReadOnly = true;
                        ui_btnOK.Visibility = Visibility.Hidden;
                        ui_btnCancel.Content = "Затвори";
                        ui_btnAddImage.IsEnabled = false;
                    }
                    break;
            }
        }

        protected override void CopyDataToControls()
        {
            ui_edbName.Text = drug.Name.ToString();
            ui_image.Source = drug.image;
        }

        protected override void CopyControlsToData()
        {
            drug.Name = ui_edbName.Text;
            drug.image = ui_image.Source;
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

        private void ui_btnAddImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select a picture";
            openFileDialog.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                ui_image.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                drug.image = ui_image.Source;
            }

        }
    }
}
