using System;
using System.Data;
using System.IO;
using System.Runtime.InteropServices;

namespace HQCWeb.FMB_FW
{
    public class ExternUtil
    {
        [DllImport("User32.dll")]
        public static extern int FindWindow(string opClassName, string lpWindowName);
        [DllImport("User32.dll")]
        public static extern bool ShowWindow(int nHwnd, int nCmdShow);
        [DllImport("User32.dll")]
        public static extern bool IsWindowVisible(int nHwnd);
        [DllImport("User32.dll")]
        public static extern bool BringWindowToTop(int nHwnd);
        [DllImport("Kernel32.dll")]
        public static extern bool CreateDirectory(string lpPathName, ExternUtil.SECURITY_ATTRIBUTES lpAttributes);
        [DllImport("Kernel32.dll")]
        public static extern bool CopyFile(string lpExistingFileName, string lpNewFileNmae, bool bFailIfExists);
        [DllImport("Kernel32.dll")]
        public static extern bool DeleteFile(string lpFileNmae);
        [DllImport("MSVCRT.dll")]
        public static extern int strtoul(string nptr, string endptr, int p_nBase);

        public static void MakeXML(string fullPath, DataSet ds)
        {
            string path = fullPath;
            FileStream fileStream = (FileStream)null;
            StreamWriter streamWriter = (StreamWriter)null;
            try
            {
                if(File.Exists(path))
                {
                    if(Convert.ToInt32((DateTime.Now - File.GetLastWriteTime(path)).TotalSeconds) < 30)
                    {
                        return;
                    }
                }
                else
                {
                    new System.IO.DirectoryInfo(path.Substring(0,path.LastIndexOf("\\"))).Create();
                }
                fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                streamWriter = new StreamWriter((Stream)fileStream, System.Text.Encoding.UTF8);
                streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                streamWriter.Write(ds.GetXml());
                streamWriter.Flush();
                fileStream.Flush();
            }
            finally
            {
                streamWriter?.Close();
                fileStream?.Close();
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public class SECURITY_ATTRIBUTES
        {
            public int nLength;
            public object lpSecurityDescriptor;
            public bool bInheritHandle;
            public SECURITY_ATTRIBUTES()
            {
                this.nLength = Marshal.SizeOf(typeof(ExternUtil.SECURITY_ATTRIBUTES));
                this.lpSecurityDescriptor = (object)null;
                this.bInheritHandle = false;
            }
        }


    }
}