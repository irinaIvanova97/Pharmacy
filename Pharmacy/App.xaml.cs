using System.Reflection;
using System.Windows;
using Pharmacy.DataBase;

namespace Pharmacy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static readonly string AppName = "Pharmacy";

        protected override void OnStartup(StartupEventArgs e)
        {
            Log.Instance.InitializeLog();

            string version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            Log.LogInfo("**************** Pharmacy {0} Started ****************", version);

            MainWindow = new MainWindow();
            MainWindow.Show();
            
            base.OnStartup(e);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            try
            {
                DataBaseConnection.CloseConnection();
            }
            catch (System.Exception exception)
            {
                Log.LogException(exception);
            }

            base.OnExit(e);
        }
    }
}
