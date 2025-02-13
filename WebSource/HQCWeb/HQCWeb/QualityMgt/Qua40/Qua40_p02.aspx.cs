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

namespace HQCWeb.QualityMgt.Qua40
{
    public partial class Qua40_p02 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        // 비지니스 클래스 작성
        Biz.QualityManagement.Qua40 biz = new Biz.QualityManagement.Qua40();

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
            DataSet ds = new DataSet();

            ddlDCode.Items.Add(new ListItem("선택하세요.", ""));
            ddlDReasonCode.Items.Add(new ListItem("선택하세요.", ""));
            ddlDRespCd.Items.Add(new ListItem("선택하세요.", ""));

            strDBName = "GPDB";
            strQueryID = "Qua40Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    ddlDCode.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                {
                    ddlDReasonCode.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                {
                    ddlDRespCd.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                }
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            // 수정 또는 삭제
            lbWorkName.Text = Dictionary_Data.SearchDic("MATCH_HI", bp.g_language) + " " + Dictionary_Data.SearchDic("DETAIL", bp.g_language); // 상세

            lbSerialNoCBef.Text = Dictionary_Data.SearchDic("BEFORE_MOD", bp.g_language);
            lbSerialNoCAft.Text = Dictionary_Data.SearchDic("AFTER_MOD", bp.g_language);
            lbReworkMsg.Text = Dictionary_Data.SearchDic("REWORK_MSG", bp.g_language);
            lbDCode.Text = Dictionary_Data.SearchDic("D_CODE", bp.g_language);
            lbDReasonCode.Text = Dictionary_Data.SearchDic("D_REASON_CODE", bp.g_language);
            lbDRespCd.Text = Dictionary_Data.SearchDic("D_RESP_CD", bp.g_language);
        }
        #endregion

        #region GetData
        protected void GetData()
        {
            string strScript = string.Empty;

            DataSet ds = new DataSet();

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Qua40Data.Get_MatchInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("SERIAL_NO", strSplitValue[3].ToString());
            sParam.Add("SHOP_CD_C", strSplitValue[4].ToString());
            sParam.Add("LINE_CD_C", strSplitValue[5].ToString());
            sParam.Add("SERIAL_NO_C", strSplitValue[6].ToString());
            sParam.Add("WORK_FLAG", strSplitValue[7].ToString());
            sParam.Add("RPT_DATE", strSplitValue[8].ToString());

            sParam.Add("CUR_MENU_ID", "Qua40");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {   
                //수정되지 않은 데이터
                if (ds.Tables[0].Rows[0]["MOD_FLAG"].ToString() == "N")
                {
                    lbGetSerialNoCBef.Text = ds.Tables[0].Rows[0]["SERIAL_NO_C"].ToString();
                    ddlDCode.SelectedValue = ds.Tables[0].Rows[0]["D_CODE"].ToString();
                    ddlDReasonCode.SelectedValue = ds.Tables[0].Rows[0]["D_REASON_CODE"].ToString();
                    ddlDRespCd.SelectedValue = ds.Tables[0].Rows[0]["D_RESP_CD"].ToString();

                    if (strSplitValue[7].ToString() == "D")
                    {
                        aModify.Visible = false;
                        aDelete.Visible = false;
                        aRestore.Visible = true;
                    }
                    else
                    {
                        aModify.Visible = true;
                        aDelete.Visible = true;
                        aRestore.Visible = false;
                    }

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
                    strScript = " alert('수정된 데이터이므로 복원할 수 없습니다.'); parent.fn_ModalReloadCall('Qua40'); parent.fn_ModalCloseDiv(); ";
                    ScriptManager.RegisterStartupScript(Page, typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
                
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Qua40'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnModify_Click
        protected void btnModify_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();//저장시 다중플래그가 나와서 구분해줘야 함
            int iRtn = 0;
            string strRtn = string.Empty;
            string strRtn2 = string.Empty;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strUVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strUValue = cy.Decrypt(strUVal).Split('/');

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                strDBName = "GPDB";
                strQueryID = "Qua40Data.Get_MatchID_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("PLANT_CD", strSplitValue[0].ToString());
                sParamIDChk.Add("SHOP_CD", strSplitValue[1].ToString());
                sParamIDChk.Add("LINE_CD", strSplitValue[2].ToString());
                sParamIDChk.Add("SERIAL_NO", strSplitValue[3].ToString());
                sParamIDChk.Add("SHOP_CD_C", strSplitValue[4].ToString());
                sParamIDChk.Add("LINE_CD_C", strSplitValue[5].ToString());
                sParamIDChk.Add("SERIAL_NO_C", strSplitValue[6].ToString());
                sParamIDChk.Add("SERIAL_NO_C_AFT", txtSerialNoCAft.Text);

                // 아이디 체크 비지니스 메서드 작성
                ds = biz.GetDataSet(strDBName, strQueryID, sParamIDChk);

                if (ds.Tables.Count > 0)
                {
                    strRtn = ds.Tables[0].Rows[0]["VAL_CHK_1"].ToString();
                    strRtn2 = ds.Tables[0].Rows[0]["VAL_CHK_2"].ToString();

                    if (strRtn != "0" && strRtn2 == "0")
                    {
                        strDBName = "GPDB";
                        strQueryID = "Qua40Data.Set_MatchInfo";

                        FW.Data.Parameters sParam = new FW.Data.Parameters();
                        sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                        sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                        sParam.Add("LINE_CD", strSplitValue[2].ToString());
                        sParam.Add("SERIAL_NO", strSplitValue[3].ToString());
                        sParam.Add("SHOP_CD_C", strSplitValue[4].ToString());
                        sParam.Add("LINE_CD_C", strSplitValue[5].ToString());
                        sParam.Add("SERIAL_NO_C", strSplitValue[6].ToString());

                        sParam.Add("SERIAL_NO_C_AFT", txtSerialNoCAft.Text);
                        sParam.Add("REWORK_MSG", txtReworkMsg.Text);
                        sParam.Add("D_CODE", ddlDCode.SelectedValue);
                        sParam.Add("D_REASON_CODE", ddlDReasonCode.SelectedValue);
                        sParam.Add("D_RESP_CD", ddlDRespCd.SelectedValue);
                        sParam.Add("USER_ID", bp.g_userid.ToString());

                        sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                        sParam.Add("CUD_TYPE", "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                        sParam.Add("CUR_MENU_ID", "Qua40");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                        sParam.Add("PREV_DATA", strUValue[0].ToString());   // 이전 데이터 셋팅

                        // 수정 비지니스 메서드 작성
                        iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                        if (iRtn == 1)
                        {
                            (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                            strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall('Qua40'); parent.fn_ModalCloseDiv(); ";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        }
                        else
                        {
                            strScript = " alert('수정에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        }
                    }
                    else if (strRtn == "0")
                    {
                        strScript = " alert('해당 바코드 데이터가 존재하지 않습니다.'); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else if (strRtn2 != "0")
                    {
                        strScript = " alert('제품 바코드 번호가 중복입니다.'); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
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
            string strRtn = string.Empty;
            int iRtn = 0;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strDValue = cy.Decrypt(strDVal).Split('/');

            strDBName = "GPDB";
            strQueryID = "Qua40Data.Get_MatchDelInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();

            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("SERIAL_NO", strSplitValue[3].ToString());
            sParam.Add("SHOP_CD_C", strSplitValue[4].ToString());
            sParam.Add("LINE_CD_C", strSplitValue[5].ToString());
            sParam.Add("SERIAL_NO_C", strSplitValue[6].ToString());

            strRtn = biz.GetIDValChk(strDBName, strQueryID, sParam);
            
            if (strRtn == "0")
            {
                strDBName = "GPDB";
                strQueryID = "Qua40Data.Del_MatchInfo";

                sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                sParam.Add("LINE_CD", strSplitValue[2].ToString());
                sParam.Add("SERIAL_NO", strSplitValue[3].ToString());
                sParam.Add("SHOP_CD_C", strSplitValue[4].ToString());
                sParam.Add("LINE_CD_C", strSplitValue[5].ToString());
                sParam.Add("SERIAL_NO_C", strSplitValue[6].ToString());

                sParam.Add("REWORK_MSG", txtReworkMsg.Text);
                sParam.Add("D_CODE", ddlDCode.SelectedValue);
                sParam.Add("D_REASON_CODE", ddlDReasonCode.SelectedValue);
                sParam.Add("D_RESP_CD", ddlDRespCd.SelectedValue);
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Qua40");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA", strDValue[0].ToString());   // 이전 데이터 셋팅

                // 삭제 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('Qua40'); parent.fn_ModalCloseDiv(); ";
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
                strScript = " alert('하위 바코드가 존재합니다. 다시 확인해주세요.'); ";
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

            string[] strDValue = cy.Decrypt(strDVal).Split('/');

            strDBName = "GPDB";
            strQueryID = "Qua40Data.Restore_MatchInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("SERIAL_NO", strSplitValue[3].ToString());
            sParam.Add("SHOP_CD_C", strSplitValue[4].ToString());
            sParam.Add("LINE_CD_C", strSplitValue[5].ToString());
            sParam.Add("SERIAL_NO_C", strSplitValue[6].ToString());

            sParam.Add("REWORK_MSG", txtReworkMsg.Text);
            sParam.Add("D_CODE", ddlDCode.SelectedValue);
            sParam.Add("D_REASON_CODE", ddlDReasonCode.SelectedValue);
            sParam.Add("D_RESP_CD", ddlDRespCd.SelectedValue);
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            sParam.Add("CUD_TYPE", "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            sParam.Add("CUR_MENU_ID", "Qua40");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            sParam.Add("PREV_DATA", strDValue[0].ToString());   // 이전 데이터 셋팅

            // 삭제 비지니스 메서드 작성
            iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

            if (iRtn == 1)
            {
                (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                strScript = " alert('정상복구 되었습니다.');  parent.fn_ModalReloadCall('Qua40'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                strScript = " alert('복구에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion
    }
}
