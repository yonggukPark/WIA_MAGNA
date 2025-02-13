using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MES.FW.Common.CommonMgt;
using MES.FW.Common.Crypt;

using HQCWeb.FMB_FW;
using HQCWeb.FW;
using System.Web.UI.HtmlControls;

using System.Web.Services;

namespace HQCWeb.SystemMgt.AuthGroupManagement
{
    public partial class AuthGroup003 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        protected string strVal = string.Empty;

        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        // 비지니스 클래스 작성
        //Biz.Sample_Biz biz = new Biz.Sample_Biz();

        #region GRID Setting
        // 메인 그리드에 보여져야할 컬럼 정의
        public string[] arrMainColumn;
        // 메인 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrMainColumnCaption;
        // 메인 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrMainColumnWidth;
        // 메인 그리드 키값 정의
        //public string strMainKeyColumn = "COL_1";
        public string[] strMainKeyColumn = new string[] { "MENU_ID" };
        // 메인 그리드 숨김처리 필드 정의
        public string[] arrMainHiddenColumn;
        // 메인 그리드 Merge 필드 정의
        public string[] arrMainMergeColumn;

        //JSON 전달용 변수
        string jsMainField = string.Empty;
        string jsMainCol = string.Empty;
        string jsMainData = string.Empty;
        #endregion

        public string strList               = string.Empty;
        public string strButtonTableTitle   = string.Empty;
        
        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            cy.Key = strKey;

            SetPageInit();

            if (!IsPostBack)
            {

                SetGridTitle();

                SetTitle();

                if (Request.Form["hidValue"] != null)
                {
                    strVal = Request.Form["hidValue"].ToString();

                    (Master.FindControl("hidPopValue") as HiddenField).Value = strVal;

                    GetMenuData("");


                    SetCon();
                }
            }

            GetButtonTitleData();

            //GetAuthGroupMenuButtonList("");

            string script = "fn_Search()";

            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script, true);
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');
            
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strErrMessage = string.Empty;

            Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();

            strDBName = "GPDB";
            strQueryID = "AuthGroupData.Get_AuthGroupTargetList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            
            sParam.Add("CUR_MENU_ID", "WEB_00090");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            
            ds = biz.GetAuthGroupTargetList(strDBName, strQueryID, sParam);
            
            ddlAuthGroup.Items.Clear();

            ddlAuthGroup.Items.Add(new ListItem("선택하세요", ""));

            if (ds.Tables.Count > 0) {
                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strErrMessage = ds.Tables[0].Rows[0][1].ToString();

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "alert('" + strErrMessage + "');", true);
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (strSplitValue[0].ToString() != ds.Tables[0].Rows[i]["AUTHGROUP_ID"].ToString())
                        {
                            ddlAuthGroup.Items.Add(new ListItem(ds.Tables[0].Rows[i]["AUTHGROUP_TXT_KR"].ToString(), ds.Tables[0].Rows[i]["AUTHGROUP_ID"].ToString()));
                        }
                    }
                }
            }
        }
        #endregion

        #region SetPageInit
        private void SetPageInit()
        {
           
        }
        #endregion

        #region SetGridTitle
        private void SetGridTitle()
        {
            
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbAuthGroupNM.Text      = Dictionary_Data.SearchDic("AUTHGROUP_NM", bp.g_language);
            lbMenuNM.Text           = Dictionary_Data.SearchDic("MENU_NM", bp.g_language);
            lbCopyAuthGroupNM.Text  = Dictionary_Data.SearchDic("TARGET_AUTHGROUP_NM", bp.g_language);
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetMenuData(txtMenuNM.Text);
        }
        #endregion

        #region GetMenuData
        private void GetMenuData(string strMenuNM) {
            string strScript = string.Empty;

            DataSet ds = new DataSet();

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            Biz.SystemManagement.AuthGroupMgt bizAuth = new Biz.SystemManagement.AuthGroupMgt();


            strDBName = "GPDB";
            strQueryID = "AuthGroupData.Get_AuthGroupInfo";

            FW.Data.Parameters sParamAuth = new FW.Data.Parameters();

            sParamAuth.Add("AUTHGROUP_ID",  strSplitValue[0].ToString());        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            sParamAuth.Add("CUR_MENU_ID",   "WEB_00090");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // /* 
            // 상세조회 비지니스 메서드 호출
            ds = bizAuth.GetMenuList(strDBName, strQueryID, sParamAuth);


            if (ds.Tables.Count > 0)
            {
                lbGetAuthGroupNM.Text = ds.Tables[0].Rows[0]["AUTHGROUP_TXT_KR"].ToString();
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('WEB_00090'); parent.fn_ModalCloseDiv(); ";
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", strScript, true);
            }

            ds.Tables.Clear();


            lbOrgAuthGroup.Text = strPVal;
            //lbGetAuthGroupNM.Text = strSplitValue[0].ToString();

            // 비지니스 클래스 작성
            Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();
            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "AuthGroupData.Get_MenuList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            
            sParam.Add("MENU_NM",       strMenuNM);        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            sParam.Add("CUR_MENU_ID",   "WEB_00090");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // /* 
            // 상세조회 비지니스 메서드 호출
            ds = biz.GetMenuList(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                strList =
                        "<thead>\n" +
                                "<tr>\n" +
                                    "<th></th>\n" +
                                    "<th><input type='checkbox' name='menuChkAll' id='menuChkAll' onclick=\"javascript:fn_chkAll();\" /></th>\n" +
                                    "<th>" + Dictionary_Data.SearchDic("MENU_NM", bp.g_language) + " </th>\n" +
                        "</thead>\n" +
                        "<tbody>";

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {
                    strList +=
                            "<tr>\n" +
                                "<td></td>\n" +
                                "<td style='padding:0px;'><input type='checkbox' name='menuChk' id='" + ds.Tables[0].Rows[i]["MENU_ID"].ToString() + "' value='" + cy.Encrypt(ds.Tables[0].Rows[i]["MENU_ID"].ToString()) + "' onclick=\"javascript:fn_menuInfoCall(this);\" /></td>\n" +
                                "<td class='al-l'>" + ds.Tables[0].Rows[i]["MENU_NM"].ToString() + "</td>\n" +
                            "</tr>\n";
                }

                strList += "</tbody>";
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('WEB_00090'); parent.fn_ModalCloseDiv(); ";
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", strScript, true);
            }
            // */
        }
        #endregion

        #region GetButtonTitleData
        private void GetButtonTitleData()
        {
            DataSet ds = new DataSet();

            string strScript = string.Empty;

            Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();

            strDBName = "GPDB";
            strQueryID = "AuthGroupData.Get_ButtonList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("CUR_MENU_ID", "WEB_00090");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            ds = biz.GetButtonList(strDBName, strQueryID, sParam);

            HtmlTable table = new HtmlTable();

            HtmlTableRow tr = null;
            HtmlTableCell th = null;

            tr = new HtmlTableRow();

            th = new HtmlTableCell("th");
            th.InnerText = "";
            th.Style.Add("width", "10px");
            tr.Cells.Add(th);

            th = new HtmlTableCell("th");

            //HtmlInputImage btnA = new HtmlInputImage();
            //btnA.Attributes.Add("src", "../../images/btn/close_off.gif");
            //btnA.Attributes.Add("onclick", "javascript:fn_MenuChkDisabled('Info05')");
            //th.Controls.Add(btnA);

            //strScript = " fn_MenuChkDisabled('Info05'); ";
            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "searchData", strScript, true);
            
            th.InnerText = "";
            th.Style.Add("width", "40px");
            th.Style.Add("padding", "0px");
            tr.Cells.Add(th);

            th = new HtmlTableCell("th");
            th.InnerText = Dictionary_Data.SearchDic("MENU_NM", bp.g_language);
            th.Style.Add("width", "150px");
            tr.Cells.Add(th);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    th = new HtmlTableCell("th");
                    th.InnerText = ds.Tables[0].Rows[i]["BUTTON_TXT_KR"].ToString();
                    th.Style.Add("width", "90px");
                    tr.Cells.Add(th);
                }
            }

            th = new HtmlTableCell("th");
            //th.InnerText = Dictionary_Data.SearchDic("MENU_NM", bp.g_language);
            th.InnerText = "상태값";
            th.Style.Add("width", "120px");
            th.Style.Add("display", "none");
            tr.Cells.Add(th);

            th = new HtmlTableCell("th");
            //th.InnerText = Dictionary_Data.SearchDic("MENU_NM", bp.g_language);
            th.InnerText = "권한그룹";
            th.Style.Add("width", "120px");
            th.Style.Add("display", "none");
            tr.Cells.Add(th);


            tbMenuButtonList.Rows.Add(tr);
        }
        #endregion

        #region GetAuthGroupMenuButtonList
        private void GetAuthGroupMenuButtonList(string strMenuNM)
        {
            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strScript = string.Empty;
            string strTableName = string.Empty;
            string strMessage = string.Empty;
            
            DataSet ds = new DataSet();
            
            // 비지니스 클래스 작성
            Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();
            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "AuthGroupData.Get_AuthGroupMenuButtonList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("AUTHGROUP_ID",  strSplitValue[0].ToString());
            sParam.Add("MENU_NM",       strMenuNM);

            sParam.Add("CUR_MENU_ID",   "WEB_00090");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // /* 
            // 상세조회 비지니스 메서드 호출
            ds = biz.GetMenuList(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strMessage = ds.Tables[0].Rows[0][1].ToString();

                    strScript = " alert('" + strMessage + "'); parent.fn_ModalReloadCall('WEB_00090'); parent.fn_ModalCloseDiv(); ";
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", strScript, true);

                }
                else
                {
                    HtmlTable table = new HtmlTable();

                    HtmlTableRow tr = null;
                    HtmlTableCell td = null;
                    
                    int iBtnCnt = 0;
                    int iRowCnt = 0;

                    string strAuthMenuNM = string.Empty;
                    string strAuthInfo = string.Empty;
                    string strMenuIDs = string.Empty;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {

                        iRowCnt++;

                        if (i == 0) {
                            strAuthMenuNM = ds.Tables[0].Rows[i]["MENU_NM"].ToString();
                            iBtnCnt = Convert.ToInt32(ds.Tables[0].Rows[i]["BUTTON_CNT"].ToString());

                            tr = new HtmlTableRow();
                            tr.ID = "tr" + ds.Tables[0].Rows[i]["MENU_ID"].ToString();

                            td = new HtmlTableCell();
                            td.InnerText = "";
                            td.Style.Add("width", "10px");
                            tr.Cells.Add(td);

                            td = new HtmlTableCell();

                            HtmlInputImage btnA = new HtmlInputImage();
                            btnA.Attributes.Add("src", "../../images/btn/close_off.gif");
                            btnA.Attributes.Add("onclick", "javascript:fn_AuthMenuDel('" +  cy.Encrypt(ds.Tables[0].Rows[i]["MENU_ID"].ToString()) + "')");
                            td.Controls.Add(btnA);
                            
                            td.Style.Add("width", "40px");
                            td.Style.Add("padding", "0px");
                            tr.Cells.Add(td);

                            //strScript = " fn_MenuChkDisabled('" + ds.Tables[0].Rows[i]["MENU_ID"].ToString() + "'); ";
                            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "searchData", strScript, true);

                            strMenuIDs = ds.Tables[0].Rows[i]["MENU_ID"].ToString();

                            td = new HtmlTableCell();
                            td.InnerText = strAuthMenuNM;
                            td.Style.Add("width", "150px");
                            tr.Cells.Add(td);
                        }


                        if (strAuthMenuNM != ds.Tables[0].Rows[i]["MENU_NM"].ToString())
                        {
                            strAuthMenuNM = ds.Tables[0].Rows[i]["MENU_NM"].ToString();

                            tr = new HtmlTableRow();
                            tr.ID = "tr" + ds.Tables[0].Rows[i]["MENU_ID"].ToString();

                            td = new HtmlTableCell();
                            td.InnerText = "";
                            td.Style.Add("width", "10px");
                            tr.Cells.Add(td);

                            td = new HtmlTableCell();
                            HtmlInputImage btnA = new HtmlInputImage();
                            btnA.Attributes.Add("src", "../../images/btn/close_off.gif");
                            btnA.Attributes.Add("onclick", "javascript:fn_AuthMenuDel('" + cy.Encrypt(ds.Tables[0].Rows[i]["MENU_ID"].ToString()) + "')");
                            td.Controls.Add(btnA);

                            td.Style.Add("width", "40px");
                            td.Style.Add("padding", "0px");
                            tr.Cells.Add(td);

                            //strScript = " fn_MenuChkDisabled('" + ds.Tables[0].Rows[i]["MENU_ID"].ToString() + "'); ";
                            //ScriptManager.RegisterStartupScript(Page, this.GetType(), "searchData", strScript, true);

                            strMenuIDs += "," + ds.Tables[0].Rows[i]["MENU_ID"].ToString();

                            td = new HtmlTableCell();
                            td.InnerText = strAuthMenuNM;
                            td.Style.Add("width", "150px");
                            tr.Cells.Add(td);
                        }
                        
                        td = new HtmlTableCell();

                        if (ds.Tables[0].Rows[i]["USE_YN"].ToString() == "N")
                        {
                            HtmlInputCheckBox chk = new HtmlInputCheckBox();
                            chk.Value = ds.Tables[0].Rows[i]["BUTTON_ID"].ToString();
                            chk.Checked = false;
                            chk.Disabled = true;

                            td.Controls.Add(chk);

                            if (strAuthInfo == "")
                            {
                                strAuthInfo = "N";
                            }
                            else
                            {
                                strAuthInfo += ",N";
                            }

                        }
                        else {
                            if (ds.Tables[0].Rows[i]["BUTTON_USE_YN"].ToString() == "Y") {
                                HtmlInputCheckBox chk = new HtmlInputCheckBox();
                                chk.Value = ds.Tables[0].Rows[i]["BUTTON_ID"].ToString();
                                chk.Checked = true;

                                td.Controls.Add(chk);

                                if (strAuthInfo == "")
                                {
                                    strAuthInfo = "Y";
                                }
                                else {
                                    strAuthInfo += ",Y";
                                }

                            }
                            else {

                                HtmlInputCheckBox chk = new HtmlInputCheckBox();
                                chk.Value = ds.Tables[0].Rows[i]["BUTTON_ID"].ToString();
                                chk.Checked = false;

                                td.Controls.Add(chk);

                                if (strAuthInfo == "")
                                {
                                    strAuthInfo = "N";
                                }
                                else
                                {
                                    strAuthInfo += ",N";
                                }
                            }
                        }
                        
                        td.Style.Add("width", "120px");
                        tr.Cells.Add(td);

                        if (iBtnCnt == iRowCnt) {

                            td = new HtmlTableCell();
                            td.InnerText = strAuthInfo;
                            td.Style.Add("width", "120px");
                            td.Style.Add("display", "none");
                            tr.Cells.Add(td);

                            td = new HtmlTableCell();
                            td.InnerText = "A";
                            td.Style.Add("width", "120px");
                            td.Style.Add("display", "none");
                            tr.Cells.Add(td);

                            strAuthInfo = "";
                            iRowCnt = 0;
                        }

                        tbMenuButtonList.Rows.Add(tr);
                    }


                    if (strMenuIDs != "") {
                        strScript = " fn_MenuChkDisabled('" + strMenuIDs + "'); ";
                        ScriptManager.RegisterStartupScript(Page, this.GetType(), "searchData", strScript, true);
                    }
                }
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('WEB_00090'); parent.fn_ModalCloseDiv(); ";
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", strScript, true);
            }
            // */
        }
        #endregion
        
        #region SetAllMenuButtonList
        [WebMethod]
        public static string SetAllMenuButtonList(string sParams)
        {
            string strAuthButtonList = string.Empty;

            Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();

            DataSet ds = new DataSet();

            string strDBNameS = "GPDB";
            string strQueryIDS = "AuthGroupData.Get_MenuButtonList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("MENU_INFO",     sParams);

            sParam.Add("CUR_MENU_ID", "WEB_00090");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            ds = biz.GetMenuButtonList(strDBNameS, strQueryIDS, sParam);

            string strMenuNM = string.Empty;           

            int iBtnCnt = 0;
            int iRowCnt = 0;

            string strTableName = string.Empty;
            string strMessage = string.Empty;

            if (ds.Tables.Count > 0)
            {

                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strMessage = ds.Tables[0].Rows[0][1].ToString();

                    strAuthButtonList = "Error";
                }
                else
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        iRowCnt++;

                        if (i == 0)
                        {
                            strMenuNM = ds.Tables[0].Rows[0]["MENU_NM"].ToString();

                            strAuthButtonList += strMenuNM;

                            iBtnCnt = Convert.ToInt32(ds.Tables[0].Rows[0]["BUTTON_COUNT"].ToString());
                        }

                        if (strMenuNM != ds.Tables[0].Rows[i]["MENU_NM"].ToString())
                        {
                            strMenuNM = ds.Tables[0].Rows[i]["MENU_NM"].ToString();

                            strAuthButtonList += "^" + strMenuNM;
                        }

                        strAuthButtonList += "," + ds.Tables[0].Rows[i]["BUTTON_ID"].ToString() + "/" + ds.Tables[0].Rows[i]["USE_YN"].ToString();

                        if (iRowCnt == iBtnCnt)
                        {
                            strAuthButtonList += "," + ds.Tables[0].Rows[i]["MENU_ID"].ToString();

                            iRowCnt = 0;
                        }
                    }
                }                
            }

            return strAuthButtonList;
        }
        #endregion        

        #region SetMenuButtonList
        [WebMethod]
        public static string SetMenuButtonList(string sParams)
        {
            Crypt cyS = new Crypt();

            cyS.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");
            
            string[] strValue = cyS.Decrypt(sParams).Split('/');

            string strAuthButtonList = string.Empty;

            Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();
            
            DataSet ds = new DataSet();
            
            string strDBNameS = "GPDB";
            string strQueryIDS = "AuthGroupData.Get_MenuButtonList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("MENU_INFO",     strValue[0].ToString());

            sParam.Add("CUR_MENU_ID",   "WEB_00090");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            ds = biz.GetMenuButtonList(strDBNameS, strQueryIDS, sParam);

            string strMenuNM = string.Empty;
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            // /* 
            if (ds.Tables.Count > 0)
            {
                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strMessage = ds.Tables[0].Rows[0][1].ToString();

                    strAuthButtonList = "Error";
                }
                else {
                    strAuthButtonList = ds.Tables[0].Rows[0]["MENU_NM"].ToString();

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        strAuthButtonList += "," + ds.Tables[0].Rows[i]["BUTTON_ID"].ToString() + "/" + ds.Tables[0].Rows[i]["USE_YN"].ToString();
                    }

                    strAuthButtonList += "," + strValue[0].ToString();
                }
            }
            // */
            
            return strAuthButtonList;
        }
        #endregion        
       
        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            int iRtn = 0;
            string strRtnValChk = string.Empty;
            string strScript = string.Empty;

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                strDBName = "GPDB";
                strQueryID = "AuthGroupData.Set_AuthGroupMenuButtonInfo";

                FW.Data.Parameters param = new FW.Data.Parameters();
                param.Add("BUTTON_INFO", txtInsertInfo.Text);
                param.Add("AUTHGROUP_ID", strSplitValue[0].ToString());

                param.Add("REG_ID",         bp.g_userid.ToString());
                param.Add("CUD_TYPE", "C");
                param.Add("CUR_MENU_ID", "WEB_00090");

                if (txtInsertInfo.Text == "")
                {
                    strScript = " alert('권한정보가 올바르지 않습니다. 관리자에게 문의바랍니다.'); parent.fn_ModalCloseDiv(); ";
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", strScript, true);
                }
                else
                {
                    Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();

                    //  등록 서비스 작성
                    iRtn = biz.SetAuthGroupButtonInfo(strDBName, strQueryID, param);

                    if (iRtn == 1)
                    {
                        strScript = " alert('정상등록 되었습니다.'); parent.fn_ModalReloadCall('WEB_00090'); parent.fn_ModalCloseDiv(); ";
                        //strScript = " alert('정상등록 되었습니다.'); ";
                        //ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", strScript, true);
                    }
                    else
                    {
                        strScript = " alert('등록에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                        //ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", strScript, true);
                    }
                }
            }
            else
            {
                strScript = " fn_ExError(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }            
        }
        #endregion
        
        #region SetAuthGroupButtonInfoAdd
        [WebMethod]
        public static string SetAuthGroupButtonInfoAdd(string sParams, string sParams2)
        {
            Crypt cy = new Crypt();
            BasePage bp = new BasePage();

            cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

            string strRtn = string.Empty;
            
            int iRtn = 0;
            string strRtnValChk = string.Empty;
            string strScript = string.Empty;

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            string[] strSplitAuthGroup = cy.Decrypt(sParams2).Split('/');


            strDBName = "GPDB";
            strQueryID = "AuthGroupData.Set_AuthGroupMenuButtonInfo";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("BUTTON_INFO",    sParams);
            param.Add("AUTHGROUP_ID",   strSplitAuthGroup[0].ToString());

            param.Add("REG_ID",         bp.g_userid.ToString());
            param.Add("CUD_TYPE",       "C");
            param.Add("CUR_MENU_ID",    "WEB_00090");

            if (sParams == "")
            {
                strRtn = "N";
            }
            else
            {
                Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();

                //  등록 서비스 작성
                iRtn = biz.SetAuthGroupButtonInfo(strDBName, strQueryID, param);

                if (iRtn == 1)
                {
                    strRtn = "C";
                }
                else
                {
                    strRtn = "E";
                }
            }

            return strRtn;
        }
        #endregion        

        #region SetAuthGroupMenuDel
        [WebMethod]
        public static string SetAuthGroupMenuDel(string sParams, string sParams2)
        {
            Crypt cy = new Crypt();
            BasePage bp = new BasePage();

            cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

            string strRtn = string.Empty;

            int iRtn = 0;
            string strRtnValChk = string.Empty;
            string strScript = string.Empty;

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            string[] strSplitAuthGroup  = cy.Decrypt(sParams).Split('/');
            string[] strSplitMenu       = cy.Decrypt(sParams2).Split('/');

            strDBName = "GPDB";
            strQueryID = "AuthGroupData.Set_AuthGroupMenuDel";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("AUTHGROUP_ID",   strSplitAuthGroup[0].ToString());
            param.Add("MENU_ID",        strSplitMenu[0].ToString());

            param.Add("REG_ID",         bp.g_userid.ToString());
            param.Add("CUD_TYPE",       "D");
            param.Add("CUR_MENU_ID",    "WEB_00090");
                       
            Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();

            //  등록 서비스 작성
            iRtn = biz.DelAuthGroupMenu(strDBName, strQueryID, param);

            if (iRtn == 1)
            {
                strRtn = "C," + strSplitMenu[0].ToString();
            }
            else
            {
                strRtn = "E";
            }
            
            return strRtn;
        }
        #endregion        

        #region GetMenuSearch
        [WebMethod]
        public static string GetMenuSearch(string sParams, string sParams2)
        {
            //GetAuthGroupMenuButtonList(strMenuNM);
            Crypt cy = new Crypt();

            cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

            string[] strValue = cy.Decrypt(sParams).Split('/');

            string strScript    = string.Empty;
            string strTableName = string.Empty;
            string strMessage   = string.Empty;

            string strDBName    = string.Empty;
            string strQueryID   = string.Empty;
            string strRtn       = string.Empty;
            string strMenuNM    = string.Empty;

            int iRowCnt = 0;
            int iBtnCnt = 0;

            DataSet ds = new DataSet();

            // 비지니스 클래스 작성
            Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();
            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "AuthGroupData.Get_AuthGroupMenuButtonList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("AUTHGROUP_ID",  strValue[0].ToString());
            sParam.Add("MENU_NM",       sParams2);

            sParam.Add("CUR_MENU_ID",   "WEB_00090");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            
            ds = biz.GetMenuButtonList(strDBName, strQueryID, sParam);

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
                        iRowCnt++;

                        if (i == 0)
                        {
                            strMenuNM = ds.Tables[0].Rows[0]["MENU_NM"].ToString();

                            strRtn += strMenuNM;

                            iBtnCnt = Convert.ToInt32(ds.Tables[0].Rows[0]["BUTTON_CNT"].ToString());
                        }

                        if (strMenuNM != ds.Tables[0].Rows[i]["MENU_NM"].ToString())
                        {
                            strMenuNM = ds.Tables[0].Rows[i]["MENU_NM"].ToString();

                            strRtn += "^" + strMenuNM;
                        }

                        strRtn += "," + ds.Tables[0].Rows[i]["BUTTON_ID"].ToString() + "/" + ds.Tables[0].Rows[i]["BUTTON_USE_YN"].ToString() + "/" + ds.Tables[0].Rows[i]["USE_YN"].ToString();

                        if (iRowCnt == iBtnCnt)
                        {
                            strRtn += "," + cy.Encrypt(ds.Tables[0].Rows[i]["MENU_ID"].ToString()) + "||" + ds.Tables[0].Rows[i]["MENU_ID"].ToString();

                            iRowCnt = 0;
                        }
                    }
                }
            }

            return strRtn;
        }
        #endregion

        #region SetAuthGroupMenuCopy
        [WebMethod]
        public static string SetAuthGroupMenuCopy(string sParams, string sParams2)
        {
            Crypt cy = new Crypt();
            BasePage bp = new BasePage();

            cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

            string strRtn = string.Empty;

            int iRtn = 0;
            string strRtnValChk = string.Empty;
            string strScript = string.Empty;

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            string[] strSplitAuthGroup = cy.Decrypt(sParams).Split('/');

            strDBName = "GPDB";
            strQueryID = "AuthGroupData.Set_AuthGroupMenuCopy";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("ORG_AUTHGROUP_ID",       strSplitAuthGroup[0].ToString());
            param.Add("TARGET_AUTHGROUP_ID",    sParams2);

            param.Add("REG_ID",         bp.g_userid.ToString());
            param.Add("CUD_TYPE",               "U");
            param.Add("CUR_MENU_ID",            "WEB_00090");

            Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();

            //  등록 서비스 작성
            iRtn = biz.DelAuthGroupMenu(strDBName, strQueryID, param);

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
    }
}
