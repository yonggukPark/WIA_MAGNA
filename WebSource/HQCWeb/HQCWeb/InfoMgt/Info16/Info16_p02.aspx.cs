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

namespace HQCWeb.InfoMgt.Info16
{
    public partial class Info16_p02 : System.Web.UI.Page
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
        Biz.InfoManagement.Info16 biz = new Biz.InfoManagement.Info16();

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

            lbProdDt.Text = Dictionary_Data.SearchDic("OCCUR_DT", bp.g_language);
            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbNoworkTime.Text = Dictionary_Data.SearchDic("NOWORK_TIME", bp.g_language);
            lbStopMin.Text = Dictionary_Data.SearchDic("STOP_MIN", bp.g_language);
            lbNoworkType.Text = Dictionary_Data.SearchDic("NOWORK_TYPE", bp.g_language);

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
            strQueryID = "Info16Data.Get_NoworkInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("PROD_DT", strSplitValue[3].ToString());
            sParam.Add("NOWORK_TIME", strSplitValue[4].ToString());

            sParam.Add("CUR_MENU_ID", "Info16");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
                    GetDDL(ds.Tables[0].Rows[0]["SHOP_CD"].ToString(), ds.Tables[0].Rows[0]["LINE_CD"].ToString());
                    lbGetProdDt.Text = ds.Tables[0].Rows[0]["PROD_DT"].ToString();
                    lbGetNoworkTime.Text = ds.Tables[0].Rows[0]["NOWORK_TIME"].ToString();
                    ddlNoworkType.SelectedValue = ds.Tables[0].Rows[0]["NOWORK_TYPE"].ToString();
                    txtStopMin.Text = ds.Tables[0].Rows[0]["STOP_MIN"].ToString();
                    ddlUseYN.SelectedValue = ds.Tables[0].Rows[0]["USE_YN"].ToString();
                    txtRemark1.Text = ds.Tables[0].Rows[0]["REMARK1"].ToString();
                    txtRemark2.Text = ds.Tables[0].Rows[0]["REMARK2"].ToString();

                    if (ds.Tables[0].Rows[0]["DEL_YN"].ToString().Equals("Y"))
                    {
                        aModify.Visible = false;
                        aDelete.Visible = false;
                        //aRestore.Visible = true;

                        txtStopMin.Enabled = false;
                        ddlNoworkType.Enabled = false;
                        ddlUseYN.Enabled = false;
                        txtRemark1.Enabled = false;
                        txtRemark2.Enabled = false;
                    }
                    else
                    {
                        aModify.Visible = true;
                        aDelete.Visible = true;
                        //aRestore.Visible = false;

                        txtStopMin.Enabled = true;
                        ddlNoworkType.Enabled = true;
                        ddlUseYN.Enabled = true;
                        txtRemark1.Enabled = true;
                        txtRemark2.Enabled = true;
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
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Info16'); parent.fn_ModalCloseDiv(); ";
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
                strQueryID = "Info16Data.Set_NoworkInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                sParam.Add("LINE_CD", strSplitValue[2].ToString());
                sParam.Add("PROD_DT", strSplitValue[3].ToString());
                sParam.Add("NOWORK_TIME", strSplitValue[4].ToString());
                sParam.Add("NOWORK_TYPE", ddlNoworkType.SelectedValue);
                sParam.Add("STOP_MIN", txtStopMin.Text);

                sParam.Add("REMARK1", txtRemark1.Text);
                sParam.Add("REMARK2", txtRemark2.Text);
                sParam.Add("USE_YN", ddlUseYN.SelectedValue);
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Info16");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA", strUValue);    // 이전 데이터 셋팅

                // 수정 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall('Info16'); parent.fn_ModalCloseDiv(); ";
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

            // 비지니스 클래스 작성
            //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();

            strDBName = "GPDB";
            strQueryID = "Info16Data.Set_NoworkInfoDel";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("PROD_DT", strSplitValue[3].ToString());
            sParam.Add("NOWORK_TIME", strSplitValue[4].ToString());

            sParam.Add("FLAG", "Y");
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            sParam.Add("CUD_TYPE", "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            sParam.Add("CUR_MENU_ID", "Info16");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            sParam.Add("PREV_DATA", strDValue);   // 이전 데이터 셋팅

            // 삭제 비지니스 메서드 작성
            iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

            if (iRtn == 1)
            {
                (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('Info16'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                strScript = " alert('삭제에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnRestore_Click(비활성)
        //protected void btnRestore_Click(object sender, EventArgs e)
        //{
        //    int iRtn = 0;
        //    string strScript = string.Empty;

        //    string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

        //    string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

        //    string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

        //    string strDValue = cy.Decrypt(strDVal);

        //    // 비지니스 클래스 작성
        //    //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();

        //    strDBName = "GPDB";
        //    strQueryID = "Info16Data.Set_NoworkInfoDel";

        //    FW.Data.Parameters sParam = new FW.Data.Parameters();
        //    sParam.Add("PLANT_CD", strSplitValue[0].ToString());
        //    sParam.Add("SHOP_CD", strSplitValue[1].ToString());
        //    sParam.Add("LINE_CD", strSplitValue[2].ToString());
        //    sParam.Add("PROD_DT", strSplitValue[3].ToString());
        //    sParam.Add("NOWORK_TIME", strSplitValue[4].ToString());

        //    sParam.Add("FLAG", "N");
        //    sParam.Add("USER_ID", bp.g_userid.ToString());

        //    sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
        //    sParam.Add("CUD_TYPE", "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
        //    sParam.Add("CUR_MENU_ID", "Info16");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
        //    sParam.Add("PREV_DATA", strDValue);   // 이전 데이터 셋팅

        //    // 삭제 비지니스 메서드 작성
        //    iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

        //    if (iRtn == 1)
        //    {
        //        (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

        //        strScript = " alert('정상복원 되었습니다.');  parent.fn_ModalReloadCall('Info16'); parent.fn_ModalCloseDiv(); ";
        //        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
        //    }
        //    else
        //    {
        //        strScript = " alert('복원에 실패하였습니다. 관리자에게 문의바립니다.'); ";
        //        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
        //    }
        //}
        #endregion

        #region GetDDL
        protected void GetDDL(string shopCd, string lineCd)
        {
            //GetData 에서 호출(LINE 콤보 초기 설정값 필요)
            DataSet ds = new DataSet();

            ddlShopCd.Items.Add(new ListItem("선택하세요", ""));
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));

            strDBName = "GPDB";
            strQueryID = "Info16Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", shopCd);

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
                    ddlNoworkType.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                {
                    ddlUseYN.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                }
            }
        }
        #endregion
    }
}
