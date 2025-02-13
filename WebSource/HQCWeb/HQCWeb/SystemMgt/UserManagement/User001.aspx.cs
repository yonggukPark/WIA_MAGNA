using HQCWeb.FMB_FW;
using HQCWeb.FW;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MES.FW.Common.CommonMgt;
using MES.FW.Common.Crypt;

using System.Data;

namespace HQCWeb.SystemMgt.UserManagement
{
    public partial class User001 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetCon();

                SetTitle();
            }
        }
        #endregion

        #region SetCon
        private void SetCon() {
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
            lbUserID.Text   = Dictionary_Data.SearchDic("USER_ID",      bp.g_language);
            lbUserNM.Text   = Dictionary_Data.SearchDic("USER_NM",      bp.g_language);
            lbTel.Text      = Dictionary_Data.SearchDic("USER_TEL",     bp.g_language);
            lbMobile.Text   = Dictionary_Data.SearchDic("USER_MOBILE",  bp.g_language);
            lbEmail.Text    = Dictionary_Data.SearchDic("USER_EMAIL",   bp.g_language);
            lbUserDept.Text = Dictionary_Data.SearchDic("USER_DEPT",    bp.g_language);

            lbWorkName.Text = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language);
        }
        #endregion

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strRtn = string.Empty;
            int iRtn = 0;
            string strRtnValChk = string.Empty;
            string strScript = string.Empty;
            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

                strDBName = "GPDB";
                strQueryID = "UserInfoData.Get_UserID_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("USER_ID", txtUserID.Text);

                // 임시비밀번호 생성
                string strTempPWD = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8).ToUpper();
                
                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("USER_ID",       txtUserID.Text);
                sParam.Add("USER_NM",       txtUserNM.Text);
                sParam.Add("USER_TEL",      txtTel.Text);
                sParam.Add("USER_MOBILE",   txtMobile.Text);
                sParam.Add("USER_EMAIL",    txtEmail.Text);
                sParam.Add("USER_PWD",      cy.Hash(strTempPWD));
                sParam.Add("USER_DEPT",     ddlUserDept.SelectedValue);

                sParam.Add("PLANT_CD",      bp.g_plant.ToString());     // 공장코드
                sParam.Add("REG_ID",        bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE",      "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID",   "WEB_00040");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                
                strRtn = biz.GetUserIDValChk(strDBName, strQueryID, sParamIDChk);

                if (strRtn == "0")
                {
                    string strTempPWDRtn = biz.SendTempPWD(txtMobile.Text, strTempPWD);

                    if (strTempPWDRtn == "OK")
                    {
                        strQueryID = "UserInfoData.Set_UserInfo";
                        iRtn = biz.SetUserInfo(strDBName, strQueryID, sParam);

                        if (iRtn == 1)
                        {
                            (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                            strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('WEB_00040'); parent.fn_ModalCloseDiv(); ";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        }
                        else
                        {
                            strScript = " alert('등록에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        }
                    }
                    else {
                        strScript = " alert('임시비밀번호 발급이 실패하였습니다. 관리자에게 문의바립니다.'); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                }
                else
                {
                    strScript = " alert('존재하는 아이디 입니다. 등록하려는 아이디를 다시 입력하세요.'); ";
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

    }
}