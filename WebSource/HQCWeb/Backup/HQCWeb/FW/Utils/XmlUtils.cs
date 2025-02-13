using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Xml;

namespace HQCWeb.FMB_FW.Utils
{
    public class XmlUtils
    {

        public static string GetMessageName(string xml)
        {
            string messageName = string.Empty;
            XmlDocument xmlMessage = new XmlDocument();
            xmlMessage.LoadXml(xml);
            XmlNodeList nodeList = xmlMessage.GetElementsByTagName("messagename");

            return messageName = nodeList[0].ChildNodes[0].Value;
        }

        public static string GetElementValueByTagName(XmlDocument xmlMessage, string elementName)
        {
            string value = string.Empty;

            XmlNodeList nodeList = xmlMessage.GetElementsByTagName(elementName);
            if (nodeList != null)
            {
                value = nodeList[0].ChildNodes[0].Value;
            }

            return value;
        }

        public static string GetElementValueByXpath(XmlDocument xmlMessage, string xPathExpression)
        {
            string value = string.Empty;

            XmlElement xmlElement = (XmlElement)xmlMessage.SelectSingleNode(xPathExpression);
            if (xmlElement != null && xmlElement.FirstChild != null)
            {
                value = xmlElement.FirstChild.Value;
            }

            return value;
        }

        public static XmlNodeList GetXmlNodeListByXpath(XmlDocument xmlMessage, string xPathExpression)
        {
            XmlNodeList xnList = xmlMessage.SelectNodes(xPathExpression);

            return xnList;
        }

        public static XmlNode GetXmlNodeByXpath(XmlDocument xmlMessage, string xPathExpression)
        {
            XmlNode xmlNode = xmlMessage.SelectSingleNode(xPathExpression);

            return xmlNode;
        }

        public static XmlNodeList GetXmlNodeListByXpath(XmlNode parentXmlNode, string xPathExpression)
        {
            XmlNodeList xnList = parentXmlNode.SelectNodes(xPathExpression);

            return xnList;
        }

        public static XmlNodeList GetXmlVariableNodeListByXpath(XmlNode xmlNode, string xPathExpression)
        {
            XmlNode clone = xmlNode.Clone();

            XmlNodeList xnList = clone.SelectNodes(xPathExpression);

            return xnList;
        }

        public static string GetElementValueByXpath(XmlNode parentXmlNode, string xPathExpression)
        {
            string value = string.Empty;

            //Clone 을 하지 않게 되면 해당 Node 가 속한 Document 에서 찾는듯.. 
            //두번째 Node 의 값이 첫번째 Node 의 값으로 가져 온다.
            XmlNode clone = parentXmlNode.Clone();
            XmlElement xmlElement = (XmlElement)clone.SelectSingleNode(xPathExpression);

            if (xmlElement != null && xmlElement.FirstChild != null)
            {
                value = xmlElement.FirstChild.Value;
            }

            return value;
        }

        public static string GetElementValueByXpath(XmlElement parentElement, string xPathExpression)
        {
            string value = string.Empty;

            //Clone 을 하지 않게 되면 해당 Node 가 속한 Document 에서 찾는듯.. 
            //두번째 Node 의 값이 첫번째 Node 의 값으로 가져 온다.
            XmlElement clone = (XmlElement)parentElement.Clone();

            XmlElement xmlElement = (XmlElement)clone.SelectSingleNode(xPathExpression);
            if (xmlElement != null && xmlElement.FirstChild != null)
            {
                value = xmlElement.FirstChild.Value;
            }

            return value;
        }

        public static string GetElementValue(XmlNode xmlNode)
        {
            string value = string.Empty;

            if (xmlNode.FirstChild != null)
            {
                XmlNode child = xmlNode.FirstChild;

                value = child.Value;
            }

            return value;
        }

        public static string SelectStringValueByXPath(XPathNavigator xnavi, string xpath)
        {
            XPathNavigator found = xnavi.SelectSingleNode(xpath);

            if (found != null)
                return found.Value;

            return null;
        }

        public static XPathNavigator SelectNodeByXPath(XPathNavigator xnavi, string xpath)
        {
            XPathNavigator found = xnavi.SelectSingleNode(xpath);
            return found;
        }

        public static XPathNodeIterator SelectNodeListByXPath(XPathNavigator xnavi, string xpath)
        {
            var iter = xnavi.Select(xpath);
            return iter;
        }

        public static void SetElementValueByXpath(XmlDocument xmlMessage, string xPathExpression, string value)
        {
            XmlElement xmlElement = (XmlElement)xmlMessage.SelectSingleNode(xPathExpression);

            if (xmlElement != null)
            {
                if (xmlElement.FirstChild != null)
                {
                    xmlElement.FirstChild.RemoveAll();
                }
                xmlElement.InnerText = value;
            }
        }

        public static void SetXmlNodeValueByXpath(XmlNode xmlNode, string xPathExpression, string value)
        {
            if (xmlNode != null)
            {
                if (xmlNode.FirstChild != null)
                {
                    xmlNode.FirstChild.RemoveAll();
                }
                xmlNode.InnerText = value;
            }
        }

        public static void SetAttributeValue(XmlNode xmlNode, string attributeName, string attributeValue)
        {
            XmlAttributeCollection attributeCollection = xmlNode.Attributes;
            foreach (XmlAttribute attribute in attributeCollection)
            {
                if (attribute.Name.Equals(attributeName))
                {
                    attribute.Value = attributeValue;
                    break;
                }
            }
        }

        public static XmlNode GetChildNode(XmlNode node, string nodeName)
        {
            try
            {
                return node.SelectSingleNode("./" + nodeName);
            }
            catch { }

            return null;
        }

        public static string GetChildNodeText(XmlNode node, string chileNodeName)
        {
            try
            {
                XmlNode childNode = node.SelectSingleNode("./" + chileNodeName);
                if (childNode != null)
                    return childNode.InnerText;
            }
            catch { }

            return "";
        }

        public static string GetNodeText(XmlDocument doc, string path)
        {
            try
            {
                XmlNodeList node = doc.SelectNodes(path);
                if (node != null && node.Count > 0)
                    return node[0].InnerText;
            }
            catch { }

            return "";
        }


        public static XmlNode SetChildNodeText(XmlNode element, string path, string value)
        {
            try
            {
                XmlNode node = element.SelectSingleNode(path);
                if (node != null)
                {
                    node.InnerText = value;
                    return node;
                }
            }
            catch { }

            return null;
        }

        public static XmlNode SetChildNodeTextByCData(XmlNode node, string path, string value)
        {
            try
            {
                XmlNode childNode = node.SelectSingleNode(path);
                if (childNode != null)
                {
                    childNode.InnerText = "";
                    XmlCDataSection cdata = node.OwnerDocument.CreateCDataSection(value);
                    childNode.AppendChild(cdata);

                    return childNode;
                }
            }
            catch { }

            return null;
        }

        public static XmlNode SetNodeText(XmlDocument doc, string path, string value)
        {
            try
            {
                XmlNode node = doc.SelectSingleNode(path);
                if (node != null)
                {
                    node.InnerText = value;

                    return node;
                }
            }
            catch { }

            return null;
        }

        public static XmlNode SetNodeTextByCDATA(XmlDocument doc, string path, string value)
        {
            try
            {
                XmlNode node = doc.SelectSingleNode(path);
                if (node != null)
                {
                    XmlCDataSection cdata = doc.CreateCDataSection(value);
                    node.AppendChild(cdata);
                    return node;
                }
            }
            catch { }

            return null;
        }

        public static void SetAllNodeText(XmlDocument doc, string nodeName, string value)
        {
            try
            {
                XmlNodeList nodeList = doc.SelectNodes(nodeName);
                foreach (XmlNode node in nodeList)
                    node.InnerText = value;
            }
            catch { }
        }

        public static void SetAllChildNodeText(XmlNode node, string nodeName, string value)
        {
            try
            {
                XmlNodeList nodeList = node.SelectNodes(nodeName);
                foreach (XmlNode childNode in nodeList)
                    childNode.InnerText = value;
            }
            catch { }
        }

        public static XmlNodeList GetNodeList(XmlDocument doc, string path)
        {
            try
            {
                return doc.SelectNodes(path);
            }
            catch { }

            return null;
        }

        public static XmlNodeList GetChildNodeListByName(XmlNode node, string nodeName)
        {
            try
            {
                return node.SelectNodes(nodeName);
            }
            catch { }

            return null;
        }

        public static string GetNodeAttributeValue(XmlDocument doc, string path, string attribute)
        {
            try
            {
                XmlNode node = doc.SelectSingleNode(path);
                if (node != null)
                {
                    XmlAttribute att = node.Attributes[attribute];
                    if (att != null)
                        return att.Value;
                }
            }
            catch { }

            return "";
        }

        public static string GetChildNodeAttributeValue(XmlNode node, string path, string attribute)
        {
            try
            {
                XmlNode childNode = node.SelectSingleNode(path);
                if (childNode != null)
                {
                    XmlAttribute att = childNode.Attributes[attribute];
                    if (att != null)
                        return att.Value;
                }
            }
            catch { }

            return "";
        }

        public static bool SetNodeAttribute(XmlDocument doc, string path, string attribute, string value)
        {
            try
            {
                XmlNode node = doc.SelectSingleNode(path);
                if (node != null)
                {
                    XmlAttribute att = node.Attributes[attribute];
                    if (att != null)
                    {
                        att.Value = value;
                        return true;
                    }
                }
            }
            catch { }

            return false;
        }

        public static bool SetChildNodeAttributeValue(XmlNode node, string path, string attribute, string value)
        {
            try
            {
                XmlNode childNode = node.SelectSingleNode(path);
                if (childNode != null)
                {
                    XmlAttribute att = childNode.Attributes[attribute];
                    if (att != null)
                    {
                        att.Value = value;
                        return true;
                    }
                }
            }
            catch { }

            return false;
        }

        public static XmlNode AddNode(XmlDocument doc, string path, string newNodeName, string newNodeValue)
        {
            try
            {
                XmlNode node = doc.SelectSingleNode(path);
                if (node != null)
                {
                    XmlElement element = doc.CreateElement(newNodeName);
                    element.InnerText = newNodeValue;

                    node.AppendChild(element);
                    return element;
                }
            }
            catch (System.Exception ex)
            {
            }

            return null;
        }

        public static bool AddNode(XmlDocument doc, string path, XmlNode node)
        {
            try
            {
                XmlNode element = doc.SelectSingleNode(path);
                if (element != null)
                {
                    element.AppendChild(doc.ImportNode(node, true));
                    return true;
                }
            }
            catch (System.Exception ex) { Console.WriteLine(ex.Message); }
            return false;
        }

        public static XmlNode AddChildNode(XmlNode node, string newNodeName, string newNodeValue)
        {
            try
            {
                XmlElement element = node.OwnerDocument.CreateElement(newNodeName);
                if (element != null)
                {
                    element.InnerText = newNodeValue;
                    node.AppendChild(element);

                    return element;
                }
            }
            catch { }

            return null;
        }

        public static XmlNode AddAttribue(XmlNode node, string newAttributName, string newAttribueValue)
        {
            try
            {
                XmlAttribute attr = node.OwnerDocument.CreateAttribute(newAttributName);
                if (attr != null)
                {
                    attr.Value = newAttribueValue;
                    node.Attributes.Append(attr);

                    return node;
                }
            }
            catch { }

            return null;
        }

        public static XmlDocument makeReplyDocument(XmlDocument doc, string returnCode, string returnMessage)
        {
            //<return>
            //    <returncode>0</returncode>
            //    <returnmessage />
            //    <사용자 Node>
            //       ....
            //    </사용자 Node>
            //</return>
            XmlDocument replyXml = (XmlDocument)doc.Clone();

            XmlNode returnNode = replyXml.CreateElement("return");
            XmlNode returnCodeNode = replyXml.CreateElement("returncode");
            XmlNode returnMessageNode = replyXml.CreateElement("returnmessage");

            returnCodeNode.Value = returnCode;
            returnMessageNode.Value = returnMessage;

            returnNode.AppendChild(returnCodeNode);
            returnNode.AppendChild(returnMessageNode);

            XmlNode messageNode = replyXml.SelectSingleNode("//message");
            messageNode.AppendChild(returnNode);

            return replyXml;
        }

        public static XmlDocument makeReplyDocument(XmlDocument doc)
        {
            //<return>
            //    <returncode>0</returncode>
            //    <returnmessage />
            //    <사용자 Node>
            //       ....
            //    </사용자 Node>
            //</return>
            XmlDocument replyXml = (XmlDocument)doc.Clone();

            XmlNode returnNode = replyXml.CreateElement("return");
            XmlNode returnCodeNode = replyXml.CreateElement("returncode");
            XmlNode returnMessageNode = replyXml.CreateElement("returnmessage");

            returnNode.AppendChild(returnCodeNode);
            returnNode.AppendChild(returnMessageNode);

            XmlNode messageNode = replyXml.SelectSingleNode("//message");
            messageNode.AppendChild(returnNode);

            return replyXml;
        }

        #region 2017.07.18 CYH : 사용하지 않음 - 주석처리
        //public static XmlDocument makeTerminalMessage(DbContext dbContext, XmlDocument doc)
        //{
        //    string prevmessagename = XmlUtils.GetNodeText(doc, "//message/header/messagename");
        //    string transactionid = XmlUtils.GetNodeText(doc, "//message/header/transactionid");
        //    string replysubjectname = XmlUtils.GetNodeText(doc, "//message/header/replysubjectname");
        //    string mainMachineName = XmlUtils.GetNodeText(doc, "//message/body/EQPID");
        //    string machineName = XmlUtils.GetNodeText(doc, "//message/body/UNITID");
        //    string transtime = DateTime.Now.ToString("yyyyMMddHHmmddssff");
        //    string eventUser = XmlUtils.GetNodeText(doc, "//message/body/USER");

        //    string messageTitle = CommonConstantDef.TerminalMessage;
        //    string query = "SELECT MESSAGEFRAME FROM CT_MESSAGESETMASTER WITH (NOLOCK) WHERE MESSAGENAME ='{0}'";
        //    query = string.Format(query, messageTitle);
        //    DataTable dataTable = dbContext.ExecuteDataTable(query);
        //    string messageFrame = string.Empty;
        //    if (dataTable != null && dataTable.Rows.Count > 0)
        //        messageFrame = dataTable.Rows[0]["MESSAGEFRAME"].ToString();

        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.LoadXml(messageFrame);

        //    XmlUtils.SetNodeText(xmlDocument, "//message/header/messagename", messageTitle);
        //    XmlUtils.SetNodeText(xmlDocument, "//message/header/transactionid", transactionid);
        //    XmlUtils.SetNodeText(xmlDocument, "//message/header/replysubjectname", replysubjectname);
        //    XmlUtils.SetNodeText(xmlDocument, "//message/body/EQPID", mainMachineName);
        //    XmlUtils.SetNodeText(xmlDocument, "//message/body/UNITID", machineName);
        //    XmlUtils.SetNodeText(xmlDocument, "//message/body/TRANSTIME", transtime);
        //    XmlUtils.SetNodeText(xmlDocument, "//message/body/USER", eventUser);

        //    XmlNode headerElement = xmlDocument.SelectSingleNode("//message/header");
        //    XmlElement prevMessageElement = xmlDocument.CreateElement("prevmessagename");
        //    prevMessageElement.InnerText = prevmessagename;
        //    headerElement.AppendChild(prevMessageElement);

        //    return xmlDocument;
        //} 
        #endregion

        public static Hashtable GetNamedValue(XmlDocument doc, string path)
        {
            Hashtable udfs = null;

            try
            {
                XmlNode E = doc.SelectSingleNode(path);

                if (E != null)
                {
                    XmlNodeList list = E.ChildNodes;

                    if (E.ChildNodes.Count > 0)
                    {
                        udfs = new Hashtable();

                        foreach (XmlNode node in E.ChildNodes)
                        {
                            udfs.Add(node.Name, node.InnerText);
                        }
                    }
                }
                else
                    udfs = new Hashtable();

                return udfs;
            }
            catch { }

            return null;
        }


    }
}