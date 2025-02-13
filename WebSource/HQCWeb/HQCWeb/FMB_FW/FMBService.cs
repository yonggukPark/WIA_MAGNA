using HQC.FW.Common;
using HQCWeb.FW;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data;

namespace HQCWeb.FMB_FW
{
    public class FMBService
    {
        public static JObject jReceiveResult_FMB01;
        public static JObject jReceiveResult_FMB02;
        public static JObject jReceiveResult_FMB03;

        #region FMB01
        public static void FMB01()
        {
            JObject jDoc;
            JObject jParams;

            string receiveResult = string.Empty;

            JObject jReceiveResult;
            JToken jReturnTable;
            DataTable dtReturnTable;
            string strRtnCode = string.Empty;
            string strRtnMsg = string.Empty;

            jDoc = JsonUtils.MakeBaseMessage("FMBData.Get_CustomQuery", "0", string.Empty, "9012C");
            JsonUtils.AddTextNode(jDoc, "//message/header", "fmbmode", "Y");

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     "DEVMESDB");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "FMBData.Get_CustomQuery");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");

            jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            JsonUtils.AddChildTextNode(jParams, "siteid",   "9012C");
            JsonUtils.AddChildTextNode(jParams, "queryid",  "Get_FMB_QueryList_FM");

            receiveResult = string.Empty;
            TibcoService.SendTibcoMessage_FMB(null, jDoc.ToString(), ref receiveResult, true);

            jReceiveResult_FMB01 = (JObject)JsonConvert.DeserializeObject(receiveResult);
            jReturnTable = JsonUtils.GetNode(jReceiveResult_FMB01, "//message/return/returnTable");
            dtReturnTable = JsonUtils.ConvertJsonToDataTable(jReturnTable);
            strRtnCode = JsonUtils.GetNodeText(jReceiveResult_FMB01, "//message/return/returncode");
            strRtnMsg = JsonUtils.GetNodeText(jReceiveResult_FMB01, "//message/return/returnmessage");

            if (strRtnCode == "1")
            {
                // Tibco ReConnection
                TibcoService.TIBCO_Open();
                TibcoService.FMB_Conn();
            }
        }
        #endregion

        #region FMB02
        public static void FMB02()
        {
            JObject jDoc;
            JObject jParams;

            string receiveResult = string.Empty;

            JToken jReturnTable;
            DataTable dtReturnTable;
            string strRtnCode = string.Empty;
            string strRtnMsg = string.Empty;

            jDoc = JsonUtils.MakeBaseMessage("FMBData.Get_FMB_QueryList_CELL", "0", string.Empty, "9012C");
            JsonUtils.AddTextNode(jDoc, "//message/header", "fmbmode", "Y");

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME", "CELL");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID", "FMBData.Get_FMB_QueryList_CELL");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE", "0");

            jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            JsonUtils.AddChildTextNode(jParams, "siteid", "9012C");
            JsonUtils.AddChildTextNode(jParams, "queryid", "Get_FMB_QueryList_FM");

            receiveResult = string.Empty;
            TibcoService.SendTibcoMessage_FMB(null, jDoc.ToString(), ref receiveResult, true);

            jReceiveResult_FMB02 = (JObject)JsonConvert.DeserializeObject(receiveResult);

            jReturnTable = JsonUtils.GetNode(jReceiveResult_FMB02, "//message/return/returnTable");
            dtReturnTable = JsonUtils.ConvertJsonToDataTable(jReturnTable);
            strRtnCode = JsonUtils.GetNodeText(jReceiveResult_FMB02, "//message/return/returncode");
            strRtnMsg = JsonUtils.GetNodeText(jReceiveResult_FMB02, "//message/return/returnmessage");

            if (strRtnCode == "1")
            {
                // Tibco ReConnection
                TibcoService.TIBCO_Open();
                TibcoService.FMB_Conn();
            }
        }
        #endregion

        #region FMB03
        public static void FMB03()
        {
            JObject jDoc;
            JObject jParams;

            string receiveResult = string.Empty;

            JToken jReturnTable;
            DataTable dtReturnTable;
            string strRtnCode = string.Empty;
            string strRtnMsg = string.Empty;

            jDoc = JsonUtils.MakeBaseMessage("FMBData.Get_FMB_QueryList_MODULE", "0", string.Empty, "9012C");
            JsonUtils.AddTextNode(jDoc, "//message/header", "fmbmode", "Y");

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME", "MODULE");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID", "FMBData.Get_FMB_QueryList_MODULE");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE", "0");

            jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            JsonUtils.AddChildTextNode(jParams, "siteid", "9012C");
            JsonUtils.AddChildTextNode(jParams, "queryid", "Get_FMB_QueryList_FM");

            receiveResult = string.Empty;
            TibcoService.SendTibcoMessage_FMB(null, jDoc.ToString(), ref receiveResult, true);

            jReceiveResult_FMB03 = (JObject)JsonConvert.DeserializeObject(receiveResult);

            jReturnTable = JsonUtils.GetNode(jReceiveResult_FMB03, "//message/return/returnTable");
            dtReturnTable = JsonUtils.ConvertJsonToDataTable(jReturnTable);
            strRtnCode = JsonUtils.GetNodeText(jReceiveResult_FMB03, "//message/return/returncode");
            strRtnMsg = JsonUtils.GetNodeText(jReceiveResult_FMB03, "//message/return/returnmessage");

            if (strRtnCode == "1")
            {
                // Tibco ReConnection
                TibcoService.TIBCO_Open();
                TibcoService.FMB_Conn();
            }
        }
        #endregion
    }
}