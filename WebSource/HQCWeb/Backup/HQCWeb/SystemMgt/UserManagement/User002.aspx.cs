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

namespace HQCWeb.SystemMgt.UserManagement
{
    public partial class User002 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        protected string strVal = string.Empty;

        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            cy.Key = strKey;

            if (!IsPostBack)
            {
                SetCon();

                SetTitle();

                if (Request.Form["hidValue"] != null)
                {
                    strVal = Request.Form["hidValue"].ToString();

                    (Master.FindControl("hidPopValue") as HiddenField).Value = strVal;

                    GetData();
                }
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            ddlUserDept.Items.Clear();

            ddlUserDept.Items.Add(new ListItem("선택하세요.", ""));

            DataSet ds = new DataSet();

            strDBName = "GPDB";
            strQueryID = "ComCodeData.Get_ComCodeByComTypeInfo";

            Biz.SystemManagement.ComCodeMgt biz = new Biz.SystemManagement.ComCodeMgt();

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("COMM_TYPE",     "USER_DEPT");
            sParam.Add("CUR_MENU_ID",   "WEB_00040");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            ds = biz.GetComCodeByComTypeInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlUserDept.Items.Add(new ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                }
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbUserID.Text   = Dictionary_Data.SearchDic("USER_ID", bp.g_language);
            lbUserNM.Text   = Dictionary_Data.SearchDic("USER_NM", bp.g_language);
            lbTel.Text      = Dictionary_Data.SearchDic("USER_TEL", bp.g_language);
            lbMobile.Text   = Dictionary_Data.SearchDic("USER_MOBILE", bp.g_language);
            lbEmail.Text    = Dictionary_Data.SearchDic("USER_EMAIL", bp.g_language);
            lbUserDept.Text = Dictionary_Data.SearchDic("USER_DEPT", bp.g_language);

            lbWorkName.Text = Dictionary_Data.SearchDic("DETAIL", bp.g_language);
        }
        #endregion

        #region GetData
        protected void GetData()
        {
            string strScript = string.Empty;
            
            strDBName = "GPDB";
            strQueryID = "UserInfoData.Get_UserInfo";

            DataSet ds = new DataSet();

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strDetailValue = string.Empty;

            // 검색조건 생성
            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("USER_ID",        strSplitValue[0].ToString());
            sParam.Add("CUR_MENU_ID",   "WEB_00040");

            Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

            ds = biz.GetUserInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                lbGetUserID.Text    = ds.Tables[0].Rows[0]["USER_ID"].ToString();
                txtUserNM.Text      = ds.Tables[0].Rows[0]["USER_NM"].ToString();
                txtTel.Text         = ds.Tables[0].Rows[0]["USER_TEL"].ToString();
                txtMobile.Text      = ds.Tables[0].Rows[0]["USER_MOBILE"].ToString();
                txtEmail.Text       = ds.Tables[0].Rows[0]["USER_EMAIL"].ToString();
                ddlUserDept.Text    = ds.Tables[0].Rows[0]["USER_DEPT"].ToString();

                // 변경전 데이터 셋팅
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    string strColumns = string.Empty;

                    strColumns = ds.Tables[0].Columns[i].ToString();

                    if (strDetailValue == "")
                    {
                        strDetailValue = strColumns + ":" + ds.Tables[0].Rows[0][strColumns].ToString();
                    }
                    else
                    {
                        strDetailValue += "," + strColumns + ":" + ds.Tables[0].Rows[0][strColumns].ToString();
                    }
                }

                (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = cy.Encrypt(strDetailValue);
            }
            else
            {
                strScript = " alert('사용자 정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('WEB_00040'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnModify_Click
        protected void btnModify_Click(object sender, EventArgs e)
        {
            string strRtn = string.Empty;
            int iRtn = 0;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strDValue = cy.Decrypt(strDVal).Split('/');

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                strDBName = "GPDB";
                strQueryID = "UserInfoData.Set_UserInfo";

                // 검색조건 생성
                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("USER_ID",       strValue[0].ToString());
                sParam.Add("USER_NM",       txtUserNM.Text);
                sParam.Add("USER_TEL",      txtTel.Text);
                sParam.Add("USER_MOBILE",   txtMobile.Text);
                sParam.Add("USER_EMAIL",    txtEmail.Text);
                sParam.Add("USER_PWD",      "");
                sParam.Add("USER_DEPT",     ddlUserDept.SelectedValue);

                sParam.Add("REG_ID",        bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE",      "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID",   "WEB_00040");              // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA",     strDValue[0].ToString());                  // 이전 데이터 셋팅

                Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

                iRtn = biz.SetUserInfo(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상 수정 되었습니다.');  parent.fn_ModalReloadCall('WEB_00040'); parent.fn_ModalCloseDiv(); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
                else
                {
                    strScript = " alert('수정에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
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
            string strRtn = string.Empty;
            int iRtn = 0;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strDValue = cy.Decrypt(strDVal).Split('/');

            strDBName = "GPDB";
            strQueryID = "UserInfoData.Set_UserInfoDel";            

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("USER_ID",       strValue[0].ToString());

            sParam.Add("REG_ID",        bp.g_userid.ToString());    // 등록자
            sParam.Add("CUD_TYPE",      "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            sParam.Add("CUR_MENU_ID",   "WEB_00040");              // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            sParam.Add("PREV_DATA",     strDValue[0].ToString());                  // 이전 데이터 셋팅

            Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

            iRtn = biz.DelUserInfo(strDBName, strQueryID ,sParam);

            if (iRtn == 1)
            {
                (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('WEB_00040'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                strScript = " alert('삭제에 실패하였습니다. 관리자에게 문의바랍니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnSendPWD_Click
        protected void btnSendPWD_Click(object sender, EventArgs e)
        {
            string strScript    = string.Empty;
            string strMobile    = string.Empty;
            int iRtn            = 0;
            
            strDBName = "GPDB";
            strQueryID = "UserInfoData.Get_UserInfo";
            
            DataSet ds = new DataSet();

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strDetailValue = string.Empty;

            // 검색조건 생성
            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("USER_ID",       strSplitValue[0].ToString());
            sParam.Add("CUR_MENU_ID",   "WEB_00040");

            Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

            ds = biz.GetUserInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                strMobile = ds.Tables[0].Rows[0]["USER_MOBILE"].ToString();

                // 임시비밀번호 생성
                string strTempPWD = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8).ToUpper();

                string strTempPWDRtn = biz.SendTempPWD(strMobile, strTempPWD);

                if (strTempPWDRtn == "OK") {
                    strQueryID = "UserInfoData.Set_UserTempPWDUpdate";

                    Biz.SystemManagement.UserMgt bizPWD = new Biz.SystemManagement.UserMgt();

                    sParam.Clear();
                    sParam.Add("USER_ID",       strSplitValue[0].ToString());
                    sParam.Add("TEMP_PWD",      cy.Hash(strTempPWD));

                    //sParam.Add("REG_ID",        bp.g_userid.ToString());    // 등록자
                    sParam.Add("REG_ID",        "JYJ");    // 등록자
                    sParam.Add("CUD_TYPE",      "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                    sParam.Add("CUR_MENU_ID",   "WEB_00040");              // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                    iRtn = bizPWD.SetUserTempPWDUpdate(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        strScript = " alert('임시비밀번호가 발급되었습니다.'); parent.fn_ModalReloadCall('WEB_00040'); parent.fn_ModalCloseDiv(); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else {
                        strScript = " alert('임시비밀번호가 발급되었습니다. 다시 시도하시기 바랍니다.'); parent.fn_ModalReloadCall('WEB_00040'); parent.fn_ModalCloseDiv(); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                }
                else {
                    strScript = " alert('임시비밀번호 발급에 실패하였습니다." + strTempPWDRtn + ". 다시 시도하시기 바랍니다.'); parent.fn_ModalReloadCall('WEB_00040'); parent.fn_ModalCloseDiv(); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
                
            }
            else
            {
                strScript = " alert('사용자 정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('WEB_00040'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion
    }
}