using Sharp7;
using System;
using System.Threading;

namespace YiRongMachine
{
    public class SiemensPLCHelper
    {
        private object o1 = new object();
        private int _iConnectType = 0;
        private S7Client _s7Client = new S7Client();
        private SerialPortByNilifu _serial = new SerialPortByNilifu();         //串口封装的意义在于接受数据的事件
        private int _hardwareNo = 0;
        private int _offset = 0;
        private byte[] _again = { 0x10, 0x02, 0x00, 0x5C, 0x5E, 0x16 };
        private byte[] _writeOK = { 0x68, 0x12, 0x12, 0x68, 0x00, 0x02, 0x08, 0x32, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00, 0x02, 0x00, 0x01, 0x00, 0x00, 0x05, 0x01, 0xFF, 0x47, 0x16 };

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

        public bool WriteOneData(int DBNumber, int addr, short value, ref int errorCode)
        {
            if (_iConnectType == (int)HardwareConnectType.Tcp)
            {
                addr = (addr - _offset) * 2;
                if (addr < 0)
                {
                    addr = 0;
                }
                return WriteOneDataByTcp(DBNumber, addr, value, ref errorCode);
            }
            else
            {
                addr = (addr - _offset) * 2;
                if (addr < 0)
                {
                    addr = 0;
                }
                return WriteOneDataBySerial(addr, value, ref errorCode);
            }
        }

        public bool ReadOneData(int DBNumber, int addr, ref short value, ref int errorCode)
        {
            if (_iConnectType == (int)HardwareConnectType.Tcp)
            {
                addr = (addr - _offset) * 2;
                if (addr < 0)
                {
                    addr = 0;
                }
                return ReadOneDataByTcp(DBNumber, addr, ref value, ref errorCode);
            }
            else
            {
                addr = (addr - _offset) * 2;
                if (addr < 0)
                {
                    addr = 0;
                }
                return ReadOneDataBySerial(addr, ref value, ref errorCode);
            }
        }

        public bool WriteMultiData(int DBNumber, int addr, short[] value, ref int errorCode)
        {
            if (_iConnectType == (int)HardwareConnectType.Tcp)
            {
                addr = (addr - _offset) * 2;
                if (addr < 0)
                {
                    addr = 0;
                }
                return WriteMultiDataByTcp(DBNumber, addr, value, ref errorCode);
            }
            else
            {
                addr = (addr - _offset) * 2;
                if (addr < 0)
                {
                    addr = 0;
                }
                return WriteMultiDataBySerial(addr, value, ref errorCode);
            }
        }

        public bool ReadMultiData(int DBNumber, int addr, ref short[] value, ref int errorCode)
        {
            if (_iConnectType == (int)HardwareConnectType.Tcp)
            {
                addr = (addr - _offset) * 2;
                if (addr < 0)
                {
                    addr = 0;
                }
                return ReadMultiDataByTcp(DBNumber, addr, ref value, ref errorCode);
            }
            else
            {
                addr = (addr - _offset) * 2;
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
                byte[] sendBuffer = PPIProtocol.WriteVWOne(address, value);
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
                        byte[] buffer = new byte[1];
                        // 读取PLC响应
                        int ret = _serial.ReadByte(buffer);
                        //Thread.Sleep(10);
                        if (ret != 1 && buffer[0] != 229)
                        {
                            return false;
                        }
                        Thread.Sleep(10);
                        _serial.ClearStream();
                        //第二次发送继续指令
                        bRtn = _serial.WriteData(_again);
                        if (!bRtn)
                        {
                            errorCode = (int)PLCErrorCode.Serial_SendData_Error;
                            return false;
                        }

                        dtNow = DateTime.Now;
                        Thread.Sleep(10);
                        while (true)
                        {
                            if (_serial.IsDataReceived)
                            {
                                byte[] finalData = new byte[50];
                                int totalbytenumber = 0;
                                while (totalbytenumber < 24 && dtNow.AddSeconds(1) > DateTime.Now)
                                {
                                    byte[] vvv = new byte[8];
                                    ret = _serial.ReadByte(vvv);
                                    Array.Copy(vvv, 0, finalData, totalbytenumber, ret);
                                    totalbytenumber += ret;
                                    Thread.Sleep(10);
                                }
                                if (dtNow.AddSeconds(1) < DateTime.Now)
                                {
                                    errorCode = (int)PLCErrorCode.Serial_LongTime_Error;
                                    return false;
                                }

                                if (finalData[22] == 0x47)
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
                                return false;
                            }
                            Thread.Sleep(10);
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
                byte[] sendBuffer = PPIProtocol.ReadVW(1, address);
                bool bRtn = _serial.WriteData(sendBuffer);
                if (!bRtn)
                {
                    errorCode = (int)PLCErrorCode.Serial_SendData_Error;
                    return false;
                }
                DateTime dtNow = DateTime.Now;
                //Thread.Sleep(50);
                while (true)
                {
                    if (_serial.IsDataReceived)
                    {
                        byte[] buffer = new byte[1];
                        // 读取PLC响应
                        int ret = _serial.ReadByte(buffer);
                        Thread.Sleep(10);
                        if (ret != 1 && buffer[0] != 229)
                        {
                            return false;
                        }
                        _serial.ClearStream();
                        //第二次发送继续指令
                        bRtn = _serial.WriteData(_again);
                        if (!bRtn)
                        {
                            errorCode = (int)PLCErrorCode.Serial_SendData_Error;
                            return false;
                        }
                        dtNow = DateTime.Now;
                        Thread.Sleep(10);
                        while (true)
                        {
                            if (_serial.IsDataReceived)
                            {
                                byte[] finalData = new byte[50];
                                int totalbytenumber = 0;
                                while (totalbytenumber < 29 && dtNow.AddSeconds(1) > DateTime.Now)
                                {
                                    byte[] vvv = new byte[8];
                                    ret = _serial.ReadByte(vvv);
                                    Array.Copy(vvv, 0, finalData, totalbytenumber, ret);
                                    totalbytenumber += ret;
                                    Thread.Sleep(10);
                                }
                                if (dtNow.AddSeconds(1) < DateTime.Now)
                                {
                                    errorCode = (int)PLCErrorCode.Serial_LongTime_Error;
                                    return false;
                                }
                                byte[] targert = new byte[2];
                                targert[0] = finalData[26];
                                targert[1] = finalData[25];
                                value = BitConverter.ToInt16(targert, 0);
                                return true;
                            }
                            else if (dtNow.AddSeconds(1) < DateTime.Now)
                            {
                                errorCode = (int)PLCErrorCode.Serial_LongTime_Error;
                                return false;
                            }
                            Thread.Sleep(10);
                        }
                    }
                    else if (dtNow.AddSeconds(1) < DateTime.Now)
                    {
                        errorCode = (int)PLCErrorCode.Serial_LongTime_Error;
                        return false;
                    }
                }
            }
        }

        private bool WriteMultiDataBySerial(int address, short[] value, ref int errorCode)
        {
            lock (o1)
            {
                bool bRtn = false;
                for (int i = 0; i < value.Length; i++)
                {
                    bRtn = WriteOneDataBySerial(address + i * 2, value[i], ref errorCode);
                    if (!bRtn)
                    {
                        errorCode = (int)PLCErrorCode.Serial_Check_Error;
                        return false;
                    }
                }
                return true;
            }
        }

        private bool ReadMultiDataBySerial(int address, ref short[] value, ref int errorCode)
        {
            lock (o1)
            {
                _serial.ClearStream();
                byte[] sendBuffer = PPIProtocol.ReadVW(value.Length, address);
                bool bRtn = _serial.WriteData(sendBuffer);
                if (!bRtn)
                {
                    errorCode = (int)PLCErrorCode.Serial_SendData_Error;
                    return false;
                }
                DateTime dtNow = DateTime.Now;
                //Thread.Sleep(50);
                while (true)
                {
                    if (_serial.IsDataReceived)
                    {
                        byte[] buffer = new byte[1];
                        // 读取PLC响应
                        int ret = _serial.ReadByte(buffer);
                        //Thread.Sleep(10);
                        if (ret != 1 && buffer[0] != 229)
                        {
                            return false;
                        }
                        _serial.ClearStream();
                        //第二次发送继续指令
                        bRtn = _serial.WriteData(_again);
                        if (!bRtn)
                        {
                            errorCode = (int)PLCErrorCode.Serial_SendData_Error;
                            return false;
                        }
                        dtNow = DateTime.Now;
                        //Thread.Sleep(50);
                        while (true)
                        {
                            if (_serial.IsDataReceived)
                            {
                                byte[] finalData = new byte[50];
                                int totalbytenumber = 0;
                                while (totalbytenumber < 27 + value.Length * 2 && dtNow.AddSeconds(1) > DateTime.Now)
                                {
                                    byte[] vvv = new byte[8];
                                    ret = _serial.ReadByte(vvv);
                                    Array.Copy(vvv, 0, finalData, totalbytenumber, ret);
                                    totalbytenumber += ret;
                                }
                                if (dtNow.AddSeconds(1) < DateTime.Now)
                                {
                                    errorCode = (int)PLCErrorCode.Serial_LongTime_Error;
                                    return false;
                                }
                                for (int i = 0; i < value.Length; i++)
                                {
                                    byte[] targert = new byte[2];
                                    targert[0] = finalData[26 + i * 2];
                                    targert[1] = finalData[25 + i * 2];
                                    value[i] = BitConverter.ToInt16(targert, 0);
                                }
                                return true;
                            }
                            else if (dtNow.AddSeconds(1) < DateTime.Now)
                            {
                                errorCode = (int)PLCErrorCode.Serial_LongTime_Error;
                                return false;
                            }
                        }
                    }
                    else if (dtNow.AddSeconds(1) < DateTime.Now)
                    {
                        errorCode = (int)PLCErrorCode.Serial_LongTime_Error;
                        return false;
                    }
                }
            }
        }

        private bool OpenByTcp(PLCSetting plcSetting, ref int errorCode)
        {
            try
            {
                int reslut = _s7Client.ConnectTo(plcSetting.IP, 0, 0);

                if (reslut == 0)
                {
                    return true;
                }
                else
                {
                    errorCode = (int)PLCErrorCode.Tcp_Open_Error;
                    return false;
                }
            }
            catch (Exception)
            {
                errorCode = (int)PLCErrorCode.Tcp_Open_Error;
                return false;
            }
        }

        private void CloseByTcp()
        {
            if (_s7Client != null)
            {
                _s7Client.Disconnect();
            }
            return;
        }

        private bool WriteOneDataByTcp(int DBNumber, int address, short value, ref int errorCode)
        {
            lock (o1)
            {
                byte[] blocks = new byte[2];
                S7.SetWordAt(blocks, 0, (ushort)value);
                //写入值
                int errcode = _s7Client.DBWrite(DBNumber, address, blocks.Length, blocks);
                if (errcode == 0)
                {
                    return true;
                }
                else
                {
                    errorCode = (int)PLCErrorCode.Tcp_Check_Error;
                    return false;
                }
            }
        }

        private bool ReadOneDataByTcp(int DBNumber, int address, ref short value, ref int errorCode)
        {
            lock (o1)
            {
                value = 0;
                //Dint
                byte[] blocks = new byte[2];
                int errcode = _s7Client.DBRead(DBNumber, address, blocks.Length, blocks);
                if (0 != errcode)
                {
                    errorCode = (int)PLCErrorCode.Tcp_Check_Error;
                    return false;
                }
                value = (short)S7.GetWordAt(blocks, 0);
                return true;
            }
        }

        private bool WriteMultiDataByTcp(int DBNumber, int address, short[] value, ref int errorCode)
        {
            lock (o1)
            {
                byte[] blocks = new byte[value.Length * 2];
                for (int i = 0; i < value.Length; i++)
                {
                    byte[] buffer = new byte[2];
                    S7.SetWordAt(buffer, 0, (ushort)value[i]);
                    buffer.CopyTo(blocks, 2 * i);
                }
                //写入值
                int errcode = _s7Client.DBWrite(DBNumber, address, blocks.Length, blocks);
                if (errcode == 0)
                {
                    return true;
                }
                else
                {
                    errorCode = (int)PLCErrorCode.Tcp_Check_Error;
                    return false;
                }
            }
        }

        private bool ReadMultiDataByTcp(int DBNumber, int address, ref short[] value, ref int errorCode)
        {
            lock (o1)
            {
                //Dint
                byte[] blocks = new byte[2 * value.Length];
                int errcode = _s7Client.DBRead(DBNumber, address, blocks.Length, blocks);
                if (0 != errcode)
                {
                    errorCode = (int)PLCErrorCode.Tcp_Check_Error;
                    return false;
                }
                for (int i = 0; i < value.Length; i++)
                {
                    byte[] buffer = new byte[2];
                    buffer[0] = blocks[2 * i];
                    buffer[1] = blocks[2 * i + 1];
                    value[i] = (short)S7.GetWordAt(buffer, 0);
                }

                return true;
            }
        }
    }
}