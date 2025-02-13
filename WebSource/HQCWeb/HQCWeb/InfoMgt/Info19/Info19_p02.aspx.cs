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
    public partial class Info19_p02 : System.Web.UI.Page
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

            // 상세내용 확인후 수정 또는 삭제일 경우
            lbWorkName.Text = Dictionary_Data.SearchDic("DETAIL", bp.g_language); // 상세
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
                lbGetWorkSeq.Text = ds.Tables[0].Rows[0]["WORK_SEQ"].ToString();
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

                //work code를 고치면 안됨(20250107 변석현 차장님 요청)
                txtWorkCd.Enabled = false;

                if (ds.Tables[0].Rows[0]["DEL_YN"].ToString().Equals("Y"))
                {
                    aModify.Visible = false;
                    //aDelete.Visible = false;
                    aRestore.Visible = true;

                    txtWorkNm.Enabled = false;
                    //txtWorkCd.Enabled = false;
                    txtToolType.Enabled = false;
                    txtQty.Enabled = false;
                    txtTorqueType.Enabled = false;
                    txtPset.Enabled = false;
                    txtToolHole.Enabled = false;
                    ddlDevID.Enabled = false;
                    ddlWorkType.Enabled = false;
                    ddlScanCd.Enabled = false;
                    ddlMatchCd.Enabled = false;
                    ddlModeFlag.Enabled = false;
                    ddlUseYN.Enabled = false;
                    txtRemark1.Enabled = false;
                    txtRemark2.Enabled = false;
                }
                else
                {
                    aModify.Visible = true;
                    //aDelete.Visible = true;
                    aRestore.Visible = false;

                    txtWorkNm.Enabled = true;
                    //txtWorkCd.Enabled = true;
                    txtToolType.Enabled = true;
                    txtQty.Enabled = true;
                    txtTorqueType.Enabled = true;
                    txtPset.Enabled = true;
                    txtToolHole.Enabled = true;
                    ddlDevID.Enabled = true;
                    ddlWorkType.Enabled = true;
                    ddlScanCd.Enabled = true;
                    ddlMatchCd.Enabled = true;
                    ddlModeFlag.Enabled = true;
                    ddlUseYN.Enabled = true;
                    txtRemark1.Enabled = true;
                    txtRemark2.Enabled = true;
                }

                ddlShopCd.Enabled = false;
                ddlLineCd.Enabled = false;
                ddlCarType.Enabled = false;
                ddlStnCd.Enabled = false;

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
                strQueryID = "Info19Data.Set_WorkInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                sParam.Add("LINE_CD", strSplitValue[2].ToString());
                sParam.Add("STN_CD", strSplitValue[3].ToString());
                sParam.Add("CAR_TYPE", strSplitValue[4].ToString());
                sParam.Add("WORK_SEQ", strSplitValue[5].ToString());
                
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
                sParam.Add("CUD_TYPE", "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Info19");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA", strUValue);                  // 이전 데이터 셋팅

                // 수정 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall('Info19'); parent.fn_ModalCloseDiv(); ";
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

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
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
                    strDBName = "GPDB";
                    strQueryID = "Info19Data.Set_WorkInfoDel";

                    sParam = new FW.Data.Parameters();
                    sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                    sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                    sParam.Add("LINE_CD", strSplitValue[2].ToString());
                    sParam.Add("STN_CD", strSplitValue[3].ToString());
                    sParam.Add("CAR_TYPE", strSplitValue[4].ToString());
                    sParam.Add("WORK_SEQ", strSplitValue[5].ToString());

                    sParam.Add("FLAG", "Y");
                    sParam.Add("COMP_FLAG", ds.Tables[0].Rows[0]["DEL_YN"].ToString()); // 이미 삭제된 놈 삭제시 완전삭제
                    sParam.Add("USER_ID", bp.g_userid.ToString());

                    sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                    sParam.Add("CUD_TYPE", "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                    sParam.Add("CUR_MENU_ID", "Info19");              // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                    sParam.Add("PREV_DATA", strDValue);                  // 이전 데이터 셋팅

                    // 삭제 비지니스 메서드 작성
                    iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                        strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('Info19'); parent.fn_ModalCloseDiv(); ";
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
                    strScript = " alert('이미 정보가 존재하지 않습니다. '); parent.fn_ModalReloadCall('Info19'); parent.fn_ModalCloseDiv(); ";
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

        #region btnRestore_Click
        protected void btnRestore_Click(object sender, EventArgs e)
        {
            int iRtn = 0;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string strDValue = cy.Decrypt(strDVal);

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                strDBName = "GPDB";
                strQueryID = "Info19Data.Set_WorkInfoDel";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                sParam.Add("LINE_CD", strSplitValue[2].ToString());
                sParam.Add("STN_CD", strSplitValue[3].ToString());
                sParam.Add("CAR_TYPE", strSplitValue[4].ToString());
                sParam.Add("WORK_SEQ", strSplitValue[5].ToString());

                sParam.Add("FLAG", "N");
                sParam.Add("COMP_FLAG", "N");
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "R");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Info19");              // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA", strDValue);                  // 이전 데이터 셋팅

                // 복원 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상복원 되었습니다.');  parent.fn_ModalReloadCall('Info19'); parent.fn_ModalCloseDiv(); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
                else
                {
                    strScript = " alert('복원에 실패하였습니다. 관리자에게 문의바립니다.'); ";
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
