using BearingInspection;
using System;
using System.Collections.Generic;
using System.IO;

namespace YiRongMachine
{
    public class GlobalVariable
    {
        public static int iWorkStation = 2;
        public static PCState pcState = PCState.Pause;
        public const int iDuanMianAPictureNumber = 13;
        public const int iWaiYuanAPictureNumber = 2;
        public const int iDaoJiaoAPictureNumber = 60;
        public const int iNeiKongPictureNumber = 60;
        public const int iDuanMianBPictureNumber = 13;
        public const int iWaiYuanBPictureNumber = 60;
        public const int iDaoJiaoBPictureNumber = 60;

        //相機相關設置
        public static IAreaScanCameraHelper DuanMianACameraHelper;

        public static IAreaScanCameraHelper WaiYuanACameraHelper;
        public static IAreaScanCameraHelper DaoJiaoACameraHelper;
        public static IAreaScanCameraHelper NeiKongCameraHelper;
        public static IAreaScanCameraHelper DuanMianBCameraHelper;
        public static IAreaScanCameraHelper WaiYuanBCameraHelper;
        public static IAreaScanCameraHelper DaoJiaoBCameraHelper;

        public static Dictionary<string, int> duanMianAErrCodeStatistic = new Dictionary<string, int>();
        public static Dictionary<string, int> waiYuanAErrCodeStatistic = new Dictionary<string, int>();
        public static Dictionary<string, int> daoJiaoAErrCodeStatistic = new Dictionary<string, int>();
        public static Dictionary<string, int> neiKongErrCodeStatistic = new Dictionary<string, int>();
        public static Dictionary<string, int> duanMianBErrCodeStatistic = new Dictionary<string, int>();
        public static Dictionary<string, int> waiYuanBErrCodeStatistic = new Dictionary<string, int>();
        public static Dictionary<string, int> daoJiaoBErrCodeStatistic = new Dictionary<string, int>();

        //算法相关的类
        public static surfaceImageProcess DuanMianAProcess;
        public static outerSideImageProcess WaiYuanAProcess;
        public static multiImgProcess DaoJiaoAProcess;
        public static multiImgProcess NeiKongProcess;
        public static surfaceImageProcess DuanMianBProcess;
        public static multiImgProcess WaiYuanBProcess;
        public static multiImgProcess DaoJiaoBProcess;

        //线程相关
        public static DuanMianAAutoFlow duanMianAAutoFlow;
        public static WaiYuanAAutoFlow waiYuanAAutoFlow;
        public static DaoJiaoAAutoFlow daoJiaoAAutoFlow;
        //public static MultiImageAutoFlow daoJiaoAAutoFlow;
        public static NeiKongAutoFlow neiKongAutoFlow;
        public static DuanMianBAutoFlow duanMianBAutoFlow;
        public static WaiYuanBAutoFlow waiYuanBAutoFlow;
        public static DaoJiaoBAutoFlow daoJiaoBAutoFlow;
        public static NeiKongAutoFlow neiKongBAutoFlow;

        public const int iStationNumber = 7;
        public const string version = "V2.2.1[2024-04-30] ";
        public const string path = "D:\\";//**************************
        public static bool bPLCConnect = false;

        //硬件实例化
        public static IOBoardHelper ioBoardHelper;

        public static LightHelper[] lightHelper;

        //配置文件加载
        public static MachineSetting machineSetting;

        public static ConfigSetting configSetting;
        public static ConfigSetting TmpConfigSetting;
        public static HardwareSetting hardwareSetting;
        public static GeneralSetting generalSetting;

        //数据统计
        public static TotalDataCollect totalDataCollect;

        public static TotalBears totalBears;
        public static List<int[]> OneBearInfor;
        public static CT[] ctCalculate;

        public static bool[] bForbidden;
        public static string configname = "Bear";
        public static bool bDuanMianAParamUpdate = true;
        public static bool bWaiYuanAParamUpdate = true;
        public static bool bDaoJiaoAParamUpdate = true;
        public static bool bNeiKongParamUpdate = true;
        public static bool bDuanMianBParamUpdate = true;
        public static bool bWaiYuanBParamUpdate = true;
        public static bool bDaoJiaoBParamUpdate = true;
        public static bool bNeiKongBParamUpdate = true;
        //手动运行控制标记

        public static bool bDuanMianARunManual = false;
        public static bool bWaiYuanARunManual = false;
        public static bool bDaoJiaoARunManual = false;
        public static bool bNeiKongRunManual = false;
        public static bool bDuanMianBRunManual = false;
        public static bool bWaiYuanBRunManual = false;
        public static bool bDaoJiaoBRunManual = false;
        public static bool bNeiKongBRunManual = false;
        public static bool bNeiQuanBRunManual = false;
        public static bool[] bParamFreshForManual;
        public static LightType light_type = LightType.HaiShi_12T;//**************************

        public static Dictionary<int, string> _dicErrorInfo = new Dictionary<int, string>();
        #region 集合
        public static string[] m_stationName =
        {
            "端面A",
            "外圆A",
            "倒角A",
            "内孔",
            "端面B",
            "外圆B",
            "倒角B",
            "内孔B"
        };
        public static multiImgProcess[] m_multiImgProcess =
        {
            null,
            null,
            GlobalVariable.DaoJiaoAProcess,
            GlobalVariable.NeiKongProcess,
            null,
            GlobalVariable.WaiYuanBProcess,
            GlobalVariable.DaoJiaoBProcess
        };
        public static MultiImg_Model_Info[] m_MultiImgModel =
        {
            GlobalVariable.configSetting.daoJiaoAModel,
            GlobalVariable.configSetting.daoJiaoAModel,
            GlobalVariable.configSetting.daoJiaoAModel,
            GlobalVariable.configSetting.neiKongModel,
            GlobalVariable.configSetting.daoJiaoAModel,
            GlobalVariable.configSetting.waiYuanBModel,
            GlobalVariable.configSetting.daoJiaoBModel
        };
        public static MultiImg_Flag[] m_MultiImgFlags =
        {
            GlobalVariable.configSetting.daoJiaoAFlag,
            GlobalVariable.configSetting.daoJiaoAFlag,
            GlobalVariable.configSetting.daoJiaoAFlag,
            GlobalVariable.configSetting.neiKongFlag,
            GlobalVariable.configSetting.daoJiaoAFlag,
            GlobalVariable.configSetting.waiYuanBFlag,
            GlobalVariable.configSetting.daoJiaoBFlag
        };
        public static MultiImg_Param_Info[] m_MultiImgParams =
        {
            GlobalVariable.configSetting.daoJiaoAParam,
            GlobalVariable.configSetting.daoJiaoAParam,
            GlobalVariable.configSetting.daoJiaoAParam,
            GlobalVariable.configSetting.neiKongParam,
            GlobalVariable.configSetting.daoJiaoAParam,
            GlobalVariable.configSetting.waiYuanBParam,
            GlobalVariable.configSetting.daoJiaoBParam
        };
        public static Dictionary<string, int>[] m_ErrCodeStatistic =
        {
            GlobalVariable.duanMianAErrCodeStatistic,
            GlobalVariable.waiYuanAErrCodeStatistic,
            GlobalVariable.daoJiaoAErrCodeStatistic,
            GlobalVariable.neiKongErrCodeStatistic,
            GlobalVariable.duanMianBErrCodeStatistic,
            GlobalVariable.waiYuanBErrCodeStatistic,
            GlobalVariable.daoJiaoBErrCodeStatistic
        };
        public static int[] m_PictureNumber =
        {
            GlobalVariable.iDuanMianAPictureNumber ,
            GlobalVariable.iWaiYuanAPictureNumber ,
            GlobalVariable.iDaoJiaoAPictureNumber ,
            GlobalVariable.iNeiKongPictureNumber ,
            GlobalVariable.iDuanMianBPictureNumber,
            GlobalVariable.iWaiYuanBPictureNumber,
            GlobalVariable.iDaoJiaoBPictureNumber
        };
        public static bool[] m_ParamUpdate =
        {
            GlobalVariable.bDuanMianAParamUpdate,
            GlobalVariable.bWaiYuanAParamUpdate,
            GlobalVariable.bDaoJiaoAParamUpdate,
            GlobalVariable.bNeiKongParamUpdate,
            GlobalVariable.bDuanMianBParamUpdate,
            GlobalVariable.bWaiYuanBParamUpdate,
            GlobalVariable.bDaoJiaoBParamUpdate,
            GlobalVariable.bNeiKongBParamUpdate
        };
        public static bool[] m_RunManual =
        {
            GlobalVariable.bDuanMianARunManual,
            GlobalVariable.bWaiYuanARunManual,
            GlobalVariable.bDaoJiaoARunManual,
            GlobalVariable.bNeiKongRunManual,
            GlobalVariable.bDuanMianBRunManual,
            GlobalVariable.bWaiYuanBRunManual,
            GlobalVariable.bDaoJiaoBRunManual,
            GlobalVariable.bNeiKongBRunManual
        };
        public static IAreaScanCameraHelper[] m_cameraHelper =
        {
            GlobalVariable.DuanMianACameraHelper,
            GlobalVariable.WaiYuanACameraHelper,
            GlobalVariable.DaoJiaoACameraHelper,
            GlobalVariable.NeiKongCameraHelper,
            GlobalVariable.DuanMianBCameraHelper,
            GlobalVariable.WaiYuanBCameraHelper,
            GlobalVariable.DaoJiaoBCameraHelper
        };
        public static CameraPhotoSetting[] m_photoSetting =
        {
            GlobalVariable.configSetting.duanMianACameraPhotoSetting,
            GlobalVariable.configSetting.waiYuanACameraPhotoSetting,
            GlobalVariable.configSetting.daoJiaoACameraPhotoSetting,
            GlobalVariable.configSetting.neiKongCameraPhotoSetting,
            GlobalVariable.configSetting.duanMianBCameraPhotoSetting,
            GlobalVariable.configSetting.waiYuanBCameraPhotoSetting,
            GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting
        };
        public static int[] m_TotalDataCollect =
        {
            GlobalVariable.totalDataCollect.DuanMianANGNumber,
            GlobalVariable.totalDataCollect.WaiYuanANGNumber,
            GlobalVariable.totalDataCollect.DaoJiaoANGNumber,
            GlobalVariable.totalDataCollect.NeiKongNGNumber,
            GlobalVariable.totalDataCollect.DuanMianBNGNumber,
            GlobalVariable.totalDataCollect.WaiYuanBNGNumber,
            GlobalVariable.totalDataCollect.DaoJiaoBNGNumber,
        };
        public static int[] m_totalBears =
        {
            GlobalVariable.totalBears.DuanMianACount,
            GlobalVariable.totalBears.WaiYuanACount,
            GlobalVariable.totalBears.DaoJiaoACount,
            GlobalVariable.totalBears.NeiKongCount,
            GlobalVariable.totalBears.DuanMianBCount,
            GlobalVariable.totalBears.WaiYuanBCount,
            GlobalVariable.totalBears.DaoJiaoBCount,
        };
        #endregion
        public static bool Init()
        {
            DuanMianAProcess = new surfaceImageProcess();
            WaiYuanAProcess = new outerSideImageProcess();
            DaoJiaoAProcess = new multiImgProcess();
            NeiKongProcess = new multiImgProcess();
            DuanMianBProcess = new surfaceImageProcess();
            WaiYuanBProcess = new multiImgProcess();
            DaoJiaoBProcess = new multiImgProcess();
            DuanMianAProcess.setSide(true);
            DuanMianBProcess.setSide(false);

            //硬件实例化
            DuanMianACameraHelper = new HikCameraHelper();
            WaiYuanACameraHelper = new HikCameraHelper();
            DaoJiaoACameraHelper = new HikCameraHelper();
            NeiKongCameraHelper = new HikCameraHelper();
            DuanMianBCameraHelper = new HikCameraHelper();
            WaiYuanBCameraHelper = new HikCameraHelper();
            DaoJiaoBCameraHelper = new HikCameraHelper();

            ioBoardHelper = new IOBoardHelper();

            lightHelper = new LightHelper[2]; //目前上位机能控制的光源只有2个， 每个12路
            lightHelper[0] = new LightHelper();
            lightHelper[1] = new LightHelper();

            hardwareSetting = new HardwareSetting();
            machineSetting = new MachineSetting();
            generalSetting = new GeneralSetting();
            generalSetting.bSaveNG = new bool[iStationNumber];
           iWorkStation = IniHelper.IniReadInt("Password", "iWorkStation", 0, FilePath.UserPasswordPath);
            //线程相关
            if (iWorkStation == 2)
            {
                duanMianAAutoFlow = new DuanMianAAutoFlow();
                duanMianBAutoFlow = new DuanMianBAutoFlow();
            }
            else if(iWorkStation == 5)
            {
                duanMianAAutoFlow = new DuanMianAAutoFlow();
                waiYuanAAutoFlow = new WaiYuanAAutoFlow();
                neiKongAutoFlow = new NeiKongAutoFlow();
                duanMianBAutoFlow = new DuanMianBAutoFlow();
                waiYuanBAutoFlow = new WaiYuanBAutoFlow();
            }
            else
            {
                duanMianAAutoFlow = new DuanMianAAutoFlow();
                waiYuanAAutoFlow = new WaiYuanAAutoFlow();
                daoJiaoAAutoFlow = new DaoJiaoAAutoFlow();
                //daoJiaoAAutoFlow = new MultiImageAutoFlow(2);
                neiKongAutoFlow = new NeiKongAutoFlow();
                duanMianBAutoFlow = new DuanMianBAutoFlow();
                waiYuanBAutoFlow = new WaiYuanBAutoFlow();
                daoJiaoBAutoFlow = new DaoJiaoBAutoFlow();
            }

            bForbidden = new bool[iStationNumber];
            for (int i = 0; i < bForbidden.Length; i++)
            {
                bForbidden[i] = false;
            }
            configname = IniHelper.IniReadString("Password", "CurrentConfig", "", FilePath.UserPasswordPath);
            totalDataCollect = new TotalDataCollect();
            totalDataCollect.DuanMianANGNumber = IniHelper.IniReadInt("DataStatistic", "DuanMianANG", 0, FilePath.UserPasswordPath);
            totalDataCollect.WaiYuanANGNumber = IniHelper.IniReadInt("DataStatistic", "WaiYuanANG", 0, FilePath.UserPasswordPath);
            totalDataCollect.DaoJiaoANGNumber = IniHelper.IniReadInt("DataStatistic", "DaoJiaoANG", 0, FilePath.UserPasswordPath);
            totalDataCollect.NeiKongNGNumber = IniHelper.IniReadInt("DataStatistic", "NeiKongNG", 0, FilePath.UserPasswordPath);
            totalDataCollect.DuanMianBNGNumber = IniHelper.IniReadInt("DataStatistic", "DuanMianBNG", 0, FilePath.UserPasswordPath);
            totalDataCollect.WaiYuanBNGNumber = IniHelper.IniReadInt("DataStatistic", "WaiYuanBNG", 0, FilePath.UserPasswordPath);
            totalDataCollect.DaoJiaoBNGNumber = IniHelper.IniReadInt("DataStatistic", "DaoJiaoBNG", 0, FilePath.UserPasswordPath);

            totalBears = new TotalBears();
            totalBears.DuanMianACount = IniHelper.IniReadInt("DataStatistic", "DuanMianACOUNT", 0, FilePath.UserPasswordPath);
            totalBears.WaiYuanACount = IniHelper.IniReadInt("DataStatistic", "WaiYuanACOUNT", 0, FilePath.UserPasswordPath);
            totalBears.DaoJiaoACount = IniHelper.IniReadInt("DataStatistic", "DaoJiaoACOUNT", 0, FilePath.UserPasswordPath);
            totalBears.NeiKongCount = IniHelper.IniReadInt("DataStatistic", "NeiKongCOUNT", 0, FilePath.UserPasswordPath);
            totalBears.DuanMianBCount = IniHelper.IniReadInt("DataStatistic", "DuanMianBCOUNT", 0, FilePath.UserPasswordPath);
            totalBears.WaiYuanBCount = IniHelper.IniReadInt("DataStatistic", "WaiYuanBCOUNT", 0, FilePath.UserPasswordPath);
            totalBears.DaoJiaoBCount = IniHelper.IniReadInt("DataStatistic", "DaoJiaoBCOUNT", 0, FilePath.UserPasswordPath);
            OneBearInfor = new List<int[]>();

            ctCalculate = new CT[4];
            for (int i = 0; i < ctCalculate.Length; i++)
            {
                ctCalculate[i] = new CT();
            }

            bParamFreshForManual = new bool[iStationNumber];
            for (int i = 0; i < iStationNumber; i++)
            {
                bParamFreshForManual[i] = true;
            }

            if (!File.Exists(FilePath.ErrorCodePath))
            {
                return false;
            }

            string[] content = File.ReadAllLines(FilePath.ErrorCodePath);
            for (int i = 0; i < content.Length; i++)
            {
                if (string.IsNullOrEmpty(content[i]))
                {
                    continue;
                }
                string[] split = content[i].Split('\t');
                int index = Convert.ToInt32(split[0]);
                _dicErrorInfo.Add(index, split[1]);
            }

            return true;
        }

        public static void resetParam()
        {
            bDuanMianAParamUpdate = true;
            bWaiYuanAParamUpdate = true;
            bDaoJiaoAParamUpdate = true;
            bNeiKongParamUpdate = true;
            bDuanMianBParamUpdate = true;
            bWaiYuanBParamUpdate = true;
            bDaoJiaoBParamUpdate = true;
        }
    }
}