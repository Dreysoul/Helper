using System;
using System.Data;
using System.IO;
using System.Text;

namespace YiRongMachine
{
    internal class CSVHelper
    {
        private static object log_Lock = new object();        // 写入CSV文件时加锁

        /// <summary>
        /// 用于写LOG，将数组写成CSV中的一行数据，path仅仅为目录路径
        /// </summary>
        /// <param name="log">要存储进入csv文件中的内容</param>
        /// <param name="type">路径不包含时间</param>
        public static bool WriteCSVLog(string[] Log, string Path, ref string ErrorMsg)
        {
            try
            {
                string _log = "";
                if (!Directory.Exists(Path))
                {
                    Directory.CreateDirectory(Path);
                }
                for (int i = 0; i < Log.Length; i++)
                {
                    if (i < Log.Length - 1)
                    {
                        _log = _log + Log[i] + ",";
                    }
                    else
                    {
                        _log = _log + Log[i];
                    }
                }
                string strLogFile = Path + "\\" + DateTime.Now.ToString("yyyy_MM_dd") + ".CSV";
                lock (log_Lock)
                {
                    FileStream fs = new FileStream(strLogFile, FileMode.Append);
                    StreamWriter sw = new StreamWriter(fs, Encoding.Default);
                    sw.WriteLine(_log);
                    sw.Close();
                    fs.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 将DataTable写入到CSV中，Path为全路径
        /// </summary>
        /// <param name="log">要存储进入csv文件中的内容</param>
        /// <param name="type">路径不包含时间</param>
        public static bool WriteDataTableToCSV_NoHead(DataTable dt, string Path, ref string ErrorMsg)
        {
            try
            {
                if (File.Exists(Path))
                {
                    File.Delete(Path);
                }
                File.Create(Path).Close();

                FileStream fs = new FileStream(Path, System.IO.FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string _log = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            _log += dt.Rows[i][j] + ",";
                        }
                        else
                        {
                            _log += dt.Rows[i][j];
                        }
                    }
                    lock (log_Lock)
                    {
                        sw.WriteLine(_log);
                    }
                }
                sw.Close();
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 将DataTable写入到CSV中，Path为全路径
        /// </summary>
        /// <param name="log">要存储进入csv文件中的内容</param>
        /// <param name="type">路径不包含时间</param>
        public static bool WriteDataTableToCSV_HaveHead(DataTable dt, string Path, string[] head, ref string ErrorMsg)
        {
            try
            {
                if (File.Exists(Path))
                {
                    File.Delete(Path);
                }
                File.Create(Path).Close();

                FileStream fs = new FileStream(Path, System.IO.FileMode.Append);
                StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
                string _log = "";
                for (int i = 0; i < head.Length; i++)
                {
                    if (i < head.Length - 1)
                    {
                        _log = _log + head[i] + ",";
                    }
                    else
                    {
                        _log = _log + head[i];
                    }
                }
                lock (log_Lock)
                {
                    sw.WriteLine(_log);
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    _log = "";
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (j < dt.Columns.Count - 1)
                        {
                            _log += dt.Rows[i][j] + ",";
                        }
                        else
                        {
                            _log += dt.Rows[i][j];
                        }
                    }
                    lock (log_Lock)
                    {
                        sw.WriteLine(_log);
                    }
                }
                sw.Close();
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 读取CSV文件中的数据，返回DataTable,path为文件完整路径,tableHead为一个与列数长度的数组
        /// </summary>
        /// <param name="filePath">路径不包含时间</param>
        /// <param name="tableHead">dt的列名</param>
        /// <returns></returns>
        public static bool OpenCSV_NoHead(string path, string[] tableHead, DataTable dt, ref string ErrorMsg)
        {
            try
            {
                lock (log_Lock)
                {
                    dt.Rows.Clear();
                    FileStream fs = new FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    StreamReader sr = new StreamReader(fs, Encoding.Default);
                    //记录每次读取的一行记录
                    string strLine = "";
                    //记录每行记录中的各字段内容
                    string[] aryLine = null;
                    //string[] tableHead = null;
                    //标示列数
                    int columnCount = 0;
                    //标示是否是读取的第一行
                    columnCount = tableHead.Length;
                    //创建列
                    for (int i = 0; i < columnCount; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                    //逐行读取CSV中的数据
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        aryLine = strLine.Split(',');
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            dr[j] = aryLine[j];
                        }
                        dt.Rows.Add(dr);
                    }
                    sr.Close();
                    fs.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 读取CSV文件中的数据，返回DataTable.CSV文件里第一行为标题，path为文件完整路径
        /// </summary>
        /// <param name="filePath">路径不包含时间</param>
        /// <param name="tableHead">dt的列名</param>
        /// <returns></returns>
        public static bool OpenCSV_HaveHead(string path, DataTable dt, ref string ErrorMsg)
        {
            try
            {
                lock (log_Lock)
                {
                    FileStream fs = new FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read);
                    StreamReader sr = new StreamReader(fs, Encoding.Default);

                    //读取第一行，确定标题
                    string[] tableHead = sr.ReadLine().Split(',');
                    //创建列
                    for (int i = 0; i < tableHead.Length; i++)
                    {
                        DataColumn dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                    //剩下的每一行的内容
                    string strLine = "";
                    string[] aryLine = null;
                    //逐行读取CSV中的数据
                    while ((strLine = sr.ReadLine()) != null)
                    {
                        aryLine = strLine.Split(',');
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < tableHead.Length; j++)
                        {
                            dr[j] = aryLine[j];
                        }
                        dt.Rows.Add(dr);
                    }
                    sr.Close();
                    fs.Close();
                    return true;
                }
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
                return false;
            }
        }
    }
}