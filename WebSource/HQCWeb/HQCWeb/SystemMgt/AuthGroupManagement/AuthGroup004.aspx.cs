using HQCWeb.FMB_FW;
using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using MES.FW.Common.Crypt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;

using System.Web.Services;

namespace HQCWeb.SystemMgt.AuthGroupManagement
{
    public partial class AuthGroup004 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        protected string strVal = string.Empty;

        public string strUserJson = string.Empty;
        public string strChkUserJson = string.Empty;


        // 암복호화 키값 셋팅
        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            cy.Key = strKey;

            if (!IsPostBack)
            {
                SetCon();

                SetTitle();

                if (Request.Form["hidValue"] != null)
                {
                    strVal = Request.Form["hidValue"].ToString();

                    (Master.FindControl("hidPopValue") as HiddenField).Value = strVal;

                    GetData();
                }
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strErrMessage = string.Empty;

            Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();

            strDBName = "GPDB";
            strQueryID = "AuthGroupData.Get_UserList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();

            sParam.Add("CUR_MENU_ID", "WEB_00090");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            ds = biz.GetUserList(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strErrMessage = ds.Tables[0].Rows[0][1].ToString();

                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), "alert('" + strErrMessage + "');", true);
                }
                else
                {
                    strUserJson = DataTableToJson(ds.Tables[0]);
                }
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbAuthGroupNM.Text = Dictionary_Data.SearchDic("AUTHGROUP_NM", bp.g_language);
            lbUserNM.Text = Dictionary_Data.SearchDic("USER_NM", bp.g_language);
        }
        #endregion

        #region GetData
        protected void GetData()
        {
            string strScript = string.Empty;
            string strTableName = string.Empty;
            string strMessage = string.Empty;
            string strRtn = string.Empty;


            string strUserID = string.Empty;
            string strDeptNM = string.Empty;
            string strUserNM = string.Empty;


            DataSet ds = new DataSet();
            Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            strDBName = "GPDB";
            strQueryID = "AuthGroupData.Get_AuthGroupInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("AUTHGROUP_ID", strSplitValue[0].ToString());

            sParam.Add("CUR_MENU_ID", "WEB_00090");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            
            ds = biz.GetAuthGroupInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strMessage = ds.Tables[0].Rows[0][1].ToString();

                    strRtn = "Error";
                }
                else
                {
                    lbGetAuthGroupNM.Text = ds.Tables[0].Rows[0]["AUTHGROUP_TXT_KR"].ToString();
                }
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('WEB_00090'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }

            lbOrgAuthGroupID.Text = strPVal;

            // 비지니스 클래스 작성            
            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "AuthGroupData.Get_AuthGroupUserInfo";
            
            sParam.Clear();
            sParam.Add("AUTHGROUP_ID", strSplitValue[0].ToString());

            sParam.Add("CUR_MENU_ID", "WEB_00090");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요


            ds.Tables.Clear();

            //상세조회 비지니스 메서드 호출
            ds = biz.GetAuthGroupUserInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strMessage = ds.Tables[0].Rows[0][1].ToString();

                    strRtn = "Error";
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strUserID = ds.Tables[0].Rows[i]["USER_ID"].ToString();
                        strDeptNM = ds.Tables[0].Rows[i]["USER_DEPT"].ToString();
                        strUserNM = ds.Tables[0].Rows[i]["USER_NM"].ToString();

                        if (i == 0)
                        {
                            strRtn = strDeptNM + "^" + strUserNM + "^" + strUserID + "^" + cy.Encrypt(strUserID) + "^" + strUserID;
                        }
                        else
                        {
                            strRtn += "," + strDeptNM + "^" + strUserNM + "^" + strUserID + "^" + cy.Encrypt(strUserID) + "^" + strUserID;
                        }
                    }

                    strChkUserJson = DataTableToJson(ds.Tables[0]);
                }


                if (ds.Tables[0].Rows.Count > 0) {
                    // 변경전 데이터 셋팅
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        string strColumns = string.Empty;

                        strColumns = ds.Tables[0].Columns[i].ToString();

                        if (strDetailValue == "")
                        {
                            strDetailValue = strColumns + ":" + ds.Tables[0].Rows[0][strColumns].ToString();
                        }
                        else
                        {
                            strDetailValue += "," + strColumns + ":" + ds.Tables[0].Rows[0][strColumns].ToString();
                        }
                    }

                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = cy.Encrypt(strDetailValue);
                }
                
                strScript = " fn_AuthUserInfo('" + strRtn + "');  ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('WEB_00090'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion
        
        #region DataTableToJson
        public static string DataTableToJson(DataTable ds)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = 2147483647;
            List<Dictionary<string, object>> listRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in ds.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in ds.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                listRows.Add(row);
            }
            return serializer.Serialize(listRows);
        }
        #endregion

        #region GetUserInfo
        [WebMethod]
        public static string GetUserInfo(string sParams, string sParams2)
        {
            Crypt cy = new Crypt();

            cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

            string[] strValue = cy.Decrypt(sParams).Split('/');

            string strScript = string.Empty;
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            string strDBName = string.Empty;
            string strQueryID = string.Empty;
            string strRtn = string.Empty;
            string strUserID = string.Empty;

            //string[] arrUserID = sParams2.Split(',');

            string strDeptNM = string.Empty;
            string strUserNM = string.Empty;

            /*
            for (int i = 0; i < arrUserID.Length; i++) {
                if (i == 0)
                {
                    strUserID = cy.Decrypt(arrUserID[i].ToString()).Split('/')[0].ToString();
                } else
                {
                    strUserID += "," + cy.Decrypt(arrUserID[i].ToString()).Split('/')[0].ToString();
                }
            }
            // */

            strUserID = sParams2;

            DataSet ds = new DataSet();

            // 비지니스 클래스 작성
            Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();

            strDBName = "GPDB";
            strQueryID = "AuthGroupData.Get_UserInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("USER_ID", strUserID);

            sParam.Add("CUR_MENU_ID", "WEB_00090");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            ds = biz.GetUserInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {

                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strMessage = ds.Tables[0].Rows[0][1].ToString();

                    strRtn = "Error";
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strUserID = ds.Tables[0].Rows[i]["USER_ID"].ToString();
                        strDeptNM = ds.Tables[0].Rows[i]["USER_DEPT"].ToString();
                        strUserNM = ds.Tables[0].Rows[i]["USER_NM"].ToString();

                        if (i == 0)
                        {
                            strRtn = strDeptNM + "^" + strUserNM + "^" + strUserID + "^" + cy.Encrypt(strUserID) + "^" + strUserID;
                        }
                        else
                        {
                            strRtn += "," + strDeptNM + "^" + strUserNM + "^" + strUserID + "^" + cy.Encrypt(strUserID) + "^" + strUserID;
                        }
                    }
                }
            }

            return strRtn;
        }
        #endregion

        #region SetUserInfo
        [WebMethod]
        public static string SetUserInfo(string sParams, string sParams2)
        {
            Crypt cy = new Crypt();
            BasePage bp = new BasePage();

            cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

            string[] strValue = cy.Decrypt(sParams).Split('/');

            string strScript = string.Empty;
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            string strDBName = string.Empty;
            string strQueryID = string.Empty;
            string strRtn = string.Empty;
            string strUserID = string.Empty;

            string[] arrUserID = sParams2.Split(',');

            string strDeptNM = string.Empty;
            string strUserNM = string.Empty;

            int iRtn = 0;

            ///*
            for (int i = 0; i < arrUserID.Length; i++)
            {
                if (i == 0)
                {
                    strUserID = cy.Decrypt(arrUserID[i].ToString()).Split('/')[0].ToString();
                }
                else
                {
                    strUserID += "," + cy.Decrypt(arrUserID[i].ToString()).Split('/')[0].ToString();
                }
            }
            // */

            DataSet ds = new DataSet();

            // 비지니스 클래스 작성
            Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();

            strDBName = "GPDB";
            strQueryID = "AuthGroupData.Set_AuthGroupUserInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("AUTHGROUP_ID",  strValue[0].ToString());
            sParam.Add("USER_ID",       strUserID);


            sParam.Add("REG_ID",        bp.g_userid.ToString());    // 등록자
            sParam.Add("CUD_TYPE",      "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            sParam.Add("CUR_MENU_ID",   "WEB_00090");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요


            ds = biz.GetUserInfo(strDBName, strQueryID, sParam);

            iRtn = biz.DelAuthGroupMenu(strDBName, strQueryID, sParam);

            if (iRtn == 1)
            {
                strRtn = "C";
            }
            else
            {
                strRtn = "E";
            }

            return strRtn;
        }
        #endregion

        #region DelUserInfo
        [WebMethod]
        public static string DelUserInfo(string sParams, string sParams2)
        {
            Crypt cy = new Crypt();
            BasePage bp = new BasePage();

            cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

            string strRtn           = string.Empty;
            string strRtnTable      = string.Empty;

            int iRtn = 0;
            string strRtnValChk     = string.Empty;
            string strScript        = string.Empty;

            string strDBName        = string.Empty;
            string strQueryID       = string.Empty;

            string strTableName     = string.Empty;
            string strMessage       = string.Empty;
            string strUserID        = string.Empty;
            string strDeptNM        = string.Empty;
            string strUserNM        = string.Empty;
            
            string strChkUserJson   = string.Empty;

            string[] strSplitAuthGroup  = cy.Decrypt(sParams).Split('/');
            string[] strSplitUser       = cy.Decrypt(sParams2).Split('/');

            strDBName = "GPDB";
            strQueryID = "AuthGroupData.Set_AuthUserInfoDel";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("AUTHGROUP_ID",   strSplitAuthGroup[0].ToString());
            sParam.Add("USER_ID",        strSplitUser[0].ToString());

            sParam.Add("REG_ID",         bp.g_userid.ToString());
            sParam.Add("CUD_TYPE",       "D");
            sParam.Add("CUR_MENU_ID",    "WEB_00090");

            Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();

            //  등록 서비스 작성
            iRtn = biz.DelAuthUserInfo(strDBName, strQueryID, sParam);

            if (iRtn == 1)
            {
                DataSet ds = new DataSet();
                
                strDBName = "GPDB";
                strQueryID = "AuthGroupData.Get_AuthGroupUserInfo";

                sParam.Clear();

                sParam.Add("AUTHGROUP_ID", strSplitAuthGroup[0].ToString());
                sParam.Add("CUR_MENU_ID", "WEB_00090");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                
                ds.Tables.Clear();

                ds = biz.GetAuthGroupInfo(strDBName, strQueryID, sParam);

                if (ds.Tables.Count > 0)
                {
                    strTableName = ds.Tables[0].TableName.ToString();

                    if (strTableName == "ErrorLog")
                    {
                        strRtn = "E";
                    }
                    else
                    {
                        if (ds.Tables[0].Rows.Count> 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {
                                if (i == 0) {
                                    strChkUserJson = ds.Tables[0].Rows[i]["id"].ToString() + "/" + ds.Tables[0].Rows[i]["title"].ToString();
                                } else {
                                    strChkUserJson += "," + ds.Tables[0].Rows[i]["id"].ToString() + "/" + ds.Tables[0].Rows[i]["title"].ToString();
                                }
                            }

                        }
                    }
                }

                strRtn = "C^" + strSplitUser[0].ToString() + "^" + strChkUserJson;

            }
            else
            {
                strRtn = "E";
            }

            return strRtn;
        }
        #endregion        




    }
}

