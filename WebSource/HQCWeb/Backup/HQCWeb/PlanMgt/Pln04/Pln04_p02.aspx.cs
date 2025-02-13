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

namespace HQCWeb.PlanMgt.Pln04
{
    public partial class Pln04_p02 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        protected string strVal = string.Empty;

        Biz.PlanManagement.Pln04 biz = new Biz.PlanManagement.Pln04();

        // 암복호화 키값 셋팅
        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        //JSON 전달용 변수
        string jsDdl1 = string.Empty;

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

                // 클라이언트 사이드 변수에 JSON 데이터 할당
                string script = $@" cPrint = {jsDdl1}; ";

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            //데이터셋 설정
            string script = string.Empty;
            jsDdl1 = "[]";
            DataSet ds = new DataSet();

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Pln04Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", "P1");
            param.Add("SHOP_CD", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                jsDdl1 = ConvertJSONData.ConvertDataTableToJSON2(ds.Tables[7]);

                for (int i = 0; i < ds.Tables[8].Rows.Count; i++)
                {
                    ddlShipOutType.Items.Add(new ListItem(ds.Tables[8].Rows[i]["CODE_NM"].ToString(), ds.Tables[8].Rows[i]["CODE_ID"].ToString()));
                }
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbWorkName.Text = Dictionary_Data.SearchDic("PRINT", bp.g_language); // 출력

            lbShipNo.Text = Dictionary_Data.SearchDic("SHIP_NO", bp.g_language);
            lbPrintReason.Text = Dictionary_Data.SearchDic("PRINT_REASON", bp.g_language);
            lbShipOutType.Text = Dictionary_Data.SearchDic("SHIP_OUT_TYPE", bp.g_language);
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
            strQueryID = "Pln04Data.Get_PrintInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("ORDER_NO", strSplitValue[1].ToString());
            sParam.Add("PART_NO", strSplitValue[2].ToString());

            sParam.Add("CUR_MENU_ID", "Pln04");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                lbGetShipNo.Text = ds.Tables[0].Rows[0]["SHIP_NO"].ToString();

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
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Pln04'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnModify_Click
        protected void btnModify_Click(object sender, EventArgs e)
        {
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');
            string strPlantCd = strSplitValue[0].ToString();

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strDValue = cy.Decrypt(strDVal).Split('/');
            string strShipNo = strDValue[0].Split(':')[1];

            string param = cy.Encrypt(strPlantCd + "/" + strShipNo + "/" + bp.g_userid.ToString() + "/" + ddlShipOutType.SelectedValue.ToString() + "/" + txtPrintReasonHidden.Text.ToString());

            strScript = "fn_PopupPostOpenPop('" + param + "', 'Pln04_p03.aspx', 1024, 800);";
            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), strScript, true);

        }
        #endregion

        #region btnDelete_Click
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string strRtn = string.Empty;
            int iRtn = 0;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strDValue = cy.Decrypt(strDVal).Split('/');
            string strShipNo = strDValue[0].Split(':')[1];

            strDBName = "GPDB";
            strQueryID = "Pln04Data.Get_ShipNo_Cancel_Chk";
            FW.Data.Parameters sParamCheck = new FW.Data.Parameters();
            sParamCheck.Add("PLANT_CD", strValue[0].ToString());
            sParamCheck.Add("SHIP_NO", strShipNo);

            // 출하번호 삭제 가능성 체크
            strRtn = biz.GetShipValChk(strDBName, strQueryID, sParamCheck);

            if (strRtn.Equals("0"))
            {
                strDBName = "GPDB";
                strQueryID = "Pln04Data.Set_CancelShip";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strValue[0].ToString());
                sParam.Add("SHIP_NO", strShipNo);

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Pln04");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA", strDValue[0].ToString());   // 이전 데이터 셋팅

                // 삭제 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('Pln04'); parent.fn_ModalCloseDiv(); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
                else
                {
                    strScript = " alert('삭제에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
            }
            else if(strRtn.Equals("1"))
            {
                strScript = " alert('운송중인 출하번호입니다. 삭제가 불가능합니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);

            }
            else if (strRtn.Equals("2"))
            {
                strScript = " alert('운송완료된 출하번호입니다. 삭제가 불가능합니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }

        }
        #endregion
    }
}
