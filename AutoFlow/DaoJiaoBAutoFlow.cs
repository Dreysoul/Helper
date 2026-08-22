using BearingInspection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;

namespace YiRongMachine
{
    public class DaoJiaoBAutoFlow
    {
        /// <summary>
        /// 0=SideA,1=SideB,2=SideIn,3=SideOut
        /// </summary>
        private int _stationNoo = 3;

        public Bitmap[] bitmapArray;
        public Bitmap[] bitmapCopy;
        public Bitmap bitmapShowPicture;
        public int ControlStep = 0;
        private int result = 0;
        private List<Error_Area> defectList = new List<Error_Area>();    //缺陷列表
        public MultiImg_Result_Info info;
        private Graphics ngGraphics;        //这个在自动化流程里实例化了
        private Pen pen = new Pen(Color.Red, 3);
        private string ErrorMsg = "";       //异常信息
        private string log = "";

        private bool bRtn = false;
        private bool bMath = false;
        private bool bShowStart = false;
        private bool bShowEnd = false;
        //Semaphore semaphoreMath = new Semaphore(0, 2);
        //Semaphore semaphoreShowStart = new Semaphore(0, 2);
        //Semaphore semaphoreShowEnd = new Semaphore(0, 2);

        private List<string> errorReason = new List<string>();
        private string strErrorType = "";
        private Stopwatch st = new Stopwatch();
        private Stopwatch stall = new Stopwatch();
        private PointF[] errorPoint = new PointF[5];
        public int plcErrorCode = 0;
        private string strDateTime = "";
        private SolidBrush brush = new SolidBrush(Color.Red);
        private SolidBrush brushString = new SolidBrush(Color.Blue);
        private Font wordFont = new Font("宋体", 45);

        private int errorCode = 0;
        private bool bOK = true;

        bool previousState = true;

        //计算的指针
        private int m_iIndexCalculate = -1;

        //拍照的指针
        private int m_iIndexPhoto = -1;

        private Queue<BitmapContent> queue = new Queue<BitmapContent>();
        private BitmapContent[] data = new BitmapContent[GlobalVariable.iDaoJiaoBPictureNumber];

        public DaoJiaoBAutoFlow()
        {
            strDateTime = DateTime.Now.ToString("yyyy-MM-dd");
            for (int i = 0; i < GlobalVariable.iDaoJiaoBPictureNumber; i++)
            {
                data[i] = new BitmapContent();
            }
            init();
        }

        public void init()
        {
            ControlStep = 0;
            bMath = false;
            bShowStart = false;
            bShowEnd = true;

            m_iIndexCalculate = -1;
            m_iIndexPhoto = -1;
        }

        /// <summary>
        /// 主要的控制流程
        /// </summary>
        private void ControlAutoFlow()
        {
            switch (ControlStep)
            {
                #region 0：读取IO板卡信号

                case 0:
                    {
                        HomeForm.home.daoJiaoBPicForm.ChangeSignStartColor(Color.Blue);
                        HomeForm.home.daoJiaoBPicForm.ChangeSignCameraColor(Color.Blue);
                        HomeForm.home.daoJiaoBPicForm.ChangeSignPictureColor(Color.Blue);
                        m_iIndexCalculate = -1;
                        m_iIndexPhoto = -1;

                        bool bret = GlobalVariable.ioBoardHelper.readInput((int)IOBoardHelper.enInputType.DaoJiaoB_CS_QD);
                        bool rising = !previousState && bret;
                        previousState = bret;
                        if (rising || GlobalVariable.bDaoJiaoBRunManual)
                        {
                            GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DaoJiaoB_TXG, false);
                            GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DaoJiaoB_PZ_WC, false);
                            GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DaoJiaoB_OK, false);
                            GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DaoJiaoB_NG, false);
                            ControlStep = 10;
                        }
                    }
                    break;

                #endregion 0：读取IO板卡信号

                #region 10：参数更改后需要更新参数

                case 10:
                    {
                        try
                        {
                            //如果被禁用了直接写PLC结果，不拍照不计算了
                            if (GlobalVariable.bForbidden[6])
                            {
                                result = (int)ResultCMD.OK;
                                if (GlobalVariable.generalSetting.bUseLog)
                                {
                                    LogHelper.AddDaoJiaoBLog("倒角B-----该工位已经被禁用");
                                }
                                ControlStep = 30;
                                break;
                            }

                            if (GlobalVariable.bDaoJiaoBParamUpdate)
                            {
                                GlobalVariable.DaoJiaoBProcess.setModelInfo(GlobalVariable.configSetting.daoJiaoBModel);
                                GlobalVariable.DaoJiaoBProcess.setParamInfo(GlobalVariable.configSetting.daoJiaoBParam);
                                GlobalVariable.DaoJiaoBProcess.setFlag(GlobalVariable.configSetting.daoJiaoBFlag);
                                //GlobalVariable.WaiYuanAProcess.updateModel();
                                //更新ROI
                                bRtn = GlobalVariable.DaoJiaoBCameraHelper.SetCameraROI(
                                        GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.ROIWidthTrue,
                                        GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.ROIHeihgtTrue,
                                        GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.OffsetX,
                                        GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.OffsetY);
                                //更改参数后重新实例化Bitmap
                                bitmapArray = new Bitmap[GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.photoNumber];
                                for (int i = 0; i < bitmapArray.Length; i++)
                                {
                                    bitmapArray[i] = new Bitmap(GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.ROIWidthTrue, GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.ROIHeihgtTrue);
                                }
                                bitmapShowPicture = new Bitmap(bitmapArray[0].Width, bitmapArray[0].Height);
                                ngGraphics = Graphics.FromImage(bitmapShowPicture);
                                if (GlobalVariable.generalSetting.bUseLog)
                                {
                                    LogHelper.AddDaoJiaoBLog("倒角B-----参数已经发生更改，成功重新将参数信息上传给算法");
                                }
                                GlobalVariable.bDaoJiaoBParamUpdate = false;
                            }

                            ControlStep = 20;
                        }
                        catch (Exception e)
                        {
                            log = "倒角B-----更新参数步骤出现异常，异常信息为" + e.Message;
                            LogHelper.AddDaoJiaoBLog(log);
                            AlarmDialog a = new AlarmDialog(log, MsgType.Retry);
                            a.ShowDialog();
                            GlobalVariable.bDaoJiaoBRunManual = false;
                        }
                    }
                    break;

                #endregion 10：参数更改后需要更新参数

                #region 20：拍照+数据传输给算法

                case 20:
                    {
                        st.Restart();
                        if (!bShowEnd)
                            break;

                        HomeForm.home.daoJiaoBPicForm.ChangeSignStartColor(Color.Lime);
                        HomeForm.home.daoJiaoBPicForm.ChangeSignCameraColor(Color.Blue);
                        HomeForm.home.daoJiaoBPicForm.ChangeSignPictureColor(Color.Blue);

                        double oldExposure = 0f;
                        GlobalVariable.DaoJiaoBCameraHelper.Start();

                        //打开光源
                        GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DaoJiaoB_TXG, true);
                        //拍照全流程
                        bRtn = GlobalVariable.DaoJiaoBCameraHelper.SetCameraExposureTime(GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.exposure[0], ref ErrorMsg);
                        if (!bRtn)
                        {
                            log = "倒角B相机，写入曝光值失败，异常信息为" + ErrorMsg;
                            LogHelper.AddDaoJiaoBLog(log);
                            AlarmDialog a = new AlarmDialog(log, MsgType.Retry);
                            a.ShowDialog();
                        }
                        for (int i = 0; i < bitmapArray.Length; i++)
                        {
                            //拍照
                            GlobalMethod.delay_ms((uint)GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.photoSpan);
                            bRtn = GlobalVariable.DaoJiaoBCameraHelper.TakeCameraImage(ref bitmapArray[i], ref ErrorMsg);
                            if (!bRtn)
                            {
                                i--;
                                continue;
                            }
                            m_iIndexPhoto = i;
                            LogHelper.AddDaoJiaoBLog("倒角B-----第" + (i + 1) + "张图片加入算法");
                            //Thread.Sleep(2);
                            if (GlobalVariable.generalSetting.bUseLog)
                            {
                                LogHelper.AddDaoJiaoBLog("倒角B-----第" + i + "张图片拍照并加入算法计算");
                            }
                        }

                        //拍照完成
                        GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DaoJiaoB_PZ_WC, true);
                        //关闭光源
                        GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DaoJiaoB_TXG, false);
                        Thread.Sleep(60);
                        GlobalVariable.DaoJiaoBCameraHelper.Stop();
                        st.Stop();
                        LogHelper.AddDaoJiaoBLog("倒角B拍照共用时" + st.ElapsedMilliseconds);
                        //GlobalVariable.plcControl.WriteOneData(GlobalVariable.plcSetting.SiemensPLCDBNumber, 511, 1, ref plcErrorCode);
                        st.Restart();
                        //GlobalVariable.ctCalculate[_stationNoo].TakePicture = st.ElapsedMilliseconds;
                        bShowEnd = false;
                        ControlStep = 25;
                    }
                    break;

                #endregion 20：拍照+数据传输给算法

                #region 25：等待算法计算完成

                case 25:
                    {
                        HomeForm.home.daoJiaoBPicForm.ChangeSignStartColor(Color.Lime);
                        HomeForm.home.daoJiaoBPicForm.ChangeSignCameraColor(Color.Lime);
                        HomeForm.home.daoJiaoBPicForm.ChangeSignPictureColor(Color.Lime);

                        if (!bMath)
                            break;
                        bMath = false;
                        bShowStart = true;
                        st.Stop();
                        LogHelper.AddDaoJiaoBLog("倒角B计算共用时" + st.ElapsedMilliseconds);
                        ControlStep = 30;
                    }
                    break;

                #endregion 25：等待算法计算完成

                #region 30：向IO卡写入结果

                case 30:
                    {
                        st.Restart();
                        //清除拍照完成的状态
                        GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DaoJiaoB_PZ_WC, false);

                        bool bret1 = GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DaoJiaoB_OK, (result == (int)ResultCMD.OK ? true : false));
                        bool bret2 = GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DaoJiaoB_NG, (result == (int)ResultCMD.NG ? true : false));
                        ControlStep = 0;
                        GlobalVariable.bDaoJiaoBRunManual = false;
                    }
                    break;

                    #endregion 30：向IO卡写入结果
            }
        }

        private int eorraaaaaaa = 0;

        private void CalculateAutoFlow()
        {
            if (m_iIndexPhoto < 0)
                return;
            if (ControlStep <= 0)
                return;
            if (m_iIndexCalculate < m_iIndexPhoto)
            {
                //BitmapContent data = queue.Dequeue();
                //if (data == null)
                //{
                //    semaphoreMath.Release();
                //    return;
                //}
                m_iIndexCalculate++;
                if (m_iIndexCalculate == 0)
                {
                    bOK = true;
                }
                LogHelper.AddDaoJiaoBLog("倒角B-----第" + (m_iIndexCalculate + 1) + "张图片开始计算");
                //需要bOK为true才需要做下去，为false的话就不需要算了
                if (bOK)
                {
                    errorCode = GlobalVariable.DaoJiaoBProcess.Process(bitmapArray[m_iIndexCalculate], m_iIndexCalculate);
                    if (errorCode == (int)NGCode.OK)
                    {
                    }
                    else
                    {
                        ngGraphics.DrawImage(bitmapArray[m_iIndexCalculate], 0, 0);
                        eorraaaaaaa = errorCode;
                        bOK = false;
                    }
                }
                LogHelper.AddDaoJiaoBLog("倒角B-----第" + (m_iIndexCalculate + 1) + "张图片计算完毕");
                if (m_iIndexCalculate == GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.photoNumber - 1)
                {
                  //  LogHelper.AddDaoJiaoBLog("结束计算");
                    if (bOK)
                    {
                        result = (int)ResultCMD.OK;
                    }
                    else
                    {
                        result = (int)ResultCMD.NG;
                        GlobalVariable.totalDataCollect.DaoJiaoBNGNumber++;
                    }
                    GlobalVariable.totalBears.DaoJiaoBCount++;
                    bMath = true;
                }
            }
        }

        /// <summary>
        /// 主要的显示界面流程
        /// </summary>
        private void DisplayAutoFlow()
        {
            if (!bShowStart)
                return;
            bShowStart = false;

            errorReason.Clear();
            if (result == (int)ResultCMD.NG)
            {
                if (!GlobalVariable._dicErrorInfo.ContainsKey(eorraaaaaaa))
                {
                    errorReason.Add(eorraaaaaaa.ToString());
                }
                else
                {
                    errorReason.Add(GlobalVariable._dicErrorInfo[eorraaaaaaa]);
                }

                //获得缺陷
                defectList = GlobalVariable.DaoJiaoBProcess.getDefectsList(eorraaaaaaa);
                string directoryPath = GlobalVariable.path + "pic\\" + strDateTime + "\\DaoJiaoB\\" + DateTime.Now.ToString("HH-mm-ss") + "-" + errorCode;

                if (GlobalVariable.generalSetting.bSaveNG[6])
                {
                    bool bRtn = GlobalMethod.SavePicture(bitmapArray, directoryPath, ref ErrorMsg);
                    if (!bRtn)
                    {
                        LogHelper.AddDaoJiaoBLog("-----倒角B相机的图片并未能够保存下来，异常信息为" + ErrorMsg);
                    }
                }
                else
                {
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                }

                if (defectList.Count > 0)
                {
                    pen.Color = Color.Red;
                    //画红色圆圈，标注错误
                    for (int i = 0; i < defectList.Count; i++)
                    {
                        errorPoint[0].Y = defectList[i].row1;
                        errorPoint[0].X = defectList[i].col1;
                        errorPoint[1].Y = defectList[i].row2;
                        errorPoint[1].X = defectList[i].col2;
                        errorPoint[2].Y = defectList[i].row3;
                        errorPoint[2].X = defectList[i].col3;
                        errorPoint[3].Y = defectList[i].row4;
                        errorPoint[3].X = defectList[i].col4;
                        errorPoint[4].X = errorPoint[0].X;
                        errorPoint[4].Y = errorPoint[0].Y;
                        ngGraphics.DrawLines(pen, errorPoint);

                        ngGraphics.DrawString(defectList[i].area.ToString(), wordFont, brushString, errorPoint[0]);
                    }
                }

                HomeForm.home.daoJiaoBPicForm.showResult(bitmapShowPicture, "NG");

                if (GlobalVariable._dicErrorInfo.ContainsKey(eorraaaaaaa))
                {
                    if (GlobalVariable.daoJiaoBErrCodeStatistic.ContainsKey(GlobalVariable._dicErrorInfo[eorraaaaaaa]))
                    {
                        GlobalVariable.daoJiaoBErrCodeStatistic[GlobalVariable._dicErrorInfo[eorraaaaaaa]]++;
                    }
                    else
                    {
                        GlobalVariable.daoJiaoBErrCodeStatistic.Add(GlobalVariable._dicErrorInfo[eorraaaaaaa], 1);
                    }
                }
                else
                {
                    if (GlobalVariable.daoJiaoBErrCodeStatistic.ContainsKey(eorraaaaaaa.ToString()))
                    {
                        GlobalVariable.daoJiaoBErrCodeStatistic[eorraaaaaaa.ToString()]++;
                    }
                    else
                    {
                        GlobalVariable.daoJiaoBErrCodeStatistic.Add(eorraaaaaaa.ToString(), 1);
                    }
                }
                string msg = "";
                JsonHelper.WriteJsonFile(GlobalVariable.daoJiaoBErrCodeStatistic, FilePath.daoJiaoBStatisticPath, ref msg);
            }
            else
            {
                ngGraphics.DrawImage(bitmapArray[bitmapArray.Length - 1], 0, 0);
                HomeForm.home.daoJiaoBPicForm.showResult(bitmapShowPicture, "OK");
            }

            strErrorType = "";
            if (errorReason.Count != 0)
            {
                for (int i = 0; i < errorReason.Count; i++)
                {
                    strErrorType += errorReason[i] + "\r\n";
                }
            }
            HomeForm.home.daoJiaoBPicForm.ShowErrorMsg(strErrorType);
            HomeForm.home.ShowData(GlobalVariable.totalDataCollect, GlobalVariable.totalBears);
            LogHelper.AddDaoJiaoBLog("结束进行显示");
            bShowEnd = true;
        }

        public void CreateControlThread()
        {
            while (true)
            {
                if (GlobalVariable.pcState == PCState.Run || GlobalVariable.bDaoJiaoBRunManual)
                {
                    ControlAutoFlow();
                }
                Thread.Sleep(20);
            }
        }

        public void CreateCalculateThread()
        {
            while (true)
            {
                if (GlobalVariable.pcState == PCState.Run || GlobalVariable.bDaoJiaoBRunManual)
                {
                    CalculateAutoFlow();
                }
                Thread.Sleep(2);
            }
        }

        public void CreateShowThread()
        {
            while (true)
            {
                if (GlobalVariable.pcState == PCState.Run || GlobalVariable.bDaoJiaoBRunManual)
                {
                    DisplayAutoFlow();
                }
                Thread.Sleep(20);
            }
        }
    }
}