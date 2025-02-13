using HQC.FW.Common;
using HQCWeb.FMB_FW;
using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;

namespace HQCWeb.Biz
{
    public class Mgt_Biz
    {
        //JsonParse jp = new JsonParse();

        //BasePage bp = new BasePage();

        //public string strPlant = string.Empty;
        //public Mgt_Biz() {
        //    strPlant = bp.g_plant;

        //    if (bp.g_plant == "9012C")
        //    {
        //        strPlant = "GPDB";
        //    }
        //    else {
        //        strPlant = "MODULE";
        //    }
        //}

        //#region GetMenuInfoList_Module
        //public DataSet GetMenuInfoList_Module()
        //{
        //    DataSet dsSampleList = new DataSet();
        //    DataTable dt = new DataTable();

        //    JObject jDoc;
        //    JObject jParams;

        //    string receiveResult = string.Empty;
        //    string receiveFlag = string.Empty;

        //    JObject jReceiveResult;
        //    JToken jReturnTable;
        //    DataTable dtReturnTable;
        //    string strRtnCode = string.Empty;
        //    string strRtnMsg = string.Empty;

        //    jDoc = JsonUtils.MakeBaseMessage("MenuData.Get_Menus", "0", string.Empty);

        //    JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     "MODULE");     // DB이름 : CELL, MODULE 둘중 한가지 입력
        //    JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "MenuData.Get_Menus");
        //    JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

        //    jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
        //    JsonUtils.AddChildTextNode(jParams, "menuid", "");
        //    JsonUtils.AddChildTextNode(jParams, "menunm", "");

        //    receiveResult = string.Empty;
        //    TibcoService.SendTibcoMessage_WEB("HQC.JCC2.WEB.JYJ", jDoc.ToString(), ref receiveResult, true);  // 호출 대상 서비스이름

        //    jReceiveResult = (JObject)JsonConvert.DeserializeObject(receiveResult);
        //    jReturnTable = JsonUtils.GetNode(jReceiveResult, "//message/return/returnTable");
        //    dtReturnTable = JsonUtils.ConvertJsonToDataTable(jReturnTable);
        //    strRtnCode = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returncode");
        //    strRtnMsg = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returnmessage");

        //    if (strRtnCode == "1" || strRtnCode == "")
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

        //#region GetMenuInfoList_Cell
        //public DataSet GetMenuInfoList_Cell()
        //{
        //    DataSet dsSampleList = new DataSet();
        //    DataTable dt = new DataTable();

        //    JObject jDoc;
        //    JObject jParams;

        //    string receiveResult = string.Empty;
        //    string receiveFlag = string.Empty;

        //    JObject jReceiveResult;
        //    JToken jReturnTable;
        //    DataTable dtReturnTable;
        //    string strRtnCode = string.Empty;
        //    string strRtnMsg = string.Empty;

        //    jDoc = JsonUtils.MakeBaseMessage("MenuData.Get_Menus", "0", string.Empty);

        //    JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME",     "GPDB");     // DB이름 : CELL, MODULE 둘중 한가지 입력
        //    JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID",    "MenuData.Get_Menus");
        //    JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE",  "0");        // 0:조회,1:입력,2:수정,3:삭제

        //    jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");
        //    JsonUtils.AddChildTextNode(jParams, "menuid", "");
        //    JsonUtils.AddChildTextNode(jParams, "menunm", "");

        //    receiveResult = string.Empty;
        //    TibcoService.SendTibcoMessage_WEB("HQC.JCC2.WEB.JYJ", jDoc.ToString(), ref receiveResult, true);  // 호출 대상 서비스이름

        //    jReceiveResult = (JObject)JsonConvert.DeserializeObject(receiveResult);
        //    jReturnTable = JsonUtils.GetNode(jReceiveResult, "//message/return/returnTable");
        //    dtReturnTable = JsonUtils.ConvertJsonToDataTable(jReturnTable);
        //    strRtnCode = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returncode");
        //    strRtnMsg = JsonUtils.GetNodeText(jReceiveResult, "//message/return/returnmessage");

        //    if (strRtnCode == "1" || strRtnCode == "")
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
    }
}