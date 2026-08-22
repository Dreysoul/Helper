using BearingInspection;
using System.Drawing;
using System.Windows.Forms;

namespace YiRongMachine
{
    /// <summary>
    /// 所有文件路径
    /// </summary>
    public struct FilePath
    {
        public static string UserPasswordPath = Application.StartupPath + "\\System\\UserPassword.cfg";
        public static string PLCSettingPath = Application.StartupPath + "\\System\\PLCSetting.Json";
        public static string VendorSettingPath = Application.StartupPath + "\\System\\VendorSetting.Json";
        public static string GeneralSettingPath = Application.StartupPath + "\\System\\GeneralSetting.Json";
        public static string HardwareSettingPath = Application.StartupPath + "\\System\\HardwareSetting.Json";
        public static string ErrorCodePath = Application.StartupPath + "\\System\\ErrorCode.txt";
        public static string ParamSettingPath = Application.StartupPath + "\\Param\\";
        public static string AlarmLogPath = GlobalVariable.path + "Log\\Alarm\\";
        public static string ResInfoPath = Application.StartupPath + "\\Tmp\\res-info\\";
        //public static string SideAProductInfoPath = GlobalVariable.path + "Log\\SideA\\";
        //public static string SideBProductInfoPath = GlobalVariable.path + "Log\\SideB\\";
        //public static string SideInProductInfoPath = GlobalVariable.path + "Log\\SideIn\\";
        //public static string SideOutProductInfoPath = GlobalVariable.path + "Log\\SideOut\\";

        public static string PLCAddressPath = Application.StartupPath + "\\System\\PLCAddress.csv";

        public static string duanMianAStatisticPath = Application.StartupPath + "\\System\\duanMianAStatistic.Json";
        public static string waiYuanAStatisticPath = Application.StartupPath + "\\System\\waiYuanAStatistic.Json";
        public static string daoJiaoAStatisticPath = Application.StartupPath + "\\System\\daoJiaoAStatistic.Json";
        public static string neiKongStatisticPath = Application.StartupPath + "\\System\\neiKongStatistic.Json";
        public static string duanMianBStatisticPath = Application.StartupPath + "\\System\\duanMianBStatistic.Json";
        public static string waiYuanBStatisticPath = Application.StartupPath + "\\System\\waiYuanBStatistic.Json";
        public static string daoJiaoBStatisticPath = Application.StartupPath + "\\System\\daoJiaoBStatistic.Json";
    }

    public struct ConfigSetting
    {
        public Surface_Param_Info duanMianAParam;      //端面A检测参数
        public OuterSide_Param_Info waiYuanAParam;    //外圈检测参数
        public MultiImg_Param_Info daoJiaoAParam;
        public MultiImg_Param_Info neiKongParam;
        public Surface_Param_Info duanMianBParam;      //端面B检测参数
        public MultiImg_Param_Info waiYuanBParam;
        public MultiImg_Param_Info daoJiaoBParam;

        public surface_Flag duanMianAFlag;
        public outerSide_Flag waiYuanAFlag;
        public MultiImg_Flag daoJiaoAFlag;
        public MultiImg_Flag neiKongFlag;
        public surface_Flag duanMianBFlag;
        public MultiImg_Flag waiYuanBFlag;
        public MultiImg_Flag daoJiaoBFlag;

        public CameraPhotoSetting duanMianACameraPhotoSetting;
        public CameraPhotoSetting waiYuanACameraPhotoSetting;
        public CameraPhotoSetting daoJiaoACameraPhotoSetting;
        public CameraPhotoSetting neiKongCameraPhotoSetting;
        public CameraPhotoSetting duanMianBCameraPhotoSetting;
        public CameraPhotoSetting waiYuanBCameraPhotoSetting;
        public CameraPhotoSetting daoJiaoBCameraPhotoSetting;

        public Surface_Model_Info duanMianAModel;     //A面生成模板后的参数
        public OuterSide_Model_Info waiYuanAModel;  //外圈生成模板后的参数
        public MultiImg_Model_Info daoJiaoAModel;
        public MultiImg_Model_Info neiKongModel;
        public Surface_Model_Info duanMianBModel;     //B面生成模板后的参数
        public MultiImg_Model_Info waiYuanBModel;
        public MultiImg_Model_Info daoJiaoBModel;

        public Outer_Extra SideOutExtra;
    }

    /// <summary>
    /// 端面AB拍摄出的图片的参数
    /// </summary>
    public struct CameraPhotoSetting
    {
        public short ROIWidthMax;          //图片最大宽度 -- 设置用于分配内存 不得小于相机的最大值
        public short ROIHeightMax;          //图片最大高度 -- 设置用于分配内存 不得小于相机的最大值
        public short ROIWidthTrue;         //设置关注的图片宽度
        public short ROIHeihgtTrue;         //设置关注的图片高度
        public short OffsetX;               //关注区域X偏移
        public short OffsetY;               //关注区域Y偏移
        public short photoNumber;           //照片累计需要拍摄数量
        public short photoSpan;             //照片拍摄的时间间隔
        public short xsSpeed;             //线扫的速度
        //总共拍摄5张图片
        public double[] exposure;     //曝光时间0-4
        public string[] lightsCmd;   //光源控制码0-4
        public double[] gain; 
    }

    public struct HardwareSetting
    {
        public string DuanMianACameraSn;
        public string WaiYuanACameraSn;
        public string DaoJiaoACameraSn;
        public string NeiKongCameraSn;
        public string DuanMianBCameraSn;
        public string WaiYuanBCameraSn;
        public string DaoJiaoBCameraSn;

        public string light1Com;
        public string light2Com;
    }

    public struct TotalDataCollect
    {
        public int DuanMianANGNumber;
        public int WaiYuanANGNumber;
        public int DaoJiaoANGNumber;
        public int NeiKongNGNumber;
        public int DuanMianBNGNumber;
        public int WaiYuanBNGNumber;
        public int DaoJiaoBNGNumber;
    }

    public struct TotalBears
    {
        public int DuanMianACount;
        public int WaiYuanACount;
        public int DaoJiaoACount;
        public int NeiKongCount;
        public int DuanMianBCount;
        public int WaiYuanBCount;
        public int DaoJiaoBCount;
    }

    public struct CT
    {
        public double TakePicture;
        public double Calculate;
        public double WriteResult;
    }

    public struct MachineSetting
    {
        public int MachineType;
    }

    public struct GeneralSetting
    {
        public bool bUseLog;
        public int sleepTime;
        public bool[] bSaveNG;
    }

    public class BitmapContent
    {
        public int index;
        public Bitmap bitmap;
    }
    
    }