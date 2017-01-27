using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace WindowsFormsApplication2
{
    class Util
    {
        public static string GetMD5Value(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return null;
            }
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return Util.GetMD5Value(bytes);
        }

        public static string GetMD5Value(byte[] bytes)
        {
            if (bytes == null || bytes.Length.Equals(0))
            {
                return null;
            }
            MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
            byte[] array = mD5CryptoServiceProvider.ComputeHash(bytes);
            StringBuilder stringBuilder = new StringBuilder();
            byte[] array2 = array;
            for (int i = 0; i < array2.Length; i++)
            {
                byte b = array2[i];
                stringBuilder.Append(b.ToString("x2"));
            }
            return stringBuilder.ToString();
        }
    }
}
