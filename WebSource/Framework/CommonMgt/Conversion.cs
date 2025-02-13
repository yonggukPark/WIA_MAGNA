using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MES.FW.Common.CommonMgt
{
    public class Conversion
    {
        public int nation_code = 1;

        public Conversion()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        public string GetEncordingPassword(string emp_pwd)
        {
            string strTemp = string.Empty;
            char[] strChar = emp_pwd.ToCharArray();

            for (int i = 0; i < strChar.Length; i++)
            {
                strTemp = strTemp + (char)(strChar[i] ^ 127);
            }

            return strTemp;
        }

        public string SetRowSumScript(string sValues, string sSum)
        {
            string sSumPSScript = "";

            sSumPSScript = "var obj=document.Form1;";
            sSumPSScript += "var sum=0; ";
            sSumPSScript += " if(obj." + sValues + ".length) {";
            sSumPSScript += " for(i=0;i<obj." + sValues + ".length; i++) {";
            sSumPSScript += " if(obj." + sValues + "[i].value*0==0) {";
            sSumPSScript += " sum += obj." + sValues + "[i].value*1; } }";
            sSumPSScript += " } else {";
            sSumPSScript += " sum += obj." + sValues + ".value*1;  }";
            sSumPSScript += " obj." + sSum + ".value = Math.round(eval(sum)*100)/100; ";

            return sSumPSScript;
        }

        public string SetColSubScript(string sSum, string sValue1, string sValue2)
        {
            string sSumPSScript = "";

            sSumPSScript = "var obj=document.Form1;";
            sSumPSScript += "obj." + sSum + ".value = Math.round(eval(Number(obj." + sValue1 + ".value)-Number(obj." + sValue2 + ".value))*100)/100;";

            return sSumPSScript;
        }

        public string SetColSumScript(string sSum, string sValue1, string sValue2)
        {
            string sSumPSScript = "";

            sSumPSScript = "var obj=document.Form1;";
            sSumPSScript += "obj." + sSum + ".value = Math.round(eval(Number(obj." + sValue1 + ".value)+Number(obj." + sValue2 + ".value))*100)/100;";

            return sSumPSScript;
        }

        public string SetColSumScript(string sSum, string sValue1, string sValue2, string sValue3)
        {
            string sSumPSScript = "";

            sSumPSScript = "var obj=document.Form1;";
            sSumPSScript += "obj." + sSum + ".value = Math.round(eval(Number(obj." + sValue1 + ".value)+Number(obj." + sValue2 + ".value)+Number(obj." + sValue3 + ".value))*100)/100;";

            return sSumPSScript;
        }

        public string SetColDoubleScript(string sSum, string sValue1, string sValue2)
        {
            string sSumPSScript = "";

            sSumPSScript = "var obj=document.Form1;";
            sSumPSScript += "obj." + sSum + ".value = Math.round(eval(Number(obj." + sValue1 + ".value)*Number(obj." + sValue2 + ".value))*100)/100;";

            return sSumPSScript;
        }

        public string SetBadRateScript(int itempCnt, string sValues)
        {
            string sSumPSScript = "";
            string iCnt = itempCnt.ToString();
            sValues = sValues.Substring(0, 1);

            sSumPSScript = "var obj=document.Form1;";
            sSumPSScript += "var vTol=0;";
            sSumPSScript += "if(obj.p_" + sValues + "OKCnt[" + iCnt + "].value && obj.p_" + sValues + "NGCnt[" + iCnt + "].value) {";
            sSumPSScript += " vTol=Number(obj.p_" + sValues + "OKCnt[" + iCnt + "].value)+Number(obj.p_" + sValues + "NGCnt[" + iCnt + "].value);";
            sSumPSScript += " if(vTol==0) {";
            sSumPSScript += " obj.sum_" + sValues + "GR[" + iCnt + "].value = 0; ";
            sSumPSScript += " } else { ";
            sSumPSScript += " obj.sum_" + sValues + "GR[" + iCnt + "].value = Math.round(((Number(obj.p_" + sValues + "OKCnt[" + iCnt + "].value)/vTol)*100)*100)/100; }";
            sSumPSScript += " } else { ";
            sSumPSScript += "   return false; }";

            return sSumPSScript;
        }

        public string trim(string str)
        {
            return (str == null || str.Trim() == null || str.Trim().Equals("")) ? "" : str.Trim();
        }

        public string trimalpha(string str)
        {
            string alphaCheck = "1234567890";
            string sTempstr = "";
            string sReturnstr = "";

            for (int i = 0; i < str.Length; i++)
            {
                sTempstr = str.Substring(i, 1).ToString();

                if (alphaCheck.IndexOf(sTempstr) >= 1)
                {
                    sReturnstr = sReturnstr + sTempstr;
                }
            }

            return sReturnstr;
        }

        public bool intalphaTypeCheck(string str)
        {
            string intalphaCheck = "1234567890abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string sTempstr = "";

            for (int i = 0; i < str.Length; i++)
            {
                sTempstr = str.Substring(i, 1).ToString();

                if (intalphaCheck.IndexOf(sTempstr) == -1)
                {
                    break;
                }
            }

            if (intalphaCheck.IndexOf(sTempstr) == -1)
            {
                return false;
            }
            return true;
        }

        public string trimalpha(string str, string squbun)
        {
            string alphaCheck = "1234567890" + squbun;
            string sTempstr = "";
            string sReturnstr = "";

            for (int i = 0; i < str.Length; i++)
            {
                sTempstr = str.Substring(i, 1).ToString();

                if (alphaCheck.IndexOf(sTempstr) >= 1)
                {
                    sReturnstr = sReturnstr + sTempstr;
                }
            }

            return sReturnstr;
        }

        public string trim(string str1, string str2)
        {
            return (!trim(str1).Equals("")) ? str1.Trim() : (trim(str2).Equals("")) ? " " : str2;
        }

        public int page(string str)
        {
            return (trim(str).Equals("")) ? 1 : Convert.ToInt32(trim(str));
        }

        public string setDate(int nation_code, string data, string str) // 국가구분, 날짜, 변경할 문자
        {
            StringBuilder rtn = new StringBuilder();

            string sPlantDate = "";

            //sPlantDate = CondUtils.getData("select 'PlantDate'=dbo.uf_YYYYMMDD(dbo.uf_DateChar(getdate()))", "PlantDate");

            if (trim(str).Equals("")) str = (nation_code == 1) ? "-" : ".";
            data = (trim(data).Equals("")) ? sPlantDate : data.Trim();

            string y = (data.Length >= 4) ? data.Substring(0, 4) : "";
            string m = (data.Length >= 6) ? data.Substring(4, 2) : "";
            string d = (data.Length >= 8) ? data.Substring(6, 2) : "";

            if (y.Length > 0 && nation_code == 1) y += str;
            if ((m.Length > 0 && nation_code == 1) || (d.Length > 0 && nation_code == 1)) m += str;
            if (d.Length > 0 && nation_code != 1) d += str;

            return (nation_code == 1) ? rtn.Append(y).Append(m).Append(d).ToString() :
              (nation_code == 2) ? rtn.Append(m).Append(d).Append(y).ToString() :
              rtn.Append(d).Append(m).Append(y).ToString();
        }

        public string setYYYYMM(string str)
        {
            StringBuilder strRtn = new StringBuilder();

            if (str.Length >= 4)
                strRtn.Append(str.Substring(0, 4)).Append("/");
            if (str.Length < 6)
                strRtn.Append(str.Substring(4));
            else
                strRtn.Append(str.Substring(4, 2));

            return strRtn.ToString();
        }

        public string setYYYYMMDD(string str)
        {
            StringBuilder strRtn = new StringBuilder();

            if (str.Length >= 4)
                strRtn.Append(str.Substring(0, 4)).Append("-");
            if (str.Length >= 6)
                strRtn.Append(str.Substring(4, 2)).Append("-");
            if (str.Length < 6)
                strRtn.Append(str.Substring(6));
            else
                strRtn.Append(str.Substring(6, 2));

            return strRtn.ToString();
        }

        public string setYYYYMMDate(string str)
        {
            StringBuilder strRtn = new StringBuilder();

            if (str.Length >= 4)
                strRtn.Append(str.Substring(0, 4)).Append("/");
            if (str.Length >= 6)
                strRtn.Append(str.Substring(4, 2)).Append("/");
            if (str.Length >= 8)
                strRtn.Append(str.Substring(6, 2)).Append(" ");
            if (str.Length >= 10)
                strRtn.Append(str.Substring(8, 2)).Append(":");
            if (str.Length >= 12)
                strRtn.Append(str.Substring(10, 2)).Append(":");
            if (str.Length <= 14)
                strRtn.Append(str.Substring(12));
            else
                strRtn.Append(str.Substring(12, 2));

            return strRtn.ToString();
        }

        public string setMMDate(string str)
        {
            StringBuilder strRtn = new StringBuilder();

            if (str.Length >= 6)
                strRtn.Append(str.Substring(4, 2)).Append("/");
            if (str.Length >= 8)
                strRtn.Append(str.Substring(6, 2)).Append(" ");
            if (str.Length >= 10)
                strRtn.Append(str.Substring(8, 2)).Append(":");
            if (str.Length >= 12)
                strRtn.Append(str.Substring(10, 2)).Append(":");
            if (str.Length <= 14)
                strRtn.Append(str.Substring(12));
            else
                strRtn.Append(str.Substring(12, 2));

            return strRtn.ToString();
        }

        public string setMMHHMM(string str)
        {
            StringBuilder strRtn = new StringBuilder();

            if (str.Length >= 6)
                strRtn.Append(str.Substring(4, 2)).Append("/");
            if (str.Length >= 8)
                strRtn.Append(str.Substring(6, 2)).Append(" ");
            if (str.Length >= 10)
                strRtn.Append(str.Substring(8, 2)).Append(":");
            if (str.Length >= 12)
                strRtn.Append(str.Substring(10, 2));

            return strRtn.ToString();
        }

        public string setDate(string data)
        {
            return setDate(nation_code, data);
        }

        public string setDate(int n_code, string data)
        {
            return setDate(n_code, data, "");
        }

        public string getDate(int nation_code, string data)
        {
            StringBuilder strRtn = new StringBuilder();
            string str = (nation_code == 1) ? "-" : ".";

            data = (trim(data).Equals("")) ? setDate("").Trim().Replace(str, "") : data.Trim().Replace(str, "");

            return (nation_code == 1) ? strRtn.Append(data.Substring(0, 4)).Append(data.Substring(4, 2)).Append(data.Substring(6, 2)).ToString() :
              (nation_code == 2) ? strRtn.Append(data.Substring(4, 4)).Append(data.Substring(0, 2)).Append(data.Substring(2, 2)).ToString() :
              strRtn.Append(data.Substring(4, 4)).Append(data.Substring(2, 2)).Append(data.Substring(0, 2)).ToString();
        }

        public string getDate(string str)
        {
            return getDate(nation_code, str);
        }

        public string setTime(string str)
        {
            StringBuilder strRtn = new StringBuilder();

            if (str.Length >= 10)
                strRtn.Append(str.Substring(8, 2)).Append(":");
            if (str.Length >= 12)
                strRtn.Append(str.Substring(10, 2)).Append(":");
            if (str.Length <= 14)
                strRtn.Append(str.Substring(12));
            else
                strRtn.Append(str.Substring(12, 2));

            return strRtn.ToString();
        }

        public string setDual(string str)
        {
            int istr = Convert.ToInt32(str);

            if (istr < 10)
                str = "0" + istr.ToString();
            else
                str = istr.ToString();

            return str;
        }

        public string setCovertTime(string str)
        {
            StringBuilder strRtn = new StringBuilder();

            int istr = Convert.ToInt32(str);
            DateTime dstr = new DateTime(0);
            dstr = dstr.AddSeconds(istr);

            strRtn.Append(setDual(dstr.Hour.ToString())).Append(":")
              .Append(setDual(dstr.Minute.ToString())).Append(":").Append(setDual(dstr.Second.ToString()));

            return strRtn.ToString();
        }

        public string checkType(string str1, string str2)
        {
            return (str2.Equals("DATE")) ? setDate(str1) : (str2.Equals("DATETIME")) ? setDate(str1) + " " + setTime(str1) : str1;
        }

        public string fill(string str, string fill, int nLength, bool first)
        {
            StringBuilder strRtn = new StringBuilder();
            str = trim(str);
            for (int i = 0; i < nLength - str.Length; i++) strRtn.Append(fill);

            return (first) ? strRtn.Append(str).ToString() : str + strRtn.ToString();
        }

        // ---------------------------------------- 15만대에서 쓰는 함수 입니다 -------------------------------------------
        public string DataTrim(object strValue)
        {
            var returnValue = Convert.ToString(strValue);

            return string.IsNullOrEmpty(returnValue) ? string.Empty : returnValue.Trim();
        }

        public string DataTrim(object strValue, int length)
        {
            var returnValue = DataTrim(strValue);
            var stringLength = returnValue.Length;

            if (stringLength > length)
                returnValue = returnValue.Substring(0, length) + "...";

            return returnValue;
        }

        public string DataNoTrim(object strValue)
        {
            var returnValue = Convert.ToString(strValue);

            return string.IsNullOrEmpty(returnValue) ? string.Empty : returnValue;
        }

        public string DataNoTrim(object strValue, int length)
        {
            var returnValue = DataNoTrim(strValue);
            var stringLength = returnValue.Length;

            if (stringLength > length)
                returnValue = returnValue.Substring(0, length) + "...";

            return string.IsNullOrEmpty(returnValue) ? string.Empty : returnValue;
        }

        public string ReplaceParam(object paramValue, bool isTrim)
        {
            var returnValue = (isTrim ? DataTrim(paramValue) : DataNoTrim(paramValue));

            return string.IsNullOrEmpty(returnValue) ? string.Empty : returnValue.Replace("\'", "\'\'");
        }

        public string ReplaceParam(object paramValue)
        {
            return ReplaceParam(paramValue, false);
        }

        public string DateSimpleFormat(object strValue, string dateDelimiter)
        {
            var returnValue = DataTrim(strValue);
            var stringLength = returnValue.Length;

            switch (stringLength)
            {
                case 8:
                    return returnValue.Substring(4, (stringLength - 4)).Insert(2, dateDelimiter);
                case 14:
                    return returnValue.Substring(4, (stringLength - 4)).Insert(8, ":").Insert(6, ":").Insert(4, " ").Insert(2, dateDelimiter);
                case 17:
                    return returnValue.Substring(4, (stringLength - 4)).Insert(10, ".").Insert(8, ":").Insert(6, ":").Insert(4, " ").Insert(2, dateDelimiter);
                default:
                    return string.Empty;
            }
        }

        public string DateSimpleFormat(object strValue)
        {
            return DateSimpleFormat(strValue, "/");
        }
        // ---------------------------------------- 15만대에서 쓰는 함수 입니다 -------------------------------------------

        public bool IsDouble(object obj)
        {
            bool rtn = true;
            try { Double.Parse(obj.ToString()); }
            catch { rtn = false; }
            return rtn;
        }

        public bool IsDouble(string str)
        {
            bool rtn = true;
            try { Double.Parse(str); }
            catch { rtn = false; }
            return rtn;
        }

        public bool IsInteger(object obj)
        {
            bool rtn = true;
            try { Int32.Parse(obj.ToString()); }
            catch { rtn = false; }
            return rtn;
        }

        public bool IsInteger(string str)
        {
            bool rtn = true;
            try { Double.Parse(str); }
            catch { rtn = false; }
            return rtn;
        }

        public string TrimLen(string Value, int txtLen)
        {
            int Len = Value.Length;
            string RetVal = Value;

            if (Len > txtLen)
            {
                RetVal = Value.Substring(0, txtLen) + "...";
            }
            return RetVal;
        }
    }
}
