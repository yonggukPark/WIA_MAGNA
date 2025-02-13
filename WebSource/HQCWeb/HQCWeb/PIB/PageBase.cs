using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace WebPIB
{
	public class PageBase : System.Web.UI.Page
	{
        #region PageBase
        public SqlConnection m_SqlDBCon;
		public SqlCommand m_SqlDBCmd;
		public SqlDataReader m_SqlDataReader;
		public SqlTransaction m_SqlDBTrans;
		public string m_strConnString;
		public bool m_DBTransaction = false;
		public string m_strErrCode;
		public string m_strErrText;
		public static string strLinkedServer = ConfigurationManager.AppSettings.Get("LinkedServer");

		// db연결
		public void DbConnect()
		{
			string dbLocation = ConfigurationManager.AppSettings.Get("dbLocation");
			string dbUser = ConfigurationManager.AppSettings.Get("dbUser");
			string dbPassword = ConfigurationManager.AppSettings.Get("dbPassword");
			string dbName = ConfigurationManager.AppSettings.Get("dbName");
			m_strConnString = string.Format("Data source={0};User Id={1};Password={2};Initial Catalog={3}", dbLocation, dbUser, dbPassword, dbName);

			if (m_SqlDBCon == null || m_SqlDBCon.State == ConnectionState.Closed)
			{
				m_SqlDBCon = new SqlConnection(m_strConnString);
				m_SqlDBCon.Open();
			}
			if (m_DBTransaction) m_SqlDBTrans = m_SqlDBCon.BeginTransaction();
		}

		public void DBDisconnect()
		{
			if (m_DBTransaction) m_SqlDBTrans = null;
			if (m_SqlDBCon != null && m_SqlDBCon.State == ConnectionState.Open)
			{
				m_SqlDBCon.Close();
				m_SqlDBCon.Dispose();
			}

			if (m_SqlDBCmd != null) ((SqlCommand)m_SqlDBCmd).Dispose();
		}

		public void Commit()
		{
			try
			{
				if (m_DBTransaction)
				{
					m_SqlDBTrans.Commit();
					m_SqlDBTrans = m_SqlDBCon.BeginTransaction();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public void RollBack()
		{
			try
			{
				if (m_DBTransaction)
				{
					m_SqlDBTrans.Rollback();
					m_SqlDBTrans = m_SqlDBCon.BeginTransaction();
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public bool ExecuteNonQuery(string p_strCmdText, System.Data.SqlClient.SqlParameter[] p_sqlDbParams, CommandType p_cmdType, ref string p_strErrCode, ref string p_strErrText)
		{
			bool bReturn = true;
			int nResult = 0;

			try
			{
				DbConnect();
				m_SqlDBCmd = m_SqlDBCon.CreateCommand();
				m_SqlDBCmd.CommandType = p_cmdType;
				m_SqlDBCmd.CommandText = p_strCmdText;
				m_SqlDBCmd.Transaction = m_SqlDBTrans;

				if (p_sqlDbParams != null)
				{
					foreach (SqlParameter param in p_sqlDbParams)
					{
						if ((param.Value == null) || ((param.Value.GetType().ToString().Equals("System.String")) && ((string)(param.Value)).Length == 0))
						{
							m_SqlDBCmd.Parameters.Add(new SqlParameter(param.ParameterName, System.DBNull.Value));
							//((SqlParameter)p_objParam).Value = System.DBNull.Value;
						}
						else
						{
							m_SqlDBCmd.Parameters.Add(new SqlParameter(param.ParameterName, param.Value));

						}
					}
				}

				nResult = m_SqlDBCmd.ExecuteNonQuery();

				if (nResult < 1)
				{
					p_strErrCode = "NODATA";
					bReturn = false;
				}
				Commit();

			}
			catch (Exception ex)
			{
				RollBack();
				p_strErrCode = "ERROR";
				p_strErrCode = ex.Message;
				bReturn = false;
			}
			finally
			{
				DBDisconnect();
			}

			return bReturn;
		}

		public bool GetDataSet(string p_strCmdText, System.Data.SqlClient.SqlParameter[] p_sqlDbParams, CommandType p_cmdType, ref DataSet p_dataSet, ref string p_strErrCode, ref string p_strErrText)
		{
			bool bReturn = true;
			SqlDataAdapter da = null;

			try
			{
				DbConnect();
	
				m_SqlDBCmd = m_SqlDBCon.CreateCommand();
				m_SqlDBCmd.CommandType = p_cmdType;
				m_SqlDBCmd.CommandText = p_strCmdText;
				m_SqlDBCmd.Transaction = m_SqlDBTrans;

				if (p_sqlDbParams != null)
				{
					foreach (SqlParameter param in p_sqlDbParams)
					{
						if ((param.Value == null) || ((param.Value.GetType().ToString().Equals("System.String")) && ((string)(param.Value)).Length == 0))
						{
							m_SqlDBCmd.Parameters.Add(new SqlParameter(param.ParameterName, System.DBNull.Value));
							//((SqlParameter)p_objParam).Value = System.DBNull.Value;
						}
						else
						{
							m_SqlDBCmd.Parameters.Add(new SqlParameter(param.ParameterName, param.Value));

						}
					}
				}

				p_dataSet = new DataSet();
				da = new SqlDataAdapter(m_SqlDBCmd);
				da.Fill(p_dataSet);

				if (p_dataSet == null || p_dataSet.Tables.Count < 1)
				{
					p_strErrCode = "NODATA";
					bReturn = false;
				}

			}
			catch (Exception ex)
			{
				p_strErrCode = "ERROR";
				p_strErrCode = ex.Message;
				bReturn = false;
			}
			finally
			{
				if (da != null) da.Dispose();
				DBDisconnect();
			}

			return bReturn;
		}

		public void UpdateHeartBit(string strMonId, string strPibId, string strMonIp)
		{
			bool bResult = true;
			//m_DBTransaction = true;
			try
			{
				SqlParameter[] param = new SqlParameter[3];
				param[0] = new SqlParameter("@P_MON_ID", strMonId);
				param[1] = new SqlParameter("@P_PIB_ID", strPibId);
				param[2] = new SqlParameter("@P_MON_IP", strMonIp);
				bResult = ExecuteNonQuery(strLinkedServer + "SP_PIB_SET_HEARTBIT", param, CommandType.StoredProcedure, ref m_strErrCode, ref m_strErrText);
			}
			catch
			{

			}
			finally
			{
				m_DBTransaction = false;
			}
		}
        #endregion
    }
}