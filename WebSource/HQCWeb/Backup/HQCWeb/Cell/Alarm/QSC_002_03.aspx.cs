using DevExpress.Utils.CodedUISupport;
using DevExpress.Web;
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
    public partial class QSC_002_03 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();

        Biz.Cell.Alarm.QSC_002_03 biz = new Biz.Cell.Alarm.QSC_002_03();

        public string strStandardJson = string.Empty;
        public string strOperationJson = string.Empty;
        public string strEquipmentJson = string.Empty;

        #region GRID Setting

        // 그리드에 보여져야할 컬럼 정의
        public string[] arrColumn;
        // 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrColumnCaption;
        // 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrColumnWidth;
        // 그리드 키값 정의
        public string strKeyColumn = "Process";
        // 팝업창에 전달할 Param 컬럼 정의
        public string[] arrParams = new string[] { "Process" };
        // 그리드 옵션 정의 , 첫번째 값에 따라서 [ P : (Popup) / D : (Detail)] 로 나뉜다.
        public string[] arrOption = new string[] { "P", "풀경로 팝업창 이름 : Popup01.aspx" };
        //public string[] arrOption = new string[] { "D"};
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

        #region SetPageInit
        private void SetPageInit()
        {
            arrColumn = new string[] { "Process", "EQUIPMENTID", "Standard", "ALARM_CODE", "ALARM_CREATETIME", "OWNERID", "ALARM_ACCEPTTIME", "TIMEDIFF", "ACCEPTOR", "ALARMGRADE", "BANDTEXT", "ALARMDATA", "ROW_NUM" };
            arrColumnWidth = new string[] { "80", "120", "80", "100", "140", "250", "140", "80", "80", "80", "350", "350", "1" };


            arrColumnCaption = new string[arrColumn.Length];


            for (int i = 0; i < arrColumn.Length; i++)
            {
                arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            txtFromDt.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtToDt.Text = System.DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");

            ddlFromTime.Items.Clear();
            ddlToTime.Items.Clear();

            ddlFromMin.Items.Clear();
            ddlToMin.Items.Clear();

            string strTime = string.Empty;

            for (int i = 0; i < 24; i++)
            {
                if (i < 10)
                {

                    strTime = "0" + i.ToString();

                    ddlFromTime.Items.Add(new ListItem(strTime, strTime));
                    ddlToTime.Items.Add(new ListItem(strTime, strTime));
                }
                else
                {
                    strTime = i.ToString();

                    ddlFromTime.Items.Add(new ListItem(strTime, strTime));
                    ddlToTime.Items.Add(new ListItem(strTime, strTime));
                }
            }

            for (int i = 0; i < 60; i++)
            {
                if (i < 10)
                {

                    strTime = "0" + i.ToString();

                    ddlFromMin.Items.Add(new ListItem(strTime, strTime));
                    ddlToMin.Items.Add(new ListItem(strTime, strTime));
                }
                else
                {
                    strTime = i.ToString();

                    ddlFromMin.Items.Add(new ListItem(strTime, strTime));
                    ddlToMin.Items.Add(new ListItem(strTime, strTime));
                }
            }

            string strSiteID = string.Empty;

            strSiteID = bp.g_plant;

            DataSet ds = new DataSet();

            string[] sParam = {
                strSiteID
            };

            ds = biz.GetStandardList(sParam);
            strStandardJson = DataTableToJson(ds.Tables[0]);

            ds = biz.GetOperationList(sParam);
            strOperationJson = DataTableToJson(ds.Tables[0]);

            ds = biz.GetEquipmentList(sParam);
            strEquipmentJson = DataTableToJson(ds.Tables[0]);
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
            lbSearchDate.Text       = Dictionary_Data.SearchDic("W_SEARCHDATE", bp.g_language);
            lbSearchStandard.Text   = Dictionary_Data.SearchDic("W_STANDARD", bp.g_language);
            lbSearchOpration.Text   = Dictionary_Data.SearchDic("W_OPERATION", bp.g_language);
            lbSearchEquipment.Text  = Dictionary_Data.SearchDic("W_EQUIPMENT", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData(int iPageIndex)
        {
            DataSet ds = new DataSet();

            string strFromTime = string.Empty;
            string strToTtime = string.Empty;

            //strFromTime = txtFromDt.Text + " " + ddlFromTime.SelectedValue + ":" + ddlFromMin.SelectedValue + ":00";
            //strToTtime = txtToDt.Text + " " + ddlToTime.SelectedValue + ":" + ddlToMin.SelectedValue + ":00";
            strFromTime = "2022-11-01" + " " + ddlFromTime.SelectedValue + ":" + ddlFromMin.SelectedValue + ":00";
            strToTtime = "2022-11-02" + " " + ddlToTime.SelectedValue + ":" + ddlToMin.SelectedValue + ":00";

            // 검색조건 생성
            string[] sParam = {
                txtStandard.Text.Replace(" ", "")
                , txtEquipment.Text.Replace(" ", "")
                , txtOperationHidden.Text.Replace(" ", "")
                , bp.g_plant
                , strFromTime
                , strToTtime
            };

            // 조회 서비스 클래스 작성
            ds = biz.GetQCSAlarmList(sParam);

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
            GetData(1);
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
}