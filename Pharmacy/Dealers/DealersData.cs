using Pharmacy.Drugs;
using Pharmacy.DrugsInfo;
using Pharmacy.Utillities;
using System.Collections.Generic;

namespace Pharmacy.Dealers
{
    public class DealersData
    {
        private DealersTable dealersTable;
        private DrugsInfoData drugsInfoData;
        private DrugsData drugsData;

        public DealersData()
        {
            dealersTable = new DealersTable();
            drugsInfoData = new DrugsInfoData();
            drugsData = new DrugsData();
        }

        public bool SelectAll(List<Dealers> dealersList)
        {
            return dealersTable.SelectAllRecords(dealersList);
        }

        public bool SelectAllDrugsInfo(List<DrugsInfo.DrugsInfo> drugsInfoList, int dealerID)
        {
            return drugsInfoData.SelectAll(drugsInfoList, " WHERE DEALER_ID = " + dealerID);
        }

        public bool SelectAllDrugs(List<Drugs.Drugs> drugsList, int drugID)
        {
            return drugsData.SelectAll(drugsList, " WHERE ID = " + drugID);
        }

        public bool SelectDrugWhereID(out Drugs.Drugs drug, int drugID)
        {
            return drugsData.SelectWhereID(drugID, out drug);
        }

        public bool SelectWhereID(int ID, out DealerInfo dealerInfo)
        {
            Dealers dealer = new Dealers();
            dealerInfo = new DealerInfo();

            //Get dealer
            if (!dealersTable.SelectRecord(ID, out dealer))
            {
                MessageBoxes.ShowError(MessageBoxes.LoadDataErrorMessage);
                return false;
            }

            //Get drug info list
            List<DrugsInfo.DrugsInfo> drugsInfoList = new List<DrugsInfo.DrugsInfo>();
            if (!SelectAllDrugsInfo(drugsInfoList, ID))
            {
                MessageBoxes.ShowError(MessageBoxes.LoadDataErrorMessage);
                return false;
            }

            //Get drugs list
            List<Drugs.Drugs> drugsList = new List<Drugs.Drugs>();

            for (int i = 0; i < drugsInfoList.Count; i++)
            {
                Drugs.Drugs drug = new Drugs.Drugs();
                if (!drugsData.SelectWhereID(drugsInfoList[i].DrugID, out drug))
                {
                    MessageBoxes.ShowError(MessageBoxes.LoadDataErrorMessage);
                    return false;
                }
                drugsList.Add(drug);
            }


            dealerInfo.dealer = dealer;
            dealerInfo.drugsInfoList = drugsInfoList;
            dealerInfo.drugsList = drugsList;

            return true;
        }

        public bool UpdateWhereID(int ID, DealerInfo dealerInfo)
        {
            //Get dealer
            if (!dealersTable.EditRecord(ID, dealerInfo.dealer))
            {
                MessageBoxes.ShowError(MessageBoxes.LoadDataErrorMessage);
                return false;
            }

            //Get dealer
            foreach (var drugInfo in dealerInfo.drugsInfoList)
            {
                if (!drugsInfoData.UpdateWhereID(drugInfo.ID, drugInfo))
                {
                    MessageBoxes.ShowError(MessageBoxes.EditErrorMessage);
                    return false;
                }
            }

            return true;
        }

        public bool Insert(Dealers delaer)
        {
            return dealersTable.InsertRecord(delaer);
        }

        public bool DeleteWhereID(int ID)
        {
            List<DrugsInfo.DrugsInfo> drugsInfoList = new List<DrugsInfo.DrugsInfo>();
            if (!drugsInfoData.SelectAll(drugsInfoList, " WHERE DEALER_ID = " + ID))
            {
                MessageBoxes.ShowError(MessageBoxes.LoadDataErrorMessage);
                return false;

            }

            foreach (var drugsInfo in drugsInfoList)
            {
                if (!drugsInfoData.DeleteWhereID(drugsInfo.ID))
                {
                    MessageBoxes.ShowError(MessageBoxes.DeleteErrorMessage);
                    return false;
                }
            }

            if (!dealersTable.DeleteRecord(ID))
            {
                MessageBoxes.ShowError(MessageBoxes.DeleteErrorMessage);
                return false;
            }


            return true;
        }
    }
}
