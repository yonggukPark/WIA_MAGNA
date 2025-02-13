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

namespace HQCWeb.QualityMgt.Qua38
{
    public partial class Qua38_p01 : System.Web.UI.Page
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
        Biz.QualityManagement.Qua38 biz = new Biz.QualityManagement.Qua38();

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
            ddlDefectCompany.Items.Add(new ListItem("선택하세요", ""));
            ddlDecomposeType.Items.Add(new ListItem("선택하세요", ""));
            ddlStorageCd.Items.Add(new ListItem("선택하세요", ""));

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

            txtDate.Text = System.DateTime.Now.ToString("yyyy-MM-dd");
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbDefectDt.Text = Dictionary_Data.SearchDic("DEFECT_DT", bp.g_language);
            lbPartNo.Text = Dictionary_Data.SearchDic("PART_NO", bp.g_language);
            lbLotNo.Text = Dictionary_Data.SearchDic("LOT_NO", bp.g_language);
            lbDefectCompany.Text = Dictionary_Data.SearchDic("DEFECT_COMPANY", bp.g_language);
            lbDefectCnt.Text = Dictionary_Data.SearchDic("DEFECT_CNT", bp.g_language);
            lbDefectReason.Text = Dictionary_Data.SearchDic("DEFECT_REASON", bp.g_language);
            lbDecomposeType.Text = Dictionary_Data.SearchDic("DECOMPOSE_TYPE", bp.g_language);
            lbStorageCd.Text = Dictionary_Data.SearchDic("STORAGE_CD", bp.g_language);

            // 등록일경우
            lbWorkName.Text = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language); // 등록
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
            int iRtn = 0;
            string strScript = string.Empty;

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                // 비지니스 클래스 작성
                //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();
                
                strDBName = "GPDB";
                strQueryID = "Qua38Data.Set_DecomposeInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", bp.g_plant.ToString());
                sParam.Add("DEFECT_DT", txtDate.Text.Replace("-", ""));
                sParam.Add("LOG_SEQ", "");
                sParam.Add("PART_NO", ddlPartNo.SelectedValue);

                sParam.Add("LOT_NO", txtLotNo.Text);
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
                sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Qua38");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                // 등록 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('Qua38'); parent.fn_ModalCloseDiv(); ";
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
