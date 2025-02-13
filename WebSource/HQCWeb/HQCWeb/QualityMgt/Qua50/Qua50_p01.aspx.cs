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

namespace HQCWeb.QualityMgt.Qua50
{
    public partial class Qua50_p01 : System.Web.UI.Page
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
        Biz.QualityManagement.Qua50 biz = new Biz.QualityManagement.Qua50();

        string jsDdl = string.Empty;

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

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid' 함수 호출
                string script = $@" cLine = {jsDdl}; ";

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            DataSet ds = new DataSet();
            
            ddlShopCd.Items.Add(new ListItem("선택하세요.", ""));
            ddlCarType.Items.Add(new ListItem("선택하세요.", ""));
            ddlCarType.Enabled = false;

            strDBName = "GPDB";
            strQueryID = "Qua50Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("PART_NO", "");
            param.Add("SHOP_CD", " ");
            param.Add("LINE_CD", " ");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlEo4mFlag.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                {
                    ddlShopCd.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                }
                //다중선택 콤보용
                jsDdl = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[5]);
            }

            txtDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            // 등록일경우
            lbWorkName.Text = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language); // 등록

            lbRegDt.Text = Dictionary_Data.SearchDic("REG_DT", bp.g_language);
            lbEo4mFlag.Text = Dictionary_Data.SearchDic("EO4M_FLAG", bp.g_language);
            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbPartNo.Text = Dictionary_Data.SearchDic("PART_NO", bp.g_language);
            lbPartDesc.Text = Dictionary_Data.SearchDic("PART_DESC", bp.g_language);
            lbPartNoSearch.Text = Dictionary_Data.SearchDic("PART_NO_SEARCH", bp.g_language);
            lbCarType.Text = Dictionary_Data.SearchDic("CAR_TYPE", bp.g_language);
            lbEoNo.Text = Dictionary_Data.SearchDic("EO_NO", bp.g_language);
            lbModRemark.Text = Dictionary_Data.SearchDic("MOD_REMARK", bp.g_language);
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
                strQueryID = "Qua50Data.Get_EOID_PartChk";
                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("PLANT_CD", bp.g_plant.ToString());
                sParamIDChk.Add("PART_NO", txtPartNoSearch.Text);

                ds = biz.GetDataSet(strDBName, strQueryID, sParamIDChk);

                if (ds.Tables.Count > 0)
                {
                    strRtn = ds.Tables[0].Rows[0]["VAL_CHK"].ToString();

                    //데이터 클리어
                    ddlPartNo.Items.Clear();

                    //비활성
                    ddlPartNo.Enabled = false;

                    //초기화
                    ddlPartNo.Items.Add(new ListItem("선택하세요", ""));

                    if (strRtn != "0")
                    {

                        if(ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                ddlPartNo.Items.Add(new ListItem(ds.Tables[0].Rows[i]["PART_NO"].ToString(), ds.Tables[0].Rows[i]["PART_NO"].ToString()));
                            }
                            ddlPartNo.Enabled = true;
                        }
                        
                        lbGetPartDesc.Text = ds.Tables[0].Rows[0]["PART_DESC"].ToString();
                        strScript = " alert('품번 조회가 완료되었습니다.'); fn_loadingEnd();";
                        ScriptManager.RegisterStartupScript(Page, typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else
                    {
                        txtPartNoSearch.Text = "";
                        lbGetPartDesc.Text = "";
                        strScript = " alert('품번을 확인해 주세요'); fn_loadingEnd();";
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
            DataSet ds = new DataSet();
            string strRtn = string.Empty;
            int iRtn = 0;
            string strScript = string.Empty;

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                strDBName = "GPDB";
                strQueryID = "Qua50Data.Get_EOID_CarChk";

                FW.Data.Parameters sParam = null;

                sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", bp.g_plant.ToString());
                sParam.Add("PART_NO", ddlPartNo.SelectedValue);
                sParam.Add("CAR_TYPE", ddlCarType.SelectedValue);

                ds = biz.GetDataSet(strDBName, strQueryID, sParam);

                if (ds.Tables.Count > 0)
                {
                    strRtn = ds.Tables[0].Rows[0]["VAL_CHK"].ToString();

                    if (strRtn == "0")
                    {
                        strScript = " alert('해당하는 품번과 차종이 일치하는 값이 없습니다.'); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else
                    {
                        strDBName = "GPDB";
                        strQueryID = "Qua50Data.Set_EOInfo";

                        sParam = new FW.Data.Parameters();
                        sParam.Add("PLANT_CD", bp.g_plant.ToString());
                        sParam.Add("PROD_DT", txtDate.Text.Replace("-", ""));
                        sParam.Add("EO4M_FLAG", ddlEo4mFlag.SelectedValue);
                        sParam.Add("SHOP_CD", ddlShopCd.SelectedValue);
                        sParam.Add("LINE_CD", txtLineCdHidden.Text);
                        sParam.Add("PART_NO", ddlPartNo.SelectedValue);
                        sParam.Add("CAR_TYPE", ddlCarType.SelectedValue);
                        sParam.Add("EO_NO", txtEoNo.Text);
                        sParam.Add("MOD_REMARK", txtModRemark.Text);

                        sParam.Add("USER_ID", bp.g_userid.ToString());

                        sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                        sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                        sParam.Add("CUR_MENU_ID", "Qua50");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                        iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                        if (iRtn == 1)
                        {
                            strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('Qua50'); parent.fn_ModalCloseDiv(); ";
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
                strScript = " fn_ExError(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region ddlPartNo_SelectedIndexChanged
        protected void ddlPartNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();
            string script = string.Empty;
            string jsData1 = "[]";

            //데이터 클리어
            ddlShopCd.Items.Clear();
            ddlCarType.Items.Clear();

            //비활성
            ddlShopCd.Enabled = false;
            ddlCarType.Enabled = false;

            //초기화
            ddlShopCd.Items.Add(new ListItem("선택하세요", ""));
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Qua50Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("PART_NO", ddlPartNo.SelectedValue);
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Shop Code 있으면
                if (ds.Tables[4].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        ddlShopCd.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlShopCd.Enabled = true;
                }

                //Car Type 있으면
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarType.Enabled = true;
                }
                //Line Code 있으면
                if (ds.Tables[5].Rows.Count > 0)
                {
                    jsData1 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[5]);
                }
                
                if (ds.Tables[8].Rows.Count > 0)
                {
                    lbGetPartDesc.Text = ds.Tables[8].Rows[0]["PART_DESC"].ToString();
                }
            }

            script = $@" cLine = {jsData1}; fn_Set_Line(); ";
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "comboData", script, true);
        }
        #endregion

        #region ddlShopCd_SelectedIndexChanged
        protected void ddlShopCd_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();
            string script = string.Empty;
            string jsData1 = "[]";

            //데이터 클리어
            ddlCarType.Items.Clear();

            //비활성
            ddlCarType.Enabled = false;

            //초기화
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Qua50Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("PART_NO", ddlPartNo.SelectedValue);
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Car Type 있으면
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarType.Enabled = true;
                }
                //Line Code 있으면
                if (ds.Tables[5].Rows.Count > 0)
                {
                    jsData1 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[5]);
                }

            }

            script = $@" cLine = {jsData1}; fn_Set_Line(); ";
            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "comboData", script, true);
        }
        #endregion

        #region btnFunctionCall_Click
        protected void btnFunctionCall_Click(object sender, EventArgs e)
        {
            txtLineCd_SelectedIndexChanged();
        }
        #endregion

        #region txtLineCd_SelectedIndexChanged
        private void txtLineCd_SelectedIndexChanged()
        {
            //데이터셋 설정
            DataSet ds = new DataSet();
            string script = string.Empty;

            //데이터 클리어
            ddlCarType.Items.Clear();

            //비활성
            ddlCarType.Enabled = false;

            //초기화
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Qua50Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("PART_NO", ddlPartNo.SelectedValue);
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", txtLineCdHidden.Text);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Car Type 있으면
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarType.Enabled = true;
                }
            }
        }
        #endregion
    }
}
