using HQCWeb.FMB_FW;
using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using MES.FW.Common.Crypt;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HQCWeb.SystemMgt.NoticeManagement
{
    public partial class Notice005 : System.Web.UI.Page
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

        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbNoticeTitle.Text = Dictionary_Data.SearchDic("NOTICE_TITLE", bp.g_language);
            lbNoticeContent.Text = Dictionary_Data.SearchDic("NOTICE_CONTENT", bp.g_language);
            lbAttachFile.Text = Dictionary_Data.SearchDic("ATTACHMENT_FILE", bp.g_language);

            lbWorkName.Text = Dictionary_Data.SearchDic("DETAIL", bp.g_language); // 상세
        }
        #endregion

        #region GetData
        protected void GetData()
        {
            //AppSettingsReader myAppSet = new AppSettingsReader();
            //string ATTACH_PATH = (string)myAppSet.GetValue("ATTACH_PATH", typeof(string));
            string ATTACH_PATH = System.Configuration.ConfigurationManager.AppSettings.Get("ATTACH_PATH");
            string strScript = string.Empty;

            DataSet ds = new DataSet();

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            // 비지니스 클래스 작성
            Biz.SystemManagement.NoticeMgt biz = new Biz.SystemManagement.NoticeMgt();

            strDBName = "GPDB";
            strQueryID = "NoticeData.Get_NoticeInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("NOTICE_NUM", strSplitValue[0].ToString());

            sParam.Add("CUR_MENU_ID", "WEB_00100");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetNoticeInfo(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                lbGetNoticeTitle.Text = ds.Tables[0].Rows[0]["NOTICE_TITLE"].ToString();
                //ltGetNoticeContent.Text = "<br><br>" + ds.Tables[0].Rows[0]["NOTICE_CONTENT"].ToString().Replace("`", "<p>").Replace("|", "</p>").Replace("￡", "<br>");

                content_Editer.InnerHtml = ds.Tables[0].Rows[0]["NOTICE_CONTENT"].ToString();
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('MENU_ID'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }

            ds.Tables.Clear();

            strQueryID = "NoticeData.Get_NoticeAttachFileInfo";

            // 상세조회 비지니스 메서드 호출
            ds = biz.GetNoticeInfo(strDBName, strQueryID, sParam);


            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string strChgFileName = string.Empty;
                    string strOrgFileName = string.Empty;
                    string strFilePath = string.Empty;
                    string strFileExt = string.Empty;
                    string strICon = string.Empty;
                    string strReplaceFileName = string.Empty;


                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        strChgFileName = ds.Tables[0].Rows[i]["CHG_FILE_NAME"].ToString();
                        strOrgFileName = ds.Tables[0].Rows[i]["ORG_FILE_NAME"].ToString();
                        strFilePath = ds.Tables[0].Rows[i]["FILE_PATH"].ToString();
                        strFileExt = ds.Tables[0].Rows[i]["FILE_EXT"].ToString();

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

                        strReplaceFileName = ds.Tables[0].Rows[i]["ORG_FILE_NAME"].ToString().Replace(strFileExt, "").Replace(".", "");


                        if (strReplaceFileName.Length > 12)
                        {
                            strReplaceFileName = strReplaceFileName.Substring(0, 10) + "...";
                        }

                        strFileInfo += "<div style=\"width: 100px; float:left; text-align:center;\">" +
                                            "<a href=\"" + ATTACH_PATH + strFilePath + "/" + strChgFileName + "\" download=\"" + strOrgFileName + "\"><img src=\"/images/btn/" + strICon + "_80.png\" /></a>" +
                                            "<br />" +
                                            "<span>" + strReplaceFileName + "</span>" +
                                        "</div>";
                    }
                }
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('MENU_ID'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion
    }
}
