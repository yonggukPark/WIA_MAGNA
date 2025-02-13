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

namespace HQCWeb.QualityMgt.Qua24
{
    public partial class Qua24_p02 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        // 비지니스 클래스 작성
        Biz.QualityManagement.Qua24 biz = new Biz.QualityManagement.Qua24();

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
            // 수정 또는 삭제
            lbWorkName.Text = Dictionary_Data.SearchDic("DETAIL", bp.g_language); // 상세

            lbPartSerialNoBef.Text = Dictionary_Data.SearchDic("BEFORE_MOD", bp.g_language);
            lbPartSerialNoAft.Text = Dictionary_Data.SearchDic("AFTER_MOD", bp.g_language);
            lbDiffMsg.Text = Dictionary_Data.SearchDic("DIFF_MSG", bp.g_language);
        }
        #endregion

        #region GetData
        protected void GetData()
        {
            string strScript = string.Empty;

            DataSet ds = new DataSet();

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');
            
            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Qua24Data.Get_DiffInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("STN_CD", strSplitValue[3].ToString());
            sParam.Add("SERIAL_NO", strSplitValue[4].ToString());
            sParam.Add("PART_SERIAL_NO", strSplitValue[5].ToString());
            sParam.Add("RPT_DATE", strSplitValue[6].ToString());

            sParam.Add("CUR_MENU_ID", "Qua24");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                lbGetPartSerialNoBef.Text = ds.Tables[0].Rows[0]["PART_SERIAL_NO"].ToString();

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
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Qua24'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnModify_Click
        protected void btnModify_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();//저장시 다중플래그가 나와서 구분해줘야 함
            int iRtn = 0;
            string strRtn = string.Empty;
            string strRtn2 = string.Empty;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strUVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strUValue = cy.Decrypt(strUVal).Split('/');

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                strDBName = "GPDB";
                strQueryID = "Qua24Data.Get_DiffID_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("PLANT_CD", strSplitValue[0].ToString());
                sParamIDChk.Add("SHOP_CD", strSplitValue[1].ToString());
                sParamIDChk.Add("LINE_CD", strSplitValue[2].ToString());
                sParamIDChk.Add("STN_CD", strSplitValue[3].ToString());
                sParamIDChk.Add("CAR_TYPE", strSplitValue[7].ToString());
                sParamIDChk.Add("SCAN_CD", strSplitValue[8].ToString());
                sParamIDChk.Add("PART_SERIAL_NO", strSplitValue[5].ToString());
                sParamIDChk.Add("PART_SERIAL_NO_AFT", txtPartSerialNoAft.Text);
                
                // 아이디 체크 비지니스 메서드 작성
                ds = biz.GetDataSet(strDBName, strQueryID, sParamIDChk);

                if (ds.Tables.Count > 0)
                {
                    strRtn = ds.Tables[0].Rows[0]["VAL_CHK_1"].ToString();
                    strRtn2 = ds.Tables[0].Rows[0]["VAL_CHK_2"].ToString();

                    if (strRtn == "0" && strRtn2 == "0")
                    {
                        strDBName = "GPDB";
                        strQueryID = "Qua24Data.Set_DiffInfo";

                        FW.Data.Parameters sParam = new FW.Data.Parameters();
                        sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                        sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                        sParam.Add("LINE_CD", strSplitValue[2].ToString());
                        sParam.Add("STN_CD", strSplitValue[3].ToString());
                        sParam.Add("SERIAL_NO", strSplitValue[4].ToString());
                        sParam.Add("PART_SERIAL_NO", strSplitValue[5].ToString());
                        sParam.Add("RPT_DATE", strSplitValue[6].ToString());
                        sParam.Add("PART_SERIAL_NO_AFT", txtPartSerialNoAft.Text);
                        sParam.Add("DIFF_MSG", txtDiffMsg.Text);
                        sParam.Add("USER_ID", bp.g_userid.ToString());

                        sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                        sParam.Add("CUD_TYPE", "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                        sParam.Add("CUR_MENU_ID", "Qua24");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                        sParam.Add("PREV_DATA", strUValue[0].ToString());   // 이전 데이터 셋팅

                        // 수정 비지니스 메서드 작성
                        iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                        if (iRtn == 1)
                        {
                            (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                            strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall('Qua24'); parent.fn_ModalCloseDiv(); ";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        }
                        else
                        {
                            strScript = " alert('수정에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                        }
                    }
                    else if(strRtn != "0")
                    {
                        strScript = " alert('잘못된 형식의 바코드입니다.'); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else if(strRtn2 != "0")
                    {
                        strScript = " alert('제품 바코드 번호가 중복입니다.'); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
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

        #region btnDelete_Click
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int iRtn = 0;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strDValue = cy.Decrypt(strDVal).Split('/');

            strDBName = "GPDB";
            strQueryID = "Qua24Data.Del_DiffInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("STN_CD", strSplitValue[3].ToString());
            sParam.Add("SERIAL_NO", strSplitValue[4].ToString());
            sParam.Add("PART_SERIAL_NO", strSplitValue[5].ToString());
            sParam.Add("RPT_DATE", strSplitValue[6].ToString());
            sParam.Add("DIFF_MSG", txtDiffMsg.Text);
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            sParam.Add("CUD_TYPE", "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            sParam.Add("CUR_MENU_ID", "Qua24");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            sParam.Add("PREV_DATA", strDValue[0].ToString());   // 이전 데이터 셋팅

            // 삭제 비지니스 메서드 작성
            iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

            if (iRtn == 1)
            {
                (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('Qua24'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                strScript = " alert('삭제에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion
    }
}
