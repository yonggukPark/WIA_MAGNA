using HQC.FW.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml.XPath;

namespace HQCWeb.FW
{
    public class Message_Data
    {
        public static Hashtable htMessageList;
        private static ArrayList alKeys;
        private static ArrayList alValues;

        public static string DefaultLangType { get; set; } = "EN-US";
        

        public static void SetMessageMemory(DataSet ds)
        {
            if (Message_Data.htMessageList == null || Message_Data.htMessageList.Count == 0)
            {
                Message_Data.htMessageList = new Hashtable();
                Message_Data.htMessageList.Clear();
            }
            Message_Data.SetMessageMemory(ds.Tables[0]);
        }

        public static void SetMessageMemory(DataTable dt)
        {
            if (Message_Data.htMessageList == null || Message_Data.htMessageList.Count == 0)
            {
                Message_Data.htMessageList = new Hashtable();
                Message_Data.htMessageList.Clear();
            }
            Message_Data.alKeys = new ArrayList();
            Message_Data.alValues = new ArrayList();

            try
            {
                for (int idx = 0; idx < dt.Rows.Count; ++idx)
                {
                    MessageInfo MessageInfo = new MessageInfo();
                    Message_Data.alKeys.Add((object)dt.Rows[idx]["MSG_ID"].ToString().Trim());
                    MessageInfo.DicKo = dt.Rows[idx]["MSG_TXT_KR"].ToString().Trim();
                    MessageInfo.DicEn = dt.Rows[idx]["MSG_TXT_EN"].ToString().Trim();
                    MessageInfo.DicLo = dt.Rows[idx]["MSG_TXT_LO"].ToString().Trim();
                    Message_Data.alValues.Add((object)MessageInfo);

                }

                if (Message_Data.alKeys.Count > 0)
                {
                    for (int idx = 0; idx < Message_Data.alKeys.Count; ++idx)
                    {
                        if (!Message_Data.htMessageList.Contains(Message_Data.alKeys[idx]))
                        {
                            Message_Data.htMessageList.Add((object)(string)Message_Data.alKeys[idx], (object)(MessageInfo)Message_Data.alValues[idx]);
                        }
                    }
                }
                else
                {
                    Message_Data.htMessageList = new Hashtable();
                    Message_Data.alKeys.Clear();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string SearchDic(string msgId, string langType, bool isSearchToFile = true, params string[] addMsg)
        {

            string strTemp = SearchDic(msgId, langType, isSearchToFile);
            if (addMsg != null && addMsg.Length > 0)
            {
                strTemp = string.Format(strTemp, addMsg);
            }
            return strTemp;
        }
        public static string SearchDic(string msgId, params string[] addMsg)
        {
            return SearchDic(msgId, DefaultLangType, true, addMsg);
        }
        public static string SearchDic(string msgId, bool isSearchToFile = true, params string[] addMsg)
        {
            return SearchDic(msgId, DefaultLangType, isSearchToFile, addMsg);
        }
        public static string SearchDic(string dicId, string langType, bool isSearchToFile = true)
        {
            MessageInfo MessageInfo = new MessageInfo();
            if (dicId != null && dicId.Length > 0)
            {
                try
                {
                    if (Message_Data.htMessageList != null)
                    {
                        MessageInfo dicInfo = (MessageInfo)Message_Data.htMessageList[(object)dicId];
                        if (dicInfo != null)
                        {
                            switch (langType.ToUpper())
                            {
                                case "KO_KR":
                                    MessageInfo.DicText = dicInfo.DicKo;
                                    break;
                                case "EN_US":
                                    MessageInfo.DicText = dicInfo.DicEn;
                                    break;
                                case "LO_LN":
                                    MessageInfo.DicText = dicInfo.DicLo;
                                    break;
                                default:
                                    MessageInfo.DicText = dicInfo.DicLo;
                                    break;

                            }
                        }
                    }

                    if (MessageInfo.DicText == null || MessageInfo.DicText.Length == 0)
                    {
                        MessageInfo.DicText = dicId;
                    }
                    else if (MessageInfo.DicText.Trim() == string.Empty)
                    {
                        MessageInfo.DicText = dicId;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return MessageInfo.DicText;
        }
    }
}