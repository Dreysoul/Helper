using System;
using System.Runtime.InteropServices;

namespace YiRongMachine
{
    public class HuiChuanPLCHelper
    {
        #region 标准库

        [DllImport("StandardModbusApi.dll", EntryPoint = "Init_ETH_String", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Init_ETH_String(string sIpAddr, int nNetId = 0, int IpPort = 502);

        [DllImport("StandardModbusApi.dll", EntryPoint = "Exit_ETH", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool Exit_ETH(int nNetId = 0);

        [DllImport("StandardModbusApi.dll", EntryPoint = "H5u_Write_Soft_Elem", CallingConvention = CallingConvention.Cdecl)]
        public static extern int H5u_Write_Soft_Elem(SoftElemType eType, int nStartAddr, int nCount, byte[] pValue, int nNetId = 0);

        [DllImport("StandardModbusApi.dll", EntryPoint = "H5u_Read_Soft_Elem", CallingConvention = CallingConvention.Cdecl)]
        public static extern int H5u_Read_Soft_Elem(SoftElemType eType, int nStartAddr, int nCount, byte[] pValue, int nNetId = 0);

        [DllImport("StandardModbusApi.dll", EntryPoint = "H5u_Read_Device_Block", CallingConvention = CallingConvention.Cdecl)]
        public static extern int H5u_Read_Device_Block(SoftElemType eType, int nStartAddr, int nCount, byte[] pValue, int nNetId = 0);

        [DllImport("StandardModbusApi.dll", EntryPoint = "H5u_Write_Device_Block", CallingConvention = CallingConvention.Cdecl)]
        public static extern int H5u_Write_Device_Block(SoftElemType eType, int nStartAddr, int nCount, byte[] pValue, int nNetId = 0);

        #endregion 标准库

        private object o1 = new object();
        private int _iConnectType = 0;

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
            errorCode = (int)PLCErrorCode.Serial_NoSuchConnect_Error;
            return false;
        }

        private void CloseBySerial()
        {
        }

        private bool WriteOneDataBySerial(int address, short value, ref int errorCode)
        {
            errorCode = (int)PLCErrorCode.Serial_NoSuchConnect_Error;
            return false;
        }

        private bool ReadOneDataBySerial(int address, ref short value, ref int errorCode)
        {
            errorCode = (int)PLCErrorCode.Serial_NoSuchConnect_Error;
            return false;
        }

        private bool WriteMultiDataBySerial(int address, short[] value, ref int errorCode)
        {
            errorCode = (int)PLCErrorCode.Serial_NoSuchConnect_Error;
            return false;
        }

        private bool ReadMultiDataBySerial(int address, ref short[] value, ref int errorCode)
        {
            errorCode = (int)PLCErrorCode.Serial_NoSuchConnect_Error;
            return false;
        }

        private int nNetId = 0;
        private int nIpPort = 502;

        private bool OpenByTcp(PLCSetting plcSetting, ref int errorCode)
        {
            bool bRtn = Init_ETH_String(plcSetting.IP, nNetId, nIpPort);
            if (bRtn)
            {
                return true;
            }
            else
            {
                errorCode = (int)PLCErrorCode.Tcp_Open_Error;
                return false;
            }
        }

        private void CloseByTcp()
        {
            Exit_ETH(nNetId);
        }

        private bool WriteOneDataByTcp(int address, short value, ref int errorCode)
        {
            lock (o1)
            {
                byte[] pValue = new byte[2];//缓冲区

                //把要写的数据存入缓冲区，备写
                pValue[0] = (byte)(value % 256);
                pValue[1] = (byte)(value / 256);

                //调用api写数据
                int nRet = H5u_Write_Soft_Elem(SoftElemType.REGI_H5U_D, address, 1, pValue, nNetId);
                if (nRet == 1)
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
                byte[] pValue = new byte[2];//缓冲区
                int nRet = H5u_Read_Soft_Elem(SoftElemType.REGI_H5U_D, address, 1, pValue, nNetId);
                if (1 == nRet)
                {
                    value = (short)(pValue[0] + pValue[1] * 256);
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
                byte[] target = new byte[value.Length * 2];
                for (int i = 0; i < value.Length; i++)
                {
                    byte[] b = BitConverter.GetBytes(value[i]);
                    target[2 * i] = b[0];
                    target[2 * i + 1] = b[1];
                }

                //调用api写数据
                int nRet = H5u_Write_Soft_Elem(SoftElemType.REGI_H5U_D, address, value.Length, target, nNetId);
                if (nRet == 1)
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
                byte[] pValue = new byte[value.Length * 2];//缓冲区
                int nRet = H5u_Read_Soft_Elem(SoftElemType.REGI_H5U_D, address, 1, pValue, nNetId);
                if (1 == nRet)
                {
                    for (int i = 0; i < value.Length; i++)
                    {
                        value[i] = (short)(pValue[2 * i] + pValue[2 * i + 1] * 256);
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
    }

    public enum SoftElemType
    {
        //AM600
        ELEM_QX = 0,     //QX元件

        ELEM_MW = 1,     //MW元件
        ELEM_X = 2,		 //X元件(对应QX200~QX300)
        ELEM_Y = 3,		 //Y元件(对应QX300~QX400)

        //H3U
        REGI_H3U_Y = 0x20,       //Y元件的定义

        REGI_H3U_X = 0x21,		//X元件的定义
        REGI_H3U_S = 0x22,		//S元件的定义
        REGI_H3U_M = 0x23,		//M元件的定义
        REGI_H3U_TB = 0x24,		//T位元件的定义
        REGI_H3U_TW = 0x25,		//T字元件的定义
        REGI_H3U_CB = 0x26,		//C位元件的定义
        REGI_H3U_CW = 0x27,		//C字元件的定义
        REGI_H3U_DW = 0x28,		//D字元件的定义
        REGI_H3U_CW2 = 0x29,	    //C双字元件的定义
        REGI_H3U_SM = 0x2a,		//SM
        REGI_H3U_SD = 0x2b,		//
        REGI_H3U_R = 0x2c,		//

        //H5u
        REGI_H5U_Y = 0x30,       //Y元件的定义

        REGI_H5U_X = 0x31,		//X元件的定义
        REGI_H5U_S = 0x32,		//S元件的定义
        REGI_H5U_M = 0x33,		//M元件的定义
        REGI_H5U_B = 0x34,       //B元件的定义
        REGI_H5U_D = 0x35,       //D字元件的定义
        REGI_H5U_R = 0x36,       //R字元件的定义
    }
}