using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;

using MES.FW.Common.CommonMgt;
using HQCWeb.FW;

namespace HQCWeb
{
    public partial class Sample : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        #region GRID Setting
        // 그리드에 보여져야할 컬럼 정의
        public string[] arrColumn;
        // 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrColumnCaption;
        // 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrColumnWidth;
        // 그리드 키값 정의
        public string strKeyColumn = "COL_1";
        // 팝업창에 전달할 Param 컬럼 정의
        public string[] arrParams = new string[] { "COL_1" };
        // 그리드 옵션 정의 , 첫번째 값에 따라서 [ P : (Popup) / D : (Detail)] 로 나뉜다.
        public string[] arrOption = new string[] { "P", "풀경로 팝업창 이름 : /path/path/path/Popup01.aspx" };
        //public string[] arrOption = new string[] { "D"};
        // 메인 그리드 숨김처리 필드 정의
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

                List<string> items = new List<string> { "Apple", "Banana", "Cherry", "Date", "Grapes", "Mango", "Orange" };
                ddlItems.Items.Insert(0, new ListItem("Type to filter...", "")); // Placeholder item
                ddlItems.Items[0].Attributes["disabled"] = "disabled";
                foreach (var item in items)
                {
                    ddlItems.Items.Add(new ListItem(item));
                }
            }
        }
        #endregion
        
        #region SetCon
        private void SetCon()
        {
            txtFromDt.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
            txtToDt.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
        }
        #endregion

        #region SetPageInit
        private void SetPageInit()
        {
            arrColumn = new string[] { "COL_1", "COL_2", "COL_3", "COL_4", "COL_5" };
            arrColumnCaption = new string[arrColumn.Length];
            arrColumnWidth = new string[arrColumn.Length];
            arrHiddenColumn = new string[] { "COL_1" };

            for (int i = 0; i < arrColumn.Length; i++)
            {
                // Module
                arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);

                arrColumnWidth[i] = "100";
            }

            arrHiddenColumn = new string[] { "COL_5" };

            arrMergeColumn = new string[] { "COL_4" };
        }
        #endregion
        
        #region btnExcelNew_Click
        protected void btnExcelNew_Click(object sender, EventArgs e)
        {
            Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

            DataSet ds = new DataSet();
            DataSet dsTitle = new DataSet();

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            strDBName = "GPDB";
            strQueryID = "UserInfoData.Get_UserInfoList";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("USER_ID", "");
            sParam.Add("USER_NM", "");

            // 비지니스 메서드 호출
            ds = biz.GetUserInfoList(strDBName, strQueryID, sParam);

            string strPageName = string.Empty;
            string strContditionTitle = string.Empty;
            string strContditionValue = string.Empty;

            string strHeaderInfo = string.Empty;

            strPageName = "Default";

            // 검색조건 추가시 필요
            //strContditionTitle = "Condition_Title1" + "," + "Condition_Title2" + "," + "Condition_Title3";
            //strContditionValue = "Condition_Control1" + "," + "Condition_Control2" + "," + "Condition_Control3";
            
            //// 타이틀 생성 예제
            //DataTable dt = new DataTable();

            //dt.Columns.Add("TITLE", typeof(string));
            //dt.Columns.Add("CNT", typeof(string));

            //DataRow dr = null;

            //dr = dt.NewRow();
            //dr["TITLE"] = "TITLE1";
            //dr["CNT"] = "1";
            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["TITLE"] = "TITLE2";
            //dr["CNT"] = "3";
            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["TITLE"] = "TITLE3";
            //dr["CNT"] = "1";
            //dt.Rows.Add(dr);

            //dsTitle.Tables.Add(dt);

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

            ee.ExcelDownLoad(strPageName, strContditionTitle, strContditionValue, ds, arrColumnTitle, false, null);
        }
        #endregion
    }
}