using System;
using System.Collections.Generic;
using HQC.FW.Common;
using Newtonsoft.Json.Linq;

using System.Data;

namespace HQCWeb.FMB_FW
{
    public class JsonParse
    {
        public DataTable SetDataListParse(string receiveResult)
        {
            DataTable dt = null;

            if (receiveResult != "")
            {
                JObject jDoc1 = JObject.Parse(receiveResult);
                JArray jColumns = (JArray)JsonUtils.GetNode(jDoc1, "//message/return/returnColumns");
                JArray jlist = (JArray)JsonUtils.GetNode(jDoc1, "//message/return/returnTable");
                string returnCode = JsonUtils.GetNodeText(jDoc1, "//message/return/returncode");

                if (returnCode == "0" || returnCode == "1")
                {
                    dt = JsonUtils.ConvertJsonToDataTable(jlist);

                    if (dt == null || dt.Columns.Count == 0)
                    {
                        foreach (JValue jobj in jColumns)
                        {
                            dt = new DataTable();
                            dt.Columns.Add(jobj.Value.ToString());
                        }
                    }

                }
            }

            return dt;
        }

    }
}