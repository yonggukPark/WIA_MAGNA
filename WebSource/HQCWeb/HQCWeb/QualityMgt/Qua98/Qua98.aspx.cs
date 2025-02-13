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

namespace HQCWeb.QualityMgt.Qua98
{
    public partial class Qua98 : System.Web.UI.Page
    {
        Crypt cy = new Crypt();

        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        Biz.QualityManagement.Qua98 biz = new Biz.QualityManagement.Qua98();

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
        public string[] strKeyColumn = new string[] { "COL_1" };

        //JSON 전달용 변수
        string[] jsField;
        string[] jsCol;
        string jsData1 = string.Empty;
        string jsData2 = string.Empty;
        string jsData3 = string.Empty;
        string jsData4 = string.Empty;
        string jsData5 = string.Empty;
        string jsData6 = string.Empty;
        string jsData7 = string.Empty;

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

                //실적팝업 세션 체크
                string sharedData = Session["ResCommToQua98"] as string;

                if (!string.IsNullOrEmpty(sharedData))
                {
                    string[] strSplitValue = cy.Decrypt(sharedData).Split('/');
                    string scriptStart = " startBarcode = '"+ strSplitValue[1].ToString() +"';";

                    ddlShopCd.SelectedValue = strSplitValue[0].ToString();
                    ddlSearchType.SelectedValue = strSplitValue[2].ToString();

                    //세션 초기화
                    Session["ResCommToQua98"] = string.Empty;

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), scriptStart, true);
                }

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

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid6' 함수 호출
                string script6 = $@" column6 = {jsCol[5]}; 
                                field6 = {jsField[5]}; 
                                createGrid6('" + strFix[5] + "'); ";

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid7' 함수 호출
                string script7 = $@" column7 = {jsCol[6]}; 
                                field7 = {jsField[6]}; 
                                createGrid7('" + strFix[6] + "'); ";

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

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script6, true);

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script7, true);
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            strErrMessage = Message_Data.SearchDic("SearchError", bp.g_language);

            DataSet ds = new DataSet();

            strDBName = "GPDB";
            strQueryID = "Qua98Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlShopCd.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    ddlSearchType.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    ddlSearchType2.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                }
            }
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
            strFix = new string[7];
            arrColumn = new string[7][];
            arrColumnCaption = new string[7][];
            arrColumnWidth = new string[7][];
            strTab = new string[7];

            for (int j = 0; j < 7; j++)
            {
                strTab[j] = cy.Encrypt(j.ToString()); 

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("MENU_ID", "Qua98_"+(j+1).ToString());
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("CUR_MENU_ID", "Qua98");

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

                                if(j == 2 )
                                    arrColumnCaption[j][i] = (arrColumn[j][i].ToString().Substring(0, 4).Equals("ITEM")) ? ((arrColumn[j][i].ToString().Substring(0, 7).Equals("ITEM_NM")) ? " 검사 항목명" + arrColumn[j][i].ToString().Substring(7) : "검사 측정값" + arrColumn[j][i].ToString().Substring(4)) : Dictionary_Data.SearchDic(arrColumn[j][i].ToString(), bp.g_language);
                                else
                                    arrColumnCaption[j][i] = Dictionary_Data.SearchDic(arrColumn[j][i].ToString(), bp.g_language);

                                if (ds.Tables[0].Rows[i]["COLUMN_FIX"].ToString() == "true")
                                {
                                    strFix[j] = (i + 1).ToString();
                                }
                            }
                        }
                        else
                        {
                            if(j == 0)
                                arrColumn[j] = new string[] { "PLANT_CD", "BSA_SHOP_CD","BSA_LINE_CD","BSA_CAR_TYPE","BSA_PART_NO","BSA_RPT_DATE","BSA_COMPLETE_NO", "BSA_SERIAL_NO", "BSA_CASE_BARCODE", "BPA_SHOP_CD","BPA_LINE_CD","BPA_CAR_TYPE","BPA_PART_NO","BPA_RPT_DATE","BPA_COMPLETE_NO","BPA_SERIAL_NO","BMA_SHOP_CD","BMA_LINE_CD","BMA_CAR_TYPE","BMA_PART_NO","BMA_RPT_DATE","BMA_COMPLETE_NO","BMA_SERIAL_NO","CELL_COMPLETE_NO", "KEY_HID" };
                            else if(j == 1)
                                arrColumn[j] = new string[] { "PLANT_CD", "SHOP_CD", "LINE_CD", "CAR_TYPE", "PART_NO", "COMPLETE_NO", "SERIAL_NO", "STN_NM", "WORK_CD", "DIV", "PART_SERIAL_NO", "REG_DATE", "RPT_USER", "DIFF_MSG", "KEY_HID" };
                            else if(j == 2)
                                arrColumn[j] = new string[] { "PLANT_CD", "SHOP_CD", "LINE_CD", "CAR_TYPE", "PART_NO", "STN_NM", "INSP_DEV", "COMPLETE_NO", "SERIAL_NO", "PROD_DT", "RESULT", "ITEM_NM", "ITEM_VALUE", "KEY_HID" };
                            else if (j == 3)
                                arrColumn[j] = new string[] { "PLANT_CD", "SHOP_CD", "LINE_CD", "CAR_TYPE", "PART_NO", "COMPLETE_NO", "SERIAL_NO", "STN_NM", "WORK_SEQ", "TORQUE_VALUE", "TORQUE_MIN", "TORQUE_MAX", "TORQUE_TARGET", "ANGLE_VALUE", "ANGLE_RESULT", "ANGLE_MIN", "ANGLE_MAX", "R_ANGLE", "R_ANGLE_MIN", "R_ANGLE_MAX", "R_ANGLE_RESULT", "REASON", "RESULT", "PROD_DT", "INSP_DATE", "LOG_SEQ", "DEV_ID", "DEV_NM", "P_SET", "WORK_NM", "KEY_HID" };
                            else if (j == 4)
                                arrColumn[j] = new string[] { "PLANT_CD", "SHOP_CD", "LINE_CD", "CAR_TYPE", "PART_NO", "COMPLETE_NO", "SERIAL_NO", "STN_NM", "STATE", "CAUSE1", "CAUSE2", "CAUSE3", "CAUSE4", "CAUSE5", "MEMO", "PROD_DT", "REINPUT_PROD_DT", "KEY_HID" };
                            else if (j == 5)
                                arrColumn[j] = new string[] { "PLANT_CD", "CREATE_DT", "SEQ", "LINE_CD", "CAR_TYPE", "STN_CD", "WORK_MENU_NM", "SERIAL_NO", "COMPLETE_NO", "D_SYMPTOM", "D_REASON", "D_RESP", "REWORK_MSG", "WORK_TYPE_NM", "DATA_BEF", "DATA_AFT", "RPT_DATE", "RPT_USER", "KEY_HID" };
                            else if (j == 6)
                                arrColumn[j] = new string[] { "PLANT_CD", "SHOP_CD", "LINE_CD", "SERIAL_NO", "COMPLETE_NO", "SERIAL_NO_BEF", "COMPLETE_NO_BEF", "DIV", "PART_SERIAL_NO", "SCAN_PART_NO", "D_SYMPTOM", "D_REASON", "D_RESP", "REWORK_MSG", "MOD_DATE", "MOD_USER", "KEY_HID" };
                            arrColumnCaption[j] = new string[arrColumn[j].Length];
                            arrColumnWidth[j] = new string[arrColumn[j].Length];
                            strFix[j] = "";

                            for (int i = 0; i < arrColumn[j].Length; i++)
                            {
                                arrColumnCaption[j][i] = Dictionary_Data.SearchDic(arrColumn[j][i].ToString(), bp.g_language);
                                arrColumnWidth[j][i] = "200";
                            }
                            //강제초기화
                            if (j == 6)
                            {
                                arrColumnWidth[0][0] = "50";
                                arrColumnWidth[0][1] = "94";
                                arrColumnWidth[0][2] = "105";
                                arrColumnWidth[0][3] = "118";
                                arrColumnWidth[0][4] = "93";
                                arrColumnWidth[0][5] = "98";
                                arrColumnWidth[0][6] = "170";
                                arrColumnWidth[0][7] = "157";
                                arrColumnWidth[0][8] = "157";
                                arrColumnWidth[0][9] = "94";
                                arrColumnWidth[0][10] = "106";
                                arrColumnWidth[0][11] = "97";
                                arrColumnWidth[0][12] = "93";
                                arrColumnWidth[0][13] = "98";
                                arrColumnWidth[0][14] = "158";
                                arrColumnWidth[0][15] = "158";
                                arrColumnWidth[0][16] = "97";
                                arrColumnWidth[0][17] = "129";
                                arrColumnWidth[0][18] = "142";
                                arrColumnWidth[0][19] = "96";
                                arrColumnWidth[0][20] = "101";
                                arrColumnWidth[0][21] = "159";
                                arrColumnWidth[0][22] = "159";
                                arrColumnWidth[0][23] = "170";
                                arrColumnWidth[0][24] = "0";

                                arrColumnWidth[1][0] = "40";
                                arrColumnWidth[1][1] = "43";
                                arrColumnWidth[1][2] = "129";
                                arrColumnWidth[1][3] = "142";
                                arrColumnWidth[1][4] = "92";
                                arrColumnWidth[1][5] = "159";
                                arrColumnWidth[1][6] = "159";
                                arrColumnWidth[1][7] = "192";
                                arrColumnWidth[1][8] = "55";
                                arrColumnWidth[1][9] = "40";
                                arrColumnWidth[1][10] = "325";
                                arrColumnWidth[1][11] = "129";
                                arrColumnWidth[1][12] = "55";
                                arrColumnWidth[1][13] = "53";
                                arrColumnWidth[1][14] = "0";

                                arrColumnWidth[2][0] = "40";
                                arrColumnWidth[2][1] = "42";
                                arrColumnWidth[2][2] = "200";
                                arrColumnWidth[2][3] = "200";
                                arrColumnWidth[2][4] = "92";
                                arrColumnWidth[2][5] = "204";
                                arrColumnWidth[2][6] = "200";
                                arrColumnWidth[2][7] = "143";
                                arrColumnWidth[2][8] = "157";
                                arrColumnWidth[2][9] = "80";
                                arrColumnWidth[2][10] = "35";
                                arrColumnWidth[2][11] = "190";
                                arrColumnWidth[2][12] = "143";
                                arrColumnWidth[2][13] = "0";

                                arrColumnWidth[3][0] = "50";
                                arrColumnWidth[3][1] = "55";
                                arrColumnWidth[3][2] = "129";
                                arrColumnWidth[3][3] = "142";
                                arrColumnWidth[3][4] = "92";
                                arrColumnWidth[3][5] = "159";
                                arrColumnWidth[3][6] = "159";
                                arrColumnWidth[3][7] = "192";
                                arrColumnWidth[3][8] = "83";
                                arrColumnWidth[3][9] = "109";
                                arrColumnWidth[3][10] = "125";
                                arrColumnWidth[3][11] = "123";
                                arrColumnWidth[3][12] = "110";
                                arrColumnWidth[3][13] = "90";
                                arrColumnWidth[3][14] = "101";
                                arrColumnWidth[3][15] = "104";
                                arrColumnWidth[3][16] = "103";
                                arrColumnWidth[3][17] = "80";
                                arrColumnWidth[3][18] = "115";
                                arrColumnWidth[3][19] = "119";
                                arrColumnWidth[3][20] = "101";
                                arrColumnWidth[3][21] = "200";
                                arrColumnWidth[3][22] = "76";
                                arrColumnWidth[3][23] = "80";
                                arrColumnWidth[3][24] = "146";
                                arrColumnWidth[3][25] = "58";
                                arrColumnWidth[3][26] = "63";
                                arrColumnWidth[3][27] = "200";
                                arrColumnWidth[3][28] = "66";
                                arrColumnWidth[3][29] = "199";
                                arrColumnWidth[3][30] = "0";

                                arrColumnWidth[4][0] = "50";
                                arrColumnWidth[4][1] = "55";
                                arrColumnWidth[4][2] = "129";
                                arrColumnWidth[4][3] = "142";
                                arrColumnWidth[4][4] = "92";
                                arrColumnWidth[4][5] = "159";
                                arrColumnWidth[4][6] = "159";
                                arrColumnWidth[4][7] = "200";
                                arrColumnWidth[4][8] = "65";
                                arrColumnWidth[4][9] = "200";
                                arrColumnWidth[4][10] = "200";
                                arrColumnWidth[4][11] = "200";
                                arrColumnWidth[4][12] = "200";
                                arrColumnWidth[4][13] = "200";
                                arrColumnWidth[4][14] = "200";
                                arrColumnWidth[4][15] = "80";
                                arrColumnWidth[4][16] = "80";
                                arrColumnWidth[4][17] = "0";

                                arrColumnWidth[5][0] = "40";
                                arrColumnWidth[5][1] = "71";
                                arrColumnWidth[5][2] = "32";
                                arrColumnWidth[5][3] = "83";
                                arrColumnWidth[5][4] = "119";
                                arrColumnWidth[5][5] = "203";
                                arrColumnWidth[5][6] = "95";
                                arrColumnWidth[5][7] = "157";
                                arrColumnWidth[5][8] = "140";
                                arrColumnWidth[5][9] = "79";
                                arrColumnWidth[5][10] = "79";
                                arrColumnWidth[5][11] = "124";
                                arrColumnWidth[5][12] = "112";
                                arrColumnWidth[5][13] = "54";
                                arrColumnWidth[5][14] = "159";
                                arrColumnWidth[5][15] = "159";
                                arrColumnWidth[5][16] = "129";
                                arrColumnWidth[5][17] = "43";
                                arrColumnWidth[5][18] = "0";

                                arrColumnWidth[6][0] = "40";
                                arrColumnWidth[6][1] = "67";
                                arrColumnWidth[6][2] = "91";
                                arrColumnWidth[6][3] = "157";
                                arrColumnWidth[6][4] = "157";
                                arrColumnWidth[6][5] = "157";
                                arrColumnWidth[6][6] = "157";
                                arrColumnWidth[6][7] = "40";
                                arrColumnWidth[6][8] = "339";
                                arrColumnWidth[6][9] = "101";
                                arrColumnWidth[6][10] = "84";
                                arrColumnWidth[6][11] = "79";
                                arrColumnWidth[6][12] = "124";
                                arrColumnWidth[6][13] = "203";
                                arrColumnWidth[6][14] = "129";
                                arrColumnWidth[6][15] = "43";
                                arrColumnWidth[6][16] = "0";
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
            jsCol = new string[7];
            jsField = new string[7];

            //realGrid 방식
            //그리드 컬럼 데이터를 JSON string으로 변환합니다.
            for (int i = 0; i<7; i++)
            {
                jsCol[i] = ConvertJSONData.ConvertColArrToJSON(arrColumn[i], arrColumnCaption[i], arrColumnWidth[i], "cols");
                jsField[i] = ConvertJSONData.ConvertColArrToJSON(arrColumn[i], arrColumnCaption[i], arrColumnWidth[i], "fields");
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbSearchType.Text = Dictionary_Data.SearchDic("SEARCH_TYPE", bp.g_language);
            lb1.Text = Dictionary_Data.SearchDic("CELL_PROD", bp.g_language);
            lb2.Text = Dictionary_Data.SearchDic("PART_PROD", bp.g_language);
            lb3.Text = Dictionary_Data.SearchDic("INSP_PROD", bp.g_language);
            lb4.Text = Dictionary_Data.SearchDic("TORQ_PROD", bp.g_language);
            lb5.Text = Dictionary_Data.SearchDic("REIN_PROD", bp.g_language);
            //20241023 추가탭
            lb6.Text = Dictionary_Data.SearchDic("REWORK_LOG", bp.g_language);
            lb7.Text = Dictionary_Data.SearchDic("DIFF_REWORK", bp.g_language);
        }
        #endregion

        #region GetData
        //public void GetData(int flag)
        public void GetData()
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;
            string script = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Qua98Data.Get_List";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());
            sParam.Add("SHOP_CD", ddlShopCd.SelectedValue);
            sParam.Add("TYPE", ddlSearchType.SelectedValue);
            if(ddlSearchType2.SelectedValue == "1")
                sParam.Add("BARCODE", txtSearchComboHidden.Text);
            else
                sParam.Add("BARCODE", txtSearchBarcode.Text);
            //sParam.Add("GRID", (flag + 1).ToString());

            sParam.Add("CUR_MENU_ID", "Qua98");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            FW.Data.Parameters logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_START");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Qua98");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            // 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_END");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Qua98");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
                    if (ds.Tables[0].Rows.Count > 0 || ds.Tables[1].Rows.Count > 0 || ds.Tables[2].Rows.Count > 0 || ds.Tables[3].Rows.Count > 0 || ds.Tables[4].Rows.Count > 0 || ds.Tables[5].Rows.Count > 0 || ds.Tables[6].Rows.Count > 0)
                    {
                        jsData1 = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[0], strKeyColumn);
                        jsData2 = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[1], strKeyColumn);
                        jsData3 = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[2], strKeyColumn);
                        jsData4 = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[3], strKeyColumn);
                        jsData5 = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[4], strKeyColumn);
                        jsData6 = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[5], strKeyColumn);
                        jsData7 = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[6], strKeyColumn);

                        //정상처리되면
                        if (!String.IsNullOrEmpty(jsData1) || !String.IsNullOrEmpty(jsData2) || !String.IsNullOrEmpty(jsData3) ||  !String.IsNullOrEmpty(jsData4) || !String.IsNullOrEmpty(jsData5) || !String.IsNullOrEmpty(jsData6) || !String.IsNullOrEmpty(jsData7))
                        {
                            #region 이전 버전 코드(미사용)
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            // 검사기의 경우 추가 처리 필요
                            //if (flag == 2)
                            //{
                            //    string item = string.Empty;
                            //    string itemNm = string.Empty;

                            //    //컬럼 추출(동적 쿼리문에 대응 : 컬럼명이 정해지지 않아 검색시 동적으로 그리드 생성하여 처리)
                            //    string[] columnNames = ds.Tables[0].Columns.Cast<DataColumn>()
                            //             .Select(x => x.ColumnName)
                            //             .ToArray();

                            //    // 추가할 항목 생성
                            //    List<string> newItems = new List<string>();
                            //    List<string> newItemCaption = new List<string>();
                            //    List<string> newItemWidth = new List<string>();

                            //    // 받은 컬럼 순회하여 item 추가
                            //    for (int i = 0; i < columnNames.Length; i++)
                            //    {
                            //        item = columnNames[i];
                            //        if(item.Length < 4)
                            //            itemNm = Dictionary_Data.SearchDic(item, bp.g_language);
                            //        else
                            //            itemNm = (item.Substring(0,4).Equals("ITEM")) ? ((item.Substring(0, 7).Equals("ITEM_NM")) ?  " 검사 항목명"+item.Substring(7) : "검사 측정값" + item.Substring(4)) : Dictionary_Data.SearchDic(item, bp.g_language);
                            //        newItems.Add(item);
                            //        newItemCaption.Add(itemNm);
                            //        newItemWidth.Add("200");
                            //    }


                            //    //기존 필드에 업데이트
                            //    arrColumn[flag] = newItems.ToArray();
                            //    arrColumnCaption[flag] = newItemCaption.ToArray();
                            //    arrColumnWidth[flag] = newItemWidth.ToArray();


                            //    //설정값 검색하여 width 수정
                            //    strQueryID = "UserInfoData.Get_UserMenuSettingInfo";
                            //    Biz.SystemManagement.UserMgt biz2 = new Biz.SystemManagement.UserMgt();

                            //    FW.Data.Parameters sParam2 = new FW.Data.Parameters();
                            //    sParam2.Add("MENU_ID", "Qua98_" + (flag + 1).ToString());
                            //    sParam2.Add("USER_ID", bp.g_userid.ToString());

                            //    sParam2.Add("CUR_MENU_ID", "Qua98");

                            //    ds = biz2.GetUserMenuSettingInfoList(strDBName, strQueryID, sParam2);

                            //    if (ds.Tables.Count > 0)
                            //    {
                            //        strTableName = ds.Tables[0].TableName.ToString();

                            //        if (strTableName == "ErrorLog")
                            //        {
                            //            strMessage = ds.Tables[0].Rows[0][1].ToString();

                            //            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "createGrid();", true);

                            //            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_ErrorMessage('" + strMessage + "');", true);
                            //        }
                            //        else
                            //        {
                            //            if (ds.Tables[0].Rows.Count > 0)
                            //            {
                            //                int len = (ds.Tables[0].Rows.Count > arrColumn[flag].Length) ? arrColumn[flag].Length : ds.Tables[0].Rows.Count;
                            //                for (int i = 0; i < len; i++)
                            //                {
                            //                    if (arrColumn[flag][i].Equals(ds.Tables[0].Rows[i]["COLUMN_NAME"].ToString()))
                            //                    {
                            //                        arrColumnWidth[flag][i] = ds.Tables[0].Rows[i]["COLUMN_WIDTH"].ToString();
                            //                    }

                            //                    if (ds.Tables[0].Rows[i]["COLUMN_FIX"].ToString() == "true")
                            //                    {
                            //                        strFix[flag] = (i + 1).ToString();
                            //                    }
                            //                }
                            //            }
                            //        }
                            //    }

                            //    //컬럼설정, 필드설정
                            //    string newJsCol = ConvertJSONData.ConvertColArrToJSON(arrColumn[flag], arrColumnCaption[flag], arrColumnWidth[flag], "cols");
                            //    string newJsField = ConvertJSONData.ConvertColArrToJSON(arrColumn[flag], arrColumnCaption[flag], arrColumnWidth[flag], "fields");

                            //    script = " column" + (flag + 1).ToString() + $@"= {newJsCol}; field" + (flag + 1).ToString() + $@"= {newJsField}; "
                            //              + " data" + (flag + 1).ToString() + $@"= {jsData}; 
                            //    createGrid" + (flag + 1).ToString() + "('" + strFix[flag] + "'); ";
                            //}
                            //else
                            //{
                            //    script = " data" + (flag + 1).ToString() + $@"= {jsData}; 
                            //    createGrid" + (flag + 1).ToString() + "('" + strFix[flag] + "'); ";
                            //}

                            //script = " data" + (flag + 1).ToString() + $@"= {jsData}; 
                            //createGrid" + (flag + 1).ToString() + "('" + strFix[flag] + "'); ";

                            //(Master.FindControl("MainContent").FindControl("up" + (flag + 1).ToString()) as UpdatePanel).Update();
                            #endregion
                            
                            script = $@" data1 = {jsData1}; data2 = {jsData2}; data3 = {jsData3}; data4 = {jsData4}; data5 = {jsData5}; data6 = {jsData6}; data7 = {jsData7}; 
                            createGrid1('" + strFix[0] + "'); " + "createGrid2('" + strFix[1] + "'); " + "createGrid3('" + strFix[2] + "'); "
                            + "createGrid4('" + strFix[3] + "'); " + "createGrid5('" + strFix[4] + "'); " + "createGrid6('" + strFix[5] + "'); " + "createGrid7('" + strFix[6] + "'); ";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);
                        }
                        else
                        {
                            #region 이전 버전 코드(미사용)
                            //// 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            //script = " data" + (flag + 1).ToString() + " = ''; createGrid" + (flag + 1).ToString() + "(); ";

                            //(Master.FindControl("MainContent").FindControl("up" + (flag + 1).ToString()) as UpdatePanel).Update();
                            #endregion

                            script = $@" data1 = ''; data2 = ''; data3 = ''; data4 = ''; data5 = ''; data6 = ''; data7 = '';
                            createGrid1(); createGrid2(); createGrid3(); createGrid4(); createGrid5(); createGrid6(); createGrid7();";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);

                            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData();", true);
                        }

                    }
                    else
                    {
                        #region 이전 버전 코드(미사용)
                        //// 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                        //script = " data" + (flag + 1).ToString() + " = ''; createGrid" + (flag + 1).ToString() + "(); ";

                        //(Master.FindControl("MainContent").FindControl("up" + (flag + 1).ToString()) as UpdatePanel).Update();
                        #endregion

                        script = $@" data1 = ''; data2 = ''; data3 = ''; data4 = ''; data5 = ''; data6 = ''; data7 = '';
                        createGrid1(); createGrid2(); createGrid3(); createGrid4(); createGrid5(); createGrid6(); createGrid7(); ";

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
            //GetData(Convert.ToInt32(txtTabCount.Text)-1);
            GetData();
        }
        #endregion
    }
}