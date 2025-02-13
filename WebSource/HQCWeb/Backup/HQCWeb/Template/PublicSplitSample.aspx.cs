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

namespace HQCWeb.Template
{
    public partial class PublicSplitSample : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();
        Crypt cy = new Crypt();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        
        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        // 비지니스 클래스 작성
        //Biz.Sample_Biz biz = new Biz.Sample_Biz();

        #region GRID Setting
        // 메인 그리드에 보여져야할 컬럼 정의
        public string[] arrMainColumn;
        // 메인 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrMainColumnCaption;
        // 메인 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrMainColumnWidth;
        // 메인 그리드 키값 정의
        //public string strMainKeyColumn = "COL_1";
        public string[] strMainKeyColumn = new string[] { "COL_1" };
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
        // 그리드 고정값 정의
        public string strMainFix;
        // 상세 그리드 키값 정의
        public string[] strDetailKeyColumn = new string[] { "COL_1" };
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

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createMainGrid' 함수 호출
                string scriptMain = $@" column = {jsMainCol}; 
                                field = {jsMainField}; 
                                createMainGrid('" + strMainFix + "'); ";


                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createDetailGrid' 함수 호출
                string scriptDetail = $@" column = {jsDetailCol}; 
                                field = {jsDetailField}; 
                                createDetailGrid(); ";


                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), scriptMain, true);

                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), scriptDetail, true);
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
            sParam.Add("MENU_ID", "MENU_ID");
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("CUR_MENU_ID", "MENU_ID");

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
                        arrMainColumn = new string[] { "COL_1", "COL_2", "COL_3", "COL_4", "COL_5" };
                        arrMainColumnCaption = new string[arrMainColumn.Length];
                        arrMainColumnWidth = new string[arrMainColumn.Length];
                        strMainFix = "";

                        for (int i = 0; i < arrMainColumn.Length; i++)
                        {
                            arrMainColumnCaption[i] = Dictionary_Data.SearchDic(arrMainColumn[i].ToString(), bp.g_language);
                            arrMainColumnWidth[i] = "200";
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Faile", "fn_NoData();", true);
            }

            arrDetailColumn = new string[] { "COL_1", "COL_2", "COL_3", "COL_4", "COL_5" };
            arrDetailColumnCaption = new string[arrDetailColumn.Length];
            arrDetailColumnWidth = new string[arrDetailColumn.Length];

            for (int i = 0; i < arrDetailColumn.Length; i++)
            {
                arrDetailColumnCaption[i] = Dictionary_Data.SearchDic(arrDetailColumn[i].ToString(), bp.g_language);
                arrDetailColumnWidth[i] = "100";
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
            lbCond_01.Text = Dictionary_Data.SearchDic("CON_01", bp.g_language);
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            strDBName = "DBNAME";
            strQueryID = "SQLMAP_NAMESPACE.STATEMENT_ID";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("Param1", "");

            sParam.Add("CUR_MENU_ID", lbTitle.Text);                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 비지니스 메서드 호출
            //ds = biz.(strDBName, strQueryID, param);

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

        #region btnDetailSearch_Click
        protected void btnDetailSearch_Click(object sender, EventArgs e)
        {
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            string[] strSplitValue = cy.Decrypt(hidParams.Value).Split('/');

            strDBName = "DBNAME";
            strQueryID = "SQLMAP_NAMESPACE.STATEMENT_ID";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("Param1",        strSplitValue[0]);

            sParam.Add("CUR_MENU_ID",   "MENU_ID");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            DataSet ds = new DataSet();

            //ds = biz.(strDBName, strQueryID, param);

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
                            string script = $@" data = {jsDetailData}; 
                            createDetailGrid(); ";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);
                        }
                        else
                        {
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            string script = $@" data = ''; 
                                createDetailGrid(); ";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);

                            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData();", true);
                        }

                    }
                    else
                    {
                        // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                        string script = $@" data = ''; 
                            createDetailGrid(); ";

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
    }
}