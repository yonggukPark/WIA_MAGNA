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

namespace HQCWeb.MonitorMgt.Mon18
{
    public partial class Mon18_p02 : System.Web.UI.Page
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
        Biz.MonitorManagement.Mon18 biz = new Biz.MonitorManagement.Mon18();

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
            strQueryID = "Mon18Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    ddlBlockFlag.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                }
            }
            ddlBlockFlag.SelectedValue = "Y";
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            // 상세내용 확인후 수정 또는 삭제일 경우
            lbWorkName.Text = Dictionary_Data.SearchDic("DETAIL", bp.g_language); // 상세

            lbModifyCnt.Text = Dictionary_Data.SearchDic("MODIFY_CNT", bp.g_language);
            lbBlockFlag.Text = Dictionary_Data.SearchDic("BLOCK_FLAG", bp.g_language);
            lbReworkMsg.Text = Dictionary_Data.SearchDic("REWORK_MSG", bp.g_language);
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
            string[] chkArr = strPVal.Split('|');
            string[] strSplitValue = null;

            string strDetailValue = string.Empty;

            FW.Data.Parameters sParam = null;

            // 수정대상 개수는 받은 splitValue 개수로 설정
            lbGetModifyCnt.Text = chkArr.Length.ToString();

            strDBName = "GPDB";
            strQueryID = "Mon18Data.Get_LockInfo";

            for (int j = 0; j < chkArr.Length; j++)
            {

                sParam = new FW.Data.Parameters();

                strSplitValue = cy.Decrypt(chkArr[j]).Split('/');

                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("GUBUN_CD", strSplitValue[1].ToString());
                sParam.Add("BARCODE_NO", strSplitValue[2].ToString());

                sParam.Add("CUR_MENU_ID", "Mon18");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
                        // 변경전 데이터 셋팅
                        for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                        {
                            string strColumns = string.Empty;

                            strColumns = ds.Tables[0].Columns[i].ToString();

                            if (strDetailValue == "")
                            {
                                strDetailValue = strColumns + ":" + ds.Tables[0].Rows[0][strColumns].ToString();
                            }
                            else if (strDetailValue.Substring(strDetailValue.Length - 1) == "|")
                            {
                                strDetailValue += strColumns + ":" + ds.Tables[0].Rows[0][strColumns].ToString();
                            }
                            else
                            {
                                strDetailValue += "," + strColumns + ":" + ds.Tables[0].Rows[0][strColumns].ToString();
                            }
                        }
                        strDetailValue += "|";

                    }
                }
                else
                {
                    strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Mon18'); parent.fn_ModalCloseDiv(); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
            }

            strDetailValue = strDetailValue.Substring(0, strDetailValue.Length - 1);

            (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = cy.Encrypt(strDetailValue);
        }
        #endregion

        #region btnModify_Click
        protected void btnModify_Click(object sender, EventArgs e)
        {
            int iRtn = 0;
            int uCnt = 0;
            int fCnt = 0;
            string strScript = string.Empty;

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;
                string[] chkArr = strPVal.Split('|');
                string strUVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;
                string[] chkArr2 = cy.Decrypt(strUVal).Split('|');
                string[] strSplitValue = null;
                string[] strUValue = null;

                FW.Data.Parameters sParam = null;

                strDBName = "GPDB";
                strQueryID = "Mon18Data.Set_LockInfo";
                for (int i = 0; i < chkArr.Length; i++)
                {
                    strSplitValue = cy.Decrypt(chkArr[i]).Split('/');
                    strUValue = chkArr2[i].Split('/');

                    sParam = new FW.Data.Parameters();
                    sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                    sParam.Add("GUBUN_CD", strSplitValue[1].ToString());
                    sParam.Add("BARCODE_NO", strSplitValue[2].ToString());

                    sParam.Add("BLOCK_FLAG", ddlBlockFlag.SelectedValue);
                    sParam.Add("REWORK_MSG", txtReworkMsg.Text);

                    sParam.Add("USER_ID", bp.g_userid.ToString());

                    sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                    sParam.Add("CUD_TYPE", "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                    sParam.Add("CUR_MENU_ID", "Mon18");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                    sParam.Add("PREV_DATA", strUValue[0].ToString());    // 이전 데이터 셋팅

                    // 수정 비지니스 메서드 작성
                    iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        uCnt++;
                    }
                    else
                    {
                        fCnt++;
                    }
                }

                // HiddenField 삭제
                (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                //분기처리
                if (fCnt == 0)
                {
                    strScript = " alert(' " + uCnt + "건 정상수정 되었습니다.');  parent.fn_ModalReloadCall('Mon18'); parent.fn_ModalCloseDiv(); ";
                }
                else if (uCnt > 0)
                {
                    strScript = " alert(' " + chkArr.Length + "건 중 " + uCnt + "건 수정이 완료되었습니다. 조회 데이터를 확인해주세요.');  parent.fn_ModalReloadCall('Mon18'); parent.fn_ModalCloseDiv(); ";
                }
                else
                {
                    strScript = " alert('수정에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                }

                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
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
