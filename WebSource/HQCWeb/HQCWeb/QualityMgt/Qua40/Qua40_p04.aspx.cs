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
    public partial class Qua40_p04 : System.Web.UI.Page
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
        Biz.QualityManagement.Qua40 biz = new Biz.QualityManagement.Qua40();

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
            ddlDCode.Items.Add(new ListItem("선택하세요", ""));
            ddlDReasonCode.Items.Add(new ListItem("선택하세요.", ""));
            ddlDRespCd.Items.Add(new ListItem("선택하세요.", ""));
            ddlDecomposeType.Items.Add(new ListItem("선택하세요", ""));
            ddlSStorageCd.Items.Add(new ListItem("선택하세요", ""));
            //ddlDStorageCd.Items.Add(new ListItem("선택하세요", ""));
            ddlDStorageCd.Enabled = false;

            strDBName = "GPDB";
            strQueryID = "Qua40Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());

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
                for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                {
                    ddlResultCd.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[9].Rows.Count; i++)
                {
                    ddlDecomposeType.Items.Add(new ListItem(ds.Tables[9].Rows[i]["CODE_NM"].ToString(), ds.Tables[9].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[10].Rows.Count; i++)
                {
                    ddlSStorageCd.Items.Add(new ListItem(ds.Tables[10].Rows[i]["CODE_NM"].ToString(), ds.Tables[10].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[11].Rows.Count; i++)
                {
                    ddlDStorageCd.Items.Add(new ListItem(ds.Tables[11].Rows[i]["CODE_NM"].ToString(), ds.Tables[11].Rows[i]["CODE_ID"].ToString()));
                }
            }
            
            txtReturnDt.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbDefectDt.Text = Dictionary_Data.SearchDic("DEFECT_DT", bp.g_language);
            lbPartNo.Text = Dictionary_Data.SearchDic("PART_NO", bp.g_language);
            lbLotNo.Text = Dictionary_Data.SearchDic("LOT_NO", bp.g_language);
            lbDRespCd.Text = Dictionary_Data.SearchDic("D_RESP_CD", bp.g_language);
            lbDCode.Text = Dictionary_Data.SearchDic("D_CODE", bp.g_language);
            lbDReasonCode.Text = Dictionary_Data.SearchDic("D_REASON_CODE", bp.g_language);
            lbDefectCnt.Text = Dictionary_Data.SearchDic("DEFECT_CNT", bp.g_language);
            lbReturnDt.Text = Dictionary_Data.SearchDic("RETURN_DT", bp.g_language);
            lbReworkMsg.Text = Dictionary_Data.SearchDic("REWORK_MSG", bp.g_language);
            lbDecomposeType.Text = Dictionary_Data.SearchDic("DECOMPOSE_CD", bp.g_language);
            lbResultCd.Text = Dictionary_Data.SearchDic("RESULT", bp.g_language);
            lbSStorageCd.Text = Dictionary_Data.SearchDic("S_STORAGE_NM", bp.g_language);
            lbDStorageCd.Text = Dictionary_Data.SearchDic("D_STORAGE_NM", bp.g_language);

            // 등록일경우
            lbWorkName.Text = Dictionary_Data.SearchDic("DECOMPOSE_HI", bp.g_language) + " " + Dictionary_Data.SearchDic("DETAIL", bp.g_language); // 상세
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
            strQueryID = "Qua40Data.Get_DecomposeInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("DEFECT_DT", strSplitValue[1].ToString());
            sParam.Add("LOG_SEQ", strSplitValue[2].ToString());
            sParam.Add("PART_NO", strSplitValue[3].ToString());
            sParam.Add("LOT_NO", strSplitValue[4].ToString());

            sParam.Add("CUR_MENU_ID", "Qua40");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                lbGetDefectDt.Text = ds.Tables[0].Rows[0]["DEFECT_DT"].ToString();
                lbGetPartNo.Text = ds.Tables[0].Rows[0]["PART_NO"].ToString();
                lbGetLotNo.Text = ds.Tables[0].Rows[0]["LOT_NO"].ToString();
                ddlDRespCd.SelectedValue = ds.Tables[0].Rows[0]["D_RESP_CD"].ToString();
                ddlDCode.SelectedValue = ds.Tables[0].Rows[0]["D_CODE"].ToString();
                ddlDReasonCode.SelectedValue = ds.Tables[0].Rows[0]["D_REASON_CODE"].ToString();
                txtDefectCnt.Text = ds.Tables[0].Rows[0]["DEFECT_CNT"].ToString();
                txtReturnDt.Text = ds.Tables[0].Rows[0]["RETURN_DT"].ToString();
                txtReworkMsg.Text = ds.Tables[0].Rows[0]["REWORK_MSG"].ToString();
                ddlDecomposeType.SelectedValue = ds.Tables[0].Rows[0]["DECOMPOSE_CD"].ToString();
                ddlResultCd.SelectedValue = ds.Tables[0].Rows[0]["RESULT"].ToString();
                ddlSStorageCd.SelectedValue = ds.Tables[0].Rows[0]["STORAGE_CD"].ToString();
                ddlSStorageCd.Enabled = false;

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
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Qua40'); parent.fn_ModalCloseDiv(); ";
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
                strQueryID = "Qua40Data.Set_DecomposeInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("DEFECT_DT", strSplitValue[1].ToString());
                sParam.Add("LOG_SEQ", strSplitValue[2].ToString());
                sParam.Add("PART_NO", strSplitValue[3].ToString());
                sParam.Add("LOT_NO", strSplitValue[4].ToString());

                sParam.Add("D_RESP_CD", ddlDRespCd.SelectedValue);
                sParam.Add("D_CODE", ddlDCode.SelectedValue);
                sParam.Add("D_REASON_CODE", ddlDReasonCode.SelectedValue);
                sParam.Add("DEFECT_CNT", txtDefectCnt.Text);
                sParam.Add("RETURN_DT", txtReturnDt.Text.Replace("-", ""));
                sParam.Add("REWORK_MSG", txtReworkMsg.Text);
                sParam.Add("DECOMPOSE_CD", ddlDecomposeType.SelectedValue);
                sParam.Add("RESULT", ddlResultCd.SelectedValue);
                sParam.Add("S_STORAGE_CD", ddlSStorageCd.SelectedValue);
                sParam.Add("T_STORAGE_CD", ddlDStorageCd.SelectedValue);
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Qua40");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA", strUValue);                  // 이전 데이터 셋팅

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
            
            strDBName = "GPDB";
            strQueryID = "Qua40Data.Del_DecomposeInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("DEFECT_DT", strSplitValue[1].ToString());
            sParam.Add("LOG_SEQ", strSplitValue[2].ToString());
            sParam.Add("PART_NO", strSplitValue[3].ToString());
            sParam.Add("LOT_NO", strSplitValue[4].ToString());

            sParam.Add("REWORK_MSG", txtReworkMsg.Text);
            sParam.Add("D_CODE", ddlDCode.SelectedValue);
            sParam.Add("D_REASON_CODE", ddlDReasonCode.SelectedValue);
            sParam.Add("D_RESP_CD", ddlDRespCd.SelectedValue);
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            sParam.Add("CUD_TYPE", "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            sParam.Add("CUR_MENU_ID", "Qua40");              // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            sParam.Add("PREV_DATA", strDValue);                  // 이전 데이터 셋팅

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
        #endregion


    }
}
