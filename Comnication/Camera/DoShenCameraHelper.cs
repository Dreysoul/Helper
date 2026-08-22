using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DALSA.SaperaLT.SapClassBasic;
using DVPCameraType;
using System.Threading;
using System.Drawing.Imaging;

namespace YiRongMachine
{
    public class DoShenCameraHelper:ILineScanCameraHelper
    {
        private SapLocation m_ServerLocation; // 设备的连接地址
        private SapAcqDevice m_AcqDevice; //采集设备
        private SapBuffer m_Buffers; // 缓存对象
        private SapAcqDeviceToBuf m_Xfer; // 传输对象
        public bool _IsReceived = false;
        public Bitmap[] _bitmap;
        private int frameIndex = 0;  //帧数量
        int pictureIndex = 0;


        uint m_handle = 0;

        public DoShenCameraHelper()
        {
            //m_ServerLocation = null;   // 设备的连接地址
            //m_AcqDevice = null;     // 采集设备
            //m_Buffers = null;             // 缓存对象
            //m_Xfer = null;       // 传输对象
            ////_bitmap = null;
        }

        public bool CameraOpen(string name,ref string ErrorMsg)
        {
            dvpStatus status;
            //uint i;
            uint n = 0;
            dvpCameraInfo dev_info = new dvpCameraInfo();

            status = DVPCamera.dvpRefresh(ref n);
            if (status == dvpStatus.DVP_STATUS_OK)
            {
                status = DVPCamera.dvpEnum(0, ref dev_info);
                if (status == dvpStatus.DVP_STATUS_OK)
                {

                }
            }



            string CameraName;
            int Index;
            bool bRtn = GetCameraInfo(name ,out CameraName, out Index);
            if (!bRtn)
            {
                ErrorMsg="未能够获得dalsa相机列表";
                return false;
            }


            m_ServerLocation = new SapLocation(CameraName, 0);


            // 创建采集设备，new SapAcqDevice()的括号中第二个参数既可以写配置文件路径，也可以写false,猜测false是用相机当前的设置
            m_AcqDevice = new SapAcqDevice(m_ServerLocation,Application.StartupPath+ "\\System\\1.ccf");
            //m_AcqDevice = new SapAcqDevice(m_ServerLocation);
            bRtn = m_AcqDevice.Create();
            if (!bRtn )
            {
                CameraClose();
                ErrorMsg = "创建相机对象失败";
                return false;
            }

            bRtn = GlobalVariable.LineScanCameraHelper.SetCameraImage(GlobalVariable.configSetting.SideOutCameraPhotoSetting.ROIHeihgtTrue, GlobalVariable.configSetting.SideOutCameraPhotoSetting.ROIWidthTrue, GlobalVariable.configSetting.SideOutCameraPhotoSetting.OffsetX, GlobalVariable.configSetting.SideOutCameraPhotoSetting.OffsetY);
            if (!bRtn)
            {
                MessageBox.Show("设置外圈相机ROI错误");
            }

            // 创建缓存对象
            bRtn = SapBuffer.IsBufferTypeSupported(m_ServerLocation, SapBuffer.MemoryType.ScatterGather);
            if (bRtn)
            {
                m_Buffers = new SapBufferWithTrash(2, m_AcqDevice, SapBuffer.MemoryType.ScatterGather);
            }
            else
            {
                m_Buffers = new SapBufferWithTrash(2, m_AcqDevice, SapBuffer.MemoryType.ScatterGatherPhysical);
            }
            bRtn = m_Buffers.Create();
            if (!bRtn )
            {
                CameraClose();
                return false;
            }

            // 创建传输对象
            m_Xfer = new SapAcqDeviceToBuf(m_AcqDevice, m_Buffers);
             m_Xfer.XferNotify += new SapXferNotifyHandler(m_Xfer_XferNotify);
            m_Xfer.XferNotifyContext = this;
            m_Xfer.Pairs[0].EventType = SapXferPair.XferEventType.EndOfFrame;
            m_Xfer.Pairs[0].Cycle = SapXferPair.CycleMode.NextWithTrash;
            if (m_Xfer.Pairs[0].Cycle != SapXferPair.CycleMode.NextWithTrash)
            {
                CameraClose();
                return false;
            }
            if (m_Xfer.Create() == false)
            {
                CameraClose();
                return false;
            }
            _bitmap = new Bitmap[2];
            for (int i = 0; i < _bitmap.Length; i++)
            {
                _bitmap[i] = new Bitmap(GlobalVariable.configSetting.SideOutCameraPhotoSetting.ROIWidthTrue, GlobalVariable.configSetting.SideOutCameraPhotoSetting.ROIHeihgtTrue);
            }
            return true;
        }



        public bool SetCameraExposureTime(double exposure)
        {
            try
            {
                return m_AcqDevice.SetFeatureValue("ExposureTime", exposure);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 单步采集
        /// </summary>
        public bool Snap()
        {
            frameIndex = 0;
            _IsReceived = false;
            //Snap()只采集一张，如果是Snap(15)则连续采集15张
            return m_Xfer.Snap(1);
        }

        /// <summary>
        /// 连续采集
        /// </summary>
        public bool Grab()
        {
            frameIndex = 0;
            _IsReceived = false;
            return m_Xfer.Grab();
        }

        /// <summary>
        /// 冻结采集
        /// </summary>
        public bool Freeze()
        {
            _IsReceived = false;
            return m_Xfer.Freeze(); //还有m_Xfer.Abort()的用法;
        }


        /// <summary>
        /// 得到所有连接的相机信息，并将它们加入到ArrayList里面去
        /// </summary>
        /// <param name="sCameraName"></param>
        /// <param name="nIndex"></param>
        /// <returns></returns>
        public bool GetCameraInfo(string name, out string sCameraName, out int nIndex)
        {
            sCameraName = "";
            nIndex = 0;
            int serverCount = SapManager.GetServerCount();
            int GenieIndex = 0;
            System.Collections.ArrayList listServerNames = new System.Collections.ArrayList();

            if (serverCount == 0)
            {
                Console.WriteLine("No device found!\n");
                return false;
            }
            string serverName = "";
            bool cameraFound = false;
            for (int serverIndex = 0; serverIndex < serverCount; serverIndex++)
            {
                if (SapManager.GetResourceCount(serverIndex, SapManager.ResourceType.AcqDevice) != 0)
                {
                    serverName = SapManager.GetServerName(serverIndex);
                    if (SapManager.GetResourceCount(serverIndex, SapManager.ResourceType.Acq) == 0)
                    {
                        listServerNames.Add(serverName);
                        GenieIndex++;
                    }
                }
            }
            for (int i = 0; i < listServerNames.Count; i++)
            {
                if (listServerNames[i].ToString().Contains(name))
                {
                    sCameraName = listServerNames[i].ToString();
                    cameraFound = true;
                }
            }
            nIndex = GenieIndex;
            return cameraFound;
        }

        public void CameraClose()
        {
            try
            {
                if (m_Xfer != null && m_Xfer.Initialized)
                    m_Xfer.Destroy();
                if (m_Buffers != null && m_Buffers.Initialized)
                    m_Buffers.Destroy();
                if (m_AcqDevice != null && m_AcqDevice.Initialized)
                    m_AcqDevice.Destroy();

                if (m_Xfer != null)
                { m_Xfer.Dispose(); m_Xfer = null; }
                if (m_Buffers != null)
                { m_Buffers.Dispose(); m_Buffers = null; }
                if (m_AcqDevice != null)
                { m_AcqDevice.Dispose(); m_AcqDevice = null; }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool SetCameraImage(int w, int h, int x, int y)
        {
            bool bRtn;
            try
            {
                bRtn = m_AcqDevice.SetFeatureValue("Width", w);
                if (!bRtn)
                {
                    return false;
                }
                bRtn = m_AcqDevice.SetFeatureValue("Height", h);
                if (!bRtn)
                {
                    return false;
                }
                bRtn = m_AcqDevice.SetFeatureValue("OffsetX", x);
                if (!bRtn)
                {
                    return false;
                }
                //bRtn = m_AcqDevice.SetFeatureValue("OffsetY", y);
                //if (!bRtn)
                //{
                //    return false;
                //}
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void m_Xfer_XferNotify(object sender, SapXferNotifyEventArgs argsNotify)
        {
            //首先需判断此帧是否是废弃帧，若是则立即返回，等待下一帧(但这句话似乎有时候m_Xfer.Snap(n)时会导致丢帧,可以注释掉试试)
            if (argsNotify.Trash)
            {
                return;
            }
            //frameIndex++;
            //if (frameIndex == 3)
            //{
            //获取m_Buffers的地址（指针），只要知道了图片内存的地址，其实就能有各种办法搞出图片了（例如转成Bitmap）
            IntPtr addr;
            m_Buffers.GetAddress(out addr);
            //观察buffer中的图片的一些属性值，语句后注释里面的值是可能的值
            int count = m_Buffers.Count;            //2
            SapFormat format = m_Buffers.Format;    //Uint8
            double rate = m_Buffers.FrameRate;      //30.0，连续采集时，这个值会动态变化
            int height = m_Buffers.Height;          //2800
            int weight = m_Buffers.Width;           //4096
            int pixd = m_Buffers.PixelDepth;        //8

            _bitmap[pictureIndex] = new Bitmap(m_Buffers.Width, m_Buffers.Height, m_Buffers.Pitch, PixelFormat.Format8bppIndexed, addr);
            ColorPalette palette = _bitmap[pictureIndex].Palette;
            for (int i = 0; i < 256; i++)
            {
                palette.Entries[i] = Color.FromArgb(i, i, i);
            }
            _bitmap[pictureIndex].Palette = palette;
            _IsReceived = true;
            //}
        }
        
        


        public  bool takeCameraImage(int picture, ref Bitmap bitmap,ref string ErrorMsg)
        {
            pictureIndex = picture;
            bool bRtn = Snap();
            if (!bRtn)
            {
                ErrorMsg = "触发抓取指令错误";
                return false;
            }
            DateTime dt = DateTime.Now;
            while (true)
            {
                if (_IsReceived)
                {
                    Freeze();
                    bitmap =(Bitmap)_bitmap[pictureIndex].Clone();
                    return true;
                }
                else if (dt .AddSeconds(2)< DateTime.Now)
                {
                    ErrorMsg = "拍照反馈数据超时";
                    Freeze();
                    return false;
                }
            }
        }

        
    }
}
