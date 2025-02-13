using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQCWeb.FW.Constants
{
    public struct ProcessStatusType
    {
        public const string Stopped = "0";
        public const string Started = "1";
        public const string Normal = "2";
        public const string Error = "3";
        public const string Warning = "4";
    }
}
