using Pharmacy.Controls;
using Pharmacy.Utillities;
using System.Windows;
using System.Windows.Controls;

namespace Pharmacy.Pharmacies
{
    /// <summary>
    /// Interaction logic for PharmaciesDialog.xaml
    /// </summary>
    public partial class PharmaciesDialog : BaseDialog
    {
        public Pharmacies pharmacy { get; set; }

        public PharmaciesDialog(Pharmacies _pharmacy, DialogModes _dialogMode, DependencyObject parent = null) : base(_dialogMode, parent)
        {
            InitializeComponent();

            pharmacy = _pharmacy;
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
    }
}
