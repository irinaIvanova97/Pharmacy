using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Pharmacies
{
    public class PharmaciesData
    {
        private PharmaciesTable pharmacyTable;

        public PharmaciesData()
        {
            pharmacyTable = new PharmaciesTable();
        }

        public bool SelectAll(List<Pharmacies> pharmacyList, string whereClause = "")
        {
            return pharmacyTable.SelectAllRecords(pharmacyList);
        }

        public bool SelectWhereID(int ID, out Pharmacies pharmacy)
        {
            return pharmacyTable.SelectRecord(ID, out pharmacy);
        }

        public bool UpdateWhereID(int ID, Pharmacies pharmacy)
        {
            return pharmacyTable.EditRecord(ID, pharmacy);
        }

        public bool Insert(Pharmacies pharmacy)
        {
            return pharmacyTable.InsertRecord(pharmacy);
        }

        public bool DeleteWhereID(int ID)
        {
            return pharmacyTable.DeleteRecord(ID);
        }
    }
}
