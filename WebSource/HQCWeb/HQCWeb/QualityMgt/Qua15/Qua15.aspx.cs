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
using System.IO;
using ClosedXML.Excel;
using System.Web.Services;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text;

namespace HQCWeb.QualityMgt.Qua15
{
    public partial class Qua15 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        // 비지니스 클래스 작성
        Biz.QualityManagement.Qua15 biz = new Biz.QualityManagement.Qua15();

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
            //엑셀출력이므로 쓸일없음
            //SetPageInit();

            if (!IsPostBack)
            {
                SetCon();

                //엑셀출력이므로 쓸일없음
                //SetGridTitle();

                SetTitle();

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid' 함수 호출
                string script = $@" cStn = {jsDdl1}; 
                                cCarType = {jsDdl2}; ";

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
            ddlShopCd2.Items.Add(new ListItem("선택하세요", ""));
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));
            ddlLineCd2.Items.Add(new ListItem("선택하세요", ""));
            ddlRslt1.Items.Add(new ListItem("ALL", ""));
            ddlRslt2.Items.Add(new ListItem("ALL", ""));
            ddlEopFlag.Items.Add(new ListItem("ALL", ""));
            ddlEopFlag2.Items.Add(new ListItem("ALL", ""));

            strDBName = "GPDB";
            strQueryID = "Qua15Data.Get_DdlData";

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
                    ddlShopCd2.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    ddlLineCd.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                    ddlLineCd2.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                }
                //for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                //{
                //    ddlStnCd.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                //}
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    ddlRslt1.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                    ddlRslt2.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                {
                    ddlWctCd.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    ddlWctCd2.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                {
                    ddlEopFlag.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                    ddlEopFlag2.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                }
            }

            ddlWctCd.SelectedValue = "H";
            txtFromDt.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtToDt.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtFromDt2.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtToDt2.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtFromDt2.Enabled = false;
            txtToDt2.Enabled = false;
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
            sParam.Add("MENU_ID", "Qua15");
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("CUR_MENU_ID", "Qua15");

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
                        arrColumn = new string[] { "PLANT_CD", "COMPLETE_NO", "SERIAL_NO", "STN_CD", "STN_NM", "P_SET", "WORK_NM", "ANGLE_VALUE", "ANGLE_MIN", "ANGLE_MAX", "ANGLE_RESULT", "R_ANGLE", "R_ANGLE_MIN", "R_ANGLE_MAX", "R_ANGLE_RESULT", "WORK_SEQ", "DEV_NM", "TORQUE_VALUE", "TORQUE_MIN", "TORQUE_MAX", "RESULT", "REASON", "INSP_DATE", "KEY_HID" };
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
            lbShopCd2.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbLineCd2.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbStnCd.Text = Dictionary_Data.SearchDic("STN_CD", bp.g_language);
            lbStnCd2.Text = Dictionary_Data.SearchDic("STN_CD", bp.g_language);
            lbRslt1.Text = Dictionary_Data.SearchDic("RSLT1", bp.g_language);
            lbRslt2.Text = Dictionary_Data.SearchDic("RSLT1", bp.g_language);
            lbProdDt.Text = Dictionary_Data.SearchDic("PROD_DT", bp.g_language);
            lbProdDt2.Text = Dictionary_Data.SearchDic("PROD_DT", bp.g_language);
            //lbRslt2.Text = Dictionary_Data.SearchDic("RSLT2", bp.g_language);
        }
        #endregion

        #region GetData
        public DataTable GetData(int flag)
        {
            DataSet ds = new DataSet();
            DataTable dt = null;
            DataTable dtSum = null;
            string strTableName = string.Empty;
            string strMessage = string.Empty;
            string script = string.Empty;
            int iNo = 0;

            FW.Data.Parameters sParam = null;
            string fromDate = string.Empty; 
            string toDate = string.Empty;
            DateTime startDate = new DateTime();
            DateTime endDate = new DateTime();

            strDBName = "GPDB";

            FW.Data.Parameters logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnExcelNew");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_START");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Qua15");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            if (flag == 1)
            {
                fromDate = txtFromDt.Text.Replace("-", ""); // 시작 날짜
                toDate = txtToDt.Text.Replace("-", "");     // 종료 날짜
                startDate = DateTime.ParseExact(fromDate, "yyyyMMdd", null);
                endDate = DateTime.ParseExact(toDate, "yyyyMMdd", null);
                
                strQueryID = "Qua15Data.Get_TorqueList";

                // 날짜 범위 출력
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    sParam = new FW.Data.Parameters();
                    //sParam.Add("PLANT_CD", "P1")
                    sParam.Add("PLANT_CD", bp.g_plant.ToString());
                    sParam.Add("SHOP_CD", ddlShopCd.SelectedValue);
                    sParam.Add("LINE_CD", ddlLineCd.SelectedValue);
                    sParam.Add("STN_CD", txtStnCdHidden.Text);
                    sParam.Add("CAR_TYPE", txtCarTypeHidden.Text);
                    sParam.Add("PROD_DT", date.ToString("yyyyMMdd"));//날짜는 필수값(이력DB 접근문제)
                    sParam.Add("RESULT", ddlRslt1.SelectedValue);
                    sParam.Add("NO", iNo);

                    sParam.Add("CUR_MENU_ID", "Qua15");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                    // 비지니스 메서드 호출
                    ds = biz.GetTorque(strDBName, strQueryID, sParam);

                    //1회전이면
                    if(date.ToString("yyyyMMdd") == fromDate)
                    {
                        dt = ds.Tables[0];//Datatable 초기화
                    }
                    else
                    {
                        dtSum = ds.Tables[0];//Datatable 초기화

                        // 두 번째 DataTable의 모든 행을 첫 번째 DataTable에 추가
                        foreach (DataRow row in dtSum.Rows)
                        {
                            dt.ImportRow(row); // ImportRow는 원본 DataRow의 값을 복사하여 추가
                        }
                    }
                    //No 개수 초기화
                    iNo = dt.Rows.Count;

                    //엑셀 함수와 충돌 발생....
                    //script = $"<script>console.log('{date:yyyy-MM-dd} 진행중...');</script>";
                    //Response.Write(script);
                    //Response.Flush(); // 클라이언트로 즉시 전달

                    //// 서버 작업
                    //System.Threading.Thread.Sleep(1000); // 예제: 1초 대기 (서버 작업 대체)
                }
            }
            else if (flag == 2)
            {
                fromDate = txtFromDt2.Text.Replace("-", ""); // 시작 날짜
                toDate = txtToDt2.Text.Replace("-", "");     // 종료 날짜
                startDate = DateTime.ParseExact(fromDate, "yyyyMMdd", null);
                endDate = DateTime.ParseExact(toDate, "yyyyMMdd", null);
                
                strQueryID = "Qua15Data.Get_Inspection";
                // 날짜 범위 출력
                for (DateTime date = startDate; date <= endDate; date = date.AddDays(1))
                {
                    sParam = new FW.Data.Parameters();
                    sParam.Add("PLANT_CD", bp.g_plant.ToString());
                    sParam.Add("SHOP_CD", ddlShopCd2.SelectedValue);
                    sParam.Add("LINE_CD", ddlLineCd2.SelectedValue);
                    sParam.Add("STN_CD", ddlStnCd.SelectedValue);
                    sParam.Add("DEV_ID", ddlDevCd.SelectedValue);
                    sParam.Add("CAR_TYPE", ddlCarType.SelectedValue);
                    sParam.Add("FR_DT", date.ToString("yyyyMMdd"));
                    sParam.Add("RESULT", ddlRslt2.SelectedValue);
                    sParam.Add("DIV_FLAG", "H");

                    sParam.Add("CUR_MENU_ID", "Qua15");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                    // 비지니스 메서드 호출
                    ds = biz.GetTorque(strDBName, strQueryID, sParam);

                    //1회전이면
                    if (date.ToString("yyyyMMdd") == fromDate)
                    {
                        dt = ds.Tables[0];//Datatable 초기화
                    }
                    else
                    {
                        dtSum = ds.Tables[0];//Datatable 초기화

                        // 두 번째 DataTable의 모든 행을 첫 번째 DataTable에 추가
                        foreach (DataRow row in dtSum.Rows)
                        {
                            dt.ImportRow(row); // ImportRow는 원본 DataRow의 값을 복사하여 추가
                        }
                    }

                    //엑셀 함수와 충돌 발생....
                    //script = $"<script>console.log('{date:yyyy-MM-dd} 진행중...');</script>";
                    //Response.Write(script);
                    //Response.Flush(); // 클라이언트로 즉시 전달

                    //// 서버 작업
                    //System.Threading.Thread.Sleep(1000); // 예제: 1초 대기 (서버 작업 대체)
                }
            }

            logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnExcelNew");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_END");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Qua15");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            return dt;
        }
        #endregion

        #region btnExcelNew_Click
        protected void btnExcelNew_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            dt = GetData(1);

            if (dt.Rows.Count > 0)
            {
                makeCSV(dt);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData(); fn_loadingEnd();", true);
            }
        }
        #endregion

        #region btnExcelNew2_Click
        protected void btnExcelNew2_Click(object sender, EventArgs e)
        {
            DataTable dt = null;
            dt = GetData(2);

            if (dt.Rows.Count > 0)
            {
                makeCSV(dt);
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData(); fn_loadingEnd();", true);
            }
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

        #region ddlWct2_SelectedIndexChanged
        protected void ddlWct2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] date = WCTService.WCTDatepickerData(ddlWctCd2.SelectedValue);
            if (String.IsNullOrEmpty(date[0]))
            {
                txtFromDt2.Enabled = true;
                txtToDt2.Enabled = true;
                //datepicker 재활성화
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "refreshDate();", true);
            }
            else
            {
                txtFromDt2.Text = date[0];
                txtToDt2.Text = date[1];
                txtFromDt2.Enabled = false;
                txtToDt2.Enabled = false;
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
            strQueryID = "Qua15Data.Get_DdlData";

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

        #region ddlShopCd2_SelectedIndexChanged
        protected void ddlShopCd2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlLineCd2.Items.Clear();
            ddlStnCd.Items.Clear();
            ddlDevCd.Items.Clear();
            ddlCarType.Items.Clear();

            //비활성
            ddlLineCd2.Enabled = false;
            ddlStnCd.Enabled = false;
            ddlDevCd.Enabled = false;
            ddlCarType.Enabled = false;

            //초기화
            ddlLineCd2.Items.Add(new ListItem("선택하세요", ""));
            ddlStnCd.Items.Add(new ListItem("선택하세요", ""));
            ddlDevCd.Items.Add(new ListItem("선택하세요", ""));
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Qua15Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            //param.Add("PLANT_CD", "P1");
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd2.SelectedValue);
            param.Add("LINE_CD", "");
            param.Add("STN_CD", "");
            param.Add("EOP_FLAG", ddlEopFlag2.SelectedValue);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Line code 있으면
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ddlLineCd2.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlLineCd2.Enabled = true;
                }
                //Stn Code 있으면
                if (ds.Tables[8].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                    {
                        ddlStnCd.Items.Add(new ListItem(ds.Tables[8].Rows[i]["CODE_NM"].ToString(), ds.Tables[8].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlStnCd.Enabled = true;
                }
                //Device Code 있으면
                if (ds.Tables[9].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[9].Rows.Count; i++)
                    {
                        ddlDevCd.Items.Add(new ListItem(ds.Tables[9].Rows[i]["CODE_NM"].ToString(), ds.Tables[9].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevCd.Enabled = true;
                }
                //Car Type 있으면
                if (ds.Tables[10].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[10].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[10].Rows[i]["CODE_NM"].ToString(), ds.Tables[10].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarType.Enabled = true;
                }
            }
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
            strQueryID = "Qua15Data.Get_DdlData";

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

        #region ddlLineCd2_SelectedIndexChanged
        protected void ddlLineCd2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlStnCd.Items.Clear();
            ddlDevCd.Items.Clear();
            ddlCarType.Items.Clear();

            //비활성
            ddlStnCd.Enabled = false;
            ddlDevCd.Enabled = false;
            ddlCarType.Enabled = false;

            //초기화
            ddlStnCd.Items.Add(new ListItem("선택하세요", ""));
            ddlDevCd.Items.Add(new ListItem("선택하세요", ""));
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Qua15Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            //param.Add("PLANT_CD", "P1");
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd2.SelectedValue);
            param.Add("LINE_CD", ddlLineCd2.SelectedValue);
            param.Add("STN_CD", "");
            param.Add("EOP_FLAG", ddlEopFlag2.SelectedValue);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Stn Code 있으면
                if (ds.Tables[8].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                    {
                        ddlStnCd.Items.Add(new ListItem(ds.Tables[8].Rows[i]["CODE_NM"].ToString(), ds.Tables[8].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlStnCd.Enabled = true;
                }
                //Device Code 있으면
                if (ds.Tables[9].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[9].Rows.Count; i++)
                    {
                        ddlDevCd.Items.Add(new ListItem(ds.Tables[9].Rows[i]["CODE_NM"].ToString(), ds.Tables[9].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevCd.Enabled = true;
                }
                //Car Type 있으면
                if (ds.Tables[10].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[10].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[10].Rows[i]["CODE_NM"].ToString(), ds.Tables[10].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarType.Enabled = true;
                }
            }
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
            strQueryID = "Qua15Data.Get_DdlData";

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

        #region ddlStnCd_SelectedIndexChanged
        protected void ddlStnCd_SelectedIndexChanged(object sender, EventArgs e)
        {

            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlDevCd.Items.Clear();
            ddlCarType.Items.Clear();

            //비활성
            ddlDevCd.Enabled = false;
            ddlCarType.Enabled = false;

            //초기화
            ddlDevCd.Items.Add(new ListItem("선택하세요", ""));
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Qua15Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            //param.Add("PLANT_CD", "P1");
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd2.SelectedValue);
            param.Add("LINE_CD", ddlLineCd2.SelectedValue);
            param.Add("STN_CD", ddlStnCd.SelectedValue);
            param.Add("EOP_FLAG", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Device Code 있으면
                if (ds.Tables[9].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[9].Rows.Count; i++)
                    {
                        ddlDevCd.Items.Add(new ListItem(ds.Tables[9].Rows[i]["CODE_NM"].ToString(), ds.Tables[9].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevCd.Enabled = true;
                }
                //Car Type 있으면
                if (ds.Tables[10].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[10].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[10].Rows[i]["CODE_NM"].ToString(), ds.Tables[10].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarType.Enabled = true;
                }
            }
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
            strQueryID = "Qua15Data.Get_DdlData";

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

        #region ddlEopFlag_SelectedIndexChanged
        protected void ddlEopFlag2_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlLineCd2.Items.Clear();
            ddlStnCd.Items.Clear();
            ddlDevCd.Items.Clear();
            ddlCarType.Items.Clear();

            //비활성
            ddlLineCd2.Enabled = false;
            ddlStnCd.Enabled = false;
            ddlDevCd.Enabled = false;
            ddlCarType.Enabled = false;

            //초기화
            ddlLineCd2.Items.Add(new ListItem("선택하세요", ""));
            ddlStnCd.Items.Add(new ListItem("선택하세요", ""));
            ddlDevCd.Items.Add(new ListItem("선택하세요", ""));
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Qua15Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            //param.Add("PLANT_CD", "P1");
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd2.SelectedValue);
            param.Add("LINE_CD", "");
            param.Add("STN_CD", "");
            param.Add("EOP_FLAG", ddlEopFlag2.SelectedValue);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Line code 있으면
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ddlLineCd2.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlLineCd2.Enabled = true;
                }
                //Stn Code 있으면
                if (ds.Tables[8].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                    {
                        ddlStnCd.Items.Add(new ListItem(ds.Tables[8].Rows[i]["CODE_NM"].ToString(), ds.Tables[8].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlStnCd.Enabled = true;
                }
                //Device Code 있으면
                if (ds.Tables[9].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[9].Rows.Count; i++)
                    {
                        ddlDevCd.Items.Add(new ListItem(ds.Tables[9].Rows[i]["CODE_NM"].ToString(), ds.Tables[9].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevCd.Enabled = true;
                }
                //Car Type 있으면
                if (ds.Tables[10].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[10].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[10].Rows[i]["CODE_NM"].ToString(), ds.Tables[10].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarType.Enabled = true;
                }
            }
        }
        #endregion

        #region makeCSV
        public void makeCSV(DataTable dt)
        {
            try
            {
                Session["Qua15DownExcel"] = "S";
                const int MaxRowsPerFile = 900000; // 한 파일당 최대 행 수
                int fileIndex = 1; // 파일 번호

                // 전체 데이터 테이블을 90만 건씩 분할
                for (int startRow = 0; startRow < dt.Rows.Count; startRow += MaxRowsPerFile)
                {
                    DataTable partialTable = dt.AsEnumerable()
                                               .Skip(startRow)
                                               .Take(MaxRowsPerFile)
                                               .CopyToDataTable();

                    // 메모리 스트림을 사용하여 CSV 데이터 생성
                    using (var stream = new MemoryStream())
                    using (var writer = new StreamWriter(stream, new UTF8Encoding(true))) // UTF-8 BOM 추가
                    using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)
                    {
                        HasHeaderRecord = true, // 헤더 포함
                    }))
                    {
                        // 컬럼 헤더 작성
                        foreach (DataColumn column in partialTable.Columns)
                        {
                            string columnName = Dictionary_Data.SearchDic(column.ColumnName, bp.g_language);
                            csv.WriteField(columnName);
                        }
                        csv.NextRecord(); // 헤더 작성 후 줄 바꿈

                        // 데이터 작성
                        foreach (DataRow row in partialTable.Rows)
                        {
                            foreach (var item in row.ItemArray)
                            {
                                csv.WriteField(item?.ToString() ?? string.Empty);
                            }
                            csv.NextRecord(); // 데이터 작성 후 줄 바꿈
                        }

                        // CSV 작성 완료 후 스트림에 기록
                        writer.Flush();
                        stream.Position = 0; // 스트림 위치 초기화

                        // 파일 이름에 번호 추가
                        string fileName = $"Qua15_Part{fileIndex}.csv";

                        // 클라이언트에 파일 다운로드로 응답
                        Response.Clear();
                        Response.ContentType = "text/csv"; // CSV MIME 타입
                        Response.AddHeader("content-disposition", $"attachment;filename={fileName}");
                        Response.AddHeader("X-Download-Status", "success");
                        Response.BinaryWrite(stream.ToArray());
                        Response.Flush();

                        fileIndex++; // 다음 파일 번호 증가
                    }
                }

                Session["Qua15DownExcel"] = "C"; // 작업 완료 상태 설정
                Response.End();
            }
            catch (Exception ex)
            {
                string errorMessage = $"Error in CSV : {ex.Message}\nStack Trace: {ex.StackTrace}";
                LogMessage(errorMessage);

                Response.Clear();
                Response.ContentType = "text/plain";
                Response.AddHeader("X-Download-Status", "error");
                Response.Write("Error: " + ex.Message);
                Session["Qua15DownCsv"] = "C";
                Response.End();
            }
        }
        #endregion

        #region LogMessage
        public void LogMessage(string message)
        {
            // 로그 파일 경로 설정
            string logPath = Server.MapPath("~/Log/log_"+ System.DateTime.Now.ToString("yyyy-MM-dd") + ".txt");

            // 로그 메시지에 시간 정보 추가 (선택 사항)
            string logMessage = $"{DateTime.Now}: {message}";

            // 로그 파일에 메시지를 개행 문자로 구분하여 추가
            System.IO.File.AppendAllText(logPath, logMessage + Environment.NewLine);
        }
        #endregion

        #region GetStatus
        [WebMethod]
        public static string GetStatus()
        {
            var status = HttpContext.Current.Session["Qua15DownExcel"] as string;

            // "C" 상태를 리턴하기 직전에 상태를 "S"로 변경
            if (status == "C")
            {
                HttpContext.Current.Session["Qua15DownExcel"] = "S";
            }

            return status ?? "N";

        }
        #endregion
    }
}