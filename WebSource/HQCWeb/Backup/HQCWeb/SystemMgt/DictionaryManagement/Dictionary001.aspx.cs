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

namespace HQCWeb.SystemMgt.DictionaryManagement
{
    public partial class Dictionary001 : System.Web.UI.Page
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
            lbDictionaryID.Text = Dictionary_Data.SearchDic("DIC_ID", bp.g_language);
            lbDictionaryKR.Text = Dictionary_Data.SearchDic("DIC_TXT_KR", bp.g_language);
            lbDictionaryEN.Text = Dictionary_Data.SearchDic("DIC_TXT_EN", bp.g_language);
            lbDictionaryLO.Text = Dictionary_Data.SearchDic("DIC_TXT_LO", bp.g_language);

            lbWorkName.Text     = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language);
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
                Biz.SystemManagement.DictionaryMgt biz = new Biz.SystemManagement.DictionaryMgt();

                strDBName = "GPDB";
                strQueryID = "DictionaryData.Get_DictionaryID_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("DIC_ID", txtDictionaryID.Text);

                // 호출 서비스 작성
                strRtn = biz.GetDictionaryIDValChk(strDBName, strQueryID, sParamIDChk);

                if (strRtn == "0")
                {
                    strDBName = "GPDB";
                    strQueryID = "DictionaryData.Set_DictionaryInfo";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("DIC_ID", txtDictionaryID.Text);
                    sParam.Add("DIC_TXT_KR", txtDictionaryKR.Text);
                    sParam.Add("DIC_TXT_EN", txtDictionaryEN.Text);
                    sParam.Add("DIC_TXT_LO", txtDictionaryLO.Text);
                    sParam.Add("REG_ID",        bp.g_userid.ToString());
                    sParam.Add("CUD_TYPE", "C");
                    sParam.Add("CUR_MENU_ID", "WEB_00020");

                    iRtn = biz.SetDictionaryInfo(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('WEB_00020'); parent.fn_ModalCloseDiv(); ";
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