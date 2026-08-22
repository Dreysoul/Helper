namespace YiRongMachine
{
    public class PLCControl
    {
        private PLCType _plcType = PLCType.SanLingFX3GPLC;
        private SanLingFX3GPLCHelper sanlingPLCHelper;
        private OmronCP1HPLCHelper omronPLCHelper;
        private SiemensPLCHelper siemensPLCHelper;
        private HuiChuanPLCHelper huiChuanPLCHelper;
        private ModbusRTUPLCHelper modbusRtuPLCHelper;
        private TaiDaPLCHelper taiDaPLCHelper;

        public PLCControl()
        {
            sanlingPLCHelper = new SanLingFX3GPLCHelper();
            omronPLCHelper = new OmronCP1HPLCHelper();
            siemensPLCHelper = new SiemensPLCHelper();
            huiChuanPLCHelper = new HuiChuanPLCHelper();
            modbusRtuPLCHelper = new ModbusRTUPLCHelper();
            taiDaPLCHelper = new TaiDaPLCHelper();
        }

        /// <summary>
        /// 打开PLC
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public bool Open(PLCSetting plcSetting, ref int errorCode)
        {
            _plcType = (PLCType)plcSetting.plcType;
            if (_plcType == PLCType.SanLingFX3GPLC)
            {
                return sanlingPLCHelper.Open(plcSetting, ref errorCode);
            }
            else if (_plcType == PLCType.OmronPLC)
            {
                return omronPLCHelper.Open(plcSetting, ref errorCode);
            }
            else if (_plcType == PLCType.SiemensPLC)
            {
                return siemensPLCHelper.Open(plcSetting, ref errorCode);
            }
            else if (_plcType == PLCType.HuiChuanPLC)
            {
                return huiChuanPLCHelper.Open(plcSetting, ref errorCode);
            }
            else if (_plcType == PLCType.ModbusRtuPLC)
            {
                return modbusRtuPLCHelper.Open(plcSetting, ref errorCode);
            }
            else if (_plcType == PLCType.TaiDaPLC)
            {
                return taiDaPLCHelper.Open(plcSetting, ref errorCode);
            }
            return true;
        }

        /// <summary>
        /// 关闭PLC
        /// </summary>
        public void Close()
        {
            if (_plcType == PLCType.SanLingFX3GPLC)
            {
                sanlingPLCHelper.Close();
            }
            else if (_plcType == PLCType.OmronPLC)
            {
                omronPLCHelper.Close();
            }
            else if (_plcType == PLCType.SiemensPLC)
            {
                siemensPLCHelper.Close();
            }
            else if (_plcType == PLCType.HuiChuanPLC)
            {
                huiChuanPLCHelper.Close();
            }
            else if (_plcType == PLCType.ModbusRtuPLC)
            {
                modbusRtuPLCHelper.Close();
            }
            else if (_plcType == PLCType.TaiDaPLC)
            {
                taiDaPLCHelper.Close();
            }
        }

        public bool WriteOneData(int DBNumber, int addr, short value, ref int errorCode)
        {
            if (_plcType == PLCType.SanLingFX3GPLC)
            {
                return sanlingPLCHelper.WriteOneData(addr, value, ref errorCode);
            }
            else if (_plcType == PLCType.OmronPLC)
            {
                return omronPLCHelper.WriteOneData(addr, value, ref errorCode);
            }
            else if (_plcType == PLCType.SiemensPLC)
            {
                return siemensPLCHelper.WriteOneData(DBNumber, addr, value, ref errorCode);
            }
            else if (_plcType == PLCType.HuiChuanPLC)
            {
                return huiChuanPLCHelper.WriteOneData(addr, value, ref errorCode);
            }
            else if (_plcType == PLCType.ModbusRtuPLC)
            {
                return modbusRtuPLCHelper.WriteOneData(addr, value, ref errorCode);
            }
            else if (_plcType == PLCType.TaiDaPLC)
            {
                return taiDaPLCHelper.WriteOneData(addr, value, ref errorCode);
            }
            return true;
        }

        public bool ReadOneData(int DBNumber, int addr, ref short value, ref int errorCode)
        {
            if (_plcType == PLCType.SanLingFX3GPLC)
            {
                return sanlingPLCHelper.ReadOneData(addr, ref value, ref errorCode);
            }
            else if (_plcType == PLCType.OmronPLC)
            {
                return omronPLCHelper.ReadOneData(addr, ref value, ref errorCode);
            }
            else if (_plcType == PLCType.SiemensPLC)
            {
                return siemensPLCHelper.ReadOneData(DBNumber, addr, ref value, ref errorCode);
            }
            else if (_plcType == PLCType.HuiChuanPLC)
            {
                return huiChuanPLCHelper.ReadOneData(addr, ref value, ref errorCode);
            }
            else if (_plcType == PLCType.ModbusRtuPLC)
            {
                return modbusRtuPLCHelper.ReadOneData(addr, ref value, ref errorCode);
            }
            else if (_plcType == PLCType.TaiDaPLC)
            {
                return taiDaPLCHelper.ReadOneData(addr, ref value, ref errorCode);
            }
            return true;
        }

        public bool WriteMultiData(int DBNumber, int addr, short[] value, ref int errorCode)
        {
            if (_plcType == PLCType.SanLingFX3GPLC)
            {
                return sanlingPLCHelper.WriteMultiData(addr, value, ref errorCode);
            }
            else if (_plcType == PLCType.OmronPLC)
            {
                return omronPLCHelper.WriteMultiData(addr, value, ref errorCode);
            }
            else if (_plcType == PLCType.SiemensPLC)
            {
                return siemensPLCHelper.WriteMultiData(DBNumber, addr, value, ref errorCode);
            }
            else if (_plcType == PLCType.HuiChuanPLC)
            {
                return huiChuanPLCHelper.WriteMultiData(addr, value, ref errorCode);
            }
            else if (_plcType == PLCType.ModbusRtuPLC)
            {
                return modbusRtuPLCHelper.WriteMultiData(addr, value, ref errorCode);
            }
            else if (_plcType == PLCType.TaiDaPLC)
            {
                return taiDaPLCHelper.WriteMultiData(addr, value, ref errorCode);
            }
            return true;
        }

        public bool ReadMultiData(int DBNumber, int addr, ref short[] value, ref int errorCode)
        {
            if (_plcType == PLCType.SanLingFX3GPLC)
            {
                return sanlingPLCHelper.ReadMultiData(addr, ref value, ref errorCode);
            }
            else if (_plcType == PLCType.OmronPLC)
            {
                return omronPLCHelper.ReadMultiData(addr, ref value, ref errorCode);
            }
            else if (_plcType == PLCType.SiemensPLC)
            {
                return siemensPLCHelper.ReadMultiData(DBNumber, addr, ref value, ref errorCode);
            }
            else if (_plcType == PLCType.HuiChuanPLC)
            {
                return huiChuanPLCHelper.ReadMultiData(addr, ref value, ref errorCode);
            }
            else if (_plcType == PLCType.ModbusRtuPLC)
            {
                return modbusRtuPLCHelper.ReadMultiData(addr, ref value, ref errorCode);
            }
            else if (_plcType == PLCType.TaiDaPLC)
            {
                return taiDaPLCHelper.ReadMultiData(addr, ref value, ref errorCode);
            }
            return true;
        }

        public static string[] PLCErrorMsg = {"打开串口失败","没有串口连接程序","串口发送信息失败","串口接收信息校验失败","串口接收信息超时",
                                               "打开网口失败","没有网口连接程序","网口发送信息失败","网口接收信息校验失败","网口接收信息超时"};
    }

    /// <summary>
    /// PLC的种类
    /// </summary>
    public enum PLCType
    {
        SanLingFX3GPLC,
        OmronPLC,
        SiemensPLC,
        HuiChuanPLC,
        ModbusRtuPLC,
        TaiDaPLC,
    }

    public struct PLCSetting
    {
        public int plcType;
        public int connectType;
        public string IP;
        public int TcpPort;
        public string SerialPort;
        public int BaudRate;
        public int DataBits;
        public int StopBits;
        public string Parity;
        public int SiemensPLCDBNumber;
        public int HardwareNo;
        public int Offset;
    }

    public enum PLCErrorCode
    {
        Serial_Open_Error,
        Serial_NoSuchConnect_Error,
        Serial_SendData_Error,
        Serial_Check_Error,
        Serial_LongTime_Error,

        Tcp_Open_Error,
        Tcp_NoSuchConnect_Error,
        Tcp_SendData_Error,
        Tcp_Check_Error,
        Tcp_LongTime_Error,
    }
}