using System;
using System.Collections.Generic;

namespace HQCWeb.FW
{
    /// <summary>Entity already exists exception</summary>
    [Serializable]
    public class MesException : ApplicationException
    {
        private List<string> paramList = new List<string>();
        protected string errorCode;

        public List<string> ParamList => this.paramList;

        /// <summary>Default constructor</summary>
        public MesException()
        {
        }

        /// <summary>Constructor with errorCode, message</summary>
        /// <param name="errorCode">ErrorCode defined in ErrorCodeDefinition</param>
        /// <param name="message">Message</param>
        public MesException(string errorCode, string message)
          : base(string.Format("[{0}] {1}", (object)errorCode, (object)message))
        {
            this.errorCode = errorCode;
        }

        /// <summary>Constructor with errorCode, message, innerException</summary>
        /// <param name="errorCode"></param>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public MesException(string errorCode, string message, System.Exception innerException)
          : base(string.Format("[{0}] {1}", (object)errorCode, (object)message), innerException)
        {
        }

        /// <summary>
        /// Error Code for this exception (defined in ErrorCodeDefinition)
        /// </summary>
        public virtual string ErrorCode => this.errorCode;
    }
}
