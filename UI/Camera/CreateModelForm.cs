using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using BearingInspection.IPL;
using System.Threading;

namespace YiRongMachine
{
    public partial class CreateModelForm : UserControl
    {
        Pen pen;
        Bitmap[] bitmapArray;         //6张图片，A相机图片和文字，B相机图片和文字，In相机图片，Out相机图片
        Graphics[] graphicArray;        //6张图片，6个画图工具
        public CreateModelForm()
        {
            InitializeComponent();
            pen = new Pen(Color.Red);
            pen.Width = 5;
            bitmapArray = new Bitmap[6];
            graphicArray = new Graphics[6];
        }

        private void ModelSetting_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 加载6张图形的模板文件
        /// </summary>
        private void LoadCalculateModel()
        {
            string name = GlobalVariable.configname.Substring(0, GlobalVariable.configname.LastIndexOf('.'));

            #region SideA
            if (GlobalVariable.totalConfigData.sideAParam.capMaterial == iBearingCapMaterial.Metal)
            {
                cmbSideAMateral.SelectedIndex = 0;
            }
            else
            {
                cmbSideAMateral.SelectedIndex = 1;
            }
            if (GlobalVariable.totalConfigData.sideAParam.bText)
            {
                ckbSideABHaveWord.Checked = true;
            }
            else
            {
                ckbSideABHaveWord.Checked = false;
            }
            txtSideA3.Text = GlobalVariable.totalConfigData.SideACalculateModel.BearingCap_OutRadium.ToString();
            txtSideA4.Text = GlobalVariable.totalConfigData.SideACalculateModel.BearingCap_InRadium.ToString();
            txtSideA5.Text = GlobalVariable.totalConfigData.SideACalculateModel.InnerRing_OutRadium.ToString();
            txtSideA6.Text = GlobalVariable.totalConfigData.SideACalculateModel.InnerRing_InRadium.ToString();
            txtSideA1.Text = GlobalVariable.totalConfigData.SideACalculateModel.OuterRing_OutRadium.ToString();
            txtSideA2.Text = GlobalVariable.totalConfigData.SideACalculateModel.OuterRing_InRadium.ToString();


            //显示图片模板
            if (File.Exists(FilePath.ParamSettingPath + name + "_PictureA.bmp"))
            {
                bitmapArray[0] = new Bitmap(FilePath.ParamSettingPath + name + "_PictureA.bmp");
                graphicArray[0] = Graphics.FromImage(bitmapArray[0]);
                DrawCircleAB(ref graphicArray[0], GlobalVariable.totalConfigData.SideACalculateModel);
                picboxSideAPictureModel.Image = bitmapArray[0];
            }
            //显示文字模板
            if (File.Exists(FilePath.ParamSettingPath + name + "_WordA.bmp"))
            {
                bitmapArray[1] = new Bitmap(FilePath.ParamSettingPath + name + "_WordA.bmp");
                graphicArray[1] = Graphics.FromImage(bitmapArray[1]);
                picboxSideAWordModel.BackgroundImage = bitmapArray[1];
            }
            #endregion

            #region SideB
            if (GlobalVariable.totalConfigData.sideBParam.capMaterial == iBearingCapMaterial.Metal)
            {
                cmbSideBMateral.SelectedIndex = 0;
            }
            else
            {
                cmbSideBMateral.SelectedIndex = 1;
            }
            if (GlobalVariable.totalConfigData.sideBParam.bText)
            {
                ckbSideBBHaveWord.Checked = true;
            }
            else
            {
                ckbSideBBHaveWord.Checked = false;
            }
            txtSideB3.Text = GlobalVariable.totalConfigData.SideBCalculateModel.BearingCap_OutRadium.ToString();
            txtSideB4.Text = GlobalVariable.totalConfigData.SideBCalculateModel.BearingCap_InRadium.ToString();
            txtSideB5.Text = GlobalVariable.totalConfigData.SideBCalculateModel.InnerRing_OutRadium.ToString();
            txtSideB6.Text = GlobalVariable.totalConfigData.SideBCalculateModel.InnerRing_InRadium.ToString();
            txtSideB1.Text = GlobalVariable.totalConfigData.SideBCalculateModel.OuterRing_OutRadium.ToString();
            txtSideB2.Text = GlobalVariable.totalConfigData.SideBCalculateModel.OuterRing_InRadium.ToString();

            //显示图片模板
            if (File.Exists(FilePath.ParamSettingPath + name + "_PictureB.bmp"))
            {
                bitmapArray[2] = new Bitmap(FilePath.ParamSettingPath + name + "_PictureB.bmp");
                graphicArray[2] = Graphics.FromImage(bitmapArray[2]);
                DrawCircleAB(ref graphicArray[2], GlobalVariable.totalConfigData.SideBCalculateModel);
                picboxSideBPictureModel.Image = bitmapArray[2];
            }
            //显示文字模板
            if (File.Exists(FilePath.ParamSettingPath + name + "_WordB.bmp"))
            {
                bitmapArray[3] = new Bitmap(FilePath.ParamSettingPath + name + "_WordB.bmp");
                graphicArray[3] = Graphics.FromImage(bitmapArray[3]);
                picboxSideBWordModel.BackgroundImage = bitmapArray[3];
            }
            #endregion

            #region SideIn
            this.iInRTxt.Text = GlobalVariable.totalConfigData.SideInCalculateModel.InnerRing_InRadium.ToString();
            this.iOutRTxt.Text = GlobalVariable.totalConfigData.SideInCalculateModel.ToString();

            //显示图片模板
            if (File.Exists(FilePath.ParamSettingPath + name + "_PictureIn.bmp"))
            {
                bitmapArray[4] = new Bitmap(FilePath.ParamSettingPath + name + "_PictureIn.bmp");
                graphicArray[4] = Graphics.FromImage(bitmapArray[4]);
                DrawCirecleIn(ref graphicArray[4], GlobalVariable.totalConfigData.SideInCalculateModel);
                picI.Image = bitmapArray[4];
            }

            #endregion

            #region SideOut
            oxTxt.Text = GlobalVariable.totalConfigData.SideOutCalculateModel.MainArea_OffsetX.ToString (); 
            oyTxt.Text = GlobalVariable.totalConfigData.SideOutCalculateModel.MainArea_OffsetY.ToString();
            owTxt.Text = GlobalVariable.totalConfigData.SideOutCalculateModel.MainArea_Width.ToString();
            ohTxt.Text = GlobalVariable.totalConfigData.SideOutCalculateModel.MainArea_Height.ToString();

            topXofTxt.Text = GlobalVariable.totalConfigData.SideOutCalculateModel.UpArea_OffsetX.ToString();
            topYofTxt.Text = GlobalVariable.totalConfigData.SideOutCalculateModel.UpArea_OffsetY.ToString();
            topWTxt.Text = GlobalVariable.totalConfigData.SideOutCalculateModel.UpArea_Width.ToString();
            topHTxt.Text = GlobalVariable.totalConfigData.SideOutCalculateModel.UpArea_Height.ToString();

            dnXofTxt.Text = GlobalVariable.totalConfigData.SideOutCalculateModel.DownArea_OffsetX.ToString();
            dnYofTxt.Text = GlobalVariable.totalConfigData.SideOutCalculateModel.DownArea_OffsetY.ToString();
            dnWTxt.Text = GlobalVariable.totalConfigData.SideOutCalculateModel.DownArea_Width.ToString();
            dnHTxt.Text = GlobalVariable.totalConfigData.SideOutCalculateModel.DownArea_Height.ToString();
            

            //显示图片模板
            if (File.Exists(FilePath.ParamSettingPath + name + "_PictureOut.bmp"))
            {
                bitmapArray[5] = new Bitmap(FilePath.ParamSettingPath + name + "_PictureOut.bmp");
                graphicArray[5] = Graphics.FromImage(bitmapArray[5]);
                DrawCirecleOut( ref graphicArray[5], GlobalVariable.totalConfigData.SideOutCalculateModel);
                picO.Image = bitmapArray[5];
            }
            #endregion
            return;
        }


        private void SaveCalculateModel()
        {

        }

        /// <summary>
        /// AB面的模板画圆
        /// </summary>
        /// <param name="graphics"></param>
        /// <param name="totalconfidata"></param>
        private void DrawCircleAB(ref Graphics g, CalculateModelAB pcm)
        {
            //外圈是蓝色
            if ((pcm.OuterRing_OutRadium > 0) && (pcm.OuterRing_InRadium > 0))
            {
                pen.Color = Color.Blue;
                g.DrawEllipse(pen, (float )(pcm.InnerCenterX - pcm.OuterRing_OutRadium), (float)(pcm.InnerCenterY - pcm.OuterRing_OutRadium), (float)pcm.OuterRing_OutRadium * 2, (float)pcm.OuterRing_OutRadium * 2);
                g.DrawEllipse(pen, (float)(pcm.InnerCenterX - pcm.OuterRing_InRadium), (float)(pcm.InnerCenterY - pcm.OuterRing_InRadium), (float)pcm.OuterRing_InRadium * 2, (float)pcm.OuterRing_InRadium * 2);
            }
            //端盖是红色
            if ((pcm.BearingCap_OutRadium > 0) && (pcm.BearingCap_InRadium > 0))
            {
                pen.Color = Color.Red;
                g.DrawEllipse(pen, (float)(pcm.InnerCenterX - pcm.BearingCap_OutRadium), (float)(pcm.InnerCenterY - pcm.BearingCap_OutRadium), (float)pcm.BearingCap_OutRadium * 2, (float)pcm.BearingCap_OutRadium * 2);
                g.DrawEllipse(pen, (float)(pcm.InnerCenterX - pcm.BearingCap_InRadium), (float)(pcm.InnerCenterY - pcm.BearingCap_InRadium), (float)pcm.BearingCap_InRadium * 2, (float)pcm.BearingCap_InRadium * 2);
            }
            //内圈是黄绿色
            if ((pcm.InnerRing_OutRadium > 0) && (pcm.InnerRing_InRadium > 0))
            {
                pen.Color = Color.GreenYellow;
                g.DrawEllipse(pen, (float)(pcm.InnerCenterX - pcm.InnerRing_OutRadium), (float)(pcm.InnerCenterY - pcm.InnerRing_OutRadium), (float)pcm.InnerRing_OutRadium * 2, (float)pcm.InnerRing_OutRadium * 2);
                g.DrawEllipse(pen, (float)(pcm.InnerCenterX - pcm.InnerRing_InRadium), (float)(pcm.InnerCenterY - pcm.InnerRing_InRadium), (float)pcm.InnerRing_InRadium * 2, (float)pcm.InnerRing_InRadium * 2);
            }
            return;
        }

        /// <summary>
        /// 内圈的模板画图
        /// </summary>
        private void DrawCirecleIn(ref Graphics g, CalculateModelIn pcm)
        {
            pen.Color = Color.GreenYellow;
            g.DrawEllipse(pen, (pcm.InnerCenterX - pcm.InnerRing_InRadium), (pcm.InnerCenterY - pcm.InnerRing_InRadium), pcm.InnerRing_InRadium * 2, pcm.InnerRing_InRadium * 2);

            pen.Color = Color.Red;
            g.DrawEllipse(pen, (pcm.InnerCenterX - pcm.OuterRing_OutRadium), (pcm.InnerCenterY - pcm.OuterRing_OutRadium), pcm.OuterRing_OutRadium * 2, pcm.OuterRing_OutRadium * 2);
            return;
        }

        /// <summary>
        /// 外圈的模板画圆
        /// </summary>
        private void DrawCirecleOut(ref Graphics g, CalculateModelOut pcm)
        {
            int sx, sy, ex, ey;
            pen.Color = Color.Red;
            sx = pcm.MainArea_OffsetX;
            sy = pcm.MainArea_OffsetY;
            ex = pcm.MainArea_OffsetX + pcm.MainArea_Width;
            ey = pcm.MainArea_OffsetY + pcm.MainArea_Height;
            g.DrawLine(pen, sx, sy, ex, sy);
            g.DrawLine(pen, sx, ey, ex, ey);
            g.DrawLine(pen, sx, sy, sx, ey);
            g.DrawLine(pen, ex, sy, ex, ey);
            
            pen.Color = Color.Blue;
            sx = pcm.UpArea_OffsetX;
            sy = pcm.UpArea_OffsetY;
            ex = pcm.UpArea_OffsetX + pcm.UpArea_Width;
            ey = pcm.UpArea_OffsetY + pcm.UpArea_Height;
            g.DrawLine(pen, sx, sy, ex, sy);
            g.DrawLine(pen, sx, ey, ex, ey);
            g.DrawLine(pen, sx, sy, sx, ey);
            g.DrawLine(pen, ex, sy, ex, ey);
            
            pen.Color = Color.Blue;
            sx = pcm.DownArea_OffsetX;
            sy = pcm.DownArea_OffsetY;
            ex = pcm.DownArea_OffsetX + pcm.DownArea_Width;
            ey = pcm.DownArea_OffsetY + pcm.DownArea_Height;
            g.DrawLine(pen, sx, sy, ex, sy);
            g.DrawLine(pen, sx, ey, ex, ey);
            g.DrawLine(pen, sx, sy, sx, ey);
            g.DrawLine(pen, ex, sy, ex, ey);
            return;
        }
        
        /// <summary>
        /// 轴承盖的半径变动时，重新加载区域展开图
        /// </summary>
        /// <param name="starget"></param>
        /// <param name="number"></param>
        /// <param name="picturebox"></param>
        /// <param name="InOrOut"></param>
        /// <param name="AOrB"></param>
        private void ChangeWordTemplate(SideSurfaceDefectProcessor starget, int number, PictureBox  picturebox, int InOrOut,ref Bitmap bitmap)
        {
            BearingDimensionInfo info = starget.TemplateDimensionInfo;
            if (InOrOut == 0)
            {
                info.BearingCap_InRadium += number;
            }
            else
            {
                info.BearingCap_OutRadium += number;
            }
            starget.EditTemplateDimension(info);
            bitmap = starget.CapBitmap;
            //picturebox.Width = bitmap.Width;
            //picturebox.Height = bitmap.Height;
            picturebox.BackgroundImage = bitmap;
            picturebox.Image = null;
        }


        


        /// <summary>
        /// 绘制文字模板
        /// </summary>
        /// <param name="cal"></param>
        /// <param name="graphic"></param>
        private void DrawWordTemplateRectengle(CalculateModelAB cal , ref Graphics graphic)
        {
            //重新绘制文本框
            graphic.Clear(Color.FromArgb(0x00000000));
            for (int i = 0; i < cal.wordTemplateRectengle.Count ; i++)
            {
               int x = cal.wordTemplateRectengle[i].topLeftX;
                int y = cal.wordTemplateRectengle[i].topLeftY;
                int w = cal.wordTemplateRectengle[i].BottomRigthX - cal.wordTemplateRectengle[i].topLeftX;
                int h = cal.wordTemplateRectengle[i].BottomRigthY - cal.wordTemplateRectengle[i].topLeftY;
                graphic.DrawRectangle(pen, x, y, w, h);
            }
            return;
        }


        #region A面6个减少按钮
        private void btnSideADecrease1_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideACalculateModel.OuterRing_OutRadium > 0)
            {
                GlobalVariable.totalConfigData.SideACalculateModel.OuterRing_OutRadium -= 1;
                txtSideA1.Text = GlobalVariable.totalConfigData.SideACalculateModel.OuterRing_OutRadium.ToString();
            }
        }

        private void btnSideADecrease2_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideACalculateModel.OuterRing_InRadium > 0)
            {
                GlobalVariable.totalConfigData.SideACalculateModel.OuterRing_InRadium -= 1;
                txtSideA2.Text = GlobalVariable.totalConfigData.SideACalculateModel.OuterRing_InRadium.ToString();
            }
        }

        private void btnSideADecrease3_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideACalculateModel.BearingCap_OutRadium > 0)
            {
                GlobalVariable.totalConfigData.SideACalculateModel.BearingCap_OutRadium -= 1;
                txtSideA3.Text = GlobalVariable.totalConfigData.SideACalculateModel.BearingCap_OutRadium.ToString();
                ChangeWordTemplate(GlobalVariable.SideAProcess,-1,picboxSideAWordModel,1,ref bitmapArray[1]);
            }
        }

        private void btnSideADecrease4_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideACalculateModel.BearingCap_InRadium > 0)
            {
                GlobalVariable.totalConfigData.SideACalculateModel.BearingCap_InRadium -= 1;
                txtSideA4.Text = GlobalVariable.totalConfigData.SideACalculateModel.BearingCap_InRadium.ToString();
                ChangeWordTemplate(GlobalVariable.SideAProcess, -1, picboxSideAWordModel, 0, ref bitmapArray[1]);
            }
        }

        private void btnSideADecrease5_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideACalculateModel.InnerRing_OutRadium > 0)
            {
                GlobalVariable.totalConfigData.SideACalculateModel.InnerRing_OutRadium -= 1;
                txtSideA5.Text = GlobalVariable.totalConfigData.SideACalculateModel.InnerRing_OutRadium.ToString();
            }
        }

        private void btnSideADecrease6_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideACalculateModel.InnerRing_InRadium > 0)
            {
                GlobalVariable.totalConfigData.SideACalculateModel.InnerRing_InRadium -= 1;
                txtSideA6.Text = GlobalVariable.totalConfigData.SideACalculateModel.InnerRing_InRadium.ToString();
            }
        }
        #endregion

        #region A面6个增加按钮
        private void btnSideAAdd1_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideACalculateModel.OuterRing_OutRadium > 0)
            {
                GlobalVariable.totalConfigData.SideACalculateModel.OuterRing_OutRadium += 1;
                txtSideA1.Text = GlobalVariable.totalConfigData.SideACalculateModel.OuterRing_OutRadium.ToString();
            }
        }

        private void btnSideAAdd2_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideACalculateModel.OuterRing_InRadium > 0)
            {
                GlobalVariable.totalConfigData.SideACalculateModel.OuterRing_InRadium += 1;
                txtSideA2.Text = GlobalVariable.totalConfigData.SideACalculateModel.OuterRing_InRadium.ToString();
            }
        }

        private void btnSideAAdd3_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideACalculateModel.BearingCap_OutRadium > 0)
            {
                GlobalVariable.totalConfigData.SideACalculateModel.BearingCap_OutRadium += 1;
                txtSideA3.Text = GlobalVariable.totalConfigData.SideACalculateModel.BearingCap_OutRadium.ToString();
                ChangeWordTemplate(GlobalVariable.SideAProcess, 1, picboxSideAWordModel, 1, ref bitmapArray[1]);
            }
        }

        private void btnSideAAdd4_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideACalculateModel.BearingCap_InRadium > 0)
            {
                GlobalVariable.totalConfigData.SideACalculateModel.BearingCap_InRadium += 1;
                txtSideA4.Text = GlobalVariable.totalConfigData.SideACalculateModel.BearingCap_InRadium.ToString();
                ChangeWordTemplate(GlobalVariable.SideAProcess, 1, picboxSideAWordModel, 0, ref bitmapArray[1]);
            }
        }

        private void btnSideAAdd5_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideACalculateModel.InnerRing_OutRadium > 0)
            {
                GlobalVariable.totalConfigData.SideACalculateModel.InnerRing_OutRadium += 1;
                txtSideA5.Text = GlobalVariable.totalConfigData.SideACalculateModel.InnerRing_OutRadium.ToString();
            }
        }

        private void btnSideAAdd6_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideACalculateModel.InnerRing_InRadium > 0)
            {
                GlobalVariable.totalConfigData.SideACalculateModel.InnerRing_InRadium += 1;
                txtSideA6.Text = GlobalVariable.totalConfigData.SideACalculateModel.InnerRing_InRadium.ToString();
            }
        }
        #endregion

        #region B面6个减少按钮
        private void btnSideBDecrease1_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideBCalculateModel.OuterRing_OutRadium > 0)
            {
                GlobalVariable.totalConfigData.SideBCalculateModel.OuterRing_OutRadium -= 1;
                txtSideB1.Text = GlobalVariable.totalConfigData.SideBCalculateModel.OuterRing_OutRadium.ToString();
            }
        }

        private void btnSideBDecrease2_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideBCalculateModel.OuterRing_InRadium > 0)
            {
                GlobalVariable.totalConfigData.SideBCalculateModel.OuterRing_InRadium -= 1;
                txtSideB2.Text = GlobalVariable.totalConfigData.SideBCalculateModel.OuterRing_InRadium.ToString();
            }
        }

        private void btnSideBDecrease3_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideBCalculateModel.BearingCap_OutRadium > 0)
            {
                GlobalVariable.totalConfigData.SideBCalculateModel.BearingCap_OutRadium -= 1;
                txtSideB3.Text = GlobalVariable.totalConfigData.SideBCalculateModel.BearingCap_OutRadium.ToString();
                ChangeWordTemplate(GlobalVariable.SideBProcess, -1, picboxSideBWordModel, 1, ref bitmapArray[3]);
            }
        }

        private void btnSideBDecrease4_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideBCalculateModel.BearingCap_InRadium > 0)
            {
                GlobalVariable.totalConfigData.SideBCalculateModel.BearingCap_InRadium -= 1;
                txtSideB4.Text = GlobalVariable.totalConfigData.SideBCalculateModel.BearingCap_InRadium.ToString();
                ChangeWordTemplate(GlobalVariable.SideBProcess, -1, picboxSideBWordModel, 0, ref bitmapArray[3]);
            }
        }

        private void btnSideBDecrease5_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideBCalculateModel.InnerRing_OutRadium > 0)
            {
                GlobalVariable.totalConfigData.SideBCalculateModel.InnerRing_OutRadium -= 1;
                txtSideB5.Text = GlobalVariable.totalConfigData.SideBCalculateModel.InnerRing_OutRadium.ToString();
            }
        }

        private void btnSideBDecrease6_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideBCalculateModel.InnerRing_InRadium > 0)
            {
                GlobalVariable.totalConfigData.SideBCalculateModel.InnerRing_InRadium -= 1;
                txtSideB6.Text = GlobalVariable.totalConfigData.SideBCalculateModel.InnerRing_InRadium.ToString();
            }
        }
        #endregion

        #region B面6个增加按钮
        private void btnSideBAdd1_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideBCalculateModel.OuterRing_OutRadium > 0)
            {
                GlobalVariable.totalConfigData.SideBCalculateModel.OuterRing_OutRadium += 1;
                txtSideB1.Text = GlobalVariable.totalConfigData.SideBCalculateModel.OuterRing_OutRadium.ToString();
            }
        }

        private void btnSideBAdd2_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideBCalculateModel.OuterRing_InRadium > 0)
            {
                GlobalVariable.totalConfigData.SideBCalculateModel.OuterRing_InRadium += 1;
                txtSideB2.Text = GlobalVariable.totalConfigData.SideBCalculateModel.OuterRing_InRadium.ToString();
            }
        }

        private void btnSideBAdd3_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideBCalculateModel.BearingCap_OutRadium > 0)
            {
                GlobalVariable.totalConfigData.SideBCalculateModel.BearingCap_OutRadium += 1;
                txtSideB3.Text = GlobalVariable.totalConfigData.SideBCalculateModel.BearingCap_OutRadium.ToString();
                ChangeWordTemplate(GlobalVariable.SideBProcess, 1, picboxSideBWordModel, 1, ref bitmapArray[3]);
            }
        }

        private void btnSideBAdd4_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideBCalculateModel.BearingCap_InRadium > 0)
            {
                GlobalVariable.totalConfigData.SideBCalculateModel.BearingCap_InRadium += 1;
                txtSideB4.Text = GlobalVariable.totalConfigData.SideBCalculateModel.BearingCap_InRadium.ToString();
                ChangeWordTemplate(GlobalVariable.SideBProcess, 1, picboxSideBWordModel, 0, ref bitmapArray[3]);
            }
        }

        private void btnSideBAdd5_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideBCalculateModel.InnerRing_OutRadium > 0)
            {
                GlobalVariable.totalConfigData.SideBCalculateModel.InnerRing_OutRadium += 1;
                txtSideB5.Text = GlobalVariable.totalConfigData.SideBCalculateModel.InnerRing_OutRadium.ToString();
            }
        }

        private void btnSideBAdd6_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.totalConfigData.SideBCalculateModel.InnerRing_InRadium > 0)
            {
                GlobalVariable.totalConfigData.SideBCalculateModel.InnerRing_InRadium += 1;
                txtSideB6.Text = GlobalVariable.totalConfigData.SideBCalculateModel.InnerRing_InRadium.ToString();
            }
        }
        #endregion

        #region A面6个textbox发生改变后重绘图形
        private void txtSideA1_TextChanged(object sender, EventArgs e)
        {
            graphicArray[0]. DrawImage(bitmapArray[0], 0, 0);
            DrawCircleAB(ref graphicArray[0],GlobalVariable.totalConfigData.SideACalculateModel);
            picboxSideAPictureModel.Image = bitmapArray[0];
        }

        private void txtSideA2_TextChanged(object sender, EventArgs e)
        {
            graphicArray[0].DrawImage(bitmapArray[0], 0, 0);
            DrawCircleAB(ref graphicArray[0], GlobalVariable.totalConfigData.SideACalculateModel);
            picboxSideAPictureModel.Image = bitmapArray[0];
        }

        private void txtSideA3_TextChanged(object sender, EventArgs e)
        {
            graphicArray[0].DrawImage(bitmapArray[0], 0, 0);
            DrawCircleAB(ref graphicArray[0], GlobalVariable.totalConfigData.SideACalculateModel);
            picboxSideAPictureModel.Image = bitmapArray[0];
        }

        private void txtSideA4_TextChanged(object sender, EventArgs e)
        {
            graphicArray[0].DrawImage(bitmapArray[0], 0, 0);
            DrawCircleAB(ref graphicArray[0], GlobalVariable.totalConfigData.SideACalculateModel);
            picboxSideAPictureModel.Image = bitmapArray[0];
        }

        private void txtSideA5_TextChanged(object sender, EventArgs e)
        {
            graphicArray[0].DrawImage(bitmapArray[0], 0, 0);
            DrawCircleAB(ref graphicArray[0], GlobalVariable.totalConfigData.SideACalculateModel);
            picboxSideAPictureModel.Image = bitmapArray[0];
        }

        private void txtSideA6_TextChanged(object sender, EventArgs e)
        {
            graphicArray[0].DrawImage(bitmapArray[0], 0, 0);
            DrawCircleAB(ref graphicArray[0], GlobalVariable.totalConfigData.SideACalculateModel);
            picboxSideAPictureModel.Image = bitmapArray[0];
        }
        #endregion

        #region B面6个textbox发生改变后重绘图形
        private void txtSideB1_TextChanged(object sender, EventArgs e)
        {
            graphicArray[2].DrawImage(bitmapArray[2], 0, 0);
            DrawCircleAB(ref graphicArray[2], GlobalVariable.totalConfigData.SideBCalculateModel);
            picboxSideBPictureModel.Image = bitmapArray[2];
        }

        private void txtSideB2_TextChanged(object sender, EventArgs e)
        {
            graphicArray[2].DrawImage(bitmapArray[2], 0, 0);
            DrawCircleAB(ref graphicArray[2], GlobalVariable.totalConfigData.SideBCalculateModel);
            picboxSideBPictureModel.Image = bitmapArray[2];
        }

        private void txtSideB3_TextChanged(object sender, EventArgs e)
        {
            graphicArray[2].DrawImage(bitmapArray[2], 0, 0);
            DrawCircleAB(ref graphicArray[2], GlobalVariable.totalConfigData.SideBCalculateModel);
            picboxSideBPictureModel.Image = bitmapArray[2];
        }

        private void txtSideB4_TextChanged(object sender, EventArgs e)
        {
            graphicArray[2].DrawImage(bitmapArray[2], 0, 0);
            DrawCircleAB(ref graphicArray[2], GlobalVariable.totalConfigData.SideBCalculateModel);
            picboxSideBPictureModel.Image = bitmapArray[2];
        }

        private void txtSideB5_TextChanged(object sender, EventArgs e)
        {
            graphicArray[2].DrawImage(bitmapArray[2], 0, 0);
            DrawCircleAB(ref graphicArray[2], GlobalVariable.totalConfigData.SideBCalculateModel);
            picboxSideBPictureModel.Image = bitmapArray[2];
        }

        private void txtSideB6_TextChanged(object sender, EventArgs e)
        {
            graphicArray[2].DrawImage(bitmapArray[2], 0, 0);
            DrawCircleAB(ref graphicArray[2], GlobalVariable.totalConfigData.SideBCalculateModel);
            picboxSideBPictureModel.Image = bitmapArray[2];
        }
        #endregion

        #region A面4个功能按钮
        /// <summary>
        /// 拍照
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSideATakePhoto_Click(object sender, EventArgs e)
        {
            GlobalMethod.TakePhoto(0,0,ref graphicArray[0],ref bitmapArray[0]);
            picboxSideAPictureModel.Image = bitmapArray[0];
        }

        /// <summary>
        /// 生成模板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSideACreateTemplate_Click(object sender, EventArgs e)
        {
            if (bitmapArray [0] == null )
            {
                GlobalMethod.ShowMessage("无图片能够生成模板");
                return;
            }
            DrawCircleAB(ref graphicArray[0], GlobalVariable.totalConfigData.SideACalculateModel);
            picboxSideAPictureModel.Image = bitmapArray[0];
            ChangeWordTemplate(GlobalVariable.SideAProcess,0,picboxSideAWordModel,0,ref bitmapArray[0]);
        }
        public bool bSideANewWordRect = false;
        /// <summary>
        /// 新增文本框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSideACreateWordRect_Click(object sender, EventArgs e)
        {
            if (btnSideACreateWordRect.Text == "新增文本框")
            {
                btnSideACreateWordRect.Text = "结束新增文本框";
                bSideANewWordRect = true;
            }
            else
            {
                btnSideACreateWordRect.Text = "新增文本框";
                bSideANewWordRect = false;
            }
        }
        /// <summary>
        /// 删除所有文本框
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSideADeleteWordRect_Click(object sender, EventArgs e)
        {
            DrawWordTemplateRectengle(GlobalVariable.totalConfigData.SideACalculateModel,ref graphicArray[1]);
        }
        #endregion

        #region B面4个功能按钮
        private void btnSideBTakePhoto_Click(object sender, EventArgs e)
        {
            GlobalMethod. TakePhoto(1, 0,ref graphicArray[2], ref bitmapArray[2]);
            picboxSideAPictureModel.Image = bitmapArray[2];
        }

        private void btnSideBCreateTemplate_Click(object sender, EventArgs e)
        {
            if (bitmapArray[2] == null)
            {
                GlobalMethod.ShowMessage("无图片能够生成模板");
                return;
            }
            DrawCircleAB(ref graphicArray[2], GlobalVariable.totalConfigData.SideBCalculateModel);
            picboxSideBPictureModel.Image = bitmapArray[2];
            ChangeWordTemplate(GlobalVariable.SideBProcess, 0, picboxSideBWordModel, 0, ref bitmapArray[2]);
        }
        public bool bSideBNewWordRect = false;
        private void btnSideBCreateWordRect_Click(object sender, EventArgs e)
        {
            if (btnSideBCreateWordRect.Text == "新增文本框")
            {
                btnSideBCreateWordRect.Text = "结束新增文本框";
                bSideBNewWordRect = true;
            }
            else
            {
                btnSideBCreateWordRect.Text = "新增文本框";
                bSideBNewWordRect = false;
            }
        }

        private void btnSideBDeleteWordRect_Click(object sender, EventArgs e)
        {
            //GlobalVariable.totalConfigData.SideBCalculateModel.wordTemplateNumber = 0;
            DrawWordTemplateRectengle(GlobalVariable.totalConfigData.SideBCalculateModel, ref graphicArray[3]);
        }

        private void cmbSideAMateral_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ckbSideABHaveWord_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void iBtn_Click(object sender, EventArgs e)
        {

        }
        #endregion





        //       private void createSample(SideSurfaceDefectProcessor starget, Graphics g, Bitmap bmp,  CalculateModelAB pcm, BearingCapMaterial capMaterial, bool wordType) 
        //           {

        //       starget.CapBitmap = null;
        //	starget.CreateTemplateInfo(bmp, capMaterial, wordType);
        //       BearingDimensionInfo info = starget.TemplateDimensionInfo;	//获取计算的模板数值
        //	//内圈
        //	pcm.InnerCenterX = info.InnerCenterX;
        //	pcm.InnerCenterY = info.InnerCenterY;
        //	pcm.InnerRing_OutRadium = info.InnerRing_OutRadium;
        //	pcm.InnerRing_InRadium = info.InnerRing_InRadium;
        //	//外圈
        //	pcm.OuterCenterX = info.OuterCenterX;
        //	pcm.OuterCenterY = info.OuterCenterY;
        //	pcm.OuterRing_OutRadium = info.OuterRing_OutRadium;
        //	pcm.OuterRing_InRadium = info.OuterRing_InRadium;
        //	//端盖
        //	pcm.CapCenterX = info.CapCenterX;
        //	pcm.CapCenterY = info.CapCenterY;
        //	pcm.BearingCap_InRadium = info.BearingCap_InRadium;
        //	pcm.BearingCap_OutRadium = info.BearingCap_OutRadium;
        //	return;
        //}



    }
}
