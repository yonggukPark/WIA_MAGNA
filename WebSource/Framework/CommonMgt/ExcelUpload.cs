using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.IO;
using System.Data.OleDb;

namespace MES.FW.Common.CommonMgt
{
    public class ExcelUpload : IDisposable
    {
        #region 선언부

        private OleDbConnection m_ConnectionToExcelBook;
        private OleDbDataAdapter m_AdapterForExcelBook;

        private string sStartPoint = string.Empty;
        private string sEndPoint = string.Empty;
        private string sFileName = string.Empty;
        private string sFileUrl = string.Empty;

        #endregion

        #region 속성부

        public string _FILENAME
        {
            get
            {
                return sFileName;
            }
        }

        public string _START_POINT
        {
            get
            {
                return sStartPoint;
            }
            set
            {
                sStartPoint = value;
            }
        }

        public string _END_POINT
        {
            get
            {
                return sEndPoint;
            }
            set
            {
                sEndPoint = value;
            }
        }

        #endregion

        #region Loading

        public ExcelUpload()
        {
        }

        #endregion

        #region Function

        public DataTable ExcelFile_DataTable(FileUpload F_Upload, string strUrl, int SheetCnt, bool AutoClose, out string sMsg)
        {
            sMsg = string.Empty;
            DataTable Excel_DT = null;
            string sExcelFileName = string.Empty;
            try
            {
                sExcelFileName = saveFile(F_Upload, strUrl);
                sFileName = sExcelFileName;
                sFileUrl = strUrl;
                bool bReturn = openConnection(sExcelFileName, strUrl, out sMsg);
                
                if (!bReturn)
                {
                    return null;
                }
                else
                {
                    Excel_DT = ReadExcelFile(ReadExcelSheet(SheetCnt), sStartPoint, sEndPoint, out sMsg);

                    if (AutoClose)
                    {
                        closeConnection(out sMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                sMsg = ex.Message.ToString();
            }

            return Excel_DT;
        }

        public DataTable ExcelFile_DataTable(FileUpload F_Upload, string strUrl, int SheetCnt, bool AutoClose, out string sSheetName, out string sMsg)
        {
            sMsg = string.Empty;
            DataTable Excel_DT = null;
            string sExcelFileName = string.Empty;
            sSheetName = string.Empty;
            try
            {
                sExcelFileName = saveFile(F_Upload, strUrl);
                sFileName = sExcelFileName;
                sFileUrl = strUrl;
                bool bReturn = openConnection(sExcelFileName, strUrl, out sMsg);

                if (!bReturn)
                {
                    return null;
                }
                else
                {
                    sSheetName = ReadExcelSheet(SheetCnt);
                    Excel_DT = ReadExcelFile(ReadExcelSheet(SheetCnt), sStartPoint, sEndPoint, out sMsg);

                    if (AutoClose)
                    {
                        closeConnection(out sMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                sMsg = ex.Message.ToString();
            }

            return Excel_DT;
        }

        public DataTable ExcelFile_DataTable(FileUpload F_Upload, string strUrl, string SheetName, bool AutoClose, out string sMsg)
        {
            sMsg = string.Empty;
            DataTable Excel_DT = null;
            string sExcelFileName = string.Empty;
            try
            {
                sExcelFileName = saveFile(F_Upload, strUrl);
                sFileName = sExcelFileName;
                sFileUrl = strUrl;
                bool bReturn = openConnection(sExcelFileName, strUrl, out sMsg);

                if (!bReturn)
                {
                    return null;
                }
                else
                {
                    Excel_DT = ReadExcelFile(SheetName + "$", sStartPoint, sEndPoint, out sMsg);
                    if (AutoClose)
                    {
                        closeConnection(out sMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                sMsg = ex.Message.ToString();
            }

            return Excel_DT;
        }

        public DataTable ExcelFile_DataTable(int SheetCnt, bool AutoClose, out string sMsg)
        {
            sMsg = string.Empty;
            DataTable Excel_DT = null;

            try
            {
                bool bReturn = ConnectionStateCheck(out sMsg);

                if (!bReturn)
                {
                    return null;
                }
                else
                {
                    Excel_DT = ReadExcelFile(ReadExcelSheet(SheetCnt), sStartPoint, sEndPoint, out sMsg);
                    if (AutoClose)
                    {
                        closeConnection(out sMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                sMsg = ex.Message.ToString();
            }

            return Excel_DT;
        }

        public DataTable ExcelFile_DataTable(string SheetName, bool AutoClose, out string sMsg)
        {
            sMsg = string.Empty;
            DataTable Excel_DT = null;

            try
            {
                bool bReturn = ConnectionStateCheck(out sMsg);

                if (!bReturn)
                {
                    return null;
                }
                else
                {
                    Excel_DT = ReadExcelFile(SheetName + "$", sStartPoint, sEndPoint, out sMsg);
                    if (AutoClose)
                    {
                        closeConnection(out sMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                sMsg = ex.Message.ToString();
            }

            return Excel_DT;
        }

        public string saveFile(FileUpload F_Upload, string strUrl, bool DeleteFileGubun)
        {
            DirectoryInfo dInfo = new DirectoryInfo(strUrl);
            if (!dInfo.Exists)
            {
                dInfo.Create();
            }

            if (DeleteFileGubun)
            {
                DeleteFile(strUrl);
            }

            FileInfo fInfo = new FileInfo(strUrl + F_Upload.FileName);

            string fExtension = fInfo.Extension;
            string fRealName = F_Upload.FileName.Replace(fExtension, "");

            string fileName = string.Format("{0}{1}"
                                            , fRealName
                                            , fExtension
                                            );

            string fileFullName = string.Format("{0}{1}"
                                            , strUrl
                                            , fileName
                                            );

            fInfo = new FileInfo(fileFullName);

            if (!fInfo.Exists)
            {
                F_Upload.PostedFile.SaveAs(fileFullName);

                return fileName;
            }
            else
            {
                return string.Empty;
            }
        }

        private string saveFile(FileUpload F_Upload, string strUrl)
        {
            DirectoryInfo dInfo = new DirectoryInfo(strUrl);
            if (!dInfo.Exists)
            {
                dInfo.Create();
            }

            DeleteFile(strUrl);

            FileInfo fInfo = new FileInfo(strUrl + F_Upload.FileName);

            string fExtension = fInfo.Extension;
            string fRealName = F_Upload.FileName.Replace(fExtension, "");

            string fileName = string.Format("{0}_{1}{2}"
                                            , fRealName
                                            , DateTime.Now.ToString("yyyyMMddHHmmss")
                                            , fExtension
                                            );

            string fileFullName = string.Format("{0}{1}"
                                            , strUrl
                                            , fileName
                                            );

            fInfo = new FileInfo(fileFullName);

            if (!fInfo.Exists)
            {
                F_Upload.PostedFile.SaveAs(fileFullName);

                return fileName;
            }
            else
            {
                return string.Empty;
            }
        }

        public bool openConnection(string ExcelFileName, string strUrl, out string sMsg)
        {
            string[] str = strUrl.Split(':');
            string sExcelPathName = strUrl + ExcelFileName;
            sMsg = string.Empty;

            try
            {
                if (ExcelFileType(sExcelPathName) == 0)
                {
                    m_ConnectionToExcelBook = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + sExcelPathName + ";Extended Properties=\"Excel 8.0;HDR=NO;\"");
                }
                else if (ExcelFileType(sExcelPathName) == 1)
                {
                    m_ConnectionToExcelBook = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + sExcelPathName + ";Extended Properties=\"Excel 12.0;HDR=NO;\"");
                }
                else
                {
                    return false;
                }

                if (m_ConnectionToExcelBook.State != ConnectionState.Open)
                {
                    m_ConnectionToExcelBook.Open();
                }

            }
            catch (Exception ex)
            {
                sMsg = "ERROR : " + ex.Message;
                return false;
            }

            return true;
        }

        public bool closeConnection(out string sMsg)
        {
            sMsg = string.Empty;
            try
            {
                if (m_ConnectionToExcelBook.State == ConnectionState.Open)
                {
                    m_ConnectionToExcelBook.Close();
                }
            }
            catch (Exception ex)
            {
                sMsg = "ERROR " + ex.Message;
                return false;
            }

            return true;
        }

        private bool ConnectionStateCheck(out string sMsg)
        {
            sMsg = string.Empty;
            try
            {
                if (m_ConnectionToExcelBook.State == ConnectionState.Open)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                sMsg = "ERROR " + ex.Message;
                return false;
            }
        }

        private string ReadExcelSheet(int iSheetCnt)
        {
            DataTable dtSheet = new DataTable();
            string SheetName = string.Empty;

            int iSheetCount = 0;

            dtSheet = m_ConnectionToExcelBook.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

            if (dtSheet == null)
            {
                SheetName = "$Sheet1";
            }
            else
            {
                for (int iIndex = 0; iIndex < dtSheet.Rows.Count; iIndex++)
                {
                    if (dtSheet.Rows[iIndex]["TABLE_NAME"].ToString().Substring(dtSheet.Rows[iIndex]["TABLE_NAME"].ToString().Length - 1, 1) == "$")
                    {
                        iSheetCount++;

                        if (iSheetCount == iSheetCnt)
                        {
                            SheetName = dtSheet.Rows[iIndex]["TABLE_NAME"].ToString();
                            break;
                        }
                    }
                }
            }

            return SheetName;
        }

        private DataTable ReadExcelFile(string SheetName, string StartPoint, string EndPoint, out string sMsg)
        {
            DataTable result = new DataTable();

            if (StartPoint == string.Empty || EndPoint == string.Empty)
            {
                result = readForSpecificQuery("select * from  [" + SheetName + "]", out sMsg);
            }
            else
            {
                result = readForSpecificQuery("select * from  [" + SheetName + StartPoint + ":" + EndPoint + "]", out sMsg);
            }

            return result;
        }

        private DataTable readForSpecificQuery(string iQuery, out string sMsg)
        {
            try
            {
                DataTable returnDataObject = new DataTable();

                OleDbCommand selectCommand = new OleDbCommand(iQuery);
                selectCommand.Connection = this.m_ConnectionToExcelBook;
                this.m_AdapterForExcelBook = new OleDbDataAdapter();

                this.m_AdapterForExcelBook.SelectCommand = selectCommand;
                this.m_AdapterForExcelBook.Fill(returnDataObject);

                sMsg = "SUCCESS - " + returnDataObject.Rows.Count + " Records Loaded ";

                return returnDataObject;
            }
            catch (Exception ex)
            {
                sMsg = "ERROR " + ex.Message;
                return null;
            }
        }

        private int ExcelFileType(string UrlFullFileName)
        {
            byte[,] ExcelHeader = {
                { 0xD0, 0xCF, 0x11, 0xE0, 0xA1 }, // XLS  File Header
                { 0x50, 0x4B, 0x03, 0x04, 0x14 }  // XLSX File Header
            };

            // result -2=error, -1=not excel , 0=xls , 1=xlsx
            int iReturn_Result = -1;

            FileInfo F_Info = new FileInfo(UrlFullFileName);
            FileStream F_Stream = F_Info.Open(FileMode.Open);

            try
            {
                byte[] FileHeader = new byte[5];

                F_Stream.Read(FileHeader, 0, 5);

                for (int iRow = 0; iRow < 2; iRow++)
                {
                    for (int iCol = 0; iCol < 5; iCol++)
                    {
                        if (FileHeader[iCol] != ExcelHeader[iRow, iCol])
                        {
                            break;
                        }
                        else if (iCol == 4)
                        {
                            iReturn_Result = iRow;
                        }
                    }
                    if (iReturn_Result >= 0) break;
                }
            }
            catch
            {
                iReturn_Result = (-2);
            }
            finally
            {
                F_Stream.Close();
            }
            return iReturn_Result;
        }

        private void DeleteFile(string strUrl)
        {
            try
            {
                string[] dirs = Directory.GetFiles(strUrl, "*.*");
                DateTime dDelTime = DateTime.Now;
                foreach (string dir in dirs)
                {
                    try
                    {
                        DateTime dFileTime = File.GetLastWriteTime(dir);

                        double nGapDay = Math.Round(((TimeSpan)(dDelTime - dFileTime)).TotalDays);
                        if (nGapDay > 30)
                        {
                            File.Delete(dir);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            catch
            {
            }
        }

        #endregion

        #region IDisposable 멤버

        public void Dispose()
        {
            m_ConnectionToExcelBook = null;
            m_AdapterForExcelBook = null;
        }

        #endregion
    }
}
