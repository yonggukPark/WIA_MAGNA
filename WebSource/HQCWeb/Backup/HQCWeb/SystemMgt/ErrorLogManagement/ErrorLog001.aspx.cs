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

namespace HQCWeb.SystemMgt.ErrorLogManagement
{
    public partial class ErrorLog001 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        protected string strVal = string.Empty;

        // 암복호화 키값 셋팅
        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            cy.Key = strKey;

            if (!IsPostBack)
            {
                SetCon();

                SetTitle();

                if (Request.Form["hidValue"] != null)
                {
                    strVal = Request.Form["hidValue"].ToString();

                    (Master.FindControl("hidPopValue") as HiddenField).Value = strVal;

                    GetData();
                }
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {

        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbSeq.Text      = Dictionary_Data.SearchDic("ERROR_NO", bp.g_language);
            lbMenuNM.Text   = Dictionary_Data.SearchDic("MENU_NM", bp.g_language);
            lbData.Text     = Dictionary_Data.SearchDic("ERROR_LOG", bp.g_language);

            lbWorkName.Text = Dictionary_Data.SearchDic("DETAIL", bp.g_language); // 상세
        }
        #endregion

        #region GetData
        protected void GetData()
        {
            string strScript = string.Empty;

            DataSet ds = new DataSet();

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            // 비지니스 클래스 작성
            Biz.SystemManagement.ErrorLogMgt biz = new Biz.SystemManagement.ErrorLogMgt();
            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "ErrorLogData.Get_ErrorLogInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("ERROR_NO", strSplitValue[0].ToString());

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetErrorLogInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                lbGetSeq.Text       = ds.Tables[0].Rows[0]["ERROR_NO"].ToString();
                lbGetMenuNM.Text    = ds.Tables[0].Rows[0]["MENU_NM"].ToString();
                ltGetData.Text      = ds.Tables[0].Rows[0]["ERROR_LOG"].ToString().Replace("\r\n", "<br><br>").Replace("\n\r", "<br><br>");
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('WEB_00070'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion
    }
}
