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

namespace HQCWeb.InfoMgt.Info08
{
    public partial class Info08_p02 : System.Web.UI.Page
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
        Biz.InfoManagement.Info08 biz = new Biz.InfoManagement.Info08();

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
            lbDevId.Text = Dictionary_Data.SearchDic("PRINT_ID", bp.g_language);
            lbDevKind.Text = Dictionary_Data.SearchDic("PRINT_KIND_ID", bp.g_language);
            lbCarType.Text = Dictionary_Data.SearchDic("CAR_TYPE", bp.g_language);
            lbCode.Text = Dictionary_Data.SearchDic("ID_CODE", bp.g_language);
            lbCodeNm.Text = Dictionary_Data.SearchDic("ID_CODE_NM", bp.g_language);
            lbHeight.Text = Dictionary_Data.SearchDic("HEIGHT", bp.g_language);
            lbWidth.Text = Dictionary_Data.SearchDic("WIDTH", bp.g_language);
            lbFontHeight.Text = Dictionary_Data.SearchDic("FONT_HEIGHT", bp.g_language);
            lbFontWidth.Text = Dictionary_Data.SearchDic("FONT_WIDTH", bp.g_language);
            lbZpl.Text = Dictionary_Data.SearchDic("ZPL", bp.g_language);
            lbType.Text = Dictionary_Data.SearchDic("TYPE", bp.g_language);
            lbValue.Text = Dictionary_Data.SearchDic("VALUE", bp.g_language);

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
            strQueryID = "Info08Data.Get_PrintInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("CAR_TYPE", strSplitValue[3].ToString());
            sParam.Add("SEQ", strSplitValue[4].ToString());
            sParam.Add("DEV_ID", strSplitValue[5].ToString());
            sParam.Add("DEV_KIND", strSplitValue[6].ToString());
            sParam.Add("TYPE", strSplitValue[7].ToString());
            sParam.Add("CODE", strSplitValue[8].ToString()); 

            sParam.Add("CUR_MENU_ID", "Info08");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
                    GetDDL(ds.Tables[0].Rows[0]["SHOP_CD"].ToString(), ds.Tables[0].Rows[0]["LINE_CD"].ToString(), ds.Tables[0].Rows[0]["DEV_ID"].ToString());
                    ddlCarType.SelectedValue = ds.Tables[0].Rows[0]["CAR_TYPE"].ToString();
                    ddlDevKind.SelectedValue = ds.Tables[0].Rows[0]["DEV_KIND"].ToString();
                    ddlType.SelectedValue = ds.Tables[0].Rows[0]["TYPE"].ToString();
                    lbGetCode.Text = ds.Tables[0].Rows[0]["CODE"].ToString();

                    txtCodeNm.Text = ds.Tables[0].Rows[0]["CODE_NM"].ToString();
                    txtWidth.Text = ds.Tables[0].Rows[0]["WIDTH"].ToString();
                    txtHeight.Text = ds.Tables[0].Rows[0]["HEIGHT"].ToString();
                    txtFontWidth.Text = ds.Tables[0].Rows[0]["FONT_WIDTH"].ToString();
                    txtFontHeight.Text = ds.Tables[0].Rows[0]["FONT_HEIGHT"].ToString();
                    txtValue.Text = ds.Tables[0].Rows[0]["VALUE"].ToString();
                    txtZpl.Text = ds.Tables[0].Rows[0]["ZPL"].ToString();
                    
                    ddlUseYN.SelectedValue = ds.Tables[0].Rows[0]["USE_YN"].ToString();
                    txtRemark1.Text = ds.Tables[0].Rows[0]["REMARK1"].ToString();
                    txtRemark2.Text = ds.Tables[0].Rows[0]["REMARK2"].ToString();

                    if (ds.Tables[0].Rows[0]["DEL_YN"].ToString().Equals("Y"))
                    {
                        aModify.Visible = false;
                        //aDelete.Visible = false;
                        aRestore.Visible = true;

                        txtCodeNm.Enabled = false;
                        txtWidth.Enabled = false;
                        txtHeight.Enabled = false;
                        txtFontWidth.Enabled = false;
                        txtFontHeight.Enabled = false;
                        txtValue.Enabled = false;
                        txtZpl.Enabled = false;

                        ddlUseYN.Enabled = false;
                        txtRemark1.Enabled = false;
                        txtRemark2.Enabled = false;
                    }
                    else
                    {
                        aModify.Visible = true;
                        //aDelete.Visible = true;
                        aRestore.Visible = false;

                        txtCodeNm.Enabled = true;
                        txtWidth.Enabled = true;
                        txtHeight.Enabled = true;
                        txtFontWidth.Enabled = true;
                        txtFontHeight.Enabled = true;
                        txtValue.Enabled = true;
                        txtZpl.Enabled = true;

                        ddlUseYN.Enabled = true;
                        txtRemark1.Enabled = true;
                        txtRemark2.Enabled = true;
                    }

                    ddlShopCd.Enabled = false;
                    ddlLineCd.Enabled = false;
                    ddlCarType.Enabled = false;
                    ddlDevId.Enabled = false;
                    ddlDevKind.Enabled = false;
                    ddlType.Enabled = false;

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
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Info08'); parent.fn_ModalCloseDiv(); ";
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
                strQueryID = "Info08Data.Set_PrintInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                sParam.Add("LINE_CD", strSplitValue[2].ToString());
                sParam.Add("CAR_TYPE", strSplitValue[3].ToString());
                sParam.Add("SEQ", strSplitValue[4].ToString());
                sParam.Add("DEV_ID", strSplitValue[5].ToString());
                sParam.Add("DEV_KIND", strSplitValue[6].ToString());
                sParam.Add("TYPE", strSplitValue[7].ToString());
                sParam.Add("CODE", strSplitValue[8].ToString());

                sParam.Add("CODE_NM", txtCodeNm.Text);
                sParam.Add("WIDTH", txtWidth.Text);
                sParam.Add("HEIGHT", txtHeight.Text);
                sParam.Add("FONT_WIDTH", txtFontWidth.Text);
                sParam.Add("FONT_HEIGHT", txtFontHeight.Text);
                sParam.Add("VALUE", txtValue.Text);
                sParam.Add("ZPL", txtZpl.Text);

                sParam.Add("REMARK1", txtRemark1.Text);
                sParam.Add("REMARK2", txtRemark2.Text);
                sParam.Add("USE_YN", ddlUseYN.SelectedValue);
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Info08");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA", strUValue);    // 이전 데이터 셋팅

                // 수정 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall('Info08'); parent.fn_ModalCloseDiv(); ";
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
            strQueryID = "Info08Data.Get_PrintInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("CAR_TYPE", strSplitValue[3].ToString());
            sParam.Add("SEQ", strSplitValue[4].ToString());
            sParam.Add("DEV_ID", strSplitValue[5].ToString());
            sParam.Add("DEV_KIND", strSplitValue[6].ToString());
            sParam.Add("TYPE", strSplitValue[7].ToString());
            sParam.Add("CODE", strSplitValue[8].ToString());

            sParam.Add("CUR_MENU_ID", "Info08");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                strDBName = "GPDB";
                strQueryID = "Info08Data.Set_PrintInfoDel";

                sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                sParam.Add("LINE_CD", strSplitValue[2].ToString());
                sParam.Add("CAR_TYPE", strSplitValue[3].ToString());
                sParam.Add("SEQ", strSplitValue[4].ToString());
                sParam.Add("DEV_ID", strSplitValue[5].ToString());
                sParam.Add("DEV_KIND", strSplitValue[6].ToString());
                sParam.Add("TYPE", strSplitValue[7].ToString());
                sParam.Add("CODE", strSplitValue[8].ToString());

                sParam.Add("FLAG", "Y");
                sParam.Add("COMP_FLAG", ds.Tables[0].Rows[0]["DEL_YN"].ToString()); // 이미 삭제된 놈 삭제시 완전삭제
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Info08");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA", strDValue);   // 이전 데이터 셋팅

                // 삭제 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('Info08'); parent.fn_ModalCloseDiv(); ";
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
                strScript = " alert('이미 정보가 존재하지 않습니다.'); parent.fn_ModalReloadCall('Info08'); parent.fn_ModalCloseDiv(); ";
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
            strQueryID = "Info08Data.Set_PrintInfoDel";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("CAR_TYPE", strSplitValue[3].ToString());
            sParam.Add("SEQ", strSplitValue[4].ToString());
            sParam.Add("DEV_ID", strSplitValue[5].ToString());
            sParam.Add("DEV_KIND", strSplitValue[6].ToString());
            sParam.Add("TYPE", strSplitValue[7].ToString());
            sParam.Add("CODE", strSplitValue[8].ToString());

            sParam.Add("FLAG", "N");
            sParam.Add("COMP_FLAG", "N");
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            sParam.Add("CUD_TYPE", "R");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            sParam.Add("CUR_MENU_ID", "Info08");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            sParam.Add("PREV_DATA", strDValue);   // 이전 데이터 셋팅

            // 삭제 비지니스 메서드 작성
            iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

            if (iRtn == 1)
            {
                (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                strScript = " alert('정상복구 되었습니다.');  parent.fn_ModalReloadCall('Info08'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                strScript = " alert('복구에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region GetDDL
        protected void GetDDL(string shopCd, string lineCd, string devId)
        {
            //GetData 에서 호출(LINE 콤보 초기 설정값 필요)
            DataSet ds = new DataSet();

            ddlShopCd.Items.Add(new ListItem("선택하세요", ""));
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));

            strDBName = "GPDB";
            strQueryID = "Info08Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", shopCd);
            param.Add("LINE_CD", lineCd);
            param.Add("DEV_ID", devId);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlShopCd.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                }
                ddlShopCd.SelectedValue = shopCd;
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    ddlLineCd.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                }
                ddlLineCd.SelectedValue = lineCd;
                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                {
                    ddlCarType.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                {
                    ddlDevId.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                }
                ddlDevId.SelectedValue = devId;
                for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                {
                    ddlDevKind.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                {
                    ddlType.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                {
                    ddlUseYN.Items.Add(new ListItem(ds.Tables[8].Rows[i]["CODE_NM"].ToString(), ds.Tables[8].Rows[i]["CODE_ID"].ToString()));
                }
            }
        }
        #endregion
    }
}
