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
    public partial class Info08_p01 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

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
            strErrMessage = Message_Data.SearchDic("SearchError", bp.g_language);

            DataSet ds = new DataSet();

            ddlShopCd.Items.Add(new ListItem("선택하세요", ""));
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));
            ddlDevId.Items.Add(new ListItem("선택하세요", ""));
            ddlDevKind.Items.Add(new ListItem("선택하세요", ""));
            ddlType.Items.Add(new ListItem("선택하세요", ""));

            strDBName = "GPDB";
            strQueryID = "Info08Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", "");
            param.Add("LINE_CD", "");
            param.Add("DEV_ID", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlShopCd.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                {
                    ddlUseYN.Items.Add(new ListItem(ds.Tables[8].Rows[i]["CODE_NM"].ToString(), ds.Tables[8].Rows[i]["CODE_ID"].ToString()));
                }
            }

            ddlLineCd.Enabled = false;
            ddlCarType.Enabled = false;
            ddlDevId.Enabled = false;
            ddlDevKind.Enabled = false;
            ddlType.Enabled = false;
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            // 등록일경우
            lbWorkName.Text = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language); // 등록

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
                //strDBName = "GPDB";
                //strQueryID = "Info08Data.Get_PrintID_ValChk";

                //FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                //sParamIDChk.Add("PLANT_CD", bp.g_plant.ToString());
                //sParamIDChk.Add("SHOP_CD", ddlShopCd.SelectedValue);
                //sParamIDChk.Add("LINE_CD", ddlLineCd.SelectedValue);
                //sParamIDChk.Add("CAR_TYPE", ddlCarType.SelectedValue);
                //sParamIDChk.Add("DEV_ID", ddlDevId.SelectedValue);
                //sParamIDChk.Add("DEV_KIND", ddlDevKind.SelectedValue);
                //sParamIDChk.Add("TYPE", ddlType.SelectedValue);
                //sParamIDChk.Add("CODE", txtCode.Text);

                // 아이디 체크 비지니스 메서드 작성
                //strRtn = biz.GetIDValChk(strDBName, strQueryID, sParamIDChk);
                //if (strRtn == "0")
                //{
                    strDBName = "GPDB";
                    strQueryID = "Info08Data.Set_PrintInfo";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("PLANT_CD", bp.g_plant.ToString());
                    sParam.Add("SHOP_CD", ddlShopCd.SelectedValue);
                    sParam.Add("LINE_CD", ddlLineCd.SelectedValue);
                    sParam.Add("CAR_TYPE", ddlCarType.SelectedValue);
                    sParam.Add("SEQ", "0");
                    sParam.Add("DEV_ID", ddlDevId.SelectedValue);
                    sParam.Add("DEV_KIND", ddlDevKind.SelectedValue);
                    sParam.Add("TYPE", ddlType.SelectedValue);
                    sParam.Add("CODE", txtCode.Text);

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
                    sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                    sParam.Add("CUR_MENU_ID", "Info08");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                    // 등록 비지니스 메서드 작성
                    iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('Info08'); parent.fn_ModalCloseDiv(); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else
                    {
                        strScript = " alert('등록에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                //}
                //else
                //{
                //    strScript = " alert('존재하는 데이터 입니다. 등록하려는 데이터를 다시 입력하세요.'); ";
                //    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                //}

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
            ddlCarType.Items.Clear();
            ddlDevId.Items.Clear();
            ddlDevKind.Items.Clear();
            ddlType.Items.Clear();

            //비활성
            ddlLineCd.Enabled = false;
            ddlCarType.Enabled = false;
            ddlDevId.Enabled = false;
            ddlDevKind.Enabled = false;
            ddlType.Enabled = false;

            //초기화
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));
            ddlDevId.Items.Add(new ListItem("선택하세요", ""));
            ddlDevKind.Items.Add(new ListItem("선택하세요", ""));
            ddlType.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info08Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", "");
            param.Add("DEV_ID", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Line code 있으면
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        ddlLineCd.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlLineCd.Enabled = true;
                }
                //Car code 있으면
                if (ds.Tables[4].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarType.Enabled = true;
                }
                //Dev ID 있으면
                if (ds.Tables[5].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                    {
                        ddlDevId.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevId.Enabled = true;
                }
                //Dev Kind 있으면
                if (ds.Tables[6].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                    {
                        ddlDevKind.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevKind.Enabled = true;
                }
                //Type 있으면
                if (ds.Tables[7].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {
                        ddlType.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlType.Enabled = true;
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
            ddlCarType.Items.Clear();
            ddlDevId.Items.Clear();

            //비활성
            ddlCarType.Enabled = false;
            ddlDevId.Enabled = false;

            //초기화
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));
            ddlDevId.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info08Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", ddlLineCd.SelectedValue);
            param.Add("DEV_ID", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Car code 있으면
                if (ds.Tables[4].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarType.Enabled = true;
                }
                //Dev ID 있으면
                if (ds.Tables[5].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                    {
                        ddlDevId.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevId.Enabled = true;
                }
            }
        }
        #endregion

        #region ddlDevId_SelectedIndexChanged
        protected void ddlDevId_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlType.Items.Clear();

            //비활성
            ddlType.Enabled = false;

            //초기화
            ddlType.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info08Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", ddlLineCd.SelectedValue);
            param.Add("DEV_ID", ddlDevId.SelectedValue);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Type 있으면
                if (ds.Tables[7].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {
                        ddlType.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlType.Enabled = true;
                }
            }
        }
        #endregion

    }
}
