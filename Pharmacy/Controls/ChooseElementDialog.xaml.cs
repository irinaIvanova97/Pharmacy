using Pharmacy.Utillities;
using System.Windows;
using System.Windows.Controls;

namespace Pharmacy.Controls
{
    /// <summary>
    /// Interaction logic for ChooseElementDialog.xaml
    /// </summary>
    public partial class ChooseElementDialog : BaseDialog
    {
        private BaseTabView view;
        private string titlePostfix;
        public object SelectedItem { get; private set; }

        public ChooseElementDialog(BaseTabView _view, string _titlePostfix = "елемент", DependencyObject parent = null) : base(DialogModes.Add, parent)
        {
            view = _view;
            titlePostfix = _titlePostfix;

            InitializeComponent();
        }

        protected override void OnInitControls()
        {
            base.OnInitControls();

            Title += " " + titlePostfix;

            view.Width = double.NaN;
            view.Height = double.NaN;
            view.HorizontalAlignment = HorizontalAlignment.Stretch;
            view.VerticalAlignment = VerticalAlignment.Stretch;

            ui_gridListView.Children.Add(view);
        }

        protected override void GetOKButton(ref Button okButton)
        {
            okButton = ui_btnChoose;
        }

        protected override void GetCancelButton(ref Button cancelButton)
        {
            cancelButton = ui_btnCancel;
        }

        protected override void CopyControlsToData()
        {
            base.CopyControlsToData();

            SelectedItem = view.GetSelectedItem();
        }

        protected override bool ValidateData()
        {
            if (!base.ValidateData())
            {
                return false;
            }

            string message = "";
            bool focus = false;

            if (view.GetSelectedItem() == null)
            {
                message += string.Format("Моля изеберете {0}\n", titlePostfix);

                if (!focus)
                    focus = view.Focus();
            }

            if (!message.Equals(""))
            {
                MessageBoxes.ShowWarning(message);
                return false;
            }

            return true;
        }
    }
}
