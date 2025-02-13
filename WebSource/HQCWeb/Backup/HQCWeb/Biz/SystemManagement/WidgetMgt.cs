using System.Data;
using Newtonsoft.Json.Linq;
using HQC.FW.Common;
using HQCWeb.FW;
using HQCWeb.FW.Data;
using HQCWeb.FW.Rule;

namespace HQCWeb.Biz.SystemManagement
{
    public class WidgetMgt
    {
        DataTable dt = new DataTable();
        DataSet dsList = new DataSet();

        SqlMapper Mapper = null;

        MesRuleBase MRB = new MesRuleBase();

        #region WidgetMgt
        public WidgetMgt()
        {

        }
        #endregion

        #region GetWidgetList
        public DataSet GetWidgetList(string strDBName, string strQueryID, Parameters sParam)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;
        }
        #endregion

        #region GetWidgetInfo
        public DataSet GetWidgetInfo(string strDBName, string strQueryID, Parameters sParam)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;
        }
        #endregion
        
        #region SetWidgetInfo
        public int SetWidgetInfo(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;
        }
        #endregion

        #region DelWidgetInfo
        public int DelWidgetInfo(string strDBName, string strQueryID, Parameters sParam)
        {
            int iRtn = 0;

            Mapper = DataBaseService.mappers[strDBName];

            iRtn = MRB.GetExecuteQueryResult(Mapper, strQueryID, sParam);

            return iRtn;
        }
        #endregion
    }
}