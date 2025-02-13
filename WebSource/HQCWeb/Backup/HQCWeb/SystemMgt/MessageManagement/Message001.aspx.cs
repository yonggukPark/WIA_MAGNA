using HQCWeb.FMB_FW;
using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using MES.FW.Common.Crypt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HQCWeb.SystemMgt.MessageManagement
{
    public partial class Message001 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetTitle();
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

            lbWorkName.Text  = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language);
        }
        #endregion

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strRtn = string.Empty;
            int iRtn = 0;
            string strScript = string.Empty;

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                // 등록 서비스 클래스 작성
                Biz.SystemManagement.MessageMgt biz = new Biz.SystemManagement.MessageMgt();

                strDBName = "GPDB";
                strQueryID = "MessageData.Get_MessageID_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("MSG_ID", txtMessageID.Text);

                strRtn = biz.GetMessageIDValChk(strDBName, strQueryID, sParamIDChk);

                if (strRtn == "0")
                {
                    strDBName = "GPDB";
                    strQueryID = "MessageData.Set_MessageInfo";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("MSG_ID", txtMessageID.Text);
                    sParam.Add("MSG_TXT_KR", txtMessageKR.Text);
                    sParam.Add("MSG_TXT_EN", txtMessageEN.Text);
                    sParam.Add("MSG_TXT_LO", txtMessageLO.Text);
                    sParam.Add("REG_ID",        bp.g_userid.ToString());
                    sParam.Add("CUD_TYPE", "C");
                    sParam.Add("CUR_MENU_ID", "WEB_00030");

                    // 호출 서비스 작성
                    iRtn = biz.SetMessageInfo(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('WEB_00030'); parent.fn_ModalCloseDiv(); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else
                    {
                        strScript = " alert('등록에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                }
                else
                {
                    strScript = " alert('존재하는 아이디 입니다. 등록하려는 아이디를 다시 입력하세요.'); ";
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

    }
}