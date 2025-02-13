using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Web.Services;

using MES.FW.Common.CommonMgt;
using System.Text;

namespace HQCWeb
{
    public partial class Main : System.Web.UI.Page
    {
        string strDBName            = string.Empty;
        string strQueryID           = string.Empty;
        string strErrMessage        = string.Empty;
        public string strWebIpPort  = string.Empty;

        BasePage bp = new BasePage();

        protected void Page_Load(object sender, EventArgs e)
        {
            strWebIpPort = System.Configuration.ConfigurationManager.AppSettings.Get("WEB_IP_PORT");

            GetData();
        }

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            strDBName = "GPDB";
            strQueryID = "WidgetData.Get_UserByWidgetInfo";

            // 비지니스 클래스 작성
            Biz.SystemManagement.WidgetMgt biz = new Biz.SystemManagement.WidgetMgt();

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("USER_ID",       bp.g_userid.ToString());
            sParam.Add("LogFlag",       "N");
            //sParam.Add("CUR_MENU_ID",   "WEB_00130");        // 조회페이지 메뉴 아이디 입력-에러로그 생성시 메뉴 아이디 필요

            // 비지니스 메서드 호출
            ds = biz.GetUserByWidgetInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strErrMessage = ds.Tables[0].Rows[0][1].ToString();

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_ErrorMessage('" + strErrMessage + "');", true);
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string strWidget01Url = string.Empty;
                        string strWidget02Url = string.Empty;
                        string strWidget03Url = string.Empty;
                        string strWidget04Url = string.Empty;
                        string strWidget05Url = string.Empty;

                        string strWidget06Url = string.Empty;
                        string strWidget07Url = string.Empty;
                        string strWidget08Url = string.Empty;
                        string strWidget09Url = string.Empty;

                        string strWidget10Url = string.Empty;
                        string strWidget11Url = string.Empty;
                        string strWidget12Url = string.Empty;
                        string strWidget13Url = string.Empty;
                        string strWidget14Url = string.Empty;
                        string strWidget15Url = string.Empty;

                        string strFrameUrl = string.Empty;

                        strWidget01Url = ds.Tables[0].Rows[0]["WIDGET_01_URL"].ToString();
                        strWidget02Url = ds.Tables[0].Rows[0]["WIDGET_02_URL"].ToString();
                        strWidget03Url = ds.Tables[0].Rows[0]["WIDGET_03_URL"].ToString();
                        strWidget04Url = ds.Tables[0].Rows[0]["WIDGET_04_URL"].ToString();
                        strWidget05Url = ds.Tables[0].Rows[0]["WIDGET_05_URL"].ToString();

                        strWidget06Url = ds.Tables[0].Rows[0]["WIDGET_06_URL"].ToString();
                        strWidget07Url = ds.Tables[0].Rows[0]["WIDGET_07_URL"].ToString();
                        strWidget08Url = ds.Tables[0].Rows[0]["WIDGET_08_URL"].ToString();
                        strWidget09Url = ds.Tables[0].Rows[0]["WIDGET_09_URL"].ToString();

                        strWidget10Url = ds.Tables[0].Rows[0]["WIDGET_10_URL"].ToString();
                        strWidget11Url = ds.Tables[0].Rows[0]["WIDGET_11_URL"].ToString();
                        strWidget12Url = ds.Tables[0].Rows[0]["WIDGET_12_URL"].ToString();
                        strWidget13Url = ds.Tables[0].Rows[0]["WIDGET_13_URL"].ToString();
                        strWidget14Url = ds.Tables[0].Rows[0]["WIDGET_14_URL"].ToString();
                        strWidget15Url = ds.Tables[0].Rows[0]["WIDGET_15_URL"].ToString();

                        strFrameUrl = ds.Tables[0].Rows[0]["MAIN_FRAME_URL"].ToString();

                        if (strFrameUrl != "")
                        {
                            var sb = new StringBuilder();
                            string versionSb = "?version=" + DateTime.Now.ToString("yyyyMMdd");
                            sb.AppendLine("function fn_FrameMainSetting() {");
                            sb.AppendLine("    $('#MainContents').load('" + strFrameUrl + "', function() {");

                            if (strWidget01Url != "") { sb.AppendLine($"        fn_WidgetMainSetting('1', '{strWidget01Url + versionSb}');"); }
                            if (strWidget02Url != "") { sb.AppendLine($"        fn_WidgetMainSetting('2', '{strWidget02Url + versionSb}');"); }
                            if (strWidget03Url != "") { sb.AppendLine($"        fn_WidgetMainSetting('3', '{strWidget03Url + versionSb}');"); }
                            if (strWidget04Url != "") { sb.AppendLine($"        fn_WidgetMainSetting('4', '{strWidget04Url + versionSb}');"); }
                            if (strWidget05Url != "") { sb.AppendLine($"        fn_WidgetMainSetting('5', '{strWidget05Url + versionSb}');"); }
                            if (strWidget06Url != "") { sb.AppendLine($"        fn_WidgetMainSetting('6', '{strWidget06Url + versionSb}');"); }
                            if (strWidget07Url != "") { sb.AppendLine($"        fn_WidgetMainSetting('7', '{strWidget07Url + versionSb}');"); }
                            if (strWidget08Url != "") { sb.AppendLine($"        fn_WidgetMainSetting('8', '{strWidget08Url + versionSb}');"); }
                            if (strWidget09Url != "") { sb.AppendLine($"        fn_WidgetMainSetting('9', '{strWidget09Url + versionSb}');"); }

                            if (strWidget10Url != "") { sb.AppendLine($"        fn_WidgetMainSetting('10', '{strWidget10Url + versionSb}');"); }
                            if (strWidget11Url != "") { sb.AppendLine($"        fn_WidgetMainSetting('11', '{strWidget11Url + versionSb}');"); }
                            if (strWidget12Url != "") { sb.AppendLine($"        fn_WidgetMainSetting('12', '{strWidget12Url + versionSb}');"); }
                            if (strWidget13Url != "") { sb.AppendLine($"        fn_WidgetMainSetting('13', '{strWidget13Url + versionSb}');"); }
                            if (strWidget14Url != "") { sb.AppendLine($"        fn_WidgetMainSetting('14', '{strWidget14Url + versionSb}');"); }
                            if (strWidget15Url != "") { sb.AppendLine($"        fn_WidgetMainSetting('15', '{strWidget15Url + versionSb}');"); }

                            sb.AppendLine("    });");
                            sb.AppendLine("}; fn_FrameMainSetting(); setInterval(fn_FrameMainSetting, 60000); ");

                            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), sb.ToString(), true);
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "createGrid();", true);

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Faile", "fn_NoData();", true);
            }
        }
        #endregion
        
        #region SetMenuAccess
        [WebMethod]
        public static void SetMenuAccess(string sParams)
        {
            BasePage bp = new BasePage();

            string strRtn = string.Empty;
            
            string strDBName = string.Empty;
            string strQueryID = string.Empty;
            
            int iRtn = 0;

            strDBName = "GPDB";
            strQueryID = "CUDData.Set_MESACCESS_Data";

            Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();
            FW.Data.Parameters sParam = new FW.Data.Parameters();

            sParam.Add("MENU_ID",   sParams);
            sParam.Add("REG_ID",    bp.g_userid.ToString());
            //sParam.Add("REG_ID", "JYJ");
            sParam.Add("LogFlag",   "N");

            iRtn = biz.SetMenuAccess(strDBName, strQueryID, sParam);

        }
        #endregion

    }
}