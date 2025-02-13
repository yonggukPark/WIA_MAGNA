using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HQCWeb.FMB_FW.Utils
{
    public static class CommonExtensionMethod
    {
        public static string ToNullSafeString(this Object obj)
        {
            if (obj == null)
            {
                return string.Empty;
            }
            else if (obj == DBNull.Value)
            {
                return string.Empty;
            }
            else
            {
                return obj.ToString();
            }
        }
        public static bool IsNullOrEmpty(this Object obj)
        {
            if (obj == null)
            {
                return true;
            }
            else if (obj.ToNullSafeString() == string.Empty)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public class MD5Crypto
        {
            internal static string _key = "http://www.u-tech.com";

            public static string Encrypt(string plainText)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(plainText);
                MD5CryptoServiceProvider cryptoServiceProvider1 = new MD5CryptoServiceProvider();
                byte[] hash = cryptoServiceProvider1.ComputeHash(Encoding.UTF8.GetBytes(MD5Crypto._key));
                cryptoServiceProvider1.Clear();
                TripleDESCryptoServiceProvider cryptoServiceProvider2 = new TripleDESCryptoServiceProvider();
                cryptoServiceProvider2.Key = hash;
                cryptoServiceProvider2.Mode = CipherMode.ECB;
                cryptoServiceProvider2.Padding = PaddingMode.PKCS7;
                byte[] inArray = cryptoServiceProvider2.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
                cryptoServiceProvider2.Clear();
                return Convert.ToBase64String(inArray, 0, inArray.Length);
            }

            public static string Decrypt(string cipherString)
            {
                byte[] inputBuffer = Convert.FromBase64String(cipherString);
                MD5CryptoServiceProvider cryptoServiceProvider1 = new MD5CryptoServiceProvider();
                byte[] hash = cryptoServiceProvider1.ComputeHash(Encoding.UTF8.GetBytes(MD5Crypto._key));
                cryptoServiceProvider1.Clear();
                TripleDESCryptoServiceProvider cryptoServiceProvider2 = new TripleDESCryptoServiceProvider();
                cryptoServiceProvider2.Key = hash;
                cryptoServiceProvider2.Mode = CipherMode.ECB;
                cryptoServiceProvider2.Padding = PaddingMode.PKCS7;
                byte[] bytes = cryptoServiceProvider2.CreateDecryptor().TransformFinalBlock(inputBuffer, 0, inputBuffer.Length);
                cryptoServiceProvider2.Clear();
                return Encoding.UTF8.GetString(bytes);
            }
        }
        public static int ToNullSafeInt(this Object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else if (obj.ToNullSafeString() == string.Empty)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj.ToNullSafeDouble());
            }
        }

        public static double ToNullSafeDouble(this Object obj)
        {
            if (obj == null)
            {
                return 0;
            }
            else if (obj.ToNullSafeString() == string.Empty)
            {
                return 0;
            }
            else
            {
                return Convert.ToDouble(obj.ToString());
            }
        }


    }
}
