using System.Data;
using Newtonsoft.Json.Linq;
using HQC.FW.Common;
using HQCWeb.FW;
using HQCWeb.FW.Data;
using HQCWeb.FW.Rule;

namespace HQCWeb.Biz.QualityManagement
{
    public class Qua13
    {
        DataTable dt = new DataTable();
        DataSet dsList = new DataSet();

        SqlMapper Mapper = null;

        MesRuleBase MRB = new MesRuleBase();

        #region Qua13
        public Qua13()
        {

        }
        #endregion

        #region GetDataSet
        public DataSet GetDataSet(string strDBName, string strQueryID, Parameters sParam)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dsList = MRB.GetSearchQuerySetResult(Mapper, strQueryID, sParam);

            return dsList;
        }
        #endregion

        #region GetIDValChk
        public string GetIDValChk(string strDBName, string strQueryID, Parameters sParam)
        {
            string strRtnCode = string.Empty;

            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            strRtnCode = dt.Rows[0]["VAL_CHK"].ToString();

            return strRtnCode;
        }
        #endregion

        #region SetCUD
        public int SetCUD(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;
        }
        #endregion
    }
}