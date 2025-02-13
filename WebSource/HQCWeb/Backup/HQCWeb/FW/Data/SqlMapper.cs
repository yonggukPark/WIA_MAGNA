/* NOTE : 2023.08.01 생성
 * 1. GetData : 단일 String 반환 
 * 2. ExcuteInUpDeQuery : Insert, Update, Delete
 * 3. ExcuteDataTable : DataTable 반환
 * 묵시적(p0, p1, p3, .... p*), 명시적 파라메터 지정
 */

//using HQCWeb.FW.Common.Data;
using HQCWeb.FW.Data.DataMapper;
using HQCWeb.FW.Data.DataMapper.Configuration.Statements;
using HQCWeb.FW.Data.DataMapper.Exceptions;
using HQCWeb.FW.Data.DataMapper.SessionStore;
using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;

namespace HQCWeb.FW.Data
{
    public class SqlMapper
    {
        private ISqlMapper mapper;
        public ISqlMapSession sqlMapSession;
        private string connectionstring = null;
        private bool activeTransaction = false;
        private static bool activeLogging = false;
        private static string processName = MethodBase.GetCurrentMethod().DeclaringType.FullName;
        
        public SqlMapper()
        {
            activeLogging = false;
            mapper = EntityMapper;
        }
        public SqlMapper(string p_processName)
        {
            processName = p_processName;
            activeLogging = true;
            mapper = EntityMapper;
        }
        public SqlMapper(string p_processName, string p_connectionString)
        {
            processName = p_processName;
            activeLogging = true;
            ConnectionString = p_connectionString;
            mapper = EntityMapper;
        }
        public SqlMapper(string p_processName, bool p_activeLogging)
        {
            processName = p_processName;
            activeLogging = p_activeLogging;
            mapper = EntityMapper;
        }
        public SqlMapper(string p_processName, string p_connectionString, bool p_activeLogging)
        {
            processName = p_processName;
            activeLogging = p_activeLogging;
            ConnectionString = p_connectionString;
            mapper = EntityMapper;
        }

        public string ConnectionString
        {
            set
            {
                connectionstring = value;
                if (mapper != null)
                {
                    mapper.DataSource.ConnectionString = value;
                }
            }
            get
            {
                return connectionstring;
            }
        }
        public ISqlMapper EntityMapper
        {
            get
            {
                try
                {
                    mapper = Mapper.Instance(processName, activeLogging);

                    if(mapper == null) return mapper;

                    if (!string.IsNullOrEmpty(connectionstring))
                    {
                        mapper.DataSource.ConnectionString = ConnectionString;
                    }
                    else
                    {
                        ConnectionString = mapper.DataSource.ConnectionString;
                    }

                    return mapper;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private bool mapperStatus
        {
            get
            {
                if (mapper == null)
                {
                    mapper = EntityMapper;
                }

                if (mapper == null) return false;
                return true;
            }
        }

        public ISqlMapSession BeginTransaction()
        {
            return mapper.BeginTransaction();
        }
        
        public void CommitTransaction(bool closeConnection = true)
        {
            mapper.CommitTransaction(closeConnection);
        }

        public void RollBackTransaction()
        {
            RollBackTransaction(true);
        }

        public void RollBackTransaction(bool closeConnection)
        {
            mapper.RollBackTransaction(closeConnection);
        }

        public void Connect(bool bActiveTransaction)
        {
            if (!mapperStatus) return;

            if(string.IsNullOrEmpty(ConnectionString))
            {
                mapper.OpenConnection();
            }
            else
            {
                mapper.OpenConnection(ConnectionString);
            }
            activeTransaction = bActiveTransaction;

            if (activeTransaction)
            {
                mapper.BeginTransaction();
            }
        }

        public void Disconnect()
        {
            if (!mapperStatus) return;

            if (mapper.IsSessionStarted)
            {
                mapper.CommitTransaction();
                mapper.CloseConnection();
            }
        }

        public string GetScalar(string queryId, Parameters parameters)
        {
            object obj = mapper.QueryForObject(queryId, parameters.GetParmas());
            if (obj.GetType() == typeof(string))
            {
                return (string)obj;
            }
            else if (obj.GetType() == typeof(string[]))
            {
                string[] _Temp = (string[])obj;
                return _Temp[0];
            }
            else if (obj.GetType() == typeof(Hashtable))
            {

                Hashtable _Temp = (Hashtable)obj;
                return (string)_Temp.Values.GetEnumerator().Current;
            }
            else if (obj.GetType() == typeof(Object[]))
            {
                string[] _Temp = Array.ConvertAll((Object[])obj, p => (p ?? String.Empty).ToString());
                return _Temp[0];
            }
            else
            {
                return null;
            }

        }
        public IList GetDataList(string queryId, Parameters parameters)
        {
            if (parameters != null)
            {
                return mapper.QueryForList(queryId, parameters.GetParmas());
            }
            else
            {
                return mapper.QueryForList(queryId, null);
            }
        }
        public object QueryForDataTable(string queryId, Parameters parameters)
        {
            if (parameters != null)
            {
                return mapper.QueryForDataTable(queryId, parameters.GetParmas());
            }
            else
            {
                return mapper.QueryForDataTable(queryId, null);
            }
        }
        public object GetDataTable(string queryId, Parameters parameters)
        {
            if (parameters != null)
            {
                return mapper.QueryForDataTable(queryId, parameters.GetParmas());
            }
            else
            {
                return mapper.QueryForDataTable(queryId, null);
            }
        }
        public object GetDataSet(string queryId, Parameters parameters)
        {
            if (parameters != null)
            {
                return mapper.QueryForDataSet(queryId, parameters.GetParmas());
            }
            else
            {
                return mapper.QueryForDataSet(queryId, null);
            }
        }

        #region "Excute : Insert, Update, Delete"

        /// <summary>
        /// Excute : Insert, Update, Delete (묵시적 파라메터)
        /// </summary>
        /// <param name="_mapper"></param>
        /// <param name="statementID"></param>
        /// <param name="paramValues"></param>
        /// <returns>'-1' : 실패 / '0' : 처리없음(0건) / '1' : 성공 및 처리건수</returns>
        public int Excute(string queryId, Parameters parameters)
        {

            int iReturn = -1;

            // iReturn - '-1' : 실패 / '0' : 처리없음(0건) / '1' : 성공 및 처리건수
            iReturn = (int)mapper.Update(queryId, parameters.GetParmas());

            return iReturn;
        }

        /// <summary>
        /// Excute : Insert, Update, Delete (묵시적 파라메터)
        /// </summary>
        /// <param name="_mapper"></param>
        /// <param name="statementID"></param>
        /// <param name="paramValues"></param>
        /// <returns>'-1' : 실패 / '0' : 처리없음(0건) / '1' : 성공 및 처리건수</returns>
        public int ExcuteNonQuery(string queryId, Parameters parameters)
        {
            int iReturn = -1;
            int iCUDReturn = -1;

            string strCUD_ChangeData    = string.Empty;
            string strCUD_PrevData      = string.Empty;
            string strLogFlag           = string.Empty;
            string strRegID             = string.Empty;
            string strMenuID            = string.Empty;
            string strCUD_Type          = string.Empty;

            Hashtable htParameter = new Hashtable();

            htParameter = parameters.GetParmas();

            foreach (DictionaryEntry de in htParameter) {
                if (de.Key.ToString() == "REG_ID")
                {
                    strRegID = de.Value.ToString();
                }

                if (de.Key.ToString() == "LogFlag") {
                    strLogFlag = de.Value.ToString();
                }

                if (de.Key.ToString() == "CUD_TYPE")
                {
                    strCUD_Type = de.Value.ToString();
                }

                if (de.Key.ToString() == "CUR_MENU_ID")
                {
                    strMenuID = de.Value.ToString();
                }

                if (de.Key.ToString() == "PREV_DATA")
                {
                    strCUD_PrevData = de.Value.ToString();
                }

                if (de.Key.ToString() != "REG_ID" && de.Key.ToString() != "LogFlag" && de.Key.ToString() != "CUD_TYPE" && de.Key.ToString() != "CUR_MENU_ID" && de.Key.ToString() != "PREV_DATA")
                {
                    if (strCUD_ChangeData == "")
                    {
                        strCUD_ChangeData = de.Key + " : " + de.Value;
                    }
                    else
                    {
                        strCUD_ChangeData += "," + de.Key + " : " + de.Value;
                    }
                }
            }

            if (strLogFlag == "") {
                strLogFlag = "Y";
            }
            
            if (strLogFlag == "Y")
            {
                //strRegID = strRegID;

                Parameters sParams = new Parameters();

                sParams.Add("MENU_ID",          strMenuID);
                sParams.Add("QUERY_ID",         queryId);
                sParams.Add("CUD_TYPE",         strCUD_Type);
                sParams.Add("CUD_PREV_DATA",    strCUD_PrevData);
                sParams.Add("CUD_CHANGE_DATA",  strCUD_ChangeData);
                sParams.Add("REG_ID",           strRegID);
                
                iCUDReturn = (int)mapper.Update("CUDData.Set_MESCUD_Data", sParams.GetParmas());

                if (iCUDReturn == 1)
                {
                    // iReturn - '-1' : 실패 / '0' : 처리없음(0건) / '1' : 성공 및 처리건수
                    iReturn = (int)mapper.Update(queryId, parameters.GetParmas());
                }
                else
                {
                    iReturn = -1;
                }
            }
            else {
                // iReturn - '-1' : 실패 / '0' : 처리없음(0건) / '1' : 성공 및 처리건수
                iReturn = (int)mapper.Update(queryId, parameters.GetParmas());
            }

            return iReturn;
        }
        #endregion
    }
}