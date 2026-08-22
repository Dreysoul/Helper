using System;
using System.IO.Ports;

namespace YiRongMachine
{
    public delegate void ReceiveData(string data);

    public class SerialPortByNilifu : IDisposable
    {
        #region 变量

        private string _strName = "";

        public string HardwareName
        {
            get { return _strName; }
            set { _strName = value; }
        }

        private string _strPortName = "COM1";

        public string PortName
        {
            get { return _strPortName; }
            set { _strPortName = value; }
        }

        private int _iBaudRate = 9600;

        public int BaudRate
        {
            get { return _iBaudRate; }
            set { _iBaudRate = value; }
        }

        private int _iDataBits = 8;

        public int DataBits
        {
            get { return _iDataBits; }
            set { _iDataBits = value; }
        }

        private int _iStopBits = 1;

        public int StopBits
        {
            get { return _iStopBits; }
            set { _iStopBits = value; }
        }

        private string _strParity = "NONE";

        public string Parity
        {
            get { return _strParity; }
            set { _strParity = value; }
        }

        //private HARDWARE _type = HARDWARE.SerialPort;
        //public HARDWARE HardwareType
        //{
        //    get { return _type; }
        //    set { _type = value; }
        //}

        private string _strMark = "";

        public string Mark
        {
            get { return _strMark; }
            set { _strMark = value; }
        }

        /// <summary>
        /// 串口操作类
        /// </summary>
        private SerialPort _sp = null; //串口類

        public SerialPort SerialPortClient
        {
            get { return _sp; }
        }

        private bool _bIsOpen = false; //串口是否打開状态

        public bool IsOpen
        {
            get { return _sp.IsOpen; }
        }

        private bool _bIsDataReceived = false;  //串口是否接受到数据

        public bool IsDataReceived
        {
            get { return _bIsDataReceived; }
            set { _bIsDataReceived = value; }
        }

        private bool _bIsErrorReceived = false; //串口是否错误事件

        public bool IsErrorReceived
        {
            get { return _bIsErrorReceived; }
            set { _bIsErrorReceived = value; }
        }

        #endregion 变量

        public SerialPortByNilifu()
        {
            _sp = new SerialPort();
            if (_sp != null)
            {
                _sp.DataReceived += DataReceived;
            }
        }

        public Parity StringConvertToParity(string str)
        {
            string strParity = str.ToUpper();
            Parity Temp = System.IO.Ports.Parity.None;
            switch (strParity)
            {
                case "NONE":
                    {
                        Temp = System.IO.Ports.Parity.None;
                    }
                    break;

                case "ODD":
                    {
                        Temp = System.IO.Ports.Parity.Odd;
                    }
                    break;

                case "EVEN":
                    {
                        Temp = System.IO.Ports.Parity.Even;
                    }
                    break;

                case "MARK":
                    {
                        Temp = System.IO.Ports.Parity.Mark;
                    }
                    break;

                case "SPACE":
                    {
                        Temp = System.IO.Ports.Parity.Space;
                    }
                    break;
            }
            return Temp;
        }

        /// <summary>
        /// 打开串口
        /// </summary>
        /// <returns></returns>
        public bool OpenPort()
        {
            if (_sp == null)
            {
                _sp = new SerialPort();
            }

            if (_sp != null)
            {
                if (_bIsOpen)
                {
                    return true;
                }

                _sp.PortName = _strPortName; //通信端口
                _sp.BaudRate = _iBaudRate; //波特率
                _sp.Parity = StringConvertToParity(_strParity);      //奇偶校验
                _sp.DataBits = _iDataBits;  //数据位
                _sp.StopBits = (StopBits)_iStopBits;  //停止位

                _sp.ReadTimeout = 3000;
                _sp.WriteTimeout = -1;

                try
                {
                    _sp.Open();
                    _bIsOpen = true;
                }
                catch (Exception)
                {
                    _bIsOpen = false;
                }
            }
            return _bIsOpen;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            try
            {
                if (_sp != null)
                {
                    _sp.Dispose();
                    _sp = null;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            try
            {
                if (_sp != null)
                {
                    _sp.Close();
                }
                _bIsOpen = false;
            }
            catch
            {
            }

            //if (_sp != null)
            //{
            //    _sp.DataReceived -= DataReceived;
            //}

            //try
            //{
            //    Dispose();
            //}
            //catch
            //{
            //}
        }

        /// <summary>
        /// 接受数据时引发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            _bIsDataReceived = true;
        }

        /// <summary>
        /// pin 更改时引发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void PinChanged(object sender, SerialPinChangedEventArgs e)
        {
            string str = (sender as SerialPort).ReadExisting();
        }

        /// <summary>
        /// 获取当前电脑所有串口名
        /// </summary>
        /// <returns>返回当前电脑所有串口名</returns>
        public string[] GetAllCommName()
        {
            try
            {
                return System.IO.Ports.SerialPort.GetPortNames();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="strData">发送的数据</param>
        public bool WriteData(byte[] buf)
        {
            _bIsDataReceived = false;
            try
            {
                _sp.Write(buf, 0, buf.Length);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="strData">发送的数据</param>
        public bool WriteData(string buf)
        {
            _bIsDataReceived = false;
            try
            {
                _sp.Write(buf);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public int ReadByte(byte[] buf)
        {
            return _sp.Read(buf, 0, buf.Length);
        }

        public int ReadOneByte()
        {
            return _sp.ReadByte();
        }

        public string ReadString()
        {
            return _sp.ReadLine();
        }

        public string ReadString2()
        {
            return _sp.ReadTo("*");
        }

        public void ClearStream()
        {
            try
            {
                _sp.DiscardOutBuffer();
                _sp.DiscardInBuffer();
            }
            catch (Exception)
            {
            }
        }
    }
}