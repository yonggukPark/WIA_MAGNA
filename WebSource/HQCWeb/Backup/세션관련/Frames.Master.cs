using MES.FW.Common.CommonMgt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HQCWeb.MasterPage 
{
    public partial class Frames : System.Web.UI.MasterPage
    {
        BasePage bp = new BasePage();

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
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

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Opener", " alert('세션이 만료되어 로그인 페이지로 이동합니다'); location.href = '/login.aspx';", true);
            }
            else
            {
                if (bp.g_CurrentUrl == "")
                {
                    bp.g_CurrentUrl = Request.Url.AbsolutePath;
                }

                if (bp.g_plant == "")
                {
                    bp.g_plant = "H20";
                }

                if (bp.g_language == "")
                {
                    bp.g_language = "KO_KR";
                }
            }
        }
        #endregion

        #region btnAccessLog_Click
        protected void btnAccessLog_Click(object sender, EventArgs e)
        {
            string strDBName = string.Empty;
            string strQueryID = string.Empty;
            int iRtn = 0;

            strDBName = "GPDB";
            strQueryID = "CUDData.Set_MESACCESS_Data";
            
            Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();
            FW.Data.Parameters sParam = new FW.Data.Parameters();

            sParam.Add("MENU_ID",   txtMenuID.Text);
            sParam.Add("REG_ID",    bp.g_userid.ToString());
            //sParam.Add("REG_ID", "JYJ");
            sParam.Add("LogFlag",   "N");

            iRtn = biz.SetMenuAccess(strDBName, strQueryID, sParam);
        }
        #endregion

        #region btnApprovalRoad_Click
        protected void btnApprovalRoad_Click(object sender, EventArgs e)
        {
            //int iCnt = Convert.ToInt32(lbApprovalCount.Text);
            
            //iCnt++;
            
            //lbApprovalCount.Text = iCnt.ToString();
        }
        #endregion
    }
}