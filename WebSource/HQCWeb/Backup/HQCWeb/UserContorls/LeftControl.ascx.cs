using HQCWeb.FW;
using HQCWeb.FW.Data.DataMapper.Configuration;
using MES.FW.Common.CommonMgt;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace HQCWeb.UserContorls
{
    public partial class LeftControl : System.Web.UI.UserControl
    {
        BasePage bp = new BasePage();

        public string strMenu = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            GetMenuInfo();
        }

        #region GetMenuInfo
        private void GetMenuInfo()
        {
            Biz.Sample_Biz sp = new Biz.Sample_Biz();

            DataSet ds = new DataSet();

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            strDBName = "GPDB";
            strQueryID = "MenuData.Get_Menus";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("USER_ID",   bp.g_userid.ToString());
            //sParam.Add("USER_ID",   "JYJ");

            ds = sp.GetList(strDBName, strQueryID, sParam);

            string strGroupID = string.Empty;

            string strParentID = string.Empty;

            string strMenuName = string.Empty;
            string strMenuID = string.Empty;
            string strUrl = string.Empty;

            // 1Depth
            int iTopLevelChild_CNT = 0;
            int iSecondCurrent_CNT = 0;

            // 2Depth
            int iNextLevelChild_CNT = 0;
            int iLastCurrent_CNT = 0;

            strMenu = "";

            if (ds.Tables.Count > 0)
            {
                DataTable dtParentLevel = new DataTable();
                dtParentLevel = ds.Tables[0];

                DataTable dtParentGroupTable = dtParentLevel.Clone();  // 그룹핑될 DataTable 을 정의하고 구조를 원본 DataTable 스키마 복사
                dataTableGroupBy(dtParentLevel, ref dtParentGroupTable, "MENU_LEVEL", "1");

                DataTable DT_ChildLevel = new DataTable();

                for (int i = 0; i < dtParentGroupTable.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        strGroupID = dtParentGroupTable.Rows[i]["MENU_GROUP_ID"].ToString();
                    }

                    if (strGroupID != dtParentGroupTable.Rows[i]["MENU_GROUP_ID"].ToString())
                    {
                        strGroupID = dtParentGroupTable.Rows[i]["MENU_GROUP_ID"].ToString();

                        iTopLevelChild_CNT = 0;
                        iSecondCurrent_CNT = 0;

                        iNextLevelChild_CNT = 0;
                        iLastCurrent_CNT = 0;
                    }

                    DT_ChildLevel = dtParentLevel.Clone();

                    dataTableGroupBy(dtParentLevel, ref DT_ChildLevel, "MENU_GROUP_ID", dtParentGroupTable.Rows[i]["MENU_ID"].ToString());

                    for (int j = 0; j < DT_ChildLevel.Rows.Count; j++)
                    {
                        strMenuName = DT_ChildLevel.Rows[j]["MENU_NM"].ToString();
                        strMenuID = DT_ChildLevel.Rows[j]["MENU_ID"].ToString();
                        strUrl = DT_ChildLevel.Rows[j]["ASSEMBLY_ID"].ToString();

                        if (DT_ChildLevel.Rows[j]["MENU_LEVEL"].ToString() == "1")
                        {
                            iTopLevelChild_CNT = Convert.ToInt32(DT_ChildLevel.Rows[j]["CHILD_CNT"].ToString());

                            if (Convert.ToInt32(DT_ChildLevel.Rows[j]["CHILD_CNT"].ToString()) == 0)
                            {
                                strMenu += "<li>\n" +
                                                "<a href=\"javascript:fn_Add('" + strUrl + "','" + strMenuName + "','" + strMenuID + "');\" >" + strMenuName + "</a>\n" +  // class='menu0" + (i + 1).ToString() + "'
                                            "</li>\n";
                            }
                            else
                            {
                                strMenu += "<li>\n" +
                                                "<a href='#'>" + strMenuName + "</a>\n" +  // class='menu0" + (i + 1).ToString() + "'
                                                "<ul>\n";
                            }
                        }
                        else if (DT_ChildLevel.Rows[j]["MENU_LEVEL"].ToString() == "2")
                        {
                            iSecondCurrent_CNT++;

                            iNextLevelChild_CNT = Convert.ToInt32(DT_ChildLevel.Rows[j]["CHILD_CNT"].ToString());

                            if (Convert.ToInt32(DT_ChildLevel.Rows[j]["CHILD_CNT"].ToString()) == 0)
                            {
                                strMenu += "       <li><a href=\"javascript:fn_Add('" + strUrl + "','" + strMenuName + "','" + strMenuID + "');\" id='a" + strMenuID + "'>" + strMenuName + "</a></li>\n";
                            }
                            else
                            {
                                strMenu += "        <li>\n" + //  class='#'
                                                    "   <a href='#'>" + strMenuName + "</a>\n" +
                                                    "   <ul>\n";
                            }

                            if (iTopLevelChild_CNT == iSecondCurrent_CNT)
                            {
                                if (iNextLevelChild_CNT == 0)
                                {
                                    strMenu += "    </ul>\n" +
                                            "</li>\n";
                                }
                            }
                        }
                        else
                        {
                            iLastCurrent_CNT++;

                            strMenu += "                    <li><a href=\"javascript:fn_Add('" + strUrl + "','" + strMenuName + "','" + strMenuID + "');\" id='a" + strMenuID + "'>" + strMenuName + "</a></li>\n";

                            if (iNextLevelChild_CNT == iLastCurrent_CNT)
                            {
                                strMenu += "            </ul>\n" +
                                            "       </li>\n";

                                iLastCurrent_CNT = 0;

                                if (iTopLevelChild_CNT == iSecondCurrent_CNT)
                                {
                                    strMenu += "    </ul>\n" +
                                                "</li>\n";

                                    iSecondCurrent_CNT = 0;

                                }
                            }

                        }
                    }
                }
            }
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
    }
}