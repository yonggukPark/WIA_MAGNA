using DevExpress.Utils;
using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Web;
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
using static DevExpress.Web.FilterControl.WebFilterControlOperations;

namespace HQCWeb.Cell.BR
{
    public partial class OM_010_56 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();
        Crypt cy = new Crypt();

        //public string strKey = System.Configuration.ConfigurationSettings.AppSettings["HQC_CRYPTKEY"].ToString();
        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        public string strOperationJSON = string.Empty;
        public string strLineJSON = string.Empty;
        public string strEquipmentJSON = string.Empty;

        // 비지니스 클래스 작성
        Biz.Cell.BR.OM_010_56 biz = new Biz.Cell.BR.OM_010_56();

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

            ddlProductSpec.Items.Add(new System.Web.UI.WebControls.ListItem("ALL", ""));
            ddlProductType.Items.Add(new System.Web.UI.WebControls.ListItem("ALL", ""));

            ds = biz.GetComCodeList("ProductSpec");
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlProductSpec.Items.Add(new ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
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
                        ddlProductType.Items.Add(new ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                    }
                }
            }
        }
        #endregion

        #region SetPageInit
        private void SetPageInit()
        {
            arrMainColumn = new string[] { "PROCESS_SEGMENT_ID", "PROCESS_SEGMENT_NAME", "WORKTEAM_A", "WORKTEAM_B", "WORKTEAM_C", "WORKTEAM_D", "TOTAL" };
            arrMainColumnCaption = new string[arrMainColumn.Length];
            arrMainColumnWidth = new string[arrMainColumn.Length];
            arrMainHiddenColumn = new string[] { "PROCESS_SEGMENT_ID"};

            for (int i = 0; i < arrMainColumn.Length; i++)
            {
                // Cell

                if (i == 1)
                {
                    arrMainColumnCaption[i] = Dictionary_Data.SearchDic("W_" + arrMainColumn[i].ToString(), bp.g_language);
                }
                else {
                    arrMainColumnCaption[i] = Dictionary_Data.SearchDic(arrMainColumn[i].ToString(), bp.g_language);
                }

                
                arrMainColumnWidth[i] = "100";
            }

            MainGrid.CellClickEvent = "Y";
            MainGrid.CellClickJsFunc = "fn_Detail";
            MainGrid.CellClickParamType = "C";
            MainGrid.CellClickEventColumn = new string[] { "WORKTEAM_A", "WORKTEAM_B", "WORKTEAM_C", "WORKTEAM_D", "TOTAL" };

            arrDetailColumn = new string[] { "CREATE_TIME", "WORK_DATE", "PRODUCT_ORDER_ID", "PRODUCT_DEFINITION_ID", "PRODUCT_DEFINITION_NAME", "PRODUCT_CLASS_ID", "PROCESS_SEGMENT_ID", "PROCESS_SEGMENT_NAME", "EQP_ID", "CREATOR", "WORKTEAM", "DEFECT_QTY" };
            arrDetailColumnCaption = new string[arrDetailColumn.Length];
            arrDetailColumnWidth = new string[arrDetailColumn.Length];

            for (int i = 0; i < arrDetailColumn.Length; i++)
            {
                arrDetailColumnCaption[i] = Dictionary_Data.SearchDic(arrDetailColumn[i].ToString(), bp.g_language);


                if (i == 0)
                {
                    arrDetailColumnWidth[i] = "150";
                } 
                else if (i == 4)
                {
                    arrDetailColumnWidth[i] = "250";
                }
                else if (i == 11)
                {
                    arrDetailColumnWidth[i] = "80";
                }
                else {
                    arrDetailColumnWidth[i] = "100";
                }
            }

            if (hidScreenType.Text == "T")
            {
                DetailGrid.Horizontal = "Y";
            }

            MainGrid.Height = hidMainGridHeight.Text;
            DetailGrid.Height = hidDetailGriddHeight.Text;
        }
        #endregion

        #region SetGridTitle
        private void SetGridTitle()
        {
            MainGrid.SetColumn(arrMainColumn, arrMainColumnCaption, arrMainColumnWidth, strMainKeyColumn, arrMainHiddenColumn, arrMainMergeColumn);

            DetailGrid.SetColumn(arrDetailColumn, arrDetailColumnCaption, arrDetailColumnWidth, stDetailKeyColumn, arrDetailHiddenColumn, arrDetailMergeColumn);
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
            lbProductSpec.Text  = Dictionary_Data.SearchDic("W_POCLASS", bp.g_language);
            lbProductType.Text  = Dictionary_Data.SearchDic("W_POTYPE", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();

            // 검색조건 생성
            string[] sParam = {
                //txtFromDt.Text.Replace("-", "")
                //, txtToDt.Text.Replace("-", "")
                "20231013"
                , "20231014"
                , txtEquipmentHidden.Text
                , ddlProductType.SelectedValue
                //, bp.g_plant
                , "9021C"
            };

            // 비지니스 메서드 호출
            ds = biz.GetShiftInputList(sParam);

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
                    /// <param name="arrMainColumn">컬럼</param>
                    /// <param name="arrMainColumnCaption">컬럼 타이틀</param>
                    /// <param name="arrMainColumnWidth">컬럼 사이즈</param>
                    /// <param name="strMainKeyColumn">그리드 키값</param>
                    /// <param name="ds">DataSet</param>
                    /// <param name="arrMainHiddenColumn">숨김처리 컬럼</param>
                    /// <param name="arrMainMergeColumn">Merge처리 컬럼</param>
                    MainGrid.SetDataListToGrid(arrMainColumn, arrMainColumnCaption, arrMainColumnWidth, strMainKeyColumn, ds, arrMainHiddenColumn, arrMainMergeColumn);

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
            GetData();
        }
        #endregion

        #region btnDetailSearch_Click
        protected void btnDetailSearch_Click(object sender, EventArgs e)
        {
            string[] strSplitValue = cy.Decrypt(hidParams.Value).Split('/');

            string[] sParam = {
                //txtFromDt.Text.Replace("-", "")
                //, txtToDt.Text.Replace("-","")
                "20231013"
                , "20231014"
                , txtEquipmentHidden.Text
                , ddlProductType.SelectedValue
                , strSplitValue[1].ToString()
                , strSplitValue[0].Replace("WORKTEAM_", "")
            };

            DataSet ds = new DataSet();

            ds = biz.GetShiftInputDetailList(sParam);


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
            else {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoData();", true);

                DetailGrid.SetDataListReset();
            }

            
        }
        #endregion

        #region btnFunctionCall_Click
        protected void btnFunctionCall_Click(object sender, EventArgs e)
        {
            SetDDLListCondition();
        }
        #endregion

        #region SetDDLListCondition
        private void SetDDLListCondition()
        {
            DataSet ds = new DataSet();

            string[] sParamPOList = {
                //bp.g_plant
                "9012C"
                , txtOperationHidden.Text
                , txtLine.Text
            };

            ds = biz.GetEquipmentList(sParamPOList);
            if (ds.Tables.Count > 0)
            {
                strEquipmentJSON = DataTableToJson(ds.Tables[0]);
            }
            else
            {
                strEquipmentJSON = "[]";
            }

            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_SetDDL(" + strEquipmentJSON + "); ", true);
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
                DetailGrid.Horizontal = "Y";
            }
        }
        #endregion
    }
}