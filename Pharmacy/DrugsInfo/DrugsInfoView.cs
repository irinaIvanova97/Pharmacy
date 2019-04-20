using Pharmacy.Controls;
using Pharmacy.DataBase;
using Pharmacy.Drugs;
using Pharmacy.Utillities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using static Pharmacy.Controls.BaseDialog;

namespace Pharmacy.DrugsInfo
{
    class DrugsInfoView : BaseTabView
    {
        // Members
        // ----------------
        private DrugsInfoData drugsInfoData = new DrugsInfoData();
        private List<Drugs.Drugs> drugsList = new List<Drugs.Drugs>();
        List<DrugsInfo> drugsInfoList = new List<DrugsInfo>();
        private DrugsData drugsData = new DrugsData();
        List<DrugsInfo> drugsInfoArray;
        int dealerID;

        private class DrugsItem : BaseRecord
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public int Number { get; set; }
            public double Price { get; set; }
            public DateTime ExpiryDate { get; set; }
            public ImageSource image { get; set; }

            public DrugsItem() : base(typeof(DrugsItem), 6)
            {
            }
        }

        public DrugsInfoView()
        {

        }

        public DrugsInfoView(int _delaerID, List<Drugs.Drugs> _drugsList, List<DrugsInfo> _drugsInfoList)
        {
            drugsList = _drugsList;
            dealerID = _delaerID;
            drugsInfoList = _drugsInfoList;
        }

        protected override void LoadData()
        {
            drugsInfoArray = new List<DrugsInfo>();
            if (!drugsInfoData.SelectAll(drugsInfoArray, " WHERE DEALER_ID = " + dealerID))
            {
                MessageBoxes.ShowError(MessageBoxes.LoadDataErrorMessage);
                return;
            }

            itemsSource.Clear();

            foreach (var drug in drugsInfoArray)
            {
                DrugsItem item = toDrugItem(drug, drugsList);
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
                Header = "Цена",
                DisplayMemberBinding = new Binding("Price"),
                Width = 120
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Брой",
                DisplayMemberBinding = new Binding("Number"),
                Width = 120
            });

            gridView.Columns.Add(new GridViewColumn
            {
                Header = "Дата на годност",
                DisplayMemberBinding = new Binding("ExpiryDate"),
                Width = 120,
            });
           
        }

        /// <summary>Метод при натискане на "Преглед" от контекстното меню</summary>
        protected override void OnPreviewClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != 1)
                return;

            DrugsInfo drugInfo = toDrugsInfo((DrugsItem)SelectedItem);
            if (!drugsInfoData.SelectWhereID(drugInfo.ID, out drugInfo))
             {
                 MessageBoxes.ShowError(MessageBoxes.PreviewErrorMessage);
                 return;
             }

            DrugsInfoDialog drugsInfoDialog = new DrugsInfoDialog(drugInfo, drugsList, DialogModes.Preview);
            drugsInfoDialog.ShowDialog();
        }

        protected override void OnAddClick(object sender, RoutedEventArgs e)
        {
            List<Drugs.Drugs> drugsList = new List<Drugs.Drugs>();
            if (!drugsData.SelectAll(drugsList))
            {
                MessageBoxes.ShowError(MessageBoxes.LoadDataErrorMessage);
                return;
            }

            DrugsInfoDialog drugsInfoDialog = new DrugsInfoDialog(new DrugsInfo(), drugsList, DialogModes.Add, this);
            bool? dialogResult = drugsInfoDialog.ShowDialog();
            if (dialogResult == false)
            {
                return;
            }

            DrugsInfo drugsInfo = drugsInfoDialog.drugInfo;
            drugsInfo.DealerID = dealerID;

            ///////////
            if (!drugsInfoData.Insert(drugsInfo))
            {
                MessageBoxes.ShowError(MessageBoxes.AddErrorMessage);
                return;
            }
            DrugsItem drugItem = toDrugItem(drugsInfo, drugsList);
            itemsSource.Add(drugItem);
        }

        protected override void OnEditClick(object sender, RoutedEventArgs e)
        {
            DrugsInfo drugInfo = toDrugsInfo((DrugsItem)SelectedItem);
            if (!drugsInfoData.SelectWhereID(drugInfo.ID, out drugInfo))
            {
                MessageBoxes.ShowError(MessageBoxes.PreviewErrorMessage);
                return;
            }

            DrugsInfoDialog drugsInfoDialog = new DrugsInfoDialog(drugInfo, drugsList, DialogModes.Edit);
            bool? dialogResult = drugsInfoDialog.ShowDialog(); 
            if (dialogResult == false)
            {
                return;
            }

            if (!drugsInfoData.UpdateWhereID(drugInfo.ID, drugInfo))
            {
                MessageBoxes.ShowError(MessageBoxes.EditErrorMessage);
                return;
            }

            itemsSource[SelectedIndex] = toDrugItem(drugInfo, drugsList);
        }

        protected override void OnDeleteClick(object sender, RoutedEventArgs e)
        {
            if (SelectedItems.Count != 1)
                return;

            MessageBoxResult result = MessageBoxes.MessageBoxShowDeleteMessage();
            if (result == MessageBoxResult.No)
                return;

            DrugsInfo drugInfo = toDrugsInfo((DrugsItem)SelectedItem);
            if (!drugsInfoData.DeleteWhereID(drugInfo.ID))
            {
                MessageBoxes.ShowError(MessageBoxes.DeleteErrorMessage);
                return;
            }

            itemsSource.RemoveAt(SelectedIndex);
        }

        private DrugsItem toDrugItem(DrugsInfo drugsInfo, List<Drugs.Drugs> drugsList)
        {
            DrugsItem drugItem = new DrugsItem();
            drugItem.ID = drugsInfo.ID;
            drugItem.Number = drugsInfo.Number;
            drugItem.Price = drugsInfo.Price;
            drugItem.ExpiryDate = drugsInfo.ExpiryDate;

            Drugs.Drugs drug = drugsList.Find(element => element.ID == drugsInfo.DrugID);
            if (drug != null)
            {
                drugItem.Name = drug.Name;
                drugItem.image = drug.image;
            }

            return drugItem;
        }

        private DrugsInfo toDrugsInfo(DrugsItem drugsItem)
        {
            if (!drugsData.SelectAll(drugsList))
            {
                MessageBoxes.ShowError(MessageBoxes.EditErrorMessage);
                return new DrugsInfo();
            }

            Drugs.Drugs drug = drugsList.Find(element => element.Name == drugsItem.Name);

            DrugsInfo drugsInfo = new DrugsInfo();
            if (drug != null)
            {

                drugsInfo.ID = drugsItem.ID;
                drugsInfo.Number = drugsItem.Number;
                drugsInfo.Price = drugsItem.Price;
                drugsInfo.ExpiryDate = drugsItem.ExpiryDate;
                drugsInfo.DrugID = drug.ID;
                drugsInfo.DealerID = dealerID;
            }

            return drugsInfo;
        }
    }
}
