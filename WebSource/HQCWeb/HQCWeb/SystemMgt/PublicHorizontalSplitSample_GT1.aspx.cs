using HQCWeb.FMB_FW;
using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
using DevExpress.Utils;

namespace HQCWeb.SystemMgt
{
    public partial class PublicHorizontalSplitSample_GT1 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        // 비지니스 클래스 작성
        //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();

        #region GRID Setting

        // 그리드에 보여져야할 컬럼 정의
        public string[] arrColumn;
        // 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrColumnCaption;
        // 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrColumnWidth;
        // 그리드 키값 정의
        public string strKeyColumn = "COL_1";
        // 팝업창에 전달할 Param 컬럼 정의
        public string[] arrParams = new string[] { "COL_1" };
        // 그리드 옵션 정의 , 첫번째 값에 따라서 [ P : (Popup) / D : (Detail)] 로 나뉜다.
        public string[] arrOption = new string[] { "P", "풀경로 팝업창 이름 : Popup01.aspx" };
        //public string[] arrOption = new string[] { "D"};
        // 그리드 체크박스 노출여부
        public bool bShowCheckBox = false;
        // 메인 그리드 숨김처리 필드 정의
        public string[] arrHiddenColumn;
        // 그리드 Merge 필드 정의
        public string[] arrMergeColumn;

        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageInit();

           // grid.PopupWidth = hidWidth.Value;
           // grid.PopupHeight = hidHeight.Value;

            if (!IsPostBack)
            {
                SetCon();

                SetGridTitle();

                SetTitle();
            }

            //ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), "fn_gridCall();", true);
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
            arrColumn = new string[] { "COL_1", "COL_2", "COL_3", "COL_4", "COL_5" };
            arrColumnCaption = new string[arrColumn.Length];
            arrColumnWidth = new string[arrColumn.Length];
            arrHiddenColumn = new string[] { "COL_1" };

            for (int i = 0; i < arrColumn.Length; i++)
            {
                // Cell
                arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);
                arrColumnWidth[i] = "100";
            }

            arrHiddenColumn = new string[] { "COL_5" };

            arrMergeColumn = new string[] { "COL_4" };

            //grid.Height = hidGridHeight.Text;
        }
        #endregion

        #region SetGridTitle
        private void SetGridTitle()
        {
            // 단순 조회 컬럼용
            //grid.SetColumn(arrColumn, arrColumnCaption, arrColumnWidth, strKeyColumn, arrHiddenColumn, arrMergeColumn);

            // 팝업 호출
            //grid.SetColumn(arrColumn, arrColumnCaption, arrColumnWidth, strKeyColumn, arrParams, arrOption, arrHiddenColumn, arrMergeColumn);
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

            strDBName = "DBNAME";
            strQueryID = "SQLMAP_NAMESPACE.STATEMENT_ID";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("Param1", "");

            // 비지니스 메서드 호출
            //ds = biz.(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                SetGraphData(ds);

                /// <summary>
                /// 데이터 조회(단순)
                /// </summary>
                /// <param name="arrColumn">컬럼</param>
                /// <param name="arrColumnCaption">컬럼 타이틀</param>
                /// <param name="arrColumnWidth">컬럼 사이즈</param>
                /// <param name="strKeyColumn">그리드 키값</param>
                /// <param name="ds">DataSet</param>
                /// <param name="arrHiddenColumn">숨김컬럼</param>
                /// <param name="arrMergeColumn">Merge 컬럼</param>
                //grid.SetDataListToGrid(arrColumn, arrColumnCaption, arrColumnWidth, strKeyColumn, ds, arrHiddenColumn, arrMergeColumn);
            }
            else
            {
               // ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoData();", true);

              //  grid.SetDataListReset();
            }
        }
        #endregion

        #region SetGraphData
        private void SetGraphData(DataSet ds)
        {

            // 화면에 보여야할 Series 만큼 생성
            // ViewType.Bar, ViewType.Line
            Series series1 = new Series("Hour Out Qty Sum", ViewType.Line);

            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                series1.Points.Add(new SeriesPoint(i.ToString(), new double[] { (i + 1) }));
            }

            series1.LabelsVisibility = DefaultBoolean.True;

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

        #region Excel_Click
        protected void Excel_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataSet dsTitle = new DataSet();

            strDBName = "DBNAME";
            strQueryID = "SQLMAP_NAMESPACE.STATEMENT_ID";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("Param1", "");

            // 비지니스 메서드 호출
            //ds = biz.(strDBName, strQueryID, param);

            string strPageName = string.Empty;
            string strContditionTitle = string.Empty;
            string strContditionValue = string.Empty;

            string strHeaderInfo = string.Empty;

            strPageName = "Default";
            strContditionTitle = "Condition_1" + "," + "Condition_2" + "," + "Condition_3";
            strContditionValue = txtCondi1.Text + "," + txtCondi1.Text + "," + txtCondi1.Text;

            DataTable dt = new DataTable();

            dt.Columns.Add("TITLE", typeof(string));
            dt.Columns.Add("CNT", typeof(string));

            DataRow dr = null;

            dr = dt.NewRow();
            dr["TITLE"] = "TITLE1";
            dr["CNT"] = "1";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["TITLE"] = "TITLE2";
            dr["CNT"] = "3";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["TITLE"] = "TITLE3";
            dr["CNT"] = "1";
            dt.Rows.Add(dr);

            dsTitle.Tables.Add(dt);

            int iArrayCnt = arrColumnCaption.Length;

            string[] arrColumnTitle = new string[iArrayCnt];

            for (int i = 0; i < iArrayCnt; i++)
            {
                arrColumnTitle[i] = Dictionary_Data.SearchDic(arrColumnCaption[i].ToString(), bp.g_language);
            }

            /// <summary>
            /// 엑셀 다운로드
            /// </summary>
            /// <param name="strPageName">페이지명</param>
            /// <param name="strContditionTitle">검색조건 타이틀</param>
            /// <param name="strContditionValue">검색조건 값</param>
            /// <param name="ds">조회된 DataSet</param>
            /// <param name="bMultiRow">상단 타이틀이 2줄이상일 경우 : true // 일반일경우 : false </param>
            /// <param name="dsTitle">멀티 타이틀값</param>

            ee.ExcelDownLoad(strPageName, strContditionTitle, strContditionValue, ds, arrColumnTitle, false, dsTitle);
        }
        #endregion

        #region btnReload_Click
        protected void btnReload_Click(object sender, EventArgs e)
        {
            //WebChartControl.Width = Convert.ToInt32(hidGraphWidth.Value);
            //WebChartControl.Height = Convert.ToInt32(hidGraphHeight.Value);

            SetGraphData();
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
    }
}