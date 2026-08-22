using System;

namespace YiRongMachine
{
    internal class TimeHelper
    {
        /// <summary>
        /// 设定开始时间和结束时间，得到经过的秒数
        /// </summary>
        /// <param name="dtBegin"></param>
        /// <param name="dtEnd"></param>
        /// <returns></returns>
        public static double TimeSpanSecods(DateTime dtBegin, DateTime dtEnd)
        {
            TimeSpan tsBegin = new TimeSpan(dtBegin.Ticks);
            TimeSpan tsEnd = new TimeSpan(dtEnd.Ticks);
            TimeSpan tsSpan = tsBegin.Subtract(tsEnd).Duration();
            return tsSpan.TotalSeconds;
        }
    }
}