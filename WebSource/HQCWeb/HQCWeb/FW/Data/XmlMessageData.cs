using HQCWeb.FMB_FW.Constants;
using HQCWeb.FMB_FW.Utils;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MES.FW.Common;

namespace HQCWeb.FMB_FW.Data
{
    [Serializable]
    public class XmlMessageData
    {
        // Original xml message
        public object InnerDoc
        {
            get;
            private set;
        }

        public string MessageName { get; set; }
        public string TransactionId { get; set; }

        private MessageType _messageType = MessageType.JSON;

        public MessageType MessageType
        {
            get { return _messageType; }
            set { _messageType = value; }
        }

        public XmlMessageData(MessageType msgType)
        {
            _messageType = msgType;

            if (this.MessageType == Constants.MessageType.XML)
            {
                this.InnerDoc = new XmlDocument();
            }
            else
            {
                this.InnerDoc = new JObject();
            }
        }

        public XmlMessageData(MessageType msgType, string xml)
            : this(msgType)
        {
            if (string.IsNullOrEmpty(xml))
                throw new ArgumentException("xml is null or empty");

            if (this.MessageType == Constants.MessageType.XML)
            {
                XmlDocument docXml = new XmlDocument();
                docXml.LoadXml(xml);

                this.MessageName = XmlUtils.GetElementValueByXpath(docXml, XmlNames.XPATH_MESSAGE_NAME);
                this.TransactionId = XmlUtils.GetElementValueByXpath(docXml, XmlNames.XPATH_TRANSACTIONID);

                this.InnerDoc = docXml;
            }
            else
            {
                JObject docJson = JObject.Parse(xml);

                this.MessageName = JsonUtils.GetElementValueByXpath(docJson, XmlNames.XPATH_MESSAGE_NAME);
                this.TransactionId = JsonUtils.GetElementValueByXpath(docJson, XmlNames.XPATH_TRANSACTIONID);

                this.InnerDoc = docJson;
            }
        }
    }
}