using Pharmacy.Controls;
using Pharmacy.Utillities;
using System;
using System.Data.SqlClient;
using System.Windows;


namespace Pharmacy
{
    public partial class MainWindow
    {
        private void DataBaseConnection_OnConnectingStart(object sender, EventArgs e)
        {
            SqlConnection sqlConnection = sender as SqlConnection;

            ui_sbiServerDatabase.Content = "";
            ui_sbiStatus.Content = string.Format("Свързване към: {0}\\{1}",
                                                 sqlConnection.DataSource,
                                                 sqlConnection.Database);

            UpdateUIOnDatabaseConnection(false);
        }

        private void DataBaseConnection_OnConnectingEnd(object sender, EventArgs e)
        {
            SqlConnection sqlConnection = sender as SqlConnection;

            ui_sbiStatus.Content = "Готово";

            Log.LogInfo("Connected to Server: {0}, Database: {1}",
                        sqlConnection.DataSource,
                        sqlConnection.Database);

            ui_sbiServerDatabase.Content = string.Format("Server: {0}, Database: {1}",
                                                         sqlConnection.DataSource,
                                                         sqlConnection.Database);

            UpdateUIOnDatabaseConnection(true);

            // refresh current view
            if (tabControl.SelectedIndex != -1)
            {
                (tabControl.SelectedItem as BaseTabView).Update();
            }

            if (tabControl.Items.Count > 1)
            {
               MessageBoxes.ShowInfo("Необходимо е да отворите и затворите другите прозорци ръчно.");
            }
        }

        private void DataBaseConnection_OnConnectingError(object sender, EventArgs<Exception> e)
        {
            UpdateUIOnDatabaseConnection(true);

            MessageBox.Show("Error while opening connection to database.", "Pharmacy", MessageBoxButton.OK, MessageBoxImage.Error);

            ui_sbiStatus.Content = "Няма връзка";
            ui_sbiServerDatabase.Content = "";
        }

        private void UpdateUIOnDatabaseConnection(bool isEnable)
        {
            ui_psbLoading.Visibility = isEnable ? Visibility.Hidden : Visibility.Visible;

            ui_mRegisters.IsEnabled = isEnable;
        }
    }
}
