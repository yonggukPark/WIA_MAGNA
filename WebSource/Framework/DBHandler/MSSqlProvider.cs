using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Collections;

using MES.FW.Common.CommonMgt;

using System.Web.UI.WebControls;

namespace MES.FW.Common.DBHandler
{
    public class MSSqlProvider
    {
        #region Public Members
        public string strConnectString = string.Empty;

        public string ConnectString
        { 
            set
            {
                strConnectString = value;
            }
            get
            {
                return strConnectString;
            }    
        }
        #endregion

        #region Private Members
        private SqlConnection SqlConn;
        private SqlCommand SqlComm;
        private SqlDataAdapter SqlAdapter;

        private SqlConnection SqlConnLog;
        private SqlCommand SqlCommLog;
        private SqlDataAdapter SqlAdapterLog;

        private string strProcessQuery = string.Empty;
        private string strUserID = string.Empty;
        #endregion

        #region 생성자
        /// <summary>
        /// Default 생성자
        /// </summary>
        public MSSqlProvider()
        {

        }

        public MSSqlProvider(string ConnectString)
        {
            strConnectString = ConnectString;
        }

        #endregion

        #region Connection 객체 생성 및 소멸 선언
        private SqlConnection SQLConn()
        {
            SqlConn = new SqlConnection();

            SqlConn.ConnectionString = strConnectString;

            SqlConn.Open();

            return SqlConn;
        }

        /// <summary>
        /// DB Connection Close, Dispose, 개체소멸
        /// </summary>
        private void SqlConnClose()
        {
            SqlConn.Close();
            SqlConn.Dispose();
        }



        private SqlConnection SQLConnLog()
        {
            SqlConnLog = new SqlConnection();

            SqlConnLog.ConnectionString = strConnectString;

            SqlConnLog.Open();

            return SqlConnLog;
        }

        /// <summary>
        /// DB Connection Close, Dispose, 개체소멸
        /// </summary>
        private void SqlConnCloseLog()
        {
            SqlConnLog.Close();
            SqlConnLog.Dispose();
        }

        #endregion

        #region DataSet
        /// <summary>
        /// DataSet생성
        /// </summary>
        /// <param name="pSql">SQL문</param>
        /// <param name="pSys">연결대상 서버</param>
        public DataSet ExecuteDataSet(string pSql)
        {
            try
            {
                using (this.SQLConn())
                {
                    SqlComm = new SqlCommand(pSql, SqlConn);
                    SqlComm.CommandType = CommandType.StoredProcedure;

                    SqlAdapter = new SqlDataAdapter(SqlComm);
                    DataSet ds = new DataSet();
                    SqlAdapter.Fill(ds, "ds");

                    return ds;
                }
            }
            catch (SqlException sqlExce)
            {
                LogUtils._DBErrorWirte(sqlExce, pSql);
                throw sqlExce;
            }
            catch (Exception ex)
            {
                LogUtils._ErrorWirte(ex);
                throw ex;
            }
            finally
            {
                SqlAdapter.Dispose();
                SqlComm.Dispose();
                this.SqlConnClose();
            }
        }


        /// <summary>
        /// DML(select)사용 DataSet생성
        /// </summary>
        /// <param name="pSql">SQL문</param>
        /// <param name="sqlParameters">sqlParameters문</param>
        /// <param name="pSys">연결대상 서버</param>
        public DataSet ExecuteDataSet(string pSql, SqlParameter[] sqlParameters)
        {
            try
            {
                using (this.SQLConn())
                {
                    SqlComm = new SqlCommand(pSql, SqlConn);
                    SqlComm.CommandType = CommandType.StoredProcedure;

                    if (sqlParameters != null)
                    {
                        SetParamAdd(SqlComm, sqlParameters);
                    }

                    SqlAdapter = new SqlDataAdapter(SqlComm);
                    DataSet ds = new DataSet();
                    SqlAdapter.Fill(ds, "ds");

                    return ds;
                }
            }
            catch (SqlException sqlExce)
            {
                LogUtils._DBErrorWirte(sqlExce, pSql);
                throw sqlExce;
            }
            catch (Exception ex)
            {
                LogUtils._ErrorWirte(ex);
                throw ex;
            }
            finally
            {
                SqlAdapter.Dispose();
                SqlComm.Dispose();
                this.SqlConnClose();
            }
        }

        public DataSet Fill(string query)
        {
            DataSet ds = new DataSet();

            try
            {
                using (SqlConnection sqlCon = new SqlConnection(ConnectString))
                {
                    sqlCon.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter(query, ConnectString);

                    dataAdapter.Fill(ds);

                    sqlCon.Close();
                }
            }
            catch (Exception ex)
            {
                //LogUtils._ErrorWirte(ex);
                //System.Diagnostics.Debug.WriteLine("Fill : " + ex.Message);
            }

            return ds;
        }

        #endregion

        #region Scalar
        /// <summary>
        /// DML(select)사용 단일 값(예: 집계 값, 데이터 유무확인)을 검색합니다.
        /// </summary>
        /// <param name="pSql">SQL문</param>
        /// <param name="pType">명령문자열 타입</param>
        /// <param name="pSys">연결대상 서버</param>
        /// <returns>object .NET Framework 데이터 형식</returns>
        public object ExecuteScalarData(string pSql)
        {
            try
            {
                SqlComm = new SqlCommand();

                using (this.SQLConn())
                {
                    SqlComm.CommandText = pSql;
                    SqlComm.CommandType = CommandType.StoredProcedure;
                    SqlComm.Connection = this.SqlConn;

                    return SqlComm.ExecuteScalar();
                }
            }
            catch (SqlException sqlExce)
            {
                //DBLogUtils._DBErrorWirte(sqlExce, pSql);
                throw sqlExce;
            }
            catch (Exception ex)
            {
                //DBLogUtils._ErrorWirte(ex);
                throw ex;
            }
            finally
            {
                SqlComm.Dispose();
                this.SqlConnClose();
            }
        }

        public object ExecuteScalarData(string pSql, SqlParameter[] pParam)
        {
            try
            {
                SqlComm = new SqlCommand();

                using (this.SQLConn())
                {
                    SqlComm.CommandText = pSql;
                    SqlComm.CommandType = CommandType.StoredProcedure;
                    SqlComm.Connection = this.SqlConn;

                    if (pParam != null)
                    {
                        this.SetParamAdd(SqlComm, pParam);
                    }

                    return SqlComm.ExecuteScalar();
                }
            }
            catch (SqlException sqlExce)
            {
                //DBLogUtils._DBErrorWirte(sqlExce, pSql);
                throw sqlExce;
            }
            catch (Exception ex)
            {
                //DBLogUtils._ErrorWirte(ex);
                throw ex;
            }
            finally
            {
                SqlComm.Dispose();
                this.SqlConnClose();
            }
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// DML(insert, update, delete)사용 CommandType.Text전용, SqlParameter, SqlTransaction처리
        /// </summary>
        /// <param name="pSql">SQL문</param>
        /// <param name="pParam">SqlParameter 배열</param>
        /// <param name="pSys">연결대상 서버</param>
        /// <returns>int</returns>
        public int ExecuteNonQuery(string pSql, SqlParameter[] pParam)
        {
            MSSqlTrans SqlTran = new MSSqlTrans();

            try
            {
                SqlComm = new SqlCommand();
                int result;

                using (this.SQLConn())
                {
                    SqlTran.BeginTrans = SqlConn.BeginTransaction();

                    SqlComm.CommandText = pSql;
                    SqlComm.CommandType = CommandType.StoredProcedure;
                    SqlComm.Connection = this.SqlConn;

                    SqlComm.Transaction = SqlTran.GetTrans;

                    // 파라미터 셋팅
                    if (pParam != null)
                    {
                        this.SetParamAdd(SqlComm, pParam);
                    }

                    result = SqlComm.ExecuteNonQuery();

                    SqlTran.CommitTrans();
                }

                string[] strSql;
                int iLen = 0;
                string strProcessType = string.Empty;
                string strTableName = string.Empty;
                string strPageID = string.Empty;
                string strWorkType = string.Empty;

                strSql = pSql.Split('_');
                iLen = strSql.Length - 1;

                if (strSql.Length > 1)
                {
                    strWorkType = strSql[1].ToString();

                    if (strWorkType == "WB")
                    {
                        if (strSql[3].ToString() == "M")
                        {
                            strProcessType = strSql[iLen].ToString();
                            strTableName = strSql[3].ToString() + "_" + strSql[4].ToString();
                            strPageID = "Master" + strSql[4].ToString();

                            SqlParameter[] sParam =
                            {
                                new SqlParameter("pPageID",          strPageID)
                                , new SqlParameter("pUserID",        strUserID)
                                , new SqlParameter("pTableName",     strTableName)
                                , new SqlParameter("pProcessQuery",  strProcessQuery)
                                , new SqlParameter("pProcessType",   strProcessType)
                            };

                            ExecuteNonQuery_LogInsert("UP_WB_ZZ_H_WorkLog_Insert", sParam);
                        }
                    }
                }

                return result;
            }
            //catch (SqlException sqlExce)
            //{
            //    LogUtils._DBErrorWirte(sqlExce, pSql);
            //    SqlTran.RollBackTrans();
            //    throw sqlExce;
                
            //}
            //catch (Exception ex)
            //{
            //    LogUtils._ErrorWirte(ex);
            //    SqlTran.RollBackTrans();
            //    throw ex;
            //}
            finally
            {
                SqlTran.Dispose();
                SqlComm.Dispose();
                this.SqlConnClose();
            }
        }


        /// <summary>
        /// DML(insert, update, delete)사용 CommandType.Text전용, SqlParameter, SqlTransaction처리
        /// </summary>
        /// <param name="pSql">SQL문</param>
        /// <param name="pParam">SqlParameter 배열</param>
        /// <param name="pSys">연결대상 서버</param>
        /// <returns>int</returns>
        //public int Execute_Insert(string pSql, SqlParameter[] pParam)
        //{
        //    MSSqlTrans SqlTran = new MSSqlTrans();

        //    string strResult;

        //    try
        //    {
        //        SqlComm = new SqlCommand();
                
        //        using (this.SQLConn())
        //        {
        //            SqlTran.BeginTrans = SqlConn.BeginTransaction();

        //            SqlComm.CommandText = pSql;
        //            SqlComm.CommandType = CommandType.StoredProcedure;
        //            SqlComm.Connection = this.SqlConn;

        //            SqlComm.Transaction = SqlTran.GetTrans;

        //            // 파라미터 셋팅
        //            if (pParam != null)
        //            {
        //                this.SetParamAdd(SqlComm, pParam);
        //            }

        //            strResult = SqlComm.ExecuteNonQuery().ToString();

        //            SqlTran.CommitTrans();
        //        }

        //        string[] strSql;
        //        int iLen = 0;
        //        string strProcessType = string.Empty;
        //        string strTableName = string.Empty;
        //        string strPageID = string.Empty;
        //        string strWorkType = string.Empty;

        //        strSql = pSql.Split('_');
        //        iLen = strSql.Length - 1;

        //        if (strSql.Length > 1)
        //        {
        //            strWorkType = strSql[1].ToString();

        //            if (strWorkType == "WB")
        //            {
        //                if (strSql[3].ToString() == "M")
        //                {
        //                    strProcessType = strSql[iLen].ToString();
        //                    strTableName = strSql[3].ToString() + "_" + strSql[4].ToString();
        //                    strPageID = "Master" + strSql[4].ToString();

        //                    SqlParameter[] sParam =
        //                    {
        //                        new SqlParameter("pPageID",          strPageID)
        //                        , new SqlParameter("pUserID",        strUserID)
        //                        , new SqlParameter("pTableName",     strTableName)
        //                        , new SqlParameter("pProcessQuery",  strProcessQuery)
        //                        , new SqlParameter("pProcessType",   strProcessType)
        //                    };

        //                    ExecuteNonQuery_LogInsert("UP_WB_ZZ_H_WorkLog_Insert", sParam);
        //                }
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        SqlTran.Dispose();
        //        SqlComm.Dispose();
        //        this.SqlConnClose();
        //    }

        //    return Convert.ToInt32(strResult);

        //    //catch (SqlException sqlExce)
        //    //{
        //    //    strResult = sqlExce.Message.ToString();
                
        //    //    //LogUtils._DBErrorWirte(sqlExce, pSql);
        //    //    //SqlTran.RollBackTrans();
        //    //    //throw sqlExce;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    //LogUtils._ErrorWirte(ex);
        //    //    //SqlTran.RollBackTrans();
        //    //    //throw ex;
        //    //}
        //}


        
        /// <summary>
        /// DML(insert, update, delete)사용 CommandType.Text전용, SqlParameter, SqlTransaction처리, 트랜잭션없음... 되도록 사용금지
        /// </summary>
        /// <param name="pSql">SQL문</param>
        /// <param name="pParam">SqlParameter 배열</param>
        /// <param name="pSys">연결대상 서버</param>
        /// <returns>int</returns>
        public int ExecuteNonQuery_NoTran(string pSql, SqlParameter[] pParam, SqlConnection _SqlConn)
        {
            try
            {
                SqlComm = new SqlCommand();
                int result;

                SqlComm.CommandText = pSql;
                SqlComm.CommandType = CommandType.StoredProcedure;
                SqlComm.Connection = _SqlConn;

                //SqlComm.Transaction = SqlTran.GetTrans;

                // 파라미터 셋팅
                if (pParam != null)
                {
                    this.SetParamAdd(SqlComm, pParam);
                }

                result = SqlComm.ExecuteNonQuery();

                return result;
            }
            catch (SqlException sqlExce)
            {
                //DBLogUtils._DBErrorWirte(sqlExce, pSql);
                throw sqlExce;
            }
            catch (Exception ex)
            {
                //DBLogUtils._ErrorWirte(ex);
                throw ex;
            }
            finally
            {
                SqlComm.Dispose();
                //this.SqlConnClose();
            }
        }


        /// <summary>
        /// DML(insert, update, delete)사용 CommandType.Text전용, SqlParameter, SqlTransaction처리
        /// </summary>
        /// <param name="pSql">SQL문</param>
        /// <param name="pParam">SqlParameter 배열</param>
        /// <param name="pSys">연결대상 서버</param>
        /// <returns>int</returns>
        public void ExecuteNonQuery_LogInsert(string pSql, SqlParameter[] pParam)
        {
            MSSqlTrans SqlTran = new MSSqlTrans();

            try
            {
                SqlCommLog = new SqlCommand();

                using (this.SQLConnLog())
                {
                    SqlTran.BeginTrans = SqlConnLog.BeginTransaction();

                    SqlCommLog.CommandText = pSql;
                    SqlCommLog.CommandType = CommandType.StoredProcedure;
                    SqlCommLog.Connection = this.SqlConnLog;

                    SqlCommLog.Transaction = SqlTran.GetTrans;

                    // 파라미터 셋팅
                    if (pParam != null)
                    {
                        this.SetParamAdd(SqlCommLog, pParam);
                    }

                    SqlCommLog.ExecuteNonQuery();

                    SqlTran.CommitTrans();
                    
                }
            }
            catch (SqlException sqlExce)
            {
                SqlTran.RollBackTrans();
                //DBLogUtils._DBErrorWirte(sqlExce, pSql);
                throw sqlExce;
            }
            catch (Exception ex)
            {
                SqlTran.RollBackTrans();
                //DBLogUtils._ErrorWirte(ex);
                throw ex;
            }
            finally 
            {
                SqlTran.Dispose();
                SqlCommLog.Dispose();
                this.SqlConnCloseLog();
            }
        }


        /// <summary>
        /// DML(insert, update, delete OR Procedure)사용, SqlParameter, OracleTransaction처리
        /// </summary>
        /// <param name="pSql">SQL문</param>
        /// <param name="pParam">SqlParameter 배열</param>
        /// <param name="pType">명령문자열 타입</param>
        /// <param name="pSys">연결대상 서버</param>
        /// <returns>object형 ArrayList, OutPut없을경우 실행하고 영향받은 행번호 리턴</returns>
        public object[] ExecuteNonQuery_obj(string pSql, SqlParameter[] pParam)
        {
            MSSqlTrans SqlTran = new MSSqlTrans();

            try
            {
                SqlComm = new SqlCommand();
                int result;

                using (this.SQLConn())
                {
                    SqlTran.BeginTrans = SqlConn.BeginTransaction();

                    SqlComm.CommandText = pSql;
                    SqlComm.CommandType = CommandType.StoredProcedure;
                    SqlComm.Connection = this.SqlConn;
                    SqlComm.Transaction = SqlTran.GetTrans;

                    // 파라미터 셋팅
                    if (pParam != null)
                    {
                        this.SetParamAdd(SqlComm, pParam);
                    }

                    result = SqlComm.ExecuteNonQuery();

                    SqlTran.CommitTrans();

                    ArrayList arry = new ArrayList();

                    this.GetParamAdd(SqlComm, arry);

                    return arry.ToArray();
                }
            }
            catch (SqlException sqlExce)
            {
                SqlTran.RollBackTrans();
                //DBLogUtils._DBErrorWirte(sqlExce, pSql);
                throw sqlExce;
            }
            catch (Exception ex)
            {
                SqlTran.RollBackTrans();
                //DBLogUtils._ErrorWirte(ex);
                throw ex;
            }
            finally
            {
                SqlTran.Dispose();
                SqlComm.Dispose();
                this.SqlConnClose();
            }
        }
        #endregion

        #region ---> Parameter Setting
        /// <summary>
        /// SqlCommand개체에 Parameter값을 셋팅한다.( Type : Input or Output )
        /// </summary>
        /// <param name="pSqlComm">SqlCommand 개체</param>
        /// <param name="pParam">SqlParameter Collection</param>
        private void SetParamAdd(SqlCommand pSqlComm, SqlParameter[] pParam)
        {
            foreach (SqlParameter iparam in pParam)
            {
                SqlParameter sParam = new SqlParameter(iparam.ParameterName, iparam.SqlDbType, iparam.Size);

                if (iparam.Direction == ParameterDirection.Input)
                {
                    sParam.Value = iparam.Value;

                    if (iparam.Value != null)
                    {
                        if (strProcessQuery == "")
                        {
                            strProcessQuery = iparam.ParameterName + "=" + iparam.Value.ToString();
                        }
                        else
                        {
                            strProcessQuery += "|" + iparam.ParameterName + "=" + iparam.Value.ToString();
                        }

                        if (iparam.ParameterName.ToUpper() == "PREGUSER" || iparam.ParameterName.ToUpper() == "PCHGUSER")
                        {
                            strUserID = iparam.Value.ToString();
                        }
                    }
                }
                else if (iparam.Direction == ParameterDirection.Output)
                {
                    sParam.Direction = ParameterDirection.Output;
                }

                pSqlComm.Parameters.Add(sParam);

            }
        }
        private ArrayList GetParamAdd(SqlCommand pSqlComm, ArrayList pArry)
        {
            for (int i = 0; i < pSqlComm.Parameters.Count; i++)
            {
                if (pSqlComm.Parameters[i].Direction == ParameterDirection.Output)
                    pArry.Add(pSqlComm.Parameters[i].Value);
            }

            return pArry;
        }
        #endregion
    }
}
