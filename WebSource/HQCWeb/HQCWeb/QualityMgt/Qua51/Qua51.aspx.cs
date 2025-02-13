using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MES.FW.Common.Crypt;
using System.Configuration;

namespace HQCWeb.QualityMgt.Qua51
{
    public partial class Qua51 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();
        Crypt cy = new Crypt();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        // 비지니스 클래스 작성
        Biz.QualityManagement.Qua51 biz = new Biz.QualityManagement.Qua51();

        //버튼 로그 클래스 작성
        Biz.SystemManagement.ButtonStatisticsMgt btnlog = new Biz.SystemManagement.ButtonStatisticsMgt();

        #region GRID Setting
        // 메인 그리드에 보여져야할 컬럼 정의
        public string[] arrMainColumn;
        // 메인 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrMainColumnCaption;
        // 메인 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrMainColumnWidth;
        // 그리드 고정값 정의
        public string strMainFix;
        // 메인 그리드 키값 정의
        public string[] strMainKeyColumn = new string[] { "PLANT_CD", "MAN_NO" };
        //JSON 전달용 변수
        string jsMainField = string.Empty;
        string jsMainCol = string.Empty;
        string jsMainData = string.Empty;


        // 상세 그리드에 보여져야할 컬럼 정의
        public string[] arrDetailColumn;
        // 상세 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrDetailColumnCaption;
        // 상세 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrDetailColumnWidth;
        // 상세 그리드 고정값 정의
        public string strDetailFix;
        // 상세 그리드 키값 정의
        public string[] strDetailKeyColumn = new string[] { "PLANT_CD", "MAN_NO", "CERT_NO" };

        //JSON 전달용 변수
        string jsDetailField = string.Empty;
        string jsDetailCol = string.Empty;
        string jsDetailData = string.Empty;

        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            cy.Key = strKey;

            SetPageInit();

            if (!IsPostBack)
            {
                SetCon();

                SetGridTitle();

                SetTitle();

                //캘린더 세션 체크
                string sharedData = Session["CalenderToQua51"] as string;

                if (!string.IsNullOrEmpty(sharedData))
                {
                    string[] strSplitValue = cy.Decrypt(sharedData).Split('/');

                    txtManNo.Text = strSplitValue[0].ToString();
                    ddlYear.SelectedValue = strSplitValue[1].ToString();

                    //세션 초기화
                    Session["CalenderToQua51"] = string.Empty;
                }

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createMainGrid' 함수 호출
                string scriptMain = $@" column = {jsMainCol}; 
                                field = {jsMainField}; 
                                createMainGrid('" + strMainFix + "'); ";


                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createDetailGrid' 함수 호출
                string scriptDetail = $@" column2 = {jsDetailCol}; 
                                field2 = {jsDetailField}; 
                                createDetailGrid('" + strDetailFix + "'); ";


                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), scriptMain, true);

                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), scriptDetail, true);
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            strErrMessage = Message_Data.SearchDic("SearchError", bp.g_language);

            DataSet ds = new DataSet();
            ddlManDept.Items.Add(new ListItem("ALL", ""));
            ddlStatus.Items.Add(new ListItem("ALL", ""));

            strDBName = "GPDB";
            strQueryID = "Qua51Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlYear.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    ddlManDept.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    ddlStatus.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                }
            }
            ddlYear.SelectedValue = System.DateTime.Now.ToString("yyyy");
        }
        #endregion

        #region SetPageInit
        private void SetPageInit()
        {
            //파일경로 설정
            this.hidFileAttachPath.Value = ConfigurationManager.AppSettings["FILE_ATTACH_PATH"];

            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

            strDBName = "GPDB";
            strQueryID = "UserInfoData.Get_UserMenuSettingInfo";

            FW.Data.Parameters sParam = null;

            sParam = new FW.Data.Parameters();
            sParam.Add("MENU_ID", "Qua51");
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("CUR_MENU_ID", "Qua51");

            ds = biz.GetUserMenuSettingInfoList(strDBName, strQueryID, sParam);

            // 메인 그리드
            if (ds.Tables.Count > 0)
            {
                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strMessage = ds.Tables[0].Rows[0][1].ToString();

                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "createMainGrid();", true);

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_ErrorMessage('" + strMessage + "');", true);
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int iRowCnt = ds.Tables[0].Rows.Count;

                        arrMainColumn = new string[iRowCnt];
                        arrMainColumnCaption = new string[iRowCnt];
                        arrMainColumnWidth = new string[iRowCnt];
                        //arrFix              = new string[iRowCnt];

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            arrMainColumn[i] = ds.Tables[0].Rows[i]["COLUMN_NAME"].ToString();
                            arrMainColumnWidth[i] = ds.Tables[0].Rows[i]["COLUMN_WIDTH"].ToString();
                            arrMainColumnCaption[i] = Dictionary_Data.SearchDic(arrMainColumn[i].ToString(), bp.g_language);

                            if (ds.Tables[0].Rows[i]["COLUMN_FIX"].ToString() == "true")
                            {
                                strMainFix = (i + 1).ToString();
                            }
                        }
                    }
                    else
                    {
                        arrMainColumn = new string[] { "PLANT_CD", "STATUS", "MAN_NO", "MAN_DEPT", "PART_NAME", "STANDARD", "PART_SERIAL_NO", "INSP_CYCLE", "INSPECT_DATE", "NEXT_INSPECT_DATE", "KEY_HID" };
                        arrMainColumnCaption = new string[arrMainColumn.Length];
                        arrMainColumnWidth = new string[arrMainColumn.Length];
                        strMainFix = "";

                        for (int i = 0; i < arrMainColumn.Length; i++)
                        {
                            arrMainColumnCaption[i] = Dictionary_Data.SearchDic(arrMainColumn[i].ToString(), bp.g_language);
                            //arrMainColumnWidth[i] = "200";
                        }

                        arrMainColumnWidth[0] = "52";
                        arrMainColumnWidth[1] = "76";
                        arrMainColumnWidth[2] = "200";
                        arrMainColumnWidth[3] = "200";
                        arrMainColumnWidth[4] = "381";
                        arrMainColumnWidth[5] = "200";
                        arrMainColumnWidth[6] = "204";
                        arrMainColumnWidth[7] = "136";
                        arrMainColumnWidth[8] = "137";
                        arrMainColumnWidth[9] = "153";
                        arrMainColumnWidth[10] = "0";
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Faile", "fn_NoData();", true);
            }

            sParam = new FW.Data.Parameters();
            sParam.Add("MENU_ID", "Qua51_2");
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("CUR_MENU_ID", "Qua51_2");

            // 서브 그리드
            ds = biz.GetUserMenuSettingInfoList(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strMessage = ds.Tables[0].Rows[0][1].ToString();

                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "createDetailGrid();", true);

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_ErrorMessage('" + strMessage + "');", true);
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int iRowCnt = ds.Tables[0].Rows.Count;

                        arrDetailColumn = new string[iRowCnt];
                        arrDetailColumnCaption = new string[iRowCnt];
                        arrDetailColumnWidth = new string[iRowCnt];
                        //arrFix              = new string[iRowCnt];

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            arrDetailColumn[i] = ds.Tables[0].Rows[i]["COLUMN_NAME"].ToString();
                            arrDetailColumnWidth[i] = ds.Tables[0].Rows[i]["COLUMN_WIDTH"].ToString();
                            arrDetailColumnCaption[i] = Dictionary_Data.SearchDic(arrDetailColumn[i].ToString(), bp.g_language);

                            if (ds.Tables[0].Rows[i]["COLUMN_FIX"].ToString() == "true")
                            {
                                strDetailFix = (i + 1).ToString();
                            }
                        }
                    }
                    else
                    {
                        arrDetailColumn = new string[] { "PLANT_CD", "RESULT", "CERT_NO", "ISSUE_DT", "FILE_ATTACH", "REG_DATE", "REG_USER", "MOD_DATE", "MOD_USER",
                                                   "ORG_FILE_NAME1", "CHG_FILE_NAME1", "FILE_PATH1", "FILE_EXT1" , "ORG_FILE_NAME2", "CHG_FILE_NAME2", "FILE_PATH2", "FILE_EXT2" , "ORG_FILE_NAME3", "CHG_FILE_NAME3", "FILE_PATH3", "FILE_EXT3" , "ORG_FILE_NAME4", "CHG_FILE_NAME4", "FILE_PATH4", "FILE_EXT4" , "ORG_FILE_NAME5", "CHG_FILE_NAME5", "FILE_PATH5", "FILE_EXT5", "KEY_HID" };
                        arrDetailColumnCaption = new string[arrDetailColumn.Length];
                        arrDetailColumnWidth = new string[arrDetailColumn.Length];
                        strDetailFix = "";

                        for (int i = 0; i < arrMainColumn.Length; i++)
                        {
                            arrDetailColumnCaption[i] = Dictionary_Data.SearchDic(arrDetailColumn[i].ToString(), bp.g_language);
                            //arrDetailColumnWidth[i] = "200";
                        }

                        arrDetailColumnWidth[0] = "40";
                        arrDetailColumnWidth[1] = "59";
                        arrDetailColumnWidth[2] = "114";
                        arrDetailColumnWidth[3] = "200";
                        arrDetailColumnWidth[4] = "200";
                        arrDetailColumnWidth[5] = "130";
                        arrDetailColumnWidth[6] = "200";
                        arrDetailColumnWidth[7] = "130";
                        arrDetailColumnWidth[8] = "200";
                        arrDetailColumnWidth[9] = "0";
                        arrDetailColumnWidth[10] = "0";
                        arrDetailColumnWidth[11] = "0";
                        arrDetailColumnWidth[12] = "0";
                        arrDetailColumnWidth[13] = "0";
                        arrDetailColumnWidth[14] = "0";
                        arrDetailColumnWidth[15] = "0";
                        arrDetailColumnWidth[16] = "0";
                        arrDetailColumnWidth[17] = "0";
                        arrDetailColumnWidth[18] = "0";
                        arrDetailColumnWidth[19] = "0";
                        arrDetailColumnWidth[20] = "0";
                        arrDetailColumnWidth[21] = "0";
                        arrDetailColumnWidth[22] = "0";
                        arrDetailColumnWidth[23] = "0";
                        arrDetailColumnWidth[24] = "0";
                        arrDetailColumnWidth[25] = "0";
                        arrDetailColumnWidth[26] = "0";
                        arrDetailColumnWidth[27] = "0";
                        arrDetailColumnWidth[28] = "0";
                        arrDetailColumnWidth[29] = "0";
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
            jsMainCol = ConvertJSONData.ConvertColArrToJSON(arrMainColumn, arrMainColumnCaption, arrMainColumnWidth, "cols");
            jsMainField = ConvertJSONData.ConvertColArrToJSON(arrMainColumn, arrMainColumnCaption, arrMainColumnWidth, "fields");

            jsDetailCol = ConvertJSONData.ConvertColArrToJSON(arrDetailColumn, arrDetailColumnCaption, arrDetailColumnWidth, "cols");
            jsDetailField = ConvertJSONData.ConvertColArrToJSON(arrDetailColumn, arrDetailColumnCaption, arrDetailColumnWidth, "fields");
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbYear.Text = Dictionary_Data.SearchDic("CORRECT_YEAR", bp.g_language);
            lbStatus.Text = Dictionary_Data.SearchDic("STATUS", bp.g_language);
            lbManDept.Text = Dictionary_Data.SearchDic("MAN_DEPT", bp.g_language);
            lbManNo.Text = Dictionary_Data.SearchDic("MAN_NO", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Qua51Data.Get_CurrectList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());
            sParam.Add("YEAR", ddlYear.SelectedValue);
            sParam.Add("MAN_DEPT", ddlManDept.SelectedValue);
            sParam.Add("STATUS", ddlStatus.SelectedValue);
            sParam.Add("MAN_NO", txtManNo.Text);

            sParam.Add("CUR_MENU_ID", "Qua51");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            FW.Data.Parameters logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_START");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Qua51");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            // 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_END");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Qua51");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            if (ds.Tables.Count > 0)
            {
                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strMessage = ds.Tables[0].Rows[0][1].ToString();

                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "createMainGrid();", true);

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_ErrorMessage('" + strMessage + "');", true);
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        jsMainData = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[0], strMainKeyColumn);

                        //정상처리되면
                        if (!String.IsNullOrEmpty(jsMainData))
                        {
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            string script = $@" data = {jsMainData}; 
                            createMainGrid('" + strMainFix + "'); ";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);
                        }
                        else
                        {
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            string script = $@" data = ''; 
                            createMainGrid(); ";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);

                            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData();", true);
                        }

                    }
                    else
                    {
                        // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                        string script = $@" data = ''; 
                            createMainGrid(); ";

                        //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);

                        ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData();", true);
                    }
                }
            }
            else
            {
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

        #region btnDetailSearch_Click
        protected void btnDetailSearch_Click(object sender, EventArgs e)
        {
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            string[] strSplitValue = cy.Decrypt(hidParams.Value).Split('/');

            strDBName = "GPDB";
            strQueryID = "Qua51Data.Get_CurrectDetailList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0]);
            sParam.Add("MAN_NO", strSplitValue[1]);

            sParam.Add("CUR_MENU_ID", "Qua51");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요


            DataSet ds = new DataSet();

            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

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
                        jsDetailData = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[0], strDetailKeyColumn);

                        //정상처리되면
                        if (!String.IsNullOrEmpty(jsDetailData))
                        {
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            string script = $@" data2 = {jsDetailData}; 
                            createDetailGrid('" + strDetailFix + "'); ";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);
                        }
                        else
                        {
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            string script = $@" data2 = ""; 
                            createDetailGrid(); ";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);

                            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData();", true);
                        }

                    }
                    else
                    {
                        // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                        string script = $@" data2 = ''; 
                            createDetailGrid(); ";

                        //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);

                        ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData();", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoData();", true);
            }
        }
        #endregion
    }
}