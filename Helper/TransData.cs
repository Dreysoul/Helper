using System;
using System.Collections.Generic;

namespace YiRongMachine
{
    public struct TransData
    {
        public int No;
        public string startTime;
        public int SideAResult;
        public int SideBResult;
        public int SideInResult;
        public int SideOutResult;
        public int TotalResult;
        public string endTime;

        public void Clear()
        {
            No = 0;
            startTime = "";
            SideAResult = 0;
            SideBResult = 0;
            SideInResult = 0;
            SideOutResult = 0;
            TotalResult = 0;
            endTime = "";
        }
    }

    public class CTransDataQue
    {
        private Queue<TransData> TransDataQue;
        private static object locker = new object();

        public CTransDataQue()
        {
            TransDataQue = new Queue<TransData>();
        }

        ~CTransDataQue()
        {
            TransDataQue = null;
        }

        /// <summary>
        /// 添加队列数据
        /// </summary>
        /// <param name="_TransData"></param>
        public void Add(TransData _TransData)
        {
            lock (locker)
            {
                TransDataQue.Enqueue(_TransData);
            }
        }

        /// <summary>
        /// 删除队列的首个数据
        /// </summary>
        /// <returns></returns>
        public TransData DeleteFirst()
        {
            lock (locker)
            {
                TransData Data = new TransData();
                if (TransDataQue.Count > 0)
                {
                    try
                    {
                        Data = TransDataQue.Dequeue();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.AddCommLog(ex.Message);
                    }
                }
                return Data;
            }
        }

        /// <summary>
        /// 获取首个队列数据
        /// </summary>
        /// <returns></returns>
        public TransData GetFirst()
        {
            lock (locker)
            {
                TransData Data = new TransData();
                if (TransDataQue.Count > 0)
                {
                    try
                    {
                        Data = TransDataQue.Peek();
                    }
                    catch (Exception ex)
                    {
                        LogHelper.AddCommLog(ex.Message.ToString());
                    }
                }
                return Data;
            }
        }

        /// <summary>
        /// 判断队列是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty()
        {
            int num = 0;
            lock (locker)
            {
                num = TransDataQue.Count;
            }
            if (num > 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获得数量
        /// </summary>
        /// <returns></returns>
        public int GetNumer()
        {
            lock (locker)
            {
                return TransDataQue.Count;
            }
        }

        /// <summary>
        /// 清空队列
        /// </summary>
        public void Empty()
        {
            lock (locker)
            {
                TransDataQue.Clear();
            }
        }

        ///// <summary>
        ///// 查找是否在队列中
        ///// </summary>
        ///// <returns></returns>
        //public bool IsExistTransData(string sn)
        //{
        //    lock (locker)
        //    {
        //        foreach (var item in TransDataQue)
        //        {
        //            if (item.CarrierSN   == sn)
        //            {
        //                return true;
        //            }
        //        }
        //        return false;
        //    }
        //}
    }
}