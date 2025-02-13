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

namespace HQCWeb.PlanMgt.Pln02
{
    public partial class Pln02_p02 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        protected string strVal = string.Empty;

        // 암복호화 키값 셋팅
        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        //비즈니스 선언
        Biz.PlanManagement.Pln02 biz = new Biz.PlanManagement.Pln02();

        // 비지니스 클래스 작성
        //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();

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
            lbOrderNo.Text = Dictionary_Data.SearchDic("ORDER_NO", bp.g_language);
            lbPlanDt.Text = Dictionary_Data.SearchDic("PLAN_DT", bp.g_language);
            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbCarType.Text = Dictionary_Data.SearchDic("CAR_TYPE", bp.g_language);
            lbPartNo.Text = Dictionary_Data.SearchDic("PART_NO", bp.g_language);
            lbStatusFlg.Text = Dictionary_Data.SearchDic("STATUS_FLG", bp.g_language);
            lbPlanQty.Text = Dictionary_Data.SearchDic("PLAN_QTY", bp.g_language);
            lbPlanDQty.Text = Dictionary_Data.SearchDic("PLAN_D_QTY", bp.g_language);
            lbPlanNQty.Text = Dictionary_Data.SearchDic("PLAN_N_QTY", bp.g_language);
            //lbFinishFlg.Text = Dictionary_Data.SearchDic("FINISH_FLG", bp.g_language);
            lbPlanCd.Text = Dictionary_Data.SearchDic("PLAN_CD", bp.g_language);
            lbPlanDetailCd.Text = Dictionary_Data.SearchDic("PLAN_DETAIL_CD", bp.g_language);
            lbRemark.Text = Dictionary_Data.SearchDic("REMARK", bp.g_language);
            lbErpSend.Text = Dictionary_Data.SearchDic("ERP_SEND", bp.g_language);

            // 상세내용 확인후 수정 또는 삭제일 경우
            lbWorkName.Text = Dictionary_Data.SearchDic("DETAIL", bp.g_language); // 상세
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
            strQueryID = "Pln02Data.Get_PlanInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("PLAN_DT", strSplitValue[1].ToString().Replace("-", ""));
            sParam.Add("ORDER_NO", strSplitValue[2].ToString());
            sParam.Add("SHOP_CD", "");
            sParam.Add("LINE_CD", "");
            sParam.Add("PLAN_SEQ", "");

            sParam.Add("CUR_MENU_ID", "Pln02");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetPlan(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //팝업창 초기 설정
                GetDDL(ds.Tables[0].Rows[0]["SHOP_CD"].ToString(), ds.Tables[0].Rows[0]["LINE_CD"].ToString());

                lbGetOrderNo.Text = ds.Tables[0].Rows[0]["ORDER_NO"].ToString();
                lbGetPlanDt.Text = ds.Tables[0].Rows[0]["PLAN_DT"].ToString();
                lbGetCarType.Text = ds.Tables[0].Rows[0]["CAR_TYPE"].ToString();
                lbGetPartNo.Text = ds.Tables[0].Rows[0]["PART_NO"].ToString();
                txtPlanQty.Text = ds.Tables[0].Rows[0]["PLAN_QTY"].ToString();

                ddlShopCd.SelectedValue = ds.Tables[0].Rows[0]["SHOP_CD"].ToString();
                ddlLineCd.SelectedValue = ds.Tables[0].Rows[0]["LINE_CD"].ToString();
                txtPlanDQty.Text = ds.Tables[0].Rows[0]["PLAN_D_QTY"].ToString();
                txtPlanNQty.Text = ds.Tables[0].Rows[0]["PLAN_N_QTY"].ToString();
                txtRemark.Text = ds.Tables[0].Rows[0]["REMARK"].ToString();
                ddlStatusFlg.SelectedValue = ds.Tables[0].Rows[0]["STATUS_FLG"].ToString();
                //ddlFinishFlg.SelectedValue = ds.Tables[0].Rows[0]["FINISH_FLG"].ToString();
                ddlPlanCd.SelectedValue = ds.Tables[0].Rows[0]["PLAN_CD"].ToString();
                ddlPlanDetailCd.SelectedValue = ds.Tables[0].Rows[0]["PLAN_DETAIL_CD"].ToString();
                ddlErpSend.SelectedValue = ds.Tables[0].Rows[0]["ERP_SEND"].ToString();
                hidOrderType.Value = ds.Tables[0].Rows[0]["ORDER_TYPE"].ToString();
                hidPlanSeq.Value = ds.Tables[0].Rows[0]["PLAN_SEQ"].ToString();

                //1달전 1일 이후 데이터면 허가, 아니면 비활성
                if (ds.Tables[0].Rows[0]["EDIT_FLAG"].ToString() == "N")
                {
                    txtPlanQty.Enabled = false;
                    txtPlanDQty.Enabled = false;
                    txtPlanNQty.Enabled = false;
                }

                //수정으로 호출되므로 비활성
                ddlShopCd.Enabled = false;
                ddlLineCd.Enabled = false;

                //ERP(ORDER_TYPE = "S") 면 숫자 변경 불가
                if (ds.Tables[0].Rows[0]["ORDER_TYPE"].ToString().Equals("S"))
                    txtPlanQty.Enabled = false;

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
                hidPopDefaultValue2.Value = cy.Encrypt(strDetailValue);
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Pln02'); parent.fn_ModalCloseDiv(); ";
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", strScript, true);
            }
        }
        #endregion

        #region btnModify_Click
        protected void btnModify_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();//저장시 다중플래그가 나와서 구분해줘야 함
            string strRtn = string.Empty;
            string strOverRtn = string.Empty;
            int iRtn = 0;
            string strScript = string.Empty;

            //string strPVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;
            string strPVal = hidPopDefaultValue2.Value;

            string strValue = cy.Decrypt(strPVal);

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                strDBName = "GPDB";
                strQueryID = "Pln02Data.Get_PlanID_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("PLANT_CD", bp.g_plant.ToString());
                sParamIDChk.Add("SHOP_CD", ddlShopCd.SelectedValue);
                sParamIDChk.Add("LINE_CD", ddlLineCd.SelectedValue);
                sParamIDChk.Add("PART_NO", lbGetPartNo.Text);
                sParamIDChk.Add("PLAN_DT", lbGetPlanDt.Text);
                sParamIDChk.Add("CAR_TYPE", lbGetCarType.Text);
                sParamIDChk.Add("ORDER_NO", lbGetOrderNo.Text);
                sParamIDChk.Add("PLAN_QTY", txtPlanQty.Text);
                sParamIDChk.Add("PLAN_D_QTY", txtPlanDQty.Text);
                sParamIDChk.Add("PLAN_N_QTY", txtPlanNQty.Text);

                // 아이디 체크 비지니스 메서드 작성
                ds = biz.GetDataSet(strDBName, strQueryID, sParamIDChk);
                if (ds.Tables.Count > 0)
                {
                    strRtn = ds.Tables[0].Rows[0]["VAL_CHK"].ToString();
                    strOverRtn = ds.Tables[3].Rows[0]["VAL_CHK"].ToString();

                    if (strRtn != "0" && strOverRtn == "0")
                    {
                        strDBName = "GPDB";
                        strQueryID = "Pln02Data.Update_PlanInfo";

                        FW.Data.Parameters sParam = new FW.Data.Parameters();
                        sParam.Add("PLANT_CD", bp.g_plant.ToString());
                        sParam.Add("SHOP_CD", ddlShopCd.SelectedValue);
                        sParam.Add("LINE_CD", ddlLineCd.SelectedValue);
                        sParam.Add("PART_NO", lbGetPartNo.Text);
                        sParam.Add("PLAN_DT", lbGetPlanDt.Text);
                        sParam.Add("PLAN_QTY", txtPlanQty.Text);
                        sParam.Add("CAR_TYPE", lbGetCarType.Text);
                        sParam.Add("ORDER_NO", lbGetOrderNo.Text);
                        sParam.Add("PLAN_D_QTY", txtPlanDQty.Text);
                        sParam.Add("PLAN_N_QTY", txtPlanNQty.Text);
                        sParam.Add("REMARK", txtRemark.Text);
                        sParam.Add("ORDER_TYPE", hidOrderType.Value);
                        sParam.Add("STATUS_FLG", ddlStatusFlg.SelectedValue);
                        //sParam.Add("FINISH_FLG", ddlFinishFlg.SelectedValue);
                        sParam.Add("PLAN_CD", ddlPlanCd.SelectedValue);
                        //sParam.Add("PLAN_DETAIL_CD", (ddlPlanDetailCd.SelectedValue == "" ? hidPlanDetailCd.Value : ddlPlanDetailCd.SelectedValue));
                        sParam.Add("PLAN_DETAIL_CD", ddlPlanDetailCd.SelectedValue);
                        sParam.Add("ERP_SEND", ddlErpSend.SelectedValue);
                        sParam.Add("PLAN_SEQ", hidPlanSeq.Value);
                        sParam.Add("USER_ID", bp.g_userid.ToString());

                        sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                        sParam.Add("CUD_TYPE", "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                        sParam.Add("CUR_MENU_ID", "Pln02");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                        sParam.Add("PREV_DATA", strValue);                  // 이전 데이터 셋팅

                        // 수정 비지니스 메서드 작성
                        iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                        if (iRtn == 1)
                        {
                            //Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";
                            hidPopDefaultValue2.Value = "";

                            strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall('Pln02'); parent.fn_ModalCloseDiv(); ";
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "modData", strScript, true);
                            //ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                            GetData();
                        }
                        else
                        {
                            strScript = " alert('수정에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "modData", strScript, true);
                            //ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        }
                    }
                    else if (strRtn == "0")
                    {
                        strScript = " alert('해당일자에 작업스케쥴이 등록되지 않았습니다.'); ";
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "modData", strScript, true);
                        //ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else if (strOverRtn != "0")
                    {
                        strScript = " alert('해당 Order의 최대 계획치를 초과합니다.'); ";
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "modData", strScript, true);
                        //ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
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
            DataSet ds = new DataSet();//저장시 다중플래그가 나와서 구분해줘야 함
            string strProdRtn = string.Empty;
            int iRtn = 0;
            string strScript = string.Empty;

            //string strPVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;
            string strPVal = hidPopDefaultValue2.Value;

            string strValue = cy.Decrypt(strPVal);

            strDBName = "GPDB";
            strQueryID = "Pln02Data.Get_PlanID_ValChk";

            FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
            sParamIDChk.Add("PLANT_CD", bp.g_plant.ToString());
            sParamIDChk.Add("SHOP_CD", ddlShopCd.SelectedValue);
            sParamIDChk.Add("LINE_CD", ddlLineCd.SelectedValue);
            sParamIDChk.Add("PART_NO", lbGetPartNo.Text);
            sParamIDChk.Add("PLAN_DT", lbGetPlanDt.Text);
            sParamIDChk.Add("CAR_TYPE", lbGetCarType.Text);
            sParamIDChk.Add("ORDER_NO", lbGetOrderNo.Text);
            //sParamIDChk.Add("PLAN_D_QTY", "");
            //sParamIDChk.Add("PLAN_N_QTY", "");

            // 아이디 체크 비지니스 메서드 작성
            ds = biz.GetDataSet(strDBName, strQueryID, sParamIDChk);

            if (ds.Tables.Count > 0)
            {
                strProdRtn = ds.Tables[4].Rows[0]["VAL_CHK"].ToString();

                if (strProdRtn == "0")
                {
                    strDBName = "GPDB";
                    strQueryID = "Pln02Data.Set_PlanInfoDel";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("PLANT_CD", bp.g_plant.ToString());
                    sParam.Add("SHOP_CD", ddlShopCd.SelectedValue);
                    sParam.Add("LINE_CD", ddlLineCd.SelectedValue);
                    sParam.Add("CAR_TYPE", lbGetCarType.Text);
                    sParam.Add("PLAN_DT", lbGetPlanDt.Text);
                    sParam.Add("ORDER_NO", lbGetOrderNo.Text);
                    sParam.Add("PLAN_SEQ", hidPlanSeq.Value);

                    sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                    sParam.Add("CUD_TYPE", "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                    sParam.Add("CUR_MENU_ID", "Pln02");              // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                    sParam.Add("PREV_DATA", strValue);                  // 이전 데이터 셋팅

                    // 삭제 비지니스 메서드 작성
                    iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        //Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";
                        hidPopDefaultValue2.Value = "";

                        strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('Pln02'); parent.fn_ModalCloseDiv(); ";
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "delData", strScript, true);
                        //ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        //GetData();
                    }
                    else
                    {
                        strScript = " alert('삭제에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "delData", strScript, true);
                        //ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                }
                else
                {
                    strScript = " alert('이미 해당 품번의 생산이 완료되었습니다.'); ";
                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "delData", strScript, true);
                    //ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
            }

            //GetData();
        }
        #endregion

        #region GetDDL
        protected void GetDDL(string shopCd, string lineCd)
        {
            //GetData 에서 호출(LINE 콤보 초기 설정값 필요)
            DataSet ds = new DataSet();

            ddlShopCd.Items.Add(new ListItem("선택하세요", ""));
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));

            strDBName = "GPDB";
            strQueryID = "Pln02Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", shopCd);
            param.Add("LINE_CD", "");
            param.Add("PLAN_CD", "");
            param.Add("PART_NO", "");

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
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    ddlPlanCd.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                {
                    ddlPlanDetailCd.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                {
                    ddlStatusFlg.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                {
                    ddlFinishFlg.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[11].Rows.Count; i++)
                {
                    ddlErpSend.Items.Add(new ListItem(ds.Tables[11].Rows[i]["CODE_NM"].ToString(), ds.Tables[11].Rows[i]["CODE_ID"].ToString()));
                }
            }
        }
        #endregion

        #region ddlPlanCd_SelectedIndexChanged
        protected void ddlPlanCd_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlPlanDetailCd.Items.Clear();

            //비활성
            ddlPlanDetailCd.Enabled = false;

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Pln02Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", "");
            param.Add("LINE_CD", "");
            param.Add("PLAN_CD", ddlPlanCd.SelectedValue);
            param.Add("PART_NO", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //PLAN DETAIL TYPE code 있으면
                if (ds.Tables[4].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        ddlPlanDetailCd.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlPlanDetailCd.Enabled = true;
                }
            }
        }
        #endregion
    }
}
