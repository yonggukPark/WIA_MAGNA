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
using MES.FW.Common.Crypt;

namespace HQCWeb.MonitorMgt.Mon19
{
    public partial class Mon19 : System.Web.UI.Page
    {
        Crypt cy = new Crypt();

        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        Biz.MonitorManagement.Mon19 biz = new Biz.MonitorManagement.Mon19();

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

            DataSet ds = new DataSet();

            strDBName = "GPDB";
            strQueryID = "Mon19Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlWctCd.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                }
            }
            txtFromDt.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtToDt.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtFromDt.Enabled = false;
            txtToDt.Enabled = false;
        }
        #endregion

        #region SetPageInit
        private void SetPageInit()
        {
            cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

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
                strTab[j] = cy.Encrypt(j.ToString());

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("MENU_ID", "Mon19_" + (j + 1).ToString());
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("CUR_MENU_ID", "Mon19");

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
                                arrColumnCaption[j][i] = arrColumn[j][i].ToString();

                                if (ds.Tables[0].Rows[i]["COLUMN_FIX"].ToString() == "true")
                                {
                                    strFix[j] = (i + 1).ToString();
                                }
                            }
                        }
                        else
                        {
                            if (j == 0)
                                arrColumn[j] = new string[] { "SEQ", "COMPANY", "PLANT", "SHOP", "LINE", "PRDPLNDATE", "SENVER", "PRDORDER", "ITEMCODE", "PRDQTY", "PRDCQTY", "LINENAME", "ITEMNAME", "CREATEDBY", "CREATEDAT", "CHANGEDBY", "CHANGEDAT", "EFLAG", "EDATE", "EMSG", "MES_IF_TIME", "KEY_HID" };
                            else if (j == 1)
                                arrColumn[j] = new string[] { "COMPANY", "PLANT", "SHOP", "LINE", "PRODDATE", "PRDSEQ", "ITEMSERIAL", "SENDTYPE", "ITEMCODE", "PRDORDER", "PRDQTY", "RPTDATE", "RPTTIME", "EFLAG", "EDATE", "EMSG", "KEY_HID" };
                            else if (j == 2)
                                arrColumn[j] = new string[] { "SEQ", "COMPANY", "PLANT", "ORDER_NO", "PART_NO", "PLAN_DATE", "PLAN_TIME", "ROUTID", "PLAN_QTY", "CUSTOMER_CD", "CUSTOMER_NM", "START_STORAGE_CD", "DEST_STORAGE_CD", "PLAN_ORDER_NO", "EVENT_FLAG", "TRUCKCD", "CARNO", "DRIVERCD", "DRIVERNM", "EFLAG", "EDATE", "EMSG", "MES_IF_TIME", "KEY_HID" };
                            else if (j == 3)
                                arrColumn[j] = new string[] { "SEQ", "COMPANY", "PLANT", "DLVPLANDT", "DLVACTDT", "CARNO", "DLVPLANOD", "OUTCERTINO", "ORDERNO", "PLATENO", "MATERIAL", "PLANACT", "ACTQTY", "COMPSTATUS", "FROMWAREHOUSE", "STOCKPLACE", "TOWAREHOUSE", "CUSTOMER", "ROUTID", "DRIVERCD", "SCOMMENT", "EFLAG", "EDATE", "EMSG", "KEY_HID" };
                            else if (j == 4)
                                arrColumn[j] = new string[] { "SEQ", "COMPANY", "PLANT", "MOVE_DATE", "MOVE_TYPE", "PART_NO", "S_STORAGE_CD", "T_STORAGE_CD", "S_STOCKPLACE", "T_STOCKPLACE", "MOVE_QTY", "CD_DESC1", "CD_DESC2", "CD_DESC3", "EFLAG", "EDATE", "EMSG", "KEY_HID" };
                            arrColumnCaption[j] = new string[arrColumn[j].Length];
                            arrColumnWidth[j] = new string[arrColumn[j].Length];
                            strFix[j] = "";

                            for (int i = 0; i < arrColumn[j].Length; i++)
                            {
                                arrColumnCaption[j][i] = arrColumn[j][i].ToString();
                                arrColumnWidth[j][i] = "200";
                            }

                            //강제초기화
                            if (j == 4)
                            {
                                arrColumnWidth[0][0] = "38";
                                arrColumnWidth[0][1] = "66";
                                arrColumnWidth[0][2] = "47";
                                arrColumnWidth[0][3] = "43";
                                arrColumnWidth[0][4] = "41";
                                arrColumnWidth[0][5] = "86";
                                arrColumnWidth[0][6] = "54";
                                arrColumnWidth[0][7] = "99";
                                arrColumnWidth[0][8] = "87";
                                arrColumnWidth[0][9] = "56";
                                arrColumnWidth[0][10] = "64";
                                arrColumnWidth[0][11] = "95";
                                arrColumnWidth[0][12] = "306";
                                arrColumnWidth[0][13] = "76";
                                arrColumnWidth[0][14] = "133";
                                arrColumnWidth[0][15] = "80";
                                arrColumnWidth[0][16] = "133";
                                arrColumnWidth[0][17] = "46";
                                arrColumnWidth[0][18] = "111";
                                arrColumnWidth[0][19] = "63";
                                arrColumnWidth[0][20] = "157";
                                arrColumnWidth[0][21] = "0";

                                arrColumnWidth[1][0] = "66";
                                arrColumnWidth[1][1] = "47";
                                arrColumnWidth[1][2] = "43";
                                arrColumnWidth[1][3] = "41";
                                arrColumnWidth[1][4] = "72";
                                arrColumnWidth[1][5] = "57";
                                arrColumnWidth[1][6] = "157";
                                arrColumnWidth[1][7] = "69";
                                arrColumnWidth[1][8] = "87";
                                arrColumnWidth[1][9] = "99";
                                arrColumnWidth[1][10] = "56";
                                arrColumnWidth[1][11] = "71";
                                arrColumnWidth[1][12] = "60";
                                arrColumnWidth[1][13] = "46";
                                arrColumnWidth[1][14] = "111";
                                arrColumnWidth[1][15] = "414";
                                arrColumnWidth[1][16] = "0";

                                arrColumnWidth[2][0] = "38";
                                arrColumnWidth[2][1] = "66";
                                arrColumnWidth[2][2] = "47";
                                arrColumnWidth[2][3] = "111";
                                arrColumnWidth[2][4] = "90";
                                arrColumnWidth[2][5] = "76";
                                arrColumnWidth[2][6] = "74";
                                arrColumnWidth[2][7] = "70";
                                arrColumnWidth[2][8] = "54";
                                arrColumnWidth[2][9] = "46";
                                arrColumnWidth[2][10] = "69";
                                arrColumnWidth[2][11] = "129";
                                arrColumnWidth[2][12] = "122";
                                arrColumnWidth[2][13] = "128";
                                arrColumnWidth[2][14] = "82";
                                arrColumnWidth[2][15] = "65";
                                arrColumnWidth[2][16] = "91";
                                arrColumnWidth[2][17] = "78";
                                arrColumnWidth[2][18] = "78";
                                arrColumnWidth[2][19] = "46";
                                arrColumnWidth[2][20] = "111";
                                arrColumnWidth[2][21] = "63";
                                arrColumnWidth[2][22] = "157";
                                arrColumnWidth[2][23] = "0";

                                arrColumnWidth[3][0] = "45";
                                arrColumnWidth[3][1] = "66";
                                arrColumnWidth[3][2] = "47";
                                arrColumnWidth[3][3] = "77";
                                arrColumnWidth[3][4] = "111";
                                arrColumnWidth[3][5] = "91";
                                arrColumnWidth[3][6] = "128";
                                arrColumnWidth[3][7] = "199";
                                arrColumnWidth[3][8] = "111";
                                arrColumnWidth[3][9] = "82";
                                arrColumnWidth[3][10] = "90";
                                arrColumnWidth[3][11] = "62";
                                arrColumnWidth[3][12] = "55";
                                arrColumnWidth[3][13] = "89";
                                arrColumnWidth[3][14] = "116";
                                arrColumnWidth[3][15] = "85";
                                arrColumnWidth[3][16] = "99";
                                arrColumnWidth[3][17] = "74";
                                arrColumnWidth[3][18] = "70";
                                arrColumnWidth[3][19] = "78";
                                arrColumnWidth[3][20] = "76";
                                arrColumnWidth[3][21] = "46";
                                arrColumnWidth[3][22] = "111";
                                arrColumnWidth[3][23] = "71";
                                arrColumnWidth[3][24] = "0";

                                arrColumnWidth[4][0] = "32";
                                arrColumnWidth[4][1] = "66";
                                arrColumnWidth[4][2] = "47";
                                arrColumnWidth[4][3] = "71";
                                arrColumnWidth[4][4] = "78";
                                arrColumnWidth[4][5] = "90";
                                arrColumnWidth[4][6] = "100";
                                arrColumnWidth[4][7] = "100";
                                arrColumnWidth[4][8] = "99";
                                arrColumnWidth[4][9] = "99";
                                arrColumnWidth[4][10] = "72";
                                arrColumnWidth[4][11] = "141";
                                arrColumnWidth[4][12] = "71";
                                arrColumnWidth[4][13] = "143";
                                arrColumnWidth[4][14] = "46";
                                arrColumnWidth[4][15] = "111";
                                arrColumnWidth[4][16] = "131";
                                arrColumnWidth[4][17] = "0";
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
            lbSearchDt.Text = Dictionary_Data.SearchDic("CREATE_DT", bp.g_language);
            lb1.Text = Dictionary_Data.SearchDic("CANIAS_IF_PROD_PLAN", bp.g_language);
            lb2.Text = Dictionary_Data.SearchDic("CANIAS_IF_PROD_RESULT", bp.g_language);
            lb3.Text = Dictionary_Data.SearchDic("CANIAS_IF_SHIP_PLAN", bp.g_language);
            lb4.Text = Dictionary_Data.SearchDic("CANIAS_IF_SHIP_RESULT", bp.g_language);
            lb5.Text = Dictionary_Data.SearchDic("CANIAS_IF_MOVE_MATERIAL", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;
            string script = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Mon19Data.Get_List";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());
            sParam.Add("FR_DT", txtFromDt.Text.Replace("-", ""));
            sParam.Add("TO_DT", txtToDt.Text.Replace("-", ""));
            sParam.Add("TAB_SEQ", txtTabCount.Text);

            sParam.Add("CUR_MENU_ID", "Mon19");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            FW.Data.Parameters logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_START");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Mon19");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            // 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_END");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Mon19");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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

        #region ddlWct_SelectedIndexChanged
        protected void ddlWct_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] date = WCTService.WCTDatepickerData(ddlWctCd.SelectedValue);
            if (String.IsNullOrEmpty(date[0]))
            {
                txtFromDt.Enabled = true;
                txtToDt.Enabled = true;
                //datepicker 재활성화
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "refreshDate();", true);
            }
            else
            {
                txtFromDt.Text = date[0];
                txtToDt.Text = date[1];
                txtFromDt.Enabled = false;
                txtToDt.Enabled = false;
            }
        }
        #endregion

    }
}