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

        public bool ifAvailable(string DrugName, int numberDrug)
        {
            Drugs.Drugs drug = drugsList.Find(x => x.Name == DrugName);

            if (drug != null)
            {
                DrugsInfo.DrugsInfo drugsInfo = drugsInfoList.Find(x => x.DrugID == drug.ID);

                if (drugsInfo != null)
                {
                    if (drugsInfo.Number >= numberDrug)
                        return true;
                }
            }

            return false;
        }

        public List<DrugsInfo.DrugsInfo> ifDrugIsWorthy(out int numberNotWorthy, out int numberDiscountedPrice)
        {
            DateTime today = DateTime.Today;
            numberNotWorthy = 0;
            numberDiscountedPrice = 0;
            foreach (DrugsInfo.DrugsInfo drugsInfo in drugsInfoList)
            {
                TimeSpan timeSpan = drugsInfo.ExpiryDate - today;

                if (timeSpan.Days < 0)
                {
                    //drugsInfoList.Remove(drugsInfo);
                    drugsInfo.Number = 0;
                    numberNotWorthy++;
                }

                if (timeSpan.Days >= 0 && timeSpan.Days <= 30)
                {
                    drugsInfo.Price /= 2;
                    numberDiscountedPrice++;
                }
            }

            return drugsInfoList;
        }
    }
}
