using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
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

namespace HQCWeb.Cell.Product
{
    public partial class EQ_001_02 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        // 비지니스 클래스 작성
        Biz.Cell.Product.EQ_001_02 biz = new Biz.Cell.Product.EQ_001_02();

        public string strPOJson = string.Empty;

        #region GRID Setting

        // 그리드에 보여져야할 컬럼 정의
        public string[] arrColumn;
        // 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrColumnCaption;
        // 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrColumnWidth;
        // 그리드 키값 정의
        public string strKeyColumn = "PROCESS_SEGMENT_ID";
        // 팝업창에 전달할 Param 컬럼 정의
        public string[] arrParams = new string[] { "PROCESS_SEGMENT_ID" };
        // 그리드 체크박스 노출여부
        public bool bShowCheckBox = false;
        // 그리드 숨김처리 필드 정의
        public string[] arrHiddenColumn;
        // 그리드 Merge 필드 정의
        public string[] arrMergeColumn;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageInit();

            grid.PopupWidth = hidWidth.Value;
            grid.PopupHeight = hidHeight.Value;

            if (!IsPostBack)
            {
                SetCon();

                SetGridTitle();

                SetTitle();
            }

            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), "fn_gridCall();", true);

        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            DataSet ds = new DataSet();

            txtDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");


            string[] sParam = {
                "9012C"
            };

            ddlShift.Items.Add(new System.Web.UI.WebControls.ListItem("ALL", ""));
            ddlWorkTeam.Items.Add(new System.Web.UI.WebControls.ListItem("ALL", ""));


            ds = biz.GetComCodeList("Shift");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlShift.Items.Add(new System.Web.UI.WebControls.ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                    }
                }
            }

            ds = biz.GetComCodeList("WorkTeam");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlWorkTeam.Items.Add(new System.Web.UI.WebControls.ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                    }
                }
            }

            string[] sParamPo = {
                "9012C"
                , "20231026"
                //, txtDate.Text.Replace("-", "");
            };

            ds = biz.GetRealPOForWorkDate(sParamPo);
            if (ds.Tables.Count > 0)
            {
                strPOJson = DataTableToJson(ds.Tables[0]);
            }
            else
            {
                strPOJson = "[]";
            }
        }
        #endregion

        #region SetPageInit
        private void SetPageInit()
        {
            arrColumn = new string[] { "PROCESS_SEGMENT_ID", "HOUR08", "HOUR09", "HOUR10", "HOUR11", "HOUR12", "HOUR13", "HOUR14", "HOUR15", "HOUR16", "HOUR17", "HOUR18", "HOUR19", "HOUR20", "HOUR21", "HOUR22", "HOUR23", "HOUR00", "HOUR01", "HOUR02", "HOUR03", "HOUR04", "HOUR05", "HOUR06", "HOUR07" };
            arrColumnCaption = new string[arrColumn.Length];
            arrColumnWidth = new string[arrColumn.Length];

            for (int i = 0; i < arrColumn.Length; i++)
            {
                arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);
                arrColumnWidth[i] = "100";

                //if (i == 0)
                //{
                //    arrColumnWidth[i] = "100";
                //}
                //else
                //{
                //    arrColumnWidth[i] = "100";
                //}
            }
        }
        #endregion

        #region SetGridTitle
        private void SetGridTitle()
        {
            // 단순 조회 컬럼용
            grid.SetColumn(arrColumn, arrColumnCaption, arrColumnWidth, strKeyColumn, arrHiddenColumn, arrMergeColumn);
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            // CELL과 MODULE 중 하나를 선택하여 다국어 처리

            // CELL
            lbWorkDate.Text     = Dictionary_Data.SearchDic("W_WORKDATE", bp.g_language);
            lbShift.Text        = Dictionary_Data.SearchDic("W_SHIFT", bp.g_language);
            lbWorkTeam.Text     = Dictionary_Data.SearchDic("W_WORKTEAM", bp.g_language);
            lbPO.Text           = Dictionary_Data.SearchDic("W_PO", bp.g_language);

        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();

            // 검색조건 생성
            string[] sParam = {
                "9012C"
                //, txtDate.Text.Replace("-", "");
                , "20231026"
                , ddlShift.SelectedValue
                , ddlWorkTeam.SelectedValue
                , txtPOHidden.Text
            };

            // 비지니스 메서드 호출
            ds = biz.GetOutCountList(sParam);

            if (ds.Tables.Count > 0)
            {
                string strRtnMsg = ds.Tables[0].Rows[0][0].ToString();

                if (strRtnMsg == "Tibco Service Error")
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoConnectMsg();", true);
                }
                else
                {
                    bp.g_GridDataSource = ds;

                    SetGraphData(ds);

                    /// <summary>
                    /// 데이터 조회(단순)
                    /// </summary>
                    /// <param name="arrColumn">컬럼</param>
                    /// <param name="arrColumnCaption">컬럼 타이틀</param>
                    /// <param name="arrColumnWidth">컬럼 사이즈</param>
                    /// <param name="strKeyColumn">그리드 키값</param>
                    /// <param name="ds">DataSet</param>
                    /// <param name="arrHiddenColumn">숨김처리 컬럼</param>
                    /// <param name="arrMergeColumn">Merge처리 컬럼</param>
                    grid.SetDataListToGrid(arrColumn, arrColumnCaption, arrColumnWidth, strKeyColumn, ds, arrHiddenColumn, arrMergeColumn);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoData();", true);

                bp.g_GridDataSource = null;

                grid.SetDataListReset();
            }
        }
        #endregion

        #region SetGraphData
        private void SetGraphData(DataSet ds)
        {
            chart_04.Series.Clear();

            string strColName = string.Empty;

            DataTable baseDt = ds.Tables[0].Copy();
            DataTable chartDt = createChartDt(baseDt);

            Dictionary<string, Series> seriesList = new Dictionary<string, Series>();

            foreach (DataRow row in chartDt.Rows)
            {
                string prcesssegmentid = row["PROCESSSEGMENTID"].ToString();

                string hour = row["HOUR"].ToString();

                string qty = row["OUTQTY"].ToString();

                Series series;

                if (seriesList.TryGetValue(prcesssegmentid, out series) == false)
                {
                    seriesList.Add(prcesssegmentid, series = new Series(prcesssegmentid, ViewType.Bar));
                    series.LabelsVisibility = DefaultBoolean.True;
                }

                chart_04.Series.Add(series);

                SeriesPoint point = new SeriesPoint(hour, Convert.ToInt64(qty));
                series.Points.Add(point);
            }

            ChartTitle title = new ChartTitle();
            title.Text = "MES 전체공정 시간별 생산량";

            chart_04.Titles.Add(title);
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            chart_04.Width = Convert.ToInt32(5000);
            chart_04.Height = Convert.ToInt32(hidGraphHeight.Value);

            GetData();
        }
        #endregion

        #region btnReload_Click
        protected void btnReload_Click(object sender, EventArgs e)
        {
            chart_04.Width = Convert.ToInt32(5000);
            chart_04.Height = Convert.ToInt32(hidGraphHeight.Value);
        }
        #endregion

        #region DataTableToJson
        public static string DataTableToJson(DataTable ds)
        {
            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            serializer.MaxJsonLength = 2147483647;
            List<Dictionary<string, object>> listRows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in ds.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in ds.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                listRows.Add(row);
            }
            return serializer.Serialize(listRows);
        }

        #endregion

        #region createChartDt
        private DataTable createChartDt(DataTable dt)
        {
            DataTable chartDt = new DataTable();

            chartDt.Columns.Add("HOUR");
            chartDt.Columns.Add("PROCESSSEGMENTID");
            chartDt.Columns.Add("OUTQTY", typeof(decimal));

            for (int i = 1; i < dt.Columns.Count; i++)
            {
                for (int j = 0; j < dt.Rows.Count; j++)
                {
                    DataRow dr = chartDt.NewRow();
                    dr["HOUR"] = dt.Columns[dt.Columns[i].Caption];
                    dr["PROCESSSEGMENTID"] = dt.Rows[j][0];
                    dr["OUTQTY"] = dt.Rows[j][i];
                    chartDt.Rows.Add(dr);
                }
            }

            return chartDt;
        }
        #endregion
    }
}