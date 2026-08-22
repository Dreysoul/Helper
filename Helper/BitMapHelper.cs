using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace YiRongMachine
{
    public class BitMapHelper
    {
        /// <summary>
        /// 将byte数组转换为bitmap
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="bmp"></param>
        public static void BufferToBmp(byte[] buffer, ref Bitmap bmp)
        {
            Rectangle rect = new Rectangle();
            rect.X = 0;
            rect.Y = 0;
            rect.Width = bmp.Width;
            rect.Height = bmp.Height;
            int length = bmp.Width * bmp.Height;
            BitmapData bmpData = bmp.LockBits(rect, ImageLockMode.WriteOnly, bmp.PixelFormat);
            //获取图像参数
            int stride = bmpData.Stride;         // 扫描线的宽度
            int offset = stride - rect.Width;    // 显示宽度与扫描线宽度的间隙
            IntPtr iptr = bmpData.Scan0;         // 获取bmpData的内存起始位置
            int scanBytes = stride * rect.Height;// 用stride宽度，表示这是内存区域的大小

            //下面把原始的显示大小字节数组转换为内存中实际存放的字节数组
            int posScan = 0, posReal = 0;         //分别设置两个位置指针，指向源数组和目标数组
            byte[] pixelValues = new byte[scanBytes];  //为目标数组分配内存
            for (int x = 0; x < rect.Height; x++)
            {
                //下面的循环节是模拟行扫描
                for (int y = 0; y < rect.Width; y++)
                {
                    pixelValues[posScan++] = buffer[posReal++];
                }
                posScan += offset;  //行扫描结束，要将目标位置指针移过那段“间隙”
            }

            //用Marshal的Copy方法，将刚才得到的内存字节数组复制到BitmapData中
            System.Runtime.InteropServices.Marshal.Copy(pixelValues, 0, iptr, scanBytes);
            bmp.UnlockBits(bmpData);  // 解锁内存区域
            return;
        }

        /// <summary>
        /// 获得灰度值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int getPixelDot(int x, int y, Bitmap bitmap)
        {
            try
            {
                Color color = bitmap.GetPixel(x, y);
                //int i = (int)(color.R * 0.299 + color.G * 0.587 + color.B * 0.114);
                int i = color.R;
                return i;
            }
            catch (Exception)
            {
                return 0;
            }
            //return 1;
        }

        /// <summary>
        /// 将图片翻转90度
        /// </summary>
        /// <param name="btResource"></param>
        public static void RotateBitmap(ref Bitmap btResource)
        {
            //btResource.RotateFlip(RotateFlipType.Rotate90FlipX);
            btResource.RotateFlip(RotateFlipType.Rotate90FlipNone);
            return;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            Bitmap resizedImage = new Bitmap(width, height);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

                using (ImageAttributes wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                    Rectangle rect = new Rectangle(0, 0, width, height);
                    graphics.DrawImage(image, rect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return resizedImage;
        }
    }
}