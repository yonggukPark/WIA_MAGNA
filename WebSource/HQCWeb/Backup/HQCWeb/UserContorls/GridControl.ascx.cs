using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
//using DevExpress.Web;
using MES.FW.Common.CommonMgt;
using System.ComponentModel;
using HQCWeb.Biz;
//using DevExpress.DocumentServices.ServiceModel.DataContracts;
//using DevExpress.Web.Internal.XmlProcessor;
using MES.FW.Common.Crypt;
//using DevExpress.Web.Internal.Dialogs;
//using DevExpress.XtraPrinting;
//using DevExpress.XtraGrid.Controls;
using HQCWeb.FMB_FW;
using HQCWeb.FW;

//using DevExpress.Utils.Menu;
//using DevExpress.Utils.DPI;

namespace HQCWeb.UserContorls
{
    public partial class GridControl : System.Web.UI.UserControl
    {

        public string strMenuID = string.Empty;
        [Category("메뉴아이디"), Description("메뉴아이디")]
        public string MenuID
        {
            get
            {
                return strMenuID;
            }
            set
            {
                strMenuID = value;
            }
        }


        public string strTest = "sakjsdfkjadsf";




        #region
        //// 그리드 클릭 이벤트
        //public event EventHandler RowClick;

        //// 그리드 컬럼표현 이벤트 
        //public event ASPxGridViewTableRowEventHandler HtmlRowPrepared;

        //public static string[] strCoustomFiled;

        //BasePage bp = new BasePage();
        //Crypt cy = new Crypt();

        //#region Property
        //// SESSION 사용여부
        //public string strSessionYN = string.Empty;
        //[Category("SessionYN"), Description("SESSION 사용여부")]
        //public string SessionYN
        //{
        //    get
        //    {
        //        return strSessionYN;
        //    }
        //    set
        //    {
        //        strSessionYN = value;
        //    }
        //}

        //public string strGridHeight = string.Empty;
        //[Category("Height"), Description("Grid 높이")]
        //public string Height
        //{
        //    get
        //    {
        //        return strGridHeight;
        //    }
        //    set
        //    {
        //        strGridHeight = value;
        //    }
        //}

        //public string strWidth = string.Empty;
        //[Category("PopupWidth"), Description("팝업창 넓이")]
        //public string PopupWidth
        //{
        //    get
        //    {
        //        return strWidth;
        //    }
        //    set
        //    {
        //        strWidth = value;
        //    }
        //}

        //public string strHeight = string.Empty;
        //[Category("PopupHeight"), Description("팝업창 높이")]
        //public string PopupHeight
        //{
        //    get
        //    {
        //        return strHeight;
        //    }
        //    set
        //    {
        //        strHeight = value;
        //    }
        //}

        //public string strHorizontal = string.Empty;
        //[Category("Horizontal"), Description("Grid 가로스크롤 사용유무")]
        //public string Horizontal
        //{
        //    get
        //    {
        //        return strHorizontal;
        //    }
        //    set
        //    {
        //        strHorizontal = value;
        //    }
        //}

        //public string strRowColorUsed = string.Empty;
        //[Category("RowColorUsed"), Description("Grid 색상표시 유무")]
        //public string RowColorUsed
        //{
        //    get
        //    {
        //        return strRowColorUsed;
        //    }
        //    set
        //    {
        //        strRowColorUsed = value;
        //    }
        //}

        //public string strCellClickEvent = string.Empty;
        //[Category("CellClickEvent"), Description("Cell Click Event 사용유무")]
        //public string CellClickEvent
        //{
        //    get
        //    {
        //        return strCellClickEvent;
        //    }
        //    set
        //    {
        //        strCellClickEvent = value;
        //    }
        //}

        //public string[] strCellClickColumn;
        //[Category("CellClickColumn"), Description("Cell Click 이벤트를 사용할 컬럼목록")]
        //public string[] CellClickEventColumn
        //{
        //    get
        //    {
        //        return strCellClickColumn;
        //    }
        //    set
        //    {
        //        strCellClickColumn = value;
        //    }
        //}

        //public string strCellClickJsFunc;
        //[Category("CellClickJsFunc"), Description("Cell Click 이벤트후 상세조회시 사용할 자바스크립트 함수명")]
        //public string CellClickJsFunc
        //{
        //    get
        //    {
        //        return strCellClickJsFunc;
        //    }
        //    set
        //    {
        //        strCellClickJsFunc = value;
        //    }
        //}

        //public string strCellClickParamType;
        //[Category("CellClickParamType"), Description("Cell Click시 상세조회에 사용할 변수타입")]
        //public string CellClickParamType
        //{
        //    get
        //    {
        //        return strCellClickParamType;
        //    }
        //    set
        //    {
        //        strCellClickParamType = value;
        //    }
        //}

        //public string strAllowSort;
        //[Category("AllowSort"), Description("컬럼 Sorting 사용유무")]
        //public string AllowSort
        //{
        //    get
        //    {
        //        return strAllowSort;
        //    }
        //    set
        //    {
        //        strAllowSort = value;
        //    }
        //}

        //#endregion

        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (bp.g_userid.ToString() == "")
            //{
            //    //ScriptManager.RegisterStartupScript(this, this.GetType(), "Opener", " alert('세션이 만료되어 로그인 페이지로 이동합니다'); parent.location.href = '/login.aspx';", true);
            //}

            //// 기본적으로 Column Sorting 기능 사용하지 않는것으로 설정
            //grid.SettingsBehavior.AllowSort = false;

            //if (strSessionYN == "Y")
            //{
            //    if (bp.g_GridDataSource != null)
            //    {
            //        grid.DataSource = bp.g_GridDataSource;
            //        grid.DataBind();
            //    }
            //}

            //// /*
            //if (strGridHeight == "")
            //{
            //    grid.Settings.VerticalScrollableHeight = 705;
            //}
            //else {
            //    grid.Settings.VerticalScrollableHeight = Convert.ToInt32(strGridHeight);
            //}

            //if (strHorizontal == "Y")
            //{
            //    grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Visible;
            //}
            //else {
            //    grid.Settings.HorizontalScrollBarMode = ScrollBarMode.Hidden;
            //}

            //if (strCellClickEvent == "Y") {
            //    grid.HtmlDataCellPrepared += grid_HtmlDataCellPrepared;
            //}

            //if (AllowSort == "Y") {
            //    grid.SettingsBehavior.AllowSort = true;
            //}
            //// */

            //// /* 

            //GridViewEditingMode mode = (GridViewEditingMode)Enum.Parse(typeof(GridViewEditingMode), "Inline");
            //grid.SettingsEditing.Mode = mode;

            //// */
            ////cy.Key = System.Configuration.ConfigurationSettings.AppSettings["HQC_CRYPTKEY"].ToString(); 
            //cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");
        }
        #endregion

        #region Not Used


        //#region SetColumn(string[] arrColumn, string[] arrColumnCaption, string[] arrWidth, string strKeyColumn, string[] arrWidth, string[] arrMergeColumn)
        //public void SetColumn(string[] arrColumn, string[] arrColumnCaption, string[] arrWidth, string strKeyColumn, string[] arrHiddenColumn, string[] arrMergeColumn)
        //{
        //    grid.Columns.Clear();

        //    grid.KeyFieldName = strKeyColumn;

        //    string strCaption = string.Empty;

        //    //strCoustomFiled = arrColumn;

        //    if (arrHiddenColumn == null) {
        //        for (int i = 0; i < arrColumn.Length; i++)
        //        {
        //            strCaption = Dictionary_Data.SearchDic(arrColumnCaption[i].ToString(), bp.g_language);

        //            GridViewDataTextColumn gridCol = new GridViewDataTextColumn();
        //            gridCol.FieldName = arrColumn[i].ToString();
        //            gridCol.VisibleIndex = i;
        //            gridCol.PropertiesTextEdit.DisplayFormatString = "#,##0";
        //            gridCol.Caption = strCaption;
        //            gridCol.Width = Convert.ToInt32(arrWidth[i].ToString());

        //            if (arrMergeColumn != null) {
        //                for (int j = 0; j < arrMergeColumn.Length; j++)
        //                {
        //                    if (arrMergeColumn[j].ToString() == arrColumn[i].ToString())
        //                    {
        //                        gridCol.Settings.AllowCellMerge = DevExpress.Utils.DefaultBoolean.True;
        //                    }
        //                }
        //            }                   

        //            grid.Columns.Add(gridCol);
        //        }
        //    }
        //    else {
        //        if (arrHiddenColumn.Length > 0)
        //        {
        //            string strVisible = string.Empty;

        //            for (int i = 0; i < arrColumn.Length; i++)
        //            {
        //                strCaption = Dictionary_Data.SearchDic(arrColumnCaption[i].ToString(), bp.g_language);

        //                GridViewDataTextColumn gridCol = new GridViewDataTextColumn();
        //                gridCol.FieldName = arrColumn[i].ToString();
        //                gridCol.VisibleIndex = i;

        //                for (int j = 0; j < arrHiddenColumn.Length; j++)
        //                {
        //                    if (arrHiddenColumn[j].ToString() == arrColumn[i].ToString())
        //                    {
        //                        gridCol.Visible = false;
        //                    }
        //                }

        //                if (arrMergeColumn != null) {
        //                    for (int j = 0; j < arrMergeColumn.Length; j++)
        //                    {
        //                        if (arrMergeColumn[j].ToString() == arrColumn[i].ToString())
        //                        {
        //                            gridCol.Settings.AllowCellMerge = DevExpress.Utils.DefaultBoolean.True;
        //                        }
        //                    }
        //                }                        

        //                gridCol.PropertiesTextEdit.DisplayFormatString = "#,##0";
        //                gridCol.Caption = strCaption;
        //                gridCol.Width = Convert.ToInt32(arrWidth[i].ToString());
        //                grid.Columns.Add(gridCol);
        //            }
        //        }
        //        else
        //        {
        //            for (int i = 0; i < arrColumn.Length; i++)
        //            {
        //                strCaption = Dictionary_Data.SearchDic(arrColumnCaption[i].ToString(), bp.g_language);

        //                GridViewDataTextColumn gridCol = new GridViewDataTextColumn();
        //                gridCol.FieldName = arrColumn[i].ToString();
        //                gridCol.VisibleIndex = i;
        //                gridCol.PropertiesTextEdit.DisplayFormatString = "#,##0";
        //                gridCol.Caption = strCaption;
        //                gridCol.Width = Convert.ToInt32(arrWidth[i].ToString());

        //                if (arrMergeColumn != null) {
        //                    for (int j = 0; j < arrMergeColumn.Length; j++)
        //                    {
        //                        if (arrMergeColumn[j].ToString() == arrColumn[i].ToString())
        //                        {
        //                            gridCol.Settings.AllowCellMerge = DevExpress.Utils.DefaultBoolean.True;
        //                        }
        //                    }
        //                }                        

        //                grid.Columns.Add(gridCol);
        //            }
        //        }
        //    }
        //}
        //#endregion

        //#region SetColumn(string[] arrColumn, string[] arrWidth, string strKeyColumn, string[] arrParams, string[] arrOption, string[] arrHiddenColumn, string[] arrMergeColumn)
        //public void SetColumn(string[] arrColumn, string[] arrColumnCaption, string[] arrWidth, string strKeyColumn, string[] arrParams, string[] arrOption, string[] arrHiddenColumn, string[] arrMergeColumn)
        //{
        //    grid.Columns.Clear();

        //    grid.KeyFieldName = strKeyColumn;

        //    string strCaption = string.Empty;

        //    if (arrHiddenColumn == null)
        //    {
        //        for (int i = 0; i < arrColumn.Length; i++)
        //        {
        //            strCaption = Dictionary_Data.SearchDic(arrColumnCaption[i].ToString(), bp.g_language);

        //            GridViewDataTextColumn gridCol = new GridViewDataTextColumn();

        //            if (arrColumn[i].ToString() == strKeyColumn)
        //            {
        //                //GridViewDataTextColumn colItemTemplate = new GridViewDataTextColumn();

        //                if (arrOption[0] == "P")
        //                {
        //                    gridCol.DataItemTemplate = new MyHyperlinkTemplate_Popup(strKeyColumn, arrParams, arrOption[1], PopupWidth, PopupHeight);
        //                }
        //                else
        //                {
        //                    gridCol.DataItemTemplate = new MyHyperlinkTemplate_Detail(strKeyColumn, arrParams);
        //                }

        //                gridCol.FieldName = arrColumn[i].ToString();
        //                gridCol.Caption = strCaption;
        //                gridCol.Width = Convert.ToInt32(arrWidth[i].ToString());
        //                //gridCol.SortIndex = 1;
        //                //colItemTemplate.SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
        //                grid.Columns.Add(gridCol);
        //            }
        //            else
        //            {
        //                gridCol.FieldName = arrColumn[i].ToString();
        //                gridCol.Caption = strCaption;
        //                gridCol.VisibleIndex = i;
        //                //gridCol.SortIndex = i;
        //                gridCol.PropertiesTextEdit.DisplayFormatString = "#,##0";
        //                gridCol.Width = Convert.ToInt32(arrWidth[i].ToString());

        //                if (arrMergeColumn != null) {
        //                    for (int j = 0; j < arrMergeColumn.Length; j++)
        //                    {
        //                        if (arrMergeColumn[j].ToString() == arrColumn[i].ToString())
        //                        {
        //                            gridCol.Settings.AllowCellMerge = DevExpress.Utils.DefaultBoolean.True;
        //                        }
        //                    }
        //                }

        //                grid.Columns.Add(gridCol);
        //            }
        //        }

        //        GridViewDataTextColumn gridColNull = new GridViewDataTextColumn();

        //        gridColNull.FieldName = "";
        //        //gridColNull.SortIndex = 1;
        //        gridColNull.Width = 5;

        //        grid.Columns.Add(gridColNull);
        //    }
        //    else {
        //        if (arrHiddenColumn.Length > 0)
        //        {
        //            for (int i = 0; i < arrColumn.Length; i++)
        //            {
        //                strCaption = Dictionary_Data.SearchDic(arrColumnCaption[i].ToString(), bp.g_language);

        //                GridViewDataTextColumn gridCol = new GridViewDataTextColumn();

        //                if (arrColumn[i].ToString() == strKeyColumn)
        //                {
        //                    if (arrOption[0] == "P")
        //                    {
        //                        gridCol.DataItemTemplate = new MyHyperlinkTemplate_Popup(strKeyColumn, arrParams, arrOption[1], PopupWidth, PopupHeight);
        //                    }
        //                    else
        //                    {
        //                        gridCol.DataItemTemplate = new MyHyperlinkTemplate_Detail(strKeyColumn, arrParams);
        //                    }

        //                    for (int j = 0; j < arrHiddenColumn.Length; j++)
        //                    {
        //                        if (arrHiddenColumn[j].ToString() == arrColumn[i].ToString())
        //                        {
        //                            gridCol.Visible = false;
        //                        }
        //                    }

        //                    gridCol.FieldName = arrColumn[i].ToString();
        //                    gridCol.Caption = strCaption;
        //                    gridCol.Width = Convert.ToInt32(arrWidth[i].ToString());
        //                    //gridCol.SortIndex = 1;
        //                    grid.Columns.Add(gridCol);
        //                }
        //                else
        //                {
        //                    gridCol.FieldName = arrColumn[i].ToString();
        //                    gridCol.Caption = strCaption;
        //                    gridCol.VisibleIndex = i;
        //                    //gridCol.SortIndex = i;

        //                    for (int j = 0; j < arrHiddenColumn.Length; j++)
        //                    {
        //                        if (arrHiddenColumn[j].ToString() == arrColumn[i].ToString())
        //                        {
        //                            gridCol.Visible = false;
        //                        }
        //                    }

        //                    if (arrMergeColumn != null) {
        //                        for (int j = 0; j < arrMergeColumn.Length; j++)
        //                        {
        //                            if (arrMergeColumn[j].ToString() == arrColumn[i].ToString())
        //                            {
        //                                gridCol.Settings.AllowCellMerge = DevExpress.Utils.DefaultBoolean.True;
        //                            }
        //                        }
        //                    }                            

        //                    gridCol.PropertiesTextEdit.DisplayFormatString = "#,##0";
        //                    gridCol.Width = Convert.ToInt32(arrWidth[i].ToString());
        //                    grid.Columns.Add(gridCol);
        //                }
        //            }

        //            GridViewDataTextColumn gridColNull = new GridViewDataTextColumn();

        //            gridColNull.FieldName = "";
        //            gridColNull.Caption = "";
        //            //gridColNull.SortIndex = 1;
        //            gridColNull.Width = 5;

        //            grid.Columns.Add(gridColNull);
        //        }
        //        else {
        //            for (int i = 0; i < arrColumn.Length; i++)
        //            {
        //                strCaption = Dictionary_Data.SearchDic(arrColumnCaption[i].ToString(), bp.g_language);

        //                GridViewDataTextColumn gridCol = new GridViewDataTextColumn();

        //                if (arrColumn[i].ToString() == strKeyColumn)
        //                {
                            
        //                    if (arrOption[0] == "P")
        //                    {
        //                        gridCol.DataItemTemplate = new MyHyperlinkTemplate_Popup(strKeyColumn, arrParams, arrOption[1], PopupWidth, PopupHeight);
        //                    }
        //                    else
        //                    {
        //                        gridCol.DataItemTemplate = new MyHyperlinkTemplate_Detail(strKeyColumn, arrParams);
        //                    }

        //                    gridCol.FieldName = arrColumn[i].ToString();
        //                    gridCol.Caption = strCaption;
        //                    gridCol.Width = Convert.ToInt32(arrWidth[i].ToString());
        //                    //gridCol.SortIndex = 1;
        //                    grid.Columns.Add(gridCol);
        //                }
        //                else
        //                {
        //                    gridCol.FieldName = arrColumn[i].ToString();
        //                    gridCol.Caption = strCaption;
        //                    gridCol.VisibleIndex = i;
        //                    //gridCol.SortIndex = i;
        //                    gridCol.PropertiesTextEdit.DisplayFormatString = "#,##0";
        //                    gridCol.Width = Convert.ToInt32(arrWidth[i].ToString());

        //                    if (arrMergeColumn != null) {
        //                        for (int j = 0; j < arrMergeColumn.Length; j++)
        //                        {
        //                            if (arrMergeColumn[j].ToString() == arrColumn[i].ToString())
        //                            {
        //                                gridCol.Settings.AllowCellMerge = DevExpress.Utils.DefaultBoolean.True;
        //                            }
        //                        }
        //                    }

        //                    grid.Columns.Add(gridCol);
        //                }
        //            }
        //        }
        //    }
        //}
        //#endregion

        //#region SetColumn_MultiHeader(DataSet dsTitle, int iColWidth, string[] arrHiddenColumn, string[] arrMergeColumn)
        //public void SetColumn_MultiHeader(DataSet dsTitle, string[] arrColWidth, string strKeyColumn, string[] arrHiddenColumn, string[] arrMergeColumn)
        //{
        //    grid.Columns.Clear();

        //    grid.KeyFieldName = strKeyColumn;

        //    DataTable DT = new DataTable();
        //    DT = dsTitle.Tables[0];

        //    DataTable groupByTable = DT.Clone();  // 그룹핑될 DataTable 을 정의하고 구조를 원본 DataTable 스키마 복사
        //    dataTableGroupBy(DT, ref groupByTable);

        //    // groupByTable 그룹핑된 데이터 테이블
        //    string strTitle = string.Empty;
        //    string strFieldName = string.Empty;
        //    string strCaption = string.Empty;

        //    string strVisible = "T";
        //    string strMerge = "N";


        //    if (arrHiddenColumn == null)
        //    {
        //        for (int i = 0; i < groupByTable.Rows.Count; i++)
        //        {
        //            strTitle = groupByTable.Rows[i]["COL_GROUP_ID"].ToString();

        //            DataRow[] drSelect = dsTitle.Tables[0].Select("COL_GROUP_ID = '" + strTitle + "'");

        //            if (drSelect.Length == 1)
        //            {
        //                strCaption = Dictionary_Data.SearchDic(drSelect[0]["COL_CAPTION"].ToString(), bp.g_language);

        //                GridViewDataTextColumn gridCol = new GridViewDataTextColumn();
        //                gridCol.FieldName = drSelect[0]["COL_FIELDNAME"].ToString();
        //                gridCol.PropertiesTextEdit.DisplayFormatString = "#,##0";
        //                gridCol.Caption = strCaption;

        //                if (arrMergeColumn != null) {
        //                    for (int j = 0; j < arrMergeColumn.Length; j++)
        //                    {
        //                        if (arrMergeColumn[j].ToString() == drSelect[0]["COL_FIELDNAME"].ToString())
        //                        {
        //                            gridCol.Settings.AllowCellMerge = DevExpress.Utils.DefaultBoolean.True;
        //                        }
        //                    }
        //                }

        //                //gridCol.Width = Convert.ToInt32(iColWidth);
        //                gridCol.Width = Convert.ToInt32(arrColWidth[0].ToString());
        //                grid.Columns.Add(gridCol);
        //            }
        //            else
        //            {
        //                GridViewBandColumn tbandColumn = new GridViewBandColumn(strTitle);
        //                tbandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //                tbandColumn.VisibleIndex = i;
        //                grid.Columns.Add(tbandColumn);

        //                for (int j = 0; j < drSelect.Length; j++)
        //                {
        //                    strFieldName = drSelect[j]["COL_FIELDNAME"].ToString();
        //                    strCaption = drSelect[j]["COL_CAPTION"].ToString();

        //                    if (arrMergeColumn != null) {
        //                        for (int k = 0; k < arrMergeColumn.Length; k++)
        //                        {
        //                            if (arrMergeColumn[k].ToString() == drSelect[0]["COL_FIELDNAME"].ToString())
        //                            {
        //                                strMerge = "Y";
        //                            }
        //                        }
        //                    }

        //                    tbandColumn.Columns.Add(CreateColumn(strFieldName, strCaption, Convert.ToInt32(arrColWidth[i].ToString()), strVisible, strMerge));
        //                }
        //            }
        //        }
        //    }
        //    else {
        //        if (arrHiddenColumn.Length > 0)
        //        {
        //            for (int k = 0; k < arrHiddenColumn.Length; k++)
        //            {
        //                for (int i = 0; i < groupByTable.Rows.Count; i++)
        //                {
        //                    strTitle = groupByTable.Rows[i]["COL_GROUP_ID"].ToString();

        //                    DataRow[] drSelect = dsTitle.Tables[0].Select("COL_GROUP_ID = '" + strTitle + "'");

        //                    if (drSelect.Length == 1)
        //                    {

        //                        strCaption = Dictionary_Data.SearchDic(drSelect[0]["COL_CAPTION"].ToString(), bp.g_language);

        //                        GridViewDataTextColumn gridCol = new GridViewDataTextColumn();
        //                        gridCol.FieldName = drSelect[0]["COL_FIELDNAME"].ToString();
        //                        gridCol.PropertiesTextEdit.DisplayFormatString = "#,##0";

        //                        if (arrHiddenColumn[k].ToString() == drSelect[0]["COL_FIELDNAME"].ToString())
        //                        {
        //                            gridCol.Visible = false;
        //                        }

        //                        if (arrMergeColumn != null) {
        //                            for (int j = 0; j < arrMergeColumn.Length; j++)
        //                            {
        //                                if (arrMergeColumn[j].ToString() == drSelect[0]["COL_FIELDNAME"].ToString())
        //                                {
        //                                    gridCol.Settings.AllowCellMerge = DevExpress.Utils.DefaultBoolean.True;
        //                                }
        //                            }
        //                        }

        //                        gridCol.Caption = strCaption;
        //                        gridCol.Width = Convert.ToInt32(arrColWidth[0].ToString());
        //                        grid.Columns.Add(gridCol);
        //                    }
        //                    else
        //                    {
        //                        GridViewBandColumn tbandColumn = new GridViewBandColumn(strTitle);
        //                        tbandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //                        tbandColumn.VisibleIndex = i;
        //                        grid.Columns.Add(tbandColumn);

        //                        for (int j = 0; j < drSelect.Length; j++)
        //                        {
        //                            strFieldName = drSelect[j]["COL_FIELDNAME"].ToString();
        //                            strCaption = drSelect[j]["COL_CAPTION"].ToString();

        //                            if (arrHiddenColumn[k].ToString() == strFieldName)
        //                            {
        //                                strVisible = "F";
        //                            }

        //                            if (arrMergeColumn != null) {
        //                                for (int p = 0; p < arrMergeColumn.Length; p++)
        //                                {
        //                                    if (arrMergeColumn[p].ToString() == strFieldName)
        //                                    {
        //                                        strMerge = "Y";
        //                                    }
        //                                }
        //                            }

        //                            tbandColumn.Columns.Add(CreateColumn(strFieldName, strCaption, Convert.ToInt32(arrColWidth[i].ToString()), strVisible, strMerge));
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else {
        //            for (int i = 0; i < groupByTable.Rows.Count; i++)
        //            {
        //                strTitle = groupByTable.Rows[i]["COL_GROUP_ID"].ToString();

        //                DataRow[] drSelect = dsTitle.Tables[0].Select("COL_GROUP_ID = '" + strTitle + "'");

        //                if (drSelect.Length == 1)
        //                {

        //                    strCaption = Dictionary_Data.SearchDic(drSelect[0]["COL_CAPTION"].ToString(), bp.g_language);

        //                    GridViewDataTextColumn gridCol = new GridViewDataTextColumn();
        //                    gridCol.FieldName = drSelect[0]["COL_FIELDNAME"].ToString();
        //                    gridCol.PropertiesTextEdit.DisplayFormatString = "#,##0";
        //                    gridCol.Caption = strCaption;
        //                    gridCol.Width = Convert.ToInt32(Convert.ToInt32(arrColWidth[0].ToString()));
        //                    grid.Columns.Add(gridCol);
        //                }
        //                else
        //                {
        //                    GridViewBandColumn tbandColumn = new GridViewBandColumn(strTitle);
        //                    tbandColumn.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;
        //                    tbandColumn.VisibleIndex = i;
        //                    grid.Columns.Add(tbandColumn);

        //                    for (int j = 0; j < drSelect.Length; j++)
        //                    {
        //                        strFieldName = drSelect[j]["COL_FIELDNAME"].ToString();
        //                        strCaption = drSelect[j]["COL_CAPTION"].ToString();

        //                        if (arrMergeColumn != null) {
        //                            for (int p = 0; p < arrMergeColumn.Length; p++)
        //                            {
        //                                if (arrMergeColumn[p].ToString() == strFieldName)
        //                                {
        //                                    strMerge = "Y";
        //                                }
        //                            }
        //                        }

        //                        tbandColumn.Columns.Add(CreateColumn(strFieldName, strCaption, Convert.ToInt32(arrColWidth[i].ToString()), strVisible, strMerge));
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}
        //#endregion 

        //#region SetDataListToGrid(string[] arrColumn, string[] arrColumnCaption, string[] arrWidth, string strKeyColumn, DataSet ds, string[] arrHiddenColumn, string[] arrMergeColumn)
        //public void SetDataListToGrid(string[] arrColumn, string[] arrColumnCaption, string[] arrWidth, string strKeyColumn, DataSet ds, string[] arrHiddenColumn, string[] arrMergeColumn)
        //{
        //    SetColumn(arrColumn, arrColumnCaption, arrWidth, strKeyColumn, arrHiddenColumn, arrMergeColumn);

        //    if (strSessionYN == "Y")
        //    {
        //        bp.g_GridDataSource = ds;
        //    }

        //    //grid.KeyFieldName = strKeyColumn;
        //    grid.DataSource = ds;
        //    grid.DataBind();
        //}
        //#endregion

        //#region SetDataListToGrid(string[] arrColumn, string[] arrColumnCaption, string[] arrWidth, string strKeyColumn, string[] arrParams, string[] arrOption, DataSet ds, string[] arrHiddenColumn, string[] arrMergeColumn )
        //public void SetDataListToGrid(string[] arrColumn, string[] arrColumnCaption, string[] arrWidth, string strKeyColumn, string[] arrParams, string[] arrOption, DataSet ds, string[] arrHiddenColumn, string[] arrMergeColumn)
        //{
        //    SetColumn(arrColumn, arrColumnCaption, arrWidth, strKeyColumn, arrParams, arrOption, arrHiddenColumn, arrMergeColumn);

        //    if (strSessionYN == "Y")
        //    {
        //        bp.g_GridDataSource = ds;
        //    }

        //    //grid.KeyFieldName = strKeyColumn;
        //    grid.DataSource = ds;
        //    grid.DataBind();
        //}
        //#endregion

        //#region SetDataListToGrid_MultiHeader(DataSet dsTitle, DataSet dsData, int iColWidth, string[] arrHiddenColumn, string[] arrMergeColumn )
        //public void SetDataListToGrid_MultiHeader(DataSet dsTitle, DataSet dsData, string strKeyColumn, string[] arrColWidth, string[] arrHiddenColumn, string[] arrMergeColumn)
        //{
        //    SetColumn_MultiHeader(dsTitle, arrColWidth, strKeyColumn, arrHiddenColumn, arrMergeColumn);

        //    if (strSessionYN == "Y")
        //    {
        //        bp.g_GridDataSource = dsData;
        //    }

        //    //grid.KeyFieldName = strKeyColumn;
        //    grid.DataSource = dsData;
        //    grid.DataBind();
        //}
        //#endregion

        //#region dataTableGroupBy
        //public void dataTableGroupBy(DataTable oriData, ref DataTable copyData)
        //{
        //    DataRow[] drSelect = null;

        //    DataRow[] comSelect = null;


        //    string filter = string.Empty;
        //    string order = string.Empty;

        //    try
        //    {
        //        int oriCnt = oriData.Rows.Count;
        //        DataView dv = oriData.DefaultView;
        //        dv.Sort = order;

        //        DataTable dt = dv.ToTable();

        //        for (int i = 0; i < oriCnt; i++)
        //        {
        //            filter = string.Format("COL_GROUP_ID='{0}'", dt.Rows[i]["COL_GROUP_ID"]);

        //            drSelect = dt.Select(filter);


        //            if (drSelect.Length > 0)
        //            {
        //                comSelect = copyData.Select(filter);
        //                if (comSelect.Length <= 0)
        //                {
        //                    copyData.ImportRow(drSelect[0]);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception err)
        //    {
        //    }
        //}
        //#endregion

        //#region CreateColumn
        //protected GridViewDataColumn CreateColumn(string fieldName, string caption, int iColWidth, string strVisible, string strMerge)
        //{
        //    GridViewDataTextColumn column = new GridViewDataTextColumn();
        //    column.FieldName = fieldName;
        //    column.Caption = caption;

        //    if (strVisible == "F") {
        //        column.Visible = false;
        //    }

        //    if (strMerge == "Y") {
        //        column.Settings.AllowCellMerge = DevExpress.Utils.DefaultBoolean.True;
        //    }

        //    column.Width = iColWidth;
        //    column.PropertiesTextEdit.DisplayFormatString = "#,##0";
        //    return column;
        //}
        //#endregion

        //#region SetDataListReset
        //public void SetDataListReset()
        //{
        //    grid.DataSource = null;
        //    grid.DataBind();
        //}
        //#endregion

        //#region grid_HtmlRowPrepared
        //protected void grid_HtmlRowPrepared(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        //{
        //    if (e.RowType != GridViewRowType.Data) return;

        //    int idx = Convert.ToInt32(e.VisibleIndex);
        //    if (idx % 2 == 0 && idx != 0)
        //    {
        //        e.Row.BackColor = System.Drawing.Color.FromArgb(247, 249, 250);
        //    }

        //    if (strRowColorUsed == "Y")
        //    {
        //        Send_HtmlRowPreparedEvent(sender, e);
        //    }
        //}
        //#endregion

        //#region grid_HtmlDataCellPrepared
        //protected void grid_HtmlDataCellPrepared(object sender, ASPxGridViewTableDataCellEventArgs e)
        //{
        //    string[] arrColumn = CellClickEventColumn;

        //    string strParamVal = string.Empty;

        //    for (int i = 0; i < arrColumn.Length; i++)
        //    {
        //        if (arrColumn[i].ToString() == e.DataColumn.FieldName)
        //        {
        //            e.Cell.Attributes.Add("style", "cursor:pointer;");

        //            if (CellClickParamType == "C")
        //            {
        //                strParamVal = e.DataColumn.FieldName + "/" + e.KeyValue;
        //            }

        //            if (CellClickParamType == "V")
        //            {
        //                strParamVal = e.CellValue.ToString() + "/" + e.KeyValue;
        //            }

        //            e.Cell.Attributes.Add("onclick", String.Format("{0}(\"{1}\");", strCellClickJsFunc, cy.Encrypt(strParamVal)));
        //        }
        //    }
        //}
        //#endregion

        //#region Send_HtmlRowPreparedEvent
        //protected void Send_HtmlRowPreparedEvent(object sender, DevExpress.Web.ASPxGridViewTableRowEventArgs e)
        //{
        //    HtmlRowPrepared(sender, e);
        //}
        //#endregion

        //#region grid_HtmlRowCreated
        //protected void grid_HtmlRowCreated(object sender, ASPxGridViewTableRowEventArgs e)
        //{

        //}
        //#endregion


        #endregion
    }

    #region MyHyperlinkTemplate_Popup
    //class MyHyperlinkTemplate_Popup : ITemplate {
    //    public string strSetKeyColumn = string.Empty;
    //    public string[] arrSetParams = null;
    //    public string strSetPopupName = string.Empty;
    //    public string strSetPopupWidth = string.Empty;
    //    public string strSetPopupHeight = string.Empty;

    //    public MyHyperlinkTemplate_Popup(string strKeyColumn, string[] arrParams, string strPopupName, string strPopupWidth, string strPopupHeight) {
    //        strSetKeyColumn = strKeyColumn;
    //        arrSetParams = arrParams;
    //        strSetPopupName = strPopupName;
    //        strSetPopupWidth = strPopupWidth;
    //        strSetPopupHeight = strPopupHeight;
    //    }

    //    public void InstantiateIn(Control container) {
    //        Crypt cy = new Crypt();

    //        //cy.Key = System.Configuration.ConfigurationSettings.AppSettings["HQC_CRYPTKEY"].ToString(); 
    //        cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

    //        Literal control = new Literal();

    //        GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;

    //        string strInfo = string.Empty;
    //        string strSetParams = string.Empty;

    //        if (arrSetParams != null || arrSetParams.Length > 0) 
    //        {
    //            for (int i = 0; i < arrSetParams.Length; i++)
    //            {
    //                if (i == 0)
    //                {
    //                    strSetParams = DataBinder.Eval(gridContainer.DataItem, arrSetParams[i]).ToString();
    //                }
    //                else
    //                {
    //                    strSetParams += "/" + DataBinder.Eval(gridContainer.DataItem, arrSetParams[i]).ToString();
    //                }
    //            }

    //            strInfo = "<a href=\"javascript:fn_PostOpenPop('";
    //            strInfo += cy.Encrypt(strSetParams);
    //            strInfo += "', '" + strSetPopupName + "','" + strSetPopupWidth + "','" + strSetPopupHeight + "');\">";
    //            strInfo += "<font color='blue'>" + DataBinder.Eval(gridContainer.DataItem, strSetKeyColumn) + "</font></a>";
    //        }
    //        else {
    //            strInfo = DataBinder.Eval(gridContainer.DataItem, strSetKeyColumn).ToString();
    //        }
            
    //        control.Text = strInfo;
    //        container.Controls.Add(control);
    //    }
    //}
    #endregion

    #region MyHyperlinkTemplate_Detail
    //class MyHyperlinkTemplate_Detail : ITemplate
    //{
    //    public string strSetKeyColumn = string.Empty;
    //    public string[] arrSetParams = null;

    //    public MyHyperlinkTemplate_Detail(string strKeyColumn, string[] arrParams)
    //    {
    //        strSetKeyColumn = strKeyColumn;
    //        arrSetParams = arrParams;
    //    }

    //    public void InstantiateIn(Control container)
    //    {
    //        Crypt cy = new Crypt();

    //        //cy.Key = System.Configuration.ConfigurationSettings.AppSettings["HQC_CRYPTKEY"].ToString(); 
    //        cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

    //        Literal control = new Literal();

    //        GridViewDataItemTemplateContainer gridContainer = (GridViewDataItemTemplateContainer)container;

    //        string strInfo = string.Empty;
    //        string strSetParams = string.Empty;

    //        if (arrSetParams != null || arrSetParams.Length > 0)
    //        {
    //            for (int i = 0; i < arrSetParams.Length; i++)
    //            {
    //                if (i == 0)
    //                {
    //                    strSetParams = DataBinder.Eval(gridContainer.DataItem, arrSetParams[i]).ToString();
    //                }
    //                else
    //                {
    //                    strSetParams += "/" + DataBinder.Eval(gridContainer.DataItem, arrSetParams[i]).ToString();
    //                }
    //            }

    //            strInfo = "<a href=\"javascript:fn_Detail('" + cy.Encrypt(strSetParams) + "');\">";
    //            strInfo += DataBinder.Eval(gridContainer.DataItem, strSetKeyColumn) + "</a>";
    //        }
    //        else
    //        {
    //            strInfo = DataBinder.Eval(gridContainer.DataItem, strSetKeyColumn).ToString();
    //        }

    //        control.Text = strInfo;
    //        container.Controls.Add(control);
    //    }
    //}
    #endregion
}