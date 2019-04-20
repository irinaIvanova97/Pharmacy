using Pharmacy.Controls;
using Pharmacy.Utillities;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using static Pharmacy.Controls.BaseDialog;

namespace Pharmacy.Drugs
{
    class DrugsView : BaseTabView
    {
        // Members
        // ----------------
        private DrugsData drugsData = new DrugsData();
        private List<Drugs> drugsList = new List<Drugs>();
        private DrugsInfo.DrugsInfoData drugsInfoData = new DrugsInfo.DrugsInfoData();
        bool outerDrugList = false;

        public DrugsView(List<Drugs> _drugsList, bool _outerDrugList)
        {
            drugsList = _drugsList;
            outerDrugList = _outerDrugList;
        }

        protected override void LoadData()
        {
            if (outerDrugList)
            {
                foreach (var item in drugsList)
                {
                    itemsSource.Add(item);
                }
            }
            else
            {
                List<Drugs> drugsArray = new List<Drugs>();
                if (!drugsData.SelectAll(drugsArray))
                {
                    MessageBoxes.ShowError(MessageBoxes.LoadDataErrorMessage);
                    return;
                }

                foreach (var item in drugsArray)
                {
                    itemsSource.Add(item);
                }
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
                Width = 120
            });
            
        }

        /* Context menu override */
        protected override void OnPreviewClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != 1)
                return;

            Drugs drug = (Drugs)SelectedItem;
            if (!drugsData.SelectWhereID(drug.ID, out drug))
            {
                MessageBoxes.ShowError(MessageBoxes.PreviewErrorMessage);
                return;
            }

            DrugsDialog drugsDialog = new DrugsDialog(drug, DialogModes.Preview, this);
            drugsDialog.ShowDialog();
        }

        /* Context menu override */
        protected override void OnAddClick(object sender, RoutedEventArgs e)
        {
            DrugsDialog drugsDialog = new DrugsDialog(new Drugs(), DialogModes.Add, this);
            bool? dialogResult = drugsDialog.ShowDialog();
            if (dialogResult == false)
            {
                return;
            }

            Drugs drug = drugsDialog.drug;
            if (!drugsData.Insert(drug))
            {
                MessageBoxes.ShowError(MessageBoxes.AddErrorMessage);
                return;
            }

            itemsSource.Add(drug);
        }

        /* Context menu override */
        protected override void OnEditClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != 1)
                return;

            Drugs drug = (Drugs)SelectedItem;
            if (!drugsData.SelectWhereID(drug.ID, out drug))
            {
                MessageBoxes.ShowError(MessageBoxes.EditErrorMessage);
                return;
            }

            DrugsDialog drugsDialog = new DrugsDialog(drug, DialogModes.Edit, this);
            bool? dialogResult = drugsDialog.ShowDialog();
            if (dialogResult == false)
            {
                return;
            }

            drug = drugsDialog.drug;
            if (!drugsData.UpdateWhereID(drug.ID, drug))
            {
                MessageBoxes.ShowError(MessageBoxes.EditErrorMessage);
                return;
            }

            itemsSource[SelectedIndex] = drug;
        }

        /* Context menu override */
        protected override void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != 1)
                return;

            MessageBoxResult result = MessageBoxes.MessageBoxShowDeleteMessage();
            if (result == MessageBoxResult.No)
                return;


            List<DrugsInfo.DrugsInfo> drugsInfoList = new List<DrugsInfo.DrugsInfo>();
            Drugs drug = (Drugs)SelectedItem;

            if (!drugsInfoData.SelectAll(drugsInfoList, " WHERE DRUG_ID = " + drug.ID))
            {
                MessageBoxes.ShowError(MessageBoxes.DeleteErrorMessage);
                return;
            }

            foreach (DrugsInfo.DrugsInfo drugInfo in drugsInfoList)
            {
                if (!drugsInfoData.DeleteWhereID(drugInfo.ID))
                {
                    MessageBoxes.ShowError(MessageBoxes.DeleteErrorMessage);
                    return;
                }
            }

            if (!drugsData.DeleteWhereID(drug.ID))
            {
                MessageBoxes.ShowError(MessageBoxes.DeleteErrorMessage);
                return;
            }

            itemsSource.RemoveAt(SelectedIndex);
        }
    }
}
