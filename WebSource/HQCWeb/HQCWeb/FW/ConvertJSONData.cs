using System;
using System.Collections.Generic;
using System.Data;

using System.Dynamic;
using Newtonsoft.Json;

using MES.FW.Common.Crypt;

namespace HQCWeb.FW
{
    public class ConvertJSONData
    {
        #region ConvertColArrToJSON
        //string[] -> JSON으로 변경
        public static string ConvertColArrToJSON(string[] arrCol, string[] arrCap, string[] arrWidth, string type)
        {
            var dynamicList = new List<ExpandoObject>();

            for (int i = 0; i < arrCol.Length; i++)
            {
                dynamic dynamicRow = new ExpandoObject();
                var dict = dynamicRow as IDictionary<string, object>;
                if (type.Equals("fields"))
                {
                    dict["fieldName"] = arrCol[i];
                    dict["dataType"] = "text";
                }
                else if (type.Equals("cols"))
                {
                    dict["name"] = arrCol[i];
                    dict["fieldName"] = arrCol[i];
                    dict["width"] = arrWidth[i];
                    dict["header"] = new { text = arrCap[i] };
                }
                dynamicList.Add(dynamicRow);
            }

            return JsonConvert.SerializeObject(dynamicList);
        }
        #endregion

        #region ConvertDataTableToJSON
        //DataTable -> JSON으로 변경
        public static string ConvertDataTableToJSON(DataTable dataTable, string[] strKeyCol)
        {
            Crypt cy = new Crypt();

            cy.Key = System.Configuration.ConfigurationManager.AppSettings.Get("HQC_CRYPTKEY");

            var dynamicList = new List<ExpandoObject>();
            
            foreach (DataRow row in dataTable.Rows)
            {
                string strEncrypt = string.Empty;
                
                dynamic dynamicRow = new ExpandoObject();
                var rowDict = dynamicRow as IDictionary<string, object>;

                for (int i = 0; i < strKeyCol.Length; i++) {

                    foreach (DataColumn column in dataTable.Columns)
                    {
                        rowDict[column.ColumnName] = Convert.ChangeType(row[column], column.DataType);

                        if (column.ColumnName == strKeyCol[i].ToString())
                        {
                            if (strEncrypt == "") {
                                strEncrypt = Convert.ChangeType(row[column], column.DataType).ToString();
                            } else
                            {
                                strEncrypt += "/" +  Convert.ChangeType(row[column], column.DataType).ToString();
                            }
                        }
                    }
                }

                rowDict["KEY_HID"] = cy.Encrypt(strEncrypt);

                dynamicList.Add(dynamicRow);
            }
            return JsonConvert.SerializeObject(dynamicList);
        }
        #endregion

        #region ConvertDataTableToJSON2
        //DataTable -> JSON으로 단순변경
        public static string ConvertDataTableToJSON2(DataTable dataTable)
        {
            var dynamicList = new List<ExpandoObject>();
            
            foreach (DataRow row in dataTable.Rows)
            {
                string strEncrypt = string.Empty;
                
                dynamic dynamicRow = new ExpandoObject();
                var rowDict = dynamicRow as IDictionary<string, object>;

                foreach (DataColumn column in dataTable.Columns)
                {
                    rowDict[column.ColumnName] = Convert.ChangeType(row[column], column.DataType);
                }

                dynamicList.Add(dynamicRow);
            }
            return JsonConvert.SerializeObject(dynamicList);
        }
        #endregion
    }
}