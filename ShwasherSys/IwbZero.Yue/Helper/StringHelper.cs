using System;

namespace IwbZero.Helper
{
    public static class StringHelper
    {

        /// <summary>
        /// 检查空字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 检查是否以某字符串开头，不是加上此字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="startStr"></param>
        /// <returns></returns>
        public static string Sw(this string str,string startStr)
        {
            return str.StartsWith(startStr) ? str : $"{startStr}{str}";
        }

        /// <summary>
        /// 检查是否以某字符串结尾，不是加上此字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="endStr"></param>
        /// <returns></returns>
        public static string Ew(this string str,string endStr)
        {
            return str.EndsWith(endStr) ? str : $"{str}{endStr}";
        }

        public static string LeftPad(this int i, int num,string charStr="0")
        {
            return i.ToString().PadLeft(num, Convert.ToChar(charStr));
        }
        public static string RightPad(this int i, int num,string charStr="0")
        {
            return i.ToString().PadRight(num, Convert.ToChar(charStr));
        }

        public static string LeftPad(this string s, int num,string charStr="0")
        {
            return s.PadLeft(num, Convert.ToChar(charStr));
        }
        public static string RightPad(this string s, int num,string charStr="0")
        {
            return s.PadRight(num, Convert.ToChar(charStr));
        }

        #region DateTime

        /// <summary>
        /// 生成周期起始时间
        /// </summary>
        /// <param name="year"></param>
        /// <param name="type">1-12:月份 13-16:季度 17:上半年 18:下半年 other:整年</param>
        /// <returns></returns>
        public static DateTime GetDateByType(this int year, int? type,out DateTime endDate,out string dateStr)
        {
            DateTime startDate;
            //生成周期起始时间
            if (type == null)
            {
                startDate= new DateTime(year,1,1);
                endDate = startDate.AddYears(1);
                dateStr = $"{year}年度";
            }
            else if(type>12)
            {
                switch (type)
                {
                    //第一季度
                    case 13:
                        startDate= new DateTime(year,1,1);
                        endDate = new DateTime(year,4,1);
                        dateStr = $"{year}年一季度";
                        break;
                    //第二季度
                    case 14:
                        startDate= new DateTime(year,4,1);
                        endDate = new DateTime(year,7,1);
                        dateStr = $"{year}年二季度";
                        break;
                    //第三季度
                    case 15:
                        startDate= new DateTime(year,7,1);
                        endDate = new DateTime(year,10,1);
                        dateStr = $"{year}年三季度";
                        break;
                    //第四季度
                    case 16:
                        startDate= new DateTime(year,10,1);
                        endDate = new DateTime(year+1,1,1);
                        dateStr = $"{year}年四季度";
                        break;
                    //上半年
                    case 17:
                        startDate= new DateTime(year,1,1);
                        endDate = new DateTime(year,7,1);
                        dateStr = $"{year}年上半年";
                        break;
                    //下半年
                    case 18:
                        startDate= new DateTime(year,7,1);
                        endDate = new DateTime(year+1,1,1);
                        dateStr = $"{year}年下半年";
                        break;
                    //整年
                    default:
                        startDate= new DateTime(year,1,1);
                        endDate = startDate.AddYears(1);
                        dateStr = $"{year}年度";
                        break;
                }
            }
            else 
            {
                startDate= new DateTime(year,type??0,1);
                endDate = startDate.AddMonths(1);
                dateStr = $"{year}年{type}月";
            }

            return startDate;
        }


        /// <summary>
        /// 时间差（分）
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static int GetTimeSpanMinute(this DateTime start, DateTime end)
        {
            return Convert.ToInt32(GetTimeSpan(start, end).TotalMinutes);
        }
        /// <summary>
        /// 时间差
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static TimeSpan GetTimeSpan(this DateTime start, DateTime end)
        {
            TimeSpan timeSpan = end - start;
            return timeSpan;
        } 
        #endregion
    }
}
