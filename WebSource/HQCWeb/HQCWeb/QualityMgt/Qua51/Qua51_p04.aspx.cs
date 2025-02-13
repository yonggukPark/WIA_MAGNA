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
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HQCWeb.QualityMgt.Qua51
{
    public partial class Qua51_p04 : System.Web.UI.Page
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

        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            // 등록일경우
            lbWorkName.Text = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language); // 등록

            lbCertNo.Text = Dictionary_Data.SearchDic("CERT_NO", bp.g_language);
            lbIssueDt.Text = Dictionary_Data.SearchDic("ISSUE_DT", bp.g_language);
            lbAttachFile.Text = Dictionary_Data.SearchDic("ATTACHMENT_FILE", bp.g_language);

            // 상세내용 확인후 수정 또는 삭제일 경우
            //lbWorkName.Text = Dictionary_Data.SearchDic("DETAIL", bp.g_language); // 상세
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
            string strRtn = string.Empty;
            int iRtn = 0;
            string strScript = string.Empty;

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                strDBName = "GPDB";
                strQueryID = "Qua51Data.Get_CurrectDetailID_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("PLANT_CD", strSplitValue[0].ToString());
                sParamIDChk.Add("MAN_NO", strSplitValue[1].ToString());
                sParamIDChk.Add("CERT_NO", txtCertNo.Text);

                // 아이디 체크 비지니스 메서드 작성
                strRtn = biz.GetIDValChk(strDBName, strQueryID, sParamIDChk);
                if (strRtn == "0")
                {

                    strRtn = SetFileUplad(strSplitValue[0].ToString(), strSplitValue[1].ToString(), txtCertNo.Text);
                    
                    strDBName = "GPDB";
                    strQueryID = "Qua51Data.Set_CurrectDetailInfo";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                    sParam.Add("MAN_NO", strSplitValue[1].ToString());
                    sParam.Add("CERT_NO", txtCertNo.Text);
                    sParam.Add("ISSUE_DT", txtDate.Text.Replace("-", ""));

                    sParam.Add("USER_ID", bp.g_userid.ToString());

                    sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                    sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                    sParam.Add("CUR_MENU_ID", "Qua51");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                    // 등록 비지니스 메서드 작성
                    iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('Qua51'); parent.fn_ModalCloseDiv(); ";
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
                    strScript = " alert('존재하는 데이터 입니다. 등록하려는 데이터를 다시 입력하세요.'); ";
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

        #region SetFileUplad
        public string SetFileUplad(string plantCd, string manNo, string certNo)
        {
            string strRtn = string.Empty;
            int iRow = 0;
            int iRtn = 0;
            string FilePath = null;

            string[] arrFileName = txtFileName.Text.Split(',');

            int iCnt = fuUpload.PostedFiles.Count;

            string File_Path = System.Configuration.ConfigurationManager.AppSettings.Get("FILE_PATH") + "Qua51\\";
            string FolderNM = DateTime.Now.ToString("yyyyMMddhhssmm"); // 신규
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

            //리턴이 1이 아니면, 모두 삭제
            if (!strRtn.Equals("1") && FilePath.Length > 0)
            {
                if(Directory.Exists(FilePath))
                    Directory.Delete(FilePath, true);
            }

            return strRtn;
        }
        #endregion
    }
}
