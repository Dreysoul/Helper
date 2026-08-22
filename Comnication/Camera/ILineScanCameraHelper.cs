using DALSA.SaperaLT.SapClassBasic;
using System.Drawing;

namespace YiRongMachine
{
    public interface ILineScanCameraHelper
    {
        bool CameraOpen(string name, ref string ErrorMsg);

        bool SetCameraExposureTime(double exposure);

        bool Snap();

        bool Grab();

        bool Freeze();

        bool GetCameraInfo(string name, out string sCameraName, out int nIndex);

        void CameraClose();

        bool SetCameraImage(int w, int h, int x, int y);

        void m_Xfer_XferNotify(object sender, SapXferNotifyEventArgs argsNotify);

        bool takeCameraImage(int picture, ref Bitmap bitmap, ref string ErrorMsg);
    }
}