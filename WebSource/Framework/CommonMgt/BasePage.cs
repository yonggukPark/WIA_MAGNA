using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Collections;
using System.ComponentModel;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Web.Security;
using System.Data;
//using DevExpress.Web;

namespace MES.FW.Common.CommonMgt
{
    public class BasePage : System.Web.UI.Page //System.Web.UI.MasterPage //
    {
		MasterPage dfMaster = new MasterPage();

        public BasePage()
        {

        }

        #region 사번
        //아이디 
        public string g_userid
        {
            set
            {
                Session["g_userid"] = value;
            }
            get
            {
                if (Session["g_userid"] == null)
                {
                    return "";
                }
                else
                {
                    return Session["g_userid"].ToString();
                }
            }
        }
        #endregion

        #region 이름
        //이름 
        public string g_username
        {
            set
            {
                Session["g_username"] = value;
            }
            get
            {
                if (Session["g_username"] == null)
                {
                    return "";
                }
                else
                {
                    return Session["g_username"].ToString();
                }
            }
        }
		#endregion

		#region 사이트 아이디
		//이름 
		public string g_siteid
		{
			set
			{
				Session["g_siteid"] = value;
			}
			get
			{
				if (Session["g_siteid"] == null)
				{
					return "";
				}
				else
				{
					return Session["g_siteid"].ToString();
				}
			}
		}
		#endregion

		#region TotalCount
		//이름 
		public string g_TotalCount
		{
			set
			{
				Session["g_TotalCount"] = value;
			}
			get
			{
				if (Session["g_TotalCount"] == null)
				{
					return "";
				}
				else
				{
					return Session["g_TotalCount"].ToString();
				}
			}
		}
		#endregion

		#region PageCount
		//이름 
		public string g_PageCount
		{
			set
			{
				Session["g_totalCount"] = value;
			}
			get
			{
				if (Session["g_totalCount"] == null)
				{
					return "";
				}
				else
				{
					return Session["g_totalCount"].ToString();
				}
			}
		}
		#endregion

		#region PageIndex
		//이름 
		public string g_PageIndex
		{
			set
			{
				Session["g_PageIndex"] = value;
			}
			get
			{
				if (Session["g_PageIndex"] == null)
				{
					return "";
				}
				else
				{
					return Session["g_PageIndex"].ToString();
				}
			}
		}
		#endregion

		#region PageSize
		//이름 
		public string g_PageSize
		{
			set
			{
				Session["g_PageSize"] = value;
			}
			get
			{
				if (Session["g_PageSize"] == null)
				{
					return "";
				}
				else
				{
					return Session["g_PageSize"].ToString();
				}
			}
		}
		#endregion

		#region g_GridDataSource
		//이름 
		public DataSet g_GridDataSource
		{
			set
			{
				Session["g_GridDataSource"] = value;
			}
			get
			{
				if (Session["g_GridDataSource"] == null)
				{
					return null;
				}
				else
				{
                    return Session["g_GridDataSource"] as DataSet;
				}
			}
		}
		#endregion

		#region CurrentUrl
		//이름 
		public string g_CurrentUrl
		{
			set
			{
				Session["g_CurrentUrl"] = value;
			}
			get
			{
				if (Session["g_CurrentUrl"] == null)
				{
					return "";
				}
				else
				{
					return Session["g_CurrentUrl"].ToString();
				}
			}
		}
		#endregion

		#region SetLoginChk (로그인 체크)
		public void SetLoginChk(string strDefaultUrl)
		{
			if (g_username == "")
			{
				Response.Redirect(strDefaultUrl);
			}
		}
		#endregion

		#region SetPaginInfo (페이징 정보 셋팅)
		public void SetPaginInfo(string strCurrentUrl)
		{
            if (g_CurrentUrl == null || g_CurrentUrl == "") {
                g_CurrentUrl = strCurrentUrl;
			}

			if (g_CurrentUrl != strCurrentUrl)
			{
                g_TotalCount = "1";
				g_PageCount = "1";
				g_PageIndex = "1";
				g_PageSize = "10";

				g_GridDataSource = null;
			}
		}
		#endregion

		#region FWInitControl
		public void FWInitControl(MasterPage mp)
		{
			dfMaster = mp;
            
            //20240202 LDS 수정(PostBack시 그리드 스크립트 상실 문제)
            Button btnSearch = (Button)dfMaster.FindControl("MainContent").FindControl("btnSearch");

            if (btnSearch != null)
			{
				btnSearch.Click += btnSearch_Click;
			}

            /*
            ASPxComboBox cbPageSize = null;

            if (dfMaster.FindControl("MainContent").FindControl("PagingControl") == null)
			{
                cbPageSize = (ASPxComboBox)dfMaster.FindControl("MainContent").FindControl("cbPageSize");
            }
			else {
                cbPageSize = (ASPxComboBox)dfMaster.FindControl("MainContent").FindControl("PagingControl").FindControl("cbPageSize");
            }

			if (cbPageSize != null)
			{
				cbPageSize.SelectedIndexChanged += cbPageSize_SelectedIndexChanged;
			}
            */
		}
		#endregion

		#region Control Event Araa
		private void btnSearch_Click(object sender, EventArgs e)
		{
			InitSetValue();
		}

		protected void cbPageSize_SelectedIndexChanged(object sender, EventArgs e)
		{
			InitSetValue();
		}

		public  void InitSetValue()
		{
			HiddenField hidTotalCount = (HiddenField)dfMaster.FindControl("hidTotalCount");
			HiddenField hidPageCount = (HiddenField)dfMaster.FindControl("hidPageCount");
			HiddenField hidPageIndex = (HiddenField)dfMaster.FindControl("hidPageIndex");
			HiddenField hidPageSize = (HiddenField)dfMaster.FindControl("hidPageSize");

			if (hidTotalCount != null)
				hidTotalCount.Value = g_TotalCount;

			if (hidTotalCount != null)
				hidPageCount.Value = g_PageCount;

			if (hidTotalCount != null)
				hidPageIndex.Value = g_PageIndex;

			if (hidTotalCount != null)
				hidPageSize.Value = g_PageSize;
		}
		#endregion

		#region 공장코드
		//이름 
		public string g_plant
		{
			set
			{
				Session["g_plant"] = value;
			}
			get
			{
				if (Session["g_plant"] == null)
				{
					return "";
				}
				else
				{
					return Session["g_plant"].ToString();
				}
			}
		}
		#endregion

		#region 공장이름
		//이름 
		public string g_plantname
        {
            set
            {
                Session["g_plantname"] = value;
            }
            get
            {
                if (Session["g_plantname"] == null)
                {
                    return "";
                }
                else
                {
                    return Session["g_plantname"].ToString();
                }
            }
        }
        #endregion
		
        #region 언어
        //이름 
        public string g_language
        {
            set
            {
                Session["g_language"] = value;
            }
            get
            {
                if (Session["g_language"] == null)
                {
                    return "";
                }
                else
                {
                    return Session["g_language"].ToString();
                }
            }
        }
        #endregion

        #region IP
        //이름 
        public string g_IP
        {
            set
            {
                Session["g_IP"] = value;
            }
            get
            {
                if (Session["g_IP"] == null)
                {
                    return "";
                }
                else
                {
                    return Session["g_IP"].ToString();
                }
            }
        }
        #endregion  
    }
}