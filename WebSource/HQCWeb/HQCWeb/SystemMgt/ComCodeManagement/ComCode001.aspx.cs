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

namespace HQCWeb.SystemMgt.ComCodeManagement
{
    public partial class ComCode001 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        Biz.SystemManagement.ComCodeMgt biz = new Biz.SystemManagement.ComCodeMgt();
        
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
            lbComType.Text  = Dictionary_Data.SearchDic("COMM_TYPE", bp.g_language);
            lbComCD.Text    = Dictionary_Data.SearchDic("COMM_CD", bp.g_language);
            lbComNM.Text    = Dictionary_Data.SearchDic("COMM_DESC", bp.g_language);
            lbComSeq.Text   = Dictionary_Data.SearchDic("COMM_SEQ", bp.g_language);

            lbWorkName.Text = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language);
        }
        #endregion
        
        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strRtn = string.Empty;
            int iRtn = 0;
            string strRtnValChk = string.Empty;
            string strScript = string.Empty;


            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                strDBName = "GPDB";
                strQueryID = "ComCodeData.Get_ComCodeCD_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("COMM_TYPE", txtComType.Text);
                sParamIDChk.Add("COMM_CD", txtComCD.Text);

                strRtn = biz.GetComTypeCDValChk(strDBName, strQueryID, sParamIDChk);

                if (strRtn == "0")
                {
                    strDBName = "GPDB";
                    strQueryID = "ComCodeData.Set_ComCodeInfo";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();

                    sParam.Add("COMM_TYPE", txtComType.Text);
                    sParam.Add("COMM_CD", txtComCD.Text);
                    sParam.Add("COMM_DESC", txtComNM.Text);
                    sParam.Add("COMM_SEQ", txtComSeq.Text);
                    sParam.Add("USE_YN", "Y");
                    sParam.Add("REG_ID", bp.g_userid.ToString());                    
                    sParam.Add("CUD_TYPE", "C");
                    sParam.Add("CUR_MENU_ID", "WEB_00050");

                    //  등록 서비스 작성
                    iRtn = biz.SetComCodeInfo(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('WEB_00050'); parent.fn_ModalCloseDiv(); ";
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