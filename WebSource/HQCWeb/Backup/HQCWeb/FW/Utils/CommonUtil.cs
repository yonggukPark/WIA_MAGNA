using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using HQCWeb.FMB_FW.Data;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HQCWeb.FMB_FW.Utils
{
    public class CommonUtil
    {
        public CommonUtil() { }

        /// <summary>
        /// 전송용 Json Object를 생성하여 리턴한다. 
        /// </summary>
        /// <param name="strMessageName"></param>
        /// <param name="strTransactionId"></param>
        /// <param name="strReplysubjectname"></param>
        /// <param name="strSiteId"></param>
        /// <param name="strQueryId">SQLMAP에 생성된 호출할 strQueryId 입력</param>
        /// <param name="parameter">쿼리 실행시 전달할 parameter</param>
        /// <param name="strQueryType">쿼리 종류[SELECT:0, INSERT:1, UPDATE:2,DELETE:3, MERGE:4</param>
        /// <param name="strDbName"></param>
        /// <returns></returns>
        public JObject SetSendMessage(string strMessageName, string strTransactionId, string strReplysubjectname, string strSiteId, string strQueryId, Parameters parameter, string strQueryType, string strDbName)
        {
            DataTable dt = null;
            JObject jDoc = JsonUtils.MakeBaseMessage(strMessageName, strTransactionId, strReplysubjectname, strSiteId);
            JsonUtils.AddTextNode(jDoc, "//message/body", "DBNAME", string.IsNullOrEmpty(strDbName) ? "": strDbName);
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYID", strQueryId);
            JsonUtils.AddTextNode(jDoc, "//message/body", "QUERYTYPE", strQueryType);
            JObject jParams = JsonUtils.AddObjectNode(jDoc, "//message/body", "PARAMS");

            /*if (parameter != null && parameter.Count > 0)
            {
                foreach (object key in parameter.GetParmas().Keys)
                {
                    JsonUtils.AddChildTextNode(jParams, (string)key, (string)parameter.GetParmas()[key]);
                }
            }*/



            if (parameter != null && parameter.Count > 0)
            {
                foreach (object key in parameter.GetParmas().Keys)
                {
                    if (parameter.GetParmas()[key].GetType().Name == "List`1")
                    {
                        JObject parentNode = JsonUtils.GetNode(jDoc, "//message/body/PARAMS") as JObject;
                        JArray jSubParams = JsonUtils.AddChildArrayNode(parentNode, (string)key);
                        List<Hashtable> _list = (List<Hashtable>)(parameter.GetParmas()[key]);
                        for(int i=0;i<_list.Count; i++)
                        {
                            Hashtable htSub = _list[i];
                            JObject jo = new JObject();
                            foreach (object subkey in htSub.Keys)
                            {
                                //JsonUtils.AddChildNode(jo, (string)subkey, (string)htSub[subkey]);
                                JsonUtils.AddChildTextNode(jo, (string)subkey, (string)htSub[subkey]);
                            }
                            JsonUtils.AddArrayObject(jSubParams, jo);
                            //JsonUtils.AddChildTextNode(jSubParams, (string)subkey, (string)htSub[subkey]);
                        }
                    }
                    else
                    {
                        JsonUtils.AddChildTextNode(jParams, (string)key, (string)parameter.GetParmas()[key]);
                    }
                }
            }


            return jDoc;
        }

        public JObject SetSendMessage(string strMessageName, string strTransactionId, string strSiteId, string strQueryId, Parameters parameter, string queryType, string strDbName = null)
        {
            return SetSendMessage(strMessageName, strTransactionId, string.Empty, strSiteId, strQueryId, parameter, queryType, strDbName);
        }
        public JObject SetSendMessage(string strMessageName, string strQueryId, Parameters parameter, string queryType, string strDbName = null)
        {
            return SetSendMessage(strMessageName, string.Empty, string.Empty, string.Empty, strQueryId, parameter, queryType, strDbName);
        }
        public JObject SetSendMessage(string strMessageName, Parameters parameter, string queryType, string strDbName = null)
        {
            return SetSendMessage(strMessageName, string.Empty, string.Empty, string.Empty, string.Empty, parameter, queryType, strDbName);
        }

        public static Hashtable convertDataRowToHashTable(DataRow drIn)
        {
            Hashtable htOut = new Hashtable();

            for(int i=0; i<drIn.Table.Columns.Count; i++)
            {
                htOut.Add(drIn.Table.Columns[i].ColumnName, drIn[drIn.Table.Columns[i].ColumnName].ToString());
            }
            return htOut;
        }
        public Hashtable convertDataTableToHashTable(DataTable dtIn, string keyField, string valueField)
        {
            Hashtable htOut = new Hashtable();

            foreach (DataRow drIn in dtIn.Rows)
            {
                htOut.Add(drIn[keyField].ToString(), drIn[valueField].ToString());
            }
            return htOut;
        }

        // jolee 추가
        public DataTable SetDataListToDataTable(string receiveResult)
        {
            DataTable dt = null;

            JObject jDoc1 = JObject.Parse(receiveResult);
            JArray jColumns = (JArray)JsonUtils.GetNode(jDoc1, "//message/return/returnColumns");
            JArray jlist = (JArray)JsonUtils.GetNode(jDoc1, "//message/return/returnTable");
            string returnCode = JsonUtils.GetNodeText(jDoc1, "//message/return/returncode");

            if (returnCode == "0" || returnCode == "1")
            {
                dt = JsonUtils.ConvertJsonToDataTable(jlist);

                if (dt == null || dt.Columns.Count == 0)
                {
                    foreach (JValue jobj in jColumns)
                    {
                        dt = new DataTable();
                        dt.Columns.Add(jobj.Value.ToString());
                    }
                }
            }

            return dt;
        }

        /// <summary>
        /// 날짜 형식 값을 문자열로 변환한다.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="stringFormat">표시할 날짜 형식(yyyy,MM,dd, HH, mm, ss) 사용가능</param>
        /// <returns></returns>

        public static string GetDateToString(object obj, string stringFormat = null)
        {
            string rtnString = string.Empty;
            if (obj == null) return rtnString;

            if (obj.GetType().Name == "DateTime")
            {
                if (stringFormat != null)
                {
                    rtnString = ((DateTime)obj).ToString(stringFormat.Replace("/", @"\/"));
                }
                else
                {
                    rtnString = ((DateTime)obj).ToString();
                }
            }
            else if (obj.GetType().Name == "String")
            {
                string str = (string)obj;
                long lstr = 0;
                if (Int64.TryParse(str, out lstr))
                {
                    if (str.Length == 4)
                    {
                        DateTime dt = Convert.ToDateTime(str + "-01-01");
                        if (stringFormat != null)
                        {
                            rtnString = ((DateTime)obj).ToString(stringFormat.Replace("/", @"\/"));
                        }
                        else
                        {
                            rtnString = ((DateTime)obj).ToString();
                        }
                    }
                    if (str.Length == 6)
                    {
                        DateTime dt = Convert.ToDateTime(
                            str.Substring(0, 4)
                            + "-"
                            + str.Substring(4, 2)
                            + "-"
                            + "01");
                        if (stringFormat != null)
                        {
                            rtnString = ((DateTime)obj).ToString(stringFormat.Replace("/", @"\/"));
                        }
                        else
                        {
                            rtnString = ((DateTime)obj).ToString();
                        }
                    }
                    else if (str.Length == 8)
                    {
                        DateTime dt = Convert.ToDateTime(
                            str.Substring(0, 4)
                            + "-"
                            + str.Substring(4, 2)
                            + "-"
                            + str.Substring(6, 2));
                        if (stringFormat != null)
                        {
                            rtnString = ((DateTime)obj).ToString(stringFormat.Replace("/", @"\/"));
                        }
                        else
                        {
                            rtnString = ((DateTime)obj).ToString();
                        }
                    }
                    else if (str.Length == 10)
                    {
                        DateTime dt = Convert.ToDateTime(
                            str.Substring(0, 4)
                            + "-"
                            + str.Substring(4, 2)
                            + "-"
                            + str.Substring(6, 2)
                            + " "
                            + str.Substring(8, 2));
                        if (stringFormat != null)
                        {
                            rtnString = ((DateTime)obj).ToString(stringFormat.Replace("/", @"\/"));
                        }
                        else
                        {
                            rtnString = ((DateTime)obj).ToString();
                        }
                    }
                    else if (str.Length == 12)
                    {
                        DateTime dt = Convert.ToDateTime(
                            str.Substring(0, 4)
                            + "-"
                            + str.Substring(4, 2)
                            + "-"
                            + str.Substring(6, 2)
                            + " "
                            + str.Substring(8, 2)
                            + ":"
                            + str.Substring(10, 2));
                        if (stringFormat != null)
                        {
                            rtnString = ((DateTime)obj).ToString(stringFormat.Replace("/", @"\/"));
                        }
                        else
                        {
                            rtnString = ((DateTime)obj).ToString();
                        }
                    }
                    else if (str.Length == 14)
                    {
                        DateTime dt = Convert.ToDateTime(
                            str.Substring(0, 4)
                            + "-"
                            + str.Substring(4, 2)
                            + "-"
                            + str.Substring(6, 2)
                            + " "
                            + str.Substring(8, 2)
                            + ":"
                            + str.Substring(10, 2)
                            + ":"
                            + str.Substring(12, 2));
                        if (stringFormat != null)
                        {
                            rtnString = ((DateTime)obj).ToString(stringFormat.Replace("/", @"\/"));
                        }
                        else
                        {
                            rtnString = ((DateTime)obj).ToString();
                        }
                    }
                }
                else
                {
                    try
                    {
                        DateTime dt = Convert.ToDateTime(str);
                        if (stringFormat != null)
                        {
                            rtnString = ((DateTime)obj).ToString(stringFormat.Replace("/", @"\/"));
                        }
                        else
                        {
                            rtnString = ((DateTime)obj).ToString();
                        }

                    }
                    catch (System.Exception ex)
                    {
                        rtnString = ex.Message;
                    }
                }
            }
            else
            {
                rtnString = obj.ToString();
            }

            return rtnString;
        }
    }
}
