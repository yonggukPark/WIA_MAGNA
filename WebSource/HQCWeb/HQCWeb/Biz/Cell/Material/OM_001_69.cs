using HQC.FW.Common;
using HQCWeb.FMB_FW;
using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace HQCWeb.Biz.Cell.Material
{
    public class OM_001_69
    {
        JsonParse jp = new JsonParse();

        BasePage bp = new BasePage();

        public string strPlant = string.Empty;

        //string strTargetName = System.Configuration.ConfigurationSettings.AppSettings["SUBJECT_NM_WEB"].ToString();
        string strTargetName = System.Configuration.ConfigurationManager.AppSettings.Get("SUBJECT_NM_WEB");

        #region OM_001_69
        public OM_001_69()
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

            jDoc = JsonUtils.MakeBaseMessage("OM_001_69.Get_EquipmentList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "OM_001_69.Get_EquipmentList");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",   "0");        // 0:조회,1:입력,2:수정,3:삭제

            jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            JsonUtils.AddChildTextNode(jParams, "siteid",               pValue[0]);
            JsonUtils.AddChildTextNode(jParams, "processsegmentid",     pValue[1]);

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

        #region GetScreenReplaceTitleList
        public DataSet GetScreenReplaceTitleList()
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

            jDoc = JsonUtils.MakeBaseMessage("OM_001_69.Get_ScreenReplaceTitleList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "OM_001_69.Get_ScreenReplaceTitleList");
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

        #region GetLifetimeTitleList
        public DataSet GetLifetimeTitleList()
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

            jDoc = JsonUtils.MakeBaseMessage("OM_001_69.Get_LifetimeTitleList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "OM_001_69.Get_LifetimeTitleList");
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

        #region GetScreenReplaceList
        public DataSet GetScreenReplaceList(string[] pValue)
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

            jDoc = JsonUtils.MakeBaseMessage("OM_001_69.Get_ScreenReplaceList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "OM_001_69.Get_ScreenReplaceList");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

            jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            JsonUtils.AddChildTextNode(jParams, "eqpid",        pValue[0]);
            JsonUtils.AddChildTextNode(jParams, "startdate",    pValue[1]);
            JsonUtils.AddChildTextNode(jParams, "enddate",      pValue[2]);

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

        #region GetLifetimeList
        public DataSet GetLifetimeList(string[] pValue)
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

            jDoc = JsonUtils.MakeBaseMessage("OM_001_69.Get_LifetimeList", "0", string.Empty);

            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME", strPlant);     // DB이름 : CELL, MODULE 둘중 한가지 입력
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID", "OM_001_69.Get_LifetimeList");
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE", "0");        // 0:조회,1:입력,2:수정,3:삭제

            jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
            JsonUtils.AddChildTextNode(jParams, "eqpid",        pValue[0]);
            JsonUtils.AddChildTextNode(jParams, "startdate",    pValue[1]);
            JsonUtils.AddChildTextNode(jParams, "enddate",      pValue[2]);

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