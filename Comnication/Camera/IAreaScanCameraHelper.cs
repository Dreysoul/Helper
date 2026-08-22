using System.Drawing;

namespace YiRongMachine
{
    public interface IAreaScanCameraHelper
    {
        bool CameraOpen(string ip);

        void CameraClose();

        bool SetCameraROI(int w, int h, int x, int y, bool bOffsetY = true);

        bool SetXSSpeed(int spd);

        void Start();

        void Stop();

        bool TakeCameraImage(ref Bitmap bitmap, ref string ErrorMsg);

        bool SetCameraExposureTime(double exposure, ref string ErrorMsg);

        bool SetCameraGain(double Gain, ref string ErrorMsg);
  
    }
}