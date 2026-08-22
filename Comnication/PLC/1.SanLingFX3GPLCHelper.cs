using System;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace YiRongMachine
{
    public class SanLingFX3GPLCHelper
    {
        private object o1 = new object();
        public int _iConnectType = 0;
        public TcpClient _tcp;                     //官方的Tcp已经封装好了
        public Stream _streamTcp;
        public SerialPortByNilifu _serial;         //串口封装的意义在于接受数据的事件

        #region 解析PLC收发指令的方法

        private string CheckSum(byte[] cmd)
        {
            byte sum = 0;

            for (int i = 0; i < cmd.Length; i++)
            {
                sum = (byte)(sum + cmd[i]);
            }

            return sum.ToString("X2");
        }

        private int AsciiToInt(byte ascVal)
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

        private short TranslateToShortValue(byte[] buf)
        {
            int lowByte = AsciiToInt(buf[0]) * 16 + AsciiToInt(buf[1]);
            int highByte = AsciiToInt(buf[2]) * 16 + AsciiToInt(buf[3]);
            int intVal = highByte * 256 + lowByte;
            return (short)intVal;
        }

        private static string STX = "\x02";
        private static string ETX = "\x03";

        #endregion 解析PLC收发指令的方法

        public bool Open(PLCSetting plcSetting, ref int errorCode)
        {
            _iConnectType = plcSetting.connectType;
            if (_iConnectType == (int)HardwareConnectType.Tcp)
            {
                return OpenByTcp(plcSetting, ref errorCode);
            }
            else
            {
                return OpenBySerial(plcSetting, ref errorCode);
            }
        }

        public void Close()
        {
            if (_iConnectType == (int)HardwareConnectType.Tcp)
            {
                CloseByTcp();
            }
            else
            {
                CloseBySerial();
            }
        }

        public bool WriteOneData(int addr, short value, ref int errorCode)
        {
            if (_iConnectType == (int)HardwareConnectType.Tcp)
            {
                return WriteOneDataByTcp(addr, value, ref errorCode);
            }
            else
            {
                return WriteOneDataBySerial(addr, value, ref errorCode);
            }
        }

        public bool ReadOneData(int addr, ref short value, ref int errorCode)
        {
            if (_iConnectType == (int)HardwareConnectType.Tcp)
            {
                return ReadOneDataByTcp(addr, ref value, ref errorCode);
            }
            else
            {
                return ReadOneDataBySerial(addr, ref value, ref errorCode);
            }
        }

        public bool WriteMultiData(int addr, short[] value, ref int errorCode)
        {
            if (_iConnectType == (int)HardwareConnectType.Tcp)
            {
                return WriteMultiDataByTcp(addr, value, ref errorCode);
            }
            else
            {
                return WriteMultiDataBySerial(addr, value, ref errorCode);
            }
        }

        public bool ReadMultiData(int addr, ref short[] value, ref int errorCode)
        {
            if (_iConnectType == (int)HardwareConnectType.Tcp)
            {
                return ReadMultiDataByTcp(addr, ref value, ref errorCode);
            }
            else
            {
                return ReadMultiDataBySerial(addr, ref value, ref errorCode);
            }
        }

        private bool OpenBySerial(PLCSetting plcSetting, ref int errorCode)
        {
            _serial = new SerialPortByNilifu();
            _serial.PortName = plcSetting.SerialPort;
            _serial.BaudRate = plcSetting.BaudRate;
            _serial.DataBits = plcSetting.DataBits;
            _serial.StopBits = plcSetting.StopBits;
            _serial.Parity = plcSetting.Parity;

            bool bRtn = _serial.OpenPort();
            if (bRtn)
            {
                Thread.Sleep(100);
                short value = 0;
                bRtn = ReadOneDataBySerial(500, ref value, ref errorCode);
                if (bRtn)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                errorCode = (int)PLCErrorCode.Serial_Open_Error;
                return false;
            }
        }

        private void CloseBySerial()
        {
            if (_serial != null)
            {
                _serial.Close();
            }
        }

        private bool WriteOneDataBySerial(int address, short value, ref int errorCode)
        {
            lock (o1)
            {
                _serial.ClearStream();
                StringBuilder sb = new StringBuilder();

                sb.Append("1"); // CMD
                address = address * 2 + 4096;
                sb.Append(address.ToString("X4")); // 首地址
                sb.Append("02");    //字节数
                string strValue = value.ToString("X4"); // 数据
                sb.Append(strValue.Substring(2, 2));    // 低字节在先
                sb.Append(strValue.Substring(0, 2));    // 高字节在后
                sb.Append(ETX);   // 结束符

                // 计算SUM
                byte[] cmd = Encoding.ASCII.GetBytes(sb.ToString());
                string sum = CheckSum(cmd);
                sb.Append(sum); // SUM
                sb.Insert(0, STX);  // 插入起始符

                // 转换成字节并写入串口
                byte[] cmdArr = Encoding.ASCII.GetBytes(sb.ToString());
                bool bRtn = _serial.WriteData(cmdArr);
                if (!bRtn)
                {
                    errorCode = (int)PLCErrorCode.Serial_SendData_Error;
                    return false;
                }

                DateTime dtNow = DateTime.Now;
                Thread.Sleep(20);
                while (true)
                {
                    if (_serial.IsDataReceived)
                    {
                        // 读取PLC响应
                        int response = _serial.ReadOneByte();
                        if (response == 6)
                        {
                            // 正确应答
                            return true;
                        }
                        else
                        {
                            errorCode = (int)PLCErrorCode.Serial_Check_Error;
                            return false;
                        }
                    }
                    else if (dtNow.AddSeconds(1) < DateTime.Now)
                    {
                        errorCode = (int)PLCErrorCode.Serial_LongTime_Error;
                        return false;
                    }
                    Thread.Sleep(5);
                }
            }
        }

        private bool ReadOneDataBySerial(int address, ref short value, ref int errorCode)
        {
            lock (o1)
            {
                _serial.ClearStream();
                StringBuilder sb = new StringBuilder();
                sb.Append("0"); // CMD
                address = address * 2 + 4096;
                sb.Append(address.ToString("X4")); // 首地址
                sb.Append("02");    //字节数
                sb.Append(ETX);   // 结束符

                // 计算SUM
                byte[] cmd = Encoding.ASCII.GetBytes(sb.ToString());
                string sum = CheckSum(cmd);
                sb.Append(sum); // SUM
                sb.Insert(0, STX);  // 插入起始符

                // 转换成字节并写入串口
                byte[] cmdArr = Encoding.ASCII.GetBytes(sb.ToString());
                bool bRtn = _serial.WriteData(cmdArr);
                if (!bRtn)
                {
                    errorCode = (int)PLCErrorCode.Serial_SendData_Error;
                    return false;
                }

                DateTime dtNow = DateTime.Now;
                Thread.Sleep(20);
                while (true)
                {
                    if (_serial.IsDataReceived)
                    {
                        byte[] buffer = new byte[8];
                        // 读取PLC响应
                        int ret = _serial.ReadByte(buffer);
                        //Thread.Sleep(10);
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
                            errorCode = (int)PLCErrorCode.Serial_Check_Error;
                            return false;
                        }
                    }
                    else if (dtNow.AddSeconds(10) < DateTime.Now)
                    {
                        errorCode = (int)PLCErrorCode.Serial_LongTime_Error;
                        return false;
                    }
                    Thread.Sleep(5);
                }
            }
        }

        private bool WriteMultiDataBySerial(int address, short[] value, ref int errorCode)
        {
            lock (o1)
            {
                _serial.ClearStream();
                StringBuilder sb = new StringBuilder();
                // CMD
                sb.Append("1");
                // 首地址
                address = address * 2 + 4096;
                sb.Append(address.ToString("X4"));
                // 字节数
                sb.Append((value.Length * 2).ToString("X2"));

                for (int i = 0; i < value.Length; i++)
                {
                    string strValue = value[i].ToString("X4");  // 数据
                    sb.Append(strValue.Substring(2, 2));        // 低字节在先
                    sb.Append(strValue.Substring(0, 2));        // 高字节在后
                }

                sb.Append(ETX);                                 // 结束符
                // 计算SUM
                byte[] cmd = Encoding.ASCII.GetBytes(sb.ToString());
                string sum = CheckSum(cmd);
                sb.Append(sum);                         // SUM
                sb.Insert(0, STX);                      // 插入起始符

                // 转换成字节并写入串口
                byte[] cmdArr = Encoding.ASCII.GetBytes(sb.ToString());
                bool bRtn = _serial.WriteData(cmdArr);
                if (!bRtn)
                {
                    errorCode = (int)PLCErrorCode.Serial_SendData_Error;
                    return false;
                }

                DateTime dtNow = DateTime.Now;
                Thread.Sleep(20);
                while (true)
                {
                    if (_serial.IsDataReceived)
                    {
                        // 读取PLC响应
                        int ret = _serial.ReadOneByte();
                        //Thread.Sleep(10);
                        // 正确应答
                        if (ret == 6)
                        {
                            return true;
                        }
                        else
                        {
                            errorCode = (int)PLCErrorCode.Serial_Check_Error;
                            return false;
                        }
                    }
                    else if (dtNow.AddSeconds(1) < DateTime.Now)
                    {
                        errorCode = (int)PLCErrorCode.Serial_LongTime_Error;
                        return false;
                    }
                    Thread.Sleep(5);
                }
            }
        }

        private bool ReadMultiDataBySerial(int address, ref short[] value, ref int errorCode)
        {
            lock (o1)
            {
                _serial.ClearStream();
                StringBuilder sb = new StringBuilder();
                // CMD
                sb.Append("0");
                // 首地址
                address = address * 2 + 4096;
                sb.Append(address.ToString("X4")); // 首地址
                // 字节数
                sb.Append((value.Length * 2).ToString("X2"));
                // 结束符
                sb.Append(ETX);

                // 计算SUM
                byte[] cmd = Encoding.ASCII.GetBytes(sb.ToString());
                string sum = CheckSum(cmd);
                sb.Append(sum); // SUM
                sb.Insert(0, STX);  // 插入起始符

                // 转换成字节并写入串口
                byte[] cmdArr = Encoding.ASCII.GetBytes(sb.ToString());
                bool bRtn = _serial.WriteData(cmdArr);
                if (!bRtn)
                {
                    errorCode = (int)PLCErrorCode.Serial_SendData_Error;
                    return false;
                }

                DateTime dtNow = DateTime.Now;
                Thread.Sleep(40);
                while (true)
                {
                    if (_serial.IsDataReceived)
                    {
                        //1 +  数据*4 + 1 + 2个校验位
                        int length = 2 + value.Length * 4 + 2;
                        byte[] buffer = new byte[length];
                        // 读取PLC响应
                        int ret = _serial.ReadByte(buffer);
                        //Thread.Sleep(10);

                        //首位必须为2
                        if (buffer[0] == 2)
                        {
                            //转换需要4位byte
                            byte[] data = new byte[4];
                            //取出相应的数据，并转换为short的数组
                            for (int i = 1; i < buffer.Length - 3; i++)
                            {
                                data[((i - 1) % 4)] = buffer[i];
                                if (((i - 1) % 4) == 3)
                                {
                                    value[(i - 1) / 4] = TranslateToShortValue(data);
                                }
                            }
                            return true;
                        }
                        else
                        {
                            errorCode = (int)PLCErrorCode.Serial_Check_Error;
                            return false;
                        }
                    }
                    else if (dtNow.AddSeconds(1) < DateTime.Now)
                    {
                        errorCode = (int)PLCErrorCode.Serial_LongTime_Error;
                        return false;
                    }
                    Thread.Sleep(5);
                }
            }
        }

        private bool OpenByTcp(PLCSetting plcSetting, ref int errorCode)
        {
            errorCode = (int)PLCErrorCode.Tcp_NoSuchConnect_Error;
            return false;
        }

        private void CloseByTcp()
        {
            return;
        }

        private bool WriteOneDataByTcp(int address, short value, ref int errorCode)
        {
            lock (o1)
            {
                errorCode = (int)PLCErrorCode.Tcp_NoSuchConnect_Error;
                return false;
            }
        }

        private bool ReadOneDataByTcp(int address, ref short value, ref int errorCode)
        {
            lock (o1)
            {
                errorCode = (int)PLCErrorCode.Tcp_NoSuchConnect_Error;
                return false;
            }
        }

        private bool WriteMultiDataByTcp(int address, short[] value, ref int errorCode)
        {
            lock (o1)
            {
                errorCode = (int)PLCErrorCode.Tcp_NoSuchConnect_Error;
                return false;
            }
        }

        private bool ReadMultiDataByTcp(int address, ref short[] value, ref int errorCode)
        {
            lock (o1)
            {
                errorCode = (int)PLCErrorCode.Tcp_NoSuchConnect_Error;
                return false;
            }
        }
    }
}