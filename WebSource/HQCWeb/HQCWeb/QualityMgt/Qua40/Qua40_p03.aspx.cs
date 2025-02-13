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
    public partial class Qua40_p03 : System.Web.UI.Page
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
            ddlPartNo.Items.Add(new ListItem("선택하세요", ""));
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
                for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                {
                    ddlPartNo.Items.Add(new ListItem(ds.Tables[8].Rows[i]["CODE_NM"].ToString(), ds.Tables[8].Rows[i]["CODE_ID"].ToString()));
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

            txtDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
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
            lbWorkName.Text = Dictionary_Data.SearchDic("DECOMPOSE_HI", bp.g_language) + " " + Dictionary_Data.SearchDic("REGISTRATION", bp.g_language); // 등록
        }
        #endregion

        #region GetData
        protected void GetData()
        {

        }
        #endregion

        #region btnCheck_Click
        protected void btnCheck_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();//저장시 다중플래그가 나와서 구분해줘야 함
            string strScript = string.Empty;
            string strRtn = string.Empty;

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                strDBName = "GPDB";
                strQueryID = "Qua40Data.Get_Decompose_ValChk";
                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("PLANT_CD", bp.g_plant.ToString());
                sParamIDChk.Add("PART_NO", ddlPartNo.SelectedValue);
                sParamIDChk.Add("LOT_NO", txtSearchComboHidden.Text);

                ds = biz.GetDataSet(strDBName, strQueryID, sParamIDChk);

                if (ds.Tables.Count > 0)
                {
                    strRtn = ds.Tables[0].Rows[0]["VAL_CHK"].ToString();
                    if (strRtn == "0")
                    {
                        ddlSStorageCd.SelectedValue = ds.Tables[0].Rows[0]["STORAGE_CD"].ToString();
                        strScript = " alert('LOT NO가 정상입니다.'); fn_loadingEnd();";
                        ScriptManager.RegisterStartupScript(Page, typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else if (strRtn == "1") //인터페이스 완료
                    {
                        txtSearchComboHidden.Text = "";
                        strScript = " alert('이미 인터페이스된 LOT NO 입니다.'); fn_Combo_Refresh(); fn_loadingEnd();";
                        ScriptManager.RegisterStartupScript(Page,typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else if (strRtn == "2") //해당 LOT 처리된 이력 존재
                    {
                        txtSearchComboHidden.Text = "";
                        strScript = " alert('이미 분해대기중인 LOT NO 입니다.'); fn_Combo_Refresh(); fn_loadingEnd();";
                        ScriptManager.RegisterStartupScript(Page, typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else if (strRtn == "3") //재고없음
                    {
                        txtSearchComboHidden.Text = "";
                        strScript = " alert('데이터가 존재하지 않습니다. 품번과 LOT NO를 확인해 주세요.'); fn_Combo_Refresh(); fn_loadingEnd();";
                        ScriptManager.RegisterStartupScript(Page, typeof(string), Guid.NewGuid().ToString(), strScript, true);
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

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int iRtn = 0;
            string strScript = string.Empty;

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                // 비지니스 클래스 작성
                //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();

                strDBName = "GPDB";
                strQueryID = "Qua40Data.Set_DecomposeInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", bp.g_plant.ToString());
                sParam.Add("DEFECT_DT", txtDate.Text.Replace("-", ""));
                sParam.Add("LOG_SEQ", "-1");
                sParam.Add("PART_NO", ddlPartNo.SelectedValue);

                sParam.Add("LOT_NO", txtSearchComboHidden.Text);
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
                sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Qua40");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                // 등록 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    strScript = " alert('정상등록 되었습니다.'); parent.fn_ModalReloadCall('Qua40'); parent.fn_ModalCloseDiv(); ";
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
    }
}
