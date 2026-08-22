using BearingInspection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;

namespace YiRongMachine
{
    public class DuanMianAAutoFlow
    {
        /// <summary>
        /// 0=SideA,1=SideB,2=SideIn,3=SideOut
        /// </summary>
        private int _stationNoo = 0;
        
        public Bitmap[] bitmapArray;
        public Bitmap m_bitmapShowPicture;
        public int ControlStep = 0;
        private int result = 0;
        private List<Error_Area> defectList = new List<Error_Area>();    //缺陷列表
        public Surface_Result_Info info;           //DLL返回的图片轴承信息,在自动化流程实例化了
        private Graphics ngGraphics;        //这个在自动化流程里实例化了
        private Pen pen = new Pen(Color.Red, 4);     //画笔
        private string ErrorMsg = "";               //异常信息
        private string log = "";
        private bool bRtn = false;
        private bool bMath = false;
        private bool m_bShowStart = false;
        private bool bShowEnd = false;
        //Semaphore semaphoreMath = new Semaphore(0,2);
        //Semaphore semaphoreShowStart = new Semaphore(0, 2);
        //Semaphore semaphoreShowEnd = new Semaphore(0, 2);

        private DateTime dtStart;
        private List<string> errorReason = new List<string>();
        private string strErrorType = "";
        private PointF[] errorPoint = new PointF[5];
        private Stopwatch st = new Stopwatch();
        private Stopwatch stStep = new Stopwatch();
        private int ControlStep0 = 0;
        //Stopwatch stall = new Stopwatch();

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
        private Stopwatch stpaizhao = new Stopwatch();

        public DuanMianAAutoFlow()
        {
            strDateTime = DateTime.Now.ToString("yyyy-MM-dd");
            init();
        }

        public void init()
        {
            ControlStep = 0;
            bMath = false;
            m_bShowStart = false;
            bShowEnd = true;

            m_iIndexCalculate = -1;
            m_iIndexPhoto = -1;
        }

        /// <summary>
        /// 主要的控制流程
        /// </summary>
        private void ControlAutoFlow()
        {
            if (ControlStep0 != ControlStep)
            {
                stStep.Stop();
                LogHelper.AddDuanMianALog("端面A Step用时(" + ControlStep0.ToString() + "到" + ControlStep.ToString() + "):" + stStep.ElapsedMilliseconds);
                stStep.Restart();
                ControlStep0 = ControlStep;
            }
            switch (ControlStep)
            {
                #region 0：读取IO板卡信号

                case 0:
                    {
                        HomeForm.home.duanMianAPicForm.ChangeSignStartColor(Color.Blue);
                        HomeForm.home.duanMianAPicForm.ChangeSignCameraColor(Color.Blue);
                        HomeForm.home.duanMianAPicForm.ChangeSignPictureColor(Color.Blue);
                        m_iIndexCalculate = -1;
                        m_iIndexPhoto = -1;

                        bool bret = GlobalVariable.ioBoardHelper.readInput((int)IOBoardHelper.enInputType.DuanMianA_CS_QD);
                        bool rising = !previousState && bret;
                        previousState = bret;
                        if (rising || GlobalVariable.bDuanMianARunManual)
                        {
                            GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DuanMianA_PZ_WC, false);
                            GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DuanMianA_GY_YD, false);
                            GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DuanMianA_OK, false);
                            GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DuanMianA_NG, false);
                            Thread.Sleep(20);
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
                            if (GlobalVariable.bForbidden[0])
                            {
                                result = (int)ResultCMD.OK;
                                if (GlobalVariable.generalSetting.bUseLog)
                                {
                                    LogHelper.AddDuanMianALog("端面A-----该工位已经被禁用");
                                }
                                ControlStep = 40;
                                break;
                            }

                            if (GlobalVariable.bDuanMianAParamUpdate)
                            {
                                GlobalVariable.DuanMianAProcess.setModelInfo(GlobalVariable.configSetting.duanMianAModel);
                                GlobalVariable.DuanMianAProcess.setParamInfo(GlobalVariable.configSetting.duanMianAParam);
                                GlobalVariable.DuanMianAProcess.setFlag(GlobalVariable.configSetting.duanMianAFlag);
                                GlobalVariable.DuanMianAProcess.updateModel();
                                //更新ROI
                                bRtn = GlobalVariable.DuanMianACameraHelper.SetCameraROI(
                                        GlobalVariable.configSetting.duanMianACameraPhotoSetting.ROIWidthTrue,
                                        GlobalVariable.configSetting.duanMianACameraPhotoSetting.ROIHeihgtTrue,
                                        GlobalVariable.configSetting.duanMianACameraPhotoSetting.OffsetX,
                                        GlobalVariable.configSetting.duanMianACameraPhotoSetting.OffsetY);
                                //更改参数后重新实例化Bitmap
                                bitmapArray = new Bitmap[GlobalVariable.iDuanMianAPictureNumber];
                                for (int i = 0; i < bitmapArray.Length; i++)
                                {
                                    bitmapArray[i] = new Bitmap(GlobalVariable.configSetting.duanMianACameraPhotoSetting.ROIWidthTrue, GlobalVariable.configSetting.duanMianACameraPhotoSetting.ROIHeihgtTrue);
                                }
                                m_bitmapShowPicture = new Bitmap(bitmapArray[0].Width, bitmapArray[0].Height);
                                ngGraphics = Graphics.FromImage(m_bitmapShowPicture);
                                if (GlobalVariable.generalSetting.bUseLog)
                                {
                                    LogHelper.AddDuanMianALog("端面A-----参数已经发生更改，成功重新将参数信息上传给算法");
                                }
                                GlobalVariable.bDuanMianAParamUpdate = false;
                            }

                            ControlStep = 20;
                        }
                        catch (Exception e)
                        {
                            log = "端面A-----更新参数步骤出现异常，异常信息为" + e.Message;
                            LogHelper.AddDuanMianALog(log);
                            AlarmDialog a = new AlarmDialog(log, MsgType.Retry);
                            a.ShowDialog();
                            GlobalVariable.bDuanMianARunManual = false;
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

                        HomeForm.home.duanMianAPicForm.ChangeSignStartColor(Color.Lime);
                        HomeForm.home.duanMianAPicForm.ChangeSignCameraColor(Color.Blue);
                        HomeForm.home.duanMianAPicForm.ChangeSignPictureColor(Color.Blue);

                        double oldExposure = 0f;
                        GlobalVariable.DuanMianACameraHelper.Start();
                        st.Start();
                        GlobalVariable.lightHelper[0].setL();
                        //拍照全流程
                        for (int i = 0; i < 4; i++)
                        {
                            //打开光源
                            GlobalVariable.lightHelper[0].OperateLight(GlobalVariable.configSetting.duanMianACameraPhotoSetting.lightsCmd[i]);
                            Thread.Sleep(5);
                            LogHelper.AddDuanMianALog("打開光源：" + st.ElapsedMilliseconds.ToString());
                            //写入曝光值，如果下一个曝光值和上一个一样，则不需要写入
                            if (oldExposure != GlobalVariable.configSetting.duanMianACameraPhotoSetting.exposure[i])
                            {
                                bRtn = GlobalVariable.DuanMianACameraHelper.SetCameraExposureTime(GlobalVariable.configSetting.duanMianACameraPhotoSetting.exposure[i], ref ErrorMsg);
                                if (!bRtn)
                                {
                                    log = "A面相机拍第" + i + "照片时，写入曝光值失败，异常信息为" + ErrorMsg;
                                    LogHelper.AddDuanMianALog(log);
                                    AlarmDialog a = new AlarmDialog(log, MsgType.Retry);
                                    a.ShowDialog();
                                    break;
                                }
                                oldExposure = GlobalVariable.configSetting.duanMianACameraPhotoSetting.exposure[i];
                            }
                            //拍照
                            LogHelper.AddDaoJiaoALog("T: " + i.ToString());
                            bRtn = GlobalVariable.DuanMianACameraHelper.TakeCameraImage(ref bitmapArray[i], ref ErrorMsg);
                            if (!bRtn)
                            {
                                i--;
                                continue;
                            }
                            m_iIndexPhoto = i;
                            LogHelper.AddDuanMianALog("第" + (i + 1) + "张图片加入算法");
                            //Thread.Sleep(2);
                            if (GlobalVariable.generalSetting.bUseLog)
                            {
                                LogHelper.AddDuanMianALog("端面A-----第" + i + "张图片拍照并加入算法计算");
                            }
                        }
                        bShowEnd = false;
                        ControlStep = 21;
                    }
                    break;

                #endregion 20：拍照+数据传输给算法

                #region 21:光源运动

                case 21:
                    {
                        GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DuanMianA_GY_YD, true);
                        ControlStep = 22;
                    }
                    break;

                #endregion 21:光源运动

                #region 22:光源运动到位

                case 22:
                    {
                        if (GlobalVariable.ioBoardHelper.readInput((int)IOBoardHelper.enInputType.DuanMianA_GY_DW) ||
                           GlobalVariable.bDuanMianARunManual)
                            ControlStep = 23;
                    }
                    break;

                #endregion 22:光源运动到位

                #region 23：继续拍照+数据传输给算法

                case 23:
                    {
                        double oldExposure = 0f;
                        //拍照全流程
                        for (int i = 4; i < bitmapArray.Length; i++)
                        {
                            LogHelper.AddDaoJiaoALog("T: " + i.ToString());
                            //打开光源
                            GlobalVariable.lightHelper[0].OperateLight(GlobalVariable.configSetting.duanMianACameraPhotoSetting.lightsCmd[i]);
                            Thread.Sleep(5);
                            //写入曝光值，如果下一个曝光值和上一个一样，则不需要写入
                            if (oldExposure != GlobalVariable.configSetting.duanMianACameraPhotoSetting.exposure[i])
                            {
                                bRtn = GlobalVariable.DuanMianACameraHelper.SetCameraExposureTime(GlobalVariable.configSetting.duanMianACameraPhotoSetting.exposure[i], ref ErrorMsg);
                                if (!bRtn)
                                {
                                    log = "A面相机拍第" + i + "照片时，写入曝光值失败，异常信息为" + ErrorMsg;
                                    LogHelper.AddDuanMianALog(log);
                                    AlarmDialog a = new AlarmDialog(log, MsgType.Retry);
                                    a.ShowDialog();
                                    break;
                                }
                                oldExposure = GlobalVariable.configSetting.duanMianACameraPhotoSetting.exposure[i];
                            }
                            //拍照
                            bRtn = GlobalVariable.DuanMianACameraHelper.TakeCameraImage(ref bitmapArray[i], ref ErrorMsg);
                            if (!bRtn)
                            {
                                i--;
                                continue;
                            }

                            m_iIndexPhoto = i;
                            LogHelper.AddDuanMianALog("第" + (i + 1) + "张图片加入算法");
                            Thread.Sleep(2);
                            if (GlobalVariable.generalSetting.bUseLog)
                            {
                                LogHelper.AddDuanMianALog("端面A-----第" + i + "张图片拍照并加入算法计算");
                            }
                        }

                        //拍照完成
                        GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DuanMianA_PZ_WC, true);
                        //清空光源运动信号
                        GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DuanMianA_GY_YD, false);
                        Thread.Sleep(60);
                        //关闭光源
                        GlobalVariable.lightHelper[0].OperateLight("S01000#S02000#S10000#S07000#S08000#S09000#S03000#");
                        GlobalVariable.DuanMianACameraHelper.Stop();
                        st.Stop();
                        LogHelper.AddDuanMianALog("A面拍照共用时" + st.ElapsedMilliseconds);
                        st.Restart();
                        GlobalVariable.ctCalculate[_stationNoo].TakePicture = st.ElapsedMilliseconds;
                        ControlStep = 30;
                    }
                    break;

                #endregion 23：继续拍照+数据传输给算法

                #region 30：等待算法计算完成

                case 30:
                    {
                        HomeForm.home.duanMianAPicForm.ChangeSignStartColor(Color.Lime);
                        HomeForm.home.duanMianAPicForm.ChangeSignCameraColor(Color.Lime);
                        HomeForm.home.duanMianAPicForm.ChangeSignPictureColor(Color.Lime);

                        if (!bMath)
                            break;
                        bMath = false;
                        st.Stop();
                        LogHelper.AddDuanMianALog("A面计算共用时" + st.ElapsedMilliseconds);
                        ControlStep = 40;
                    }
                    break;

                #endregion 30：等待算法计算完成

                #region 40：向IO卡写入结果

                case 40:
                    {
                        st.Restart();
                        if (!GlobalVariable.bForbidden[0])
                        {
                            if (result == (int)ResultCMD.OK)
                            {

                                result = GlobalVariable.DuanMianAProcess.ringRegionProcess();
                                eorraaaaaaa = result;
                                if (result != (int)ResultCMD.OK)
                                {
                                    result = (int)ResultCMD.NG;
                                    GlobalVariable.totalDataCollect.DuanMianANGNumber++;
                                }
                            }
                            m_bShowStart = true;
                        }
                        //清除拍照完成的状态
                        GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DuanMianA_PZ_WC, false);

                        bool bret1 = GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DuanMianA_OK, (result == (int)ResultCMD.OK ? true : false));
                        bool bret2 = GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DuanMianA_NG, (result == (int)ResultCMD.NG ? true : false));
                        ControlStep = 0;
                    }
                    break;

                    #endregion 40：向IO卡写入结果
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
                m_iIndexCalculate++;
                if (m_iIndexCalculate == 0)
                {
                    bOK = true;
                }
                if (m_iIndexCalculate == 2)
                {
                    ngGraphics.DrawImage(bitmapArray[m_iIndexCalculate], 0, 0);
                }
                //需要bOK为true才需要做下去，为false的话就不需要算了
                if (bOK)
                {
                    string eee = "";
                    errorCode = GlobalVariable.DuanMianAProcess.Process(bitmapArray[m_iIndexCalculate], m_iIndexCalculate);
                    if (errorCode != (int)NGCode.OK)
                    {
                        ngGraphics.DrawImage(bitmapArray[m_iIndexCalculate], 0, 0);
                        eorraaaaaaa = errorCode;
                        bOK = false;
                    }
                }
                LogHelper.AddDuanMianALog("端面A-----第" + (m_iIndexCalculate + 1) + "张图片计算完毕");
                if (m_iIndexCalculate == GlobalVariable.configSetting.duanMianACameraPhotoSetting.photoNumber - 1)
                {
                   // LogHelper.AddDuanMianALog("结束计算");
                    if (bOK)
                    {
                        result = (int)ResultCMD.OK;
                    }
                    else
                    {
                        result = (int)ResultCMD.NG;
                        GlobalVariable.totalDataCollect.DuanMianANGNumber++;
                    }
                    GlobalVariable.totalBears.DuanMianACount++;
                    bMath = true;
                }
            }
        }

        /// <summary>
        /// 主要的显示界面流程
        /// </summary>
        private void DisplayAutoFlow()
        {
            if (!m_bShowStart)
                return;
            m_bShowStart = false;
            GlobalVariable.bDuanMianARunManual = false;
            errorReason.Clear();
            info = GlobalVariable.DuanMianAProcess.getResultInfo();
            JsonHelper.WriteJsonFile(info, FilePath.ResInfoPath + "sideA.Json", ref ErrorMsg);

            if (result != (int)ResultCMD.OK)
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
                defectList = GlobalVariable.DuanMianAProcess.getDefectsList(eorraaaaaaa);
                string directoryPath = GlobalVariable.path + "pic\\" + strDateTime + "\\DuanMianA\\" + DateTime.Now.ToString("HH-mm-ss") + "-" + errorCode;

                if (GlobalVariable.generalSetting.bSaveNG[0])
                {
                    bool bRtn = GlobalMethod.SavePicture(bitmapArray, directoryPath, ref ErrorMsg);
                    if (!bRtn)
                    {
                        LogHelper.AddDuanMianALog("-----端面A相机的图片并未能够保存下来，异常信息为" + ErrorMsg);
                    }
                }
                else
                {
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }
                }

                //ngGraphics.DrawImage(bitmapArray[2], 0, 0);
                //ngGraphics.FillRectangle(new SolidBrush(Color.Red), 100, 50, 100, 100);
                //ngGraphics.DrawRectangle(new Pen(Color.Red), 0, 0, 100, 100);

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
                else
                {
                    ngGraphics.DrawImage(bitmapArray[2], 0, 0);
                }
                pen.Color = Color.Lime;
                GlobalMethod.DrawYuan_SideAB(ngGraphics, pen, info);
                HomeForm.home.duanMianAPicForm.showResult(m_bitmapShowPicture, "NG");

                if (GlobalVariable._dicErrorInfo.ContainsKey(eorraaaaaaa))
                {
                    if (GlobalVariable.duanMianAErrCodeStatistic.ContainsKey(GlobalVariable._dicErrorInfo[eorraaaaaaa]))
                    {
                        GlobalVariable.duanMianAErrCodeStatistic[GlobalVariable._dicErrorInfo[eorraaaaaaa]]++;
                    }
                    else
                    {
                        GlobalVariable.duanMianAErrCodeStatistic.Add(GlobalVariable._dicErrorInfo[eorraaaaaaa], 1);
                    }
                }
                else
                {
                    if (GlobalVariable.duanMianAErrCodeStatistic.ContainsKey(eorraaaaaaa.ToString()))
                    {
                        GlobalVariable.duanMianAErrCodeStatistic[eorraaaaaaa.ToString()]++;
                    }
                    else
                    {
                        GlobalVariable.duanMianAErrCodeStatistic.Add(eorraaaaaaa.ToString(), 1);
                    }
                }
                string msg = "";
                JsonHelper.WriteJsonFile(GlobalVariable.duanMianAErrCodeStatistic, FilePath.duanMianAStatisticPath, ref msg);
            }
            else
            {
                ngGraphics.DrawImage(bitmapArray[2], 0, 0);
                pen.Color = Color.Lime;
                GlobalMethod.DrawYuan_SideAB(ngGraphics, pen, info);
                HomeForm.home.duanMianAPicForm.showResult(m_bitmapShowPicture, "OK");
            }

            strErrorType = "";
            if (errorReason.Count != 0)
            {
                for (int i = 0; i < errorReason.Count; i++)
                {
                    strErrorType += errorReason[i] + "\r\n";
                }
            }
            HomeForm.home.duanMianAPicForm.ShowErrorMsg(strErrorType);
            HomeForm.home.ShowData(GlobalVariable.totalDataCollect, GlobalVariable.totalBears);
            LogHelper.AddDuanMianALog("结束进行显示");
            bShowEnd = true;
        }

        public void CreateControlThread()
        {
            while (true)
            {
                if (GlobalVariable.pcState == PCState.Run || GlobalVariable.bDuanMianARunManual)
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
                if (GlobalVariable.pcState == PCState.Run || GlobalVariable.bDuanMianARunManual)
                {
                    CalculateAutoFlow();
                }
                Thread.Sleep(5);
            }
        }

        public void CreateShowThread()
        {
            while (true)
            {
                if (GlobalVariable.pcState == PCState.Run || GlobalVariable.bDuanMianARunManual)
                {
                    DisplayAutoFlow();
                }
                Thread.Sleep(20);
            }
        }
    }
}