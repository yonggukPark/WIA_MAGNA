using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;

using MES.FW.Common.DBHandler;
using System.Configuration;

namespace MES.FW.Common.CommonMgt
{
    public class CondUtils
    {
        public string ConnectionString = ConfigurationManager.ConnectionStrings["MMXMES"].ConnectionString;

        MSSqlProvider db;

        public CondUtils()
        {
            db = new MSSqlProvider(ConnectionString);
        }

        //공통코드에 등록된 DropDownList 생성
        public void FillCommonCodeDropDownList(string strSpName, string Plant, string ParentCodeID, DropDownList ddl, bool Header)
        {
            ddl.Items.Clear();

            try
            {
                if (Header == true)
                {
                    ddl.Items.Add(new ListItem("ALL", ""));
                }

                DataSet ds = new DataSet();

                if (ParentCodeID == "")
                {

                    SqlParameter[] sParam =
                        {
                            new SqlParameter("pPlant",      Plant)
                        };

                    ds = db.ExecuteDataSet(strSpName, sParam);
                }
                else
                {
                    SqlParameter[] sParam =
                        {
                            new SqlParameter("pPlant",               Plant)
                            , new SqlParameter("pParentCodeID",      ParentCodeID)
                        };

                    ds = db.ExecuteDataSet(strSpName, sParam);
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddl.Items.Add(new ListItem("[" + ds.Tables[0].Rows[i]["CodeID"].ToString().Trim() + "] " +  ds.Tables[0].Rows[i]["CodeName"].ToString().Trim(), ds.Tables[0].Rows[i]["CodeID"].ToString().Trim()));
                }
            }
            catch (Exception e)
            {
                //LogUtils._ErrorWirte(e);
            }
        }

        //공통코드에 등록된 DropDownList 생성
        public void FillETCDropDownList(string strSpName, string Plant, DropDownList ddl, bool Header, string HeaderText = "All",  string[] pParam =null, string[] pValue=null)
        {
            #region ConnectionString_Plant

            string ConnectionString_Plant = string.Empty;

            switch (Plant)
            {
                case "M1":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXMODULE"].ConnectionString;
                    break;
                case "L1":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXLAMP"].ConnectionString;
                    break;
                case "A1":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXAIRBAG"].ConnectionString;
                    break;
                case "C1":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXCBS"].ConnectionString;
                    break;
                case "P1":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXPAINT"].ConnectionString;
                    break;
                case "":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXMES"].ConnectionString;
                    break;
            };

            #endregion

            MSSqlProvider MP = new MSSqlProvider(ConnectionString_Plant);

            ddl.Items.Clear();

            try
            {
                if (Header == true)
                {
                    ddl.Items.Add(new ListItem(HeaderText, ""));
                }

                DataSet ds = new DataSet();

                if (pParam != null)
                {
                    int iParam = 0;

                    iParam = pParam.Length;

                    SqlParameter[] sParam = new SqlParameter[iParam];

                    for (int i = 0; i < pParam.Length; i++)
                    {
                        sParam[i] = new SqlParameter(pParam[i], pValue[i]);
                    }

                    ds = MP.ExecuteDataSet(strSpName, sParam);
                }
                else
                {
                    ds = MP.ExecuteDataSet(strSpName);
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ddl.Items.Add(new ListItem("[" + ds.Tables[0].Rows[i]["CodeID"].ToString().Trim() + "] " + ds.Tables[0].Rows[i]["CodeName"].ToString().Trim(), ds.Tables[0].Rows[i]["CodeID"].ToString().Trim()));
                }
            }
            catch (Exception e)
            {
                //LogUtils._ErrorWirte(e);
            }
        }

        //공통코드에 등록된 CheckBoxList 생성
        public void FillCommonCodeCheckBoxList(string strSpName, string Plant, string ParentCodeID, CheckBoxList cbl)
        {
            cbl.Items.Clear();

            try
            {
                DataSet ds = new DataSet();

                SqlParameter[] sParam =
                    {
                        new SqlParameter("pPlant",               Plant)
                        , new SqlParameter("pParentCodeID",      ParentCodeID)
                    };

                ds = db.ExecuteDataSet(strSpName, sParam);
                

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cbl.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CodeName"].ToString().Trim(), ds.Tables[0].Rows[i]["CodeID"].ToString().Trim()));
                }
            }
            catch (Exception e)
            {
                //LogUtils._ErrorWirte(e);
            }
        }

        //공통코드에 등록된 CheckBoxList 생성
        public void FillETCCheckBoxList(string strSpName, string Plant, CheckBoxList cbl, string[] pParam = null, string[] pValue = null)
        {
            #region ConnectionString_Plant

            string ConnectionString_Plant = string.Empty;

            switch (Plant)
            {
                case "M1":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXMODULE"].ConnectionString;
                    break;
                case "L1":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXLAMP"].ConnectionString;
                    break;
                case "A1":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXAIRBAG"].ConnectionString;
                    break;
                case "C1":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXCBS"].ConnectionString;
                    break;
                case "P1":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXPAINT"].ConnectionString;
                    break;
                case "":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXMES"].ConnectionString;
                    break;
            };

            #endregion

            MSSqlProvider MP = new MSSqlProvider(ConnectionString_Plant);

            cbl.Items.Clear();

            try
            {
                DataSet ds = new DataSet();

                if (pParam != null)
                {
                    int iParam = 0;

                    iParam = pParam.Length;

                    SqlParameter[] sParam = new SqlParameter[iParam];

                    for (int i = 0; i < pParam.Length; i++)
                    {
                        sParam[i] = new SqlParameter(pParam[i], pValue[i]);
                    }

                    ds = MP.ExecuteDataSet(strSpName, sParam);
                }
                else
                {
                    ds = MP.ExecuteDataSet(strSpName);
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    cbl.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CodeName"].ToString().Trim(), ds.Tables[0].Rows[i]["CodeID"].ToString().Trim()));
                }
            }
            catch (Exception e)
            {
                //LogUtils._ErrorWirte(e);
            }
        }

        //공통코드에 등록된 RadioButtonList 생성
        public void FillCommonCodeRadioButtonList(string strSpName, string Plant, string ParentCodeID, RadioButtonList rbl)
        {
            rbl.Items.Clear();

            try
            {
                DataSet ds = new DataSet();

                SqlParameter[] sParam =
                    {
                        new SqlParameter("pPlant",               Plant)
                        , new SqlParameter("pParentCodeID",      ParentCodeID)
                    };

                ds = db.ExecuteDataSet(strSpName, sParam);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    rbl.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CodeName"].ToString().Trim(), ds.Tables[0].Rows[i]["CodeID"].ToString().Trim()));
                }
            }
            catch (Exception e)
            {
                //LogUtils._ErrorWirte(e);
            }
        }

        //공통코드에 등록된 RadioButtonList 생성
        public void FillETCRadioButtonList(string strSpName, string Plant, RadioButtonList rbl, string[] pParam = null, string[] pValue = null)
        {
            #region ConnectionString_Plant

            string ConnectionString_Plant = string.Empty;

            switch (Plant)
            {
                case "M1":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXMODULE"].ConnectionString;
                    break;
                case "L1":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXLAMP"].ConnectionString;
                    break;
                case "A1":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXAIRBAG"].ConnectionString;
                    break;
                case "C1":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXCBS"].ConnectionString;
                    break;
                case "P1":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXPAINT"].ConnectionString;
                    break;
                case "":
                    ConnectionString_Plant = ConfigurationManager.ConnectionStrings["MMXMES"].ConnectionString;
                    break;
            };

            #endregion

            MSSqlProvider MP = new MSSqlProvider(ConnectionString_Plant);

            rbl.Items.Clear();

            try
            {
                DataSet ds = new DataSet();

                if (pParam != null)
                {
                    int iParam = 0;

                    iParam = pParam.Length;

                    SqlParameter[] sParam = new SqlParameter[iParam];

                    for (int i = 0; i < pParam.Length; i++)
                    {
                        sParam[i] = new SqlParameter(pParam[i], pValue[i]);
                    }

                    ds = MP.ExecuteDataSet(strSpName, sParam);
                }
                else
                {
                    ds = MP.ExecuteDataSet(strSpName);
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    rbl.Items.Add(new ListItem(ds.Tables[0].Rows[i]["CodeName"].ToString().Trim(), ds.Tables[0].Rows[i]["CodeID"].ToString().Trim()));
                }
            }
            catch (Exception e)
            {
                //LogUtils._ErrorWirte(e);
            }
        }
    }
}
