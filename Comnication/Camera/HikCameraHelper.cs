using MvCameraControl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;

namespace YiRongMachine
{
    public class HikCameraHelper : IAreaScanCameraHelper
    {
        private Stopwatch st = new Stopwatch();
        private IDevice m_MyCamera = null;

        private const DeviceTLayerType devLayerType = DeviceTLayerType.MvGigEDevice | DeviceTLayerType.MvUsbDevice | DeviceTLayerType.MvGenTLCameraLinkDevice
            | DeviceTLayerType.MvGenTLCXPDevice | DeviceTLayerType.MvGenTLXoFDevice;

        public HikCameraHelper()
        {
        }

        public bool CameraOpen(string chSerialNumber)
        {
            List<IDeviceInfo> devInfoList;

            // ch:枚举设备 | en:Enum device
            int ret = DeviceEnumerator.EnumDevices(devLayerType, out devInfoList);
            if (ret != MvError.MV_OK)
            {
                Console.WriteLine("Enum device failed:{0:x8}", ret);
                return false;
            }

            Console.WriteLine("Enum device count : {0}", devInfoList.Count);

            if (0 == devInfoList.Count)
            {
                return false;
            }

            // ch:打印设备信息 en:Print device info
            for (int i = 0; i < devInfoList.Count(); i++)
            {
                var devInfo = devInfoList[i];
                if (devInfo.SerialNumber == chSerialNumber)
                {
                    m_MyCamera = DeviceFactory.CreateDevice(devInfoList[i]);
                    break;
                }
            }
            if (m_MyCamera == null)
                return false;

            int result = m_MyCamera.Open();
            if (result != MvError.MV_OK)
            {
                return false;
            }

            //ch: 判断是否为gige设备 | en: Determine whether it is a GigE device
            if (m_MyCamera is IGigEDevice)
            {
                //ch: 转换为gigE设备 | en: Convert to Gige device
                IGigEDevice gigEDevice = m_MyCamera as IGigEDevice;

                // ch:探测网络最佳包大小(只对GigE相机有效) | en:Detection network optimal package size(It only works for the GigE camera)
                int optionPacketSize;
                result = gigEDevice.GetOptimalPacketSize(out optionPacketSize);
                if (result != MvError.MV_OK)
                {
                    //return false;
                }
                else
                {
                    result = m_MyCamera.Parameters.SetIntValue("GevSCPSPacketSize", (long)optionPacketSize);
                    if (result != MvError.MV_OK)
                    {
                        //return false;
                    }
                }
            }

            m_MyCamera.Parameters.SetEnumValueByString("TriggerMode", "On");
            m_MyCamera.Parameters.SetEnumValueByString("TriggerSource", "Software");

            //m_MyCamera.Parameters.SetEnumValueByString("AcquisitionMode", "Continuous");
            //m_MyCamera.Parameters.SetEnumValueByString("TriggerMode", "Off");
            return true;
        }

        public void CameraClose()
        {
            try
            {
                m_MyCamera.Close();
                m_MyCamera.Dispose();
                m_MyCamera = null;
            }
            catch
            {
            }
        }

        public bool SetCameraROI(int w, int h, int x, int y, bool bOffsetY = true)
        {
            int nRet = MvError.MV_OK;

            try
            {
                w = w - w % 8;
                h = h - h % 2;
                x = x - x % 2;
                y = y - y % 2;
                nRet = m_MyCamera.Parameters.SetIntValue("Width", w);
                if (nRet != MvError.MV_OK)
                {
                    return false;
                }
                nRet = m_MyCamera.Parameters.SetIntValue("Height", h);
                if (nRet != MvError.MV_OK)
                {
                    return false;
                }
                nRet = m_MyCamera.Parameters.SetIntValue("OffsetX", x);
                if (nRet != MvError.MV_OK)
                {
                    return false;
                }
                if (bOffsetY)
                {
                    nRet = m_MyCamera.Parameters.SetIntValue("OffsetY", y);
                    if (nRet != MvError.MV_OK)
                    {
                        return false;
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SetXSSpeed(int spd)
        {
            int nRet = MvError.MV_OK;
            nRet = m_MyCamera.Parameters.SetIntValue("AcquisitionLineRate", spd);
            if (nRet != MvError.MV_OK)
            {
                return false;
            }
            nRet = m_MyCamera.Parameters.SetBoolValue("AcquisitionLineRateEnable", true);
            if (nRet != MvError.MV_OK)
            {
                return false;
            }
            return true;
        }

        public void Start()
        {
            int nRet = m_MyCamera.StreamGrabber.StartGrabbing();
        }

        public void Stop()
        {
            int nRet = m_MyCamera.StreamGrabber.StopGrabbing();
        }

        public bool TakeCameraImage(ref Bitmap bitmap, ref string ErrorMsg)//读取相机buffer并生成HImage格式的图像
        {
            int nRet = MvError.MV_OK;
            try
            {
                IFrameOut pcFrameInfo = null;

                nRet = m_MyCamera.Parameters.SetCommandValue("TriggerSoftware");
                //st.Reset();
                //st.Start();
                nRet = m_MyCamera.StreamGrabber.GetImageBuffer(1000, out pcFrameInfo);
                //st.Stop();
                //LogHelper.AddDaoJiaoALog("p: "  + st.ElapsedMilliseconds.ToString());
                if (nRet == MvError.MV_OK)
                {
                    bitmap = pcFrameInfo.Image.ToBitmap();
                    m_MyCamera.StreamGrabber.FreeImageBuffer(pcFrameInfo);
                    return true;
                }
                else
                {
                    m_MyCamera.StreamGrabber.StopGrabbing();
                    m_MyCamera.StreamGrabber.StartGrabbing();
                    return false;
                }
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
                return false;
            }
        }

        public bool SetCameraExposureTime(double exposure, ref string ErrorMsg)//设置曝光时间us
        {
            try
            {
                m_MyCamera.Parameters.SetEnumValue("ExposureAuto", 0);
                int nRet = m_MyCamera.Parameters.SetFloatValue("ExposureTime", (float)exposure);
                //myCamera.Parameters[PLCamera.ExposureTimeAbs].SetValue(exposure);
                if (nRet != MvError.MV_OK)
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
                return false;
            }
        }
        public bool SetCameraGain(double Gain, ref string ErrorMsg)
        {
            try
            {
                int nRet = m_MyCamera.Parameters.SetBoolValue("DigitalShiftEnable", true);
                nRet = m_MyCamera.Parameters.SetFloatValue("DigitalShift", (float)Gain);
                if (nRet != MvError.MV_OK)
                {
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
                return false;
            }
        }
    }
}