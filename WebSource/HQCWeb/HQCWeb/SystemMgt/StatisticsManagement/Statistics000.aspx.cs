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

namespace HQCWeb.SystemMgt.StatisticsManagement
{
    public partial class Statistics000 : System.Web.UI.Page
    {
        BasePage bp         = new BasePage();
        ExcelExport ee      = new ExcelExport();

        string strDBName        = string.Empty;
        string strQueryID       = string.Empty;
        string strErrMessage    = string.Empty;

        //버튼 로그 클래스 작성
        Biz.SystemManagement.ButtonStatisticsMgt btnlog = new Biz.SystemManagement.ButtonStatisticsMgt();

        #region GRID Setting
        // 그리드에 보여져야할 컬럼 정의
        public string[] arrColumn;
        // 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrColumnCaption;
        // 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrColumnWidth;
        // 그리드 고정값 정의
        public string strFix;
        // 그리드 키값 정의
        public string[] strKeyColumn = new string[] { "MENU_ID" };

        //JSON 전달용 변수
        string jsField = string.Empty;
        string jsCol = string.Empty;
        string jsData = string.Empty;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            //SetPageInit();

            if (!IsPostBack)
            {
                SetCon();

                SetGridTitle();

                SetTitle();


                if (arrColumn != null)
                {
                    // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid' 함수 호출
                    string script = $@" column = {jsCol}; 
                                field = {jsField}; 
                                createGrid('" + strFix + "'); ";

                    //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script, true);
                }
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            ddlYear.Items.Clear();
            ddlMonth.Items.Clear();
            ddlGroupCode.Items.Clear();

            ddlGroupCode.Items.Add(new ListItem("선택하세요.", ""));
                        
            int iYear = 0;
            int iMonth = 0;

            iYear = Convert.ToInt16(System.DateTime.Now.ToString("yyyy"));
            iMonth = 12;

            // 신공장 생성일자기준으로 초기 셋팅을 2024로 하드코딩 처리 한다.
            for (int i = 2024; i < (iYear + 5); i++) {
                ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            for (int i = 1; i <= iMonth; i++) {
                if (i < 10)
                {
                    ddlMonth.Items.Add(new ListItem("0" + i.ToString(), "0" + i.ToString()));
                }
                else {
                    ddlMonth.Items.Add(new ListItem(i.ToString(), i.ToString()));
                }
            }

            ddlYear.SelectedValue = iYear.ToString();
            ddlMonth.SelectedValue = System.DateTime.Now.ToString("MM").ToString();

            DataSet ds = new DataSet();

            strDBName = "GPDB";
            strQueryID = "ComCodeData.Get_ComCodeByComTypeInfo";

            Biz.SystemManagement.ComCodeMgt biz = new Biz.SystemManagement.ComCodeMgt();

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("COMM_TYPE",     "USER_DEPT");
            sParam.Add("CUR_MENU_ID",   "WEB_00120");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            ds = biz.GetComCodeByComTypeInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlGroupCode.Items.Add(new ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                }
            }
        }
        #endregion

        #region SetPageInit
        private void SetPageInit()
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

            strDBName = "GPDB";
            strQueryID = "UserInfoData.Get_UserMenuSettingInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("MENU_ID",       "WEB_00120");
            sParam.Add("USER_ID",       bp.g_userid.ToString());

            sParam.Add("CUR_MENU_ID",   "WEB_00120");

            ds = biz.GetUserMenuSettingInfoList(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strMessage = ds.Tables[0].Rows[0][1].ToString();

                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "createGrid();", true);

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_ErrorMessage('" + strMessage + "');", true);
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int iRowCnt = ds.Tables[0].Rows.Count;

                        arrColumn = new string[iRowCnt];
                        arrColumnCaption = new string[iRowCnt];
                        arrColumnWidth = new string[iRowCnt];

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            arrColumn[i] = ds.Tables[0].Rows[i]["COLUMN_NAME"].ToString();
                            arrColumnWidth[i] = ds.Tables[0].Rows[i]["COLUMN_WIDTH"].ToString();
                            arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);

                            if (ds.Tables[0].Rows[i]["COLUMN_FIX"].ToString() == "true")
                            {
                                strFix = (i + 1).ToString();
                            }
                        }
                    }
                    else
                    {
                        arrColumn = new string[] { "COL_1", "COL_2", "COL_3", "COL_4", "COL_5", "KEY_HID" };
                        arrColumnCaption = new string[arrColumn.Length];
                        arrColumnWidth = new string[arrColumn.Length];
                        strFix = "";

                        for (int i = 0; i < arrColumn.Length; i++)
                        {
                            arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);
                            arrColumnWidth[i] = "200";
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Faile", "fn_NoData();", true);
            }
        }
        #endregion

        #region SetGridTitle
        private void SetGridTitle()
        {
            if (arrColumn != null) {
                //realGrid 방식
                //그리드 컬럼 데이터를 JSON string으로 변환합니다.
                jsCol = ConvertJSONData.ConvertColArrToJSON(arrColumn, arrColumnCaption, arrColumnWidth, "cols");
                jsField = ConvertJSONData.ConvertColArrToJSON(arrColumn, arrColumnCaption, arrColumnWidth, "fields");
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbStatisticsType.Text   = Dictionary_Data.SearchDic("STATISTICS_TYPE", bp.g_language); 
            lbSearchDate.Text       = Dictionary_Data.SearchDic("SEARCH_DATE", bp.g_language);
            lbSearchText.Text       = Dictionary_Data.SearchDic("SEARCH_TXT", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;
            
            strDBName = "GPDB";

            FW.Data.Parameters logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_START");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "WEB_00120");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            if (rb01.Checked)
            {
                // 비지니스 클래스 작성
                Biz.SystemManagement.StatisticsMgt biz = new Biz.SystemManagement.StatisticsMgt();

                strQueryID = "StatisticsData.Get_MenuByStatisticsList";

                FW.Data.Parameters sParam = new FW.Data.Parameters();

                sParam.Add("YYYYMM",        ddlYear.SelectedValue + ddlMonth.SelectedValue);
                sParam.Add("SEARCH_TEXT",   txtSearchText.Text);

                sParam.Add("CUR_MENU_ID",   "WEB_00120");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                ds = biz.GetMenuByStatisticsList(strDBName, strQueryID, sParam);
            }

            if (rb02.Checked)
            {
                // 비지니스 클래스 작성
                Biz.SystemManagement.StatisticsMgt biz2 = new Biz.SystemManagement.StatisticsMgt();

                strQueryID = "StatisticsData.Get_UserByStatisticsList";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("YYYYMM",        ddlYear.SelectedValue + ddlMonth.SelectedValue);
                sParam.Add("SEARCH_TEXT",   txtSearchText.Text);

                sParam.Add("CUR_MENU_ID",   "WEB_00120");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                ds = biz2.GetMenuByStatisticsList(strDBName, strQueryID, sParam);
            }

            if (rb03.Checked)
            {
                // 비지니스 클래스 작성
                Biz.SystemManagement.StatisticsMgt biz3 = new Biz.SystemManagement.StatisticsMgt();

                strQueryID = "StatisticsData.Get_UserGroupByStatisticsList";
                
                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("YYYYMM",        ddlYear.SelectedValue + ddlMonth.SelectedValue);
                sParam.Add("GROUP_CODE",    ddlGroupCode.SelectedValue);

                sParam.Add("CUR_MENU_ID",   "WEB_00120");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                ds = biz3.GetMenuByStatisticsList(strDBName, strQueryID, sParam);
            }

            logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_END");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "WEB_00090");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            // 비지니스 메서드 호출
            //

            if (ds.Tables.Count > 0)
            {
                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strErrMessage = ds.Tables[0].Rows[0][1].ToString();

                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "createGrid();", true);

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_ErrorMessage('" + strErrMessage + "');", true);
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        jsData = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[0], strKeyColumn);

                        //정상처리되면
                        if (!String.IsNullOrEmpty(jsData))
                        {
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            string script = $@" data = {jsData}; 
                            createGrid('" + strFix + "'); ";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);
                        }
                        else
                        {
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            string script = $@" data = ''; 
                            createGrid(); ";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);

                            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData();", true);
                        }

                    }
                    else
                    {
                        // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                        string script = $@" data = ''; 
                            createGrid(); ";

                        //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);

                        ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData();", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "createGrid();", true);

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Faile", "fn_NoData();", true);
            }
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }
        #endregion

        #region rb01_CheckedChanged
        protected void rb01_CheckedChanged(object sender, EventArgs e)
        {
            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
            //string script = $@" data = ''; 
            //                createGrid(); ";

            ////직렬화된 JSON을 자바스크립트 변수에 적용합니다.
            //ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);
            
            txtSearchText.Visible = true;
            ddlGroupCode.Visible = false;

            txtSearchText.Text = "";
            ddlGroupCode.SelectedValue = "";

            int iYear = Convert.ToInt32(ddlYear.SelectedValue);
            int iMonth = Convert.ToInt32(ddlMonth.SelectedValue);
            
            int lastday = DateTime.DaysInMonth(iYear, iMonth);

            arrColumn = new string[lastday + 2];

            arrColumn[0] = "MENU_NM";

            for (int i = 1; i <= lastday; i++) {

                if (i < 10)
                {
                    arrColumn[i] = "0" + i.ToString();
                }
                else {
                    arrColumn[i] = i.ToString();
                }                
            }

            arrColumn[lastday + 1] = "KEY_HID";

            //arrColumn = new string[] { "COL_1", "COL_2", "COL_3", "COL_4", "COL_5", "KEY_HID" };
            arrColumnCaption = new string[arrColumn.Length];
            arrColumnWidth = new string[arrColumn.Length];

            for (int i = 0; i < arrColumn.Length; i++)
            {
                arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);
                if (i == 0)
                {
                    arrColumnWidth[i] = "150";
                }
                else {
                    arrColumnWidth[i] = "50";
                }
            }            

            strFix = "1";

            SetGridTitle();

            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid' 함수 호출
            string script = $@" data = ''; 
                                column = {jsCol}; 
                                field = {jsField}; 
                                createGrid('" + strFix + "'); ";

            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script, true);
        }
        #endregion

        #region rb02_CheckedChanged
        protected void rb02_CheckedChanged(object sender, EventArgs e)
        {
            txtSearchText.Visible = true;
            ddlGroupCode.Visible = false;

            txtSearchText.Text = "";
            ddlGroupCode.SelectedValue = "";

            int iYear = Convert.ToInt32(ddlYear.SelectedValue);
            int iMonth = Convert.ToInt32(ddlMonth.SelectedValue);

            int lastday = DateTime.DaysInMonth(iYear, iMonth);

            arrColumn = new string[lastday + 3];

            arrColumn[0] = "USER_NM";
            arrColumn[1] = "MENU_NM";

            for (int i = 1; i <= lastday; i++)
            {
                if (i < 10)
                {
                    arrColumn[i + 1] = "0" + i.ToString();
                }
                else
                {
                    arrColumn[i + 1] = i.ToString();
                }
            }

            arrColumn[lastday + 2] = "KEY_HID";
            
            arrColumnCaption = new string[arrColumn.Length];
            arrColumnWidth = new string[arrColumn.Length];

            for (int i = 0; i < arrColumn.Length; i++)
            {
                arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);
                if (i < 2)
                {
                    arrColumnWidth[i] = "150";
                }
                else
                {
                    arrColumnWidth[i] = "50";
                }
            }

            strFix = "2";

            SetGridTitle();

            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid' 함수 호출
            string script = $@" data = ''
                                column = {jsCol}; 
                                field = {jsField}; 
                                createGrid('" + strFix + "'); ";

            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script, true);
        }
        #endregion

        #region rb03_CheckedChanged
        protected void rb03_CheckedChanged(object sender, EventArgs e)
        {
            txtSearchText.Visible = false;
            ddlGroupCode.Visible = true;

            txtSearchText.Text = "";
            ddlGroupCode.SelectedValue = "";

            int iYear = Convert.ToInt32(ddlYear.SelectedValue);
            int iMonth = Convert.ToInt32(ddlMonth.SelectedValue);

            int lastday = DateTime.DaysInMonth(iYear, iMonth);

            arrColumn = new string[lastday + 3];

            arrColumn[0] = "USER_DEPT";
            arrColumn[1] = "MENU_NM";

            for (int i = 1; i <= lastday; i++)
            {

                if (i < 10)
                {
                    arrColumn[i + 1] = "0" + i.ToString();
                }
                else
                {
                    arrColumn[i + 1] = i.ToString();
                }
            }

            arrColumn[lastday + 2] = "KEY_HID";

            arrColumnCaption = new string[arrColumn.Length];
            arrColumnWidth = new string[arrColumn.Length];

            for (int i = 0; i < arrColumn.Length; i++)
            {
                arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);
                if (i < 2)
                {
                    arrColumnWidth[i] = "150";
                }
                else
                {
                    arrColumnWidth[i] = "50";
                }
            }

            strFix = "2";

            SetGridTitle();

            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid' 함수 호출
            string script = $@" data = ''; 
                                column = {jsCol}; 
                                field = {jsField}; 
                                createGrid('" + strFix + "'); ";

            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script, true);

        }
        #endregion
    }
}