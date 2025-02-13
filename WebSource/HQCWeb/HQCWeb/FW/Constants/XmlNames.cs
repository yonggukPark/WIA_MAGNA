using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQCWeb.FW.Constants
{
    public struct XmlNames
    {
        public const string ELEMENT_MESSAGE = "message";
        public const string ELEMENT_HEADER = "header";
        public const string ELEMENT_MESSAGE_NAME = "messagename";
        public const string ELEMENT_TRANSACTIONID = "transactionid";
        public const string ELEMENT_BODY = "body";

        public const string XPATH_MESSAGE_NAME = "//messagename";
        public const string XPATH_BODY = "//body";
        public const string XPATH_TRANSACTIONID = "//transactionid";
    }
}
