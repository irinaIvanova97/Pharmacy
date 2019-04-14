using Pharmacy.DataBase;

namespace Pharmacy.Dealers
{
    public class Dealers : BaseRecord
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Distributor { get; set; }
        public string PhoneNumber { get; set; }

        public const int ColumnID = 0;
        public const int ColumnName = 1;
        public const int ColumnDistributor = 2;
        public const int ColumnPhoneNumber = 3;

        public const int NameSize = 32;
        public const int DistributorSize = 32;
        public const int PhoneNumberSize = 32;

        public Dealers() : base(typeof(Dealers), 4)
        {
            ID = 0;
            Name = string.Empty;
            Distributor = string.Empty;
            PhoneNumber = string.Empty;
        }
    }
}
