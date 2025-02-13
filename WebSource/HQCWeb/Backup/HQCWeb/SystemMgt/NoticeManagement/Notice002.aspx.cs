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

using System.Web.UI.HtmlControls;

using System.Web.Services;
using System.IO;

namespace HQCWeb.SystemMgt.NoticeManagement
{
    public partial class Notice002 : System.Web.UI.Page
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
            strQueryID = "ComCodeData.Get_ComCodeByComTypeInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("COMM_TYPE", "USE_YN");

            sParam.Add("CUR_MENU_ID", "WEB_00100");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            Biz.SystemManagement.ComCodeMgt biz = new Biz.SystemManagement.ComCodeMgt();

            ds = biz.GetComCodeByComTypeInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlUseYN.Items.Add(new ListItem(ds.Tables[0].Rows[i]["COMM_DESC"].ToString(), ds.Tables[0].Rows[i]["COMM_CD"].ToString()));
                }
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbNoticeTitle.Text      = Dictionary_Data.SearchDic("NOTICE_TITLE", bp.g_language);
            lbNoticeContent.Text    = Dictionary_Data.SearchDic("NOTICE_CONTENT", bp.g_language);
            lbUseYN.Text            = Dictionary_Data.SearchDic("USE_YN", bp.g_language);
            lbAttachFile.Text       = Dictionary_Data.SearchDic("ATTACHMENT_FILE", bp.g_language);

            // 상세내용 확인후 수정 또는 삭제일 경우
            lbWorkName.Text         = Dictionary_Data.SearchDic("DETAIL", bp.g_language); // 상세
        }
        #endregion

        #region GetData
        protected void GetData()
        {
            string strScript = string.Empty;

            DataSet ds = new DataSet();

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            lbNoticeNum.Text = strPVal;
            
            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');
            
            // 비지니스 클래스 작성
            Biz.SystemManagement.NoticeMgt biz = new Biz.SystemManagement.NoticeMgt();
            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "NoticeData.Get_NoticeInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("NOTICE_NUM", strSplitValue[0].ToString());

            sParam.Add("CUR_MENU_ID", "WEB_00100");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetNoticeInfo(strDBName, strQueryID, sParam);
            
            if (ds.Tables.Count > 0)
            {
                txtNoticeTitle.Text         = ds.Tables[0].Rows[0]["NOTICE_TITLE"].ToString();
                content_Editer.InnerHtml    = ds.Tables[0].Rows[0]["NOTICE_CONTENT"].ToString().Replace("`", "<p>").Replace("|", "</p>").Replace("￡", "<br>"); 
                ddlUseYN.SelectedValue      = ds.Tables[0].Rows[0]["USE_YN"].ToString();
                //txtFilePath.Text            = cy.Encrypt(ds.Tables[0].Rows[0]["FILE_PATH"].ToString());

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
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('WEB_00100'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            
            ds.Tables.Clear();

            strQueryID = "NoticeData.Get_NoticeAttachFileInfo";

            ds = biz.GetNoticeInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {

                if (ds.Tables[0].Rows.Count > 0) {

                    string strChgFileName       = string.Empty;
                    string strOrgFileName       = string.Empty;
                    string strFilePath          = string.Empty;
                    string strFileExt           = string.Empty;
                    string strICon              = string.Empty;
                    string strReplaceFileName   = string.Empty;
                    string strFileSize          = string.Empty;

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++) {

                        strChgFileName  = ds.Tables[0].Rows[i]["CHG_FILE_NAME"].ToString();
                        strOrgFileName  = ds.Tables[0].Rows[i]["ORG_FILE_NAME"].ToString();
                        strFilePath     = ds.Tables[0].Rows[i]["FILE_PATH"].ToString();
                        strFileExt      = ds.Tables[0].Rows[i]["FILE_EXT"].ToString();
                        strFileSize     = ds.Tables[0].Rows[i]["FILE_SIZE"].ToString();

                        txtFilePath.Text = cy.Encrypt(ds.Tables[0].Rows[0]["FILE_PATH"].ToString());
                        
                        strReplaceFileName = ds.Tables[0].Rows[i]["ORG_FILE_NAME"].ToString().Replace(strFileExt, "").Replace(".", "");
                        
                        if (strFileExt == "png" || strFileExt == "gif" || strFileExt == "jpg")
                        {
                            strICon = "image";
                        }

                        if (strFileExt == "ppt" || strFileExt == "pptx")
                        {
                            strICon = "powerpoint";
                        }

                        if (strFileExt == "xls" || strFileExt == "xlsx")
                        {
                            strICon = "excel";
                        }

                        if (strFileExt == "doc" || strFileExt == "docx")
                        {
                            strICon = "word";
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
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('WEB_00100'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion
        
        #region SetNoticeInfoAdd
        [WebMethod]
        public static string SetNoticeInfoAdd(string sParams, string sParams2, string sParams3, string sParams4, string sParams5)
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

            string[] strNoticeNum = cy.Decrypt(sParams4).Split('/');

            string[] strValue = cy.Decrypt(sParams5).Split('/');
            
            strDBName = "GPDB";
            strQueryID = "NoticeData.Set_NoticeUpdateInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("NOTICE_TITLE",      sParams);
            sParam.Add("NOTICE_CONTENT",    sParams2);
            sParam.Add("USE_YN",            sParams3);
            sParam.Add("NOTICE_NUM",        strNoticeNum[0].ToString());

            sParam.Add("REG_ID",            bp.g_userid.ToString());    // 등록자
            sParam.Add("CUD_TYPE",          "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            sParam.Add("CUR_MENU_ID",       "WEB_00100");               // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            sParam.Add("PREV_DATA",         strValue[0].ToString());    // 이전 데이터 셋팅

            if (sParams == "")
            {
                strRtn = "N";
            }
            else
            {
                Biz.SystemManagement.NoticeMgt biz = new Biz.SystemManagement.NoticeMgt();

                //  등록 서비스 작성
                iRtn = biz.SetNoticeInfo(strDBName, strQueryID, sParam);

                if (iRtn == 1)
                {
                    strRtn = "C";
                }
                else
                {
                    strRtn = "E";
                }
            }
           
            return strRtn;
        }
        #endregion

        #region btnModify_Click
        protected void btnModify_Click(object sender, EventArgs e)
        {

            /*
            int iRtn = 0;
            string strScript = string.Empty;
            
            string strPNum = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strNumValue = cy.Decrypt(strPNum).Split('/');


            string strPVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strValue = cy.Decrypt(strPVal).Split('/');

            //if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            //{
            // 비지니스 클래스 작성
            Biz.SystemManagement.NoticeMgt biz = new Biz.SystemManagement.NoticeMgt();

            strDBName = "GPDB";
            strQueryID = "NoticeData.Set_NoticeInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            //sParam.Add("Param1", txtDetail.Text);

            sParam.Add("NOTICE_TITLE", txtNoticeTitle.Text);
            sParam.Add("NOTICE_CONTENT",    sParams2);
            sParam.Add("USE_YN",            sParams3);
            sParam.Add("NOTICE_NUM",        strNumValue[0].ToString());
            

            sParam.Add("REG_ID",            bp.g_userid.ToString());    // 등록자
            sParam.Add("CUD_TYPE",          "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            sParam.Add("CUR_MENU_ID",       "WEB_00100");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            sParam.Add("PREV_DATA",         strValue[0].ToString());    // 이전 데이터 셋팅

            // 수정 비지니스 메서드 작성
            //iRtn = biz.(strDBName, strQueryID, sParam);

            if (iRtn == 1)
            {
                (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall('WEB_00100'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                strScript = " alert('수정에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }

            //}
            //else
            //{
            //    strScript = " fn_ExError(); ";
            //    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            //}
            */
        }
        #endregion
        
        #region btnDelete_Click
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int iRtn = 0;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strValue = cy.Decrypt(strPVal).Split('/');
            
            string strDVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string[] strDValue = cy.Decrypt(strDVal).Split('/');

            // 비지니스 클래스 작성
            Biz.SystemManagement.NoticeMgt biz = new Biz.SystemManagement.NoticeMgt();

            strDBName = "GPDB";
            strQueryID = "NoticeData.Set_NoticeinfoDel";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("NOTICE_NUM",    strValue[0].ToString());

            sParam.Add("REG_ID",        bp.g_userid.ToString());    // 등록자
            sParam.Add("CUD_TYPE",      "D");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            sParam.Add("CUR_MENU_ID",   "WEB_00100");               // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
            sParam.Add("PREV_DATA",     strDValue[0].ToString());                  // 이전 데이터 셋팅

            // 삭제 비지니스 메서드 작성
            iRtn = biz.DelNoticeInfo(strDBName, strQueryID, sParam);

            if (iRtn == 1)
            {

                string Notice_Path = System.Configuration.ConfigurationManager.AppSettings.Get("NOTICE_PATH");
                string FolderNM = cy.Decrypt(txtFilePath.Text);
                string FilePath = Notice_Path + FolderNM;

                Directory.Delete(FilePath, true);


                (Master.FindControl("hidPopDefaultValue") as HiddenField).Value = "";

                strScript = " alert('정상삭제 되었습니다.');  parent.fn_ModalReloadCall('WEB_00100'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                strScript = " alert('삭제에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion
        
        #region SetFileUplad
        public string SetFileUplad(string strNoticeNum)
        {
            string strRtn = string.Empty;
            int iRow = 0;
            int iRtn = 0;

            string[] arrFileName = txtFileName.Text.Split(',');

            int iCnt = fuUpload.PostedFiles.Count;

            string Notice_Path = System.Configuration.ConfigurationManager.AppSettings.Get("NOTICE_PATH");
            string FolderNM = cy.Decrypt(txtFilePath.Text);
            string FilePath = Notice_Path + FolderNM;

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

                                if (su.strBadFileNameChk(strValName))
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


                        Biz.SystemManagement.NoticeMgt biz = new Biz.SystemManagement.NoticeMgt();

                        strDBName = "GPDB";
                        strQueryID = "NoticeData.Set_NoticeFileInfo";

                        string[] arrVal = cy.Decrypt(strNoticeNum).Split('/');

                        FW.Data.Parameters sParam = new FW.Data.Parameters();

                        sParam.Add("FILE_INFO",     strRtn);
                        sParam.Add("NOTICE_NUM",    arrVal[0].ToString());

                        sParam.Add("REG_ID",        bp.g_userid.ToString());    // 등록자
                        sParam.Add("CUD_TYPE",      "U");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                        sParam.Add("CUR_MENU_ID",   "WEB_00100");               // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                        // 등록 비지니스 메서드 작성
                        iRtn = biz.SetNoticeFileInfo(strDBName, strQueryID, sParam);

                        strRtn = iRtn.ToString();
                    }
                }
                else {
                    strRtn = "1";
                }
            }
            catch
            {

                Directory.Delete(FilePath, true);

                //strRtn = "ERROR";

                strRtn = "0";
            }

            return strRtn;
        }
        #endregion

        #region SetImageUpload
        [WebMethod]
        public static string SetImageUpload(string ImageData, string strFileName, string strFileSize, string strFielType)
        {
            string strNewName = string.Empty;
            string strExt = string.Empty;
            string strRtn = string.Empty;

            string Image_Path = System.Configuration.ConfigurationManager.AppSettings.Get("NOTICE_PATH");
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

                    strRtn = "C|" + "http://10.208.163.30/Notice/" + FolderNM.Replace("\\", "/") + "/" + strNewName;
                }
                catch (Exception e)
                {
                    strRtn = "E";
                }
            }
            else
            {
                strRtn = "N";
            }

            return strRtn;
        }
        #endregion

        #region btnUpload_Click
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string strScript = string.Empty;

            string strRtn = string.Empty;

            strRtn = SetFileUplad(lbNoticeNum.Text);

            if (strRtn == "1")
            {
                strScript = " alert('정상수정 되었습니다.');  parent.fn_ModalReloadCall('WEB_00100'); parent.fn_ModalCloseDiv(); ";
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), strScript, true);
            }
            else
            {
                strScript = " alert('수정에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion
        
        #region SetNoticeInfoAdd
        [WebMethod]
        public static string SetNoticeFileInfoDel(string sParams, string sParams2, string sParams3, string sParams4)
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

            string strNoticePath = System.Configuration.ConfigurationManager.AppSettings.Get("NOTICE_PATH");

            string[] strNoticeNum = cy.Decrypt(sParams).Split('/');

            string[] strOrgFileName = cy.Decrypt(sParams2).Split('/');

            string[] strChgFileName = cy.Decrypt(sParams3).Split('/');

            string[] strPath = cy.Decrypt(sParams4).Split('/');

            strDBName = "GPDB";
            strQueryID = "NoticeData.Set_NoticeFileInfoDel";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("NOTICE_NUM",        strNoticeNum[0].ToString());
            sParam.Add("ORG_FILE_NAME",     strOrgFileName[0].ToString());
            
            Biz.SystemManagement.NoticeMgt biz = new Biz.SystemManagement.NoticeMgt();

            //  등록 서비스 작성
            iRtn = biz.SetNoticeInfo(strDBName, strQueryID, sParam);
            
            if (iRtn == 1)
            {
                string fileName = strNoticePath + "\\" + strPath[0].ToString() + "\\" + strChgFileName[0].ToString();
                
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

    }
}

