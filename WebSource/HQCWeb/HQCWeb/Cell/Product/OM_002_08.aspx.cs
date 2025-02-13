using DocumentFormat.OpenXml.Spreadsheet;
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
    public partial class OM_002_08 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        // 비지니스 클래스 작성
        Biz.Cell.Product.OM_002_08 biz = new Biz.Cell.Product.OM_002_08();

        public string strPOTypeJson = string.Empty;
        public string strPOJson = string.Empty;
        public string strSizeJson = string.Empty;
        public string strBusBarJson = string.Empty;
        public string strSpecialJson = string.Empty;

        #region GRID Setting

        // 그리드에 보여져야할 컬럼 정의
        public string[] arrColumn;
        // 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrColumnCaption;
        // 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrColumnWidth;
        // 그리드 키값 정의
        public string strKeyColumn = "WORK_DATE";
        // 팝업창에 전달할 Param 컬럼 정의
        public string[] arrParams = new string[] { "WORK_DATE" };
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

            string[] sParam = { 
                "9012C"
            };

            string[] sParamPOList = {
                "9012C"
                //, txtDate.Text.Replace("-", "")
                , "20231013"
            };

            ddlLine.Items.Add(new ListItem ("ALL", ""));
            ddlOperation.Items.Add(new ListItem("ALL", ""));
            ddlPOClass.Items.Add(new ListItem("ALL", ""));


            txtDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

            ds = biz.GetLineList(sParam);
            if (ds.Tables.Count > 0) {
                if (ds.Tables[0].Rows.Count > 0) {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {
                        ddlLine.Items.Add(new ListItem(ds.Tables[0].Rows[i]["FACILITY_NAME"].ToString(), ds.Tables[0].Rows[i]["FACILITY_ID"].ToString()));
                    }
                }
            }

            ds = biz.GetOperationList(sParam);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlOperation.Items.Add(new ListItem(ds.Tables[0].Rows[i]["PROCESS_SEGMENT_NAME"].ToString(), ds.Tables[0].Rows[i]["PROCESS_SEGMENT_ID"].ToString()));
                    }
                }
            }

            ds = biz.GetPOClassList(sParam);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlPOClass.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NAME"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                    }
                }
            }

            ds = biz.GetPOTypeList(sParam);
            if (ds.Tables.Count > 0)
            {
                strPOTypeJson = DataTableToJson(ds.Tables[0]);
            }
            else {
                strPOTypeJson = "[]";
            }

            ds = biz.GetPOList(sParamPOList);
            if (ds.Tables.Count > 0)
            {
                strPOJson = DataTableToJson(ds.Tables[0]);
            }
            else {
                strPOJson = "[]";
            }

            ds = biz.GetSizeList(sParam);
            if (ds.Tables.Count > 0)
            {
                strSizeJson = DataTableToJson(ds.Tables[0]);
            }
            else
            {
                strSizeJson = "[]";
            }

            ds = biz.GetBusBarList(sParam);
            if (ds.Tables.Count > 0)
            {
                strBusBarJson = DataTableToJson(ds.Tables[0]);
            }
            else
            {
                strBusBarJson = "[]";
            }

            ds = biz.GetSpecialList(sParam);
            if (ds.Tables.Count > 0)
            {
                strSpecialJson = DataTableToJson(ds.Tables[0]);
            }
            else
            {
                strSpecialJson = "[]";
            }
        }
        #endregion

        #region SetPageInit
        private void SetPageInit()
        {
            arrColumn = new string[] { "WORK_DATE", "SHIFT", "PROCESS_SEGMENT_ID", "PROCESS_SEGMENT_NAME", "PREVDAILY_WIP_QTY", "DAILY_WIP_QTY", "DAILY_IN_QTY", "DAILY_BR_QTY", "DAILY_OUT_QTY", "DAILY_RW_QTY", "DAILY_RW_TK_QTY", "DAILY_YIELD", "DAILY_BREAKAGE", "DAILY_REWORK_YIELD", "DAILY_REWORK_TK_YIELD", "DAILY_WC_QTY" };
            arrColumnCaption = new string[arrColumn.Length];
            arrColumnWidth = new string[arrColumn.Length];

            //arrColumnWidth = new string[] { "100", "50", "80", "120", "100", "100", "100", "100", "100", "100", "100", "100", "100", "100", "100", "100" };

            for (int i = 0; i < arrColumn.Length; i++)
            {
                arrColumnCaption[i] = Dictionary_Data.SearchDic("W_" + arrColumn[i].ToString(), bp.g_language);

                if (i == 1)
                {
                    arrColumnWidth[i] = "50";
                }
                else if (i == 2)
                {
                    arrColumnWidth[i] = "120";
                }
                else if (i == 3)
                {
                    arrColumnWidth[i] = "160";
                }
                else {
                    arrColumnWidth[i] = "100";
                }
            }

            grid.Height = hidGridHeight.Text;


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
            lbWorkDate.Text     = Dictionary_Data.SearchDic("W_WORKDATE", bp.g_language);
            lbLine.Text         = Dictionary_Data.SearchDic("W_LINE", bp.g_language);
            lbOperation.Text    = Dictionary_Data.SearchDic("W_OPERATION", bp.g_language);
            lbPOClass.Text      = Dictionary_Data.SearchDic("W_POCLASS", bp.g_language);
            lbPOType.Text       = Dictionary_Data.SearchDic("W_POTYPE", bp.g_language);
                                             
            lbPO.Text           = Dictionary_Data.SearchDic("W_PO", bp.g_language);
            lbSize.Text         = Dictionary_Data.SearchDic("W_SIZE", bp.g_language);
            lbBusBar.Text       = Dictionary_Data.SearchDic("W_BUSBAR", bp.g_language);
            lbSpecial.Text      = Dictionary_Data.SearchDic("W_SPECIAL", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();

            // 검색조건 생성
            string[] sParam = {
                "9012C"
                , txtDate.Text.Replace("-", "")
                , ddlLine.SelectedValue
                , ddlOperation.SelectedValue
                , txtPOHidden.Text

                , ddlPOClass.SelectedValue
                , txtPOType.Text
                , txtSize.Text
                , txtBusBar.Text
                , txtSpecial.Text
            };

            // 비지니스 메서드 호출
            ds = biz.GetDailyProcessWaferCountList(sParam);

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

                    /// <summary>
                    /// 데이터 조회(단순)
                    /// </summary>
                    /// <param name="arrColumn">컬럼</param>
                    /// <param name="arrColumnCaption">컬럼 타이틀</param>
                    /// <param name="arrColumnWidth">컬럼 사이즈</param>
                    /// <param name="strKeyColumn">그리드 키값</param>
                    /// <param name="ds">DataSet</param>
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

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
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
    }
};