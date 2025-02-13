using System;

namespace HQCWeb.FW
{
    /// <summary>MesRule Not Found Exception</summary>
    [Serializable]
    public class MesRuleNotFoundException : MesException
    {
        private static readonly string _errorCode = "MES-0115";

        /// <summary>
        /// MesRuleNotFoundException string : "MesRule not found. RULENAME='RULE-001', TID='TID-001'"
        /// </summary>
        /// <example>MesRuleNotFoundException("RULE-001", "TID-001")</example>
        /// <param name="name">entity name</param>
        public MesRuleNotFoundException(string ruleName, string tid)
          : base(MesRuleNotFoundException._errorCode + "_1", string.Format("MesRule not found. RULENAME='{0}', TID='{1}'", (object)ruleName, (object)tid))
        {
            this.ParamList.Add(ruleName);
            this.ParamList.Add(tid);
        }
    }
}
