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

namespace HQCWeb.PlanMgt.Pln04
{
    public partial class Pln04 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        // 비지니스 클래스 작성
        Biz.PlanManagement.Pln04 biz = new Biz.PlanManagement.Pln04();

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
        public string[] strKeyColumn = new string[] { "PLANT_CD","ORDER_NO","PART_NO" };

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
            ddlCustomerCd.Items.Add(new ListItem("ALL", ""));
            ddlStatus.Items.Add(new ListItem("ALL", ""));
            ddlPlanType.Items.Add(new ListItem("ALL", ""));
            ddlStorageCd.Items.Add(new ListItem("ALL", ""));
            ddlSubStorageCd.Items.Add(new ListItem("ALL", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Pln04Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            //param.Add("PLANT_CD", "P1");
            param.Add("SHOP_CD", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                jsDdl1 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[0]);
                jsDdl2 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[1]);

                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    ddlCustomerCd.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
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
            sParam.Add("MENU_ID", "Pln04");
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("CUR_MENU_ID", "Pln04");

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
                        arrColumn = new string[] { "PLANT_CD", "PLAN_DATE", "PLAN_ORDER_NO", "TRANS_FLG", "SHIP_NO", "CAR_NM_KOR", "PLAN_QTY", "DELIVERY_QTY", "PART_NO", "PART_DESC", "S_STORAGE_NM", "D_STORAGE_NM", "CUSTOMER_NM", "DRIVER_NM", "TRANS_INFO", "EVENT_FLG_NM", "ORDER_NO", "STATUS_FLG", "EVENT_FLG", "KEY_HID" };
                        arrColumnCaption = new string[arrColumn.Length];
                        arrColumnWidth = new string[arrColumn.Length];
                        strFix = "8";

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
            lbPlanDt.Text = Dictionary_Data.SearchDic("PLAN_DT", bp.g_language);
            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbPartNo.Text = Dictionary_Data.SearchDic("PART_NO", bp.g_language);
            lbStatus.Text = Dictionary_Data.SearchDic("SHIP_STATUS", bp.g_language);
            lbShipNo.Text = Dictionary_Data.SearchDic("SHIP_NO", bp.g_language);
            lbCustomerCd.Text = Dictionary_Data.SearchDic("CUSTOMER_CD", bp.g_language);
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
            strQueryID = "Pln04Data.Get_PlanList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());
            //sParam.Add("PLANT_CD", "P1");
            sParam.Add("FR_DT", txtFromDt.Text.Replace("-",""));
            sParam.Add("TO_DT", txtToDt.Text.Replace("-", ""));
            sParam.Add("SHOP_CD", txtShopCdHidden.Text);
            sParam.Add("PART_NO", txtPartNoHidden.Text);
            sParam.Add("STATUS_CD", ddlStatus.SelectedValue);
            sParam.Add("CUSTOMER_CD", ddlCustomerCd.SelectedValue);
            sParam.Add("PLAN_TYPE", ddlPlanType.SelectedValue);
            sParam.Add("SHIP_NO", txtShipNo.Text);
            sParam.Add("SUB_STORAGE_CD", ddlSubStorageCd.SelectedValue);
            sParam.Add("STORAGE_CD", ddlStorageCd.SelectedValue);

            sParam.Add("CUR_MENU_ID", "Pln04");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            FW.Data.Parameters logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_START");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Pln04");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            // 비지니스 메서드 호출
            ds = biz.GetPlan(strDBName, strQueryID, sParam);

            logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_END");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Pln04");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
            strQueryID = "Pln04Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            //param.Add("PLANT_CD", "P1");
            param.Add("SHOP_CD", txtShopCdHidden.Text);

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

        #region SetShipNo
        [WebMethod]
        public static string SetShipNo(string[] sParams )
        {
            DataSet ds = new DataSet();//저장시 다중플래그가 나와서 구분해줘야 함
            Crypt cy = new Crypt();
            BasePage bp = new BasePage();

            cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");
            
            int iRtn = 0;
            string strRtn = string.Empty;
            string strRtn1 = string.Empty;
            string strRtn2 = string.Empty;
            string strSub = string.Empty;
            string strStorage = string.Empty;
            string strCust = string.Empty;
            string strPlan = string.Empty;
            string strPlantCd = string.Empty;
            string strOrderNo = string.Empty;
            string strPartNo = string.Empty;

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            string[] dataList = sParams;
            string[] strSplitValue = null;

            FW.Data.Parameters paramCheck = null;
            FW.Data.Parameters param = null;
            Biz.PlanManagement.Pln04 biz = new Biz.PlanManagement.Pln04();

            strDBName = "GPDB";
            strQueryID = "Pln04Data.Get_ShipNo_ValChk";
            
            //출하증 유무, 실적수량 유무 체크 및 출고창고/대상창고/고객사/계획형태 정보 일치 확인
            foreach (string data in dataList)
            {
                strSplitValue = cy.Decrypt(data).Split('/');
                paramCheck = new FW.Data.Parameters();
                paramCheck.Add("PLANT_CD", strSplitValue[0].ToString());
                paramCheck.Add("ORDER_NO", strSplitValue[1].ToString());
                paramCheck.Add("PART_NO", strSplitValue[2].ToString());
                paramCheck.Add("SUB_STORAGE_CD", strSub);
                paramCheck.Add("STORAGE_CD", strStorage);
                paramCheck.Add("CUSTOMER_CD", strCust);
                paramCheck.Add("PLAN_TYPE", strPlan);

                ds = biz.GetDataSet(strDBName, strQueryID, paramCheck);
                if (ds.Tables.Count > 0)
                {
                    strRtn1 = ds.Tables[0].Rows[0]["VAL_CHK"].ToString();//출하증(정상 0)
                    strRtn2 = ds.Tables[1].Rows[0]["VAL_CHK"].ToString();//실적수량(정상 1 이상)

                    if (!strRtn1.Equals("0"))
                    {
                        iRtn = 1;
                        break;
                    }
                    else if (strRtn2.Equals("0"))
                    {
                        iRtn = 2;
                        break;
                    }
                    else if(!strSub.Equals("") &&
                               ( 
                                !strSub.Equals(ds.Tables[2].Rows[0]["START_STORAGE_CD"].ToString()) || !strStorage.Equals(ds.Tables[2].Rows[0]["DEST_STORAGE_CD"].ToString()) ||
                                !strCust.Equals(ds.Tables[2].Rows[0]["CUSTOMER_CD"].ToString()) || !strPlan.Equals(ds.Tables[2].Rows[0]["EVENT_FLG"].ToString())
                               )
                            )//출고창고/대상창고/고객사/계획형태 정보 불일치
                    {
                        iRtn = 3;
                        break;
                    }
                    else if (strSub.Equals(""))//초기입력일경우
                    {
                        strSub = ds.Tables[2].Rows[0]["START_STORAGE_CD"].ToString();//출고창고
                        strStorage = ds.Tables[2].Rows[0]["DEST_STORAGE_CD"].ToString();//입고창고
                        strCust = ds.Tables[2].Rows[0]["CUSTOMER_CD"].ToString();//고객사
                        strPlan = ds.Tables[2].Rows[0]["EVENT_FLG"].ToString();//계획형태
                        strPlantCd = strSplitValue[0].ToString(); //공장코드 저장
                    }
                }
            }

            if (iRtn == 0)//데이터 문제없을시
            {
                strDBName = "GPDB";
                strQueryID = "Pln04Data.Set_ShipNo";


                //ORDER_NO, PART_NO 직렬화 작업
                foreach (string data in dataList)
                {
                    strSplitValue = cy.Decrypt(data).Split('/');
                    if (strOrderNo.Equals(""))
                    {
                        strOrderNo = strSplitValue[1].ToString();
                        strPartNo = strSplitValue[2].ToString();
                    }
                    else
                    {
                        strOrderNo += "," + strSplitValue[1].ToString();
                        strPartNo += "," + strSplitValue[2].ToString();
                    }
                }


                param = new FW.Data.Parameters();
                param.Add("PLANT_CD", strPlantCd);
                param.Add("ORDER_NO", strOrderNo);
                param.Add("PART_NO", strPartNo);

                param.Add("REG_ID", bp.g_userid.ToString());
                param.Add("CUD_TYPE", "U");
                param.Add("CUR_MENU_ID", "Pln04");

                iRtn = biz.SetCUD(strDBName, strQueryID, param);

                if (iRtn == 1)
                {
                    strRtn = "OK";
                    //strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('Pln02'); parent.fn_ModalCloseDiv(); ";
                    //ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "saveData", strScript, true);
                    //ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    //GetData();
                }
                else
                {
                    strRtn = "등록에 실패하였습니다. 관리자에게 문의바립니다.";
                    //strScript = " alert('등록에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                    //ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "saveData", strScript, true);
                    //ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
            }
            else if (iRtn == 1)
            {
                strRtn = "이미 출하증이 발행되어 생성할 수 없습니다.";
            }
            else if (iRtn == 2)
            {
                strRtn = "실적수량이 있는 경우에만 출하증 발행이 가능합니다.";
            }
            else if (iRtn == 3)
            {
                strRtn = "출고창고/대상창고/고객사/계획형태 정보가 상이합니다.";
            }

            return strRtn;
        }
        #endregion  
    }
}