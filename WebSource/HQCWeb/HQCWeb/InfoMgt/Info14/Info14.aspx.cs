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

namespace HQCWeb.InfoMgt.Info14
{
    public partial class Info14 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        ExcelExport ee = new ExcelExport();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        // 비지니스 클래스 작성
        Biz.InfoManagement.Info14 biz = new Biz.InfoManagement.Info14();

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
        string jsCol = string.Empty;
        string jsField = string.Empty;
        string jsData = string.Empty;

        //데이터 저장용 변수
        public static string[,] iPtnTime = new string[24, 12];

        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageInit();

            if (!IsPostBack)
            {
                SetCon();

                //SetGridTitle();

                SetTitle();

                //Data "0" 초기화
                GetClear();

                jsData = JsonConvert.SerializeObject(iPtnTime);

                string script = $@" data = {jsData}; ";

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);
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
            ddlPtnCd.Items.Add(new ListItem("선택하세요", ""));

            strDBName = "GPDB";
            strQueryID = "Info14Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", "");
            param.Add("LINE_CD", "");

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
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    ddlPtnCd.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
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
            //jsCol = ConvertJSONData.ConvertColArrToJSON(arrColumn, arrColumnCaption, arrColumnWidth, "cols");
            //jsField = ConvertJSONData.ConvertColArrToJSON(arrColumn, arrColumnCaption, arrColumnWidth, "fields");
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbPtnCd.Text = Dictionary_Data.SearchDic("PTN_CD", bp.g_language);
            lbPtnCdNm.Text = Dictionary_Data.SearchDic("PTN_CD", bp.g_language);
            lbPtnNm.Text = Dictionary_Data.SearchDic("PTN_NM", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData(string flag)
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;
            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Info14Data.Get_PtnList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());
            sParam.Add("SHOP_CD", ddlShopCd.SelectedValue);
            sParam.Add("LINE_CD", ddlLineCd.SelectedValue);
            if (flag == "I")
                sParam.Add("PTN_CD", txtPtnCd.Text);
            else
            sParam.Add("PTN_CD", ddlPtnCd.SelectedValue);
            sParam.Add("FLAG", "N");

            sParam.Add("CUR_MENU_ID", "Info14");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            FW.Data.Parameters logParam = new FW.Data.Parameters();
            if (flag == "N") logParam.Add("BUTTON_ID", "btnSearch"); else logParam.Add("BUTTON_ID", "btnRestore");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_START");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Info14");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            // 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            logParam = new FW.Data.Parameters();
            if (flag == "N") logParam.Add("BUTTON_ID", "btnSearch"); else logParam.Add("BUTTON_ID", "btnRestore");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_END");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Info14");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
                    //Data "0" 초기화
                    GetClear();

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        //iPtnTime에 1 넣기 과정
                        int cnt = 0;

                        for (int dtCnt = 0; dtCnt < ds.Tables[0].Rows.Count; dtCnt++)
                        {
                            for (int iHour = 0; iHour < iPtnTime.GetLength(0); iHour++)
                            {
                                bool flg = false;

                                if (Convert.ToInt32(ds.Tables[0].Rows[dtCnt]["WK_TM"].ToString()) == iHour)
                                {

                                    for (int iMin = 0; iMin < iPtnTime.GetLength(1); iMin++)
                                    {
                                        int min = Convert.ToInt32(ds.Tables[0].Rows[dtCnt]["WK_FR_MIN"].ToString());
                                        if (min == iMin * 5) //5분단위
                                        {
                                            flg = true;
                                        }
                                        if (flg == true)
                                        {
                                            if (Convert.ToInt32(ds.Tables[0].Rows[dtCnt]["WK_TO_MIN"].ToString()) >= (iMin + 1) * 5 - 1)
                                            {
                                                iPtnTime[iHour, iMin] = "1";
                                                cnt++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        
                        // 초기화 작업
                        jsData = JsonConvert.SerializeObject(iPtnTime);

                        //GetClear();

                        //정상처리되면
                        if (cnt > 0)
                        {
                            txtPtnCd.Text = ds.Tables[0].Rows[0]["PTN_CD"].ToString();
                            txtPtnNm.Text = ds.Tables[0].Rows[0]["PTN_NM"].ToString();

                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            string script = $@" data = {jsData}; 
                            createGrid(); ";

                            if (flag == "U")
                                script += " alert('정상수정 되었습니다.'); ";
                            else if (flag == "I")
                                script += " alert('정상등록 되었습니다.'); ";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);
                        }
                        else
                        {
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            string script = $@" data = {jsData};
                            createGrid(); ";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);

                            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData();", true);
                        }

                    }
                    else
                    {
                        // 초기화 작업
                        jsData = JsonConvert.SerializeObject(iPtnTime);
                        
                        // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                        string script = $@" data = {jsData}; 
                        createGrid(); ";
                        
                        if (flag == "D")
                            script += " alert('정상삭제 되었습니다.'); ";

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

        #region GetPattern
        [WebMethod]
        public static void GetPattern(string clicked)
        {
            //hidPattern에서 패턴값을 받아서 iPtnTime에 적용
            string[] strPattern = clicked.Split('/');
            string strHour = strPattern[0].Substring(0,2), strMin = strPattern[0].Substring(2, 3).Replace("m", "");
            int iHour = 0, iMin = 0;

            iHour = (strHour.Equals("24")) ? 0 : Convert.ToInt32(strHour);
            iMin = (Convert.ToInt32(strMin) / 5) - 1;

            if (iMin < 12)
                iPtnTime[iHour, iMin] = strPattern[1];
            else
            {
                for (int i = 0; i < iMin; i++)
                {
                    iPtnTime[iHour, i] = strPattern[1];
                }
            }
        }
        #endregion

        #region GetClear
        public void GetClear()
        {
            for (int iHour = 0; iHour < iPtnTime.GetLength(0); iHour++)
            {
                for (int iMin = 0; iMin < iPtnTime.GetLength(1); iMin++)
                {
                    iPtnTime[iHour, iMin] = "0";
                }
            }
        }
        #endregion

        #region setPtnDropdown
        public void setPtnDropdown()
        {
            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlPtnCd.Items.Clear();

            //비활성
            ddlPtnCd.Enabled = false;

            //초기화
            ddlPtnCd.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info14Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", ddlLineCd.SelectedValue);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Car Type 있으면
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        ddlPtnCd.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlPtnCd.Enabled = true;
                }
            }
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetData("S");
        }
        #endregion

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int iRtn = 0;
            string strRtn = string.Empty;
            string strScript = string.Empty;

            if (su.SetControlValChk(this.Page.Master, "MainContent"))
            {
                strDBName = "GPDB";
                strQueryID = "Info14Data.Get_PtnID_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("PLANT_CD", bp.g_plant.ToString());
                sParamIDChk.Add("SHOP_CD", ddlShopCd.SelectedValue);
                sParamIDChk.Add("LINE_CD", ddlLineCd.SelectedValue);
                sParamIDChk.Add("PTN_CD", txtPtnCd.Text);

                // 아이디 체크 비지니스 메서드 작성
                strRtn = biz.GetIDValChk(strDBName, strQueryID, sParamIDChk);
                if (strRtn == "0")
                {
                    DataSet ds = new DataSet();
                    string strTableName = string.Empty;

                    //체크 재호출(Shift 값 호출)
                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("PLANT_CD", bp.g_plant.ToString());
                    sParam.Add("SHOP_CD", "");
                    sParam.Add("LINE_CD", "");
                    sParam.Add("PTN_CD", "");

                    sParam.Add("CUR_MENU_ID", "Info14");        // 조회페이지 메뉴 아이디 입력 - Shift 조회이므로 로그 필요

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
                            for (int iHour = 0; iHour < iPtnTime.GetLength(0); iHour++)
                            {
                                int iFrdt = -1, iTodt = -1;
                                string strHour, strToMin, strFrMin;
                                int orderFlg = 0;
                                for (int iMin = 0; iMin < iPtnTime.GetLength(1); iMin++)
                                {
                                    if (iFrdt == -1) //시작 시간 지정
                                        iFrdt = iMin * 5;
                                    if (iMin < 11)
                                    {
                                        if (!iPtnTime[iHour, iMin + 1].Equals(iPtnTime[iHour, iMin])) //뒤의 분이 없으면
                                            iTodt = iMin * 5 + 4;
                                    }
                                    else
                                        iTodt = 59;

                                    if (iFrdt > -1 && iTodt > -1)
                                    {
                                        strHour = Convert.ToString(iHour).PadLeft(2, '0');
                                        strFrMin = Convert.ToString(iFrdt).PadLeft(2, '0');
                                        strToMin = Convert.ToString(iTodt).PadLeft(2, '0');

                                        if (iPtnTime[iHour, iMin].Equals("1"))
                                            orderFlg = 1; //가동
                                        else
                                            orderFlg = 2; //비가동

                                        //저장 로직 지정
                                        strDBName = "GPDB";
                                        strQueryID = "Info14Data.Set_PtnInfo";

                                        FW.Data.Parameters sParam2 = new FW.Data.Parameters();
                                        sParam2.Add("PLANT_CD", bp.g_plant.ToString());
                                        sParam2.Add("SHOP_CD", ddlShopCd.SelectedValue);
                                        sParam2.Add("LINE_CD", ddlLineCd.SelectedValue);
                                        sParam2.Add("PTN_CD", txtPtnCd.Text);

                                        sParam2.Add("WK_TM", strHour);
                                        sParam2.Add("WK_FR_MIN", strFrMin);
                                        sParam2.Add("WK_TO_MIN", strToMin);
                                        sParam2.Add("PTN_NM", txtPtnNm.Text);
                                        sParam2.Add("ORDER_FLG", orderFlg.ToString());

                                        sParam2.Add("REMARK1", "");
                                        sParam2.Add("REMARK2", "");
                                        sParam2.Add("USE_YN", "Y");
                                        sParam2.Add("USER_ID", bp.g_userid.ToString());

                                        // 수정이력 저장용 파라미터
                                        sParam2.Add("REG_ID", bp.g_userid.ToString());
                                        sParam2.Add("CUD_TYPE", "C");           // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                                        sParam2.Add("CUR_MENU_ID", "Info14");     // 조회페이지 메뉴 아이디 입력

                                        // shift 순회
                                        for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                        {
                                            int iShFrDt = Convert.ToInt32(ds.Tables[1].Rows[i]["START_TIME"].ToString());
                                            int iShToDt = Convert.ToInt32(ds.Tables[1].Rows[i]["END_TIME"].ToString());
                                            int iFrTm = Convert.ToInt32(strHour + strFrMin); //스케쥴 시작시간
                                            int iToTm = Convert.ToInt32(strHour + strToMin);//스케쥴 끝시간

                                            //Shift type 지정
                                            sParam2.Add("SHIFT_TYPE", ds.Tables[1].Rows[i]["CODE"].ToString());
                                            if (iShFrDt < iShToDt) //주간
                                            {
                                                if (iShFrDt <= iFrTm && iToTm <= iShToDt)
                                                {
                                                    // 등록 비지니스 메서드 작성
                                                    iRtn = biz.SetCUD(strDBName, strQueryID, sParam2);
                                                    if (iRtn != 1)//오류발생
                                                    {
                                                        break;
                                                    }

                                                    iFrdt = -1; iTodt = -1;
                                                    break;
                                                }
                                            }
                                            else //야간
                                            {
                                                if (iShFrDt <= iFrTm || iTodt <= iShToDt)
                                                {
                                                    // 등록 비지니스 메서드 작성
                                                    iRtn = biz.SetCUD(strDBName, strQueryID, sParam2);
                                                    if (iRtn != 1)//오류발생
                                                    {
                                                        break;
                                                    }

                                                    iFrdt = -1; iTodt = -1;
                                                    break;
                                                }
                                            }

                                        }
                                    }
                                }
                            } //for

                            //0 초기화
                            GetClear();

                            if (iRtn == 1)
                            {
                                setPtnDropdown();
                                GetData("I");
                                strScript = " alert('정상등록 되었습니다.'); ";
                                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                            }
                            else
                            {
                                strScript = " alert('등록에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                            }
                        }
                    }
                }
                else
                {
                    strScript = " alert('패턴코드가 중복입니다. 등록하려는 패턴코드를 다시 확인하세요.'); ";
                    ScriptManager.RegisterStartupScript(Page, typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
            }
            else
            {
                strScript = " fn_ExError(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnModify_Click
        protected void btnModify_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;

            int iRtn = 0;
            string strScript = string.Empty;

            if (su.SetControlValChk(this.Page.Master, "MainContent"))
            {

                strDBName = "GPDB";
                strQueryID = "Info14Data.Get_PtnID_ValChk";

                //체크 재호출(Shift 값 호출)
                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", bp.g_plant.ToString());
                sParam.Add("SHOP_CD", "");
                sParam.Add("LINE_CD", "");
                sParam.Add("PTN_CD", "");

                sParam.Add("CUR_MENU_ID", "Info14");        // 조회페이지 메뉴 아이디 입력 - Shift 조회이므로 로그 필요

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
                        for (int iHour = 0; iHour < iPtnTime.GetLength(0); iHour++)
                        {
                            int iFrdt = -1, iTodt = -1;
                            string strHour, strToMin, strFrMin;
                            int orderFlg = 0;
                            for (int iMin = 0; iMin < iPtnTime.GetLength(1); iMin++)
                            {
                                if (iFrdt == -1) //시작 시간 지정
                                    iFrdt = iMin * 5;
                                if (iMin < 11)
                                {
                                    if (!iPtnTime[iHour, iMin + 1].Equals(iPtnTime[iHour, iMin])) //뒤의 분이 없으면
                                        iTodt = iMin * 5 + 4;
                                }
                                else
                                    iTodt = 59;

                                if (iFrdt > -1 && iTodt > -1)
                                {
                                    strHour = Convert.ToString(iHour).PadLeft(2, '0');
                                    strFrMin = Convert.ToString(iFrdt).PadLeft(2, '0');
                                    strToMin = Convert.ToString(iTodt).PadLeft(2, '0');

                                    if (iPtnTime[iHour, iMin].Equals("1"))
                                        orderFlg = 1; //가동
                                    else
                                        orderFlg = 2; //비가동

                                    //저장 로직 지정
                                    strDBName = "GPDB";
                                    strQueryID = "Info14Data.Upt_PtnInfo";

                                    FW.Data.Parameters sParam2 = new FW.Data.Parameters();
                                    sParam2.Add("PLANT_CD", bp.g_plant.ToString());
                                    sParam2.Add("SHOP_CD", ddlShopCd.SelectedValue);
                                    sParam2.Add("LINE_CD", ddlLineCd.SelectedValue);
                                    sParam2.Add("PTN_CD", txtPtnCd.Text);

                                    sParam2.Add("WK_TM", strHour);
                                    sParam2.Add("WK_FR_MIN", strFrMin);
                                    sParam2.Add("WK_TO_MIN", strToMin);
                                    sParam2.Add("PTN_NM", txtPtnNm.Text);
                                    sParam2.Add("ORDER_FLG", orderFlg.ToString());

                                    sParam2.Add("REMARK1", "");
                                    sParam2.Add("REMARK2", "");
                                    sParam2.Add("USE_YN", "Y");
                                    sParam2.Add("USER_ID", bp.g_userid.ToString());

                                    // 수정이력 저장용 파라미터
                                    sParam2.Add("REG_ID", bp.g_userid.ToString());
                                    sParam2.Add("CUD_TYPE", "U");           // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                                    sParam2.Add("CUR_MENU_ID", "Info14");     // 조회페이지 메뉴 아이디 입력
                                    sParam2.Add("PREV_DATA", ""); //기존데이터 : 추가 쿼리 필요

                                    // shift 순회
                                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                                    {
                                        int iShFrDt = Convert.ToInt32(ds.Tables[1].Rows[i]["START_TIME"].ToString());
                                        int iShToDt = Convert.ToInt32(ds.Tables[1].Rows[i]["END_TIME"].ToString());
                                        int iFrTm = Convert.ToInt32(strHour + strFrMin); //스케쥴 시작시간
                                        int iToTm = Convert.ToInt32(strHour + strToMin);//스케쥴 끝시간

                                        //Shift type 지정
                                        sParam2.Add("SHIFT_TYPE", ds.Tables[1].Rows[i]["CODE"].ToString());
                                        if (iShFrDt < iShToDt) //주간
                                        {
                                            if (iShFrDt <= iFrTm && iToTm <= iShToDt)
                                            {
                                                // 등록 비지니스 메서드 작성
                                                iRtn = biz.SetCUD(strDBName, strQueryID, sParam2);
                                                if (iRtn != 1)//오류발생
                                                {
                                                    break;
                                                }

                                                iFrdt = -1; iTodt = -1;
                                                break;
                                            }
                                        }
                                        else //야간
                                        {
                                            if (iShFrDt <= iFrTm || iTodt <= iShToDt)
                                            {
                                                // 등록 비지니스 메서드 작성
                                                iRtn = biz.SetCUD(strDBName, strQueryID, sParam2);
                                                if (iRtn != 1)//오류발생
                                                {
                                                    break;
                                                }

                                                iFrdt = -1; iTodt = -1;
                                                break;
                                            }
                                        }

                                    }
                                }
                            }
                        } //for

                        // 0 초기화
                        GetClear();

                        if (iRtn == 1)
                        {
                            GetData("U");
                            strScript = " alert('정상수정 되었습니다.'); ";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        }
                        else
                        {
                            strScript = " alert('수정에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        }
                    }
                }
            }
            else
            {
                strScript = " fn_ExError(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnDelete_Click
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int iRtn = 0;
            string strScript = string.Empty;

            if (su.SetControlValChk(this.Page.Master, "MainContent"))
            {
                strDBName = "GPDB";
                strQueryID = "Info14Data.Set_PtnInfoDel";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", bp.g_plant.ToString());
                sParam.Add("SHOP_CD", ddlShopCd.SelectedValue);
                sParam.Add("LINE_CD", ddlLineCd.SelectedValue);
                sParam.Add("PTN_CD", ddlPtnCd.SelectedValue);
                sParam.Add("FLAG", "Y");
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Info14");                 // 조회페이지 메뉴 아이디 입력
                sParam.Add("PREV_DATA", "");

                // 삭제 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    setPtnDropdown();
                    GetData("D");
                }
                else
                {
                    strScript = " alert('삭제에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
            }
            else
            {
                strScript = " fn_ExError(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region ddlShopCd_SelectedIndexChanged
        protected void ddlShopCd_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlLineCd.Items.Clear();
            ddlPtnCd.Items.Clear();

            //비활성
            ddlLineCd.Enabled = false;
            ddlPtnCd.Enabled = false;

            //초기화
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));
            ddlPtnCd.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info14Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", "");

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
                //Car Type 있으면
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        ddlPtnCd.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlPtnCd.Enabled = true;
                }
            }
        }
        #endregion

        #region ddlLineCd_SelectedIndexChanged
        protected void ddlLineCd_SelectedIndexChanged(object sender, EventArgs e)
        {
            setPtnDropdown();
        }
        #endregion
        
    }
}