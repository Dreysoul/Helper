using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BearingInspection;
using System.Drawing.Imaging;

namespace YiRongMachine
{
    public partial class MultiZhuJieMian : UserControl
    {
        OuterSide_Model_Info modelInfo;
        OuterSide_Result_Info resultInfo;
        Bitmap bitmapPhoto;
        Bitmap bitmapCopy;
        Bitmap bitmapShow;
        Graphics graphicsShow;
        string errorMsg = "";
        Pen pen = new Pen(Color.Lime, 4);
        Pen pen2 = new Pen(Color.Red, 4);
        string path = FilePath.ParamSettingPath + GlobalVariable.configname + "\\WaiYuanAModel.bmp";
        public MultiZhuJieMian(int iType)
        {
            InitializeComponent();
        }
    }
}
