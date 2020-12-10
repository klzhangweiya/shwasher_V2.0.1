using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShwasherSys
{
    public static class DataTimeHelper
    {
        public static int GetTimeSpanMinute(this DateTime start, DateTime end)
        {
            return Convert.ToInt32(GetTimeSpan(start, end).TotalMinutes);
        }

        public static TimeSpan GetTimeSpan(this DateTime start, DateTime end)
        {
            TimeSpan timeSpan = end - start;
            return timeSpan;
        }
        /// <summary>
        /// 字符串转换成日期格式
        /// </summary>
        /// <param name="pcStr"></param>
        /// <returns></returns>
        public static DateTime StrToDt(string pcStr)
        {
            DateTime time1 = new DateTime(1900, 1, 1);
            try
            {
                return DateTime.Parse(pcStr);
            }
            catch (Exception)
            {
                // ignored
            }

            return new DateTime(1900, 1, 1);
        }
    }
}
