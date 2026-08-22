using System;
using System.Net;
using System.Text.RegularExpressions;

namespace YiRongMachine
{
    internal class CommonHelper
    {
        /// <summary>
        /// 判断IP是否合法
        /// </summary>
        /// <param name="strIP">ip地址</param>
        /// <returns></returns>
        public static bool IsValidIP(string strIP, out IPAddress ip)
        {
            ip = null;
            if (IPAddress.TryParse(strIP, out ip))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断端口是否合法
        /// </summary>
        /// <param name="strPort"></param>
        /// <returns></returns>
        public static bool IsValidPort(string strPort)
        {
            if (!IsInt(strPort))
            {
                return false;
            }
            int iPort = Convert.ToInt32(strPort);
            if (iPort > 65535 || iPort < 1024)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 判断一个字符串是否是Int
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsInt(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*$");
        }
    }
}