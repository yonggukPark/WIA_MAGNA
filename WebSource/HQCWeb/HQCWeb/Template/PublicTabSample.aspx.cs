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

namespace HQCWeb.Template
{
    public partial class PublicTabSample : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;
        
        //버튼 로그 클래스 작성
        Biz.SystemManagement.ButtonStatisticsMgt btnlog = new Biz.SystemManagement.ButtonStatisticsMgt();

        #region GRID Setting
        // 그리드에 보여져야할 컬럼 정의
        public string[][] arrColumn;
        // 그리드에 보여져야할 컬럼타이틀 정의
        public string[][] arrColumnCaption;
        // 그리드에 보여져야할 컬럼 넓이 정의
        public string[][] arrColumnWidth;
        // 그리드 고정값 정의
        public string[] strFix;
        // 그리드 키값 정의
        public string[] strKeyColumn1 = new string[] { "COL_1" };
        public string[] strKeyColumn2 = new string[] { "COL_1" };
        public string[] strKeyColumn3 = new string[] { "COL_1" };
        public string[] strKeyColumn4 = new string[] { "COL_1" };
        public string[] strKeyColumn5 = new string[] { "COL_1" };

        //JSON 전달용 변수
        string[] jsField;
        string[] jsCol;
        string jsData = string.Empty;

        //번호 전달용 변수
        string[] strTab;

        //번호 변수
        public int cnt = 5;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageInit();

            if (!IsPostBack)
            {
                SetCon();

                SetGridTitle();

                SetTitle();

                txtTabCount.Text = "1";

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid1' 함수 호출
                string script1 = $@" column1 = {jsCol[0]}; 
                                field1 = {jsField[0]}; 
                                createGrid1('" + strFix[0] + "'); ";

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid2' 함수 호출
                string script2 = $@" column2 = {jsCol[1]}; 
                                field2 = {jsField[1]}; 
                                createGrid2('" + strFix[1] + "'); ";

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid3' 함수 호출
                string script3 = $@" column3 = {jsCol[2]}; 
                                field3 = {jsField[2]}; 
                                createGrid3('" + strFix[2] + "'); ";

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid4' 함수 호출
                string script4 = $@" column4 = {jsCol[3]}; 
                                field4 = {jsField[3]}; 
                                createGrid4('" + strFix[3] + "'); ";

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid5' 함수 호출
                string script5 = $@" column5 = {jsCol[4]}; 
                                field5 = {jsField[4]}; 
                                createGrid5('" + strFix[4] + "'); ";

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script1, true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script2, true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script3, true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script4, true);
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script5, true);
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            strErrMessage = Message_Data.SearchDic("SearchError", bp.g_language);
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
            strFix = new string[cnt];
            arrColumn = new string[cnt][];
            arrColumnCaption = new string[cnt][];
            arrColumnWidth = new string[cnt][];
            strTab = new string[cnt];

            for (int j = 0; j < cnt; j++)
            {
                //Crypt 추가 후 주석해제 필요
                //strTab[j] = cy.Encrypt(j.ToString());
                strTab[j] = j.ToString();

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("MENU_ID", "MENU_ID_" + (j + 1).ToString());
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("CUR_MENU_ID", "MENU_ID");

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

                            arrColumn[j] = new string[iRowCnt];
                            arrColumnCaption[j] = new string[iRowCnt];
                            arrColumnWidth[j] = new string[iRowCnt];

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                arrColumn[j][i] = ds.Tables[0].Rows[i]["COLUMN_NAME"].ToString();
                                arrColumnWidth[j][i] = ds.Tables[0].Rows[i]["COLUMN_WIDTH"].ToString();
                                arrColumnCaption[j][i] = Dictionary_Data.SearchDic(arrColumn[j][i].ToString(), bp.g_language);  

                                if (ds.Tables[0].Rows[i]["COLUMN_FIX"].ToString() == "true")
                                {
                                    strFix[j] = (i + 1).ToString();
                                }
                            }
                        }
                        else
                        {
                            if (j == 0)
                                arrColumn[j] = new string[] { "COL_1", "COL_2", "COL_3", "COL_4", "COL_5", "KEY_HID" };
                            else if (j == 1)
                                arrColumn[j] = new string[] { "COL_1", "COL_2", "COL_3", "COL_4", "COL_5", "KEY_HID" };
                            else if (j == 2)
                                arrColumn[j] = new string[] { "COL_1", "COL_2", "COL_3", "COL_4", "COL_5", "KEY_HID" };
                            else if (j == 3)
                                arrColumn[j] = new string[] { "COL_1", "COL_2", "COL_3", "COL_4", "COL_5", "KEY_HID" };
                            else if (j == 4)
                                arrColumn[j] = new string[] { "COL_1", "COL_2", "COL_3", "COL_4", "COL_5", "KEY_HID" };
                            arrColumnCaption[j] = new string[arrColumn[j].Length];
                            arrColumnWidth[j] = new string[arrColumn[j].Length];
                            strFix[j] = "";

                            for (int i = 0; i < arrColumn[j].Length; i++)
                            {
                                arrColumnCaption[j][i] = Dictionary_Data.SearchDic(arrColumn[j][i].ToString(), bp.g_language);
                                arrColumnWidth[j][i] = "200";
                            }

                            //강제초기화
                            if (j == cnt - 1)
                            {
                            }
                        }
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), "Faile", "fn_NoData();", true);
                }
            }
        }
        #endregion

        #region SetGridTitle
        private void SetGridTitle()
        {
            jsCol = new string[cnt];
            jsField = new string[cnt];

            //realGrid 방식
            //그리드 컬럼 데이터를 JSON string으로 변환합니다.
            for (int i = 0; i < cnt; i++)
            {
                jsCol[i] = ConvertJSONData.ConvertColArrToJSON(arrColumn[i], arrColumnCaption[i], arrColumnWidth[i], "cols");
                jsField[i] = ConvertJSONData.ConvertColArrToJSON(arrColumn[i], arrColumnCaption[i], arrColumnWidth[i], "fields");
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbCond_01.Text = Dictionary_Data.SearchDic("CON_01", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;
            string script = string.Empty;

            strDBName = "DBNAME";
            strQueryID = "SQLMAP_NAMESPACE.STATEMENT_ID";

            // 비지니스 클래스 작성
            //Biz.Sample_Biz biz = new Biz.Sample_Biz();

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("Param1", "");

            sParam.Add("CUR_MENU_ID", "MENU_ID");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            FW.Data.Parameters logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_START");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "MENU_ID");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            // 비지니스 메서드 호출
            //ds = biz.(strDBName, strQueryID, sParam);

            logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_END");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "MENU_ID");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

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
                        string[] strKeyColumn = null;

                        switch (txtTabCount.Text)
                        {
                            case "1":
                                strKeyColumn = strKeyColumn1;
                                break;
                            case "2":
                                strKeyColumn = strKeyColumn2;
                                break;
                            case "3":
                                strKeyColumn = strKeyColumn3;
                                break;
                            case "4":
                                strKeyColumn = strKeyColumn4;
                                break;
                            case "5":
                                strKeyColumn = strKeyColumn5;
                                break;
                        }

                        jsData = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[0], strKeyColumn);

                        //정상처리되면
                        if (!String.IsNullOrEmpty(jsData))
                        {
                            script = " data" + txtTabCount.Text.ToString() + $@"= {jsData}; 
                            createGrid" + txtTabCount.Text.ToString() + "('" + strFix[Convert.ToInt32(txtTabCount.Text) - 1] + "'); ";
                            (Master.FindControl("MainContent").FindControl("up" + txtTabCount.Text) as UpdatePanel).Update();

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);
                        }
                        else
                        {
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            script = " data" + txtTabCount.Text + " = ''; createGrid" + txtTabCount.Text + "(); ";
                            (Master.FindControl("MainContent").FindControl("up" + txtTabCount.Text) as UpdatePanel).Update();

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);
                            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData();", true);
                        }

                    }
                    else
                    {

                        // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                        script = " data" + txtTabCount.Text + " = ''; createGrid" + txtTabCount.Text + "(); ";
                        (Master.FindControl("MainContent").FindControl("up" + txtTabCount.Text) as UpdatePanel).Update();

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
        
    }
}