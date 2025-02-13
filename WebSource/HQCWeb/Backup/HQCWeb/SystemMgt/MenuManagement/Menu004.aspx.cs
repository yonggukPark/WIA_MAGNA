using HQCWeb.FMB_FW;
using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using MES.FW.Common.Crypt;
using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.RightsManagement;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HQCWeb.SystemMgt.MenuManagement
{
    public partial class Menu004 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();

        protected string strVal = string.Empty;

        protected string strBtnList = string.Empty;

        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        // 비지니스 클래스 작성

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            cy.Key = strKey;

            if (!IsPostBack)
            {
                SetCon();

                SetTitle();
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            ddl_Category_B.Items.Add(new ListItem("선택하세요", ""));
            ddl_Category_M.Items.Add(new ListItem("선택하세요", ""));
            ddl_Category_S.Items.Add(new ListItem("선택하세요", ""));

            Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

            DataSet ds = new DataSet();

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            strDBName = "GPDB";
            strQueryID = "MenuData.Get_MenuIDByDepthInfo";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("MENU_LEVEL",         "1");
            param.Add("MENU_PARENT_ID",     "");

            ds = biz.GetMenuDepthInfo(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddl_Category_B.Items.Add(new ListItem(ds.Tables[0].Rows[i]["MENU_NM"].ToString(), ds.Tables[0].Rows[i]["MENU_ID"].ToString()));
                }
            }

            SetButtonSetting("");
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbMenu_B.Text       = Dictionary_Data.SearchDic("CATEGORY_B", bp.g_language);
            lbMenu_M.Text       = Dictionary_Data.SearchDic("CATEGORY_M", bp.g_language);
            lbMenu_S.Text       = Dictionary_Data.SearchDic("CATEGORY_S", bp.g_language);

            lbConID.Text        = Dictionary_Data.SearchDic("CONTROL_ID", bp.g_language);
            lbConName.Text      = Dictionary_Data.SearchDic("CONTROL_NM", bp.g_language);
            lbFunction.Text     = Dictionary_Data.SearchDic("CONTROL_FUNCTION", bp.g_language);

            lbButtonList.Text   = Dictionary_Data.SearchDic("BUTTION_LIST", bp.g_language);

        }
        #endregion

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int iRtn = 0;
            string strRtnValChk = string.Empty;
            string strScript = string.Empty;

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            strDBName = "GPDB";
            strQueryID = "MenuData.Set_MenuControl";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("INSERT_INFO",    txtInsertInfo.Text);
            param.Add("MENU_ID",        lbSelectMenuID.Text);
            param.Add("REG_ID",         bp.g_userid.ToString());
            param.Add("CUD_TYPE",       "C");
            param.Add("CUR_MENU_ID",    "WEB_00010");

            if (txtInsertInfo.Text == "")
            {
                strScript = " alert('등록정보가 올바르지 않습니다. 관리자에게 문의바랍니다.'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else {
                Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

                //  등록 서비스 작성
                iRtn = biz.SetMenuControl(strDBName, strQueryID, param);

                if (iRtn == 1)
                {
                    strScript = " alert('정상등록 되었습니다.'); parent.fn_ModalReloadCall('WEB_00010'); parent.fn_ModalCloseDiv(); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
                else
                {
                    strScript = " alert('등록에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
            }
        }
        #endregion

        #region ddl_Category_B_SelectedIndexChanged
        protected void ddl_Category_B_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbSelectMenuID.Text = "";

            ddl_Category_M.Items.Clear();
            ddl_Category_S.Items.Clear();

            ddl_Category_M.Enabled = false;
            ddl_Category_S.Enabled = false;

            ddl_Category_M.Items.Add(new ListItem("선택하세요", ""));
            ddl_Category_S.Items.Add(new ListItem("선택하세요", ""));

            Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

            DataSet ds = new DataSet();

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            strDBName = "GPDB";
            strQueryID = "MenuData.Get_MenuIDByDepthInfo";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("MENU_LEVEL",     "2");
            param.Add("MENU_PARENT_ID", ddl_Category_B.SelectedValue);

            ds = biz.GetMenuDepthInfo(strDBName, strQueryID, param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Category_M.Enabled = true;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddl_Category_M.Items.Add(new ListItem(ds.Tables[0].Rows[i]["MENU_NM"].ToString(), ds.Tables[0].Rows[i]["MENU_ID"].ToString()));
                }

                SetButtonList("");
                SetButtonSetting("");
            }
            else
            {
                SetButtonList(ddl_Category_B.SelectedValue);
            }
        }
        #endregion

        #region ddl_Category_M_SelectedIndexChanged
        protected void ddl_Category_M_SelectedIndexChanged(object sender, EventArgs e)
        {
            lbSelectMenuID.Text = "";

            Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

            DataSet ds = new DataSet();


            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            strDBName = "GPDB";
            strQueryID = "MenuData.Get_MenuIDByDepthInfo";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("MENU_LEVEL",     "3");
            param.Add("MENU_PARENT_ID", ddl_Category_M.SelectedValue);

            ds = biz.GetMenuDepthInfo(strDBName, strQueryID, param);

            if (ds.Tables[0].Rows.Count > 0)
            {
                ddl_Category_S.Enabled = true;

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddl_Category_S.Items.Add(new ListItem(ds.Tables[0].Rows[i]["MENU_NM"].ToString(), ds.Tables[0].Rows[i]["MENU_ID"].ToString()));
                }

                SetButtonList("");
                SetButtonSetting("");
            }
            else {
                SetButtonList(ddl_Category_M.SelectedValue);
            }
        }
        #endregion

        #region ddl_Category_S_SelectedIndexChanged
        protected void ddl_Category_S_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetButtonList(ddl_Category_S.SelectedValue);
        }
        #endregion

        #region SetButtonList
        private void SetButtonList(string strMenuID) {
            DataSet ds = new DataSet();

            string strDBName = string.Empty;
            string strQueryID = string.Empty;
            string strConList = string.Empty;

            strDBName = "GPDB";
            strQueryID = "MenuData.Get_MenuByControlInfo";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("MENU_ID", strMenuID);
            
            Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

            ds = biz.GetMenuControlInfo(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strBtnList += "<tr style=\"height=30px;\" id='tr" + ds.Tables[0].Rows[i]["CONTROL_ID"].ToString() + "'> ";
                        strBtnList += "   <td style=\"text-align:left; padding-left:5px;\">" + ds.Tables[0].Rows[i]["CONTROL_ID"].ToString() + "</td>";
                        strBtnList += "   <td style=\"text-align:left; padding-left:5px;\">" + ds.Tables[0].Rows[i]["CONTROL_NM"].ToString() + "</td>";
                        strBtnList += "   <td style=\"text-align:left; padding-left:5px;\">" + ds.Tables[0].Rows[i]["CONTROL_FUNCTION"].ToString() + "</td> ";
                        strBtnList += "   <td><img src=\"../../images/btn/close_on.gif\" style=\"padding-bottom:2px;\" onclick=\"javascript:fn_del('tr" + ds.Tables[0].Rows[i]["CONTROL_ID"].ToString() + "');\" /></td>";
                        strBtnList += "</tr> ";

                        if (strConList == "") {
                            strConList = ds.Tables[0].Rows[i]["CONTROL_ID"].ToString();
                        }
                        else
                        {
                            strConList += "," + ds.Tables[0].Rows[i]["CONTROL_ID"].ToString();
                        }
                    }
                }
            }

            SetButtonSetting(strConList);

            lbSelectMenuID.Text = strMenuID;

            txtBtnList.Text = strBtnList;

            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_BtnListSet();", true);
        }
        #endregion

        #region SetButtonSetting
        private void SetButtonSetting(string strBtnIDs)
        {
            ltButtonList.Text = "";
            
            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            DataSet ds = new DataSet();

            Biz.SystemManagement.ButtonMgt bizBtn = new Biz.SystemManagement.ButtonMgt();

            strDBName = "GPDB";
            strQueryID = "ButtonData.Get_ButtonListInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("CUR_MENU_ID", "WEB_00010");

            ds.Clear();

            ds = bizBtn.GetButtonListInfo(strDBName, strQueryID, null);

            string strButtonNM = string.Empty;
            string strButtonList = string.Empty;

            string strButtonID = string.Empty;
            string strButtonFunction = string.Empty;

            string strBr = string.Empty;

            int iRowCnt = 0;

            if (strBtnIDs == "")
            {
                if (ds.Tables.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        /*
                        if (bp.g_language == "KO_KR") {
                            strButtonNM = "BUTTON_TXT_KR";
                        }

                        if (bp.g_language == "EO_EN")
                        {
                            strButtonNM = "BUTTON_TXT_EN";
                        }

                        if (bp.g_language == "LO_LN")
                        {
                            strButtonNM = "BUTTON_TXT_LO";
                        }

                        chlButtonList.Items.Add(new ListItem(ds.Tables[0].Rows[i][strButtonNM].ToString(), ds.Tables[0].Rows[i]["BUTTON_ID"].ToString()));
                        */

                        strButtonID = ds.Tables[0].Rows[i]["BUTTON_ID"].ToString();
                        strButtonNM = ds.Tables[0].Rows[i]["BUTTON_TXT_KR"].ToString();
                        strButtonFunction = ds.Tables[0].Rows[i]["BUTTON_FUNCTION"].ToString();

                        if (iRowCnt == 6)
                        {
                            strBr = "<br>";

                            iRowCnt = 0;
                        }

                        strButtonList += strBr + "<input type='checkbox' id=\'" + strButtonID + "\' onclick=\"javascript:fn_AutoAdd('" + strButtonID + "', '" + strButtonNM + "', '" + strButtonFunction + "');\" /> " + "&nbsp;&nbsp;" + strButtonNM + "&nbsp;&nbsp;";

                        iRowCnt++;
                        
                        strBr = "";
                    }
                }
            }
            else {
                if (ds.Tables.Count > 0)
                {
                    string[] arrID = strBtnIDs.Split(',');
                    string strCheck = string.Empty;
                    

                    iRowCnt = 0;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strButtonID         = ds.Tables[0].Rows[i]["BUTTON_ID"].ToString();
                        strButtonNM         = ds.Tables[0].Rows[i]["BUTTON_TXT_KR"].ToString();
                        strButtonFunction   = ds.Tables[0].Rows[i]["BUTTON_FUNCTION"].ToString();

                        for (int j = 0; j < arrID.Length; j++)
                        {
                            /*
                            if (bp.g_language == "KO_KR") {
                                strButtonNM = "BUTTON_TXT_KR";
                            }

                            if (bp.g_language == "EO_EN")
                            {
                                strButtonNM = "BUTTON_TXT_EN";
                            }

                            if (bp.g_language == "LO_LN")
                            {
                                strButtonNM = "BUTTON_TXT_LO";
                            }

                            chlButtonList.Items.Add(new ListItem(ds.Tables[0].Rows[i][strButtonNM].ToString(), ds.Tables[0].Rows[i]["BUTTON_ID"].ToString()));
                            */

                            if (strButtonID == arrID[j].ToString())
                            {
                                strCheck = "Y";
                            }
                        }
                        
                        if (iRowCnt == 6)
                        {
                            strBr = "<br>";

                            iRowCnt = 0;
                        }

                        if (strCheck == "Y")
                        {
                            strButtonList += strBr + "<input type='checkbox' checked='checked' id=\'" + strButtonID + "\' onclick=\"javascript:fn_AutoAdd('" + strButtonID + "', '" + strButtonNM + "', '" + strButtonFunction + "');\" /> " + "&nbsp;&nbsp;" + strButtonNM + "&nbsp;&nbsp;";

                            strCheck = "N";
                        }
                        else
                        {
                            strButtonList += strBr + "<input type='checkbox' id=\'" + strButtonID + "\' onclick=\"javascript:fn_AutoAdd('" + strButtonID + "', '" + strButtonNM + "', '" + strButtonFunction + "');\" /> " + "&nbsp;&nbsp;" + strButtonNM + "&nbsp;&nbsp;";
                        }

                        iRowCnt++;

                        strBr = "";
                    }
                }                
            }

            ltButtonList.Text = strButtonList;

        }
        #endregion


    }
}