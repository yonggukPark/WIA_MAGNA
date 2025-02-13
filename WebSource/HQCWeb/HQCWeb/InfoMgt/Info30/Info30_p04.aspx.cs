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

namespace HQCWeb.InfoMgt.Info30
{
    public partial class Info30_p04 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        Crypt cy = new Crypt();
        StringUtil su = new StringUtil();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;

        //포맷 그리드 생성용
        string jsField = string.Empty;
        string jsCol = string.Empty;

        Biz.InfoManagement.Info30 biz = new Biz.InfoManagement.Info30();
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
            string[] columnCols = { "PLANT_CD", "SHOP_CD", "LINE_CD", "DEV_ID", "CAR_TYPE", "SPEC_001", "SPEC_002", "SPEC_003", "SPEC_004", "SPEC_005", "SPEC_006", "SPEC_007", "SPEC_008", "SPEC_009", "SPEC_010", "SPEC_011", "SPEC_012", "SPEC_013", "SPEC_014", "SPEC_015", "SPEC_016", "SPEC_017", "SPEC_018", "SPEC_019", "SPEC_020", "SPEC_021", "SPEC_022", "SPEC_023", "SPEC_024", "SPEC_025", "SPEC_026", "SPEC_027", "SPEC_028", "SPEC_029", "SPEC_030", "SPEC_031", "SPEC_032", "SPEC_033", "SPEC_034", "SPEC_035", "SPEC_036", "SPEC_037", "SPEC_038", "SPEC_039", "SPEC_040", "SPEC_041", "SPEC_042", "SPEC_043", "SPEC_044", "SPEC_045", "SPEC_046", "SPEC_047", "SPEC_048", "SPEC_049", "SPEC_050", "SPEC_051", "SPEC_052", "SPEC_053", "SPEC_054", "SPEC_055", "SPEC_056", "SPEC_057", "SPEC_058", "SPEC_059", "SPEC_060", "SPEC_061", "SPEC_062", "SPEC_063", "SPEC_064", "SPEC_065", "SPEC_066", "SPEC_067", "SPEC_068", "SPEC_069", "SPEC_070", "SPEC_071", "SPEC_072", "SPEC_073", "SPEC_074", "SPEC_075", "SPEC_076", "SPEC_077", "SPEC_078", "SPEC_079", "SPEC_080", "SPEC_081", "SPEC_082", "SPEC_083", "SPEC_084", "SPEC_085", "SPEC_086", "SPEC_087", "SPEC_088", "SPEC_089", "SPEC_090", "SPEC_091", "SPEC_092", "SPEC_093", "SPEC_094", "SPEC_095", "SPEC_096", "SPEC_097", "SPEC_098", "SPEC_099", "SPEC_100", "SPEC_101", "SPEC_102", "SPEC_103", "SPEC_104", "SPEC_105", "SPEC_106", "SPEC_107", "SPEC_108", "SPEC_109", "SPEC_110", "SPEC_111", "SPEC_112", "SPEC_113", "SPEC_114", "SPEC_115", "SPEC_116", "SPEC_117", "SPEC_118", "SPEC_119", "SPEC_120", "SPEC_121", "SPEC_122", "SPEC_123", "SPEC_124", "SPEC_125", "SPEC_126", "SPEC_127", "SPEC_128", "SPEC_129", "SPEC_130", "SPEC_131", "SPEC_132", "SPEC_133", "SPEC_134", "SPEC_135", "SPEC_136", "SPEC_137", "SPEC_138", "SPEC_139", "SPEC_140", "SPEC_141", "SPEC_142", "SPEC_143", "SPEC_144", "SPEC_145", "SPEC_146", "SPEC_147", "SPEC_148", "SPEC_149", "SPEC_150", "SPEC_151", "SPEC_152", "SPEC_153", "SPEC_154", "SPEC_155", "SPEC_156", "SPEC_157", "SPEC_158", "SPEC_159", "SPEC_160", "SPEC_161", "SPEC_162", "SPEC_163", "SPEC_164", "SPEC_165", "SPEC_166", "SPEC_167", "SPEC_168", "SPEC_169", "SPEC_170", "SPEC_171", "SPEC_172", "SPEC_173", "SPEC_174", "SPEC_175", "SPEC_176", "SPEC_177", "SPEC_178", "SPEC_179", "SPEC_180", "SPEC_181", "SPEC_182", "SPEC_183", "SPEC_184", "SPEC_185", "SPEC_186", "SPEC_187", "SPEC_188", "SPEC_189", "SPEC_190", "SPEC_191", "SPEC_192", "SPEC_193", "SPEC_194", "SPEC_195", "SPEC_196", "SPEC_197", "SPEC_198", "SPEC_199", "SPEC_200", "SPEC_201", "SPEC_202", "SPEC_203", "SPEC_204", "SPEC_205", "SPEC_206", "SPEC_207", "SPEC_208", "SPEC_209", "SPEC_210", "SPEC_211", "SPEC_212", "SPEC_213", "SPEC_214", "SPEC_215", "SPEC_216", "SPEC_217", "SPEC_218", "SPEC_219", "SPEC_220", "SPEC_221", "SPEC_222", "SPEC_223", "SPEC_224", "SPEC_225", "SPEC_226", "SPEC_227", "SPEC_228", "SPEC_229", "SPEC_230", "SPEC_231", "SPEC_232", "SPEC_233", "SPEC_234", "SPEC_235", "SPEC_236", "SPEC_237", "SPEC_238", "SPEC_239", "SPEC_240", "SPEC_241", "SPEC_242", "SPEC_243", "SPEC_244", "SPEC_245", "SPEC_246", "SPEC_247", "SPEC_248", "SPEC_249", "SPEC_250", "SPEC_251", "SPEC_252", "SPEC_253", "SPEC_254", "SPEC_255", "SPEC_256", "SPEC_257", "SPEC_258", "SPEC_259", "SPEC_260", "SPEC_261", "SPEC_262", "SPEC_263", "SPEC_264", "SPEC_265", "SPEC_266", "SPEC_267", "SPEC_268", "SPEC_269", "SPEC_270", "SPEC_271", "SPEC_272", "SPEC_273", "SPEC_274", "SPEC_275", "SPEC_276", "SPEC_277", "SPEC_278", "SPEC_279", "SPEC_280", "SPEC_281", "SPEC_282", "SPEC_283", "SPEC_284", "SPEC_285", "SPEC_286", "SPEC_287", "SPEC_288", "SPEC_289", "SPEC_290", "SPEC_291", "SPEC_292", "SPEC_293", "SPEC_294", "SPEC_295", "SPEC_296", "SPEC_297", "SPEC_298", "SPEC_299", "SPEC_300" };

            //길이 설정
            string[] arrColumnWidth = new string[columnCols.Length];

            //캡션 설정
            string[] columnNames = new string[columnCols.Length];

            for (int i = 0; i < columnNames.Length; i++)
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
                            strQueryID = "Info30Data.Set_MasterInfo";

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
                                    sParam.Add("SHOP_CD", ExcelDataSet.Tables[0].Rows[i][1].ToString());
                                    sParam.Add("LINE_CD", ExcelDataSet.Tables[0].Rows[i][2].ToString());
                                    sParam.Add("DEV_ID", ExcelDataSet.Tables[0].Rows[i][3].ToString());
                                    sParam.Add("CAR_TYPE", ExcelDataSet.Tables[0].Rows[i][4].ToString());

                                    sParam.Add("ITEM_01", ExcelDataSet.Tables[0].Rows[i][5].ToString());
                                    sParam.Add("ITEM_02", ExcelDataSet.Tables[0].Rows[i][6].ToString());
                                    sParam.Add("ITEM_03", ExcelDataSet.Tables[0].Rows[i][7].ToString());
                                    sParam.Add("ITEM_04", ExcelDataSet.Tables[0].Rows[i][8].ToString());
                                    sParam.Add("ITEM_05", ExcelDataSet.Tables[0].Rows[i][9].ToString());
                                    sParam.Add("ITEM_06", ExcelDataSet.Tables[0].Rows[i][10].ToString());
                                    sParam.Add("ITEM_07", ExcelDataSet.Tables[0].Rows[i][11].ToString());
                                    sParam.Add("ITEM_08", ExcelDataSet.Tables[0].Rows[i][12].ToString());
                                    sParam.Add("ITEM_09", ExcelDataSet.Tables[0].Rows[i][13].ToString());
                                    sParam.Add("ITEM_10", ExcelDataSet.Tables[0].Rows[i][14].ToString());
                                    sParam.Add("ITEM_11", ExcelDataSet.Tables[0].Rows[i][15].ToString());
                                    sParam.Add("ITEM_12", ExcelDataSet.Tables[0].Rows[i][16].ToString());
                                    sParam.Add("ITEM_13", ExcelDataSet.Tables[0].Rows[i][17].ToString());
                                    sParam.Add("ITEM_14", ExcelDataSet.Tables[0].Rows[i][18].ToString());
                                    sParam.Add("ITEM_15", ExcelDataSet.Tables[0].Rows[i][19].ToString());
                                    sParam.Add("ITEM_16", ExcelDataSet.Tables[0].Rows[i][20].ToString());
                                    sParam.Add("ITEM_17", ExcelDataSet.Tables[0].Rows[i][21].ToString());
                                    sParam.Add("ITEM_18", ExcelDataSet.Tables[0].Rows[i][22].ToString());
                                    sParam.Add("ITEM_19", ExcelDataSet.Tables[0].Rows[i][23].ToString());
                                    sParam.Add("ITEM_20", ExcelDataSet.Tables[0].Rows[i][24].ToString());
                                    sParam.Add("ITEM_21", ExcelDataSet.Tables[0].Rows[i][25].ToString());
                                    sParam.Add("ITEM_22", ExcelDataSet.Tables[0].Rows[i][26].ToString());
                                    sParam.Add("ITEM_23", ExcelDataSet.Tables[0].Rows[i][27].ToString());
                                    sParam.Add("ITEM_24", ExcelDataSet.Tables[0].Rows[i][28].ToString());
                                    sParam.Add("ITEM_25", ExcelDataSet.Tables[0].Rows[i][29].ToString());
                                    sParam.Add("ITEM_26", ExcelDataSet.Tables[0].Rows[i][30].ToString());
                                    sParam.Add("ITEM_27", ExcelDataSet.Tables[0].Rows[i][31].ToString());
                                    sParam.Add("ITEM_28", ExcelDataSet.Tables[0].Rows[i][32].ToString());
                                    sParam.Add("ITEM_29", ExcelDataSet.Tables[0].Rows[i][33].ToString());
                                    sParam.Add("ITEM_30", ExcelDataSet.Tables[0].Rows[i][34].ToString());
                                    sParam.Add("ITEM_31", ExcelDataSet.Tables[0].Rows[i][35].ToString());
                                    sParam.Add("ITEM_32", ExcelDataSet.Tables[0].Rows[i][36].ToString());
                                    sParam.Add("ITEM_33", ExcelDataSet.Tables[0].Rows[i][37].ToString());
                                    sParam.Add("ITEM_34", ExcelDataSet.Tables[0].Rows[i][38].ToString());
                                    sParam.Add("ITEM_35", ExcelDataSet.Tables[0].Rows[i][39].ToString());
                                    sParam.Add("ITEM_36", ExcelDataSet.Tables[0].Rows[i][40].ToString());
                                    sParam.Add("ITEM_37", ExcelDataSet.Tables[0].Rows[i][41].ToString());
                                    sParam.Add("ITEM_38", ExcelDataSet.Tables[0].Rows[i][42].ToString());
                                    sParam.Add("ITEM_39", ExcelDataSet.Tables[0].Rows[i][43].ToString());
                                    sParam.Add("ITEM_40", ExcelDataSet.Tables[0].Rows[i][44].ToString());
                                    sParam.Add("ITEM_41", ExcelDataSet.Tables[0].Rows[i][45].ToString());
                                    sParam.Add("ITEM_42", ExcelDataSet.Tables[0].Rows[i][46].ToString());
                                    sParam.Add("ITEM_43", ExcelDataSet.Tables[0].Rows[i][47].ToString());
                                    sParam.Add("ITEM_44", ExcelDataSet.Tables[0].Rows[i][48].ToString());
                                    sParam.Add("ITEM_45", ExcelDataSet.Tables[0].Rows[i][49].ToString());
                                    sParam.Add("ITEM_46", ExcelDataSet.Tables[0].Rows[i][50].ToString());
                                    sParam.Add("ITEM_47", ExcelDataSet.Tables[0].Rows[i][51].ToString());
                                    sParam.Add("ITEM_48", ExcelDataSet.Tables[0].Rows[i][52].ToString());
                                    sParam.Add("ITEM_49", ExcelDataSet.Tables[0].Rows[i][53].ToString());
                                    sParam.Add("ITEM_50", ExcelDataSet.Tables[0].Rows[i][54].ToString());
                                    sParam.Add("ITEM_51", ExcelDataSet.Tables[0].Rows[i][55].ToString());
                                    sParam.Add("ITEM_52", ExcelDataSet.Tables[0].Rows[i][56].ToString());
                                    sParam.Add("ITEM_53", ExcelDataSet.Tables[0].Rows[i][57].ToString());
                                    sParam.Add("ITEM_54", ExcelDataSet.Tables[0].Rows[i][58].ToString());
                                    sParam.Add("ITEM_55", ExcelDataSet.Tables[0].Rows[i][59].ToString());
                                    sParam.Add("ITEM_56", ExcelDataSet.Tables[0].Rows[i][60].ToString());
                                    sParam.Add("ITEM_57", ExcelDataSet.Tables[0].Rows[i][61].ToString());
                                    sParam.Add("ITEM_58", ExcelDataSet.Tables[0].Rows[i][62].ToString());
                                    sParam.Add("ITEM_59", ExcelDataSet.Tables[0].Rows[i][63].ToString());
                                    sParam.Add("ITEM_60", ExcelDataSet.Tables[0].Rows[i][64].ToString());
                                    sParam.Add("ITEM_61", ExcelDataSet.Tables[0].Rows[i][65].ToString());
                                    sParam.Add("ITEM_62", ExcelDataSet.Tables[0].Rows[i][66].ToString());
                                    sParam.Add("ITEM_63", ExcelDataSet.Tables[0].Rows[i][67].ToString());
                                    sParam.Add("ITEM_64", ExcelDataSet.Tables[0].Rows[i][68].ToString());
                                    sParam.Add("ITEM_65", ExcelDataSet.Tables[0].Rows[i][69].ToString());
                                    sParam.Add("ITEM_66", ExcelDataSet.Tables[0].Rows[i][70].ToString());
                                    sParam.Add("ITEM_67", ExcelDataSet.Tables[0].Rows[i][71].ToString());
                                    sParam.Add("ITEM_68", ExcelDataSet.Tables[0].Rows[i][72].ToString());
                                    sParam.Add("ITEM_69", ExcelDataSet.Tables[0].Rows[i][73].ToString());
                                    sParam.Add("ITEM_70", ExcelDataSet.Tables[0].Rows[i][74].ToString());
                                    sParam.Add("ITEM_71", ExcelDataSet.Tables[0].Rows[i][75].ToString());
                                    sParam.Add("ITEM_72", ExcelDataSet.Tables[0].Rows[i][76].ToString());
                                    sParam.Add("ITEM_73", ExcelDataSet.Tables[0].Rows[i][77].ToString());
                                    sParam.Add("ITEM_74", ExcelDataSet.Tables[0].Rows[i][78].ToString());
                                    sParam.Add("ITEM_75", ExcelDataSet.Tables[0].Rows[i][79].ToString());
                                    sParam.Add("ITEM_76", ExcelDataSet.Tables[0].Rows[i][80].ToString());
                                    sParam.Add("ITEM_77", ExcelDataSet.Tables[0].Rows[i][81].ToString());
                                    sParam.Add("ITEM_78", ExcelDataSet.Tables[0].Rows[i][82].ToString());
                                    sParam.Add("ITEM_79", ExcelDataSet.Tables[0].Rows[i][83].ToString());
                                    sParam.Add("ITEM_80", ExcelDataSet.Tables[0].Rows[i][84].ToString());
                                    sParam.Add("ITEM_81", ExcelDataSet.Tables[0].Rows[i][85].ToString());
                                    sParam.Add("ITEM_82", ExcelDataSet.Tables[0].Rows[i][86].ToString());
                                    sParam.Add("ITEM_83", ExcelDataSet.Tables[0].Rows[i][87].ToString());
                                    sParam.Add("ITEM_84", ExcelDataSet.Tables[0].Rows[i][88].ToString());
                                    sParam.Add("ITEM_85", ExcelDataSet.Tables[0].Rows[i][89].ToString());
                                    sParam.Add("ITEM_86", ExcelDataSet.Tables[0].Rows[i][90].ToString());
                                    sParam.Add("ITEM_87", ExcelDataSet.Tables[0].Rows[i][91].ToString());
                                    sParam.Add("ITEM_88", ExcelDataSet.Tables[0].Rows[i][92].ToString());
                                    sParam.Add("ITEM_89", ExcelDataSet.Tables[0].Rows[i][93].ToString());
                                    sParam.Add("ITEM_90", ExcelDataSet.Tables[0].Rows[i][94].ToString());
                                    sParam.Add("ITEM_91", ExcelDataSet.Tables[0].Rows[i][95].ToString());
                                    sParam.Add("ITEM_92", ExcelDataSet.Tables[0].Rows[i][96].ToString());
                                    sParam.Add("ITEM_93", ExcelDataSet.Tables[0].Rows[i][97].ToString());
                                    sParam.Add("ITEM_94", ExcelDataSet.Tables[0].Rows[i][98].ToString());
                                    sParam.Add("ITEM_95", ExcelDataSet.Tables[0].Rows[i][99].ToString());
                                    sParam.Add("ITEM_96", ExcelDataSet.Tables[0].Rows[i][100].ToString());
                                    sParam.Add("ITEM_97", ExcelDataSet.Tables[0].Rows[i][101].ToString());
                                    sParam.Add("ITEM_98", ExcelDataSet.Tables[0].Rows[i][102].ToString());
                                    sParam.Add("ITEM_99", ExcelDataSet.Tables[0].Rows[i][103].ToString());
                                    sParam.Add("ITEM_100", ExcelDataSet.Tables[0].Rows[i][104].ToString());
                                    sParam.Add("ITEM_101", ExcelDataSet.Tables[0].Rows[i][105].ToString());
                                    sParam.Add("ITEM_102", ExcelDataSet.Tables[0].Rows[i][106].ToString());
                                    sParam.Add("ITEM_103", ExcelDataSet.Tables[0].Rows[i][107].ToString());
                                    sParam.Add("ITEM_104", ExcelDataSet.Tables[0].Rows[i][108].ToString());
                                    sParam.Add("ITEM_105", ExcelDataSet.Tables[0].Rows[i][109].ToString());
                                    sParam.Add("ITEM_106", ExcelDataSet.Tables[0].Rows[i][110].ToString());
                                    sParam.Add("ITEM_107", ExcelDataSet.Tables[0].Rows[i][111].ToString());
                                    sParam.Add("ITEM_108", ExcelDataSet.Tables[0].Rows[i][112].ToString());
                                    sParam.Add("ITEM_109", ExcelDataSet.Tables[0].Rows[i][113].ToString());
                                    sParam.Add("ITEM_110", ExcelDataSet.Tables[0].Rows[i][114].ToString());
                                    sParam.Add("ITEM_111", ExcelDataSet.Tables[0].Rows[i][115].ToString());
                                    sParam.Add("ITEM_112", ExcelDataSet.Tables[0].Rows[i][116].ToString());
                                    sParam.Add("ITEM_113", ExcelDataSet.Tables[0].Rows[i][117].ToString());
                                    sParam.Add("ITEM_114", ExcelDataSet.Tables[0].Rows[i][118].ToString());
                                    sParam.Add("ITEM_115", ExcelDataSet.Tables[0].Rows[i][119].ToString());
                                    sParam.Add("ITEM_116", ExcelDataSet.Tables[0].Rows[i][120].ToString());
                                    sParam.Add("ITEM_117", ExcelDataSet.Tables[0].Rows[i][121].ToString());
                                    sParam.Add("ITEM_118", ExcelDataSet.Tables[0].Rows[i][122].ToString());
                                    sParam.Add("ITEM_119", ExcelDataSet.Tables[0].Rows[i][123].ToString());
                                    sParam.Add("ITEM_120", ExcelDataSet.Tables[0].Rows[i][124].ToString());
                                    sParam.Add("ITEM_121", ExcelDataSet.Tables[0].Rows[i][125].ToString());
                                    sParam.Add("ITEM_122", ExcelDataSet.Tables[0].Rows[i][126].ToString());
                                    sParam.Add("ITEM_123", ExcelDataSet.Tables[0].Rows[i][127].ToString());
                                    sParam.Add("ITEM_124", ExcelDataSet.Tables[0].Rows[i][128].ToString());
                                    sParam.Add("ITEM_125", ExcelDataSet.Tables[0].Rows[i][129].ToString());
                                    sParam.Add("ITEM_126", ExcelDataSet.Tables[0].Rows[i][130].ToString());
                                    sParam.Add("ITEM_127", ExcelDataSet.Tables[0].Rows[i][131].ToString());
                                    sParam.Add("ITEM_128", ExcelDataSet.Tables[0].Rows[i][132].ToString());
                                    sParam.Add("ITEM_129", ExcelDataSet.Tables[0].Rows[i][133].ToString());
                                    sParam.Add("ITEM_130", ExcelDataSet.Tables[0].Rows[i][134].ToString());
                                    sParam.Add("ITEM_131", ExcelDataSet.Tables[0].Rows[i][135].ToString());
                                    sParam.Add("ITEM_132", ExcelDataSet.Tables[0].Rows[i][136].ToString());
                                    sParam.Add("ITEM_133", ExcelDataSet.Tables[0].Rows[i][137].ToString());
                                    sParam.Add("ITEM_134", ExcelDataSet.Tables[0].Rows[i][138].ToString());
                                    sParam.Add("ITEM_135", ExcelDataSet.Tables[0].Rows[i][139].ToString());
                                    sParam.Add("ITEM_136", ExcelDataSet.Tables[0].Rows[i][140].ToString());
                                    sParam.Add("ITEM_137", ExcelDataSet.Tables[0].Rows[i][141].ToString());
                                    sParam.Add("ITEM_138", ExcelDataSet.Tables[0].Rows[i][142].ToString());
                                    sParam.Add("ITEM_139", ExcelDataSet.Tables[0].Rows[i][143].ToString());
                                    sParam.Add("ITEM_140", ExcelDataSet.Tables[0].Rows[i][144].ToString());
                                    sParam.Add("ITEM_141", ExcelDataSet.Tables[0].Rows[i][145].ToString());
                                    sParam.Add("ITEM_142", ExcelDataSet.Tables[0].Rows[i][146].ToString());
                                    sParam.Add("ITEM_143", ExcelDataSet.Tables[0].Rows[i][147].ToString());
                                    sParam.Add("ITEM_144", ExcelDataSet.Tables[0].Rows[i][148].ToString());
                                    sParam.Add("ITEM_145", ExcelDataSet.Tables[0].Rows[i][149].ToString());
                                    sParam.Add("ITEM_146", ExcelDataSet.Tables[0].Rows[i][150].ToString());
                                    sParam.Add("ITEM_147", ExcelDataSet.Tables[0].Rows[i][151].ToString());
                                    sParam.Add("ITEM_148", ExcelDataSet.Tables[0].Rows[i][152].ToString());
                                    sParam.Add("ITEM_149", ExcelDataSet.Tables[0].Rows[i][153].ToString());
                                    sParam.Add("ITEM_150", ExcelDataSet.Tables[0].Rows[i][154].ToString());
                                    sParam.Add("ITEM_151", ExcelDataSet.Tables[0].Rows[i][155].ToString());
                                    sParam.Add("ITEM_152", ExcelDataSet.Tables[0].Rows[i][156].ToString());
                                    sParam.Add("ITEM_153", ExcelDataSet.Tables[0].Rows[i][157].ToString());
                                    sParam.Add("ITEM_154", ExcelDataSet.Tables[0].Rows[i][158].ToString());
                                    sParam.Add("ITEM_155", ExcelDataSet.Tables[0].Rows[i][159].ToString());
                                    sParam.Add("ITEM_156", ExcelDataSet.Tables[0].Rows[i][160].ToString());
                                    sParam.Add("ITEM_157", ExcelDataSet.Tables[0].Rows[i][161].ToString());
                                    sParam.Add("ITEM_158", ExcelDataSet.Tables[0].Rows[i][162].ToString());
                                    sParam.Add("ITEM_159", ExcelDataSet.Tables[0].Rows[i][163].ToString());
                                    sParam.Add("ITEM_160", ExcelDataSet.Tables[0].Rows[i][164].ToString());
                                    sParam.Add("ITEM_161", ExcelDataSet.Tables[0].Rows[i][165].ToString());
                                    sParam.Add("ITEM_162", ExcelDataSet.Tables[0].Rows[i][166].ToString());
                                    sParam.Add("ITEM_163", ExcelDataSet.Tables[0].Rows[i][167].ToString());
                                    sParam.Add("ITEM_164", ExcelDataSet.Tables[0].Rows[i][168].ToString());
                                    sParam.Add("ITEM_165", ExcelDataSet.Tables[0].Rows[i][169].ToString());
                                    sParam.Add("ITEM_166", ExcelDataSet.Tables[0].Rows[i][170].ToString());
                                    sParam.Add("ITEM_167", ExcelDataSet.Tables[0].Rows[i][171].ToString());
                                    sParam.Add("ITEM_168", ExcelDataSet.Tables[0].Rows[i][172].ToString());
                                    sParam.Add("ITEM_169", ExcelDataSet.Tables[0].Rows[i][173].ToString());
                                    sParam.Add("ITEM_170", ExcelDataSet.Tables[0].Rows[i][174].ToString());
                                    sParam.Add("ITEM_171", ExcelDataSet.Tables[0].Rows[i][175].ToString());
                                    sParam.Add("ITEM_172", ExcelDataSet.Tables[0].Rows[i][176].ToString());
                                    sParam.Add("ITEM_173", ExcelDataSet.Tables[0].Rows[i][177].ToString());
                                    sParam.Add("ITEM_174", ExcelDataSet.Tables[0].Rows[i][178].ToString());
                                    sParam.Add("ITEM_175", ExcelDataSet.Tables[0].Rows[i][179].ToString());
                                    sParam.Add("ITEM_176", ExcelDataSet.Tables[0].Rows[i][180].ToString());
                                    sParam.Add("ITEM_177", ExcelDataSet.Tables[0].Rows[i][181].ToString());
                                    sParam.Add("ITEM_178", ExcelDataSet.Tables[0].Rows[i][182].ToString());
                                    sParam.Add("ITEM_179", ExcelDataSet.Tables[0].Rows[i][183].ToString());
                                    sParam.Add("ITEM_180", ExcelDataSet.Tables[0].Rows[i][184].ToString());
                                    sParam.Add("ITEM_181", ExcelDataSet.Tables[0].Rows[i][185].ToString());
                                    sParam.Add("ITEM_182", ExcelDataSet.Tables[0].Rows[i][186].ToString());
                                    sParam.Add("ITEM_183", ExcelDataSet.Tables[0].Rows[i][187].ToString());
                                    sParam.Add("ITEM_184", ExcelDataSet.Tables[0].Rows[i][188].ToString());
                                    sParam.Add("ITEM_185", ExcelDataSet.Tables[0].Rows[i][189].ToString());
                                    sParam.Add("ITEM_186", ExcelDataSet.Tables[0].Rows[i][190].ToString());
                                    sParam.Add("ITEM_187", ExcelDataSet.Tables[0].Rows[i][191].ToString());
                                    sParam.Add("ITEM_188", ExcelDataSet.Tables[0].Rows[i][192].ToString());
                                    sParam.Add("ITEM_189", ExcelDataSet.Tables[0].Rows[i][193].ToString());
                                    sParam.Add("ITEM_190", ExcelDataSet.Tables[0].Rows[i][194].ToString());
                                    sParam.Add("ITEM_191", ExcelDataSet.Tables[0].Rows[i][195].ToString());
                                    sParam.Add("ITEM_192", ExcelDataSet.Tables[0].Rows[i][196].ToString());
                                    sParam.Add("ITEM_193", ExcelDataSet.Tables[0].Rows[i][197].ToString());
                                    sParam.Add("ITEM_194", ExcelDataSet.Tables[0].Rows[i][198].ToString());
                                    sParam.Add("ITEM_195", ExcelDataSet.Tables[0].Rows[i][199].ToString());
                                    sParam.Add("ITEM_196", ExcelDataSet.Tables[0].Rows[i][200].ToString());
                                    sParam.Add("ITEM_197", ExcelDataSet.Tables[0].Rows[i][201].ToString());
                                    sParam.Add("ITEM_198", ExcelDataSet.Tables[0].Rows[i][202].ToString());
                                    sParam.Add("ITEM_199", ExcelDataSet.Tables[0].Rows[i][203].ToString());
                                    sParam.Add("ITEM_200", ExcelDataSet.Tables[0].Rows[i][204].ToString());
                                    sParam.Add("ITEM_201", ExcelDataSet.Tables[0].Rows[i][205].ToString());
                                    sParam.Add("ITEM_202", ExcelDataSet.Tables[0].Rows[i][206].ToString());
                                    sParam.Add("ITEM_203", ExcelDataSet.Tables[0].Rows[i][207].ToString());
                                    sParam.Add("ITEM_204", ExcelDataSet.Tables[0].Rows[i][208].ToString());
                                    sParam.Add("ITEM_205", ExcelDataSet.Tables[0].Rows[i][209].ToString());
                                    sParam.Add("ITEM_206", ExcelDataSet.Tables[0].Rows[i][210].ToString());
                                    sParam.Add("ITEM_207", ExcelDataSet.Tables[0].Rows[i][211].ToString());
                                    sParam.Add("ITEM_208", ExcelDataSet.Tables[0].Rows[i][212].ToString());
                                    sParam.Add("ITEM_209", ExcelDataSet.Tables[0].Rows[i][213].ToString());
                                    sParam.Add("ITEM_210", ExcelDataSet.Tables[0].Rows[i][214].ToString());
                                    sParam.Add("ITEM_211", ExcelDataSet.Tables[0].Rows[i][215].ToString());
                                    sParam.Add("ITEM_212", ExcelDataSet.Tables[0].Rows[i][216].ToString());
                                    sParam.Add("ITEM_213", ExcelDataSet.Tables[0].Rows[i][217].ToString());
                                    sParam.Add("ITEM_214", ExcelDataSet.Tables[0].Rows[i][218].ToString());
                                    sParam.Add("ITEM_215", ExcelDataSet.Tables[0].Rows[i][219].ToString());
                                    sParam.Add("ITEM_216", ExcelDataSet.Tables[0].Rows[i][220].ToString());
                                    sParam.Add("ITEM_217", ExcelDataSet.Tables[0].Rows[i][221].ToString());
                                    sParam.Add("ITEM_218", ExcelDataSet.Tables[0].Rows[i][222].ToString());
                                    sParam.Add("ITEM_219", ExcelDataSet.Tables[0].Rows[i][223].ToString());
                                    sParam.Add("ITEM_220", ExcelDataSet.Tables[0].Rows[i][224].ToString());
                                    sParam.Add("ITEM_221", ExcelDataSet.Tables[0].Rows[i][225].ToString());
                                    sParam.Add("ITEM_222", ExcelDataSet.Tables[0].Rows[i][226].ToString());
                                    sParam.Add("ITEM_223", ExcelDataSet.Tables[0].Rows[i][227].ToString());
                                    sParam.Add("ITEM_224", ExcelDataSet.Tables[0].Rows[i][228].ToString());
                                    sParam.Add("ITEM_225", ExcelDataSet.Tables[0].Rows[i][229].ToString());
                                    sParam.Add("ITEM_226", ExcelDataSet.Tables[0].Rows[i][230].ToString());
                                    sParam.Add("ITEM_227", ExcelDataSet.Tables[0].Rows[i][231].ToString());
                                    sParam.Add("ITEM_228", ExcelDataSet.Tables[0].Rows[i][232].ToString());
                                    sParam.Add("ITEM_229", ExcelDataSet.Tables[0].Rows[i][233].ToString());
                                    sParam.Add("ITEM_230", ExcelDataSet.Tables[0].Rows[i][234].ToString());
                                    sParam.Add("ITEM_231", ExcelDataSet.Tables[0].Rows[i][235].ToString());
                                    sParam.Add("ITEM_232", ExcelDataSet.Tables[0].Rows[i][236].ToString());
                                    sParam.Add("ITEM_233", ExcelDataSet.Tables[0].Rows[i][237].ToString());
                                    sParam.Add("ITEM_234", ExcelDataSet.Tables[0].Rows[i][238].ToString());
                                    sParam.Add("ITEM_235", ExcelDataSet.Tables[0].Rows[i][239].ToString());
                                    sParam.Add("ITEM_236", ExcelDataSet.Tables[0].Rows[i][240].ToString());
                                    sParam.Add("ITEM_237", ExcelDataSet.Tables[0].Rows[i][241].ToString());
                                    sParam.Add("ITEM_238", ExcelDataSet.Tables[0].Rows[i][242].ToString());
                                    sParam.Add("ITEM_239", ExcelDataSet.Tables[0].Rows[i][243].ToString());
                                    sParam.Add("ITEM_240", ExcelDataSet.Tables[0].Rows[i][244].ToString());
                                    sParam.Add("ITEM_241", ExcelDataSet.Tables[0].Rows[i][245].ToString());
                                    sParam.Add("ITEM_242", ExcelDataSet.Tables[0].Rows[i][246].ToString());
                                    sParam.Add("ITEM_243", ExcelDataSet.Tables[0].Rows[i][247].ToString());
                                    sParam.Add("ITEM_244", ExcelDataSet.Tables[0].Rows[i][248].ToString());
                                    sParam.Add("ITEM_245", ExcelDataSet.Tables[0].Rows[i][249].ToString());
                                    sParam.Add("ITEM_246", ExcelDataSet.Tables[0].Rows[i][250].ToString());
                                    sParam.Add("ITEM_247", ExcelDataSet.Tables[0].Rows[i][251].ToString());
                                    sParam.Add("ITEM_248", ExcelDataSet.Tables[0].Rows[i][252].ToString());
                                    sParam.Add("ITEM_249", ExcelDataSet.Tables[0].Rows[i][253].ToString());
                                    sParam.Add("ITEM_250", ExcelDataSet.Tables[0].Rows[i][254].ToString());
                                    sParam.Add("ITEM_251", ExcelDataSet.Tables[0].Rows[i][255].ToString());
                                    sParam.Add("ITEM_252", ExcelDataSet.Tables[0].Rows[i][256].ToString());
                                    sParam.Add("ITEM_253", ExcelDataSet.Tables[0].Rows[i][257].ToString());
                                    sParam.Add("ITEM_254", ExcelDataSet.Tables[0].Rows[i][258].ToString());
                                    sParam.Add("ITEM_255", ExcelDataSet.Tables[0].Rows[i][259].ToString());
                                    sParam.Add("ITEM_256", ExcelDataSet.Tables[0].Rows[i][260].ToString());
                                    sParam.Add("ITEM_257", ExcelDataSet.Tables[0].Rows[i][261].ToString());
                                    sParam.Add("ITEM_258", ExcelDataSet.Tables[0].Rows[i][262].ToString());
                                    sParam.Add("ITEM_259", ExcelDataSet.Tables[0].Rows[i][263].ToString());
                                    sParam.Add("ITEM_260", ExcelDataSet.Tables[0].Rows[i][264].ToString());
                                    sParam.Add("ITEM_261", ExcelDataSet.Tables[0].Rows[i][265].ToString());
                                    sParam.Add("ITEM_262", ExcelDataSet.Tables[0].Rows[i][266].ToString());
                                    sParam.Add("ITEM_263", ExcelDataSet.Tables[0].Rows[i][267].ToString());
                                    sParam.Add("ITEM_264", ExcelDataSet.Tables[0].Rows[i][268].ToString());
                                    sParam.Add("ITEM_265", ExcelDataSet.Tables[0].Rows[i][269].ToString());
                                    sParam.Add("ITEM_266", ExcelDataSet.Tables[0].Rows[i][270].ToString());
                                    sParam.Add("ITEM_267", ExcelDataSet.Tables[0].Rows[i][271].ToString());
                                    sParam.Add("ITEM_268", ExcelDataSet.Tables[0].Rows[i][272].ToString());
                                    sParam.Add("ITEM_269", ExcelDataSet.Tables[0].Rows[i][273].ToString());
                                    sParam.Add("ITEM_270", ExcelDataSet.Tables[0].Rows[i][274].ToString());
                                    sParam.Add("ITEM_271", ExcelDataSet.Tables[0].Rows[i][275].ToString());
                                    sParam.Add("ITEM_272", ExcelDataSet.Tables[0].Rows[i][276].ToString());
                                    sParam.Add("ITEM_273", ExcelDataSet.Tables[0].Rows[i][277].ToString());
                                    sParam.Add("ITEM_274", ExcelDataSet.Tables[0].Rows[i][278].ToString());
                                    sParam.Add("ITEM_275", ExcelDataSet.Tables[0].Rows[i][279].ToString());
                                    sParam.Add("ITEM_276", ExcelDataSet.Tables[0].Rows[i][280].ToString());
                                    sParam.Add("ITEM_277", ExcelDataSet.Tables[0].Rows[i][281].ToString());
                                    sParam.Add("ITEM_278", ExcelDataSet.Tables[0].Rows[i][282].ToString());
                                    sParam.Add("ITEM_279", ExcelDataSet.Tables[0].Rows[i][283].ToString());
                                    sParam.Add("ITEM_280", ExcelDataSet.Tables[0].Rows[i][284].ToString());
                                    sParam.Add("ITEM_281", ExcelDataSet.Tables[0].Rows[i][285].ToString());
                                    sParam.Add("ITEM_282", ExcelDataSet.Tables[0].Rows[i][286].ToString());
                                    sParam.Add("ITEM_283", ExcelDataSet.Tables[0].Rows[i][287].ToString());
                                    sParam.Add("ITEM_284", ExcelDataSet.Tables[0].Rows[i][288].ToString());
                                    sParam.Add("ITEM_285", ExcelDataSet.Tables[0].Rows[i][289].ToString());
                                    sParam.Add("ITEM_286", ExcelDataSet.Tables[0].Rows[i][290].ToString());
                                    sParam.Add("ITEM_287", ExcelDataSet.Tables[0].Rows[i][291].ToString());
                                    sParam.Add("ITEM_288", ExcelDataSet.Tables[0].Rows[i][292].ToString());
                                    sParam.Add("ITEM_289", ExcelDataSet.Tables[0].Rows[i][293].ToString());
                                    sParam.Add("ITEM_290", ExcelDataSet.Tables[0].Rows[i][294].ToString());
                                    sParam.Add("ITEM_291", ExcelDataSet.Tables[0].Rows[i][295].ToString());
                                    sParam.Add("ITEM_292", ExcelDataSet.Tables[0].Rows[i][296].ToString());
                                    sParam.Add("ITEM_293", ExcelDataSet.Tables[0].Rows[i][297].ToString());
                                    sParam.Add("ITEM_294", ExcelDataSet.Tables[0].Rows[i][298].ToString());
                                    sParam.Add("ITEM_295", ExcelDataSet.Tables[0].Rows[i][299].ToString());
                                    sParam.Add("ITEM_296", ExcelDataSet.Tables[0].Rows[i][300].ToString());
                                    sParam.Add("ITEM_297", ExcelDataSet.Tables[0].Rows[i][301].ToString());
                                    sParam.Add("ITEM_298", ExcelDataSet.Tables[0].Rows[i][302].ToString());
                                    sParam.Add("ITEM_299", ExcelDataSet.Tables[0].Rows[i][303].ToString());
                                    sParam.Add("ITEM_300", ExcelDataSet.Tables[0].Rows[i][304].ToString());

                                    sParam.Add("USE_YN", "Y");
                                    sParam.Add("USER_ID", bp.g_userid.ToString());

                                    sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                                    sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                                    sParam.Add("CUR_MENU_ID", "Info30");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

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
                                    strScript = " alert('전체 데이터 정상등록 되었습니다.');  parent.fn_ModalReloadCall('Info30'); parent.fn_ModalCloseDiv(); ";
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
