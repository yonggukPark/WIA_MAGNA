using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace HQCWeb.FW
{
    public class Dictionary_Data
    {
        public static Hashtable htDictionaryList;
        private static ArrayList alKeys;
        private static ArrayList alValues;

        public static string DefaultLangType { get; set; } = "EN-US";


        public static void SetDictionaryMemory(DataSet ds)
        {
            if (Dictionary_Data.htDictionaryList == null || Dictionary_Data.htDictionaryList.Count == 0)
            {
                Dictionary_Data.htDictionaryList = new Hashtable();
                Dictionary_Data.htDictionaryList.Clear();
            }
            Dictionary_Data.SetDictionaryMemory(ds.Tables[0]);
        }

        public static void SetDictionaryMemory(DataTable dt)
        {
            if (Dictionary_Data.htDictionaryList == null || Dictionary_Data.htDictionaryList.Count == 0)
            {
                Dictionary_Data.htDictionaryList = new Hashtable();
                Dictionary_Data.htDictionaryList.Clear();
            }
            Dictionary_Data.alKeys = new ArrayList();
            Dictionary_Data.alValues = new ArrayList();

            try
            {
                for (int idx = 0; idx < dt.Rows.Count; ++idx)
                {
                    DictionaryInfo dictionaryInfo = new DictionaryInfo();
                    Dictionary_Data.alKeys.Add((object)dt.Rows[idx]["DIC_ID"].ToString().Trim());
                    dictionaryInfo.DicKo = dt.Rows[idx]["DIC_TXT_KR"].ToString().Trim();
                    dictionaryInfo.DicEn = dt.Rows[idx]["DIC_TXT_EN"].ToString().Trim();
                    dictionaryInfo.DicLo = dt.Rows[idx]["DIC_TXT_LO"].ToString().Trim();
                    Dictionary_Data.alValues.Add((object)dictionaryInfo);

                }

                if (Dictionary_Data.alKeys.Count > 0)
                {
                    for (int idx = 0; idx < Dictionary_Data.alKeys.Count; ++idx)
                    {
                        if (!Dictionary_Data.htDictionaryList.Contains(Dictionary_Data.alKeys[idx]))
                        {
                            Dictionary_Data.htDictionaryList.Add((object)(string)Dictionary_Data.alKeys[idx], (object)(DictionaryInfo)Dictionary_Data.alValues[idx]);
                        }
                    }
                }
                else
                {
                    Dictionary_Data.htDictionaryList = new Hashtable();
                    Dictionary_Data.alKeys.Clear();
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
            DictionaryInfo dictionaryInfo = new DictionaryInfo();
            if (dicId != null && dicId.Length > 0)
            {
                try
                {
                    if (Dictionary_Data.htDictionaryList != null)
                    {
                        DictionaryInfo dicInfo = (DictionaryInfo)Dictionary_Data.htDictionaryList[(object)dicId];
                        if (dicInfo != null)
                        {
                            switch (langType.ToUpper())
                            {
                                case "KO_KR":
                                    dictionaryInfo.DicText = dicInfo.DicKo;
                                    break;
                                case "EN_US":
                                    dictionaryInfo.DicText = dicInfo.DicEn;
                                    break;
                                case "LO_LN":
                                    dictionaryInfo.DicText = dicInfo.DicLo;
                                    break;
                                default:
                                    dictionaryInfo.DicText = dicInfo.DicLo;
                                    break;

                            }
                        }
                    }

                    if (dictionaryInfo.DicText == null || dictionaryInfo.DicText.Length == 0)
                    {
                        dictionaryInfo.DicText = dicId;
                    }
                    else if (dictionaryInfo.DicText.Trim() == string.Empty)
                    {
                        dictionaryInfo.DicText = dicId;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return dictionaryInfo.DicText;
        }
    }
}