using Pharmacy.Pharmacies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Pharmacy
{
    /// <summary>
    /// Interaction logic for MainDialog.xaml
    /// </summary>
    public partial class MainDialog : Window
    {

        public MainDialog()
        {
            InitializeComponent();

            //ui_gridPharmacies.Children.Add(new PharmaciesView());
        }
    }
}
