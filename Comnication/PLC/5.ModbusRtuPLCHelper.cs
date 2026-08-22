using System;
using System.Threading;

namespace YiRongMachine
{
    public class ModbusRTUPLCHelper
    {
        private object o1 = new object();
        private int _iConnectType = 0;
        private SerialPortByNilifu _serial = new SerialPortByNilifu();         //串口封装的意义在于接受数据的事件
        private int _hardwareNo = 0;
        private int _offset = 0;

        public ModbusRTUPLCHelper()
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
                addr = addr - _offset;
                if (addr < 0)
                {
                    addr = 0;
                }
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
                addr = addr - _offset;
                if (addr < 0)
                {
                    addr = 0;
                }
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
                addr = addr - _offset;
                if (addr < 0)
                {
                    addr = 0;
                }
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
                addr = addr - _offset;
                if (addr < 0)
                {
                    addr = 0;
                }
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
                bRtn = ReadOneDataBySerial(0, ref value, ref errorCode);
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
                byte[] target = BitConverter.GetBytes(value);
                byte b = target[0];
                target[0] = target[1];
                target[1] = b;
                byte[] sendBuffer = ModbusRtu.WriteOne(_hardwareNo, address, target);
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
                        byte[] buffer = new byte[8];
                        // 读取PLC响应
                        int ret = _serial.ReadByte(buffer);
                        //Thread.Sleep(10);
                        if (ret != 8)
                        {
                            return false;
                        }
                        bRtn = ModbusRtu.CheckWriteOne(_hardwareNo, address, buffer);
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
                byte[] sendBuffer = ModbusRtu.Read(_hardwareNo, address, 1);
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
                        byte[] buffer = new byte[7];
                        // 读取PLC响应
                        int ret = _serial.ReadByte(buffer);
                        //Thread.Sleep(10);
                        if (ret != 7)
                        {
                            return false;
                        }
                        byte[] targert = new byte[2];
                        bRtn = ModbusRtu.CheckRead(_hardwareNo, address, 1, buffer, ref targert);
                        if (bRtn)
                        {
                            byte b = targert[0];
                            targert[0] = targert[1];
                            targert[1] = b;
                            value = BitConverter.ToInt16(targert, 0);
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
                byte[] target = new byte[value.Length * 2];
                for (int i = 0; i < value.Length; i++)
                {
                    byte[] b = BitConverter.GetBytes(value[i]);
                    byte c = b[0];
                    b[0] = b[1];
                    b[1] = c;
                    Array.Copy(b, 0, target, 2 * i, 2);
                }

                byte[] sendBuffer = ModbusRtu.WriteMulti(_hardwareNo, address, value.Length, target);
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
                        byte[] buffer = new byte[8];
                        // 读取PLC响应
                        int ret = _serial.ReadByte(buffer);
                        //Thread.Sleep(10);
                        if (ret != 8)
                        {
                            return false;
                        }
                        bRtn = ModbusRtu.CheckWriteMulti(_hardwareNo, address, value.Length, buffer);
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
                byte[] sendBuffer = ModbusRtu.Read(_hardwareNo, address, value.Length);
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
                        Thread.Sleep(20);
                        byte[] buffer = new byte[5 + value.Length * 2];
                        // 读取PLC响应
                        int ret = _serial.ReadByte(buffer);

                        if (ret != 5 + value.Length * 2)
                        {
                            return false;
                        }
                        byte[] targert = new byte[value.Length * 2];
                        bRtn = ModbusRtu.CheckRead(_hardwareNo, address, value.Length, buffer, ref targert);
                        if (bRtn)
                        {
                            for (int i = 0; i < value.Length; i++)
                            {
                                byte[] b = new byte[2];
                                b[0] = targert[2 * i + 1];
                                b[1] = targert[2 * i];
                                value[i] = BitConverter.ToInt16(b, 0);
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