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

namespace HQCWeb.Cell.Material
{
    public partial class OM_001_70 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        // 비지니스 클래스 작성
        Biz.Cell.Material.OM_001_70 biz = new Biz.Cell.Material.OM_001_70();

        #region GRID Setting

        // 그리드에 보여져야할 컬럼 정의
        public string[] arrColumn;
        // 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrColumnCaption;
        // 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrColumnWidth;
        // 그리드 키값 정의
        public string strKeyColumn = "LAST_EVENT_SEQ";
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

            txtFromDt.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtToDt.Text = System.DateTime.Now.ToString("yyyy-MM-dd");

            ddlLine.Items.Add(new ListItem("ALL", ""));
            ddlEquipment.Items.Add(new ListItem("ALL", ""));
            ddlPrinter.Items.Add(new ListItem("ALL", ""));
            ddlHeader.Items.Add(new ListItem("ALL", ""));

            string[] sParam = { 
                //bp.g_plant
                "9012C"
            };

            ds = biz.GetLineList(sParam);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlLine.Items.Add(new ListItem(ds.Tables[0].Rows[i]["DESCRIPTION"].ToString(), ds.Tables[0].Rows[i]["FACILITY_ID"].ToString()));
                    }
                }
            }

            string[] sParamEquipment = {
                //bp.g_plant
                "9012C"
                , "P08"
            };

            ds = biz.GetEquipmentList(sParamEquipment);
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

            string[] sParamPrinter = {
                //bp.g_plant
                "9012C"
                , "PRINTER"
            };
            ds = biz.GetSubEquipmentPartList(sParamPrinter);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlPrinter.Items.Add(new ListItem(ds.Tables[0].Rows[i]["PART"].ToString(), ds.Tables[0].Rows[i]["PART"].ToString()));
                    }
                }
            }

            string[] sParamHeader = {
                //bp.g_plant
                "9012C"
                , "HEADER"
            };
            ds = biz.GetSubEquipmentPartList(sParamHeader);
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlHeader.Items.Add(new ListItem(ds.Tables[0].Rows[i]["PART"].ToString(), ds.Tables[0].Rows[i]["PART"].ToString()));
                    }
                }
            }
        }
        #endregion

        #region SetPageInit
        private void SetPageInit()
        {
            arrColumn = new string[] { "LAST_EVENT_SEQ", "CREATE_TIME", "INPUT_TIME", "UNKIT_TIME", "LINE", "EQP_ID", "PRINTER", "HEADER", "BARCODE", "LIFE_TIME", "REASON_CODE" };
            arrColumnCaption = new string[arrColumn.Length];
            arrColumnWidth = new string[arrColumn.Length];

            arrHiddenColumn = new string[] { "LAST_EVENT_SEQ" };

            for (int i = 0; i < arrColumn.Length; i++)
            {
                arrColumnCaption[i] = Dictionary_Data.SearchDic("W_" + arrColumn[i].ToString(), bp.g_language);
            }

            arrColumnWidth = new string[] { "10", "80", "150", "150", "50", "100", "80", "80", "350", "120", "120" };

            grid.Height = hidGridHeight.Text;

        }
        #endregion

        #region SetGridTitle
        private void SetGridTitle()
        {
            grid.SetColumn(arrColumn, arrColumnCaption, arrColumnWidth, strKeyColumn, arrHiddenColumn, arrMergeColumn);
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbPeriod.Text       = Dictionary_Data.SearchDic("W_PERIOD", bp.g_language);
            lbLine.Text         = Dictionary_Data.SearchDic("W_LINE", bp.g_language);
            lbEquipment.Text    = Dictionary_Data.SearchDic("W_EQUIPMENT", bp.g_language);
            lbPrinter.Text      = Dictionary_Data.SearchDic("W_PRINTER", bp.g_language);
            lbHeader.Text       = Dictionary_Data.SearchDic("W_HEADER", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();

            string strSubEquipment = string.Empty;

            strSubEquipment = ddlEquipment.SelectedValue + "-" + ddlPrinter.SelectedValue + "-" + ddlHeader.SelectedValue;

            // 검색조건 생성
            string[] sParam = {
                //bp.g_plant
                "9012C"
                , ddlLine.SelectedValue
                , ddlEquipment.SelectedValue
                , strSubEquipment
                //, txtFromDt.Text.Replace("-", "")
                //, txtToDt.Text.Replace("-", "")
                , "20231201"
                , "20231231"
            };

            // 비지니스 메서드 호출
            ds = biz.GetHoleScreenUseHistList(sParam);

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

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }
        #endregion

        #region btnGridReload_Click
        protected void btnGridReload_Click(object sender, EventArgs e)
        {
            if (hidScreenType.Text == "T")
            {
                grid.Horizontal = "Y";
            }
        }
        #endregion
    }
}