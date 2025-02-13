using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MES.FW.Common.CommonMgt;

namespace HQCWeb.MasterPage
{
    public partial class Widget : System.Web.UI.MasterPage
    {
        BasePage bp = new BasePage();

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
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {

        } 
    }
}