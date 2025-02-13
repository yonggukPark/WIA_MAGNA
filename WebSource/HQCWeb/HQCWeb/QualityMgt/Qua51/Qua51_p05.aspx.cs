using HQCWeb.FMB_FW;
using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using MES.FW.Common.Crypt;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HQCWeb.QualityMgt.Qua51
{
    public partial class Qua51_p05 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        protected string strVal = string.Empty;

        public string strFileInfo = string.Empty;

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

        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            // 상세내용 확인후 수정 또는 삭제일 경우
            lbWorkName.Text = Dictionary_Data.SearchDic("DETAIL", bp.g_language); // 상세

            lbCertNo.Text = Dictionary_Data.SearchDic("CERT_NO", bp.g_language);
            lbIssueDt.Text = Dictionary_Data.SearchDic("ISSUE_DT", bp.g_language);
            lbAttachFile.Text = Dictionary_Data.SearchDic("ATTACHMENT_FILE", bp.g_language);
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

            // 세션에 키값 저장
            Session["Qua51PopValue"] = strPVal;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Qua51Data.Get_CurrectDetailInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("MAN_NO", strSplitValue[1].ToString());
            sParam.Add("CERT_NO", strSplitValue[2].ToString());

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
                    lbGetCertNo.Text = ds.Tables[0].Rows[0]["CERT_NO"].ToString();
                    txtDate.Text = ds.Tables[0].Rows[0]["ISSUE_DT"].ToString();

                    //업로드된 파일 출력 개시
                    string strChgFileName = string.Empty;
                    string strOrgFileName = string.Empty;
                    string strFilePath = string.Empty;
                    string strFileExt = string.Empty;
                    string strICon = string.Empty;
                    string strReplaceFileName = string.Empty;
                    string strFileSize = string.Empty;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //파일명 존재하면 출력
                        if (ds.Tables[0].Rows[i]["ORG_FILE_NAME"].ToString().Length > 0)
                        {
                            strChgFileName = ds.Tables[0].Rows[i]["CHG_FILE_NAME"].ToString();
                            strOrgFileName = ds.Tables[0].Rows[i]["ORG_FILE_NAME"].ToString();
                            strFileExt = ds.Tables[0].Rows[i]["FILE_EXT"].ToString();
                            strFileSize = ds.Tables[0].Rows[i]["FILE_SIZE"].ToString();

                            txtFilePath.Text = cy.Encrypt(ds.Tables[0].Rows[0]["FILE_PATH"].ToString());

                            //strReplaceFileName = ds.Tables[0].Rows[i]["ORG_FILE_NAME"].ToString().Replace(".", "_").Replace(" ", "_");
                            strReplaceFileName = Regex.Replace(ds.Tables[0].Rows[i]["ORG_FILE_NAME"].ToString(), @"[^a-zA-Z0-9가-힣-_:.]", "_");

                            if (strFileExt.ToLower() == "pdf")
                            {
                                strICon = "pdf";
                            }

                            strFileInfo += "<tr style=\"height=30px;\"  id='tr" + strReplaceFileName + "'>" +
                                                "<td><img src=\"/images/btn/delete.png\" style=\"width:15px;height:15px;padding-bottom:2px;\" onclick=\"javascript:fn_fileDel('" + strReplaceFileName + "', '" + strFileExt.Replace(".", "") + "', 'P', '" + cy.Encrypt(strOrgFileName) + "', '" + cy.Encrypt(strChgFileName) + "');\" /></td>" +
                                                "<td><img src=\"/images/btn/" + strICon + "_23.png\" /></td>" +
                                                "<td style=\"text-align:left; padding-left:10px;\">" + strOrgFileName + "</td>" +
                                                "<td style=\"display:none;\">P</td>" +
                                                "<td style=\"display:none;\">" + strFileSize + "</td>" +
                                            "</tr>";
                        }
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
            string strRtn = string.Empty;
            int iRtn = 0;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strUVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string strUValue = cy.Decrypt(strUVal);
            strRtn = SetFileUplad(strSplitValue[0].ToString(), strSplitValue[1].ToString(), strSplitValue[2].ToString());

            if (strRtn.Equals("1"))
            {
                // 비지니스 클래스 작성
                //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();

                strDBName = "GPDB";
                strQueryID = "Qua51Data.Upt_CurrectDetailInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("MAN_NO", strSplitValue[1].ToString());
                sParam.Add("CERT_NO", strSplitValue[2].ToString());

                sParam.Add("ISSUE_DT", txtDate.Text.Replace("-", ""));
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

                    strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall2('Qua51'); parent.fn_ModalCloseDiv(); ";
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
                strScript = " alert('파일 첨부에 실패했습니다. 관리자에게 문의바립니다.'); ";
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
            
            strDBName = "GPDB";
            strQueryID = "Qua51Data.Del_CurrectDetailInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("MAN_NO", strSplitValue[1].ToString());
            sParam.Add("CERT_NO", strSplitValue[2].ToString());

            sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            sParam.Add("CUD_TYPE", "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            sParam.Add("CUR_MENU_ID", "Qua51");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            sParam.Add("PREV_DATA", strDValue);   // 이전 데이터 셋팅

            // 삭제 비지니스 메서드 작성
            iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

            if (iRtn == 1)
            {
                //첨부파일이 있는 경우, 삭제
                if (txtFilePath.Text.Length > 0)
                {
                    string File_Path = System.Configuration.ConfigurationManager.AppSettings.Get("FILE_PATH") + "Qua51\\";
                    string FolderNM = cy.Decrypt(txtFilePath.Text);
                    string FilePath = File_Path + FolderNM;

                    if(Directory.Exists(FilePath))
                        Directory.Delete(FilePath, true);
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
        #endregion

        #region SetFileInfoDel
        [WebMethod]
        public static string SetFileInfoDel(string sParams, string sParams2, string sParams3)
        {
            StringUtil su = new StringUtil();
            Crypt cy = new Crypt();
            BasePage bp = new BasePage();

            string strRtn = string.Empty;

            int iRtn = 0;
            string strScript = string.Empty;

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

            string strFilePath = System.Configuration.ConfigurationManager.AppSettings.Get("FILE_PATH");

            //세션에서 키값 추출
            string strPVal = HttpContext.Current.Session["Qua51PopValue"]?.ToString();

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string[] strOrgFileName = cy.Decrypt(sParams).Split('/');

            string[] strChgFileName = cy.Decrypt(sParams2).Split('/');

            string[] strPath = cy.Decrypt(sParams3).Split('/');

            strDBName = "GPDB";
            strQueryID = "Qua51Data.Set_FileInfoDel";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("MAN_NO", strSplitValue[1].ToString());
            sParam.Add("CERT_NO", strSplitValue[2].ToString());
            sParam.Add("ORG_FILE_NAME", strOrgFileName[0].ToString());

            Biz.QualityManagement.Qua51 biz = new Biz.QualityManagement.Qua51();

            //  등록 서비스 작성
            iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

            if (iRtn == 1)
            {
                string fileName = strFilePath + "\\Qua51\\" + strPath[0].ToString() + "\\" + strChgFileName[0].ToString();

                if (File.Exists(fileName))
                {
                    try
                    {
                        File.Delete(fileName);

                        strRtn = "D";
                    }
                    catch (Exception e)
                    {
                        strRtn = "E";
                    }
                }
                else
                {
                    strRtn = "E";
                }
            }
            else
            {
                strRtn = "E";
            }

            return strRtn;
        }
        #endregion

        #region SetFileUplad
        public string SetFileUplad(string plantCd, string manNo, string certNo)
        {
            string strRtn = string.Empty;
            int iRow = 0;
            int iRtn = 0;
            string FilePath = null;

            if (txtFileName.Text.Length > 0)
            {
                string[] arrFileName = txtFileName.Text.Split(',');

                int iCnt = fuUpload.PostedFiles.Count;

                string File_Path = System.Configuration.ConfigurationManager.AppSettings.Get("FILE_PATH") + "Qua51\\";
                string FolderNM = (txtFilePath.Text.Length > 0) ? cy.Decrypt(txtFilePath.Text) : DateTime.Now.ToString("yyyyMMddhhssmm"); // txtFilePath 없으면 신규, 있으면 기존등록
                FilePath = File_Path + FolderNM;

                try
                {
                    if (arrFileName.Length > 0)
                    {
                        if (!Directory.Exists(FilePath))
                        {
                            Directory.CreateDirectory(FilePath);
                        }

                        if (iCnt > 0)
                        {
                            foreach (var Files in fuUpload.PostedFiles)
                            {
                                string strNewName = string.Empty;
                                string strExt = string.Empty;
                                string strChgFileName = string.Empty;
                                string strOrgFileName = string.Empty;
                                string strValName = string.Empty;
                                string strSize = string.Empty;

                                if (arrFileName[iRow].ToString() != "")
                                {
                                    strNewName = Guid.NewGuid().ToString().Replace("-", "");
                                    strExt = Path.GetExtension(Files.FileName);

                                    strValName = Files.FileName.Replace(strExt, "");
                                    strSize = Files.ContentLength.ToString();

                                    if (su.strBadFileNameChk(strExt))
                                    {
                                        strOrgFileName = Files.FileName;
                                        strChgFileName = strNewName + strExt;

                                        if (strRtn == "")
                                        {
                                            strRtn = FolderNM + "@";

                                            strRtn += strOrgFileName + "/" + strChgFileName + "/" + strSize + "/" + strExt.Replace(".", "");
                                        }
                                        else
                                        {
                                            strRtn += "," + strOrgFileName + "/" + strChgFileName + "/" + strSize + "/" + strExt.Replace(".", "");
                                        }

                                        //strRtn = "1";

                                        Files.SaveAs(FilePath + "\\" + strChgFileName);
                                    }
                                    else
                                    {
                                        Directory.Delete(FilePath, true);

                                        //strRtn = "FILEERROR";

                                        strRtn = "0";

                                        break;
                                    }
                                }

                                iRow++;
                            }

                            //정상 진행되었다면 진행
                            if (strRtn != "0")
                            {
                                strDBName = "GPDB";
                                strQueryID = "Qua51Data.Set_FileInfo";

                                FW.Data.Parameters sParam = new FW.Data.Parameters();
                                sParam.Add("PLANT_CD", plantCd);
                                sParam.Add("MAN_NO", manNo);
                                sParam.Add("CERT_NO", certNo);
                                sParam.Add("FILE_INFO", strRtn);
                                sParam.Add("USER_ID", bp.g_userid.ToString());

                                sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                                sParam.Add("CUD_TYPE", "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                                sParam.Add("CUR_MENU_ID", "Qua51");               // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                                // 등록 비지니스 메서드 작성
                                iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                                strRtn = iRtn.ToString();
                            }
                        }
                    }
                    else
                    {
                        strRtn = "1";
                    }
                }
                catch
                {

                    Directory.Delete(FilePath, true);

                    //strRtn = "ERROR";

                    strRtn = "0";
                }
            }
            else
            {
                //수정할 파일 자체가 없으므로, 정상처리
                strRtn = "1";
            }

            //리턴이 1이 아니면, 모두 삭제
            if (!strRtn.Equals("1") && FilePath.Length > 0)
            {
                Directory.Delete(FilePath, true);
            }

            return strRtn;
        }
        #endregion
    }
}
