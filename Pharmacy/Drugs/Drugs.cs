using Pharmacy.DataBase;
using System.Windows.Media;

namespace Pharmacy.Drugs
{
    public class Drugs : BaseRecord
    {

        public int ID { get; set; }
        public string Name { get; set; }
        public ImageSource image { get; set; }

        public const int ColumnID = 0;
        public const int ColumnName = 1;
        public const int ColumnImage = 2;

        public const int NameSize = 32;

        public Drugs() : base(typeof(Drugs), 3)
        {
            ID = 0;
            Name = string.Empty;
            image = null;
        }
    }
}
