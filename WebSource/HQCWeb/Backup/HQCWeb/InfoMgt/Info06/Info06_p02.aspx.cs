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

namespace HQCWeb.InfoMgt.Info06
{
    public partial class Info06_p02 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        // 비지니스 클래스 작성
        Biz.InfoManagement.Info06 biz = new Biz.InfoManagement.Info06();

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
            lbWorkName.Text = Dictionary_Data.SearchDic("DETAIL", bp.g_language); // 상세

            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbStnCd.Text = Dictionary_Data.SearchDic("STN_CD", bp.g_language);
            lbStnNm.Text = Dictionary_Data.SearchDic("STN_NM", bp.g_language);
            lbNgCd.Text = Dictionary_Data.SearchDic("NG_CD", bp.g_language);
            lbMulti.Text = Dictionary_Data.SearchDic("MULTIPLI", bp.g_language);
            lbStn.Text = Dictionary_Data.SearchDic("STN", bp.g_language);
            lbMergeStn.Text = Dictionary_Data.SearchDic("MERGE_STN_CD", bp.g_language);
            lbReworkStn.Text = Dictionary_Data.SearchDic("REWORK_STN_CD", bp.g_language);
            lbPrStn.Text = Dictionary_Data.SearchDic("P_R_STN_CD", bp.g_language);
            lbTotStn.Text = Dictionary_Data.SearchDic("TOTAL_STN_CD", bp.g_language);
            lbRemark1.Text = Dictionary_Data.SearchDic("REMARK1", bp.g_language);
            lbRemark2.Text = Dictionary_Data.SearchDic("REMARK2", bp.g_language);
            lbViewYn.Text = Dictionary_Data.SearchDic("VIEW_YN", bp.g_language);
            lbUseYn.Text = Dictionary_Data.SearchDic("USE_YN", bp.g_language);
            chkListStn.Items[0].Text = Dictionary_Data.SearchDic("REINPUT", bp.g_language);
            chkListStn.Items[1].Text = Dictionary_Data.SearchDic("FINISH", bp.g_language);
            chkListStn.Items[2].Text = Dictionary_Data.SearchDic("COM_PROD", bp.g_language);
            chkListStn.Items[3].Text = Dictionary_Data.SearchDic("TORQUE", bp.g_language);
            chkListStn.Items[4].Text = Dictionary_Data.SearchDic("INSPECTION", bp.g_language);
            chkListStn.Items[5].Text = Dictionary_Data.SearchDic("INPUT", bp.g_language);
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
            //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();
            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Info06Data.Get_StnInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("STN_CD", strSplitValue[3].ToString());

            sParam.Add("CUR_MENU_ID", "Info06");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetStnList(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                //GetData 에서 호출(STN 콤보 초기 설정값 필요)
                GetDDL(ds.Tables[0].Rows[0]["SHOP_CD"].ToString(), ds.Tables[0].Rows[0]["LINE_CD"].ToString());
                lbGetStnCd.Text = ds.Tables[0].Rows[0]["STN_CD"].ToString();
                txtStnNm.Text = ds.Tables[0].Rows[0]["STN_NM"].ToString();
                txtNgCd.Text = ds.Tables[0].Rows[0]["NG_CD"].ToString();
                txtMulti.Text = ds.Tables[0].Rows[0]["MULTIPLI"].ToString();
                //"REINPUT", "FINISH", "COM_PROD", "TORQUE", "INSPECTION", "INPUT"
                chkListStn.Items[0].Selected = (ds.Tables[0].Rows[0]["REINPUT"].ToString().Equals("1"))? true : false;
                chkListStn.Items[1].Selected = (ds.Tables[0].Rows[0]["FINISH"].ToString().Equals("1")) ? true : false;
                chkListStn.Items[2].Selected = (ds.Tables[0].Rows[0]["COM_PROD"].ToString().Equals("1")) ? true : false;
                chkListStn.Items[3].Selected = (ds.Tables[0].Rows[0]["TORQUE"].ToString().Equals("1")) ? true : false;
                chkListStn.Items[4].Selected = (ds.Tables[0].Rows[0]["INSPECTION"].ToString().Equals("1")) ? true : false;
                chkListStn.Items[5].Selected = (ds.Tables[0].Rows[0]["INPUT"].ToString().Equals("1")) ? true : false;
                ddlReworkStn.SelectedValue = ds.Tables[0].Rows[0]["REWORK_STN_CD"].ToString();
                ddlPrStn.SelectedValue     = ds.Tables[0].Rows[0]["P_R_STN_CD"].ToString();
                ddlTotStn.SelectedValue    = ds.Tables[0].Rows[0]["TOTAL_STN_CD"].ToString();
                ddlMergeStn.SelectedValue  = ds.Tables[0].Rows[0]["MERGE_STN_CD"].ToString();
                ddlViewYn.SelectedValue    = ds.Tables[0].Rows[0]["VIEW_YN"].ToString();
                ddlUseYN.SelectedValue     = ds.Tables[0].Rows[0]["USE_YN"].ToString();
                txtRemark1.Text = ds.Tables[0].Rows[0]["REMARK1"].ToString();
                txtRemark2.Text = ds.Tables[0].Rows[0]["REMARK2"].ToString();

                if (ds.Tables[0].Rows[0]["DEL_YN"].ToString().Equals("Y"))
                {
                    aModify.Visible = false;
                    aDelete.Visible = false;
                    aRestore.Visible = true;

                    txtStnNm.Enabled =     false;
                    txtNgCd.Enabled =      false;
                    txtMulti.Enabled =     false;
                    chkListStn.Enabled =   false;
                    ddlReworkStn.Enabled = false;
                    ddlPrStn.Enabled =     false;  
                    ddlTotStn.Enabled =    false;
                    ddlMergeStn.Enabled =  false;
                    ddlViewYn.Enabled =    false;
                    ddlUseYN.Enabled =     false;
                    txtRemark1.Enabled =   false;
                    txtRemark2.Enabled =   false;
                }
                else
                {
                    aModify.Visible = true;
                    aDelete.Visible = true;
                    aRestore.Visible = false;

                    txtStnNm.Enabled =     true;
                    txtNgCd.Enabled =      true;
                    txtMulti.Enabled =     true;
                    chkListStn.Enabled =   true;
                    ddlReworkStn.Enabled = true;
                    ddlPrStn.Enabled =     true;
                    ddlTotStn.Enabled =    true;
                    ddlMergeStn.Enabled =  true;
                    ddlViewYn.Enabled =    true;
                    ddlUseYN.Enabled =     true;
                    txtRemark1.Enabled =   true;
                    txtRemark2.Enabled =   true;
                }

                ddlShopCd.Enabled = false;
                ddlLineCd.Enabled = false;

                // 변경전 데이터 셋팅
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    string strColumns = string.Empty;

                    strColumns = ds.Tables[0].Columns[i].ToString();

                    if (strDetailValue == "")
                    {
                        strDetailValue = strColumns + ":" + ds.Tables[0].Rows[0][strColumns].ToString();
                    }
                    else
                    {
                        strDetailValue += "," + strColumns + ":" + ds.Tables[0].Rows[0][strColumns].ToString();
                    }
                }

                (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = cy.Encrypt(strDetailValue);
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Info06'); parent.fn_ModalCloseDiv(); ";
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

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strUVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string strUValue = cy.Decrypt(strUVal);

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                strDBName = "GPDB";
                strQueryID = "Info06Data.Set_StnInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                sParam.Add("LINE_CD", strSplitValue[2].ToString());
                sParam.Add("STN_CD", strSplitValue[3].ToString());

                sParam.Add("STN_NM", txtStnNm.Text);
                sParam.Add("NG_CD", txtNgCd.Text);
                sParam.Add("MULTIPLI", txtMulti.Text);
                sParam.Add("REINPUT", (chkListStn.Items[0].Selected) ? "1" : "0");
                sParam.Add("FINISH", (chkListStn.Items[1].Selected) ? "1" : "0");
                sParam.Add("COM_PROD", (chkListStn.Items[2].Selected) ? "1" : "0");
                sParam.Add("TORQUE", (chkListStn.Items[3].Selected) ? "1" : "0");
                sParam.Add("INSPECTION", (chkListStn.Items[4].Selected) ? "1" : "0");
                sParam.Add("INPUT", (chkListStn.Items[5].Selected) ? "1" : "0");
                sParam.Add("MERGE_STN_CD", ddlMergeStn.SelectedValue);
                sParam.Add("REWORK_STN_CD", ddlReworkStn.SelectedValue);
                sParam.Add("P_R_STN_CD", ddlPrStn.SelectedValue);
                sParam.Add("TOTAL_STN_CD", ddlTotStn.SelectedValue);
                sParam.Add("VIEW_YN", ddlViewYn.SelectedValue);
                sParam.Add("USE_YN", ddlUseYN.SelectedValue);
                sParam.Add("REMARK1", txtRemark1.Text);
                sParam.Add("REMARK2", txtRemark2.Text);
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Info06");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA", strUValue);                  // 이전 데이터 셋팅

                // 수정 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall('Info06'); parent.fn_ModalCloseDiv(); ";
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
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string strDValue = cy.Decrypt(strDVal);

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                strDBName = "GPDB";
                strQueryID = "Info06Data.Set_StnInfoDel";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                sParam.Add("LINE_CD", strSplitValue[2].ToString());
                sParam.Add("STN_CD", strSplitValue[3].ToString());

                sParam.Add("FLAG", "Y");
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Info06");              // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA", strDValue);                  // 이전 데이터 셋팅

                // 삭제 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('Info06'); parent.fn_ModalCloseDiv(); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
                else
                {
                    strScript = " alert('삭제에 실패하였습니다. 관리자에게 문의바립니다.'); ";
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

        #region btnRestore_Click
        protected void btnRestore_Click(object sender, EventArgs e)
        {
            int iRtn = 0;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string strDValue = cy.Decrypt(strDVal);

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                strDBName = "GPDB";
                strQueryID = "Info06Data.Set_StnInfoDel";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                sParam.Add("LINE_CD", strSplitValue[2].ToString());
                sParam.Add("STN_CD", strSplitValue[3].ToString());

                sParam.Add("FLAG", "N");
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "R");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Info06");              // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA", strDValue);                  // 이전 데이터 셋팅

                // 복원 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상복원 되었습니다.');  parent.fn_ModalReloadCall('Info06'); parent.fn_ModalCloseDiv(); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
                else
                {
                    strScript = " alert('복원에 실패하였습니다. 관리자에게 문의바립니다.'); ";
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

        #region GetDDL
        protected void GetDDL(string shopCd, string lineCd)
        {
            //GetData 에서 호출(STN 콤보 초기 설정값 필요)
            DataSet ds = new DataSet();

            ddlShopCd.Items.Add(new ListItem("", ""));
            ddlLineCd.Items.Add(new ListItem("", ""));
            ddlReworkStn.Items.Add(new ListItem("", ""));
            ddlPrStn.Items.Add(new ListItem("", ""));
            ddlTotStn.Items.Add(new ListItem("", ""));
            ddlMergeStn.Items.Add(new ListItem("", ""));

            strDBName = "GPDB";
            strQueryID = "Info06Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", "H20");
            param.Add("SHOP_CD", shopCd);
            param.Add("LINE_CD", lineCd);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlShopCd.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                }
                ddlShopCd.SelectedValue = shopCd;
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    ddlLineCd.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                }
                ddlLineCd.SelectedValue = lineCd;
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    ddlReworkStn.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    ddlPrStn.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    ddlTotStn.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    ddlMergeStn.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    ddlUseYN.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                    ddlViewYn.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                }
            }
        }
        #endregion
    }
}
