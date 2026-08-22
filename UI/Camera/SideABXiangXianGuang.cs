using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BearingInspection;

namespace YiRongMachine
{
    public partial class SideABXiangXianGuang : UserControl
    {
        public int sideAB = 0;
        Surface_Param_Info info;
        public Bitmap[] bitmapOrignal = new Bitmap[4];
        Bitmap[] bitmapShow = new Bitmap[4];
        Graphics[] graghicsShow = new Graphics[4];
        Pen pen = new Pen(Color.Red, 11);
        public SideABXiangXianGuang(int side)
        {
            sideAB = side;
            InitializeComponent();
            if (sideAB == 0)
            {
                info = GlobalVariable.configSetting.sideAParam;
            }
            else if (sideAB == 1)
            {
                info = GlobalVariable.configSetting.sideBParam;
            }
            LoadParam();
            
            // 事件绑定
            List<Control> allControl = new List<Control>();
            UIHelper.GeiAllControls(this, allControl);
            for (int i = 0; i < allControl.Count; i++)
            {
                if (allControl[i].GetType() == typeof(TextBox))
                {
                    allControl[i].KeyPress += UIHelper.textBox_KeyPress;
                    allControl[i].Enter += UIHelper.textBox_Enter;
                    allControl[i].Leave += UIHelper.textBox_Leave;
                }
            }
        }

        void LoadParam()
        {
            txt324.Text = info.dwMifengSubRegionOuterOffset.ToString();
            txt325.Text = info.dwMifengSubRegionInnerOffset.ToString();
            txt326.Text = info.dwMifengRegionOneStartAngle.ToString();
            txt327.Text = info.dwMifengRegionOneEndAngle.ToString();
            txt328.Text = info.dwMifengRegionTwoStartAngle.ToString();
            txt329.Text = info.dwMifengRegionTwoEndAngle.ToString();
            txt330.Text = info.dwMifengRegionThreeStartAngle.ToString();
            txt331.Text = info.dwMifengRegionThreeEndAngle.ToString();
            txt332.Text = info.dwMifengRegionFourStartAngle.ToString();
            txt333.Text = info.dwMifengRegionFourEndAngle.ToString();
        }
       
        #region 加减按钮和画图的绑定
        private void btnDecreaseAll1_Click(object sender, EventArgs e)
        {
            info.dwMifengSubRegionOuterOffset--;
            txt324.Text = info.dwMifengSubRegionOuterOffset.ToString();
        }

        private void btnDecreaseAll2_Click(object sender, EventArgs e)
        {
            info.dwMifengSubRegionInnerOffset--;
            txt325.Text = info.dwMifengSubRegionInnerOffset.ToString();
        }

        private void btnAddAll1_Click(object sender, EventArgs e)
        {
            info.dwMifengSubRegionOuterOffset++;
            txt324.Text = info.dwMifengSubRegionOuterOffset.ToString();
        }

        private void btnAddAll2_Click(object sender, EventArgs e)
        {
            info.dwMifengSubRegionInnerOffset--;
            txt325.Text = info.dwMifengSubRegionInnerOffset.ToString();
        }
        private void txt324_TextChanged(object sender, EventArgs e)
        {
            info.dwMifengSubRegionOuterOffset = Convert.ToInt32(txt324.Text );
            drawAllArea();
        }

        private void txt325_TextChanged(object sender, EventArgs e)
        {
            info.dwMifengSubRegionInnerOffset = Convert.ToInt32(txt325.Text);
            drawAllArea();
        }

        private void txt326_TextChanged(object sender, EventArgs e)
        {
            info.dwMifengRegionOneStartAngle = Convert.ToUInt32(txt326.Text);
            if (graghicsShow[0] == null)
            {
                return;
            }
            graghicsShow[0].DrawImage(bitmapOrignal[0], 0, 0);
            drawOnePictureArea(0, info.dwMifengRegionOneStartAngle, info.dwMifengRegionOneEndAngle, info.dwMifengSubRegionOuterOffset, info.dwMifengSubRegionInnerOffset);
            pictureBox1.Image = bitmapShow[0];
        }

        private void txt327_TextChanged(object sender, EventArgs e)
        {
            info.dwMifengRegionOneEndAngle = Convert.ToUInt32(txt327.Text);
            if (graghicsShow[0] == null)
            {
                return;
            }
            graghicsShow[0].DrawImage(bitmapOrignal[0], 0, 0);
            drawOnePictureArea(0, info.dwMifengRegionOneStartAngle, info.dwMifengRegionOneEndAngle, info.dwMifengSubRegionOuterOffset, info.dwMifengSubRegionInnerOffset);
            pictureBox1.Image = bitmapShow[0];
        }

        private void txt328_TextChanged(object sender, EventArgs e)
        {
            info.dwMifengRegionTwoStartAngle = Convert.ToUInt32(txt328.Text);
            if (graghicsShow[0] == null)
            {
                return;
            }
            graghicsShow[1].DrawImage(bitmapOrignal[1], 0, 0);
            drawOnePictureArea(1, info.dwMifengRegionTwoStartAngle, info.dwMifengRegionTwoEndAngle, info.dwMifengSubRegionOuterOffset, info.dwMifengSubRegionInnerOffset);
            pictureBox2.Image = bitmapShow[1];
        }

        private void txt329_TextChanged(object sender, EventArgs e)
        {
            info.dwMifengRegionTwoEndAngle = Convert.ToUInt32(txt329.Text);
            if (graghicsShow[0] == null)
            {
                return;
            }
            graghicsShow[1].DrawImage(bitmapOrignal[1], 0, 0);
            drawOnePictureArea(1, info.dwMifengRegionTwoStartAngle, info.dwMifengRegionTwoEndAngle, info.dwMifengSubRegionOuterOffset, info.dwMifengSubRegionInnerOffset);
            pictureBox2.Image = bitmapShow[1];
        }

        private void txt330_TextChanged(object sender, EventArgs e)
        {
            info.dwMifengRegionThreeStartAngle = Convert.ToUInt32(txt330.Text);
            if (graghicsShow[0] == null)
            {
                return;
            }
            graghicsShow[2].DrawImage(bitmapOrignal[2], 0, 0);
            drawOnePictureArea(2, info.dwMifengRegionThreeStartAngle, info.dwMifengRegionThreeEndAngle, info.dwMifengSubRegionOuterOffset, info.dwMifengSubRegionInnerOffset);
            pictureBox3.Image = bitmapShow[2];
        }

        private void txt331_TextChanged(object sender, EventArgs e)
        {
            info.dwMifengRegionThreeEndAngle = Convert.ToUInt32(txt331.Text);
            if (graghicsShow[0] == null)
            {
                return;
            }
            graghicsShow[2].DrawImage(bitmapOrignal[2], 0, 0);
            drawOnePictureArea(2, info.dwMifengRegionThreeStartAngle, info.dwMifengRegionThreeEndAngle, info.dwMifengSubRegionOuterOffset, info.dwMifengSubRegionInnerOffset);
            pictureBox3.Image = bitmapShow[2];
        }

        private void txt332_TextChanged(object sender, EventArgs e)
        { 
            info.dwMifengRegionFourStartAngle = Convert.ToUInt32(txt332.Text);
            if (graghicsShow[0] == null)
            {
                return;
            }
            graghicsShow[3].DrawImage(bitmapOrignal[3], 0, 0);
            drawOnePictureArea(3, info.dwMifengRegionFourStartAngle, info.dwMifengRegionFourEndAngle, info.dwMifengSubRegionOuterOffset, info.dwMifengSubRegionInnerOffset);
            pictureBox4.Image = bitmapShow[3];
        }

        private void txt333_TextChanged(object sender, EventArgs e)
        {
            info.dwMifengRegionFourEndAngle = Convert.ToUInt32(txt333.Text);
            if (graghicsShow[0] == null)
            {
                return;
            }
            graghicsShow[3].DrawImage(bitmapOrignal[3], 0, 0);
            drawOnePictureArea(3, info.dwMifengRegionFourStartAngle, info.dwMifengRegionFourEndAngle, info.dwMifengSubRegionOuterOffset, info.dwMifengSubRegionInnerOffset);
            pictureBox4.Image = bitmapShow[3];
        }


        private void btnd1_Click(object sender, EventArgs e)
        {
            info.dwMifengRegionOneStartAngle--;
            txt326.Text = info.dwMifengRegionOneStartAngle.ToString();
        }

        private void btna1_Click(object sender, EventArgs e)
        {
            info.dwMifengRegionOneStartAngle++;
            txt326.Text = info.dwMifengRegionOneStartAngle.ToString();
        }

        private void btnd2_Click(object sender, EventArgs e)
        {
            info.dwMifengRegionOneEndAngle--;
            txt327.Text = info.dwMifengRegionOneEndAngle.ToString();
        }

        private void btna2_Click(object sender, EventArgs e)
        {
            info.dwMifengRegionOneEndAngle++;
            txt327.Text = info.dwMifengRegionOneEndAngle.ToString();
        }

        private void btnd3_Click(object sender, EventArgs e)
        {
            info.dwMifengRegionTwoStartAngle--;
            txt328.Text = info.dwMifengRegionTwoStartAngle.ToString();
        }

        private void btna3_Click(object sender, EventArgs e)
        {
            info.dwMifengRegionTwoStartAngle++;
            txt328.Text = info.dwMifengRegionTwoStartAngle.ToString();
        }

        private void btnd4_Click(object sender, EventArgs e)
        {
            info.dwMifengRegionTwoEndAngle--;
            txt329.Text = info.dwMifengRegionTwoEndAngle.ToString();
        }

        private void btna4_Click(object sender, EventArgs e)
        {
            info.dwMifengRegionTwoEndAngle++;
            txt329.Text = info.dwMifengRegionTwoEndAngle.ToString();
        }

        private void btnd5_Click(object sender, EventArgs e)
        {
            info.dwMifengRegionThreeStartAngle--;
            txt330.Text = info.dwMifengRegionThreeStartAngle.ToString();
        }

        private void btna5_Click(object sender, EventArgs e)
        {
            info.dwMifengRegionThreeStartAngle++;
            txt330.Text = info.dwMifengRegionThreeStartAngle.ToString();
        }

        private void btnd6_Click(object sender, EventArgs e)
        {
            info.dwMifengRegionThreeEndAngle--;
            txt331.Text = info.dwMifengRegionThreeEndAngle.ToString();
        }

        private void btna6_Click(object sender, EventArgs e)
        {
            info.dwMifengRegionThreeEndAngle++;
            txt331.Text = info.dwMifengRegionThreeEndAngle.ToString();
        }

        private void btnd7_Click(object sender, EventArgs e)
        {
            info.dwMifengRegionFourStartAngle--;
            txt332.Text = info.dwMifengRegionFourStartAngle.ToString();
        }

        private void btna7_Click(object sender, EventArgs e)
        {
            info.dwMifengRegionFourStartAngle++;
            txt332.Text = info.dwMifengRegionFourStartAngle.ToString();
        }

        private void btnd8_Click(object sender, EventArgs e)
        {
            info.dwMifengRegionFourEndAngle--;
            txt333.Text = info.dwMifengRegionFourEndAngle.ToString();
        }

        private void btna8_Click(object sender, EventArgs e)
        {
            info.dwMifengRegionFourEndAngle++;
            txt333.Text = info.dwMifengRegionFourEndAngle.ToString();
        }
        #endregion
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (sideAB == 0)
            {
                GlobalVariable.configSetting.sideAParam.dwMifengSubRegionOuterOffset = info.dwMifengSubRegionOuterOffset;
                GlobalVariable.configSetting.sideAParam.dwMifengSubRegionInnerOffset = info.dwMifengSubRegionInnerOffset;
                GlobalVariable.configSetting.sideAParam.dwMifengRegionOneStartAngle = info.dwMifengRegionOneStartAngle;
                GlobalVariable.configSetting.sideAParam.dwMifengRegionOneEndAngle = info.dwMifengRegionOneEndAngle;
                GlobalVariable.configSetting.sideAParam.dwMifengRegionTwoStartAngle = info.dwMifengRegionTwoStartAngle;
                GlobalVariable.configSetting.sideAParam.dwMifengRegionTwoEndAngle = info.dwMifengRegionTwoEndAngle;
                GlobalVariable.configSetting.sideAParam.dwMifengRegionThreeStartAngle = info.dwMifengRegionThreeStartAngle;
                GlobalVariable.configSetting.sideAParam.dwMifengRegionThreeEndAngle = info.dwMifengRegionThreeEndAngle;
                GlobalVariable.configSetting.sideAParam.dwMifengRegionFourStartAngle = info.dwMifengRegionFourStartAngle;
                GlobalVariable.configSetting.sideAParam.dwMifengRegionFourEndAngle = info.dwMifengRegionFourEndAngle;
                GlobalVariable.bParamFreshForAutoFlow[0] = true;
                GlobalVariable.bParamFreshForManual[0] = true;
            }
            else
            {
                GlobalVariable.configSetting.sideBParam.dwMifengSubRegionOuterOffset = info.dwMifengSubRegionOuterOffset;
                GlobalVariable.configSetting.sideBParam.dwMifengSubRegionInnerOffset = info.dwMifengSubRegionInnerOffset;
                GlobalVariable.configSetting.sideBParam.dwMifengRegionOneStartAngle = info.dwMifengRegionOneStartAngle;
                GlobalVariable.configSetting.sideBParam.dwMifengRegionOneEndAngle = info.dwMifengRegionOneEndAngle;
                GlobalVariable.configSetting.sideBParam.dwMifengRegionTwoStartAngle = info.dwMifengRegionTwoStartAngle;
                GlobalVariable.configSetting.sideBParam.dwMifengRegionTwoEndAngle = info.dwMifengRegionTwoEndAngle;
                GlobalVariable.configSetting.sideBParam.dwMifengRegionThreeStartAngle = info.dwMifengRegionThreeStartAngle;
                GlobalVariable.configSetting.sideBParam.dwMifengRegionThreeEndAngle = info.dwMifengRegionThreeEndAngle;
                GlobalVariable.configSetting.sideBParam.dwMifengRegionFourStartAngle = info.dwMifengRegionFourStartAngle;
                GlobalVariable.configSetting.sideBParam.dwMifengRegionFourEndAngle = info.dwMifengRegionFourEndAngle;
                GlobalVariable.bParamFreshForAutoFlow[1] = true;
                GlobalVariable.bParamFreshForManual[1] = true;
            }
            string ErrorMsg = "";
            bool bRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json", ref ErrorMsg);
            if (bRtn)
            {
                for (int i = 0; i < GlobalVariable.bParamFreshForAutoFlow.Length; i++)
                {
                    GlobalVariable.bParamFreshForAutoFlow[i] = true;
                    GlobalVariable.bParamFreshForManual[i] = true;
                }
                GlobalMethod.ShowMessage("保存成功");
            }
            else
            {
                GlobalMethod.ShowMessage("保存失败，异常信息为" + ErrorMsg);
            }
        }

        float banjing1;
        float banjing2;
        void drawOnePictureArea(int index, UInt32 startAngle, UInt32 endAngle, int outJing, int innerJing)
        {
            if (sideAB == 0)
            {
                graghicsShow[index].DrawArc(pen, (float)(GlobalVariable.configSetting.SideAModel.fInnerLoopMinX - banjing1 / 2 - outJing), (float)(GlobalVariable.configSetting.SideAModel.fInnerLoopMinY - banjing1 / 2 - outJing), (float)banjing1 - outJing, (float)banjing1 - outJing, startAngle, endAngle);
                graghicsShow[index].DrawArc(pen, (float)(GlobalVariable.configSetting.SideAModel.fInnerLoopMinX - banjing2 / 2 + innerJing), (float)(GlobalVariable.configSetting.SideAModel.fInnerLoopMinY - banjing2 / 2 + innerJing), (float)banjing2 + innerJing, (float)banjing2 + innerJing, startAngle, endAngle);
                PointF start1 = new PointF();
                start1.X = GlobalVariable.configSetting.SideAModel.fInnerLoopMinX + (float)Math.Cos(startAngle) * (banjing1 - outJing);
                start1.Y = GlobalVariable.configSetting.SideAModel.fInnerLoopMinY + (float)Math.Sin(startAngle) * (banjing1 - outJing);
                PointF start2 = new PointF();
                start2.X = GlobalVariable.configSetting.SideAModel.fInnerLoopMinX + (float)Math.Cos(startAngle) * (banjing2 + innerJing);
                start2.Y = GlobalVariable.configSetting.SideAModel.fInnerLoopMinY + (float)Math.Sin(startAngle) * (banjing2 + innerJing);
                graghicsShow[index].DrawLine(pen, start1, start2);

                PointF end1 = new PointF();
                end1.X = GlobalVariable.configSetting.SideAModel.fInnerLoopMinX + (float)Math.Cos(endAngle) * (banjing1 - outJing);
                end1.Y = GlobalVariable.configSetting.SideAModel.fInnerLoopMinY + (float)Math.Sin(endAngle) * (banjing1 - outJing);
                PointF end2 = new PointF();
                end2.X = GlobalVariable.configSetting.SideAModel.fInnerLoopMinX + (float)Math.Cos(endAngle) * (banjing2 + innerJing);
                end2.Y = GlobalVariable.configSetting.SideAModel.fInnerLoopMinY + (float)Math.Sin(endAngle) * (banjing2 + innerJing);
                graghicsShow[index].DrawLine(pen, end1, end2);

                graghicsShow[index].DrawArc(pen, (float)(GlobalVariable.configSetting.SideAModel.fInnerLoopMinX - banjing1 / 2 - outJing), (float)(GlobalVariable.configSetting.SideAModel.fInnerLoopMinY - banjing1 / 2 - outJing), (float)banjing1 - outJing, (float)banjing1 - outJing, startAngle + 180, endAngle + 180);
                graghicsShow[index].DrawArc(pen, (float)(GlobalVariable.configSetting.SideAModel.fInnerLoopMinX - banjing2 / 2 + innerJing), (float)(GlobalVariable.configSetting.SideAModel.fInnerLoopMinY - banjing2 / 2 + innerJing), (float)banjing2 + innerJing, (float)banjing2 + innerJing, startAngle + 180, endAngle + 180);
                start1.X = GlobalVariable.configSetting.SideAModel.fInnerLoopMinX + (float)Math.Cos(startAngle + 180) * (banjing1 - outJing);
                start1.Y = GlobalVariable.configSetting.SideAModel.fInnerLoopMinY + (float)Math.Sin(startAngle + 180) * (banjing1 - outJing);
                start2.X = GlobalVariable.configSetting.SideAModel.fInnerLoopMinX + (float)Math.Cos(startAngle + 180) * (banjing2 + innerJing);
                start2.Y = GlobalVariable.configSetting.SideAModel.fInnerLoopMinY + (float)Math.Sin(startAngle + 180) * (banjing2 + innerJing);
                graghicsShow[index].DrawLine(pen, start1, start2);
                
                end1.X = GlobalVariable.configSetting.SideAModel.fInnerLoopMinX + (float)Math.Cos(endAngle + 180) * (banjing1 - outJing);
                end1.Y = GlobalVariable.configSetting.SideAModel.fInnerLoopMinY + (float)Math.Sin(endAngle + 180) * (banjing1 - outJing);
                end2.X = GlobalVariable.configSetting.SideAModel.fInnerLoopMinX + (float)Math.Cos(endAngle + 180) * (banjing2 + innerJing);
                end2.Y = GlobalVariable.configSetting.SideAModel.fInnerLoopMinY + (float)Math.Sin(endAngle + 180) * (banjing2 + innerJing);
                graghicsShow[index].DrawLine(pen, end1, end2);
            }
            else
            {
                graghicsShow[index].DrawArc(pen, (float)(GlobalVariable.configSetting.SideBModel.fInnerLoopMinX - banjing1 / 2 - outJing), (float)(GlobalVariable.configSetting.SideBModel.fInnerLoopMinY - banjing1 / 2 - outJing), (float)banjing1 - outJing, (float)banjing1 - outJing, startAngle, endAngle);
                graghicsShow[index].DrawArc(pen, (float)(GlobalVariable.configSetting.SideBModel.fInnerLoopMinX - banjing2 / 2 + innerJing), (float)(GlobalVariable.configSetting.SideBModel.fInnerLoopMinY - banjing2 / 2 + innerJing), (float)banjing2 + innerJing, (float)banjing2 + innerJing, startAngle, endAngle);
                PointF start1 = new PointF();
                start1.X = GlobalVariable.configSetting.SideBModel.fInnerLoopMinX + (float)Math.Cos(startAngle) * (banjing1 - outJing);
                start1.Y = GlobalVariable.configSetting.SideBModel.fInnerLoopMinY + (float)Math.Sin(startAngle) * (banjing1 - outJing);
                PointF start2 = new PointF();
                start2.X = GlobalVariable.configSetting.SideBModel.fInnerLoopMinX + (float)Math.Cos(startAngle) * banjing2;
                start2.Y = GlobalVariable.configSetting.SideBModel.fInnerLoopMinY + (float)Math.Sin(startAngle) * banjing2;
                graghicsShow[index].DrawLine(pen, start1, start2);

                PointF end1 = new PointF();
                end1.X = GlobalVariable.configSetting.SideBModel.fInnerLoopMinX + (float)Math.Cos(endAngle) * (banjing1 - outJing);
                end1.Y = GlobalVariable.configSetting.SideBModel.fInnerLoopMinY + (float)Math.Sin(endAngle) * (banjing1 - outJing);
                PointF end2 = new PointF();
                end2.X = GlobalVariable.configSetting.SideBModel.fInnerLoopMinX + (float)Math.Cos(endAngle) * (banjing2 + innerJing);
                end2.Y = GlobalVariable.configSetting.SideBModel.fInnerLoopMinY + (float)Math.Sin(endAngle) * (banjing2 + innerJing);
                graghicsShow[index].DrawLine(pen, end1, end2);

                graghicsShow[index].DrawArc(pen, (float)(GlobalVariable.configSetting.SideBModel.fInnerLoopMinX - banjing1 / 2 - outJing), (float)(GlobalVariable.configSetting.SideBModel.fInnerLoopMinY - banjing1 / 2 - outJing), (float)banjing1 - outJing, (float)banjing1 - outJing, startAngle + 180, endAngle + 180);
                graghicsShow[index].DrawArc(pen, (float)(GlobalVariable.configSetting.SideBModel.fInnerLoopMinX - banjing2 / 2 + innerJing), (float)(GlobalVariable.configSetting.SideBModel.fInnerLoopMinY - banjing2 / 2 + innerJing), (float)banjing2 + innerJing, (float)banjing2 + innerJing, startAngle + 180, endAngle + 180);
                start1.X = GlobalVariable.configSetting.SideBModel.fInnerLoopMinX + (float)Math.Cos(startAngle + 180) * (banjing1 - outJing);
                start1.Y = GlobalVariable.configSetting.SideBModel.fInnerLoopMinY + (float)Math.Sin(startAngle + 180) * (banjing1 - outJing);
                start2.X = GlobalVariable.configSetting.SideBModel.fInnerLoopMinX + (float)Math.Cos(startAngle + 180) * banjing2;
                start2.Y = GlobalVariable.configSetting.SideBModel.fInnerLoopMinY + (float)Math.Sin(startAngle + 180) * banjing2;
                graghicsShow[index].DrawLine(pen, start1, start2);
                
                end1.X = GlobalVariable.configSetting.SideBModel.fInnerLoopMinX + (float)Math.Cos(endAngle + 180) * (banjing1 - outJing);
                end1.Y = GlobalVariable.configSetting.SideBModel.fInnerLoopMinY + (float)Math.Sin(endAngle + 180) * (banjing1 - outJing);
                end2.X = GlobalVariable.configSetting.SideBModel.fInnerLoopMinX + (float)Math.Cos(endAngle + 180) * (banjing2 + innerJing);
                end2.Y = GlobalVariable.configSetting.SideBModel.fInnerLoopMinY + (float)Math.Sin(endAngle + 180) * (banjing2 + innerJing);
                graghicsShow[index].DrawLine(pen, end1, end2);
            }
        }

        void drawAllArea()
        {
            if (graghicsShow[0] == null)
            {
                return;
            }

            graghicsShow[0].DrawImage(bitmapOrignal[0], 0, 0);
            drawOnePictureArea(0,info.dwMifengRegionOneStartAngle, info.dwMifengRegionOneEndAngle, info.dwMifengSubRegionOuterOffset, info.dwMifengSubRegionInnerOffset);
            pictureBox1.Image = bitmapShow[0];

            graghicsShow[1].DrawImage(bitmapOrignal[1], 0, 0);
            drawOnePictureArea(1, info.dwMifengRegionTwoStartAngle, info.dwMifengRegionTwoEndAngle, info.dwMifengSubRegionOuterOffset, info.dwMifengSubRegionInnerOffset);
            pictureBox2.Image = bitmapShow[1];

            graghicsShow[2].DrawImage(bitmapOrignal[2], 0, 0);
            drawOnePictureArea(2, info.dwMifengRegionThreeStartAngle, info.dwMifengRegionThreeEndAngle, info.dwMifengSubRegionOuterOffset, info.dwMifengSubRegionInnerOffset);
            pictureBox3.Image = bitmapShow[2];

            graghicsShow[3].DrawImage(bitmapOrignal[3], 0, 0);
            drawOnePictureArea(3, info.dwMifengRegionFourStartAngle, info.dwMifengRegionFourEndAngle, info.dwMifengSubRegionOuterOffset, info.dwMifengSubRegionInnerOffset);
            pictureBox4.Image = bitmapShow[3];

        }
    }
}
