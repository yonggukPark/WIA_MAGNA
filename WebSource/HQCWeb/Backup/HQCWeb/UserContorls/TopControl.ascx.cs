using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using HQC.FW.Common;
using MES.FW.Common.CommonMgt;


namespace HQCWeb.UserContorls
{
    public partial class TopControl : System.Web.UI.UserControl
    {
        BasePage bp = new BasePage();

        public string strMenu = string.Empty;

        //public event EventHandler LanguageChanged;

        #region Page_Load
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) {
                
                if (bp.g_language == "")
                {
                    ddlLanguage.SelectedValue = "KO_KR";
                }
                else
                {
                    ddlLanguage.SelectedValue = bp.g_language;
                }
            }

            lbUserID.Text = bp.g_userid.ToString();
            lbPlant.Text = bp.g_plant.ToString();



            //GetMenuInfo();
        }
        #endregion

        #region GetMenuInfo
        public void GetMenuInfo()
        {
            //HQCWeb.Biz.Mgt_Biz sp = new HQCWeb.Biz.Mgt_Biz();

            //DataSet ds = new DataSet();

            //ds = sp.GetMenuInfoList_Module();

            //string strGroupID = string.Empty;

            //string strMenuName = string.Empty;

            //string strCss = string.Empty;

            //// 1Depth
            //int iTopLevelChild_CNT = 0;
            //int iSecondCurrent_CNT = 0;

            //// 2Depth
            //int iNextLevelChild_CNT = 0;
            //int iLastCurrent_CNT = 0;

            //if (ds.Tables.Count > 0)
            //{
            //    DataTable dtParentLevel = new DataTable();
            //    dtParentLevel = ds.Tables[0];

            //    DataTable dtParentGroupTable = dtParentLevel.Clone();  // 그룹핑될 DataTable 을 정의하고 구조를 원본 DataTable 스키마 복사
            //    dataTableGroupBy(dtParentLevel, ref dtParentGroupTable, "MENU_LEVEL", "1");

            //    DataTable DT_ChildLevel = new DataTable();

            //    //strMenu = "<ul class='row top_menu'>\n";

            //    for (int i = 0; i < dtParentGroupTable.Rows.Count; i++)
            //    {

            //        if (i == 0)
            //        {
            //            strGroupID = dtParentGroupTable.Rows[i]["MENU_GROUP_ID"].ToString();
            //        }
            //        else {
            //            strCss = " ml28";
            //        }

            //        if (strGroupID != dtParentGroupTable.Rows[i]["MENU_GROUP_ID"].ToString())
            //        {
            //            strGroupID = dtParentGroupTable.Rows[i]["MENU_GROUP_ID"].ToString();

            //            iTopLevelChild_CNT = 0;
            //            iSecondCurrent_CNT = 0;

            //            iNextLevelChild_CNT = 0;
            //            iLastCurrent_CNT = 0;
            //        }

            //        DT_ChildLevel = dtParentLevel.Clone();

            //        dataTableGroupBy(dtParentLevel, ref DT_ChildLevel, "MENU_GROUP_ID", dtParentGroupTable.Rows[i]["MENU_ID"].ToString());

            //        for (int j = 0; j < DT_ChildLevel.Rows.Count; j++)
            //        {

            //            strMenuName = DT_ChildLevel.Rows[j]["MENU_NM"].ToString();

            //            if (DT_ChildLevel.Rows[j]["MENU_LEVEL"].ToString() == "1")
            //            {
            //                iTopLevelChild_CNT = Convert.ToInt32(DT_ChildLevel.Rows[j]["CHILD_CNT"].ToString());

            //                if (Convert.ToInt32(DT_ChildLevel.Rows[j]["CHILD_CNT"].ToString()) == 0)
            //                {
            //                    strMenu += "<li class='cell" + strCss + "'>\n" +
            //                                    "<a href=\"javascript:fn_Add('" + DT_ChildLevel.Rows[j]["ASSEMBLY_ID"].ToString() + "','" + strMenuName + "','" + DT_ChildLevel.Rows[j]["MENU_ID"].ToString() + "');\" class='menu0" + (i + 1).ToString() + "'>" + strMenuName + "</a>\n" +
            //                                "</li>\n";
            //                }
            //                else
            //                {
            //                    strMenu += "<li class='cell" + strCss + "'>\n" +
            //                                    "<a href='#' class='menu0" + (i + 1).ToString() + "'>" + strMenuName + "</a>\n" +
            //                                    "<ul>\n";
            //                }
            //            }
            //            else if (DT_ChildLevel.Rows[j]["MENU_LEVEL"].ToString() == "2")
            //            {
            //                iSecondCurrent_CNT++;

            //                iNextLevelChild_CNT = Convert.ToInt32(DT_ChildLevel.Rows[j]["CHILD_CNT"].ToString());

            //                if (Convert.ToInt32(DT_ChildLevel.Rows[j]["CHILD_CNT"].ToString()) == 0)
            //                {
            //                    strMenu += "       <li><a href=\"javascript:fn_Add('" + DT_ChildLevel.Rows[j]["ASSEMBLY_ID"].ToString() + "','" + strMenuName + "','" + DT_ChildLevel.Rows[j]["MENU_ID"].ToString() + "');\">" + strMenuName + "</a></li>\n";
            //                }
            //                else
            //                {
            //                    strMenu += "        <li class='#'>\n" +
            //                                        "   <a href='#'>" + strMenuName + "</a>\n" +
            //                                        "   <ul>\n";
            //                }

            //                if (iTopLevelChild_CNT == iSecondCurrent_CNT) {
            //                    if (iNextLevelChild_CNT == 0) {
            //                        strMenu += "    </ul>\n" +
            //                                "</li>\n";
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                iLastCurrent_CNT++;

            //                strMenu += "                    <li><a href=\"javascript:fn_Add('" + DT_ChildLevel.Rows[j]["ASSEMBLY_ID"].ToString() + "','" + strMenuName + "','" + DT_ChildLevel.Rows[j]["MENU_ID"].ToString() + "');\">" + strMenuName + "</a></li>\n";

            //                if (iNextLevelChild_CNT == iLastCurrent_CNT)
            //                {
            //                    strMenu += "            </ul>\n" +
            //                                "       </li>\n";

            //                    iLastCurrent_CNT = 0;

            //                    if (iTopLevelChild_CNT == iSecondCurrent_CNT)
            //                    {
            //                        strMenu += "    </ul>\n" +
            //                                    "</li>\n";

            //                        iSecondCurrent_CNT = 0;

            //                    }
            //                }
                            
            //            }
            //        }
            //    }

            //    //strMenu += "    </ul>\n" +
            //    //            "</li>\n";

            //    //strMenu += "</ul>\n";
            //}
        }

        #endregion

        #region dataTableGroupBy
        public void dataTableGroupBy(DataTable oriData, ref DataTable copyData, string strParam, string strValue)
        {
            DataRow[] drSelect = null;
            DataRow[] comSelect = null;

            string filter = string.Empty;
            string order = string.Empty;

            try
            {
                int oriCnt = oriData.Rows.Count;
                DataView dv = oriData.DefaultView;
                dv.Sort = order;

                DataTable dt = dv.ToTable();
               
                filter = string.Format("{0}='{1}'", strParam, strValue);

                drSelect = dt.Select(filter);

                if (drSelect.Length > 0)
                {
                    comSelect = copyData.Select(filter);
                    if (comSelect.Length <= 0)
                    {
                        for (int i = 0; i < drSelect.Length; i++)
                        {
                            copyData.ImportRow(drSelect[i]);
                        }
                    }
                }

            }
            catch (Exception err)
            {
            }
        }
        #endregion

        #region ddlLanguage_SelectedIndexChanged
        protected void ddlLanguage_SelectedIndexChanged(object sender, EventArgs e)
        {
            bp.g_language = ddlLanguage.SelectedValue;

            Response.Redirect(Request.Url.AbsolutePath);
        }
        #endregion

        protected void btnLogOut_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("/Login.aspx");
        }


        protected void btnSessionClear_Click(object sender, EventArgs e)
        {
            Session.Clear();
        }

        protected void btnNFC_Click(object sender, EventArgs e)
        {
            //Response.Redirect("/Cell/TagMgt/OM_001_18.aspx");
            Response.Redirect("/Cell/TagMgt/NFCTest.aspx");
        }
    }
}