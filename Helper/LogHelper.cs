using System;
using System.Collections.Generic;
using System.IO;

namespace YiRongMachine
{
    public enum LogType
    {
        Log_DuanMianA,
        Log_WaiYuanA,
        Log_DaoJiaoA,
        Log_NeiKong,
        Log_DuanMianB,
        Log_WaiYuanB,
        Log_DaoJiaoB,
        Log_Comm
    }

    public class LogHelper
    {
        public static object Lock_WriteLog = new object();    //写日志时加锁

        public static object Lock_DuanMianALog = new object();    //运行日志进出队列加锁
        public static object Lock_WaiYuanALog = new object();    //运行日志进出队列加锁
        public static object Lock_DaoJiaoALog = new object();   //运行日志进出队列加锁
        public static object Lock_NeiKongLog = new object();  //运行日志进出队列加锁
        public static object Lock_DuanMianBLog = new object();    //运行日志进出队列加锁
        public static object Lock_WaiYuanBLog = new object();    //运行日志进出队列加锁
        public static object Lock_DaoJiaoBLog = new object();   //运行日志进出队列加锁
        public static object Lock_CommLog = new object();   //运行日志进出队列加锁

        public static Queue<string> Que_DuanMianALog = new Queue<string>();
        public static Queue<string> Que_WaiYuanALog = new Queue<string>();
        public static Queue<string> Que_DaoJiaoALog = new Queue<string>();
        public static Queue<string> Que_NeiKongLog = new Queue<string>();
        public static Queue<string> Que_DuanMianBLog = new Queue<string>();
        public static Queue<string> Que_WaiYuanBLog = new Queue<string>();
        public static Queue<string> Que_DaoJiaoBLog = new Queue<string>();
        public static Queue<string> Que_CommLog = new Queue<string>();

        /// <summary>
        /// 创建文件路径，如果没有目录并生成
        /// </summary>
        /// <param name="type">日志类型</param>
        /// <returns>文件目录路径</returns>
        private static string CreateDirectory(LogType type)
        {
            string LogPath = GlobalVariable.path + "Log\\";
            switch (type)
            {
                case LogType.Log_DuanMianA:
                    {
                        LogPath += "DuanMianA\\";
                    }
                    break;

                case LogType.Log_WaiYuanA:
                    {
                        LogPath += "WaiYuanA\\";
                    }
                    break;

                case LogType.Log_DaoJiaoA:
                    {
                        LogPath += "DaoJiaoA\\";
                    }
                    break;

                case LogType.Log_NeiKong:
                    {
                        LogPath += "NeiKong\\";
                    }
                    break;

                case LogType.Log_DuanMianB:
                    {
                        LogPath += "DuanMianB\\";
                    }
                    break;

                case LogType.Log_WaiYuanB:
                    {
                        LogPath += "WaiYuanB\\";
                    }
                    break;

                case LogType.Log_DaoJiaoB:
                    {
                        LogPath += "DaoJiaoB\\";
                    }
                    break;

                case LogType.Log_Comm:
                    {
                        LogPath += "Comm\\";
                    }
                    break;
            }

            if (!FileHelper.IsExistDirectory(LogPath))
            {
                FileHelper.CreateDirectory(LogPath);
            }

            return LogPath;
        }

        /// <summary>
        /// 写入TXT日志：代码里自动创建当天时间的txt文件
        /// </summary>
        /// <param name="log">日志内容</param>
        /// <param name="type">日志类型，存放在不同路径下面</param>
        private static void WriteTxtLog(string log, LogType type)
        {
            string strLogPath = CreateDirectory(type);
            string strLogFile = strLogPath + DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
            lock (Lock_WriteLog)
            {
                try
                {
                    FileStream fs = new FileStream(strLogFile, System.IO.FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);

                    sw.WriteLine(DateTime.Now.ToString("HH:mm:ss_fff") + ":" + log);
                    sw.Close();
                    fs.Close();
                }
                catch
                {
                    //这里加异常可能会导致无限循环
                }
            }
        }

        /// <summary>
        /// 添加运行日志
        /// </summary>
        /// <param name="log"></param>
        public static void AddDuanMianALog(string log)
        {
            //写入文件
            WriteTxtLog(log, LogType.Log_DuanMianA);
            //添加进入队列，在主界面上显示
            lock (Lock_DuanMianALog)
            {
                Que_DuanMianALog.Enqueue(DateTime.Now.ToString("HH:mm:ss_fff") + ":" + log);
            }
        }

        /// <summary>
        /// 添加运行日志
        /// </summary>
        /// <param name="log"></param>
        public static void AddWaiYuanALog(string log)
        {
            //写入文件
            WriteTxtLog(log, LogType.Log_WaiYuanA);
            //添加进入队列，在主界面上显示
            lock (Lock_WaiYuanALog)
            {
                Que_WaiYuanALog.Enqueue(DateTime.Now.ToString("HH:mm:ss_fff") + ":" + log);
            }
        }

        /// <summary>
        /// 添加运行日志
        /// </summary>
        /// <param name="log"></param>
        public static void AddDaoJiaoALog(string log)
        {
            //写入文件
            WriteTxtLog(log, LogType.Log_DaoJiaoA);
            //添加进入队列，在主界面上显示
            lock (Lock_DaoJiaoALog)
            {
                Que_DaoJiaoALog.Enqueue(DateTime.Now.ToString("HH:mm:ss_fff") + ":" + log);
            }
        }

        /// <summary>
        /// 添加运行日志
        /// </summary>
        /// <param name="log"></param>
        public static void AddNeiKongLog(string log)
        {
            //写入文件
            WriteTxtLog(log, LogType.Log_NeiKong);
            //添加进入队列，在主界面上显示
            lock (Lock_NeiKongLog)
            {
                Que_NeiKongLog.Enqueue(DateTime.Now.ToString("HH:mm:ss_fff") + ":" + log);
            }
        }

        public static void AddLog(string log,int station)
        {
            //写入文件
            WriteTxtLog(log, (LogType)station);
            //添加进入队列，在主界面上显示
            lock (Lock_NeiKongLog)
            {
                Que_NeiKongLog.Enqueue(DateTime.Now.ToString("HH:mm:ss_fff") + ":" + log);
            }
        }

        /// <summary>
        /// 添加运行日志
        /// </summary>
        /// <param name="log"></param>
        public static void AddDuanMianBLog(string log)
        {
            //写入文件
            WriteTxtLog(log, LogType.Log_DuanMianB);
            //添加进入队列，在主界面上显示
            lock (Lock_DuanMianBLog)
            {
                Que_DuanMianBLog.Enqueue(DateTime.Now.ToString("HH:mm:ss_fff") + ":" + log);
            }
        }

        /// <summary>
        /// 添加运行日志
        /// </summary>
        /// <param name="log"></param>
        public static void AddWaiYuanBLog(string log)
        {
            //写入文件
            WriteTxtLog(log, LogType.Log_WaiYuanB);
            //添加进入队列，在主界面上显示
            lock (Lock_WaiYuanBLog)
            {
                Que_WaiYuanBLog.Enqueue(DateTime.Now.ToString("HH:mm:ss_fff") + ":" + log);
            }
        }

        /// <summary>
        /// 添加运行日志
        /// </summary>
        /// <param name="log"></param>
        public static void AddDaoJiaoBLog(string log)
        {
            //写入文件
            WriteTxtLog(log, LogType.Log_DaoJiaoB);
            //添加进入队列，在主界面上显示
            lock (Lock_DaoJiaoBLog)
            {
                Que_DaoJiaoBLog.Enqueue(DateTime.Now.ToString("HH:mm:ss_fff") + ":" + log);
            }
        }

        /// <summary>
        /// 添加运行日志
        /// </summary>
        /// <param name="log"></param>
        public static void AddCommLog(string log)
        {
            //写入文件
            WriteTxtLog(log, LogType.Log_Comm);
            //添加进入队列，在主界面上显示
            lock (Lock_CommLog)
            {
                Que_CommLog.Enqueue(DateTime.Now.ToString("HH:mm:ss_fff") + ":" + log);
            }
        }
    }
}