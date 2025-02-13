using HQC.FW.Common;

using HQCWeb.FW.Data;
using HQCWeb.FW.Rule;

using System.Data;
namespace HQCWeb.FW
{
    public class MsgService
    {
        #region MsgData
        public static void MsgData()
        {
            DataTable dtReturnTable;

            SqlMapper Mapper = null;

            MesRuleBase MRB = new MesRuleBase();

            Mapper = DataBaseService.mappers["GPDB"];

            dtReturnTable = MRB.GetSearchQueryResult(Mapper, "MsgData.Get_MsgInfoList", null);

            Message_Data.SetMessageMemory(dtReturnTable);
        }
        #endregion
    }
}