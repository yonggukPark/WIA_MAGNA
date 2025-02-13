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
    public partial class Button001 : System.Web.UI.Page
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
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {
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

            // 등록일경우
            lbWorkName.Text = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language); // 등록            
        }
        #endregion
        
        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strRtn = string.Empty;
            int iRtn = 0;            
            string strScript = string.Empty;

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                // 비지니스 클래스 작성
                Biz.SystemManagement.ButtonMgt biz = new Biz.SystemManagement.ButtonMgt();

                strDBName = "GPDB";
                strQueryID = "ButtonData.Get_ButtonID_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("BUTTON_ID", txtButtonID.Text);

                // 아이디 체크 비지니스 메서드 작성
                strRtn = biz.GetButtonIDValChk(strDBName, strQueryID, sParamIDChk);
                if (iRtn == 0)
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

                    strDBName = "GPDB";
                    strQueryID = "ButtonData.Set_ButtonInfo";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("BUTTON_ID", txtButtonID.Text);
                    sParam.Add("BUTTON_TXT_KR", txtButtonKR.Text);
                    sParam.Add("BUTTON_TXT_EN", txtButtonEN.Text);
                    sParam.Add("BUTTON_TXT_LO", txtButtonLO.Text);
                    sParam.Add("BUTTON_FUNCTION", txtButtonFunction.Text);
                    sParam.Add("BUTTON_IDX", iMenuIDX.ToString());
                    sParam.Add("USE_YN", ddlUseYN.SelectedValue);

                    sParam.Add("REG_ID",            bp.g_userid.ToString());    // 등록자
                    sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                    sParam.Add("CUR_MENU_ID", "WEB_00080");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                    // 등록 비지니스 메서드 작성
                    iRtn = biz.SetButtonInfo(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('WEB_00080'); parent.fn_ModalCloseDiv(); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else
                    {
                        strScript = " alert('등록에 실패하였습니다. 관리자에게 문의바립니다.'); ";
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

