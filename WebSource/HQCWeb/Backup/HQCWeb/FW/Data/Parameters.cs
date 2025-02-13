using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HQCWeb.FW.Data
{
    public class Parameters
    {

        private Hashtable _params = new Hashtable();
        private List<Hashtable> UpsertcolumnList = new List<Hashtable>();
        private List<Hashtable> UpsertkeyList = new List<Hashtable>();
        private string UpsertTagtTable = string.Empty;
        private string UpsertHistTable = string.Empty;
        public Parameters()
        {
            _params.Clear();
        }
        public Parameters(string paramId, object paramValue)
        {
            _params.Clear();

            try
            {
                if (paramValue == null)
                {
                    _params.Add(paramId, null);
                }
                else if (paramValue.GetType() == typeof(Hashtable))
                {
                    _params.Add(paramId, (Hashtable)paramValue);
                }
                else if (paramValue.GetType() == typeof(Parameters))
                {
                    _params.Add(paramId, ((Parameters)paramValue).GetParmas());
                }
                else
                {
                    _params.Add(paramId, (string)paramValue);
                }
            }
            catch (Exception e) {

            }

        }
        public Parameters(string paramValue)
        {
            _params.Clear();
            _params.Add("p" + _params.Count, paramValue);
        }


        public void Add(string paramId, object paramValue)
        {
            try
            {
                if (paramValue == null)
                {
                    _params.Add(paramId, null);
                }
                else if (paramValue.GetType() == typeof(Hashtable))
                {
                    _params.Add(paramId, (Hashtable)paramValue);
                }
                else if (paramValue.GetType() == typeof(List<Hashtable>))
                {
                    _params[paramId] = (List<Hashtable>)paramValue;
                    //_params.Add(paramId, (List<Hashtable>)paramValue);
                }
                else if (paramValue.GetType() == typeof(Parameters))
                {
                    _params.Add(paramId, ((Parameters)paramValue).GetParmas());
                }
                else if (paramValue.GetType() == typeof(byte[]))
                {
                    _params.Add(paramId, (byte[])paramValue);
                }
                else
                {
                    _params.Add(paramId, (string)paramValue);
                    /*
                    string strChangeText = string.Empty;

                    strChangeText = paramValue.ToString().Replace("<", "&lt;").Replace(">", "&gt;");

                    _params.Add(paramId, strChangeText);
                    // */
                }
            }
            catch (Exception e)
            {

            }
        }
        
        public void AddContent(string paramId, object paramValue)
        {
            try
            {
                if (paramValue == null)
                {
                    _params.Add(paramId, null);
                }
                else if (paramValue.GetType() == typeof(Hashtable))
                {
                    _params.Add(paramId, (Hashtable)paramValue);
                }
                else if (paramValue.GetType() == typeof(List<Hashtable>))
                {
                    _params[paramId] = (List<Hashtable>)paramValue;
                    //_params.Add(paramId, (List<Hashtable>)paramValue);
                }
                else if (paramValue.GetType() == typeof(Parameters))
                {
                    _params.Add(paramId, ((Parameters)paramValue).GetParmas());
                }
                else if (paramValue.GetType() == typeof(byte[]))
                {
                    _params.Add(paramId, (byte[])paramValue);
                }
                else
                {
                    string strChangeText = string.Empty;

                    strChangeText = paramValue.ToString().Replace("<", "&lt;").Replace(">", "&gt;");

                    _params.Add(paramId, strChangeText);
                }
            }
            catch (Exception e)
            {

            }
        }
        
        public void Add(string paramValue)
        {
            _params.Add("p" + _params.Count, paramValue);
        }
        public void Clear()
        {
            _params.Clear();
        }

        public Hashtable GetParmas()
        {
            return _params;
        }

        public string GetParmValue(string paramId)
        {
            string strRtn = null;
            if (_params == null || !_params.ContainsKey(paramId)) return null;
            strRtn = (string)_params[paramId];
            return strRtn;
        }

        public int Count
        {
            get
            {
                if (_params != null && _params.Count > 0)
                {
                    return (_params.Count);
                }

                return 0;
            }
        }

        public void AddUpsertColumn(string name, string value)
        {
            Hashtable _htTemp = new Hashtable();
            _htTemp["columnName"] = name;
            _htTemp["columnValue"] = value;
            int nDataIdx = -1;
            nDataIdx = UpsertcolumnList.FindIndex(ht => ht.ContainsKey(name));
            if (nDataIdx >= 0)
            {
                UpsertcolumnList[nDataIdx] = _htTemp;
            }
            else
            {
                UpsertcolumnList.Add(_htTemp);
            }

            this.Add("COLUMNS", UpsertcolumnList);
        }

        public void AddUpsertKey(string name, string value)
        {
            Hashtable _htTemp = new Hashtable();
            _htTemp["columnName"] = name;
            _htTemp["columnValue"] = value;
            int nDataIdx = -1;
            nDataIdx = UpsertkeyList.FindIndex(ht => ht.ContainsKey(name));
            if (nDataIdx >= 0)
            {
                UpsertkeyList[nDataIdx] = _htTemp;
            }
            else
            {
                UpsertkeyList.Add(_htTemp);
            }
            this.Add("WHERES", UpsertkeyList);
        }

        public void AddUpsertTargetTable(string value)
        {
            this.Add("TARGET_TABLE", value);
        }

        public void AddUpsertHistoryTable(string value)
        {
            this.Add("HISTORY_TABLE", value);
        }
    }
}
