using System;
using System.Threading;

namespace YiRongMachine
{
    public class TaiDaPLCHelper
    {
        private object o1 = new object();
        private int _iConnectType = 0;
        private SerialPortByNilifu _serial = new SerialPortByNilifu();         //串口封装的意义在于接受数据的事件
        private int _hardwareNo = 0;
        private int _offset = 0;

        public TaiDaPLCHelper()
        {
        }

        public bool Open(PLCSetting plcSetting, ref int errorCode)
        {
            _iConnectType = plcSetting.connectType;
            _hardwareNo = plcSetting.HardwareNo;
            _offset = plcSetting.Offset;
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
                addr = addr + 4096;
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
                addr = addr + 4096;
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
                addr = addr + 4096;
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
                addr = addr + 4096;
                return ReadMultiDataBySerial(addr, ref value, ref errorCode);
            }
        }

        private bool OpenBySerial(PLCSetting plcSetting, ref int errorCode)
        {
            _serial.PortName = plcSetting.SerialPort;
            _serial.BaudRate = plcSetting.BaudRate;
            _serial.DataBits = plcSetting.DataBits;
            _serial.StopBits = plcSetting.StopBits;
            _serial.Parity = plcSetting.Parity;
            _hardwareNo = plcSetting.HardwareNo;
            _hardwareNo = 1;
            bool bRtn = _serial.OpenPort();
            if (bRtn)
            {
                short value = 0;
                bRtn = ReadOneDataBySerial(4596, ref value, ref errorCode);
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
            _serial.Close();
        }

        private bool WriteOneDataBySerial(int address, short value, ref int errorCode)
        {
            lock (o1)
            {
                _serial.ClearStream();
                string sendBuffer = ModbusAscii.WriteOne(_hardwareNo, address, value);
                bool bRtn = _serial.WriteData(sendBuffer);
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
                        string ret = _serial.ReadString();
                        bRtn = ModbusAscii.CheckWriteOne(ret.Trim(), sendBuffer.Trim());
                        if (bRtn)
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

        private bool ReadOneDataBySerial(int address, ref short value, ref int errorCode)
        {
            lock (o1)
            {
                _serial.ClearStream();
                string sendBuffer = ModbusAscii.Read(_hardwareNo, address, 1);
                bool bRtn = _serial.WriteData(sendBuffer);
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
                        short[] target = new short[1];
                        // 读取PLC响应
                        string ret = _serial.ReadString();
                        bRtn = ModbusAscii.CheckRead(_hardwareNo, 1, ret, ref target);
                        if (bRtn)
                        {
                            value = target[0];
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

        private bool WriteMultiDataBySerial(int address, short[] value, ref int errorCode)
        {
            lock (o1)
            {
                _serial.ClearStream();
                string sendBuffer = ModbusAscii.WriteMulti(_hardwareNo, address, value);
                bool bRtn = _serial.WriteData(sendBuffer);
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
                        string response = _serial.ReadString();
                        bRtn = ModbusAscii.CheckWriteMulti(_hardwareNo, address, value.Length, response);
                        if (bRtn)
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
                string sendBuffer = ModbusAscii.Read(_hardwareNo, address, value.Length);
                bool bRtn = _serial.WriteData(sendBuffer);
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
                        string ret = _serial.ReadString();
                        bRtn = ModbusAscii.CheckRead(_hardwareNo, value.Length, ret, ref value);
                        if (bRtn)
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
            errorCode = (int)PLCErrorCode.Tcp_NoSuchConnect_Error;
            return false;
        }

        private bool ReadOneDataByTcp(int address, ref short value, ref int errorCode)
        {
            errorCode = (int)PLCErrorCode.Tcp_NoSuchConnect_Error;
            return false;
        }

        private bool WriteMultiDataByTcp(int address, short[] value, ref int errorCode)
        {
            errorCode = (int)PLCErrorCode.Tcp_NoSuchConnect_Error;
            return false;
        }

        private bool ReadMultiDataByTcp(int address, ref short[] value, ref int errorCode)
        {
            errorCode = (int)PLCErrorCode.Tcp_NoSuchConnect_Error;
            return false;
        }
    }
}