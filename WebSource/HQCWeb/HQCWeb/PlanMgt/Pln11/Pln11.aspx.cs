using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MES.FW.Common.Crypt;
using MES.FW.Common.CommonMgt;
using HQCWeb.FMB_FW;
using HQCWeb.FW;

using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HQCWeb.PlanMgt.Pln11
{
    public partial class Pln11 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();
        Crypt cy = new Crypt();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        // 암복호화 키값 셋팅
        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");


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
            strErrMessage = Message_Data.SearchDic("SearchError", bp.g_language);
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
            sParam.Add("MENU_ID", "Pln11");
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("CUR_MENU_ID", "Pln11");

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
                        arrColumn = new string[] { "COL_1", "COL_2", "COL_3", "COL_4", "COL_5", "KEY_HID" };
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
            lbCond_01.Text = Dictionary_Data.SearchDic("CON_01", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData(string flag)
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            strDBName = "GPDB";
            strQueryID = "CUDData.Get_CUDLogList";

            // 비지니스 클래스 작성
            Biz.PlanManagement.Pln11 biz = new Biz.PlanManagement.Pln11();

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("CUD_NUM", "");

            sParam.Add("FLAG", flag);

            sParam.Add("CUR_MENU_ID", "Pln11");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
                        jsData = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[0], strKeyColumn);

                        //정상처리되면
                        if (!String.IsNullOrEmpty(jsData))
                        {

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "fn_loadingEnd();", true);
                        }
                        else
                        {

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "fn_loadingEnd();", true);
                        }

                    }
                    else
                    {
                        //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "fn_loadingEnd();", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "fn_loadingEnd();", true);

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

        #region btnModify_Click
        protected void btnModify_Click(object sender, EventArgs e)
        {

            string strScript = string.Empty;
            string strKeyDt = string.Empty;
            string strEncrypt = string.Empty;

            cy.Key = strKey;

            strEncrypt = Convert.ChangeType(hiddenSearchDt.Value.ToString(), typeof(string)).ToString();

            strKeyDt = cy.Encrypt(strEncrypt);

            strScript = " fn_ModifyConfirm('" + strKeyDt + "'); ";
            //ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);

            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", strScript, true);
        }
        #endregion

    }
}