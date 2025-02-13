using HQC.FW.Common;
using HQCWeb.FMB_FW;
using HQCWeb.FW;
using HQCWeb.FW.Data;
using HQCWeb.FW.Rule;
using MES.FW.Common.CommonMgt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace HQCWeb.Biz.SystemManagement
{
    public class MenuMgt
    {
        DataTable dt = new DataTable();
        DataSet dsList = new DataSet();
        
        SqlMapper Mapper = null;

        MesRuleBase MRB = new MesRuleBase();

        public string strPlant = string.Empty;

        #region MenuMgt
        public MenuMgt()
        {

        }
        #endregion

        #region GetMenuList
        public DataSet GetMenuList(string strDBName, string strQueryID, Parameters sParam)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;
        }
        #endregion
        
        #region GetMenuLevel
        public DataSet GetMenuLevel(string strDBName, string strQueryID)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, null);

            if (dt.Rows.Count == 0) {
                dt = new DataTable();
                dt.Columns.Add("MENU_LEVEL");
                dt.Rows.Add("1");

                dsList.Tables.Add(dt);
            } else {
                dsList.Tables.Add(dt);
            }

            return dsList;
        }
        #endregion

        #region GetParentMenuLevel
        public string GetParentMenuLevel(string strDBName, string strQueryID, Parameters sParam)
        {
            string strRtnCode = string.Empty;

            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            if (dt.Rows.Count > 0)
            {
                strRtnCode = dt.Rows[0]["MENU_LEVEL"].ToString();
            }
            else
            {
                strRtnCode = "0";
            }

            return strRtnCode;
        }
        #endregion

        #region GetMenuData
        public DataSet GetMenuData(string strDBName, string strQueryID, Parameters sParam)
        {
            Mapper = DataBaseService.mappers[strDBName];
            
            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;
        }
        #endregion

        #region GetMenuIDValChk
        public string GetMenuIDValChk(string strDBName, string strQueryID, Parameters sParam)
        {
            string strRtnCode = string.Empty;

            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            strRtnCode = dt.Rows[0]["VAL_CHK"].ToString();
            
            return strRtnCode;
        }
        #endregion

        #region SetMenuInfo
        public int SetMenuInfo(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;
        }
        #endregion

        #region DelMenuInfo
        public int DelMenuInfo(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;
        }
        #endregion

        #region GetMenuDepthInfo
        public DataSet GetMenuDepthInfo(string strDBName, string strQueryID, Parameters sParam)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;
        }
        #endregion

        #region SetMenuControl
        public int SetMenuControl(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;
        }
        #endregion

        #region GetMenuControlInfo
        public DataSet GetMenuControlInfo(string strDBName, string strQueryID, Parameters sParam)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;
        }
        #endregion

        #region SetMenuAccess
        public int SetMenuAccess(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;
        }
        #endregion
    }
}