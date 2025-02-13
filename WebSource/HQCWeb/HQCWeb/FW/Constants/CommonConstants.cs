using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQCWeb.FW.Constants
{
    public struct ReplyCode
    {
        public const string NOREPLY = "NOREPLY";
        public const string REPLY = "REPLY";
        public const string POST = "POST";
        public const string FAIL = "FAIL";

        //2018.09.11 CYH : CarrierValidationRequest 불량 Cassette Reject Reply 시 사용
        public const string DEFECTCST = "DEFECTCST";
    }

    public struct ReturnCode
    {
        public const string SUCCESS = "0";
        public const string ERROR = "-1";

        //2018.09.11 CYH : CarrierValidationRequest 불량 Cassette Reject Reply 시 사용
        public const string CODE99 = "99";
    }

    public struct Languages
    {
        public const string KOREA = "KO-KR";
        public const string ENGLISH = "EN-US";
        public const string HERE = "LO-LN";
    }
}
