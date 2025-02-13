using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
//using CIM.MES.Framework;
//using MES.Common.Constants;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TIBCO.Rendezvous;
using HQCWeb.FMB_FW.Constants;
using System.ComponentModel;

namespace HQCWeb.FMB_FW
{
    public class JsonUtils
    {
        #region Convert Method
        /// <summary>
        /// XML을 Json으로 변환한다.
        /// </summary>
        /// <param name="xml">xml</param>
        /// <returns></returns>
        public static string ConvertXmlToJson(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            string strJson = JsonConvert.SerializeXmlNode(doc);

            return strJson;
        }

        /// <summary>
        /// XML을 Json으로 변환한다.
        /// </summary>
        /// <param name="doc">xml</param>
        /// <returns></returns>
        public static string ConvertXmlToJson(XmlDocument doc)
        {
            string strJson = JsonConvert.SerializeXmlNode(doc);

            return strJson;
        }

        /// <summary>
        /// Json을 XML로 변환한다.
        /// </summary>
        /// <param name="strJson">json</param>
        /// <returns></returns>
        public static XmlDocument ConvertJsonToXml(string strJson)
        {
            XmlDocument doc = JsonConvert.DeserializeXmlNode(strJson);

            return doc;
        }

        /// <summary>
        /// Json을 XML로 변환한다.
        /// </summary>
        /// <param name="docJson">json</param>
        /// <returns></returns>
        public static XmlDocument ConvertJsonToXml(JObject docJson)
        {
            XmlDocument doc = ConvertJsonToXml(docJson.ToString());

            return doc;
        }

        /// <summary>
        /// 주소 Replace('/'-> '.')
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string ReplacePath(string path)
        {
            if (path.Contains("/"))
            {
                path = path.Replace("/", ".");
            }

            if (path == ".")
                path = string.Empty;

            return path;
        }

        public static JArray ConvertDataTableToJsonArray(DataTable dtSource)
        {
            JArray arrJson = null;

            string result = JsonConvert.SerializeObject(dtSource);

            arrJson = JArray.Parse(result);

            return arrJson;
        }

        public static DataTable ConvertJsonToDataTable(JToken arrJson)
        {
            DataTable dtResult = new DataTable();

            if (arrJson != null)
            {
                dtResult = JsonConvert.DeserializeObject(Convert.ToString(arrJson), typeof(DataTable)) as DataTable;
            }

            return dtResult;
        }

        public static DataTable ConvertJsonToDataTable(string arrJson)
        {
            DataTable dtResult = new DataTable();

            if (!string.IsNullOrEmpty(arrJson))
            {
                dtResult = JsonConvert.DeserializeObject(arrJson, typeof(DataTable)) as DataTable;
            }

            return dtResult as DataTable;
        }

        public static string ConvertDataTableToJsonString(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Merge(dt);
            StringBuilder JsonString = new StringBuilder();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                JsonString.Append("[");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    JsonString.Append("{");
                    for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                    {
                        if (j < ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\",");
                        }
                        else if (j == ds.Tables[0].Columns.Count - 1)
                        {
                            JsonString.Append("\"" + ds.Tables[0].Columns[j].ColumnName.ToString() + "\":" + "\"" + ds.Tables[0].Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == ds.Tables[0].Rows.Count - 1)
                    {
                        JsonString.Append("}");
                    }
                    else
                    {
                        JsonString.Append("},");
                    }
                }
                JsonString.Append("]");
                return JsonString.ToString();
            }
            else
            {
                return null;
            }
        }

        public static DataTable ConvertJTokenListToDataTable(List<JToken> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(data);
            DataTable table = new DataTable();
            object[] values = new object[props.Count];
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, typeof(string));
            }
            //for (int i = 0; i < data.Count; i++)
            //{
            //    data[i].Values().ToArray().CopyTo(values, i);
            //    //((JToken)data[i].Values()).ToArray().CopyTo(values, i);
            //    table.Rows.Add(values);
            //}
            foreach (JToken item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = item[props[i].Name]==null ? "": item[props[i].Name].ToString();
                }
                table.Rows.Add(values);
            }
            return table;
        }
        #endregion

        #region Get Method
        /// <summary>
        /// Node의 Property를 가져온다
        /// </summary>
        /// <param name="docJson">Document</param>
        /// <param name="path">주소</param>
        /// <returns></returns>
        public static JProperty GetNodeProperty(JObject docJson, string path)
        {
            JProperty element = null;

            try
            {
                path = JsonUtils.ReplacePath(path);

                JToken node = docJson.SelectToken(path);

                if (node != null)
                    element = node.Parent as JProperty;
            }
            catch
            { }

            return element;
        }

        /// <summary>
        /// 주소에 해당하는 노드를 가져온다.
        /// </summary>
        /// <param name="docJson">Document</param>
        /// <param name="path">주소</param>
        /// <returns></returns>
        public static JToken GetNode(JObject docJson, string path)
        {
            JToken node = null;

            try
            {
                path = JsonUtils.ReplacePath(path);

                node = docJson.SelectToken(path);
            }
            catch
            { }

            return node;
        }

        /// <summary>
        /// 노드의 텍스트 값을 가져온다.
        /// </summary>
        /// <param name="node">노드</param>
        /// <returns></returns>
        public static string GetNodeText(JToken node)
        {
            string result = string.Empty;

            if (node is JProperty)
            {
                result = Convert.ToString(((JProperty)node).Value);
            }
            else
            {
                if (node != null)
                    result = node.ToString();
            }

            return result;
        }

        /// <summary>
        /// 주소에 해당하는 노드의 텍스트 값을 가져온다.
        /// </summary>
        /// <param name="docJson">Document</param>
        /// <param name="path">주소</param>
        /// <returns></returns>
        public static string GetNodeText(JObject docJson, string path)
        {
            path = JsonUtils.ReplacePath(path);

            JToken node = JsonUtils.GetNode(docJson, path);

            //2017.05.30 원본 소스
            //string result = JsonUtils.GetNodeText(node);

            #region 2017.05.30 수정 소스 - LMS Patch 이후 원복할 것.
            string result = string.Empty;

            if (node is JArray)
            {
                List<JToken> childList = JsonUtils.GetChildNodeList(node);

                if (childList != null && childList.Count > 0)
                {
                    result = JsonUtils.GetNodeText(childList[0]);
                }
            }
            else
            {
                result = JsonUtils.GetNodeText(node);
            }
            #endregion

            return result;
        }

        /// <summary>
        /// 부모 노드에서 자식 노드명이 일치하는 자식 노드의 텍스트 값을 가져온다.
        /// </summary>
        /// <param name="parentNode">부모 노드</param>
        /// <param name="childNodeName">자식 노드명</param>
        /// <returns></returns>
        public static string GetChildNodeText(JToken parentNode, string childNodeName)
        {
            JToken childNode = JsonUtils.GetChildNode(parentNode, childNodeName);

            string result = JsonUtils.GetNodeText(childNode);

            return result;
        }

        /// <summary>
        /// 주소에 해당하는 모든 노드 리스트를 가져온다.
        /// </summary>
        /// <param name="docJson">Document</param>
        /// <param name="path">주소</param>
        /// <returns></returns>
        public static List<JToken> GetChildNodeList(JObject docJson, string path)
        {
            path = JsonUtils.ReplacePath(path);

            JToken parentNode = JsonUtils.GetNode(docJson, path);

            List<JToken> childNodeList = JsonUtils.GetChildNodeList(parentNode);

            return childNodeList;
        }

        /// <summary>
        /// 부모 노드의 모든 자식 노드를 가져온다.
        /// </summary>
        /// <param name="parentNode">부모 노드</param>
        /// <returns></returns>
        public static List<JToken> GetChildNodeList(JToken parentNode)
        {
            List<JToken> childNodeList = new List<JToken>();

            try
            {
                if (parentNode != null)
                {
                    if (parentNode.Children().Count() > 0)
                    {
                        childNodeList = parentNode.Children().ToList();
                    }
                }
            }
            catch
            { }

            return childNodeList;
        }

        /// <summary>
        /// 부모 노드에서 자식 노드명이 일치하는 자식 노드를 가져온다.
        /// </summary>
        /// <param name="parentNode">부모 노드</param>
        /// <param name="childNodeName">자식 노드명</param>
        /// <returns></returns>
        public static JToken GetChildNode(JToken parentNode, string childNodeName)
        {
            JToken childNode = null;

            try
            {
                if (parentNode != null)
                {
                    childNode = parentNode.SelectToken(".." + childNodeName);
                }
            }
            catch
            { }

            return childNode;
        }

        /// <summary>
        /// 자식 노드의 프로퍼티를 가져온다.
        /// </summary>
        /// <param name="parentNode">부모 노드</param>
        /// <param name="childNodeName">자식 노드명</param>
        /// <returns></returns>
        public static JProperty GetChildNodeProperty(JToken parentNode, string childNodeName)
        {
            JProperty childPropert = null;

            try
            {
                if (parentNode != null)
                {
                    JToken childNode = parentNode.SelectToken(".." + childNodeName);

                    if (childNode != null)
                        childPropert = childNode.Parent as JProperty;
                }
            }
            catch
            { }

            return childPropert;
        }

        /// <summary>
        /// 부모 노드에서 해당하는 인덱스의 자식 노드를 가져온다.
        /// </summary>
        /// <param name="parentNode">부모 노드</param>
        /// <param name="index">인덱스</param>
        /// <returns></returns>
        public static JToken GetChildNode(JToken parentNode, int index)
        {
            JToken childNode = null;

            try
            {
                if (parentNode == null)
                    return null;

                List<JToken> childNodeList = parentNode.Children().ToList();

                if (childNodeList != null
                    && childNodeList.Count > index)
                {
                    childNode = childNodeList[index];
                }
            }
            catch
            { }

            return childNode;
        }

        /// <summary>
        /// Document에서 해당하는 인덱스의 자식 노드를 가져온다.
        /// </summary>
        /// <param name="docJson">Document</param>
        /// <param name="path">주소</param>
        /// <param name="index">인덱스</param>
        /// <returns></returns>
        public static JToken GetChildNode(JObject docJson, string path, int index)
        {
            JToken childNode = null;

            try
            {
                path = JsonUtils.ReplacePath(path);

                JToken parentNode = JsonUtils.GetNode(docJson, path);

                if (parentNode == null)
                    return null;

                childNode = JsonUtils.GetChildNode(parentNode, index);
            }
            catch
            { }

            return childNode;
        }

        #endregion

        #region Add Method

        /// <summary>
        /// 부모 노드에 자식 노드를 추가한다.
        /// </summary>
        /// <param name="parentNode">부모 노드</param>
        /// <param name="childNode">자식 노드</param>
        /// <returns></returns>
        public static bool AddObjectNode(JObject parentNode, JObject childNode)
        {
            try
            {
                if (parentNode != null)
                {
                    foreach (JToken t in childNode.Children())
                    {
                        parentNode.Add(t);
                    }

                    return true;
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// JObject 타입의 노드를 추가한다.
        /// </summary>
        /// <param name="doc">Document</param>
        /// <param name="path">주소</param>
        /// <param name="newNodeName">신규 노드명</param>
        /// <returns></returns>
        public static JObject AddObjectNode(JObject doc, string path, string newNodeName)
        {
            JObject childNode = null;

            try
            {
                path = JsonUtils.ReplacePath(path);

                JToken parentToken = doc.SelectToken(path);

                JObject parentNode = parentToken as JObject;

                JProperty childElement = new JProperty(newNodeName, new JObject());

                if (parentNode != null)
                {
                    parentNode.Add(childElement);

                    childNode = parentNode[newNodeName] as JObject;
                }
            }
            catch
            { }

            return childNode;
        }

        /// <summary>
        /// 부모 노드에 자식 노드를 추가한다.
        /// </summary>
        /// <param name="doc">Document</param>
        /// <param name="parentPath">부모 노드 주소</param>
        /// <param name="childNode">신규 노드</param>
        /// <returns></returns>
        public static bool AddObjectNode(JObject doc, string parentPath, JObject childNode)
        {
            try
            {
                parentPath = JsonUtils.ReplacePath(parentPath);

                JToken parentToken = doc.SelectToken(parentPath);

                if (parentToken != null)
                {
                    JObject parentNode = parentToken as JObject;

                    return JsonUtils.AddObjectNode(parentNode, childNode);
                }
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// Text 타입의 노드를 추가한다.
        /// </summary>
        /// <param name="doc">Document</param>
        /// <param name="path">주소</param>
        /// <param name="newNodeName">신규 노드명</param>
        /// <param name="newNodeValue">신규 노드값</param>
        public static void AddTextNode(JObject doc, string path, string newNodeName, string newNodeValue)
        {
            try
            {
                path = JsonUtils.ReplacePath(path);

                JToken token = doc.SelectToken(path);

                JObject node = token as JObject;

                JProperty element = new JProperty(newNodeName, newNodeValue);

                if (node != null)
                    node.Add(element);

            }
            catch
            { }
        }

        /// <summary>
        /// JObject 타입의 자식 노드를 추가한다.
        /// </summary>
        /// <param name="parentNode">부모 노드</param>
        /// <param name="newNodeName">신규 노드명</param>
        /// <returns></returns>
        public static JObject AddChildObjectNode(JObject parentNode, string newNodeName)
        {
            JObject childNode = null;

            try
            {
                if (parentNode[newNodeName] != null)
                {
                    childNode = (JObject)parentNode[newNodeName];
                }
                else
                {
                    JProperty jpChild = new JProperty(newNodeName, new JObject());

                    parentNode.Add(jpChild);

                    childNode = parentNode[newNodeName] as JObject;
                }

            }
            catch
            { }

            return childNode;
        }

        /// <summary>
        /// Text 타입의 자식 노드를 추가한다.
        /// </summary>
        /// <param name="parentNode">부모 노드</param>
        /// <param name="newNodeName">신규 노드명</param>
        /// <param name="newNodeValue">신규 노드값</param>
        public static void AddChildTextNode(JObject parentNode, string newNodeName, string newNodeValue)
        {
            try
            {
                JProperty childElement = new JProperty(newNodeName, newNodeValue);

                parentNode.Add(childElement);
            }
            catch
            { }
        }

        /// <summary>
        /// JArray 타입의 자식 노드를 추가한다.
        /// </summary>
        /// <param name="parentNode">부모 노드</param>
        /// <param name="newNodeName">신규 노드명</param>
        /// <returns></returns>
        public static JArray AddChildArrayNode(JObject parentNode, string newNodeName)
        {
            JArray childNode = null;

            try
            {
                JProperty jpChild = new JProperty(newNodeName, new JArray());

                parentNode.Add(jpChild);

                childNode = parentNode[newNodeName] as JArray;
            }
            catch
            { }

            return childNode;
        }

        /// <summary>
        /// Array를 추가하고 자식 Object Node를 반환한다(XML에서 List를 추가하는 방식)
        /// </summary>
        /// <param name="parentNode">부모 노드</param>
        /// <param name="newNodeName">신규 노드명</param>
        /// <returns></returns>
        public static JObject AddChildArrayObjectNode(JObject parentNode, string newNodeName)
        {
            JObject childNode = null;

            try
            {
                JArray arrElement = null;

                if (parentNode[newNodeName] != null
                    && parentNode[newNodeName].Type == JTokenType.Array)
                {
                    arrElement = (JArray)parentNode[newNodeName];
                }
                else
                {
                    JProperty jpChild = new JProperty(newNodeName, new JArray());

                    parentNode.Add(jpChild);

                    arrElement = (JArray)parentNode[newNodeName];
                }

                childNode = new JObject();

                arrElement.Add(childNode);
            }
            catch
            { }

            return childNode;

        }

        /// <summary>
        /// JArray 타입의 부모 노드에 JObject 타입의 신규 Item을 추가한다.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="newNodeValue"></param>
        public static void AddArrayObject(JArray parentNode, JObject newNodeValue)
        {
            parentNode.Add(newNodeValue);
        }

        /// <summary>
        /// JArray 타입의 부모 노드에 Text 타입의 신규 Item을 추가한다.
        /// </summary>
        /// <param name="parentNode"></param>
        /// <param name="newNodeValue"></param>
        public static void AddArrayText(JArray parentNode, string newNodeValue)
        {
            parentNode.Add(newNodeValue);
        }

        /// <summary>
        /// body 노드에 신규 자식 노드를 추가한다 ( body 노드 Path ://message/body)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="newNode"></param>
        public static void AddChildNodeToBodyNode(JObject doc, JObject newNode)
        {
            JObject bodyNode = JsonUtils.GetNode(doc, "//message/body") as JObject;

            JsonUtils.AddObjectNode(bodyNode, newNode);
        }

        /// <summary>
        /// Text 노드의 텍스트를 수정한다
        /// </summary>
        /// <param name="doc">Document</param>
        /// <param name="path">노드 주소</param>
        /// <param name="nodevalue">수정할 텍스트</param>
        public static void ModifyNodeText(JObject doc, string path, string nodevalue)
        {
            try
            {
                JProperty node = JsonUtils.GetNodeProperty(doc, path);

                node.Value = nodevalue;
            }
            catch
            { }
        }

        /// <summary>
        /// replysubjectname 노드의 텍스트를 수정한다.(Path : //message/header/replysubjectname)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="nodevalue"></param>
        public static void ModifyReplySubjectName(JObject doc, string nodevalue)
        {
            try
            {
                string parentPath = "//message/header/replysubjectname";

                JsonUtils.ModifyNodeText(doc, parentPath, nodevalue);
            }
            catch
            { }
        }
        #endregion

        #region Fixed Method
        /// <summary>
        /// Get Message Name (Path : //message/header/messagename)
        /// </summary>
        /// <param name="docJson"></param>
        /// <returns></returns>
        public static string GetMessageName(JObject docJson)
        {
            string messageName = JsonUtils.GetNodeText(docJson, "//message/header/messagename");

            return messageName;
        }

        /// <summary>
        /// Get Transaction ID (Path : //message/header/transactionid)
        /// </summary>
        /// <param name="docJson"></param>
        /// <returns></returns>
        public static string GetTransactionID(JObject docJson)
        {
            string trId = JsonUtils.GetNodeText(docJson, "//message/header/transactionid");

            return trId;
        }

        /// <summary>
        /// Get Replysubject Name (Path : //message/header/replysubjectname)
        /// </summary>
        /// <param name="docJson"></param>
        /// <returns></returns>
        public static string GetReplysubjectName(JObject docJson)
        {
            string replysubjectname = JsonUtils.GetNodeText(docJson, "//message/header/replysubjectname");

            return replysubjectname;
        }

        /// <summary>
        /// Get Site ID (Path : //message/header/siteid)
        /// </summary>
        /// <param name="docJson"></param>
        /// <returns></returns>
        public static string GetSiteId(JObject docJson)
        {
            string replysubjectname = JsonUtils.GetNodeText(docJson, "//message/header/siteid");

            return replysubjectname;
        }

        /// <summary>
        /// 기본 메시지 포맷을 만든다.
        /// </summary>
        /// <param name="messageName">Message Name</param>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="replysubjectname">Reply Subject Name</param>
        /// <returns></returns>
        public static JObject MakeBaseMessage(string messageName, string transactionId, string replysubjectname)
        {
            //Document 생성(Root 노드)
            JObject docJson = new JObject();

            //JObject 타입의 노드 추가
            JObject xMessageJson = JsonUtils.AddObjectNode(docJson, ".", "message");
            //JObject 타입의 자식 노드 추가
            JObject xHeaderJson = JsonUtils.AddChildObjectNode(xMessageJson, "header");
            JObject xBodyJson = JsonUtils.AddChildObjectNode(xMessageJson, "body");

            //주소에 해당하는 부모 노드에 텍스트 타입의 자식 노드를 추가
            JsonUtils.AddTextNode(docJson, "//message/header", "messagename", messageName);
            JsonUtils.AddTextNode(docJson, "//message/header", "transactionid", transactionId);
            JsonUtils.AddTextNode(docJson, "//message/header", "replysubjectname", replysubjectname);

            return docJson;
        }

        /// <summary>
        /// 기본 메시지 포맷을 만든다.
        /// </summary>
        /// <param name="messageName">Message Name</param>
        /// <param name="transactionId">Transaction ID</param>
        /// <param name="replysubjectname">Reply Subject Name</param>
        /// <param name="siteId">Site ID</param>
        /// <returns></returns>
        public static JObject MakeBaseMessage(string messageName, string transactionId, string replysubjectname, string siteId)
        {
            //Document 생성(Root 노드)
            JObject docJson = new JObject();

            //JObject 타입의 노드 추가
            JObject xMessageJson = JsonUtils.AddObjectNode(docJson, ".", "message");
            //JObject 타입의 자식 노드 추가
            JObject xHeaderJson = JsonUtils.AddChildObjectNode(xMessageJson, "header");
            JObject xBodyJson = JsonUtils.AddChildObjectNode(xMessageJson, "body");

            //주소에 해당하는 부모 노드에 텍스트 타입의 자식 노드를 추가
            JsonUtils.AddTextNode(docJson, "//message/header", "messagename", messageName);
            JsonUtils.AddTextNode(docJson, "//message/header", "transactionid", transactionId);
            JsonUtils.AddTextNode(docJson, "//message/header", "replysubjectname", replysubjectname);
            JsonUtils.AddTextNode(docJson, "//message/header", "siteid", siteId);

            return docJson;
        }

        public static JObject MakeBaseMessage(string messageName, string transactionId, string queryId, string replysubjectname, string siteId)
        {
            //Document 생성(Root 노드)
            JObject docJson = new JObject();

            //JObject 타입의 노드 추가
            JObject xMessageJson = JsonUtils.AddObjectNode(docJson, ".", "message");
            //JObject 타입의 자식 노드 추가
            JObject xHeaderJson = JsonUtils.AddChildObjectNode(xMessageJson, "header");
            JObject xBodyJson = JsonUtils.AddChildObjectNode(xMessageJson, "body");

            //주소에 해당하는 부모 노드에 텍스트 타입의 자식 노드를 추가
            JsonUtils.AddTextNode(docJson, "//message/header", "messagename", messageName);
            JsonUtils.AddTextNode(docJson, "//message/header", "transactionid", transactionId);
            JsonUtils.AddTextNode(docJson, "//message/header", "replysubjectname", replysubjectname);
            JsonUtils.AddTextNode(docJson, "//message/header", "siteid", siteId);

            return docJson;
        }
        #endregion

        #region XmlUtils와 동일한 Method

        #region Get
        /// <summary>
        /// MES의 XmlUtils.cs와 동일한 기능을 위해 만든 Method (=GetNodeText)
        /// </summary>
        /// <param name="xmlMessage">doc</param>
        /// <param name="xPathExpression">path</param>
        /// <returns></returns>
        public static string GetElementValueByXpath(JObject xmlMessage, string xPathExpression)
        {
            string value = JsonUtils.GetNodeText(xmlMessage, xPathExpression);

            return value;
        }

        /// <summary>
        /// MES의 XmlUtils.cs와 동일한 기능을 위해 만든 Method (=GetChildNodeText)
        /// </summary>
        /// <param name="parentXmlNode">node</param>
        /// <param name="xPathExpression">path</param>
        /// <returns></returns>
        public static string GetElementValueByXpath(JToken parentXmlNode, string xPathExpression)
        {
            string value = JsonUtils.GetChildNodeText(parentXmlNode, xPathExpression);

            return value;
        }

        /// <summary>
        /// MES의 XmlUtils.cs와 동일한 기능을 위해 만든 Method (=GetNode)
        /// </summary>
        /// <param name="xmlMessage">doc</param>
        /// <param name="xPathExpression">path</param>
        /// <returns></returns>
        public static JToken GetXmlNodeByXpath(JObject xmlMessage, string xPathExpression)
        {
            JToken xmlNode = JsonUtils.GetNode(xmlMessage, xPathExpression);

            return xmlNode;
        }

        /// <summary>
        /// 노드를 검색한다. Child Node가 있을 경우 Child Node를 반환하고, Child Node가 없을 경우 자기 자신을 반환 (MES의 XmlUtils.cs와 동일한 기능을 위해 만든 Method)
        /// </summary>
        /// <param name="doc">doc</param>
        /// <param name="path">path</param>
        /// <returns></returns>
        public static List<JToken> GetNodeList(JObject doc, string path)
        {
            List<JToken> nodeList = new List<JToken>();
            try
            {
                path = JsonUtils.ReplacePath(path);

                JToken parentNode = JsonUtils.GetNode(doc, path);

                if (parentNode.Type == JTokenType.Array)
                {
                    nodeList = JsonUtils.GetNodeList(parentNode);
                }
                else
                {
                    nodeList = new List<JToken>();
                    nodeList.Add(parentNode);
                }
            }
            catch { }

            return nodeList;
        }

        /// <summary>
        /// 노드를 검색한다. Child Node가 있을 경우 Child Node를 반환하고, Child Node가 없을 경우 자기 자신을 반환 (MES의 XmlUtils.cs와 동일한 기능을 위해 만든 Method)
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        public static List<JToken> GetNodeList(JToken parentNode)
        {
            List<JToken> nodeList = new List<JToken>();

            try
            {
                if (parentNode != null)
                {
                    if (parentNode.Children().Count() > 0)
                    {
                        nodeList = parentNode.Children().ToList();
                    }
                    else
                    {
                        nodeList.Add(parentNode);
                    }
                }
            }
            catch
            { }

            return nodeList;
        }

        /// <summary>
        /// 부모 노드에서 자식 노드 리스트를 조회
        /// </summary>
        /// <param name="parentNode">부모노드</param>
        /// <param name="childNodeName">자식 노드명</param>
        /// <returns></returns>
        public static List<JToken> GetChildNodeListByName(JToken parentNode, string childNodeName)
        {
            List<JToken> nodeList = new List<JToken>();
            try
            {
                childNodeName = JsonUtils.ReplacePath(childNodeName);

                JToken childNode = JsonUtils.GetChildNode(parentNode, childNodeName);

                List<JToken> childNodeList = JsonUtils.GetChildNodeList(childNode);
            }
            catch { }

            return nodeList;
        }

        #endregion

        #region Set
        /// <summary>
        /// MES의 XmlUtils.cs와 동일한 기능을 위해 만든 Method (=ModifyNodeText)
        /// </summary>
        /// <param name="xmlMessage">doc</param>
        /// <param name="xPathExpression">path</param>
        /// <param name="value">value</param>
        public static void SetElementValueByXpath(JObject xmlMessage, string xPathExpression, string value)
        {
            JsonUtils.ModifyNodeText(xmlMessage, xPathExpression, value);
        }

        /// <summary>
        /// MES의 XmlUtils.cs와 동일한 기능을 위해 만든 Method (=ModifyNodeText)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="path"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static JToken SetNodeText(JObject doc, string path, string value)
        {
            try
            {
                JToken node = JsonUtils.GetNode(doc, path);

                if (node != null)
                {
                    JsonUtils.ModifyNodeText(doc, path, value);

                    return node;
                }
            }
            catch { }

            return null;
        }

        /// <summary>
        /// MES의 XmlUtils.cs와 동일한 기능을 위해 만든 Method.(newNodeValue가 null또는 공백일 경우 ObjectNode를 add함(=AddObjectNode/AddTextNode))
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="path"></param>
        /// <param name="newNodeName"></param>
        /// <param name="newNodeValue"></param>
        /// <returns></returns>
        public static JToken AddNode(JObject doc, string path, string newNodeName, string newNodeValue)
        {
            JToken element = null;

            try
            {
                path = JsonUtils.ReplacePath(path);

                JToken node = JsonUtils.GetNode(doc, path);

                if (node != null)
                {
                    if (string.IsNullOrEmpty(newNodeValue))
                    {
                        element = JsonUtils.AddObjectNode(doc, path, newNodeName);
                    }
                    else
                    {
                        JsonUtils.AddTextNode(doc, path, newNodeName, newNodeValue);
                    }
                }
            }
            catch (System.Exception ex)
            {
            }

            return element;
        }

        /// <summary>
        /// MES의 XmlUtils.cs와 동일한 기능을 위해 만든 Method.(부모 노드에 JProperty를 추가)
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="path"></param>
        /// <param name="node"></param>
        /// <returns></returns>
        public static bool AddNode(JObject doc, string path, JProperty node)
        {
            try
            {
                path = JsonUtils.ReplacePath(path);

                JObject parentNode = JsonUtils.GetNode(doc, path) as JObject;

                if (parentNode != null)
                    parentNode.Add(node);
            }
            catch
            { }

            return false;
        }

        /// <summary>
        /// MES의 XmlUtils.cs와 동일한 기능을 위해 만든 Method. (부모 노드에 자식 노드를 추가함(newNodeValue가 null 또는 공백일 경우 JObject를 추가함(=AddChildObjectNode/AddChildTextNode))
        /// </summary>
        /// <param name="node"></param>
        /// <param name="newNodeName"></param>
        /// <param name="newNodeValue"></param>
        /// <returns></returns>
        public static JToken AddChildNode(JObject node, string newNodeName, string newNodeValue)
        {
            JObject childNode = null;
            try
            {
                if (string.IsNullOrEmpty(newNodeValue))
                {
                    childNode = JsonUtils.AddChildObjectNode(node, newNodeName);
                }
                else
                {
                    JsonUtils.AddChildTextNode(node, newNodeName, newNodeValue);
                }
            }
            catch { }

            return childNode;
        }
        #endregion

        #endregion

        #region CustomMethod (프로젝트별 Custom Method)
        public static MessageType GetMessageType(MessageField msgField)
        {
            MessageType msgType = MessageType.XML;

            if (msgField != null
                && msgField.Value != null)
            {
                if (MessageType.JSON.ToString() == msgField.Value.ToString())
                {
                    msgType = MessageType.JSON;
                }
            }

            return msgType;
        }

        public static string GetMessageByMessageType(Message receiveMessage, MessageType sendMsgType)
        {
            string xmlMessage = null;

            if (receiveMessage != null)
            {
                xmlMessage = receiveMessage.GetField(MessageFieldName.XmlData).Value.ToString();

                MessageType receiveMsgType = JsonUtils.GetMessageType(receiveMessage.GetField(MessageFieldName.MessageType));

                if (receiveMsgType != sendMsgType)
                {
                    if (receiveMsgType == MessageType.JSON)
                    {
                        xmlMessage = JsonUtils.ConvertJsonToXml(xmlMessage).InnerXml;
                    }
                    else
                    {
                        xmlMessage = JsonUtils.ConvertXmlToJson(xmlMessage);
                    }
                }
            }

            return xmlMessage;
        }

        public static string ConvertToXmlByMessageType(MessageType msgType, string msg)
        {
            string returnMsg = msg;

            if (msgType == MessageType.JSON)
            {
                returnMsg = JsonUtils.ConvertJsonToXml(msg).InnerXml;
            }

            return returnMsg;
        }

        public static JObject makeReplyDocument(JObject doc)
        {
            //<return>
            //    <returncode>0</returncode>
            //    <returnmessage />
            //    <사용자 Node>
            //       ....
            //    </사용자 Node>
            //</return>
            JObject replyXml = JObject.Parse(doc.ToString());

            JObject returnNode = JsonUtils.AddObjectNode(replyXml, "//message", "return");// replyXml.CreateElement("return");
            JsonUtils.AddChildTextNode(returnNode, "returncode", ""); // replyXml.CreateElement("returncode");
            JsonUtils.AddChildTextNode(returnNode, "returnmessage", ""); //replyXml.CreateElement("returnmessage");

            return replyXml;
        }

        public static JObject makeReplyDocumentForQueryService(JObject doc)
        {
            //<return>
            //    <returncode>0</returncode>
            //    <returnmessage />
            //    <사용자 Node>
            //       ....
            //    </사용자 Node>
            //</return>
            JObject replyXml = JObject.Parse(doc.ToString());

            JObject returnNode = JsonUtils.AddObjectNode(replyXml, "//message", "return");
            JsonUtils.AddChildTextNode(returnNode, "returncode", "");
            JsonUtils.AddChildTextNode(returnNode, "returnmessage", "");
            JsonUtils.AddChildArrayNode(returnNode, "returnTable");
            JsonUtils.AddChildArrayNode(returnNode, "returnColumns");
            JsonUtils.AddChildTextNode(returnNode, "returnString", "");

            return replyXml;
        }

        #region 2017.07.18 CYH : 사용하지 앟음 - 주석처리
        //public static JObject makeTerminalMessage(DbContext dbContext, JObject doc)
        //{
        //    string prevmessagename = JsonUtils.GetNodeText(doc, "//message/header/messagename");
        //    string transactionid = JsonUtils.GetNodeText(doc, "//message/header/transactionid");
        //    string replysubjectname = JsonUtils.GetNodeText(doc, "//message/header/replysubjectname");
        //    string mainMachineName = JsonUtils.GetNodeText(doc, "//message/body/EQPID");
        //    string machineName = JsonUtils.GetNodeText(doc, "//message/body/UNITID");
        //    string transtime = DateTime.Now.ToString("yyyyMMddHHmmddssff");
        //    string eventUser = JsonUtils.GetNodeText(doc, "//message/body/USER");

        //    string messageTitle = CommonConstantDef.TerminalMessage;
        //    string query = "SELECT MESSAGEFRAME FROM CT_MESSAGESETMASTER WITH (NOLOCK) WHERE MESSAGENAME ='{0}'";
        //    query = string.Format(query, messageTitle);
        //    DataTable dataTable = dbContext.ExecuteDataTable(query);
        //    string messageFrame = string.Empty;
        //    if (dataTable != null && dataTable.Rows.Count > 0)
        //        messageFrame = dataTable.Rows[0]["MESSAGEFRAME"].ToString();

        //    JObject xmlDocument = new JObject();
        //    xmlDocument = JObject.Parse(messageFrame);

        //    JsonUtils.SetNodeText(xmlDocument, "//message/header/messagename", messageTitle);
        //    JsonUtils.SetNodeText(xmlDocument, "//message/header/transactionid", transactionid);
        //    JsonUtils.SetNodeText(xmlDocument, "//message/header/replysubjectname", replysubjectname);
        //    JsonUtils.SetNodeText(xmlDocument, "//message/body/EQPID", mainMachineName);
        //    JsonUtils.SetNodeText(xmlDocument, "//message/body/UNITID", machineName);
        //    JsonUtils.SetNodeText(xmlDocument, "//message/body/TRANSTIME", transtime);
        //    JsonUtils.SetNodeText(xmlDocument, "//message/body/USER", eventUser);

        //    JObject headerElement = JsonUtils.GetNode(xmlDocument, "//message/header") as JObject;

        //    JsonUtils.AddChildTextNode(headerElement, "prevmessagename", prevmessagename);

        //    return xmlDocument;
        //} 
        #endregion

        public static object GetNode(object docJson, string path)
        {
            object node = null;

            try
            {
                if (docJson is JObject)
                {
                    JObject jObj = (JObject)docJson;

                    path = JsonUtils.ReplacePath(path);

                    node = jObj.SelectToken(path);
                }
                else
                {
                    XmlDocument xmlDoc = (XmlDocument)docJson;

                    node = xmlDoc.SelectSingleNode(path);
                }
            }
            catch
            { }

            return node;
        }

        public static string GetElementValueByXpath(object xmlMessage, string xPathExpression)
        {
            string value = string.Empty;

            if (xmlMessage is JObject)
            {
                value = JsonUtils.GetElementValueByXpath((JObject)xmlMessage, xPathExpression);
            }
            else
            {
                XmlElement xmlElement = (XmlElement)((XmlDocument)xmlMessage).SelectSingleNode(xPathExpression);
                if (xmlElement != null && xmlElement.FirstChild != null)
                {
                    value = xmlElement.FirstChild.Value;
                }
            }

            return value;
        }

        public static string GetNodeText(object doc, string path)
        {
            try
            {
                if (doc is JObject)
                {
                    return JsonUtils.GetNodeText((JObject)doc, path);
                }
                else
                {
                    XmlNodeList node = ((XmlDocument)doc).SelectNodes(path);
                    if (node != null && node.Count > 0)
                        return node[0].InnerText;
                }
            }
            catch { }

            return "";
        }

        public static object SetNodeText(object doc, string path, string value)
        {
            try
            {
                if (doc is JObject)
                {
                    JToken jNode = JsonUtils.SetNodeText((JObject)doc, path, value);

                    return jNode;
                }
                else
                {
                    XmlNode node = ((XmlDocument)doc).SelectSingleNode(path);
                    if (node != null)
                    {
                        node.InnerText = value;

                        return node;
                    }
                }
            }
            catch { }

            return null;
        }

        public static object AddNode(object doc, string path, string newNodeName, string newNodeValue)
        {
            try
            {
                if (doc is JObject)
                {
                    JToken jNode = JsonUtils.AddNode((JObject)doc, path, newNodeName, newNodeValue);

                    return jNode;
                }
                else
                {
                    XmlNode node = ((XmlDocument)doc).SelectSingleNode(path);
                    if (node != null)
                    {
                        XmlElement element = ((XmlDocument)doc).CreateElement(newNodeName);
                        element.InnerText = newNodeValue;

                        node.AppendChild(element);
                        return element;
                    }
                }
            }
            catch (System.Exception ex)
            {
            }

            return null;
        }

        /// <summary>
        /// JObject List 중에서 searchNodeName의 값이 searchNodeValue인 JObject를 조회
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="searchNodeName"></param>
        /// <param name="searchNodeValue"></param>
        /// <returns></returns>
        public static JToken GetNode(List<JToken> dataList, string searchNodeName, string searchNodeValue)
        {
            List<JToken> jtList = dataList.Where(row => Convert.ToString(row.SelectToken(searchNodeName)) == searchNodeValue).ToList();

            if (jtList != null
                && jtList.Count > 0)
            {
                return jtList[0];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// JObject List 중에서 searchNodeName의 값이 searchNodeValue인 JObject의 TargetNodeName 값을 조회
        /// </summary>
        /// <param name="dataList"></param>
        /// <param name="searchNodeName"></param>
        /// <param name="searchNodeValue"></param>
        /// <param name="targetNodeName"></param>
        /// <returns></returns>
        public static string GetNodeText(List<JToken> dataList, string searchNodeName, string searchNodeValue, string targetNodeName)
        {
            string targetNodeValue = string.Empty;

            JToken jtTarget = JsonUtils.GetNode(dataList, searchNodeName, searchNodeValue);

            if (jtTarget != null)
            {
                targetNodeValue = JsonUtils.GetChildNodeText(jtTarget, targetNodeName);
            }

            return targetNodeValue.Trim();
        }
        #endregion
    }
}
