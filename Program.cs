using System;
using System.Windows.Forms;

namespace YiRongMachine
{
    internal static class Program
    {
        public static MyMessageFilter msgFilter = new MyMessageFilter();

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.AddMessageFilter(msgFilter);
            bool createNew;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //LightHelper lHelper = new LightHelper();
            //lHelper.OpenLightPort("COM3");
            //lHelper.setLight(5, 25);

            //HikCameraHelper cameraHelper = new HikCameraHelper();
            //cameraHelper.CameraOpen("");

            using (System.Threading.Mutex mutex = new System.Threading.Mutex(true, Application.ProductName, out createNew))
            {
                //if (createNew)
                //{
                //    Application.Run(new StartForm());
                //}
                //else
                //{
                //    MessageBox.Show("应用程序已经在运行中...");
                //    System.Threading.Thread.Sleep(1000);
                //    System.Environment.Exit(1);
                //}
                Application.Run(new StartForm());
            }
        }
    }
}