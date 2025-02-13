using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.IO;


namespace MES.FW.Common.CommonMgt
{
    public class LogUtils
    {
        private static readonly string ErrorLog_Path = ConfigurationSettings.AppSettings["ERROR_PATH"].ToString();

        public LogUtils()
        {
            //
            // TODO: 생성자 논리를 여기에 추가합니다.
            //
        }

        /// <summary>
        /// 폴더 생성 및 팔일개체 생성
        /// </summary>
        /// <returns></returns>
        private static FileStream CreateFolder()
        {
            FileStream FS = null;

            string FolderNM = DateTime.Now.Year.ToString();
            string FileNM = FolderNM + DateTime.Now.Month.ToString() + "_ErrLog.txt";
            string FilePath = ErrorLog_Path + FolderNM + @"\" + FileNM;

            if (!Directory.Exists(ErrorLog_Path + FolderNM))
            {
                Directory.CreateDirectory(ErrorLog_Path + FolderNM);
            }

            FS = new FileStream(FilePath, FileMode.Append, FileAccess.Write);

            return FS;

        }


        /// <summary>
        /// Error Message Write (Oracle)
        /// </summary>
        /// <param name="perr"></param>
        public static void _DBErrorWirte(SqlException perr, string pDML)
        {
            FileStream FS = CreateFolder();

            using (StreamWriter SW = new StreamWriter(FS))
            {

                SW.WriteLine("★ SqlException Message ★");
                SW.WriteLine("날    짜  :  {0}", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
                SW.WriteLine("원    본  :  {0}", perr.TargetSite.Name.ToString());
                SW.WriteLine("오류내용  :  {0}", perr.Message.ToString());
                SW.WriteLine(perr.StackTrace.ToString());
                SW.WriteLine("DML : [{0}]", pDML.ToString());
                SW.WriteLine("--------------------------------------------------------------------------------------------");
                SW.WriteLine(" ");

                SW.Flush();
                SW.Close();

            }

            FS.Close();
        }

        /// <summary>
        /// Error Message Write
        /// </summary>
        /// <param name="perr"></param>
        public static void _ErrorWirte(Exception perr)
        {
            FileStream FS = CreateFolder();

            using (StreamWriter SW = new StreamWriter(FS))
            {

                SW.WriteLine("★ Exception Message ★");
                SW.WriteLine("날    짜  :  {0}", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
                SW.WriteLine("원    본  :  {0}", perr.TargetSite.Name.ToString());
                SW.WriteLine("오류내용  :  {0}", perr.Message.ToString());
                SW.WriteLine(perr.StackTrace.ToString());
                SW.WriteLine("--------------------------------------------------------------------------------------------");
                SW.WriteLine(" ");

                SW.Flush();
                SW.Close();

            }

            FS.Close();
        }


        /// <summary>
        /// Error Message Write
        /// </summary>
        /// <param name="perr"></param>
        public static void _ErrorWirte(System.Web.UI.Control ctl, string strMessage)
        {
            FileStream FS = CreateFolder();

            using (StreamWriter SW = new StreamWriter(FS))
            {

                SW.WriteLine("★ Exception Message ★");
                SW.WriteLine("날    짜  :  {0}", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
                SW.WriteLine("원    본  :  {0}", ctl.Page.ToString());
                SW.WriteLine("오류내용  :  {0}", "에러발생");
                SW.WriteLine(strMessage);
                SW.WriteLine("--------------------------------------------------------------------------------------------");
                SW.WriteLine(" ");

                SW.Flush();
                SW.Close();

            }

            FS.Close();
        }

    }
}
