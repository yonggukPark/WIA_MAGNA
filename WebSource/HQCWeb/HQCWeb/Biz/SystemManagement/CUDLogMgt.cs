using System.Data;
using Newtonsoft.Json.Linq;
using HQC.FW.Common;
using HQCWeb.FW;
using HQCWeb.FW.Data;
using HQCWeb.FW.Rule;

namespace HQCWeb.Biz.SystemManagement
{
    public class CUDLogMgt
    {
        DataTable dt = new DataTable();
        DataSet dsList = new DataSet();

        SqlMapper Mapper = null;

        MesRuleBase MRB = new MesRuleBase();

        #region CUDMgt
        public CUDLogMgt()
        {

        }
        #endregion

        #region GetCUDLogList
        public DataSet GetCUDLogList(string strDBName, string strQueryID, Parameters sParam)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;
        }
        #endregion

        #region GetCUDLogInfo
        public DataSet GetCUDLogInfo(string strDBName, string strQueryID, Parameters sParam)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;
        }
        #endregion

        #region GetMenuList
        public DataSet GetMenuList(string strDBName, string strQueryID)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, null);

            dsList.Tables.Add(dt);

            return dsList;
        }
        #endregion
    }
}