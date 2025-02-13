using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MES.FW.Common.CommonMgt
{
    public class StringUtil : System.Web.UI.Page
    {
        MasterPage dfMaster = new MasterPage();

        // 입력값 체크
        public bool strChk(string val)
        {
            bool bChk = true;

            if (val.IndexOf("<") >= 0
                || val.IndexOf(">") >= 0
                || val.IndexOf("?") >= 0
                || val.IndexOf(":") >= 0
                || val.IndexOf("javascript") >= 0
                || val.IndexOf("location") >= 0
                || val.IndexOf("'") >= 0
                || val.IndexOf("=") >= 0 ) 
            {
                bChk = false;
            }

            return bChk;
        }

        // 문자열 자르기
        public string strSubString(string val, int startDigit, int endDigit, string strChgChar)
        {
            string str = string.Empty;

            if (val.Length > endDigit)
            {
                str = val.Trim().Substring(startDigit, endDigit) + strChgChar; 
            }
            else
            {
                str = val;
            }

            return str;
        }

        #region fn_CheckNull
        public bool fn_CheckNull(System.Web.UI.Control ctl, DropDownList DropDownList_ID, Label Label_ID, string strMassage)
        {
            bool boolCheck = true;

            if (DropDownList_ID.SelectedValue == "")
            {
                boolCheck = false;

                string strScript = string.Empty;

                strScript = "alert(\"[" + Label_ID.Text + "] " + strMassage + "\");";

                //System.Web.UI.ScriptManager.RegisterStartupScript                 
                System.Web.UI.ScriptManager.RegisterStartupScript(
                    ctl
                    , typeof(string), "ClientScript", strScript + "  $(\"#" + DropDownList_ID.ClientID + "\").focus();", true);
            }

            return boolCheck;
        }
        #endregion

        #region fn_CheckNull
        public bool fn_CheckNull(System.Web.UI.Control ctl, TextBox TextBox_ID, Label Label_ID, string strMassage)
        {
            bool boolCheck = true;

            if (TextBox_ID.Text == "")
            {
                boolCheck = false;

                string strScript = string.Empty;

                strScript = "alert(\"[" + Label_ID.Text + "] " + strMassage + "\");";


                //System.Web.UI.ClientScriptManager ci = new ClientScriptManager();
                System.Web.UI.ScriptManager.RegisterStartupScript(
                    ctl, typeof(string), "ClientScript", strScript + " $(\"#" + TextBox_ID.ClientID + "\").focus();", true);

                //System.Web.UI.ScriptManager.RegisterStartupScript(typeof(string), "ClientScript", strScript + " $('#" + TextBox_ID.ClientID + "').focus();", true);
            }

            return boolCheck;
        }
        #endregion

        #region fn_CheckValue
        public bool fn_CheckValue(System.Web.UI.Control ctl, TextBox TextBox_ID, Label Label_ID, string strMassage)
        {
            bool boolCheck = true;

            if (!strChk(TextBox_ID.Text))
            {
                boolCheck = false;

                string strScript = string.Empty;

                strScript = "alert(\"[" + Label_ID.Text + "] " + strMassage + "\");";

                System.Web.UI.ScriptManager.RegisterStartupScript(ctl,
                   typeof(string), "ClientScript", strScript + " $(\"#" + TextBox_ID.ClientID + "\").focus();", true);
            }

            return boolCheck;
        }
        #endregion

        #region fn_CheckFormatInt
        public bool fn_CheckFormatInt(System.Web.UI.Control ctl, TextBox TextBox_ID, Label Label_ID, string strMassage)
        {
            bool boolCheck = true;

            try
            {
                Convert.ToInt32(TextBox_ID.Text);
            }
            catch
            {
                boolCheck = false;

                string strScript = string.Empty;

                strScript = "alert(\"[" + Label_ID.Text + "] " + strMassage + "\");";

                System.Web.UI.ScriptManager.RegisterStartupScript(
                    ctl, typeof(string), "ClientScript", strScript + " $(\"#" + TextBox_ID.ClientID + "\").focus();", true);
            }

            return boolCheck;
        }
        #endregion

        #region fn_CheckFormatInt
        public bool fn_CheckFormatInt(System.Web.UI.Control ctl, TextBox TextBox_ID, Label Label_ID, int MaxLength, string strMassage)
        {
            bool boolCheck = true;

            try
            {
                Convert.ToInt32(TextBox_ID.Text);

                if (TextBox_ID.Text.Length > MaxLength)
                {
                    boolCheck = false;

                    string strScript = string.Empty;

                    strScript = "alert(\"[" + Label_ID.Text + "] " + strMassage + " : MaxLength is " + MaxLength + "\");";

                    System.Web.UI.ScriptManager.RegisterStartupScript(
                        ctl, typeof(string), "ClientScript", strScript + " $(\"#" + TextBox_ID.ClientID + "\").focus();", true);
                }
            }
            catch
            {
                boolCheck = false;

                string strScript = string.Empty;

                strScript = "alert(\"[" + Label_ID.Text + "] " + strMassage + "\");";

                System.Web.UI.ScriptManager.RegisterStartupScript(
                    ctl, typeof(string), "ClientScript", strScript + " $(\"#" + TextBox_ID.ClientID + "\").focus();", true);
            }

            return boolCheck;
        }
        #endregion

        #region fn_CheckFormatInt
        public bool fn_CheckFormatInt(System.Web.UI.Control ctl, TextBox TextBox_ID, Label Label_ID, int MaxLength, int MaxValue, string strMassage)
        {
            bool boolCheck = true;

            try
            {
                if (Convert.ToInt32(TextBox_ID.Text) >= MaxValue)
                {
                    boolCheck = false;

                    string strScript = string.Empty;

                    strScript = "alert(\"[" + Label_ID.Text + "] " + strMassage + " : MaxValue is " + MaxValue + "\");";

                    System.Web.UI.ScriptManager.RegisterStartupScript(
                        ctl, typeof(string), "ClientScript", strScript + " $(\"#" + TextBox_ID.ClientID + "\").focus();", true);
                }

                if (TextBox_ID.Text.Length > MaxLength)
                {
                    boolCheck = false;

                    string strScript = string.Empty;

                    strScript = "alert(\"[" + Label_ID.Text + "] " + strMassage + " : MaxLength is " + MaxLength + "\");";

                    System.Web.UI.ScriptManager.RegisterStartupScript(
                        ctl, typeof(string), "ClientScript", strScript + " $(\"#" + TextBox_ID.ClientID + "\").focus();", true);
                }
            }
            catch
            {
                boolCheck = false;

                string strScript = string.Empty;

                strScript = "alert(\"[" + Label_ID.Text + "] " + strMassage + "\");";

                System.Web.UI.ScriptManager.RegisterStartupScript(
                    ctl, typeof(string), "ClientScript", strScript + " $(\"#" + TextBox_ID.ClientID + "\").focus();", true);
            }

            return boolCheck;
        }
        #endregion

        #region fn_CheckFormatIPAddress
        public bool fn_CheckFormatIPAddress(System.Web.UI.Control ctl, TextBox TextBox_ID, Label Label_ID, string strMassage)
        {
            bool boolCheck = true;

            int intLength = TextBox_ID.Text.Length;
            char[] arrDotCountCheck = TextBox_ID.Text.ToCharArray(0, intLength);
            int intDotCount = 0;

            // DotCount Set
            for (int i = 0; i < intLength; i++)
            {
                if (arrDotCountCheck[i] == '.')
                {
                    intDotCount++;
                }
            }

            // DotCount Check
            if (intDotCount != 3)
            {
                boolCheck = false;

                string strScript = string.Empty;

                strScript = "alert(\"[" + Label_ID.Text + "] " + strMassage + "\");";

                System.Web.UI.ScriptManager.RegisterStartupScript(
                    ctl, typeof(string), "ClientScript", strScript + " $(\"#" + TextBox_ID.ClientID + "\").focus();", true);
            }

            try
            {
                // IP Class Split and Variable Set
                string[] arrIPClassSplit = TextBox_ID.Text.Split('.');

                // IP Class Value Check
                for (int i = 0; i < arrIPClassSplit.Length; i++)
                {
                    if (!(Convert.ToInt32(arrIPClassSplit[i]) >= 0 && Convert.ToInt32(arrIPClassSplit[i]) <= 255))
                    {
                        boolCheck = false;

                        string strScript = string.Empty;

                        strScript = "alert(\"[" + Label_ID.Text + "] " + strMassage + "\");";

                        System.Web.UI.ScriptManager.RegisterStartupScript(
                            ctl, typeof(string), "ClientScript", strScript + " $('#" + TextBox_ID.ClientID + "\").focus();", true);

                        break;
                    }
                }
            }
            catch
            {
                boolCheck = false;

                string strScript = string.Empty;

                strScript = "alert(\"[" + Label_ID.Text + "] " + strMassage + "\");";

                System.Web.UI.ScriptManager.RegisterStartupScript(
                    ctl, typeof(string), "ClientScript", strScript + " $(\"#" + TextBox_ID.ClientID + "\").focus();", true);
            }

            return boolCheck;
        }
        #endregion

        #region fn_CheckFormatPort
        public bool fn_CheckFormatPort(System.Web.UI.Control ctl, TextBox TextBox_ID, Label Label_ID, string strMassage)
        {
            bool boolCheck = true;

            try
            {
                if (Convert.ToInt32(TextBox_ID.Text) > 65535)
                {
                    boolCheck = false;

                    string strScript = string.Empty;

                    strScript = "alert(\"[" + Label_ID.Text + "] " + strMassage + " : MaxValue is " + 65535 + "\");";

                    System.Web.UI.ScriptManager.RegisterStartupScript(
                        ctl, typeof(string), "ClientScript", strScript + " $(\"#" + TextBox_ID.ClientID + "\").focus();", true);
                }

                if (TextBox_ID.Text.Length > 5)
                {
                    boolCheck = false;

                    string strScript = string.Empty;

                    strScript = "alert(\"[" + Label_ID.Text + "] " + strMassage + " : MaxLength is " + 5 + "\");";

                    System.Web.UI.ScriptManager.RegisterStartupScript(
                        ctl, typeof(string), "ClientScript", strScript + " $(\"#" + TextBox_ID.ClientID + "\").focus();", true);
                }
            }
            catch
            {
                boolCheck = false;

                string strScript = string.Empty;

                strScript = "alert(\"[" + Label_ID.Text + "] " + strMassage + "\");";

                System.Web.UI.ScriptManager.RegisterStartupScript(
                    ctl, typeof(string), "ClientScript", strScript + " $(\"#" + TextBox_ID.ClientID + "\").focus();", true);
            }

            return boolCheck;
        }
        #endregion

        #region fn_CheckDropDownList
        public void fn_CheckValueDropDownList(System.Web.UI.Control ctl, DropDownList DropDownList_ID, Label Label_ID, string strValue, string strMessage)
        {
            string strScript = string.Empty;

            DropDownList_ID.SelectedValue = strValue;

            if (DropDownList_ID.SelectedValue != strValue)
            {
                LogUtils._ErrorWirte(ctl, strMessage);

                strScript = " alert ('[" + Label_ID.Text + "] " + strMessage + "');";

                System.Web.UI.ScriptManager.RegisterStartupScript(
                        ctl, typeof(string), "ClientScript", strScript + " $(\"#" + DropDownList_ID.ClientID + "\").focus();", true);
            }
            
        }
        #endregion

        #region fn_CheckDate
        public bool fn_CheckDate(System.Web.UI.Control ctl, TextBox txtStartDate, TextBox txtEndDate, string strLanguage, string strMassage)
        {
            bool boolCheck = true;

            string strScript = string.Empty;

            int iStartDate = 0;

            int iEndDate = 0;

            if (strLanguage == "KR")
            {
                iStartDate = Convert.ToInt32(txtStartDate.Text.Replace("-", ""));
                iEndDate = Convert.ToInt32(txtEndDate.Text.Replace("-", ""));
            }
            else if (strLanguage == "EN")
            {
                iStartDate = Convert.ToInt32(txtStartDate.Text.Replace("/", ""));
                iEndDate = Convert.ToInt32(txtEndDate.Text.Replace("/", ""));
            }
            else if (strLanguage == "OT")
            {
                iStartDate = Convert.ToInt32(txtStartDate.Text.Replace(".", ""));
                iEndDate = Convert.ToInt32(txtEndDate.Text.Replace(".", ""));
            }

            if (iStartDate > iEndDate)
            {
                boolCheck = false;

                strScript = "alert(\"" + strMassage + "\");";

                System.Web.UI.ScriptManager.RegisterStartupScript(
                    ctl
                    , typeof(string)
                    , "ClientScript"
                    , strScript + " $(\"#" + txtStartDate.ClientID + "\").val(\"\"); $(\"#" + txtEndDate.ClientID + "\").val(\"\"); $(\"#" + txtStartDate.ClientID + "\").focus();"
                    , true
                );
            }

            return boolCheck;
        }
        #endregion

        #region fn_CheckDate
        public bool fn_CheckDate(System.Web.UI.Control ctl, TextBox txtStartDate, TextBox txtStartTime, TextBox txtEndDate, TextBox txtEndTime, string strLanguage, string strMassage)
        {
            bool boolCheck = true;

            string strScript = string.Empty;

            long longStartDate = 0;
            int intStartDate = 0;

            long longEndDate = 0;
            int intEndDate = 0;

            string strStartTime = string.Empty;
            string strEndTime = string.Empty;

            if (strLanguage == "KR")
            {
                intStartDate = Convert.ToInt32(txtStartDate.Text.Replace("-", ""));
                intEndDate = Convert.ToInt32(txtEndDate.Text.Replace("-", ""));
            }
            else if (strLanguage == "EN")
            {
                intStartDate = Convert.ToInt32(txtStartDate.Text.Replace("/", ""));
                intEndDate = Convert.ToInt32(txtEndDate.Text.Replace("/", ""));
            }
            else if (strLanguage == "OT")
            {
                intStartDate = Convert.ToInt32(txtStartDate.Text.Replace(".", ""));
                intEndDate = Convert.ToInt32(txtEndDate.Text.Replace(".", ""));
            }


            if (intStartDate > intEndDate)
            {
                boolCheck = false;

                strScript = "alert(\"" + strMassage + "\");";

                System.Web.UI.ScriptManager.RegisterStartupScript(
                    ctl
                    , typeof(string)
                    , "ClientScript"
                    //, strScript + " $(\"#" + txtStartTime.ClientID + "\").val(\"\"); $(\"#" + txtEndTime.ClientID + "\").val(\"\"); $(\"#" + txtEndDate.ClientID + "\").focus();"
                    , strScript + " $(\"#" + txtEndDate.ClientID + "\").val(\"\"); $(\"#" + txtEndTime.ClientID + "\").val(\"\"); $(\"#" + txtEndDate.ClientID + "\").focus();"
                    , true
                );
            }
            else
            {
                if (strLanguage == "KR")
                {
                    strStartTime = (txtStartDate.Text + txtStartTime.Text).Replace("-", "").Replace(":", "");
                    strEndTime = (txtEndDate.Text + txtEndTime.Text).Replace("-", "").Replace(":", "");

                    longStartDate = Convert.ToInt64(strStartTime);
                    longEndDate = Convert.ToInt64(strEndTime);
                }
                else if (strLanguage == "EN")
                {
                    strStartTime = (txtStartDate.Text + txtStartTime.Text).Replace("/", "").Replace(":", "");
                    strEndTime = (txtEndDate.Text + txtEndTime.Text).Replace("/", "").Replace(":", "");

                    longStartDate = Convert.ToInt64(strStartTime);
                    longEndDate = Convert.ToInt64(strEndTime);
                }
                else if (strLanguage == "OT")
                {
                    strStartTime = (txtStartDate.Text + txtStartTime.Text).Replace(".", "").Replace(":", "");
                    strEndTime = (txtEndDate.Text + txtEndTime.Text).Replace(".", "").Replace(":", "");

                    longStartDate = Convert.ToInt64(strStartTime);
                    longEndDate = Convert.ToInt64(strEndTime);
                }


                if (longStartDate > longEndDate)
                {
                    boolCheck = false;

                    strScript = "alert(\"" + strMassage + "\");";

                    System.Web.UI.ScriptManager.RegisterStartupScript(
                        ctl
                        , typeof(string)
                        , "ClientScript"
                        , strScript + " $(\"#" + txtEndDate.ClientID + "\").val(\"\"); $(\"#" + txtEndTime.ClientID + "\").val(\"\"); $(\"#" + txtEndDate.ClientID + "\").focus();"
                        , true
                    );
                }
            }


            return boolCheck;
        }
        #endregion

        #region fn_CheckTime
        public bool fn_CheckTime(System.Web.UI.Control ctl, TextBox txtStartTime, TextBox txtStartMin, TextBox txtEndTime, TextBox txtEndMin, string strMassage)
        {
            bool boolCheck = true;

            string strScript = string.Empty;

            int iStartTime = 0;

            int iEndTime = 0;

            iStartTime = Convert.ToInt32(txtStartTime.Text + txtStartMin.Text);
            iEndTime = Convert.ToInt32(txtEndTime.Text + txtEndMin.Text);


            if (iStartTime > iEndTime)
            {
                boolCheck = false;

                strScript = "alert(\"" + strMassage + "\");";

                System.Web.UI.ScriptManager.RegisterStartupScript(
                    ctl
                    , typeof(string)
                    , "ClientScript"
                    , strScript + " $(\"#" + txtEndTime.ClientID + "\").val(\"\"); $(\"#" + txtEndMin.ClientID + "\").val(\"\"); $(\"#" + txtEndTime.ClientID + "\").focus();"
                    , true
                );
            }

            return boolCheck;
        }
        #endregion

        #region fn_CheckTime
        public bool fn_CheckTime(System.Web.UI.Control ctl, DropDownList ddlStartTime, DropDownList ddlStartMin, DropDownList ddlEndTime, DropDownList ddlEndMin, string strMassage)
        {
            bool boolCheck = true;

            string strScript = string.Empty;

            int iStartTime = 0;

            int iEndTime = 0;

            iStartTime = Convert.ToInt32(ddlStartTime.SelectedValue + ddlStartMin.SelectedValue);
            iEndTime = Convert.ToInt32(ddlEndTime.SelectedValue + ddlEndMin.SelectedValue);


            if (iStartTime > iEndTime)
            {
                boolCheck = false;

                strScript = "alert(\"" + strMassage + "\");";

                System.Web.UI.ScriptManager.RegisterStartupScript(
                    ctl
                    , typeof(string)
                    , "ClientScript"
                    , strScript + " $(\"#" + ddlEndTime.ClientID + "\").val(\"00\"); $(\"#" + ddlEndMin.ClientID + "\").val(\"00\"); $(\"#" + ddlEndTime.ClientID + "\").focus();"
                    , true
                );
            }

            return boolCheck;
        }
        #endregion

        #region fn_CheckCompare
        public bool fn_CheckCompare(System.Web.UI.Control ctl, TextBox txtMinValue, Label lbMinValue, TextBox txtMaxValue, Label lbMaxValue, string strMassage)
        {
            bool boolCheck = true;

            string strScript = string.Empty;

            int iMinValue = 0;

            int iMaxValue = 0;

            iMinValue = Convert.ToInt32(txtMinValue.Text);
            iMaxValue = Convert.ToInt32(txtMaxValue.Text);

            if (iMinValue > iMaxValue)
            {
                boolCheck = false;

                strScript = "alert(\"" + strMassage.Replace("MinValue", lbMinValue.Text).Replace("MaxValue", lbMaxValue.Text) + "\");";

                System.Web.UI.ScriptManager.RegisterStartupScript(
                    ctl
                    , typeof(string)
                    , "ClientScript"
                    , strScript + " $(\"#" + txtMaxValue.ClientID + "\").val(\"\");  $(\"#" + txtMaxValue.ClientID + "\").focus();"
                    , true
                );
            }

            return boolCheck;
        }
        #endregion

        #region SetControlValChk
        public bool SetControlValChk(MasterPage mp, string strContentPlaceHolderName)
        {
            dfMaster = mp;
            
            bool bRtnVal = true;

            ContentPlaceHolder cph = (ContentPlaceHolder)dfMaster.FindControl(strContentPlaceHolderName);

            if (cph != null)
            {
                foreach (Control ctrl in cph.Controls)
                {
                    if (ctrl is TextBox)
                    {
                        bRtnVal = strChk(((TextBox)(ctrl)).Text.ToLower());

                        if (bRtnVal)
                        {
                            bRtnVal = true;
                        }
                        else
                        { 
                            bRtnVal = false;

                            break;
                        }
                    }
                }
            }

            return bRtnVal;
        }
        #endregion

        #region strHTMLChk
        public bool strHTMLChk(string val)
        {
            bool bChk = true;

            if (val.IndexOf("<a") >= 0
                || val.IndexOf("</a>") >= 0
                || val.IndexOf("?") >= 0
                || val.IndexOf(":") >= 0
                || val.IndexOf("javascript") >= 0
                || val.IndexOf("location") >= 0
                || val.IndexOf("'") >= 0
                || val.IndexOf("=") >= 0
                || val.IndexOf("href") >= 0)
            {
                bChk = false;
            }

            return bChk;
        }
        #endregion

        #region strBadFileNameChk
        public bool strBadFileNameChk(string val)
        {
            bool bChk = true;

            if (val.IndexOf("asp") >= 0
                || val.IndexOf("asa") >= 0
                || val.IndexOf("aspx") >= 0
                || val.IndexOf("html") >= 0
                || val.IndexOf("htm") >= 0
                || val.IndexOf("hta") >= 0
                || val.IndexOf("htr") >= 0
                || val.IndexOf("cdx") >= 0
                || val.IndexOf("cer") >= 0
                || val.IndexOf("jsp") >= 0
                || val.IndexOf("php") >= 0)
            {
                bChk = false;
            }

            return bChk;
        }
        #endregion
    }
}
