using System;

namespace CC
{
    public class UnixUtility
    {
        /// <summary>
        /// 获取当前的时间戳(s)
        /// </summary>
        /// <returns></returns>
        public static int GetCurrentStamp()
        {
            return (int) ((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }

        /// <summary>
        /// 将时间转换成时间戳(s)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int ToTimeStamp(DateTime dateTime)
        {
            return (int) ((dateTime - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds);
        }

        /// <summary>
        /// 将时间戳转换为系统时间
        /// </summary>
        /// <param name="unixTimeStamp"></param>
        /// <returns></returns>
        public static DateTime UnixToDate(int unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(unixTimeStamp);
        }

        /// <summary>
        /// 比较是否是同一天
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <param name="anotherStamp"></param>
        /// <returns></returns>
        public static bool IsSameDay(int timeStamp, int anotherStamp)
        {
            return UnixToDate(timeStamp).Date.Equals(UnixToDate(anotherStamp).Date);
        }

        /// <summary>
        /// 是否是同一周
        /// </summary>
        public static bool IsSameWeek(DateTime time1, DateTime time2)
        {
            return time1.AddDays(-(int) time1.DayOfWeek).Date == time2.AddDays(-(int) time2.DayOfWeek).Date;
        }

        /// <summary>
        /// 获取某日某小时的时间戳(m)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="hour"></param>
        /// <returns></returns>
        public static int GetHourStartOfDate(DateTime dateTime, int hour)
        {
            return ToTimeStamp(new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, 0, 0));
        }
    }
}