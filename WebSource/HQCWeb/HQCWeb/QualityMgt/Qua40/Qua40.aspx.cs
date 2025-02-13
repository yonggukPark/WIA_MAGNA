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
using System.Web.Services;

namespace HQCWeb.QualityMgt.Qua40
{
    public partial class Qua40 : System.Web.UI.Page
    {
        Crypt cy = new Crypt();

        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        Biz.QualityManagement.Qua40 biz = new Biz.QualityManagement.Qua40();

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
        public string[] strKeyColumn1 = new string[] { "PLANT_CD", "SEQ" };
        public string[] strKeyColumn2 = new string[] { "PLANT_CD", "SHOP_CD", "P_LINE_CD", "STN_CD", "SERIAL_NO", "PART_SERIAL_NO", "RPT_DATE", "CAR_TYPE", "WORK_CD", "WORK_FLAG" };
        public string[] strKeyColumn3 = new string[] { "PLANT_CD", "SHOP_CD", "P_LINE_CD", "SERIAL_NO", "SHOP_CD_C", "LINE_CD_C", "SERIAL_NO_C", "WORK_FLAG", "RPT_DATE" };
        public string[] strKeyColumn4 = new string[] { "PLANT_CD", "P_DEFECT_DT", "LOG_SEQ", "PART_NO", "LOT_NO" };
        public string[] strKeyColumn5 = new string[] { "COL_1" };

        //JSON 전달용 변수
        string[] jsField;
        string[] jsCol;
        string jsData = string.Empty;
        
        string jsDdl1 = string.Empty;
        string jsDdl2 = string.Empty;
        string jsDdl3 = string.Empty;

        //번호 전달용 변수
        string[] strTab;
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

                // 클라이언트 사이드 변수에 JSON 데이터 할당
                string script0 = $@" cShop = {jsDdl1}; 
                                cLine = {jsDdl2}; 
                                cPart = {jsDdl3}; ";

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script0, true);

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

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid4' 함수 호출
                string script5 = $@" column5 = {jsCol[4]}; 
                                field5 = {jsField[4]}; 
                                createGrid5('" + strFix[4] + "'); ";

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script1, true);

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script2, true);

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script3, true);

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script4, true);

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script5, true);
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            strErrMessage = Message_Data.SearchDic("SearchError", bp.g_language);

            string script = string.Empty;
            jsDdl1 = "[]";
            jsDdl2 = "[]";
            jsDdl3 = "[]";
            DataSet ds = new DataSet();

            ddlDCode.Items.Add(new ListItem("ALL", ""));
            ddlDReasonCode.Items.Add(new ListItem("ALL", ""));
            ddlDRespCd.Items.Add(new ListItem("ALL", ""));
            ddlResultCd.Items.Add(new ListItem("ALL", ""));
            ddlEopFlag.Items.Add(new ListItem("ALL", ""));

            strDBName = "GPDB";
            strQueryID = "Qua40Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", "");
            param.Add("EOP_FLAG", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                jsDdl1 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[0]);
                jsDdl2 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[1]);
                jsDdl3 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[2]);

                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    ddlDCode.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                {
                    ddlDReasonCode.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                {
                    ddlDRespCd.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                {
                    ddlResultCd.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                {
                    ddlWctCd.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[12].Rows.Count; i++)
                {
                    ddlEopFlag.Items.Add(new ListItem(ds.Tables[12].Rows[i]["CODE_NM"].ToString(), ds.Tables[12].Rows[i]["CODE_ID"].ToString()));
                }
            }

            ddlWctCd.SelectedValue = "H";
            txtFromDt.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtToDt.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            rdNo.Checked = true;

            //txtFromDt.Enabled = false;
            //txtToDt.Enabled = false;
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
            strFix = new string[5];
            arrColumn = new string[5][];
            arrColumnCaption = new string[5][];
            arrColumnWidth = new string[5][];
            strTab = new string[5];

            for (int j = 0; j < 5; j++)
            {
                strTab[j] = cy.Encrypt(j.ToString());

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("MENU_ID", "Qua40_" + (j + 1).ToString());
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("CUR_MENU_ID", "Qua40");

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
                                arrColumn[j] = new string[] { "PLANT_CD", "CREATE_DT", "SEQ", "LINE_CD", "CAR_TYPE", "STN_CD", "WORK_MENU_NM", "SERIAL_NO", "COMPLETE_NO", "D_SYMPTOM", "D_REASON", "D_RESP", "REWORK_MSG", "WORK_TYPE_NM", "DATA_BEF", "DATA_AFT", "RPT_DATE", "RPT_USER", "KEY_HID" };
                            else if (j == 1)
                                arrColumn[j] = new string[] { "PLANT_CD", "SHOP_CD", "LINE_CD", "SERIAL_NO", "COMPLETE_NO", "DIV", "PART_SERIAL_NO", "SCAN_PART_NO", "D_SYMPTOM", "D_REASON", "D_RESP", "REWORK_MSG", "MOD_DATE", "MOD_USER", "BARCODE_FLAG", "KEY_HID" };
                            else if (j == 2)
                                arrColumn[j] = new string[] { "PLANT_CD", "SHOP_CD", "LINE_CD", "SERIAL_NO", "COMPLETE_NO", "DIV", "SERIAL_NO_C", "D_SYMPTOM", "D_REASON", "D_RESP", "REWORK_MSG", "MOD_DATE", "MOD_USER", "BARCODE_FLAG", "KEY_HID" };
                            else if (j == 3)
                                arrColumn[j] = new string[] { "PLANT_CD", "DEFECT_DT", "LOG_SEQ", "PART_NO", "PART_DESC", "LOT_NO", "D_SYMPTOM", "D_REASON", "D_RESP", "REWORK_MSG", "RESULT", "DEFECT_CNT", "RETURN_DT", "DECOMPOSE_CD", "STORAGE_CD", "REG_DATE", "REG_USER", "MOD_DATE", "MOD_USER", "KEY_HID" };
                            else if (j == 4)
                                arrColumn[j] = new string[] { "PLANT_CD", "SHOP_CD", "LINE_CD", "SERIAL_NO", "COMPLETE_NO", "DIV", "PART_SERIAL_NO", "SCAN_PART_NO", "D_SYMPTOM", "D_REASON", "D_RESP", "REWORK_MSG", "MOD_DATE", "MOD_USER", "KEY_HID" };
                            arrColumnCaption[j] = new string[arrColumn[j].Length];
                            arrColumnWidth[j] = new string[arrColumn[j].Length];
                            strFix[j] = "";

                            for (int i = 0; i < arrColumn[j].Length; i++)
                            {
                                arrColumnCaption[j][i] = Dictionary_Data.SearchDic(arrColumn[j][i].ToString(), bp.g_language);
                                arrColumnWidth[j][i] = "200";
                            }

                            //강제초기화
                            if (j == 4)
                            { 
                                arrColumnWidth[0][0] = "40";
                                arrColumnWidth[0][1] = "71";
                                arrColumnWidth[0][2] = "32";
                                arrColumnWidth[0][3] = "83";
                                arrColumnWidth[0][4] = "119";
                                arrColumnWidth[0][5] = "203";
                                arrColumnWidth[0][6] = "95";
                                arrColumnWidth[0][7] = "157";
                                arrColumnWidth[0][8] = "140";
                                arrColumnWidth[0][9] = "79";
                                arrColumnWidth[0][10] = "79";
                                arrColumnWidth[0][11] = "124";
                                arrColumnWidth[0][12] = "112";
                                arrColumnWidth[0][13] = "54";
                                arrColumnWidth[0][14] = "159";
                                arrColumnWidth[0][15] = "159";
                                arrColumnWidth[0][16] = "129";
                                arrColumnWidth[0][17] = "43";
                                arrColumnWidth[0][18] = "0";

                                arrColumnWidth[1][0] = "40";
                                arrColumnWidth[1][1] = "67";
                                arrColumnWidth[1][2] = "91";
                                arrColumnWidth[1][3] = "157";
                                arrColumnWidth[1][4] = "157";
                                arrColumnWidth[1][5] = "40";
                                arrColumnWidth[1][6] = "339";
                                arrColumnWidth[1][7] = "101";
                                arrColumnWidth[1][8] = "84";
                                arrColumnWidth[1][9] = "79";
                                arrColumnWidth[1][10] = "124";
                                arrColumnWidth[1][11] = "203";
                                arrColumnWidth[1][12] = "129";
                                arrColumnWidth[1][13] = "43";
                                arrColumnWidth[1][14] = "0";
                                arrColumnWidth[1][15] = "0";

                                arrColumnWidth[2][0] = "40";
                                arrColumnWidth[2][1] = "67";
                                arrColumnWidth[2][2] = "83";
                                arrColumnWidth[2][3] = "155";
                                arrColumnWidth[2][4] = "156";
                                arrColumnWidth[2][5] = "40";
                                arrColumnWidth[2][6] = "173";
                                arrColumnWidth[2][7] = "84";
                                arrColumnWidth[2][8] = "79";
                                arrColumnWidth[2][9] = "124";
                                arrColumnWidth[2][10] = "333";
                                arrColumnWidth[2][11] = "129";
                                arrColumnWidth[2][12] = "43";
                                arrColumnWidth[2][13] = "0";
                                arrColumnWidth[2][14] = "0";

                                arrColumnWidth[3][0] = "40";
                                arrColumnWidth[3][1] = "80";
                                arrColumnWidth[3][2] = "87";
                                arrColumnWidth[3][3] = "92";
                                arrColumnWidth[3][4] = "73";
                                arrColumnWidth[3][5] = "143";
                                arrColumnWidth[3][6] = "84";
                                arrColumnWidth[3][7] = "79";
                                arrColumnWidth[3][8] = "112";
                                arrColumnWidth[3][9] = "102";
                                arrColumnWidth[3][10] = "140";
                                arrColumnWidth[3][11] = "54";
                                arrColumnWidth[3][12] = "80";
                                arrColumnWidth[3][13] = "133";
                                arrColumnWidth[3][14] = "109";
                                arrColumnWidth[3][15] = "129";
                                arrColumnWidth[3][16] = "43";
                                arrColumnWidth[3][17] = "161";
                                arrColumnWidth[3][18] = "43";
                                arrColumnWidth[3][19] = "0";

                                arrColumnWidth[4][0] = "40";
                                arrColumnWidth[4][1] = "67";
                                arrColumnWidth[4][2] = "91";
                                arrColumnWidth[4][3] = "157";
                                arrColumnWidth[4][4] = "157";
                                arrColumnWidth[4][5] = "40";
                                arrColumnWidth[4][6] = "339";
                                arrColumnWidth[4][7] = "101";
                                arrColumnWidth[4][8] = "84";
                                arrColumnWidth[4][9] = "79";
                                arrColumnWidth[4][10] = "124";
                                arrColumnWidth[4][11] = "203";
                                arrColumnWidth[4][12] = "129";
                                arrColumnWidth[4][13] = "43";
                                arrColumnWidth[4][14] = "0";
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
            jsCol = new string[5];
            jsField = new string[5];

            //realGrid 방식
            //그리드 컬럼 데이터를 JSON string으로 변환합니다.
            for (int i = 0; i < 5; i++)
            {
                jsCol[i] = ConvertJSONData.ConvertColArrToJSON(arrColumn[i], arrColumnCaption[i], arrColumnWidth[i], "cols");
                jsField[i] = ConvertJSONData.ConvertColArrToJSON(arrColumn[i], arrColumnCaption[i], arrColumnWidth[i], "fields");
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            //lbSearchDt.Text = Dictionary_Data.SearchDic("SEARCH_DATE", bp.g_language);
            lbSearchDt.Text = Dictionary_Data.SearchDic("CREATE_DT", bp.g_language);
            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbPartNo.Text = Dictionary_Data.SearchDic("PART_NO", bp.g_language);
            lbDCode.Text = Dictionary_Data.SearchDic("D_CODE", bp.g_language);
            lbDReasonCode.Text = Dictionary_Data.SearchDic("D_REASON_CODE", bp.g_language);
            lbDRespCd.Text = Dictionary_Data.SearchDic("D_RESP_CD", bp.g_language);
            lbResultCd.Text = Dictionary_Data.SearchDic("RESULT", bp.g_language);
            lbBarcode.Text = Dictionary_Data.SearchDic("BARCODE", bp.g_language);
            lbSerialNo.Text = Dictionary_Data.SearchDic("SERIAL_NO", bp.g_language);
            lbPartSerialNo.Text = Dictionary_Data.SearchDic("PART_SERIAL_NO", bp.g_language);
            lbPartBarcodeNo.Text = Dictionary_Data.SearchDic("PART_SERIAL_NO", bp.g_language);
            lbDownLevel.Text = Dictionary_Data.SearchDic("DOWN_LEVEL", bp.g_language);
            lb1.Text = Dictionary_Data.SearchDic("REWORK_LOG", bp.g_language);
            lb2.Text = Dictionary_Data.SearchDic("DIFF_HI", bp.g_language);
            lb3.Text = Dictionary_Data.SearchDic("MATCH_HI", bp.g_language);
            lb4.Text = Dictionary_Data.SearchDic("DECOMPOSE_HI", bp.g_language);
            lb5.Text = Dictionary_Data.SearchDic("INPUT_WAIT_HI", bp.g_language);
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
            strQueryID = "Qua40Data.Get_List";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());
            sParam.Add("FR_DT", txtFromDt.Text.Replace("-", ""));
            sParam.Add("TO_DT", txtToDt.Text.Replace("-", ""));
            sParam.Add("SHOP_CD", txtShopCdHidden.Text);
            sParam.Add("LINE_CD", txtLineCdHidden.Text);
            sParam.Add("PART_NO", txtPartNoHidden.Text);
            sParam.Add("D_CODE", ddlDCode.SelectedValue);
            sParam.Add("D_REASON_CODE", ddlDReasonCode.SelectedValue);
            sParam.Add("D_RESP_CD", ddlDRespCd.SelectedValue);
            sParam.Add("SERIAL_NO", txtSerialNo.Text);
            sParam.Add("COMPLETE_NO", txtBarcode.Text);
            sParam.Add("PART_SERIAL_NO", txtPartSerialNo.Text);
            sParam.Add("PART_BARCODE_NO", txtPartBarcodeNo.Text);
            sParam.Add("RESULT", ddlResultCd.SelectedValue);
            sParam.Add("TAB_SEQ", txtTabCount.Text);
            sParam.Add("DOWN_LEVEL", (rdNo.Checked ? "N" : "Y"));

            sParam.Add("CUR_MENU_ID", "Qua40");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            FW.Data.Parameters logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_START");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Qua40");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            // 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_END");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Qua40");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
                            createGrid" + txtTabCount.Text.ToString() + "('" + strFix[Convert.ToInt32(txtTabCount.Text)-1] + "'); ";
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

        #region btnFunctionCall_Click
        protected void btnFunctionCall_Click(object sender, EventArgs e)
        {
            txtShopCd_SelectedIndexChanged();
        }
        #endregion

        #region txtShopCd_SelectedIndexChanged
        private void txtShopCd_SelectedIndexChanged()
        {
            //데이터셋 설정
            string script = string.Empty;
            string jsData1 = "[]";
            DataSet ds = new DataSet();

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Qua40Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", txtShopCdHidden.Text);
            param.Add("EOP_FLAG", ddlEopFlag.SelectedValue);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Line Code 있으면
                if (ds.Tables[1].Rows.Count > 0)
                {
                    jsData1 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[1]);
                }
            }
            script = $@" cLine = {jsData1}; fn_Set_Line(); ";
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "comboData", script, true);
        }
        #endregion

        #region ddlEopFlag_SelectedIndexChanged
        protected void ddlEopFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtShopCd_SelectedIndexChanged();
        }
        #endregion

        #region rd_CheckedChanged
        protected void rd_CheckedChanged(object sender, EventArgs e)
        {
            if (rdYes.Checked)
            {
                txtPartBarcodeNo.Enabled = false;
            }
            else if (rdNo.Checked)
            {
                txtPartBarcodeNo.Enabled = true;
            }
        }
        #endregion
    }
}