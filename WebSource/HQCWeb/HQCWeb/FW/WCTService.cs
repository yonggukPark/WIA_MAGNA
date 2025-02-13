using System;
using System.Collections.Generic;
using System.Data;

using System.Dynamic;
using Newtonsoft.Json;

using MES.FW.Common.Crypt;

namespace HQCWeb.FW
{
    public class WCTService
    {
        #region WCTDatepickerData
        //플래그를 받아서 치환작업
        public static string[] WCTDatepickerData(string type)
        {
            DateTime start = DateTime.Now;
            DateTime end = DateTime.Now;
            string[] strRtn = new string[]{string.Empty, string.Empty};

            if (type.Equals("A"))//당일
            {
                strRtn[0] = start.ToString("yyyy-MM-dd");
                strRtn[1] = end.ToString("yyyy-MM-dd");
            }
            else if (type.Equals("B"))//전일
            {
                strRtn[0] = start.AddDays(-1).ToString("yyyy-MM-dd");
                strRtn[1] = end.AddDays(-1).ToString("yyyy-MM-dd");
            }
            else if (type.Equals("C"))//전전일
            {
                strRtn[0] = start.AddDays(-2).ToString("yyyy-MM-dd");
                strRtn[1] = end.AddDays(-2).ToString("yyyy-MM-dd");
            }
            else if (type.Equals("D"))//당주
            {
                start = start.AddDays(-(int)start.DayOfWeek + (int)DayOfWeek.Monday);
                end = start.AddDays(6);
                strRtn[0] = start.ToString("yyyy-MM-dd");
                strRtn[1] = end.ToString("yyyy-MM-dd");
            }
            else if (type.Equals("E"))//전주
            {
                start = start.AddDays(-(int)start.DayOfWeek + (int)DayOfWeek.Monday - 7);
                end = start.AddDays(6);
                strRtn[0] = start.ToString("yyyy-MM-dd");
                strRtn[1] = end.ToString("yyyy-MM-dd");
            }
            else if (type.Equals("F"))//당월
            {
                start = new DateTime(start.Year, start.Month, 1);
                end = start.AddMonths(1).AddDays(-1);
                strRtn[0] = start.ToString("yyyy-MM-dd");
                strRtn[1] = end.ToString("yyyy-MM-dd");
            }
            else if (type.Equals("G"))//전월
            {
                start = new DateTime(start.Year, start.Month, 1).AddMonths(-1);
                end = start.AddMonths(1).AddDays(-1);
                strRtn[0] = start.ToString("yyyy-MM-dd");
                strRtn[1] = end.ToString("yyyy-MM-dd");
            }
            else if (type.Equals("I"))//내일
            {
                strRtn[0] = start.AddDays(1).ToString("yyyy-MM-dd");
                strRtn[1] = end.AddDays(1).ToString("yyyy-MM-dd");
            }

            return strRtn;
        }
        #endregion
    }
}