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

namespace HQCWeb.InfoMgt.Info19
{
    public partial class Info19_p04 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        // 비지니스 클래스 작성
        Biz.InfoManagement.Info19 biz = new Biz.InfoManagement.Info19();

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
            
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbCarType.Text = Dictionary_Data.SearchDic("CAR_TYPE", bp.g_language);
            lbStnCd.Text = Dictionary_Data.SearchDic("STN_CD", bp.g_language);
            lbDevID.Text = Dictionary_Data.SearchDic("DEV_ID", bp.g_language);
            lbMatchCd.Text = Dictionary_Data.SearchDic("MATCH_CD", bp.g_language);
            lbModeFlag.Text = Dictionary_Data.SearchDic("MODE_FLAG", bp.g_language);
            lbPset.Text = Dictionary_Data.SearchDic("P_SET", bp.g_language);
            lbQty.Text = Dictionary_Data.SearchDic("QTY", bp.g_language);
            lbScanCd.Text = Dictionary_Data.SearchDic("SCAN_CD", bp.g_language);
            lbToolHole.Text = Dictionary_Data.SearchDic("TOOL_HOLE", bp.g_language);
            lbToolType.Text = Dictionary_Data.SearchDic("TOOL_TYPE", bp.g_language);
            lbTorqueType.Text = Dictionary_Data.SearchDic("TORQUE_TYPE", bp.g_language);
            lbWorkCd.Text = Dictionary_Data.SearchDic("WORK_CD", bp.g_language);
            lbWorkNm.Text = Dictionary_Data.SearchDic("WORK_NM", bp.g_language);
            lbWorkSeq.Text = Dictionary_Data.SearchDic("WORK_SEQ", bp.g_language);
            lbWorkType.Text = Dictionary_Data.SearchDic("WORK_TYPE", bp.g_language);
            lbRemark1.Text = Dictionary_Data.SearchDic("REMARK1", bp.g_language);
            lbRemark2.Text = Dictionary_Data.SearchDic("REMARK2", bp.g_language);
            lbUseYn.Text = Dictionary_Data.SearchDic("USE_YN", bp.g_language);

            // 등록일경우
            lbWorkName.Text = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language); // 등록
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
            strQueryID = "Info19Data.Get_WorkInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("STN_CD", strSplitValue[3].ToString());
            sParam.Add("CAR_TYPE", strSplitValue[4].ToString());
            sParam.Add("WORK_SEQ", strSplitValue[5].ToString());

            sParam.Add("CUR_MENU_ID", "Info19");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                //GetData 에서 호출(STN 콤보 초기 설정값 필요)
                GetDDL(ds.Tables[0].Rows[0]["SHOP_CD"].ToString(), ds.Tables[0].Rows[0]["LINE_CD"].ToString(), ds.Tables[0].Rows[0]["CAR_TYPE"].ToString());
                ddlStnCd.SelectedValue = ds.Tables[0].Rows[0]["STN_CD"].ToString();
                txtWorkSeq.Text = ds.Tables[0].Rows[0]["WORK_SEQ"].ToString();
                txtWorkNm.Text = ds.Tables[0].Rows[0]["WORK_NM"].ToString();
                txtWorkCd.Text = ds.Tables[0].Rows[0]["WORK_CD"].ToString();
                txtToolType.Text = ds.Tables[0].Rows[0]["TOOL_TYPE"].ToString();
                txtQty.Text = ds.Tables[0].Rows[0]["CNT"].ToString();
                txtTorqueType.Text = ds.Tables[0].Rows[0]["TORQUE_TYPE"].ToString();
                txtPset.Text = ds.Tables[0].Rows[0]["P_SET"].ToString();
                txtToolHole.Text = ds.Tables[0].Rows[0]["TOOL_HOLE"].ToString();
                ddlDevID.SelectedValue = ds.Tables[0].Rows[0]["DEV_ID"].ToString();
                ddlWorkType.SelectedValue = ds.Tables[0].Rows[0]["WORK_TYPE"].ToString();
                ddlScanCd.SelectedValue = ds.Tables[0].Rows[0]["SCAN_CD"].ToString();
                ddlMatchCd.SelectedValue = ds.Tables[0].Rows[0]["MATCH_CD"].ToString();
                ddlModeFlag.SelectedValue = ds.Tables[0].Rows[0]["MODE_FLAG"].ToString();
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
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Info19'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int iRtn = 0;
            string strRtn = string.Empty;
            string strScript = string.Empty;

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {

                strDBName = "GPDB";
                strQueryID = "Info19Data.Get_WorkID_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("PLANT_CD", bp.g_plant.ToString());
                sParamIDChk.Add("SHOP_CD", ddlShopCd.SelectedValue);
                sParamIDChk.Add("LINE_CD", ddlLineCd.SelectedValue);
                sParamIDChk.Add("STN_CD", ddlStnCd.SelectedValue);
                sParamIDChk.Add("CAR_TYPE", ddlCarType.SelectedValue);
                sParamIDChk.Add("WORK_SEQ", txtWorkSeq.Text);

                // 아이디 체크 비지니스 메서드 작성
                strRtn = biz.GetWorkIDValChk(strDBName, strQueryID, sParamIDChk);
                if (iRtn == 0)
                {
                    strDBName = "GPDB";
                    strQueryID = "Info19Data.Set_WorkInfo";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("PLANT_CD", bp.g_plant.ToString());
                    sParam.Add("SHOP_CD", ddlShopCd.SelectedValue);
                    sParam.Add("LINE_CD", ddlLineCd.SelectedValue);
                    sParam.Add("STN_CD", ddlStnCd.SelectedValue);
                    sParam.Add("CAR_TYPE", ddlCarType.SelectedValue);
                    sParam.Add("WORK_SEQ", txtWorkSeq.Text);
                    sParam.Add("WORK_NM", txtWorkNm.Text);
                    sParam.Add("WORK_CD", txtWorkCd.Text);
                    sParam.Add("TOOL_TYPE", txtToolType.Text);
                    sParam.Add("CNT", txtQty.Text);
                    sParam.Add("TORQUE_TYPE", txtTorqueType.Text);
                    sParam.Add("P_SET", txtPset.Text);
                    sParam.Add("TOOL_HOLE", txtToolHole.Text);
                    sParam.Add("DEV_ID", ddlDevID.SelectedValue);
                    sParam.Add("WORK_TYPE", ddlWorkType.SelectedValue);
                    sParam.Add("SCAN_CD", ddlScanCd.SelectedValue);
                    sParam.Add("MATCH_CD", ddlMatchCd.SelectedValue);
                    sParam.Add("MODE_FLAG", ddlModeFlag.SelectedValue);
                    sParam.Add("USE_YN", ddlUseYN.SelectedValue);
                    sParam.Add("REMARK1", txtRemark1.Text);
                    sParam.Add("REMARK2", txtRemark2.Text);
                    sParam.Add("USER_ID", bp.g_userid.ToString());

                    sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                    sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                    sParam.Add("CUR_MENU_ID", "Info19");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                    // 등록 비지니스 메서드 작성
                    iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('Info19'); parent.fn_ModalCloseDiv(); ";
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
            ddlDevID.Items.Clear();
            ddlScanCd.Items.Clear();
            ddlMatchCd.Items.Clear();

            //비활성
            ddlLineCd.Enabled = false;
            ddlCarType.Enabled = false;
            ddlStnCd.Enabled = false;
            ddlDevID.Enabled = false;
            ddlScanCd.Enabled = false;
            ddlMatchCd.Enabled = false;


            //초기화
            ddlLineCd.Items.Add(new ListItem("선택하세요", ""));
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));
            ddlStnCd.Items.Add(new ListItem("선택하세요", ""));
            ddlDevID.Items.Add(new ListItem("선택하세요", ""));
            ddlScanCd.Items.Add(new ListItem("선택하세요", ""));
            ddlMatchCd.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info19Data.Get_DdlData";

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
                //Device Code 있으면
                if (ds.Tables[6].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                    {
                        ddlDevID.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevID.Enabled = true;
                }
                //Scan Code 있으면
                if (ds.Tables[7].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {
                        ddlScanCd.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlScanCd.Enabled = true;
                }
                //Match Code 있으면
                if (ds.Tables[8].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        ddlMatchCd.Items.Add(new ListItem(ds.Tables[8].Rows[i]["CODE_NM"].ToString(), ds.Tables[8].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlMatchCd.Enabled = true;
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
            ddlDevID.Items.Clear();
            ddlScanCd.Items.Clear();
            ddlMatchCd.Items.Clear();

            //비활성
            ddlCarType.Enabled = false;
            ddlStnCd.Enabled = false;
            ddlDevID.Enabled = false;
            ddlScanCd.Enabled = false;
            ddlMatchCd.Enabled = false;


            //초기화
            ddlCarType.Items.Add(new ListItem("선택하세요", ""));
            ddlStnCd.Items.Add(new ListItem("선택하세요", ""));
            ddlDevID.Items.Add(new ListItem("선택하세요", ""));
            ddlScanCd.Items.Add(new ListItem("선택하세요", ""));
            ddlMatchCd.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info19Data.Get_DdlData";

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
                //Device Code 있으면
                if (ds.Tables[6].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                    {
                        ddlDevID.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevID.Enabled = true;
                }
                //Scan Code 있으면
                if (ds.Tables[7].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {
                        ddlScanCd.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlScanCd.Enabled = true;
                }
                //Match Code 있으면
                if (ds.Tables[8].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                    {
                        ddlMatchCd.Items.Add(new ListItem(ds.Tables[8].Rows[i]["CODE_NM"].ToString(), ds.Tables[8].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlMatchCd.Enabled = true;
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
            ddlStnCd.Items.Clear();
            ddlDevID.Items.Clear();
            ddlScanCd.Items.Clear();
            ddlMatchCd.Items.Clear();

            //비활성
            ddlStnCd.Enabled = false;
            ddlDevID.Enabled = false;
            ddlScanCd.Enabled = false;
            ddlMatchCd.Enabled = false;


            //초기화
            ddlStnCd.Items.Add(new ListItem("선택하세요", ""));
            ddlDevID.Items.Add(new ListItem("선택하세요", ""));
            ddlScanCd.Items.Add(new ListItem("선택하세요", ""));
            ddlMatchCd.Items.Add(new ListItem("선택하세요", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info19Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", ddlLineCd.SelectedValue);
            param.Add("CAR_TYPE", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Stn Code 있으면
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        ddlStnCd.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlStnCd.Enabled = true;
                }
                //Device Code 있으면
                if (ds.Tables[6].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                    {
                        ddlDevID.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevID.Enabled = true;
                }
                //Scan Code 있으면
                if (ds.Tables[7].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {
                        ddlScanCd.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlScanCd.Enabled = true;
                }
                //Match Code 있으면
                if (ds.Tables[8].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                    {
                        ddlMatchCd.Items.Add(new ListItem(ds.Tables[8].Rows[i]["CODE_NM"].ToString(), ds.Tables[8].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlMatchCd.Enabled = true;
                }
            }
        }
        #endregion

        #region GetDDL
        protected void GetDDL(string shopCd, string lineCd, string carType)
        {
            //GetData 에서 호출(STN 콤보 초기 설정값 필요)
            DataSet ds = new DataSet();

            ddlShopCd.Items.Add(new ListItem("", ""));
            ddlLineCd.Items.Add(new ListItem("", ""));
            ddlCarType.Items.Add(new ListItem("", ""));
            ddlStnCd.Items.Add(new ListItem("", ""));
            ddlWorkType.Items.Add(new ListItem("", ""));
            ddlModeFlag.Items.Add(new ListItem("", ""));
            ddlDevID.Items.Add(new ListItem("", ""));
            ddlScanCd.Items.Add(new ListItem("", ""));
            ddlMatchCd.Items.Add(new ListItem("", ""));

            strDBName = "GPDB";
            strQueryID = "Info19Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", shopCd);
            param.Add("LINE_CD", lineCd);
            param.Add("CAR_TYPE", carType);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //shop code
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlShopCd.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlShopCd.SelectedValue = shopCd;
                }

                //line code
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ddlLineCd.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlLineCd.SelectedValue = lineCd;
                }

                //car type
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarType.SelectedValue = carType;
                }

                //stn code
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        ddlStnCd.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                    }
                }

                //work type
                if (ds.Tables[4].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                    {
                        ddlWorkType.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                    }
                }

                //mode flag
                if (ds.Tables[5].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
                    {
                        ddlModeFlag.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
                    }
                }

                //device
                if (ds.Tables[6].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
                    {
                        ddlDevID.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
                    }
                }

                //scan code
                if (ds.Tables[7].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
                    {
                        ddlScanCd.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
                    }
                }

                //match code
                if (ds.Tables[8].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                    {
                        ddlMatchCd.Items.Add(new ListItem(ds.Tables[8].Rows[i]["CODE_NM"].ToString(), ds.Tables[8].Rows[i]["CODE_ID"].ToString()));
                    }
                }

                //use YN
                if (ds.Tables[9].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[9].Rows.Count; i++)
                    {
                        ddlUseYN.Items.Add(new ListItem(ds.Tables[9].Rows[i]["CODE_NM"].ToString(), ds.Tables[9].Rows[i]["CODE_ID"].ToString()));
                    }
                }
            }
        }
        #endregion
    }
}
