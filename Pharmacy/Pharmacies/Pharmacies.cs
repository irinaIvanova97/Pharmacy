using Pharmacy.DataBase;

namespace Pharmacy.Pharmacies
{
    public class Pharmacies : BaseRecord
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public const int ColumnID = 0;
        public const int ColumnName = 1;

        public const int NameSize = 32;

        public Pharmacies() : base(typeof(Pharmacies), 2)
        {
            ID = 0;
            Name = string.Empty;
        }
    }
}
