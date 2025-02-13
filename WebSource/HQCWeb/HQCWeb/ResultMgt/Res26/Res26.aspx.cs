using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using MES.FW.Common.Crypt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HQCWeb.ResultMgt.Res26
{
    public partial class Res26 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        // 비지니스 클래스 작성
        Biz.ResultManagement.Res26 biz = new Biz.ResultManagement.Res26();

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
        public string[] strKeyColumn = new string[] { "PLANT_CD", "P_PART_NO" };
        //JSON 전달용 변수
        string jsField = string.Empty;
        string jsCol = string.Empty;
        string jsData = string.Empty;
        string jsData2 = string.Empty;
        string jsData3 = string.Empty;

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

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid' 함수 호출
                string script = $@" column = {jsCol}; 
                                field = {jsField}; 
                                cPart = {jsData2};
                                createGrid('" + strFix + "'); ";

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            DataSet ds = new DataSet();

            strDBName = "GPDB";
            strQueryID = "Res26Data.Get_DdlData";
            jsData2 = "[]";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    jsData2 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[0]);
                }
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    ddlYear.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    ddlMonth.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                }
            }
            ddlYear.SelectedValue = System.DateTime.Now.ToString("yyyy");
            ddlMonth.SelectedValue = System.DateTime.Now.ToString("MM");
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
            sParam.Add("MENU_ID", "Res26");
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("CUR_MENU_ID", "Res26");

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
                        arrColumn = new string[] {"PLANT_CD", "PART_NO", "DIVISION", "PLAN_SUM", "PLAN_DAY01", "PLAN_DAY02", "PLAN_DAY03", "PLAN_DAY04", "PLAN_DAY05", "PLAN_DAY06", "PLAN_DAY07", "PLAN_DAY08", "PLAN_DAY09", "PLAN_DAY10", "PLAN_DAY11", "PLAN_DAY12", "PLAN_DAY13", "PLAN_DAY14", "PLAN_DAY15", "PLAN_DAY16", "PLAN_DAY17", "PLAN_DAY18", "PLAN_DAY19", "PLAN_DAY20", "PLAN_DAY21", "PLAN_DAY22", "PLAN_DAY23", "PLAN_DAY24", "PLAN_DAY25", "PLAN_DAY26", "PLAN_DAY27", "PLAN_DAY28", "PLAN_DAY29", "PLAN_DAY30", "PLAN_DAY31", "PROD_DT", "KEY_HID" };
                        arrColumnCaption = new string[arrColumn.Length];
                        arrColumnWidth = new string[arrColumn.Length];
                        strFix = "";

                        for (int i = 0; i < arrColumn.Length; i++)
                        {
                            arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);
                            //arrColumnWidth[i] = "200";
                        }

                        arrColumnWidth[0] = "50";
                        arrColumnWidth[1] = "425";
                        arrColumnWidth[2] = "79";
                        arrColumnWidth[3] = "70";
                        arrColumnWidth[4] = "70";
                        arrColumnWidth[5] = "70";
                        arrColumnWidth[6] = "70";
                        arrColumnWidth[7] = "70";
                        arrColumnWidth[8] = "70";
                        arrColumnWidth[9] = "70";
                        arrColumnWidth[10] = "70";
                        arrColumnWidth[11] = "70";
                        arrColumnWidth[12] = "70";
                        arrColumnWidth[13] = "70";
                        arrColumnWidth[14] = "70";
                        arrColumnWidth[15] = "70";
                        arrColumnWidth[16] = "70";
                        arrColumnWidth[17] = "70";
                        arrColumnWidth[18] = "70";
                        arrColumnWidth[19] = "70";
                        arrColumnWidth[20] = "70";
                        arrColumnWidth[21] = "70";
                        arrColumnWidth[22] = "70";
                        arrColumnWidth[23] = "70";
                        arrColumnWidth[24] = "70";
                        arrColumnWidth[25] = "70";
                        arrColumnWidth[26] = "70";
                        arrColumnWidth[27] = "70";
                        arrColumnWidth[28] = "70";
                        arrColumnWidth[29] = "70";
                        arrColumnWidth[30] = "70";
                        arrColumnWidth[31] = "70";
                        arrColumnWidth[32] = "70";
                        arrColumnWidth[33] = "70";
                        arrColumnWidth[34] = "70";
                        arrColumnWidth[35] = "0";
                        arrColumnWidth[36] = "0";
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
            //realGrid 방식
            //그리드 컬럼 데이터를 JSON string으로 변환합니다.
            jsCol = ConvertJSONData.ConvertColArrToJSON(arrColumn, arrColumnCaption, arrColumnWidth, "cols");
            jsField = ConvertJSONData.ConvertColArrToJSON(arrColumn, arrColumnCaption, arrColumnWidth, "fields");
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbProdDt.Text = Dictionary_Data.SearchDic("PROD_MONTH", bp.g_language);
            lbPartNo.Text = Dictionary_Data.SearchDic("PART_NO", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Res26Data.Get_ProdList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());
            sParam.Add("PROD_DT", ddlYear.SelectedValue.ToString() + ddlMonth.SelectedValue.ToString());
            sParam.Add("PART_NO", txtPartNoHidden.Text);

            sParam.Add("CUR_MENU_ID", "Res26");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            FW.Data.Parameters logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_START");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Res26");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            // 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_END");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Res26");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

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
                        jsData = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[0], strKeyColumn);
                        jsData2 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[2]);
                        jsData3 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[3]);

                        //정상처리되면
                        if (!String.IsNullOrEmpty(jsData))
                        {
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            string script = $@" data = {jsData}; chartData1 = {jsData2}; chartData2 = {jsData3};
                            resetChart(); 
                            createGrid('" + strFix + "'); ";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);
                        }
                        else
                        {
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            string script = $@" data = '';  chartData1 = {jsData2}; chartData2 = {jsData3};
                            resetChart();
                            createGrid(); ";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);

                            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData();", true);
                        }

                    }
                    else
                    {
                        // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                        string script = $@" data = '';  chartData1 = ''; chartData2 = '';
                            resetChart();
                            createGrid(); ";

                        //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);

                        ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData();", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Faile", "fn_NoData();", true);

                //grid.SetDataListReset();
            }
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }
        #endregion

        #region GetPkCode
        [WebMethod]
        public static string GetPkCode(string sParams, string sParams2)
        {
            Crypt cy = new Crypt();

            cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

            string decrpted = cy.Decrypt(sParams);

            string strValue = cy.Encrypt(decrpted + '/' + sParams2);

            return strValue;
        }
        #endregion
    }
}