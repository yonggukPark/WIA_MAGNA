using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
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

namespace HQCWeb.Cell.ILP
{
    public partial class OM_001_56 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();
        Crypt cy = new Crypt();

        DataSet dsColumn = new DataSet();

        //public string strKey = System.Configuration.ConfigurationSettings.AppSettings["HQC_CRYPTKEY"].ToString();
        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        public string strOperationJSON = string.Empty;
        public string strLineJSON = string.Empty;
        public string strEquipmentJSON = string.Empty;
        public string strPOJSON = string.Empty;

        // 비지니스 클래스 작성
        Biz.Cell.ILP.OM_001_56 biz = new Biz.Cell.ILP.OM_001_56();

        #region GRID Setting
        // 메인 그리드에 보여져야할 컬럼 정의
        public string[] arrMainColumn;
        // 메인 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrMainColumnCaption;
        // 메인 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrMainColumnWidth;
        // 메인 그리드 키값 정의
        public string strMainKeyColumn = "PROCESS_SEGMENT_ID";
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
        public string stDetailKeyColumn = "CREATE_TIME";
        // 상세 그리드 숨김처리 필드 정의
        public string[] arrDetailHiddenColumn;
        // 상세 그리드 Merge 필드 정의
        public string[] arrDetailMergeColumn;
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

            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), "fn_gridCall();", true);
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            DataSet ds = new DataSet();

            txtFromDt.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtToDt.Text = System.DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            string[] sParam = { 
                //bp.g_plant
                "9012C"
            };

            ds = biz.GetOperationList(sParam);
            if (ds.Tables.Count > 0)
            {
                strOperationJSON = DataTableToJson(ds.Tables[0]);
            }
            else
            {
                strOperationJSON = "[]";
            }

            ds = biz.GetLineList(sParam);
            if (ds.Tables.Count > 0)
            {
                strLineJSON = DataTableToJson(ds.Tables[0]);
            }
            else
            {
                strLineJSON = "[]";
            }

            strEquipmentJSON = "[]";
            strPOJSON = "[]";

            ddlProductSpec.Items.Add(new System.Web.UI.WebControls.ListItem("ALL", ""));
            ddlProductType.Items.Add(new System.Web.UI.WebControls.ListItem("ALL", ""));
            ddlReworkType.Items.Add(new System.Web.UI.WebControls.ListItem("ALL", ""));

            ds = biz.GetComCodeList("ProductSpec");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlProductSpec.Items.Add(new System.Web.UI.WebControls.ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                    }
                }
            }

            ds = biz.GetComCodeList("ProductType");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlProductType.Items.Add(new System.Web.UI.WebControls.ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                    }
                }
            }


            ds = biz.GetComCodeList("ReworkType");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlReworkType.Items.Add(new System.Web.UI.WebControls.ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                    }
                }
            }

        }
        #endregion

        #region SetPageInit
        private void SetPageInit()
        {
            dsColumn.Tables.Clear();

            DataSet ds = new DataSet();

            ds = biz.GetTitleList(ddlReworkType.SelectedValue);

            DataTable dt = new DataTable();

            dt = ds.Tables[0].Copy();

            dsColumn.Tables.Add(dt);  

            // Cell Click Event 사용유무 체크 (Y : N)
            MainGrid.CellClickEvent = "Y";

            // Cell Click 클릭시 호출할 자바스크립트 함수명
            MainGrid.CellClickJsFunc = "fn_Detail";

            // Cell Click 클릭시 상세조회 조건을 넘겨주는 타입 ( C : 컬럼명 / V : Cell 값)
            MainGrid.CellClickParamType = "C";

            int iCalLength = 0;

            iCalLength = ds.Tables[0].Rows.Count;

            string[] arrTitle = new string[iCalLength];

            arrMainColumnWidth = new string[iCalLength];


            for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {
                arrTitle[i] = ds.Tables[0].Rows[i]["COL_FIELDNAME"].ToString();

                if (i == 0 || i == 1)
                {
                    arrMainColumnWidth[i] = "200";
                }
                else {
                    arrMainColumnWidth[i] = "110";
                }
            }

            // Cell Click을 할 컬럼 배열
            MainGrid.CellClickEventColumn = arrTitle;

            arrMainHiddenColumn = new string[] { "PROCESS_SEGMENT_ID" };

            arrDetailColumn = new string[] { "CREATE_TIME", "PRODUCT_ORDER_ID", "PRODUCT_DEFINITION_ID", "PRODUCT_DEFINITION_NAME", "PRODUCT_CLASS_ID", "PROCESS_SEGMENT_ID", "PROCESS_SEGMENT_NAME", "EQP_ID", "CREATOR", "WORKTEAM", "WORK_DATE", "REWORKTYPE", "DEFECT_QTY" };
            arrDetailColumnCaption = new string[arrDetailColumn.Length];
            arrDetailColumnWidth = new string[arrDetailColumn.Length];

            for (int i = 0; i < arrDetailColumn.Length; i++)
            {
                arrDetailColumnCaption[i] = Dictionary_Data.SearchDic(arrDetailColumn[i].ToString(), bp.g_language);

                if (i == 0)
                {
                    arrDetailColumnWidth[i] = "150";
                }
                else if (i == 3)
                {
                    arrDetailColumnWidth[i] = "250";
                }
                else if (i == 12)
                {
                    arrDetailColumnWidth[i] = "80";
                }
                else {
                    arrDetailColumnWidth[i] = "100";
                }
            }

            if (hidScreenType.Text == "T") {
                MainGrid.Horizontal = "Y";
                DetailGrid.Horizontal = "Y";
            }

            MainGrid.Height = hidMainGridHeight.Text;
            DetailGrid.Height = hidDetailGriddHeight.Text;
        }
        #endregion

        #region SetGridTitle
        private void SetGridTitle()
        {
            //MainGrid.SetColumn(arrMainColumn, arrMainColumnCaption, arrMainColumnWidth, strMainKeyColumn);

            MainGrid.SetColumn_MultiHeader(dsColumn, arrMainColumnWidth, strMainKeyColumn, arrMainHiddenColumn, null);

            DetailGrid.SetColumn(arrDetailColumn, arrDetailColumnCaption, arrDetailColumnWidth, "", null, null);
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            // CELL
            lbWorkDate.Text     = Dictionary_Data.SearchDic("W_WORK_DATE", bp.g_language);
          
            lbOperation.Text    = Dictionary_Data.SearchDic("W_OPERATION", bp.g_language);
            lbLine.Text         = Dictionary_Data.SearchDic("W_LINE", bp.g_language);
            lbEquipment.Text    = Dictionary_Data.SearchDic("W_EQUIPMENT", bp.g_language);

            lbReworkType.Text   = Dictionary_Data.SearchDic("W_REWORKTYPE", bp.g_language);
            lbPO.Text           = Dictionary_Data.SearchDic("W_PO", bp.g_language);

            lbProductSpec.Text  = Dictionary_Data.SearchDic("W_POCLASS", bp.g_language);
            lbProductType.Text  = Dictionary_Data.SearchDic("W_POTYPE", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();

            string strSpec = string.Empty;
            string strType = string.Empty;

            if (ddlProductSpec.SelectedItem.Text == "ALL")
            {
                strSpec = "";
            }
            else
            {
                strSpec = ddlProductSpec.SelectedItem.Text;
            }

            if (ddlProductType.SelectedItem.Text == "ALL")
            {
                strType = "";
            }
            else
            {
                strType = ddlProductType.SelectedItem.Text;
            }

            // 검색조건 생성
            string[] sParam = {
                //, txtFromDt.Text.Replace("-", "")
                //, txtToDt.Text.Replace("-", "")
                "20231013"
                , "20231014"
                , txtEquipmentHidden.Text
                , txtPOHidden.Text
                , strSpec
                , strType
                //, bp.g_plant
                , "9012C"
            };


            if (ddlReworkType.SelectedValue == "") {
                // 비지니스 메서드 호출
                ds = biz.GetShiftInputList(sParam);
            }

            if (ddlReworkType.SelectedValue == "REWORK")
            {
                // 비지니스 메서드 호출
                ds = biz.GetShiftInputList_REWORK(sParam);
            }

            if (ddlReworkType.SelectedValue == "REWORK_TK")
            {
                // 비지니스 메서드 호출
                ds = biz.GetShiftInputList_REWORK_TK(sParam);
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

                    /// <summary>
                    /// 데이터 조회(단순)
                    /// </summary>
                    /// <param name="dsColumn">컬럼 DataSet </param>
                    /// <param name="ds">DataSet</param>
                    /// <param name="strMainKeyColumn">그리드 키값</param>
                    /// <param name="arrMainColumnWidth">컬럼 사이즈</param>
                    /// <param name="arrMainHiddenColumn">숨김처리 컬럼</param>
                    /// <param name="arrMainMergeColumn">Merge처리 컬럼</param>
                    MainGrid.SetDataListToGrid_MultiHeader(dsColumn, ds, strMainKeyColumn, arrMainColumnWidth, arrMainHiddenColumn, arrMainMergeColumn);

                    /// <summary>
                    /// 데이터 조회(단순)
                    /// </summary>
                    /// <param name="arrDetailColumn">컬럼</param>
                    /// <param name="arrDetailColumnCaption">컬럼 타이틀</param>
                    /// <param name="arrDetailColumnWidth">컬럼 사이즈</param>
                    /// <param name="stDetailKeyColumn">그리드 키값</param>
                    /// <param name="arrDetailHiddenColumn">숨김처리 컬럼</param>
                    /// <param name="arrDetailMergeColumn">Merge처리 컬럼</param>
                    DetailGrid.SetColumn(arrDetailColumn, arrDetailColumnCaption, arrDetailColumnWidth, stDetailKeyColumn, arrDetailHiddenColumn, arrDetailMergeColumn);

                    DetailGrid.SetDataListReset();
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoData();", true);

                bp.g_GridDataSource = null;

                MainGrid.SetDataListReset();

                DetailGrid.SetDataListReset();
            }
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SetGridTitle();

            GetData();
        }
        #endregion

        #region btnDetailSearch_Click
        protected void btnDetailSearch_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();

            string[] strSplitValue = cy.Decrypt(hidParams.Value).Split('/');

            string strSpec = string.Empty;
            string strType = string.Empty;

            if (ddlProductSpec.SelectedItem.Text == "ALL")
            {
                strSpec = "";
            }
            else
            {
                strSpec = ddlProductSpec.SelectedItem.Text;
            }

            if (ddlProductType.SelectedItem.Text == "ALL")
            {
                strType = "";
            }
            else
            {
                strType = ddlProductType.SelectedItem.Text;
            }
            
            string lastShiftText = strSplitValue[0].Substring(strSplitValue[0].Length - 2, 2);
            string shiftId = string.Empty;
            string strDtlType = string.Empty;

            if (string.Equals(lastShiftText, "TK"))
            {
                shiftId = strSplitValue[0].Replace("_TK", "");

                if (!shiftId.Contains("_"))
                {
                    shiftId = "Total";
                }
                else {
                    shiftId = shiftId.Replace("WORKTEAM_", "");
                }


                strDtlType = "REWORK_TK";
            }
            else if (string.Equals(lastShiftText, "TT"))
            {
                shiftId = strSplitValue[0].Replace("_TT", "");

                if (!shiftId.Contains("_"))
                {
                    shiftId = "Total";
                }
                else
                {
                    shiftId = shiftId.Replace("WORKTEAM_", "");
                }

                strDtlType = "";
            }
            else {

                if (!lastShiftText.Contains("_"))
                {
                    shiftId = "Total";
                }
                else {
                    shiftId = lastShiftText.Replace("_", "");
                }


                strDtlType = "REWORK";
            }

            string[] sParam = {
                //txtFromDt.Text.Replace("-", "")
                //, txtToDt.Text.Replace("-","")
                "20231013"
                , "20231014"
                , txtEquipmentHidden.Text
                , strSplitValue[1].ToString()
                , txtPOHidden.Text
                , strType
                , strSpec
                //, bp.g_plant
                , "9012C"
                , shiftId
            };

            if (strDtlType == "REWORK") {
                ds = biz.GetShiftInputDetailList_REWORK(sParam);
            }

            if (strDtlType == "REWORK_TK")
            {
                ds = biz.GetShiftInputDetailList_REWORK_TK(sParam);
            }

            if (strDtlType == "")
            {
                ds = biz.GetShiftInputDetailList(sParam);
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
                    DetailGrid.SetDataListToGrid(arrDetailColumn, arrDetailColumnCaption, arrDetailColumnWidth, stDetailKeyColumn, ds, arrDetailHiddenColumn, arrDetailMergeColumn);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoData();", true);

                DetailGrid.SetDataListReset();
            }
        }
        #endregion

        #region btnEquipmentCall_Click
        protected void btnEquipmentCall_Click(object sender, EventArgs e)
        {
            SetDDLEquipmentListCondition();
        }
        #endregion

        #region SetDDLEquipmentListCondition
        private void SetDDLEquipmentListCondition()
        {
            DataSet ds = new DataSet();

            string[] sParam = {
                //bp.g_plant
                "9012C"
                , txtOperationHidden.Text
                , txtLine.Text
            };

            ds = biz.GetEquipmentList(sParam);
            if (ds.Tables.Count > 0)
            {
                strEquipmentJSON = DataTableToJson(ds.Tables[0]);
            }
            else
            {
                strEquipmentJSON = "[]";
            }

            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_SetEquipmentDDL(" + strEquipmentJSON + "); ", true);
        }
        #endregion

        #region btnPOCall_Click
        protected void btnPOCall_Click(object sender, EventArgs e)
        {
            SetDDLPOListCondition();
        }
        #endregion

        #region SetDDLPOListCondition
        private void SetDDLPOListCondition()
        {
            DataSet ds = new DataSet();

            string strSpec = string.Empty;
            string strType = string.Empty;

            if (ddlProductSpec.SelectedItem.Text == "ALL")
            {
                strSpec = "";
            }
            else {
                strSpec = ddlProductSpec.SelectedItem.Text;
            }

            if (ddlProductType.SelectedItem.Text == "ALL")
            {
                strType = "";
            }
            else
            {
                strType = ddlProductType.SelectedItem.Text;
            }

            string[] sParam = {
                //bp.g_plant
                "9012C"
                //, txtFromDt.Text.Replace("-", "")
                //, txtToDt.Text.Replace("-", "")
                , "20231013"
                , "20231014"

                , txtOperationHidden.Text
                , strSpec
                , strType
            };

            ds = biz.GetPOList(sParam);
            if (ds.Tables.Count > 0)
            {
                strPOJSON = DataTableToJson(ds.Tables[0]);
            }
            else
            {
                strPOJSON = "[]";
            }

            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_SetPODDL(" + strPOJSON + "); ", true);
        }
        #endregion

        #region ddlProductType_SelectedIndexChanged
        protected void ddlProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDDLPOListCondition();
        }
        #endregion

        #region ddlProductSpec_SelectedIndexChanged
        protected void ddlProductSpec_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetDDLPOListCondition();
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

        #region btnGridReload_Click
        protected void btnGridReload_Click(object sender, EventArgs e)
        {
            if (hidScreenType.Text == "T")
            {
                MainGrid.Horizontal = "Y";
                DetailGrid.Horizontal = "Y";
            }
        }
        #endregion

    }
}