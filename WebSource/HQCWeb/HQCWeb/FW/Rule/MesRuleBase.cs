using System;
using System.Data;
using System.Collections.Generic;
using HQCWeb.FW.Data;
using HQCWeb.FW.Logging;
using System.Collections;

namespace HQCWeb.FW.Rule
{
    //public abstract class MesRuleBase
    public class MesRuleBase
    {
        public MesRuleBase() { 
        
        }

        public DataSet GetSearchQuerySetResult(SqlMapper mapper, string strQueryId, Parameters parameters)
        {
            //string strRtn = string.Empty;
            return (DataSet)mapper.GetDataSet(strQueryId, parameters);
        }


        public DataTable GetSearchQueryResult(SqlMapper mapper, string strQueryId, Parameters parameters)
        {
            //string strRtn = string.Empty;
            return (DataTable)mapper.GetDataTable(strQueryId, parameters);
        }

        public int GetExecuteQueryResult(SqlMapper mapper, string strQueryId, Parameters parameters)
        {
            //string strRtn = string.Empty;
            return (int)mapper.ExcuteNonQuery(strQueryId, parameters);
        }
    }
}
