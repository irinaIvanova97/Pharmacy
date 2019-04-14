using Pharmacy.Controls;
using System;
using System.Windows;
using System.Windows.Controls;
using System.IO;

namespace Pharmacy
{
    /// <summary>
    /// Interaction logic for AboutDialog.xaml
    /// </summary>
    public partial class AboutDialog : BaseDialog
    {
        public AboutDialog(DependencyObject parent = null) : base(DialogModes.Preview, parent, true, false)
        {
            InitializeComponent();
        }

        protected override void GetOKButton(ref Button okButton)
        {
            okButton = ui_btnClose;
        }

        protected override void OnInitControls()
        {
            base.OnInitControls();

            LoadLicense();
        }

        private void LoadLicense()
        {
            try
            {
                var uri = new Uri("pack://application:,,,/Pharmacy;component/Resources/license.txt");
                var resourceStream = Application.GetResourceStream(uri);

                using (var reader = new StreamReader(resourceStream.Stream))
                {
                    var text = reader.ReadToEnd();
                    ui_txbLicense.Text = text;
                }
            }
            catch (Exception exception)
            {
                Log.LogException(exception);
            }
        }
    }
}
