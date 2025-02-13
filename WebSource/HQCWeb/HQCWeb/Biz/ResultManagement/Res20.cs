using System.Data;
using Newtonsoft.Json.Linq;
using HQC.FW.Common;
using HQCWeb.FW;
using HQCWeb.FW.Data;
using HQCWeb.FW.Rule;

namespace HQCWeb.Biz.ResultManagement
{
    public class Res20
    {
        DataTable dt = new DataTable();
        DataSet dsList = new DataSet();

        SqlMapper Mapper = null;

        MesRuleBase MRB = new MesRuleBase();

        #region Res20
        public Res20()
        {

        }
        #endregion

        #region GetProd
        public DataSet GetProd(string strDBName, string strQueryID, Parameters sParam)
        {
            Mapper = DataBaseService.mappers[strDBName];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;
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
    }
}