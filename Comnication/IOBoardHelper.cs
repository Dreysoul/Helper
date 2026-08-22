using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace YiRongMachine
{
    public class IOBoardHelper
    {
        #region 标准库

        [DllImport("CH367DLL64.dll", EntryPoint = "CH367GetVersion", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong CH367GetVersion();

        [DllImport("CH367DLL64.dll", EntryPoint = "CH367mOpenDevice", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr CH367mOpenDevice(int iIndex, int iEnableMemory, int iEnableInterrupt, byte iInterruptMode);

        [DllImport("CH367DLL64.dll", EntryPoint = "CH367CloseDevice", CallingConvention = CallingConvention.Cdecl)]
        public static extern ulong CH367CloseDevice(int iIndex);

        [DllImport("CH367DLL64.dll", EntryPoint = "CH367mAccessBlock", CallingConvention = CallingConvention.StdCall)]
        public static extern int CH367mAccessBlock(int iIndex, int iAccessMode, IntPtr iAddr, IntPtr ioBuffer, int iLength);

        [DllImport("CH367DLL64.dll", EntryPoint = "CH367mGetIoBaseAddr", CallingConvention = CallingConvention.StdCall)]
        public static extern int CH367mGetIoBaseAddr(int iIndex, IntPtr oIoBaseAddr);

        [DllImport("CH367DLL64.dll", EntryPoint = "CH367mWriteIoByte", CallingConvention = CallingConvention.StdCall)]
        public static extern int CH367mWriteIoByte(int iIndex, IntPtr iAddr, byte val);

        #endregion 标准库

        private Mutex mutexR = new Mutex();
        private Mutex mutexW = new Mutex();

        public enum enInputType
        {
            DuanMianA_CS_QD,    //
            DuanMianA_GY_DW,    //
            WaiYuanA_CS_QD,
            Reserve1,
            DaoJiaoA_CS_QD,
            Reserve2,
            NeiKong_CS_QD,
            Reserve3,
            DuanMianB_CS_QD,
            DuanMianB_GY_DW,
            WaiYuanB_CS_QD,
            Reserve4,
            DaoJiaoB_CS_QD,
            Reserve5,
            NeiQuanB_CS_QD,
            Reserve6
        }

        public enum enOutputType
        {
            DuanMianA_PZ_WC,
            DuanMianA_OK,
            DuanMianA_NG,
            DuanMianA_GY_YD,
            Reserve1,
            WaiYuanA_PZ_WC,
            WaiYuanA_OK,
            WaiYuanA_NG,
            WaiYuanA_TXG,
            WaiYuanA_TZG,
            DaoJiaoA_PZ_WC,
            DaoJiaoA_OK,
            DaoJiaoA_NG,
            DaoJiaoA_TXG,
            Reserve2,
            NeiKong_PZ_WC,
            NeiKong_OK,
            NeiKong_NG,
            NeiKong_BG,
            Reserve3,
            DuanMianB_PZ_WC,
            DuanMianB_OK,
            DuanMianB_NG,
            DuanMianB_GY_YD,
            Reserve4,
            WaiYuanB_PZ_WC,
            WaiYuanB_OK,
            WaiYuanB_NG,
            WaiYuanB_TXG,
            Reserve5,
            DaoJiaoB_PZ_WC,
            DaoJiaoB_OK,
            DaoJiaoB_NG,
            DaoJiaoB_TXG,
            Reserve6,
            NeiQuanB_PZ_WC,
            NeiQuanB_OK,
            NeiQuanB_NG,
            NeiQuanB_BG,
            Reserve7,
            NeiQuanB_TXG
        }

        public bool readInput(int index)
        {
            mutexR.WaitOne();
            IntPtr ioBuf = Marshal.AllocHGlobal(4);
            AccessBlock(0, 0x12, (IntPtr)(0x10 | baseAdr), (IntPtr)(ioBuf), 4);
            byte[] arr = new byte[4];
            Marshal.Copy(ioBuf, arr, 0, 4);

            int ip = index / 8;
            int ir = index % 8;
            if (((arr[ip] >> ir) & (0x01)) == 1)
            {
                mutexR.ReleaseMutex();
                return true;
            }
            mutexR.ReleaseMutex();
            return false;
        }

        private byte[] arr1 = new byte[4];
        private byte[] arr2 = new byte[4];
        private int baseAdr = 0;

        public bool setOutput(int index, bool bv)
        {
            bool bret = false;
            mutexW.WaitOne();
            //LogHelper.AddNeiKongLog( "zz "+index.ToString());
            int ip = index / 8;
            int ir = index % 8;
            if (ip < 4)
            {
                if (bv)
                {
                    arr1[ip] |= (byte)((byte)(0x01) << ir);
                }
                else
                {
                    arr1[ip] &= (byte)(~((byte)(0x01) << ir));
                }

                IntPtr ioBuf = Marshal.AllocHGlobal(4);
                Marshal.Copy(arr1, 0, ioBuf, 4);
                //0x16 写
                // LogHelper.AddNeiKongLog("11");
                bret = AccessBlock(0, 0x16, (IntPtr)(0x18 | baseAdr), (IntPtr)(ioBuf), 4);
            }
            else
            {
                ip -= 4;
                if (bv)
                {
                    arr2[ip] |= (byte)((byte)(0x01) << ir);
                }
                else
                {
                    arr2[ip] &= (byte)(~((byte)(0x01) << ir));
                }

                IntPtr ioBuf = Marshal.AllocHGlobal(4);
                Marshal.Copy(arr2, 0, ioBuf, 4);
                //0x16 写
                //LogHelper.AddNeiKongLog("22");
                bret = AccessBlock(0, 0x16, (IntPtr)(0x1c | baseAdr), (IntPtr)(ioBuf), 4);
            }
            if (!bret)
            {
                LogHelper.AddCommLog("IO:" + index.ToString() + " 写入失败");
            }
            mutexW.ReleaseMutex();
            return bret;
        }

        public IOBoardHelper()
        {
            for (int i = 0; i < 4; i++)
            {
                arr1[i] = 0x00;
                arr2[i] = 0x00;
            }
        }

        public ulong GetVersion()
        {
            return CH367GetVersion();
        }

        public IntPtr OpenDevice(int iIndex, bool iEnableMemory, bool iEnableInterrupt, byte iInterruptMode)
        {
            return CH367mOpenDevice(iIndex, iEnableMemory ? 1 : 0, iEnableInterrupt ? 1 : 0, iInterruptMode);
        }

        public int WriteIoByte(int iIndex)
        {
            return CH367mWriteIoByte(iIndex, (IntPtr)(0xfa | baseAdr), 0x47);
        }

        public void ReadBaseAdr(int iIndex)
        {
            IntPtr baseAdrBuf = Marshal.AllocHGlobal(1024 * 5);
            CH367mGetIoBaseAddr(iIndex, baseAdrBuf);
            byte[] arr = new byte[0xE8];
            Marshal.Copy(baseAdrBuf, arr, 0, 0xE8);
            baseAdr = arr[0] | arr[1] << 8;
            int aa = 0;
        }

        public ulong CloseDevice(int iIndex)
        {
            return CH367CloseDevice(iIndex);
        }

        public bool AccessBlock(int iIndex, int iAccessMode, IntPtr iAddr, IntPtr ioBuffer, int iLength)
        {
            for (int i = 0; i < 100; i++)
            {
                int iRet = CH367mAccessBlock(iIndex, iAccessMode, iAddr, ioBuffer, iLength);
                if (iRet == 1)
                    return true;
            }
            return false;
        }
    }
}