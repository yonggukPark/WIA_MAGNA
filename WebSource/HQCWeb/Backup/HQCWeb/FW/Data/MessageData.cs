using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;

namespace HQCWeb.FW.Data
{
    [KnownType(typeof(List<object>))]
    [KnownType(typeof(Dictionary<string, object>))]
    [KnownType(typeof(Dictionary<string, Dictionary<string, object>>))]
    [KnownType(typeof(DBNull))]
    [KnownType(typeof(List<Dictionary<string, Dictionary<string, object>>>))]
    [DataContract]
    [Serializable]
    public class MessageData
    {
        [DataMember]
        public string TID { get; set; }

        [DataMember]
        public string COMMAND { get; set; }

        [DataMember]
        public string COMMANDTYPE { get; set; }

        [DataMember]
        public bool ISREQUEST { get; set; }

        [DataMember]
        public string SITEID { get; set; }

        [DataMember]
        public string USERID { get; set; }

        [DataMember]
        public string IPADDRESS { get; set; }

        [DataMember]
        public string LANGUAGE { get; set; }

        [DataMember]
        public object OBJECT { get; set; }

        [DataMember]
        public string CODE { get; set; }

        [DataMember]
        public string MESSAGE { get; set; }

        [DataMember]
        public Hashtable HASHTABLE { get; set; }

        [DataMember]
        public List<Hashtable> HASHLIST { get; set; }

        [DataMember]
        public List<Dictionary<string, object>> DATALIST { get; set; }

        [DataMember]
        public Dictionary<string, Dictionary<string, object>> DATADIC { get; set; }

        [DataMember]
        public DataTable DATATABLE { get; set; }

        [DataMember]
        public DataSet DATASET { get; set; }

        [DataMember]
        public bool ISSUCCESS { get; set; }

        [DataMember]
        public string EXCEPTIONMESSAGE { get; set; }

        [DataMember]
        public string QUERYID { get; set; }

        public MessageData()
        {
            this.HASHTABLE = new Hashtable();
            this.HASHLIST = new List<Hashtable>();
            this.DATALIST = new List<Dictionary<string, object>>();
            this.DATADIC = new Dictionary<string, Dictionary<string, object>>();
        }

        public override string ToString() => string.Format("TID={0} CMD={1} TYPE={2} REQ={3} SITE={4} USER={5} IP={6} SUCCESS={7}", (object)this.TID, (object)this.COMMAND, (object)this.COMMANDTYPE, this.ISREQUEST ? (object)"Y" : (object)"N", (object)this.SITEID, (object)this.USERID, (object)this.IPADDRESS, this.ISSUCCESS ? (object)"Y" : (object)"N");
    }
}
