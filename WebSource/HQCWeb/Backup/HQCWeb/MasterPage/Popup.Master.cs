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
			if (bp.g_userid.ToString() == "")
			{
                Response.Write("<script>opener.location.href='/login.aspx'; window.close();</script>");
                Response.End();
            }

            Label lbTitle = (Label)PopupContent.FindControl("lbTitle");

            if (lbTitle != null)
            {
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
		#endregion

		#region Page_Load
		protected void Page_Load(object sender, EventArgs e)
		{
			//lbUserInfo.Text = bp.g_userid.ToString() + " 반갑습니다.";

			//DevExpress.Web.ASPxWebControl.GlobalTheme = "Aqua";

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
    }
}