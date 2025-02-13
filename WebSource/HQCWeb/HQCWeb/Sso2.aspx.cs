using MES.FW.Common.CommonMgt;

using System;
using System.Data;
using System.Net;
using System.Web;
using System.Web.UI;

namespace HQCWeb
{
    public partial class Sso2 : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            string strScript = string.Empty;
            string strUserId = string.Empty;
            string strUserName = string.Empty;
            string strIP = string.Empty;
            string code = Request["code"];
            string url = "https://portal.h-greenpower.co.kr/sso/mes?code=" + code;

            try
            {
                using (WebClient client = new WebClient())
                {
                    string result = client.DownloadString(url);
                    string[] aa = result.Split('|');

                    strUserId = aa[0];
                    strUserName = aa[1];
                    
                    /* 받기 완료 */

                    DataSet ds = new DataSet();

                    string strDBName = "GPDB";
                    string strQueryID = "UserInfoData.Get_SSOUserInfo";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("USER_ID", strUserId);

                    Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

                    //사용자 정보조회
                    ds = biz.GetLoginData(strDBName, strQueryID, sParam);
                    if (ds.Tables.Count > 0)
                    {
                        int iLoginDTUpdate = 0;
                       
                        strQueryID = "UserInfoData.Set_UserLoginDTUpdate";

                        iLoginDTUpdate = biz.SetUserLoginDTUpdate(strDBName, strQueryID, sParam);
                        if (iLoginDTUpdate > 0)
                        {
                            g_userid = strUserId;
                            g_username = strUserName;
                            g_language = "";
                            g_plant = ds.Tables[0].Rows[0]["SITE_ID"].ToString();
                            g_IP = GetClientIpAddress();

                            Session["LoginChk"] = "Y";

                            Response.Redirect("Main.aspx");
                        }
                        else
                        {
                            strScript = " alert('로그인이 원활하지 않습니다. 다시 시도하세요.'); ";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                            Response.Redirect("Login.aspx");
                        }
                    }
                    else
                    {
                        strScript = " alert('사용자 정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        Response.Redirect("Login.aspx");
                    }
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), null, "<script>alert(\"" + ex.Message + "\");</script>");
            }
        }

        #region GetClientIpAddress
        public string GetClientIpAddress()
        {
            string ipAddress = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (string.IsNullOrEmpty(ipAddress) || ipAddress.ToLower() == "unknown")
            {
                ipAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            // HTTP_X_FORWARDED_FOR 헤더가 여러 개의 IP를 가질 수 있으므로 첫 번째 IP를 가져옵니다.
            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    ipAddress = addresses[0];
                }
            }

            return ipAddress;
        }
        #endregion
    }
}