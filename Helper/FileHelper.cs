using System;
using System.IO;
using System.Windows.Forms;

namespace YiRongMachine
{
    public class FileHelper
    {
        /// <summary>
        /// 获取完整路径
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetAbsolutePathName(string fileName)
        {
            string strSystemPath = @Application.StartupPath + fileName;
            return strSystemPath;
        }

        /// <summary>
        /// 检测指定文件夹是否存在
        /// </summary>
        /// <param name="directoryPath">目录的绝对路径</param>
        public static bool IsExistDirectory(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                return false;
            }
            return Directory.Exists(directoryPath);
        }

        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="directoryPath"></param>
        public static void CreateDirectory(string directoryPath)
        {
            if (string.IsNullOrEmpty(directoryPath))
            {
                return;
            }
            System.IO.Directory.CreateDirectory(directoryPath);
        }

        /// <summary>
        /// 检测指定文件是否存在
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static bool IsExistFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return false;
            }
            return File.Exists(filePath);
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="filePath"></param>
        public static void DeleteFile(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            if (!IsExistFile(filePath))
            {
                return;
            }
            File.Delete(filePath);
        }

        /// <summary>
        /// 将一个文件夹的内容复制到另一个文件夹
        /// </summary>
        /// <param name="oldDirectory"></param>
        /// <param name="newDirectory"></param>
        /// <returns></returns>
        public static bool CopyDirectory(string oldDirectory, string newDirectory)
        {
            try
            {
                if (oldDirectory == newDirectory)
                {
                    return true;
                }
                if (Directory.Exists(newDirectory))
                {
                    Directory.Delete(newDirectory, true);
                    Directory.CreateDirectory(newDirectory);
                }
                else
                {
                    Directory.CreateDirectory(newDirectory);
                }

                string[] filenames = Directory.GetFileSystemEntries(oldDirectory);
                foreach (string file in filenames)// 遍历所有的文件和目录
                {
                    if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    {
                        string currentdir = newDirectory + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                        if (!Directory.Exists(currentdir))
                        {
                            Directory.CreateDirectory(currentdir);
                        }
                        CopyDirectory(file, currentdir);
                    }
                    else // 否则直接copy文件
                    {
                        string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);
                        srcfileName = newDirectory + "\\" + srcfileName;
                        File.Copy(file, srcfileName);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}