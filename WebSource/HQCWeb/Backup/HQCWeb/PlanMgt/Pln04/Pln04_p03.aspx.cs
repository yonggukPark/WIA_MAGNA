using HQCWeb.FMB_FW;
using HQCWeb.FW;
using HQCWeb.Report;
using MES.FW.Common.CommonMgt;
using MES.FW.Common.Crypt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HQCWeb.PlanMgt.Pln04
{
    public partial class Pln04_p03 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

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

                if (Request.Form["hidPopupValue"] != null)
                {
                    strVal = Request.Form["hidPopupValue"].ToString();

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
            // 상세내용 확인후 수정 또는 삭제일 경우
            lbWorkName.Text = Dictionary_Data.SearchDic("PRINT", bp.g_language); // 출력
        }
        #endregion

        #region GetData
        protected void GetData()
        {
            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            Telerik.Reporting.ReportBook reportBook = new Telerik.Reporting.ReportBook();
            Telerik.Reporting.InstanceReportSource instanceReportSource = new Telerik.Reporting.InstanceReportSource();
            var deviceInfo = new System.Collections.Hashtable();
            Telerik.Reporting.Processing.ReportProcessor reportProcessor = new Telerik.Reporting.Processing.ReportProcessor();

            string strPlantCd = strSplitValue[0].ToString();
            string strShipNo = strSplitValue[1].ToString();
            string strUserId = strSplitValue[2].ToString();
            string strShipType = strSplitValue[3].ToString();
            string strPrintType = strSplitValue[4].ToString();

            string[] used = strPrintType.Split(',');


            for (int i = 0; i < used.Length; i++)
            {
                Report4 report = new Report4();

                deviceInfo = new System.Collections.Hashtable();
                deviceInfo["JavaScript"] = "this.print({bUI: true, bSilent: false, bShrinkToFit: true});";

                instanceReportSource.ReportDocument = report;
                instanceReportSource.Parameters.Add(new Telerik.Reporting.Parameter("PLANT_CD", strPlantCd)); //출하증번호
                instanceReportSource.Parameters.Add(new Telerik.Reporting.Parameter("SHIP_NO", strShipNo)); //출하증번호
                instanceReportSource.Parameters.Add(new Telerik.Reporting.Parameter("EMP_NO", strUserId));   //발행자
                instanceReportSource.Parameters.Add(new Telerik.Reporting.Parameter("SHIP_TYPE", strShipType));  //출하유형
                instanceReportSource.Parameters.Add(new Telerik.Reporting.Parameter("PRINT_TYPE", used[i])); //출하증 용도

                reportBook.Reports.Add(report);

                ((Telerik.Reporting.TextBox)report.Items.Find("txtType", true)[0]).Value = "[ " + used[i] + " ]";

            }

            instanceReportSource.ReportDocument = reportBook;
            var result = reportProcessor.RenderReport("PDF", instanceReportSource, deviceInfo);

            this.Response.Clear();
            this.Response.ContentType = result.MimeType;
            this.Response.Cache.SetCacheability(HttpCacheability.Private);
            this.Response.Expires = -1;
            this.Response.Buffer = true;

            this.Response.BinaryWrite(result.DocumentBytes);

            this.Response.End();
        }
        #endregion
    }
}
