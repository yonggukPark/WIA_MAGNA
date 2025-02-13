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
using Newtonsoft.Json.Linq;

using System.Web.Services;

namespace HQCWeb.SystemMgt.UserManagement
{
    public partial class UserMenuSetting : System.Web.UI.Page
    {
        #region SetUserMenuSettingInfo
        [WebMethod]
        public static string SetUserMenuSettingInfo(string sParams)
        {
            BasePage bp = new BasePage();
            
            string strRtn = string.Empty;

            int iRtn = 0;
            string strRtnValChk = string.Empty;
            string strScript = string.Empty;

            string strDBName = string.Empty;
            string strQueryID = string.Empty;

            JObject jObject = JObject.Parse(sParams);

            string strMenuID = (jObject["MenuID"]).ToString();

            JToken jToken = jObject["ColumnList"];

            string strInfo = string.Empty;
            string strFixYN = string.Empty;
            int iRow = 0;

            string strName  = string.Empty;
            string strWidth = string.Empty;
            string strIndex = string.Empty;
            string strFix   = string.Empty;


            Item item = JsonConvert.DeserializeObject<Item>(sParams);
            
            foreach (ColList colList in item.ColumnList)
            {
                strName =   colList.name;
                strWidth =  colList.width;
                strIndex =  colList.vindex;
                strFix =    colList.colFix;

                if (strName == null)
                {
                    strName = "NoColumn";
                }

                if (strWidth == null)
                {
                    strWidth = "0";
                }

                if (strIndex == null)
                {
                    strIndex = "100";
                }

                if (strFix == null)
                {
                    strFix = "false";
                }
                

                if (iRow == 0)
                {
                    //strInfo = colList.name + "," + colList.width + "," + colList.vindex + "," + colList.colFix;
                    strInfo = strName + "|" + strWidth + "|" + strIndex + "|" + strFix;
                }
                else
                {
                    //strInfo += "/" + colList.name + "," + colList.width + "," + colList.vindex + "," + colList.colFix;
                    strInfo += "/" + strName + "|" + strWidth + "|" + strIndex + "|" + strFix;
                }

                iRow++;
            }
            
            
            strDBName = "GPDB";
            strQueryID = "UserInfoData.Set_UserMenuSettingInfo";

            FW.Data.Parameters sParam = new FW.Data.Parameters();
            sParam.Add("MENU_ID",       strMenuID);
            sParam.Add("USER_ID",       bp.g_userid.ToString());
            sParam.Add("COL_INFO",      strInfo);

            sParam.Add("CUR_MENU_ID",   strMenuID);

            Biz.SystemManagement.AuthGroupMgt biz = new Biz.SystemManagement.AuthGroupMgt();

            //  등록 서비스 작성
            iRtn = biz.DelAuthGroupMenu(strDBName, strQueryID, sParam);

            if (iRtn == 1)
            {
                strRtn = "C";
            }
            else
            {
                strRtn = "E";
            }
            

            return strRtn;
        }

        class Item {
            public string MenuID;
            public List<ColList> ColumnList;
        }

        class ColList
        {
            public string name;
            public string width;
            public string vindex;
            public string colFix;
        }

        #endregion
    }
}
