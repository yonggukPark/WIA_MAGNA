using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

using HQCWeb.FW.Data;

namespace HQCWeb.FW
{
    public class DataBaseService
    {
        //public static string defaultDatabaseConnection = string.Empty;
        public static Dictionary<string, SqlMapper> mappers = new Dictionary<string, SqlMapper>();

        public static SqlMapper mapper;

        public static void SetDataBase()
        {
            mapper = new SqlMapper();

            //defaultDatabaseConnection = ConfigurationManager.AppSettings.Get("DefaultConnectionstring");
            string strDbNames = System.Configuration.ConfigurationManager.AppSettings.Get("DatabaseList");
            string[] arrDbNames = string.IsNullOrEmpty(strDbNames) ? null : strDbNames.Split(',');
            if (arrDbNames != null)
            {
                foreach (string strDbName in arrDbNames)
                {
                    //string strDbConnectionString = System.Configuration.ConfigurationManager.AppSettings.Get(strDbName + "_Connectionstring");
                    string strDbConnectionString = ConfigurationManager.ConnectionStrings[strDbName + "_Connectionstring"].ConnectionString;
                    Data.SqlMapper _mapper = new Data.SqlMapper(strDbName, strDbConnectionString);
                    mappers.Add(strDbName, _mapper);
                }
            }
        }
    }
}