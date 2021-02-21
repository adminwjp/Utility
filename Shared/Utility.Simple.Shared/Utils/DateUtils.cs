#if !(NETCOREAPP1_0 || NETCOREAPP1_1 || NETCOREAPP1_2 || NETSTANDARD1_0 || NETSTANDARD1_1 || NETSTANDARD1_2 || NETSTANDARD1_3 || NETSTANDARD1_4 || NETSTANDARD1_5 || NETSTANDARD1_6)
using System;

namespace Utility.Utils
{
    /// <summary>时间戳 公共类</summary>
    public  class DateUtils
    {
        #region 时间戳帮助类

        /// <summary>
        /// 1970, 1, 1, 0, 0, 0, 0 时间
        /// </summary>
        /// <returns></returns>
        public static DateTime ToLocalTime()
        {
#pragma warning disable CS0618 // 类型或成员已过时
            DateTime dt = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0, 0));//将指定的时间转换为本地时间
#pragma warning restore CS0618 // 类型或成员已过时
            return dt;
        }

        ///<sumary>将时间戳转为本地时间</sumary>
        ///<param name="timespan">时间戳</param>
        /// <returns></returns>
        public static DateTime AddTimeSpan(long timespan)
        {
            DateTime dt = ToLocalTime();
            TimeSpan tp = new TimeSpan(timespan * 10000);
            return dt.Add(tp);
        }

        ///<sumary>将本地时间转为时间戳</sumary>
        /// <param name="currentDate">本地时间</param>
        /// <returns></returns>
        public static long TotalMilliseconds(DateTime currentDate)
        {
            DateTime dt = ToLocalTime();
            return (long)(currentDate - dt).TotalMilliseconds;
        }

        ///<sumary>将时间戳(秒)转为本地时间</sumary>
        /// <param name="second">时间戳(秒)</param>
        /// <returns></returns>
        public static DateTime AddSecond(long second)
        {
            return ToLocalTime().Add(new TimeSpan(second * 10000000));
        }

        ///<sumary>将本地时间转为时间戳(秒)</sumary>
        /// <param name="currentDt">本地时间</param>
        /// <returns></returns>
        public static long TotalSeconds(DateTime currentDt)
        {
            return (long)(currentDt - ToLocalTime()).TotalSeconds;
        }

        /// <summary> 获取时间戳 </summary>
        /// <param name="dt">时间</param>
        /// <returns></returns>
        public static long TotalMilliseconds(DateTime? dt = null)
        {
            return Convert.ToInt64((( dt ?? DateTime.Now)  - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds);
        }

        /// <summary>获取时间戳 </summary>
        /// <param name="seconds">秒</param>
        /// <param name="date">时间</param>
        /// <returns></returns>
        public static long TotalMilliseconds(int seconds, DateTime? date = null)
        {
            return Convert.ToInt64((( date ?? DateTime.Now) - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalMilliseconds) + seconds * 1000;
        }

        /// <summary> 获取时间格式 </summary>
        /// <param name="dt">时间</param>
        /// <param name="format">格式化 yyyy-MM-dd hh:mm:ss</param>
        /// <returns></returns>
        public static string DateFormat(DateTime? dt = null, string format = "yyyy-MM-dd hh:mm:ss")
        {
            return ( dt ?? DateTime.Now).ToString(format);
        }

        /// <summary>
        /// 时间格式 化
        /// </summary>
        /// <param name="format">yyyy-MM-dd-hh-mm-ss</param>
        /// <returns></returns>
        public static string DateString(string format = "yyyy-MM-dd-hh-mm-ss")
        {
            return DateTime.Now.ToString(format);
        }
        #endregion 时间戳帮助类




    }
}
#endif