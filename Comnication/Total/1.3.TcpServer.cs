using System;
using System.Collections.Generic;
using System.Text;

namespace YiRongMachine.Comnication
{
    /// <summary>
    /// 服务器
    /// </summary>
    public class TcpServer1 : IDisposable
    {
        /// <summary>
        /// 有新的连接
        /// </summary>
        public event UpdateConnectionStatusHandler UpdateConnectionStatus;

        /// <summary>
        /// 接受到消息事件
        /// </summary>
        public event IsRecievedMessageHandler IsRecievedMessage;

        /// <summary>
        /// 接收消息事件
        /// </summary>
        public event ReceiveMessageHandler RecieveMessage;

        /// <summary>
        /// 接受到消息事件
        /// </summary>
        public event IsRecievedIPMessageHandler IsRecievedIPMessage;

        /// <summary>
        /// 接收消息事件
        /// </summary>
        public event IPMessageHandler RecieveIPMessage;

        #region 变量

        //lock
        private static object asyn_reciLock = new object();

        //声明套接字,用于监听远程端口
        private System.Net.Sockets.Socket _serverSocket;

        //监听服务器是否有远程主机新的请求连接的线程
        private System.Threading.Thread _listenThread = null;

        //监听到有远程主机请求的连接的套接字,用于发送与接收消息
        private List<System.Net.Sockets.Socket> _sockets = new List<System.Net.Sockets.Socket>();

        //接受消息线程集合
        private List<System.Threading.Thread> _reciThreads = new List<System.Threading.Thread>();

        //处理服务器关闭异常
        private bool is_ServerSocket_colse = false;

        //处理服务器关闭异常
        private string address_Temp = "0";

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

        //private HARDWARE _type = HARDWARE.TcpServer;
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
        /// 服务器是否正在监听
        /// </summary>
        private bool _bIsListening = true;

        public bool IsListening
        {
            get { return _bIsListening; }
        }

        /// <summary>
        /// 服务器接收端最大个数
        /// </summary>
        private int _iMaxClientCount = 5;

        public int MaxClientCount
        {
            get { return _iMaxClientCount; }
            set { _iMaxClientCount = value; }
        }

        /// <summary>
        /// 返回是否接受到消息
        /// </summary>
        private bool _isRecieved;

        public bool IsRecieved
        {
            get { return _isRecieved; }
            set { _isRecieved = value; }
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

        #endregion 变量

        /// <summary>
        /// 构造函数
        /// </summary>
        public TcpServer1()
        {
            _serverSocket = null;
            _bIsListening = false;

            _isRecieved = false;
            _recieveString = "";
        }

        /// <summary>
        /// 析构函数
        /// </summary>
        ~TcpServer1()
        {
            Dispose();
        }

        /// <summary>
        /// 服务器开始侦听
        /// </summary>
        /// <returns>侦听完成,返回True,否则False</returns>
        public bool Start()
        {
            if (_serverSocket != null)
            {
                is_ServerSocket_colse = false;
                if (_bIsListening)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            if (_serverSocket == null)
            {
                try
                {
                    //实例化套接字
                    _serverSocket = new System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp);

                    System.Net.IPAddress address = System.Net.IPAddress.Parse(_strIP);

                    ////绑定IP地址、端口
                    _serverSocket.Bind(new System.Net.IPEndPoint(address, _iPort));

                    //侦听
                    if (_serverSocket.IsBound)
                    {
                        _serverSocket.Listen(_iMaxClientCount);
                        //正在侦听
                        _bIsListening = true;
                    }
                }
                catch (System.Exception e)
                {
                    string s = string.Format("{0}类中{1}方法产生异常，异常为{2}", "TcpServer", "Start", e.Message);
                    LogHelper.AddCommLog(s);
                    return false;
                }
            }

            //监听服务器是否有新连接的线程
            if (_listenThread == null)
            {
                _listenThread = new System.Threading.Thread(ListenConnectionFunc);
                _listenThread.Start();
            }

            return true;
        }

        /// <summary>
        /// 服务器关闭侦听
        /// </summary>
        /// <returns></returns>
        public void Stop()
        {
            if (_bIsListening)
            {
                Dispose();
            }
            _bIsListening = false;
        }

        /// <summary>
        /// 服务器侦听远程主机函数
        /// </summary>
        private void ListenConnectionFunc()
        {
            System.Net.Sockets.Socket socket = null;
            while (true)
            {
                //等待远程主机连接
                socket = _serverSocket.Accept();
                if (is_ServerSocket_colse)
                {
                    is_ServerSocket_colse = false;
                    return;
                }

                //是否已经连接到
                if (socket.Connected)
                {
                    //连接到远程主机
                    //if (RecieveMessage != null) RecieveMessage(string.Format("[Recieve]ConnectedClient({0}).", socket.RemoteEndPoint));

                    System.Threading.Thread recieveThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RecieveMessageFunc));
                    recieveThread.Start(socket);

                    _sockets.Add(socket);
                    _reciThreads.Add(recieveThread);

                    //刷新连接状态
                    if (UpdateConnectionStatus != null) UpdateConnectionStatus(true);
                }
            }
        }

        /// <summary>
        /// 发送消息给所有客户端
        /// </summary>
        /// <param name="msg">发送的消息</param>
        public bool SendMessage(string msg)
        {
            if (_bIsListening && _sockets.Count > 0)
            {
                try
                {
                    for (int i = 0; i < _sockets.Count; i++)
                    {
                        if (_sockets[i] != null)
                        {
                            byte[] buf = new byte[1024];
                            buf = Encoding.Default.GetBytes(msg);
                            int count = _sockets[i].Send(buf);
                        }
                    }
                    return true;
                }
                catch (System.Exception e)
                {
                    string s = string.Format("{0}类中{1}方法产生异常，异常为{2}", "TcpServer", "SendMessage", e.Message);
                    LogHelper.AddCommLog(s);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg">发送的消息</param>
        /// <param name="index">客户端的索引</param>
        /// <returns>发送成功返回True,否则False</returns>
        public bool SendMessage(string msg, int index)
        {
            try
            {
                if (_sockets[index] != null)
                {
                    byte[] buf = new byte[1024];
                    buf = Encoding.Default.GetBytes(msg);
                    int count = _sockets[index].Send(buf);
                    if (count <= buf.Length)
                    {
                        return true;
                    }
                    return false;
                }
                else return false;
            }
            catch (System.Exception e)
            {
                string s = string.Format("{0}类中{1}方法产生异常，异常为{2}", "TcpServer", "SendMessage", e.Message);
                LogHelper.AddCommLog(s);
                return false;
            }
        }

        /// <summary>
        /// 通过IP发送消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="ip">IP+端口</param>
        /// <returns>发送完成返回True,否则False</returns>
        ///

        public bool SendMessageByIP(string msg, System.Net.IPEndPoint ip)
        {
            int index = 0;
            try
            {
                for (int i = 0; i < _sockets.Count; i++)
                {
                    if (_sockets[i].RemoteEndPoint == ip)
                    {
                        index = i;
                        break;
                    }
                }

                if (_sockets[index] != null)
                {
                    byte[] buf = new byte[1024];
                    buf = Encoding.Default.GetBytes(msg);
                    int count = _sockets[index].Send(buf);
                    if (count <= buf.Length)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (System.Exception e)
            {
                string s = string.Format("{0}类中{1}方法产生异常，异常为{2}", "TcpServer", "SendMessageByIP", e.Message);
                LogHelper.AddCommLog(s);
                return false;
            }
        }

        /// <summary>
        /// 通过IP地址发送消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="addr">IP地址</param>
        /// <returns>发送完成返回True,否则False</returns>
        public bool SendMessageByIP(string msg, System.Net.IPAddress addr)
        {
            int index = 0;
            try
            {
                for (int i = 0; i < _sockets.Count; i++)
                {
                    string ipStr = _sockets[i].RemoteEndPoint.ToString();
                    int len = ipStr.IndexOf(':');
                    if (ipStr.Substring(0, len) == addr.ToString())
                    {
                        index = i;
                        break;
                    }
                }

                if (_sockets[index] != null)
                {
                    byte[] buf = new byte[1024];
                    buf = Encoding.Default.GetBytes(msg);
                    int count = _sockets[index].Send(buf);
                    if (count <= buf.Length)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (System.Exception e)
            {
                string s = string.Format("{0}类中{1}方法产生异常，异常为{2}", "TcpServer", "SendMessageByIP", e.Message);
                LogHelper.AddCommLog(s);
                return false;
            }
        }

        /// <summary>
        /// 通过IP地址发送消息
        /// </summary>
        /// <param name="msg">消息</param>
        /// <param name="addr">IP地址</param>
        /// <param name="port">端口号</param>
        /// <returns>发送完成返回True,否则False</returns>
        public bool SendMessageByIP(string msg, System.Net.IPAddress addr, int port)
        {
            int index = 0;
            try
            {
                for (int i = 0; i < _sockets.Count; i++)
                {
                    string ipStr = _sockets[i].RemoteEndPoint.ToString();
                    int len = ipStr.IndexOf(':');
                    if (ipStr.Substring(0, len) == addr.ToString() && ipStr.Substring(len + 1) == port.ToString())
                    {
                        index = i;
                        break;
                    }
                }
                //string str = _client.ServerIPAdr.ToString();
                //int len = str.IndexOf(':');
                //textBox_ClientIP.Text = str.Substring(0, len);
                //textBox_ClientPort.Text = str.Substring(len + 1);

                if (_sockets[index] != null)
                {
                    byte[] buf = new byte[1024];
                    buf = Encoding.Default.GetBytes(msg);
                    int count = _sockets[index].Send(buf);
                    if (count <= buf.Length)
                    {
                        return true;
                    }
                    return false;
                }
                return false;
            }
            catch (System.Exception e)
            {
                string s = string.Format("{0}类中{1}方法产生异常，异常为{2}", "TcpServer", "SendMessageByIP", e.Message);
                LogHelper.AddCommLog(s);
                return false;
            }
        }

        /// <summary>
        /// 接受消息函数
        /// </summary>
        private void RecieveMessageFunc(object socket)
        {
            System.Net.Sockets.Socket mysocket = null;
            if (socket.GetType() == typeof(System.Net.Sockets.Socket)) mysocket = (System.Net.Sockets.Socket)socket;

            while (mysocket != null)
            {
                //try
                //{
                //接受到消息
                if (mysocket.Poll(5, System.Net.Sockets.SelectMode.SelectRead))
                {
                    //消息有效长度
                    if (mysocket.Available > 0)
                    {
                        lock (asyn_reciLock)
                        {
                            //读取消息
                            byte[] buf = new byte[mysocket.Available];
                            int count = mysocket.Receive(buf);
                            _recieveString = System.Text.Encoding.Default.GetString(buf, 0, count);
                            _isRecieved = true;
                        }
                        //触发接受消息事件
                        // if (RecieveMessage != null) RecieveMessage(string.Format("[Recieve]Client({0})Message:{1}.", mysocket.RemoteEndPoint, _recieveString));

                        if (RecieveMessage != null) RecieveMessage(string.Format(_recieveString));

                        //触发接受到消息事件
                        if (IsRecievedMessage != null) IsRecievedMessage(_isRecieved);
                        //触发接受IP消息事件
                        if (RecieveIPMessage != null) RecieveIPMessage(_recieveString, mysocket.RemoteEndPoint.ToString());
                        //触发接受到IP消息事件
                        if (IsRecievedIPMessage != null) IsRecievedIPMessage(_isRecieved, mysocket.RemoteEndPoint.ToString());
                    }
                    else
                    {
                        _isRecieved = false;
                        //触发接受消息事件
                        if (RecieveMessage != null) RecieveMessage(string.Format("客户端({0})断开", mysocket.RemoteEndPoint));

                        //if (RecieveMessage != null) RecieveMessage(string.Format(_recieveString));

                        //触发接受IP消息事件
                        if (RecieveIPMessage != null) RecieveIPMessage("Quit", mysocket.RemoteEndPoint.ToString());
                        //
                        if (_sockets.Contains(mysocket))
                        {
                            int i = _sockets.IndexOf(mysocket);
                            mysocket.Close(50);
                            _sockets.RemoveAt(i);
                            _reciThreads.RemoveAt(i);
                        }
                        //刷新连接状态
                        if (UpdateConnectionStatus != null) UpdateConnectionStatus(true);
                        break;
                    }
                }
                else
                {
                    //没有读取到消息
                }
            }
            //循环退出
        }

        /// <summary>
        /// 关闭套接字,终止线程
        /// </summary>
        public void Dispose()
        {
            if (_serverSocket != null)
            {
                is_ServerSocket_colse = true;
                TcpClient1 _MyTcpClient = new TcpClient1();
                _MyTcpClient.TcpPort = TcpPort;
                _MyTcpClient.TcpIP = address_Temp;
                _MyTcpClient.Connect();
                _serverSocket.Close(50);
                _serverSocket = null;
            }

            if (_listenThread != null)
            {
                if (_listenThread.IsAlive)
                {
                    _listenThread.Abort();
                }
                _listenThread = null;
            }

            _bIsListening = false;

            //关闭线程
            foreach (System.Threading.Thread t in _reciThreads)
            {
                t.Abort();
            }
            _reciThreads.Clear();

            //关闭套接字
            foreach (System.Net.Sockets.Socket socket in _sockets)
            {
                socket.Close(50);
            }
            _sockets.Clear();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public void Close()
        {
            if (UpdateConnectionStatus != null) UpdateConnectionStatus = null;
            if (RecieveMessage != null) RecieveMessage = null;
            if (IsRecievedMessage != null) IsRecievedMessage = null;

            if (RecieveIPMessage != null) RecieveIPMessage = null;
            if (IsRecievedIPMessage != null) IsRecievedIPMessage = null;

            Dispose();
        }
    }
}