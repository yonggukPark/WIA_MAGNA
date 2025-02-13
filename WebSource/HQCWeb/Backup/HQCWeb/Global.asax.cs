using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.UI;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using HQCWeb.FW;

using HQC.FW.Common;
using HQC.FW.Common.Constants;
using System.Timers;

namespace HQCWeb
{
    public class Global : System.Web.HttpApplication
    {

        #region Application_Start
        protected void Application_Start(object sender, EventArgs e)
        {
			// 애플리케이션 시작 시 실행되는 코드
			//DevExpress.Web.ASPxWebControl.CallbackError += Application_Error;

            DataBaseService.SetDataBase();

            DicService.DicData();

            MsgService.MsgData();
        }
        #endregion

        #region Application_Error
        void Application_Error(object sender, EventArgs e)
        {
            // Use HttpContext.Current to get a Web request processing helper 
            HttpServerUtility server = HttpContext.Current.Server;
            Exception exception = server.GetLastError();
            // Log an exception 
            //AddToLog(exception.Message, exception.StackTrace);
        }
        #endregion

        #region Application_PreRequestHandlerExecute
        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            //DevExpress.Web.ASPxWebControl.GlobalThemeFont = "Segoe UI";
            //DevExpress.Web.ASPxWebControl.GlobalThemeFont = "Asap";
            //DevExpress.Web.ASPxWebControl.GlobalThemeFont = "ArimaMadurai";
            //DevExpress.Web.ASPxWebControl.GlobalThemeFont = "Comfortaa";

            Page page = HttpContext.Current.CurrentHandler as Page;
            if (page != null)
            {
                page.PreInit += new EventHandler(page_PreInit);
            }
        }
        #endregion

        #region page_PreInit
        void page_PreInit(object sender, EventArgs e)
        {
            //Page page = HttpContext.Current.CurrentHandler as Page;
            //if (page != null && page.Request.Cookies["theme"] != null)
            //{
            //    DevExpress.Web.ASPxWebControl.GlobalTheme = page.Request.Cookies["theme"].Value;
            //}
            //else
            //{
            //    DevExpress.Web.ASPxWebControl.GlobalTheme = "MetropolisBlue";
            //}
        }
        #endregion

        #region Application_EndRequest
        void Application_EndRequest(object sender, EventArgs e)
        {

        }
        #endregion
    }
}