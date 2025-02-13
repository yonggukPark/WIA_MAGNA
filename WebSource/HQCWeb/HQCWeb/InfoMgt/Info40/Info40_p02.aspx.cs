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

namespace HQCWeb.InfoMgt.Info40
{
    public partial class Info40_p02 : System.Web.UI.Page
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
        Biz.InfoManagement.Info40 biz = new Biz.InfoManagement.Info40();

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
            // 상세내용 확인후 수정 또는 삭제일 경우
            lbWorkName.Text = Dictionary_Data.SearchDic("DETAIL", bp.g_language); // 상세

            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbStnCd.Text = Dictionary_Data.SearchDic("STN_CD", bp.g_language);
            lbDevCd.Text = Dictionary_Data.SearchDic("DEV_ID", bp.g_language);
            lbCarType.Text = Dictionary_Data.SearchDic("CAR_TYPE", bp.g_language);
            lbInsCd.Text = Dictionary_Data.SearchDic("INS_CD", bp.g_language);
            lbInsNm.Text = Dictionary_Data.SearchDic("INS_NM", bp.g_language);
            lbDivFlag.Text = Dictionary_Data.SearchDic("DIV_FLAG", bp.g_language);
            lbSeqId.Text = Dictionary_Data.SearchDic("SEQ_ID", bp.g_language);
            lbTableNm.Text = Dictionary_Data.SearchDic("TABLE_NM", bp.g_language);
            lbInsCdMin.Text = Dictionary_Data.SearchDic("INS_CD_MIN", bp.g_language);
            lbInsCdMax.Text = Dictionary_Data.SearchDic("INS_CD_MAX", bp.g_language);
            lbfinishFlag.Text = Dictionary_Data.SearchDic("FINISH_FLAG", bp.g_language);
            lbRomidFlag.Text = Dictionary_Data.SearchDic("ROMID_FLAG", bp.g_language);

            lbRemark1.Text = Dictionary_Data.SearchDic("REMARK1", bp.g_language);
            lbRemark2.Text = Dictionary_Data.SearchDic("REMARK2", bp.g_language);
            lbUseYn.Text = Dictionary_Data.SearchDic("USE_YN", bp.g_language);
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
            strQueryID = "Info40Data.Get_InspInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("CAR_TYPE", strSplitValue[3].ToString());
            sParam.Add("STN_CD", strSplitValue[4].ToString());
            sParam.Add("DEV_ID", strSplitValue[5].ToString());
            sParam.Add("SEQ_ID", strSplitValue[6].ToString());
            sParam.Add("DIV_FLAG", strSplitValue[7].ToString());

            sParam.Add("CUR_MENU_ID", "Info40");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
                    GetDDL(ds.Tables[0].Rows[0]["SHOP_CD"].ToString(), ds.Tables[0].Rows[0]["LINE_CD"].ToString(), ds.Tables[0].Rows[0]["STN_CD"].ToString(),
                    ds.Tables[0].Rows[0]["CAR_TYPE"].ToString(), ds.Tables[0].Rows[0]["DEV_ID"].ToString());
                    
                    ddlDivFlag.SelectedValue = ds.Tables[0].Rows[0]["DIV_FLAG"].ToString();
                    lbGetSeqId.Text = ds.Tables[0].Rows[0]["SEQ_ID"].ToString();

                    txtInsCd.Text = ds.Tables[0].Rows[0]["INS_CD"].ToString();
                    txtInsNm.Text = ds.Tables[0].Rows[0]["INS_NM"].ToString();
                    txtTableNm.Text = ds.Tables[0].Rows[0]["TABLE_NM"].ToString();
                    ddlInsCdMin.SelectedValue = ds.Tables[0].Rows[0]["INS_CD_MIN"].ToString();
                    ddlInsCdMax.SelectedValue = ds.Tables[0].Rows[0]["INS_CD_MAX"].ToString();
                    ddlFinishFlag.SelectedValue = ds.Tables[0].Rows[0]["FINISH_FLAG"].ToString();
                    ddlRomidFlag.SelectedValue = ds.Tables[0].Rows[0]["ROMID_FLAG"].ToString();
                    
                    ddlUseYN.SelectedValue = ds.Tables[0].Rows[0]["USE_YN"].ToString();
                    txtRemark1.Text = ds.Tables[0].Rows[0]["REMARK1"].ToString();
                    txtRemark2.Text = ds.Tables[0].Rows[0]["REMARK2"].ToString();

                    if (ds.Tables[0].Rows[0]["DEL_YN"].ToString().Equals("Y"))
                    {
                        aModify.Visible = false;
                        //aDelete.Visible = false;
                        aRestore.Visible = true;

                        txtInsCd.Enabled = false;
                        txtInsNm.Enabled = false;
                        txtTableNm.Enabled = false;
                        ddlInsCdMin.Enabled = false;
                        ddlInsCdMax.Enabled = false;
                        ddlFinishFlag.Enabled = false;
                        ddlRomidFlag.Enabled = false;
                        
                        txtRemark1.Enabled = false;
                        txtRemark2.Enabled = false;
                        ddlUseYN.Enabled = false;
                    }
                    else
                    {
                        aModify.Visible = true;
                        //aDelete.Visible = true;
                        aRestore.Visible = false;

                        txtInsCd.Enabled = true;
                        txtInsNm.Enabled = true;
                        txtTableNm.Enabled = true;
                        ddlInsCdMin.Enabled = true;
                        ddlInsCdMax.Enabled = true;
                        ddlFinishFlag.Enabled = true;
                        ddlRomidFlag.Enabled = true;

                        txtRemark1.Enabled = true;
                        txtRemark2.Enabled = true;
                        ddlUseYN.Enabled = true;
                    }

                    ddlShopCd.Enabled = false;
                    ddlLineCd.Enabled = false;
                    ddlStnCd.Enabled = false;
                    ddlCarType.Enabled = false;
                    ddlDevCd.Enabled = false;
                    ddlDivFlag.Enabled = false;

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
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Info40'); parent.fn_ModalCloseDiv(); ";
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
                // 비지니스 클래스 작성
                //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();

                strDBName = "GPDB";
                strQueryID = "Info40Data.Set_InspInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                sParam.Add("LINE_CD", strSplitValue[2].ToString());
                sParam.Add("CAR_TYPE", strSplitValue[3].ToString());
                sParam.Add("STN_CD", strSplitValue[4].ToString());
                sParam.Add("DEV_ID", strSplitValue[5].ToString());
                sParam.Add("SEQ_ID", strSplitValue[6].ToString());
                sParam.Add("DIV_FLAG", strSplitValue[7].ToString());

                sParam.Add("INS_CD", txtInsCd.Text);
                sParam.Add("INS_NM", txtInsNm.Text);
                sParam.Add("TABLE_NM", txtTableNm.Text);
                sParam.Add("FINISH_FLAG", ddlFinishFlag.SelectedValue);
                sParam.Add("INS_CD_MIN", ddlInsCdMin.SelectedValue);
                sParam.Add("INS_CD_MAX", ddlInsCdMax.SelectedValue);
                sParam.Add("ROMID_FLAG", ddlRomidFlag.SelectedValue);

                sParam.Add("REMARK1", txtRemark1.Text);
                sParam.Add("REMARK2", txtRemark2.Text);
                sParam.Add("USE_YN", ddlUseYN.SelectedValue);
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Info40");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA", strUValue);    // 이전 데이터 셋팅

                // 수정 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall('Info40'); parent.fn_ModalCloseDiv(); ";
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

            DataSet ds = new DataSet();

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string strDValue = cy.Decrypt(strDVal);

            // 비지니스 클래스 작성
            //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();

            strDBName = "GPDB";
            strQueryID = "Info40Data.Get_InspInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("CAR_TYPE", strSplitValue[3].ToString());
            sParam.Add("STN_CD", strSplitValue[4].ToString());
            sParam.Add("DEV_ID", strSplitValue[5].ToString());
            sParam.Add("SEQ_ID", strSplitValue[6].ToString());
            sParam.Add("DIV_FLAG", strSplitValue[7].ToString());

            sParam.Add("CUR_MENU_ID", "Info40");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {

                strDBName = "GPDB";
                strQueryID = "Info40Data.Set_InspInfoDel";

                sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                sParam.Add("LINE_CD", strSplitValue[2].ToString());
                sParam.Add("CAR_TYPE", strSplitValue[3].ToString());
                sParam.Add("STN_CD", strSplitValue[4].ToString());
                sParam.Add("DEV_ID", strSplitValue[5].ToString());
                sParam.Add("SEQ_ID", strSplitValue[6].ToString());
                sParam.Add("DIV_FLAG", strSplitValue[7].ToString());

                sParam.Add("FLAG", "Y");
                sParam.Add("COMP_FLAG", ds.Tables[0].Rows[0]["DEL_YN"].ToString()); // 이미 삭제된 놈 삭제시 완전삭제
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Info40");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA", strDValue);   // 이전 데이터 셋팅

                // 삭제 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('Info40'); parent.fn_ModalCloseDiv(); ";
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
                strScript = " alert('이미 정보가 존재하지 않습니다.'); parent.fn_ModalReloadCall('Info40'); parent.fn_ModalCloseDiv(); ";
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

            // 비지니스 클래스 작성
            //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();

            strDBName = "GPDB";
            strQueryID = "Info40Data.Set_InspInfoDel";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("CAR_TYPE", strSplitValue[3].ToString());
            sParam.Add("STN_CD", strSplitValue[4].ToString());
            sParam.Add("DEV_ID", strSplitValue[5].ToString());
            sParam.Add("SEQ_ID", strSplitValue[6].ToString());
            sParam.Add("DIV_FLAG", strSplitValue[7].ToString());

            sParam.Add("FLAG", "N");
            sParam.Add("COMP_FLAG", "N");
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            sParam.Add("CUD_TYPE", "R");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            sParam.Add("CUR_MENU_ID", "Info40");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            sParam.Add("PREV_DATA", strDValue);   // 이전 데이터 셋팅

            // 삭제 비지니스 메서드 작성
            iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

            if (iRtn == 1)
            {
                (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                strScript = " alert('정상복원 되었습니다.');  parent.fn_ModalReloadCall('Info40'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                strScript = " alert('복원에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region GetDDL
        protected void GetDDL(string shopCd, string lineCd, string stnCd, string carType, string devId)
        {
            //GetData 에서 호출(STN 콤보 초기 설정값 필요)
            DataSet ds = new DataSet();

            ddlShopCd.Items.Add(new ListItem("", ""));
            ddlLineCd.Items.Add(new ListItem("", ""));
            ddlCarType.Items.Add(new ListItem("", ""));
            ddlStnCd.Items.Add(new ListItem("", ""));
            ddlDevCd.Items.Add(new ListItem("", ""));
            ddlInsCdMin.Items.Add(new ListItem("미지정", ""));
            ddlInsCdMax.Items.Add(new ListItem("미지정", ""));
            ddlDivFlag.Items.Clear();
            ddlUseYN.Items.Clear();

            strDBName = "GPDB";
            strQueryID = "Info40Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", shopCd);
            param.Add("LINE_CD", lineCd);
            param.Add("STN_CD", stnCd);

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
                    ddlStnCd.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                }
                ddlStnCd.SelectedValue = stnCd;
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    ddlDevCd.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                }
                ddlDevCd.SelectedValue = devId;
                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                {
                    ddlCarType.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                }
                ddlCarType.SelectedValue = carType;
                for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                {
                    ddlDivFlag.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                {
                    ddlFinishFlag.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                    ddlUseYN.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                    ddlRomidFlag.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                {
                    ddlInsCdMin.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                    ddlInsCdMax.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                }
            }
        }
        #endregion
    }
}
