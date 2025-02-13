using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

using MES.FW.Common.CommonMgt;

namespace HQCWeb
{
    public partial class SSOLogin : BasePage // System.Web.UI.Page
    {
        string strUserID = string.Empty;
        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        BasePage bp = new BasePage();

        protected void Page_Load(object sender, EventArgs e)
        {
            //strUserID = Request.QueryString["USER_ID"].ToString();

            //LoginLogic(strUserID);

            SendMessage();
        }

        #region LoginLogic
        public void LoginLogic(string strUserID) {
            string strRtn = string.Empty;
            string strRtnValChk = string.Empty;
            string strScript = string.Empty;
            string strPwd = string.Empty;
            string strInputPwd = string.Empty;
            string strUserName = string.Empty;
            string strExpireYN = string.Empty;
            string strLoginStatCD = string.Empty;

            int iLoginDTUpdate = 0;

            DataSet ds = new DataSet();

            strDBName = "GPDB";
            strQueryID = "UserInfoData.Get_UserInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("USER_ID", strUserID);            

            Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

            //사용자 정보조회
            ds = biz.GetLoginData(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    g_userid =      ds.Tables[0].Rows[0]["USER_ID"].ToString();
                    g_username =    ds.Tables[0].Rows[0]["USER_NM"].ToString(); ;
                    g_language =    "KO_KR";
                    g_plant =       ds.Tables[0].Rows[0]["SITE_ID"].ToString();

                    Session["LoginChk"] = "Y";

                    strQueryID = "UserInfoData.Set_UserLoginDTUpdate";

                    iLoginDTUpdate = biz.SetUserLoginDTUpdate(strDBName, strQueryID, sParam);

                    if (iLoginDTUpdate > 0)
                    {
                        Response.Redirect("Main.aspx");
                    }
                    else
                    {
                        strScript = " alert('로그인이 원활하지 않습니다. 다시 시도하세요.'); location.href = 'Login.aspx'; ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                }
                else
                {
                    strScript = " alert('사용자 정보가 존재하지 않습니다. 관리자에게 문의바립니다.');  location.href = 'Login.aspx';";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
            }
            else
            {
                strScript = " alert('사용자 정보가 존재하지 않습니다. 관리자에게 문의바립니다.');  location.href = 'Login.aspx'; ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region SendMessage
        public void SendMessage() {
            // 한번 요청으로 1만건의 알림톡 발송이 가능합니다.
            // 등록되어 있는 템플릿의 변수 부분을 제외한 나머지 부분(상수)은 100% 일치해야 합니다.
            // 템플릿 내용이 "#{이름}님 가입을 환영합니다."으로 등록되어 있는 경우 변수 #{이름}을 홍길동으로 치환하여 "홍길동님 가입을 환영합니다."로 입력해 주세요.
            // 버튼은 5개까지 입력 가능합니다.

            MessagingLib.Messages messages = new MessagingLib.Messages();

            string strTest = "Temp Password입니다. -장용제";

            // #{변수} 에 값을 입력합니다. (템플릿의 모든 변수값을 입력해야 오류 발생하지 않습니다.)
            Dictionary<string, string> variables = new Dictionary<string, string> { { "#{temppassword}", strTest } };

            // variables에 변수값을 입력하여 요청하면 알림톡의 내용은 서버쪽에서 자동으로 채워져 발송됩니다.
            messages.Add(new MessagingLib.Message()
            {
                to = "01025124176",
                from = "029302266",
                kakaoOptions = new MessagingLib.KakaoOptions()
                {
                    pfId = "KA01PF240219011728428NZDqHClvCQz",
                    templateId = "KA01TP240401075156127HI3UxhIE7ip", // 템플릿 등록 후 발급받은 값을 사용해 주세요
                    variables = variables
                }
            });


            MessagingLib.Response response = MessagingLib.SendMessages(messages);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Response.Write("OK");
                Response.End();


                //Console.WriteLine("전송 결과");
                //Console.WriteLine("Group ID:" + response.Data.SelectToken("groupId").ToString());
                //Console.WriteLine("Status:" + response.Data.SelectToken("status").ToString());
                //Console.WriteLine("Count:" + response.Data.SelectToken("count").ToString());
            }
            else
            {
                Response.Write("ERROR Code=" + response.ErrorCode.ToString());
                Response.Write("ERROR Message=" + response.ErrorMessage.ToString());
                Response.End();

                //Console.WriteLine("Error Code:" + response.ErrorCode);
                //Console.WriteLine("Error Message:" + response.ErrorMessage);
            }


        }
        #endregion
    }
}