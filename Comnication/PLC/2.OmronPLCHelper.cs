using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace YiRongMachine
{
    public class OmronCP1HPLCHelper
    {
        private object o1 = new object();
        public int _iConnectType = 0;
        public TcpClient _tcp;                     //官方的Tcp已经封装好了
        public Stream _streamTcp;
        public SerialPortByNilifu _serial;         //串口封装的意义在于接受数据的事件

        private byte[] Client = new byte[4];
        private byte[] Server = new byte[4];//PLC和本机数据
        private byte[] dataLeng = { 0x00, 0x01 };

        public bool Open(PLCSetting plcSetting, ref int errorCode)
        {
            _iConnectType = (int)plcSetting.connectType;
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
            try
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
            catch (Exception)
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
                string cmd = "@00FA000000000010282";
                string straddress = address.ToString("X4");
                cmd += straddress;
                cmd += "00";
                string strLength = 1.ToString("X4");
                cmd += strLength;
                cmd += value.ToString("X4");
                string fcs = FCS(cmd);
                cmd += fcs;
                cmd += "*\r\n";

                bool bRtn = _serial.WriteData(cmd);
                if (!bRtn)
                {
                    errorCode = (int)PLCErrorCode.Serial_SendData_Error;
                    return false;
                }

                DateTime dtNow = DateTime.Now;
                //Thread.Sleep(10);
                while (true)
                {
                    if (_serial.IsDataReceived)
                    {
                        // 读取PLC响应
                        string ret = _serial.ReadString2();
                        //@00FA004000000001010000000340 *\r
                        string s1 = ret.Substring(0, 19);
                        if (s1 != "@00FA00400000000102")
                        {
                            errorCode = (int)PLCErrorCode.Serial_Check_Error;
                            return false;
                        }
                        s1 = ret.Substring(19, 4);
                        if (s1 != "0000")
                        {
                            errorCode = (int)PLCErrorCode.Serial_Check_Error;
                            return false;
                        }
                        return true;
                        //Thread.Sleep(10);
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

        private bool ReadOneDataBySerial(int address, ref short value, ref int errorCode)
        {
            lock (o1)
            {
                _serial.ClearStream();
                string cmd = "@00FA000000000010182";
                string straddress = address.ToString("X4");
                cmd += straddress;
                cmd += "00";
                string strLength = 1.ToString("X4");
                cmd += strLength;
                string fcs = FCS(cmd);
                cmd += fcs;
                cmd += "*\r\n";

                bool bRtn = _serial.WriteData(cmd);
                if (!bRtn)
                {
                    errorCode = (int)PLCErrorCode.Serial_SendData_Error;
                    return false;
                }

                DateTime dtNow = DateTime.Now;
                //Thread.Sleep(10);
                while (true)
                {
                    if (_serial.IsDataReceived)
                    {
                        // 读取PLC响应
                        string ret = _serial.ReadString2();
                        //@00FA004000000001010000000340 *\r
                        string s1 = ret.Substring(0, 19);
                        if (s1 != "@00FA00400000000101")
                        {
                            errorCode = (int)PLCErrorCode.Serial_Check_Error;
                            return false;
                        }
                        s1 = ret.Substring(19, 4);
                        if (s1 != "0000")
                        {
                            errorCode = (int)PLCErrorCode.Serial_Check_Error;
                            return false;
                        }
                        string data = ret.Substring(23, 4);
                        value = TranslateToShortValue(Encoding.ASCII.GetBytes(data));
                        return true;
                        //Thread.Sleep(10);
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
                string cmd = "@00FA000000000010282";
                string straddress = address.ToString("X4");
                cmd += straddress;
                cmd += "00";
                string strLength = value.Length.ToString("X4");
                cmd += strLength;
                for (int i = 0; i < value.Length; i++)
                {
                    cmd += value[i].ToString("X4");
                }
                string fcs = FCS(cmd);
                cmd += fcs;
                cmd += "*\r\n";

                bool bRtn = _serial.WriteData(cmd);
                if (!bRtn)
                {
                    errorCode = (int)PLCErrorCode.Serial_SendData_Error;
                    return false;
                }

                DateTime dtNow = DateTime.Now;
                //Thread.Sleep(10);
                while (true)
                {
                    if (_serial.IsDataReceived)
                    {
                        // 读取PLC响应
                        string ret = _serial.ReadString2();
                        //@00FA004000000001010000000340 *\r
                        string s1 = ret.Substring(0, 19);
                        if (s1 != "@00FA00400000000102")
                        {
                            errorCode = (int)PLCErrorCode.Serial_Check_Error;
                            return false;
                        }
                        s1 = ret.Substring(19, 4);
                        if (s1 != "0000")
                        {
                            errorCode = (int)PLCErrorCode.Serial_Check_Error;
                            return false;
                        }
                        return true;
                        //Thread.Sleep(10);
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

        private bool ReadMultiDataBySerial(int address, ref short[] value, ref int errorCode)
        {
            lock (o1)
            {
                _serial.ClearStream();
                string cmd = "@00FA000000000010182";
                string straddress = address.ToString("X4");
                cmd += straddress;
                cmd += "00";
                string strLength = value.Length.ToString("X4");
                cmd += strLength;
                string fcs = FCS(cmd);
                cmd += fcs;
                cmd += "*\r\n";

                bool bRtn = _serial.WriteData(cmd);
                if (!bRtn)
                {
                    errorCode = (int)PLCErrorCode.Serial_SendData_Error;
                    return false;
                }

                DateTime dtNow = DateTime.Now;
                //Thread.Sleep(20);
                while (true)
                {
                    if (_serial.IsDataReceived)
                    {
                        // 读取PLC响应
                        string ret = _serial.ReadString2();
                        //@00FA004000000001010000000340 *\r
                        string s1 = ret.Substring(0, 19);
                        if (s1 != "@00FA00400000000101")
                        {
                            errorCode = (int)PLCErrorCode.Serial_Check_Error;
                            return false;
                        }
                        s1 = ret.Substring(19, 4);
                        if (s1 != "0000")
                        {
                            errorCode = (int)PLCErrorCode.Serial_Check_Error;
                            return false;
                        }
                        for (int i = 0; i < value.Length; i++)
                        {
                            string data = ret.Substring(23 + 4 * i, 4);
                            value[i] = TranslateToShortValue(Encoding.ASCII.GetBytes(data));
                        }
                        return true;
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

        private bool OpenByTcp(PLCSetting plcSetting, ref int errorCode)
        {
            try
            {
                Ping p = new Ping();
                PingReply cc = p.Send(plcSetting.IP);
                if (p.Send(plcSetting.IP).Status != IPStatus.Success)
                {
                    errorCode = (int)PLCErrorCode.Tcp_Open_Error;
                    return false;
                }

                if (_tcp != null)
                {
                    _tcp.Close();
                }
                byte MyComputerJieDian = 0;
                //本机校验码是IP的最后一位
                Client[3] = MyComputerJieDian;
                //Client[3] = Convert.ToByte(plcSetting.IP.Substring(plcSetting.IP.LastIndexOf('.') + 1));
                _tcp = new TcpClient();
                _tcp.Connect(plcSetting.IP, plcSetting.TcpPort);
                _streamTcp = _tcp.GetStream();
                if (_tcp.Connected)
                {
                    byte[] Handshake = new byte[20];
                    Handshake[0] = 0x46;//F
                    Handshake[1] = 0x49;//I
                    Handshake[2] = 0x4e;//N
                    Handshake[3] = 0x53;//S

                    Handshake[4] = 0;
                    Handshake[5] = 0;
                    Handshake[6] = 0;
                    Handshake[7] = 0x0c;//Length长度

                    Handshake[8] = 0;
                    Handshake[9] = 0;
                    Handshake[10] = 0;
                    Handshake[11] = 0;//Command

                    Handshake[12] = 0;
                    Handshake[13] = 0;
                    Handshake[14] = 0;
                    Handshake[15] = 0;//Error Code

                    Handshake[16] = 0;
                    Handshake[17] = 0;
                    Handshake[18] = 0;
                    Handshake[19] = Client[3];//FINS Frame (本机节点)
                    //Handshake[19] = 0;

                    _streamTcp.Write(Handshake, 0, Handshake.Length);

                    byte[] buffer = new byte[24];
                    _streamTcp.Read(buffer, 0, buffer.Length);
                    byte[] checkBuff = new byte[16];
                    checkBuff[0] = 0x46;//F
                    checkBuff[1] = 0x49;//I
                    checkBuff[2] = 0x4e;//N
                    checkBuff[3] = 0x53;//S
                    checkBuff[4] = 0;
                    checkBuff[5] = 0;
                    checkBuff[6] = 0;
                    checkBuff[7] = 0x10;
                    checkBuff[8] = 0;
                    checkBuff[9] = 0;
                    checkBuff[10] = 0;
                    checkBuff[11] = 1;
                    checkBuff[12] = 0;
                    checkBuff[13] = 0;
                    checkBuff[14] = 0;
                    checkBuff[15] = 0;
                    bool bcheck = true;
                    for (int i = 0; i < checkBuff.Length; i++)
                    {
                        if (buffer[i] != checkBuff[i])
                        {
                            bcheck = false;
                            break;
                        }
                    }
                    if (bcheck)
                    {
                        Client[0] = buffer[16];
                        Client[1] = buffer[17];
                        Client[2] = buffer[18];
                        Client[3] = buffer[19];

                        Server[0] = buffer[20];
                        Server[1] = buffer[21];
                        Server[2] = buffer[22];
                        Server[3] = buffer[23];
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
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
            if (_tcp != null)
            {
                _tcp.Close();
            }
            return;
        }

        private bool WriteOneDataByTcp(int address, short value, ref int errorCode)
        {
            lock (o1)
            {
                _streamTcp.Flush();
                byte[] receivedata = new byte[2];
                byte[] sendData = new byte[2];
                sendData[0] = (byte)(value / 256);
                sendData[1] = (byte)value;
                int iError = SendByte("DM." + address.ToString() + ".1", false, false, ref receivedata, sendData);
                if (iError == 0)
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

        private bool ReadOneDataByTcp(int address, ref short value, ref int errorCode)
        {
            lock (o1)
            {
                _streamTcp.Flush();
                byte[] receivedata = new byte[2];
                int iError = SendByte("DM." + address.ToString() + ".1", false, true, ref receivedata, null);
                if (iError == 0)
                {
                    byte[] b = new byte[2];
                    b[0] = receivedata[1];
                    b[1] = receivedata[0];
                    value = BitConverter.ToInt16(b, 0);
                    return true;
                }
                else
                {
                    errorCode = (int)PLCErrorCode.Tcp_Check_Error;
                    return false;
                }
            }
        }

        private bool WriteMultiDataByTcp(int address, short[] value, ref int errorCode)
        {
            lock (o1)
            {
                _streamTcp.Flush();
                byte[] receivedata = new byte[value.Length * 2];
                byte[] sendData = new byte[value.Length * 2];
                for (int i = 0; i < value.Length; i++)
                {
                    sendData[i * 2] = (byte)(value[i] / 256);
                    sendData[i * 2 + 1] = (byte)value[i];
                }
                int iError = SendByte("DM." + address.ToString() + "." + value.Length, false, false, ref receivedata, sendData);
                if (iError == 0)
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

        private bool ReadMultiDataByTcp(int address, ref short[] value, ref int errorCode)
        {
            lock (o1)
            {
                _streamTcp.Flush();
                byte[] receivedata = new byte[value.Length * 2];
                int iError = SendByte("DM." + address.ToString() + "." + value.Length, false, true, ref receivedata, null);
                if (iError == 0)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        byte[] b = new byte[2];
                        b[0] = receivedata[i * 2 + 1];
                        b[1] = receivedata[i * 2];
                        value[i] = BitConverter.ToInt16(b, 0);
                    }
                    return true;
                }
                else
                {
                    errorCode = (int)PLCErrorCode.Tcp_Check_Error;
                    return false;
                }
            }
        }

        private byte[] addsToByte(string memory, bool isBit)
        {
            string[] AddrParts = memory.Split('.');
            byte[] CH = BitConverter.GetBytes(Convert.ToInt32(AddrParts[1]));
            byte[] Count = BitConverter.GetBytes(Convert.ToInt32(AddrParts[2]));
            byte[] Addrs = new byte[6];
            Addrs[1] = CH[1];
            Addrs[2] = CH[0];

            if (!isBit)   //字处理
            {
                switch (AddrParts[0])
                {
                    case "CIO":
                        Addrs[0] = 0xB0;
                        break;

                    case "WR":
                        Addrs[0] = 0xB1;
                        break;

                    case "DM":
                        Addrs[0] = 0x82;
                        break;

                    case "HR":
                        Addrs[0] = 0xB2;
                        break;

                    case "TIM":
                        Addrs[0] = 0x89;
                        break;

                    case "AR":
                        Addrs[0] = 0xB3;
                        break;

                    case "CNT":
                        Addrs[0] = 0x89;
                        break;

                    default:
                        Addrs[0] = 0x00;
                        break;
                }
                Addrs[3] = 0x00;
                Addrs[4] = Count[1];
                Addrs[5] = Count[0];//读写字的长度
            }
            else //位处理
            {
                switch (AddrParts[0])
                {
                    case "CIO":
                        Addrs[0] = 0x30;
                        break;

                    case "WR":
                        Addrs[0] = 0x31;
                        break;

                    case "DM":
                        Addrs[0] = 0x02;
                        break;

                    case "HR":
                        Addrs[0] = 0x32;
                        break;

                    case "TIM":
                        Addrs[0] = 0x09;
                        break;

                    case "AR":
                        Addrs[0] = 0x33;
                        break;

                    case "CNT":
                        Addrs[0] = 0x09;
                        break;

                    default:
                        Addrs[0] = 0x00;
                        break;
                }
                Addrs[3] = Count[0];
                Addrs[4] = 0x00;
                Addrs[5] = 0x01;//每次读写一位
            }

            return Addrs;
        }

        private int SendByte(string memory, bool isBit, bool isRead, ref byte[] reveiveData, byte[] writeDatas = null)
        {
            try
            {
                int dataLength;
                if (writeDatas == null)
                {
                    dataLength = 26;
                }
                else
                {
                    dataLength = writeDatas.Length + 26;
                }

                byte[] SendByte = new byte[dataLength + 8];
                SendByte[0] = 0x46;//F
                SendByte[1] = 0x49;//I
                SendByte[2] = 0x4e;//N
                SendByte[3] = 0x53;//S
                SendByte[4] = 0;//cmd length
                SendByte[5] = 0;
                SendByte[6] = 0;
                SendByte[7] = Convert.ToByte(dataLength);
                SendByte[8] = 0;//frame command
                SendByte[9] = 0;
                SendByte[10] = 0;
                SendByte[11] = 0x02;
                SendByte[12] = 0;//err
                SendByte[13] = 0;
                SendByte[14] = 0;
                SendByte[15] = 0;
                //command frame header
                SendByte[16] = 0x80;//ICF
                SendByte[17] = 0x00;//RSV
                SendByte[18] = 0x02;//GCT, less than 8 network layers
                SendByte[19] = 0x00;//DNA, local network
                SendByte[20] = Server[3];//DA1
                SendByte[21] = 0x00;//DA2, CPU unit
                SendByte[22] = 0x00;//SNA, local network
                SendByte[23] = Client[3];//SA1
                SendByte[24] = 0x00;//SA2, CPU unit
                SendByte[25] = Convert.ToByte(0);//SID
                SendByte[26] = 0x01;
                if (isRead)
                {
                    SendByte[27] = 0x01;
                }
                else
                {
                    SendByte[27] = 0x02;
                }

                byte[] head = addsToByte(memory, isBit);
                SendByte[28] = head[0];
                SendByte[29] = head[1];
                SendByte[30] = head[2];
                SendByte[31] = head[3];
                SendByte[32] = head[4];
                SendByte[33] = head[5];

                if (writeDatas != null)
                {
                    Array.Copy(writeDatas, 0, SendByte, 34, writeDatas.Length);
                }

                _streamTcp.Write(SendByte, 0, SendByte.Length);
                byte[] buffer = new byte[256];
                //Thread.Sleep(1000);
                _streamTcp.Read(buffer, 0, buffer.Length);
                int err = 0;
                if (buffer[0] != SendByte[0] || buffer[1] != SendByte[1] || buffer[2] != SendByte[2] || buffer[3] != SendByte[3])
                {
                    err = 1;//the head is err
                }

                if (err == 0 && buffer[11] == 3)
                {
                    switch (buffer[15])
                    {
                        case 0x01: err = 2; break;//the head is not 'FINS'
                        case 0x02: err = 3; break;//the data length is too long
                        case 0x03: err = 4; break;//the command is not supported
                    }
                }

                if (err == 0 && buffer[28] != 0 && buffer[29] != 0)
                {
                    err = 5; //end code err

                    switch (buffer[28])
                    {
                        case 0x00:
                            if (buffer[29] == 0x01) err = 6;//service canceled
                            break;

                        case 0x01:
                            switch (buffer[29])
                            {
                                case 0x01: err = 7; break; //local node not in network
                                case 0x02: err = 8; break; //token timeout
                                case 0x03: err = 9; break; //retries failed
                                case 0x04: err = 10; break; //too many send frames
                                case 0x05: err = 11; break; //node address range error
                                case 0x06: err = 12; break; //node address duplication
                            }
                            break;

                        case 0x02:
                            switch (buffer[29])
                            {
                                case 0x01: err = 13; break; //destination node not in network
                                case 0x02: err = 14; break; //unit missing
                                case 0x03: err = 15; break; //third node missing
                                case 0x04: err = 16; break; //destination node busy
                                case 0x05: err = 17; break; //response timeout
                            }
                            break;

                        case 0x03:
                            switch (buffer[29])
                            {
                                case 0x01: err = 18; break; //communications controller error
                                case 0x02: err = 19; break; //CPU unit error
                                case 0x03: err = 20; break; //controller error
                                case 0x04: err = 21; break; //unit number error
                            }
                            break;

                        case 0x04:
                            switch (buffer[29])
                            {
                                case 0x01: err = 22; break; //undefined command
                                case 0x02: err = 23; break; //not supported by model/version
                            }
                            break;

                        case 0x05:
                            switch (buffer[29])
                            {
                                case 0x01: err = 24; break; //destination address setting error
                                case 0x02: err = 25; break; //no routing tables
                                case 0x03: err = 26; break; //routing table error
                                case 0x04: err = 27; break; //too many relays
                            }
                            break;

                        case 0x10:
                            switch (buffer[29])
                            {
                                case 0x01: err = 28; break; //command too long
                                case 0x02: err = 29; break; //command too short
                                case 0x03: err = 30; break; //elements/data don't match
                                case 0x04: err = 31; break; //command format error
                                case 0x05: err = 32; break; //header error
                            }
                            break;

                        case 0x11:
                            switch (buffer[29])
                            {
                                case 0x01: err = 33; break; //area classification missing
                                case 0x02: err = 34; break; //access size error
                                case 0x03: err = 35; break; //address range error
                                case 0x04: err = 36; break; //address range exceeded
                                case 0x06: err = 37; break; //program missing
                                case 0x09: err = 38; break; //relational error
                                case 0x0a: err = 39; break; //duplicate data access
                                case 0x0b: err = 40; break; //response too long
                                case 0x0c: err = 41; break; //parameter error
                            }
                            break;

                        case 0x20:
                            switch (buffer[29])
                            {
                                case 0x02: err = 42; break; //protected
                                case 0x03: err = 43; break; //table missing
                                case 0x04: err = 44; break; //data missing
                                case 0x05: err = 45; break; //program missing
                                case 0x06: err = 46; break; //file missing
                                case 0x07: err = 47; break; //data mismatch
                            }
                            break;

                        case 0x21:
                            switch (buffer[29])
                            {
                                case 0x01: err = 48; break; //read-only
                                case 0x02: err = 49; break; //protected , cannot write data link table
                                case 0x03: err = 50; break; //cannot register
                                case 0x05: err = 51; break; //program missing
                                case 0x06: err = 52; break; //file missing
                                case 0x07: err = 53; break; //file name already exists
                                case 0x08: err = 54; break; //cannot change
                            }
                            break;

                        case 0x22:
                            switch (buffer[29])
                            {
                                case 0x01: err = 55; break; //not possible during execution
                                case 0x02: err = 56; break; //not possible while running
                                case 0x03: err = 57; break; //wrong PLC mode
                                case 0x04: err = 58; break; //wrong PLC mode
                                case 0x05: err = 59; break; //wrong PLC mode
                                case 0x06: err = 60; break; //wrong PLC mode
                                case 0x07: err = 61; break; //specified node not polling node
                                case 0x08: err = 62; break; //step cannot be executed
                            }
                            break;

                        case 0x23:
                            switch (buffer[29])
                            {
                                case 0x01: err = 63; break; //file device missing
                                case 0x02: err = 64; break; //memory missing
                                case 0x03: err = 65; break; //clock missing
                            }
                            break;

                        case 0x24:
                            if (buffer[29] == 0x01) err = 66; //table missing
                            break;

                        case 0x25:
                            switch (buffer[29])
                            {
                                case 0x02: err = 67; break; //memory error
                                case 0x03: err = 68; break; //I/O setting error
                                case 0x04: err = 69; break; //too many I/O points
                                case 0x05: err = 70; break; //CPU bus error
                                case 0x06: err = 71; break; //I/O duplication
                                case 0x07: err = 72; break; //CPU bus error
                                case 0x09: err = 73; break; //SYSMAC BUS/2 error
                                case 0x0a: err = 74; break; //CPU bus unit error
                                case 0x0d: err = 75; break; //SYSMAC BUS No. duplication
                                case 0x0f: err = 76; break; //memory error
                                case 0x10: err = 77; break; //SYSMAC BUS terminator missing
                            }
                            break;

                        case 0x26:
                            switch (buffer[29])
                            {
                                case 0x01: err = 78; break; //no protection
                                case 0x02: err = 79; break; //incorrect password
                                case 0x04: err = 80; break; //protected
                                case 0x05: err = 81; break; //service already executing
                                case 0x06: err = 82; break; //service stopped
                                case 0x07: err = 83; break; //no execution right
                                case 0x08: err = 84; break; //settings required before execution
                                case 0x09: err = 85; break; //necessary items not set
                                case 0x0a: err = 86; break; //number already defined
                                case 0x0b: err = 87; break; //error will not clear
                            }
                            break;

                        case 0x30:
                            if (buffer[29] == 0x01) err = 88; //no access right
                            break;

                        case 0x40:
                            if (buffer[29] == 0x01) err = 89;//service aborted
                            break;
                    }
                }

                if (err == 0 && isRead)
                {
                    Array.Copy(buffer, 30, reveiveData, 0, reveiveData.Length);
                }
                return err;
            }
            catch (Exception)
            {
                return 100;
            }
        }

        private string FCS(string s)　　//帧校验函数FCS
        {
            //获取s对应的字节数组
            byte[] b = Encoding.ASCII.GetBytes(s);
            // xorResult 存放校验结果。注意：初值去首元素值！
            byte xorResult = b[0];
            // 求xor校验和。注意：XOR运算从第二元素开始
            for (int i = 1; i < b.Length; i++)
            {
                //**进行异或运算，^=就是异或运算符，具体可查阅异或运算
                //**异或运算：两个二进制数的每一位进行比较，如果相同则为0，不同则为1,如下面2个10进制数37、     48的二进制值异或结果为21
                //**  37(10)       100101(2)
                //**  48(10)       110000(2)
                //**  21(10)       010101(2)
                //**这里的意思是：如a^=b，就是a与b先进行异或比较，得出的结果赋值给a；
                xorResult ^= b[i];
            }
            //**Convert.ToString(xorResult, 16):将当前值转换为16进制；ToUpper()：结果大写；
            //**这里的意思是：将xorResult转换成16进制并大写；

            //**（//**返回的结果为一个两个ASCII码的异或值）
            return xorResult.ToString("X2");
        }

        private short TranslateToShortValue(byte[] buf)
        {
            int lowByte = AsciiToInt(buf[0]) * 16 + AsciiToInt(buf[1]);
            int highByte = AsciiToInt(buf[2]) * 16 + AsciiToInt(buf[3]);
            int intVal = highByte + lowByte * 256;
            return (short)intVal;
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
    }
}