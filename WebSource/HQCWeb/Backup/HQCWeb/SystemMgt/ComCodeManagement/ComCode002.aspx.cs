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

namespace HQCWeb.SystemMgt.ComCodeManagement
{
    public partial class ComCode002 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        protected string strVal = string.Empty;

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            cy.Key = strKey;

            if (!IsPostBack)
            {
                SetTitle();

                SetControl();
                
                if (Request.Form["hidValue"] != null)
                {
                    strVal = Request.Form["hidValue"].ToString();

                    (Master.FindControl("hidPopValue") as HiddenField).Value = strVal;

                    GetData();
                }
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbComType.Text  = Dictionary_Data.SearchDic("COMM_TYPE", bp.g_language);
            lbComCD.Text    = Dictionary_Data.SearchDic("COMM_CD", bp.g_language);
            lbComNM.Text    = Dictionary_Data.SearchDic("COMM_DESC", bp.g_language);
            lbComSeq.Text   = Dictionary_Data.SearchDic("COMM_SEQ", bp.g_language);
            lbUseYN.Text    = Dictionary_Data.SearchDic("USE_YN", bp.g_language);

            lbWorkName.Text = Dictionary_Data.SearchDic("DETAIL", bp.g_language);
        }
        #endregion

        #region SetControl
        private void SetControl()
        {
            DataSet ds = new DataSet();

            Biz.SystemManagement.ComCodeMgt biz = new Biz.SystemManagement.ComCodeMgt();

            strDBName = "GPDB";
            strQueryID = "ComCodeData.Get_ComCodeByComTypeInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("COMM_TYPE", "USE_YN");
            
            ds = biz.GetComCodeByComTypeInfo(strDBName, strQueryID, sParam);

            ddlUseYN.Items.Clear();

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlUseYN.Items.Add(new ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                }
            }
        }
        #endregion

        #region GetData
        protected void GetData()
        {
            string strScript = string.Empty;
            string strDetailValue = string.Empty;

            DataSet ds = new DataSet();

            Biz.SystemManagement.ComCodeMgt biz = new Biz.SystemManagement.ComCodeMgt();
            
            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');
            
            strDBName = "GPDB";
            strQueryID = "ComCodeData.Get_ComCodeInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("COMM_TYPE",     strSplitValue[0].ToString());
            sParam.Add("COMM_CD",       strSplitValue[1].ToString());

            sParam.Add("CUR_MENU_ID",   "WEB_00050");


            ds = biz.GetComCodeInfo(strDBName, strQueryID, sParam);
            
            if (ds.Tables.Count > 0)
            {
                lbGetComType.Text       = ds.Tables[0].Rows[0]["COMM_TYPE"].ToString();
                lbGetComCD.Text         = ds.Tables[0].Rows[0]["COMM_CD"].ToString();
                txtComNM.Text           = ds.Tables[0].Rows[0]["COMM_DESC"].ToString();
                txtComSeq.Text          = ds.Tables[0].Rows[0]["COMM_SEQ"].ToString();
                ddlUseYN.SelectedValue  = ds.Tables[0].Rows[0]["USE_YN"].ToString();

                // 변경전 데이터 셋팅
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    string strColumns = string.Empty;

                    strColumns = ds.Tables[0].Columns[i].ToString();

                    if (strDetailValue == "")
                    {
                        strDetailValue = strColumns + " : " + ds.Tables[0].Rows[0][strColumns].ToString();
                    }
                    else
                    {
                        strDetailValue += "," + strColumns + " : " + ds.Tables[0].Rows[0][strColumns].ToString();
                    }
                }

                (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = cy.Encrypt(strDetailValue);
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('WEB_00050'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnModify_Click
        protected void btnModify_Click(object sender, EventArgs e)
        {
            int iRtn = 0;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strDValue = cy.Decrypt(strDVal).Split('/');

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                Biz.SystemManagement.ComCodeMgt biz = new Biz.SystemManagement.ComCodeMgt();

                strDBName = "GPDB";
                strQueryID = "ComCodeData.Set_ComCodeInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("COMM_TYPE",     strValue[0].ToString());
                sParam.Add("COMM_CD",       strValue[1].ToString());
                sParam.Add("COMM_DESC",     txtComNM.Text);
                sParam.Add("COMM_SEQ",      txtComSeq.Text);
                sParam.Add("USE_YN",        ddlUseYN.SelectedValue);

                sParam.Add("REG_ID",        bp.g_userid.ToString());
                sParam.Add("CUD_TYPE",      "U");
                sParam.Add("CUR_MENU_ID",   "WEB_00050");
                sParam.Add("PREV_DATA",     strDValue[0].ToString());                  // 이전 데이터 셋팅

                // 서비스 작성
                iRtn = biz.SetComCodeInfo(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall('WEB_00050'); parent.fn_ModalCloseDiv(); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
                else
                {
                    strScript = " alert('수정에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
            }
            else
            {
                strScript = " fn_ExError(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }

            
        }
        #endregion

        #region btnDelete_Click
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            Biz.SystemManagement.ComCodeMgt biz = new Biz.SystemManagement.ComCodeMgt();

            int iRtn = 0;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strDValue = cy.Decrypt(strDVal).Split('/');

            strDBName = "GPDB";
            strQueryID = "ComCodeData.Set_ComCodeInfoDel";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("COMM_TYPE",     strValue[0].ToString());
            sParam.Add("COMM_CD",       strValue[1].ToString());
            sParam.Add("REG_ID",        bp.g_userid.ToString());
            sParam.Add("CUD_TYPE",      "D");
            sParam.Add("CUR_MENU_ID",   "WEB_00050");

            sParam.Add("PREV_DATA",     strDValue[0].ToString());

            // 서비스 작성
            iRtn = biz.DelComCodeInfo(strDBName, strQueryID, sParam);

            if (iRtn == 1)
            {

                (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('WEB_00050'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                strScript = " alert('삭제에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion
    }
}