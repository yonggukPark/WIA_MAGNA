using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MES.FW.Common.CommonMgt;

using System.Windows;

namespace HQCWeb.MasterPage 
{
    public partial class Site : System.Web.UI.MasterPage
    {
		BasePage bp = new BasePage();

        public string strMenu = string.Empty;

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        public string strWebIpPort = string.Empty;

        #region Page_Init
        protected void Page_Init(object sender, EventArgs e)
		{
            //if (bp.g_CurrentUrl == "")
            //{
            //    bp.g_CurrentUrl = Request.Url.AbsolutePath;
            //}

            //if (bp.g_plant == "")
            //{
            //    bp.g_plant = "9012C";
            //}

            if (bp.g_language == "")
            {
                bp.g_language = "KO_KR";
            }

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
                bp.SetPaginInfo(Request.Url.AbsolutePath);

                Label lbTitle = (Label)MainContent.FindControl("lbTitle");

                if (lbTitle != null)
                {
                    string strMenuID = string.Empty;

                    strMenuID = lbTitle.Text;

                    DataSet ds = new DataSet();

                    Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

                    strDBName = "GPDB";
                    strQueryID = "MenuData.Get_MenuInfo";

                    FW.Data.Parameters param = new FW.Data.Parameters();
                    param.Add("MENU_ID", strMenuID);

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
            strWebIpPort = System.Configuration.ConfigurationManager.AppSettings.Get("WEB_IP_PORT");

            lbUserInfo.Text = bp.g_userid.ToString() + " 반갑습니다.";

			lbUrl.Text = Request.Url.AbsolutePath;

			//DevExpress.Web.ASPxWebControl.GlobalTheme = "Aqua";
            
            if (!IsPostBack)
			{
                PageInit();
            }

			if (bp.g_plant == "")
			{
				lbTest.Text = "Nothing";
			}
			else {
				lbTest.Text = bp.g_plant;
			}

			bp.FWInitControl(this);
        }
        #endregion
        
        #region PageInit
        private void PageInit()
        {
            DataSet ds = new DataSet();
            
            System.Web.UI.HtmlControls.HtmlSelect ddlPaging;

            System.Web.UI.HtmlControls.HtmlSelect ddlPaging2;

            ddlPaging = (System.Web.UI.HtmlControls.HtmlSelect)MainContent.FindControl("current_page_value");
            ddlPaging2 = (System.Web.UI.HtmlControls.HtmlSelect)MainContent.FindControl("current_page_value2");

            if (ddlPaging != null)
            {
                strDBName = "GPDB";
                strQueryID = "ComCodeData.Get_ComCodeByComTypeInfo";

                FW.Data.Parameters param = new FW.Data.Parameters();
                param.Add("COMM_TYPE", "PAGING");

                Biz.SystemManagement.ComCodeMgt biz = new Biz.SystemManagement.ComCodeMgt();

                ds = biz.GetComCodeByComTypeInfo(strDBName, strQueryID, param);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {
                            ddlPaging.Items.Add(new ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                        }
                        if (ddlPaging2 != null)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                ddlPaging2.Items.Add(new ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                            }
                        }
                    }
                }
            }
        }
        #endregion
        
        #region SetContorl
        public void SetContorl(string strMenuID) {

            string strBtnID = string.Empty;
            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            System.Web.UI.WebControls.Button btnSearch              = (System.Web.UI.WebControls.Button)MainContent.FindControl("btnSearch");
            System.Web.UI.WebControls.Button btnRestore             = (System.Web.UI.WebControls.Button)MainContent.FindControl("btnRestore");
            System.Web.UI.WebControls.Button btnExcelNew            = (System.Web.UI.WebControls.Button)MainContent.FindControl("btnExcelNew");
            System.Web.UI.WebControls.Button btnExcelNew2           = (System.Web.UI.WebControls.Button)MainContent.FindControl("btnExcelNew2");
            System.Web.UI.WebControls.Button btnExcelNew3           = (System.Web.UI.WebControls.Button)MainContent.FindControl("btnExcelNew3");
            System.Web.UI.WebControls.Button btnGSave               = (System.Web.UI.WebControls.Button)MainContent.FindControl("btnSave");
            System.Web.UI.WebControls.Button btnGModify             = (System.Web.UI.WebControls.Button)MainContent.FindControl("btnModify");
            System.Web.UI.WebControls.Button btnGDelete             = (System.Web.UI.WebControls.Button)MainContent.FindControl("btnDelete");
            System.Web.UI.WebControls.Button btnGCopy               = (System.Web.UI.WebControls.Button)MainContent.FindControl("btnGCopy");

            System.Web.UI.HtmlControls.HtmlInputButton btnExcel     = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnExcel");
            System.Web.UI.HtmlControls.HtmlInputButton btnExcel2    = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnExcel2");
            System.Web.UI.HtmlControls.HtmlInputButton btnNew       = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnNew");
            System.Web.UI.HtmlControls.HtmlInputButton btnBCate     = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnBCate");
            System.Web.UI.HtmlControls.HtmlInputButton btnGCate     = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnGCate");
            System.Web.UI.HtmlControls.HtmlInputButton btnFunction  = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnFunction");
            System.Web.UI.HtmlControls.HtmlInputButton btnCopy      = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnCopy");
            System.Web.UI.HtmlControls.HtmlInputButton btnSubNew    = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnSubNew");
            System.Web.UI.HtmlControls.HtmlInputButton btnDeleteNew = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnDeleteNew");
            System.Web.UI.HtmlControls.HtmlInputButton btnConfirm   = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnConfirm");
            System.Web.UI.HtmlControls.HtmlInputButton btnUpload    = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnUpload");
            System.Web.UI.HtmlControls.HtmlInputButton btnLock      = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnLock");

            System.Web.UI.HtmlControls.HtmlAnchor btnSave           = (System.Web.UI.HtmlControls.HtmlAnchor)MainContent.FindControl("aSave");
            System.Web.UI.HtmlControls.HtmlAnchor btnModify         = (System.Web.UI.HtmlControls.HtmlAnchor)MainContent.FindControl("aModify");
            System.Web.UI.HtmlControls.HtmlAnchor btnDelete         = (System.Web.UI.HtmlControls.HtmlAnchor)MainContent.FindControl("aDelete");

            DataSet ds = new DataSet();

            Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

            strDBName = "GPDB";
            strQueryID = "MenuData.Get_MenuControlInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("MENU_ID", strMenuID);

            sParam.Add("USER_ID", bp.g_userid.ToString());
            //sParam.Add("USER_ID", "JYJ");

            ds = biz.GetMenuControlInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0) {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strBtnID = ds.Tables[0].Rows[i]["CONTROL_ID"].ToString();

                        if (strBtnID == "btnSearch")
                        {
                            if (btnSearch != null) { btnSearch.Visible = true; }
                        }

                        if (strBtnID == "btnExcel")
                        {
                            if (btnExcel != null) { btnExcel.Visible = true; }
                            if (btnExcel2 != null) { btnExcel2.Visible = true; }
                        }

                        if (strBtnID == "btnExcelNew")
                        {
                            if (btnExcelNew != null) { btnExcelNew.Visible = true; }
                            if (btnExcelNew2 != null) { btnExcelNew2.Visible = true; }
                            if (btnExcelNew3 != null) { btnExcelNew3.Visible = true; }
                        }

                        if (strBtnID == "btnSave")
                        {
                            if (btnGSave != null) { btnGSave.Visible = true; }
                            if (btnSave != null) { btnSave.Visible = true; }
                        }

                        if (strBtnID == "btnModify")
                        {
                            if (btnGModify != null) { btnGModify.Visible = true; }
                            if (btnModify != null) { btnModify.Visible = true; }
                        }

                        if (strBtnID == "btnDelete")
                        {
                            if (btnGDelete != null) { btnGDelete.Visible = true; }
                            if (btnDelete != null) { btnDelete.Visible = true; }
                            if (btnDeleteNew != null) { btnDeleteNew.Visible = true; }
                        }

                        if (strBtnID == "btnNew")
                        {
                            if (btnNew != null) { btnNew.Visible = true; }
                            if (btnSubNew != null) { btnSubNew.Visible = true; }
                        }

                        if (strBtnID == "btnBCate")
                        {
                            if (btnBCate != null) { btnBCate.Visible = true; }
                        }

                        if (strBtnID == "btnGCate")
                        {
                            if (btnGCate != null) { btnGCate.Visible = true; }
                        }

                        if (strBtnID == "btnConfirm")
                        {
                            if (btnConfirm != null) { btnConfirm.Visible = true; }
                        }

                        if (strBtnID == "btnUpload")
                        {
                            if (btnUpload != null) { btnUpload.Visible = true; }
                        }

                        if (strBtnID == "btnFunction")
                        {
                            if (btnFunction != null) { btnFunction.Visible = true; }
                        }

                        if (strBtnID == "btnCopy")
                        {
                            if (btnCopy != null) { btnCopy.Visible = true; }
                            if (btnGCopy != null) { btnGCopy.Visible = true; }
                        }

                        if (strBtnID == "btnRestore")
                        {
                            if (btnRestore != null) { btnRestore.Visible = true; }
                        }

                        if (strBtnID == "btnLock")
                        {
                            if (btnLock != null) { btnLock.Visible = true; }
                        }
                    }
                }
                else {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Opener", " alert('접근 권한이 없습니다.'); parent.location.href= '/Main.aspx';", true);
                }
            }

            TextBox hidScreenType = (TextBox)MainContent.FindControl("hidScreenType");

            if (hidScreenType != null)
            {
                //systempara
            }
        }
        #endregion

        #region setButtonLog
        public void setButtonLog(string buttonId, string pageFileName, string remark)
        {
            // 페이지의 메뉴명 추출 작업(페이지 != 메뉴 인 경우가 있음)
            DataSet ds = new DataSet();
            string menuId = string.Empty;

            Biz.SystemManagement.MenuMgt menulog = new Biz.SystemManagement.MenuMgt();

            FW.Data.Parameters menuParam = new FW.Data.Parameters();
            menuParam.Add("VIEW_ID", pageFileName);

            ds = menulog.GetMenuList("GPDB", "MenuData.Get_MenuIDByViewID", menuParam);

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

        #region btnNewCount_Click
        protected void btnNewCount_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnNew", pageFileName, "Click");
        }
        #endregion

        #region btnSubNewCount_Click
        protected void btnSubNewCount_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnSubNew", pageFileName, "Click");
        }
        #endregion

        #region btnCopyCount_Click
        protected void btnCopyCount_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnCopy", pageFileName, "Click");
        }
        #endregion

        #region btnExcelCount_Click
        protected void btnExcelCount_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnExcel", pageFileName, "Click");
        }
        #endregion

        #region btnExcel2Count_Click
        protected void btnExcel2Count_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnExcel2", pageFileName, "Click");
        }
        #endregion

        #region btnConfirmCount_Click
        protected void btnConfirmCount_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnConfirm", pageFileName, "Click");
        }
        #endregion

        #region btnDeleteCount_Click
        protected void btnDeleteCount_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnDelete", pageFileName, "Click");
        }
        #endregion

        #region btnSaveCount_Click
        protected void btnSaveCount_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnSave", pageFileName, "Click");
        }
        #endregion

        #region btnUploadCount_Click
        protected void btnUploadCount_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnUpload", pageFileName, "Click");
        }
        #endregion

        #region btnLockCount_Click
        protected void btnLockCount_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnLock", pageFileName, "Click");
        }
        #endregion

        #region btnExcelNewCount_Click
        protected void btnExcelNewCount_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnExcelNew", pageFileName, "Click");
        }
        #endregion

        #region btnExcelNew2Count_Click
        protected void btnExcelNew2Count_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnExcelNew2", pageFileName, "Click");
        }
        #endregion

        #region btnExcelNew3Count_Click
        protected void btnExcelNew3Count_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("btnExcelNew3", pageFileName, "Click");
        }
        #endregion

        #region ibtnRegisterCount_Click
        protected void ibtnRegisterCount_Click(object sender, EventArgs e)
        {
            //파일 이름 추출
            string pageFileName = System.IO.Path.GetFileNameWithoutExtension(Page.AppRelativeVirtualPath); // 예: "Info01"

            setButtonLog("ibtnRegisterCount", pageFileName, "Click");
        }
        #endregion
    }
}