using DevExpress.Utils;
using DevExpress.Web;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
using DocumentFormat.OpenXml.Wordprocessing;
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
    public partial class OM_001_50 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        // 비지니스 클래스 작성
        Biz.Cell.Product.OM_001_50 biz = new Biz.Cell.Product.OM_001_50();

        public string strPOTypeJson = string.Empty;
        public string strPOJson = string.Empty;

        public static string[] strCoustomFiled;


        #region GRID Setting

        // 그리드에 보여져야할 컬럼 정의
        public string[] arrColumn;
        // 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrColumnCaption;
        // 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrColumnWidth;
        // 그리드 키값 정의
        public string strKeyColumn = "ROWRANK";
        // 팝업창에 전달할 Param 컬럼 정의
        public string[] arrParams = new string[] { "ROWRANK" };
        // 그리드 옵션 정의 , 첫번째 값에 따라서 [ P : (Popup) / D : (Detail)] 로 나뉜다.
        public string[] arrOption = new string[] { "P", "풀경로 팝업창 이름 : Popup01.aspx" };
        //public string[] arrOption = new string[] { "D"};
        // 그리드 체크박스 노출여부
        public bool bShowCheckBox = false;

        // 그리드에서 숨겨야할 컬럼 정의
        public string[] arrHiddenColumn;

        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageInit();

            //grid.PopupWidth = hidWidth.Value;
            //grid.PopupHeight = hidHeight.Value;

            if (!IsPostBack)
            {
                SetCon();

                SetGridTitle();

                SetTitle();
            }

            if (bp.g_GridDataSource != null)
            {
                grid.DataSource = bp.g_GridDataSource;
                grid.DataBind();
            }

            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), "fn_gridCall();", true);
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            DataSet ds = new DataSet();

            string[] sParam = {
                "9012C"
            };

            txtDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

            ddlShift.Items.Add(new System.Web.UI.WebControls.ListItem("ALL", ""));
            
            ds = biz.GetOperationList(sParam);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlOperation.Items.Add(new System.Web.UI.WebControls.ListItem(ds.Tables[0].Rows[i]["PROCESS_SEGMENT_NAME"].ToString(), ds.Tables[0].Rows[i]["PROCESS_SEGMENT_ID"].ToString()));
                    }
                }
            }

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

            ds = biz.GetComCodeList("SearchType");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlSearchType.Items.Add(new System.Web.UI.WebControls.ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                    }
                }
            }

            ds = biz.GetPOTypeList(sParam);
            if (ds.Tables.Count > 0)
            {
                strPOTypeJson = DataTableToJson(ds.Tables[0]);
            }
            else
            {
                strPOTypeJson = "[]";
            }

            strPOJson = "[]";

        }
        #endregion

        #region SetPageInit
        private void SetPageInit()
        {
            arrColumn = new string[] { "ROWRANK", "EQP_ID", "EQP_NAME", "BASELINE", "LSL", "HOUR08", "HOUR09", "HOUR10", "HOUR11", "HOUR12", "HOUR13", "HOUR14", "HOUR15", "HOUR16", "HOUR17", "HOUR18", "HOUR19", "HOUR20", "HOUR21", "HOUR22", "HOUR23", "HOUR00", "HOUR01", "HOUR02", "HOUR03", "HOUR04", "HOUR05", "HOUR06", "HOUR07", "SUMQTY" };
            arrHiddenColumn = new string[] { "ROWRANK", "EQP_ID" };
            arrColumnCaption = new string[arrColumn.Length];
            arrColumnWidth = new string[arrColumn.Length];

            for (int i = 0; i < arrColumn.Length; i++)
            {
                //if (i < 3)
                //{
                //    arrColumnCaption[i] = Dictionary_Data.SearchDic("W_" + arrColumn[i].ToString(), bp.g_language);
                //}
                //else {
                //    arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);
                //}

                arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);

                if (i == 2) {
                    arrColumnWidth[i] = "160";
                }
                else
                {
                    arrColumnWidth[i] = "100";
                }
            }

            //grid.Settings.VerticalScrollableHeight = Convert.ToInt32(hidGridHeight.Text);

        }
        #endregion

        #region SetGridTitle
        private void SetGridTitle()
        {
            // 단순 조회 컬럼용
            //grid.SetColumn(arrColumn, arrColumnCaption, arrColumnWidth, strKeyColumn, arrHiddenColumn);

            grid.Columns.Clear();

            grid.KeyFieldName = strKeyColumn;

            string strCaption = string.Empty;

            strCoustomFiled = arrColumn;

            if (arrHiddenColumn.Length > 0)
            {
                for (int i = 0; i < arrColumn.Length; i++)
                {

                    if (bp.g_plant == "9011M")
                    {
                        strCaption = Dictionary_Data.SearchDic(arrColumnCaption[i].ToString(), bp.g_language);
                    }
                    else
                    {
                        strCaption = Dictionary_Data.SearchDic(arrColumnCaption[i].ToString(), bp.g_language);
                    }

                    GridViewDataTextColumn gridCol = new GridViewDataTextColumn();
                    gridCol.FieldName = arrColumn[i].ToString();
                    gridCol.VisibleIndex = i;

                    for (int j = 0; j < arrHiddenColumn.Length; j++)
                    {
                        if (arrHiddenColumn[j].ToString() == arrColumn[i].ToString())
                        {
                            gridCol.Visible = false;
                        }
                    }

                    gridCol.PropertiesTextEdit.DisplayFormatString = "#,##0";
                    gridCol.Caption = strCaption;

                    gridCol.Width = Convert.ToInt32(arrColumnWidth[i].ToString());
                    grid.Columns.Add(gridCol);
                }
            }
            else
            {
                for (int i = 0; i < arrColumn.Length; i++)
                {
                    if (bp.g_plant == "9011M")
                    {
                        strCaption = Dictionary_Data.SearchDic(arrColumnCaption[i].ToString(), bp.g_language);
                    }
                    else
                    {
                        strCaption = Dictionary_Data.SearchDic(arrColumnCaption[i].ToString(), bp.g_language);
                    }

                    GridViewDataTextColumn gridCol = new GridViewDataTextColumn();
                    gridCol.FieldName = arrColumn[i].ToString();
                    gridCol.VisibleIndex = i;
                    gridCol.PropertiesTextEdit.DisplayFormatString = "#,##0";
                    gridCol.Caption = strCaption;
                    gridCol.Width = Convert.ToInt32(arrColumnWidth[i].ToString());
                    grid.Columns.Add(gridCol);
                }
            }


        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbWorkDate.Text     = Dictionary_Data.SearchDic("W_WORK_DATE", bp.g_language);
            lbShift.Text        = Dictionary_Data.SearchDic("W_SHIFT", bp.g_language);
            lbOperation.Text    = Dictionary_Data.SearchDic("W_OPERATION", bp.g_language);
            lbPOType.Text       = Dictionary_Data.SearchDic("W_POTYPE", bp.g_language);
            lbPO.Text           = Dictionary_Data.SearchDic("W_PO", bp.g_language);
            lbSearchType.Text   = Dictionary_Data.SearchDic("W_SEARCHTYPE", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();
            DataTable dtCopy = new DataTable();

            DataTable dtHourSum = new DataTable();

            string strDate = string.Empty;

            strDate = txtDate.Text.Replace("-", "");
            strDate = "20231026";

            // 검색조건 생성
                //bp.g_plant
            string[] sParam = {
                "9012C"
                , strDate
                , ddlOperation.SelectedValue
                , txtPOType.Text
                , txtPOHidden.Text
                , ddlShift.SelectedValue
            };

            // 비지니스 메서드 호출
            if (ddlSearchType.SelectedValue == "1") {
                ds = biz.GetHourTargetInfoList(sParam);

                dtCopy = ds.Tables[0].Copy();

                foreach (DataRow dr in dtCopy.Rows)
                {
                    //SUM MOVE Row의 경우 19-20시의 수량을 보여준다.
                    if (Convert.ToString(dr["EQP_ID"]).Equals("SUM_MOVE"))
                    {
                        dr["SUMQTY"] = dr["HOUR19"];
                    }

                    //BottomInfo의 SUMQTY는 공백처리한다.
                    if (Convert.ToString(dr["EQP_ID"]).Equals("ACTIVE_TOOL")
                        || Convert.ToString(dr["EQP_ID"]).Equals("HOURLY_TARGET")
                        || Convert.ToString(dr["EQP_ID"]).Equals("HOURLY_TARGET_ADD")
                        || Convert.ToString(dr["EQP_ID"]).Equals("TARGET_DIFF_QTY")
                        || Convert.ToString(dr["EQP_ID"]).Equals("TARGET_DIFF_QTY_ADD"))
                    {
                        dr["SUMQTY"] = DBNull.Value;
                    }


                    if (Convert.ToString(dr["EQP_ID"]).Equals("MOVE_HOURLY")
                        || Convert.ToString(dr["EQP_ID"]).Equals("SUM_MOVE")
                        || Convert.ToString(dr["EQP_ID"]).Equals("ACTIVE_TOOL")
                        || Convert.ToString(dr["EQP_ID"]).Equals("HOURLY_TARGET")
                        || Convert.ToString(dr["EQP_ID"]).Equals("HOURLY_TARGET_ADD")
                        || Convert.ToString(dr["EQP_ID"]).Equals("TARGET_DIFF_QTY")
                        || Convert.ToString(dr["EQP_ID"]).Equals("TARGET_DIFF_QTY_ADD"))
                    {
                        dr["BASELINE"] = DBNull.Value;
                        dr["LSL"] = DBNull.Value;
                    }

                }

                ds.Tables.Clear();

                ds.Tables.Add(dtCopy);

                if (rb01.Checked)
                {
                    rb01_CheckedChanged(null, null);
                }

                if (rb02.Checked)
                {
                    rb02_CheckedChanged(null, null);
                }

                if (rb03.Checked)
                {
                    rb03_CheckedChanged(null, null);
                }


                SetGraphData(ds);

            }
            else if (ddlSearchType.SelectedValue == "2")
            {
                if (rb01.Checked)
                {
                    rb01_CheckedChanged(null, null);
                }

                if (rb02.Checked)
                {
                    rb02_CheckedChanged(null, null);
                }

                if (rb03.Checked)
                {
                    rb03_CheckedChanged(null, null);
                }

                ds = biz.GetInOutDiffQtyList(sParam);

                SetGraphData(ds);

            }
            else {

                if (rb01.Checked)
                {
                    rb01_CheckedChanged(null, null);
                }

                if (rb02.Checked)
                {
                    rb02_CheckedChanged(null, null);
                }

                if (rb03.Checked)
                {
                    rb03_CheckedChanged(null, null);
                }

                ds = biz.GetHourINQTYInfoList(sParam);

                SetGraphData(ds);
            }

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

                    //SetGraphData(ds);

                    /// <summary>
                    /// 데이터 조회(단순)
                    /// </summary>
                    /// <param name="arrColumn">컬럼</param>
                    /// <param name="arrColumnCaption">컬럼 타이틀</param>
                    /// <param name="arrColumnWidth">컬럼 사이즈</param>
                    /// <param name="strKeyColumn">그리드 키값</param>
                    /// <param name="ds">DataSet</param>
                    //grid.SetDataListToGrid(arrColumn, arrColumnCaption, arrColumnWidth, strKeyColumn, ds, arrHiddenColumn);

                    grid.DataSource = ds;
                    grid.DataBind();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoData();", true);

                bp.g_GridDataSource = null;

                //grid.SetDataListReset();

                grid.DataSource = null;
                grid.DataBind();
            }
        }
        #endregion

        #region SetGraphData
        private void SetGraphData(DataSet ds)
        {
            chart_01.Series.Clear();
            chart_02.Series.Clear();
            chart_03.Series.Clear();

            string strColName = string.Empty;

            #region Hourly
            DataRow[] drHourly = ds.Tables[0].Select("ROWRANK = '1'");

            // 화면에 보여야할 Series 만큼 생성
            // ViewType.Bar, ViewType.Line
            Series sHourly = new Series("Hour Out Qty Sum", ViewType.Bar);

            for (int j = 0; j < drHourly.Length; j++)
            {
                for (int i = 8; i < 24; i++)
                {
                    if (i < 10)
                    {
                        strColName = "HOUR0" + i.ToString();
                    }
                    else
                    {
                        strColName = "HOUR" + i.ToString();
                    }

                    sHourly.Points.Add(new SeriesPoint(strColName, new double[] { Convert.ToInt32(drHourly[j][strColName].ToString()) }));
                }

                for (int i = 0; i < 8; i++)
                {
                    strColName = "HOUR0" + i.ToString();

                    sHourly.Points.Add(new SeriesPoint(strColName, new double[] { Convert.ToInt32(drHourly[j][strColName].ToString()) }));
                }
            }

            sHourly.LabelsVisibility = DefaultBoolean.True;

            // Add the series to the chart.
            chart_01.Series.Add(sHourly);
            #endregion

            if (ddlSearchType.SelectedValue == "1") {
                #region SumHour
                DataRow[] drSumHour_01 = ds.Tables[0].Select("ROWRANK = '5'");
                DataRow[] drSumHour_02 = ds.Tables[0].Select("ROWRANK = '6'");

                Series sSumHour_01 = new Series("OutAccumulate", ViewType.Line);
                for (int j = 0; j < drSumHour_01.Length; j++)
                {
                    for (int i = 8; i < 24; i++)
                    {
                        if (i < 10)
                        {
                            strColName = "HOUR0" + i.ToString();
                        }
                        else
                        {
                            strColName = "HOUR" + i.ToString();
                        }

                        sSumHour_01.Points.Add(new SeriesPoint(strColName, new double[] { Convert.ToInt32(drSumHour_01[j][strColName].ToString()) }));
                    }

                    for (int i = 0; i < 8; i++)
                    {
                        strColName = "HOUR0" + i.ToString();

                        sSumHour_01.Points.Add(new SeriesPoint(strColName, new double[] { Convert.ToInt32(drSumHour_01[j][strColName].ToString()) }));
                    }
                }


                Series sSumHour_02 = new Series("TargetAccumulate", ViewType.Line);
                for (int j = 0; j < drSumHour_02.Length; j++)
                {
                    for (int i = 8; i < 24; i++)
                    {
                        if (i < 10)
                        {
                            strColName = "HOUR0" + i.ToString();
                        }
                        else
                        {
                            strColName = "HOUR" + i.ToString();
                        }

                        sSumHour_02.Points.Add(new SeriesPoint(strColName, new double[] { Convert.ToInt32(drSumHour_02[j][strColName].ToString()) }));
                    }

                    for (int i = 0; i < 8; i++)
                    {
                        strColName = "HOUR0" + i.ToString();

                        sSumHour_02.Points.Add(new SeriesPoint(strColName, new double[] { Convert.ToInt32(drSumHour_02[j][strColName].ToString()) }));
                    }
                }


                sSumHour_01.LabelsVisibility = DefaultBoolean.True;
                sSumHour_02.LabelsVisibility = DefaultBoolean.True;

                // Add the series to the chart.
                chart_02.Series.Add(sSumHour_01);
                chart_02.Series.Add(sSumHour_02);
                #endregion
            }

            if (ddlSearchType.SelectedValue == "3")
            {
                #region SumHour
                int iRow = ds.Tables[0].Rows.Count - 3;

                DataRow drSumHour_01 = ds.Tables[0].Rows[1];
                DataRow drSumHour_02 = ds.Tables[0].Rows[iRow];

                Series sSumHour_01 = new Series("OutAccumulate", ViewType.Line);
                Series sSumHour_02 = new Series("TargetAccumulate", ViewType.Line);

                for (int i = 8; i < 24; i++)
                {
                    if (i < 10)
                    {
                        strColName = "HOUR0" + i.ToString();
                    }
                    else
                    {
                        strColName = "HOUR" + i.ToString();
                    }

                    sSumHour_01.Points.Add(new SeriesPoint(strColName, new double[] { Convert.ToInt32(drSumHour_01[strColName].ToString()) }));
                    sSumHour_02.Points.Add(new SeriesPoint(strColName, new double[] { Convert.ToInt32(drSumHour_02[strColName].ToString()) }));
                }

                for (int i = 0; i < 8; i++)
                {
                    strColName = "HOUR0" + i.ToString();

                    sSumHour_01.Points.Add(new SeriesPoint(strColName, new double[] { Convert.ToInt32(drSumHour_01[strColName].ToString()) }));
                    sSumHour_02.Points.Add(new SeriesPoint(strColName, new double[] { Convert.ToInt32(drSumHour_02[strColName].ToString()) }));
                }

                sSumHour_01.LabelsVisibility = DefaultBoolean.True;
                sSumHour_02.LabelsVisibility = DefaultBoolean.True;

                // Add the series to the chart.
                chart_02.Series.Add(sSumHour_01);
                chart_02.Series.Add(sSumHour_02);
                #endregion
            }

            #region DRUK Tools
            Series sDurkTools = new Series("EquipmentSum", ViewType.Bar);

            for (int i = 2; i < ds.Tables[0].Rows.Count - 5; i++)
            {
                sDurkTools.Points.Add(new SeriesPoint(ds.Tables[0].Rows[i]["EQP_NAME"].ToString(), new double[] { Convert.ToInt32(ds.Tables[0].Rows[i]["SUMQTY"].ToString()) }));
            }

            sDurkTools.LabelsVisibility = DefaultBoolean.True;
            chart_03.Series.Add(sDurkTools);
            #endregion
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            chart_01.Width = Convert.ToInt32(hidGraphWidth.Text);
            chart_01.Height = Convert.ToInt32(hidGraphHeight.Text);

            chart_02.Width = Convert.ToInt32(hidGraphWidth.Text);
            chart_02.Height = Convert.ToInt32(hidGraphHeight.Text);

            chart_03.Width = Convert.ToInt32(hidGraphWidth.Text);
            chart_03.Height = Convert.ToInt32(hidGraphHeight.Text);


            GetData();
        }
        #endregion

        #region btnReload_Click
        protected void btnReload_Click(object sender, EventArgs e)
        {
            //chart_01.Width = Convert.ToInt32(hidGraphWidth.Value);
            //chart_01.Height = Convert.ToInt32(hidGraphHeight.Value);

            //chart_02.Width = Convert.ToInt32(hidGraphWidth.Value);
            //chart_02.Height = Convert.ToInt32(hidGraphHeight.Value);

            //chart_03.Width = Convert.ToInt32(hidGraphWidth.Value);
            //chart_03.Height = Convert.ToInt32(hidGraphHeight.Value);


            chart_01.Width = Convert.ToInt32(hidGraphWidth.Text);
            chart_01.Height = Convert.ToInt32(hidGraphHeight.Text);

            chart_02.Width = Convert.ToInt32(hidGraphWidth.Text);
            chart_02.Height = Convert.ToInt32(hidGraphHeight.Text);

            chart_03.Width = Convert.ToInt32(hidGraphWidth.Text);
            chart_03.Height = Convert.ToInt32(hidGraphHeight.Text);
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

        #region SetPOListCondition
        private void SetPOListCondition() {
            DataSet ds = new DataSet();

            string[] sParamPOList = {
                "9012C"
                //, txtDate.Text.Replace("-", "")
                , "20231026"
                , ddlOperation.SelectedValue
                , txtPOType.Text
            };

            ds = biz.GetPOList(sParamPOList);
            if (ds.Tables.Count > 0)
            {
                strPOJson = DataTableToJson(ds.Tables[0]);
            }
            else
            {
                strPOJson = "[]";
            }

            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_SetPODDL(" + strPOJson + "); ", true);
        }
        #endregion

        #region ddlOperation_SelectedIndexChanged
        protected void ddlOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetPOListCondition();
        }
        #endregion

        #region btnFunctionCall_Click
        protected void btnFunctionCall_Click(object sender, EventArgs e)
        {
            //chart_01.Width = Convert.ToInt32(hidGraphWidth.Value);
            //chart_01.Height = Convert.ToInt32(hidGraphHeight.Value);

            //chart_02.Width = Convert.ToInt32(hidGraphWidth.Value);
            //chart_02.Height = Convert.ToInt32(hidGraphHeight.Value);

            //chart_03.Width = Convert.ToInt32(hidGraphWidth.Value);
            //chart_03.Height = Convert.ToInt32(hidGraphHeight.Value);

            chart_01.Width = Convert.ToInt32(hidGraphWidth.Text);
            chart_01.Height = Convert.ToInt32(hidGraphHeight.Text);

            chart_02.Width = Convert.ToInt32(hidGraphWidth.Text);
            chart_02.Height = Convert.ToInt32(hidGraphHeight.Text);

            chart_03.Width = Convert.ToInt32(hidGraphWidth.Text);
            chart_03.Height = Convert.ToInt32(hidGraphHeight.Text);

            grid.Settings.VerticalScrollableHeight = Convert.ToInt32(hidGridHeight.Text);

            SetPOListCondition();
        }
        #endregion

        #region ddlSearchType_SelectedIndexChanged
        protected void ddlSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlSearchType.SelectedValue == "2" || ddlSearchType.SelectedValue == "3")
            {
                arrColumn = new string[] { "ROWRANK", "EQP_ID", "EQP_NAME", "HOUR08", "HOUR09", "HOUR10", "HOUR11", "HOUR12", "HOUR13", "HOUR14", "HOUR15", "HOUR16", "HOUR17", "HOUR18", "HOUR19", "HOUR20", "HOUR21", "HOUR22", "HOUR23", "HOUR00", "HOUR01", "HOUR02", "HOUR03", "HOUR04", "HOUR05", "HOUR06", "HOUR07", "SUMQTY" };
                arrHiddenColumn = new string[] { "ROWRANK", "EQP_ID" };
                arrColumnCaption = new string[arrColumn.Length];
                arrColumnWidth = new string[arrColumn.Length];

                for (int i = 0; i < arrColumn.Length; i++)
                {
                    //if (i < 3)
                    //{
                    //    arrColumnCaption[i] = Dictionary_Data.SearchDic("W_" + arrColumn[i].ToString(), bp.g_language);
                    //}
                    //else {
                    //    arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);
                    //}

                    arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);
                    arrColumnWidth[i] = "100";
                }
            }

            SetGridTitle();

        }
        #endregion

        #region grid_HtmlRowPrepared
        protected void grid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            string strROWRANK = string.Empty;

            #region ddlSearchType.SelectedValue == "1"
            if (ddlSearchType.SelectedValue == "1")
            {
                int iBaseLine = 0;
                int iLSL = 0;

                strROWRANK = e.GetValue("ROWRANK").ToString();

                if (strROWRANK == "1" || strROWRANK == "2" || strROWRANK == "8")
                {
                    for (int columnIndex = 0; columnIndex < 3; columnIndex++)
                    {
                        e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Yellow");
                    }

                }
                else if (strROWRANK == "4" || strROWRANK == "5" || strROWRANK == "6")
                {
                    for (int columnIndex = 0; columnIndex < 3; columnIndex++)
                    {
                        e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "LightGray");
                    }

                    for (int columnIndex = 3; columnIndex < e.Row.Cells.Count - 2; columnIndex++)
                    {
                        e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Silver");
                    }
                }
                else if (strROWRANK == "7")
                {
                    for (int columnIndex = 0; columnIndex < 3; columnIndex++)
                    {
                        e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "LightGray");
                    }


                    for (int columnIndex = 3; columnIndex < e.Row.Cells.Count; columnIndex++)
                    {
                        e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.Color, "Red");
                    }
                }
                else if (strROWRANK == "3")
                {
                    for (int columnIndex = 0; columnIndex < 3; columnIndex++)
                    {
                        e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "LightGray");
                    }

                    iBaseLine = Convert.ToInt32(e.GetValue("BASELINE").ToString().Replace(",", ""));
                    iLSL = Convert.ToInt32(e.GetValue("LSL").ToString().Replace(",", ""));


                    int iColumnIndex = 3;

                    // /*

                    for (int j = 8; j < 24; j++)
                    {
                        string strColName = string.Empty;

                        if (j < 10)
                        {
                            strColName = "HOUR" + "0" + j.ToString();
                        }
                        else
                        {
                            strColName = "HOUR" + j.ToString();
                        }

                        int iCellValue = Convert.ToInt32(e.GetValue(strColName).ToString().Replace(",", ""));

                        if (iCellValue >= iLSL && iCellValue <= iBaseLine)
                        {
                            e.Row.Cells[iColumnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Yellow");
                        }

                        if (iCellValue < iLSL)
                        {
                            e.Row.Cells[iColumnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Red");
                        }

                        if (iCellValue > iBaseLine)
                        {
                            e.Row.Cells[iColumnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "LimeGreen");
                        }

                        if (iCellValue == 0)
                        {
                            e.Row.Cells[iColumnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "White");
                        }

                        iColumnIndex++;
                    }


                    for (int j = 0; j < 8; j++)
                    {
                        string strColName = string.Empty;

                        strColName = "HOUR0" + j.ToString();


                        int iCellValue = Convert.ToInt32(e.GetValue(strColName).ToString().Replace(",", ""));

                        if (iCellValue >= iLSL && iCellValue <= iBaseLine)
                        {
                            e.Row.Cells[iColumnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Yellow");
                        }

                        if (iCellValue < iLSL)
                        {
                            e.Row.Cells[iColumnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Red");
                        }

                        if (iCellValue > iBaseLine)
                        {
                            e.Row.Cells[iColumnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "LimeGreen");
                        }

                        if (iCellValue == 0)
                        {
                            e.Row.Cells[iColumnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "White");
                        }


                        iColumnIndex++;
                    }

                    // */
                }
            }
            #endregion

            #region ddlSearchType.SelectedValue == "2"
            if (ddlSearchType.SelectedValue == "2")
            {
                strROWRANK = e.GetValue("ROWRANK").ToString();

                if (strROWRANK == "1")
                {
                    e.Row.Cells[0].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Yellow");
                }

                if (strROWRANK == "3")
                {
                    e.Row.Cells[0].Style.Add(HtmlTextWriterStyle.BackgroundColor, "LightGray");
                }

            }
            #endregion

            #region ddlSearchType.SelectedValue == "3"
            if (ddlSearchType.SelectedValue == "3")
            {
                strROWRANK = e.GetValue("ROWRANK").ToString();

                if (strROWRANK == "1")
                {
                    e.Row.Cells[0].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Yellow");
                }

                if (strROWRANK == "3")
                {
                    e.Row.Cells[0].Style.Add(HtmlTextWriterStyle.BackgroundColor, "LightGray");
                }
            }
            #endregion
        }
        #endregion

        #region grid_HtmlDataCellPrepared
        protected void grid_HtmlDataCellPrepared(object sender, DevExpress.Web.ASPxGridViewTableDataCellEventArgs e)
        {
            string strROWRANK = string.Empty;
            int iBaseLine = 0;
            int iLSL = 0;

            if (e.DataColumn.FieldName == "BASELINE")
            {
                if (e.CellValue.ToString() != "")
                {
                    iBaseLine = Convert.ToInt32(e.CellValue);
                }
            }

            if (e.DataColumn.FieldName == "LSL")
            {
                if (e.CellValue.ToString() != "")
                {
                    iLSL = Convert.ToInt32(e.CellValue);
                }
            }

            //if (e.DataColumn.FieldName != "HOUR08") return;

            if (iBaseLine > 0 && iLSL > 0) {
                int iCellValue = Convert.ToInt32(e.CellValue);

                if (iCellValue >= iLSL && iCellValue <= iBaseLine)
                {
                    //e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.BorderColor, "Yellow");
                    //e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Yellow");

                    e.Cell.BackColor = System.Drawing.Color.Yellow;
                }

                if (iCellValue < iLSL)
                {
                    //e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.BorderColor, "Red");
                }

                if (iCellValue > iBaseLine)
                {
                    //e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.BorderColor, "LimeGreen");
                }

                if (iCellValue == 0)
                {
                    //e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.BorderColor, "White");
                }
            }
        }



        #endregion

        #region rb01_CheckedChanged
        protected void rb01_CheckedChanged(object sender, EventArgs e)
        {
            chart_01.Visible = true;
            chart_02.Visible = false;
            chart_03.Visible = false;

            chart_01.Width = Convert.ToInt32(hidGraphWidth.Text);
            chart_01.Height = Convert.ToInt32(hidGraphHeight.Text);

            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_ConditionSH('S');", true);
        }
        #endregion

        #region rb02_CheckedChanged
        protected void rb02_CheckedChanged(object sender, EventArgs e)
        {
            chart_01.Visible = false;
            chart_02.Visible = true;
            chart_03.Visible = false;

            chart_02.Width = Convert.ToInt32(hidGraphWidth.Text);
            chart_02.Height = Convert.ToInt32(hidGraphHeight.Text);

            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_ConditionSH('H');", true);
        }
        #endregion

        #region rb03_CheckedChanged
        protected void rb03_CheckedChanged(object sender, EventArgs e)
        {
            chart_01.Visible = false;
            chart_02.Visible = false;
            chart_03.Visible = true;

            chart_03.Width = Convert.ToInt32(hidGraphWidth.Text);
            chart_03.Height = Convert.ToInt32(hidGraphHeight.Text);

            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_ConditionSH('H');", true);
        }
        #endregion
    }
}