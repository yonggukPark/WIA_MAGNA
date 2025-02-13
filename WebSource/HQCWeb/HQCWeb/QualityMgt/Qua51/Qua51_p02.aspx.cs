using HQCWeb.FMB_FW;
using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using MES.FW.Common.Crypt;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HQCWeb.QualityMgt.Qua51
{
    public partial class Qua51_p02 : System.Web.UI.Page
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
        Biz.QualityManagement.Qua51 biz = new Biz.QualityManagement.Qua51();

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
            ddlManDept.Items.Add(new ListItem("선택하세요", ""));
            ddlInspCycle.Items.Add(new ListItem("선택하세요", ""));

            strDBName = "GPDB";
            strQueryID = "Qua51Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    ddlManDept.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    ddlInspCycle.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                }
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            // 상세내용 확인후 수정 또는 삭제일 경우
            lbWorkName.Text = Dictionary_Data.SearchDic("DETAIL", bp.g_language); // 상세

            lbManNo.Text = Dictionary_Data.SearchDic("MAN_NO", bp.g_language);
            lbManDept.Text = Dictionary_Data.SearchDic("MAN_DEPT", bp.g_language);
            lbInspCycle.Text = Dictionary_Data.SearchDic("INSP_CYCLE", bp.g_language);
            lbPartDesc.Text = Dictionary_Data.SearchDic("PART_NAME", bp.g_language);
            lbPartSerialNo.Text = Dictionary_Data.SearchDic("PART_SERIAL_NO", bp.g_language);
            lbStandard.Text = Dictionary_Data.SearchDic("STANDARD", bp.g_language);
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
            strQueryID = "Qua51Data.Get_CurrectInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("MAN_NO", strSplitValue[1].ToString());

            sParam.Add("CUR_MENU_ID", "Qua51");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
                    lbGetManNo.Text = ds.Tables[0].Rows[0]["MAN_NO"].ToString();
                    txtPartDesc.Text = ds.Tables[0].Rows[0]["PART_NAME"].ToString();
                    txtPartSerialNo.Text = ds.Tables[0].Rows[0]["PART_SERIAL_NO"].ToString();
                    ddlInspCycle.Text = ds.Tables[0].Rows[0]["INSP_CYCLE"].ToString();
                    ddlManDept.Text = ds.Tables[0].Rows[0]["MAN_DEPT"].ToString();
                    txtStandard.Text = ds.Tables[0].Rows[0]["STANDARD"].ToString();

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
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Qua51'); parent.fn_ModalCloseDiv(); ";
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
                // 비지니스 클래스 작성
                //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();

                strDBName = "GPDB";
                strQueryID = "Qua51Data.Upt_CurrectInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("MAN_NO", strSplitValue[1].ToString());
                sParam.Add("MAN_DEPT", ddlManDept.SelectedValue);
                sParam.Add("PART_NAME", txtPartDesc.Text);
                sParam.Add("STANDARD", txtStandard.Text);
                sParam.Add("PART_SERIAL_NO", txtPartSerialNo.Text);
                sParam.Add("INSP_CYCLE", ddlInspCycle.SelectedValue);
                sParam.Add("USER_ID", bp.g_userid.ToString());

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Qua51");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA", strUValue);    // 이전 데이터 셋팅

                // 수정 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall('Qua51'); parent.fn_ModalCloseDiv(); ";
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
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strErrMessage = string.Empty;

            int iRtn = 0;
            string strRtn = string.Empty;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string strDValue = cy.Decrypt(strDVal);

            FW.Data.Parameters sParam = null;

            //strDBName = "GPDB";
            //strQueryID = "Qua51Data.Get_FileInfo";

            //sParam = new FW.Data.Parameters();
            //sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            //sParam.Add("MAN_NO", strSplitValue[1].ToString());

            //sParam.Add("CUR_MENU_ID", "Qua51");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            //// 파일리스트 저장
            //ds = biz.GetDataSet(strDBName, strQueryID, sParam);
            
            strDBName = "GPDB";
            strQueryID = "Qua51Data.Get_CurrectDetailInfo";

            sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("MAN_NO", strSplitValue[1].ToString());
            sParam.Add("CERT_NO", "");

            sParam.Add("CUR_MENU_ID", "Qua51");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            
            // 데이터 조회
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
                    // 개수 추출
                    strRtn = ds.Tables[1].Rows[0]["CNT"].ToString();
                }
            }

            if (strRtn != "0")
            {
                strScript = " alert('성적서 데이터가 존재합니다. 먼저 성적서 데이터를 삭제하세요.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                strDBName = "GPDB";
                strQueryID = "Qua51Data.Del_CurrectInfo";

                sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("MAN_NO", strSplitValue[1].ToString());

                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE", "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID", "Qua51");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                sParam.Add("PREV_DATA", strDValue);   // 이전 데이터 셋팅

                // 삭제 비지니스 메서드 작성
                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    //첨부파일이 있는 경우, 삭제
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        string File_Path = System.Configuration.ConfigurationManager.AppSettings.Get("FILE_PATH") + "Qua51\\";
                        string FolderNM = string.Empty;
                        string FilePath = string.Empty;

                        //파일 리스트 순회하며 삭제
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            FolderNM = cy.Decrypt(ds.Tables[0].Rows[0]["FILE_PATH"].ToString());
                            FilePath = File_Path + FolderNM;

                            if (Directory.Exists(FilePath))
                                Directory.Delete(FilePath, true);
                        }
                    }

                    (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                    strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('Qua51'); parent.fn_ModalCloseDiv(); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
                else
                {
                    strScript = " alert('삭제에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
            }
            
        }
        #endregion
    }
}
