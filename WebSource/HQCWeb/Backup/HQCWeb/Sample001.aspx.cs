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
using System.Drawing;

using Spire.Presentation;

namespace HQCWeb
{
    public partial class Sample001 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        protected string strVal = string.Empty;

        public string Image_Path = System.Configuration.ConfigurationManager.AppSettings.Get("PPT_PATH");

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
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
            int iSYear = Convert.ToInt32(System.DateTime.Now.AddYears(-10).ToString("yyyy"));
            int iEYear = Convert.ToInt32(System.DateTime.Now.ToString("yyyy"));

            for (int i = iSYear; i <= iEYear; i++) {
                ddlYear.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            for (int i = 1; i <= 12; i++) {

                string strValue = string.Empty;

                if (i < 10)
                {
                    strValue = "0" + i.ToString();
                }
                else {
                    strValue = i.ToString();
                }

                ddlMonth.Items.Add(new ListItem(strValue, strValue));
            }


            ddlYear.SelectedValue = iEYear.ToString();
            ddlMonth.SelectedValue = System.DateTime.Now.ToString("MM");



        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            // 등록일경우
            lbWorkName.Text = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language); // 등록
        }
        #endregion
        
        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (FileUpload1.PostedFile.FileName == "")
            {
                string strScript = string.Empty;

                strScript = " alert('파일을 선택하세요.'); ";
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), strScript, true);
            }
            else {
                string PPT_Path = System.Configuration.ConfigurationManager.AppSettings.Get("PPT_PATH");
                string FolderNM = ddlYear.SelectedValue + ddlMonth.SelectedValue;
                string FilePath = PPT_Path + FolderNM;

                if (!Directory.Exists(FilePath))
                {
                    Directory.CreateDirectory(FilePath);
                }

                string strNewName = string.Empty;
                string strExt = string.Empty;
                string strChgFileName = string.Empty;
                string strValName = string.Empty;
                
                strNewName = Guid.NewGuid().ToString().Replace("-", "");
                strExt = Path.GetExtension(FileUpload1.PostedFile.FileName);
                strChgFileName = strNewName + strExt;

                FileUpload1.PostedFile.SaveAs(FilePath + "\\" + strChgFileName);

                string strUploadFilePath = FilePath + "\\" + strChgFileName;

                try
                {
                    //Create PPT document
                    Presentation presentation = new Presentation();
                    presentation.LoadFromFile(strUploadFilePath);

                    int iSlides = 0;
                    
                    string strFile = string.Empty;

                    iSlides = presentation.Slides.Count;
                    
                    double iNum = Math.Ceiling((double)iSlides / 3);

                    int iTCnt = 0;

                    for (int k = 0; k < iNum; k++) {

                        int iCnt = 0;

                        int iLoop = 3;

                        if (strFile == "")
                        {
                            if (presentation.Slides.Count < 3)
                            {
                                iLoop = presentation.Slides.Count;
                            }
                            
                            for (int i = 0; i < iLoop; i++)
                            {
                                String fileName = String.Format(FilePath + "\\" + "ToImage-img-{0}.png", iCnt);
                                System.Drawing.Image image = presentation.Slides[i].SaveAsImage(1280, 400);
                                image.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);

                                iCnt++;
                                iTCnt++;

                                if (iCnt == 2)
                                {
                                    for (int j = 0; j < iLoop; j++)
                                    {
                                        if (j == 0)
                                        {
                                            strFile = DeleteSlide(strUploadFilePath, 0);
                                        }
                                        else
                                        {
                                            strFile = DeleteSlide(strFile, 0);
                                        }
                                    }
                                }
                            }
                        }
                        else {
                            presentation.LoadFromFile(strFile);
                            
                            if (presentation.Slides.Count < 3) {
                                iLoop = presentation.Slides.Count;
                            }
                            
                            for (int i = 0; i < iLoop; i++)
                            {
                                String fileName = String.Format(FilePath + "\\" + "ToImage-img-{0}.png", iTCnt);
                                System.Drawing.Image image = presentation.Slides[i].SaveAsImage(1280, 400);
                                image.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);

                                iCnt++;
                                iTCnt++;

                                if (iCnt == 2)
                                {
                                    for (int j = 0; j < iLoop; j++)
                                    {
                                        strFile = DeleteSlide(strFile, 0);
                                    }
                                }
                            }
                        }

                        if (k == (iNum - 1)) {
                            File.Delete(strFile);
                        }
                    }

                    File.Delete(strUploadFilePath);
                }
                catch {
                    File.Delete(strUploadFilePath);
                }
            }
        }
        #endregion

        #region DeleteSlide
        public string DeleteSlide(string presentationFile, int slideIndex)
        {
            string PPT_Path = System.Configuration.ConfigurationManager.AppSettings.Get("PPT_PATH");
            string FolderNM = ddlYear.SelectedValue + ddlMonth.SelectedValue;
            string FilePath = PPT_Path + FolderNM;
            string strNewName = string.Empty;

            //Get PPT document
            Presentation presentation = new Presentation();
            presentation.LoadFromFile(presentationFile);
            
            presentation.Slides.RemoveAt(slideIndex);

            strNewName = FilePath + "\\" + "Temp.pptx";
            
            presentation.SaveToFile(strNewName, FileFormat.Pptx2010);
            
            return strNewName;
        }
        #endregion
    }
}
