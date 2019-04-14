using Pharmacy.Drugs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DrugsInfo
{
    public class DrugsInfoData
    {
        private DrugsInfoTable drugsInfoTable;
        private DrugsTable drugsTable;

        public DrugsInfoData()
        {
            drugsInfoTable = new DrugsInfoTable();
            drugsTable = new DrugsTable();
        }

        public bool SelectAll(List<DrugsInfo> drugsInfoList, string whereClause="")
        {
            return drugsInfoTable.SelectAllRecords(drugsInfoList, whereClause);
        }

        public bool SelectAllDrugs(List<Drugs.Drugs> drugsList)
        {
            return drugsTable.SelectAllRecords(drugsList);
        }

        public bool SelectWhereID(int ID, out DrugsInfo drugsInfo)
        {
            return drugsInfoTable.SelectRecord(ID, out drugsInfo);
        }

        public bool UpdateWhereID(int ID, DrugsInfo drugsInfo)
        {
            return drugsInfoTable.EditRecord(ID, drugsInfo);
        }

        public bool Insert(DrugsInfo drugsInfo)
        {
            return drugsInfoTable.InsertRecord(drugsInfo);
        }

        public bool DeleteWhereID(int ID)
        {
            return drugsInfoTable.DeleteRecord(ID);
        }
    }
}
