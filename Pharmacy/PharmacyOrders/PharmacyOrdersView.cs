using Pharmacy.Controls;
using Pharmacy.Utillities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static Pharmacy.Controls.BaseDialog;

namespace Pharmacy.PharmacyOrders
{
    class PharmacyOrdersView : BaseTabView
    {
        // Members
        // ----------------
        private PharmacyOrdersData pharmacyOrdersData = new PharmacyOrdersData();
        private Pharmacies.PharmaciesData pharmacyData = new Pharmacies.PharmaciesData();
        private List<Pharmacies.Pharmacies> pharmacies = new List<Pharmacies.Pharmacies>();
        private Pharmacies.Pharmacies pharmacy;

        public PharmacyOrdersView()
        {
        }

        public PharmacyOrdersView(Pharmacies.Pharmacies _pharmacy)
        {
            pharmacy = _pharmacy;
        }

        public override object GetSelectedItem()
        {
            return SelectedItem;
        }

        protected override void LoadData()
        {
            List<PharmacyOrders> pharmacyOrdersList = new List<PharmacyOrders>();
            if (!pharmacyOrdersData.SelectAll(pharmacyOrdersList, " WHERE PHARMACY_ID = " + pharmacy.ID.ToString()))
            {
                MessageBoxes.ShowError(MessageBoxes.LoadDataErrorMessage);
                return;
            }

            foreach (var item in pharmacyOrdersList)
            {
                itemsSource.Add(item);
            }

            if (!pharmacyData.SelectAll(pharmacies))
            {
                MessageBoxes.ShowError(MessageBoxes.LoadDataErrorMessage);
                return;
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
                Header = "Име на лекарство",
                DisplayMemberBinding = new Binding("DrugName"),
                Width = 80
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Брой",
                DisplayMemberBinding = new Binding("NumberOrders"),
                Width = 80
            });
        }

        /* Context menu override */
        protected override void OnPreviewClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != 1)
                return;

            PharmacyOrders pharmacyOrder = ((PharmacyOrders)SelectedItem);

            if (!pharmacyOrdersData.SelectWhereID(pharmacyOrder.ID, out pharmacyOrder))
            {
                MessageBoxes.ShowError(MessageBoxes.PreviewErrorMessage);
                return;
            }

            PharmacyOrdersDialog pharmacyOrderDialog = new PharmacyOrdersDialog(pharmacyOrder, pharmacy, DialogModes.Preview, this);
            pharmacyOrderDialog.ShowDialog();
        }

        /* Context menu override */
        protected override void OnAddClick(object sender, RoutedEventArgs e)
        {
            PharmacyOrdersDialog pharmacyOrderDialog = new PharmacyOrdersDialog(new PharmacyOrders(), pharmacy, DialogModes.Add, this);
            bool? dialogResult = pharmacyOrderDialog.ShowDialog();
            if (dialogResult == false)
            {
                return;
            }

            PharmacyOrders pharmacyOrders = pharmacyOrderDialog.pharmacyOrders;
            if (!pharmacyOrdersData.Insert(pharmacyOrders))
            {
                MessageBoxes.ShowError(MessageBoxes.AddErrorMessage);
                return;
            }

            itemsSource.Add(pharmacyOrders);
        }

        /* Context menu override */
        protected override void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != 1)
                return;

            PharmacyOrders pharmacyOrders = (PharmacyOrders)SelectedItem;
            if (!pharmacyOrdersData.SelectWhereID(pharmacyOrders.ID, out pharmacyOrders))
            {
                MessageBoxes.ShowError(MessageBoxes.EditErrorMessage);
                return;
            }

            PharmacyOrdersDialog phramacyDialog = new PharmacyOrdersDialog(pharmacyOrders, pharmacy, DialogModes.Edit, this);
            bool? dialogResult = phramacyDialog.ShowDialog();
            if (dialogResult == false)
            {
                return;
            }

            pharmacyOrders = phramacyDialog.pharmacyOrders;
            if (!pharmacyOrdersData.UpdateWhereID(pharmacyOrders.ID, pharmacyOrders))
            {
                MessageBoxes.ShowError(MessageBoxes.EditErrorMessage);
                return;
            }

            itemsSource[SelectedIndex] = pharmacyOrders;
        }

        /* Context menu override */
        protected override void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != 1)
                return;

            MessageBoxResult result = MessageBoxes.MessageBoxShowDeleteMessage();
            if (result == MessageBoxResult.No)
                return;

            PharmacyOrders phramacy = (PharmacyOrders)SelectedItem;
            if (!pharmacyOrdersData.DeleteWhereID(phramacy.ID))
            {
                MessageBoxes.ShowError(MessageBoxes.DeleteErrorMessage);
                return;
            }

            itemsSource.RemoveAt(SelectedIndex);
        }
    }
}
