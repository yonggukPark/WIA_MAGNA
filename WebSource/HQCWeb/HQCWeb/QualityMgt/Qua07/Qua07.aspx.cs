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

using MES.FW.Common.Crypt;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HQCWeb.QualityMgt.Qua07
{
    public partial class Qua07 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();
        Crypt cy = new Crypt();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        // 비지니스 클래스 작성
        Biz.QualityManagement.Qua07 biz = new Biz.QualityManagement.Qua07();

        #region GRID Setting
        // 그리드에 보여져야할 컬럼 정의
        public string[] arrMainColumn;
        public string[] arrDetailColumn;
        // 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrMainColumnCaption;
        public string[] arrDetailColumnCaption;
        // 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrMainColumnWidth;
        public string[] arrDetailColumnWidth;
        // 그리드 고정값 정의
        public string strMainFix;
        public string strDetailFix;
        // 그리드 키값 정의
        public string[] strKeyColumn = new string[] { "ORDER_NO" };

        // 상세 그리드 키값 정의
        public string[] strDetailKeyColumn = new string[] { "" };

        //JSON 전달용 변수
        string jsMainField = string.Empty;
        string jsMainCol = string.Empty;
        string jsMainData = string.Empty;

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

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createMainGrid' 함수 호출
                string scriptMain = $@" column = {jsMainCol}; 
                                field = {jsMainField}; 
                                createGrid('" + strMainFix + "'); ";


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

            strDBName = "GPDB";
            strQueryID = "Qua07Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                ddlWctCd.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
            }

            for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
            {
                ddlLineCd.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
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
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

            strDBName = "GPDB";
            strQueryID = "UserInfoData.Get_UserMenuSettingInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("MENU_ID", "Qua07");
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("CUR_MENU_ID", "Qua07");

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

                        arrMainColumn = new string[iRowCnt];
                        arrMainColumnCaption = new string[iRowCnt];
                        arrMainColumnWidth = new string[iRowCnt];

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
                        arrMainColumn = new string[] { "ORDER_NO", "ORDER_DATE", "SHIFT", "LOT_NO", "NUM_QTY", "CAR_TYPE", "SEQ_NO", "KEY_HID" };
                        arrMainColumnCaption = new string[arrMainColumn.Length];
                        arrMainColumnWidth = new string[arrMainColumn.Length];
                        strMainFix = "";

                        for (int i = 0; i < arrMainColumn.Length; i++)
                        {
                            arrMainColumnCaption[i] = Dictionary_Data.SearchDic(arrMainColumn[i].ToString(), bp.g_language);
                            arrMainColumnWidth[i] = "200";
                        }

                        arrDetailColumn = new string[] { "STATION", "WORK_NM", "CONTENTS", "DATA", "UNIT", "KEY_HID" };
                        arrDetailColumnCaption = new string[arrDetailColumn.Length];
                        arrDetailColumnWidth = new string[arrDetailColumn.Length];
                        strMainFix = "";

                        for (int i = 0; i < arrDetailColumn.Length; i++)
                        {
                            arrDetailColumnCaption[i] = Dictionary_Data.SearchDic(arrDetailColumn[i].ToString(), bp.g_language);
                            arrDetailColumnWidth[i] = "200";
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
            //realGrid 방식
            //그리드 컬럼 데이터를 JSON string으로 변환합니다.
            jsMainCol = ConvertJSONData.ConvertColArrToJSON(arrMainColumn, arrMainColumnCaption, arrMainColumnWidth, "cols");
            jsMainField = ConvertJSONData.ConvertColArrToJSON(arrMainColumn, arrMainColumnCaption, arrMainColumnWidth, "fields");

            jsDetailCol = ConvertJSONData.ConvertColArrToJSON(arrDetailColumn, arrDetailColumnCaption, arrDetailColumnWidth, "cols");
            jsDetailField = ConvertJSONData.ConvertColArrToJSON(arrDetailColumn, arrDetailColumnCaption, arrDetailColumnWidth, "fields");
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbCond_01.Text = Dictionary_Data.SearchDic("제품번호", bp.g_language);
            lbLotNo.Text = Dictionary_Data.SearchDic("로트번호", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("라인", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData(string flag)
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Qua07Data.Get_QualityList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("ToDate", txtToDt.Text.Replace("-", ""));
            sParam.Add("FromDate", txtFromDt.Text.Replace("-", ""));
            sParam.Add("FromLot", txtFromLot.Text);
            sParam.Add("ToLot", textToLot.Text);
            sParam.Add("Line", ddlLineCd.SelectedValue);

            sParam.Add("FLAG", flag);

            sParam.Add("CUR_MENU_ID", "Qua07");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

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
                        jsMainData = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[0], strKeyColumn);

                        //정상처리되면
                        if (!String.IsNullOrEmpty(jsMainData))
                        {
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            string script = $@" data = {jsMainData}; 
                            createGrid('" + strMainFix + "'); ";

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
            GetData("N");
        }
        #endregion

        #region btnRestore_Click
        protected void btnRestore_Click(object sender, EventArgs e)
        {
            GetData("Y");
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

        #region btnDetailSearch_Click
        protected void btnDetailSearch_Click(object sender, EventArgs e)
        {
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            string[] strSplitValue = cy.Decrypt(hidParams.Value).Split('/');

            strDBName = "GPDB";
            strQueryID = "Qua07Data.Get_CurrectDetailList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0]);
            sParam.Add("MAN_NO", strSplitValue[1]);

            sParam.Add("CUR_MENU_ID", "Lms11");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요


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