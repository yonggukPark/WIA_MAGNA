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

namespace HQCWeb.InfoMgt.Qua38
{
    public partial class Qua38_p02 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        protected string strVal = string.Empty;

        // 비지니스 클래스 작성
        Biz.QualityManagement.Qua38 biz = new Biz.QualityManagement.Qua38();

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

            strDBName = "GPDB";
            strQueryID = "Qua38Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlPartNo.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    ddlDefectCompany.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    ddlDecomposeType.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    ddlStorageCd.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                }
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            // 상세내용 확인후 수정 또는 삭제일 경우
            lbWorkName.Text = Dictionary_Data.SearchDic("DETAIL", bp.g_language); // 상세

            lbDefectDt.Text = Dictionary_Data.SearchDic("DEFECT_DT", bp.g_language);
            lbPartNo.Text = Dictionary_Data.SearchDic("PART_NO", bp.g_language);
            lbLotNo.Text = Dictionary_Data.SearchDic("LOT_NO", bp.g_language);
            lbDefectCompany.Text = Dictionary_Data.SearchDic("DEFECT_COMPANY", bp.g_language);
            lbDefectCnt.Text = Dictionary_Data.SearchDic("DEFECT_CNT", bp.g_language);
            lbDefectReason.Text = Dictionary_Data.SearchDic("DEFECT_REASON", bp.g_language);
            lbDecomposeType.Text = Dictionary_Data.SearchDic("DECOMPOSE_TYPE", bp.g_language);
            lbStorageCd.Text = Dictionary_Data.SearchDic("STORAGE_CD", bp.g_language);
            lbDeleteDesc.Text = Dictionary_Data.SearchDic("DELETE_DESC", bp.g_language);
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
            strQueryID = "Qua38Data.Get_DecomposeInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("DEFECT_DT", strSplitValue[1].ToString());
            sParam.Add("LOG_SEQ", strSplitValue[2].ToString());
            sParam.Add("PART_NO", strSplitValue[3].ToString());

            sParam.Add("CUR_MENU_ID", "Qua38");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                lbGetDefectDt.Text = ds.Tables[0].Rows[0]["DEFECT_DT"].ToString();
                lbGetLotNo.Text = ds.Tables[0].Rows[0]["LOT_NO"].ToString();
                ddlPartNo.SelectedValue = ds.Tables[0].Rows[0]["PART_NO"].ToString();
                ddlDefectCompany.SelectedValue = ds.Tables[0].Rows[0]["DEFECT_COMPANY"].ToString();
                ddlDecomposeType.SelectedValue = ds.Tables[0].Rows[0]["DECOMPOSE_CD"].ToString();
                ddlStorageCd.SelectedValue = ds.Tables[0].Rows[0]["STORAGE_CD"].ToString();
                txtDeleteDesc.Text = ds.Tables[0].Rows[0]["DELETE_DESC"].ToString();
                txtDefectCnt.Text = ds.Tables[0].Rows[0]["DEFECT_CNT"].ToString();
                txtDefectReason.Text = ds.Tables[0].Rows[0]["DEFECT_REASON"].ToString();
                
                ddlPartNo.Enabled = false;
                ddlDefectCompany.Enabled = false;
                ddlDecomposeType.Enabled = false;
                ddlStorageCd.Enabled = false;

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
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Qua38'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnModify_Click
        protected void btnModify_Click(object sender, EventArgs e)
        {
            string strRtn = string.Empty;
            int iRtn = 0;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strUVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string strUValue = cy.Decrypt(strUVal);

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                // 비지니스 클래스 작성
                //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();
                
                strDBName = "GPDB";
                strQueryID = "Qua38Data.Get_DecomposeID_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("PLANT_CD", strSplitValue[0].ToString());
                sParamIDChk.Add("DEFECT_DT", strSplitValue[1].ToString());
                sParamIDChk.Add("LOG_SEQ", strSplitValue[2].ToString());
                sParamIDChk.Add("PART_NO", strSplitValue[3].ToString());

                // 아이디 체크 비지니스 메서드 작성
                strRtn = biz.GetIDValChk(strDBName, strQueryID, sParamIDChk);
                if (strRtn == "0")
                {
                    strDBName = "GPDB";
                    strQueryID = "Qua38Data.Set_DecomposeInfo";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                    sParam.Add("DEFECT_DT", strSplitValue[1].ToString());
                    sParam.Add("LOG_SEQ", strSplitValue[2].ToString());
                    sParam.Add("PART_NO", strSplitValue[3].ToString());

                    sParam.Add("LOT_NO", lbGetLotNo.Text);
                    sParam.Add("DEFECT_COMPANY", ddlDefectCompany.SelectedValue);
                    sParam.Add("DEFECT_CNT", txtDefectCnt.Text);
                    sParam.Add("DEFECT_REASON", txtDefectReason.Text);
                    sParam.Add("STORAGE_CD", ddlStorageCd.SelectedValue);
                    sParam.Add("DECOMPOSE_CD", ddlDecomposeType.SelectedValue);

                    sParam.Add("REMARK1", "");
                    sParam.Add("REMARK2", "");
                    sParam.Add("USE_YN", "Y");
                    sParam.Add("USER_ID", bp.g_userid.ToString());

                    sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                    sParam.Add("CUD_TYPE", "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                    sParam.Add("CUR_MENU_ID", "Qua38");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                    sParam.Add("PREV_DATA", strUValue);                  // 이전 데이터 셋팅

                    // 수정 비지니스 메서드 작성
                    iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                        strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall('Qua38'); parent.fn_ModalCloseDiv(); ";
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
                    strScript = " alert('ERP 승인 완료된 데이터는 수정 불가능합니다.'); ";
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
            string strRtn = string.Empty;
            int iRtn = 0;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string strDValue = cy.Decrypt(strDVal);

            // 비지니스 클래스 작성
            //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();

            strDBName = "GPDB";
            strQueryID = "Qua38Data.Get_DecomposeID_ValChk";

            FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
            sParamIDChk.Add("PLANT_CD", strSplitValue[0].ToString());
            sParamIDChk.Add("DEFECT_DT", strSplitValue[1].ToString());
            sParamIDChk.Add("LOG_SEQ", strSplitValue[2].ToString());
            sParamIDChk.Add("PART_NO", strSplitValue[3].ToString());

            // 아이디 체크 비지니스 메서드 작성
            strRtn = biz.GetIDValChk(strDBName, strQueryID, sParamIDChk);
            if (strRtn == "0")
            {

                strDBName = "GPDB";
                strQueryID = "Qua38Data.Set_DecomposeInfoDel";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("DEFECT_DT", strSplitValue[1].ToString());
                sParam.Add("LOG_SEQ", strSplitValue[2].ToString());
                sParam.Add("PART_NO", strSplitValue[3].ToString());

                sParam.Add("FLAG", "Y");
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Qua38");              // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA", strDValue);                  // 이전 데이터 셋팅

                // 삭제 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('Qua38'); parent.fn_ModalCloseDiv(); ";
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
                strScript = " alert('ERP 승인 완료된 데이터는 삭제 불가능합니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion
    }
}
