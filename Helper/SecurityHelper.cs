using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace YiRongMachine
{
    public class SecurityHelper
    {
        private const string security = "EDCBYXWVUTSRQPONMMKJIHGFA";

        /// <summary>
        /// 传入的data是时间，例如2022-06-21
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetSecurity(string date)
        {
            string data = "";
            byte[] buffer = Encoding.ASCII.GetBytes(date);
            for (int i = 0; i < buffer.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                data = data + buffer[i].ToString("X");
            }

            string msg = "";
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] == 'A')
                {
                    msg += security[10];
                }
                else if (data[i] == 'B')
                {
                    msg += security[11];
                }
                else if (data[i] == 'C')
                {
                    msg += security[12];
                }
                else if (data[i] == 'D')
                {
                    msg += security[13];
                }
                else if (data[i] == 'E')
                {
                    msg += security[14];
                }
                else if (data[i] == 'F')
                {
                    msg += security[15];
                }
                else
                {
                    int number = Convert.ToInt32(data[i].ToString());
                    msg += security[number];
                }
            }
            return msg;
        }

        /// <summary>
        /// 传入的是加密过的数据，传出的是时间2022-06-21
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static string FromSecurity(string msg)
        {
            string data = "";
            for (int i = 0; i < msg.Length; i++)
            {
                for (int j = 0; j < security.Length; j++)
                {
                    if (msg[i] == security[j])
                    {
                        if (j == 10)
                        {
                            data += 'A';
                        }
                        else if (j == 11)
                        {
                            data += 'B';
                        }
                        else if (j == 12)
                        {
                            data += 'C';
                        }
                        else if (j == 13)
                        {
                            data += 'D';
                        }
                        else if (j == 14)
                        {
                            data += 'E';
                        }
                        else if (j == 15)
                        {
                            data += 'F';
                        }
                        else
                        {
                            data += j.ToString();
                        }
                    }
                }
            }
            byte[] temp = new byte[1];
            byte[] buffer = new byte[data.Length / 2];
            for (int i = 0; i < buffer.Length; i++)
            {
                temp[0] = Convert.ToByte(data.Substring(i * 2, 2).Trim(), 16);
                buffer[i] = temp[0];
            }
            string date = Encoding.ASCII.GetString(buffer);
            return date;
        }

        public static string GetMachineCodeString()
        {
            string machineCodeString = string.Empty;
            machineCodeString = "PC." + GetCpuInfo();
            return machineCodeString;
        }

        ///   <summary>
        ///   获取cpu序列号
        ///   </summary>
        ///   <returns> string </returns>
        private static string GetCpuInfo()
        {
            string cpuInfo = "";
            try
            {
                using (ManagementClass cimobject = new ManagementClass("Win32_Processor"))
                {
                    ManagementObjectCollection moc = cimobject.GetInstances();

                    foreach (ManagementObject mo in moc)
                    {
                        cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return cpuInfo.ToString();
        }

        ///   <summary>
        ///   获取硬盘ID
        ///   </summary>
        ///   <returns> string </returns>
        private static string GetHDid()
        {
            string HDid = "";
            try
            {
                using (ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive"))
                {
                    ManagementObjectCollection moc1 = cimobject1.GetInstances();
                    foreach (ManagementObject mo in moc1)
                    {
                        HDid = (string)mo.Properties["Model"].Value;
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return HDid.ToString();
        }

        ///   <summary>
        ///   获取网卡硬件地址
        ///   </summary>
        ///   <returns> string </returns>
        private static string GetMoAddress()
        {
            string MoAddress = "";
            try
            {
                using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
                {
                    ManagementObjectCollection moc2 = mc.GetInstances();
                    foreach (ManagementObject mo in moc2)
                    {
                        if ((bool)mo["IPEnabled"] == true)
                            MoAddress = mo["MacAddress"].ToString();
                        mo.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return MoAddress.ToString();
        }

        public static string GetMD5(string machineCode)
        {
            string msg = "";
            MD5 md5 = MD5.Create();
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(machineCode));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符
                msg = msg + s[i].ToString("X");
            }
            return msg;
        }
    }
}