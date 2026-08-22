using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace YiRongMachine
{
    public class IniHelper
    {
        #region ini文件处理

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        #endregion ini文件处理

        /// <summary>
        /// 从INI读取string类型值：四个参数分别为节，主键，默认值，完整路径
        /// </summary>
        /// <param name="section">节</param>
        /// <param name="key">主键</param>
        /// <param name="idefalut">默认值</param>
        /// <param name="iniPath">绝对路径</param>
        /// <returns>返回读取的值</returns>
        public static string IniReadString(string section, string key, string strDefault, string iniPath)
        {
           
           try
            {
                if (!File.Exists(iniPath))
                {
                    return strDefault;
                }

                System.Text.StringBuilder temp = new System.Text.StringBuilder(255);
                GetPrivateProfileString(section, key, strDefault, temp, 255, iniPath);
                string strTemp = temp.ToString().Trim();
                int n = strTemp.IndexOf(';');
                string strValue = "";
                if (n > -1)
                {
                    strValue = strTemp.Substring(0, n).Trim();
                }
                else
                {
                    strValue = strTemp;
                }
                return strValue;
            }
            catch (Exception ex)
            {
                LogHelper.AddCommLog(ex.Message.ToString());
                return strDefault;
            }
        }

        /// <summary>
        /// 从INI读取Int类型值：四个参数分别为节，主键，默认值，完整路径
        /// </summary>
        /// <param name="section">字段</param>
        /// <param name="key">主键</param>
        /// <param name="idefalut">默认值</param>
        /// <param name="iniPath">绝对路径</param>
        /// <returns>返回读取的值</returns>
        public static int IniReadInt(string section, string key, int iDefault, string iniPath)
        {
            string strDefalut = Convert.ToString(iDefault);
            string strReturn = IniReadString(section, key, strDefalut, iniPath);
            int iValue = iDefault;
            try
            {
                iValue = Convert.ToInt32(strReturn);
            }
            catch (Exception ex)
            {
                LogHelper.AddCommLog(ex.Message.ToString());
                MessageBox.Show(ex.Message.ToString());
            }
            return iValue;
        }

        /// <summary>
        /// 从INI读取double类型值：四个参数分别为节，主键，默认值，完整路径
        /// </summary>
        /// <param name="section">字段</param>
        /// <param name="key">主键</param>
        /// <param name="idefalut">默认值</param>
        /// <param name="iniPath">绝对路径</param>
        /// <returns>返回读取的值</returns>
        public static double IniReadDouble(string section, string key, double dDefault, string iniPath)
        {
            string strDefalut = Convert.ToString(dDefault);
            string strReturn = IniReadString(section, key, strDefalut, iniPath);
            double dValue = dDefault;
            try
            {
                dValue = Convert.ToDouble(strReturn);
            }
            catch (Exception ex)
            {
                LogHelper.AddCommLog(ex.Message.ToString());
                MessageBox.Show(ex.Message.ToString());
            }
            return dValue;
        }

        /// <summary>
        /// 从INI读取bool类型值：四个参数分别为节，主键，默认值，完整路径
        /// </summary>
        /// <param name="section">字段</param>
        /// <param name="key">主键</param>
        /// <param name="idefalut">默认值</param>
        /// <param name="iniPath">绝对路径</param>
        /// <returns>返回bool类型的值</returns>
        public static bool IniReadBoolean(string section, string key, bool bDefault, string iniPath)
        {
            string strDefault = "FALSE";
            if (bDefault)
                strDefault = "TRUE";
            string strReturn = IniReadString(section, key, strDefault, iniPath);
            bool bValue;
            if (strReturn.ToString().ToUpper() == "TRUE" || strReturn.ToString().ToUpper() == "YES")
            {
                bValue = true;
            }
            else
            {
                bValue = false;
            }
            return bValue;
        }

        /// <summary>
        /// 写入string类型内容：四个参数分别为节，主键，写入的值，完整路径
        /// </summary>
        /// <param name="section">字段</param>
        /// <param name="key">主键</param>
        /// <param name="idefalut">写入的值</param>
        /// <param name="iniPath">绝对路径</param>
        /// <returns></returns>
        public static void IniWriteString(string section, string key, string value, string iniPath)
        {
            WritePrivateProfileString(section, key, value, iniPath);
        }

        /// <summary>
        /// 写入Int类型内容：四个参数分别为节，主键，写入的值，完整路径
        /// </summary>
        /// <param name="section">字段</param>
        /// <param name="key">主键</param>
        /// <param name="idefalut">写入的值</param>
        /// <param name="iniPath">绝对路径</param>
        /// <returns></returns>
        public static void IniWriteInt(string section, string key, int value, string iniPath)
        {
            string data = Convert.ToString(value);
            WritePrivateProfileString(section, key, data, iniPath);
        }

        /// <summary>
        /// 写入double类型内容：四个参数分别为节，主键，写入的值，完整路径
        /// </summary>
        /// <param name="section">字段</param>
        /// <param name="key">主键</param>
        /// <param name="idefalut">写入的值</param>
        /// <param name="iniPath">绝对路径</param>
        /// <returns></returns>
        public static void IniWriteDouble(string section, string key, double value, string iniPath)
        {
            string data = Convert.ToString(value);
            WritePrivateProfileString(section, key, data, iniPath);
        }

        /// <summary>
        /// 写入INI文件bool类型内容：四个参数分别为节，主键，写入的值，完整路径
        /// </summary>
        /// <param name="section">字段</param>
        /// <param name="key">主键</param>
        /// <param name="idefalut">写入的值</param>
        /// <param name="iniPath">绝对路径</param>
        /// <returns></returns>
        public static void IniWriteBoolean(string section, string key, bool value, string iniPath)
        {
            string data = "FALSE";
            if (value)
            {
                data = "TRUE";
            }
            WritePrivateProfileString(section, key, data, iniPath);
        }
    }
}