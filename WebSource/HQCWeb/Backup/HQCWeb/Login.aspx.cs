using MES.FW.Common.CommonMgt;
using MES.FW.Common.Crypt;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
                                    g_userid = txtID.Text;
                                    g_username = strUserName;
                                    g_language = ddlLanguage.SelectedValue;

                                    Session["LoginChk"] = "Y";

                                    strQueryID = "UserInfoData.Set_UserLoginDTUpdate";

                                    iLoginDTUpdate = biz.SetUserLoginDTUpdate(strDBName, strQueryID, sParam);

                                    if (iLoginDTUpdate > 0)
                                    {
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

            DataSet ds = new DataSet();

            strDBName = "GPDB";
            strQueryID = "UserInfoData.Get_UserInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("USER_ID",   txtUserID.Text);
            sParam.Add("USER_PWD",  cy.Hash(txtNewPwdConfirm.Text));
            sParam.Add("LogFlag",   "N");

            Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

            // 사용자 정보조회
            ds = biz.GetLoginData(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    strPWD      = ds.Tables[0].Rows[0]["USER_PWD"].ToString();
                    strPrevPWD  = ds.Tables[0].Rows[0]["PREV_PWD"].ToString();
                    
                    // 비밀번호 동일여부 체크
                    if (strPWD == strOldPWD)
                    {
                        if (strPrevPWD == strNewPWD) {
                            strScript = " alert('변경한 비밀번호와 이전 비밀번호가 동일합니다. 다시 확인하시기 바랍니다.'); ";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        } else {
                            
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
        #endregion
    }
}