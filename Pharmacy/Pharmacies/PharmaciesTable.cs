using Pharmacy.DataBase;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Pharmacies
{
    class PharmaciesTable : BaseTable<Pharmacies>
    {
        public PharmaciesTable() : base("PHARMACY", "ID", false)
        {

        }

        // Overrides
        // ---------
        protected override Pharmacies DataToRecord(SqlDataReader reader)
        {
            Pharmacies record = new Pharmacies();
            record.ID = reader.GetInt32(Pharmacies.ColumnID);
            record.Name = reader.GetString(Pharmacies.ColumnName);

            return record;
        }

        protected override Pharmacies DefaultRecordConstructor()
        {
            return new Pharmacies();
        }

        protected override List<string> GetColumns()
        {
            return new List<string> { "NAME" };
        }

        protected override int GetIDValue(Pharmacies record)
        {
            return record.ID;
        }

        protected override void RecordToData(SqlCommand command, Pharmacies record)
        {
            command.Parameters.Add("@NAME", SqlDbType.NVarChar, Pharmacies.NameSize).Value = record.Name;
        }

        protected override void SetIDValue(int ID, Pharmacies record)
        {
            record.ID = ID;
        }
    }
}
