using System;
using System.Text;

namespace ShwasherSys
{
    public static class Base64String
    {
        /// <summary>
        /// Base64 编码
        /// </summary>
        /// <param name="encode">编码方式</param>
        /// <param name="source">要编码的字符串</param>
        /// <returns>返回编码后的字符串</returns>
        public static string EncodeBase64(this string source, Encoding encode = null)
        {
            string result;
            try
            {
                encode = encode ?? Encoding.UTF8;
                byte[] bytes = encode.GetBytes(source);
                result = Convert.ToBase64String(bytes);
            }
            catch
            {
                result = source;
            }
            return result;
        }


        /// <summary>
        /// Base64 解码
        /// </summary>
        /// <param name="encode">解码方式</param>
        /// <param name="source">要解码的字符串</param>
        /// <returns>返回解码后的字符串</returns>
        public static string DecodeBase64(this string source, Encoding encode = null)
        {
            string result;
            try
            {
                byte[] bytes = Convert.FromBase64String(source);
                encode = encode ?? Encoding.UTF8;
                result = encode.GetString(bytes);
            }
            catch
            {
                result = source;
            }
            return result;
        }

    }
}
