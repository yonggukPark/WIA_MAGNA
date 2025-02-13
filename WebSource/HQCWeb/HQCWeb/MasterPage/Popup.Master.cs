using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MES.FW.Common.CommonMgt;

namespace HQCWeb.MasterPage
{
	public partial class Popup : System.Web.UI.MasterPage
	{
		BasePage bp = new BasePage();

		#region Page_Init
		protected void Page_Init(object sender, EventArgs e)
		{
            //세션체크
            if (bp.g_userid.ToString() == "")
            {
                // 세션 초기화
                Session.Clear();
                Session.Abandon();

                // 세션 쿠키 제거
                if (Request.Cookies["ASP.NET_SessionId"] != null)
                {
                    HttpCookie cookie = new HttpCookie("ASP.NET_SessionId");
                    cookie.Expires = DateTime.Now.AddDays(-1); // 쿠키 만료
                    Response.Cookies.Add(cookie);
                }

                // 프레임 전체 새로고침을 위한 JavaScript 삽입
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "RefreshFrames", "parent.location.reload(true);", true);
            }
            else
            {
                Label lbTitle = (Label)PopupContent.FindControl("lbTitle");

                if (lbTitle != null)
                {
                    //세션에 메뉴코드 저장
                    Session["MenuCode"] = lbTitle.Text;

                    string strMenuID = string.Empty;

                    strMenuID = lbTitle.Text;

                    DataSet ds = new DataSet();

                    Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

                    string strDBName = string.Empty;
                    string strQueryID = string.Empty;

                    strDBName = "GPDB";
                    strQueryID = "MenuData.Get_MenuInfo";

                    FW.Data.Parameters param = new FW.Data.Parameters();
                    param.Add("MENU_ID", lbTitle.Text);

                    ds = biz.GetMenuData(strDBName, strQueryID, param);

                    if (ds.Tables.Count > 0)
                    {
                        if (ds.Tables[0].Rows.Count > 0)
                        {
                            lbTitle.Text = ds.Tables[0].Rows[0]["MENU_NM"].ToString();
                        }
                    }

                    SetContorl(strMenuID);
                }
            }
        }
		#endregion

		#region Page_Load
		protected void Page_Load(object sender, EventArgs e)
		{
            //lbUserInfo.Text = bp.g_userid.ToString() + " 반갑습니다.";

            //DevExpress.Web.ASPxWebControl.GlobalTheme = "Aqua";

            if (bp.g_userid.ToString() == "")
            {
                Response.Write("<script>opener.location.href='/login.aspx'; window.close();</script>");
                Response.End();
            }


            if (!IsPostBack)
			{
				PageInit();
			}
		}
		#endregion

		#region PageInit
		private void PageInit()
		{

		}
        #endregion

        #region Page_PreRender
        protected void Page_PreRender(object sender, EventArgs e)
        {
            string strMenuID = Session["MenuCode"].ToString();
            if (strMenuID != null)
            {
                //권한 강제조정(권한이 없는데 true로 바뀌는 팝업들이 있음)
                SetContorlRender(strMenuID);
            }
        }
        #endregion

        #region SetContorl
        public void SetContorl(string strMenuID)
        {

            string strBtnID = string.Empty;
            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            //System.Web.UI.WebControls.Button btnSearch = (System.Web.UI.WebControls.Button)PopupContent.FindControl("btnSearch");
            //System.Web.UI.WebControls.Button btnExcel = (System.Web.UI.WebControls.Button)MainContent.FindControl("btnExcel");

            System.Web.UI.HtmlControls.HtmlAnchor btnSave       = (System.Web.UI.HtmlControls.HtmlAnchor)PopupContent.FindControl("aSave");
            System.Web.UI.HtmlControls.HtmlAnchor btnModify     = (System.Web.UI.HtmlControls.HtmlAnchor)PopupContent.FindControl("aModify");
            System.Web.UI.HtmlControls.HtmlAnchor btnDelete     = (System.Web.UI.HtmlControls.HtmlAnchor)PopupContent.FindControl("aDelete");
            System.Web.UI.HtmlControls.HtmlAnchor btnTempPWD    = (System.Web.UI.HtmlControls.HtmlAnchor)PopupContent.FindControl("aTempPWD");
            System.Web.UI.HtmlControls.HtmlAnchor btnConfirm    = (System.Web.UI.HtmlControls.HtmlAnchor)PopupContent.FindControl("aConfirm");

            //System.Web.UI.HtmlControls.HtmlInputButton btnExcel     = (System.Web.UI.HtmlControls.HtmlInputButton)PopupContent.FindControl("btnExcel");
            //System.Web.UI.HtmlControls.HtmlInputButton btnNew       = (System.Web.UI.HtmlControls.HtmlInputButton)PopupContent.FindControl("btnNew");
            //System.Web.UI.HtmlControls.HtmlInputButton btnBCate     = (System.Web.UI.HtmlControls.HtmlInputButton)PopupContent.FindControl("btnBCate");
            //System.Web.UI.HtmlControls.HtmlInputButton btnGCate     = (System.Web.UI.HtmlControls.HtmlInputButton)PopupContent.FindControl("btnGCate");
            //System.Web.UI.HtmlControls.HtmlInputButton btnFunction  = (System.Web.UI.HtmlControls.HtmlInputButton)PopupContent.FindControl("btnFunction");
            //System.Web.UI.HtmlControls.HtmlInputButton btnRestore   = (System.Web.UI.HtmlControls.HtmlInputButton)PopupContent.FindControl("btnRestore");

            DataSet ds = new DataSet();

            Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

            strDBName = "GPDB";
            strQueryID = "MenuData.Get_MenuControlInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("MENU_ID", strMenuID);

            sParam.Add("USER_ID", bp.g_userid.ToString());
            //sParam.Add("USER_ID", "JYJ");

            ds = biz.GetMenuControlInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strBtnID = ds.Tables[0].Rows[i]["CONTROL_ID"].ToString();

                        if (strBtnID == "btnSave")
                        {
                            if (btnSave != null)
                            {
                                btnSave.Visible = true;
                            }
                        }

                        if (strBtnID == "btnModify")
                        {
                            if (btnModify != null)
                            {
                                btnModify.Visible = true;
                            }
                        }

                        if (strBtnID == "btnDelete")
                        {
                            if (btnDelete != null)
                            {
                                btnDelete.Visible = true;
                            }
                        }

                        if (strBtnID == "btnConfirm")
                        {
                            if (btnConfirm != null)
                            {
                                btnConfirm.Visible = true;
                            }
                        }

                        if (strBtnID == "btnTempPWD")
                        {
                            if (btnTempPWD != null)
                            {
                                btnTempPWD.Visible = true;
                            }
                        }
                    }
                }
                else {
                    Response.Write("<script>alert('접근권한이 없습니다'); window.close();</script>");
                    Response.End();
                }
            }
        }
        #endregion

        #region SetContorlRender
        public void SetContorlRender(string strMenuID)
        {

            string strBtnID = string.Empty;
            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            System.Web.UI.HtmlControls.HtmlAnchor btnSave = (System.Web.UI.HtmlControls.HtmlAnchor)PopupContent.FindControl("aSave");
            System.Web.UI.HtmlControls.HtmlAnchor btnModify = (System.Web.UI.HtmlControls.HtmlAnchor)PopupContent.FindControl("aModify");
            System.Web.UI.HtmlControls.HtmlAnchor btnDelete = (System.Web.UI.HtmlControls.HtmlAnchor)PopupContent.FindControl("aDelete");
            System.Web.UI.HtmlControls.HtmlAnchor btnTempPWD = (System.Web.UI.HtmlControls.HtmlAnchor)PopupContent.FindControl("aTempPWD");
            System.Web.UI.HtmlControls.HtmlAnchor btnConfirm = (System.Web.UI.HtmlControls.HtmlAnchor)PopupContent.FindControl("aConfirm");

            DataSet ds = new DataSet();

            Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

            strDBName = "GPDB";
            strQueryID = "MenuData.Get_MenuControlInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("MENU_ID", strMenuID);
            sParam.Add("USER_ID", bp.g_userid.ToString());

            ds = biz.GetMenuControlInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                // true 상태면, 강제로 false로 변경 후 저장하고, 실제 권한있는지 확인

                bool[] btnBool = {false,false,false,false};

                if (btnSave != null && btnSave.Visible)
                {
                    btnBool[0] = true;
                    btnSave.Visible = false;
                }

                if (btnModify != null && btnModify.Visible)
                {
                    btnBool[1] = true;
                    btnModify.Visible = false;
                }

                if (btnDelete != null && btnDelete.Visible)
                {
                    btnBool[2] = true;
                    btnDelete.Visible = false;
                }

                if (btnConfirm != null && btnConfirm.Visible)
                {
                    btnBool[3] = true;
                    btnConfirm.Visible = false;
                }

                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strBtnID = ds.Tables[0].Rows[i]["CONTROL_ID"].ToString();

                        if (strBtnID == "btnSave")
                        {
                            if (btnSave != null && btnBool[0])
                            {
                                btnSave.Visible = true;
                            }
                        }

                        if (strBtnID == "btnModify")
                        {
                            if (btnModify != null && btnBool[1])
                            {
                                btnModify.Visible = true;
                            }
                        }

                        if (strBtnID == "btnDelete")
                        {
                            if (btnDelete != null && btnBool[2])
                            {
                                btnDelete.Visible = true;
                            }
                        }

                        if (strBtnID == "btnConfirm")
                        {
                            if (btnConfirm != null)
                            {
                                btnConfirm.Visible = true;
                            }
                        }

                        if (strBtnID == "btnTempPWD")
                        {
                            if (btnTempPWD != null)
                            {
                                btnTempPWD.Visible = true;
                            }
                        }
                    }
                }
                else
                {
                    Response.Write("<script>alert('접근권한이 없습니다'); window.close();</script>");
                    Response.End();
                }
            }
        }
        #endregion

        #region setButtonLog
        public void setButtonLog(string buttonId, string pageFileName, string remark)
        {
            // 페이지의 메뉴명 추출 작업(페이지 != 메뉴 인 경우가 있음)
            DataSet ds = new DataSet();
            string menuId = string.Empty;
            string pageParam = string.Empty;

            if (pageFileName.Contains('_'))
            {
                pageParam = pageFileName.Split('_')[0];
            }
            else
            {
                pageParam = pageFileName.Substring(0,pageFileName.Length-3);
            }

            Biz.SystemManagement.MenuMgt menulog = new Biz.SystemManagement.MenuMgt();

            FW.Data.Parameters menuParam = new FW.Data.Parameters();
            menuParam.Add("VIEW_ID", pageParam);

            ds = menulog.GetMenuList("GPDB", "MenuData.Get_MenuIDByViewIDLike", menuParam);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].TableName.ToString() != "ErrorLog")
                {
                    menuId = ds.Tables[0].Rows[0]["MENU_ID"].ToString();
                }
            }
            else
            {
                menuId = pageFileName;
            }

            //버튼 로그 클래스 작성
            Biz.SystemManagement.ButtonStatisticsMgt btnlog = new Biz.SystemManagement.ButtonStatisticsMgt();

            FW.Data.Parameters logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", buttonId);
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", pageFileName + "_" + buttonId + "_" + remark);

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", menuId);             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD("GPDB", "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장
        }
        #endregion

        #region btnSaveCount_Click
        protected void btnSaveCount_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnSave", pageFileName, "Popup_Click");
        }
        #endregion

        #region btnModifyCount_Click
        protected void btnModifyCount_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnModify", pageFileName, "Popup_Click");
        }
        #endregion
        
        #region btnDeleteCount_Click
        protected void btnDeleteCount_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnDelete", pageFileName, "Popup_Click");
        }
        #endregion

        #region btnRestoreCount_Click
        protected void btnRestoreCount_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnRestore", pageFileName, "Popup_Click");
        }
        #endregion
        
    }
}