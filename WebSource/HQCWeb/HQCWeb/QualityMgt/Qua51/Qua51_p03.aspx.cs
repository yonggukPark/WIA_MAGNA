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

namespace HQCWeb.QualityMgt.Qua51
{
    public partial class Qua51_p03 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        //포맷 그리드 생성용
        string jsField = string.Empty;
        string jsCol = string.Empty;

        Biz.QualityManagement.Qua51 biz = new Biz.QualityManagement.Qua51();
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

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid' 함수 호출
                string script = $@" column = {jsCol}; 
                                field = {jsField}; 
                                createGrid(); ";

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script, true);

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
            //컬럼 설정
            string[] columnCols = { "PLANT_CD", "MAN_NO", "MAN_DEPT", "PART_NAME", "STANDARD", "PART_SERIAL_NO", "INSP_CYCLE" };

            //길이 설정
            string[] arrColumnWidth = new string[columnCols.Length];

            //캡션 설정
            string[] columnNames = new string[columnCols.Length];

            for (int i = 0; i < columnCols.Length; i++)
            {
                arrColumnWidth[i] = "200";
                columnNames[i] = Dictionary_Data.SearchDic(columnCols[i], bp.g_language);
            }

            //컬럼설정, 필드설정
            jsCol = ConvertJSONData.ConvertColArrToJSON(columnCols, columnNames, arrColumnWidth, "cols");
            jsField = ConvertJSONData.ConvertColArrToJSON(columnCols, columnNames, arrColumnWidth, "fields");
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            // 업로드
            lbWorkName.Text = Dictionary_Data.SearchDic("UPLOAD", bp.g_language);
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
            DataSet ds = new DataSet();
            string strRtn = string.Empty;
            int iRtn = 0, errRow = -1;
            string strScript = string.Empty;

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                try
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
                            DataSet ExcelDataSet = null;

                            try
                            {
                                // xls는 오류 발생중...
                                if (FileExt == ".xls")
                                {
                                    con = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.8.0;Data Source=" + uploadpath + @";Extended Properties=""Excel 8.0;HDR=Yes;IMEX=1;""");
                                }
                                else if (FileExt == ".xlsx")
                                {
                                    con = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + uploadpath + @";Extended Properties=""Excel 12.0;HDR=Yes;IMEX=1;""");
                                }
                                //엑셀 추출 시작
                                con.Open();
                                DataTable dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                string getExcelSheetName = dt.Rows[0]["Table_Name"].ToString();
                                OleDbCommand ExcelCommand = new OleDbCommand(@"SELECT * FROM [" + getExcelSheetName + @"]", con);
                                OleDbDataAdapter ExcelAdapter = new OleDbDataAdapter(ExcelCommand);
                                ExcelDataSet = new DataSet();
                                ExcelAdapter.Fill(ExcelDataSet);
                                con.Close();
                            }
                            catch (Exception ex)
                            {
                                // 예외가 발생한 메서드 이름 및 예외 메시지 기록
                                string errorMessage = $"Error in Excel : {ex.Message}";

                                // 스택 트레이스 정보를 추가로 기록하고 싶은 경우:
                                errorMessage += $"\nStack Trace: {ex.StackTrace}";

                                // 로그 파일에 저장
                                LogMessage(errorMessage);
                            }
                            finally
                            {
                                // 연결이 null이 아니고 열려 있는 경우에만 닫기
                                if (con != null && con.State == ConnectionState.Open)
                                {
                                    con.Close();
                                }
                            }

                            //엑셀 추출 끝

                            strDBName = "GPDB";
                            strQueryID = "Qua51Data.Set_CurrectUploadInfo";

                            FW.Data.Parameters sParam = null;

                            if (ExcelDataSet == null)
                            {
                                strScript = " alert('엑셀 추출 실패 : " + uploadpath + "'); ";
                                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                File.Delete(uploadpath);
                            }
                            else
                            {
                                for (int i = 0; i < ExcelDataSet.Tables[0].Rows.Count; i++)
                                {
                                    sParam = new FW.Data.Parameters();
                                    sParam.Add("PLANT_CD", ExcelDataSet.Tables[0].Rows[i][0].ToString());
                                    sParam.Add("MAN_NO", ExcelDataSet.Tables[0].Rows[i][1].ToString());
                                    sParam.Add("MAN_DEPT", ExcelDataSet.Tables[0].Rows[i][2].ToString());
                                    sParam.Add("PART_NAME", ExcelDataSet.Tables[0].Rows[i][3].ToString());
                                    sParam.Add("STANDARD", ExcelDataSet.Tables[0].Rows[i][4].ToString());
                                    sParam.Add("PART_SERIAL_NO", ExcelDataSet.Tables[0].Rows[i][5].ToString());
                                    sParam.Add("INSP_CYCLE", ExcelDataSet.Tables[0].Rows[i][6].ToString());
                                    
                                    sParam.Add("USER_ID", bp.g_userid.ToString());

                                    sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                                    sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                                    sParam.Add("CUR_MENU_ID", "Qua51");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                                    iRtn = biz.SetCUD(strDBName, strQueryID, sParam);
                                    if (iRtn != 1)//오류발생
                                    {
                                        errRow = i;
                                        break;
                                    }
                                }

                                //최종 저장
                                if (iRtn == 1)
                                {
                                    strScript = " alert('전체 데이터 정상등록 되었습니다.');  parent.fn_ModalReloadCall('Qua51'); parent.fn_ModalCloseDiv(); ";
                                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                                    File.Delete(uploadpath);
                                }
                                else if (errRow > -1)
                                {
                                    strScript = " alert('엑셀 저장이 중단되었습니다. 엑셀의 " + errRow + " 행까지 삽입했습니다.'); ";
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
                        }
                    }
                    else
                    {
                        strScript = " alert('엑셀 파일(.xlsx)만 저장할 수 있습니다.'); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                }
                catch (Exception ex2)
                {
                    // 예외가 발생한 메서드 이름 및 예외 메시지 기록
                    string errorMessage = $"Error in btnSave_Click : {ex2.Message}";

                    // 스택 트레이스 정보를 추가로 기록하고 싶은 경우:
                    errorMessage += $"\nStack Trace: {ex2.StackTrace}";

                    // 로그 파일에 저장
                    LogMessage(errorMessage);
                }
            }
            else
            {
                strScript = " fn_ExError(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region LogMessage
        public void LogMessage(string message)
        {
            // 로그 파일 경로 설정
            string logPath = Server.MapPath("~/Log/log.txt");

            // 로그 메시지에 시간 정보 추가 (선택 사항)
            string logMessage = $"{DateTime.Now}: {message}";

            // 로그 파일에 메시지를 개행 문자로 구분하여 추가
            System.IO.File.AppendAllText(logPath, logMessage + Environment.NewLine);
        }
        #endregion
    }
}
