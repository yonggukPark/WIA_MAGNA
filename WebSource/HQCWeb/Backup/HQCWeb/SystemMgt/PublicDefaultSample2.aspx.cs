using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MES.FW.Common.CommonMgt;
using HQCWeb.FMB_FW;
using HQCWeb.FW;
//realgrid용
using System.Dynamic;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using MES.FW.Common.Crypt;

namespace HQCWeb.SystemMgt
{
    public partial class PublicDefaultSample2 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();
        
        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        // 비지니스 클래스 작성
        //Biz.Sample_Biz biz = new Biz.Sample_Biz();

        #region GRID Setting

        // 그리드에 보여져야할 컬럼 정의
        public string[] arrColumn;
        // 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrColumnCaption;
        // 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrColumnWidth;
        // 그리드 키값 정의
        public string[] strKeyColumn = new string[] { "DIC_ID" };
        // 팝업창에 전달할 Param 컬럼 정의
        public string[] arrParams = new string[] { "COL_1" };
        // 그리드 옵션 정의 , 첫번째 값에 따라서 [ P : (Popup) / D : (Detail)] 로 나뉜다.
        public string[] arrOption = new string[] { "P", "풀경로 팝업창 이름 : /path/path/path/Popup01.aspx" };
        //public string[] arrOption = new string[] { "D"};
        // 메인 그리드 숨김처리 필드 정의
        public string[] arrHiddenColumn;
        // 그리드 Merge 필드 정의
        public string[] arrMergeColumn;
        
        //JSON 전달용 변수
        string jsField = string.Empty;
        string jsCol = string.Empty;
        string jsData = string.Empty;
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

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid' 함수 호출
                string script = $@" column = {jsCol}; 
                                field = {jsField}; 
                                createGrid(); ";

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script, true);

            }

            //ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), "fn_gridCall();", true);

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
            arrColumn = new string[] { "DIC_ID", "DIC_TXT_KR", "DIC_TXT_EN", "DIC_TXT_LO", "REMARK1", "REMARK2", "USE_YN", "DEL_YN", "REGIST_DATE", "REGIST_USER_ID", "MODIFY_DATE", "MODIFY_USER_ID", "KEY_HID" };
            arrColumnCaption = new string[arrColumn.Length];
            arrColumnWidth = new string[arrColumn.Length];

            for (int i = 0; i < arrColumn.Length; i++)
            {
                // Module
                arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);

                arrColumnWidth[i] = "100";
            }
        }
        #endregion

        #region SetGridTitle
        private void SetGridTitle()
        {
            // 단순 조회 컬럼용
            //grid.SetColumn(arrColumn, arrColumnCaption, arrColumnWidth, strKeyColumn, arrHiddenColumn, arrMergeColumn);

            // 팝업 호출
            ////grid.SetColumn(arrColumn, arrColumnCaption, arrColumnWidth, strKeyColumn, arrParams, arrOption, arrHiddenColumn, arrMergeColumn);
            
            //realGrid 방식
            //그리드 컬럼 데이터를 JSON string으로 변환합니다.
            jsCol = ConvertJSONData.ConvertColArrToJSON(arrColumn, arrColumnCaption, arrColumnWidth, "cols");
            jsField = ConvertJSONData.ConvertColArrToJSON(arrColumn, arrColumnCaption, arrColumnWidth, "fields");
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbDictionaryID.Text = Dictionary_Data.SearchDic("DIC_ID", bp.g_language);
            lbDictionaryNM.Text = Dictionary_Data.SearchDic("DIC_NM", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData()
        {
            DataSet ds = new DataSet();

            Biz.SystemManagement.PublicBizSample2 biz = new Biz.SystemManagement.PublicBizSample2();

            strDBName = "GPDB";
            strQueryID = "Sample1.Get_DictionaryList";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("dicid", txtDictionaryID.Text);
            param.Add("dicnm", txtDictionaryNM.Text);

            // 비지니스 메서드 호출
            ds = biz.GetSelectSample(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                // 팝업창이 있을경우 사이즈 전달
                //grid.PopupWidth = hidWidth.Value;
                //grid.PopupHeight = hidHeight.Value;

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
               // grid.SetDataListToGrid(arrColumn, arrColumnCaption, arrColumnWidth, strKeyColumn, ds, arrHiddenColumn, arrMergeColumn);

                /// <summary>
                /// 데이터 조회(팝업 호출)
                /// </summary>
                /// <param name="arrColumn">컬럼</param>
                /// <param name="arrColumnCaption">컬럼 타이틀</param>
                /// <param name="arrColumnWidth">컬럼 사이즈</param>
                /// <param name="strKeyColumn">그리드 키값</param>
                /// <param name="arrParams">PARAMETER 컬럼</param>
                /// <param name="arrOption">상세 옵션 [ P : 팝업창 호출 || D : 상세내용 조회 // 팝업창 명 ] </param>
                /// <param name="ds">DataSet</param>
                //grid.SetDataListToGrid(arrColumn, arrColumnCaption, arrColumnWidth, strKeyColumn, arrParams, arrOption, ds, arrHiddenColumn, arrMergeColumn);

                //realGrid 방식
                jsData = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[0], strKeyColumn);
                
                //정상처리되면
                if (!String.IsNullOrEmpty(jsData))
                {
                    // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                    string script = $@" data = {jsData}; 
                            createGrid(); ";

                    //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "Faile", "fn_NoData();", true);

                //grid.SetDataListReset();
            }
        }
        #endregion

        #region SetUpdate
        public void SetUpdate()
        {
            string jsonData = string.Empty;
            string propertyName = string.Empty;
            string keyValue = string.Empty;
            string strScript = string.Empty;
            int intRtn, rowCnt = 0, failCnt = 0;
            DataSet ds = new DataSet();
            List<ExpandoObject> dataObjects;
            List<string> failID = new List<string> ();
            FW.Data.Parameters sParam;
            Biz.SystemManagement.PublicBizSample2 biz = new Biz.SystemManagement.PublicBizSample2();

            strDBName = "GPDB";
            strQueryID = "Sample1.Set_DictionaryInfo";

            jsonData = hidupdateJSON.Value;
            dataObjects = JsonConvert.DeserializeObject<List<ExpandoObject>>(jsonData, new ExpandoObjectConverter());//Row 변경사항 역직렬화

            //Row 순회
            foreach (dynamic data in dataObjects)
            {
                intRtn = 0;
                sParam = new FW.Data.Parameters();

                // Row 내 Column 순회하며 파라미터 수집
                foreach (var property in data)
                {
                    propertyName = property.Key;
                    if (propertyName.Equals("DIC_ID"))
                    {
                        sParam.Add("dicid", property.Value);
                        keyValue = property.Value;
                    }
                    else if (propertyName.Equals("DIC_TXT_KR"))
                        sParam.Add("dictxtkr", property.Value);
                    else if (propertyName.Equals("DIC_TXT_EN"))
                        sParam.Add("dictxten", property.Value);
                    else if (propertyName.Equals("DIC_TXT_LO"))
                        sParam.Add("dictxtlo", property.Value);
                }
                sParam.Add("regid", bp.g_userid.ToString());//사용자ID

                //INSERT & UPDATE 시도
                intRtn = biz.SetInsertUpdateDeleteSample(strDBName, strQueryID, sParam);

                // 연속시도하므로 실패ID 저장하고 계속진행
                if(intRtn != 1)
                {
                    failID.Add(keyValue);
                    failCnt++;
                }
                rowCnt++;
            }
            strScript = (failCnt == 0) ? " alert('저장이 완료되었습니다. '); " : $@" alert('저장이 완료되었습니다. {rowCnt} 개의 데이터 중 {failCnt} 개가 실패했습니다.'); ";
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "saveData", strScript, true);
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetData();
        }
        #endregion


        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            SetUpdate();
            GetData();
        }
        #endregion

        #region Excel_Click
        protected void Excel_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            DataSet dsTitle = new DataSet();

            strDBName = "DBNAME";
            strQueryID = "SQLMAP_NAMESPACE.STATEMENT_ID";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("Param1", "");

            // 비지니스 메서드 호출
            //ds = biz.(strDBName, strQueryID, param);

            string strPageName = string.Empty;
            string strContditionTitle = string.Empty;
            string strContditionValue = string.Empty;

            string strHeaderInfo = string.Empty;

            strPageName = "Default";
            strContditionTitle = "Condition_1" + "," + "Condition_2" + "," + "Condition_3";
            strContditionValue = lbDictionaryID.Text + "," + lbDictionaryID.Text + "," + lbDictionaryID.Text;

            DataTable dt = new DataTable();

            dt.Columns.Add("TITLE", typeof(string));
            dt.Columns.Add("CNT", typeof(string));

            DataRow dr = null;

            dr = dt.NewRow();
            dr["TITLE"] = "TITLE1";
            dr["CNT"] = "1";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["TITLE"] = "TITLE2";
            dr["CNT"] = "3";
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["TITLE"] = "TITLE3";
            dr["CNT"] = "1";
            dt.Rows.Add(dr);

            dsTitle.Tables.Add(dt);

            int iArrayCnt = arrColumnCaption.Length;

            string[] arrColumnTitle = new string[iArrayCnt];

            for (int i = 0; i < iArrayCnt; i++)
            {
                arrColumnTitle[i] = Dictionary_Data.SearchDic(arrColumnCaption[i].ToString(), bp.g_language);
            }

            /// <summary>
            /// 엑셀 다운로드
            /// </summary>
            /// <param name="strPageName">페이지명</param>
            /// <param name="strContditionTitle">검색조건 타이틀</param>
            /// <param name="strContditionValue">검색조건 값</param>
            /// <param name="ds">조회된 DataSet</param>
            /// <param name="bMultiRow">상단 타이틀이 2줄이상일 경우 : true // 일반일경우 : false </param>
            /// <param name="dsTitle">멀티 타이틀값</param>

            ee.ExcelDownLoad(strPageName, strContditionTitle, strContditionValue, ds, arrColumnTitle, false, dsTitle);
        }
        #endregion

        #region ConvertColArrToJSON
        //string[] -> JSON으로 변경
        //public static string ConvertColArrToJSON(string[] arrCol, string[] arrCap, string[] arrWidth, string type)
        //{
        //    var dynamicList = new List<ExpandoObject>();

        //    for (int i = 0; i < arrCol.Length; i++)
        //    {
        //        dynamic dynamicRow = new ExpandoObject();
        //        var dict = dynamicRow as IDictionary<string, object>;
        //        if (type.Equals("fields"))
        //        {
        //            dict["fieldName"] = arrCol[i];
        //            dict["dataType"] = "text";
        //        }
        //        else if (type.Equals("cols"))
        //        {
        //            dict["name"] = arrCol[i];
        //            dict["fieldName"] = arrCol[i];
        //            dict["width"] = arrWidth[i];
        //            dict["header"] = new { text = arrCap[i] };
        //        }
        //        dynamicList.Add(dynamicRow);
        //    }

        //    return JsonConvert.SerializeObject(dynamicList);
        //}
        #endregion

        #region ConvertDataTableToJSON
        //DataTable -> JSON으로 변경
        //public string ConvertDataTableToJSON(DataTable dataTable)
        //{
        //    var dynamicList = new List<ExpandoObject>();

        //    foreach (DataRow row in dataTable.Rows)
        //    {
        //        dynamic dynamicRow = new ExpandoObject();
        //        var rowDict = dynamicRow as IDictionary<string, object>;

        //        foreach (DataColumn column in dataTable.Columns)
        //        {
        //            rowDict[column.ColumnName] = Convert.ChangeType(row[column], column.DataType);

        //            if (column.ColumnName == strKeyColumn) {
                        
        //                rowDict["KEY_HID"] = cy.Encrypt(Convert.ChangeType(row[column], column.DataType).ToString());
        //            }
        //        }

        //        dynamicList.Add(dynamicRow);
        //    }

        //    return JsonConvert.SerializeObject(dynamicList);
        //}
        #endregion

    }
}