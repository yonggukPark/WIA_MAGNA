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

namespace HQCWeb.SystemMgt.ButtonManagement
{
    public partial class Button002 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        protected string strVal = string.Empty;

        // 암복호화 키값 셋팅
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
            DataSet ds = new DataSet();

            strDBName = "GPDB";
            strQueryID = "ComCodeData.Get_ComCodeByComTypeInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("COMM_TYPE", "USE_YN");

            sParam.Add("CUR_MENU_ID", "WEB_00080");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            Biz.SystemManagement.ComCodeMgt biz = new Biz.SystemManagement.ComCodeMgt();

            ds = biz.GetComCodeByComTypeInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlUseYN.Items.Add(new ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                }
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbButtonID.Text         = Dictionary_Data.SearchDic("BUTTON_ID", bp.g_language);
            lbButtonKR.Text         = Dictionary_Data.SearchDic("BUTTON_TXT_KR", bp.g_language);
            lbButtonEN.Text         = Dictionary_Data.SearchDic("BUTTON_TXT_EN", bp.g_language);
            lbButtonLO.Text         = Dictionary_Data.SearchDic("BUTTON_TXT_LO", bp.g_language);
            lbButtonFunction.Text   = Dictionary_Data.SearchDic("BUTTON_FUNCTION", bp.g_language);
            lbButtonIDX.Text        = Dictionary_Data.SearchDic("BUTTON_IDX", bp.g_language);
            lbUseYN.Text            = Dictionary_Data.SearchDic("USE_YN", bp.g_language);
            
            // 상세내용 확인후 수정 또는 삭제일 경우
            lbWorkName.Text = Dictionary_Data.SearchDic("DETAIL", bp.g_language); // 상세
        }
        #endregion

        #region GetData
        protected void GetData()
        {
            string strScript = string.Empty;

            DataSet ds = new DataSet();

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            // 비지니스 클래스 작성
            Biz.SystemManagement.ButtonMgt biz = new Biz.SystemManagement.ButtonMgt();
            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "ButtonData.Get_ButtonInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("BUTTON_ID",     strSplitValue[0].ToString());

            sParam.Add("CUR_MENU_ID",   "WEB_00080");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetButtonInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                lbGetButtonID.Text      = ds.Tables[0].Rows[0]["BUTTON_ID"].ToString();
                txtButtonKR.Text        = ds.Tables[0].Rows[0]["BUTTON_TXT_KR"].ToString();
                txtButtonEN.Text        = ds.Tables[0].Rows[0]["BUTTON_TXT_EN"].ToString();
                txtButtonLO.Text        = ds.Tables[0].Rows[0]["BUTTON_TXT_LO"].ToString();
                txtButtonFunction.Text  = ds.Tables[0].Rows[0]["BUTTON_FUNCTION"].ToString();
                txtButtonIDX.Text       = ds.Tables[0].Rows[0]["BUTTON_IDX"].ToString();
                ddlUseYN.SelectedValue  = ds.Tables[0].Rows[0]["USE_YN"].ToString();

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
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('WEB_00080'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion
        
        #region btnModify_Click
        protected void btnModify_Click(object sender, EventArgs e)
        {
            int iRtn = 0;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strDValue = cy.Decrypt(strDVal).Split('/');

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                int iMenuIDX = 0;

                if (txtButtonIDX.Text == "")
                {
                    iMenuIDX = 0;
                }
                else
                {
                    iMenuIDX = Convert.ToInt32(txtButtonIDX.Text);
                }

                // 비지니스 클래스 작성
                Biz.SystemManagement.ButtonMgt biz = new Biz.SystemManagement.ButtonMgt();

                strDBName = "GPDB";
                strQueryID = "ButtonData.Set_ButtonInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("BUTTON_ID",         strValue[0].ToString());
                sParam.Add("BUTTON_TXT_KR",     txtButtonKR.Text);
                sParam.Add("BUTTON_TXT_EN",     txtButtonEN.Text);
                sParam.Add("BUTTON_TXT_LO",     txtButtonLO.Text);
                sParam.Add("BUTTON_FUNCTION",   txtButtonFunction.Text);
                sParam.Add("BUTTON_IDX",        iMenuIDX.ToString());
                sParam.Add("USE_YN",            ddlUseYN.SelectedValue);

                sParam.Add("REG_ID",            bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE",          "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID",       "WEB_00080");               // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA",         strDValue[0].ToString());                  // 이전 데이터 셋팅

                // 수정 비지니스 메서드 작성
                iRtn = biz.SetButtonInfo(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall('WEB_00080'); parent.fn_ModalCloseDiv(); ";
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
            int iRtn = 0;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strDValue = cy.Decrypt(strDVal).Split('/');

            // 비지니스 클래스 작성
            Biz.SystemManagement.ButtonMgt biz = new Biz.SystemManagement.ButtonMgt();

            strDBName = "GPDB";
            strQueryID = "ButtonData.Set_ButtonInfoDel";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("BUTTON_ID",     strValue[0].ToString());

            sParam.Add("REG_ID",        bp.g_userid.ToString());    // 등록자
            sParam.Add("CUD_TYPE",      "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            sParam.Add("CUR_MENU_ID",   "WEB_00080");              // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            sParam.Add("PREV_DATA",     strDValue[0].ToString());                  // 이전 데이터 셋팅

            // 삭제 비지니스 메서드 작성
            iRtn = biz.DelButtonInfo(strDBName, strQueryID, sParam);

            if (iRtn == 1)
            {
                (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('WEB_00080'); parent.fn_ModalCloseDiv(); ";
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
