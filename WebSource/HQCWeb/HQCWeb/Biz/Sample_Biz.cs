using System.Data;
using Newtonsoft.Json.Linq;
using HQC.FW.Common;
using HQCWeb.FMB_FW;
using HQCWeb.FW;
using HQCWeb.FW.Data;
using HQCWeb.FW.Rule;
using Newtonsoft.Json;

namespace HQCWeb.Biz
{
	public class Sample_Biz
	{
        //JsonParse jp = new JsonParse();

        SqlMapper Mapper = null;

        MesRuleBase MRB = new MesRuleBase();

        //#region GetSampleList
        //public DataSet GetSampleList(string[] pValue)
        //{
        //    DataSet dsSampleList = new DataSet();

        //    DataTable dt = new DataTable();

        //    string strQuery = string.Empty;
        //    string strQuery2 = string.Empty;

        //    JObject jDoc;
        //    JObject jParams;

        //    string receiveResult = string.Empty;
        //    string receiveFlag = string.Empty;

        //    JObject jReceiveResult;
        //    JToken jReturnTable;
        //    DataTable dtReturnTable;
        //    string strRtnCode = string.Empty;
        //    string strRtnMsg = string.Empty;

        //    jDoc = JsonUtils.MakeBaseMessage("UserInfoData.Get_UserInfoList", "0", string.Empty);

        //    JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME", "DEVMESDB");
        //    JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID", "UserInfoData.Get_UserInfoList");
        //    JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE", "0");

        //    jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
        //    JsonUtils.AddChildTextNode(jParams, "userid", pValue[0]);
        //    JsonUtils.AddChildTextNode(jParams, "startrow", pValue[1]);
        //    JsonUtils.AddChildTextNode(jParams, "endrow", pValue[2]);

        //    receiveResult = string.Empty;
        //    TibcoService.SendTibcoMessage_WEB("HQC.JCC2.WEB.JYJ", jDoc.ToString(), ref receiveResult, true);

        //    jReceiveResult = (JObject)JsonConvert.DeserializeObject(receiveResult);
        //    jReturnTable = JsonUtils.GetNode(jReceiveResult, "//message/return/returnTable");
        //    dtReturnTable = JsonUtils.ConvertJsonToDataTable(jReturnTable);
        //    strRtnCode = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returncode");
        //    strRtnMsg = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returnmessage");

        //    if (strRtnCode == "1")
        //    {
        //        TibcoService.TIBCO_Open();
        //        TibcoService.WEB_Conn();
        //    }
        //    else {
        //        dt = jp.SetDataListParse(receiveResult);

        //        if (dt != null)
        //        {
        //            dsSampleList.Tables.Add(dt);
        //        }
        //        else
        //        {
        //            TibcoService.TIBCO_Open();
        //            TibcoService.WEB_Conn();
        //        }
        //    }

        //    return dsSampleList;
        //}
        //#endregion

        //#region GetSampleExcelList
        //public DataSet GetSampleExcelList(string[] pValue)
        //{
        //    DataSet dsSampleList = new DataSet();

        //    DataTable dt = new DataTable();

        //    string strQuery = string.Empty;
        //    string strQuery2 = string.Empty;

        //    JObject jDoc;
        //    JObject jParams;

        //    string receiveResult = string.Empty;
        //    string receiveFlag = string.Empty;

        //    JObject jReceiveResult;
        //    JToken jReturnTable;
        //    DataTable dtReturnTable;
        //    string strRtnCode = string.Empty;
        //    string strRtnMsg = string.Empty;

        //    jDoc = JsonUtils.MakeBaseMessage("UserInfoData.Get_UserInfoExcelList", "0", string.Empty);

        //    JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME", "DEVMESDB");
        //    JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID", "UserInfoData.Get_UserInfoExcelList");
        //    JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE", "0");

        //    jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
        //    JsonUtils.AddChildTextNode(jParams, "userid", pValue[0]);

        //    receiveResult = string.Empty;
        //    TibcoService.SendTibcoMessage_WEB("HQC.JCC2.WEB.JYJ", jDoc.ToString(), ref receiveResult, true);

        //    jReceiveResult = (JObject)JsonConvert.DeserializeObject(receiveResult);
        //    jReturnTable = JsonUtils.GetNode(jReceiveResult, "//message/return/returnTable");
        //    dtReturnTable = JsonUtils.ConvertJsonToDataTable(jReturnTable);
        //    strRtnCode = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returncode");
        //    strRtnMsg = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returnmessage");

        //    if (strRtnCode == "1")
        //    {
        //        TibcoService.TIBCO_Open();
        //        TibcoService.WEB_Conn();
        //    }
        //    else
        //    {
        //        dt = jp.SetDataListParse(receiveResult);

        //        if (dt != null)
        //        {
        //            dsSampleList.Tables.Add(dt);
        //        }
        //        else
        //        {
        //            TibcoService.TIBCO_Open();
        //            TibcoService.WEB_Conn();
        //        }
        //    }

        //    return dsSampleList;
        //}
        //#endregion
                
        //#region GetTitleInfo
        //public DataSet GetTitleInfo()
        //{
        //    DataSet dsSampleList = new DataSet();

        //    DataTable dt = new DataTable();

        //    string strQuery = string.Empty;
        //    string strQuery2 = string.Empty;

        //    JObject jDoc;
            
        //    string receiveResult = string.Empty;
        //    string receiveFlag = string.Empty;

        //    JObject jReceiveResult;
        //    JToken jReturnTable;
        //    DataTable dtReturnTable;
        //    string strRtnCode = string.Empty;
        //    string strRtnMsg = string.Empty;

        //    jDoc = JsonUtils.MakeBaseMessage("MultiRowData.Get_TitleInfo", "0", string.Empty);

        //    JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME", "DEVMESDB");
        //    JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID", "MultiRowData.Get_TitleInfo");
        //    JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE", "0");

        //    receiveResult = string.Empty;
        //    TibcoService.SendTibcoMessage_WEB("HQC.JCC2.WEB.JYJ", jDoc.ToString(), ref receiveResult, true);

        //    jReceiveResult = (JObject)JsonConvert.DeserializeObject(receiveResult);
        //    jReturnTable = JsonUtils.GetNode(jReceiveResult, "//message/return/returnTable");
        //    dtReturnTable = JsonUtils.ConvertJsonToDataTable(jReturnTable);
        //    strRtnCode = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returncode");
        //    strRtnMsg = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returnmessage");

        //    if (strRtnCode == "0")
        //    {
        //        TibcoService.TIBCO_Open();
        //        TibcoService.WEB_Conn();

        //        dt = new DataTable();
        //        dt.Columns.Add("MSG");
        //        dt.Rows.Add("R");

        //        dsSampleList.Tables.Add(dt);

        //    }
        //    else {
        //        dt = jp.SetDataListParse(receiveResult);

        //        if (dt != null)
        //        {
        //            dsSampleList.Tables.Add(dt);
        //        }
        //        else
        //        {
        //            TibcoService.TIBCO_Open();
        //            TibcoService.WEB_Conn();
        //        }
        //    }

        //    return dsSampleList;
        //}
        //#endregion
        
        #region GetList
        public DataSet GetList(string strDBName, string strQueryID, Parameters sParam)
        {
            DataSet dsList = new DataSet();

            DataTable dt = new DataTable();

            Mapper = DataBaseService.mappers[strDBName];
            //Mapper = DataBaseService.mappers["GPDB"];

            dt = MRB.GetSearchQueryResult(Mapper, strQueryID, sParam);

            dsList.Tables.Add(dt);

            return dsList;
        }
        #endregion


    }
}