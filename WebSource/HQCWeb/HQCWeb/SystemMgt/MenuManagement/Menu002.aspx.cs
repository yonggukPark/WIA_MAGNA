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
    public partial class Menu002 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                SetControl();

                SetTitle();
            }
        }

        #region SetTitle
        private void SetTitle()
        {
            lbMenuID.Text       = Dictionary_Data.SearchDic("MENU_ID", bp.g_language);
            lbMenuNM.Text       = Dictionary_Data.SearchDic("MENU_NM", bp.g_language);
            lbMenuParentID.Text = Dictionary_Data.SearchDic("MENU_PARENT_ID", bp.g_language);
            lbMenuLevel.Text    = Dictionary_Data.SearchDic("MENU_LEVEL", bp.g_language);
            lbAssamblyID.Text   = Dictionary_Data.SearchDic("ASSEMBLY_ID", bp.g_language);
            lbViewID.Text       = Dictionary_Data.SearchDic("VIEW_ID", bp.g_language);
            lbWorkName.Text     = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language);

            lbPopupYN.Text        = Dictionary_Data.SearchDic("POPUP_YN", bp.g_language);
            lbUseYN.Text        = Dictionary_Data.SearchDic("USE_YN", bp.g_language);
            lbApproval.Text     = Dictionary_Data.SearchDic("APPROVAL_FLAG", bp.g_language);
            lbMenuIDX.Text      = Dictionary_Data.SearchDic("MENU_IDX", bp.g_language);
        }
        #endregion

        #region SetControl
        private void SetControl()
        {
            ddlUseYN.Items.Clear();
            ddlApprovalYN.Items.Clear();

            ddlUseYN.Items.Add(new ListItem("예", "Y"));
            ddlUseYN.Items.Add(new ListItem("아니오", "N"));

            ddlPopupYN.Items.Add(new ListItem("아니오", "N"));
            ddlPopupYN.Items.Add(new ListItem("예", "Y"));

            ddlApprovalYN.Items.Add(new ListItem("예", "Y"));
            ddlApprovalYN.Items.Add(new ListItem("아니오", "N"));

            ddlUseYN.SelectedIndex = 0;
            ddlApprovalYN.SelectedIndex = 1;

            string strScript = string.Empty;
            int iMenuLevel = 0;

            DataSet ds = new DataSet();

            // 서비스 클래스 작성
            Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            strDBName = "GPDB";
            strQueryID = "MenuData.Get_MenuLevel";

            ds = biz.GetMenuLevel(strDBName, strQueryID);

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count == 1)
                {
                    ddlMenuLevel.Items.Add(new ListItem("2", "2"));
                }
                else
                {
                    for (int i = 1; i < ds.Tables[0].Rows.Count; i++)
                    {
                        iMenuLevel = Convert.ToInt32(ds.Tables[0].Rows[i][0].ToString());

                        ddlMenuLevel.Items.Add(new ListItem(ds.Tables[0].Rows[i][0].ToString(), ds.Tables[0].Rows[i][0].ToString()));
                    }

                    ddlMenuLevel.Items.Add(new ListItem((iMenuLevel + 1).ToString(), (iMenuLevel + 1).ToString()));
                }
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); opener.fn_parentReload(); window.close(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
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

                FW.Data.Parameters sParamParentID = new FW.Data.Parameters();
                sParamParentID.Add("MENU_PARENT_ID", txtMenuParentID.Text);

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                //sParam.Add("menuid",        txtMenuID.Text);
                //sParam.Add("menunm",        txtMenuNM.Text);
                //sParam.Add("menuparentid",  txtMenuParentID.Text);
                //sParam.Add("menulevel",     ddlMenuLevel.SelectedValue);
                //sParam.Add("assemblyid",    txtAssamblyID.Text);
                //sParam.Add("viewid",        txtViewID.Text);
                //sParam.Add("regid",         bp.g_userid.ToString());

                sParam.Add("MENU_ID", txtMenuID.Text);
                sParam.Add("MENU_NM", txtMenuNM.Text);
                sParam.Add("MENU_PARENT_ID", txtMenuParentID.Text);
                sParam.Add("MENU_LEVEL", ddlMenuLevel.SelectedValue);
                sParam.Add("ASSEMBLY_ID", txtAssamblyID.Text);
                sParam.Add("VIEW_ID", txtViewID.Text);
                sParam.Add("USE_YN", ddlUseYN.SelectedValue);
                sParam.Add("POPUP_YN", ddlPopupYN.SelectedValue);
                sParam.Add("MENU_IDX", iMenuIDX.ToString());
                sParam.Add("APPROVAL_FLAG", ddlApprovalYN.SelectedValue);
                sParam.Add("REG_ID", bp.g_userid.ToString());
                sParam.Add("CUD_TYPE", "C");
                sParam.Add("CUR_MENU_ID", "WEB_00010");


                // 기존 아이디 체크 서비스 클래스 작성
                Biz.SystemManagement.MenuMgt biz = new Biz.SystemManagement.MenuMgt();

                strDBName = "GPDB";
                strQueryID = "MenuData.Get_MenuID_ValChk";

                strRtn = biz.GetMenuIDValChk(strDBName, strQueryID, sParamIDChk);

                if (strRtn == "0")
                {

                    strDBName = "GPDB";
                    strQueryID = "MenuData.Get_ParentMenuLevel";

                    // 부모 아이디 Level 조회
                    strRtn = biz.GetParentMenuLevel(strDBName, strQueryID, sParamParentID);

                    if (strRtn == "0")
                    {
                        strScript = " alert('입력한 부모 메뉴 아이디가 존재하지 않습니다. 다시 확인하세요.'); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else
                    {
                        if (Convert.ToInt32(strRtn) < (Convert.ToInt32(ddlMenuLevel.SelectedValue) - 1))
                        {
                            strScript = " alert('부모 메뉴 Level은 " + Convert.ToInt32(strRtn) + " 입니다.  등록하려는 Level과 연관성이 없습니다. 다시 확인하세요.'); ";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        }
                        else
                        {

                            strDBName = "GPDB";
                            strQueryID = "MenuData.Set_MenuInfo";

                            //  등록 서비스 작성
                            iRtn = biz.SetMenuInfo(strDBName, strQueryID, sParam);

                            if (iRtn == 1)
                            {
                                strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('WEB_00010'); parent.fn_ModalCloseDiv();  ";
                                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                            }
                            else
                            {
                                strScript = " alert('등록에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                            }
                        }
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