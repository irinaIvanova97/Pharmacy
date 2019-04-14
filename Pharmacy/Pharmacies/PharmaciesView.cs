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

namespace Pharmacy.Pharmacies
{
    class PharmaciesView : BaseTabView
    {
        // Members
        // ----------------
        private PharmaciesData pharmacyData = new PharmaciesData();

        public PharmaciesView()
        {
        }

        protected override void LoadData()
        {
            List<Pharmacies> pharmacyList = new List<Pharmacies>();
            if (!pharmacyData.SelectAll(pharmacyList))
            {
                MessageBoxes.ShowError(MessageBoxes.LoadDataErrorMessage);
                return;
            }

            foreach (var item in pharmacyList)
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
        }

        /* Context menu override */
        protected override void OnPreviewClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != 1)
                return;

            Pharmacies pharmacy = ((Pharmacies)SelectedItem);

            if (!pharmacyData.SelectWhereID(pharmacy.ID, out pharmacy))
            {
                MessageBoxes.ShowError(MessageBoxes.PreviewErrorMessage);
                return;
            }

            PharmaciesDialog pharmacyDialog = new PharmaciesDialog(pharmacy, DialogModes.Preview, this);
            pharmacyDialog.ShowDialog();
        }

        /* Context menu override */
        protected override void OnAddClick(object sender, RoutedEventArgs e)
        {
            PharmaciesDialog pharmacyDialog = new PharmaciesDialog(new Pharmacies(), DialogModes.Add, this);
            bool? dialogResult = pharmacyDialog.ShowDialog();
            if (dialogResult == false)
            {
                return;
            }

            Pharmacies pharmacy = pharmacyDialog.pharmacy;
            if (!pharmacyData.Insert(pharmacy))
            {
                MessageBoxes.ShowError(MessageBoxes.AddErrorMessage);
                return;
            }

            itemsSource.Add(pharmacy);
        }

        /* Context menu override */
        protected override void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != 1)
                return;

            Pharmacies pharmacy = (Pharmacies)SelectedItem;
            if (!pharmacyData.SelectWhereID(pharmacy.ID, out pharmacy))
            {
                MessageBoxes.ShowError(MessageBoxes.EditErrorMessage);
                return;
            }

            PharmaciesDialog phramacyDialog = new PharmaciesDialog(pharmacy, DialogModes.Edit, this);
            bool? dialogResult = phramacyDialog.ShowDialog();
            if (dialogResult == false)
            {
                return;
            }

            pharmacy = phramacyDialog.pharmacy;
            if (!pharmacyData.UpdateWhereID(pharmacy.ID, pharmacy))
            {
                MessageBoxes.ShowError(MessageBoxes.EditErrorMessage);
                return;
            }

            itemsSource[SelectedIndex] = pharmacy;
        }

        /* Context menu override */
        protected override void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != 1)
                return;

            MessageBoxResult result = MessageBoxes.MessageBoxShowDeleteMessage();
            if (result == MessageBoxResult.No)
                return;

            Pharmacies phramacy = (Pharmacies)SelectedItem;
            if (!pharmacyData.DeleteWhereID(phramacy.ID))
            {
                MessageBoxes.ShowError(MessageBoxes.DeleteErrorMessage);
                return;
            }

            itemsSource.RemoveAt(SelectedIndex);
        }
    }
}
