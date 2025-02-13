using System.Data;

using HQCWeb.FW;
using HQCWeb.FW.Data;
using HQCWeb.FW.Rule;

namespace HQCWeb.Biz.SystemManagement
{
    public class DictionaryMgt
    {
        DataTable dt = new DataTable();
        DataSet dsList = new DataSet();

        SqlMapper Mapper = null;

        MesRuleBase MRB = new MesRuleBase();

        #region DictionaryMgt
        public DictionaryMgt()
        {
           
        }
        #endregion

        #region GetDictionaryList
        public DataSet GetDictionaryList(string strDBName, string strQueryID, Parameters sParam)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;
        }
        #endregion        

        #region GetDictionaryData
        public DataSet GetDictionaryData(string strDBName, string strQueryID, Parameters sParam)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;
        }
        #endregion

        #region GetDictionaryIDValChk
        public string GetDictionaryIDValChk(string strDBName, string strQueryID, Parameters sParam)
        {
            string strRtnCode = string.Empty;

            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            strRtnCode = dt.Rows[0]["VAL_CHK"].ToString();
            
            return strRtnCode;
        }
        #endregion

        #region SetDictionaryInfo
        public int SetDictionaryInfo(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;
        }
        #endregion

        #region DelDictionaryInfo
        public int DelDictionaryInfo(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);       

            return iRtn;
        }
        #endregion

    }
}