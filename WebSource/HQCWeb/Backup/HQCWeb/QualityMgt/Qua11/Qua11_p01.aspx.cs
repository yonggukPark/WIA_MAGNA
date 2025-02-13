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

using System.IO;
using System.Data.OleDb;

namespace HQCWeb.QualityMgt.Qua11
{
    public partial class Qua11_p01 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        //포맷 그리드 생성용
        string jsField = string.Empty;
        string jsCol = string.Empty;

        Biz.QualityManagement.Qua11 biz = new Biz.QualityManagement.Qua11();
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

                    // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid' 함수 호출
                    string script = $@" column = {jsCol}; 
                                field = {jsField}; 
                                createGrid(); ";

                    //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script, true);
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
            lbStnCd.Text = Dictionary_Data.SearchDic("STN_CD", bp.g_language);
            lbDevCd.Text = Dictionary_Data.SearchDic("DEV_ID", bp.g_language);
            lbCarType.Text = Dictionary_Data.SearchDic("CAR_TYPE", bp.g_language);
            lbFilePath.Text = Dictionary_Data.SearchDic("FILE_PATH", bp.g_language);

            // 업로드
            lbWorkName.Text = Dictionary_Data.SearchDic("UPLOAD", bp.g_language);
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
            strQueryID = "Qua11Data.Get_InsInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("STN_CD", strSplitValue[3].ToString());
            sParam.Add("DEV_ID", strSplitValue[4].ToString());
            sParam.Add("CAR_TYPE", strSplitValue[5].ToString());
            sParam.Add("DIV_FLAG", strSplitValue[6].ToString());

            sParam.Add("CUR_MENU_ID", "Qua11");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                GetDDL(ds.Tables[1].Rows[0]["SHOP_CD"].ToString(), ds.Tables[1].Rows[0]["LINE_CD"].ToString(), ds.Tables[1].Rows[0]["STN_CD"].ToString());
                ddlDevCd.SelectedValue = ds.Tables[1].Rows[0]["DEV_ID"].ToString();
                ddlCarType.SelectedValue = ds.Tables[1].Rows[0]["CAR_TYPE"].ToString();

                ddlShopCd.Enabled = false;
                ddlLineCd.Enabled = false;
                ddlStnCd.Enabled = false;
                ddlDevCd.Enabled = false;
                ddlCarType.Enabled = false;

                //컬럼 추출(동적 쿼리문에 대응 : 컬럼명이 정해지지 않아 검색시 동적으로 그리드 생성하여 처리)
                string[] columnNames = ds.Tables[3].Columns.Cast<DataColumn>()
                         .Select(x => x.ColumnName)
                         .ToArray();

                //길이 설정
                string[] arrColumnWidth = new string[columnNames.Length];

                for (int i = 0; i < columnNames.Length; i++)
                {
                    arrColumnWidth[i] = "200";
                }

                //컬럼설정, 필드설정
                jsCol = ConvertJSONData.ConvertColArrToJSON(columnNames, columnNames, arrColumnWidth, "cols");
                jsField = ConvertJSONData.ConvertColArrToJSON(columnNames, columnNames, arrColumnWidth, "fields");

                // 변경전 데이터 셋팅
                for (int i = 0; i < ds.Tables[1].Columns.Count; i++)
                {
                    string strColumns = string.Empty;

                    strColumns = ds.Tables[1].Columns[i].ToString();

                    if (strDetailValue == "")
                    {
                        strDetailValue = strColumns + ":" + ds.Tables[1].Rows[0][strColumns].ToString();
                    }
                    else
                    {
                        strDetailValue += "," + strColumns + ":" + ds.Tables[1].Rows[0][strColumns].ToString();
                    }
                }

                (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = cy.Encrypt(strDetailValue);
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Qua11'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            string strRtn = string.Empty;
            int iRtn = 0, errRow = -1;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                string FileExt = Path.GetExtension(fudFilePath.PostedFile.FileName);
                //xlsx 확인
                if (FileExt.ToLower().EndsWith(".xlsx"))
                {
                    //파일 실재 확인
                    if (fudFilePath.HasFile)
                    {
                        //string uploadpath = Server.MapPath("~/Uploads/ExcelUploadInsp/" + Server.UrlDecode(Session["UserId"].ToString()));
                        string uploadpath = Server.MapPath("~/Uploads/ExcelUploadInsp/");
                        string filename = Path.GetFileName(fudFilePath.PostedFile.FileName);

                        //저장경로 없으면 생성
                        if (!Directory.Exists(uploadpath))
                        {
                            Directory.CreateDirectory(uploadpath);
                        }

                        uploadpath = uploadpath + "\\" + filename;

                        if (File.Exists(uploadpath))
                        {
                            File.Delete(uploadpath);
                        }
                        fudFilePath.SaveAs(uploadpath);
                        OleDbConnection con = null;
                        if (FileExt == ".xls")
                        {
                            con = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.8.0;Data Source=" + uploadpath + @";Extended Properties=""Excel 8.0;IMEX=1;""");
                        }
                        else if (FileExt == ".xlsx")
                        {
                            con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + uploadpath + @";Extended Properties=""Excel 12.0;IMEX=1;""");
                        }
                        //엑셀 추출 시작
                        con.Open();
                        DataTable dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string getExcelSheetName = dt.Rows[0]["Table_Name"].ToString();
                        OleDbCommand ExcelCommand = new OleDbCommand(@"SELECT * FROM [" + getExcelSheetName + @"]", con);
                        OleDbDataAdapter ExcelAdapter = new OleDbDataAdapter(ExcelCommand);
                        DataSet ExcelDataSet = new DataSet();
                        ExcelAdapter.Fill(ExcelDataSet);
                        con.Close();
                        //엑셀 추출 끝

                        strDBName = "GPDB";
                        strQueryID = "Qua11Data.Get_InsInfo";

                        FW.Data.Parameters sParamExcel = new FW.Data.Parameters();
                        sParamExcel.Add("PLANT_CD", strSplitValue[0].ToString());
                        sParamExcel.Add("SHOP_CD", strSplitValue[1].ToString());
                        sParamExcel.Add("LINE_CD", strSplitValue[2].ToString());
                        sParamExcel.Add("STN_CD", strSplitValue[3].ToString());
                        sParamExcel.Add("DEV_ID", strSplitValue[4].ToString());
                        sParamExcel.Add("CAR_TYPE", strSplitValue[5].ToString());
                        sParamExcel.Add("DIV_FLAG", strSplitValue[6].ToString());

                        sParamExcel.Add("CUR_MENU_ID", "Qua11");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                        // 상세조회 비지니스 메서드 호출
                        ds = biz.GetDataSet(strDBName, strQueryID, sParamExcel);

                        if (ds.Tables.Count > 0)
                        {
                            //엑셀컬럼 리스트 조회
                            string[] columnNames = ds.Tables[2].Columns.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray();

                            if (columnNames.Length == ExcelDataSet.Tables[0].Columns.Count)
                            {
                                int intItem = 0;
                                for (int k = 0; k < ExcelDataSet.Tables[0].Columns.Count; k++)
                                {
                                    ExcelDataSet.Tables[0].Columns[k].ColumnName = columnNames[k];//엑셀 데이터셋의 컬럼명을 DB의 컬럼명으로 변경
                                    if (columnNames[k].Split('_')[0].Equals("ITEM") && intItem == 0) intItem = k;//ITEM 시작점 확인
                                }

                                if(intItem == 3)//PLANT, SERIAL_NO, RESULT 다음부터 ITEM이 나와야 함
                                { 

                                    //엑셀저장전 쿼리 파라미터 설정(넣을지말지 미정)

                                    strDBName = "GPDB";
                                    strQueryID = "Qua11Data.Set_InsInfo";

                                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                                    sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                                    sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                                    sParam.Add("LINE_CD", strSplitValue[2].ToString());
                                    sParam.Add("STN_CD", strSplitValue[3].ToString());
                                    sParam.Add("DEV_ID", strSplitValue[4].ToString());
                                    sParam.Add("CAR_TYPE", strSplitValue[5].ToString());
                                    sParam.Add("DIV_FLAG", strSplitValue[6].ToString());

                                    sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                                    sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                                    sParam.Add("CUR_MENU_ID", "Qua11");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                                    //엑셀 Row를 순회하며
                                    for (int j = 0; j < ExcelDataSet.Tables[0].Rows.Count; j++)
                                    {
                                        //컬럼 연결작업
                                        string val = "<excel><value>" + columnNames.Length + "</value>";
                                        for (int k = 0; k < ExcelDataSet.Tables[0].Columns.Count; k++)
                                        {
                                            val += "<value>" + ExcelDataSet.Tables[0].Rows[j][k].ToString() + "</value>";
                                        }
                                        val += "</excel>";

                                        sParam.Add("VALUE", val);

                                        //파라미터에 엑셀데이터 넣고 CUD 처리
                                        iRtn = biz.SetCUD(strDBName, strQueryID, sParam);
                                        if (iRtn != 1)//오류발생
                                        {
                                            errRow = j;
                                            break;
                                        }
                                    }

                                        //최종 저장
                                    if (iRtn == 1)
                                    {
                                        strScript = " alert('전체 데이터 정상등록 되었습니다.');  parent.fn_ModalReloadCall('Qua11'); parent.fn_ModalCloseDiv(); ";
                                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                        File.Delete(uploadpath);
                                    }
                                    else if(errRow > -1)
                                    {
                                        strScript = " alert('엑셀 저장이 중단되었습니다. 엑셀의 "+ errRow + " 행까지 삽입했습니다.'); ";
                                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                        File.Delete(uploadpath);
                                    }
                                    else
                                    {
                                        strScript = " alert('엑셀 저장이 중단되었습니다. 엑셀을 삽입하지 못했습니다.'); ";
                                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                        File.Delete(uploadpath);
                                    }
                                }
                                else
                                {
                                    strScript = " alert('엑셀 데이터 형식이 불일치합니다.'); ";
                                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                    File.Delete(uploadpath);
                                }
                            }
                            else
                            {
                                strScript = " alert('조건에 해당하지 않는 엑셀 파일(.xlsx)입니다.'); ";
                                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                File.Delete(uploadpath);
                            }
                        }
                        else
                        {
                            strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Qua11'); parent.fn_ModalCloseDiv(); ";
                            ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                            File.Delete(uploadpath);
                        }
                        
                    }
                }
                else
                {
                    strScript = " alert('엑셀 파일(.xlsx)만 저장할 수 있습니다.'); ";
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
        protected void GetDDL(string shopCd, string lineCd, string stnCd)
        {
            //GetData 에서 호출(STN 콤보 초기 설정값 필요)
            DataSet ds = new DataSet();

            ddlShopCd.Items.Add(new ListItem("", ""));
            ddlLineCd.Items.Add(new ListItem("", ""));

            strDBName = "GPDB";
            strQueryID = "Qua11Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", "H20");
            param.Add("SHOP_CD", shopCd);
            param.Add("LINE_CD", lineCd);
            param.Add("STN_CD", stnCd);

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
                    ddlStnCd.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                }
                ddlStnCd.SelectedValue = stnCd;
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    ddlDevCd.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                {
                    ddlCarType.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                }

            }
        }
        #endregion
    }
}
