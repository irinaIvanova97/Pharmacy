using Pharmacy.DataBase;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Pharmacy.PharmacyOrders
{
    public class PharmacyOrdersTable : BaseTable<PharmacyOrders>
    {
        public PharmacyOrdersTable() : base("PHARMACIES_ORDERS", "ID", false)
        {

        }

        // Overrides
        // ---------
        protected override PharmacyOrders DataToRecord(SqlDataReader reader)
        {
            PharmacyOrders record = new PharmacyOrders();
            record.ID = reader.GetInt32(PharmacyOrders.ColumnID);
            record.DrugName = reader.GetString(PharmacyOrders.ColumnDrugName);
            record.NumberOrders = reader.GetInt32(PharmacyOrders.ColumnNumberOrders);
            record.PharmacyID = reader.GetInt32(PharmacyOrders.ColumnPharmacyID);

            return record;
        }

        protected override PharmacyOrders DefaultRecordConstructor()
        {
            return new PharmacyOrders();
        }

        protected override List<string> GetColumns()
        {
            return new List<string> { "DRUG_NAME", "NUMBER_ORDERS", "PHARMACY_ID" };
        }

        protected override int GetIDValue(PharmacyOrders record)
        {
            return record.ID;
        }

        protected override void RecordToData(SqlCommand command, PharmacyOrders record)
        {
            command.Parameters.Add("@DRUG_NAME", SqlDbType.NVarChar, PharmacyOrders.NameSize).Value = record.DrugName;
            command.Parameters.Add("@NUMBER_ORDERS", SqlDbType.Int).Value = record.NumberOrders;
            command.Parameters.Add("@PHARMACY_ID", SqlDbType.Int).Value = record.PharmacyID;
        }

        protected override void SetIDValue(int ID, PharmacyOrders record)
        {
            record.ID = ID;
        }
    }
}
