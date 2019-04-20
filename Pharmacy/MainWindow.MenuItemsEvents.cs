using Pharmacy.Controls;
using Pharmacy.Dealers;
using Pharmacy.Drugs;
using Pharmacy.Pharmacies;
using Pharmacy.PharmacyOrders;
using Pharmacy.Utillities;
using System.Collections.Generic;
using System.Windows;


namespace Pharmacy
{
    public partial class MainWindow
    {
        private void OnDrugsMenuItemClicked(object sender, RoutedEventArgs e)
        {
            var tab = new BaseTabItem("Лекарства");
            tab.SetView(new DrugsView(new List<Drugs.Drugs>(), false));
            AddTab(tab);
        }

        private void OnDealersMenuItemClicked(object sender, RoutedEventArgs e)
        {
            var tab = new BaseTabItem("Доставчици");
            tab.SetView(new DealersView());
            AddTab(tab);
        }

        private void OnPharmaciesMenuItemClicked(object sender, RoutedEventArgs e)
        {
            var tab = new BaseTabItem("Аптеки");
            tab.SetView(new PharmaciesView());
            AddTab(tab);
        }

        /*private void OnPharmaciesOrdersMenuItemClicked(object sender, RoutedEventArgs e)
        {
            var tab = new BaseTabItem("Поръчки");
            tab.SetView(new PharmacyOrdersView());
            AddTab(tab);
        }*/
       
    }
}
