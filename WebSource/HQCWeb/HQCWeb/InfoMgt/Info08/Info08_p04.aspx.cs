using HQCWeb.FMB_FW;
using HQCWeb.FW;
using MES.FW.Common.CommonMgt;
using MES.FW.Common.Crypt;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HQCWeb.InfoMgt.Info08
{
    public partial class Info08_p04 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strError = string.Empty;

        protected string strVal = string.Empty;

        // 암복호화 키값 셋팅
        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        // 비지니스 클래스 작성
        Biz.InfoManagement.Info08 biz = new Biz.InfoManagement.Info08();

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
            lbWorkName.Text = Dictionary_Data.SearchDic("BARCODE_DISPLAY", bp.g_language); // 바코드 표시

            //lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            //lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            //lbCarType.Text = Dictionary_Data.SearchDic("CAR_TYPE", bp.g_language);
            //lbDevId.Text = Dictionary_Data.SearchDic("PRINT_ID", bp.g_language);
            //lbDevKind.Text = Dictionary_Data.SearchDic("PRINT_KIND_ID", bp.g_language);
            //lbType.Text = Dictionary_Data.SearchDic("TYPE", bp.g_language);
            lbZpl.Text = Dictionary_Data.SearchDic("ZPL", bp.g_language);

        }
        #endregion

        #region GetData
        protected void GetData()
        {
            string strScript = string.Empty;
            string strTableName = string.Empty;
            string strErrMessage = string.Empty;
            string base64Image = string.Empty;

            DataSet ds = new DataSet();

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Info08Data.Get_PrintBarcode";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("CAR_TYPE", strSplitValue[3].ToString());
            sParam.Add("DEV_ID", strSplitValue[5].ToString());
            sParam.Add("DEV_KIND", strSplitValue[6].ToString());

            sParam.Add("CUR_MENU_ID", "Info08");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
                    //GetDDL(strSplitValue[1].ToString(), strSplitValue[2].ToString(), strSplitValue[5].ToString());
                    //ddlCarType.SelectedValue = strSplitValue[3].ToString();
                    //ddlDevKind.SelectedValue = strSplitValue[6].ToString();
                    //ddlType.SelectedValue = strSplitValue[7].ToString();
                    lbGetZpl.Text = ds.Tables[0].Rows[0]["PRINT"].ToString();

                    //ddlShopCd.Enabled = false;
                    //ddlLineCd.Enabled = false;
                    //ddlCarType.Enabled = false;
                    //ddlDevId.Enabled = false;
                    //ddlDevKind.Enabled = false;
                    //ddlType.Enabled = false;

                    base64Image = GetLabelImageBase64(lbGetZpl.Text); //Labalary Viewer 요청하여 Image 소스 string 생성

                    if (!string.IsNullOrEmpty(base64Image))
                    {
                        imgBarcode.Src = "data:image/png;base64," + base64Image;
                    }
                    else
                    {
                        strScript = " alert('"+strError+"'); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    
                }
            }
            else
            {
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Info08'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region GetDDL
        //protected void GetDDL(string shopCd, string lineCd, string devId)
        //{
        //    //GetData 에서 호출(LINE 콤보 초기 설정값 필요)
        //    DataSet ds = new DataSet();

        //    ddlShopCd.Items.Add(new ListItem("선택하세요", ""));
        //    ddlLineCd.Items.Add(new ListItem("선택하세요", ""));

        //    strDBName = "GPDB";
        //    strQueryID = "Info08Data.Get_DdlData";

        //    FW.Data.Parameters param = new FW.Data.Parameters();
        //    param.Add("PLANT_CD", bp.g_plant.ToString());
        //    param.Add("SHOP_CD", shopCd);
        //    param.Add("LINE_CD", lineCd);
        //    param.Add("DEV_ID", devId);

        //    // 비지니스 메서드 호출(다중테이블 함수)
        //    ds = biz.GetDataSet(strDBName, strQueryID, param);

        //    if (ds.Tables.Count > 0)
        //    {
        //        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
        //        {
        //            ddlShopCd.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
        //        }
        //        ddlShopCd.SelectedValue = shopCd;
        //        for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
        //        {
        //            ddlLineCd.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
        //        }
        //        ddlLineCd.SelectedValue = lineCd;
        //        for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
        //        {
        //            ddlCarType.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
        //        }
        //        for (int i = 0; i < ds.Tables[5].Rows.Count; i++)
        //        {
        //            ddlDevId.Items.Add(new ListItem(ds.Tables[5].Rows[i]["CODE_NM"].ToString(), ds.Tables[5].Rows[i]["CODE_ID"].ToString()));
        //        }
        //        ddlDevId.SelectedValue = devId;
        //        for (int i = 0; i < ds.Tables[6].Rows.Count; i++)
        //        {
        //            ddlDevKind.Items.Add(new ListItem(ds.Tables[6].Rows[i]["CODE_NM"].ToString(), ds.Tables[6].Rows[i]["CODE_ID"].ToString()));
        //        }
        //        for (int i = 0; i < ds.Tables[7].Rows.Count; i++)
        //        {
        //            ddlType.Items.Add(new ListItem(ds.Tables[7].Rows[i]["CODE_NM"].ToString(), ds.Tables[7].Rows[i]["CODE_ID"].ToString()));
        //        }
        //    }
        //}
        #endregion

        #region GetLabelImageBase64
        private string GetLabelImageBase64(string zplCode)
        {
            byte[] zpl = Encoding.UTF8.GetBytes(zplCode);

            var request = (HttpWebRequest)WebRequest.Create("http://api.labelary.com/v1/printers/8dpmm/labels/4x6/0/");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = zpl.Length;
            
            try
            {
                var requestStream = request.GetRequestStream();
                requestStream.Write(zpl, 0, zpl.Length);
                requestStream.Close();
            }
            catch (WebException e)
            {
                Console.WriteLine("Error: {0}", e.Status);
                strError = "Error: " + e.Message;
                return null;
            }

            try
            {
                var response = (HttpWebResponse)request.GetResponse();
                using (var responseStream = response.GetResponseStream())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        responseStream.CopyTo(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();
                        return Convert.ToBase64String(imageBytes);
                    }
                }
            }
            catch (WebException e)
            {
                Console.WriteLine("Error: {0}", e.Status);
                strError = "Error: " + e.Message;
                return null;
            }
        }
        #endregion
    }
}
