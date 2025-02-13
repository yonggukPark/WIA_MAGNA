using HQC.FW.Common;
using HQCWeb.FMB_FW;
using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using MES.FW.Common.Crypt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HQCWeb.SystemMgt.MenuManagement
{
    public partial class Menu003 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        protected string strVal = string.Empty;

        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            cy.Key = strKey;

            if (!IsPostBack)
            {
                SetTitle();

                SetControl();

                if (Request.Form["hidValue"] != null)
                {
                    strVal = Request.Form["hidValue"].ToString();

                    (Master.FindControl("hidPopValue") as HiddenField).Value = strVal;

                    GetData();
                }
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbMenuID.Text       = Dictionary_Data.SearchDic("MENU_ID", bp.g_language);
            lbMenuNM.Text       = Dictionary_Data.SearchDic("MENU_NM", bp.g_language);
            lbMenuParentID.Text = Dictionary_Data.SearchDic("MENU_PARENT_ID", bp.g_language);
            lbMenuLevel.Text    = Dictionary_Data.SearchDic("MENU_LEVEL", bp.g_language);
            lbAssamblyID.Text   = Dictionary_Data.SearchDic("ASSEMBLY_ID", bp.g_language);
            lbViewID.Text       = Dictionary_Data.SearchDic("VIEW_ID", bp.g_language);
            lbWorkName.Text     = Dictionary_Data.SearchDic("DETAIL", bp.g_language);

            lbPopupYN.Text      = Dictionary_Data.SearchDic("POPUP_YN", bp.g_language);
            lbUseYN.Text        = Dictionary_Data.SearchDic("USE_YN", bp.g_language);
            lbApproval.Text     = Dictionary_Data.SearchDic("APPROVAL_FLAG", bp.g_language);
            lbMenuIDX.Text      = Dictionary_Data.SearchDic("MENU_IDX", bp.g_language);
        }
        #endregion

        #region SetControl
        private void SetControl() {

            ddlUseYN.Items.Clear();
            ddlApprovalYN.Items.Clear();

            ddlUseYN.Items.Add(new ListItem("예", "Y"));
            ddlUseYN.Items.Add(new ListItem("아니오", "N"));

            ddlPopupYN.Items.Add(new ListItem("아니오", "N"));
            ddlPopupYN.Items.Add(new ListItem("예", "Y"));

            ddlApprovalYN.Items.Add(new ListItem("예", "Y"));
            ddlApprovalYN.Items.Add(new ListItem("아니오", "N"));

            ddlUseYN.SelectedIndex = 0;
            ddlApprovalYN.SelectedIndex = 1;

            string strScript = string.Empty;

            DataSet ds = new DataSet();

            // 서비스 클래스 작성
            Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            strDBName = "GPDB";
            strQueryID = "MenuData.Get_MenuLevel";

            ds = biz.GetMenuLevel(strDBName, strQueryID);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {
                    ddlMenuLevel.Items.Add(new ListItem(ds.Tables[0].Rows[i][0].ToString(), ds.Tables[0].Rows[i][0].ToString()));
                }
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); opener.fn_parentReload(); window.close(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region GetData
        protected void GetData()
        {
            string strScript = string.Empty;

            DataSet ds = new DataSet();

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;
            
            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            // 서비스 클래스 작성
            Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();
            string strDBName = string.Empty;
            string strQueryID = string.Empty;
            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "MenuData.Get_MenuInfo";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("MENU_ID", strSplitValue[0].ToString());

            ds = biz.GetMenuData(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                lbGetMenuID.Text            = ds.Tables[0].Rows[0]["MENU_ID"].ToString();
                txtMenuNM.Text              = ds.Tables[0].Rows[0]["MENU_NM"].ToString().Split('(')[0];
                txtMenuParentID.Text        = ds.Tables[0].Rows[0]["MENU_PARENT_ID"].ToString().Replace(" ", "");
                ddlMenuLevel.SelectedValue  = ds.Tables[0].Rows[0]["MENU_LEVEL"].ToString();

                hidMenuLevel.Value          = ds.Tables[0].Rows[0]["MENU_LEVEL"].ToString();

                txtAssamblyID.Text          = ds.Tables[0].Rows[0]["ASSEMBLY_ID"].ToString();
                txtViewID.Text              = ds.Tables[0].Rows[0]["VIEW_ID"].ToString();

                ddlUseYN.SelectedValue      = ds.Tables[0].Rows[0]["USE_YN"].ToString();
                ddlPopupYN.SelectedValue    = ds.Tables[0].Rows[0]["POPUP_YN"].ToString();
                ddlApprovalYN.SelectedValue = ds.Tables[0].Rows[0]["APPROVAL_FLAG"].ToString();
                txtMenuIDX.Text             = ds.Tables[0].Rows[0]["MENU_IDX"].ToString();

                // 변경전 데이터 셋팅
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++) {
                    string strColumns = string.Empty;

                    strColumns = ds.Tables[0].Columns[i].ToString();

                    if (strDetailValue == "")
                    {
                        strDetailValue = strColumns + " : " + ds.Tables[0].Rows[0][strColumns].ToString();
                    }
                    else {
                        strDetailValue += "," + strColumns + " : " + ds.Tables[0].Rows[0][strColumns].ToString();
                    }
                }

                (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = cy.Encrypt(strDetailValue);
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('WEB_00010'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnModify_Click
        protected void btnModify_Click(object sender, EventArgs e)
        {
            string strRtn = string.Empty;
            int iRtn = 0;
            string strRtnValChk = string.Empty;
            string strScript = string.Empty;

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strDValue = cy.Decrypt(strDVal).Split('/');

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                int iMenuIDX = 0;

                if (txtMenuIDX.Text == "")
                {
                    iMenuIDX = 0;
                }
                else
                {
                    iMenuIDX = Convert.ToInt32(txtMenuIDX.Text);
                }

                FW.Data.Parameters paramParentID = new FW.Data.Parameters();
                paramParentID.Add("MENU_PARENT_ID", txtMenuParentID.Text);

                FW.Data.Parameters param = new FW.Data.Parameters();

                param.Add("MENU_ID",        strValue[0].ToString());
                param.Add("MENU_NM",        txtMenuNM.Text);
                param.Add("MENU_PARENT_ID", txtMenuParentID.Text);
                param.Add("MENU_LEVEL",     ddlMenuLevel.SelectedValue);
                param.Add("MENU_IDX",       txtMenuIDX.Text);
                param.Add("ASSEMBLY_ID",    txtAssamblyID.Text);
                param.Add("VIEW_ID",        txtViewID.Text);
                param.Add("USE_YN",         ddlUseYN.SelectedValue);
                param.Add("POPUP_YN",       ddlPopupYN.SelectedValue);
                param.Add("APPROVAL_FLAG",  ddlApprovalYN.SelectedValue);

                param.Add("REG_ID",         bp.g_userid.ToString());
                param.Add("CUD_TYPE",       "U");
                param.Add("CUR_MENU_ID",    "WEB_00010");
                param.Add("PREV_DATA",      strDValue[0].ToString());

                // 수정 서비스 클래스 작성
                Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

                strDBName = "GPDB";
                strQueryID = "MenuData.Get_ParentMenuLevel";


                // 부모 아이디 Level 조회
                strRtn = biz.GetParentMenuLevel(strDBName, strQueryID, paramParentID);

                if (ddlMenuLevel.SelectedValue == "1")
                {
                    strRtn = "1";
                }

                if (strRtn == "0")
                {
                    strScript = " alert('입력한 부모 메뉴 아이디가 존재하지 않습니다. 다시 확인하세요.'); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
                else
                {
                    if (Convert.ToInt32(strRtn) < (Convert.ToInt32(ddlMenuLevel.SelectedValue) - 1))
                    {
                        ddlMenuLevel.SelectedValue = hidMenuLevel.Value;

                        strScript = " alert('부모 메뉴 Level은 " + Convert.ToInt32(strRtn) + " 입니다.  등록하려는 Level과 연관성이 없습니다. 다시 확인하세요.'); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else
                    {
                        strDBName = "GPDB";
                        strQueryID = "MenuData.Set_MenuInfo";

                        // 수정 서비스 작성
                        iRtn = biz.SetMenuInfo(strDBName, strQueryID, param);

                        if (iRtn == 1)
                        {
                            (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                            strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall('WEB_00010'); parent.fn_ModalCloseDiv(); ";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        }
                        else
                        {
                            strScript = " alert('수정에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        }
                    }
                }
            }
            else
            {
                strScript = " fn_ExError(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }            
        }
        #endregion

        #region btnDelete_Click
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int iRtn = 0;
            string strRtnValChk = string.Empty;
            string strScript = string.Empty;

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strDValue = cy.Decrypt(strDVal).Split('/');

            strDBName = "GPDB";
            strQueryID = "MenuData.Set_MenuInfoDel";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("MENU_ID",        strValue[0].ToString());

            param.Add("REG_ID",         bp.g_userid.ToString());
            param.Add("CUD_TYPE",       "D");
            param.Add("CUR_MENU_ID",    "WEB_00010");
            param.Add("PREV_DATA",      strDValue[0].ToString());

            // 삭제 서비스 클래스 작성
            Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

            // 호출 서비스 작성
            iRtn = biz.DelMenuInfo(strDBName, strQueryID, param);

            if (iRtn == 1)
            {
                strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('WEB_00010'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                strScript = " alert('삭제에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion
    }
}