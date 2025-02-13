using HQC.FW.Common;

using HQCWeb.FW.Data;
using HQCWeb.FW.Rule;

using System.Data;

namespace HQCWeb.FW
{
    public class DicService
    {
        #region DicData
        public static void DicData()
        {
            DataTable dtReturnTable;

            SqlMapper Mapper = null;

            MesRuleBase MRB = new MesRuleBase();

            Mapper = DataBaseService.mappers["GPDB"];

            dtReturnTable = MRB.GetSearchQueryResult(Mapper, "DicData.Get_DicInfoList", null);

            Dictionary_Data.SetDictionaryMemory(dtReturnTable);
        }
        #endregion
    }
}