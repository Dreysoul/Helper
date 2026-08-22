using System.Text;

namespace YiRongMachine
{
    public class ModbusAscii
    {
        public static string Read(int hardwareNo, int address, int number)
        {
            byte[] originaldata = new byte[6];
            originaldata[0] = (byte)hardwareNo;
            originaldata[1] = 0x03;
            originaldata[2] = (byte)(address / 256);
            originaldata[3] = (byte)(address % 256);
            originaldata[4] = (byte)(number / 256);
            originaldata[5] = (byte)(number % 256);
            string LRC = GetLRC(originaldata);

            StringBuilder sb = new StringBuilder();
            sb.Append(":");
            //设备地址
            sb.Append(hardwareNo.ToString("X2"));
            //功能码
            sb.Append("03");
            //起始地址
            sb.Append(address.ToString("X4"));
            //数据长度
            sb.Append(number.ToString("X4"));
            sb.Append(LRC);
            sb.Append("\x0D\x0A");

            return sb.ToString();
        }

        public static bool CheckRead(int hardwareNo, int number, string response, ref short[] buffer)
        {
            if (response.Length == 10 + number * 4)
            {
                string strHardware = response.Substring(1, 2);
                string strFunction = response.Substring(3, 2);
                string strNumber = response.Substring(5, 2);
                if (response[0] == ':' && strHardware == hardwareNo.ToString("X2") && strFunction == "03" && strNumber == (number * 2).ToString("X2"))
                {
                    for (int i = 0; i < number; i++)
                    {
                        string s = response.Substring(7 + 4 * i, 4);
                        byte[] num = Encoding.ASCII.GetBytes(s);
                        buffer[i] = TranslateToShortValue(num);
                        //buffer[i]= (short)BitConverter.ToInt32(num,0);
                    }
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

        public static string WriteOne(int hardwareNo, int address, short data)
        {
            byte[] originaldata = new byte[6];
            originaldata[0] = (byte)hardwareNo;
            originaldata[1] = 0x06;
            originaldata[2] = (byte)(address / 256);
            originaldata[3] = (byte)(address % 256);
            originaldata[4] = (byte)(data / 256);
            originaldata[5] = (byte)(data % 256);
            string LRC = GetLRC(originaldata);

            StringBuilder sb = new StringBuilder();
            sb.Append(":");
            //设备地址
            sb.Append(hardwareNo.ToString("X2"));
            //功能码
            sb.Append("06");
            //起始地址
            sb.Append(address.ToString("X4"));
            //数据长度
            sb.Append(data.ToString("X4"));
            sb.Append(LRC);
            sb.Append("\x0D\x0A");

            return sb.ToString();
        }

        public static bool CheckWriteOne(string response, string send)
        {
            if (response == send)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string WriteMulti(int hardwareNo, int address, short[] data)
        {
            int number = data.Length;
            byte[] originaldata = new byte[17 + number * 2];
            originaldata[0] = (byte)hardwareNo;
            originaldata[1] = 0x10;
            originaldata[2] = (byte)(address / 256);
            originaldata[3] = (byte)(address % 256);
            originaldata[4] = (byte)(number / 256);
            originaldata[5] = (byte)(number % 256);
            originaldata[6] = (byte)(number * 2);
            for (int i = 0; i < number; i++)
            {
                originaldata[7 + i * 2] = (byte)(data[i] / 256);
                originaldata[8 + i * 2] = (byte)(data[i] % 256);
            }
            string LRC = GetLRC(originaldata);

            StringBuilder sb = new StringBuilder();
            sb.Append(":");
            //设备地址
            sb.Append(hardwareNo.ToString("X2"));
            //功能码
            sb.Append("10");
            //起始地址
            sb.Append(address.ToString("X4"));
            //数据长度
            sb.Append(number.ToString("X4"));
            //字节长度
            sb.Append((number * 2).ToString("X2"));
            //详细数据
            for (int i = 0; i < number; i++)
            {
                sb.Append(data[i].ToString("X4"));
            }
            sb.Append(LRC);
            sb.Append("\x0D\x0A");

            return sb.ToString();
        }

        public static bool CheckWriteMulti(int hardwareNo, int address, int number, string response)
        {
            if (response.Length == 16)
            {
                string strHardware = response.Substring(1, 2);
                string strFunction = response.Substring(3, 2);
                string strAddress = response.Substring(5, 4);
                string strNumber = response.Substring(9, 4);
                if (response[0] == ':' && strHardware == hardwareNo.ToString("X2") && strFunction == "10" && strAddress == address.ToString("X4") && strNumber == number.ToString("X4"))
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
                return false;
            }
        }

        private static string GetLRC(byte[] cmd)
        {
            byte sum = 0;

            for (int i = 0; i < cmd.Length; i++)
            {
                sum = (byte)(sum + cmd[i]);
            }
            sum = (byte)(0 - sum);
            return sum.ToString("X2");
        }

        private static int AsciiToInt(byte ascVal)
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

        private static short TranslateToShortValue(byte[] buf)
        {
            int lowByte = AsciiToInt(buf[0]) * 16 + AsciiToInt(buf[1]);
            int highByte = AsciiToInt(buf[2]) * 16 + AsciiToInt(buf[3]);
            int intVal = lowByte * 256 + highByte;
            return (short)intVal;
        }
    }
}