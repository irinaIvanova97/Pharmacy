using Pharmacy.DataBase;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Pharmacy.Dealers
{
    public class DealersTable : BaseTable<Dealers>
    {

        public DealersTable() : base("DEALERS", "ID", false)
        {

        }

        // Overrides
        // ---------
        protected override Dealers DataToRecord(SqlDataReader reader)
        {
            Dealers record = new Dealers();
            record.ID = reader.GetInt32(Dealers.ColumnID);
            record.Name = reader.GetString(Dealers.ColumnName);
            record.Distributor = reader.GetString(Dealers.ColumnDistributor);
            record.PhoneNumber = reader.GetString(Dealers.ColumnPhoneNumber);

            return record;
        }


        protected override Dealers DefaultRecordConstructor()
        {
            return new Dealers();
        }

        protected override List<string> GetColumns()
        {
            return new List<string> { "NAME", "DISTRIBUTOR", "PHONE_NUMBER" };
        }

        protected override int GetIDValue(Dealers record)
        {
            return record.ID;
        }

        protected override void RecordToData(SqlCommand command, Dealers record)
        {
            command.Parameters.Add("@NAME", SqlDbType.NVarChar, Dealers.NameSize).Value = record.Name;
            command.Parameters.Add("@DISTRIBUTOR", SqlDbType.NVarChar, Dealers.DistributorSize).Value = record.Distributor;
            command.Parameters.Add("@PHONE_NUMBER", SqlDbType.NVarChar, Dealers.PhoneNumberSize).Value = record.PhoneNumber;
        }

        protected override void SetIDValue(int ID, Dealers record)
        {
            record.ID = ID;
        }
    }
}
