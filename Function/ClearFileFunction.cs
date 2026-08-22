using System;
using System.IO;

namespace YiRongMachine
{
    public class ClearFileFunction
    {
        public static bool ClearLogFile(int logDay, int waveDay)
        {
            DirectoryInfo di;
            FileInfo[] filename;

            if (Directory.Exists(GlobalVariable.path + "Log\\Run"))
            {
                di = new DirectoryInfo(GlobalVariable.path + "Log\\Run");
                filename = di.GetFiles();
                for (int i = 0; i < filename.Length; i++)
                {
                    try
                    {
                        filename[i].Delete();
                    }
                    catch
                    {
                    }
                }
            }

            if (Directory.Exists(GlobalVariable.path + "Log\\PLC"))
            {
                di = new DirectoryInfo(GlobalVariable.path + "Log\\PLC");
                filename = di.GetFiles();
                for (int i = 0; i < filename.Length; i++)
                {
                    try
                    {
                        filename[i].Delete();
                    }
                    catch
                    {
                    }
                }
            }
            if (Directory.Exists(GlobalVariable.path + "Log\\CCD"))
            {
                di = new DirectoryInfo(GlobalVariable.path + "Log\\CCD");
                filename = di.GetFiles();
                for (int i = 0; i < filename.Length; i++)
                {
                    try
                    {
                        filename[i].Delete();
                    }
                    catch
                    {
                    }
                }
            }
            if (Directory.Exists(GlobalVariable.path + "Log\\SFC"))
            {
                di = new DirectoryInfo(GlobalVariable.path + "Log\\SFC");
                filename = di.GetFiles();
                for (int i = 0; i < filename.Length; i++)
                {
                    try
                    {
                        filename[i].Delete();
                    }
                    catch
                    {
                    }
                }
            }
            if (Directory.Exists(GlobalVariable.path + "Log\\Throw"))
            {
                di = new DirectoryInfo(GlobalVariable.path + "Log\\Throw");
                filename = di.GetFiles();
                for (int i = 0; i < filename.Length; i++)
                {
                    try
                    {
                        filename[i].Delete();
                    }
                    catch
                    {
                    }
                }
            }
            if (Directory.Exists(GlobalVariable.path + "Log\\NG"))
            {
                di = new DirectoryInfo(GlobalVariable.path + "Log\\NG");
                filename = di.GetFiles();
                for (int i = 0; i < filename.Length; i++)
                {
                    try
                    {
                        filename[i].Delete();
                    }
                    catch
                    {
                    }
                }
            }

            if (Directory.Exists(GlobalVariable.path + "Log\\Exception"))
            {
                di = new DirectoryInfo(GlobalVariable.path + "Log\\Exception");
                filename = di.GetFiles();
                for (int i = 0; i < filename.Length; i++)
                {
                    if (DateTime.Now.AddDays(-logDay) > filename[i].LastWriteTime)
                    {
                        try
                        {
                            filename[i].Delete();
                        }
                        catch
                        {
                        }
                    }
                }
            }
            DirectoryInfo[] directoryname;
            if (Directory.Exists(GlobalVariable.path + "pic"))
            {
                di = new DirectoryInfo(GlobalVariable.path + "pic");
                directoryname = di.GetDirectories();
                for (int i = 0; i < directoryname.Length; i++)
                {
                    if (DateTime.Now.AddDays(-waveDay) > directoryname[i].LastWriteTime)
                    {
                        try
                        {
                            directoryname[i].Delete(true);
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return true;
        }
    }
}