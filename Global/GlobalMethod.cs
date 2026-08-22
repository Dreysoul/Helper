using BearingInspection;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace YiRongMachine
{
    public class GlobalMethod
    {
        [DllImport("kernel32")]
        private static extern uint GetTickCount();

        public static uint delay_ms(uint delay_time)
        {
            //uint time_start = GetTickCount();
            //uint time_stamp = 0;
            //do
            //{
            //    time_stamp = GetTickCount() - time_start;
            //}
            //while (time_stamp < delay_time);
            uint iT = 0;
            for (int i = 0; i < delay_time; i++)
            {
                for (int j = 0; j < 1; j++)
                {
                    for (int k = 0; k < 750000; k++)
                    {
                        iT++;
                    }
                }
            }
            return iT;
        }

        /// <summary>
        /// 根据按钮来选择展示相应的界面
        /// </summary>
        /// <param name="index"></param>
        /// <param name="form"></param>
        public static void DisplayForm(Panel pnl, UserControl form)
        {
            //加载揭秘那
            form.Show();
            form.Visible = true;
            pnl.Controls.Clear();
            pnl.Controls.Add(form);
            form.Dock = DockStyle.Fill;
        }

        public static void ShowMessage(string msg)
        {
            AlarmDialog a = new AlarmDialog(msg, MsgType.Message);
            a.ShowDialog();
        }

        /// <summary>
        /// 拍一张照片，光源只开不关
        /// </summary>
        /// <param name="index"></param>
        /// <param name="graphics"></param>
        /// <param name="bitmap"></param>
        public static bool TakePhoto_SideAB(int index, int m_PictureIndex, double exposure, ref Bitmap bitmap, ref string ErrorMsg)
        {
            Stopwatch st = new Stopwatch();
            bool bRtn = false;
            try
            {
                if (index == 0)
                {
                    GlobalVariable.lightHelper[index].OperateLight(GlobalVariable.configSetting.duanMianACameraPhotoSetting.lightsCmd[m_PictureIndex]);
                    GlobalVariable.DuanMianACameraHelper.SetCameraExposureTime(exposure, ref ErrorMsg);
                    Thread.Sleep(1);
                    bRtn = GlobalVariable.DuanMianACameraHelper.TakeCameraImage(ref bitmap, ref ErrorMsg);
                    if (!bRtn)
                    {
                        ErrorMsg = "A面相机拍照失败";
                        return false;
                    }
                }
                else if (index == 1)
                {
                    GlobalVariable.lightHelper[index].OperateLight(GlobalVariable.configSetting.duanMianBCameraPhotoSetting.lightsCmd[m_PictureIndex]);
                    GlobalVariable.DuanMianBCameraHelper.SetCameraExposureTime(exposure, ref ErrorMsg);
                    Thread.Sleep(1);
                    bRtn = GlobalVariable.DuanMianBCameraHelper.TakeCameraImage(ref bitmap, ref ErrorMsg);
                    if (!bRtn)
                    {
                        ErrorMsg = "B面相机拍照失败";
                        return false;
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 拍一张照片，光源既开又关
        /// </summary>
        /// <param name="index"></param>
        /// <param name="graphics"></param>
        /// <param name="bitmap"></param>
        public static bool TakePhoto_SideInOut(int index, int m_PictureIndex, double exposure, ref Bitmap bitmap, ref string ErrorMsg)
        {
            bool bRtn = false;
            try
            {
                GlobalVariable.WaiYuanACameraHelper.SetCameraExposureTime(exposure, ref ErrorMsg);
                GlobalVariable.WaiYuanACameraHelper.Start();
                Thread.Sleep(1);
                bRtn = GlobalVariable.WaiYuanACameraHelper.TakeCameraImage(ref bitmap, ref ErrorMsg);
                GlobalVariable.WaiYuanACameraHelper.Stop();
                if (!bRtn)
                {
                    ErrorMsg = "外圆A相机拍照失败";
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

        public static void DrawYuan_SideAB(Graphics g, Pen pen, Surface_Result_Info info)
        {
            if (g == null)
            {
                return;
            }
            try
            {
                ////外圆
                //g.DrawEllipse(pen, (float)(info.fOuterLoopCenterX - info.fOuterLoopMaxRadius), (float)(info.fInnerLoopMinCenterY - info.fOuterLoopMaxRadius), info.fOuterLoopMaxRadius * 2, info.fOuterLoopMaxRadius * 2);
                //g.DrawEllipse(pen, (float)(info.fInnerLoopMinCenterX - info.fOuterLoopMinRadius), (float)(info.fInnerLoopMinCenterY - info.fOuterLoopMinRadius), info.fOuterLoopMinRadius * 2, info.fOuterLoopMinRadius * 2);
                //g.DrawEllipse(pen, (float)(info.fInnerLoopMinCenterX - info.fOuterLoopMaxValidRadius), (float)(info.fInnerLoopMinCenterY - info.fOuterLoopMaxValidRadius), info.fOuterLoopMaxValidRadius * 2, info.fOuterLoopMaxValidRadius * 2);
                ////密封圈
                //g.DrawEllipse(pen, (float)(info.fInnerLoopMinCenterX - info.fMifengMaxRadius), (float)(info.fInnerLoopMinCenterY - info.fMifengMaxRadius), info.fMifengMaxRadius * 2, info.fMifengMaxRadius * 2);
                //g.DrawEllipse(pen, (float)(info.fInnerLoopMinCenterX - info.fMifengMinRadius), (float)(info.fInnerLoopMinCenterY - info.fMifengMinRadius), info.fMifengMinRadius * 2, info.fMifengMinRadius * 2);
                ////内圈
                //g.DrawEllipse(pen, (float)(info.fInnerLoopMinCenterX - info.fInnerLoopMaxRadius), (float)(info.fInnerLoopMinCenterY - info.fInnerLoopMaxRadius), info.fInnerLoopMaxRadius * 2, info.fInnerLoopMaxRadius * 2);
                //g.DrawEllipse(pen, (float)(info.fInnerLoopMinCenterX - info.fInnerLoopMinRadius), (float)(info.fInnerLoopMinCenterY - info.fInnerLoopMinRadius), info.fInnerLoopMinRadius * 2, info.fInnerLoopMinRadius * 2);
                //g.DrawEllipse(pen, (float)(info.fInnerLoopMinCenterX - info.fInnerLoopMinValidRadius), (float)(info.fInnerLoopMinCenterY - info.fInnerLoopMinValidRadius), info.fInnerLoopMinValidRadius * 2, info.fInnerLoopMinValidRadius * 2);

                //外圆
                g.DrawEllipse(pen, (float)(info.fOuterLoopMaxCenterX - info.fOuterLoopMaxRadius), (float)(info.fOuterLoopMaxCenterY - info.fOuterLoopMaxRadius), info.fOuterLoopMaxRadius * 2, info.fOuterLoopMaxRadius * 2);
                g.DrawEllipse(pen, (float)(info.fOuterLoopMinCenterX - info.fOuterLoopMinRadius), (float)(info.fOuterLoopMinCenterY - info.fOuterLoopMinRadius), info.fOuterLoopMinRadius * 2, info.fOuterLoopMinRadius * 2);
                g.DrawEllipse(pen, (float)(info.fOuterLoopMaxValidCenterX - info.fOuterLoopMaxValidRadius), (float)(info.fOuterLoopMaxValidCenterY - info.fOuterLoopMaxValidRadius), info.fOuterLoopMaxValidRadius * 2, info.fOuterLoopMaxValidRadius * 2);
                //密封圈
                g.DrawEllipse(pen, (float)(info.fMifengMaxCenterX - info.fMifengMaxRadius), (float)(info.fMifengMaxCenterY - info.fMifengMaxRadius), info.fMifengMaxRadius * 2, info.fMifengMaxRadius * 2);
                g.DrawEllipse(pen, (float)(info.fMifengMinCenterX - info.fMifengMinRadius), (float)(info.fMifengMinCenterY - info.fMifengMinRadius), info.fMifengMinRadius * 2, info.fMifengMinRadius * 2);
                //内圈
                g.DrawEllipse(pen, (float)(info.fInnerLoopMaxCenterX - info.fInnerLoopMaxRadius), (float)(info.fInnerLoopMaxCenterY - info.fInnerLoopMaxRadius), info.fInnerLoopMaxRadius * 2, info.fInnerLoopMaxRadius * 2);
                g.DrawEllipse(pen, (float)(info.fInnerLoopMinCenterX - info.fInnerLoopMinRadius), (float)(info.fInnerLoopMinCenterY - info.fInnerLoopMinRadius), info.fInnerLoopMinRadius * 2, info.fInnerLoopMinRadius * 2);
                g.DrawEllipse(pen, (float)(info.fInnerLoopMinValidCenterX - info.fInnerLoopMinValidRadius), (float)(info.fInnerLoopMinValidCenterY - info.fInnerLoopMinValidRadius), info.fInnerLoopMinValidRadius * 2, info.fInnerLoopMinValidRadius * 2);
            }
            catch
            {
            }
        }

        public static void DrawYuan_SideIn(Graphics g, Pen pen, InnerHole_Result_Info info)
        {
            if (g == null)
            {
                return;
            }
            try
            {
                g.DrawEllipse(pen, (float)(info.fInnerLoopInnerCenterX - info.fInnerLoopInnerRadius), (float)(info.fInnerLoopInnerCenterY - info.fInnerLoopInnerRadius), info.fInnerLoopInnerRadius * 2, info.fInnerLoopInnerRadius * 2);
                g.DrawEllipse(pen, (float)(info.fInnerLoopMaxCenterX - info.fInnerLoopMaxRadius), (float)(info.fInnerLoopMaxCenterY - info.fInnerLoopMaxRadius), info.fInnerLoopMaxRadius * 2, info.fInnerLoopMaxRadius * 2);
            }
            catch
            {
            }
        }

        public static void DrawYuan_SideOut(Graphics g, Pen pen, OuterSide_Result_Info resultInfo)
        {
            if (g == null)
            {
                return;
            }
            try
            {
                //resultInfo.dwValidRegionWidth = 100;
                //resultInfo.dwDownRoundRegionHeight = 100;
                //g.DrawRectangle(pen, GlobalVariable.configSetting.SideOutModel.dwCircleRegionLeftX, GlobalVariable.configSetting.SideOutModel.dwCircleRegionTopY, resultInfo.dwValidRegionWidth, resultInfo.dwValidRegionHeight);
                //g.DrawRectangle(pen, GlobalVariable.configSetting.SideOutModel.dwCircleRegionLeftX, GlobalVariable.configSetting.SideOutModel.dwCircleRegionTopY - resultInfo.dwUpRoundRegionHeight, resultInfo.dwValidRegionWidth, resultInfo.dwUpRoundRegionHeight);
                //g.DrawRectangle(pen, GlobalVariable.configSetting.SideOutModel.dwCircleRegionLeftX, GlobalVariable.configSetting.SideOutModel.dwCircleRegionTopY + resultInfo.dwValidRegionHeight, resultInfo.dwValidRegionWidth, resultInfo.dwDownRoundRegionHeight);
                g.DrawRectangle(pen, resultInfo.fCircleRegionLeftX, resultInfo.fCircleRegionTopY, resultInfo.dwValidRegionWidth, resultInfo.dwValidRegionHeight);
                g.DrawRectangle(pen, resultInfo.fCircleRegionLeftX, resultInfo.fCircleRegionTopY - resultInfo.dwUpRoundRegionHeight, resultInfo.dwValidRegionWidth, resultInfo.dwUpRoundRegionHeight);
                g.DrawRectangle(pen, resultInfo.fCircleRegionLeftX, resultInfo.fCircleRegionTopY + resultInfo.dwValidRegionHeight, resultInfo.dwValidRegionWidth, resultInfo.dwDownRoundRegionHeight);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 根据起始点和终结点获得相应的结构体信息
        /// </summary>
        /// <param name="startPoint"></param>
        /// <param name="endPoint"></param>
        /// <param name="wordTemplate"></param>
        //public static void GetCorrectRectInfo(Point startPoint, Point endPoint, ref WordTemplateRectengle wordTemplate)
        //{
        //    if (startPoint.X == endPoint.X && startPoint.Y == endPoint.Y)
        //    {
        //        return;
        //    }
        //    if (startPoint.X < endPoint.X && startPoint.Y < endPoint.Y)
        //    {
        //        wordTemplate.topLeftX = startPoint.X;
        //        wordTemplate.topLeftY = startPoint.Y;
        //        wordTemplate.BottomRigthX = endPoint.X;
        //        wordTemplate.BottomRigthY = endPoint.Y;
        //    }
        //    else if (startPoint.X < endPoint.X && startPoint.Y > endPoint.Y)
        //    {
        //        wordTemplate.topLeftX = startPoint.X;
        //        wordTemplate.topLeftY = endPoint.Y;
        //        wordTemplate.BottomRigthX = endPoint.X;
        //        wordTemplate.BottomRigthY = startPoint.Y;
        //    }
        //    else if (startPoint.X > endPoint.X && startPoint.Y < endPoint.Y)
        //    {
        //        wordTemplate.topLeftX = endPoint.X;
        //        wordTemplate.topLeftY = startPoint.Y;
        //        wordTemplate.BottomRigthX = startPoint.X;
        //        wordTemplate.BottomRigthY = endPoint.Y;
        //    }
        //    else if (startPoint.X > endPoint.X && startPoint.Y > endPoint.Y)
        //    {
        //        wordTemplate.topLeftX = endPoint.X;
        //        wordTemplate.topLeftY = endPoint.Y;
        //        wordTemplate.BottomRigthX = startPoint.X;
        //        wordTemplate.BottomRigthY = startPoint.Y;
        //    }
        //}

        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="bitmapArray"></param>
        /// <param name="path"></param>
        /// <param name="ErrorMsg"></param>
        /// <returns></returns>
        public static bool SavePicture(Bitmap[] bitmapArray, string path, ref string ErrorMsg)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                for (int i = 0; i < bitmapArray.Length; i++)
                {
                    bitmapArray[i].Save(path + "\\" + i.ToString() + ".bmp", ImageFormat.Bmp);
                }
                return true;
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
                return false;
            }
        }

        public static bool SavePicture(Bitmap bitmapArray, string path, ref string ErrorMsg)
        {
            try
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                bitmapArray.Save(path + "\\0.bmp", ImageFormat.Bmp);

                return true;
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
                return false;
            }
        }

        //static  object o1 = new object();
        // public static  void CalculateOneBearInfor(int index, int result)
        // {
        //     lock (o1)
        //     {
        //         if (GlobalVariable.OneBearInfor == null || GlobalVariable.OneBearInfor.Count == 0)
        //         {
        //             return;
        //         }
        //         for (int i = 0; i < GlobalVariable.OneBearInfor.Count; i++)
        //         {
        //             if (GlobalVariable.OneBearInfor[i][0] == GlobalVariable.totalDataCollect.Total)
        //             {
        //                 GlobalVariable.OneBearInfor[i][index + 1] = result;
        //             }
        //             if (GlobalVariable.OneBearInfor[i][1] != 0 && GlobalVariable.OneBearInfor[i][2] != 0 && GlobalVariable.OneBearInfor[i][3] != 0 && GlobalVariable.OneBearInfor[i][4] != 0)
        //             {
        //                 for (int j = 0; j < GlobalVariable.OneBearInfor[i].Length; j++)
        //                 {
        //                     if (GlobalVariable.OneBearInfor[i][j] == 2)
        //                     {
        //                         GlobalVariable.totalDataCollect.TotalNG++;
        //                         GlobalVariable.OneBearInfor.RemoveAt(i);
        //                         return;
        //                     }
        //                 }
        //                 GlobalVariable.OneBearInfor.RemoveAt(i);
        //             }
        //             else if (GlobalVariable.totalDataCollect.Total - GlobalVariable.OneBearInfor[i][0] > 10)
        //             {
        //                 GlobalVariable.OneBearInfor.RemoveAt(i);
        //             }
        //         }
        //     }
        // }

        public static Bitmap FileToBitmap(string fileName)
        {
            FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
            // 读取文件的 byte[]
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            fileStream.Close();
            // 把 byte[] 转换成 Stream
            Stream stream = new MemoryStream(bytes);

            stream.Read(bytes, 0, bytes.Length);
            // 设置当前流的位置为流的开始
            stream.Seek(0, SeekOrigin.Begin);

            MemoryStream mstream = null;
            try
            {
                mstream = new MemoryStream(bytes);
                return new Bitmap(stream);
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }

    }
}