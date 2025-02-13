
#region Apache Notice
/*****************************************************************************
 * $Header: $
 * $Revision: 591573 $
 * $Date: 2007-11-03 04:17:59 -0600 (Sat, 03 Nov 2007) $
 * 
 * iBATIS.NET Data Mapper
 * Copyright (C) 2005 - Gilles Bayon
 *  
 * 
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * 
 *      http://www.apache.org/licenses/LICENSE-2.0
 * 
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 * 
 ********************************************************************************/
#endregion

#region Using

using System;
using System.Collections.Specialized;
using System.Data;
using System.Reflection;
using System.Text;
using HQCWeb.FW.Data.Common;
using HQCWeb.FW.Data.Common.Utilities.Objects;
using HQCWeb.FW.Data.DataMapper.Configuration.ParameterMapping;
using HQCWeb.FW.Data.DataMapper.Configuration.Statements;
using HQCWeb.FW.Data.DataMapper.Exceptions;
using HQCWeb.FW.Data.DataMapper.Scope;
using log4net;

#endregion

namespace HQCWeb.FW.Data.DataMapper.Commands
{
    /// <summary>
    /// Summary description for DefaultPreparedCommand.
    /// </summary>
    internal class DefaultPreparedCommand : IPreparedCommand
    {
        private ILog log = LogManager.GetLogger(Mapper._processName);
        #region IPreparedCommand Members

        /// <summary>
        /// Create an IDbCommand for the SqlMapSession and the current SQL Statement
        /// and fill IDbCommand IDataParameter's with the parameterObject.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="session">The SqlMapSession</param>
        /// <param name="statement">The IStatement</param>
        /// <param name="parameterObject">
        /// The parameter object that will fill the sql parameter
        /// </param>
        /// <returns>An IDbCommand with all the IDataParameter filled.</returns>
        public void Create(RequestScope request, ISqlMapSession session, IStatement statement, object parameterObject )
		{
			// the IDbConnection & the IDbTransaction are assign in the CreateCommand 
            request.IDbCommand = new DbCommandDecorator(session.CreateCommand(statement.CommandType), request);
			
			request.IDbCommand.CommandText = request.PreparedStatement.PreparedSql;

            if (Mapper._activeLogging)
            {
				if (parameterObject != null)
				{
					string strTemp = string.Format("Query:{0}\n, Params:{1}", request.PreparedStatement.PreparedSql, parameterObject.ToString());
					log.Info(strTemp);
				}
				else
				{
					string strTemp = string.Format("Query:{0}", request.PreparedStatement.PreparedSql);
					log.Info(strTemp);
				}
            }

            ApplyParameterMap( session, request.IDbCommand, request, statement, parameterObject  );
		}


        /// <summary>
        /// Applies the parameter map.
        /// </summary>
        /// <param name="session">The session.</param>
        /// <param name="command">The command.</param>
        /// <param name="request">The request.</param>
        /// <param name="statement">The statement.</param>
        /// <param name="parameterObject">The parameter object.</param>
		protected virtual void ApplyParameterMap
			( ISqlMapSession session, IDbCommand command,
			RequestScope request, IStatement statement, object parameterObject )
		{
			StringCollection properties = request.PreparedStatement.DbParametersName;
            IDbDataParameter[] parameters = request.PreparedStatement.DbParameters;
            StringBuilder paramLogList = new StringBuilder(); // Log info
            StringBuilder typeLogList = new StringBuilder(); // Log info
            
			int count = properties.Count;

            for ( int i = 0; i < count; ++i )
			{
                IDbDataParameter sqlParameter = parameters[i];
                IDbDataParameter parameterCopy = command.CreateParameter();
				ParameterProperty property = request.ParameterMap.GetProperty(i);

				if (command.CommandType == CommandType.StoredProcedure)
				{
					#region store procedure command

					// A store procedure must always use a ParameterMap 
					// to indicate the mapping order of the properties to the columns
					if (request.ParameterMap == null) // Inline Parameters
					{
						throw new DataMapperException("A procedure statement tag must alway have a parameterMap attribute, which is not the case for the procedure '"+statement.Id+"'."); 
					}
					else // Parameters via ParameterMap
					{
						if (property.DirectionAttribute.Length == 0)
						{
							property.Direction = sqlParameter.Direction;
						}

						sqlParameter.Direction = property.Direction;					
					}
					#endregion 
				}

				request.ParameterMap.SetParameter(property, parameterCopy, parameterObject );

				parameterCopy.Direction = sqlParameter.Direction;

				// With a ParameterMap, we could specify the ParameterDbTypeProperty
				if (request.ParameterMap != null)
				{
                    if (property.DbType != null && property.DbType.Length > 0)
					{
						string dbTypePropertyName = session.DataSource.DbProvider.ParameterDbTypeProperty;
						object propertyValue = ObjectProbe.GetMemberValue(sqlParameter, dbTypePropertyName, request.DataExchangeFactory.AccessorFactory);
						ObjectProbe.SetMemberValue(parameterCopy, dbTypePropertyName, propertyValue, 
							request.DataExchangeFactory.ObjectFactory, request.DataExchangeFactory.AccessorFactory);
					}
					else
					{
						//parameterCopy.DbType = sqlParameter.DbType;
					}
				}
				else
				{
					//parameterCopy.DbType = sqlParameter.DbType;
				}


				// JIRA-49 Fixes (size, precision, and scale)
				if (session.DataSource.DbProvider.SetDbParameterSize) 
				{
					if (sqlParameter.Size > 0) 
					{
						parameterCopy.Size = sqlParameter.Size;
					}
				}

				if (session.DataSource.DbProvider.SetDbParameterPrecision) 
				{
					parameterCopy.Precision = sqlParameter.Precision;
				}
				
				if (session.DataSource.DbProvider.SetDbParameterScale) 
				{
					parameterCopy.Scale = sqlParameter.Scale;
				}				

				parameterCopy.ParameterName = sqlParameter.ParameterName;

				command.Parameters.Add( parameterCopy );
			}
		}

		#endregion
	}
}
