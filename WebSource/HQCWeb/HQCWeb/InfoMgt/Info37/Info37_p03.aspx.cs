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

namespace HQCWeb.InfoMgt.Info37
{
    public partial class Info37_p03 : System.Web.UI.Page
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
        Biz.InfoManagement.Info37 biz = new Biz.InfoManagement.Info37();

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

                string script = "updateLabels()";
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script, true);
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
            lbWorkName.Text = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language);

            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbCarType.Text = Dictionary_Data.SearchDic("CAR_TYPE", bp.g_language);
            lbStnCd.Text = Dictionary_Data.SearchDic("STN_CD", bp.g_language);
            //lbScanCd.Text = Dictionary_Data.SearchDic("SCAN_CD", bp.g_language);
            lbScanNm.Text = Dictionary_Data.SearchDic("SCAN_NM", bp.g_language);
            lbPartNo.Text = Dictionary_Data.SearchDic("PART_NO", bp.g_language);
            lbScanPartNo.Text = Dictionary_Data.SearchDic("SCAN_PART_NO", bp.g_language);
            lbSeq.Text = Dictionary_Data.SearchDic("SEQ", bp.g_language);
            lbChkData2.Text = Dictionary_Data.SearchDic("CHK_DATA", bp.g_language);
            lbBrcdCheck2.Text = Dictionary_Data.SearchDic("BRCD_CHECK", bp.g_language);
            lbChkData.Text = Dictionary_Data.SearchDic("CHK_DATA", bp.g_language);
            lbBrcdCheck.Text = Dictionary_Data.SearchDic("BRCD_CHECK", bp.g_language);
            //lbChkType.Text = Dictionary_Data.SearchDic("CHK_TYPE", bp.g_language);

            lbRemark1.Text = Dictionary_Data.SearchDic("REMARK1", bp.g_language);
            lbRemark2.Text = Dictionary_Data.SearchDic("REMARK2", bp.g_language);
            lbUseYn.Text = Dictionary_Data.SearchDic("USE_YN", bp.g_language);
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
            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Info37Data.Get_ScanInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("STN_CD", strSplitValue[3].ToString());
            sParam.Add("CAR_TYPE", strSplitValue[4].ToString());
            sParam.Add("SCAN_SEQ", strSplitValue[5].ToString());
            sParam.Add("SCAN_CD", strSplitValue[6].ToString());

            sParam.Add("CUR_MENU_ID", "Info37");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                GetDDL(ds.Tables[0].Rows[0]["SHOP_CD"].ToString(), ds.Tables[0].Rows[0]["LINE_CD"].ToString(), ds.Tables[0].Rows[0]["CAR_TYPE"].ToString());
                ddlStnCd.SelectedValue = ds.Tables[0].Rows[0]["STN_CD"].ToString();
                //txtScanCd.Text = ds.Tables[0].Rows[0]["SCAN_CD"].ToString();

                txtScanNm.Text = ds.Tables[0].Rows[0]["SCAN_NM"].ToString();
                ddlPartNo.SelectedValue = ds.Tables[0].Rows[0]["PART_NO"].ToString();
                if (ddlScanPartNo.Items.FindByValue(ds.Tables[0].Rows[0]["SCAN_PART_NO"].ToString()) != null)
                    ddlScanPartNo.SelectedValue = ds.Tables[0].Rows[0]["SCAN_PART_NO"].ToString();
                else
                    ddlScanPartNo.SelectedValue = "";
                txtChkData.Text = ds.Tables[0].Rows[0]["CHK_DATA"].ToString();
                txtBrcdCheck.Text = ds.Tables[0].Rows[0]["BRCD_CHECK"].ToString();
                //ddlChkType.SelectedValue = ds.Tables[0].Rows[0]["CHK_TYPE"].ToString();
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
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Info37'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            int iRtn = 0;
            string strScript = string.Empty;

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                // 비지니스 클래스 작성
                //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();

                strDBName = "GPDB";
                strQueryID = "Info37Data.Get_ScanID_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("PLANT_CD", bp.g_plant.ToString());
                sParamIDChk.Add("SHOP_CD", ddlShopCd.SelectedValue);
                sParamIDChk.Add("LINE_CD", ddlLineCd.SelectedValue);
                sParamIDChk.Add("STN_CD", ddlStnCd.SelectedValue);
                sParamIDChk.Add("CAR_TYPE", ddlCarType.SelectedValue);
                sParamIDChk.Add("SCAN_SEQ", "-1");//INSERT 시에는 -1 고정값 장입
                sParamIDChk.Add("SCAN_CD", "0");//0 고정
                sParamIDChk.Add("CHK_DATA", txtChkData.Text);
                sParamIDChk.Add("BRCD_CHECK", txtBrcdCheck.Text);

                // 아이디 체크 비지니스 메서드 작성
                ds = biz.GetDataSet(strDBName, strQueryID, sParamIDChk);
                if (ds.Tables[0].Rows[0]["VAL_CHK"].ToString() == "0" && ds.Tables[1].Rows[0]["VAL_CHK"].ToString() == "0")
                {
                    strDBName = "GPDB";
                    strQueryID = "Info37Data.Set_ScanInfo";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("PLANT_CD", bp.g_plant.ToString());
                    sParam.Add("SHOP_CD", ddlShopCd.SelectedValue);
                    sParam.Add("LINE_CD", ddlLineCd.SelectedValue);
                    sParam.Add("STN_CD", ddlStnCd.SelectedValue);
                    sParam.Add("CAR_TYPE", ddlCarType.SelectedValue);
                    sParam.Add("SCAN_SEQ", "-1");//INSERT 시에는 -1 고정값 장입
                    sParam.Add("SCAN_CD", "0");//0 고정
                    sParam.Add("SCAN_NM", txtScanNm.Text);
                    sParam.Add("PART_NO", ddlPartNo.Text);
                    sParam.Add("SCAN_PART_NO", ddlScanPartNo.SelectedValue);
                    sParam.Add("BRCD_CHECK", txtBrcdCheck.Text);
                    sParam.Add("CHK_DATA", txtChkData.Text);
                    sParam.Add("CHK_TYPE", "");//공백
                    //sParam.Add("CHK_TYPE", ddlChkType.SelectedValue);

                    sParam.Add("REMARK1", txtRemark1.Text);
                    sParam.Add("REMARK2", txtRemark2.Text);
                    sParam.Add("USE_YN", ddlUseYN.SelectedValue);
                    sParam.Add("USER_ID", bp.g_userid.ToString());

                    sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                    sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                    sParam.Add("CUR_MENU_ID", "Info37");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                    // 등록 비지니스 메서드 작성
                    iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('Info37'); parent.fn_ModalCloseDiv(); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else
                    {
                        strScript = " alert('등록에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                }
                else if (ds.Tables[0].Rows[0]["VAL_CHK"].ToString() != "0")
                {
                    strScript = " alert('존재하는 데이터 입니다. 등록하려는 데이터를 다시 입력하세요.'); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
                else if (ds.Tables[1].Rows[0]["VAL_CHK"].ToString() != "0")
                {
                    strScript = " alert('데이터와 스캔체크 길이가 불일치합니다.'); ";
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
            ddlPartNo.Items.Clear();
            ddlScanPartNo.Items.Clear();

            //비활성
            ddlLineCd.Enabled = false;
            ddlCarType.Enabled = false;
            ddlStnCd.Enabled = false;
            ddlPartNo.Enabled = false;
            ddlScanPartNo.Enabled = false;

            //초기화
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));
            ddlStnCd.Items.Add(new ListItem("선택하세요", ""));
            ddlPartNo.Items.Add(new ListItem("", ""));
            ddlScanPartNo.Items.Add(new ListItem("", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info37Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", "");
            param.Add("CAR_TYPE", "");

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
                //Car Type 있으면
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarType.Enabled = true;
                }
                //Stn Code 있으면
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        ddlStnCd.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlStnCd.Enabled = true;
                }
                //Part Code 있으면
                if (ds.Tables[4].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        ddlPartNo.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlPartNo.Enabled = true;
                }
                //Scan Part Code 있으면
                if (ds.Tables[7].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {
                        ddlScanPartNo.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlScanPartNo.Enabled = true;
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
            ddlPartNo.Items.Clear();
            ddlScanPartNo.Items.Clear();

            //비활성
            ddlCarType.Enabled = false;
            ddlStnCd.Enabled = false;
            ddlPartNo.Enabled = false;
            ddlScanPartNo.Enabled = false;

            //초기화
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));
            ddlStnCd.Items.Add(new ListItem("선택하세요", ""));
            ddlPartNo.Items.Add(new ListItem("", ""));
            ddlScanPartNo.Items.Add(new ListItem("", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info37Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", ddlLineCd.SelectedValue);
            param.Add("CAR_TYPE", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Car Type 있으면
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarType.Enabled = true;
                }
                //Stn Code 있으면
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        ddlStnCd.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlStnCd.Enabled = true;
                }
                //Part Code 있으면
                if (ds.Tables[4].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        ddlPartNo.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlPartNo.Enabled = true;
                }
                //Scan Part Code 있으면
                if (ds.Tables[7].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {
                        ddlScanPartNo.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlScanPartNo.Enabled = true;
                }

            }
        }
        #endregion

        #region ddlCarType_SelectedIndexChanged
        protected void ddlCarType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlPartNo.Items.Clear();
            ddlScanPartNo.Items.Clear();

            //비활성
            ddlPartNo.Enabled = false;
            ddlScanPartNo.Enabled = false;

            //초기화
            ddlPartNo.Items.Add(new ListItem("", ""));
            ddlScanPartNo.Items.Add(new ListItem("", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info37Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", ddlLineCd.SelectedValue);
            param.Add("CAR_TYPE", ddlCarType.SelectedValue);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Part Code 있으면
                if (ds.Tables[4].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        ddlPartNo.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlPartNo.Enabled = true;
                }
                //Scan Part Code 있으면
                if (ds.Tables[7].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {
                        ddlScanPartNo.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlScanPartNo.Enabled = true;
                }
            }
        }
        #endregion

        #region ddlPartNo_SelectedIndexChanged
        protected void ddlPartNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlScanPartNo.Items.Clear();

            //비활성
            ddlScanPartNo.Enabled = false;

            //초기화
            ddlScanPartNo.Items.Add(new ListItem("", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info37Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", ddlLineCd.SelectedValue);
            param.Add("CAR_TYPE", ddlCarType.SelectedValue);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Scan Part Code 있으면
                if (ds.Tables[7].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {
                        ddlScanPartNo.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlScanPartNo.Enabled = true;
                }
            }
        }
        #endregion

        #region GetDDL
        protected void GetDDL(string shopCd, string lineCd, string carType)
        {
            //GetData 에서 호출(STN 콤보 초기 설정값 필요)
            DataSet ds = new DataSet();

            ddlShopCd.Items.Add(new ListItem("선택하세요", ""));
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));
            ddlStnCd.Items.Add(new ListItem("선택하세요", ""));
            ddlPartNo.Items.Add(new ListItem("", ""));
            ddlScanPartNo.Items.Add(new ListItem("", ""));

            strDBName = "GPDB";
            strQueryID = "Info37Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", shopCd);
            param.Add("LINE_CD", lineCd);
            param.Add("CAR_TYPE", carType);

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
                    ddlCarType.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                }
                ddlCarType.SelectedValue = carType;
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    ddlStnCd.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                {
                    ddlPartNo.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                }
                //for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                //{
                //    ddlChkType.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                //}
                for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                {
                    ddlUseYN.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                {
                    ddlScanPartNo.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                }
            }
        }
        #endregion
    }
}
