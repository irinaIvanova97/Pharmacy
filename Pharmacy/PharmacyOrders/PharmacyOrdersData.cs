using System.Collections.Generic;

namespace Pharmacy.PharmacyOrders
{
    public class PharmacyOrdersData
    {
        private PharmacyOrdersTable pharmacyOrdersTable;

        public PharmacyOrdersData()
        {
            pharmacyOrdersTable = new PharmacyOrdersTable();
        }

        public bool SelectAll(List<PharmacyOrders> pharmacyOrdersList, string whereClause = "")
        {
            return pharmacyOrdersTable.SelectAllRecords(pharmacyOrdersList);
        }

        public bool SelectWhereID(int ID, out PharmacyOrders pharmacyOrders)
        {
            return pharmacyOrdersTable.SelectRecord(ID, out pharmacyOrders);
        }

        public bool UpdateWhereID(int ID, PharmacyOrders pharmacyOrders)
        {
            return pharmacyOrdersTable.EditRecord(ID, pharmacyOrders);
        }

        public bool Insert(PharmacyOrders pharmacyOrder)
        {
            return pharmacyOrdersTable.InsertRecord(pharmacyOrder);
        }

        public bool DeleteWhereID(int ID)
        {
            return pharmacyOrdersTable.DeleteRecord(ID);
        }
    }
}
