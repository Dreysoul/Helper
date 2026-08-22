namespace YiRongMachine
{
    public class LightHelper
    {
        private SerialPortByNilifu com = new SerialPortByNilifu();

        public LightHelper()
        {
        }

        public bool OpenLightPort(string PortName)
        {
            if (PortName == null)
                return false;
            if (PortName == "")
                return false;

            if (GlobalVariable.light_type == LightType.HaiShi_12T)
            {
                com.PortName = PortName;
                com.BaudRate = 19200;
                com.DataBits = 8;
                com.StopBits = 1;
                com.Parity = "NONE";
            }
            else
            {
                com.PortName = PortName;
                com.BaudRate = 19200;
                com.DataBits = 8;
                com.StopBits = 2;
                com.Parity = "EVEN";
            }
            bool bRtn = com.OpenPort();

            setL();

            return bRtn;
        }

        public void CloseLightPort()
        {
            com.Close();
        }

        /*
         * channel 01 ~ 12 总共12路光源
         * level 000 ~ 999级别
         */

        public void setLight(int channel, int level)
        {
            string strChannel = channel.ToString().PadLeft(2, '0');
            string strlevel = level.ToString().PadLeft(3, '0');
            string strCmd = "S" + strChannel + strlevel + "#";
            com.SerialPortClient.Write(strCmd);
        }

        public void setL()
        {
            string strCmd = "SL#";
            try
            {
                com.SerialPortClient.Write(strCmd);
            }
            catch
            {
            }
        }

        public void OperateLight(string cmd)
        {
            if (com.SerialPortClient == null)
                return;

            if (GlobalVariable.light_type == LightType.HaiShi_12T)
            {
                cmd = cmd.Replace("SA0", "S01");
                cmd = cmd.Replace("SB0", "S02");
                cmd = cmd.Replace("SC0", "S03");
                cmd = cmd.Replace("SD0", "S04");
                cmd = cmd.Replace("SE0", "S05");
                cmd = cmd.Replace("SF0", "S06");
            }
            com.SerialPortClient.Write(cmd);
        }
    }
}