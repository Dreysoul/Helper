using Basler.Pylon;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace YiRongMachine
{
    public class BaslerCameraHelper : IAreaScanCameraHelper
    {
        private Camera myCamera = null;  //创建相机对象
        private int index;

        public BaslerCameraHelper(int CameraNumber)
        {
            index = CameraNumber;
        }

        public bool CameraOpen(string ip)
        {
            converter = new PixelDataConverter();
            List<ICameraInfo> allCameras = CameraFinder.Enumerate();//获取所有相机设备
            for (int i = 0; i < allCameras.Count; i++)
            {
                string s1 = allCameras[i][CameraInfoKey.IpConfigOptions];
                string s2 = allCameras[i][CameraInfoKey.SerialNumber];
                if (s2 == ip)
                {
                    try
                    {
                        myCamera = new Camera(allCameras[i]);
                        myCamera.Close();
                        myCamera.Open();//打开相机
                        myCamera.Parameters[PLCamera.PixelFormat].TrySetValue(PLCamera.PixelFormat.Mono8);
                        //myCamera.Parameters[PLCamera.TestImageSelector].TrySetValue(PLCamera.TestImageSelector.Off);
                        //myCamera.Parameters[PLCamera.BinningHorizontal].TrySetValue(1);
                        //myCamera.Parameters[PLCamera.BinningVertical].TrySetValue(1);
                        myCamera.Parameters[PLCamera.TriggerSource].TrySetValue(PLCamera.TriggerSource.Software);
                        myCamera.Parameters[PLCamera.TriggerMode].TrySetValue(PLCamera.TriggerMode.On);
                        myCamera.Parameters[PLCamera.GevSCPSPacketSize].TrySetValue(1500);
                        myCamera.Parameters[PLCamera.GevHeartbeatTimeout].SetValue(1000);

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
                myCamera.Close();
            }
            catch
            {
            }
        }

        public bool SetCameraROI(int w, int h, int x, int y, bool bOffsetY)
        {
            try
            {
                myCamera.Parameters[PLCamera.Width].TrySetValue(w);
                myCamera.Parameters[PLCamera.Height].TrySetValue(h);
                myCamera.Parameters[PLCamera.OffsetX].TrySetValue(x);
                myCamera.Parameters[PLCamera.OffsetY].TrySetValue(y);
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
            myCamera.StreamGrabber.Start();
        }

        public void Stop()
        {
            myCamera.StreamGrabber.Stop();
        }

        private PixelDataConverter converter;

        public bool TakeCameraImage(ref Bitmap bitmap, ref string ErrorMsg)//读取相机buffer并生成HImage格式的图像
        {
            try
            {
                //Stopwatch st = new Stopwatch();
                //st.Restart();
                if (myCamera.WaitForFrameTriggerReady(50, TimeoutHandling.ThrowException))
                {
                    myCamera.ExecuteSoftwareTrigger();
                }
                //读取buffer，超时时间为4000ms
                IGrabResult grabResult = myCamera.StreamGrabber.RetrieveResult(4000, TimeoutHandling.ThrowException);
                if (grabResult.GrabSucceeded)
                {
                    //st.Stop();
                    //LogHelper.AddSideInLog("图片获得用时" + st.ElapsedMilliseconds);
                    //st.Restart();

                    //图片转换这用时1ms
                    bitmap = new Bitmap(grabResult.Width, grabResult.Height, PixelFormat.Format8bppIndexed);
                    //格式转换
                    BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, bitmap.PixelFormat);
                    converter.OutputPixelFormat = PixelType.Mono8;
                    IntPtr ptrBmp = bmpData.Scan0;
                    converter.Convert(ptrBmp, bmpData.Stride * bitmap.Height, grabResult);
                    bitmap.UnlockBits(bmpData);
                    ColorPalette cp = bitmap.Palette;
                    for (int i = 0; i < 256; i++)
                    {
                        cp.Entries[i] = Color.FromArgb(255, i, i, i);
                    }
                    bitmap.Palette = cp;
                    //st.Stop();
                    //LogHelper.AddSideInLog("图片获aaa得用时" + st.ElapsedMilliseconds);
                    return true;
                }
                else
                {
                    myCamera.StreamGrabber.Stop();
                    myCamera.StreamGrabber.Start();
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
                myCamera.Parameters[PLCamera.ExposureTimeAbs].SetValue(exposure);
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