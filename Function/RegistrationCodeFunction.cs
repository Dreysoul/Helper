using System;
using System.IO;

namespace YiRongMachine
{
    public class RegistrationCodeFunction
    {
        private const string path = @"C:\Program Files (x86)\Yirong\code.txt";
        private const string directory = @"C:\Program Files (x86)\Yirong";

        public static bool Check()
        {
            if (!File.Exists(path))
            {
                return false;
            }
            try
            {
                string machineCode = SecurityHelper.GetMachineCodeString();
                string firstMD5 = SecurityHelper.GetMD5(machineCode);
                firstMD5 += "YIRONGMACHINE";
                string machineCodeMd5 = SecurityHelper.GetMD5(firstMD5);

                string msg = File.ReadAllText(path);
                int first = msg.IndexOf("YIRONG1");
                int second = msg.IndexOf("YIRONG2");

                //对硬件进行校验
                string s1 = msg.Substring(0, first);
                if (machineCodeMd5 != s1)
                {
                    return false;
                }

                //对时间进行校验 ,s2是开始时间，s3是结束时间
                string s2 = msg.Substring(first + 7, second - first - 7);
                string s3 = msg.Substring(second + 7);

                string s2Date = SecurityHelper.FromSecurity(s2);
                string s3Date = SecurityHelper.FromSecurity(s3);

                DateTime d2 = Convert.ToDateTime(s2Date);
                DateTime d3 = Convert.ToDateTime(s3Date);

                if (DateTime.Now > d2 && DateTime.Now < d3)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool Check(string msg)
        {
            try
            {
                string machineCode = SecurityHelper.GetMachineCodeString();
                string firstMD5 = SecurityHelper.GetMD5(machineCode);
                firstMD5 += "YIRONGMACHINE";
                string machineCodeMd5 = SecurityHelper.GetMD5(firstMD5);

                int first = msg.IndexOf("YIRONG1");
                int second = msg.IndexOf("YIRONG2");

                //对硬件进行校验
                string s1 = msg.Substring(0, first);
                if (machineCodeMd5 != s1)
                {
                    return false;
                }

                //对时间进行校验 ,s2是开始时间，s3是结束时间
                string s2 = msg.Substring(first + 7, second - first - 7);
                string s3 = msg.Substring(second + 7);

                string s2Date = SecurityHelper.FromSecurity(s2);
                string s3Date = SecurityHelper.FromSecurity(s3);

                DateTime d2 = Convert.ToDateTime(s2Date);
                DateTime d3 = Convert.ToDateTime(s3Date);
                if (DateTime.Now > d2 && DateTime.Now < d3)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void Record(string msg)
        {
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            File.WriteAllText(path, msg);
        }

        public static string GetCode(string code, DateTime start, DateTime end)
        {
            string machineCode = code + "YIRONGMACHINE";
            string machineCodeMd5 = SecurityHelper.GetMD5(machineCode);

            string s1 = start.ToString("yyyy-MM-dd");
            string s2 = end.ToString("yyyy-MM-dd");

            string s11 = SecurityHelper.GetSecurity(s1);
            string s22 = SecurityHelper.GetSecurity(s2);

            string msg = machineCodeMd5 + "YIRONG1" + s11 + "YIRONG2" + s22;
            return msg;
        }
    }
}