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

            if (bp.g_userid.ToString() == "")
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Opener", " parent.location.href = '/login.aspx';", true);
            }


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
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
		{
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
            
            ddlPaging = (System.Web.UI.HtmlControls.HtmlSelect)MainContent.FindControl("current_page_value");
            
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

            System.Web.UI.HtmlControls.HtmlInputButton btnExcel     = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnExcel");
            System.Web.UI.HtmlControls.HtmlInputButton btnNew       = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnNew");
            System.Web.UI.HtmlControls.HtmlInputButton btnBCate     = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnBCate");
            System.Web.UI.HtmlControls.HtmlInputButton btnGCate     = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnGCate");
            System.Web.UI.HtmlControls.HtmlInputButton btnFunction  = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnFunction");
            System.Web.UI.HtmlControls.HtmlInputButton btnCopy      = (System.Web.UI.HtmlControls.HtmlInputButton)MainContent.FindControl("btnCopy");

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
                        }

                        if (strBtnID == "btnExcelNew")
                        {
                            if (btnExcelNew != null) { btnExcelNew.Visible = true; }
                        }

                        if (strBtnID == "btnNew")
                        {
                            if (btnNew != null) { btnNew.Visible = true; }
                        }

                        if (strBtnID == "btnBCate")
                        {
                            if (btnBCate != null) { btnBCate.Visible = true; }
                        }

                        if (strBtnID == "btnGCate")
                        {
                            if (btnGCate != null) { btnGCate.Visible = true; }
                        }

                        if (strBtnID == "btnFunction")
                        {
                            if (btnFunction != null) { btnFunction.Visible = true; }
                        }

                        if (strBtnID == "btnCopy")
                        {
                            if (btnCopy != null) { btnCopy.Visible = true; }
                        }

                        if (strBtnID == "btnRestore")
                        {
                            if (btnRestore != null) { btnRestore.Visible = true; }
                        }

                        if (strBtnID == "btnSave")
                        {
                            if (btnSave != null) { btnSave.Visible = true; }
                        }

                        if (strBtnID == "btnModify")
                        {
                            if (btnModify != null) { btnModify.Visible = true; }
                        }

                        if (strBtnID == "btnDelete")
                        {
                            if (btnDelete != null) { btnDelete.Visible = true; }
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

        

    }
}