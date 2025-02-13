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

namespace HQCWeb.InfoMgt.Info01
{
    public partial class Info01_p03 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        protected string strVal = string.Empty;

        // 비지니스 클래스 작성
        Biz.InfoManagement.Info01 biz = new Biz.InfoManagement.Info01();

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

            ddlPlantCd.Items.Add(new ListItem("선택하세요", ""));
            ddlUseYN.Items.Clear();

            strDBName = "GPDB";
            strQueryID = "Info01Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlPlantCd.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    ddlUseYN.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                }
                ddlUseYN.SelectedIndex = 0;
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbWorkName.Text = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language);

            lbPlantCd.Text = Dictionary_Data.SearchDic("PLANT_CD", bp.g_language);
            lbCode.Text = Dictionary_Data.SearchDic("CODE", bp.g_language);
            lbCodeNm.Text = Dictionary_Data.SearchDic("CODE_NM", bp.g_language);
            lbStartTime.Text = Dictionary_Data.SearchDic("START_TIME", bp.g_language);
            lbEndTime.Text = Dictionary_Data.SearchDic("END_TIME", bp.g_language);
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
            //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();
            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Info01Data.Get_TimeInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("CODE", strSplitValue[1].ToString());

            sParam.Add("CUR_MENU_ID", "Info01");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                ddlPlantCd.SelectedValue = ds.Tables[0].Rows[0]["PLANT_CD"].ToString();
                txtCode.Text = ds.Tables[0].Rows[0]["CODE"].ToString();
                txtCodeNm.Text = ds.Tables[0].Rows[0]["CODE_NM"].ToString();
                txtStartTime.Text = ds.Tables[0].Rows[0]["START_TIME"].ToString();
                txtEndTime.Text = ds.Tables[0].Rows[0]["END_TIME"].ToString();
                txtRemark1.Text = ds.Tables[0].Rows[0]["REMARK1"].ToString();
                txtRemark2.Text = ds.Tables[0].Rows[0]["REMARK2"].ToString();
                ddlUseYN.SelectedValue = ds.Tables[0].Rows[0]["USE_YN"].ToString();

                if (ds.Tables[0].Rows[0]["DEL_YN"].ToString().Equals("Y"))
                {
                    txtCodeNm.Enabled = false;
                    txtStartTime.Enabled = false;
                    txtEndTime.Enabled = false;
                    txtRemark1.Enabled = false;
                    txtRemark2.Enabled = false;
                    ddlUseYN.Enabled = false;
                }
                else
                {
                    txtCodeNm.Enabled = true;
                    txtStartTime.Enabled = true;
                    txtEndTime.Enabled = true;
                    txtRemark1.Enabled = true;
                    txtRemark2.Enabled = true;
                    ddlUseYN.Enabled = true;
                }

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
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Info01'); parent.fn_ModalCloseDiv(); ";
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
                strQueryID = "Info01Data.Get_TimeID_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("PLANT_CD", ddlPlantCd.SelectedValue);
                sParamIDChk.Add("CODE", txtCode.Text);

                // 아이디 체크 비지니스 메서드 작성
                strRtn = biz.GetTimeValChk(strDBName, strQueryID, sParamIDChk);
                if (strRtn == "0")
                {
                    strDBName = "GPDB";
                    strQueryID = "Info01Data.Set_TimeInfo";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("PLANT_CD", ddlPlantCd.SelectedValue);
                    sParam.Add("CODE", txtCode.Text);
                    sParam.Add("CODE_NM", txtCodeNm.Text);
                    sParam.Add("START_TIME", txtStartTime.Text);
                    sParam.Add("END_TIME", txtEndTime.Text);
                    sParam.Add("REMARK1", txtRemark1.Text);
                    sParam.Add("REMARK2", txtRemark2.Text);
                    sParam.Add("USE_YN", ddlUseYN.SelectedValue);
                    sParam.Add("USER_ID", bp.g_userid.ToString());

                    sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                    sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                    sParam.Add("CUR_MENU_ID", "Info01");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                    // 등록 비지니스 메서드 작성
                    iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('Info01'); parent.fn_ModalCloseDiv(); ";
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
    }
}
