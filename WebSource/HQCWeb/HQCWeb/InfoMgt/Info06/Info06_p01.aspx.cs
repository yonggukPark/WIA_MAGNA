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
    public partial class Info06_p01 : System.Web.UI.Page
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
            DataSet ds = new DataSet();

            ddlShopCd.Items.Add(new ListItem("선택하세요", ""));
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));
            ddlReworkStn.Items.Add(new ListItem("", ""));
            ddlPrStn.Items.Add(new ListItem("", ""));
            ddlTotStn.Items.Add(new ListItem("", ""));
            ddlMergeStn.Items.Add(new ListItem("", ""));

            strDBName = "GPDB";
            strQueryID = "Info06Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", "");
            param.Add("LINE_CD", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlShopCd.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    ddlLineCd.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                }
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
                ddlUseYN.SelectedIndex = 0;
                ddlViewYn.SelectedIndex = 0;
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbWorkName.Text = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language); // 등록

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
            lbReinputStnCd.Text = Dictionary_Data.SearchDic("REINPUT_STN_CD", bp.g_language);
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
            //등록 팝업이므로 초기값 없음(복사 기능시 사용)
        }
        #endregion

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int iRtn = 0;
            string strScript = string.Empty, strRtn = string.Empty;

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                strDBName = "GPDB";
                strQueryID = "Info06Data.Get_StnID_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("PLANT_CD", bp.g_plant.ToString());
                sParamIDChk.Add("SHOP_CD", ddlShopCd.SelectedValue);
                sParamIDChk.Add("LINE_CD", ddlLineCd.SelectedValue);
                sParamIDChk.Add("STN_CD", txtStnCd.Text);

                // 아이디 체크 비지니스 메서드 작성
                strRtn = biz.GetIDValChk(strDBName, strQueryID, sParamIDChk);
                if (strRtn == "0")
                {
                    strDBName = "GPDB";
                    strQueryID = "Info06Data.Set_StnInfo";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("PLANT_CD", bp.g_plant.ToString());
                    sParam.Add("SHOP_CD", ddlShopCd.SelectedValue);
                    sParam.Add("LINE_CD", ddlLineCd.SelectedValue);
                    sParam.Add("STN_CD", txtStnCd.Text);
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
                    sParam.Add("REWORK_STN_CD", ddlMergeStn.SelectedValue);
                    sParam.Add("P_R_STN_CD", ddlMergeStn.SelectedValue);
                    sParam.Add("TOTAL_STN_CD", ddlMergeStn.SelectedValue);
                    sParam.Add("REINPUT_STN_CD", txtReinputStnCd.Text);
                    sParam.Add("VIEW_YN", ddlViewYn.SelectedValue);
                    sParam.Add("USE_YN", ddlUseYN.SelectedValue);
                    sParam.Add("REMARK1", txtRemark1.Text);
                    sParam.Add("REMARK2", txtRemark2.Text);
                    sParam.Add("USER_ID", bp.g_userid.ToString());

                    sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                    sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                    sParam.Add("CUR_MENU_ID", "Info06");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                    // 등록 비지니스 메서드 작성
                    iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('Info06'); parent.fn_ModalCloseDiv(); ";
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
                    strScript = " alert('존재하는 공정코드 입니다. 등록하려는 공정코드를 다시 입력하세요.'); ";
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

        #region ddlShopCd_SelectedIndexChanged
        protected void ddlShopCd_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlLineCd.Items.Clear();
            ddlMergeStn.Items.Clear();
            ddlReworkStn.Items.Clear();
            ddlPrStn.Items.Clear();
            ddlTotStn.Items.Clear();

            //비활성
            ddlLineCd.Enabled = false;
            ddlMergeStn.Enabled = false;
            ddlReworkStn.Enabled = false;
            ddlPrStn.Enabled = false;
            ddlTotStn.Enabled = false;

            //초기화
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));
            ddlMergeStn.Items.Add(new ListItem("", ""));
            ddlReworkStn.Items.Add(new ListItem("", ""));
            ddlPrStn.Items.Add(new ListItem("", ""));
            ddlTotStn.Items.Add(new ListItem("", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info06Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Line code 있으면
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ddlLineCd.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlLineCd.Enabled = true;
                }
                //Stn code 있으면
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        ddlMergeStn.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                        ddlReworkStn.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                        ddlPrStn.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                        ddlTotStn.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlMergeStn.Enabled = true;
                    ddlReworkStn.Enabled = true;
                    ddlPrStn.Enabled = true;
                    ddlTotStn.Enabled = true;
                }
            }
        }
        #endregion

        #region ddlLineCd_SelectedIndexChanged
        protected void ddlLineCd_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlMergeStn.Items.Clear();
            ddlReworkStn.Items.Clear();
            ddlPrStn.Items.Clear();
            ddlTotStn.Items.Clear();

            //비활성
            ddlMergeStn.Enabled = false;
            ddlReworkStn.Enabled = false;
            ddlPrStn.Enabled = false;
            ddlTotStn.Enabled = false;

            //초기화
            ddlMergeStn.Items.Add(new ListItem("", ""));
            ddlReworkStn.Items.Add(new ListItem("", ""));
            ddlPrStn.Items.Add(new ListItem("", ""));
            ddlTotStn.Items.Add(new ListItem("", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info06Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", ddlLineCd.SelectedValue);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Stn code 있으면
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        ddlMergeStn.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                        ddlReworkStn.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                        ddlPrStn.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                        ddlTotStn.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlMergeStn.Enabled = true;
                    ddlReworkStn.Enabled = true;
                    ddlPrStn.Enabled = true;
                    ddlTotStn.Enabled = true;
                }
            }
        }
        #endregion
    }
}
