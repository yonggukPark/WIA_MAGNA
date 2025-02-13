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

namespace HQCWeb.InfoMgt.Info17
{
    public partial class Info17 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        Biz.InfoManagement.Info17 biz = new Biz.InfoManagement.Info17();

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
        public string[] strKeyColumn = new string[] { "PLANT_CD", "SHOP_CD", "LINE_CD", "CAR_TYPE", "STN_CD", "WORK_CD", "FLD_CD" };

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

            DataSet ds = new DataSet();

            ddlShopCd.Items.Add(new ListItem("선택하세요", ""));
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));
            ddlStnCd.Items.Add(new ListItem("ALL", ""));
            ddlDevId.Items.Add(new ListItem("ALL", ""));
            ddlCarType.Items.Add(new ListItem("ALL", ""));

            strDBName = "GPDB";
            strQueryID = "Info17Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", "");
            param.Add("LINE_CD", "");
            param.Add("STN_CD", "");
            param.Add("CAR_TYPE", "");
            param.Add("DEV_ID", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlShopCd.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                }
            }
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
            sParam.Add("MENU_ID", "Info17");
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("CUR_MENU_ID", "Info17");

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
                        arrColumn = new string[] { "PLANT_CD", "SHOP_CD", "LINE_CD", "DEV_ID", "CAR_TYPE", "DEV_NM", "STN_CD", "STN_NM", "FLD_CD", "FLD_NM", "UCL", "LCL", "WORK_CD", "REMARK1", "REMARK2", "USE_YN", "REG_DATE", "REG_USER", "MOD_DATE", "MOD_USER", "KEY_HID" };
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
            lbCarType.Text = Dictionary_Data.SearchDic("CAR_TYPE", bp.g_language);
            lbDevId.Text = Dictionary_Data.SearchDic("DEV_ID", bp.g_language);
            lbFldCd.Text = Dictionary_Data.SearchDic("FLD_CD", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData(string flag)
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Info17Data.Get_SPCList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());
            sParam.Add("SHOP_CD",ddlShopCd.SelectedValue);
            sParam.Add("LINE_CD",ddlLineCd.SelectedValue);
            sParam.Add("CAR_TYPE",ddlCarType.SelectedValue);
            sParam.Add("STN_CD",ddlStnCd.SelectedValue);
            sParam.Add("DEV_ID",ddlDevId.SelectedValue);
            sParam.Add("FLD_CD",txtFldCd.Text);
            sParam.Add("FLAG",flag);

            sParam.Add("CUR_MENU_ID", "Info17");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            FW.Data.Parameters logParam = new FW.Data.Parameters();
            if (flag == "N") logParam.Add("BUTTON_ID", "btnSearch"); else logParam.Add("BUTTON_ID", "btnRestore");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_START");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Info17");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            // 비지니스 메서드 호출
            ds = biz.GetSPC(strDBName, strQueryID, sParam);

            logParam = new FW.Data.Parameters();
            if (flag == "N") logParam.Add("BUTTON_ID", "btnSearch"); else logParam.Add("BUTTON_ID", "btnRestore");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_END");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Info17");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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

        #region ddlShopCd_SelectedIndexChanged
        protected void ddlShopCd_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlLineCd.Items.Clear();
            ddlStnCd.Items.Clear();
            ddlDevId.Items.Clear();
            ddlCarType.Items.Clear();

            //비활성
            ddlLineCd.Enabled = false;
            ddlStnCd.Enabled = false;
            ddlDevId.Enabled = false;
            ddlCarType.Enabled = false;

            //초기화
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));
            ddlStnCd.Items.Add(new ListItem("ALL", ""));
            ddlDevId.Items.Add(new ListItem("ALL", ""));
            ddlCarType.Items.Add(new ListItem("ALL", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info17Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            //param.Add("PLANT_CD", "P1");
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", "");
            param.Add("STN_CD", "");
            param.Add("CAR_TYPE", "");
            param.Add("DEV_ID", "");

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
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        ddlStnCd.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlStnCd.Enabled = true;
                }
                //Device Code 있으면
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        ddlDevId.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevId.Enabled = true;
                }
                //Car Type 있으면
                if (ds.Tables[4].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
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

            //데이터 클리어
            ddlStnCd.Items.Clear();
            ddlDevId.Items.Clear();
            ddlCarType.Items.Clear();

            //비활성
            ddlStnCd.Enabled = false;
            ddlDevId.Enabled = false;
            ddlCarType.Enabled = false;

            //초기화
            ddlStnCd.Items.Add(new ListItem("ALL", ""));
            ddlDevId.Items.Add(new ListItem("ALL", ""));
            ddlCarType.Items.Add(new ListItem("ALL", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info17Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            //param.Add("PLANT_CD", "P1");
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", ddlLineCd.SelectedValue);
            param.Add("STN_CD", "");
            param.Add("CAR_TYPE", "");
            param.Add("DEV_ID", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Stn Code 있으면
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        ddlStnCd.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlStnCd.Enabled = true;
                }
                //Car Type 있으면
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarType.Enabled = true;
                }
            }
        }
        #endregion

        #region ddlStnCd_ddlCarType_SelectedIndexChanged
        protected void ddlStnCd_ddlCarType_SelectedIndexChanged(object sender, EventArgs e)
        {

            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlDevId.Items.Clear();

            //비활성
            ddlDevId.Enabled = false;

            //초기화
            ddlDevId.Items.Add(new ListItem("ALL", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info17Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            //param.Add("PLANT_CD", "P1");
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", ddlLineCd.SelectedValue);
            param.Add("STN_CD", ddlStnCd.SelectedValue);
            param.Add("CAR_TYPE", ddlCarType.SelectedValue);
            param.Add("DEV_ID", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Car Type 있으면
                if (ds.Tables[4].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        ddlDevId.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevId.Enabled = true;
                }
            }
        }
        #endregion


    }
}