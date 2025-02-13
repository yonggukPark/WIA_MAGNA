using DevExpress.Data.Linq.Design;
using DevExpress.Utils;
using DevExpress.Web;
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

namespace HQCWeb.Cell.Alarm
{
    public partial class EQ_013_01 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        
        // 비지니스 클래스 작성
        Biz.Cell.Alarm.EQ_013_01 biz = new Biz.Cell.Alarm.EQ_013_01();

        public static string[] strCoustomFiled;

        public int iLowColor = 0;
        public int iHighColor = 0;

        

        #region GRID Setting
        // 그리드에 보여져야할 컬럼 정의
        public string[] arrColumn;
        // 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrColumnCaption;
        // 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrColumnWidth;
        // 그리드 키값 정의
        public string strKeyColumn = "SEQ";
        // 그리드 키값 정의
        public string[] arrHiddenColumn;
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
            ddlShift.Items.Clear();
            ddlOperation.Items.Clear();
            ddlAlarmClass.Items.Clear();
            ddlEquipmentType.Items.Clear();
            ddlAlarmType.Items.Clear();
            ddlAlarmLevel.Items.Clear();
            ddlAlarmGrade.Items.Clear();
            ddlAlarmCommon.Items.Clear();
            ddlAlarmHead.Items.Clear();
            ddlAlarmCamera.Items.Clear();

            //txtDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtDate.Text = "2023-12-21";

            DataSet ds = new DataSet();

            ddlShift.Items.Add(new ListItem("ALL", ""));
            
            ddlAlarmClass.Items.Add(new ListItem("ALL", ""));
            ddlEquipmentType.Items.Add(new ListItem("ALL", ""));

            ddlAlarmType.Items.Add(new ListItem("ALL", ""));
            ddlAlarmLevel.Items.Add(new ListItem("ALL", ""));
            ddlAlarmGrade.Items.Add(new ListItem("ALL", ""));

            ddlAlarmCommon.Items.Add(new ListItem("ALL", ""));
            ddlAlarmHead.Items.Add(new ListItem("ALL", ""));
            ddlAlarmCamera.Items.Add(new ListItem("ALL", ""));

            ds = biz.GetComCodeList("Shift");
            if (ds.Tables.Count > 0) {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {
                    ddlShift.Items.Add(new ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                }
            }

            //SetTime();

            string[] sParams = { 
                //bp.g_plant
                "9012C"
            };

            ds = biz.GetOperationList(sParams);
            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlOperation.Items.Add(new ListItem(ds.Tables[0].Rows[i]["PROCESS_SEGMENT_ID"].ToString(), ds.Tables[0].Rows[i]["PROCESS_SEGMENT_ID"].ToString()));
                }
            }

            ds = biz.GetComCodeList("AlarmClass");
            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlAlarmClass.Items.Add(new ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                }

                ddlAlarmClass.SelectedIndex = 1;
            }

            ds = biz.GetEQPTypeList();
            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlEquipmentType.Items.Add(new ListItem(ds.Tables[0].Rows[i]["EQP_TYPE"].ToString(), ds.Tables[0].Rows[i]["EQP_TYPE"].ToString()));
                }
            }

            ds = biz.GetAlarmTypeList();
            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlAlarmType.Items.Add(new ListItem(ds.Tables[0].Rows[i]["ALARM_TYPE"].ToString(), ds.Tables[0].Rows[i]["ALARM_TYPE"].ToString()));
                }
            }

            ds = biz.GetAlarmLevelList();
            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlAlarmLevel.Items.Add(new ListItem(ds.Tables[0].Rows[i]["ALARM_LEVEL"].ToString(), ds.Tables[0].Rows[i]["ALARM_LEVEL"].ToString()));
                }
            }

            ds = biz.GetAlarmGradeList();
            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlAlarmGrade.Items.Add(new ListItem(ds.Tables[0].Rows[i]["ALARM_GRADE"].ToString(), ds.Tables[0].Rows[i]["ALARM_GRADE"].ToString()));
                }
            }


            ds = biz.GetAlarmCommonList();
            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlAlarmCommon.Items.Add(new ListItem(ds.Tables[0].Rows[i]["COMMON_ALARM"].ToString(), ds.Tables[0].Rows[i]["COMMON_ALARM"].ToString()));
                }
            }

            ds = biz.GetAlarmHeadList();
            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlAlarmHead.Items.Add(new ListItem(ds.Tables[0].Rows[i]["HEAD"].ToString(), ds.Tables[0].Rows[i]["HEAD"].ToString()));
                }
            }

            ds = biz.GetAlarmCameraList();
            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlAlarmCamera.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CAMERA"].ToString(), ds.Tables[0].Rows[i]["CAMERA"].ToString()));
                }
            }
        }
        #endregion

        #region SetPageInit
        private void SetPageInit()
        {
            arrColumn = new string[] { "SEQ", "PROCESS_SEGMENT_ID", "EQP_ID", "LINE", "H_08", "H_09", "H_10", "H_11", "H_12", "H_13", "H_14", "H_15", "H_16", "H_17", "H_18", "H_19", "H_20", "H_21", "H_22", "H_23", "H_00", "H_01", "H_02", "H_03", "H_04", "H_05", "H_06", "H_07", "H_SUM" };
            arrColumnCaption = new string[arrColumn.Length];
            arrColumnWidth = new string[arrColumn.Length];
            arrHiddenColumn = new string[] { "SEQ", "PROCESS_SEGMENT_ID", "LINE"};

            for (int i = 0; i < arrColumn.Length; i++)
            {
                // Cell
                arrColumnCaption[i] = Dictionary_Data.SearchDic("W_" + arrColumn[i].ToString(), bp.g_language);

                if (i == 0 || i == 1 || i == 2 || i == 3)
                {
                    arrColumnWidth[i] = "150";
                }
                else {
                    arrColumnWidth[i] = "100";
                }
            }
        }
        #endregion

        #region SetGridTitle
        private void SetGridTitle()
        {
            // 단순 조회 컬럼용
            //grid.SetColumn(arrColumn, arrColumnCaption, arrColumnWidth, strKeyColumn, strHiddenColumn);

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
            lbWorkDate.Text         = Dictionary_Data.SearchDic("WORK_DATE", bp.g_language);
            lbShift.Text            = Dictionary_Data.SearchDic("W_SHIFT", bp.g_language);
            
            lbOperation.Text        = Dictionary_Data.SearchDic("W_OPERATION", bp.g_language);
            lbAlarmClass.Text       = Dictionary_Data.SearchDic("W_ALARMCLASS", bp.g_language);
            lbEquipmentType.Text    = Dictionary_Data.SearchDic("W_EQPTYPE", bp.g_language);

            lbAlarmType.Text        = Dictionary_Data.SearchDic("W_ALARMTYPE", bp.g_language);
            lbAlarmLevel.Text       = Dictionary_Data.SearchDic("W_ALARMLEVEL", bp.g_language);
            lbAlarmGrade.Text       = Dictionary_Data.SearchDic("W_ALARMGRADE", bp.g_language);

            lbAlarmCommon.Text      = Dictionary_Data.SearchDic("W_ALARMCOMMON", bp.g_language);
            lbAlarmHead.Text        = Dictionary_Data.SearchDic("W_ALARMHEAD", bp.g_language);
            lbAlarmCamera.Text      = Dictionary_Data.SearchDic("W_ALARMCAMERA", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();

            //DataTable dtTimes = new DataTable();

            //dtTimes = SetTime();

            //string strFromTime = string.Empty;
            //string strToTime = string.Empty;

            //strFromTime = dtTimes.Rows[0]["FromTime"].ToString();
            //strToTime = dtTimes.Rows[0]["ToTime"].ToString();

            //// /* 

            //// 검색조건 생성
            //string[] sParam = {
            //    strFromTime
            //    , strToTime
            //    , ddlOperation.SelectedValue
            //    , ddlAlarmClass.SelectedValue
            //    , ddlAlarmType.SelectedValue
            //    , ddlAlarmLevel.SelectedValue
            //    , ddlAlarmGrade.SelectedValue
            //    , ddlAlarmCommon.SelectedValue
            //    , ddlAlarmHead.SelectedValue
            //    , ddlAlarmCamera.SelectedValue
            //    , ddlEquipmentType.SelectedValue
            //    //, bp.g_plant
            //    , "9012C"
            //};

            //// 비지니스 메서드 호출
            //ds = biz.GetGetAlarmCountList(sParam);

            ds = GetAlarmCount();

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
                    //grid.SetDataListToGrid(arrColumn, arrColumnCaption, arrColumnWidth, strKeyColumn, ds, strHiddenColumn);

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
            // */
        }
        #endregion

        #region SetGraphData
        private void SetGraphData(DataSet ds)
        {
            chart_01.Series.Clear();

            string strColName = string.Empty;

            DataRow[] drHourly = ds.Tables[0].Select("EQP_ID = 'MOVE HOURLY'");

            // 화면에 보여야할 Series 만큼 생성
            // ViewType.Bar, ViewType.Line
            Series sHourly = new Series("Hour Alarm Count", ViewType.Bar);

            for (int j = 0; j < drHourly.Length; j++)
            {
                for (int i = 8; i < 24; i++)
                {
                    if (i < 10)
                    {
                        strColName = "H_0" + i.ToString();
                    }
                    else
                    {
                        strColName = "H_" + i.ToString();
                    }

                    sHourly.Points.Add(new SeriesPoint(strColName, new double[] { Convert.ToInt32(drHourly[j][strColName].ToString()) }));
                }

                for (int i = 0; i < 8; i++)
                {
                    strColName = "H_0" + i.ToString();

                    sHourly.Points.Add(new SeriesPoint(strColName, new double[] { Convert.ToInt32(drHourly[j][strColName].ToString()) }));
                }
            }

            sHourly.LabelsVisibility = DefaultBoolean.True;

            // Add the series to the chart.
            chart_01.Series.Add(sHourly);
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            chart_01.Width = Convert.ToInt32(hidGraphWidth.Text);
            chart_01.Height = Convert.ToInt32(hidGraphHeight.Text);

            grid.Settings.VerticalScrollableHeight = Convert.ToInt32(hidGridHeight.Text);

            GetAlarmCountColorInfo();

            GetData();
        }
        #endregion

        #region btnReload_Click
        protected void btnReload_Click(object sender, EventArgs e)
        {
            chart_01.Width = Convert.ToInt32(hidGraphWidth.Text);
            chart_01.Height = Convert.ToInt32(hidGraphHeight.Text);

            grid.Settings.VerticalScrollableHeight = Convert.ToInt32(hidGridHeight.Text);
        }
        #endregion

        #region GetAlarmCount
        public DataSet GetAlarmCount()
        {
            DataSet ds = new DataSet();

            DataTable dtTimes = new DataTable();

            dtTimes = SetTime();

            string strFromTime = string.Empty;
            string strToTime = string.Empty;

            strFromTime = dtTimes.Rows[0]["FromTime"].ToString();
            strToTime = dtTimes.Rows[0]["ToTime"].ToString();

            // /* 

            // 검색조건 생성
            string[] sParam = {
                strFromTime
                , strToTime
                , ddlOperation.SelectedValue
                , ddlAlarmClass.SelectedValue
                , ddlAlarmType.SelectedValue
                , ddlAlarmLevel.SelectedValue
                , ddlAlarmGrade.SelectedValue
                , ddlAlarmCommon.SelectedValue
                , ddlAlarmHead.SelectedValue
                , ddlAlarmCamera.SelectedValue
                , ddlEquipmentType.SelectedValue
                //, bp.g_plant
                , "9012C"
            };

            // 비지니스 메서드 호출
            ds = biz.GetGetAlarmCountList(sParam);

            return ds;
        }
        #endregion

        #region GetAlarmCountColorInfo
        public void GetAlarmCountColorInfo()
        {
            DataSet ds = new DataSet();

            // 검색조건 생성
            string[] sParam = {
                //bp.g_plant
                "9012C"
                , ddlOperation.SelectedValue
            };

            // 비지니스 메서드 호출
            ds = biz.GetAlarmCountColorInfo(sParam);

            if (ds.Tables.Count > 0)
            {
                string strRtnMsg = ds.Tables[0].Rows[0][0].ToString();

                if (strRtnMsg == "Tibco Service Error")
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoConnectMsg();", true);
                }
                else
                {
                    iLowColor = Convert.ToInt32(ds.Tables[0].Rows[0]["LCL"].ToString());
                    iHighColor = Convert.ToInt32(ds.Tables[0].Rows[0]["UCL"].ToString());
                }
            }
            else
            {
                iLowColor = 0;
                iHighColor = 0;
            }
        }
        #endregion

        #region SetTime
        public DataTable SetTime()
        {

            DataTable dt = new DataTable();

            dt.Columns.Add("FromTime");
            dt.Columns.Add("ToTime");

            DataRow dr = null;

            string strDate = string.Empty;
            string strSetTime = string.Empty;

            string strStartTime = string.Empty;
            string strEndTime = string.Empty;

            string strFromTime = string.Empty;
            string strToTime = string.Empty;

            string[] strGetTime;

            DateTime dtStart = Convert.ToDateTime(txtDate.Text);

            if (ddlShift.SelectedValue == "")
            {
                strStartTime = ddlShift.Items[1].Text.Substring(3, ddlShift.Items[1].Text.Length - 3).Split('~')[0];

                strEndTime = ddlShift.Items[ddlShift.Items.Count - 1].Text.Substring(3, ddlShift.Items[ddlShift.Items.Count - 1].Text.Length - 3).Split('~')[1];

                strFromTime = txtDate.Text + " " + strStartTime;
                strSetTime = dtStart.AddDays(1).ToString("yyyy-MM-dd") + " " + strEndTime;

                DateTime dtTo = Convert.ToDateTime(strSetTime);

                strToTime = dtTo.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
            }
            else
            {

                strGetTime = ddlShift.SelectedItem.Text.Substring(3, ddlShift.SelectedItem.Text.Length - 3).Split('~');

                if (ddlShift.SelectedValue == "C")
                {

                    strFromTime = dtStart.AddDays(1).ToString("yyyy-MM-dd") + " " + strGetTime[0].ToString();

                    strSetTime = dtStart.AddDays(1).ToString("yyyy-MM-dd") + " " + strGetTime[1].ToString();

                    DateTime dtTo = Convert.ToDateTime(strSetTime);

                    strToTime = dtTo.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else if (ddlShift.SelectedValue == "B")
                {

                    strFromTime = txtDate.Text + " " + strGetTime[0].ToString();

                    if (strGetTime[1].ToString() == "24:00:00")
                    {
                        strSetTime = dtStart.AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00";
                    }
                    else
                    {
                        strSetTime = txtDate.Text + " " + strGetTime[1].ToString();
                    }

                    DateTime dtTo = Convert.ToDateTime(strSetTime);

                    strToTime = dtTo.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {

                    strFromTime = txtDate.Text + " " + strGetTime[0].ToString();

                    strSetTime = txtDate.Text + " " + strGetTime[1].ToString();

                    DateTime dtTo = Convert.ToDateTime(strSetTime);

                    strToTime = dtTo.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }

            dr = dt.NewRow();
            dr["FromTime"] = strFromTime;
            dr["ToTime"] = strToTime;
            dt.Rows.Add(dr);

            return dt;
        }
        #endregion

        #region grid_HtmlRowPrepared
        protected void grid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            int iSEQ = 0;

            iSEQ = Convert.ToInt32(e.GetValue("SEQ").ToString());

            if (iSEQ == 0) {
                e.Row.Cells[0].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Yellow");
            }

            if (iSEQ > 0)
            {
                e.Row.Cells[0].Style.Add(HtmlTextWriterStyle.BackgroundColor, "LightGray");

                for (int columnIndex = 1; columnIndex < e.Row.Cells.Count - 1; columnIndex++) 
                {
                    int iColumnIndex = 1;

                    for (int j = 8; j < 24; j++)
                    {
                        string strColName = string.Empty;

                        if (j < 10)
                        {
                            strColName = "H_0" + j.ToString();
                        }
                        else
                        {
                            strColName = "H_" + j.ToString();
                        }

                        int iCellValue = Convert.ToInt32(e.GetValue(strColName).ToString().Replace(",", ""));

                        if (iCellValue >= 1 && iCellValue <= iLowColor)
                        {
                            e.Row.Cells[iColumnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Green");
                        }

                        if (iCellValue > iLowColor && iCellValue <= iHighColor)
                        {
                            e.Row.Cells[iColumnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Yellow");
                        }

                        if (iCellValue > iHighColor && iHighColor > 0)
                        {
                            e.Row.Cells[iColumnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Red");
                        }

                        iColumnIndex++;
                    }


                    for (int j = 0; j < 8; j++)
                    {
                        string strColName = string.Empty;

                        strColName = "H_0" + j.ToString();

                        int iCellValue = Convert.ToInt32(e.GetValue(strColName).ToString().Replace(",", ""));

                        if (iCellValue >= 1 && iCellValue <= iLowColor)
                        {
                            e.Row.Cells[iColumnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Green");
                        }

                        if (iCellValue > iLowColor && iCellValue <= iHighColor)
                        {
                            e.Row.Cells[iColumnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Yellow");
                        }

                        if (iCellValue > iHighColor && iHighColor > 0)
                        {
                            e.Row.Cells[iColumnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Red");
                        }

                        iColumnIndex++;
                    }
                }
            }
        }
        #endregion

    }
}