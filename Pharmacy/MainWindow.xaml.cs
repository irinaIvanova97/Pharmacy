using Pharmacy.Controls;
using Pharmacy.DataBase;
using Pharmacy.Pharmacies;
using Pharmacy.PharmacyOrders;
using System.Windows;
using System.Windows.Controls;

namespace Pharmacy
{
    public partial class MainWindow : Window
    {
        private DataBaseConnection dataBaseConnection = null;

        public MainWindow()
        {
            InitializeComponent();

            InitializeDatabaseInstance();

            OnConnectButtonClicked(null, null);
            
        }

        public void InitializeDatabaseInstance()
        {
            dataBaseConnection = DataBaseConnection.Instance;
            dataBaseConnection.OnConnectingStart += DataBaseConnection_OnConnectingStart;
            dataBaseConnection.OnConnectingEnd += DataBaseConnection_OnConnectingEnd;
            dataBaseConnection.OnConnectingError += DataBaseConnection_OnConnectingError;
        }

        private void OnExitButtonClicked(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void OnConnectButtonClicked(object sender, RoutedEventArgs e)
        {
            DataBaseConnection.CloseConnection();
            DataBaseConnection.OpenConnection();
        }

        private void AddTab(BaseTabItem tab)
        {
            tabControl.Items.Add(tab);
            tabControl.SelectedItem = tab;
        }

        private void OnAboutMenuItemClicked(object sender, RoutedEventArgs e)
        {
            AboutDialog about = new AboutDialog(this);
            about.ShowDialog();
        }
        
    }
}
