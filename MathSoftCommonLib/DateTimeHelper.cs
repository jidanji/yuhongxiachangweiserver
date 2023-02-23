using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MathSoftCommonLib
{
    #region 时间类模型
    public class DateTimePare
    {
        #region 开始时间
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime BegTime { get; set; }
        #endregion

        #region 结束时间
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        #endregion

        #region 是否符合基本要求
        /// <summary>
        /// 是否符合基本要求
        /// </summary>
        /// <returns></returns>
        public bool IsValid()
        {
            return BegTime < EndTime;
        }
        #endregion

        #region 判断现在是否在区间之内
        /// <summary>
        /// 判断现在是否在区间之内
        /// </summary>
        /// <returns></returns>
        public bool NowInArea()
        {
            var now = DateTime.Now;
            return IsValid() && BegTime <= now && EndTime >= now;
        }
        #endregion


    }
    #endregion

    #region 批量时间段的处理类
    /// <summary>
    /// 批量时间段的处理类
    /// </summary>
    public class DateTimeHelper
    {
        /// <summary>
        /// 判断当前时间是否在一组时间内
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool NowInArray(List<DateTimePare> list)
        {
            bool ret = false;
            if (list == null)
            {
                return false;
            }
            if (list.Count == 0)
            {
                return false;
            }

            foreach (var item in list)
            {
                ret = ret || item.NowInArea();
            }

            return ret;
        }

        #region 时间戳处理方法集合
        /// <summary>  
        /// 获取时间戳  13位
        /// </summary>  
        /// <returns></returns>  
        public static long GetTimeStamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds * 1000);
        }
        /// <summary>
        /// 将时间戳转换为日期类型，并格式化
        /// </summary>
        /// <param name="longDateTime"></param>
        /// <returns></returns>
        private static string LongDateTimeToDateTimeString(string longDateTime)
        {
            //用来格式化long类型时间的,声明的变量
            long unixDate;
            DateTime start;
            DateTime date;
            //ENd

            unixDate = long.Parse(longDateTime);
            start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            date = start.AddMilliseconds(unixDate).ToLocalTime();

            return date.ToString("yyyy-MM-dd HH:mm:ss");

        }
        #endregion
    }
    #endregion
}
