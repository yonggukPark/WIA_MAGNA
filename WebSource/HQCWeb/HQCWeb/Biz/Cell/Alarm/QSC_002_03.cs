using HQC.FW.Common;
using HQCWeb.FMB_FW;
using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Runtime.InteropServices;

namespace HQCWeb.Biz.Cell.Alarm
{
    public class QSC_002_03
    {
        JsonParse jp = new JsonParse();

        BasePage bp = new BasePage();

        public string strPlant = string.Empty;

        //string strTargetName = System.Configuration.ConfigurationSettings.AppSettings["SUBJECT_NM_WEB"].ToString();
        string strTargetName = System.Configuration.ConfigurationManager.AppSettings.Get("SUBJECT_NM_WEB");

        #region BandAlarmMgt
        public QSC_002_03()
        {
            //strPlant = bp.g_plant;

            //if (bp.g_plant == "9012C")
            //{
            //    strPlant = "CELL";
            //}
            //else
            //{
            //    strPlant = "MODULE";
            //}

            strPlant = "CELL";

        }
        #endregion

        #region GetStandardList
        public DataSet GetStandardList(string[] pValue)
        {
            DataSet dsList = new DataSet();
            DataTable dt = new DataTable();

            JObject jDoc;
            JObject jParams;

            string receiveResult = string.Empty;
            string receiveFlag = string.Empty;

            JObject jReceiveResult;
            JToken jReturnTable;
            DataTable dtReturnTable;
            string strRtnCode = string.Empty;
            string strRtnMsg = string.Empty;

            jDoc = JsonUtils.MakeBaseMessage("QSC_002_03.Get_StandardList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME", strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "QSC_002_03.Get_StandardList");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

            jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            JsonUtils.AddChildTextNode(jParams, "siteid",   pValue[0]);


            receiveResult = string.Empty;
            TibcoService.SendTibcoMessage_WEB(strTargetName, jDoc.ToString(), ref receiveResult, true);  // 호출 대상 서비스이름

            jReceiveResult = (JObject)JsonConvert.DeserializeObject(receiveResult);
            jReturnTable = JsonUtils.GetNode(jReceiveResult, "//message/return/returnTable");
            dtReturnTable = JsonUtils.ConvertJsonToDataTable(jReturnTable);
            strRtnCode = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returncode");
            strRtnMsg = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returnmessage");

            if (strRtnCode == "")
            {
                TibcoService.TIBCO_Open();
                TibcoService.WEB_Conn();

                dt = new DataTable();
                dt.Columns.Add("ERROR");
                dt.Rows.Add("Tibco Service Error");
                dsList.Tables.Add(dt);
            }
            else
            {
                if (strRtnCode == "0")
                {
                    dt = jp.SetDataListParse(receiveResult);

                    if (dt != null)
                    {
                        dsList.Tables.Add(dt);
                    }
                }
            }

            return dsList;
        }
        #endregion

        #region GetOperationList
        public DataSet GetOperationList(string[] pValue)
        {
            DataSet dsList = new DataSet();
            DataTable dt = new DataTable();

            JObject jDoc;
            JObject jParams;

            string receiveResult = string.Empty;
            string receiveFlag = string.Empty;

            JObject jReceiveResult;
            JToken jReturnTable;
            DataTable dtReturnTable;
            string strRtnCode = string.Empty;
            string strRtnMsg = string.Empty;

            jDoc = JsonUtils.MakeBaseMessage("QSC_002_03.Get_OperationList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "QSC_002_03.Get_OperationList");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

            jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            JsonUtils.AddChildTextNode(jParams, "siteid", pValue[0]);

            receiveResult = string.Empty;
            TibcoService.SendTibcoMessage_WEB(strTargetName, jDoc.ToString(), ref receiveResult, true);  // 호출 대상 서비스이름

            jReceiveResult = (JObject)JsonConvert.DeserializeObject(receiveResult);
            jReturnTable = JsonUtils.GetNode(jReceiveResult, "//message/return/returnTable");
            dtReturnTable = JsonUtils.ConvertJsonToDataTable(jReturnTable);
            strRtnCode = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returncode");
            strRtnMsg = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returnmessage");

            if (strRtnCode == "")
            {
                TibcoService.TIBCO_Open();
                TibcoService.WEB_Conn();

                dt = new DataTable();
                dt.Columns.Add("ERROR");
                dt.Rows.Add("Tibco Service Error");
                dsList.Tables.Add(dt);
            }
            else
            {
                if (strRtnCode == "0")
                {
                    dt = jp.SetDataListParse(receiveResult);

                    if (dt != null)
                    {
                        dsList.Tables.Add(dt);
                    }
                }
            }

            return dsList;
        }
        #endregion

        #region GetEquipmentList
        public DataSet GetEquipmentList(string[] pValue)
        {
            DataSet dsList = new DataSet();
            DataTable dt = new DataTable();

            JObject jDoc;
            JObject jParams;

            string receiveResult = string.Empty;
            string receiveFlag = string.Empty;

            JObject jReceiveResult;
            JToken jReturnTable;
            DataTable dtReturnTable;
            string strRtnCode = string.Empty;
            string strRtnMsg = string.Empty;

            jDoc = JsonUtils.MakeBaseMessage("QSC_002_03.Get_EquipmentList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "QSC_002_03.Get_EquipmentList");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

            jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            JsonUtils.AddChildTextNode(jParams, "siteid", pValue[0]);

            receiveResult = string.Empty;
            TibcoService.SendTibcoMessage_WEB(strTargetName, jDoc.ToString(), ref receiveResult, true);  // 호출 대상 서비스이름


            jReceiveResult = (JObject)JsonConvert.DeserializeObject(receiveResult);
            jReturnTable = JsonUtils.GetNode(jReceiveResult, "//message/return/returnTable");
            dtReturnTable = JsonUtils.ConvertJsonToDataTable(jReturnTable);
            strRtnCode = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returncode");
            strRtnMsg = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returnmessage");

            if (strRtnCode == "")
            {
                TibcoService.TIBCO_Open();
                TibcoService.WEB_Conn();

                dt = new DataTable();
                dt.Columns.Add("ERROR");
                dt.Rows.Add("Tibco Service Error");
                dsList.Tables.Add(dt);
            }
            else
            {
                if (strRtnCode == "0")
                {
                    dt = jp.SetDataListParse(receiveResult);

                    if (dt != null)
                    {
                        dsList.Tables.Add(dt);
                    }
                }
            }

            return dsList;
        }
        #endregion

        #region GetQCSAlarmList
        public DataSet GetQCSAlarmList(string[] pValue)
        {
            DataSet dsList = new DataSet();
            DataTable dt = new DataTable();

            JObject jDoc;
            JObject jParams;

            string receiveResult = string.Empty;
            string receiveFlag = string.Empty;

            JObject jReceiveResult;
            JToken jReturnTable;
            DataTable dtReturnTable;
            string strRtnCode = string.Empty;
            string strRtnMsg = string.Empty;

            jDoc = JsonUtils.MakeBaseMessage("QSC_002_03.Get_BandSendList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "QSC_002_03.Get_BandSendList");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

            jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            JsonUtils.AddChildTextNode(jParams, "alarmtype",        pValue[0]);
            JsonUtils.AddChildTextNode(jParams, "equipmentid",      pValue[1]);
            JsonUtils.AddChildTextNode(jParams, "alarmsourceid",    pValue[2]);
            JsonUtils.AddChildTextNode(jParams, "siteid",           pValue[3]);
            JsonUtils.AddChildTextNode(jParams, "fromdate",         pValue[4]);
            JsonUtils.AddChildTextNode(jParams, "todate",           pValue[5]);


            receiveResult = string.Empty;
            TibcoService.SendTibcoMessage_WEB(strTargetName, jDoc.ToString(), ref receiveResult, true);  // 호출 대상 서비스이름

            jReceiveResult = (JObject)JsonConvert.DeserializeObject(receiveResult);
            jReturnTable = JsonUtils.GetNode(jReceiveResult, "//message/return/returnTable");
            dtReturnTable = JsonUtils.ConvertJsonToDataTable(jReturnTable);
            strRtnCode = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returncode");
            strRtnMsg = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returnmessage");

            if (strRtnCode == "")
            {
                TibcoService.TIBCO_Open();
                TibcoService.WEB_Conn();

                dt = new DataTable();
                dt.Columns.Add("ERROR");
                dt.Rows.Add("Tibco Service Error");
                dsList.Tables.Add(dt);
            }
            else
            {
                if (strRtnCode == "0")
                {
                    dt = jp.SetDataListParse(receiveResult);

                    if (dt != null)
                    {
                        dsList.Tables.Add(dt);
                    }
                }
            }

            return dsList;
        }
        #endregion
    }
}