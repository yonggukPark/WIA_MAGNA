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

namespace HQCWeb.SystemMgt.MessageManagement
{
    public partial class Message002 : System.Web.UI.Page
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
            lbMessageID.Text = Dictionary_Data.SearchDic("MSG_ID", bp.g_language);
            lbMessageKR.Text = Dictionary_Data.SearchDic("MSG_TXT_KR", bp.g_language);
            lbMessageEN.Text = Dictionary_Data.SearchDic("MSG_TXT_EN", bp.g_language);
            lbMessageLO.Text = Dictionary_Data.SearchDic("MSG_TXT_LO", bp.g_language);

            lbWorkName.Text  = Dictionary_Data.SearchDic("DETAIL", bp.g_language);
        }
        #endregion

        #region GetData
        protected void GetData()
        {
            string strScript = string.Empty;
            string strDetailValue = string.Empty;

            DataSet ds = new DataSet();

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');
            
            Biz.SystemManagement.MessageMgt biz = new Biz.SystemManagement.MessageMgt();

            strDBName = "GPDB";
            strQueryID = "MessageData.Get_MessageInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("MSG_ID", strSplitValue[0].ToString());

            sParam.Add("CUR_MENU_ID", "WEB_00030");

            ds = biz.GetMessageData(strDBName, strQueryID, sParam);
            
            if (ds.Tables.Count > 0)
            {
                lbGetMessageID.Text = ds.Tables[0].Rows[0]["MSG_ID"].ToString();
                txtMessageKR.Text   = ds.Tables[0].Rows[0]["MSG_TXT_KR"].ToString();
                txtMessageEN.Text   = ds.Tables[0].Rows[0]["MSG_TXT_EN"].ToString();
                txtMessageLO.Text   = ds.Tables[0].Rows[0]["MSG_TXT_LO"].ToString();

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
                strScript = " alert('Message 정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('WEB_00030'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnModify_Click
        protected void btnModify_Click(object sender, EventArgs e)
        {
            int iRtn = 0;
            string strRtnValChk = string.Empty;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strDValue = cy.Decrypt(strDVal).Split('/');

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                // 수정 서비스 클래스 작성
                Biz.SystemManagement.MessageMgt biz = new Biz.SystemManagement.MessageMgt();

                strDBName = "GPDB";
                strQueryID = "MessageData.Set_MessageInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("MSG_ID",        strValue[0].ToString());
                sParam.Add("MSG_TXT_KR",    txtMessageKR.Text);
                sParam.Add("MSG_TXT_EN",    txtMessageEN.Text);
                sParam.Add("MSG_TXT_LO",    txtMessageLO.Text);

                sParam.Add("REG_ID",        bp.g_userid.ToString());
                sParam.Add("CUD_TYPE",      "U");
                sParam.Add("CUR_MENU_ID",   "WEB_00030");
                sParam.Add("PREV_DATA",     strDValue[0].ToString());

                // 호출 서비스 작성
                iRtn = biz.SetMessageInfo(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall('WEB_00030'); parent.fn_ModalCloseDiv(); ";
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
            int iRtn = 0;
            string strRtnValChk = string.Empty;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strDValue = cy.Decrypt(strDVal).Split('/');

            // 삭제 서비스 클래스 작성
            Biz.SystemManagement.MessageMgt biz = new Biz.SystemManagement.MessageMgt();
            
            strDBName = "GPDB";
            strQueryID = "MessageData.Set_MessageInfoDel";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("MSG_ID",        strValue[0].ToString());

            sParam.Add("REG_ID",        bp.g_userid.ToString());
            sParam.Add("CUD_TYPE",      "D");
            sParam.Add("CUR_MENU_ID",   "WEB_00030");
            sParam.Add("PREV_DATA",     strDValue[0].ToString());

            // 호출 서비스 작성
            iRtn = biz.DelMessageInfo(strDBName, strQueryID, sParam);

            if (iRtn == 1)
            {
                strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('WEB_00030'); parent.fn_ModalCloseDiv(); ";
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