using GxIAPINET;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace YiRongMachine
{
    public class DaHengCameraHelper : IAreaScanCameraHelper
    {
        [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

        private int index;
        private IGXFactory m_objIGXFactory = null;
        private IGXDevice m_objIGXDevice = null;
        private IGXStream m_objIGXStream = null;
        private IGXFeatureControl m_objIGXFeatureControl = null;
        private IGXFeatureControl m_objIGXStreamFeatureControl = null;

        public DaHengCameraHelper(int CameraNumber)
        {
            index = CameraNumber;
        }

        private void __InitDevice()
        {
            if (null != m_objIGXFeatureControl)
            {
                //设置采集模式连续采集
                m_objIGXFeatureControl.GetEnumFeature("AcquisitionMode").SetValue("Continuous");

                //设置触发模式为开
                m_objIGXFeatureControl.GetEnumFeature("TriggerMode").SetValue("On");

                //选择触发源为软触发
                m_objIGXFeatureControl.GetEnumFeature("TriggerSource").SetValue("Software");
            }
        }

        /// <summary>
        /// 关闭流
        /// </summary>
        private void __CloseStream()
        {
            try
            {
                //关闭流
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.Close();
                    m_objIGXStream = null;
                    m_objIGXStreamFeatureControl = null;
                }
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 关闭设备
        /// </summary>
        private void __CloseDevice()
        {
            try
            {
                //关闭设备
                if (null != m_objIGXDevice)
                {
                    m_objIGXDevice.Close();
                    m_objIGXDevice = null;
                }
            }
            catch (Exception)
            {
            }
        }

        private void __CloseAll()
        {
            try
            {
            }
            catch (Exception)
            {
            }
            try
            {
                //停止流通道和关闭流
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.StopGrab();
                    m_objIGXStream.Close();
                    m_objIGXStream = null;
                    m_objIGXStreamFeatureControl = null;
                }
            }
            catch (Exception)
            {
            }

            //关闭设备
            __CloseDevice();
        }

        public bool CameraOpen(string ip)
        {
            List<IGXDeviceInfo> listGXDeviceInfo = new List<IGXDeviceInfo>();

            __CloseStream();
            __CloseDevice();

            m_objIGXFactory = IGXFactory.GetInstance();
            m_objIGXFactory.Init();
            m_objIGXFactory.UpdateDeviceList(200, listGXDeviceInfo);

            if (listGXDeviceInfo.Count <= 0)
            {
                return false;
            }
            for (int i = 0; i < listGXDeviceInfo.Count; i++)
            {
                string s1 = listGXDeviceInfo[i].GetIP();
                string s2 = listGXDeviceInfo[i].GetSN();
                if (s2 == ip)
                {
                    try
                    {
                        m_objIGXDevice = m_objIGXFactory.OpenDeviceBySN(listGXDeviceInfo[i].GetSN(), GX_ACCESS_MODE.GX_ACCESS_EXCLUSIVE);
                        m_objIGXFeatureControl = m_objIGXDevice.GetRemoteFeatureControl();
                        if (null != m_objIGXDevice)
                        {
                            m_objIGXStream = m_objIGXDevice.OpenStream((uint)i);
                            m_objIGXStreamFeatureControl = m_objIGXStream.GetFeatureControl();
                        }
                        GX_DEVICE_CLASS_LIST objDeviceClass = m_objIGXDevice.GetDeviceInfo().GetDeviceClass();
                        if (GX_DEVICE_CLASS_LIST.GX_DEVICE_CLASS_GEV == objDeviceClass)
                        {
                            // 判断设备是否支持流通道数据包功能
                            if (true == m_objIGXFeatureControl.IsImplemented("GevSCPSPacketSize"))
                            {
                                // 获取当前网络环境的最优包长值
                                uint nPacketSize = m_objIGXStream.GetOptimalPacketSize();
                                // 将最优包长值设置为当前设备的流通道包长值
                                m_objIGXFeatureControl.GetIntFeature("GevSCPSPacketSize").SetValue(nPacketSize);
                            }
                        }
                        __InitDevice();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        public void CameraClose()
        {
            try
            {
                __CloseAll();
            }
            catch
            {
            }
        }

        public bool SetCameraROI(int w, int h, int x, int y, bool bOffsetY = true)
        {
            try
            {
                m_objIGXFeatureControl.GetIntFeature("Width").SetValue(w);
                m_objIGXFeatureControl.GetIntFeature("Height").SetValue(h);
                m_objIGXFeatureControl.GetIntFeature("OffsetX").SetValue(x);
                m_objIGXFeatureControl.GetIntFeature("OffsetY").SetValue(y);
                //myCamera.Parameters[PLCamera.Width].TrySetValue(w);
                //myCamera.Parameters[PLCamera.Height].TrySetValue(h);
                //myCamera.Parameters[PLCamera.OffsetX].TrySetValue(x);
                //myCamera.Parameters[PLCamera.OffsetY].TrySetValue(y);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool SetXSSpeed(int spd)
        {
            return true;
        }

        public void Start()
        {
            try
            {
                if (null != m_objIGXStreamFeatureControl)
                {
                    try
                    {
                        //设置流层Buffer处理模式为OldestFirst
                        m_objIGXStreamFeatureControl.GetEnumFeature("StreamBufferHandlingMode").SetValue("OldestFirst");
                    }
                    catch (Exception)
                    {
                    }
                }
                //开启采集流通道
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.StartGrab();
                }
                //发送开采命令
                if (null != m_objIGXFeatureControl)
                {
                    m_objIGXFeatureControl.GetCommandFeature("AcquisitionStart").Execute();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void Stop()
        {
            try
            {
                //发送停采命令
                if (null != m_objIGXFeatureControl)
                {
                    m_objIGXFeatureControl.GetCommandFeature("AcquisitionStop").Execute();
                }
                //关闭采集流通道
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.StopGrab();
                }
            }
            catch (Exception ex)
            {
            }
        }

        public bool TakeCameraImage(ref Bitmap bitmap, ref string ErrorMsg)//读取相机buffer并生成HImage格式的图像
        {
            IImageData objIImageData = null;
            try
            {
                if (null != m_objIGXStream)
                {
                    m_objIGXStream.FlushQueue();
                }
                if (null != m_objIGXFeatureControl)
                {
                    m_objIGXFeatureControl.GetCommandFeature("TriggerSoftware").Execute();
                }
                if (null != m_objIGXStream)
                {
                    objIImageData = m_objIGXStream.GetImage(1000);
                    bitmap = new Bitmap((int)objIImageData.GetWidth(), (int)objIImageData.GetHeight(), PixelFormat.Format8bppIndexed);
                    //格式转换
                    BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                    IntPtr ptrBmp = bmpData.Scan0;
                    memcpy(ptrBmp, objIImageData.GetBuffer(), (int)objIImageData.GetWidth() * (int)objIImageData.GetHeight());
                    //Marshal.Copy(objIImageData.GetBuffer(), 0, ptrBmp, (int)objIImageData.GetWidth() * (int)objIImageData.GetHeight());
                    bitmap.UnlockBits(bmpData);
                    ColorPalette cp = bitmap.Palette;
                    for (int i = 0; i < 256; i++)
                    {
                        cp.Entries[i] = Color.FromArgb(255, i, i, i);
                    }
                    bitmap.Palette = cp;
                    objIImageData.Destroy();
                    return true;
                }
                else
                {
                    m_objIGXStream.StopGrab();
                    m_objIGXStream.StartGrab();
                    return false;
                }

                //Stopwatch st = new Stopwatch();
                //st.Restart();
                //if (myCamera.WaitForFrameTriggerReady(50, TimeoutHandling.ThrowException))
                //{
                //    myCamera.ExecuteSoftwareTrigger();
                //}
                //读取buffer，超时时间为4000ms
                //IGrabResult grabResult = myCamera.StreamGrabber.RetrieveResult(1000, TimeoutHandling.ThrowException);
                //if (grabResult.GrabSucceeded)
                //{
                //    //st.Stop();
                //    //LogHelper.AddSideInLog("图片获得用时" + st.ElapsedMilliseconds);
                //    //st.Restart();

                //    //图片转换这用时1ms
                //    bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format8bppIndexed);
                //    //格式转换
                //    BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                //    converter.OutputPixelFormat = PixelType.Mono8;
                //    IntPtr ptrBmp = bmpData.Scan0;
                //    converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult);
                //    bitmap.UnlockBits(bmpData);
                //    ColorPalette cp = bitmap.Palette;
                //    for (int i = 0; i < 256; i++)
                //    {
                //        cp.Entries[i] = Color.FromArgb(255, i, i, i);
                //    }
                //    bitmap.Palette = cp;
                //    //st.Stop();
                //    //LogHelper.AddSideInLog("图片获aaa得用时" + st.ElapsedMilliseconds);
                //   return true;
                //}
                //else
                //{
                //    myCamera.StreamGrabber.Stop();
                //    myCamera.StreamGrabber.Start();
                //    return false;
                //}
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
                m_objIGXFeatureControl.GetFloatFeature("ExposureTime").SetValue(exposure);
                //myCamera.Parameters[PLCamera.ExposureTimeAbs].SetValue(exposure);
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
            return true;
        }
    }
}