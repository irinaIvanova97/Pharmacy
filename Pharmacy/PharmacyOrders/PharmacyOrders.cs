using Pharmacy.DataBase;

namespace Pharmacy.PharmacyOrders
{
    public class PharmacyOrders : BaseRecord
    {
        public int ID { get; set; }
        public string DrugName { get; set; }
        public int NumberOrders { get; set; }
        public int PharmacyID { get; set; }

        public const int ColumnID = 0;
        public const int ColumnDrugName = 1;
        public const int ColumnNumberOrders = 2;
        public const int ColumnPharmacyID = 3;

        public const int NameSize = 32;

        public PharmacyOrders() : base(typeof(PharmacyOrders), 2)
        {
            ID = 0;
            DrugName = string.Empty;
            NumberOrders = 0;
            PharmacyID = 0;
        }
    }
}
