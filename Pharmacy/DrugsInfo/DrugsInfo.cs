using Pharmacy.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DrugsInfo
{
    public class DrugsInfo : BaseRecord
    {
        public int ID { get; set; }
        public int Number { get; set; }
        public double Price { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int DrugID { get; set; }
        public int DealerID { get; set; }

        public const int ColumnID = 0;
        public const int ColumnNumber = 1;
        public const int ColumnPrice = 2;
        public const int ColumnExpiryDate = 3;
        public const int ColumnDrugID = 4;
        public const int ColumnDealerID = 5;

        public const int NameSize = 32;

        public DrugsInfo() : base(typeof(DrugsInfo), 6)
        {
            ID = 0;
            Number = 0;
            Price = 0.0;
            ExpiryDate = new DateTime();
            DrugID = 0;
            DealerID = 0;
        }
    }
}
