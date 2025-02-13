using System.Data;
using Newtonsoft.Json.Linq;
using HQC.FW.Common;
using HQCWeb.FW;
using HQCWeb.FW.Data;
using HQCWeb.FW.Rule;

namespace HQCWeb.Biz.QualityManagement
{
    public class QuaWidget02
    {
        DataTable dt = new DataTable();
        DataSet dsList = new DataSet();

        SqlMapper Mapper = null;

        MesRuleBase MRB = new MesRuleBase();

        #region QuaWidget02
        public QuaWidget02()
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
    }
}