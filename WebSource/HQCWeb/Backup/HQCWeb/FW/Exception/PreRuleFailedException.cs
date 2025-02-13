using System;

namespace HQCWeb.FW
{
    /// <summary>PreRule Failed Exception</summary>
    [Serializable]
    public class PreRuleFailedException : MesException
    {
        private static readonly string _errorCode = "MES-0117";

        /// <summary>
        /// PreRuleFailedException string : "PreRule failed. RULENAME='RULE-001', TID='TID-001'. Check InnerException for detail"
        /// </summary>
        /// <example>
        /// PreRuleFailedException("RULE-001", "TID-001", (new Exception()))
        /// </example>
        /// <param name="ruleName"></param>
        /// <param name="tid"></param>
        /// <param name="innerException"></param>
        public PreRuleFailedException(string ruleName, string tid, System.Exception innerException)
          : base(PreRuleFailedException._errorCode + "_1", string.Format("PreRule failed. RULENAME='{0}', TID='{1}'", (object)ruleName, (object)tid), innerException)
        {
            this.ParamList.Add(ruleName);
            this.ParamList.Add(tid);
            this.ParamList.Add(innerException.Message);
        }
    }
}
