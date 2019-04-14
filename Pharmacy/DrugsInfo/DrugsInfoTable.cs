using Pharmacy.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DrugsInfo
{
    class DrugsInfoTable : BaseTable<DrugsInfo>
    {
        public DrugsInfoTable() : base("DRUGS_INFO", "ID", false)
        {

        }


        // Overrides
        // ---------
        protected override DrugsInfo DataToRecord(SqlDataReader reader)
        {
            DrugsInfo record = new DrugsInfo();
            record.ID = reader.GetInt32(DrugsInfo.ColumnID);
            record.Number = reader.GetInt32(DrugsInfo.ColumnNumber);
            record.Price = (double) reader.GetDecimal(DrugsInfo.ColumnPrice);
            record.ExpiryDate = reader.GetDateTime(DrugsInfo.ColumnExpiryDate);
            record.DrugID = reader.GetInt32(DrugsInfo.ColumnDrugID);
            record.DealerID = reader.GetInt32(DrugsInfo.ColumnDealerID);

            return record;
        }

        protected override DrugsInfo DefaultRecordConstructor()
        {
            return new DrugsInfo();
        }

        protected override List<string> GetColumns()
        {
            return new List<string> { "NUMBER", "PRICE", "EXPIRY_DATE", "DRUG_ID", "DEALER_ID" };
        }

        protected override int GetIDValue(DrugsInfo record)
        {
            return record.ID;
        }

        protected override void RecordToData(SqlCommand command, DrugsInfo record)
        {
            command.Parameters.Add("@NUMBER", SqlDbType.Int).Value = record.Number;
            command.Parameters.Add("@PRICE", SqlDbType.Decimal).Value = record.Price;
            command.Parameters.Add("@EXPIRY_DATE", SqlDbType.DateTime).Value = record.ExpiryDate;
            command.Parameters.Add("@DRUG_ID", SqlDbType.Int).Value = record.DrugID;
            command.Parameters.Add("@DEALER_ID", SqlDbType.Int).Value = record.DealerID;
        }

        protected override void SetIDValue(int ID, DrugsInfo record)
        {
            record.ID = ID;
        }
    }
}
