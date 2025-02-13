using MES.FW.Common.CommonMgt;
using MES.FW.Common.Crypt;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HQCWeb
{
    public partial class Login : BasePage // System.Web.UI.Page
    {
        Crypt cy = new Crypt();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Clear();
        }
        #endregion

        #region btnLogin_Click
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            LoginLogic();
        }

        #endregion

        #region LoginLogic
        public void LoginLogic()
        {
            //로그인 시도 전, 쿠키 삭제
            if (Request.Cookies["UserID"] != null)
            {
                HttpCookie userNameCookie = new HttpCookie("UserID");
                userNameCookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(userNameCookie);
            }

            string strRtn           = string.Empty;
            string strRtnValChk     = string.Empty;
            string strScript        = string.Empty;
            string strPwd           = string.Empty;
            string strInputPwd      = string.Empty;
            string strUserName      = string.Empty;
            string strExpireYN      = string.Empty;
            string strLoginStatCD   = string.Empty;
            string strMobile        = string.Empty;

            int iLoginDTUpdate      = 0;
            int iLoginFaultCnt      = 0;

            DataSet ds = new DataSet();
                        
            strDBName = "GPDB";
            strQueryID = "UserInfoData.Get_UserInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("USER_ID", txtID.Text);
            sParam.Add("LogFlag", "N");
            strInputPwd = cy.Hash(txtPW.Text);

            Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

            //사용자 정보조회
            ds = biz.GetLoginData(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strPwd          = ds.Tables[0].Rows[0]["USER_PWD"].ToString();
                    strUserName     = ds.Tables[0].Rows[0]["USER_NM"].ToString();
                    iLoginFaultCnt  = Convert.ToInt16(ds.Tables[0].Rows[0]["LOGIN_FAULT_CNT"].ToString());
                    strExpireYN     = ds.Tables[0].Rows[0]["EXPIRE_YN"].ToString();
                    strLoginStatCD  = ds.Tables[0].Rows[0]["LOGIN_STAT_CD"].ToString();
                    strMobile       = ds.Tables[0].Rows[0]["USER_MOBILE"].ToString();

                    //로그인 실패 Case
                    //    1.비밀번호 오입력
                    //    2.비밀번호 오입력 5회이상
                    //    2.비밀번호 만료(90일)
                    //    3.비밀번호 초기화(비밀번호 오입력 5회이상 및 최초 생성시)

                    // 로그인 상태 체크
                    // C : 비밀번호 변경이 요청된 상태라 비밀번호를 변경해야함.
                    // N : 정상 상태


                    if (strLoginStatCD == "C")
                    {
                        strScript = " alert('비밀번호 변경이 요청된 계정입니다. 비밀번호를 변경하세요.'); fn_LoginReset(); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else {
                        if (strExpireYN == "Y")
                        {
                            strScript = " alert('비밀번호 사용일자가 90일을 넘었습니다. 비밀번호를 변경하세요.'); fn_LoginReset(); ";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        }
                        else
                        {
                            if (iLoginFaultCnt > 4)
                            {
                                strScript = " alert('비밀번호 입력이 5회이상 틀렸습니다. 임시로 발급된 비밀번호 확인후 비밀번호를 변경하세요.'); fn_LoginReset(); ";
                                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                            }
                            else
                            {
                                if (strPwd == strInputPwd)
                                {
                                    strQueryID = "UserInfoData.Set_UserLoginDTUpdate";

                                    iLoginDTUpdate = biz.SetUserLoginDTUpdate(strDBName, strQueryID, sParam);

                                    if (iLoginDTUpdate > 0)
                                    {
                                        g_userid = txtID.Text;
                                        g_username = strUserName;
                                        g_language = ddlLanguage.SelectedValue;
                                        g_plant = ds.Tables[0].Rows[0]["SITE_ID"].ToString();
                                        g_IP = GetClientIpAddress();

                                        Session["LoginChk"] = "Y";

                                        // ID 쿠키 저장
                                        HttpCookie userNameCookie = new HttpCookie("UserID", txtID.Text);
                                        userNameCookie.Expires = DateTime.Now.AddDays(30); // 30일 유지
                                        Response.Cookies.Add(userNameCookie);

                                        Response.Redirect("Main.aspx");
                                    }
                                    else
                                    {
                                        strScript = " alert('로그인이 원활하지 않습니다. 다시 시도하세요.'); fn_LoginReset(); ";
                                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                    }
                                }
                                else
                                {
                                    if ((iLoginFaultCnt + 1) > 4)
                                    {
                                        // 임시비밀번호 생성
                                        string strTempPWD = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8).ToUpper();

                                        string strTempPWDRtn = biz.SendTempPWD(strMobile, strTempPWD);
                                        
                                        sParam.Add("TEMP_PWD", cy.Hash(strTempPWD));

                                        strQueryID = "UserInfoData.Set_UserLoginFaultInfoUpdate";

                                        iLoginFaultCnt = biz.SetUserLoginFaultInfoUpdate(strDBName, strQueryID, sParam);

                                        if (iLoginFaultCnt > 0)
                                        {
                                            if (strTempPWDRtn == "OK")
                                            {
                                                strScript = " alert('비밀번호 입력이 5회이상 틀렸습니다. 임시비밀번호가 발급되었습니다. 비밀번호를 변경하세요.'); fn_LoginReset(); ";
                                                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                            }
                                            else {
                                                strScript = " alert('비밀번호 입력이 5회이상 틀렸습니다. 임시비밀번호에 실패하였습니다. 관리자에게 사번을 알려주고 임시비밀번호를 발급 받으시기 바랍니다.'); fn_LoginReset(); ";
                                                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                            }
                                        }
                                        else
                                        {
                                            strScript = " alert('로그인이 원활하지 않습니다. 다시 시도하세요.'); fn_LoginReset(); ";
                                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                        }
                                    }
                                    else {
                                        strQueryID = "UserInfoData.Set_UserLoginFaultCntUpdate";

                                        iLoginFaultCnt = biz.SetUserLoginFaultCntUpdate(strDBName, strQueryID, sParam);

                                        if (iLoginFaultCnt > 0)
                                        {
                                            strScript = " alert('사용자 정보가 올바르지 않습니다. 다시 확인하시기 바랍니다.'); fn_LoginReset(); ";
                                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                        }
                                        else
                                        {
                                            strScript = " alert('로그인이 원활하지 않습니다. 다시 시도하세요.'); fn_LoginReset(); ";
                                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                        }
                                    }
                                }
                            }
                        }
                    }          
                }
                else {
                    strScript = " alert('사용자 정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); fn_LoginReset(); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }                
            }
            else
            {
                strScript = " alert('사용자 정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); fn_LoginReset(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnChange_Click
        protected void btnChange_Click(object sender, EventArgs e)
        {

            int iRtn                = 0;
            string strRtnValChk     = string.Empty;
            string strScript        = string.Empty;
            string strPWD           = string.Empty;
            string strPrevPWD       = string.Empty;
            string strOldPWD        = string.Empty;
            string strNewPWD        = string.Empty;

            strOldPWD = cy.Hash(txtOldPwd.Text);
            strNewPWD = cy.Hash(txtNewPwdConfirm.Text);
            
            //20241105 모의해킹 보강사항
            if (txtNewPwdConfirm.Text.Length < 8)
            {
                strScript = " alert('비밀번호는 최소 8자리 이상이어야 합니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else if (txtUserID.Text == txtNewPwdConfirm.Text)
            {
                strScript = " alert('아이디와 비밀번호는 동일할수 없습니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else if (!CheckPass(txtNewPwdConfirm.Text))
            {
                strScript = " alert('비밀번호는 숫자,영문자,특수문자[!@#$%^&] 조합이어야 합니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else if (txtNewPwdConfirm.Text.Length == 0)
            {
                strScript = " alert('신규 비밀번호를 다시한번 입력하세요.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else if (txtNewPwd.Text != txtNewPwdConfirm.Text)
            {
                strScript = " alert('신규 비밀번호 확인이 올바르지 않습니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                DataSet ds = new DataSet();

                strDBName = "GPDB";
                strQueryID = "UserInfoData.Get_UserInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("USER_ID", txtUserID.Text);
                sParam.Add("USER_PWD", cy.Hash(txtNewPwdConfirm.Text));
                sParam.Add("LogFlag", "N");

                Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

                // 사용자 정보조회
                ds = biz.GetLoginData(strDBName, strQueryID, sParam);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strPWD = ds.Tables[0].Rows[0]["USER_PWD"].ToString();
                        strPrevPWD = ds.Tables[0].Rows[0]["PREV_PWD"].ToString();

                        // 비밀번호 동일여부 체크
                        if (strPWD == strOldPWD)
                        {
                            if (strPrevPWD == strNewPWD)
                            {
                                strScript = " alert('변경한 비밀번호와 이전 비밀번호가 동일합니다. 다시 확인하시기 바랍니다.'); ";
                                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                            }
                            else
                            {

                                strQueryID = "UserInfoData.Set_UserPwdUpdate";

                                iRtn = biz.SetPwdChange(strDBName, strQueryID, sParam);

                                if (iRtn == 1)
                                {
                                    strScript = " alert('비밀번호가 변경 되었습니다.'); fn_hide(); ";
                                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                }
                                else
                                {
                                    strScript = " alert('비밀번호 변경에 실패하였습니다. 관리자에게 문의바립니다.'); fn_hide();";
                                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                }
                            }
                        }
                        else
                        {
                            strScript = " alert('비밀번호가 틀립니다. 다시 확인하시기 바랍니다.'); ";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        }
                    }
                    else
                    {
                        strScript = " alert('사용자 정보가 올바르지 않습니다. 다시 확인하시기 바랍니다.'); fn_hide(); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                }
                else
                {
                    strScript = " alert('사용자 정보가 올바르지 않습니다. 다시 확인하시기 바랍니다.'); fn_hide(); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
            }
        }
        #endregion

        #region btnRefresh_Click
        protected void btnRefresh_Click(object sender, EventArgs e)
        {

            int iRtn = 0;
            string strRtnValChk = string.Empty;
            string strScript = string.Empty;
            string strUserId = string.Empty;
            string strMobile = string.Empty;
            string strGetMobile = string.Empty;

            strUserId = txtUserID_2.Text;
            strMobile = txtTel.Text;

            //20241105 모의해킹 보강사항
            if (strUserId.Length == 0)
            {
                strScript = " alert('사용자 아이디를 입력하세요.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else if (strMobile.Length == 0)
            {
                strScript = " alert('핸드폰 번호를 입력하세요.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                DataSet ds = new DataSet();

                strDBName = "GPDB";
                strQueryID = "UserInfoData.Get_UserInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("USER_ID", strUserId);
                sParam.Add("LogFlag", "N");

                Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

                // 사용자 정보조회
                ds = biz.GetLoginData(strDBName, strQueryID, sParam);

                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        strGetMobile = ds.Tables[0].Rows[0]["USER_MOBILE"].ToString();

                        // 전화번호 동일여부 체크
                        if (strMobile == strGetMobile)
                        {
                            int iLoginFaultCnt = 0;
                            // 임시비밀번호 생성
                            string strTempPWD = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 8).ToUpper();

                            string strTempPWDRtn = biz.SendTempPWD(strMobile, strTempPWD);

                            sParam.Add("TEMP_PWD", cy.Hash(strTempPWD));

                            strQueryID = "UserInfoData.Set_UserLoginFaultInfoUpdate";

                            iLoginFaultCnt = biz.SetUserLoginFaultInfoUpdate(strDBName, strQueryID, sParam);

                            if (iLoginFaultCnt > 0)
                            {
                                if (strTempPWDRtn == "OK")
                                {
                                    strScript = " alert('임시비밀번호가 발급되었습니다. 비밀번호를 변경하세요.'); fn_hide(); ";
                                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                }
                                else
                                {
                                    strScript = " alert('임시비밀번호에 실패하였습니다. 관리자에게 사번을 알려주고 임시비밀번호를 발급 받으시기 바랍니다.'); fn_hide(); ";
                                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                }
                            }
                            else
                            {
                                strScript = " alert('네트워크 장애가 발생했습니다. 다시 시도하세요.'); fn_hide(); ";
                                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                            }
                        }
                        else
                        {
                            strScript = " alert('사용자 정보가 올바르지 않습니다. 다시 확인하시기 바랍니다.'); fn_hide();";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        }
                    }
                    else
                    {
                        strScript = " alert('사용자 정보가 올바르지 않습니다. 다시 확인하시기 바랍니다.'); fn_hide(); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                }
                else
                {
                    strScript = " alert('사용자 정보가 올바르지 않습니다. 다시 확인하시기 바랍니다.'); fn_hide(); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
            }
        }
        #endregion

        #region CheckPass
        public static bool CheckPass(string str)
        {
            // 정규표현식 정의
            Regex reg1 = new Regex("[a-zA-Z]");
            Regex reg2 = new Regex("[0-9]");
            Regex reg3 = new Regex("[\\{\\}\\[\\]/?.,;:|)*~`!^\\-_+<>@#$%&\\\\=\\(\\)\'\"]");
            Regex reg4 = new Regex("[`~!@#$%^&*|\\\\\'\";:/?]"); // 특수문자

            bool bChk = true;

            if (!reg1.IsMatch(str))
            {
                bChk = false;
            }

            if (!reg2.IsMatch(str))
            {
                bChk = false;
            }

            if (!reg3.IsMatch(str))
            {
                bChk = false;
            }

            if (!reg4.IsMatch(str))
            {
                bChk = false;
            }

            return bChk;
        }
        #endregion

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