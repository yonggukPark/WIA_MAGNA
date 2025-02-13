using HQC.FW.Common;
using HQCWeb.FMB_FW;
using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace HQCWeb.Biz.Cell.Alarm
{
    public class EQ_013_01
    {
        JsonParse jp = new JsonParse();

        BasePage bp = new BasePage();

        public string strPlant = string.Empty;

        //string strTargetName = System.Configuration.ConfigurationSettings.AppSettings["SUBJECT_NM_WEB"].ToString();
        string strTargetName = System.Configuration.ConfigurationManager.AppSettings.Get("SUBJECT_NM_WEB");

        #region EQ_013_01
        public EQ_013_01()
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

            jDoc = JsonUtils.MakeBaseMessage("EQ_013_01.Get_OperationList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "EQ_013_01.Get_OperationList");
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

        #region GetEQPTypeList
        public DataSet GetEQPTypeList()
        {
            DataSet dsList = new DataSet();
            DataTable dt = new DataTable();

            JObject jDoc;
            
            string receiveResult = string.Empty;
            string receiveFlag = string.Empty;

            JObject jReceiveResult;
            JToken jReturnTable;
            DataTable dtReturnTable;
            string strRtnCode = string.Empty;
            string strRtnMsg = string.Empty;

            jDoc = JsonUtils.MakeBaseMessage("EQ_013_01.Get_EQPTypeList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "EQ_013_01.Get_EQPTypeList");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

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

        #region GetAlarmTypeList
        public DataSet GetAlarmTypeList()
        {
            DataSet dsList = new DataSet();
            DataTable dt = new DataTable();

            JObject jDoc;
            
            string receiveResult = string.Empty;
            string receiveFlag = string.Empty;

            JObject jReceiveResult;
            JToken jReturnTable;
            DataTable dtReturnTable;
            string strRtnCode = string.Empty;
            string strRtnMsg = string.Empty;

            jDoc = JsonUtils.MakeBaseMessage("EQ_013_01.Get_AlarmTypeList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "EQ_013_01.Get_AlarmTypeList");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

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

        #region GetAlarmLevelList
        public DataSet GetAlarmLevelList()
        {
            DataSet dsList = new DataSet();
            DataTable dt = new DataTable();

            JObject jDoc;

            string receiveResult = string.Empty;
            string receiveFlag = string.Empty;

            JObject jReceiveResult;
            JToken jReturnTable;
            DataTable dtReturnTable;
            string strRtnCode = string.Empty;
            string strRtnMsg = string.Empty;

            jDoc = JsonUtils.MakeBaseMessage("EQ_013_01.Get_AlarmLevelList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "EQ_013_01.Get_AlarmLevelList");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

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

        #region GetAlarmGradeList
        public DataSet GetAlarmGradeList()
        {
            DataSet dsList = new DataSet();
            DataTable dt = new DataTable();

            JObject jDoc;

            string receiveResult = string.Empty;
            string receiveFlag = string.Empty;

            JObject jReceiveResult;
            JToken jReturnTable;
            DataTable dtReturnTable;
            string strRtnCode = string.Empty;
            string strRtnMsg = string.Empty;

            jDoc = JsonUtils.MakeBaseMessage("EQ_013_01.Get_AlarmGradeList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "EQ_013_01.Get_AlarmGradeList");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

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

        #region GetAlarmCommonList
        public DataSet GetAlarmCommonList()
        {
            DataSet dsList = new DataSet();
            DataTable dt = new DataTable();

            JObject jDoc;

            string receiveResult = string.Empty;
            string receiveFlag = string.Empty;

            JObject jReceiveResult;
            JToken jReturnTable;
            DataTable dtReturnTable;
            string strRtnCode = string.Empty;
            string strRtnMsg = string.Empty;

            jDoc = JsonUtils.MakeBaseMessage("EQ_013_01.Get_AlarmCommonList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "EQ_013_01.Get_AlarmCommonList");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

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

        #region GetAlarmHeadList
        public DataSet GetAlarmHeadList()
        {
            DataSet dsList = new DataSet();
            DataTable dt = new DataTable();

            JObject jDoc;

            string receiveResult = string.Empty;
            string receiveFlag = string.Empty;

            JObject jReceiveResult;
            JToken jReturnTable;
            DataTable dtReturnTable;
            string strRtnCode = string.Empty;
            string strRtnMsg = string.Empty;

            jDoc = JsonUtils.MakeBaseMessage("EQ_013_01.Get_AlarmHeadList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "EQ_013_01.Get_AlarmHeadList");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

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

        #region GetAlarmCameraList
        public DataSet GetAlarmCameraList()
        {
            DataSet dsList = new DataSet();
            DataTable dt = new DataTable();

            JObject jDoc;

            string receiveResult = string.Empty;
            string receiveFlag = string.Empty;

            JObject jReceiveResult;
            JToken jReturnTable;
            DataTable dtReturnTable;
            string strRtnCode = string.Empty;
            string strRtnMsg = string.Empty;

            jDoc = JsonUtils.MakeBaseMessage("EQ_013_01.Get_AlarmCameraList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "EQ_013_01.Get_AlarmCameraList");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

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

        #region GetComCodeList
        public DataSet GetComCodeList(string pValue)
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

            jDoc = JsonUtils.MakeBaseMessage("ComCodeData.Get_ComCodeByComTypeInfo", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "ComCodeData.Get_ComCodeByComTypeInfo");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

            jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            JsonUtils.AddChildTextNode(jParams, "commtype", pValue);

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

        #region GetAlarmCountColorInfo
        public DataSet GetAlarmCountColorInfo(string[] pValue)
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

            jDoc = JsonUtils.MakeBaseMessage("EQ_013_01.Get_AlarmCountColorInfo", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "EQ_013_01.Get_AlarmCountColorInfo");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

            jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            JsonUtils.AddChildTextNode(jParams, "siteid",       pValue[0]);
            JsonUtils.AddChildTextNode(jParams, "operation",    pValue[1]);

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

        #region GetGetAlarmCountList
        public DataSet GetGetAlarmCountList(string[] pValue)
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

            jDoc = JsonUtils.MakeBaseMessage("EQ_013_01.Get_GetAlarmCountList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "EQ_013_01.Get_GetAlarmCountList");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

            jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            JsonUtils.AddChildTextNode(jParams, "fromtime",     pValue[0]);
            JsonUtils.AddChildTextNode(jParams, "totime",       pValue[1]);
            JsonUtils.AddChildTextNode(jParams, "operation",    pValue[2]);
            JsonUtils.AddChildTextNode(jParams, "alarmclass",   pValue[3]);
            JsonUtils.AddChildTextNode(jParams, "alarmtype",    pValue[4]);
            JsonUtils.AddChildTextNode(jParams, "alarmlevel",   pValue[5]);
            JsonUtils.AddChildTextNode(jParams, "alarmgrade",   pValue[6]);
            JsonUtils.AddChildTextNode(jParams, "commonalarm",  pValue[7]);
            JsonUtils.AddChildTextNode(jParams, "head",         pValue[8]);
            JsonUtils.AddChildTextNode(jParams, "camera",       pValue[9]);
            JsonUtils.AddChildTextNode(jParams, "eqptype",      pValue[10]);
            JsonUtils.AddChildTextNode(jParams, "siteid",       pValue[11]);


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