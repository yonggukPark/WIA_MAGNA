using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MES.FW.Common.CommonMgt;
using HQCWeb.FMB_FW;
using HQCWeb.FW;

using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Web.Services;

namespace HQCWeb.SystemMgt
{
    public partial class SessionCheck : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        
        #region InitializePage
        private void InitializePage()
        {
            ScriptManager.RegisterStartupScript(Page, this.GetType(), "RefreshFrames", "parent.location.reload(true);", true);
            //if (bp.g_userid.ToString() == "")
            //{
            //    // 세션 초기화
            //    Session.Clear();
            //    Session.Abandon();

            //    // 세션 쿠키 제거
            //    if (Request.Cookies["ASP.NET_SessionId"] != null)
            //    {
            //        HttpCookie cookie = new HttpCookie("ASP.NET_SessionId");
            //        cookie.Expires = DateTime.Now.AddDays(-1); // 쿠키 만료
            //        Response.Cookies.Add(cookie);
            //    }

            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Opener", " alert('세션이 만료되어 로그인 페이지로 이동합니다'); location.href = '/login.aspx';", true);
            //}
        }
        #endregion

        [WebMethod]
        public static void GetStatus()
        {
            var currentPage = (SessionCheck)HttpContext.Current.Handler;

            // InitializePage를 호출
            currentPage.InitializePage();
        }
    }
}