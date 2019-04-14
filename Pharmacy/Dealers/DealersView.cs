using Pharmacy.Controls;
using Pharmacy.Utillities;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static Pharmacy.Controls.BaseDialog;

namespace Pharmacy.Dealers
{
    class DealersView : BaseTabView
    {
        // Members
        // ----------------
        private DealersData dealersData = new DealersData();

        public DealersView()
        {

        }

        protected override void LoadData()
        {
            List<Dealers> dealersArray = new List<Dealers>();
            if (!dealersData.SelectAll(dealersArray))
            {
                MessageBoxes.ShowError(MessageBoxes.LoadDataErrorMessage);
                return;
            }

            foreach (var item in dealersArray)
            {
                itemsSource.Add(item);
            }
        }

        // Overrides
        // ----------------
        protected override void OnInitColumns()
        {
            var gridView = new GridView();
            base.View = gridView;

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "№",
                DisplayMemberBinding = new Binding("ID"),
                Width = 40
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Име",
                DisplayMemberBinding = new Binding("Name"),
                Width = 80
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Дистрибутор",
                DisplayMemberBinding = new Binding("Distributor"),
                Width = 120
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Телефонен номер",
                DisplayMemberBinding = new Binding("PhoneNumber"),
                Width = 120
            });
        }

        /* Context menu override */
        protected override void OnPreviewClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != 1)
                return;

            int selectedID = ((Dealers)SelectedItem).ID;
            DealerInfo dealerInfo = new DealerInfo();

            if (!dealersData.SelectWhereID(selectedID, out dealerInfo))
            {
                MessageBoxes.ShowError(MessageBoxes.PreviewErrorMessage);
                return;
            }

            DealersDialog drugsDialog = new DealersDialog(dealerInfo, DialogModes.Preview, this);
            drugsDialog.ShowDialog();
        }

        /* Context menu override */
        protected override void OnAddClick(object sender, RoutedEventArgs e)
        {
            DealersDialog drugsDialog = new DealersDialog(new DealerInfo(), DialogModes.Add, this);
            bool? dialogResult = drugsDialog.ShowDialog();
            if (dialogResult == false)
            {
                return;
            }

            DealerInfo dealerInfo = drugsDialog.dealerInfo;
            if (!dealersData.Insert(dealerInfo.dealer))
            {
                MessageBoxes.ShowError(MessageBoxes.AddErrorMessage);
                return;
            }

            itemsSource.Add(dealerInfo.dealer);
        }

        protected override void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != 1)
                return;
            
            int selectedID = ((Dealers)SelectedItem).ID;
            DealerInfo dealerInfo = new DealerInfo();

            if (!dealersData.SelectWhereID(selectedID, out dealerInfo))
            {
                MessageBoxes.ShowError(MessageBoxes.PreviewErrorMessage);
                return;
            }

            DealersDialog drugsDialog = new DealersDialog(dealerInfo, DialogModes.Edit, this);
            bool? dialogResult = drugsDialog.ShowDialog();
            if (dialogResult == false)
            {
                return;
            }

            if (!dealersData.UpdateWhereID(selectedID, dealerInfo))
            {
                MessageBoxes.ShowError(MessageBoxes.EditErrorMessage);
                return;
            }

            itemsSource[SelectedIndex] = dealerInfo.dealer;
        }

        protected override void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != 1)
                return;

            MessageBoxResult result = MessageBoxes.MessageBoxShowDeleteMessage();
            if (result == MessageBoxResult.No)
                return;

            Dealers dealer = (Dealers)SelectedItem;
            if (!dealersData.DeleteWhereID(dealer.ID))
            {
                MessageBoxes.ShowError(MessageBoxes.DeleteErrorMessage);
                return;
            }

            itemsSource.RemoveAt(SelectedIndex);
        }
    }
}
