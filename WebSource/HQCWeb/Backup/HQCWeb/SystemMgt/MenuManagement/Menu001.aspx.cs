using HQC.FW.Common;
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

namespace HQCWeb.SystemMgt.MenuManagement
{
    public partial class Menu001 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        protected string strVal = string.Empty;

        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetCon();

                SetTitle();
            }
        }

        #region SetCon
        private void SetCon()
        {
            ddlUseYN.Items.Clear();

            ddlUseYN.Items.Add(new ListItem("예", "Y"));
            ddlUseYN.Items.Add(new ListItem("아니오", "N"));

            ddlUseYN.SelectedIndex = 0;
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbMenuID.Text       = Dictionary_Data.SearchDic("MENU_ID", bp.g_language);
            lbMenuNM.Text       = Dictionary_Data.SearchDic("MENU_NM", bp.g_language);
            lbAssamblyID.Text   = Dictionary_Data.SearchDic("ASSEMBLY_ID", bp.g_language);
            lbViewID.Text       = Dictionary_Data.SearchDic("VIEW_ID", bp.g_language);            

            lbWorkName.Text     = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language);
            lbUseYN.Text        = Dictionary_Data.SearchDic("USE_YN", bp.g_language);
            lbMenuIDX.Text      = Dictionary_Data.SearchDic("MENU_IDX", bp.g_language);
        }
        #endregion

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string strRtn = string.Empty;
            int iRtn = 0;
            string strRtnValChk = string.Empty;
            string strScript = string.Empty;
            string strDBName = string.Empty;
            string strQueryID = string.Empty;
            
            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                int iMenuIDX = 0;

                if (txtMenuIDX.Text == "")
                {
                    iMenuIDX = 0;
                }
                else
                {
                    iMenuIDX = Convert.ToInt32(txtMenuIDX.Text);
                }

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("MENU_ID", txtMenuID.Text);

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("MENU_ID", txtMenuID.Text);
                sParam.Add("MENU_NM", txtMenuNM.Text);
                sParam.Add("MENU_PARENT_ID", "");
                sParam.Add("MENU_LEVEL", "1");
                sParam.Add("ASSEMBLY_ID", txtAssamblyID.Text);
                sParam.Add("VIEW_ID", txtViewID.Text);
                sParam.Add("USE_YN", ddlUseYN.SelectedValue);
                sParam.Add("MENU_IDX", iMenuIDX.ToString());
                sParam.Add("REG_ID",            bp.g_userid.ToString());
                sParam.Add("CUD_TYPE", "C");
                sParam.Add("CUR_MENU_ID", "WEB_00010");
                //

                // 기존 아이디 체크 서비스 클래스 작성
                Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

                strDBName = "GPDB";
                strQueryID = "MenuData.Get_MenuID_ValChk";
                strRtn = biz.GetMenuIDValChk(strDBName, strQueryID, sParamIDChk);

                if (strRtn == "0")
                {
                    strDBName = "GPDB";
                    strQueryID = "MenuData.Set_MenuInfo";
                    //  등록 서비스 작성
                    iRtn = biz.SetMenuInfo(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('WEB_00010'); parent.fn_ModalCloseDiv(); ";
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

        public bool SetControlValChk_2(System.Web.UI.MasterPage mp, string strContentPlaceHolderName)
        {
            System.Web.UI.MasterPage dfMaster = new System.Web.UI.MasterPage();

            dfMaster = mp;

            bool bRtnVal = true;

            ContentPlaceHolder cph = (ContentPlaceHolder)dfMaster.FindControl(strContentPlaceHolderName);

            if (cph != null)
            {
                foreach (Control ctrl in cph.Controls)
                {
                    if (ctrl is TextBox)
                    {
                        bRtnVal = su.strChk(((TextBox)(ctrl)).Text.ToLower());

                        if (bRtnVal)
                        {
                            bRtnVal = true;
                        }
                        else
                        {
                            bRtnVal = false;

                            break;
                        }
                    }
                }
            }
            
            return bRtnVal;
        }



        public bool SetControlValChk_3(Control Page, string strContentPlaceHolderName)
        {
            bool bRtnVal = true;

            ContentPlaceHolder cph = (ContentPlaceHolder)this.Page.Master.FindControl(strContentPlaceHolderName);

            if (cph != null)
            {
                foreach (Control ctrl in Page.Controls)
                {
                    if (ctrl is TextBox)
                    {
                        if (Page.ClientID.ToString() == strContentPlaceHolderName)
                        {
                            bRtnVal = su.strChk(((TextBox)(ctrl)).Text.ToLower());

                            if (bRtnVal)
                            {
                                bRtnVal = true;
                            }
                            else
                            {
                                bRtnVal = false;

                                break;
                            }
                        }
                    }
                    else
                    {
                        if (bRtnVal)
                        {
                            if (ctrl.Controls.Count > 0)
                            {
                                SetControlValChk_3(ctrl, strContentPlaceHolderName);
                            }
                        }
                    }
                }
            }

            return bRtnVal;
        }
    }
}