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

using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Web.Services;

namespace HQCWeb.ResultMgt.Res28
{
    public partial class Res28 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        // 비지니스 클래스 작성
        Biz.ResultManagement.Res28 biz = new Biz.ResultManagement.Res28();

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
        public string[] strKeyColumn = new string[] { "COL_1" };

        //JSON 전달용 변수
        string jsField = string.Empty;
        string jsCol = string.Empty;
        string jsData = string.Empty;

        string jsDdl1 = string.Empty;
        string jsDdl2 = string.Empty;
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
                                cShop = {jsDdl1}; 
                                cPart = {jsDdl2}; 
                                field = {jsField}; 
                                createGrid('" + strFix + "'); ";

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            strErrMessage = Message_Data.SearchDic("SearchError", bp.g_language);

            //데이터셋 설정
            string script = string.Empty;
            jsDdl1 = "[]";
            jsDdl2 = "[]";
            DataSet ds = new DataSet();

            //초기화
            ddlStatus.Items.Add(new ListItem("ALL", ""));
            ddlPlanType.Items.Add(new ListItem("ALL", ""));
            ddlStorageCd.Items.Add(new ListItem("ALL", ""));
            ddlSubStorageCd.Items.Add(new ListItem("ALL", ""));
            ddlDiv.Items.Add(new ListItem("사용안함", ""));
            ddlDriverCd.Items.Add(new ListItem("ALL", ""));
            ddlCarCd.Items.Add(new ListItem("ALL", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Res28Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", "");
            param.Add("DRIVER_CD", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                jsDdl1 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[0]);
                jsDdl2 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[1]);

                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    ddlDiv.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                }

                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    ddlStatus.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                }

                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                {
                    ddlPlanType.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                }

                for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                {
                    ddlStorageCd.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                }

                for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                {
                    ddlSubStorageCd.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                }

                for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                {
                    ddlDriverCd.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                }

                for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                {
                    ddlCarCd.Items.Add(new ListItem(ds.Tables[8].Rows[i]["CODE_NM"].ToString(), ds.Tables[8].Rows[i]["CODE_ID"].ToString()));
                }

                for (int i = 0; i < ds.Tables[9].Rows.Count; i++)
                {
                    ddlWctCd.Items.Add(new ListItem(ds.Tables[9].Rows[i]["CODE_NM"].ToString(), ds.Tables[9].Rows[i]["CODE_ID"].ToString()));
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
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

            strDBName = "GPDB";
            strQueryID = "UserInfoData.Get_UserMenuSettingInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("MENU_ID", "Res28");
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("CUR_MENU_ID", "Res28");

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
                        arrColumn = new string[] { "NO_DATA", "KEY_HID" };
                        arrColumnCaption = new string[arrColumn.Length];
                        arrColumnWidth = new string[arrColumn.Length];
                        strFix = "";

                        for (int i = 0; i < arrColumn.Length; i++)
                        {
                            arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);
                            arrColumnWidth[i] = "200";
                        }

                        //arrColumn = new string[] { "PLANT_CD", "PLAN_DATE", "PLAN_ORDER_NO", "TRANS_FLG", "SHIP_NO", "CAR_NM_KOR", "PLAN_QTY", "DELIVERY_QTY", "PART_NO", "PART_DESC", "S_STORAGE_NM", "D_STORAGE_NM", "CUSTOMER_NM", "DRIVER_NM", "TRANS_INFO", "EVENT_FLG_NM", "ORDER_NO", "STATUS_FLG", "EVENT_FLG", "KEY_HID" };
                        //arrColumnCaption = new string[arrColumn.Length];
                        //arrColumnWidth = new string[arrColumn.Length];
                        //strFix = "8";

                        //for (int i = 0; i < arrColumn.Length; i++)
                        //{
                        //    arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);
                        //    if (arrColumn[i].Equals("SHIP_NO") || arrColumn[i].Equals("PLAN_ORDER_NO") || arrColumn[i].Equals("DRIVER_NM") || arrColumn[i].Equals("TRANS_INFO")) arrColumnWidth[i] = "130";
                        //    else if (arrColumn[i].Equals("PLAN_QTY") || arrColumn[i].Equals("DELIVERY_QTY") || arrColumn[i].Equals("EVENT_FLG_NM")) arrColumnWidth[i] = "70";
                        //    else if (arrColumn[i].Equals("CAR_NM_KOR") || arrColumn[i].Equals("S_STORAGE_NM") || arrColumn[i].Equals("D_STORAGE_NM") || arrColumn[i].Equals("CUSTOMER_NM") || arrColumn[i].Equals("PART_DESC")) arrColumnWidth[i] = "250";
                        //    else arrColumnWidth[i] = "100";
                        //}
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
            lbPlanDt.Text = Dictionary_Data.SearchDic("PLAN_DT", bp.g_language);
            lbGubun.Text = Dictionary_Data.SearchDic("DIVISION", bp.g_language);
            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbPartNo.Text = Dictionary_Data.SearchDic("PART_NO", bp.g_language);
            lbCarCd.Text = Dictionary_Data.SearchDic("TRANS_INFO", bp.g_language);
            lbDriverCd.Text = Dictionary_Data.SearchDic("DRIVER_NM", bp.g_language);
            lbStatus.Text = Dictionary_Data.SearchDic("SHIP_STATUS", bp.g_language);
            lbPlanType.Text = Dictionary_Data.SearchDic("PLAN_TYPE", bp.g_language);
            lbSubStorageCd.Text = Dictionary_Data.SearchDic("SUB_STORAGE_CD", bp.g_language);
            lbStorageCd.Text = Dictionary_Data.SearchDic("D_STORAGE_CD", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Res28Data.Get_ProdList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());
            sParam.Add("FR_DT", txtFromDt.Text.Replace("-", ""));
            sParam.Add("TO_DT", txtToDt.Text.Replace("-", ""));
            sParam.Add("SHOP_CD", txtShopCdHidden.Text);
            sParam.Add("PART_NO", txtPartNoHidden.Text);
            sParam.Add("STATUS_CD", ddlStatus.SelectedValue);
            sParam.Add("PLAN_TYPE", ddlPlanType.SelectedValue);
            sParam.Add("SUB_STORAGE_CD", ddlSubStorageCd.SelectedValue);
            sParam.Add("STORAGE_CD", ddlStorageCd.SelectedValue);
            sParam.Add("CAR_CD", ddlCarCd.SelectedValue);
            sParam.Add("DRIVER_CD", ddlDriverCd.SelectedValue);
            sParam.Add("DIV", ddlDiv.SelectedValue);
            sParam.Add("ID_NO", txtIdNo.Text);

            sParam.Add("CUR_MENU_ID", "Res28");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            FW.Data.Parameters logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_START");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Res28");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            // 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_END");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Res28");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            if (ds.Tables.Count > 0)
            {
                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strErrMessage = ds.Tables[0].Rows[0][1].ToString();

                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "data=null; createGrid();", true);

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_ErrorMessage('" + strErrMessage + "');", true);
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        jsData = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[0], strKeyColumn);

                        string item = string.Empty;
                        string itemNm = string.Empty;
                        string itemWidth = string.Empty;

                        //컬럼 추출(동적 쿼리문에 대응 : 컬럼명이 정해지지 않아 검색시 동적으로 그리드 생성하여 처리)
                        string[] columnNames = ds.Tables[0].Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();

                        DataRow row = ds.Tables[1].Rows[0]; // 길이 테이블의 첫 번째 행
                        object[] rowWidthData = row.ItemArray; // 해당 행의 데이터 배열

                        // 추가할 항목 생성
                        List<string> newItems = new List<string>();
                        List<string> newItemCaption = new List<string>();
                        List<string> newItemWidth = new List<string>();

                        // 받은 컬럼 순회하여 item 추가
                        for (int i = 0; i < columnNames.Length; i++)
                        {
                            item = columnNames[i];
                            itemNm = columnNames[i];
                            itemWidth = rowWidthData[i].ToString();
                            newItems.Add(item);
                            newItemCaption.Add(itemNm);
                            newItemWidth.Add(itemWidth);
                        }

                        //기존 필드에 업데이트
                        arrColumn = newItems.ToArray();
                        arrColumnCaption = newItemCaption.ToArray();
                        arrColumnWidth = newItemWidth.ToArray();

                        //컬럼설정, 필드설정
                        jsCol = ConvertJSONData.ConvertColArrToJSON(arrColumn, arrColumnCaption, arrColumnWidth, "cols");
                        jsField = ConvertJSONData.ConvertColArrToJSON(arrColumn, arrColumnCaption, arrColumnWidth, "fields");

                        //정상처리되면
                        if (!String.IsNullOrEmpty(jsData))
                        {
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            string script = $@" column = {jsCol}; 
                                    field = {jsField}; 
                                    data = {jsData};
                            createGrid('" + strFix + "'); ";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);
                        }
                        else
                        {
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            string script = $@" column = {jsCol}; 
                                    field = {jsField};
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
            strQueryID = "Res28Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", txtShopCdHidden.Text);
            param.Add("DRIVER_CD", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Part No 있으면
                if (ds.Tables[1].Rows.Count > 0)
                {
                    jsData1 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[1]);
                }
            }
            script = $@" cPart = {jsData1}; fn_Set_Part(); ";
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "comboData", script, true);
        }
        #endregion

        #region ddlDriverCd_SelectedIndexChanged
        protected void ddlDriverCd_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Res28Data.Get_DdlData";

            ddlCarCd.Items.Clear();
            ddlCarCd.Items.Add(new ListItem("ALL", ""));
            ddlCarCd.Enabled = false;

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", "");
            param.Add("DRIVER_CD", ddlDriverCd.SelectedValue);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Part No 있으면
                if (ds.Tables[8].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                    {
                        ddlCarCd.Items.Add(new ListItem(ds.Tables[8].Rows[i]["CODE_NM"].ToString(), ds.Tables[8].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarCd.Enabled = true;
                }
            }
        }
        #endregion
    }
}