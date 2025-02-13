using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQCWeb.FW.Constants
{
    public struct QueryType
    {
        public const string SELECT = "0";
        public const string INSERT = "1";
        public const string UPDATE = "2";
        public const string DELETE = "3";
        public const string MERGE = "4";
        public const string DELETE_KEEP_DATA = "5";
    }
}
