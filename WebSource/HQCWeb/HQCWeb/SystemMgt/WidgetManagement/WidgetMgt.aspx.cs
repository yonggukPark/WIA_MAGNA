using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MES.FW.Common.CommonMgt;
using HQCWeb.FMB_FW;
using HQCWeb.FW;
using MES.FW.Common.Crypt;

using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using System.Web.Services;
using System.Text;

namespace HQCWeb.SystemMgt.WidgetManagement
{
    public partial class WidgetMgt : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        
        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;
        public string strWidgetInfo = string.Empty;
        public string strWebIpPort = string.Empty;

        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            strWebIpPort = System.Configuration.ConfigurationManager.AppSettings.Get("WEB_IP_PORT");
            
            cy.Key = strKey;

            if (!IsPostBack)
            {
                SetCon();

                GetData();
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            strErrMessage = Message_Data.SearchDic("SearchError", bp.g_language);

            DataSet ds = new DataSet();
            
            strDBName = "GPDB";
            strQueryID = "WidgetData.Get_ddlFrame";

            // 비지니스 클래스 작성
            Biz.SystemManagement.WidgetMgt biz = new Biz.SystemManagement.WidgetMgt();

            FW.Data.Parameters param = new FW.Data.Parameters();

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetFrameInfo(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlFrameNum.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                }
            }
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();
            string strTableName=string.Empty;
            string strMessage=string.Empty;

            strDBName="GPDB";
            strQueryID="WidgetData.Get_UserByWidgetInfo";

            // 비지니스 클래스 작성
            Biz.SystemManagement.WidgetMgt biz=new Biz.SystemManagement.WidgetMgt();

            FW.Data.Parameters sParam=new FW.Data.Parameters();
            sParam.Add("USER_ID",       bp.g_userid.ToString());

            sParam.Add("CUR_MENU_ID",   "WEB_00130");        // 조회페이지 메뉴 아이디 입력-에러로그 생성시 메뉴 아이디 필요

            // 비지니스 메서드 호출
            ds = biz.GetUserByWidgetInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count>0)
            {
                strTableName=ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strErrMessage=ds.Tables[0].Rows[0][1].ToString();
                    
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_ErrorMessage('" + strErrMessage + "');", true);
                }
                else
                {
                    if (ds.Tables[0].Rows.Count>0)
                    {
                        string strWidget01=string.Empty;
                        string strWidget02=string.Empty;
                        string strWidget03=string.Empty;
                        string strWidget04=string.Empty;
                        string strWidget05=string.Empty;

                        string strWidget06=string.Empty;
                        string strWidget07=string.Empty;
                        string strWidget08=string.Empty;
                        string strWidget09=string.Empty;

                        string strWidget10 = string.Empty;
                        string strWidget11 = string.Empty;
                        string strWidget12 = string.Empty;
                        string strWidget13 = string.Empty;
                        string strWidget14 = string.Empty;
                        string strWidget15 = string.Empty;

                        string strWidget01Url=string.Empty;
                        string strWidget02Url=string.Empty;
                        string strWidget03Url=string.Empty;
                        string strWidget04Url=string.Empty;
                        string strWidget05Url=string.Empty;
                                          
                        string strWidget06Url=string.Empty;
                        string strWidget07Url=string.Empty;
                        string strWidget08Url=string.Empty;
                        string strWidget09Url=string.Empty;

                        string strWidget10Url = string.Empty;
                        string strWidget11Url = string.Empty;
                        string strWidget12Url = string.Empty;
                        string strWidget13Url = string.Empty;
                        string strWidget14Url = string.Empty;
                        string strWidget15Url = string.Empty;

                        string strWidget01Size = string.Empty;
                        string strWidget02Size = string.Empty;
                        string strWidget03Size = string.Empty;
                        string strWidget04Size = string.Empty;
                        string strWidget05Size = string.Empty;

                        string strWidget06Size = string.Empty;
                        string strWidget07Size = string.Empty;
                        string strWidget08Size = string.Empty;
                        string strWidget09Size = string.Empty;

                        string strWidget10Size = string.Empty;
                        string strWidget11Size = string.Empty;
                        string strWidget12Size = string.Empty;
                        string strWidget13Size = string.Empty;
                        string strWidget14Size = string.Empty;
                        string strWidget15Size = string.Empty;

                        string strFrameUrl = string.Empty;

                        strWidget01=ds.Tables[0].Rows[0]["WIDGET_01"].ToString();
                        strWidget02=ds.Tables[0].Rows[0]["WIDGET_02"].ToString();
                        strWidget03=ds.Tables[0].Rows[0]["WIDGET_03"].ToString();
                        strWidget04=ds.Tables[0].Rows[0]["WIDGET_04"].ToString();
                        strWidget05=ds.Tables[0].Rows[0]["WIDGET_05"].ToString();

                        strWidget06=ds.Tables[0].Rows[0]["WIDGET_06"].ToString();
                        strWidget07=ds.Tables[0].Rows[0]["WIDGET_07"].ToString();
                        strWidget08=ds.Tables[0].Rows[0]["WIDGET_08"].ToString();
                        strWidget09=ds.Tables[0].Rows[0]["WIDGET_09"].ToString();

                        strWidget10 = ds.Tables[0].Rows[0]["WIDGET_10"].ToString();
                        strWidget11 = ds.Tables[0].Rows[0]["WIDGET_11"].ToString();
                        strWidget12 = ds.Tables[0].Rows[0]["WIDGET_12"].ToString();
                        strWidget13 = ds.Tables[0].Rows[0]["WIDGET_13"].ToString();
                        strWidget14 = ds.Tables[0].Rows[0]["WIDGET_14"].ToString();
                        strWidget15 = ds.Tables[0].Rows[0]["WIDGET_15"].ToString();

                        strWidget01Url=ds.Tables[0].Rows[0]["WIDGET_01_URL"].ToString();
                        strWidget02Url=ds.Tables[0].Rows[0]["WIDGET_02_URL"].ToString();
                        strWidget03Url=ds.Tables[0].Rows[0]["WIDGET_03_URL"].ToString();
                        strWidget04Url=ds.Tables[0].Rows[0]["WIDGET_04_URL"].ToString();
                        strWidget05Url=ds.Tables[0].Rows[0]["WIDGET_05_URL"].ToString();
                                                                        
                        strWidget06Url=ds.Tables[0].Rows[0]["WIDGET_06_URL"].ToString();
                        strWidget07Url=ds.Tables[0].Rows[0]["WIDGET_07_URL"].ToString();
                        strWidget08Url=ds.Tables[0].Rows[0]["WIDGET_08_URL"].ToString();
                        strWidget09Url=ds.Tables[0].Rows[0]["WIDGET_09_URL"].ToString();

                        strWidget10Url = ds.Tables[0].Rows[0]["WIDGET_10_URL"].ToString();
                        strWidget11Url = ds.Tables[0].Rows[0]["WIDGET_11_URL"].ToString();
                        strWidget12Url = ds.Tables[0].Rows[0]["WIDGET_12_URL"].ToString();
                        strWidget13Url = ds.Tables[0].Rows[0]["WIDGET_13_URL"].ToString();
                        strWidget14Url = ds.Tables[0].Rows[0]["WIDGET_14_URL"].ToString();
                        strWidget15Url = ds.Tables[0].Rows[0]["WIDGET_15_URL"].ToString();
                        
                        strWidget01Size = ds.Tables[0].Rows[0]["WIDGET_01_SIZE"].ToString();
                        strWidget02Size = ds.Tables[0].Rows[0]["WIDGET_02_SIZE"].ToString();
                        strWidget03Size = ds.Tables[0].Rows[0]["WIDGET_03_SIZE"].ToString();
                        strWidget04Size = ds.Tables[0].Rows[0]["WIDGET_04_SIZE"].ToString();
                        strWidget05Size = ds.Tables[0].Rows[0]["WIDGET_05_SIZE"].ToString();

                        strWidget06Size = ds.Tables[0].Rows[0]["WIDGET_06_SIZE"].ToString();
                        strWidget07Size = ds.Tables[0].Rows[0]["WIDGET_07_SIZE"].ToString();
                        strWidget08Size = ds.Tables[0].Rows[0]["WIDGET_08_SIZE"].ToString();
                        strWidget09Size = ds.Tables[0].Rows[0]["WIDGET_09_SIZE"].ToString();

                        strWidget10Size = ds.Tables[0].Rows[0]["WIDGET_10_SIZE"].ToString();
                        strWidget11Size = ds.Tables[0].Rows[0]["WIDGET_11_SIZE"].ToString();
                        strWidget12Size = ds.Tables[0].Rows[0]["WIDGET_12_SIZE"].ToString();
                        strWidget13Size = ds.Tables[0].Rows[0]["WIDGET_13_SIZE"].ToString();
                        strWidget14Size = ds.Tables[0].Rows[0]["WIDGET_14_SIZE"].ToString();
                        strWidget15Size = ds.Tables[0].Rows[0]["WIDGET_15_SIZE"].ToString();
                        
                        ddlFrameNum.SelectedValue = ds.Tables[0].Rows[0]["FRAME_NUM"].ToString();
                        strFrameUrl = ds.Tables[0].Rows[0]["MOD_FRAME_URL"].ToString();

                        if (strFrameUrl != "")
                        {
                            var sb = new StringBuilder();
                            sb.AppendLine("function fn_FrameMainSetting() {");
                            sb.AppendLine("    $('#MainContents').load('" + strFrameUrl + "', function() {");

                             sb.AppendLine($"    fn_WidgetSetting('1', '{cy.Encrypt(strWidget01)}', '{strWidget01Url}', '{cy.Encrypt(strWidget01Size)}');"); 
                             sb.AppendLine($"    fn_WidgetSetting('2', '{cy.Encrypt(strWidget02)}', '{strWidget02Url}', '{cy.Encrypt(strWidget01Size)}');"); 
                             sb.AppendLine($"    fn_WidgetSetting('3', '{cy.Encrypt(strWidget03)}', '{strWidget03Url}', '{cy.Encrypt(strWidget01Size)}');"); 
                             sb.AppendLine($"    fn_WidgetSetting('4', '{cy.Encrypt(strWidget04)}', '{strWidget04Url}', '{cy.Encrypt(strWidget01Size)}');"); 
                             sb.AppendLine($"    fn_WidgetSetting('5', '{cy.Encrypt(strWidget05)}', '{strWidget05Url}', '{cy.Encrypt(strWidget01Size)}');"); 
                             sb.AppendLine($"    fn_WidgetSetting('6', '{cy.Encrypt(strWidget06)}', '{strWidget06Url}', '{cy.Encrypt(strWidget01Size)}');"); 
                             sb.AppendLine($"    fn_WidgetSetting('7', '{cy.Encrypt(strWidget07)}', '{strWidget07Url}', '{cy.Encrypt(strWidget01Size)}');"); 
                             sb.AppendLine($"    fn_WidgetSetting('8', '{cy.Encrypt(strWidget08)}', '{strWidget08Url}', '{cy.Encrypt(strWidget01Size)}');"); 
                             sb.AppendLine($"    fn_WidgetSetting('9', '{cy.Encrypt(strWidget09)}', '{strWidget09Url}', '{cy.Encrypt(strWidget01Size)}');"); 

                             sb.AppendLine($"    fn_WidgetSetting('10', '{cy.Encrypt(strWidget10)}', '{strWidget10Url}', '{cy.Encrypt(strWidget10Size)}');");
                             sb.AppendLine($"    fn_WidgetSetting('11', '{cy.Encrypt(strWidget11)}', '{strWidget11Url}', '{cy.Encrypt(strWidget11Size)}');");
                             sb.AppendLine($"    fn_WidgetSetting('12', '{cy.Encrypt(strWidget12)}', '{strWidget12Url}', '{cy.Encrypt(strWidget12Size)}');");
                             sb.AppendLine($"    fn_WidgetSetting('13', '{cy.Encrypt(strWidget13)}', '{strWidget13Url}', '{cy.Encrypt(strWidget13Size)}');");
                             sb.AppendLine($"    fn_WidgetSetting('14', '{cy.Encrypt(strWidget14)}', '{strWidget14Url}', '{cy.Encrypt(strWidget14Size)}');");
                             sb.AppendLine($"    fn_WidgetSetting('15', '{cy.Encrypt(strWidget15)}', '{strWidget15Url}', '{cy.Encrypt(strWidget15Size)}');");

                            sb.AppendLine("    });");
                            sb.AppendLine("}; fn_FrameMainSetting();");

                            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), sb.ToString(), true);
                        }
                    }
                    else
                    {
                        
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

        #region ddlFrameNum_SelectedIndexChanged
        protected void ddlFrameNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            strDBName = "GPDB";
            strQueryID = "WidgetData.Get_WidgetByFrameInfo";

            // 비지니스 클래스 작성
            Biz.SystemManagement.WidgetMgt biz = new Biz.SystemManagement.WidgetMgt();

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("USER_ID", bp.g_userid.ToString());
            sParam.Add("FRAME_NUM", ddlFrameNum.SelectedValue);

            sParam.Add("CUR_MENU_ID", "WEB_00130");        // 조회페이지 메뉴 아이디 입력-에러로그 생성시 메뉴 아이디 필요

            // 비지니스 메서드 호출
            ds = biz.GetFrameInfo(strDBName, strQueryID, sParam);

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
                        string strWidget01 = string.Empty;
                        string strWidget02 = string.Empty;
                        string strWidget03 = string.Empty;
                        string strWidget04 = string.Empty;
                        string strWidget05 = string.Empty;

                        string strWidget06 = string.Empty;
                        string strWidget07 = string.Empty;
                        string strWidget08 = string.Empty;
                        string strWidget09 = string.Empty;

                        string strWidget10 = string.Empty;
                        string strWidget11 = string.Empty;
                        string strWidget12 = string.Empty;
                        string strWidget13 = string.Empty;
                        string strWidget14 = string.Empty;
                        string strWidget15 = string.Empty;

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

                        string strWidget01Size = string.Empty;
                        string strWidget02Size = string.Empty;
                        string strWidget03Size = string.Empty;
                        string strWidget04Size = string.Empty;
                        string strWidget05Size = string.Empty;

                        string strWidget06Size = string.Empty;
                        string strWidget07Size = string.Empty;
                        string strWidget08Size = string.Empty;
                        string strWidget09Size = string.Empty;

                        string strWidget10Size = string.Empty;
                        string strWidget11Size = string.Empty;
                        string strWidget12Size = string.Empty;
                        string strWidget13Size = string.Empty;
                        string strWidget14Size = string.Empty;
                        string strWidget15Size = string.Empty;

                        string strFrameUrl = string.Empty;

                        strWidget01 = ds.Tables[0].Rows[0]["WIDGET_01"].ToString();
                        strWidget02 = ds.Tables[0].Rows[0]["WIDGET_02"].ToString();
                        strWidget03 = ds.Tables[0].Rows[0]["WIDGET_03"].ToString();
                        strWidget04 = ds.Tables[0].Rows[0]["WIDGET_04"].ToString();
                        strWidget05 = ds.Tables[0].Rows[0]["WIDGET_05"].ToString();

                        strWidget06 = ds.Tables[0].Rows[0]["WIDGET_06"].ToString();
                        strWidget07 = ds.Tables[0].Rows[0]["WIDGET_07"].ToString();
                        strWidget08 = ds.Tables[0].Rows[0]["WIDGET_08"].ToString();
                        strWidget09 = ds.Tables[0].Rows[0]["WIDGET_09"].ToString();

                        strWidget10 = ds.Tables[0].Rows[0]["WIDGET_10"].ToString();
                        strWidget11 = ds.Tables[0].Rows[0]["WIDGET_11"].ToString();
                        strWidget12 = ds.Tables[0].Rows[0]["WIDGET_12"].ToString();
                        strWidget13 = ds.Tables[0].Rows[0]["WIDGET_13"].ToString();
                        strWidget14 = ds.Tables[0].Rows[0]["WIDGET_14"].ToString();
                        strWidget15 = ds.Tables[0].Rows[0]["WIDGET_15"].ToString();

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

                        strWidget01Size = ds.Tables[0].Rows[0]["WIDGET_01_SIZE"].ToString();
                        strWidget02Size = ds.Tables[0].Rows[0]["WIDGET_02_SIZE"].ToString();
                        strWidget03Size = ds.Tables[0].Rows[0]["WIDGET_03_SIZE"].ToString();
                        strWidget04Size = ds.Tables[0].Rows[0]["WIDGET_04_SIZE"].ToString();
                        strWidget05Size = ds.Tables[0].Rows[0]["WIDGET_05_SIZE"].ToString();

                        strWidget06Size = ds.Tables[0].Rows[0]["WIDGET_06_SIZE"].ToString();
                        strWidget07Size = ds.Tables[0].Rows[0]["WIDGET_07_SIZE"].ToString();
                        strWidget08Size = ds.Tables[0].Rows[0]["WIDGET_08_SIZE"].ToString();
                        strWidget09Size = ds.Tables[0].Rows[0]["WIDGET_09_SIZE"].ToString();

                        strWidget10Size = ds.Tables[0].Rows[0]["WIDGET_10_SIZE"].ToString();
                        strWidget11Size = ds.Tables[0].Rows[0]["WIDGET_11_SIZE"].ToString();
                        strWidget12Size = ds.Tables[0].Rows[0]["WIDGET_12_SIZE"].ToString();
                        strWidget13Size = ds.Tables[0].Rows[0]["WIDGET_13_SIZE"].ToString();
                        strWidget14Size = ds.Tables[0].Rows[0]["WIDGET_14_SIZE"].ToString();
                        strWidget15Size = ds.Tables[0].Rows[0]["WIDGET_15_SIZE"].ToString();

                        strFrameUrl = ds.Tables[0].Rows[0]["MOD_FRAME_URL"].ToString();

                        if (strFrameUrl != "")
                        {
                            var sb = new StringBuilder();
                            sb.AppendLine("function fn_FrameMainSetting() {");
                            sb.AppendLine("    $('#MainContents').load('" + strFrameUrl + "', function() {");

                            sb.AppendLine($"    fn_WidgetSetting('1', '{cy.Encrypt(strWidget01)}', '{strWidget01Url}', '{cy.Encrypt(strWidget01Size)}');");
                            sb.AppendLine($"    fn_WidgetSetting('2', '{cy.Encrypt(strWidget02)}', '{strWidget02Url}', '{cy.Encrypt(strWidget01Size)}');");
                            sb.AppendLine($"    fn_WidgetSetting('3', '{cy.Encrypt(strWidget03)}', '{strWidget03Url}', '{cy.Encrypt(strWidget01Size)}');");
                            sb.AppendLine($"    fn_WidgetSetting('4', '{cy.Encrypt(strWidget04)}', '{strWidget04Url}', '{cy.Encrypt(strWidget01Size)}');");
                            sb.AppendLine($"    fn_WidgetSetting('5', '{cy.Encrypt(strWidget05)}', '{strWidget05Url}', '{cy.Encrypt(strWidget01Size)}');");
                            sb.AppendLine($"    fn_WidgetSetting('6', '{cy.Encrypt(strWidget06)}', '{strWidget06Url}', '{cy.Encrypt(strWidget01Size)}');");
                            sb.AppendLine($"    fn_WidgetSetting('7', '{cy.Encrypt(strWidget07)}', '{strWidget07Url}', '{cy.Encrypt(strWidget01Size)}');");
                            sb.AppendLine($"    fn_WidgetSetting('8', '{cy.Encrypt(strWidget08)}', '{strWidget08Url}', '{cy.Encrypt(strWidget01Size)}');");
                            sb.AppendLine($"    fn_WidgetSetting('9', '{cy.Encrypt(strWidget09)}', '{strWidget09Url}', '{cy.Encrypt(strWidget01Size)}');");

                            sb.AppendLine($"    fn_WidgetSetting('10', '{cy.Encrypt(strWidget10)}', '{strWidget10Url}', '{cy.Encrypt(strWidget10Size)}');");
                            sb.AppendLine($"    fn_WidgetSetting('11', '{cy.Encrypt(strWidget11)}', '{strWidget11Url}', '{cy.Encrypt(strWidget11Size)}');");
                            sb.AppendLine($"    fn_WidgetSetting('12', '{cy.Encrypt(strWidget12)}', '{strWidget12Url}', '{cy.Encrypt(strWidget12Size)}');");
                            sb.AppendLine($"    fn_WidgetSetting('13', '{cy.Encrypt(strWidget13)}', '{strWidget13Url}', '{cy.Encrypt(strWidget13Size)}');");
                            sb.AppendLine($"    fn_WidgetSetting('14', '{cy.Encrypt(strWidget14)}', '{strWidget14Url}', '{cy.Encrypt(strWidget14Size)}');");
                            sb.AppendLine($"    fn_WidgetSetting('15', '{cy.Encrypt(strWidget15)}', '{strWidget15Url}', '{cy.Encrypt(strWidget15Size)}');");

                            sb.AppendLine("    });");
                            sb.AppendLine("}; fn_FrameMainSetting();");

                            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), sb.ToString(), true);
                        }
                    }
                    else
                    {

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

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }
        #endregion

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int iRtn=0;
            string strScript=string.Empty;

            string[] arrWidget01 = new string[0];
            string[] arrWidget02 = new string[0];
            string[] arrWidget03 = new string[0];
            string[] arrWidget04 = new string[0];
            string[] arrWidget05 = new string[0];
            string[] arrWidget06 = new string[0];
            string[] arrWidget07 = new string[0];
            string[] arrWidget08 = new string[0];
            string[] arrWidget09 = new string[0];
            string[] arrWidget10 = new string[0];
            string[] arrWidget11 = new string[0];
            string[] arrWidget12 = new string[0];
            string[] arrWidget13 = new string[0];
            string[] arrWidget14 = new string[0];
            string[] arrWidget15 = new string[0];

            if (txtdivCon1Widget.Text.Replace(" ", "") != "") { arrWidget01 = cy.Decrypt(txtdivCon1Widget.Text).Split('/'); }
            if (txtdivCon2Widget.Text.Replace(" ", "") != "") { arrWidget02 = cy.Decrypt(txtdivCon2Widget.Text).Split('/'); }
            if (txtdivCon3Widget.Text.Replace(" ", "") != "") { arrWidget03 = cy.Decrypt(txtdivCon3Widget.Text).Split('/'); }
            if (txtdivCon4Widget.Text.Replace(" ", "") != "") { arrWidget04 = cy.Decrypt(txtdivCon4Widget.Text).Split('/'); }
            if (txtdivCon5Widget.Text.Replace(" ", "") != "") { arrWidget05 = cy.Decrypt(txtdivCon5Widget.Text).Split('/'); }
            if (txtdivCon6Widget.Text.Replace(" ", "") != "") { arrWidget06 = cy.Decrypt(txtdivCon6Widget.Text).Split('/'); }
            if (txtdivCon7Widget.Text.Replace(" ", "") != "") { arrWidget07 = cy.Decrypt(txtdivCon7Widget.Text).Split('/'); }
            if (txtdivCon8Widget.Text.Replace(" ", "") != "") { arrWidget08 = cy.Decrypt(txtdivCon8Widget.Text).Split('/'); }
            if (txtdivCon9Widget.Text.Replace(" ", "") != "") { arrWidget09 = cy.Decrypt(txtdivCon9Widget.Text).Split('/'); }
            if (txtdivCon10Widget.Text.Replace(" ", "") != "") { arrWidget10 = cy.Decrypt(txtdivCon10Widget.Text).Split('/'); }
            if (txtdivCon11Widget.Text.Replace(" ", "") != "") { arrWidget11 = cy.Decrypt(txtdivCon11Widget.Text).Split('/'); }
            if (txtdivCon12Widget.Text.Replace(" ", "") != "") { arrWidget12 = cy.Decrypt(txtdivCon12Widget.Text).Split('/'); }
            if (txtdivCon13Widget.Text.Replace(" ", "") != "") { arrWidget13 = cy.Decrypt(txtdivCon13Widget.Text).Split('/'); }
            if (txtdivCon14Widget.Text.Replace(" ", "") != "") { arrWidget14 = cy.Decrypt(txtdivCon14Widget.Text).Split('/'); }
            if (txtdivCon15Widget.Text.Replace(" ", "") != "") { arrWidget15 = cy.Decrypt(txtdivCon15Widget.Text).Split('/'); }

            // 비지니스 클래스 작성
            HQCWeb.Biz.SystemManagement.WidgetMgt biz=new HQCWeb.Biz.SystemManagement.WidgetMgt();

            strDBName="GPDB";
            strQueryID="WidgetData.Set_UserByWidgetInfo";

            FW.Data.Parameters sParam=new FW.Data.Parameters();

            if (arrWidget01.Length>0) { sParam.Add("WIDGET_01", arrWidget01[0].ToString()); } else { sParam.Add("WIDGET_01", "0"); }
            if (arrWidget02.Length>0) { sParam.Add("WIDGET_02", arrWidget02[0].ToString()); } else { sParam.Add("WIDGET_02", "0"); }
            if (arrWidget03.Length>0) { sParam.Add("WIDGET_03", arrWidget03[0].ToString()); } else { sParam.Add("WIDGET_03", "0"); }
            if (arrWidget04.Length>0) { sParam.Add("WIDGET_04", arrWidget04[0].ToString()); } else { sParam.Add("WIDGET_04", "0"); }
            if (arrWidget05.Length>0) { sParam.Add("WIDGET_05", arrWidget05[0].ToString()); } else { sParam.Add("WIDGET_05", "0"); }
            if (arrWidget06.Length>0) { sParam.Add("WIDGET_06", arrWidget06[0].ToString()); } else { sParam.Add("WIDGET_06", "0"); }
            if (arrWidget07.Length>0) { sParam.Add("WIDGET_07", arrWidget07[0].ToString()); } else { sParam.Add("WIDGET_07", "0"); }
            if (arrWidget08.Length>0) { sParam.Add("WIDGET_08", arrWidget08[0].ToString()); } else { sParam.Add("WIDGET_08", "0"); }
            if (arrWidget09.Length>0) { sParam.Add("WIDGET_09", arrWidget09[0].ToString()); } else { sParam.Add("WIDGET_09", "0"); }
            if (arrWidget10.Length > 0) { sParam.Add("WIDGET_10", arrWidget10[0].ToString()); } else { sParam.Add("WIDGET_10", "0"); }
            if (arrWidget11.Length > 0) { sParam.Add("WIDGET_11", arrWidget11[0].ToString()); } else { sParam.Add("WIDGET_11", "0"); }
            if (arrWidget12.Length > 0) { sParam.Add("WIDGET_12", arrWidget12[0].ToString()); } else { sParam.Add("WIDGET_12", "0"); }
            if (arrWidget13.Length > 0) { sParam.Add("WIDGET_13", arrWidget13[0].ToString()); } else { sParam.Add("WIDGET_13", "0"); }
            if (arrWidget14.Length > 0) { sParam.Add("WIDGET_14", arrWidget14[0].ToString()); } else { sParam.Add("WIDGET_14", "0"); }
            if (arrWidget15.Length > 0) { sParam.Add("WIDGET_15", arrWidget15[0].ToString()); } else { sParam.Add("WIDGET_15", "0"); }

            sParam.Add("USER_ID",       bp.g_userid.ToString());
            sParam.Add("FRAME_NUM",     ddlFrameNum.SelectedValue);

            sParam.Add("REG_ID",        bp.g_userid.ToString());    // 등록자
            sParam.Add("CUD_TYPE",      "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            sParam.Add("CUR_MENU_ID",   "WEB_00130");                 // 조회페이지 메뉴 아이디 입력-에러로그 생성시 메뉴 아이디 필요
            
            // 삭제 비지니스 메서드 작성
            iRtn=biz.DelWidgetInfo(strDBName, strQueryID, sParam);

            if (iRtn == 1)
            {
                strScript=" alert('정상 반영되었습니다.'); $('#MainContent_btnSearch').click(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                strScript=" alert('반영에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region GetWidgetTemplateInfo
        [WebMethod]
        public static string GetWidgetTemplateInfo(string sParams)
        {
            DataSet ds=new DataSet();
            Crypt cy=new Crypt();
            
            cy.Key=System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

            string strTableName    =string.Empty;
            string strMessage      =string.Empty;
            string strDBName       =string.Empty;
            string strQueryID      =string.Empty;

            string strRtn=string.Empty;

            string strWidgetNM=string.Empty;
            string strWidgetSize=string.Empty;
            string strWidgetNum=string.Empty;
            string strWidgetUrl=string.Empty;

            strDBName="GPDB";
            strQueryID="WidgetData.Get_WidgetTemplateInfo";

            // 비지니스 클래스 작성
            Biz.SystemManagement.WidgetMgt biz=new Biz.SystemManagement.WidgetMgt();

            FW.Data.Parameters sParam=new FW.Data.Parameters();
            //sParam.Add("WIDGET_SIZE", sParams.Replace("^", "*"));
            sParam.Add("WIDGET_SIZE", cy.Decrypt(sParams));

            ///*
            // 비지니스 메서드 호출
            ds =biz.GetUserByWidgetInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count>0)
            {
                if (ds.Tables[0].Rows.Count>0) 
                {
                    for (int i=0; i<ds.Tables[0].Rows.Count; i++) {
                        strWidgetNM        =ds.Tables[0].Rows[i]["WIDGET_NM"].ToString();
                        //strWidgetSize      =ds.Tables[0].Rows[i]["WIDGET_SIZE"].ToString();
                        //strWidgetSize      ="180*100"; 
                        strWidgetNum       =ds.Tables[0].Rows[i]["WIDGET_NUM"].ToString();
                        strWidgetUrl       =ds.Tables[0].Rows[i]["WIDGET_URL"].ToString();

                        strRtn += "<tr><td>" + strWidgetNM + "</td><td><a href=\"javascript: fn_Change('" + strWidgetNM + "', '"  + strWidgetSize + "', '" + cy.Encrypt(strWidgetNum) + "', '" + strWidgetUrl + "'); \">선택</a></td></tr>";        
                    }                    
                }
                else
                {
                    strRtn += "<tr><td colspan='3'>No Data</td></tr>";
                }
            }
            //*/
            return strRtn;
        }
        #endregion        

        #region GetWidgetStat01
        [WebMethod]
        public static string GetWidgetStat01()
        {
            string strRtn=string.Empty;
            DataSet ds = new DataSet();
            BasePage bp = new BasePage();

            // 비지니스 클래스 작성(실적)
            Biz.ResultManagement.ResComm biz = new Biz.ResultManagement.ResComm();

            string strDBName = "GPDB";
            string strQueryID = "ResWidgetData.Get_BSAList_Widget";

            //위젯의 경우에도 메뉴코드를 주는가?
            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());

            //sParam.Add("CUR_MENU_ID", "WEB_00101");

            ds = biz.GetDataSet(strDBName, strQueryID, sParam);
            if (ds.Tables.Count > 0)
            {
                strRtn = ds.Tables[0].Rows[0]["STR_RTN"].ToString();
            }

            //strRtn = "3,359^2,731^81.3%";
            
            return strRtn;
        }
        #endregion        

        #region GetWidgetStat02
        [WebMethod]
        public static string GetWidgetStat02()
        {
            string strRtn = string.Empty;
            DataSet ds = new DataSet();
            BasePage bp = new BasePage();

            // 비지니스 클래스 작성(실적)
            Biz.ResultManagement.ResComm biz = new Biz.ResultManagement.ResComm();

            string strDBName = "GPDB";
            string strQueryID = "PlnWidgetData.Get_PlanList_Widget";

            //위젯의 경우에도 메뉴코드를 주는가?
            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());

            //sParam.Add("CUR_MENU_ID", "WEB_00101");

            ds = biz.GetDataSet(strDBName, strQueryID, sParam);
            if (ds.Tables.Count > 0)
            {
                strRtn = ds.Tables[0].Rows[0]["STR_RTN"].ToString();
            }

            //strRtn = "3,359^2,731^81.3%";

            return strRtn;
        }
        #endregion        

        #region GetWidgetStat03
        [WebMethod]
        public static string GetWidgetStat03()
        {
            string strRtn = string.Empty;
            DataSet ds = new DataSet();
            BasePage bp = new BasePage();

            // 비지니스 클래스 작성(실적)
            Biz.ResultManagement.ResComm biz = new Biz.ResultManagement.ResComm();

            string strDBName = "GPDB";
            string strQueryID = "QuaWidgetData.Get_DecomposeList_Widget";

            //위젯의 경우에도 메뉴코드를 주는가?
            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());

            //sParam.Add("CUR_MENU_ID", "WEB_00101");

            ds = biz.GetDataSet(strDBName, strQueryID, sParam);
            if (ds.Tables.Count > 0)
            {
                strRtn = ds.Tables[0].Rows[0]["STR_RTN"].ToString();
            }

            return strRtn;
        }
        #endregion

        #region GetWidgetStat04
        [WebMethod]
        public static string GetWidgetStat04()
        {
            string strRtn = string.Empty;

            strRtn = "100^80^20^90^70^10";

            return strRtn;
        }
        #endregion

        #region GetStatusBoard01
        [WebMethod]
        public static string GetStatusBoard01()
        {
            string strRtn = string.Empty;
            DataSet ds = new DataSet();
            BasePage bp = new BasePage();

            // 비지니스 클래스 작성(실적)
            Biz.ResultManagement.ResComm biz = new Biz.ResultManagement.ResComm();

            string strDBName = "GPDB";
            string strQueryID = "ResWidgetData.Get_RealTime_PIB_Widget_01";

            //위젯의 경우에도 메뉴코드를 주는가?
            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());

            ds = biz.GetDataSet(strDBName, strQueryID, sParam);
            if (ds.Tables.Count > 0)
            {
                for(int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strRtn += "<tr>";
                    strRtn += "<td>" + ds.Tables[0].Rows[i]["LINE_NM"].ToString() + "</td>";
                    strRtn += "<td>" + ds.Tables[0].Rows[i]["PLAN_QTY"].ToString() + "</td>";
                    strRtn += "<td>" + ds.Tables[0].Rows[i]["TARGET_QTY"].ToString() + "</td>";
                    strRtn += "<td>" + ds.Tables[0].Rows[i]["RES_QTY"].ToString() + "</td>";
                    strRtn += "<td style='background:#fcf4f4'>" + ds.Tables[0].Rows[i]["ACHIEVE_RATE"].ToString() + "</td>";
                    strRtn += "<td style='background:#f5fcf3'>" + ds.Tables[0].Rows[i]["EFFICIENCY_RATE"].ToString() + "</td>";
                    strRtn += "</tr>";
                }
            }

            return strRtn;
        }
        #endregion        
        
        #region GetCalendar01
        [WebMethod]
        public static string GetCalendar01(string year, string month)
        {
            DateTime start = new DateTime(Convert.ToInt32(year), Convert.ToInt32(month), 1).Date;
            DateTime end = start.AddMonths(1).AddDays(-1);
            DateTime Now = DateTime.Now.Date;

            DataSet ds = new DataSet();
            BasePage bp = new BasePage();

            Crypt cy = new Crypt();

            cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

            // 비지니스 클래스 작성
            Biz.SystemManagement.WidgetMgt biz = new Biz.SystemManagement.WidgetMgt();

            string strRtn = string.Empty;
            string strTableName = string.Empty;
            string strMessage = string.Empty;
            string strDBName = string.Empty;
            string strQueryID = string.Empty;
            string strDate = string.Empty;
            string strNum = string.Empty;
            string strTitle = string.Empty;
            string strManNo = string.Empty;
            string pMenu = string.Empty;

            int intBrk = 0;
            int x1 = 0, x2 = 0, x3 = 0;
            int len1 = 0, len2 = 0, len3 = 0;
            int cnt1 = 0, cnt2 = 0, cnt3 = 0;
            bool dbExist = false;
            bool dateExist = false;

            strDBName = "GPDB";
            strQueryID = "WidgetData.Get_CalendarData";

            //위젯의 경우에도 메뉴코드를 주는가?
            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());
            sParam.Add("PROD_DT", start.ToString("yyyyMM"));

            ds = biz.GetDataSet(strDBName, strQueryID, sParam);
            if (ds.Tables.Count > 0)
            {
                len1 = ds.Tables[0].Rows.Count;
                len2 = ds.Tables[1].Rows.Count;
                len3 = ds.Tables[2].Rows.Count;
                dbExist = true;
            }

            strRtn = "<tr><td colspan=\"7\" style=\"height:14px;\"></td></tr>";
            
            for (int i = 0; i < 6; i++) //최대 6주
            {
                strRtn += "<tr>";

                for ( int j = 0; j < 7; j++ ) //일주일
                {
                    //date 데이터 존재 검사
                    strDate = start.ToString("yyyyMMdd");
                    
                    //날짜 데이터 검사
                    if (len1 > x1 && strDate.Equals(ds.Tables[0].Rows[x1]["INSPECT_DATE"].ToString()))
                    {
                        pMenu = "Qua51";
                        strDate = strDate.Substring(6, 2);
                        dateExist = true;
                        cnt1 = Convert.ToInt32(ds.Tables[0].Rows[x1]["CNT"].ToString());
                    }

                    //날짜 데이터 검사
                    if (len3 > x3 && strDate.Equals(ds.Tables[2].Rows[x3]["DATA_DATE"].ToString()))
                    {
                        strDate = strDate.Substring(6, 2);
                        dateExist = true;
                        cnt3 = Convert.ToInt32(ds.Tables[2].Rows[x3]["CNT"].ToString());
                    }


                    if (i == 0) // 1주
                    {
                        if(j < (int)start.DayOfWeek) //시작일보다 이전
                        {
                            strRtn += "<td>&nbsp;</td>";
                        }
                        else if(DateTime.Compare(start, Now) == 0) //오늘
                        {
                            strRtn += "<td><div class=\"today\" ";
                            if (dateExist) // 스케줄 있음
                            {
                                strRtn += "style=\"cursor:pointer;\">" + start.Day + "</div>";
                                strRtn += "<div class=\"tooltip-content\"><table style=\"margin-top: 0px;\"> ";

                                if (cnt1 > 0)
                                {
                                    strRtn += "<tr>";
                                    strRtn += "<th class=\"tooltip_th chk_label\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000080\">검교정</th>";
                                    strRtn += "<td class=\"tooltip_td chk_label\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000000\">" + cnt1 + "건</td>";
                                    strRtn += "</tr>";

                                    //검교정 상세
                                    for (int y = x1; y < x1 + cnt1; y++)
                                    {
                                        strManNo = cy.Encrypt(ds.Tables[0].Rows[y]["MAN_NO"].ToString() + '/' + ds.Tables[0].Rows[y]["INSPECT_YEAR"].ToString());
                                        strTitle = ds.Tables[0].Rows[y]["MAN_NO"].ToString();
                                        strRtn += "<tr class=\"chk_item\"><td colspan=\"2\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000000\"><a href = \"#\" onclick=\"fn_Chk_Move('" + pMenu +"', '"+ strManNo + "'); return false; \" >" + strTitle + "</a></td></tr>";
                                    }

                                    x1 += cnt1;
                                }

                                //strRtn += "<th class=\"tooltip_th\" style=\"color:#008000\">적용관리</th>";
                                //strRtn += "<td class=\"tooltip_td\" style=\"color:#000000\">1건</td>";
                                //strRtn += "</tr><tr>";

                                if (cnt3 > 0)
                                {
                                    strRtn += "<tr>";
                                    strRtn += "<th class=\"tooltip_th notice_label\" style=\"height:20px;font-size:12px;cursor:pointer;color:#FF0000\">공지사항</th>";
                                    strRtn += "<td class=\"tooltip_td notice_label\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000000\">" + cnt3+"건</td>";
                                    strRtn += "</tr>";
                                    
                                    //공지사항 상세
                                    for(int y = x3; y < x3+cnt3; y++)
                                    {
                                        strNum = cy.Encrypt(ds.Tables[2].Rows[y]["NOTICE_NUM"].ToString());
                                        strTitle = ds.Tables[2].Rows[y]["NOTICE_TITLE"].ToString();
                                        strRtn += "<tr class=\"notice_item\"><td colspan=\"2\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000000\"><a href = \"#\" onclick=\"fn_PostOpenPop('" + strNum + "', '/SystemMgt/NoticeManagement/Notice005.aspx', '1000', '800'); return false; \" >" + strTitle + "</a></td></tr>";
                                    }
                                        
                                    x3 += cnt3;
                                }

                                strRtn += "</table></div> ";
                            }
                            else 
                                strRtn += ">" + start.Day + "</div>";// 스케줄 없음

                            strRtn += "</td>";
                            start = start.AddDays(1); //1일 추가
                        }
                        else // 일반일
                        {
                            // 스케줄 있음
                            if (dateExist)
                            {
                                strRtn += "<td><a href=\"#\" class=\"schedule\">" + start.Day + "</a>";
                                strRtn += "<div class=\"tooltip-content\"><table style=\"margin-top: 0px;\"> ";

                                if (cnt1 > 0)
                                {
                                    strRtn += "<tr>";
                                    strRtn += "<th class=\"tooltip_th chk_label\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000080\">검교정</th>";
                                    strRtn += "<td class=\"tooltip_td chk_label\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000000\">" + cnt1 + "건 </td>";
                                    strRtn += "</tr>";

                                    //검교정 상세
                                    for (int y = x1; y < x1 + cnt1; y++)
                                    {
                                        strManNo = cy.Encrypt(ds.Tables[0].Rows[y]["MAN_NO"].ToString()+'/'+ ds.Tables[0].Rows[y]["INSPECT_YEAR"].ToString());
                                        strTitle = ds.Tables[0].Rows[y]["MAN_NO"].ToString();
                                        strRtn += "<tr class=\"chk_item\"><td colspan=\"2\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000000\"><a href = \"#\" onclick=\"fn_Chk_Move('" + pMenu + "', '" + strManNo + "'); return false; \" >" + strTitle + "</a></td></tr>";
                                    }

                                    x1 += cnt1;
                                }

                                if (cnt3 > 0)
                                {
                                    strRtn += "<tr>";
                                    strRtn += "<th class=\"tooltip_th notice_label\" style=\"height:20px;font-size:12px;cursor:pointer;color:#FF0000\">공지사항</th>";
                                    strRtn += "<td class=\"tooltip_td notice_label\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000000\">" + cnt3 + "건</td>";
                                    strRtn += "</tr>";

                                    //공지사항 상세
                                    for (int y = x3; y < x3 + cnt3; y++)
                                    {
                                        strNum = cy.Encrypt(ds.Tables[2].Rows[y]["NOTICE_NUM"].ToString());
                                        strTitle = ds.Tables[2].Rows[y]["NOTICE_TITLE"].ToString();
                                        strRtn += "<tr class=\"notice_item\"><td colspan=\"2\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000000\"><a href = \"#\" onclick=\"fn_PostOpenPop('" + strNum + "', '/SystemMgt/NoticeManagement/Notice005.aspx', '1000', '800'); return false; \" >" + strTitle + "</a></td></tr>";
                                    }

                                    x3 += cnt3;
                                }

                                strRtn += "</table></div></td>";
                            }
                            else
                                strRtn += "<td>" + start.Day + "</td>"; // 스케줄 없음
                            start = start.AddDays(1); //1일 추가
                        }
                    }
                    else
                    {
                        if (DateTime.Compare(start, Now) == 0) //오늘
                        {
                            strRtn += "<td><div class=\"today\" ";
                            if (dateExist) // 스케줄 있음
                            {
                                strRtn += "style=\"cursor:pointer;\">" + start.Day + "</div>";
                                strRtn += "<div class=\"tooltip-content\"><table style=\"margin-top: 0px;\"> ";
                                if (cnt1 > 0)
                                {
                                    strRtn += "<tr>";
                                    strRtn += "<th class=\"tooltip_th chk_label\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000080\">검교정</th>";
                                    strRtn += "<td class=\"tooltip_td chk_label\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000000\">" + cnt1+"건</td>";
                                    strRtn += "</tr>";

                                    //검교정 상세
                                    for (int y = x1; y < x1 + cnt1; y++)
                                    {
                                        strManNo = cy.Encrypt(ds.Tables[0].Rows[y]["MAN_NO"].ToString() + '/' + ds.Tables[0].Rows[y]["INSPECT_YEAR"].ToString());
                                        strTitle = ds.Tables[0].Rows[y]["MAN_NO"].ToString();
                                        strRtn += "<tr class=\"chk_item\"><td colspan=\"2\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000000\"><a href = \"#\" onclick=\"fn_Chk_Move('" + pMenu + "', '" + strManNo + "'); return false; \" >" + strTitle + "</a></td></tr>";
                                    }

                                    x1 += cnt1;
                                }

                                if (cnt3 > 0)
                                {
                                    strRtn += "<tr>";
                                    strRtn += "<th class=\"tooltip_th notice_label\" style=\"height:20px;font-size:12px;cursor:pointer;color:#FF0000\">공지사항</th>";
                                    strRtn += "<td class=\"tooltip_td notice_label\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000000\">" + cnt3 + "건</td>";
                                    strRtn += "</tr>";

                                    //공지사항 상세
                                    for (int y = x3; y < x3 + cnt3; y++)
                                    {
                                        strNum = cy.Encrypt(ds.Tables[2].Rows[y]["NOTICE_NUM"].ToString());
                                        strTitle = ds.Tables[2].Rows[y]["NOTICE_TITLE"].ToString();
                                        strRtn += "<tr class=\"notice_item\"><td colspan=\"2\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000000\"><a href = \"#\" onclick=\"fn_PostOpenPop('" + strNum + "', '/SystemMgt/NoticeManagement/Notice005.aspx', '1000', '800'); return false; \" >" + strTitle + "</a></td></tr>";
                                    }

                                    x3 += cnt3;
                                }

                                strRtn += "</table></div> ";
                            }
                            else 
                                strRtn += ">" + start.Day + "</div>";// 스케줄 없음

                            strRtn += "</td>";
                        }
                        else // 일반일
                        {
                            // 스케줄 있음
                            if (dateExist)
                            {
                                strRtn += "<td><a href=\"#\" class=\"schedule\">" + start.Day + "</a>";
                                strRtn += "<div class=\"tooltip-content\"><table style=\"margin-top: 0px;\"><tr> ";

                                if(cnt1 > 0)
                                {
                                    strRtn += "<tr>";
                                    strRtn += "<th class=\"tooltip_th chk_label\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000080\">검교정</th>";
                                    strRtn += "<td class=\"tooltip_td chk_label\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000000\">" + cnt1+"건</td>";
                                    strRtn += "</tr>";

                                    //검교정 상세
                                    for (int y = x1; y < x1 + cnt1; y++)
                                    {
                                        strManNo = cy.Encrypt(ds.Tables[0].Rows[y]["MAN_NO"].ToString() + '/' + ds.Tables[0].Rows[y]["INSPECT_YEAR"].ToString());
                                        strTitle = ds.Tables[0].Rows[y]["MAN_NO"].ToString();
                                        strRtn += "<tr class=\"chk_item\"><td colspan=\"2\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000000\"><a href = \"#\" onclick=\"fn_Chk_Move('" + pMenu + "', '" + strManNo + "'); return false; \" >" + strTitle + "</a></td></tr>";
                                    }

                                    x1 += cnt1;
                                }


                                if (cnt3 > 0)
                                {
                                    strRtn += "<tr>";
                                    strRtn += "<th class=\"tooltip_th notice_label\" style=\"height:20px;font-size:12px;cursor:pointer;color:#FF0000\">공지사항</th>";
                                    strRtn += "<td class=\"tooltip_td notice_label\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000000\">" + cnt3 + "건</td>";
                                    strRtn += "</tr>";

                                    //공지사항 상세
                                    for (int y = x3; y < x3 + cnt3; y++)
                                    {
                                        strNum = cy.Encrypt(ds.Tables[2].Rows[y]["NOTICE_NUM"].ToString());
                                        strTitle = ds.Tables[2].Rows[y]["NOTICE_TITLE"].ToString();
                                        strRtn += "<tr class=\"notice_item\"><td colspan=\"2\" style=\"height:20px;font-size:12px;cursor:pointer;color:#000000\"><a href = \"#\" onclick=\"fn_PostOpenPop('" + strNum + "', '/SystemMgt/NoticeManagement/Notice005.aspx', '1000', '800'); return false; \" >" + strTitle + "</a></td></tr>";
                                    }

                                    x3 += cnt3;
                                }

                                strRtn += "</table></div></td>";
                            }
                            else
                                strRtn += "<td>" + start.Day + "</td>";// 스케줄 없음
                        }

                        //마지막일이었으면
                        if (DateTime.Compare(start, end) == 0)
                        {
                            intBrk++;
                            break;
                        }
                        else
                        {
                            start = start.AddDays(1); //1일 추가
                        }
                    }
                    cnt1 = 0;
                    cnt2 = 0;
                    cnt3 = 0;
                    dateExist = false;
                }
                strRtn += "</tr>";

                //마지막일이었으면
                if (intBrk > 0)
                {
                    break;
                }
            }

            return strRtn;

            //strRtn ="<tr><td colspan=\"7\" style=\"height:14px;\"></td></tr>" +
            //    "<tr>" +
            //    "<td>&nbsp;</td>" +
            //    "<td>&nbsp;</td>" +
            //    "<td>&nbsp;</td>" +
            //    "<td>&nbsp;</td>" +
            //    "<td>&nbsp;</td>" +
            //    "<td>1</td>" +
            //    "<td>2</td>" +
            //    "</tr>" +
            //    "<tr>" +
            //    "<td>3</td>" +
            //    "<td>4</td>" +
            //    "<td>5</td>" +
            //    "<td>6</td>" +
            //    "<td>7</td>" +
            //    "<td><a href=\"#\" class=\"schedule\">8</a></td>" +
            //    "   <td>9</td>" +
            //    "</tr>" +
            //    "<tr>" +
            //    "   <td>10</td>" +
            //    "   <td>11</td>" +
            //    "   <td>12</td>" +
            //    "   <td>13</td>" +
            //    "   <td>14</td>" +
            //    "   <td>15</td>" +
            //    "   <td>16</td>" +
            //    "</tr>" +
            //    "<tr>" +
            //    "   <td>17</td>" +
            //    "   <td>18</td>" +
            //    "   <td>19</td>" +
            //    "   <td>20</td>" +
            //    "   <td><p class=\"today\">21</p></td>" +
            //    "   <td>22</td>" +
            //    "   <td>23</td>" +
            //    "</tr>" +
            //    "<tr>" +
            //    "   <td>24</td>" +
            //    "   <td><a href=\"#\" class=\"schedule\">25</a></td>" +
            //    "   <td>26</td>" +
            //    "   <td>27</td>" +
            //    "   <td>28</td>" +
            //    "   <td>29</td>" +
            //    "   <td>30</td>" +
            //    "</tr>" +
            //    "<tr>" +
            //    "   <td>31</td>" +
            //    "   <td>&nbsp;</td>" +
            //    "   <td>&nbsp;</td>" +
            //    "   <td>&nbsp;</td>" +
            //    "   <td>&nbsp;</td>" +
            //    "   <td>&nbsp;</td>" +
            //    "   <td>&nbsp;</td>" +
            //    "</tr>";

            //return strRtn;
        }
        #endregion        

        #region GetNotice01
        [WebMethod]
        public static string GetNotice01()
        {
            DataSet ds = new DataSet();

            Crypt cy = new Crypt();

            cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

            // 비지니스 클래스 작성
            Biz.SystemManagement.NoticeMgt biz = new Biz.SystemManagement.NoticeMgt();

            string strRtn = string.Empty;
            string strTableName = string.Empty;
            string strMessage = string.Empty;
            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            strDBName = "GPDB";
            strQueryID = "NoticeData.Get_NoticeList_Widget";

            //위젯의 경우에도 메뉴코드를 주는가?
            FW.Data.Parameters sParam = new FW.Data.Parameters();

            //sParam.Add("CUR_MENU_ID", "WEB_00101");



            strRtn = "<colgroup>" +
               "<col />" +
               "<col style=\"width:90px;\" />" +
           "</colgroup>";

            //New Icon이 나오는 조건은?
            ds = biz.GetNoticeList(strDBName, strQueryID, sParam);
            if (ds.Tables.Count > 0)
            {
                int len = ds.Tables[0].Rows.Count;
                string link = string.Empty;
                for(int i = 0; i<len; i++)
                {
                    link = "fn_PostOpenPop('"+ cy.Encrypt(ds.Tables[0].Rows[i]["NOTICE_NUM"].ToString()) +"', '/SystemMgt/NoticeManagement/Notice005.aspx', '1000', '800'); return false;";
                    strRtn += "<tr>"
                    + "  <td><a href=\"#\" onclick=\"" + link + "\" > "
                    + ds.Tables[0].Rows[i]["NOTICE_TITLE"].ToString() +"</a></td>" 
                    +"  <td>" + ds.Tables[0].Rows[i]["NOTICE_DATE"].ToString() + "</td>" 
                    +"</tr>";
                }

            }
    //strRtn = "<colgroup>" +
    //            "<col />" +
    //            "<col style=\"width:90px;\" />" +
    //        "</colgroup>" +
    //        "<tr>" +
    //        "  <td><a href=\"#\">브랜드와 내가 함께 성장하는 브랜드 마케팅! ...</a><img src=\"/img/New/icon_new.png\"></td>" +
    //        "  <td>2024-04-02</td>" +
    //        "</tr>" +
    //        "<tr>" +
    //        "  <td><a href=\"#\">브랜드와 내가 함께 성장하는 브랜드 마케팅!트렌드와 마케팅을 한 번에 잡는 기술</a></td>" +
    //        "  <td>2024-04-02</td>" +
    //        "</tr>" +
    //        "<tr>" +
    //        "  <td><a href=\"#\">브랜드와 내가 함께 성장하는 브랜드 마케팅!트렌드와 마케팅을 한 번에 잡는 기술</a></td>" +
    //        "  <td>2024-04-02</td>" +
    //        "</tr>" +
    //        "<tr>" +
    //        "  <td><a href=\"#\">브랜드와 내가 함께 성장하는 브랜드 마케팅!트렌드와 마케팅을 한 번에 잡는 기술</a></td>" +
    //        "  <td>2024-04-02</td>" +
    //        "</tr>" +
    //        "<tr>" +
    //        "  <td><a href=\"#\">브랜드와 내가 함께 성장하는 브랜드 마케팅!트렌드와 마케팅을 한 번에 잡는 기술</a></td>" +
    //        "  <td>2024-04-02</td>" +
    //        "</tr>" +
    //        "<tr>" +
    //        "  <td><a href=\"#\">브랜드와 내가 함께 성장하는 브랜드 마케팅!트렌드와 마케팅을 한 번에 잡는 기술</a></td>" +
    //        "  <td>2024-04-02</td>" +
    //        "</tr>" +
    //        "<tr>" +
    //        "  <td><a href=\"#\">브랜드와 내가 함께 성장하는 브랜드 마케팅!트렌드와 마케팅을 한 번에 잡는 기술</a></td>" +
    //        "  <td>2024-04-02</td>" +
    //        "</tr>" +
    //        "<tr>" +
    //        "  <td><a href=\"#\">브랜드와 내가 함께 성장하는 브랜드 마케팅!트렌드와 마케팅을 한 번에 잡는 기술</a></td>" +
    //        "  <td>2024-04-02</td>" +
    //        "</tr>" +
    //        "<tr>" +
    //        "  <td><a href=\"#\">브랜드와 내가 함께 성장하는 브랜드 마케팅!트렌드와 마케팅을 한 번에 잡는 기술</a></td>" +
    //        "  <td>2024-04-02</td>" +
    //        "</tr>" +
    //        "<tr>" +
    //        "  <td><a href=\"#\">브랜드와 내가 함께 성장하는 브랜드 마케팅!트렌드와 마케팅을 한 번에 잡는 기술</a></td>" +
    //        "  <td>2024-04-02</td>" +
    //        "</tr>" +
    //        "<tr>" +
    //        "  <td><a href=\"#\">브랜드와 내가 함께 성장하는 브랜드 마케팅!트렌드와 마케팅을 한 번에 잡는 기술</a></td>" +
    //        "  <td>2024-04-02</td>" +
    //        "</tr>" +
    //        "<tr>" +
    //        "  <td><a href=\"#\">브랜드와 내가 함께 성장하는 브랜드 마케팅!트렌드와 마케팅을 한 번에 잡는 기술</a></td>" +
    //        "  <td>2024-04-02</td>" +
    //        "</tr>" +
    //        "<tr>" +
    //        "  <td><a href=\"#\">브랜드와 내가 함께 성장하는 브랜드 마케팅!트렌드와 마케팅을 한 번에 잡는 기술</a></td>" +
    //        "  <td>2024-04-02</td>" +
    //        "</tr>" +
    //        "<tr>" +
    //        "  <td><a href=\"#\">브랜드와 내가 함께 성장하는 브랜드 마케팅!트렌드와 마케팅을 한 번에 잡는 기술</a></td>" +
    //        "  <td>2024-04-02</td>" +
    //        "</tr>" +
    //        "<tr>" +
    //        "  <td><a href=\"#\">브랜드와 내가 함께 성장하는 브랜드 마케팅!트렌드와 마케팅을 한 번에 잡는 기술</a></td>" +
    //        "  <td>2024-04-02</td>" +
    //        "</tr>" +
    //        "<tr>" +
    //        "  <td><a href=\"#\">브랜드와 내가 함께 성장하는 브랜드 마케팅!트렌드와 마케팅을 한 번에 잡는 기술</a></td>" +
    //        "  <td>2024-04-02</td>" +
    //        "</tr>";

            return strRtn;
        }
        #endregion        

        #region GetAlarm01
        [WebMethod]
        public static string GetAlarm01()
        {
            string strRtn = string.Empty;
            DataSet ds = new DataSet();
            BasePage bp = new BasePage();

            // 비지니스 클래스 작성(실적)
            Biz.MonitorManagement.MonComm biz = new Biz.MonitorManagement.MonComm();

            string strDBName = "GPDB";
            string strQueryID = "MonWidgetData.Get_Alarm_Widget_01";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());

            strRtn += "<tr><td colspan=\"4\" style=\"height:1px;\"></td></tr>";
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);
            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if(i == 0)
                        strRtn += "<tr class=\"top1\">";
                    else if(i == 1)
                        strRtn += "<tr class=\"top2\">";
                    else if(i == 2)
                        strRtn += "<tr class=\"top3\">";
                    else
                        strRtn += "<tr>";

                    strRtn += "<td>" + ds.Tables[0].Rows[i]["SEQ"].ToString() + "</td>";
                    strRtn += "<td>" + ds.Tables[0].Rows[i]["ALM_MSG"].ToString() + "</td>";
                    strRtn += "<td>" + ds.Tables[0].Rows[i]["LINE_NM"].ToString() + "</td>";
                    strRtn += "<td>" + ds.Tables[0].Rows[i]["STOP_TIME"].ToString() + "</td>";
                    strRtn += "<td>" + ds.Tables[0].Rows[i]["RATIO"].ToString() + "</td>";

                    strRtn += "</tr>";
                }
            }


            //strRtn += "<tr><td colspan=\"4\" style=\"height:1px;\"></td></tr>" +
            //    "    <tr class=\"top1\">" +
            //    "    <td>1</td>" +
            //    "    <td>ST05 RH N/R 작업결과NG</td>" +
            //    "    <td>205</td>" +
            //    "    <td>10.77</td>" +
            //    "</tr>" +
            //    "<tr class=\"top2\">" +
            //    "    <td>2</td>" +
            //    "    <td>D1017 #BS60 Door1 열림감지알람</td>" +
            //    "    <td>253</td>" +
            //    "    <td>9.23</td>" +
            //    "</tr>" +
            //    "<tr class=\"top3\">" +
            //    "    <td>3</td>" +
            //    "    <td>ST05 RH N/R 작업결과NG</td>" +
            //    "    <td>629</td>" +
            //    "    <td>7.18</td>" +
            //    "</tr>" +
            //    "<tr>" +
            //    "    <td>4</td>" +
            //    "    <td>ST05 RH N/R 작업결과NG</td>" +
            //    "    <td>317</td>" +
            //    "    <td>6.15</td>" +
            //    "</tr>" +
            //    "<tr>" +
            //    "    <td>5</td>" +
            //    "    <td>ST05 RH N/R 작업결과NG</td>" +
            //    "    <td>632</td>" +
            //    "    <td>5.13</td>" +
            //    "</tr>" +
            //    "<tr>" +
            //    "    <td>6</td>" +
            //    "    <td>ST05 RH N/R 작업결과NG</td>" +
            //    "    <td>205</td>" +
            //    "    <td>4.62</td>" +
            //    "</tr>" +
            //    "<tr>" +
            //    "    <td>7</td>" +
            //    "    <td>ST05 RH N/R 작업결과NG</td>" +
            //    "    <td>205</td>" +
            //    "    <td>3.08</td>" +
            //    "</tr>" +
            //    "<tr>" +
            //    "    <td>8</td>" +
            //    "    <td>ST05 RH N/R 작업결과NG</td>" +
            //    "    <td>205</td>" +
            //    "    <td>3.07</td>" +
            //    "</tr>" +
            //    "<tr>" +
            //    "    <td>9</td>" +
            //    "    <td>ST05 RH N/R 작업결과NG</td>" +
            //    "    <td>205</td>" +
            //    "    <td>2.56</td>" +
            //    "</tr>" +
            //    "<tr>" +
            //    "    <td>10</td>" +
            //    "    <td>#01 Safety Door Open</td>" +
            //    "    <td>205</td>" +
            //    "    <td>2.05</td>" +
            //    "</tr>";

            return strRtn;
        }
        #endregion        

        #region GetWidgetGraph01
        [WebMethod]
        public static string GetWidgetGraph01(string sFlag)
        {
            string strRtn = string.Empty;
            DataSet ds = new DataSet();
            BasePage bp = new BasePage();

            // 비지니스 클래스 작성(실적)
            Biz.ResultManagement.ResComm biz = new Biz.ResultManagement.ResComm();

            string strDBName = "GPDB";
            string strQueryID = "ResWidgetData.Get_BSAChart_Widget_01";

            //위젯의 경우에도 메뉴코드를 주는가?
            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());
            sParam.Add("P_FLAG", sFlag);

            ds = biz.GetDataSet(strDBName, strQueryID, sParam);
            if (ds.Tables.Count > 0)
            {
                strRtn = "[";

                for(int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strRtn += "{date:'" + ds.Tables[0].Rows[i]["PLAN_DT"].ToString() + "', prod: '" + ds.Tables[0].Rows[i]["RES_QTY"].ToString() + "', plan: '" + ds.Tables[0].Rows[i]["PLAN_QTY"].ToString() + "'}";
                    strRtn += (i == ds.Tables[0].Rows.Count - 1) ? "" : ",";
                }

                //strRtn = "{date: '2020',prod: '26.2',plan: '22.8'},{date: '2021',prod: '30.1',plan: '23.9'},{date: '2022',prod: '29.5',plan: '25.1'},{date: '2023',prod: '30.6',plan: '27.2'},{date: '2024',prod: '34.1',plan: '29.9'}";
                strRtn += "]";
            }

            return strRtn;
        }
        #endregion

        #region GetWidgetGraph02
        [WebMethod]
        public static string GetWidgetGraph02()
        {
            string strRtn = string.Empty;
            DataSet ds = new DataSet();
            BasePage bp = new BasePage();

            // 비지니스 클래스 작성(실적)
            Biz.ResultManagement.ResComm biz = new Biz.ResultManagement.ResComm();

            string strDBName = "GPDB";
            string strQueryID = "ResWidgetData.Get_BSAChart_Widget_02";

            //위젯의 경우에도 메뉴코드를 주는가?
            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);
            if (ds.Tables.Count > 0)
            {
                strRtn = "[";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    strRtn += "{part:'" + ds.Tables[0].Rows[i]["PART_NO"].ToString() + "', qty: '" + ds.Tables[0].Rows[i]["QTY"].ToString() + "'}";
                    strRtn += (i == ds.Tables[0].Rows.Count - 1) ? "" : ",";
                }
                strRtn += "]";
            }

            //strRtn = "[{part: 'A', qty: '180'}, {part: 'B', qty: '170'},{part: 'C', qty: '120'},{part: 'D', qty: '180'},{part: 'E', qty: '280'}];";

            return strRtn;
        }
        #endregion

        #region GetMenu
        [WebMethod]
        public static string GetMenu(string sMenu)
        {
            // 변수 조회 및 암호화
            string strMenuID = string.Empty;

            string strResultData = string.Empty;
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            DataSet ds = new DataSet();
            string strDBName = "GPDB";
            string strQueryID = "MenuData.Get_MenuInfo";
            strMenuID = sMenu;
            Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("MENU_ID", strMenuID);

            ds = biz.GetMenuData(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                if (strTableName == "ErrorLog")
                {
                    strMessage = ds.Tables[0].Rows[0][1].ToString();

                    strResultData = "Error";
                }
                else if (ds.Tables[0].Rows.Count > 0)
                {
                    strResultData = ds.Tables[0].Rows[0]["MENU_NM"].ToString().Split('(')[0];
                }
            }
            return strResultData;
        }
        #endregion

        #region GetChkMenu
        [WebMethod]
        public static string GetChkMenu(string sMenu, string sData)
        {
            Crypt cy = new Crypt();

            cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

            string decrpted = cy.Decrypt(sData);

            string sendData = cy.Encrypt(decrpted);

            //세션에 데이터 저장
            HttpContext.Current.Session["CalenderToQua51"] = sendData;

            //메뉴 조회
            string strMenuID = string.Empty;

            string strResultData = string.Empty;
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            DataSet ds = new DataSet();
            string strDBName = "GPDB";
            string strQueryID = "MenuData.Get_MenuInfo";
            strMenuID = sMenu;
            Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("MENU_ID", strMenuID);

            ds = biz.GetMenuData(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                if (strTableName == "ErrorLog")
                {
                    strMessage = ds.Tables[0].Rows[0][1].ToString();

                    strResultData = "Error";
                }
                else if (ds.Tables[0].Rows.Count > 0)
                {
                    strResultData = ds.Tables[0].Rows[0]["MENU_NM"].ToString();
                }
            }
            return strResultData;
        }
        #endregion

    }
}