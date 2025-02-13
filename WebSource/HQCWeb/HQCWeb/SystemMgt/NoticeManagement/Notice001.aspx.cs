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

using System.Web.Services;
using System.IO;
using System.Text.RegularExpressions;

namespace HQCWeb.SystemMgt.NoticeManagement
{
    public partial class Notice001 : System.Web.UI.Page
    {
        public static Notice001 instance;

        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        protected string strVal = string.Empty;

        // 암복호화 키값 셋팅
        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            instance = this;
            
            cy.Key = strKey;
            
            if (!IsPostBack)
            {
                SetCon();

                SetTitle();
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            DataSet ds = new DataSet();

            strDBName = "GPDB";
            strQueryID = "ComCodeData.Get_ComCodeByComTypeInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("COMM_TYPE",     "USE_YN");

            sParam.Add("CUR_MENU_ID",   "WEB_00100");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            Biz.SystemManagement.ComCodeMgt biz = new Biz.SystemManagement.ComCodeMgt();

            ds = biz.GetComCodeByComTypeInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlUseYN.Items.Add(new ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                }
            }


            //content_Editer.InnerHtml = "<p>ㅁㄴㅇㄹ</p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p><br></p><p>ㅁㄴㅇㄹ</p>";


        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbNoticeTitle.Text      = Dictionary_Data.SearchDic("NOTICE_TITLE", bp.g_language);
            lbNoticeContent.Text    = Dictionary_Data.SearchDic("NOTICE_CONTENT", bp.g_language);
            lbUseYN.Text            = Dictionary_Data.SearchDic("USE_YN", bp.g_language);
            lbAttachFile.Text       = Dictionary_Data.SearchDic("ATTACHMENT_FILE", bp.g_language);
            
            // 등록일경우
            lbWorkName.Text         = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language); // 등록
        }
        #endregion
        
        #region SetNoticeInfoAdd
        [WebMethod]
        public static string SetNoticeInfoAdd(string sParams, string sParams2, string sParams3)
        {
            Biz.SystemManagement.NoticeMgt biz = new Biz.SystemManagement.NoticeMgt();

            StringUtil su = new StringUtil();
            Crypt cy = new Crypt();
            BasePage bp = new BasePage();
            cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

            string strRtn = string.Empty;
            string strNum = string.Empty;

            int iRtn = 0;
            string strScript = string.Empty;

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            strDBName = "GPDB";
            strQueryID = "NoticeData.Get_NoticeNum";

            FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
            sParamIDChk.Add("CUR_MENU_ID", "WEB_00100");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            strNum = biz.GetNoticeNum(strDBName, strQueryID, sParamIDChk);

            if (strNum == "") {
                strRtn = "N";
            } else {
                
                if (sParams == "")
                {
                    strRtn = "N";
                }
                else
                {
                    strDBName = "GPDB";
                    strQueryID = "NoticeData.Set_NoticeInsertInfo";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("NOTICE_TITLE",      sParams);
                    sParam.Add("NOTICE_CONTENT",    SanitizePTagString(sParams2));
                    sParam.Add("USE_YN",            sParams3);
                    sParam.Add("NOTICE_NUM",        strNum);

                    sParam.Add("REG_ID",            bp.g_userid.ToString());
                    sParam.Add("CUD_TYPE",          "C");
                    sParam.Add("CUR_MENU_ID",       "WEB_00100");
                   
                    //  등록 서비스 작성
                    iRtn = biz.SetNoticeInfo(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        strRtn = "C|" + cy.Encrypt(strNum);
                    }
                    else
                    {
                        strRtn = "E";
                    }
                }
            }
            
            return strRtn;
        }
        #endregion
        
        #region SetImageUpload
        [WebMethod]
        public static string SetImageUpload(string ImageData, string strFileName, string strFileSize, string strFielType)
        {
            string strNewName   = string.Empty;
            string strExt       = string.Empty;
            string strRtn       = string.Empty;
            
            string Image_Path = System.Configuration.ConfigurationManager.AppSettings.Get("NOTICE_PATH");
            string Attach_Path = System.Configuration.ConfigurationManager.AppSettings.Get("ATTACH_PATH");
            //string FolderNM = "Image\\" + DateTime.Now.ToString("yyyyMMddhhssmm"); 
            string FolderNM = "Image";
            string FilePath = Image_Path + FolderNM;

            string[] arrType = strFielType.Split('/');

            if (arrType[0].ToString().ToUpper() == "IMAGE")
            {
                try
                {
                    if (!Directory.Exists(FilePath))
                    {
                        Directory.CreateDirectory(FilePath);
                    }

                    string[] arrImage = ImageData.Split(',');
                    string[] arrExt = strFileName.Split('.');

                    strNewName = Guid.NewGuid().ToString().Replace("-", "");
                    strNewName = strNewName + "." + arrExt[1].ToString();

                    FileStream stm = File.Create(FilePath + "\\" + strNewName); 
                    byte[] bImgBytes = Convert.FromBase64String(arrImage[1].ToString());

                    stm.Write(bImgBytes, 0, bImgBytes.Length); 

                    stm.Close();

                    strRtn = "C|" + Attach_Path + FolderNM.Replace("\\", "/") + "/" + strNewName;
                }
                catch (Exception e)
                {
                    strRtn = "E";
                }
            }
            else {
                strRtn = "N";
            }
            
            return strRtn;
        }
        #endregion

        #region SetFileUplad
        public string SetFileUplad(string strNoticeNum) {
            string strRtn = string.Empty;
            int iRow = 0;
            int iRtn = 0;
            
            string[] arrFileName = txtFileName.Text.Split(',');

            int iCnt = fuUpload.PostedFiles.Count;

            string Notice_Path = System.Configuration.ConfigurationManager.AppSettings.Get("NOTICE_PATH");
            string FolderNM = DateTime.Now.ToString("yyyyMMddhhssmm");
            string FilePath = Notice_Path + FolderNM;
            
            try {
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

                            //if (Files.FileName != "") {
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

                                    strRtn = "0";

                                    //strRtn = "FILEERROR";

                                    break;
                                }
                            }

                            iRow++;
                        }

                        //정상 진행되었다면 진행
                        if(strRtn != "0")
                        {
                            Biz.SystemManagement.NoticeMgt biz = new Biz.SystemManagement.NoticeMgt();

                            strDBName = "GPDB";
                            strQueryID = "NoticeData.Set_NoticeFileInfo";

                            string[] arrVal = cy.Decrypt(strNoticeNum).Split('/');

                            FW.Data.Parameters sParam = new FW.Data.Parameters();

                            sParam.Add("FILE_INFO", strRtn);
                            sParam.Add("NOTICE_NUM", arrVal[0].ToString());

                            sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                            sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                            sParam.Add("CUR_MENU_ID", "WEB_00100");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                            // 등록 비지니스 메서드 작성
                            iRtn = biz.SetNoticeFileInfo(strDBName, strQueryID, sParam);

                            strRtn = iRtn.ToString();
                        }
                    }
                }
                else {
                    strRtn = "1";
                }
            }
            catch {

                Directory.Delete(FilePath, true);
                
                strRtn = "0";
            }
            
            return strRtn;
        }
        #endregion
       
        #region btnUpload_Click
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string strScript = string.Empty;

            string strRtn = string.Empty;
            
            strRtn = SetFileUplad(txtNum.Text);

            Biz.SystemManagement.NoticeMgt biz = new Biz.SystemManagement.NoticeMgt();

            string[] arrVal = cy.Decrypt(txtNum.Text).Split('/');

            try
            {
                if (strRtn == "1")
                {
                    strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('WEB_00100'); parent.fn_ModalCloseDiv(); ";
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), strScript, true);
                }
                else
                {
                    strDBName = "GPDB";
                    strQueryID = "NoticeData.Set_NoticeinfoDel";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("NOTICE_NUM",    arrVal[0].ToString());

                    sParam.Add("REG_ID",        bp.g_userid.ToString());    // 등록자
                    sParam.Add("CUD_TYPE",      "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                    sParam.Add("CUR_MENU_ID",   "WEB_00100");               // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                    // 삭제 비지니스 메서드 작성
                    biz.DelNoticeInfo(strDBName, strQueryID, sParam);

                    strScript = " alert('등록에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                    ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), strScript, true);
                }
            }
            catch {
                // 게시글이 정상적으로 등록이 되지 않았을경우 삭제 처리한다.

                strDBName = "GPDB";
                strQueryID = "NoticeData.Set_NoticefoDel";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("NOTICE_NUM",    arrVal[0].ToString());

                sParam.Add("REG_ID",        bp.g_userid.ToString());    // 등록자
                sParam.Add("CUD_TYPE",      "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                sParam.Add("CUR_MENU_ID",   "WEB_00100");               // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                // 삭제 비지니스 메서드 작성
                biz.DelNoticeInfo(strDBName, strQueryID, sParam);

                strScript = " alert('등록에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region SanitizePTagString
        public static string SanitizePTagString(string inputString)
        {
            // 정규 표현식을 사용하여 "<p>...</p>" 형식인지 확인
            string pattern = @"^<p>(.*?)<\/p>$";
            var match = System.Text.RegularExpressions.Regex.Match(inputString, pattern, System.Text.RegularExpressions.RegexOptions.Singleline);

            string innerContent;

            if (match.Success)
            {
                // <p>와 </p> 사이의 내용을 가져옴
                innerContent = match.Groups[1].Value;
            }
            else
            {
                // <p>...</p> 형식이 아니면 전체 문자열을 innerContent로 설정
                innerContent = inputString;
            }

            // <, >를 &lt;, &gt;로 치환
            innerContent = innerContent.Replace("<", "&lt;").Replace(">", "&gt;");

            // 변환된 내용을 <p>...</p>로 감싸서 반환
            return $"<p>{innerContent}</p>";
        }
        #endregion
    }
}

