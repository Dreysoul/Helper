using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YiRongMachine
{
    /// <summary>
    /// 用不了
    /// </summary>
    public  interface iPLC
    {
        bool Open(PLCSetting plcSetting, ref string ErrorMsg);
        void Close();
        bool WriteOneData(int addr, short value);
        bool ReadOneData(int addr, ref short value);
        bool WriteMultiData(int addr, short[] value);
        bool ReadMultiData(int addr, ref short[] value);


        bool OpenBySerial(PLCSetting plcSetting);
        void CloseBySerial();
        bool WriteOneDataBySerial(int address, short value);
        bool ReadOneDataBySerial(int address, ref short value);
        bool WriteMultiDataBySerial(int address, short[] value);
        bool ReadMultiDataBySerial(int address, ref short[] value);


        bool OpenByTcp(PLCSetting plcSetting, ref string ErrorMsg);
        void CloseByTcp();
        bool WriteOneDataByTcp(int address, short value);
        bool ReadOneDataByTcp(int address, ref short value);
        bool WriteMultiDataByTcp(int address, short[] value);
        bool ReadMultiDataByTcp(int address, ref short[] value);
    }
}
