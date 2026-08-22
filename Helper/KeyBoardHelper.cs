using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace YiRongMachine
{
    public class KeyBoardHelper
    {
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        private const UInt32 WM_SYSCOMMAND = 0x112;
        private const UInt32 SC_RESTORE = 0xf120;

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);

        private const string OnScreenKeyboadApplication = "osk.exe";

        /// <summary>
        /// 启用系统软键盘
        /// </summary>
        public static void OpenKeyBoardFun()
        {
            try
            {
                //判断软键盘是否进程是否已经存在，如果不存在进行调用
                Process[] pro = Process.GetProcessesByName("osk");
                //如果键盘已打开，则进行关闭操作
                if (pro != null && pro.Length > 0)
                {
                    CloseKeyBoardFun();
                    return;
                }

                // Get the name of the On screen keyboard
                string processName = System.IO.Path.GetFileNameWithoutExtension(OnScreenKeyboadApplication);

                // Check whether the application is not running
                var query = from process in Process.GetProcesses()
                            where process.ProcessName == processName
                            select process;

                var keyboardProcess = query.FirstOrDefault();

                // launch it if it doesn't exist
                if (keyboardProcess == null)
                {
                    IntPtr ptr = new IntPtr(); ;
                    bool sucessfullyDisabledWow64Redirect = false;

                    // Disable x64 directory virtualization if we're on x64,
                    // otherwise keyboard launch will fail.
                    if (System.Environment.Is64BitOperatingSystem)
                    {
                        sucessfullyDisabledWow64Redirect = Wow64DisableWow64FsRedirection(ref ptr);
                    }

                    // osk.exe is in windows/system folder. So we can directky call it without path
                    using (Process osk = new Process())
                    {
                        osk.StartInfo.FileName = OnScreenKeyboadApplication;
                        osk.Start();
                        //osk.WaitForInputIdle(2000);
                    }

                    // Re-enable directory virtualisation if it was disabled.
                    if (System.Environment.Is64BitOperatingSystem)
                        if (sucessfullyDisabledWow64Redirect)
                            Wow64RevertWow64FsRedirection(ptr);
                }
                else
                {
                    // Bring keyboard to the front if it's already running
                    var windowHandle = keyboardProcess.MainWindowHandle;
                    SendMessage(windowHandle, WM_SYSCOMMAND, new IntPtr(SC_RESTORE), new IntPtr(0));
                }
            }
            catch (Exception ex)
            {
                LogHelper.AddCommLog("打开系统键盘出现异常，异常信息为" + ex.Message);
            }
        }

        /// <summary>
        /// 关闭系统软键盘
        /// </summary>
        public static void CloseKeyBoardFun()
        {
            try
            {
                Process[] pros = Process.GetProcessesByName("osk");
                foreach (Process p in pros)
                {
                    p.Kill();
                }
            }
            catch (Exception ex)
            {
                LogHelper.AddCommLog("关闭系统键盘出现异常，异常信息为" + ex.Message);
            }
        }
    }
}