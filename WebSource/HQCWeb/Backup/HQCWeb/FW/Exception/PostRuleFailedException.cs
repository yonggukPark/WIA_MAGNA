using System;

namespace HQCWeb.FW
{ 
    /// <summary>PostRule Failed Exception</summary>
    [Serializable]
    public class PostRuleFailedException : MesException
    {
        private static readonly string _errorCode = "MES-0118";

        public PostRuleFailedException(string ruleName, string tid, System.Exception innerException)
          : base(PostRuleFailedException._errorCode + "_1", string.Format("PostRule failed. RULENAME='{0}', TID='{1}'", (object)ruleName, (object)tid), innerException)
        {
            this.ParamList.Add(ruleName);
            this.ParamList.Add(tid);
            this.ParamList.Add(innerException.Message);
        }
    }
}
