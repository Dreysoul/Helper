using BearingInspection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class SideOutZhuJieMian : UserControl
    {
        private Outer_Extra extraInfo;
        private OuterSide_Model_Info modelInfo;
        private OuterSide_Result_Info resultInfo;
        private bool m_editEnable;
        private Bitmap[] allPicture = new Bitmap[GlobalVariable.iWaiYuanAPictureNumber];
        private Bitmap bitmapPhoto;
        private Bitmap bitmapCopy;
        private Bitmap bitmapShow;
        private Graphics graphicsShow;
        private string errorMsg = "";
        private Pen pen = new Pen(Color.Lime, 4);
        private Pen pen2 = new Pen(Color.Red, 4);
        private string path = FilePath.ParamSettingPath + GlobalVariable.configname + "\\WaiYuanAModel.bmp";
       // private string bkpath = FilePath.ParamSettingPath + "ParamBackup" + "\\WaiYuanAModel.bmp";
        public SideOutZhuJieMian()
        {
            m_editEnable = false;
            InitializeComponent();
            //modelInfo = GlobalVariable.configSetting.waiYuanAModel;
            modelInfo = GlobalVariable.TmpConfigSetting.waiYuanAModel;
            extraInfo = GlobalVariable.TmpConfigSetting.SideOutExtra;
            //extraInfo = GlobalVariable.configSetting.SideOutExtra;
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
            Bitmap bitmapaaa = (Bitmap)Image.FromFile(path);
            bitmapShow = new Bitmap(bitmapaaa);
            pictureBox1.Image = bitmapShow;
            GetScaleNumber(bitmapShow.Width, bitmapShow.Height);
        }

        private void LoadParam()
        {
            txt1.Text = modelInfo.dwCircleRegionLeftX.ToString();
            txt2.Text = modelInfo.dwCircleRegionTopY.ToString();
            txt3.Text = modelInfo.dwCircleRegionRightX.ToString();
            txt4.Text = modelInfo.dwCircleRegionDownY.ToString();
            txt5.Text = modelInfo.dwValidRegionWidth.ToString();
            txt6.Text = modelInfo.dwValidRegionHeight.ToString();
            txt7.Text = modelInfo.dwValidRegionMeanGray.ToString();
            txt8.Text = modelInfo.dwLowLightValidRegionMeanGray.ToString();
            txt9.Text = modelInfo.dwUpRoundRegionHeight.ToString();
            txt10.Text = modelInfo.dwUpRoundRegionMeanGray.ToString();
            txt11.Text = modelInfo.dwDownRoundRegionHeight.ToString();
            txt12.Text = modelInfo.dwDownRoundRegionMeanGray.ToString();

            txtLeftX.Text = extraInfo.LeftX.ToString();
            txTopY.Text = extraInfo.TopY.ToString();
            txtRightX.Text = extraInfo.RightX.ToString();
            txtDownY.Text = extraInfo.DownY.ToString();
            if (graphicsShow != null)
            {
                DrawModelArea();
            }
        }

        private void btnTakePicture_Click(object sender, EventArgs e)
        {
            //打开光源
            GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.WaiYuanA_TXG, true);
            GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.WaiYuanA_TZG, true);
            for (int i = 0; i < GlobalVariable.iWaiYuanAPictureNumber; i++)
            {
                //拍照
                bool bRtn = GlobalMethod.TakePhoto_SideInOut(3, 0, GlobalVariable.configSetting.waiYuanACameraPhotoSetting.exposure[0], ref allPicture[i], ref errorMsg);
                if (i == 0)
                {
                    GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.WaiYuanA_TZG, false);
                }
            }
            //关闭光源
            GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.WaiYuanA_TXG, false);
            GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.WaiYuanA_TZG, false);
            //先复制竖的
            bitmapCopy = new Bitmap(allPicture[0]);
            //旋转成横的
            BitMapHelper.RotateBitmap(ref bitmapCopy);

            bitmapShow = new Bitmap(bitmapCopy);
            graphicsShow = Graphics.FromImage(bitmapShow);
            graphicsShow.DrawImage(bitmapCopy, 0, 0);
            //额外参数的框
            graphicsShow.DrawRectangle(pen2, extraInfo.LeftX, extraInfo.TopY, extraInfo.RightX - extraInfo.LeftX, extraInfo.DownY - extraInfo.TopY);
            pictureBox1.Image = bitmapShow;

            bool btest = false;
            if (btest)
            {
                string ErrorMsg = "";
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                //打开的文件选择对话框上的标题
                saveFileDialog.Title = "请选择文件";
                //设置文件类型
                saveFileDialog.Filter = "所有文件(*.*)|*.*";
                //按下确定选择的按钮
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //获得文件路径
                    string localFilePath = saveFileDialog.FileName.ToString();
                    bool bRtn = GlobalMethod.SavePicture(bitmapPhoto, localFilePath, ref ErrorMsg);
                    if (bRtn)
                    {
                        GlobalMethod.ShowMessage("导出图片成功");
                    }
                    else
                    {
                        GlobalMethod.ShowMessage("导出图片失败，错误信息为" + ErrorMsg);
                    }
                }
            }
        }

        private void btnGetModel_Click(object sender, EventArgs e)
        {
            saveROI();

            GlobalVariable.WaiYuanAProcess.setROI(extraInfo.LeftX, extraInfo.TopY, extraInfo.RightX, extraInfo.DownY);
            for (int i = 0; i < GlobalVariable.iWaiYuanAPictureNumber; i++)
            {
                int ngcode = GlobalVariable.WaiYuanAProcess.Sample(allPicture[i], i);
                if (ngcode != (int)NGCode.OK)
                {
                    GlobalMethod.ShowMessage("建立模型失败，错误原因是" + GlobalVariable._dicErrorInfo[ngcode] + "，请先调整其他的调整项后重新建立模版");
                    return;
                }
            }

            resultInfo = GlobalVariable.WaiYuanAProcess.getResultInfo();
            modelInfo.dwCircleRegionLeftX = (UInt32)resultInfo.fCircleRegionLeftX;
            modelInfo.dwCircleRegionTopY = (UInt32)resultInfo.fCircleRegionTopY;
            modelInfo.dwCircleRegionRightX = (UInt32)resultInfo.fCircleRegionRightX;
            modelInfo.dwCircleRegionDownY = (UInt32)resultInfo.fCircleRegionDownY;
            modelInfo.dwValidRegionWidth = resultInfo.dwValidRegionWidth;
            modelInfo.dwValidRegionHeight = resultInfo.dwValidRegionHeight;
            modelInfo.dwValidRegionMeanGray = (UInt32)resultInfo.fValidRegionMeanGray;
            modelInfo.dwLowLightValidRegionMeanGray = (UInt32)resultInfo.fValidRegionLowLightMeanGray;
            modelInfo.dwUpRoundRegionHeight = (UInt32)resultInfo.dwUpRoundRegionHeight;
            modelInfo.dwUpRoundRegionMeanGray = (UInt32)resultInfo.fUpRoundRegionMeanGray;
            modelInfo.dwDownRoundRegionHeight = (UInt32)resultInfo.dwDownRoundRegionHeight;
            modelInfo.dwDownRoundRegionMeanGray = (UInt32)resultInfo.fDownRoundRegionMeanGray;
            LoadParam();
        }
        private void refreshModel()
        {
            modelInfo.dwCircleRegionLeftX = Convert.ToUInt32(txt1.Text);
            modelInfo.dwCircleRegionTopY = Convert.ToUInt32(txt2.Text);
            modelInfo.dwCircleRegionRightX = Convert.ToUInt32(txt3.Text);
            modelInfo.dwCircleRegionDownY = Convert.ToUInt32(txt4.Text);
            modelInfo.dwValidRegionWidth = Convert.ToUInt32(txt5.Text);
            modelInfo.dwValidRegionHeight = Convert.ToUInt32(txt6.Text);
            modelInfo.dwValidRegionMeanGray = Convert.ToUInt32(txt7.Text);
            modelInfo.dwLowLightValidRegionMeanGray = Convert.ToUInt32(txt8.Text);
            modelInfo.dwUpRoundRegionHeight = Convert.ToUInt32(txt9.Text);
            modelInfo.dwUpRoundRegionMeanGray = Convert.ToUInt32(txt10.Text);
            modelInfo.dwDownRoundRegionHeight = Convert.ToUInt32(txt11.Text);
            modelInfo.dwDownRoundRegionMeanGray = Convert.ToUInt32(txt12.Text);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            refreshModel();
            GlobalVariable.TmpConfigSetting.waiYuanAModel = modelInfo;
            GlobalVariable.configSetting.waiYuanAModel = modelInfo;
            string ErrorMsg = "";
           
            string mainPath = FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json";
            string backupPath = FilePath.ParamSettingPath + "ParamBackup\\Param.Json";

            // 1. 写入主配置文件
            bool mRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, mainPath, ref ErrorMsg);

            if (mRtn)
            {
                // 2. 写入备份配置文件（同样使用 WriteJsonFile）
                bool backupRtn = JsonHelper.WriteJsonFile(GlobalVariable.TmpConfigSetting, backupPath, ref ErrorMsg);
                bitmapShow.Save(path, ImageFormat.Bmp);
                //bitmapShow.Save(bkpath, ImageFormat.Bmp);
                GlobalVariable.resetParam();
                if (backupRtn)
                    GlobalMethod.ShowMessage("保存成功");
                else
                    GlobalMethod.ShowMessage("保存成功，但备份失败：" + ErrorMsg);
            }
            else
            {
                GlobalMethod.ShowMessage("保存失败，异常信息为：" + ErrorMsg);
            }

        }

        private void DrawModelArea()
        {
            if (graphicsShow == null)
            {
                return;
            }
            graphicsShow.DrawImage(bitmapCopy, 0, 0);
            //额外参数的框
            graphicsShow.DrawRectangle(pen2, extraInfo.LeftX, extraInfo.TopY, extraInfo.RightX - extraInfo.LeftX, extraInfo.DownY - extraInfo.TopY);
            //模板的框
            graphicsShow.DrawRectangle(pen, modelInfo.dwCircleRegionLeftX, modelInfo.dwCircleRegionTopY, modelInfo.dwValidRegionWidth, modelInfo.dwValidRegionHeight);
            graphicsShow.DrawRectangle(pen, modelInfo.dwCircleRegionLeftX, modelInfo.dwCircleRegionTopY - modelInfo.dwUpRoundRegionHeight, modelInfo.dwValidRegionWidth, modelInfo.dwUpRoundRegionHeight);
            graphicsShow.DrawRectangle(pen, modelInfo.dwCircleRegionLeftX, modelInfo.dwCircleRegionTopY + modelInfo.dwValidRegionHeight, modelInfo.dwValidRegionWidth, modelInfo.dwDownRoundRegionHeight);
            pictureBox1.Image = bitmapShow;
        }

        private void saveROI()
        {
            extraInfo.LeftX = int.Parse(txtLeftX.Text);
            extraInfo.TopY = int.Parse(txTopY.Text);
            extraInfo.RightX = int.Parse(txtRightX.Text);
            extraInfo.DownY = int.Parse(txtDownY.Text);
        }
 
        private void btnSaveSetting_Click(object sender, EventArgs e)
        {
            saveROI();
            GlobalVariable.configSetting.SideOutExtra = extraInfo;
            GlobalVariable.TmpConfigSetting.SideOutExtra   = extraInfo;
            string ErrorMsg = "";

            string mainPath = FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json";
            string backupPath = FilePath.ParamSettingPath + "ParamBackup\\Param.Json";

            // 1. 写入主配置文件
            bool mRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, mainPath, ref ErrorMsg);

            if (mRtn)
            {
                // 2. 写入备份配置文件
                bool backupRtn = JsonHelper.WriteJsonFile(GlobalVariable.TmpConfigSetting, backupPath, ref ErrorMsg);

                GlobalVariable.resetParam();
            }
            else
            {
                GlobalMethod.ShowMessage("保存失败，异常信息为：" + ErrorMsg);
            }

        }

        #region 事先画图

        private void txtLeftX_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = extraInfo.LeftX.ToString();
                return;
            }
            extraInfo.LeftX = i;
            DrawModelArea();
        }

        private void txTopY_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = extraInfo.TopY.ToString();
                return;
            }
            extraInfo.TopY = i;
            DrawModelArea();
        }

        private void txtRightX_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = extraInfo.RightX.ToString();
                return;
            }
            extraInfo.RightX = i;
            DrawModelArea();
        }

        private void txtDownY_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = extraInfo.DownY.ToString();
                return;
            }
            extraInfo.DownY = i;
            DrawModelArea();
        }

        private void btnD1_Click(object sender, EventArgs e)
        {
            extraInfo.LeftX--;
            txtLeftX.Text = extraInfo.LeftX.ToString();
        }

        private void btnA1_Click(object sender, EventArgs e)
        {
            extraInfo.LeftX++;
            txtLeftX.Text = extraInfo.LeftX.ToString();
        }

        private void btnD2_Click(object sender, EventArgs e)
        {
            extraInfo.TopY--;
            txTopY.Text = extraInfo.TopY.ToString();
        }

        private void btnA2_Click(object sender, EventArgs e)
        {
            extraInfo.TopY++;
            txTopY.Text = extraInfo.TopY.ToString();
        }

        private void btnD3_Click(object sender, EventArgs e)
        {
            extraInfo.RightX--;
            txtRightX.Text = extraInfo.RightX.ToString();
        }

        private void btnA3_Click(object sender, EventArgs e)
        {
            extraInfo.RightX++;
            txtRightX.Text = extraInfo.RightX.ToString();
        }

        private void btnD4_Click(object sender, EventArgs e)
        {
            extraInfo.DownY--;
            txtDownY.Text = extraInfo.DownY.ToString();
        }

        private void btnA4_Click(object sender, EventArgs e)
        {
            extraInfo.DownY++;
            txtDownY.Text = extraInfo.DownY.ToString();
        }

        #endregion 事先画图

        private double ScaleNumberX = 0.0;
        private double ScaleNumberY = 0.0;

        public void GetScaleNumber(int x, int y)
        {
            ScaleNumberY = y * 1.0 / pictureBox1.Height;
            ScaleNumberX = x * 1.0 / pictureBox1.Width;
        }

        private void pictureBox1_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                return;
            }
            Bitmap bitmap = (Bitmap)pictureBox1.Image;

            int x = e.X;
            int y = e.Y;
            x = (int)(x * ScaleNumberX);
            y = (int)(y * ScaleNumberY);
            int ch = BitMapHelper.getPixelDot(x, y, bitmap);
            lblGray.Text = "灰度：" + ch.ToString() + ";X = " + x.ToString() + ";Y = " + y.ToString();
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            m_editEnable = !m_editEnable;
            txt1.Enabled = m_editEnable;
            txt2.Enabled = m_editEnable;
            txt3.Enabled = m_editEnable;
            txt4.Enabled = m_editEnable;
            txt5.Enabled = m_editEnable;
            txt6.Enabled = m_editEnable;
            txt7.Enabled = m_editEnable;
            txt8.Enabled = m_editEnable;
            txt9.Enabled = m_editEnable;
            txt10.Enabled = m_editEnable;
            txt11.Enabled = m_editEnable;
            txt12.Enabled = m_editEnable;
        }
    }
}