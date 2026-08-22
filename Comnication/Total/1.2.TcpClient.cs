using System;
using System.Text;

namespace YiRongMachine
{
    /// <summary>
    /// 刷新连接状态
    /// </summary>
    /// <param name="connection"></param>
    public delegate void UpdateConnectionStatusHandler(bool connection);

    /// <summary>
    /// 接受消息字符串
    /// </summary>
    /// <param name="msg"></param>
    public delegate void ReceiveMessageHandler(string msg);

    /// <summary>
    /// 是否接受到消息
    /// </summary>
    /// <param name="recieve"></param>
    public delegate void IsRecievedMessageHandler(bool recieve);

    /// <summary>
    /// 接受IP消息字符串
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="ip"></param>
    public delegate void IPMessageHandler(string msg, string ip);

    /// <summary>
    /// 是否接受到IP消息
    /// </summary>
    /// <param name="recieve"></param>
    /// <param name="ip"></param>
    public delegate void IsRecievedIPMessageHandler(bool recieve, string ip);

    /// <summary>
    /// 封装好的TCPClient类
    /// </summary>
    public class TcpClient1 : IDisposable
    {
        /// <summary>
        /// 接收消息事件
        /// </summary>
        public event ReceiveMessageHandler RecieveMessage;

        /// <summary>
        /// 接受到消息事件
        /// </summary>
        public event IsRecievedMessageHandler IsRecievedMessage;

        //刷新状态事件
        public event UpdateConnectionStatusHandler UpdateConnectionStatus;

        #region 变量

        private static object _asyn_reciLock = new object();
        private static object _asyn_sendLock = new object();
        private System.Net.Sockets.Socket _socket;
        private string _strName = "";

        public string HardwareName
        {
            get { return _strName; }
            set { _strName = value; }
        }

        private string _strIP = "127.0.0.1";

        public string TcpIP
        {
            get { return _strIP; }
            set { _strIP = value; }
        }

        private int _iPort = 0;

        public int TcpPort
        {
            get { return _iPort; }
            set { _iPort = value; }
        }

        //指定本地ip&端口号
        private string _localstrIP = "";

        public string LocalTcpIP
        {
            get { return _localstrIP; }
            set { _localstrIP = value; }
        }

        private int _localiPort = 0;

        public int LocalTcpPort
        {
            get { return _localiPort; }
            set { _localiPort = value; }
        }

        //private HARDWARE _type = HARDWARE.TcpClient;
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
        /// 返回接受到的消息字符串
        /// </summary>
        private string _recieveString;

        public string RecieveString
        {
            get { return _recieveString; }
            set { _recieveString = value; }
        }

        /// <summary>
        /// 返回是否接受到消息
        /// </summary>
        private bool _isRecieved;

        public bool IsRecieced
        {
            get { return _isRecieved; }
            set { _isRecieved = value; }
        }

        /// <summary>
        /// 返回是否已连接
        /// </summary>
        private bool _isConnected;

        public bool IsConncected
        {
            get { return _isConnected; }
        }

        #endregion 变量

        public TcpClient1()
        {
            _socket = null;
            _isConnected = false;
            _recieveString = "";
            _isRecieved = false;
        }

        ~TcpClient1()
        {
            Dispose();
        }

        /// <summary>
        /// 连接服务器
        /// </summary>
        /// <param name="address">IP地址</param>
        /// <param name="port">端口号</param>
        /// <returns>连接成功,返回True;否则False</returns>
        public bool Connect()
        {
            try
            {
                if (_socket != null)
                {
                    //是否连接成功
                    if (_socket.Connected && _isConnected)
                    {
                        return true;
                    }
                    else
                    {
                        _socket.Close(50);
                        _socket = null;
                    }
                }

                if (_socket == null)
                {
                    //实例化套接字
                    _socket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);
                    System.Net.IPAddress address = System.Net.IPAddress.Parse(_strIP);

                    ////绑定本地IP地址、端口
                    if (_localstrIP != "" && _localiPort != 0)
                    {
                        //_isLocalbing = true;
                        System.Net.IPAddress address1 = System.Net.IPAddress.Parse(_localstrIP);
                        _socket.Bind(new System.Net.IPEndPoint(address1, _localiPort));
                        _localiPort++;
                    }

                    _socket.Connect(address, _iPort);
                    if (_socket.Connected)
                    {
                        _isConnected = true;
                    }
                    else
                    {
                        return false;
                    }
                }

                if (UpdateConnectionStatus != null)
                {
                    UpdateConnectionStatus(true);
                }

                //开启线程
                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(RecieveMessageFunc));
                t.Start();

                //返回True
                return true;
            }
            catch (System.Exception e)
            {
                string s = string.Format("{0}类中{1}方法产生异常，异常为{2}", "TcpClient", "Connect", e.Message);
                LogHelper.AddCommLog(s);
                return false;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns>发送消息成功,返回True,否则False</returns>
        public bool SendMessage(string msg)
        {
            try
            {
                if (_socket != null)
                {
                    if (_isConnected)
                    {
                        int numofSend = 0;
                        lock (_asyn_sendLock)
                        {
                            byte[] buf = new byte[1024];
                            buf = Encoding.Default.GetBytes(msg);
                            numofSend = _socket.Send(buf);
                        }
                        if (numofSend > 0)
                        {
                            return true;
                        }
                        return false;
                    }
                    else return false;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception e)
            {
                string s = string.Format("{0}类中{1}方法产生异常，异常为{2}", "TcpClient", "SendMessage", e.Message);
                LogHelper.AddCommLog(s);
                return false;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msgByteArray">消息</param>
        /// <returns>发送消息成功,返回True,否则False</returns>
        public bool SendMessage(byte[] msgByteArray)
        {
            try
            {
                if (_socket != null)
                {
                    if (_isConnected)
                    {
                        int numofSend = 0;
                        lock (_asyn_sendLock)
                        {
                            byte[] buf = msgByteArray;
                            numofSend = _socket.Send(buf);
                        }
                        if (numofSend > 0)
                        {
                            return true;
                        }
                        return false;
                    }
                    else return false;
                }
                else
                {
                    return false;
                }
            }
            catch (System.Exception e)
            {
                string s = string.Format("{0}类中{1}方法产生异常，异常为{2}", "TcpClient", "SendMessage", e.Message);
                LogHelper.AddCommLog(s);
                return false;
            }
        }

        /// <summary>
        /// 接受消息函数
        /// </summary>
        private void RecieveMessageFunc()
        {
            while (_socket != null)
            {
                try
                {
                    //接受到消息
                    if (_socket.Poll(5, System.Net.Sockets.SelectMode.SelectRead))
                    {
                        //消息有效长度
                        if (_socket.Available > 0)
                        {
                            lock (_asyn_reciLock)
                            {
                                byte[] buf = new byte[_socket.Available];
                                int count = _socket.Receive(buf);
                                _recieveString = System.Text.Encoding.Default.GetString(buf, 0, count);
                                _isRecieved = true;
                                RecieveString = _recieveString;
                                //触发接受消息事件
                                if (RecieveMessage != null)
                                {
                                    RecieveMessage(_recieveString);
                                }
                                //触发接受到消息事件
                                if (IsRecievedMessage != null)
                                {
                                    IsRecievedMessage(_isRecieved);
                                }
                            }
                        }
                        else
                        {
                            _isRecieved = false;
                            _isConnected = false;
                            if (RecieveMessage != null)
                            {
                                RecieveMessage("服务器断开");
                            }

                            if (UpdateConnectionStatus != null)
                            {
                                UpdateConnectionStatus(false);
                            }

                            if (_socket != null)
                            {
                                _socket.Close(50);
                            }
                            _socket = null;
                            break;
                        }
                    }
                    else
                    {
                        //没有读取到消息
                    }
                }
                catch (System.Exception e)
                {
                    string s = string.Format("{0}类中{1}方法产生异常，异常为{2}", "TcpClient", "RecieveMessageFunc", e.Message);
                    LogHelper.AddCommLog(s);
                    break;
                }
            }
            //循环退出
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Disconnect()
        {
            if (IsConncected && _socket != null)
            {
                _isConnected = false;
                Close();
            }
        }

        /// <summary>
        /// 释放套接字
        /// </summary>
        public void Dispose()
        {
            if (RecieveMessage != null)
            {
                RecieveMessage = null;
            }
            if (IsRecievedMessage != null)
            {
                IsRecievedMessage = null;
            }
            if (UpdateConnectionStatus != null)
            {
                UpdateConnectionStatus = null;
            }

            Close();
        }

        /// <summary>
        /// 关闭客户端
        /// </summary>
        public void Close()
        {
            _isConnected = false;
            //关闭套接字
            if (_socket != null)
            {
                //_socket.Shutdown(System.Net.Sockets.SocketShutdown.Both);
                _socket.Close(50);
                _socket = null;
            }
        }
    }
}