using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MES.FW.Common.CommonMgt;
using MES.FW.Common.Crypt;

using HQCWeb.FMB_FW;
using HQCWeb.FW;

namespace HQCWeb.SystemMgt
{
    public partial class PublicSplitSample1 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();
        Crypt cy = new Crypt();

        public string strKey = System.Configuration.ConfigurationSettings.AppSettings["HQC_CRYPTKEY"].ToString();

        // 비지니스 클래스 작성
        //Biz.Sample_Biz biz = new Biz.Sample_Biz();

        #region GRID Setting
        // 메인 그리드에 보여져야할 컬럼 정의
        public string[] arrMainColumn;
        // 메인 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrMainColumnCaption;
        // 메인 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrMainColumnWidth;
        // 메인 그리드 키값 정의
        public string strMainKeyColumn = "COL_1";
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
        public string stDetailKeyColumn = "COL_1";
        // 상세 그리드 숨김처리 필드 정의
        public string[] arrDetailHiddenColumn;
        // 상세 그리드 Merge 필드 정의
        public string[] arrDetailMergeColumn;

        // 그리드 체크박스 노출여부
        public bool bMainShowCheckBox = false;
        public bool bDetailShowCheckBox = false;
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

        }
        #endregion

        #region SetPageInit
        private void SetPageInit()
        {
            arrMainColumn = new string[] { "COL_1", "COL_2", "COL_3", "COL_4", "COL_5" };
            arrMainColumnCaption = new string[arrMainColumn.Length];
            arrMainColumnWidth = new string[arrMainColumn.Length];

            for (int i = 0; i < arrMainColumn.Length; i++)
            {
                // Cell
                arrMainColumnCaption[i] = Dictionary_Data.SearchDic(arrMainColumn[i].ToString(), bp.g_language);


                // Module
                arrMainColumnCaption[i] = Dictionary_Data.SearchDic(arrMainColumn[i].ToString(), bp.g_language);

                arrMainColumnWidth[i] = "100";
            }

            arrMainHiddenColumn = new string[] { "COL_1" };

            // Cell Click Event 사용유무 체크 (Y : N)
            //MainGrid.CellClickEvent = "";

            // Cell Click 클릭시 호출할 자바스크립트 함수명
            //MainGrid.CellClickJsFunc = "Function Name";

            // Cell Click 클릭시 상세조회 조건을 넘겨주는 타입 ( C : 컬럼명 / V : Cell 값)
            //MainGrid.CellClickParamType = "";

            // Cell Click을 할 컬럼 배열
            //MainGrid.CellClickEventColumn = new string[] { "COL_1", "COL_2", "COL_3" };

            arrMainHiddenColumn = new string[] { "COL_5" };

            arrMainMergeColumn = new string[] { "COL_4" };

            arrDetailColumn = new string[] { "COL_1", "COL_2", "COL_3", "COL_4", "COL_5" };
            arrDetailColumnCaption = new string[arrDetailColumn.Length];
            arrDetailColumnWidth = new string[arrDetailColumn.Length];

            for (int i = 0; i < arrDetailColumn.Length; i++)
            {
                // Cell
                arrDetailColumnCaption[i] = Dictionary_Data.SearchDic(arrDetailColumn[i].ToString(), bp.g_language);

                // Module
                arrDetailColumnCaption[i] = Dictionary_Data.SearchDic(arrDetailColumn[i].ToString(), bp.g_language);

                arrDetailColumnWidth[i] = "100";
            }

            arrDetailHiddenColumn = new string[] { "COL_5" };

            arrDetailMergeColumn = new string[] { "COL_4" };

            //MainGrid.Height = hid//MainGridHeight.Text;
            //DetailGrid.Height = hid//DetailGriddHeight.Text;

        }
        #endregion

        #region SetGridTitle
        private void SetGridTitle()
        {
            //MainGrid.SetColumn(arrMainColumn, arrMainColumnCaption, arrMainColumnWidth, strMainKeyColumn, arrMainHiddenColumn, arrMainMergeColumn);

            //DetailGrid.SetColumn(arrDetailColumn, arrDetailColumnCaption, arrDetailColumnWidth, stDetailKeyColumn, arrDetailHiddenColumn, arrDetailMergeColumn);
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            // CELL과 MODULE 중 하나를 선택하여 다국어 처리

            // CELL
            lbCond_01.Text = Dictionary_Data.SearchDic("CON_01", bp.g_language);

            // MODULE
            lbCond_01.Text = Dictionary_Data.SearchDic("CON_01", bp.g_language);
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
            DataSet ds = new DataSet();

            // 검색조건 생성
            string[] sParam = {
                "Param1"
            };

            // 비지니스 메서드 호출
            //ds = biz.(sParam);

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
                    //MainGrid.SetDataListToGrid(arrMainColumn, arrMainColumnCaption, arrMainColumnWidth, strMainKeyColumn, ds, arrMainHiddenColumn, arrMainMergeColumn);

                    /// <summary>
                    /// 데이터 조회(단순)
                    /// </summary>
                    /// <param name="arrDetailColumn">컬럼</param>
                    /// <param name="arrDetailColumnCaption">컬럼 타이틀</param>
                    /// <param name="arrDetailColumnWidth">컬럼 사이즈</param>
                    /// <param name="stDetailKeyColumn">그리드 키값</param>
                    /// <param name="arrDetailHiddenColumn">숨김처리 컬럼</param>
                    /// <param name="arrDetailMergeColumn">Merge처리 컬럼</param>
                    //DetailGrid.SetColumn(arrDetailColumn, arrDetailColumnCaption, arrDetailColumnWidth, stDetailKeyColumn, arrDetailHiddenColumn, arrDetailMergeColumn);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoData();", true);

                bp.g_GridDataSource = null;

                //MainGrid.SetDataListReset();

                //DetailGrid.SetDataListReset();
            }
        }
        #endregion

        #region btnDetailSearch_Click
        protected void btnDetailSearch_Click(object sender, EventArgs e)
        {
            string[] strSplitValue = cy.Decrypt(hidParams.Value).Split('/');

            string[] sParam = {
                strSplitValue[0]
            };

            DataSet ds = new DataSet();

            //ds = biz.(sParam);

            if (ds.Tables.Count > 0)
            {
                string strRtnMsg = ds.Tables[0].Rows[0][0].ToString();

                if (strRtnMsg == "Tibco Service Error")
                {
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoConnectMsg();", true);
                }
                else
                {
                    //DetailGrid.SetDataListToGrid(arrDetailColumn, arrDetailColumnCaption, arrDetailColumnWidth, stDetailKeyColumn, ds, arrDetailHiddenColumn, arrDetailMergeColumn);
                }

            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoData();", true);

                //DetailGrid.SetDataListReset();
            }
        }
        #endregion
    }
}