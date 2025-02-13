using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using MES.FW.Common.CommonMgt;
using HQCWeb.FMB_FW;
using HQCWeb.FW;

using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace HQCWeb.InfoMgt.Info30
{
    public partial class Info30 : System.Web.UI.Page
    {
        BasePage bp = new BasePage();
        ExcelExport ee = new ExcelExport();

        string strDBName = string.Empty;
        string strQueryID = string.Empty;
        string strErrMessage = string.Empty;

        // 비지니스 클래스 작성
        Biz.InfoManagement.Info30 biz = new Biz.InfoManagement.Info30();

        //버튼 로그 클래스 작성
        Biz.SystemManagement.ButtonStatisticsMgt btnlog = new Biz.SystemManagement.ButtonStatisticsMgt();

        #region GRID Setting
        // 그리드에 보여져야할 컬럼 정의
        public string[] arrColumn;
        // 그리드에 보여져야할 컬럼타이틀 정의
        public string[] arrColumnCaption;
        // 그리드에 보여져야할 컬럼 넓이 정의
        public string[] arrColumnWidth;
        // 그리드 고정값 정의
        public string strFix;
        // 그리드 키값 정의
        public string[] strKeyColumn = new string[] { "PLANT_CD", "SHOP_CD", "LINE_CD", "CAR_TYPE", "DEV_ID" };

        //JSON 전달용 변수
        string jsField = string.Empty;
        string jsCol = string.Empty;
        string jsData = string.Empty;
        #endregion

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageInit();

            if (!IsPostBack)
            {
                SetCon();

                SetGridTitle();

                SetTitle();

                // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'createGrid' 함수 호출
                string script = $@" column = {jsCol}; 
                                field = {jsField}; 
                                createGrid('" + strFix + "'); ";

                //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), script, true);
            }
        }
        #endregion

        #region SetCon
        private void SetCon()
        {
            strErrMessage = Message_Data.SearchDic("SearchError", bp.g_language);

            DataSet ds = new DataSet();

            ddlShopCd.Items.Add(new ListItem("ALL", ""));
            ddlLineCd.Items.Add(new ListItem("ALL", ""));
            ddlDevCd.Items.Add(new ListItem("ALL", ""));
            ddlCarType.Items.Add(new ListItem("ALL", ""));

            strDBName = "GPDB";
            strQueryID = "Info30Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", "");
            param.Add("LINE_CD", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddlShopCd.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CODE_NM"].ToString(), ds.Tables[0].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                {
                    ddlLineCd.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                {
                    ddlDevCd.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                }
                for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                {
                    ddlCarType.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                }
            }
        }
        #endregion

        #region SetPageInit
        private void SetPageInit()
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            Biz.SystemManagement.UserMgt biz = new Biz.SystemManagement.UserMgt();

            strDBName = "GPDB";
            strQueryID = "UserInfoData.Get_UserMenuSettingInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("MENU_ID", "Info30");
            sParam.Add("USER_ID", bp.g_userid.ToString());

            sParam.Add("CUR_MENU_ID", "Info30");

            ds = biz.GetUserMenuSettingInfoList(strDBName, strQueryID, sParam);

            if (ds.Tables.Count > 0)
            {
                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strMessage = ds.Tables[0].Rows[0][1].ToString();

                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "createGrid();", true);

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_ErrorMessage('" + strMessage + "');", true);
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        int iRowCnt = ds.Tables[0].Rows.Count;

                        arrColumn = new string[iRowCnt];
                        arrColumnCaption = new string[iRowCnt];
                        arrColumnWidth = new string[iRowCnt];

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            arrColumn[i] = ds.Tables[0].Rows[i]["COLUMN_NAME"].ToString();
                            arrColumnWidth[i] = ds.Tables[0].Rows[i]["COLUMN_WIDTH"].ToString();
                            arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);

                            if (ds.Tables[0].Rows[i]["COLUMN_FIX"].ToString() == "true")
                            {
                                strFix = (i + 1).ToString();
                            }
                        }
                    }
                    else
                    {
                        arrColumn = new string[] { "PLANT_CD", "SHOP_CD", "LINE_CD", "CAR_TYPE", "DEV_ID", "USE_YN", "REG_DATE", "REG_USER", "MOD_DATE", "MOD_USER", "SPEC_001", "SPEC_002", "SPEC_003", "SPEC_004", "SPEC_005", "SPEC_006", "SPEC_007", "SPEC_008", "SPEC_009", "SPEC_010", "SPEC_011", "SPEC_012", "SPEC_013", "SPEC_014", "SPEC_015", "SPEC_016", "SPEC_017", "SPEC_018", "SPEC_019", "SPEC_020", "SPEC_021", "SPEC_022", "SPEC_023", "SPEC_024", "SPEC_025", "SPEC_026", "SPEC_027", "SPEC_028", "SPEC_029", "SPEC_030", "SPEC_031", "SPEC_032", "SPEC_033", "SPEC_034", "SPEC_035", "SPEC_036", "SPEC_037", "SPEC_038", "SPEC_039", "SPEC_040", "SPEC_041", "SPEC_042", "SPEC_043", "SPEC_044", "SPEC_045", "SPEC_046", "SPEC_047", "SPEC_048", "SPEC_049", "SPEC_050", "SPEC_051", "SPEC_052", "SPEC_053", "SPEC_054", "SPEC_055", "SPEC_056", "SPEC_057", "SPEC_058", "SPEC_059", "SPEC_060", "SPEC_061", "SPEC_062", "SPEC_063", "SPEC_064", "SPEC_065", "SPEC_066", "SPEC_067", "SPEC_068", "SPEC_069", "SPEC_070", "SPEC_071", "SPEC_072", "SPEC_073", "SPEC_074", "SPEC_075", "SPEC_076", "SPEC_077", "SPEC_078", "SPEC_079", "SPEC_080", "SPEC_081", "SPEC_082", "SPEC_083", "SPEC_084", "SPEC_085", "SPEC_086", "SPEC_087", "SPEC_088", "SPEC_089", "SPEC_090", "SPEC_091", "SPEC_092", "SPEC_093", "SPEC_094", "SPEC_095", "SPEC_096", "SPEC_097", "SPEC_098", "SPEC_099", "SPEC_100", "SPEC_101", "SPEC_102", "SPEC_103", "SPEC_104", "SPEC_105", "SPEC_106", "SPEC_107", "SPEC_108", "SPEC_109", "SPEC_110", "SPEC_111", "SPEC_112", "SPEC_113", "SPEC_114", "SPEC_115", "SPEC_116", "SPEC_117", "SPEC_118", "SPEC_119", "SPEC_120", "SPEC_121", "SPEC_122", "SPEC_123", "SPEC_124", "SPEC_125", "SPEC_126", "SPEC_127", "SPEC_128", "SPEC_129", "SPEC_130", "SPEC_131", "SPEC_132", "SPEC_133", "SPEC_134", "SPEC_135", "SPEC_136", "SPEC_137", "SPEC_138", "SPEC_139", "SPEC_140", "SPEC_141", "SPEC_142", "SPEC_143", "SPEC_144", "SPEC_145", "SPEC_146", "SPEC_147", "SPEC_148", "SPEC_149", "SPEC_150", "SPEC_151", "SPEC_152", "SPEC_153", "SPEC_154", "SPEC_155", "SPEC_156", "SPEC_157", "SPEC_158", "SPEC_159", "SPEC_160", "SPEC_161", "SPEC_162", "SPEC_163", "SPEC_164", "SPEC_165", "SPEC_166", "SPEC_167", "SPEC_168", "SPEC_169", "SPEC_170", "SPEC_171", "SPEC_172", "SPEC_173", "SPEC_174", "SPEC_175", "SPEC_176", "SPEC_177", "SPEC_178", "SPEC_179", "SPEC_180", "SPEC_181", "SPEC_182", "SPEC_183", "SPEC_184", "SPEC_185", "SPEC_186", "SPEC_187", "SPEC_188", "SPEC_189", "SPEC_190", "SPEC_191", "SPEC_192", "SPEC_193", "SPEC_194", "SPEC_195", "SPEC_196", "SPEC_197", "SPEC_198", "SPEC_199", "SPEC_200", "SPEC_201", "SPEC_202", "SPEC_203", "SPEC_204", "SPEC_205", "SPEC_206", "SPEC_207", "SPEC_208", "SPEC_209", "SPEC_210", "SPEC_211", "SPEC_212", "SPEC_213", "SPEC_214", "SPEC_215", "SPEC_216", "SPEC_217", "SPEC_218", "SPEC_219", "SPEC_220", "SPEC_221", "SPEC_222", "SPEC_223", "SPEC_224", "SPEC_225", "SPEC_226", "SPEC_227", "SPEC_228", "SPEC_229", "SPEC_230", "SPEC_231", "SPEC_232", "SPEC_233", "SPEC_234", "SPEC_235", "SPEC_236", "SPEC_237", "SPEC_238", "SPEC_239", "SPEC_240", "SPEC_241", "SPEC_242", "SPEC_243", "SPEC_244", "SPEC_245", "SPEC_246", "SPEC_247", "SPEC_248", "SPEC_249", "SPEC_250", "SPEC_251", "SPEC_252", "SPEC_253", "SPEC_254", "SPEC_255", "SPEC_256", "SPEC_257", "SPEC_258", "SPEC_259", "SPEC_260", "SPEC_261", "SPEC_262", "SPEC_263", "SPEC_264", "SPEC_265", "SPEC_266", "SPEC_267", "SPEC_268", "SPEC_269", "SPEC_270", "SPEC_271", "SPEC_272", "SPEC_273", "SPEC_274", "SPEC_275", "SPEC_276", "SPEC_277", "SPEC_278", "SPEC_279", "SPEC_280", "SPEC_281", "SPEC_282", "SPEC_283", "SPEC_284", "SPEC_285", "SPEC_286", "SPEC_287", "SPEC_288", "SPEC_289", "SPEC_290", "SPEC_291", "SPEC_292", "SPEC_293", "SPEC_294", "SPEC_295", "SPEC_296", "SPEC_297", "SPEC_298", "SPEC_299", "SPEC_300", "KEY_HID" };
                        arrColumnCaption = new string[arrColumn.Length];
                        arrColumnWidth = new string[arrColumn.Length];
                        strFix = "";

                        for (int i = 0; i < arrColumn.Length; i++)
                        {
                            arrColumnCaption[i] = Dictionary_Data.SearchDic(arrColumn[i].ToString(), bp.g_language);
                            //arrColumnWidth[i] = "200";
                        }

                        arrColumnWidth[0] = "40";
                        arrColumnWidth[1] = "43";
                        arrColumnWidth[2] = "41";
                        arrColumnWidth[3] = "46";
                        arrColumnWidth[4] = "68";
                        arrColumnWidth[5] = "54";
                        arrColumnWidth[6] = "130";
                        arrColumnWidth[7] = "100";
                        arrColumnWidth[8] = "130";
                        arrColumnWidth[9] = "100";
                        arrColumnWidth[10] = "200";
                        arrColumnWidth[11] = "200";
                        arrColumnWidth[12] = "200";
                        arrColumnWidth[13] = "200";
                        arrColumnWidth[14] = "200";
                        arrColumnWidth[15] = "200";
                        arrColumnWidth[16] = "200";
                        arrColumnWidth[17] = "200";
                        arrColumnWidth[18] = "200";
                        arrColumnWidth[19] = "200";
                        arrColumnWidth[20] = "200";
                        arrColumnWidth[21] = "200";
                        arrColumnWidth[22] = "200";
                        arrColumnWidth[23] = "200";
                        arrColumnWidth[24] = "200";
                        arrColumnWidth[25] = "200";
                        arrColumnWidth[26] = "200";
                        arrColumnWidth[27] = "200";
                        arrColumnWidth[28] = "200";
                        arrColumnWidth[29] = "200";
                        arrColumnWidth[30] = "200";
                        arrColumnWidth[31] = "200";
                        arrColumnWidth[32] = "200";
                        arrColumnWidth[33] = "200";
                        arrColumnWidth[34] = "200";
                        arrColumnWidth[35] = "200";
                        arrColumnWidth[36] = "200";
                        arrColumnWidth[37] = "200";
                        arrColumnWidth[38] = "200";
                        arrColumnWidth[39] = "200";
                        arrColumnWidth[40] = "200";
                        arrColumnWidth[41] = "200";
                        arrColumnWidth[42] = "200";
                        arrColumnWidth[43] = "200";
                        arrColumnWidth[44] = "200";
                        arrColumnWidth[45] = "200";
                        arrColumnWidth[46] = "200";
                        arrColumnWidth[47] = "200";
                        arrColumnWidth[48] = "200";
                        arrColumnWidth[49] = "200";
                        arrColumnWidth[50] = "200";
                        arrColumnWidth[51] = "200";
                        arrColumnWidth[52] = "200";
                        arrColumnWidth[53] = "200";
                        arrColumnWidth[54] = "200";
                        arrColumnWidth[55] = "200";
                        arrColumnWidth[56] = "200";
                        arrColumnWidth[57] = "200";
                        arrColumnWidth[58] = "200";
                        arrColumnWidth[59] = "200";
                        arrColumnWidth[60] = "200";
                        arrColumnWidth[61] = "200";
                        arrColumnWidth[62] = "200";
                        arrColumnWidth[63] = "200";
                        arrColumnWidth[64] = "200";
                        arrColumnWidth[65] = "200";
                        arrColumnWidth[66] = "200";
                        arrColumnWidth[67] = "200";
                        arrColumnWidth[68] = "200";
                        arrColumnWidth[69] = "200";
                        arrColumnWidth[70] = "200";
                        arrColumnWidth[71] = "200";
                        arrColumnWidth[72] = "200";
                        arrColumnWidth[73] = "200";
                        arrColumnWidth[74] = "200";
                        arrColumnWidth[75] = "200";
                        arrColumnWidth[76] = "200";
                        arrColumnWidth[77] = "200";
                        arrColumnWidth[78] = "200";
                        arrColumnWidth[79] = "200";
                        arrColumnWidth[80] = "200";
                        arrColumnWidth[81] = "200";
                        arrColumnWidth[82] = "200";
                        arrColumnWidth[83] = "200";
                        arrColumnWidth[84] = "200";
                        arrColumnWidth[85] = "200";
                        arrColumnWidth[86] = "200";
                        arrColumnWidth[87] = "200";
                        arrColumnWidth[88] = "200";
                        arrColumnWidth[89] = "200";
                        arrColumnWidth[90] = "200";
                        arrColumnWidth[91] = "200";
                        arrColumnWidth[92] = "200";
                        arrColumnWidth[93] = "200";
                        arrColumnWidth[94] = "200";
                        arrColumnWidth[95] = "200";
                        arrColumnWidth[96] = "200";
                        arrColumnWidth[97] = "200";
                        arrColumnWidth[98] = "200";
                        arrColumnWidth[99] = "200";
                        arrColumnWidth[100] = "200";
                        arrColumnWidth[101] = "200";
                        arrColumnWidth[102] = "200";
                        arrColumnWidth[103] = "200";
                        arrColumnWidth[104] = "200";
                        arrColumnWidth[105] = "200";
                        arrColumnWidth[106] = "200";
                        arrColumnWidth[107] = "200";
                        arrColumnWidth[108] = "200";
                        arrColumnWidth[109] = "200";
                        arrColumnWidth[110] = "200";
                        arrColumnWidth[111] = "200";
                        arrColumnWidth[112] = "200";
                        arrColumnWidth[113] = "200";
                        arrColumnWidth[114] = "200";
                        arrColumnWidth[115] = "200";
                        arrColumnWidth[116] = "200";
                        arrColumnWidth[117] = "200";
                        arrColumnWidth[118] = "200";
                        arrColumnWidth[119] = "200";
                        arrColumnWidth[120] = "200";
                        arrColumnWidth[121] = "200";
                        arrColumnWidth[122] = "200";
                        arrColumnWidth[123] = "200";
                        arrColumnWidth[124] = "200";
                        arrColumnWidth[125] = "200";
                        arrColumnWidth[126] = "200";
                        arrColumnWidth[127] = "200";
                        arrColumnWidth[128] = "200";
                        arrColumnWidth[129] = "200";
                        arrColumnWidth[130] = "200";
                        arrColumnWidth[131] = "200";
                        arrColumnWidth[132] = "200";
                        arrColumnWidth[133] = "200";
                        arrColumnWidth[134] = "200";
                        arrColumnWidth[135] = "200";
                        arrColumnWidth[136] = "200";
                        arrColumnWidth[137] = "200";
                        arrColumnWidth[138] = "200";
                        arrColumnWidth[139] = "200";
                        arrColumnWidth[140] = "200";
                        arrColumnWidth[141] = "200";
                        arrColumnWidth[142] = "200";
                        arrColumnWidth[143] = "200";
                        arrColumnWidth[144] = "200";
                        arrColumnWidth[145] = "200";
                        arrColumnWidth[146] = "200";
                        arrColumnWidth[147] = "200";
                        arrColumnWidth[148] = "200";
                        arrColumnWidth[149] = "200";
                        arrColumnWidth[150] = "200";
                        arrColumnWidth[151] = "200";
                        arrColumnWidth[152] = "200";
                        arrColumnWidth[153] = "200";
                        arrColumnWidth[154] = "200";
                        arrColumnWidth[155] = "200";
                        arrColumnWidth[156] = "200";
                        arrColumnWidth[157] = "200";
                        arrColumnWidth[158] = "200";
                        arrColumnWidth[159] = "200";
                        arrColumnWidth[160] = "200";
                        arrColumnWidth[161] = "200";
                        arrColumnWidth[162] = "200";
                        arrColumnWidth[163] = "200";
                        arrColumnWidth[164] = "200";
                        arrColumnWidth[165] = "200";
                        arrColumnWidth[166] = "200";
                        arrColumnWidth[167] = "200";
                        arrColumnWidth[168] = "200";
                        arrColumnWidth[169] = "200";
                        arrColumnWidth[170] = "200";
                        arrColumnWidth[171] = "200";
                        arrColumnWidth[172] = "200";
                        arrColumnWidth[173] = "200";
                        arrColumnWidth[174] = "200";
                        arrColumnWidth[175] = "200";
                        arrColumnWidth[176] = "200";
                        arrColumnWidth[177] = "200";
                        arrColumnWidth[178] = "200";
                        arrColumnWidth[179] = "200";
                        arrColumnWidth[180] = "200";
                        arrColumnWidth[181] = "200";
                        arrColumnWidth[182] = "200";
                        arrColumnWidth[183] = "200";
                        arrColumnWidth[184] = "200";
                        arrColumnWidth[185] = "200";
                        arrColumnWidth[186] = "200";
                        arrColumnWidth[187] = "200";
                        arrColumnWidth[188] = "200";
                        arrColumnWidth[189] = "200";
                        arrColumnWidth[190] = "200";
                        arrColumnWidth[191] = "200";
                        arrColumnWidth[192] = "200";
                        arrColumnWidth[193] = "200";
                        arrColumnWidth[194] = "200";
                        arrColumnWidth[195] = "200";
                        arrColumnWidth[196] = "200";
                        arrColumnWidth[197] = "200";
                        arrColumnWidth[198] = "200";
                        arrColumnWidth[199] = "200";
                        arrColumnWidth[200] = "200";
                        arrColumnWidth[201] = "200";
                        arrColumnWidth[202] = "200";
                        arrColumnWidth[203] = "200";
                        arrColumnWidth[204] = "200";
                        arrColumnWidth[205] = "200";
                        arrColumnWidth[206] = "200";
                        arrColumnWidth[207] = "200";
                        arrColumnWidth[208] = "200";
                        arrColumnWidth[209] = "200";
                        arrColumnWidth[210] = "200";
                        arrColumnWidth[211] = "200";
                        arrColumnWidth[212] = "200";
                        arrColumnWidth[213] = "200";
                        arrColumnWidth[214] = "200";
                        arrColumnWidth[215] = "200";
                        arrColumnWidth[216] = "200";
                        arrColumnWidth[217] = "200";
                        arrColumnWidth[218] = "200";
                        arrColumnWidth[219] = "200";
                        arrColumnWidth[220] = "200";
                        arrColumnWidth[221] = "200";
                        arrColumnWidth[222] = "200";
                        arrColumnWidth[223] = "200";
                        arrColumnWidth[224] = "200";
                        arrColumnWidth[225] = "200";
                        arrColumnWidth[226] = "200";
                        arrColumnWidth[227] = "200";
                        arrColumnWidth[228] = "200";
                        arrColumnWidth[229] = "200";
                        arrColumnWidth[230] = "200";
                        arrColumnWidth[231] = "200";
                        arrColumnWidth[232] = "200";
                        arrColumnWidth[233] = "200";
                        arrColumnWidth[234] = "200";
                        arrColumnWidth[235] = "200";
                        arrColumnWidth[236] = "200";
                        arrColumnWidth[237] = "200";
                        arrColumnWidth[238] = "200";
                        arrColumnWidth[239] = "200";
                        arrColumnWidth[240] = "200";
                        arrColumnWidth[241] = "200";
                        arrColumnWidth[242] = "200";
                        arrColumnWidth[243] = "200";
                        arrColumnWidth[244] = "200";
                        arrColumnWidth[245] = "200";
                        arrColumnWidth[246] = "200";
                        arrColumnWidth[247] = "200";
                        arrColumnWidth[248] = "200";
                        arrColumnWidth[249] = "200";
                        arrColumnWidth[250] = "200";
                        arrColumnWidth[251] = "200";
                        arrColumnWidth[252] = "200";
                        arrColumnWidth[253] = "200";
                        arrColumnWidth[254] = "200";
                        arrColumnWidth[255] = "200";
                        arrColumnWidth[256] = "200";
                        arrColumnWidth[257] = "200";
                        arrColumnWidth[258] = "200";
                        arrColumnWidth[259] = "200";
                        arrColumnWidth[260] = "200";
                        arrColumnWidth[261] = "200";
                        arrColumnWidth[262] = "200";
                        arrColumnWidth[263] = "200";
                        arrColumnWidth[264] = "200";
                        arrColumnWidth[265] = "200";
                        arrColumnWidth[266] = "200";
                        arrColumnWidth[267] = "200";
                        arrColumnWidth[268] = "200";
                        arrColumnWidth[269] = "200";
                        arrColumnWidth[270] = "200";
                        arrColumnWidth[271] = "200";
                        arrColumnWidth[272] = "200";
                        arrColumnWidth[273] = "200";
                        arrColumnWidth[274] = "200";
                        arrColumnWidth[275] = "200";
                        arrColumnWidth[276] = "200";
                        arrColumnWidth[277] = "200";
                        arrColumnWidth[278] = "200";
                        arrColumnWidth[279] = "200";
                        arrColumnWidth[280] = "200";
                        arrColumnWidth[281] = "200";
                        arrColumnWidth[282] = "200";
                        arrColumnWidth[283] = "200";
                        arrColumnWidth[284] = "200";
                        arrColumnWidth[285] = "200";
                        arrColumnWidth[286] = "200";
                        arrColumnWidth[287] = "200";
                        arrColumnWidth[288] = "200";
                        arrColumnWidth[289] = "200";
                        arrColumnWidth[290] = "200";
                        arrColumnWidth[291] = "200";
                        arrColumnWidth[292] = "200";
                        arrColumnWidth[293] = "200";
                        arrColumnWidth[294] = "200";
                        arrColumnWidth[295] = "200";
                        arrColumnWidth[296] = "200";
                        arrColumnWidth[297] = "200";
                        arrColumnWidth[298] = "200";
                        arrColumnWidth[299] = "200";
                        arrColumnWidth[300] = "200";
                        arrColumnWidth[301] = "200";
                        arrColumnWidth[302] = "200";
                        arrColumnWidth[303] = "200";
                        arrColumnWidth[304] = "200";
                        arrColumnWidth[305] = "200";
                        arrColumnWidth[306] = "200";
                        arrColumnWidth[307] = "200";
                        arrColumnWidth[308] = "200";
                        arrColumnWidth[309] = "200";
                        arrColumnWidth[310] = "0";
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Faile", "fn_NoData();", true);
            }
        }
        #endregion

        #region SetGridTitle
        private void SetGridTitle()
        {
            //realGrid 방식
            //그리드 컬럼 데이터를 JSON string으로 변환합니다.
            jsCol = ConvertJSONData.ConvertColArrToJSON(arrColumn, arrColumnCaption, arrColumnWidth, "cols");
            jsField = ConvertJSONData.ConvertColArrToJSON(arrColumn, arrColumnCaption, arrColumnWidth, "fields");
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbDevId.Text = Dictionary_Data.SearchDic("DEV_ID", bp.g_language);
        }
        #endregion

        #region GetData
        public void GetData(string flag)
        {
            DataSet ds = new DataSet();
            string strTableName = string.Empty;
            string strMessage = string.Empty;

            strDBName = "GPDB";
            strQueryID = "Info30Data.Get_Spec";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("PLANT_CD", bp.g_plant.ToString());
            sParam.Add("SHOP_CD", ddlShopCd.SelectedValue);
            sParam.Add("LINE_CD", ddlLineCd.SelectedValue);
            sParam.Add("DEV_ID", ddlDevCd.SelectedValue);
            sParam.Add("CAR_TYPE", ddlCarType.SelectedValue);
            sParam.Add("FLAG", flag);

            sParam.Add("CUR_MENU_ID", "Info30");        // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            FW.Data.Parameters logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_START");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Info30");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            // 비지니스 메서드 호출
            ds = biz.GetDataSet(strDBName, strQueryID, sParam);

            logParam = new FW.Data.Parameters();
            logParam.Add("BUTTON_ID", "btnSearch");
            logParam.Add("IP", bp.g_IP.ToString());
            logParam.Add("REMARK", "Search_END");

            logParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
            logParam.Add("CUD_TYPE", "C");                     // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
            logParam.Add("CUR_MENU_ID", "Info30");             // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

            btnlog.SetCUD(strDBName, "ButtonStatisticsData.Set_ButtonHisInfo", logParam);// 버튼로그 저장

            if (ds.Tables.Count > 0)
            {
                strTableName = ds.Tables[0].TableName.ToString();

                if (strTableName == "ErrorLog")
                {
                    strErrMessage = ds.Tables[0].Rows[0][1].ToString();

                    ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "createGrid();", true);

                    ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_ErrorMessage('" + strErrMessage + "');", true);
                }
                else
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        jsData = ConvertJSONData.ConvertDataTableToJSON(ds.Tables[0], strKeyColumn);

                        //정상처리되면
                        if (!String.IsNullOrEmpty(jsData))
                        {
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            string script = $@" data = {jsData}; 
                            createGrid('" + strFix + "'); ";

                            for(int i=0; i < ds.Tables[1].Rows.Count; i++ )
                                script += "gridView.columnByName(\"SPEC_"+ ds.Tables[1].Rows[i]["NUM"].ToString() + "\").header.text = \""+ ds.Tables[1].Rows[i]["INS_NM"].ToString() + "\";";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);
                        }
                        else
                        {
                            // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                            string script = $@" data = ''; 
                            createGrid(); ";

                            //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                            ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);

                            ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData();", true);
                        }

                    }
                    else
                    {
                        // 클라이언트 사이드 변수에 JSON 데이터 할당 및 'search' 함수 호출
                        string script = $@" data = ''; 
                            createGrid(); ";

                        //직렬화된 JSON을 자바스크립트 변수에 적용합니다.
                        ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", script, true);

                        ScriptManager.RegisterStartupScript(Page, this.GetType(), Guid.NewGuid().ToString(), "fn_NoData();", true);
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(Page, this.GetType(), "searchData", "createGrid();", true);

                ScriptManager.RegisterStartupScript(Page, this.GetType(), "Faile", "fn_NoData();", true);
            }
        }
        #endregion

        #region btnSearch_Click
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            GetData("N");
        }
        #endregion

        #region btnRestore_Click
        protected void btnRestore_Click(object sender, EventArgs e)
        {
            GetData("Y");
        }
        #endregion

        #region ddlShopCd_SelectedIndexChanged
        protected void ddlShopCd_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlLineCd.Items.Clear();
            ddlDevCd.Items.Clear();
            ddlCarType.Items.Clear();

            //비활성
            ddlLineCd.Enabled = false;
            ddlDevCd.Enabled = false;
            ddlCarType.Enabled = false;

            //초기화
            ddlLineCd.Items.Add(new ListItem("ALL", ""));
            ddlDevCd.Items.Add(new ListItem("ALL", ""));
            ddlCarType.Items.Add(new ListItem("ALL", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info30Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            //param.Add("PLANT_CD", "P1");
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", "");

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Line code 있으면
                if (ds.Tables[1].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        ddlLineCd.Items.Add(new ListItem(ds.Tables[1].Rows[i]["CODE_NM"].ToString(), ds.Tables[1].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlLineCd.Enabled = true;
                }
                //Device Code 있으면
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        ddlDevCd.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevCd.Enabled = true;
                }
                //Car Type 있으면
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarType.Enabled = true;
                }
            }
        }
        #endregion

        #region ddlLineCd_SelectedIndexChanged
        protected void ddlLineCd_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();

            //데이터 클리어
            ddlDevCd.Items.Clear();
            ddlCarType.Items.Clear();

            //비활성
            ddlDevCd.Enabled = false;
            ddlCarType.Enabled = false;

            //초기화
            ddlDevCd.Items.Add(new ListItem("ALL", ""));
            ddlCarType.Items.Add(new ListItem("ALL", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info30Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            //param.Add("PLANT_CD", "P1");
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", ddlLineCd.SelectedValue);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                //Device Code 있으면
                if (ds.Tables[2].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[2].Rows.Count; i++)
                    {
                        ddlDevCd.Items.Add(new ListItem(ds.Tables[2].Rows[i]["CODE_NM"].ToString(), ds.Tables[2].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlDevCd.Enabled = true;
                }
                //Car Type 있으면
                if (ds.Tables[3].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[3].Rows.Count; i++)
                    {
                        ddlCarType.Items.Add(new ListItem(ds.Tables[3].Rows[i]["CODE_NM"].ToString(), ds.Tables[3].Rows[i]["CODE_ID"].ToString()));
                    }
                    ddlCarType.Enabled = true;
                }
            }
        }
        #endregion
    }
}