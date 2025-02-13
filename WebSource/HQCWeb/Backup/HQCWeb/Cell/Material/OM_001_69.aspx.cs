using DevExpress.Web;
using HQCWeb.FMB_FW;
using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using MES.FW.Common.Crypt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HQCWeb.Cell.Material
{
    public partial class OM_001_69 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();
        Crypt cy = new Crypt();

        DataSet dsMainColumn = new DataSet();
        DataSet dsDetailColumn = new DataSet();

        //public string strKey = System.Configuration.ConfigurationSettings.AppSettings["HQC_CRYPTKEY"].ToString();
        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        // 비지니스 클래스 작성
        Biz.Cell.Material.OM_001_69 biz = new Biz.Cell.Material.OM_001_69();

        #region GRID Setting
        // 메인 그리드에 보여져야할 컬럼 정의
        public string[] arrMainColumn;
        // 메인 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrMainColumnCaption;
        // 메인 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrMainColumnWidth;
        // 메인 그리드 키값 정의
        public string strMainKeyColumn = "EQP_ID";
        // 메인 그리드 숨김처리 필드 정의
        public string[] arrMainHiddenColumn;
        // 메인 그리드 Merge 필드 정의
        public string[] arrMainMergeColumn;

        // 상세 그리드에 보여져야할 컬럼 정의
        public string[] arrDetailColumn;
        // 상세 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrDetailColumnCaption;
        // 상세 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrDetailColumnWidth;
        // 상세 그리드 키값 정의
        public string stDetailKeyColumn = "EQP_ID";
        // 상세 그리드 숨김처리 필드 정의
        public string[] arrDetailHiddenColumn;
        // 상세 그리드 Merge 필드 정의
        public string[] arrDetailMergeColumn;

        // 그리드 체크박스 노출여부
        public bool bMainShowCheckBox = false;
        public bool bDetailShowCheckBox = false;

        public int iMainColumnCount = 0;
        public int iDetailColumnCount = 0;

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
            }

            GetTime();

            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), "fn_gridCall();", true);
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            DataSet ds = new DataSet();

            txtFromDt.Value = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtToDt.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

            ddlEquipment.Items.Add(new ListItem("ALL", ""));

            ds = biz.GetComCodeList("Shift");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlShift.Items.Add(new ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                    }
                }
            }

            string[] sParam = {
                //bp.g_plant
                "9012C"
                , "P08"
            };
            ds = biz.GetEquipmentList(sParam);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlEquipment.Items.Add(new ListItem(ds.Tables[0].Rows[i]["EQP_NAME"].ToString(), ds.Tables[0].Rows[i]["EQP_ID"].ToString()));
                    }
                }
            }

        }
        #endregion

        #region SetPageInit
        private void SetPageInit()
        {
            dsMainColumn.Tables.Clear();
            dsDetailColumn.Tables.Clear();

            DataSet dsMain = new DataSet();
            DataSet dsDetail = new DataSet();

            DataTable dtMain = new DataTable();
            DataTable dtDetail = new DataTable();

            dsMain = biz.GetScreenReplaceTitleList();
            dtMain = dsMain.Tables[0].Copy();
            dsMainColumn.Tables.Add(dtMain);

            dsDetail = biz.GetLifetimeTitleList();
            dtDetail = dsDetail.Tables[0].Copy();
            dsDetailColumn.Tables.Add(dtDetail);

            int iMainCalLength = 0;
            int iDetailCalLength = 0;

            iMainCalLength = dsMain.Tables[0].Rows.Count;
            iDetailCalLength = dsDetail.Tables[0].Rows.Count;

            string[] arrMainTitle = new string[iMainCalLength];
            string[] arrDetailTitle = new string[iDetailCalLength];

            arrMainColumnWidth = new string[iMainCalLength];
            arrDetailColumnWidth = new string[iDetailCalLength];

            iMainColumnCount = iMainCalLength;
            iDetailColumnCount = iDetailCalLength;

            for (int i = 0; i < dsMain.Tables[0].Rows.Count; i++)
            {
                arrMainTitle[i] = dsMain.Tables[0].Rows[i]["COL_FIELDNAME"].ToString();

                if (i == 0 || i == 1)
                {
                    arrMainColumnWidth[i] = "150";
                }
                else
                {
                    arrMainColumnWidth[i] = "110";
                }
            }

            for (int i = 0; i < dsDetail.Tables[0].Rows.Count; i++)
            {
                arrDetailTitle[i] = dsDetail.Tables[0].Rows[i]["COL_FIELDNAME"].ToString();

                if (i == 0 || i == 1)
                {
                    arrDetailColumnWidth[i] = "150";
                }
                else
                {
                    arrDetailColumnWidth[i] = "110";
                }
            }

            arrMainMergeColumn = new string[] { "EQP_ID" };
            arrDetailMergeColumn = new string[] { "EQP_ID" };

            MainGrid.Height = hidMainGridHeight.Text;
            DetailGrid.Height = hidDetailGriddHeight.Text;


        }
        #endregion

        #region SetGridTitle
        private void SetGridTitle()
        {
            //MainGrid.SetColumn(arrMainColumn, arrMainColumnCaption, arrMainColumnWidth, strMainKeyColumn);

            //DetailGrid.SetColumn(arrDetailColumn, arrDetailColumnCaption, arrDetailColumnWidth, "");

            MainGrid.SetColumn_MultiHeader(dsMainColumn, arrMainColumnWidth, strMainKeyColumn, arrMainHiddenColumn, arrMainMergeColumn);

            DetailGrid.SetColumn_MultiHeader(dsDetailColumn, arrDetailColumnWidth, stDetailKeyColumn, arrDetailHiddenColumn, arrDetailMergeColumn);
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbPeriod.Text       = Dictionary_Data.SearchDic("W_PERIOD", bp.g_language);
            lbShift.Text        = Dictionary_Data.SearchDic("W_SHIFT_ID", bp.g_language);
            lbEquipment.Text    = Dictionary_Data.SearchDic("W_EQUIPMENT", bp.g_language);
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
            DataSet dsScreenReplace = new DataSet();
            DataSet dsLifetime = new DataSet();


            DataTable dtTimes = new DataTable();

            dtTimes = GetTime();

            string strFromTime = string.Empty;
            string strToTime = string.Empty;

            strFromTime = dtTimes.Rows[0]["FromTime"].ToString();
            strToTime = dtTimes.Rows[0]["ToTime"].ToString();

            // 검색조건 생성
            string[] sParam = {
                ddlEquipment.SelectedValue
                //, strFromTime
                //, strToTime
                , "2023-08-08 00:00:00"
                , "2023-12-09 07:59:59"
            };

            // 비지니스 메서드 호출
            dsScreenReplace = biz.GetScreenReplaceList(sParam);
            dsLifetime = biz.GetLifetimeList(sParam);

            if (dsScreenReplace.Tables.Count > 0)
            {
                string strRtnMsg = dsScreenReplace.Tables[0].Rows[0][0].ToString();

                if (strRtnMsg == "Tibco Service Error")
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoConnectMsg();", true);
                }
                else
                {
                    MainGrid.SetDataListToGrid_MultiHeader(dsMainColumn, dsScreenReplace, strMainKeyColumn, arrMainColumnWidth, arrMainHiddenColumn, arrMainMergeColumn);

                    if (dsLifetime.Tables.Count > 0)
                    {
                        strRtnMsg = dsLifetime.Tables[0].Rows[0][0].ToString();

                        if (strRtnMsg == "Tibco Service Error")
                        {
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoConnectMsg();", true);
                        }
                        else {
                            DetailGrid.SetDataListToGrid_MultiHeader(dsDetailColumn, dsLifetime, stDetailKeyColumn, arrDetailColumnWidth, arrDetailHiddenColumn, arrDetailMergeColumn);
                        }
                    }
                    else {
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoData();", true); 

                        DetailGrid.SetDataListReset();
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoData();", true);

                MainGrid.SetDataListReset();

                DetailGrid.SetDataListReset();
            }
        }
        #endregion

        #region GetTime
        public DataTable GetTime() {
            DataTable dt = new DataTable();

            dt.Columns.Add("FromTime");
            dt.Columns.Add("ToTime");

            DataRow dr = null;

            string[] strGetTime;

            string strDate = string.Empty;
            string strSetTime = string.Empty;

            string strStartTime = string.Empty;
            string strEndTime = string.Empty;

            string strFromTime = string.Empty;
            string strToTime = string.Empty;

            string fromStringDate = string.Empty;

            if (chkShift.Checked)
            {
                DateTime dtStart = Convert.ToDateTime(txtFromDt.Value);

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
                    strFromTime = txtFromDt.Value + " " + strGetTime[0].ToString();

                    if (strGetTime[1].ToString() == "24:00:00")
                    {
                        strSetTime = dtStart.AddDays(1).ToString("yyyy-MM-dd") + " 00:00:00";
                    }
                    else
                    {
                        strSetTime = txtFromDt.Value + " " + strGetTime[1].ToString();
                    }

                    DateTime dtTo = Convert.ToDateTime(strSetTime);

                    strToTime = dtTo.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    strFromTime = txtFromDt.Value + " " + strGetTime[0].ToString();

                    strSetTime = txtFromDt.Value + " " + strGetTime[1].ToString();

                    DateTime dtTo = Convert.ToDateTime(strSetTime);

                    strToTime = dtTo.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
                }
            }
            else {
                DateTime dtStart = Convert.ToDateTime(txtFromDt.Value);
                DateTime dtEnd = Convert.ToDateTime(txtToDt.Text);

                strGetTime = ddlShift.SelectedItem.Text.Substring(3, ddlShift.SelectedItem.Text.Length - 3).Split('~');

                strFromTime = dtStart.ToString("yyyy-MM-dd") + " " + strGetTime[0].ToString();

                strSetTime = dtEnd.AddDays(1).ToString("yyyy-MM-dd") + " " + strGetTime[0].ToString();

                DateTime dtTo = Convert.ToDateTime(strSetTime);

                strToTime = dtTo.AddSeconds(-1).ToString("yyyy-MM-dd HH:mm:ss");
            }

            dr = dt.NewRow();
            dr["FromTime"] = strFromTime;
            dr["ToTime"] = strToTime;
            dt.Rows.Add(dr);

            return dt;
        }
        #endregion

        #region Main_HtmlRowPrepared
        protected void Main_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            string strDescription = string.Empty;

            int i = 0;

            if (iMainColumnCount == e.Row.Cells.Count)
            {
                for (int columnIndex = 1; columnIndex < e.Row.Cells.Count - 1; columnIndex++)
                {
                    i++;

                    if (i == 3)
                    {
                        e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "LemonChiffon");
                        
                        i = 0;
                    }
                }
            }
            else {
                for (int columnIndex = 2; columnIndex < e.Row.Cells.Count - 1; columnIndex++)
                {
                    i++;

                    if (i == 3)
                    {
                        e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "LemonChiffon");

                        i = 0;
                    }
                }
            }

            strDescription = e.GetValue("DESC_OM_001_69").ToString();

            if (strDescription == null || strDescription == "")
            {
                for (int columnIndex = 0; columnIndex < e.Row.Cells.Count - 1; columnIndex++)
                {
                    e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Khaki");
                }
            }
        }
        #endregion

        #region Detail_HtmlRowPrepared
        protected void Detail_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        {
            if (e.RowType != GridViewRowType.Data) return;

            string strDescription = string.Empty;

            int i = 0;

            if (iDetailColumnCount == e.Row.Cells.Count)
            {
                for (int columnIndex = 1; columnIndex < e.Row.Cells.Count; columnIndex++)
                {
                    i++;

                    if (i == 3)
                    {
                        e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "LemonChiffon");
                        i = 0;
                    }
                }
            }
            else {
                for (int columnIndex = 2; columnIndex < e.Row.Cells.Count; columnIndex++)
                {
                    i++;

                    if (i == 3)
                    {
                        e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "LemonChiffon");
                        i = 0;
                    }
                }
            }

            strDescription = e.GetValue("DESC_OM_001_69").ToString();

            if (strDescription == null || strDescription == "")
            {
                for (int columnIndex = 0; columnIndex < e.Row.Cells.Count - 1; columnIndex++)
                {
                    e.Row.Cells[columnIndex].Style.Add(HtmlTextWriterStyle.BackgroundColor, "Khaki");
                }
            }
        }
        #endregion
    }
}