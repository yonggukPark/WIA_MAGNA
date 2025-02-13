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

namespace HQCWeb.PlanMgt.Pln05
{
    public partial class Pln05 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        // 비지니스 클래스 작성
        Biz.PlanManagement.Pln05 biz = new Biz.PlanManagement.Pln05();

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
        public string[] strKeyColumn = new string[] { "PLANT_CD", "ORDER_NO", "PART_NO" };

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
                                createGrid('" + strFix + "'); ";

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            
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
            sParam.Add("MENU_ID", "Pln05");
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("CUR_MENU_ID", "Pln05");

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
                        arrColumn = new string[] { "PLANT_CD", "PLAN_DATE", "TRANS_FLG", "SHIP_NO", "CAR_NM_KOR", "DRIVER_NM", "TRANS_INFO", "PLAN_QTY", "DELIVERY_QTY", "PART_NO", "PART_DESC", "S_STORAGE_NM", "D_STORAGE_NM", "CUSTOMER_NM", "KEY_HID" };
                        arrColumnCaption = new string[arrColumn.Length];
                        arrColumnWidth = new string[arrColumn.Length];
                        strFix = "9";

                        for (int i = 0; i < arrColumn.Length; i++)
                        {
                            arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);
                            if (arrColumn[i].Equals("SHIP_NO") || arrColumn[i].Equals("PLAN_ORDER_NO") || arrColumn[i].Equals("DRIVER_NM") || arrColumn[i].Equals("TRANS_INFO")) arrColumnWidth[i] = "130";
                            else if (arrColumn[i].Equals("PLAN_QTY") || arrColumn[i].Equals("DELIVERY_QTY") || arrColumn[i].Equals("EVENT_FLG_NM")) arrColumnWidth[i] = "70";
                            else if (arrColumn[i].Equals("CAR_NM_KOR") || arrColumn[i].Equals("S_STORAGE_NM") || arrColumn[i].Equals("D_STORAGE_NM") || arrColumn[i].Equals("CUSTOMER_NM") || arrColumn[i].Equals("PART_DESC")) arrColumnWidth[i] = "250";
                            else arrColumnWidth[i] = "100";
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
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Pln05Data.Get_ShipList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();

            sParam.Add("CUR_MENU_ID", "Pln05");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            FW.Data.Parameters logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_START");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Pln05");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            // 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_END");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Pln05");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Faile", "fn_NoData();", true);
            }
        }
        #endregion

        #region [CHECK SHIP NUMBER]
        private void CheckShipNO()
        {

            if (txtShipNo.Text.Equals(string.Empty))
            {
                ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), Title, "alert('출하증 번호를 입력해 주세요');", true);
                return;
            }
            else
            {
                DataSet ds = new DataSet();
                string strTableName = string.Empty;
                string strMessage = string.Empty;
                int iRtn = 0;

                strDBName = "GPDB";
                strQueryID = "Pln05Data.Get_ShipNo_ValChk";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("SHIP_NO", txtShipNo.Text.ToString());

                sParam.Add("CUR_MENU_ID", "Pln05");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
                        lbShipNo.Text = txtShipNo.Text;
                        lbShipResult.Text = " [ " + ds.Tables[0].Rows[0]["RESULT_MSG"].ToString() + " ] ";
                        lbShipMsg.Text = ds.Tables[0].Rows[0]["DETAIL_MSG"].ToString();

                        if (ds.Tables[0].Rows[0]["CNT"].ToString() != "0")
                        {
                            string strScript = string.Empty;
                            strDBName = "GPDB";
                            strQueryID = "Pln05Data.Set_ShipNo";

                            FW.Data.Parameters param = new FW.Data.Parameters();
                            param.Add("SHIP_NO", txtShipNo.Text.ToString());

                            param.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                            param.Add("CUD_TYPE", "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                            param.Add("CUR_MENU_ID", "Pln05");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                            param.Add("PREV_DATA", "Ship No : " + txtShipNo.Text.ToString());                  // 이전 데이터 셋팅

                            iRtn = biz.SetCUD(strDBName, strQueryID, param);

                            if (iRtn == 1)
                            {
                                strScript = " alert('정상출하 되었습니다.'); ";
                                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "saveData", strScript, true);
                                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                            }
                            else
                            {
                                strScript = " alert('출하에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "saveData", strScript, true);
                                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                            }
                        }
                    }
                }
                else
                {
                    lbShipNo.Text = txtShipNo.Text;
                    lbShipResult.Text = " [ 출하불가 ] ";
                    lbShipMsg.Text = "";
                }
            }

            txtShipNo.Text = string.Empty;
            txtShipNo.Focus();
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }
        #endregion

        #region ibtnRegister_Click
        protected void ibtnRegister_Click(object sender, EventArgs e)
        {
            CheckShipNO();
            txtShipNo.Focus();
            GetData();
        }
        #endregion
    }
}