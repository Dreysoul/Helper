using BearingInspection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;

namespace YiRongMachine
{
    public class MultiImageAutoFlow
    {
        string[] m_stationName = GlobalVariable.m_stationName;
        Dictionary<string, int>[] m_ErrCodeStatistic = GlobalVariable.m_ErrCodeStatistic;
        int[] m_PictureNumber = GlobalVariable.m_PictureNumber;
        bool[] m_ParamUpdate = GlobalVariable.m_ParamUpdate;
        bool[] m_RunManual = GlobalVariable.m_RunManual;
        IAreaScanCameraHelper[] m_cameraHelper = GlobalVariable.m_cameraHelper;
        CameraPhotoSetting[] m_photoSetting = GlobalVariable.m_photoSetting;
        int[] m_TotalDataCollect = GlobalVariable.m_TotalDataCollect;
        int[] m_totalBears = GlobalVariable.m_totalBears;

        multiImgProcess[] m_process = GlobalVariable.m_multiImgProcess;
        MultiImg_Model_Info[] m_model = GlobalVariable.m_MultiImgModel;
        MultiImg_Flag[] m_flags = GlobalVariable.m_MultiImgFlags;
        MultiImg_Param_Info[] m_params = GlobalVariable.m_MultiImgParams;

        ShowPictureControl[] m_PicForm;
        private int m_station = 3;

        public Bitmap[] bitmapArray;
        public Bitmap[] bitmapCopy;
        public Bitmap bitmapShowPicture;
        public int ControlStep = 0;
        private int result = 0;
        private List<Error_Area> defectList = new List<Error_Area>();    //缺陷列表
        public OuterSide_Result_Info info;
        private Graphics ngGraphics;        //这个在自动化流程里实例化了
        private Pen pen = new Pen(Color.Red, 3);
        private string ErrorMsg = "";       //异常信息
        private string log = "";

        private bool bRtn = false;
        private bool bMath = false;
        private bool bShowStart = false;
        private bool bShowEnd = false;
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

        //计算的指针
        private int m_iIndexCalculate = -1;

        //拍照的指针
        private int m_iIndexPhoto = -1;

        private Queue<BitmapContent> m_queue = new Queue<BitmapContent>();
        private BitmapContent[] m_data;

        public MultiImageAutoFlow(int station)
        {
            m_station = station;
            strDateTime = DateTime.Now.ToString("yyyy-MM-dd");
            m_data = new BitmapContent[m_PictureNumber[m_station]];
            for (int i = 0; i < m_PictureNumber[m_station]; i++)
            {
                m_data[i] = new BitmapContent();
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

        public void initShowForm()
        {
            init();
            ShowPictureControl[] PicForm =
            {
                HomeForm.home.duanMianAPicForm,
                HomeForm.home.waiYuanAPicForm,
                HomeForm.home.daoJiaoAPicForm,
                HomeForm.home.neiKongPicForm,
                HomeForm.home.duanMianBPicForm,
                HomeForm.home.waiYuanBPicForm,
                HomeForm.home.daoJiaoBPicForm
            };
            m_PicForm = PicForm;
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
                        m_PicForm[m_station].ChangeSignStartColor(Color.Blue);
                        m_PicForm[m_station].ChangeSignCameraColor(Color.Blue);
                        m_PicForm[m_station].ChangeSignPictureColor(Color.Blue);
                        m_iIndexCalculate = -1;
                        m_iIndexPhoto = -1;

                        bool bret = GlobalVariable.ioBoardHelper.readInput((int)IOBoardHelper.enInputType.NeiKong_CS_QD);

                        if (bret || m_RunManual[m_station])
                        {
                            LogHelper.AddLog("读取启动信号",m_station);
                            GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiKong_BG, false);
                            GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiKong_PZ_WC, false);
                            GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiKong_OK, false);
                            GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiKong_NG, false);
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
                            if (GlobalVariable.bForbidden[m_station])
                            {
                                result = (int)ResultCMD.OK;
                                if (GlobalVariable.generalSetting.bUseLog)
                                {
                                    LogHelper.AddLog("-该工位已经被禁用", m_station);
                                }
                                ControlStep = 30;
                                break;
                            }

                            if (m_ParamUpdate[m_station])
                            {
                                m_process[m_station].setModelInfo(m_model[m_station]);
                                m_process[m_station].setParamInfo(m_params[m_station]);
                                m_process[m_station].setFlag(m_flags[m_station]);
                                //GlobalVariable.WaiYuanAProcess.updateModel();
                                //更新ROI
                                bRtn = m_cameraHelper[m_station].SetCameraROI(
                                        m_photoSetting[m_station].ROIWidthTrue,
                                        m_photoSetting[m_station].ROIHeihgtTrue,
                                        m_photoSetting[m_station].OffsetX,
                                        m_photoSetting[m_station].OffsetY);
                                //更改参数后重新实例化Bitmap
                                bitmapArray = new Bitmap[m_photoSetting[m_station].photoNumber];
                                for (int i = 0; i < bitmapArray.Length; i++)
                                {
                                    bitmapArray[i] = new Bitmap(m_photoSetting[m_station].ROIWidthTrue, m_photoSetting[m_station].ROIHeihgtTrue);
                                }
                                bitmapShowPicture = new Bitmap(bitmapArray[0].Width, bitmapArray[0].Height);
                                ngGraphics = Graphics.FromImage(bitmapShowPicture);
                                if (GlobalVariable.generalSetting.bUseLog)
                                {
                                    LogHelper.AddLog("-参数已经发生更改，成功重新将参数信息上传给算法", m_station);
                                }
                                m_ParamUpdate[m_station] = false;
                            }

                            ControlStep = 20;
                        }
                        catch (Exception e)
                        {
                            log = "-更新参数步骤出现异常，异常信息为" + e.Message;
                            LogHelper.AddLog(log, m_station);
                            AlarmDialog a = new AlarmDialog(log, MsgType.Retry);
                            a.ShowDialog();
                            m_RunManual[m_station] = false;
                        }
                    }
                    break;

                #endregion 10：参数更改后需要更新参数

                #region 20：拍照+数据传输给算法

                case 20:
                    {
                        st.Restart();
                        //bShowFinish = true;
                        if (!bShowEnd)
                            break;

                        m_PicForm[m_station].ChangeSignStartColor(Color.Lime);
                        m_PicForm[m_station].ChangeSignCameraColor(Color.Blue);
                        m_PicForm[m_station].ChangeSignPictureColor(Color.Blue);

                        double oldExposure = 0f;
                        m_cameraHelper[m_station].Start();

                        //打开光源
                        GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiKong_BG, true);
                        LogHelper.AddLog("打开光源", m_station);
                        //拍照全流程
                        bRtn = m_cameraHelper[m_station].SetCameraExposureTime(m_photoSetting[m_station].exposure[0], ref ErrorMsg);
                        if (!bRtn)
                        {
                            log = "相机写入曝光值失败，异常信息为" + ErrorMsg;
                            LogHelper.AddLog(log, m_station);
                            AlarmDialog a = new AlarmDialog(log, MsgType.Retry);
                            a.ShowDialog();
                        }
                        st.Restart();
                        for (int i = 0; i < bitmapArray.Length; i++)
                        {
                            //拍照
                            stall.Restart();
                            GlobalMethod.delay_ms((uint)m_photoSetting[m_station].photoSpan);
                            bRtn = m_cameraHelper[m_station].TakeCameraImage(ref bitmapArray[i], ref ErrorMsg);
                            stall.Stop();
                            long aaa = stall.ElapsedMilliseconds;
                            if (!bRtn)
                            {
                                i--;
                                continue;
                            }

                            m_iIndexPhoto = i;
                            if (GlobalVariable.generalSetting.bUseLog)
                            {
                            }
                        }
                        st.Stop();
                        LogHelper.AddLog("00", m_station);
                        //拍照完成
                        GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiKong_PZ_WC, true);
                        //关闭光源
                        GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiKong_BG, false);
                        LogHelper.AddLog("关闭光影", m_station);
                        Thread.Sleep(60);
                        m_cameraHelper[m_station].Stop();
                        st.Restart();
                        bShowEnd = false;
                        ControlStep = 25;
                    }
                    break;

                #endregion 20：拍照+数据传输给算法

                #region 25：等待算法计算完成

                case 25:
                    {
                        m_PicForm[m_station].ChangeSignStartColor(Color.Lime);
                        m_PicForm[m_station].ChangeSignCameraColor(Color.Lime);
                        m_PicForm[m_station].ChangeSignPictureColor(Color.Lime);

                        if (!bMath)
                            break;
                        bMath = false;
                        bShowStart = true;
                        st.Stop();
                        LogHelper.AddLog("计算共用时" + st.ElapsedMilliseconds, m_station);
                        ControlStep = 30;
                    }
                    break;

                #endregion 25：等待算法计算完成

                #region 30：向IO卡写入结果

                case 30:
                    {
                        st.Restart();

                        //清除拍照完成的状态
                        GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiKong_PZ_WC, false);

                        bool bret1 = GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiKong_OK, (result == (int)ResultCMD.OK ? true : false));
                        bool bret2 = GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiKong_NG, (result == (int)ResultCMD.NG ? true : false));
                        //Thread.Sleep(10);
                        ControlStep = 0;
                        m_RunManual[m_station] = false;
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
                //BitmapContent m_data = m_queue.Dequeue();
                //if (m_data == null)
                //{
                //    semaphoreMath.Release();
                //    return;
                //}
                m_iIndexCalculate++;
                if (m_iIndexCalculate == 0)
                {
                    bOK = true;
                }
                //需要bOK为true才需要做下去，为false的话就不需要算了
                if (bOK)
                {
                    //bitmapArray[m_data.index].Save("bit" + m_data.index.ToString() + ".bmp");
                    errorCode = m_process[m_station].Process(bitmapArray[m_iIndexCalculate], m_iIndexCalculate);
                    if (errorCode == (int)NGCode.OK)
                    {
                    }
                    else
                    {
                        if (m_iIndexCalculate >= 0)
                            ngGraphics.DrawImage(bitmapArray[m_iIndexCalculate], 0, 0);
                        eorraaaaaaa = errorCode;
                        bOK = false;
                    }
                }
                if (m_iIndexCalculate == m_photoSetting[m_station].photoNumber - 1)
                {
                    if (bOK)
                    {
                        result = (int)ResultCMD.OK;
                    }
                    else
                    {
                        result = (int)ResultCMD.NG;
                        m_TotalDataCollect[m_station]++;
                    }
                    m_totalBears[m_station]++;
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
                defectList = m_process[m_station].getDefectsList(eorraaaaaaa);
                string directoryPath = GlobalVariable.path + "pic\\" + strDateTime + m_stationName[m_station] + DateTime.Now.ToString("HH-mm-ss") + "-" + errorCode;

                if (GlobalVariable.generalSetting.bSaveNG[m_station])
                {
                    bool bRtn = GlobalMethod.SavePicture(bitmapArray, directoryPath, ref ErrorMsg);
                    if (!bRtn)
                    {
                        LogHelper.AddLog("-相机的图片并未能够保存下来，异常信息为" + ErrorMsg, m_station);
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

                m_PicForm[m_station].showResult(bitmapShowPicture, "NG");

                if (GlobalVariable._dicErrorInfo.ContainsKey(eorraaaaaaa))
                {
                    if (m_ErrCodeStatistic[m_station].ContainsKey(GlobalVariable._dicErrorInfo[eorraaaaaaa]))
                    {
                        m_ErrCodeStatistic[m_station][GlobalVariable._dicErrorInfo[eorraaaaaaa]]++;
                    }
                    else
                    {
                        m_ErrCodeStatistic[m_station].Add(GlobalVariable._dicErrorInfo[eorraaaaaaa], 1);
                    }
                }
                else
                {
                    if (m_ErrCodeStatistic[m_station].ContainsKey(eorraaaaaaa.ToString()))
                    {
                        m_ErrCodeStatistic[m_station][eorraaaaaaa.ToString()]++;
                    }
                    else
                    {
                        m_ErrCodeStatistic[m_station].Add(eorraaaaaaa.ToString(), 1);
                    }
                }
                string msg = "";
                JsonHelper.WriteJsonFile(m_ErrCodeStatistic[m_station], FilePath.neiKongStatisticPath, ref msg);
            }
            else
            {
                ngGraphics.DrawImage(bitmapArray[bitmapArray.Length - 1], 0, 0);
                m_PicForm[m_station].showResult(bitmapShowPicture, "OK");
            }

            strErrorType = "";
            if (errorReason.Count != 0)
            {
                for (int i = 0; i < errorReason.Count; i++)
                {
                    strErrorType += errorReason[i] + "\r\n";
                }
            }
            m_PicForm[m_station].ShowErrorMsg(strErrorType);
            HomeForm.home.ShowData(GlobalVariable.totalDataCollect, GlobalVariable.totalBears);
            LogHelper.AddLog("结束进行显示", m_station);
            bShowEnd = true;
        }

        public void CreateControlThread()
        {
            while (true)
            {
                if (GlobalVariable.pcState == PCState.Run || m_RunManual[m_station])
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
                if (GlobalVariable.pcState == PCState.Run || m_RunManual[m_station])
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
                if (GlobalVariable.pcState == PCState.Run || m_RunManual[m_station])
                {
                    DisplayAutoFlow();
                }
                Thread.Sleep(20);
            }
        }
    }
}