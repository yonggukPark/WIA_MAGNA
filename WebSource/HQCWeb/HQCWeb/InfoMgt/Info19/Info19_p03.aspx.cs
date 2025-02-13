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
using System.IO;

namespace HQCWeb.InfoMgt.Info19
{
    public partial class Info19_p03 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        #region 이미지, 파일함수변수(템플릿 미포함)
        private HtmlGenericControl[] divContainer = null;//이미지 컨테이너 변수 divContainer
        static private int[] m_Top = null;
        static private int[] m_Left = null;
        private string[] m_Key = null;
        private static HttpPostedFile filePosted;
        private static bool img_uploaded_flag = false;
        private static bool point_updated_flag = false;
        private static byte[] img_update = null;
        private static int pCnt = 0;
        #endregion

        // 비지니스 클래스 작성
        Biz.InfoManagement.Info19 biz = new Biz.InfoManagement.Info19();

        protected string strVal = string.Empty;

        // 암복호화 키값 셋팅
        public string strKey = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            cy.Key = strKey;

            SetDiv();

            if (!IsPostBack)
            {
                SetCon();

                SetTitle();

                if (Request.Form["hidValue"] != null)
                {
                    strVal = Request.Form["hidValue"].ToString();

                    (Master.FindControl("hidPopValue") as HiddenField).Value = strVal;

                    GetData();
                    GetImage("1");
                }
            }
            ScriptManager.RegisterStartupScript(Page, GetType(), Guid.NewGuid().ToString(), "enableDrag();", true);//드래그 활성화
        }
        #endregion

        #region SetDiv
        private void SetDiv()
        {
            DataSet ds = new DataSet();

            int cnt = 100; //포인터 개수

            //초기 로딩
            if (Request.Form["hidValue"] != null && pCnt == 0)
            {
                string strPVal = (strVal == string.Empty ? Request.Form["hidValue"].ToString() : strVal);
                string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

                strDBName = "GPDB";
                strQueryID = "Info19Data.Get_WorkInfo";

                FW.Data.Parameters sParam = new FW.Data.Parameters();
                sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                sParam.Add("LINE_CD", strSplitValue[2].ToString());
                sParam.Add("STN_CD", strSplitValue[3].ToString());
                sParam.Add("CAR_TYPE", strSplitValue[4].ToString());
                sParam.Add("WORK_SEQ", strSplitValue[5].ToString());

                sParam.Add("CUR_MENU_ID", "Info19");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                // 이미지 비지니스 메서드 호출
                ds = biz.GetDataSet(strDBName, strQueryID, sParam);

                if (ds.Tables.Count > 0)
                    pCnt = Convert.ToInt32(ds.Tables[0].Rows[0]["CNT"].ToString());
            }

            cnt = pCnt;

            //포인터 컨테이너 세팅
            divContainer = new HtmlGenericControl[cnt];
            m_Key = new string[cnt];
            for (int i = 0; i < cnt; i++)
            {
                divContainer[i] = new HtmlGenericControl("div");
                divContainer[i].Style.Add("position", "absolute");
                divContainer[i].Style.Add("top", "0px"); // = new System.Drawing.Point(i * 70, 0);
                divContainer[i].Style.Add("left", (i * 70).ToString() + "px");
                divContainer[i].Style.Add("z-index", "3");
                divContainer[i].Attributes.Add("class", "drag");
                divContainer[i].Style.Add("width", "35px");
                divContainer[i].Style.Add("height", "35px");
                divContainer[i].Style.Add("text-align", "center");
                divContainer[i].Style.Add("background", "#fcfcfc");
                divContainer[i].Style.Add("color", "#353535");
                divContainer[i].Style.Add("cursor", "pointer");
                divContainer[i].Style.Add("line-height", "305%");
                divContainer[i].Style.Add("border-top", "1px solid #d7d7d7");
                divContainer[i].Style.Add("border-bottom", "1px solid #d7d7d7");
                divContainer[i].Style.Add("border-left", "1px solid #d7d7d7");
                divContainer[i].Style.Add("border-right", "1px solid #d7d7d7");
                divContainer[i].Style.Add("border-radius", "6px");
                divContainer[i].ID = (i + 1).ToString();
                m_Key[i] = (i + 1).ToString();
                divContainer[i].InnerText = (i + 1).ToString();
                divContainer[i].Visible = false;

                this.divImg.Controls.Add(divContainer[i]);
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            filePosted = null;
            m_Top = new int[100];
            m_Left = new int[100];
            img_update = null;
            img_uploaded_flag = false;
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbCarType.Text = Dictionary_Data.SearchDic("CAR_TYPE", bp.g_language);
            lbStnCd.Text = Dictionary_Data.SearchDic("STN_CD", bp.g_language);
            lbWorkSeq.Text = Dictionary_Data.SearchDic("WORK_SEQ", bp.g_language);
            //lbImgFile.Text = Dictionary_Data.SearchDic("IMG_FILE", bp.g_language);
            //lbImgMain.Text = Dictionary_Data.SearchDic("IMG_MAIN", bp.g_language);

            // 등록일경우
            lbWorkName.Text = Dictionary_Data.SearchDic("IMAGE_REGISTRATION", bp.g_language); // 등록
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
            //HQCWeb.Biz.Sample_Biz biz = new HQCWeb.Biz.Sample_Biz();
            string strDetailValue = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Info19Data.Get_WorkInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("STN_CD", strSplitValue[3].ToString());
            sParam.Add("CAR_TYPE", strSplitValue[4].ToString());
            sParam.Add("WORK_SEQ", strSplitValue[5].ToString());

            sParam.Add("CUR_MENU_ID", "Info19");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 이미지 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                //GetData 에서 호출(STN 콤보 초기 설정값 필요)
                GetDDL(ds.Tables[0].Rows[0]["SHOP_CD"].ToString(), ds.Tables[0].Rows[0]["LINE_CD"].ToString(), ds.Tables[0].Rows[0]["CAR_TYPE"].ToString());
                ddlStnCd.SelectedValue = ds.Tables[0].Rows[0]["STN_CD"].ToString();
                lbGetWorkSeq.Text = ds.Tables[0].Rows[0]["WORK_SEQ"].ToString();
                
                ddlShopCd.Enabled = false;
                ddlLineCd.Enabled = false;
                ddlCarType.Enabled = false;
                ddlStnCd.Enabled = false;

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
                strScript = " alert('정보가 존재하지 않습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Info19'); parent.fn_ModalCloseDiv(); ";
                ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
            }
        }
        #endregion

        #region btnSave_Click
        protected void btnSave_Click(object sender, EventArgs e)
        {
            int iRtn = 0;
            string strScript = string.Empty;
            string strPoint = string.Empty;
            
            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            string strUVal = (Master.FindControl("hidPopDefaultValue") as HiddenField).Value;

            string strUValue = cy.Decrypt(strUVal);

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))//입력 스크립트 취약점 체크
            {

                if (img_update == null)//이미지 데이터 등록 필요
                {
                    strScript = " alert('이미지가 등록되지 않았습니다. 이미지를 등록해주세요.'); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
                else
                {
                    if (point_updated_flag)
                    {
                        strPoint = "<items>";
                        for (int i = 0; i < divContainer.Length; i++)
                        {
                            if (divContainer[i].Visible)
                            {
                                strPoint += "<item><value>" + (i + 1) + "</value><value>" + GetItemValue(divContainer[i].ID, "left") + "</value><value>" + GetItemValue(divContainer[i].ID, "top") + "</value></item>";
                            }
                            else
                                break;
                        }
                        strPoint += "</items>";
                    }

                    strDBName = "GPDB";
                    strQueryID = "Info19Data.Set_WorkImage";//이미지 및 좌표값 저장

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("PLANT_CD", strSplitValue[0].ToString());
                    sParam.Add("SHOP_CD", strSplitValue[1].ToString());
                    sParam.Add("LINE_CD", strSplitValue[2].ToString());
                    sParam.Add("STN_CD", strSplitValue[3].ToString());
                    sParam.Add("CAR_TYPE", strSplitValue[4].ToString());
                    sParam.Add("WORK_SEQ", strSplitValue[5].ToString());
                    
                    sParam.Add("FILE_DATA_UPT", img_uploaded_flag.Equals(true) ? "T" : "F");
                    sParam.Add("FILE_DATA", img_update);
                    sParam.Add("POINT_UPT", point_updated_flag.Equals(true) ? "T" : "F");
                    sParam.Add("POINT", strPoint);
                    sParam.Add("USER_ID", bp.g_userid.ToString());

                    sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                    sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                    sParam.Add("CUR_MENU_ID", "Info19");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요
                    sParam.Add("PREV_DATA", strUValue);                  // 이전 데이터 셋팅

                    iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                    if (iRtn == 1)//다중 UPDATE 발생하여, 리턴값 조정
                    {
                        strScript = " alert('정상등록 되었습니다.');";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
                    else
                    {
                        strScript = " alert('등록에 실패하였습니다. 관리자에게 문의바립니다.'); ";
                        ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    }
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
        protected void GetDDL(string shopCd, string lineCd, string carType)
        {
            //GetData 에서 호출(STN 콤보 초기 설정값 필요)
            DataSet ds = new DataSet();

            ddlShopCd.Items.Add(new ListItem("", ""));
            ddlLineCd.Items.Add(new ListItem("", ""));
            ddlCarType.Items.Add(new ListItem("", ""));
            ddlStnCd.Items.Add(new ListItem("", ""));

            strDBName = "GPDB";
            strQueryID = "Info19Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", shopCd);
            param.Add("LINE_CD", lineCd);
            param.Add("CAR_TYPE", carType);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //shop code
                if (ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        ddlShopCd.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlShopCd.SelectedValue = shopCd;
                }

                //line code
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ddlLineCd.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlLineCd.SelectedValue = lineCd;
                }

                //car type
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarType.SelectedValue = carType;
                }

                //stn code
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        ddlStnCd.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                    }
                }
            }
        }
        #endregion

        #region GetImage() - 이미지 및 포인트 불러오기
        private void GetImage(string sType)
        {
            string strScript = string.Empty;
            byte[] aa = null;
            int aaLan = 0;

            DataSet ds = new DataSet();

            strDBName = "GPDB";
            strQueryID = "Info19Data.Get_WorkImage";

            string strPVal = (Master.FindControl("hidPopValue") as HiddenField).Value;

            string[] strSplitValue = cy.Decrypt(strPVal).Split('/');

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", strSplitValue[0].ToString());
            sParam.Add("SHOP_CD", strSplitValue[1].ToString());
            sParam.Add("LINE_CD", strSplitValue[2].ToString());
            sParam.Add("STN_CD", strSplitValue[3].ToString());
            sParam.Add("CAR_TYPE", strSplitValue[4].ToString());
            sParam.Add("WORK_SEQ", strSplitValue[5].ToString());

            sParam.Add("CUR_MENU_ID", "Info19");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            // 이미지 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                HtmlGenericControl div = ((HtmlGenericControl)divImg.FindControl("divImg"));
                //????
                for (int i = 0; i < divContainer.Length; i++)
                {
                    divContainer[i].Visible = false;
                    divContainer[i].Style.Add("top", i.ToString() + "px");
                    divContainer[i].Style.Add("left", i.ToString() + "px");
                }
                if (ds.Tables[0].Rows.Count > 0)
                {
                    /* 이미지 길이 정보(ds.Tables[1])가 있으면 지정 */
                    if (ds.Tables[1].Rows.Count > 0)
                    {
                        string strWidth = ds.Tables[1].Rows[0]["WIDTH"].ToString() + "px";
                        string strHeight = ds.Tables[1].Rows[0]["HEIGHT"].ToString() + "px";

                        div.Style.Add("width", strWidth);
                        div.Style.Add("height", strHeight);
                        image1.Style.Add("width", strWidth);
                        image1.Style.Add("height", strHeight);
                    }

                    /*이미지 불러오기*/
                    if (sType.Equals("2"))//이미지 업로드된 경우
                    {
                        aa = img_update;
                        aaLan = img_update.Length;
                    }
                    else if(ds.Tables[0].Rows[0]["TOOL_IMG"].ToString().Length > 0)//초기 업로드인 경우
                    {
                        aa = (byte[])(ds.Tables[0].Rows[0]["TOOL_IMG"]);
                        img_update = aa;
                        aaLan = aa.Length;
                    }
                     

                    if (aaLan != 0)
                    {
                        if (aa[0] != 0)
                        {
                            string base64String = Convert.ToBase64String(aa, 0, aa.Length);
                            this.image1.Attributes.Add("src", "data:image/png;base64," + base64String);

                        }
                    }
                    else
                    {
                        this.image1.Attributes.Add("src", null);
                    }

                    /* 좌표 활성화 */
                    int cnt = ds.Tables[0].Rows[0]["CNT"].ToString().Length > 0 ? (int)ds.Tables[0].Rows[0]["CNT"] : 0;

                    for (int i = 0; i < cnt; i++)
                    {
                        divContainer[i].Visible = true;
                    }

                    /*포인트 지정*/
                    if (ds.Tables[2].Rows.Count > 0)
                    {
                        for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                        {

                            divContainer[i].Style.Add("left", ds.Tables[2].Rows[i]["POINT_X"].ToString() + "px");
                            divContainer[i].Style.Add("top", ds.Tables[2].Rows[i]["POINT_Y"].ToString() + "px");
                            m_Top[i] = int.Parse(ds.Tables[2].Rows[i]["POINT_Y"].ToString());
                            m_Left[i] = int.Parse(ds.Tables[2].Rows[i]["POINT_X"].ToString());

                        }

                    }
                    else
                    {
                        for (int i = 0; i < divContainer.Length; i++)
                        {
                            divContainer[i].Style.Add("top", i.ToString() + "px");
                            divContainer[i].Style.Add("left", i.ToString() + "px");
                        }
                    }
                }
                else
                {
                    strScript = " alert('이미지 로드 중 오류가 발생했습니다. 관리자에게 문의바립니다.'); parent.fn_ModalReloadCall('Info19'); parent.fn_ModalCloseDiv(); ";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                }
            }

        }
        #endregion

        #region btn1_Click - 이미지 미리보기
        protected void btn1_Click(object sender, EventArgs e)
        {
            string strScript = string.Empty;

            DataSet ds = new DataSet();

            try
            {
                //filePosted = Request.Files["PopupContent_fileupload"];
                filePosted = Request.Files[0]; //파일업로드 1개이므로

                //불러온 파일 확장자 확인
                string filePath = Path.GetFileName(filePosted.FileName); //파일이름.확장자
                string[] code2 = filePath.Split('.');

                if (!fileTypeCheck(code2[1]))
                {
                    strScript = " alert('jpg / png / gif 파일만 업로드 가능합니다.');";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    return;
                }

                //이미지 용량 체크
                if (fileSizeCheck(filePosted.ContentLength))
                {
                    strScript = " alert('이미지 파일의 용량 제한은 2MB 입니다. 파일을 확인해주세요.');";
                    ClientScript.RegisterStartupScript(typeof(string), Guid.NewGuid().ToString(), strScript, true);
                    return;
                }

                if (filePosted != null)
                {
                    //파일데이터 바이너리로 변경
                    using (var binaryReader = new BinaryReader(filePosted.InputStream))
                    {
                        img_update = binaryReader.ReadBytes(filePosted.ContentLength);
                        binaryReader.Close();
                    }
                    img_uploaded_flag = true;//업데이트됨
                    GetImage("2");//신규 이미지로 갈아끼우기

                }

            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region btn2_Click - drag&drop 이후 포인트 저장 함수 call
        protected void btn2_Click(object sender, EventArgs e)
        {
            try
            {
                GetProducts();

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region GetProducts() - drag&drop 이후 포인트 저장
        public void GetProducts()
        {
            try
            {
                if (hidPointID.Value != string.Empty)
                {
                    string index = hidPointID.Value.Replace("PopupContent_", "");//asp:Content에 감싸지므로 ID 추출시 빼기
                    HtmlGenericControl div = ((HtmlGenericControl)divImg.FindControl(index));
                    div.Style.Add("top", hidPointTop.Value);
                    div.Style.Add("left", hidPointLeft.Value);
                    for (int i = 0; i < divContainer.Length; i++)
                    {
                        if (m_Key[i] == index)
                        {
                            string sTop = hidPointTop.Value.Replace("px", "").Trim();
                            string sLeft = hidPointLeft.Value.Replace("px", "").Trim();
                            Double dTop = Convert.ToDouble(sTop);
                            Double dLeft = Convert.ToDouble(sLeft);
                            m_Top[i] = (int)dTop;
                            m_Left[i] = (int)dLeft;
                        }
                    }

                    point_updated_flag = true;
                }

            }
            catch (Exception)
            {

                throw;
            }

        }
        #endregion

        #region 업로드file 사이즈 체크
        protected bool fileSizeCheck(int fileSize)
        {
            //filesize 2MB 체크
            return (fileSize > 2 * 1024 * 1024);
        }

        #endregion

        #region 업로드file 확장자 체크
        protected bool fileTypeCheck(string sFileType)
        {
            string fileType = sFileType.ToLower();

            if (fileType == "gif" || fileType == "jpg" || fileType == "jpeg" || fileType == "png")
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        #endregion

        #region GetItemValue() - 포인트 좌표값 저장
        public int GetItemValue(string sId, string sValue)
        {
            int iItemsValue = 0;
            try
            {
                for (int i = 0; i < divContainer.Length; i++)
                {
                    if (m_Key[i] == sId)
                    {
                        if (sValue == "left")
                        {
                            iItemsValue = m_Left[i];
                        }
                        else if (sValue == "top")
                        {
                            iItemsValue = m_Top[i];
                        }



                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return iItemsValue;
        }
        #endregion

    }
}
