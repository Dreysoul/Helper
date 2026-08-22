using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace YiRongMachine
{
    public class SanLingPLCControl
    {
        object o = new object();
        SerialPortUtil com = new SerialPortUtil();
        public SanLingPLCControl()
        {
            com.PortName = "COM1";
            com.BaudRate = 9600;
            com.DataBits = 7;
            com.StopBits = 1;
            com.Parity = "EVEN";
        }
        public bool  Open()
        {
           return  com.OpenPort();
        }


        public void Close()
        {
            com.Close();
            return;
        }

        public string CheckSum(byte[] cmd)
        {
            byte sum = 0;

            for (int i = 0; i < cmd.Length; i++)
            {
                sum = (byte)(sum + cmd[i]);
            }

            return sum.ToString("X2");
        }


        public  int AsciiToInt(byte ascVal)
        {
            if (ascVal >= 0x30 && ascVal <= 0x39) // ASCII字符0-9之间
            {
                return (ascVal - 0x30);
            }
            else if (ascVal >= 0x41 && ascVal <= 0x46) // ASCII字符A-F之间
            {
                return (ascVal - 0x41 + 10);
            }
            else if (ascVal >= 0x61 && ascVal <= 0x66) // ASCII字符a-f之间
            {
                return (ascVal - 0x61 + 10);
            }
            else
            {
                return -1;
            }
        }
        public  short TranslateToShortValue(byte[] buf)
        {
            int lowByte = AsciiToInt(buf[0]) * 16 + AsciiToInt(buf[1]);
            int highByte = AsciiToInt(buf[2]) * 16 + AsciiToInt(buf[3]);
            int intVal = highByte * 256 + lowByte;
            return (short)intVal;
        }


        private static string STX = "\x02";
        private static string ETX = "\x03";

        private  bool Write(int addr, short value)
        {
            lock (o)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append("1"); // CMD

                addr = addr * 2 + 4096;
                sb.Append(addr.ToString("X4")); // 首地址

                sb.Append("02");    //字节数

                string strValue = value.ToString("X4"); // 数据
                sb.Append(strValue.Substring(2, 2));    // 低字节在先
                sb.Append(strValue.Substring(0, 2));    // 高字节在后

                sb.Append(ETX);   // 结束符

                // 计算SUM
                byte[] cmd = System.Text.Encoding.ASCII.GetBytes(sb.ToString());
                string sum = CheckSum(cmd);
                sb.Append(sum); // SUM

                sb.Insert(0, STX);  // 插入起始符

                try
                {
                    // 转换成字节并写入串口
                    byte[] cmdArr = System.Text.Encoding.ASCII.GetBytes(sb.ToString());
                    com.WriteData(cmdArr);
                }
                catch
                {
                    return false;
                }
                DateTime dtNow = DateTime.Now;
                while (true)
                {
                    if (com.IsDataReceived)
                    {
                        try
                        {
                            // 读取PLC响应
                            int ret = com.ReadOneByte();
                            if (ret == 6)
                            {
                                // 正确应答
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    else if (dtNow .AddSeconds(1) > DateTime.Now )
                    {
                        return false;
                    }
                }
            }
        }

        private  bool Read(int addr,ref  short value )
        {
            lock (o)
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("0"); // CMD
                addr = addr * 2 + 4096;
                sb.Append(addr.ToString("X4")); // 首地址
                sb.Append("02");    //字节数
                sb.Append(ETX);   // 结束符

                // 计算SUM
                byte[] cmd = Encoding.ASCII.GetBytes(sb.ToString());
                string sum = CheckSum(cmd);
                sb.Append(sum); // SUM
                sb.Insert(0, STX);  // 插入起始符

                try
                {
                    // 转换成字节并写入串口
                    byte[] cmdArr = Encoding.ASCII.GetBytes(sb.ToString());
                    com.WriteData(cmdArr) ;
                }
                catch
                {
                    return false;
                }
                DateTime dtNow = DateTime.Now;
                while (true)
                {
                    if (com.IsDataReceived)
                    {
                        try
                        {
                            byte[] buffer = new byte[10];
                            // 读取PLC响应
                            int ret = com.ReadByte(ref buffer);
                            byte[] data = new byte[4];
                            if (buffer[0] == 2)
                            {
                                for (int i = 0; i < 4; i++)
                                {
                                    data[i] = buffer[i + 1];
                                }
                                value = TranslateToShortValue(data);
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        catch
                        {
                            return false;
                        }
                    }
                    else if (dtNow.AddSeconds(1) > DateTime.Now)
                    {
                        return false;
                    }
                }
            }
        }



        public bool WriteData(int addr, short value)
        {
            short s = -1;
            for (int i = 0; i < 3; i++)
            {
                bool bRtn = Write(addr, value);
                if (bRtn)
                {
                    bRtn = Read(addr, ref s);
                    if (bRtn &&  s == value )
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool ReadData(int addr,ref  short value)
        {
            bool bRtn;
            for (int i = 0; i < 3; i++)
            {
                 bRtn = Read(addr, ref value );
                if (bRtn)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
