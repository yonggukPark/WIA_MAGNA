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

namespace HQCWeb.QualityMgt.Qua10
{
    public partial class Qua10 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        // 비지니스 클래스 작성
        Biz.QualityManagement.Qua10 biz = new Biz.QualityManagement.Qua10();

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
        public string[] strKeyColumn = new string[] { "COMPLETE_NO" };

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
                                field = {jsField}; 
                                cStn = {jsDdl1}; 
                                cCarType = {jsDdl2}; 
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

            DataSet ds = new DataSet();

            jsDdl1 = "[]";
            jsDdl2 = "[]";
            ddlShopCd.Items.Add(new ListItem("선택하세요", ""));
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));
            ddlRslt1.Items.Add(new ListItem("ALL", ""));
            //ddlRslt2.Items.Add(new ListItem("ALL", ""));
            ddlEopFlag.Items.Add(new ListItem("ALL", ""));

            strDBName = "GPDB";
            strQueryID = "Qua10Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", "");
            param.Add("LINE_CD", "");
            param.Add("STN_CD", "");
            param.Add("EOP_FLAG", "");

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
                    ddlLineCd.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                }
                //for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                //{
                //    ddlStnCd.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                //}
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    ddlRslt1.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                {
                    ddlWctCd.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                {
                    ddlEopFlag.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                }
            }

            ddlWctCd.SelectedValue = "H";
            txtFromDt.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtToDt.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            //txtFromDt.Enabled = false;
            //txtToDt.Enabled = false;
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
            sParam.Add("MENU_ID", "Qua10");
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("CUR_MENU_ID", "Qua10");

            ds = biz.GetUserMenuSettingInfoList(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strMessage = ds.Tables[0].Rows[0][1].ToString();

                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "data=null; createGrid();", true);

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
                        arrColumn = new string[] { "PLANT_CD", "COMPLETE_NO", "SERIAL_NO", "STN_CD", "STN_NM", "P_SET", "WORK_NM", "ANGLE_VALUE", "ANGLE_MIN", "ANGLE_MAX", "ANGLE_RESULT", "R_ANGLE","R_ANGLE_MIN", "R_ANGLE_MAX", "R_ANGLE_RESULT", "WORK_SEQ", "DEV_NM", "TORQUE_VALUE", "TORQUE_MIN", "TORQUE_MAX", "RESULT", "REASON", "INSP_DATE", "KEY_HID" };
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
            //realGrid 방식
            //그리드 컬럼 데이터를 JSON string으로 변환합니다.
            jsCol = ConvertJSONData.ConvertColArrToJSON(arrColumn, arrColumnCaption, arrColumnWidth, "cols");
            jsField = ConvertJSONData.ConvertColArrToJSON(arrColumn, arrColumnCaption, arrColumnWidth, "fields");
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbStnCd.Text = Dictionary_Data.SearchDic("STN_CD", bp.g_language);
            lbRslt1.Text = Dictionary_Data.SearchDic("RSLT1", bp.g_language);
            lbProdDt.Text = Dictionary_Data.SearchDic("PROD_DT", bp.g_language);
            lbBarcode.Text = Dictionary_Data.SearchDic("BARCODE", bp.g_language);
            lbSerialNo.Text = Dictionary_Data.SearchDic("SERIAL_NO", bp.g_language);
            //lbRslt2.Text = Dictionary_Data.SearchDic("RSLT2", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Qua10Data.Get_TorqueList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            //sParam.Add("PLANT_CD", "P1")
            sParam.Add("PLANT_CD", bp.g_plant.ToString());
            sParam.Add("SHOP_CD", ddlShopCd.SelectedValue);
            sParam.Add("LINE_CD", ddlLineCd.SelectedValue);
            sParam.Add("STN_CD", txtStnCdHidden.Text);
            sParam.Add("CAR_TYPE", txtCarTypeHidden.Text);
            sParam.Add("PROD_FR_DT", txtFromDt.Text.Replace("-",""));//날짜는 필수값(이력DB 접근문제)
            sParam.Add("PROD_TO_DT", txtToDt.Text.Replace("-", ""));//날짜는 필수값(이력DB 접근문제)
            sParam.Add("COMPLETE_NO", txtBarcode.Text);
            sParam.Add("SERIAL_NO", txtSerialNo.Text);
            sParam.Add("RESULT", ddlRslt1.SelectedValue);

            sParam.Add("CUR_MENU_ID", "Qua10");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            FW.Data.Parameters logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_START");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Qua10");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            // 비지니스 메서드 호출
            ds = biz.GetTorque(strDBName, strQueryID, sParam);

            logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_END");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Qua10");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "data=null; createGrid();", true);

                            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData();", true);
                        }

                    }
                    else
                    {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "data=null; createGrid();", true);

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

        #region ddlShopCd_SelectedIndexChanged
        protected void ddlShopCd_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();
            string script = string.Empty;
            string jsData1 = "[]";
            string jsData2 = "[]";

            //데이터 클리어
            ddlLineCd.Items.Clear();

            //비활성
            ddlLineCd.Enabled = false;

            //초기화
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Qua10Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            //param.Add("PLANT_CD", "P1");
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", "");
            param.Add("STN_CD", "");
            param.Add("EOP_FLAG", ddlEopFlag.SelectedValue);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Line code 있으면
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ddlLineCd.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlLineCd.Enabled = true;
                }
                //Stn Code 있으면
                if (ds.Tables[2].Rows.Count > 0)
                {
                    jsData1 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[2]);
                    //for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    //{
                    //    ddlStnCd.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    //}
                    //ddlStnCd.Enabled = true;
                }
                //Car Type 있으면
                if (ds.Tables[6].Rows.Count > 0)
                {
                    jsData2 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[6]);
                }

            }

            script = $@" cStn = {jsData1}; cCarType = {jsData2}; fn_Set_Stn(); fn_Set_CarType(); ";
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "comboData", script, true);
        }
        #endregion

        #region ddlLineCd_SelectedIndexChanged
        protected void ddlLineCd_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();
            string script = string.Empty;
            string jsData1 = "[]";
            string jsData2 = "[]";

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Qua10Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            //param.Add("PLANT_CD", "P1");
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", ddlLineCd.SelectedValue);
            param.Add("STN_CD", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Stn Code 있으면
                if (ds.Tables[2].Rows.Count > 0)
                {
                    jsData1 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[2]);
                    //for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    //{
                    //    ddlStnCd.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    //}
                    //ddlStnCd.Enabled = true;
                }
                //Car Type 있으면
                if (ds.Tables[6].Rows.Count > 0)
                {
                    jsData2 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[6]);
                }
            }

            script = $@" cStn = {jsData1}; cCarType = {jsData2}; fn_Set_Stn(); fn_Set_CarType(); ";
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "comboData", script, true);
        }
        #endregion
        
        #region btnFunctionCall_Click
        protected void btnFunctionCall_Click(object sender, EventArgs e)
        {
            txtStnCd_SelectedIndexChanged();
        }
        #endregion

        #region txtStnCd_SelectedIndexChanged
        private void txtStnCd_SelectedIndexChanged()
        {
            //데이터셋 설정
            DataSet ds = new DataSet();
            string script = string.Empty;
            string jsData1 = "[]";

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Qua10Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", ddlLineCd.SelectedValue);
            param.Add("STN_CD", txtStnCdHidden.Text);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Car Type 있으면
                if (ds.Tables[6].Rows.Count > 0)
                {
                    jsData1 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[6]);
                }
            }
            script = $@" cCarType = {jsData1}; fn_Set_CarType(); ";
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "comboData", script, true);
        }
        #endregion

        #region ddlEopFlag_SelectedIndexChanged
        protected void ddlEopFlag_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();
            string script = string.Empty;
            string jsData1 = "[]";
            string jsData2 = "[]";

            //데이터 클리어
            ddlLineCd.Items.Clear();

            //비활성
            ddlLineCd.Enabled = false;

            //초기화
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Qua10Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            //param.Add("PLANT_CD", "P1");
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", "");
            param.Add("STN_CD", "");
            param.Add("EOP_FLAG", ddlEopFlag.SelectedValue);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Line code 있으면
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ddlLineCd.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlLineCd.Enabled = true;
                }
                //Stn Code 있으면
                if (ds.Tables[2].Rows.Count > 0)
                {
                    jsData1 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[2]);
                    //for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    //{
                    //    ddlStnCd.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    //}
                    //ddlStnCd.Enabled = true;
                }
                //Car Type 있으면
                if (ds.Tables[6].Rows.Count > 0)
                {
                    jsData2 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[6]);
                }

            }

            script = $@" cStn = {jsData1}; cCarType = {jsData2}; fn_Set_Stn(); fn_Set_CarType(); ";
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "comboData", script, true);
        }
        #endregion
    }
}