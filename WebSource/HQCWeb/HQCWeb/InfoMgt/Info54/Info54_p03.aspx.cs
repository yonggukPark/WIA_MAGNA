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

namespace HQCWeb.InfoMgt.Info54
{
    public partial class Info54_p03 : System.Web.UI.Page
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
        Biz.InfoManagement.Info54 biz = new Biz.InfoManagement.Info54();

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
            // 등록일경우
            lbWorkName.Text = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language); // 등록

            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbStnCd.Text = Dictionary_Data.SearchDic("STN_CD", bp.g_language);
            lbDevId.Text = Dictionary_Data.SearchDic("DEV_ID", bp.g_language);
            lbDevChk.Text = Dictionary_Data.SearchDic("DEV_CHK", bp.g_language);
            lbInsertTable.Text = Dictionary_Data.SearchDic("INSERT_TABLE", bp.g_language);
            lbCarType.Text = Dictionary_Data.SearchDic("CAR_TYPE", bp.g_language);
            lbItemSeq.Text = Dictionary_Data.SearchDic("ITEM_SEQ", bp.g_language);
            lbItemCd.Text = Dictionary_Data.SearchDic("ITEM_CD", bp.g_language);
            lbItemNm.Text = Dictionary_Data.SearchDic("ITEM_NM", bp.g_language);
            lbItemMin.Text = Dictionary_Data.SearchDic("ITEM_MIN", bp.g_language);
            lbItemMax.Text = Dictionary_Data.SearchDic("ITEM_MAX", bp.g_language);
            lbWorkCd.Text = Dictionary_Data.SearchDic("WORK_CD", bp.g_language);
            lbShopCdT.Text = Dictionary_Data.SearchDic("SHOP_CD_T", bp.g_language);
            lbLineCdT.Text = Dictionary_Data.SearchDic("LINE_CD_T", bp.g_language);
            lbCarTypeT.Text = Dictionary_Data.SearchDic("CAR_TYPE_T", bp.g_language);
            lbPSet.Text = Dictionary_Data.SearchDic("P_SET", bp.g_language);
            lbResultLoc.Text = Dictionary_Data.SearchDic("RESULT_LOC", bp.g_language);
            lbPdaChkYn.Text = Dictionary_Data.SearchDic("PDA_CHK_YN", bp.g_language);
            lbHkmcTransInsItemNm.Text = Dictionary_Data.SearchDic("HKMC_TRANS_INS_ITEM_NM", bp.g_language);
            lbHkmcCompany.Text = Dictionary_Data.SearchDic("HKMC_COMPANY", bp.g_language);
            lbHkmcRegion.Text = Dictionary_Data.SearchDic("HKMC_REGION", bp.g_language);
            lbHkmcSupplier.Text = Dictionary_Data.SearchDic("HKMC_SUPPLIER", bp.g_language);
            lbHkmcShop.Text = Dictionary_Data.SearchDic("HKMC_SHOP", bp.g_language);
            lbHkmcLine.Text = Dictionary_Data.SearchDic("HKMC_LINE", bp.g_language);
            lbHkmcCarType.Text = Dictionary_Data.SearchDic("HKMC_CAR_TYPE", bp.g_language);
            lbToolCnt.Text = Dictionary_Data.SearchDic("TOOL_CNT", bp.g_language);

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
            strQueryID = "Info54Data.Get_IFInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("STN_CD", strSplitValue[3].ToString());
            sParam.Add("DEV_ID", strSplitValue[4].ToString());
            sParam.Add("DEV_CHK", strSplitValue[5].ToString());
            sParam.Add("INSERT_TABLE", strSplitValue[6].ToString());
            sParam.Add("CAR_TYPE", strSplitValue[7].ToString());
            sParam.Add("ITEM_SEQ", strSplitValue[8].ToString());

            sParam.Add("CUR_MENU_ID", "Info54");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
                    GetDDL(ds.Tables[0].Rows[0]["SHOP_CD"].ToString(), ds.Tables[0].Rows[0]["LINE_CD"].ToString(), ds.Tables[0].Rows[0]["STN_CD"].ToString());
                    ddlCarType.SelectedValue = ds.Tables[0].Rows[0]["CAR_TYPE"].ToString();
                    ddlDevId.SelectedValue = ds.Tables[0].Rows[0]["DEV_ID"].ToString();
                    ddlDevChk.SelectedValue = ds.Tables[0].Rows[0]["DEV_CHK"].ToString();
                    txtInsertTable.Text = ds.Tables[0].Rows[0]["INSERT_TABLE"].ToString();
                    txtItemSeq.Text = ds.Tables[0].Rows[0]["ITEM_SEQ"].ToString();

                    txtItemCd.Text = ds.Tables[0].Rows[0]["ITEM_CD"].ToString();
                    txtItemNm.Text = ds.Tables[0].Rows[0]["ITEM_NM"].ToString();
                    txtItemMin.Text = ds.Tables[0].Rows[0]["ITEM_MIN"].ToString();
                    txtItemMax.Text = ds.Tables[0].Rows[0]["ITEM_MAX"].ToString();
                    txtHkmcTransInsItemNm.Text = ds.Tables[0].Rows[0]["HKMC_TRANS_INS_ITEM_NM"].ToString();
                    txtWorkCd.Text = ds.Tables[0].Rows[0]["WORK_CD"].ToString();
                    txtShopCdT.Text = ds.Tables[0].Rows[0]["SHOP_CD_T"].ToString();
                    txtLineCdT.Text = ds.Tables[0].Rows[0]["LINE_CD_T"].ToString();
                    txtCarTypeT.Text = ds.Tables[0].Rows[0]["CAR_TYPE_T"].ToString();
                    txtPSet.Text = ds.Tables[0].Rows[0]["P_SET"].ToString();
                    txtResultLoc.Text = ds.Tables[0].Rows[0]["RESULT_LOC"].ToString();
                    txtHkmcCompany.Text = ds.Tables[0].Rows[0]["HKMC_COMPANY"].ToString();
                    txtHkmcRegion.Text = ds.Tables[0].Rows[0]["HKMC_REGION"].ToString();
                    txtHkmcSupplier.Text = ds.Tables[0].Rows[0]["HKMC_SUPPLIER"].ToString();
                    txtHkmcShop.Text = ds.Tables[0].Rows[0]["HKMC_SHOP"].ToString();
                    txtHkmcLine.Text = ds.Tables[0].Rows[0]["HKMC_LINE"].ToString();
                    txtHkmcCarType.Text = ds.Tables[0].Rows[0]["HKMC_CAR_TYPE"].ToString();
                    txtToolCnt.Text = ds.Tables[0].Rows[0]["TOOL_CNT"].ToString();

                    ddlPdaChkYn.SelectedValue = ds.Tables[0].Rows[0]["PDA_CHK_YN"].ToString();
                    ddlUseYN.SelectedValue = ds.Tables[0].Rows[0]["USE_YN"].ToString();
                    txtRemark1.Text = ds.Tables[0].Rows[0]["REMARK1"].ToString();
                    txtRemark2.Text = ds.Tables[0].Rows[0]["REMARK2"].ToString();

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
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Info54'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
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

                strDBName = "GPDB";
                strQueryID = "Info54Data.Get_IFID_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("PLANT_CD", bp.g_plant.ToString());
                sParamIDChk.Add("SHOP_CD", ddlShopCd.SelectedValue);
                sParamIDChk.Add("LINE_CD", ddlLineCd.SelectedValue);
                sParamIDChk.Add("STN_CD", ddlStnCd.SelectedValue);
                sParamIDChk.Add("DEV_ID", ddlDevId.SelectedValue);
                sParamIDChk.Add("DEV_CHK", ddlDevChk.SelectedValue);
                sParamIDChk.Add("INSERT_TABLE", txtInsertTable.Text);
                sParamIDChk.Add("CAR_TYPE", ddlCarType.SelectedValue);
                sParamIDChk.Add("ITEM_SEQ", txtItemSeq.Text);

                // 아이디 체크 비지니스 메서드 작성
                strRtn = biz.GetIDValChk(strDBName, strQueryID, sParamIDChk);
                if (strRtn == "0")
                {
                    strDBName = "GPDB";
                    strQueryID = "Info54Data.Set_IFInfo";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("PLANT_CD", bp.g_plant.ToString());
                    sParam.Add("SHOP_CD", ddlShopCd.SelectedValue);
                    sParam.Add("LINE_CD", ddlLineCd.SelectedValue);
                    sParam.Add("STN_CD", ddlStnCd.SelectedValue);
                    sParam.Add("DEV_ID", ddlDevId.SelectedValue);
                    sParam.Add("DEV_CHK", ddlDevChk.SelectedValue);
                    sParam.Add("INSERT_TABLE", txtInsertTable.Text);
                    sParam.Add("CAR_TYPE", ddlCarType.SelectedValue);
                    sParam.Add("ITEM_SEQ", txtItemSeq.Text);

                    sParam.Add("ITEM_CD", txtItemCd.Text);
                    sParam.Add("ITEM_NM", txtItemNm.Text);
                    sParam.Add("ITEM_MIN", txtItemMin.Text);
                    sParam.Add("ITEM_MAX", txtItemMax.Text);
                    sParam.Add("HKMC_TRANS_INS_ITEM_NM", txtHkmcTransInsItemNm.Text);
                    sParam.Add("WORK_CD", txtWorkCd.Text);
                    sParam.Add("SHOP_CD_T", txtShopCdT.Text);
                    sParam.Add("LINE_CD_T", txtLineCdT.Text);
                    sParam.Add("CAR_TYPE_T", txtCarTypeT.Text);
                    sParam.Add("P_SET", txtPSet.Text);
                    sParam.Add("RESULT_LOC", txtResultLoc.Text);
                    sParam.Add("HKMC_COMPANY", txtHkmcCompany.Text);
                    sParam.Add("HKMC_REGION", txtHkmcRegion.Text);
                    sParam.Add("HKMC_SUPPLIER", txtHkmcSupplier.Text);
                    sParam.Add("HKMC_SHOP", txtHkmcShop.Text);
                    sParam.Add("HKMC_LINE", txtHkmcLine.Text);
                    sParam.Add("HKMC_CAR_TYPE", txtHkmcCarType.Text);
                    sParam.Add("PDA_CHK_YN", ddlPdaChkYn.SelectedValue);
                    sParam.Add("TOOL_CNT", txtToolCnt.Text);

                    sParam.Add("REMARK1", txtRemark1.Text);
                    sParam.Add("REMARK2", txtRemark2.Text);
                    sParam.Add("USE_YN", ddlUseYN.SelectedValue);
                    sParam.Add("USER_ID", bp.g_userid.ToString());

                    sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                    sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                    sParam.Add("CUR_MENU_ID", "Info54");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                    // 등록 비지니스 메서드 작성
                    iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('Info54'); parent.fn_ModalCloseDiv(); ";
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
                    strScript = " alert('존재하는 데이터 입니다. 등록하려는 데이터를 다시 입력하세요.'); ";
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
            ddlCarType.Items.Clear();
            ddlStnCd.Items.Clear();
            ddlDevId.Items.Clear();

            //비활성
            ddlLineCd.Enabled = false;
            ddlCarType.Enabled = false;
            ddlStnCd.Enabled = false;
            ddlDevId.Enabled = false;

            //초기화
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));
            ddlStnCd.Items.Add(new ListItem("선택하세요", ""));
            ddlDevId.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info54Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", "");
            param.Add("STN_CD", "");

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
                //Stn code 있으면
                if (ds.Tables[5].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                    {
                        ddlStnCd.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlStnCd.Enabled = true;
                }
                //Dev ID 있으면
                if (ds.Tables[6].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                    {
                        ddlDevId.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevId.Enabled = true;
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
            ddlStnCd.Items.Clear();
            ddlDevId.Items.Clear();

            //비활성
            ddlCarType.Enabled = false;
            ddlStnCd.Enabled = false;
            ddlDevId.Enabled = false;

            //초기화
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));
            ddlStnCd.Items.Add(new ListItem("선택하세요", ""));
            ddlDevId.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info54Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", ddlLineCd.SelectedValue);
            param.Add("STN_CD", "");

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
                //Stn code 있으면
                if (ds.Tables[5].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                    {
                        ddlStnCd.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlStnCd.Enabled = true;
                }
                //Dev ID 있으면
                if (ds.Tables[6].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                    {
                        ddlDevId.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevId.Enabled = true;
                }
            }
        }
        #endregion

        #region ddlStnCd_SelectedIndexChanged
        protected void ddlStnCd_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlDevId.Items.Clear();

            //비활성
            ddlDevId.Enabled = false;

            //초기화
            ddlDevId.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info54Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", ddlLineCd.SelectedValue);
            param.Add("STN_CD", ddlStnCd.SelectedValue);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Dev ID 있으면
                if (ds.Tables[6].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                    {
                        ddlDevId.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevId.Enabled = true;
                }
            }
        }
        #endregion

        #region GetDDL
        protected void GetDDL(string shopCd, string lineCd, string stnCd)
        {
            //GetData 에서 호출(LINE 콤보 초기 설정값 필요)
            DataSet ds = new DataSet();

            ddlShopCd.Items.Add(new ListItem("선택하세요", ""));
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));

            strDBName = "GPDB";
            strQueryID = "Info54Data.Get_DdlData";

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
                    ddlStnCd.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                }
                ddlStnCd.SelectedValue = stnCd;
                for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                {
                    ddlDevId.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                {
                    ddlDevChk.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                {
                    ddlPdaChkYn.Items.Add(new ListItem(ds.Tables[8].Rows[i]["CODE_NM"].ToString(), ds.Tables[8].Rows[i]["CODE_ID"].ToString()));
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
