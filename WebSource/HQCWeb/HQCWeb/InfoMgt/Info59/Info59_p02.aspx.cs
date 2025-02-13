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

namespace HQCWeb.InfoMgt.Info59
{
    public partial class Info59_p02 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        protected string strVal = string.Empty;

        // 암복호화 키값 셋팅
        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        // 비지니스 클래스 작성
        Biz.InfoManagement.Info59 biz = new Biz.InfoManagement.Info59();

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
            DataSet ds = new DataSet();

            strDBName = "GPDB";
            strQueryID = "Info59Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", "");
            param.Add("LINE_CD", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                {
                    ddlItemGroup.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                {
                    ddlItem02Yn.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    ddlItem03Yn.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    ddlItem04Yn.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    ddlItem05Yn.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    ddlItem08Yn.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    ddlItem09Yn.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    ddlItem10Yn.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    ddlItem12Yn.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    ddlItem13Yn.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    ddlItem14Yn.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    ddlUseYn.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    rbListChkAll.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                }
                rbListChkAll.SelectedValue = "N";
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbCarType.Text = Dictionary_Data.SearchDic("CAR_TYPE", bp.g_language);
            lbStnCd.Text = Dictionary_Data.SearchDic("STN_CD", bp.g_language);
            lbWorkCd.Text = Dictionary_Data.SearchDic("WORK_CD", bp.g_language);
            lbDevNm.Text = Dictionary_Data.SearchDic("DEV_NM", bp.g_language);
            lbPSet.Text = Dictionary_Data.SearchDic("P_SET", bp.g_language);
            lbTorqueType.Text = Dictionary_Data.SearchDic("TORQUE_TYPE", bp.g_language);
            lbItemGroup.Text = Dictionary_Data.SearchDic("ITEM_GROUP", bp.g_language);
            
            lbItem02.Text = Dictionary_Data.SearchDic("ITEM_02", bp.g_language);
            lbItem03.Text = Dictionary_Data.SearchDic("ITEM_03", bp.g_language);
            lbItem04.Text = Dictionary_Data.SearchDic("ITEM_04", bp.g_language);
            lbItem05.Text = Dictionary_Data.SearchDic("ITEM_05", bp.g_language);
            lbItem08.Text = Dictionary_Data.SearchDic("ITEM_08", bp.g_language);
            lbItem09.Text = Dictionary_Data.SearchDic("ITEM_09", bp.g_language);
            lbItem10.Text = Dictionary_Data.SearchDic("ITEM_10", bp.g_language);
            lbItem12.Text = Dictionary_Data.SearchDic("ITEM_12", bp.g_language);
            lbItem13.Text = Dictionary_Data.SearchDic("ITEM_13", bp.g_language);
            lbItem14.Text = Dictionary_Data.SearchDic("ITEM_14", bp.g_language);

            lbChkAll.Text = Dictionary_Data.SearchDic("CHK_ALL", bp.g_language);
            lbItem02Yn.Text = Dictionary_Data.SearchDic("CHK_YN", bp.g_language);
            lbItem03Yn.Text = Dictionary_Data.SearchDic("CHK_YN", bp.g_language);
            lbItem04Yn.Text = Dictionary_Data.SearchDic("CHK_YN", bp.g_language);
            lbItem05Yn.Text = Dictionary_Data.SearchDic("CHK_YN", bp.g_language);
            lbItem08Yn.Text = Dictionary_Data.SearchDic("CHK_YN", bp.g_language);
            lbItem09Yn.Text = Dictionary_Data.SearchDic("CHK_YN", bp.g_language);
            lbItem10Yn.Text = Dictionary_Data.SearchDic("CHK_YN", bp.g_language);
            lbItem12Yn.Text = Dictionary_Data.SearchDic("CHK_YN", bp.g_language);
            lbItem13Yn.Text = Dictionary_Data.SearchDic("CHK_YN", bp.g_language);
            lbItem14Yn.Text = Dictionary_Data.SearchDic("CHK_YN", bp.g_language);

            lbUseYn.Text = Dictionary_Data.SearchDic("USE_YN", bp.g_language);

            // 등록일경우
            lbWorkName.Text = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language); // 등록
        }
        #endregion

        #region GetData
        protected void GetData()
        {
            string strScript = string.Empty;
            string strTableName = string.Empty;
            string strErrMessage = string.Empty;

            DataSet ds = new DataSet();

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Info59Data.Get_ItemGroupInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("STN_CD", strSplitValue[3].ToString());
            sParam.Add("CAR_TYPE", strSplitValue[4].ToString());
            sParam.Add("WORK_SEQ", strSplitValue[5].ToString());
            sParam.Add("DEV_ID", strSplitValue[6].ToString());

            sParam.Add("CUR_MENU_ID", "Info59");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {

                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strErrMessage = ds.Tables[0].Rows[0][1].ToString();

                    strScript = "fn_ErrorMessage('" + strErrMessage + "');";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
                else
                {
                    lbGetShopCd.Text = ds.Tables[0].Rows[0]["SHOP_CD"].ToString();
                    lbGetLineCd.Text = ds.Tables[0].Rows[0]["LINE_CD"].ToString();
                    lbGetCarType.Text = ds.Tables[0].Rows[0]["CAR_TYPE"].ToString();
                    lbGetStnCd.Text = ds.Tables[0].Rows[0]["STN_CD"].ToString();
                    lbGetWorkCd.Text = ds.Tables[0].Rows[0]["WORK_CD"].ToString();
                    lbGetDevNm.Text = ds.Tables[0].Rows[0]["DEV_NM"].ToString();
                    lbGetPSet.Text = ds.Tables[0].Rows[0]["P_SET"].ToString();
                    lbGetTorqueType.Text = ds.Tables[0].Rows[0]["TORQUE_TYPE"].ToString();

                    txtItem02.Text = ds.Tables[0].Rows[0]["ITEM_02"].ToString();
                    txtItem03.Text = ds.Tables[0].Rows[0]["ITEM_03"].ToString();
                    txtItem04.Text = ds.Tables[0].Rows[0]["ITEM_04"].ToString();
                    txtItem05.Text = ds.Tables[0].Rows[0]["ITEM_05"].ToString();
                    txtItem08.Text = ds.Tables[0].Rows[0]["ITEM_08"].ToString();
                    txtItem09.Text = ds.Tables[0].Rows[0]["ITEM_09"].ToString();
                    txtItem10.Text = ds.Tables[0].Rows[0]["ITEM_10"].ToString();
                    txtItem12.Text = ds.Tables[0].Rows[0]["ITEM_12"].ToString();
                    txtItem13.Text = ds.Tables[0].Rows[0]["ITEM_13"].ToString();
                    txtItem14.Text = ds.Tables[0].Rows[0]["ITEM_14"].ToString();
                    
                    ddlItem02Yn.SelectedValue = ds.Tables[0].Rows[0]["ITEM_02_YN"].ToString();
                    ddlItem03Yn.SelectedValue = ds.Tables[0].Rows[0]["ITEM_03_YN"].ToString();
                    ddlItem04Yn.SelectedValue = ds.Tables[0].Rows[0]["ITEM_04_YN"].ToString();
                    ddlItem05Yn.SelectedValue = ds.Tables[0].Rows[0]["ITEM_05_YN"].ToString();
                    ddlItem08Yn.SelectedValue = ds.Tables[0].Rows[0]["ITEM_08_YN"].ToString();
                    ddlItem09Yn.SelectedValue = ds.Tables[0].Rows[0]["ITEM_09_YN"].ToString();
                    ddlItem10Yn.SelectedValue = ds.Tables[0].Rows[0]["ITEM_10_YN"].ToString();
                    ddlItem12Yn.SelectedValue = ds.Tables[0].Rows[0]["ITEM_12_YN"].ToString();
                    ddlItem13Yn.SelectedValue = ds.Tables[0].Rows[0]["ITEM_13_YN"].ToString();
                    ddlItem14Yn.SelectedValue = ds.Tables[0].Rows[0]["ITEM_14_YN"].ToString();

                    ddlUseYn.SelectedValue = ds.Tables[0].Rows[0]["USE_YN"].ToString();
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
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Info59'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
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
                string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

                string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

                strDBName = "GPDB";
                strQueryID = "Info59Data.Set_ItemGroupInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                sParam.Add("LINE_CD", strSplitValue[2].ToString());
                sParam.Add("STN_CD", strSplitValue[3].ToString());
                sParam.Add("CAR_TYPE", strSplitValue[4].ToString());
                sParam.Add("WORK_CD", strSplitValue[8].ToString());
                sParam.Add("DEV_ID", strSplitValue[6].ToString());
                sParam.Add("P_SET", strSplitValue[7].ToString());
                
                sParam.Add("ITEM_02", txtItem02.Text);
                sParam.Add("ITEM_03", txtItem03.Text);
                sParam.Add("ITEM_04", txtItem04.Text);
                sParam.Add("ITEM_05", txtItem05.Text);
                sParam.Add("ITEM_08", txtItem08.Text);
                sParam.Add("ITEM_09", txtItem09.Text);
                sParam.Add("ITEM_10", txtItem10.Text);
                sParam.Add("ITEM_12", txtItem12.Text);
                sParam.Add("ITEM_13", txtItem13.Text);
                sParam.Add("ITEM_14", txtItem14.Text);
                sParam.Add("ITEM_02_YN", ddlItem02Yn.SelectedValue);
                sParam.Add("ITEM_03_YN", ddlItem03Yn.SelectedValue);
                sParam.Add("ITEM_04_YN", ddlItem04Yn.SelectedValue);
                sParam.Add("ITEM_05_YN", ddlItem05Yn.SelectedValue);
                sParam.Add("ITEM_08_YN", ddlItem08Yn.SelectedValue);
                sParam.Add("ITEM_09_YN", ddlItem09Yn.SelectedValue);
                sParam.Add("ITEM_10_YN", ddlItem10Yn.SelectedValue);
                sParam.Add("ITEM_12_YN", ddlItem12Yn.SelectedValue);
                sParam.Add("ITEM_13_YN", ddlItem13Yn.SelectedValue);
                sParam.Add("ITEM_14_YN", ddlItem14Yn.SelectedValue);
                sParam.Add("USE_YN", ddlUseYn.SelectedValue);
                sParam.Add("ITEM_GROUP", ddlItemGroup.SelectedValue);

                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Info59");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                // 등록 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('Info59'); parent.fn_ModalCloseDiv(); ";
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
                strScript = " fn_ExError(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion
        
        #region rbListChkAll_SelectedIndexChanged
        protected void rbListChkAll_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 선택된 값 가져오기
            string selValue = rbListChkAll.SelectedValue;

            ddlItem02Yn.SelectedValue = selValue;
            ddlItem03Yn.SelectedValue = selValue;
            ddlItem04Yn.SelectedValue = selValue;
            ddlItem05Yn.SelectedValue = selValue;
            ddlItem08Yn.SelectedValue = selValue;
            ddlItem09Yn.SelectedValue = selValue;
            ddlItem10Yn.SelectedValue = selValue;
            ddlItem12Yn.SelectedValue = selValue;
            ddlItem13Yn.SelectedValue = selValue;
            ddlItem14Yn.SelectedValue = selValue;
        }
        #endregion
    }
}
