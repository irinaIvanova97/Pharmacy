using System.Collections.Generic;

namespace Pharmacy.Drugs
{
    public class DrugsData
    {
        private DrugsTable drugsTable;

        public DrugsData()
        {
            drugsTable = new DrugsTable();
        }

        public bool SelectAll(List<Drugs> drugsList, string whereClause = "")
        {
            return drugsTable.SelectAllRecords(drugsList);
        }

        public bool SelectWhereID(int ID, out Drugs drug)
        {
            return drugsTable.SelectRecord(ID, out drug);
        }

        public bool UpdateWhereID(int ID, Drugs drug)
        {
            return drugsTable.EditRecord(ID, drug);
        }

        public bool Insert(Drugs drug)
        {
            return drugsTable.InsertRecord(drug);
        }

        public bool DeleteWhereID(int ID)
        {
            return drugsTable.DeleteRecord(ID);
        }
    }
}
