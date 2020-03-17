using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aida64_Esp8266_DisplayControler
{
    public class MD5Helper
    {
        /// <summary>
        /// 计算字节数组的 MD5 值
        /// </summary>
        /// <param name="buffer"></param>
        /// <returns></returns>
        public static string CalcMD5(byte[] buffer)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] md5Bytes = md5.ComputeHash(buffer);
                return BytesToString(md5Bytes);
            }
        }

        /// <summary>
        /// 将得到的 MD5 字节数组转成 字符串
        /// </summary>
        /// <param name="md5Bytes"></param>
        /// <returns></returns>
        private static string BytesToString(byte[] md5Bytes)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < md5Bytes.Length; i++)
            {
                sb.Append(md5Bytes[i].ToString("X2"));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 计算字符串的 MD5 值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string CalcMD5(string str)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(str);
            return CalcMD5(buffer);
        }

        /// <summary>
        /// 计算流的 MD5 值
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string CalcMD5(Stream stream)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] buffer = md5.ComputeHash(stream);
                return BytesToString(buffer);
            }
        }
    }
}