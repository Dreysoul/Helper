using BearingInspection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class MultiZhuJieMian : UserControl
    {
        private MultiImg_Model_Info modelInfo;
        private MultiImg_Param_Info paramInfo;
        private MultiImg_Flag flagInfo;
        private Bitmap bitmapPhoto;
        private Bitmap bitmapCopy;
        private Bitmap bitmapShow;
        private Graphics graphicsShow;
        private bool bUpdate = false;
        private string errorMsg = "";
        private Pen pen = new Pen(Color.Lime, 1);
        private Pen pen2 = new Pen(Color.Red, 1);
        private int m_iType = 0;
        private string path = FilePath.ParamSettingPath + GlobalVariable.configname + "\\WaiYuanAModel.bmp";

        public MultiZhuJieMian(int iType)
        {
            InitializeComponent();
            m_iType = iType;
            cmbRectangle.SelectedIndex = 0;

            MultiImg_Model_Info[] modelInfos = { GlobalVariable.configSetting.daoJiaoAModel, GlobalVariable.configSetting.neiKongModel, GlobalVariable.configSetting.waiYuanBModel, GlobalVariable.configSetting.daoJiaoBModel };
            modelInfo = modelInfos[iType];
            MultiImg_Param_Info[] paramInfos = { GlobalVariable.configSetting.daoJiaoAParam, GlobalVariable.configSetting.neiKongParam, GlobalVariable.configSetting.waiYuanBParam, GlobalVariable.configSetting.daoJiaoBParam };
            paramInfo = paramInfos[iType];
            MultiImg_Flag[] flagInfos = { GlobalVariable.configSetting.daoJiaoAFlag, GlobalVariable.configSetting.neiKongFlag, GlobalVariable.configSetting.waiYuanBFlag, GlobalVariable.configSetting.daoJiaoBFlag };
            flagInfo = flagInfos[iType];

            string[] picName = { "DaoJiaoAModel.bmp", "NeiKongModel.bmp", "WaiYuanBModel.bmp", "DaoJiaoBModel.bmp" };
            path = FilePath.ParamSettingPath + GlobalVariable.configname + "\\" + picName[iType];

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
            for (int i = 0; i < bitmapShow.Width; i++)
            {
                for (int j = 0; j < bitmapShow.Height; j++)
                {
                    bitmapShow.SetPixel(i, j, Color.Black);
                }
            }
            pictureBox1.Image = bitmapShow;
            GetScaleNumber(bitmapShow.Width, bitmapShow.Height);

            cmbRectangle.Items.Clear();
            cmbRectangle.Items.Add("定位框");
            for (int i = 0; i < Global.multiImgDetectNum; i++)
            {
                cmbRectangle.Items.Add("检测框" + (i + 1).ToString());
            }
        }

        private void LoadParam()
        {
            bUpdate = true;
            int index = cmbRectangle.SelectedIndex;
            if (index == 0)
            {
                nud_c1.Value = modelInfo.col1;
                nud_r1.Value = modelInfo.row1;
                nud_c2.Value = modelInfo.col2;
                nud_r2.Value = modelInfo.row2;
                ckbEnable.Checked = paramInfo.positionEnable;
            }
            else if (index > 0 && index < BearingInspection.Global.multiImgDetectNum + 1)
            {
                nud_c1.Value = paramInfo.rect[index - 1].col1;
                nud_r1.Value = paramInfo.rect[index - 1].row1;
                nud_c2.Value = paramInfo.rect[index - 1].col2;
                nud_r2.Value = paramInfo.rect[index - 1].row2;
                nud_hd_min.Value = paramInfo.rect[index - 1].minGray;
                nud_hd_max.Value = paramInfo.rect[index - 1].maxGray;
                nud_mj_min.Value = paramInfo.rect[index - 1].minArea;
                nud_mj_max.Value = paramInfo.rect[index - 1].maxArea;
                ckbEnable.Checked = paramInfo.rect[index - 1].enable;
            }

            switch (m_iType)
            {
                case 0:
                    txtPZNum.Text = GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.photoNumber.ToString();
                    txtPZSpan.Text = GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.photoSpan.ToString();
                    break;

                case 1:
                    txtPZNum.Text = GlobalVariable.configSetting.neiKongCameraPhotoSetting.photoNumber.ToString();
                    txtPZSpan.Text = GlobalVariable.configSetting.neiKongCameraPhotoSetting.photoSpan.ToString();
                    break;

                case 2:
                    txtPZNum.Text = GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.photoNumber.ToString();
                    txtPZSpan.Text = GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.photoSpan.ToString();
                    break;

                case 3:
                    txtPZNum.Text = GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.photoNumber.ToString();
                    txtPZSpan.Text = GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.photoSpan.ToString();
                    break;

                default:
                    break;
            }

            bUpdate = false;
            if (graphicsShow != null)
            {
                DrawModelArea();
            }
        }

        private void btnTakePicture_Click(object sender, EventArgs e)
        {
            IAreaScanCameraHelper[] cameraHelper = { GlobalVariable.DaoJiaoACameraHelper, GlobalVariable.NeiKongCameraHelper, GlobalVariable.WaiYuanBCameraHelper, GlobalVariable.DaoJiaoBCameraHelper };
            double[] exposure = { GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.exposure[0],
                                  GlobalVariable.configSetting.neiKongCameraPhotoSetting.exposure[0],
                                  GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.exposure[0],
                                  GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.exposure[0]};
            CameraPhotoSetting[] photoSetting = { GlobalVariable.configSetting.daoJiaoACameraPhotoSetting,
                                                  GlobalVariable.configSetting.neiKongCameraPhotoSetting,
                                                  GlobalVariable.configSetting.waiYuanBCameraPhotoSetting,
                                                  GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting};

            string ErrorMsg = "";

            cameraHelper[m_iType].SetCameraROI(photoSetting[m_iType].ROIWidthTrue, photoSetting[m_iType].ROIHeihgtTrue, photoSetting[m_iType].OffsetX, photoSetting[m_iType].OffsetY);
            cameraHelper[m_iType].SetCameraExposureTime(exposure[m_iType], ref ErrorMsg);
            cameraHelper[m_iType].Start();
            Thread.Sleep(1);

            //打开光源
            btnLight_Click(null, null);
            bool bRtn = cameraHelper[m_iType].TakeCameraImage(ref bitmapPhoto, ref ErrorMsg);
            //关闭光源
            btnLightOff_Click(null, null);

            if (!bRtn)
            {
                GlobalMethod.ShowMessage("拍照失败");
                return;
            }
            //先复制竖的
            bitmapCopy = new Bitmap(bitmapPhoto);
            //旋转成横的
            //BitMapHelper.RotateBitmap(ref bitmapCopy);

            bitmapShow = new Bitmap(bitmapCopy);
            graphicsShow = Graphics.FromImage(bitmapShow);
            graphicsShow.DrawImage(bitmapCopy, 0, 0);
            DrawModelArea();
            pictureBox1.Image = bitmapShow;
            GetScaleNumber(bitmapCopy.Width, bitmapCopy.Height);

            bool btest = false;
            if (btest)
            {
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
                    bRtn = GlobalMethod.SavePicture(bitmapPhoto, localFilePath, ref ErrorMsg);
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

        private void DrawModelArea()
        {
            if (graphicsShow == null)
            {
                return;
            }

            graphicsShow.DrawImage(bitmapCopy, 0, 0);
            //模板的框
            if (paramInfo.positionEnable)
                graphicsShow.DrawRectangle(new Pen(Color.Lime, 1), modelInfo.col1, modelInfo.row1, modelInfo.col2 - modelInfo.col1, modelInfo.row2 - modelInfo.row1);

            for (int i = 0; i < Global.multiImgDetectNum; i++)
            {
                if (paramInfo.rect[i].enable)
                {
                    graphicsShow.DrawString((i + 1).ToString(), new Font("宋体", 10), new SolidBrush(Color.Yellow), new PointF(paramInfo.rect[i].col1 - 10, paramInfo.rect[i].row1 - 10));
                    graphicsShow.DrawRectangle(new Pen(Color.Lime, 1), paramInfo.rect[i].col1, paramInfo.rect[i].row1, paramInfo.rect[i].col2 - paramInfo.rect[i].col1, paramInfo.rect[i].row2 - paramInfo.rect[i].row1);
                }
            }

            int index = cmbRectangle.SelectedIndex;
            if (index == 0)
            {
                if (paramInfo.positionEnable)
                    graphicsShow.DrawRectangle(new Pen(Color.Red, 1), modelInfo.col1, modelInfo.row1, modelInfo.col2 - modelInfo.col1, modelInfo.row2 - modelInfo.row1);
            }
            else if (index > 0 && index < Global.multiImgDetectNum + 1)
            {
                if (paramInfo.rect[index - 1].enable)
                    graphicsShow.DrawRectangle(new Pen(Color.Red, 1), paramInfo.rect[index - 1].col1, paramInfo.rect[index - 1].row1, paramInfo.rect[index - 1].col2 - paramInfo.rect[index - 1].col1, paramInfo.rect[index - 1].row2 - paramInfo.rect[index - 1].row1);
            }
            pictureBox1.Image = bitmapShow;
        }

        private bool SaveData()
        {
            try
            {
                short.Parse(txtPZNum.Text);
                short.Parse(txtPZSpan.Text);
            }
            catch
            {
                GlobalMethod.ShowMessage("拍照数量和拍照间隔数字有误，请重新输入");
                return false;
            }

            if (bUpdate)
                return false;

            int index = cmbRectangle.SelectedIndex;
            if (index == 0)
            {
                modelInfo.col1 = (int)(nud_c1.Value);
                modelInfo.row1 = (int)(nud_r1.Value);
                modelInfo.col2 = (int)(nud_c2.Value);
                modelInfo.row2 = (int)(nud_r2.Value);
                paramInfo.positionEnable = ckbEnable.Checked;
            }
            else if (index > 0 && index < Global.multiImgDetectNum + 1)
            {
                paramInfo.rect[index - 1].col1 = (int)(nud_c1.Value);
                paramInfo.rect[index - 1].row1 = (int)(nud_r1.Value);
                paramInfo.rect[index - 1].col2 = (int)(nud_c2.Value);
                paramInfo.rect[index - 1].row2 = (int)(nud_r2.Value);
                paramInfo.rect[index - 1].minGray = (int)nud_hd_min.Value;
                paramInfo.rect[index - 1].maxGray = (int)nud_hd_max.Value;
                paramInfo.rect[index - 1].minArea = (int)nud_mj_min.Value;
                paramInfo.rect[index - 1].maxArea = (int)nud_mj_max.Value;
                paramInfo.rect[index - 1].enable = ckbEnable.Checked;
            }

            switch (m_iType)
            {
                case 0:
                    GlobalVariable.configSetting.daoJiaoAModel = modelInfo;
                    GlobalVariable.configSetting.daoJiaoAParam = paramInfo;
                    GlobalVariable.configSetting.daoJiaoAFlag = flagInfo;
                    GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.photoNumber = short.Parse(this.txtPZNum.Text);
                    GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.photoSpan = short.Parse(this.txtPZSpan.Text);

                    GlobalVariable.TmpConfigSetting.daoJiaoAModel = modelInfo;
                    GlobalVariable.TmpConfigSetting.daoJiaoAParam = paramInfo;
                    GlobalVariable.TmpConfigSetting.daoJiaoAFlag = flagInfo;
                    GlobalVariable.TmpConfigSetting.daoJiaoACameraPhotoSetting.photoNumber = short.Parse(this.txtPZNum.Text);
                    GlobalVariable.TmpConfigSetting.daoJiaoACameraPhotoSetting.photoSpan = short.Parse(this.txtPZSpan.Text);
                    break;

                case 1:
                    GlobalVariable.configSetting.neiKongModel = modelInfo;
                    GlobalVariable.configSetting.neiKongParam = paramInfo;
                    GlobalVariable.configSetting.neiKongFlag = flagInfo;
                    GlobalVariable.configSetting.neiKongCameraPhotoSetting.photoNumber = short.Parse(this.txtPZNum.Text);
                    GlobalVariable.configSetting.neiKongCameraPhotoSetting.photoSpan = short.Parse(this.txtPZSpan.Text);

                    GlobalVariable.TmpConfigSetting.neiKongModel = modelInfo;
                    GlobalVariable.TmpConfigSetting.neiKongParam = paramInfo;
                    GlobalVariable.TmpConfigSetting.neiKongFlag = flagInfo;
                    GlobalVariable.TmpConfigSetting.neiKongCameraPhotoSetting.photoNumber = short.Parse(this.txtPZNum.Text);
                    GlobalVariable.TmpConfigSetting.neiKongCameraPhotoSetting.photoSpan = short.Parse(this.txtPZSpan.Text);
                    break;

                case 2:
                    GlobalVariable.configSetting.waiYuanBModel = modelInfo;
                    GlobalVariable.configSetting.waiYuanBParam = paramInfo;
                    GlobalVariable.configSetting.waiYuanBFlag = flagInfo;
                    GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.photoNumber = short.Parse(this.txtPZNum.Text);
                    GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.photoSpan = short.Parse(this.txtPZSpan.Text);

                    GlobalVariable.TmpConfigSetting.waiYuanBModel = modelInfo;
                    GlobalVariable.TmpConfigSetting.waiYuanBParam = paramInfo;
                    GlobalVariable.TmpConfigSetting.waiYuanBFlag = flagInfo;
                    GlobalVariable.TmpConfigSetting.waiYuanBCameraPhotoSetting.photoNumber = short.Parse(this.txtPZNum.Text);
                    GlobalVariable.TmpConfigSetting.waiYuanBCameraPhotoSetting.photoSpan = short.Parse(this.txtPZSpan.Text);
                    break;

                case 3:
                    GlobalVariable.configSetting.daoJiaoBModel = modelInfo;
                    GlobalVariable.configSetting.daoJiaoBParam = paramInfo;
                    GlobalVariable.configSetting.daoJiaoBFlag = flagInfo;
                    GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.photoNumber = short.Parse(this.txtPZNum.Text);
                    GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.photoSpan = short.Parse(this.txtPZSpan.Text);

                    GlobalVariable.TmpConfigSetting.daoJiaoBModel = modelInfo;
                    GlobalVariable.TmpConfigSetting.daoJiaoBParam = paramInfo;
                    GlobalVariable.TmpConfigSetting.daoJiaoBFlag = flagInfo;
                    GlobalVariable.TmpConfigSetting.daoJiaoBCameraPhotoSetting.photoNumber = short.Parse(this.txtPZNum.Text);
                    GlobalVariable.TmpConfigSetting.daoJiaoBCameraPhotoSetting.photoSpan = short.Parse(this.txtPZSpan.Text);
                    break;

                default:
                    break;
            }

            string ErrorMsg = "";
            string mainPath = FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json";
            string backupPath = FilePath.ParamSettingPath + "ParamBackup\\Param.Json";

            // 1. 写入主配置文件
            bool mRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, mainPath, ref ErrorMsg);

            if (mRtn)
            {
                // 2. 写入备份配置文件（同样使用 WriteJsonFile）
                bool backupRtn = JsonHelper.WriteJsonFile(GlobalVariable.TmpConfigSetting, backupPath, ref ErrorMsg);

                GlobalVariable.resetParam();
            }
            else
            {
                GlobalMethod.ShowMessage("保存失败，异常信息为：" + ErrorMsg);
            }

            return true;
        }

        private void btnSaveSetting_Click(object sender, EventArgs e)
        {
              bool bRtn = SaveData();
            if (bRtn)
            {
                GlobalMethod.ShowMessage("保存成功");
            }
            else
            {
                GlobalMethod.ShowMessage("保存失败");
            }
        }

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

        private void nud_c1_ValueChanged(object sender, EventArgs e)
        {
            SaveData();
            DrawModelArea();
        }

        private void nud_r1_ValueChanged(object sender, EventArgs e)
        {
            SaveData();
            DrawModelArea();
        }

        private void nud_c2_ValueChanged(object sender, EventArgs e)
        {
            SaveData();
            DrawModelArea();
        }

        private void nud_r2_ValueChanged(object sender, EventArgs e)
        {
            SaveData();
            DrawModelArea();
        }

        private void cmbRectangle_SelectedIndexChanged(object sender, EventArgs e)
        {
            nud_hd_min.Enabled = false;
            nud_hd_max.Enabled = false;
            nud_mj_min.Enabled = false;
            nud_mj_max.Enabled = false;

            if (cmbRectangle.SelectedIndex != 0)
            {
                nud_hd_min.Enabled = true;
                nud_hd_max.Enabled = true;
                nud_mj_min.Enabled = true;
                nud_mj_max.Enabled = true;
            }
            LoadParam();
        }

        private void ckbEnable_CheckedChanged(object sender, EventArgs e)
        {
            SaveData();
            DrawModelArea();
        }

        private void btnLight_Click(object sender, EventArgs e)
        {
            switch (m_iType)
            {
                case 0:
                    GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DaoJiaoA_TXG, true);
                    break;

                case 1:
                    GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiKong_BG, true);
                    break;

                case 2:
                    GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.WaiYuanB_TXG, true);
                    break;

                case 3:
                    GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DaoJiaoB_TXG, true);
                    break;

                default:
                    break;
            }
        }

        private void btnLightOff_Click(object sender, EventArgs e)
        {
            switch (m_iType)
            {
                case 0:
                    GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DaoJiaoA_TXG, false);
                    break;

                case 1:
                    GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiKong_BG, false);
                    break;

                case 2:
                    GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.WaiYuanB_TXG, false);
                    break;

                case 3:
                    GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DaoJiaoB_TXG, false);
                    break;

                default:
                    break;
            }
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (graphicsShow == null)
                return;

            List<Error_Area> defectList = new List<Error_Area>();    //缺陷列表
            PointF[] errorPoint = new PointF[5];
            SolidBrush brushString = new SolidBrush(Color.Blue);
            Font wordFont = new Font("宋体", 15);

            switch (m_iType)
            {
                case 0:
                    {
                        GlobalVariable.DaoJiaoAProcess.setModelInfo(GlobalVariable.configSetting.daoJiaoAModel);
                        GlobalVariable.DaoJiaoAProcess.setParamInfo(GlobalVariable.configSetting.daoJiaoAParam);
                        GlobalVariable.DaoJiaoAProcess.setFlag(GlobalVariable.configSetting.daoJiaoAFlag);
                        int errorCode = GlobalVariable.DaoJiaoAProcess.Process(bitmapPhoto, 0);
                        if (errorCode != (int)ResultCMD.OK)
                            defectList = GlobalVariable.DaoJiaoAProcess.getDefectsList(errorCode);
                    }
                    break;

                case 1:
                    {
                        GlobalVariable.NeiKongProcess.setModelInfo(GlobalVariable.configSetting.neiKongModel);
                        GlobalVariable.NeiKongProcess.setParamInfo(GlobalVariable.configSetting.neiKongParam);
                        GlobalVariable.NeiKongProcess.setFlag(GlobalVariable.configSetting.neiKongFlag);
                        int errorCode = GlobalVariable.NeiKongProcess.Process(bitmapPhoto, 0);
                        if (errorCode != (int)ResultCMD.OK)
                            defectList = GlobalVariable.NeiKongProcess.getDefectsList(errorCode);
                    }
                    break;

                case 2:
                    {
                        GlobalVariable.WaiYuanBProcess.setModelInfo(GlobalVariable.configSetting.waiYuanBModel);
                        GlobalVariable.WaiYuanBProcess.setParamInfo(GlobalVariable.configSetting.waiYuanBParam);
                        GlobalVariable.WaiYuanBProcess.setFlag(GlobalVariable.configSetting.waiYuanBFlag);
                        int errorCode = GlobalVariable.WaiYuanBProcess.Process(bitmapPhoto, 0);
                        if (errorCode != (int)ResultCMD.OK)
                            defectList = GlobalVariable.WaiYuanBProcess.getDefectsList(errorCode);
                    }
                    break;

                case 3:
                    {
                        GlobalVariable.DaoJiaoBProcess.setModelInfo(GlobalVariable.configSetting.daoJiaoBModel);
                        GlobalVariable.DaoJiaoBProcess.setParamInfo(GlobalVariable.configSetting.daoJiaoBParam);
                        GlobalVariable.DaoJiaoBProcess.setFlag(GlobalVariable.configSetting.daoJiaoBFlag);
                        int errorCode = GlobalVariable.DaoJiaoBProcess.Process(bitmapPhoto, 0);
                        if (errorCode != (int)ResultCMD.OK)
                            defectList = GlobalVariable.DaoJiaoBProcess.getDefectsList(errorCode);
                    }
                    break;

                default:
                    break;
            }

            if (defectList.Count > 0)
            {
                pen.Color = Color.Red;
                //画红色圆圈，标注错误
                for (int i = 0; i < defectList.Count; i++)
                {
                    errorPoint[0].Y = defectList[i].row1;
                    errorPoint[0].X = defectList[i].col1;
                    errorPoint[1].Y = defectList[i].row2;
                    errorPoint[1].X = defectList[i].col2;
                    errorPoint[2].Y = defectList[i].row3;
                    errorPoint[2].X = defectList[i].col3;
                    errorPoint[3].Y = defectList[i].row4;
                    errorPoint[3].X = defectList[i].col4;
                    errorPoint[4].X = errorPoint[0].X;
                    errorPoint[4].Y = errorPoint[0].Y;
                    graphicsShow.DrawLines(pen, errorPoint);

                    graphicsShow.DrawString(defectList[i].area.ToString(), wordFont, brushString, errorPoint[0]);
                }
            }
            //graphicsShow.DrawRectangle(pen, 10, 10, 200, 200);
            pictureBox1.Image = bitmapShow;
        }

        private void btnDoAll_Click(object sender, EventArgs e)
        {
            btnTakePicture_Click(null, null);
            btnCalculate_Click(null, null);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        }
    }
}