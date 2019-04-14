using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.Dealers
{
    public class DealerInfo
    {
        public Dealers dealer { get; set; }
        public List<Drugs.Drugs> drugsList { get; set; }
        public List<DrugsInfo.DrugsInfo> drugsInfoList { get; set; }

        public DealerInfo()
        {
            dealer = new Dealers();
            drugsList = new List<Drugs.Drugs>();
            drugsInfoList = new List<DrugsInfo.DrugsInfo>();
        }

        public DealerInfo(Dealers dealer, List<Drugs.Drugs> drugsList, List<DrugsInfo.DrugsInfo> drugsInfoList)
        {
            this.dealer = dealer;
            this.drugsList = drugsList;
            this.drugsInfoList = drugsInfoList;
        }
    }
}
