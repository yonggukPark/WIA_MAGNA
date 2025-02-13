using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Web.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HQCWeb
{
    public partial class Sample2 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        public static string strImages = string.Empty;
        public int iImgCnt = 0;


        // 비지니스 클래스 작성
        //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();

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
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            strImages = "<ul>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-0.png' alt='' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-1.png' alt='' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-2.png' alt='' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-3.png' alt='' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-4.png' alt='' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-5.png' alt='' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-6.png' alt='' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-7.png' alt='' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-8.png' alt='' width='1280' height='300'></li>" +
                "</ul>";

            //divCon1.Attributes.Add("load", "Widget01.html");
            
            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_Load('1', 'widget01.html');", true);
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
        public void GetData()
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            strDBName = "DBNAME";
            strQueryID = "SQLMAP_NAMESPACE.STATEMENT_ID";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("Param1", "");

            sParam.Add("CUR_MENU_ID", "MENU_ID");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
                        SetGraphData(ds);

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

                //grid.SetDataListReset();
            }
        }
        #endregion

        #region SetGraphData
        private void SetGraphData(DataSet ds)
        {

            // 화면에 보여야할 Series 만큼 생성
            // ViewType.Bar, ViewType.Line
            //Series series1 = new Series("Hour Out Qty Sum", ViewType.Line);

            //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            //{
            //    series1.Points.Add(new SeriesPoint(i.ToString(), new double[] { (i + 1) }));
            //}

            //series1.LabelsVisibility = DefaultBoolean.True;

            // Add the series to the chart.
            //WebChartControl.Series.Add(series1);
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }
        #endregion

        #region btnReload_Click
        protected void btnReload_Click(object sender, EventArgs e)
        {
            
        }
        #endregion

        #region SetGraphData
        private void SetGraphData()
        {


        }
        #endregion

        #region GetDataSeries
        public DataTable GetDataSeries()
        {
            DataTable table = new DataTable();
            table.Columns.AddRange(new DataColumn[] {
                new DataColumn("Region", typeof(string)),
                new DataColumn("Sales", typeof(decimal))
            });

            return table;
        }
        #endregion

        #region createChartDt
        private DataTable createChartDt(DataTable dt)
        {
            DataTable chartDt = new DataTable();

            //chartDt.Columns.Add("HOUR");
            //chartDt.Columns.Add("PROCESSSEGMENTID");
            //chartDt.Columns.Add("OUTQTY", typeof(decimal));

            //for (int i = 1; i < dt.Columns.Count; i++)
            //{
            //    for (int j = 0; j < dt.Rows.Count; j++)
            //    {
            //        DataRow dr = chartDt.NewRow();
            //        dr["HOUR"] = dt.Columns[dt.Columns[i].Caption];
            //        dr["PROCESSSEGMENTID"] = dt.Rows[j][0];
            //        dr["OUTQTY"] = dt.Rows[j][i];
            //        chartDt.Rows.Add(dr);
            //    }
            //}

            return chartDt;
        }
        #endregion


        #region SetImgCall
        [WebMethod]
        public static string SetImgCall(string sParams)
        {
            string strRtn = string.Empty;

            if (sParams == "202401")
            {
                strRtn = "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-0.png' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-1.png' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-2.png' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-3.png' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-4.png' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-5.png' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-6.png' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-7.png' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202403/ToImage-img-8.png' width='1280' height='300'></li>";
            }
            else
            {
                strRtn = "<li><img src='http://10.208.163.30:81/ppt/202404/ToImage-img-0.png' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202404/ToImage-img-1.png' width='1280' height='300'></li>" +
                "<li><img src='http://10.208.163.30:81/ppt/202404/ToImage-img-2.png' width='1280' height='300'></li>";
            }


            return strRtn;
        }
        #endregion
    }
}