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

namespace HQCWeb.InfoMgt.Info30
{
    public partial class Info30_p01 : System.Web.UI.Page
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
        Biz.InfoManagement.Info30 biz = new Biz.InfoManagement.Info30();

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

            ddlShopCd.Items.Add(new ListItem("선택하세요.", ""));
            ddlLineCd.Items.Add(new ListItem("선택하세요.", ""));
            ddlDevCd.Items.Add(new ListItem("선택하세요.", ""));
            ddlCarType.Items.Add(new ListItem("선택하세요.", ""));

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
                for (int i = 0; i < ds.Tables[4].Rows.Count; i++)
                {
                    ddlUseYN.Items.Add(new ListItem(ds.Tables[4].Rows[i]["CODE_NM"].ToString(), ds.Tables[4].Rows[i]["CODE_ID"].ToString()));
                }
                ddlUseYN.SelectedValue = "Y";
            }
        }
        #endregion

        #region SetTitle
        private void SetTitle()
        {
            // 등록일경우
            lbWorkName.Text = Dictionary_Data.SearchDic("REGISTRATION", bp.g_language); // 등록

            lbShopCd.Text = Dictionary_Data.SearchDic("SHOP_CD", bp.g_language);
            lbLineCd.Text = Dictionary_Data.SearchDic("LINE_CD", bp.g_language);
            lbDevId.Text = Dictionary_Data.SearchDic("DEV_ID", bp.g_language);
            lbCarType.Text = Dictionary_Data.SearchDic("CAR_TYPE", bp.g_language);
            lbUseYn.Text = Dictionary_Data.SearchDic("USE_YN", bp.g_language);

            lbItem001.Text = Dictionary_Data.SearchDic("SPEC_001", bp.g_language);
            lbItem002.Text = Dictionary_Data.SearchDic("SPEC_002", bp.g_language);
            lbItem003.Text = Dictionary_Data.SearchDic("SPEC_003", bp.g_language);
            lbItem004.Text = Dictionary_Data.SearchDic("SPEC_004", bp.g_language);
            lbItem005.Text = Dictionary_Data.SearchDic("SPEC_005", bp.g_language);
            lbItem006.Text = Dictionary_Data.SearchDic("SPEC_006", bp.g_language);
            lbItem007.Text = Dictionary_Data.SearchDic("SPEC_007", bp.g_language);
            lbItem008.Text = Dictionary_Data.SearchDic("SPEC_008", bp.g_language);
            lbItem009.Text = Dictionary_Data.SearchDic("SPEC_009", bp.g_language);
            lbItem010.Text = Dictionary_Data.SearchDic("SPEC_010", bp.g_language);
            lbItem011.Text = Dictionary_Data.SearchDic("SPEC_011", bp.g_language);
            lbItem012.Text = Dictionary_Data.SearchDic("SPEC_012", bp.g_language);
            lbItem013.Text = Dictionary_Data.SearchDic("SPEC_013", bp.g_language);
            lbItem014.Text = Dictionary_Data.SearchDic("SPEC_014", bp.g_language);
            lbItem015.Text = Dictionary_Data.SearchDic("SPEC_015", bp.g_language);
            lbItem016.Text = Dictionary_Data.SearchDic("SPEC_016", bp.g_language);
            lbItem017.Text = Dictionary_Data.SearchDic("SPEC_017", bp.g_language);
            lbItem018.Text = Dictionary_Data.SearchDic("SPEC_018", bp.g_language);
            lbItem019.Text = Dictionary_Data.SearchDic("SPEC_019", bp.g_language);
            lbItem020.Text = Dictionary_Data.SearchDic("SPEC_020", bp.g_language);
            lbItem021.Text = Dictionary_Data.SearchDic("SPEC_021", bp.g_language);
            lbItem022.Text = Dictionary_Data.SearchDic("SPEC_022", bp.g_language);
            lbItem023.Text = Dictionary_Data.SearchDic("SPEC_023", bp.g_language);
            lbItem024.Text = Dictionary_Data.SearchDic("SPEC_024", bp.g_language);
            lbItem025.Text = Dictionary_Data.SearchDic("SPEC_025", bp.g_language);
            lbItem026.Text = Dictionary_Data.SearchDic("SPEC_026", bp.g_language);
            lbItem027.Text = Dictionary_Data.SearchDic("SPEC_027", bp.g_language);
            lbItem028.Text = Dictionary_Data.SearchDic("SPEC_028", bp.g_language);
            lbItem029.Text = Dictionary_Data.SearchDic("SPEC_029", bp.g_language);
            lbItem030.Text = Dictionary_Data.SearchDic("SPEC_030", bp.g_language);
            lbItem031.Text = Dictionary_Data.SearchDic("SPEC_031", bp.g_language);
            lbItem032.Text = Dictionary_Data.SearchDic("SPEC_032", bp.g_language);
            lbItem033.Text = Dictionary_Data.SearchDic("SPEC_033", bp.g_language);
            lbItem034.Text = Dictionary_Data.SearchDic("SPEC_034", bp.g_language);
            lbItem035.Text = Dictionary_Data.SearchDic("SPEC_035", bp.g_language);
            lbItem036.Text = Dictionary_Data.SearchDic("SPEC_036", bp.g_language);
            lbItem037.Text = Dictionary_Data.SearchDic("SPEC_037", bp.g_language);
            lbItem038.Text = Dictionary_Data.SearchDic("SPEC_038", bp.g_language);
            lbItem039.Text = Dictionary_Data.SearchDic("SPEC_039", bp.g_language);
            lbItem040.Text = Dictionary_Data.SearchDic("SPEC_040", bp.g_language);
            lbItem041.Text = Dictionary_Data.SearchDic("SPEC_041", bp.g_language);
            lbItem042.Text = Dictionary_Data.SearchDic("SPEC_042", bp.g_language);
            lbItem043.Text = Dictionary_Data.SearchDic("SPEC_043", bp.g_language);
            lbItem044.Text = Dictionary_Data.SearchDic("SPEC_044", bp.g_language);
            lbItem045.Text = Dictionary_Data.SearchDic("SPEC_045", bp.g_language);
            lbItem046.Text = Dictionary_Data.SearchDic("SPEC_046", bp.g_language);
            lbItem047.Text = Dictionary_Data.SearchDic("SPEC_047", bp.g_language);
            lbItem048.Text = Dictionary_Data.SearchDic("SPEC_048", bp.g_language);
            lbItem049.Text = Dictionary_Data.SearchDic("SPEC_049", bp.g_language);
            lbItem050.Text = Dictionary_Data.SearchDic("SPEC_050", bp.g_language);
            lbItem051.Text = Dictionary_Data.SearchDic("SPEC_051", bp.g_language);
            lbItem052.Text = Dictionary_Data.SearchDic("SPEC_052", bp.g_language);
            lbItem053.Text = Dictionary_Data.SearchDic("SPEC_053", bp.g_language);
            lbItem054.Text = Dictionary_Data.SearchDic("SPEC_054", bp.g_language);
            lbItem055.Text = Dictionary_Data.SearchDic("SPEC_055", bp.g_language);
            lbItem056.Text = Dictionary_Data.SearchDic("SPEC_056", bp.g_language);
            lbItem057.Text = Dictionary_Data.SearchDic("SPEC_057", bp.g_language);
            lbItem058.Text = Dictionary_Data.SearchDic("SPEC_058", bp.g_language);
            lbItem059.Text = Dictionary_Data.SearchDic("SPEC_059", bp.g_language);
            lbItem060.Text = Dictionary_Data.SearchDic("SPEC_060", bp.g_language);
            lbItem061.Text = Dictionary_Data.SearchDic("SPEC_061", bp.g_language);
            lbItem062.Text = Dictionary_Data.SearchDic("SPEC_062", bp.g_language);
            lbItem063.Text = Dictionary_Data.SearchDic("SPEC_063", bp.g_language);
            lbItem064.Text = Dictionary_Data.SearchDic("SPEC_064", bp.g_language);
            lbItem065.Text = Dictionary_Data.SearchDic("SPEC_065", bp.g_language);
            lbItem066.Text = Dictionary_Data.SearchDic("SPEC_066", bp.g_language);
            lbItem067.Text = Dictionary_Data.SearchDic("SPEC_067", bp.g_language);
            lbItem068.Text = Dictionary_Data.SearchDic("SPEC_068", bp.g_language);
            lbItem069.Text = Dictionary_Data.SearchDic("SPEC_069", bp.g_language);
            lbItem070.Text = Dictionary_Data.SearchDic("SPEC_070", bp.g_language);
            lbItem071.Text = Dictionary_Data.SearchDic("SPEC_071", bp.g_language);
            lbItem072.Text = Dictionary_Data.SearchDic("SPEC_072", bp.g_language);
            lbItem073.Text = Dictionary_Data.SearchDic("SPEC_073", bp.g_language);
            lbItem074.Text = Dictionary_Data.SearchDic("SPEC_074", bp.g_language);
            lbItem075.Text = Dictionary_Data.SearchDic("SPEC_075", bp.g_language);
            lbItem076.Text = Dictionary_Data.SearchDic("SPEC_076", bp.g_language);
            lbItem077.Text = Dictionary_Data.SearchDic("SPEC_077", bp.g_language);
            lbItem078.Text = Dictionary_Data.SearchDic("SPEC_078", bp.g_language);
            lbItem079.Text = Dictionary_Data.SearchDic("SPEC_079", bp.g_language);
            lbItem080.Text = Dictionary_Data.SearchDic("SPEC_080", bp.g_language);
            lbItem081.Text = Dictionary_Data.SearchDic("SPEC_081", bp.g_language);
            lbItem082.Text = Dictionary_Data.SearchDic("SPEC_082", bp.g_language);
            lbItem083.Text = Dictionary_Data.SearchDic("SPEC_083", bp.g_language);
            lbItem084.Text = Dictionary_Data.SearchDic("SPEC_084", bp.g_language);
            lbItem085.Text = Dictionary_Data.SearchDic("SPEC_085", bp.g_language);
            lbItem086.Text = Dictionary_Data.SearchDic("SPEC_086", bp.g_language);
            lbItem087.Text = Dictionary_Data.SearchDic("SPEC_087", bp.g_language);
            lbItem088.Text = Dictionary_Data.SearchDic("SPEC_088", bp.g_language);
            lbItem089.Text = Dictionary_Data.SearchDic("SPEC_089", bp.g_language);
            lbItem090.Text = Dictionary_Data.SearchDic("SPEC_090", bp.g_language);
            lbItem091.Text = Dictionary_Data.SearchDic("SPEC_091", bp.g_language);
            lbItem092.Text = Dictionary_Data.SearchDic("SPEC_092", bp.g_language);
            lbItem093.Text = Dictionary_Data.SearchDic("SPEC_093", bp.g_language);
            lbItem094.Text = Dictionary_Data.SearchDic("SPEC_094", bp.g_language);
            lbItem095.Text = Dictionary_Data.SearchDic("SPEC_095", bp.g_language);
            lbItem096.Text = Dictionary_Data.SearchDic("SPEC_096", bp.g_language);
            lbItem097.Text = Dictionary_Data.SearchDic("SPEC_097", bp.g_language);
            lbItem098.Text = Dictionary_Data.SearchDic("SPEC_098", bp.g_language);
            lbItem099.Text = Dictionary_Data.SearchDic("SPEC_099", bp.g_language);
            lbItem100.Text = Dictionary_Data.SearchDic("SPEC_100", bp.g_language);
            lbItem101.Text = Dictionary_Data.SearchDic("SPEC_101", bp.g_language);
            lbItem102.Text = Dictionary_Data.SearchDic("SPEC_102", bp.g_language);
            lbItem103.Text = Dictionary_Data.SearchDic("SPEC_103", bp.g_language);
            lbItem104.Text = Dictionary_Data.SearchDic("SPEC_104", bp.g_language);
            lbItem105.Text = Dictionary_Data.SearchDic("SPEC_105", bp.g_language);
            lbItem106.Text = Dictionary_Data.SearchDic("SPEC_106", bp.g_language);
            lbItem107.Text = Dictionary_Data.SearchDic("SPEC_107", bp.g_language);
            lbItem108.Text = Dictionary_Data.SearchDic("SPEC_108", bp.g_language);
            lbItem109.Text = Dictionary_Data.SearchDic("SPEC_109", bp.g_language);
            lbItem110.Text = Dictionary_Data.SearchDic("SPEC_110", bp.g_language);
            lbItem111.Text = Dictionary_Data.SearchDic("SPEC_111", bp.g_language);
            lbItem112.Text = Dictionary_Data.SearchDic("SPEC_112", bp.g_language);
            lbItem113.Text = Dictionary_Data.SearchDic("SPEC_113", bp.g_language);
            lbItem114.Text = Dictionary_Data.SearchDic("SPEC_114", bp.g_language);
            lbItem115.Text = Dictionary_Data.SearchDic("SPEC_115", bp.g_language);
            lbItem116.Text = Dictionary_Data.SearchDic("SPEC_116", bp.g_language);
            lbItem117.Text = Dictionary_Data.SearchDic("SPEC_117", bp.g_language);
            lbItem118.Text = Dictionary_Data.SearchDic("SPEC_118", bp.g_language);
            lbItem119.Text = Dictionary_Data.SearchDic("SPEC_119", bp.g_language);
            lbItem120.Text = Dictionary_Data.SearchDic("SPEC_120", bp.g_language);
            lbItem121.Text = Dictionary_Data.SearchDic("SPEC_121", bp.g_language);
            lbItem122.Text = Dictionary_Data.SearchDic("SPEC_122", bp.g_language);
            lbItem123.Text = Dictionary_Data.SearchDic("SPEC_123", bp.g_language);
            lbItem124.Text = Dictionary_Data.SearchDic("SPEC_124", bp.g_language);
            lbItem125.Text = Dictionary_Data.SearchDic("SPEC_125", bp.g_language);
            lbItem126.Text = Dictionary_Data.SearchDic("SPEC_126", bp.g_language);
            lbItem127.Text = Dictionary_Data.SearchDic("SPEC_127", bp.g_language);
            lbItem128.Text = Dictionary_Data.SearchDic("SPEC_128", bp.g_language);
            lbItem129.Text = Dictionary_Data.SearchDic("SPEC_129", bp.g_language);
            lbItem130.Text = Dictionary_Data.SearchDic("SPEC_130", bp.g_language);
            lbItem131.Text = Dictionary_Data.SearchDic("SPEC_131", bp.g_language);
            lbItem132.Text = Dictionary_Data.SearchDic("SPEC_132", bp.g_language);
            lbItem133.Text = Dictionary_Data.SearchDic("SPEC_133", bp.g_language);
            lbItem134.Text = Dictionary_Data.SearchDic("SPEC_134", bp.g_language);
            lbItem135.Text = Dictionary_Data.SearchDic("SPEC_135", bp.g_language);
            lbItem136.Text = Dictionary_Data.SearchDic("SPEC_136", bp.g_language);
            lbItem137.Text = Dictionary_Data.SearchDic("SPEC_137", bp.g_language);
            lbItem138.Text = Dictionary_Data.SearchDic("SPEC_138", bp.g_language);
            lbItem139.Text = Dictionary_Data.SearchDic("SPEC_139", bp.g_language);
            lbItem140.Text = Dictionary_Data.SearchDic("SPEC_140", bp.g_language);
            lbItem141.Text = Dictionary_Data.SearchDic("SPEC_141", bp.g_language);
            lbItem142.Text = Dictionary_Data.SearchDic("SPEC_142", bp.g_language);
            lbItem143.Text = Dictionary_Data.SearchDic("SPEC_143", bp.g_language);
            lbItem144.Text = Dictionary_Data.SearchDic("SPEC_144", bp.g_language);
            lbItem145.Text = Dictionary_Data.SearchDic("SPEC_145", bp.g_language);
            lbItem146.Text = Dictionary_Data.SearchDic("SPEC_146", bp.g_language);
            lbItem147.Text = Dictionary_Data.SearchDic("SPEC_147", bp.g_language);
            lbItem148.Text = Dictionary_Data.SearchDic("SPEC_148", bp.g_language);
            lbItem149.Text = Dictionary_Data.SearchDic("SPEC_149", bp.g_language);
            lbItem150.Text = Dictionary_Data.SearchDic("SPEC_150", bp.g_language);
            lbItem151.Text = Dictionary_Data.SearchDic("SPEC_151", bp.g_language);
            lbItem152.Text = Dictionary_Data.SearchDic("SPEC_152", bp.g_language);
            lbItem153.Text = Dictionary_Data.SearchDic("SPEC_153", bp.g_language);
            lbItem154.Text = Dictionary_Data.SearchDic("SPEC_154", bp.g_language);
            lbItem155.Text = Dictionary_Data.SearchDic("SPEC_155", bp.g_language);
            lbItem156.Text = Dictionary_Data.SearchDic("SPEC_156", bp.g_language);
            lbItem157.Text = Dictionary_Data.SearchDic("SPEC_157", bp.g_language);
            lbItem158.Text = Dictionary_Data.SearchDic("SPEC_158", bp.g_language);
            lbItem159.Text = Dictionary_Data.SearchDic("SPEC_159", bp.g_language);
            lbItem160.Text = Dictionary_Data.SearchDic("SPEC_160", bp.g_language);
            lbItem161.Text = Dictionary_Data.SearchDic("SPEC_161", bp.g_language);
            lbItem162.Text = Dictionary_Data.SearchDic("SPEC_162", bp.g_language);
            lbItem163.Text = Dictionary_Data.SearchDic("SPEC_163", bp.g_language);
            lbItem164.Text = Dictionary_Data.SearchDic("SPEC_164", bp.g_language);
            lbItem165.Text = Dictionary_Data.SearchDic("SPEC_165", bp.g_language);
            lbItem166.Text = Dictionary_Data.SearchDic("SPEC_166", bp.g_language);
            lbItem167.Text = Dictionary_Data.SearchDic("SPEC_167", bp.g_language);
            lbItem168.Text = Dictionary_Data.SearchDic("SPEC_168", bp.g_language);
            lbItem169.Text = Dictionary_Data.SearchDic("SPEC_169", bp.g_language);
            lbItem170.Text = Dictionary_Data.SearchDic("SPEC_170", bp.g_language);
            lbItem171.Text = Dictionary_Data.SearchDic("SPEC_171", bp.g_language);
            lbItem172.Text = Dictionary_Data.SearchDic("SPEC_172", bp.g_language);
            lbItem173.Text = Dictionary_Data.SearchDic("SPEC_173", bp.g_language);
            lbItem174.Text = Dictionary_Data.SearchDic("SPEC_174", bp.g_language);
            lbItem175.Text = Dictionary_Data.SearchDic("SPEC_175", bp.g_language);
            lbItem176.Text = Dictionary_Data.SearchDic("SPEC_176", bp.g_language);
            lbItem177.Text = Dictionary_Data.SearchDic("SPEC_177", bp.g_language);
            lbItem178.Text = Dictionary_Data.SearchDic("SPEC_178", bp.g_language);
            lbItem179.Text = Dictionary_Data.SearchDic("SPEC_179", bp.g_language);
            lbItem180.Text = Dictionary_Data.SearchDic("SPEC_180", bp.g_language);
            lbItem181.Text = Dictionary_Data.SearchDic("SPEC_181", bp.g_language);
            lbItem182.Text = Dictionary_Data.SearchDic("SPEC_182", bp.g_language);
            lbItem183.Text = Dictionary_Data.SearchDic("SPEC_183", bp.g_language);
            lbItem184.Text = Dictionary_Data.SearchDic("SPEC_184", bp.g_language);
            lbItem185.Text = Dictionary_Data.SearchDic("SPEC_185", bp.g_language);
            lbItem186.Text = Dictionary_Data.SearchDic("SPEC_186", bp.g_language);
            lbItem187.Text = Dictionary_Data.SearchDic("SPEC_187", bp.g_language);
            lbItem188.Text = Dictionary_Data.SearchDic("SPEC_188", bp.g_language);
            lbItem189.Text = Dictionary_Data.SearchDic("SPEC_189", bp.g_language);
            lbItem190.Text = Dictionary_Data.SearchDic("SPEC_190", bp.g_language);
            lbItem191.Text = Dictionary_Data.SearchDic("SPEC_191", bp.g_language);
            lbItem192.Text = Dictionary_Data.SearchDic("SPEC_192", bp.g_language);
            lbItem193.Text = Dictionary_Data.SearchDic("SPEC_193", bp.g_language);
            lbItem194.Text = Dictionary_Data.SearchDic("SPEC_194", bp.g_language);
            lbItem195.Text = Dictionary_Data.SearchDic("SPEC_195", bp.g_language);
            lbItem196.Text = Dictionary_Data.SearchDic("SPEC_196", bp.g_language);
            lbItem197.Text = Dictionary_Data.SearchDic("SPEC_197", bp.g_language);
            lbItem198.Text = Dictionary_Data.SearchDic("SPEC_198", bp.g_language);
            lbItem199.Text = Dictionary_Data.SearchDic("SPEC_199", bp.g_language);
            lbItem200.Text = Dictionary_Data.SearchDic("SPEC_200", bp.g_language);
            lbItem201.Text = Dictionary_Data.SearchDic("SPEC_201", bp.g_language);
            lbItem202.Text = Dictionary_Data.SearchDic("SPEC_202", bp.g_language);
            lbItem203.Text = Dictionary_Data.SearchDic("SPEC_203", bp.g_language);
            lbItem204.Text = Dictionary_Data.SearchDic("SPEC_204", bp.g_language);
            lbItem205.Text = Dictionary_Data.SearchDic("SPEC_205", bp.g_language);
            lbItem206.Text = Dictionary_Data.SearchDic("SPEC_206", bp.g_language);
            lbItem207.Text = Dictionary_Data.SearchDic("SPEC_207", bp.g_language);
            lbItem208.Text = Dictionary_Data.SearchDic("SPEC_208", bp.g_language);
            lbItem209.Text = Dictionary_Data.SearchDic("SPEC_209", bp.g_language);
            lbItem210.Text = Dictionary_Data.SearchDic("SPEC_210", bp.g_language);
            lbItem211.Text = Dictionary_Data.SearchDic("SPEC_211", bp.g_language);
            lbItem212.Text = Dictionary_Data.SearchDic("SPEC_212", bp.g_language);
            lbItem213.Text = Dictionary_Data.SearchDic("SPEC_213", bp.g_language);
            lbItem214.Text = Dictionary_Data.SearchDic("SPEC_214", bp.g_language);
            lbItem215.Text = Dictionary_Data.SearchDic("SPEC_215", bp.g_language);
            lbItem216.Text = Dictionary_Data.SearchDic("SPEC_216", bp.g_language);
            lbItem217.Text = Dictionary_Data.SearchDic("SPEC_217", bp.g_language);
            lbItem218.Text = Dictionary_Data.SearchDic("SPEC_218", bp.g_language);
            lbItem219.Text = Dictionary_Data.SearchDic("SPEC_219", bp.g_language);
            lbItem220.Text = Dictionary_Data.SearchDic("SPEC_220", bp.g_language);
            lbItem221.Text = Dictionary_Data.SearchDic("SPEC_221", bp.g_language);
            lbItem222.Text = Dictionary_Data.SearchDic("SPEC_222", bp.g_language);
            lbItem223.Text = Dictionary_Data.SearchDic("SPEC_223", bp.g_language);
            lbItem224.Text = Dictionary_Data.SearchDic("SPEC_224", bp.g_language);
            lbItem225.Text = Dictionary_Data.SearchDic("SPEC_225", bp.g_language);
            lbItem226.Text = Dictionary_Data.SearchDic("SPEC_226", bp.g_language);
            lbItem227.Text = Dictionary_Data.SearchDic("SPEC_227", bp.g_language);
            lbItem228.Text = Dictionary_Data.SearchDic("SPEC_228", bp.g_language);
            lbItem229.Text = Dictionary_Data.SearchDic("SPEC_229", bp.g_language);
            lbItem230.Text = Dictionary_Data.SearchDic("SPEC_230", bp.g_language);
            lbItem231.Text = Dictionary_Data.SearchDic("SPEC_231", bp.g_language);
            lbItem232.Text = Dictionary_Data.SearchDic("SPEC_232", bp.g_language);
            lbItem233.Text = Dictionary_Data.SearchDic("SPEC_233", bp.g_language);
            lbItem234.Text = Dictionary_Data.SearchDic("SPEC_234", bp.g_language);
            lbItem235.Text = Dictionary_Data.SearchDic("SPEC_235", bp.g_language);
            lbItem236.Text = Dictionary_Data.SearchDic("SPEC_236", bp.g_language);
            lbItem237.Text = Dictionary_Data.SearchDic("SPEC_237", bp.g_language);
            lbItem238.Text = Dictionary_Data.SearchDic("SPEC_238", bp.g_language);
            lbItem239.Text = Dictionary_Data.SearchDic("SPEC_239", bp.g_language);
            lbItem240.Text = Dictionary_Data.SearchDic("SPEC_240", bp.g_language);
            lbItem241.Text = Dictionary_Data.SearchDic("SPEC_241", bp.g_language);
            lbItem242.Text = Dictionary_Data.SearchDic("SPEC_242", bp.g_language);
            lbItem243.Text = Dictionary_Data.SearchDic("SPEC_243", bp.g_language);
            lbItem244.Text = Dictionary_Data.SearchDic("SPEC_244", bp.g_language);
            lbItem245.Text = Dictionary_Data.SearchDic("SPEC_245", bp.g_language);
            lbItem246.Text = Dictionary_Data.SearchDic("SPEC_246", bp.g_language);
            lbItem247.Text = Dictionary_Data.SearchDic("SPEC_247", bp.g_language);
            lbItem248.Text = Dictionary_Data.SearchDic("SPEC_248", bp.g_language);
            lbItem249.Text = Dictionary_Data.SearchDic("SPEC_249", bp.g_language);
            lbItem250.Text = Dictionary_Data.SearchDic("SPEC_250", bp.g_language);
            lbItem251.Text = Dictionary_Data.SearchDic("SPEC_251", bp.g_language);
            lbItem252.Text = Dictionary_Data.SearchDic("SPEC_252", bp.g_language);
            lbItem253.Text = Dictionary_Data.SearchDic("SPEC_253", bp.g_language);
            lbItem254.Text = Dictionary_Data.SearchDic("SPEC_254", bp.g_language);
            lbItem255.Text = Dictionary_Data.SearchDic("SPEC_255", bp.g_language);
            lbItem256.Text = Dictionary_Data.SearchDic("SPEC_256", bp.g_language);
            lbItem257.Text = Dictionary_Data.SearchDic("SPEC_257", bp.g_language);
            lbItem258.Text = Dictionary_Data.SearchDic("SPEC_258", bp.g_language);
            lbItem259.Text = Dictionary_Data.SearchDic("SPEC_259", bp.g_language);
            lbItem260.Text = Dictionary_Data.SearchDic("SPEC_260", bp.g_language);
            lbItem261.Text = Dictionary_Data.SearchDic("SPEC_261", bp.g_language);
            lbItem262.Text = Dictionary_Data.SearchDic("SPEC_262", bp.g_language);
            lbItem263.Text = Dictionary_Data.SearchDic("SPEC_263", bp.g_language);
            lbItem264.Text = Dictionary_Data.SearchDic("SPEC_264", bp.g_language);
            lbItem265.Text = Dictionary_Data.SearchDic("SPEC_265", bp.g_language);
            lbItem266.Text = Dictionary_Data.SearchDic("SPEC_266", bp.g_language);
            lbItem267.Text = Dictionary_Data.SearchDic("SPEC_267", bp.g_language);
            lbItem268.Text = Dictionary_Data.SearchDic("SPEC_268", bp.g_language);
            lbItem269.Text = Dictionary_Data.SearchDic("SPEC_269", bp.g_language);
            lbItem270.Text = Dictionary_Data.SearchDic("SPEC_270", bp.g_language);
            lbItem271.Text = Dictionary_Data.SearchDic("SPEC_271", bp.g_language);
            lbItem272.Text = Dictionary_Data.SearchDic("SPEC_272", bp.g_language);
            lbItem273.Text = Dictionary_Data.SearchDic("SPEC_273", bp.g_language);
            lbItem274.Text = Dictionary_Data.SearchDic("SPEC_274", bp.g_language);
            lbItem275.Text = Dictionary_Data.SearchDic("SPEC_275", bp.g_language);
            lbItem276.Text = Dictionary_Data.SearchDic("SPEC_276", bp.g_language);
            lbItem277.Text = Dictionary_Data.SearchDic("SPEC_277", bp.g_language);
            lbItem278.Text = Dictionary_Data.SearchDic("SPEC_278", bp.g_language);
            lbItem279.Text = Dictionary_Data.SearchDic("SPEC_279", bp.g_language);
            lbItem280.Text = Dictionary_Data.SearchDic("SPEC_280", bp.g_language);
            lbItem281.Text = Dictionary_Data.SearchDic("SPEC_281", bp.g_language);
            lbItem282.Text = Dictionary_Data.SearchDic("SPEC_282", bp.g_language);
            lbItem283.Text = Dictionary_Data.SearchDic("SPEC_283", bp.g_language);
            lbItem284.Text = Dictionary_Data.SearchDic("SPEC_284", bp.g_language);
            lbItem285.Text = Dictionary_Data.SearchDic("SPEC_285", bp.g_language);
            lbItem286.Text = Dictionary_Data.SearchDic("SPEC_286", bp.g_language);
            lbItem287.Text = Dictionary_Data.SearchDic("SPEC_287", bp.g_language);
            lbItem288.Text = Dictionary_Data.SearchDic("SPEC_288", bp.g_language);
            lbItem289.Text = Dictionary_Data.SearchDic("SPEC_289", bp.g_language);
            lbItem290.Text = Dictionary_Data.SearchDic("SPEC_290", bp.g_language);
            lbItem291.Text = Dictionary_Data.SearchDic("SPEC_291", bp.g_language);
            lbItem292.Text = Dictionary_Data.SearchDic("SPEC_292", bp.g_language);
            lbItem293.Text = Dictionary_Data.SearchDic("SPEC_293", bp.g_language);
            lbItem294.Text = Dictionary_Data.SearchDic("SPEC_294", bp.g_language);
            lbItem295.Text = Dictionary_Data.SearchDic("SPEC_295", bp.g_language);
            lbItem296.Text = Dictionary_Data.SearchDic("SPEC_296", bp.g_language);
            lbItem297.Text = Dictionary_Data.SearchDic("SPEC_297", bp.g_language);
            lbItem298.Text = Dictionary_Data.SearchDic("SPEC_298", bp.g_language);
            lbItem299.Text = Dictionary_Data.SearchDic("SPEC_299", bp.g_language);
            lbItem300.Text = Dictionary_Data.SearchDic("SPEC_300", bp.g_language);

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

            if (su.SetControlValChk(this.Page.Master, "PopupContent"))
            {
                strDBName = "GPDB";
                strQueryID = "Info30Data.Get_SpecID_ValChk";

                FW.Data.Parameters sParamIDChk = new FW.Data.Parameters();
                sParamIDChk.Add("PLANT_CD", bp.g_plant.ToString());
                sParamIDChk.Add("SHOP_CD", ddlShopCd.SelectedValue);
                sParamIDChk.Add("LINE_CD", ddlLineCd.SelectedValue);
                sParamIDChk.Add("DEV_ID", ddlDevCd.SelectedValue);
                sParamIDChk.Add("CAR_TYPE", ddlCarType.SelectedValue);

                // 아이디 체크 비지니스 메서드 작성
                strRtn = biz.GetIDValChk(strDBName, strQueryID, sParamIDChk);
                if (strRtn == "0")
                {
                    strDBName = "GPDB";
                    strQueryID = "Info30Data.Set_SpecInfo";

                    FW.Data.Parameters sParam = new FW.Data.Parameters();
                    sParam.Add("PLANT_CD", bp.g_plant.ToString());
                    sParam.Add("SHOP_CD", ddlShopCd.SelectedValue);
                    sParam.Add("LINE_CD", ddlLineCd.SelectedValue);
                    sParam.Add("DEV_ID", ddlDevCd.SelectedValue);
                    sParam.Add("CAR_TYPE", ddlCarType.SelectedValue);

                    sParam.Add("ITEM_01", txtItem001.Text);
                    sParam.Add("ITEM_02", txtItem002.Text);
                    sParam.Add("ITEM_03", txtItem003.Text);
                    sParam.Add("ITEM_04", txtItem004.Text);
                    sParam.Add("ITEM_05", txtItem005.Text);
                    sParam.Add("ITEM_06", txtItem006.Text);
                    sParam.Add("ITEM_07", txtItem007.Text);
                    sParam.Add("ITEM_08", txtItem008.Text);
                    sParam.Add("ITEM_09", txtItem009.Text);
                    sParam.Add("ITEM_10", txtItem010.Text);
                    sParam.Add("ITEM_11", txtItem011.Text);
                    sParam.Add("ITEM_12", txtItem012.Text);
                    sParam.Add("ITEM_13", txtItem013.Text);
                    sParam.Add("ITEM_14", txtItem014.Text);
                    sParam.Add("ITEM_15", txtItem015.Text);
                    sParam.Add("ITEM_16", txtItem016.Text);
                    sParam.Add("ITEM_17", txtItem017.Text);
                    sParam.Add("ITEM_18", txtItem018.Text);
                    sParam.Add("ITEM_19", txtItem019.Text);
                    sParam.Add("ITEM_20", txtItem020.Text);
                    sParam.Add("ITEM_21", txtItem021.Text);
                    sParam.Add("ITEM_22", txtItem022.Text);
                    sParam.Add("ITEM_23", txtItem023.Text);
                    sParam.Add("ITEM_24", txtItem024.Text);
                    sParam.Add("ITEM_25", txtItem025.Text);
                    sParam.Add("ITEM_26", txtItem026.Text);
                    sParam.Add("ITEM_27", txtItem027.Text);
                    sParam.Add("ITEM_28", txtItem028.Text);
                    sParam.Add("ITEM_29", txtItem029.Text);
                    sParam.Add("ITEM_30", txtItem030.Text);
                    sParam.Add("ITEM_31", txtItem031.Text);
                    sParam.Add("ITEM_32", txtItem032.Text);
                    sParam.Add("ITEM_33", txtItem033.Text);
                    sParam.Add("ITEM_34", txtItem034.Text);
                    sParam.Add("ITEM_35", txtItem035.Text);
                    sParam.Add("ITEM_36", txtItem036.Text);
                    sParam.Add("ITEM_37", txtItem037.Text);
                    sParam.Add("ITEM_38", txtItem038.Text);
                    sParam.Add("ITEM_39", txtItem039.Text);
                    sParam.Add("ITEM_40", txtItem040.Text);
                    sParam.Add("ITEM_41", txtItem041.Text);
                    sParam.Add("ITEM_42", txtItem042.Text);
                    sParam.Add("ITEM_43", txtItem043.Text);
                    sParam.Add("ITEM_44", txtItem044.Text);
                    sParam.Add("ITEM_45", txtItem045.Text);
                    sParam.Add("ITEM_46", txtItem046.Text);
                    sParam.Add("ITEM_47", txtItem047.Text);
                    sParam.Add("ITEM_48", txtItem048.Text);
                    sParam.Add("ITEM_49", txtItem049.Text);
                    sParam.Add("ITEM_50", txtItem050.Text);
                    sParam.Add("ITEM_51", txtItem051.Text);
                    sParam.Add("ITEM_52", txtItem052.Text);
                    sParam.Add("ITEM_53", txtItem053.Text);
                    sParam.Add("ITEM_54", txtItem054.Text);
                    sParam.Add("ITEM_55", txtItem055.Text);
                    sParam.Add("ITEM_56", txtItem056.Text);
                    sParam.Add("ITEM_57", txtItem057.Text);
                    sParam.Add("ITEM_58", txtItem058.Text);
                    sParam.Add("ITEM_59", txtItem059.Text);
                    sParam.Add("ITEM_60", txtItem060.Text);
                    sParam.Add("ITEM_61", txtItem061.Text);
                    sParam.Add("ITEM_62", txtItem062.Text);
                    sParam.Add("ITEM_63", txtItem063.Text);
                    sParam.Add("ITEM_64", txtItem064.Text);
                    sParam.Add("ITEM_65", txtItem065.Text);
                    sParam.Add("ITEM_66", txtItem066.Text);
                    sParam.Add("ITEM_67", txtItem067.Text);
                    sParam.Add("ITEM_68", txtItem068.Text);
                    sParam.Add("ITEM_69", txtItem069.Text);
                    sParam.Add("ITEM_70", txtItem070.Text);
                    sParam.Add("ITEM_71", txtItem071.Text);
                    sParam.Add("ITEM_72", txtItem072.Text);
                    sParam.Add("ITEM_73", txtItem073.Text);
                    sParam.Add("ITEM_74", txtItem074.Text);
                    sParam.Add("ITEM_75", txtItem075.Text);
                    sParam.Add("ITEM_76", txtItem076.Text);
                    sParam.Add("ITEM_77", txtItem077.Text);
                    sParam.Add("ITEM_78", txtItem078.Text);
                    sParam.Add("ITEM_79", txtItem079.Text);
                    sParam.Add("ITEM_80", txtItem080.Text);
                    sParam.Add("ITEM_81", txtItem081.Text);
                    sParam.Add("ITEM_82", txtItem082.Text);
                    sParam.Add("ITEM_83", txtItem083.Text);
                    sParam.Add("ITEM_84", txtItem084.Text);
                    sParam.Add("ITEM_85", txtItem085.Text);
                    sParam.Add("ITEM_86", txtItem086.Text);
                    sParam.Add("ITEM_87", txtItem087.Text);
                    sParam.Add("ITEM_88", txtItem088.Text);
                    sParam.Add("ITEM_89", txtItem089.Text);
                    sParam.Add("ITEM_90", txtItem090.Text);
                    sParam.Add("ITEM_91", txtItem091.Text);
                    sParam.Add("ITEM_92", txtItem092.Text);
                    sParam.Add("ITEM_93", txtItem093.Text);
                    sParam.Add("ITEM_94", txtItem094.Text);
                    sParam.Add("ITEM_95", txtItem095.Text);
                    sParam.Add("ITEM_96", txtItem096.Text);
                    sParam.Add("ITEM_97", txtItem097.Text);
                    sParam.Add("ITEM_98", txtItem098.Text);
                    sParam.Add("ITEM_99", txtItem099.Text);
                    sParam.Add("ITEM_100", txtItem100.Text);
                    sParam.Add("ITEM_101", txtItem101.Text);
                    sParam.Add("ITEM_102", txtItem102.Text);
                    sParam.Add("ITEM_103", txtItem103.Text);
                    sParam.Add("ITEM_104", txtItem104.Text);
                    sParam.Add("ITEM_105", txtItem105.Text);
                    sParam.Add("ITEM_106", txtItem106.Text);
                    sParam.Add("ITEM_107", txtItem107.Text);
                    sParam.Add("ITEM_108", txtItem108.Text);
                    sParam.Add("ITEM_109", txtItem109.Text);
                    sParam.Add("ITEM_110", txtItem110.Text);
                    sParam.Add("ITEM_111", txtItem111.Text);
                    sParam.Add("ITEM_112", txtItem112.Text);
                    sParam.Add("ITEM_113", txtItem113.Text);
                    sParam.Add("ITEM_114", txtItem114.Text);
                    sParam.Add("ITEM_115", txtItem115.Text);
                    sParam.Add("ITEM_116", txtItem116.Text);
                    sParam.Add("ITEM_117", txtItem117.Text);
                    sParam.Add("ITEM_118", txtItem118.Text);
                    sParam.Add("ITEM_119", txtItem119.Text);
                    sParam.Add("ITEM_120", txtItem120.Text);
                    sParam.Add("ITEM_121", txtItem121.Text);
                    sParam.Add("ITEM_122", txtItem122.Text);
                    sParam.Add("ITEM_123", txtItem123.Text);
                    sParam.Add("ITEM_124", txtItem124.Text);
                    sParam.Add("ITEM_125", txtItem125.Text);
                    sParam.Add("ITEM_126", txtItem126.Text);
                    sParam.Add("ITEM_127", txtItem127.Text);
                    sParam.Add("ITEM_128", txtItem128.Text);
                    sParam.Add("ITEM_129", txtItem129.Text);
                    sParam.Add("ITEM_130", txtItem130.Text);
                    sParam.Add("ITEM_131", txtItem131.Text);
                    sParam.Add("ITEM_132", txtItem132.Text);
                    sParam.Add("ITEM_133", txtItem133.Text);
                    sParam.Add("ITEM_134", txtItem134.Text);
                    sParam.Add("ITEM_135", txtItem135.Text);
                    sParam.Add("ITEM_136", txtItem136.Text);
                    sParam.Add("ITEM_137", txtItem137.Text);
                    sParam.Add("ITEM_138", txtItem138.Text);
                    sParam.Add("ITEM_139", txtItem139.Text);
                    sParam.Add("ITEM_140", txtItem140.Text);
                    sParam.Add("ITEM_141", txtItem141.Text);
                    sParam.Add("ITEM_142", txtItem142.Text);
                    sParam.Add("ITEM_143", txtItem143.Text);
                    sParam.Add("ITEM_144", txtItem144.Text);
                    sParam.Add("ITEM_145", txtItem145.Text);
                    sParam.Add("ITEM_146", txtItem146.Text);
                    sParam.Add("ITEM_147", txtItem147.Text);
                    sParam.Add("ITEM_148", txtItem148.Text);
                    sParam.Add("ITEM_149", txtItem149.Text);
                    sParam.Add("ITEM_150", txtItem150.Text);
                    sParam.Add("ITEM_151", txtItem151.Text);
                    sParam.Add("ITEM_152", txtItem152.Text);
                    sParam.Add("ITEM_153", txtItem153.Text);
                    sParam.Add("ITEM_154", txtItem154.Text);
                    sParam.Add("ITEM_155", txtItem155.Text);
                    sParam.Add("ITEM_156", txtItem156.Text);
                    sParam.Add("ITEM_157", txtItem157.Text);
                    sParam.Add("ITEM_158", txtItem158.Text);
                    sParam.Add("ITEM_159", txtItem159.Text);
                    sParam.Add("ITEM_160", txtItem160.Text);
                    sParam.Add("ITEM_161", txtItem161.Text);
                    sParam.Add("ITEM_162", txtItem162.Text);
                    sParam.Add("ITEM_163", txtItem163.Text);
                    sParam.Add("ITEM_164", txtItem164.Text);
                    sParam.Add("ITEM_165", txtItem165.Text);
                    sParam.Add("ITEM_166", txtItem166.Text);
                    sParam.Add("ITEM_167", txtItem167.Text);
                    sParam.Add("ITEM_168", txtItem168.Text);
                    sParam.Add("ITEM_169", txtItem169.Text);
                    sParam.Add("ITEM_170", txtItem170.Text);
                    sParam.Add("ITEM_171", txtItem171.Text);
                    sParam.Add("ITEM_172", txtItem172.Text);
                    sParam.Add("ITEM_173", txtItem173.Text);
                    sParam.Add("ITEM_174", txtItem174.Text);
                    sParam.Add("ITEM_175", txtItem175.Text);
                    sParam.Add("ITEM_176", txtItem176.Text);
                    sParam.Add("ITEM_177", txtItem177.Text);
                    sParam.Add("ITEM_178", txtItem178.Text);
                    sParam.Add("ITEM_179", txtItem179.Text);
                    sParam.Add("ITEM_180", txtItem180.Text);
                    sParam.Add("ITEM_181", txtItem181.Text);
                    sParam.Add("ITEM_182", txtItem182.Text);
                    sParam.Add("ITEM_183", txtItem183.Text);
                    sParam.Add("ITEM_184", txtItem184.Text);
                    sParam.Add("ITEM_185", txtItem185.Text);
                    sParam.Add("ITEM_186", txtItem186.Text);
                    sParam.Add("ITEM_187", txtItem187.Text);
                    sParam.Add("ITEM_188", txtItem188.Text);
                    sParam.Add("ITEM_189", txtItem189.Text);
                    sParam.Add("ITEM_190", txtItem190.Text);
                    sParam.Add("ITEM_191", txtItem191.Text);
                    sParam.Add("ITEM_192", txtItem192.Text);
                    sParam.Add("ITEM_193", txtItem193.Text);
                    sParam.Add("ITEM_194", txtItem194.Text);
                    sParam.Add("ITEM_195", txtItem195.Text);
                    sParam.Add("ITEM_196", txtItem196.Text);
                    sParam.Add("ITEM_197", txtItem197.Text);
                    sParam.Add("ITEM_198", txtItem198.Text);
                    sParam.Add("ITEM_199", txtItem199.Text);
                    sParam.Add("ITEM_200", txtItem200.Text);
                    sParam.Add("ITEM_201", txtItem201.Text);
                    sParam.Add("ITEM_202", txtItem202.Text);
                    sParam.Add("ITEM_203", txtItem203.Text);
                    sParam.Add("ITEM_204", txtItem204.Text);
                    sParam.Add("ITEM_205", txtItem205.Text);
                    sParam.Add("ITEM_206", txtItem206.Text);
                    sParam.Add("ITEM_207", txtItem207.Text);
                    sParam.Add("ITEM_208", txtItem208.Text);
                    sParam.Add("ITEM_209", txtItem209.Text);
                    sParam.Add("ITEM_210", txtItem210.Text);
                    sParam.Add("ITEM_211", txtItem211.Text);
                    sParam.Add("ITEM_212", txtItem212.Text);
                    sParam.Add("ITEM_213", txtItem213.Text);
                    sParam.Add("ITEM_214", txtItem214.Text);
                    sParam.Add("ITEM_215", txtItem215.Text);
                    sParam.Add("ITEM_216", txtItem216.Text);
                    sParam.Add("ITEM_217", txtItem217.Text);
                    sParam.Add("ITEM_218", txtItem218.Text);
                    sParam.Add("ITEM_219", txtItem219.Text);
                    sParam.Add("ITEM_220", txtItem220.Text);
                    sParam.Add("ITEM_221", txtItem221.Text);
                    sParam.Add("ITEM_222", txtItem222.Text);
                    sParam.Add("ITEM_223", txtItem223.Text);
                    sParam.Add("ITEM_224", txtItem224.Text);
                    sParam.Add("ITEM_225", txtItem225.Text);
                    sParam.Add("ITEM_226", txtItem226.Text);
                    sParam.Add("ITEM_227", txtItem227.Text);
                    sParam.Add("ITEM_228", txtItem228.Text);
                    sParam.Add("ITEM_229", txtItem229.Text);
                    sParam.Add("ITEM_230", txtItem230.Text);
                    sParam.Add("ITEM_231", txtItem231.Text);
                    sParam.Add("ITEM_232", txtItem232.Text);
                    sParam.Add("ITEM_233", txtItem233.Text);
                    sParam.Add("ITEM_234", txtItem234.Text);
                    sParam.Add("ITEM_235", txtItem235.Text);
                    sParam.Add("ITEM_236", txtItem236.Text);
                    sParam.Add("ITEM_237", txtItem237.Text);
                    sParam.Add("ITEM_238", txtItem238.Text);
                    sParam.Add("ITEM_239", txtItem239.Text);
                    sParam.Add("ITEM_240", txtItem240.Text);
                    sParam.Add("ITEM_241", txtItem241.Text);
                    sParam.Add("ITEM_242", txtItem242.Text);
                    sParam.Add("ITEM_243", txtItem243.Text);
                    sParam.Add("ITEM_244", txtItem244.Text);
                    sParam.Add("ITEM_245", txtItem245.Text);
                    sParam.Add("ITEM_246", txtItem246.Text);
                    sParam.Add("ITEM_247", txtItem247.Text);
                    sParam.Add("ITEM_248", txtItem248.Text);
                    sParam.Add("ITEM_249", txtItem249.Text);
                    sParam.Add("ITEM_250", txtItem250.Text);
                    sParam.Add("ITEM_251", txtItem251.Text);
                    sParam.Add("ITEM_252", txtItem252.Text);
                    sParam.Add("ITEM_253", txtItem253.Text);
                    sParam.Add("ITEM_254", txtItem254.Text);
                    sParam.Add("ITEM_255", txtItem255.Text);
                    sParam.Add("ITEM_256", txtItem256.Text);
                    sParam.Add("ITEM_257", txtItem257.Text);
                    sParam.Add("ITEM_258", txtItem258.Text);
                    sParam.Add("ITEM_259", txtItem259.Text);
                    sParam.Add("ITEM_260", txtItem260.Text);
                    sParam.Add("ITEM_261", txtItem261.Text);
                    sParam.Add("ITEM_262", txtItem262.Text);
                    sParam.Add("ITEM_263", txtItem263.Text);
                    sParam.Add("ITEM_264", txtItem264.Text);
                    sParam.Add("ITEM_265", txtItem265.Text);
                    sParam.Add("ITEM_266", txtItem266.Text);
                    sParam.Add("ITEM_267", txtItem267.Text);
                    sParam.Add("ITEM_268", txtItem268.Text);
                    sParam.Add("ITEM_269", txtItem269.Text);
                    sParam.Add("ITEM_270", txtItem270.Text);
                    sParam.Add("ITEM_271", txtItem271.Text);
                    sParam.Add("ITEM_272", txtItem272.Text);
                    sParam.Add("ITEM_273", txtItem273.Text);
                    sParam.Add("ITEM_274", txtItem274.Text);
                    sParam.Add("ITEM_275", txtItem275.Text);
                    sParam.Add("ITEM_276", txtItem276.Text);
                    sParam.Add("ITEM_277", txtItem277.Text);
                    sParam.Add("ITEM_278", txtItem278.Text);
                    sParam.Add("ITEM_279", txtItem279.Text);
                    sParam.Add("ITEM_280", txtItem280.Text);
                    sParam.Add("ITEM_281", txtItem281.Text);
                    sParam.Add("ITEM_282", txtItem282.Text);
                    sParam.Add("ITEM_283", txtItem283.Text);
                    sParam.Add("ITEM_284", txtItem284.Text);
                    sParam.Add("ITEM_285", txtItem285.Text);
                    sParam.Add("ITEM_286", txtItem286.Text);
                    sParam.Add("ITEM_287", txtItem287.Text);
                    sParam.Add("ITEM_288", txtItem288.Text);
                    sParam.Add("ITEM_289", txtItem289.Text);
                    sParam.Add("ITEM_290", txtItem290.Text);
                    sParam.Add("ITEM_291", txtItem291.Text);
                    sParam.Add("ITEM_292", txtItem292.Text);
                    sParam.Add("ITEM_293", txtItem293.Text);
                    sParam.Add("ITEM_294", txtItem294.Text);
                    sParam.Add("ITEM_295", txtItem295.Text);
                    sParam.Add("ITEM_296", txtItem296.Text);
                    sParam.Add("ITEM_297", txtItem297.Text);
                    sParam.Add("ITEM_298", txtItem298.Text);
                    sParam.Add("ITEM_299", txtItem299.Text);
                    sParam.Add("ITEM_300", txtItem300.Text);

                    sParam.Add("USE_YN", ddlUseYN.SelectedValue);
                    sParam.Add("USER_ID", bp.g_userid.ToString());

                    sParam.Add("REG_ID", bp.g_userid.ToString());    // 등록자
                    sParam.Add("CUD_TYPE", "C");                       // 등록 : C / 수정 : U / 삭제 : D    -- 상태값중 한개 등록
                    sParam.Add("CUR_MENU_ID", "Info30");                 // 조회페이지 메뉴 아이디 입력 - 에러로그 생성시 메뉴 아이디 필요

                    // 등록 비지니스 메서드 작성
                    iRtn = biz.SetCUD(strDBName, strQueryID, sParam);

                    if (iRtn == 1)
                    {
                        strScript = " alert('정상등록 되었습니다.');  parent.fn_ModalReloadCall('Info30'); parent.fn_ModalCloseDiv(); ";
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
            ddlLineCd.Items.Add(new ListItem("선택하세요.", ""));
            ddlDevCd.Items.Add(new ListItem("선택하세요.", ""));
            ddlCarType.Items.Add(new ListItem("선택하세요.", ""));

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info30Data.Get_DdlData";

            FW.Data.Parameters param = new FW.Data.Parameters();
            //param.Add("PLANT_CD", "P1");
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", "");
            param.Add("STN_CD", "");

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
            ddlDevCd.Items.Add(new ListItem("선택하세요.", ""));
            ddlCarType.Items.Add(new ListItem("선택하세요.", ""));

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

        #region ddlDevId_ddlCarType_SelectedIndexChanged
        protected void ddlDevId_ddlCarType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //데이터셋 설정
            DataSet ds = new DataSet();

            //DB 연결 설정
            strDBName = "GPDB";
            strQueryID = "Info30Data.Get_SpecInfo";

            FW.Data.Parameters param = new FW.Data.Parameters();
            //param.Add("PLANT_CD", "P1");
            param.Add("PLANT_CD", bp.g_plant.ToString());
            param.Add("SHOP_CD", ddlShopCd.SelectedValue);
            param.Add("LINE_CD", ddlLineCd.SelectedValue);
            param.Add("CAR_TYPE", ddlCarType.SelectedValue);
            param.Add("DEV_ID", ddlDevCd.SelectedValue);

            // 비지니스 메서드 호출(다중테이블 함수)
            ds = biz.GetDataSet(strDBName, strQueryID, param);

            if (ds.Tables.Count > 0)
            {
                // MasterPage에서 ContentPlaceHolder 찾기
                var contentPlaceHolder = Master.FindControl("PopupContent") as ContentPlaceHolder;
                if (contentPlaceHolder != null)
                {
                    Label label = null;
                    string num = null;
                    //라벨 초기화
                    for(int j = 1; j < 301; j++)
                    {
                        num = j.ToString().PadLeft(3, '0');
                        label = contentPlaceHolder.FindControl($"lbItem" + num) as Label;
                        if (label != null)
                        {
                            label.Text = Dictionary_Data.SearchDic("SPEC_" + num, bp.g_language);
                        }
                    }

                    // 데이터 있으면 변경
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        label = contentPlaceHolder.FindControl($"lbItem" + ds.Tables[1].Rows[i]["NUM"].ToString()) as Label;
                        if (label != null)
                        {
                            label.Text = ds.Tables[1].Rows[i]["INS_NM"].ToString();
                        }
                    }
                }
            }
        }
        #endregion
    }
}
