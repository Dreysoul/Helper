using BearingInspection;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class SideABZhuJieMian : UserControl
    {
        //端面AB共一个模板设置界面，0端面A， 1端面B
        public int m_duanMianAB = 0;
        public Surface_Param_Info m_paramAB;
        private Surface_Model_Info m_modelInfo;
        private Surface_Result_Info m_resultInfo;
        private int waijing_YaRu = 0;
        private int neijing_YaRu = 0;
        private int waijing_JuanBian = 0;
        private int neijing_JuanBian = 0;
        private Surface_Param_Info paramInfo;
        private bool m_editEnable;
        private string errorMsg = "";
        private Bitmap[] allPicture = new Bitmap[GlobalVariable.iDuanMianAPictureNumber];
        private Bitmap bitmapShow_Main;
        private Graphics graphicsShow_Main;
        private Bitmap bitmapShow_YaRu;
        private Graphics graphicsShow_YaRu;
        private Bitmap bitmapShow_JuanBian;
        private Graphics graphicsShow_JuanBian;
        private Bitmap bitmapShow_WenZi;
        private Graphics graphicsShow_WenZi;
        private Bitmap[] bitmapShow_XiangXian = new Bitmap[4];
        private Graphics[] graphicsShow_XiangXian = new Graphics[4];
        private string path_Main = FilePath.ParamSettingPath + GlobalVariable.configname + "\\SideAModel_Main.bmp";
        private string path_YaRu = FilePath.ParamSettingPath + GlobalVariable.configname + "\\SideAModel_YaRu.bmp";
        private string path_JuanBian = FilePath.ParamSettingPath + GlobalVariable.configname + "\\SideAModel_JuanBian.bmp";
        private string path_WenZi = FilePath.ParamSettingPath + GlobalVariable.configname + "\\SideAModel_WenZi.bmp";
        private string[] path_XiangXian = new string[4];
        private Pen pen = new Pen(Color.Lime, 4);
        private OwnPicBox pic_box = new OwnPicBox();
        private int iCurHXIndex = -1;
        private int iFindCircleIndex = -1;
        private float m_referenceRadius = 0;
        public SideABZhuJieMian(int side)
        {
            m_duanMianAB = side;
            InitializeComponent();
            m_editEnable = false;
            m_modelInfo.modelAngle = new double[4];
            m_modelInfo.modelCol = new double[4];
            m_modelInfo.modelRow = new double[4];
            string stringAB = "A";
            if(m_duanMianAB == 0)
            {
                m_paramAB = GlobalVariable.configSetting.duanMianAParam;
                m_modelInfo = GlobalVariable.configSetting.duanMianAModel;
                stringAB = "A";
            }
            else
            {
                m_paramAB = GlobalVariable.configSetting.duanMianBParam;
                m_modelInfo = GlobalVariable.configSetting.duanMianBModel;
                stringAB = "B";
            }

            //
            waijing_YaRu = m_paramAB.dwMifengYaRuOuterOffset;
            neijing_YaRu = m_paramAB.dwMifengYaRuInnerOffset;
            waijing_JuanBian = m_paramAB.dwMifengJuanSideRegionOuterOffset;
            neijing_JuanBian = m_paramAB.dwMifengJuanSideRegionInnerOffset;
            paramInfo = m_paramAB;
            path_Main = FilePath.ParamSettingPath + GlobalVariable.configname + "\\duanMian"+stringAB+"Model_Main.bmp";
            path_YaRu = FilePath.ParamSettingPath + GlobalVariable.configname + "\\duanMian" + stringAB + "Model_YaRu.bmp";
            path_JuanBian = FilePath.ParamSettingPath + GlobalVariable.configname + "\\duanMian" + stringAB + "Model_JuanBian.bmp";
            path_WenZi = FilePath.ParamSettingPath + GlobalVariable.configname + "\\duanMian" + stringAB + "Model_WenZi.bmp";
            for (int i = 0; i < 4; i++)
            {
                path_XiangXian[i] = FilePath.ParamSettingPath + GlobalVariable.configname + "\\duanMian" + stringAB + "Model_Xiangxian" + i + ".bmp";
            }
           
            initComb_HXJC();
            LoadParam();
            LoadParam_Main();
            if (graphicsShow_Main != null)
            {
                DrawYuan_Main();
            }
            LoadParam_YaRu();
            LoadParam_JuanBian();
            LoadParam_WenZi();
            LoadParam_XiangXinaGuang();

            Bitmap bitmapaaa = (Bitmap)Image.FromFile(path_Main);
            bitmapShow_Main = new Bitmap(bitmapaaa);
            pictureBoxMain.Image = bitmapShow_Main;
            GetScaleNumber_Main(bitmapShow_Main.Width, bitmapShow_Main.Height);

            bitmapaaa = (Bitmap)Image.FromFile(path_YaRu);
            bitmapShow_YaRu = new Bitmap(bitmapaaa);
            pictureBox_YaRu.Image = bitmapShow_YaRu;

            bitmapaaa = (Bitmap)Image.FromFile(path_JuanBian);
            bitmapShow_JuanBian = new Bitmap(bitmapaaa);
            pictureBox_JuanBian.Image = bitmapShow_JuanBian;

            bitmapaaa = (Bitmap)Image.FromFile(path_WenZi);
            bitmapShow_WenZi = new Bitmap(bitmapaaa);
            pictureBox_WenZi.Image = bitmapShow_WenZi;

            bitmapaaa = (Bitmap)Image.FromFile(path_XiangXian[0]);
            bitmapShow_XiangXian[0] = new Bitmap(bitmapaaa);
            pictureBox_XiangXian0.Image = bitmapShow_XiangXian[0];

            bitmapaaa = (Bitmap)Image.FromFile(path_XiangXian[1]);
            bitmapShow_XiangXian[1] = new Bitmap(bitmapaaa);
            pictureBox_XiangXian1.Image = bitmapShow_XiangXian[1];

            bitmapaaa = (Bitmap)Image.FromFile(path_XiangXian[2]);
            bitmapShow_XiangXian[2] = new Bitmap(bitmapaaa);
            pictureBox_XiangXian2.Image = bitmapShow_XiangXian[2];

            bitmapaaa = (Bitmap)Image.FromFile(path_XiangXian[3]);
            bitmapShow_XiangXian[3] = new Bitmap(bitmapaaa);
            pictureBox_XiangXian3.Image = bitmapShow_XiangXian[3];

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

        private void refreshModel()
        {
            m_modelInfo.fOuterLoopMaxRadius = Convert.ToInt32(txt1.Text);
            m_modelInfo.fOuterLoopMinRadius = Convert.ToInt32(txt2.Text);
            m_modelInfo.fOuterLoopMidRadius = Convert.ToInt32(txt3.Text);
            m_modelInfo.fOuterLoopRound = Convert.ToInt32(txt4.Text);
            m_modelInfo.fOuterLoopCenter = (float)Convert.ToDouble(txt5.Text);
            m_modelInfo.fOuterLoopHighLightMeanGray = Convert.ToInt32(txt7.Text);
            m_modelInfo.fOuterLoopLowLightMeanGray = Convert.ToInt32(txt8.Text);

            m_modelInfo.fMifengMaxRadius = Convert.ToInt32(txt21.Text);
            m_modelInfo.fMifengMinRadius = Convert.ToInt32(txt22.Text);
            m_modelInfo.fMifengMaxValidRadius = Convert.ToInt32(txt23.Text);
            m_modelInfo.fMifengMinValidRadius = Convert.ToInt32(txt24.Text);
            m_modelInfo.fMifengCenter = (float)Convert.ToDouble(txt25.Text);
            m_modelInfo.fMifengHighLightMeanGray = Convert.ToInt32(txt27.Text);
            m_modelInfo.fMifengMeanGray = Convert.ToInt32(txt28.Text);
            m_modelInfo.fMifengValidRegionMeanGray = Convert.ToInt32(txt29.Text);
            m_modelInfo.fMifengYaRuDistance = (float)Convert.ToDouble(txt41.Text);
            m_modelInfo.nMifengJuanSideNum = Convert.ToInt32(txt42.Text);
            m_modelInfo.nMifengJuanSideArea = Convert.ToInt32(txt43.Text);

            m_modelInfo.fInnerLoopMaxRadius = Convert.ToInt32(txt51.Text);
            m_modelInfo.fInnerLoopMinRadius = Convert.ToInt32(txt52.Text);
            m_modelInfo.fInnerLoopMidRadius = Convert.ToInt32(txt53.Text);
            m_modelInfo.fInnerLoopRound = Convert.ToInt32(txt54.Text);
            m_modelInfo.fInnerLoopCenter = (float)Convert.ToDouble(txt55.Text);
            m_modelInfo.fInnerLoopHighLightMeanGray = Convert.ToInt32(txt57.Text);
            m_modelInfo.fInnerLoopLowLightMeanGray = Convert.ToInt32(txt58.Text);

            m_modelInfo.nInnerLoopTextGroupNum = Convert.ToUInt32(txt71.Text);
            m_modelInfo.nInnerLoopAllTextNum = Convert.ToUInt32(txt72.Text);
            m_modelInfo.nInnerLoopTextMinWidth = Convert.ToUInt32(txt73.Text);
            m_modelInfo.nInnerLoopTextMaxWidth = Convert.ToUInt32(txt74.Text);
            m_modelInfo.nInnerLoopTextMinHeight = Convert.ToUInt32(txt75.Text);
            m_modelInfo.nInnerLoopTextMaxHeight = Convert.ToUInt32(txt76.Text);

            m_modelInfo.nOuterLoopTextGroupNum = Convert.ToUInt32(txt77.Text);
            m_modelInfo.nOuterLoopAllTextNum = Convert.ToUInt32(txt78.Text);
            m_modelInfo.nOuterLoopTextMinWidth = Convert.ToUInt32(txt79.Text);
            m_modelInfo.nOuterLoopTextMaxWidth = Convert.ToUInt32(txt80.Text);
            m_modelInfo.nOuterLoopTextMinHeight = Convert.ToUInt32(txt81.Text);
            m_modelInfo.nOuterLoopTextMaxHeight = Convert.ToUInt32(txt82.Text);

            m_modelInfo.nMifengTextGroupNum = Convert.ToUInt32(txt83.Text);
            m_modelInfo.nMifengAllTextNum = Convert.ToUInt32(txt84.Text);
            m_modelInfo.nMifengTextMinWidth = Convert.ToUInt32(txt85.Text);
            m_modelInfo.nMifengTextMaxWidth = Convert.ToUInt32(txt86.Text);
            m_modelInfo.nMifengTextMinHeight = Convert.ToUInt32(txt87.Text);
            m_modelInfo.nMifengTextMaxHeight = Convert.ToUInt32(txt88.Text);

            m_modelInfo.dwBallNum = Convert.ToInt32(txt91.Text);
            m_modelInfo.dwDingNum = Convert.ToInt32(txt92.Text);
        }

        #region 主界面相关

        private void LoadParam()
        {
            if (m_duanMianAB == 0)
            {
                cmb19.SelectedIndex = GlobalVariable.configSetting.duanMianAParam.dwOuterLoopText;
                cmb200.SelectedIndex = GlobalVariable.configSetting.duanMianAParam.nWorkPieceType;
                if (GlobalVariable.configSetting.duanMianAParam.bMifengText)
                {
                    cmb211.SelectedIndex = 1;
                }
                else
                {
                    cmb211.SelectedIndex = 0;
                }
                cmb266.SelectedIndex = GlobalVariable.configSetting.duanMianAParam.dwMifengTextMode;
                cmb419.SelectedIndex = GlobalVariable.configSetting.duanMianAParam.dwInnerLoopText;
            }
            else
            {
                cmb19.SelectedIndex = GlobalVariable.configSetting.duanMianBParam.dwOuterLoopText;
                cmb200.SelectedIndex = GlobalVariable.configSetting.duanMianBParam.nWorkPieceType;
                if (GlobalVariable.configSetting.duanMianBParam.bMifengText)
                {
                    cmb211.SelectedIndex = 1;
                }
                else
                {
                    cmb211.SelectedIndex = 0;
                }
                cmb266.SelectedIndex = GlobalVariable.configSetting.duanMianBParam.dwMifengTextMode;
                cmb419.SelectedIndex = GlobalVariable.configSetting.duanMianBParam.dwInnerLoopText;
            }
        }

        private void LoadParam_Main()
        {
            txt1.Text = m_modelInfo.fOuterLoopMaxRadius.ToString();
            txt2.Text = m_modelInfo.fOuterLoopMinRadius.ToString();
            txt3.Text = m_modelInfo.fOuterLoopMidRadius.ToString();
            txt4.Text = m_modelInfo.fOuterLoopRound.ToString();
            txt5.Text = m_modelInfo.fOuterLoopCenter.ToString("f4");
            txt7.Text = m_modelInfo.fOuterLoopHighLightMeanGray.ToString();
            txt8.Text = m_modelInfo.fOuterLoopLowLightMeanGray.ToString();

            txt21.Text = m_modelInfo.fMifengMaxRadius.ToString();
            txt22.Text = m_modelInfo.fMifengMinRadius.ToString();
            txt23.Text = m_modelInfo.fMifengMaxValidRadius.ToString();
            txt24.Text = m_modelInfo.fMifengMinValidRadius.ToString();
            txt25.Text = m_modelInfo.fMifengCenter.ToString("f4");
            txt27.Text = m_modelInfo.fMifengHighLightMeanGray.ToString();
            txt28.Text = m_modelInfo.fMifengMeanGray.ToString();
            txt29.Text = m_modelInfo.fMifengValidRegionMeanGray.ToString();
            txt41.Text = m_modelInfo.fMifengYaRuDistance.ToString("f4");
            txt42.Text = m_modelInfo.nMifengJuanSideNum.ToString();
            txt43.Text = m_modelInfo.nMifengJuanSideArea.ToString();

            txt51.Text = m_modelInfo.fInnerLoopMaxRadius.ToString();
            txt52.Text = m_modelInfo.fInnerLoopMinRadius.ToString();
            txt53.Text = m_modelInfo.fInnerLoopMidRadius.ToString();
            txt54.Text = m_modelInfo.fInnerLoopRound.ToString();
            txt55.Text = m_modelInfo.fInnerLoopCenter.ToString("f4");
            txt57.Text = m_modelInfo.fInnerLoopHighLightMeanGray.ToString();
            txt58.Text = m_modelInfo.fInnerLoopLowLightMeanGray.ToString();

            txt71.Text = m_modelInfo.nInnerLoopTextGroupNum.ToString();
            txt72.Text = m_modelInfo.nInnerLoopAllTextNum.ToString();
            txt73.Text = m_modelInfo.nInnerLoopTextMinWidth.ToString();
            txt74.Text = m_modelInfo.nInnerLoopTextMaxWidth.ToString();
            txt75.Text = m_modelInfo.nInnerLoopTextMinHeight.ToString();
            txt76.Text = m_modelInfo.nInnerLoopTextMaxHeight.ToString();

            txt77.Text = m_modelInfo.nOuterLoopTextGroupNum.ToString();
            txt78.Text = m_modelInfo.nOuterLoopAllTextNum.ToString();
            txt79.Text = m_modelInfo.nOuterLoopTextMinWidth.ToString();
            txt80.Text = m_modelInfo.nOuterLoopTextMaxWidth.ToString();
            txt81.Text = m_modelInfo.nOuterLoopTextMinHeight.ToString();
            txt82.Text = m_modelInfo.nOuterLoopTextMaxHeight.ToString();

            txt83.Text = m_modelInfo.nMifengTextGroupNum.ToString();
            txt84.Text = m_modelInfo.nMifengAllTextNum.ToString();
            txt85.Text = m_modelInfo.nMifengTextMinWidth.ToString();
            txt86.Text = m_modelInfo.nMifengTextMaxWidth.ToString();
            txt87.Text = m_modelInfo.nMifengTextMinHeight.ToString();
            txt88.Text = m_modelInfo.nMifengTextMaxHeight.ToString();

            txt91.Text = m_modelInfo.dwBallNum.ToString();
            txt92.Text = m_modelInfo.dwDingNum.ToString();
        }
        private void btn2_Click(object sender, EventArgs e)
        {
            IAreaScanCameraHelper[] cameraHelper = { GlobalVariable.DuanMianACameraHelper, GlobalVariable.DuanMianBCameraHelper };
            cameraHelper[m_duanMianAB].Start();
            for (int i = 0; i < GlobalVariable.iDuanMianAPictureNumber; i++)
            {
                if (m_duanMianAB == 0)
                {
                    if (!GlobalMethod.TakePhoto_SideAB(m_duanMianAB, i, GlobalVariable.configSetting.duanMianACameraPhotoSetting.exposure[i], ref allPicture[i], ref errorMsg))
                        i--;
                }
                else
                {
                    if (!GlobalMethod.TakePhoto_SideAB(m_duanMianAB, i, GlobalVariable.configSetting.duanMianBCameraPhotoSetting.exposure[i], ref allPicture[i], ref errorMsg))
                        i--;
                }
            }
            cameraHelper[m_duanMianAB].Stop();
            showHXPic();
            zoomPic();
            if (m_duanMianAB == 0)
            {
                m_resultInfo = GlobalVariable.DuanMianAProcess.getResultInfo();
            }
            else
            {
                m_resultInfo = GlobalVariable.DuanMianBProcess.getResultInfo();
            }
            refreshCenterPoint();
        }
        private void btnTakePicture_Click(object sender, EventArgs e)
        {
            IAreaScanCameraHelper[] cameraHelper = { GlobalVariable.DuanMianACameraHelper, GlobalVariable.DuanMianBCameraHelper };
            cameraHelper[m_duanMianAB].Start();
            for (int i = 0; i < GlobalVariable.iDuanMianAPictureNumber; i++)
            {
                //if (i == 9)
                //{
                //    cameraHelper[m_duanMianAB].Stop();
                //    cameraHelper[m_duanMianAB].Start();
                //}
                if (m_duanMianAB == 0)
                {
                    if (!GlobalMethod.TakePhoto_SideAB(m_duanMianAB, i, GlobalVariable.configSetting.duanMianACameraPhotoSetting.exposure[i], ref allPicture[i], ref errorMsg))
                        i--;
                }
                else
                {
                    if (!GlobalMethod.TakePhoto_SideAB(m_duanMianAB, i, GlobalVariable.configSetting.duanMianBCameraPhotoSetting.exposure[i], ref allPicture[i], ref errorMsg))
                        i--;
                }
            }
            //关闭光源
            //GlobalVariable.lightHelper[m_duanMianAB].OperateLight("S00000#S01000#S02000#S03000#S04000#S05000#S06000#");
            cameraHelper[m_duanMianAB].Stop();

            bitmapShow_Main = new Bitmap(allPicture[2]);
            graphicsShow_Main = Graphics.FromImage(bitmapShow_Main);
            graphicsShow_Main.DrawImage(allPicture[2], 0, 0);
            pictureBoxMain.Image = bitmapShow_Main;

            bitmapShow_YaRu = new Bitmap(allPicture[1]);
            graphicsShow_YaRu = Graphics.FromImage(bitmapShow_YaRu);
            graphicsShow_YaRu.DrawImage(allPicture[1], 0, 0);
            pictureBox_YaRu.Image = bitmapShow_YaRu;

            bitmapShow_JuanBian = new Bitmap(allPicture[3]);
            graphicsShow_JuanBian = Graphics.FromImage(bitmapShow_JuanBian);
            graphicsShow_JuanBian.DrawImage(allPicture[3], 0, 0);
            pictureBox_JuanBian.Image = bitmapShow_JuanBian;

            bitmapShow_WenZi = new Bitmap(allPicture[2]);
            graphicsShow_WenZi = Graphics.FromImage(bitmapShow_WenZi);
            graphicsShow_WenZi.DrawImage(allPicture[2], 0, 0);
            pictureBox_WenZi.Image = bitmapShow_WenZi;

            bitmapShow_XiangXian[0] = new Bitmap(allPicture[6]);
            graphicsShow_XiangXian[0] = Graphics.FromImage(bitmapShow_XiangXian[0]);
            graphicsShow_XiangXian[0].DrawImage(allPicture[6], 0, 0);
            pictureBox_XiangXian0.Image = bitmapShow_XiangXian[0];

            bitmapShow_XiangXian[1] = new Bitmap(allPicture[7]);
            graphicsShow_XiangXian[1] = Graphics.FromImage(bitmapShow_XiangXian[1]);
            graphicsShow_XiangXian[1].DrawImage(allPicture[7], 0, 0);
            pictureBox_XiangXian1.Image = bitmapShow_XiangXian[1];

            bitmapShow_XiangXian[2] = new Bitmap(allPicture[8]);
            graphicsShow_XiangXian[2] = Graphics.FromImage(bitmapShow_XiangXian[2]);
            graphicsShow_XiangXian[2].DrawImage(allPicture[8], 0, 0);
            pictureBox_XiangXian2.Image = bitmapShow_XiangXian[2];

            bitmapShow_XiangXian[3] = new Bitmap(allPicture[9]);
            graphicsShow_XiangXian[3] = Graphics.FromImage(bitmapShow_XiangXian[3]);
            graphicsShow_XiangXian[3].DrawImage(allPicture[9], 0, 0);
            pictureBox_XiangXian3.Image = bitmapShow_XiangXian[3];

            showHXPic();
            zoomPic();
            refreshCenterPoint();
            bool btest = true;
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
                    bool bRtn = GlobalMethod.SavePicture(allPicture, localFilePath, ref ErrorMsg);
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
            if (allPicture == null)
            {
                MessageBox.Show("请先拍照再生成模板");
                return;
            }
            int ngcode = 0;
            if (m_duanMianAB == 0)
            {
                GlobalVariable.DuanMianAProcess.setModelInfo(GlobalVariable.configSetting.duanMianAModel);
                GlobalVariable.DuanMianAProcess.setParamInfo(GlobalVariable.configSetting.duanMianAParam);
            }
            else
            {
                GlobalVariable.DuanMianBProcess.setModelInfo(GlobalVariable.configSetting.duanMianBModel);
                GlobalVariable.DuanMianBProcess.setParamInfo(GlobalVariable.configSetting.duanMianBParam);
            }

            bool bSuccess = true;
            for (int i = 0; i < GlobalVariable.iDuanMianAPictureNumber; i++)
            {
                //try
                //{
                if (i == 5)
                {
                    continue;
                }

                if (m_duanMianAB == 0)
                {
                    ngcode = GlobalVariable.DuanMianAProcess.Sample(allPicture[i], i);
                    if (i == 2)
                    {
                        GlobalVariable.DuanMianAProcess.CreateSurfaceTextModel(allPicture[i]);
                    }
                }
                else
                {
                    ngcode = GlobalVariable.DuanMianBProcess.Sample(allPicture[i], i);
                    if (i == 2)
                    {
                        GlobalVariable.DuanMianBProcess.CreateSurfaceTextModel(allPicture[i]);
                    }
                }
                //}
                //catch (Exception)
                // {
                //     ngcode = -1;
                // }
                if (ngcode != (int)NGCode.OK)
                {
                    GlobalMethod.ShowMessage("建立模型失败，是第" + (i + 1) + "张图片，错误原因是" + GlobalVariable._dicErrorInfo[ngcode] + "，请先调整其他的调整项后重新建立模版");
                    //return;
                    bSuccess = false;
                    break;
                }
            }
            if (m_duanMianAB == 0)
            {
                m_resultInfo = GlobalVariable.DuanMianAProcess.getResultInfo();
            }
            else
            {
                m_resultInfo = GlobalVariable.DuanMianBProcess.getResultInfo();
            }
            ChangeStructInfo(m_resultInfo, ref m_modelInfo);
            LoadParam_Main();
            LoadParam_YaRu();
            LoadParam_WenZi();
            LoadParam_JuanBian();
            LoadParam_XiangXinaGuang();
            initCenCtrl(m_resultInfo.fOuterLoopMaxCenterX, m_resultInfo.fOuterLoopMaxCenterY);

            if (bSuccess)
            {
                if (graphicsShow_Main != null)
                {
                    DrawYuan_Main();
                }
                GlobalMethod.ShowMessage("建立模型成功");
            }
        }

        private void ChangeStructInfo(Surface_Result_Info result, ref Surface_Model_Info model)
        {
            // 外圈物理尺寸 - 取样自动计算
            model.fOuterLoopMaxRadius = (int)result.fOuterLoopMaxRadius * 2;                        //ID:1 外圈外直径
            model.fOuterLoopMinRadius = (int)result.fOuterLoopMinRadius * 2;                        //ID:2 外圈内直径
            model.fOuterLoopMidRadius = (int)result.fOuterLoopMaxValidRadius * 2;                          //ID:3 外圈有效直径
            model.fOuterLoopRound = (int)result.fOuterLoopRound;                           //ID:4 外圈倒角大小
            model.fOuterLoopCenter = result.fOuterLoopCenter;                         //ID:5 外圈同心度
            model.fOuterLoopHighLightMeanGray = (int)result.fHighLightOuterLoopMeanGray;                  //ID:7 外圈高亮度平均灰度
            model.fOuterLoopLowLightMeanGray = (int)result.fLowLightOuterLoopMeanGray;                  //ID:8 外圈低亮度平均灰度
            model.fOuterLoopMaxCir = result.fOuterLoopMaxCir;                          //ID:9 外圈圆率
            model.fOuterLoopCenterX = result.fOuterLoopMaxCenterX;              //外圈中心X
            model.fOuterLoopCenterY = result.fOuterLoopMaxCenterY;              //外圈中心Y
            //  密封圈尺寸  - 取样自动计算
            model.fMifengMaxRadius = (int)result.fMifengMaxRadius * 2;                          //ID:21 密封圈外直径
            model.fMifengMinRadius = (int)result.fMifengMinRadius * 2;                            //ID:22 密封圈内直径
            model.fMifengMaxValidRadius = (int)result.fMifengMaxValidRegionRadius * 2;                      //ID:23 密封圈区域内直径(胶盖有效)
            model.fMifengMinValidRadius = (int)result.fMifengMinValidRegionRadius * 2;                       //ID:24 密封圈区域外直径(胶盖有效)
            model.fMifengCenter = result.fMifengCenter;                               //ID:25 密封圈同心度
            model.fMifengHighLightMeanGray = (int)result.fHigLightMifengMeanGray;                     //ID:27 密封圈高亮度平均灰度
            model.fMifengMeanGray = (int)result.fMifengMeanGray;                            //ID:28 密封圈低亮平均亮度

            model.fMifengValidRegionMeanGray = (int)result.fMifengValidRegionMeanGray;                  //ID:29 密封圈超高亮度平均灰度（胶盖有效）

            //
            model.fMifengYaRuDistance = result.fMifengYaRuDistance;                           //ID:41 压入距离(铁盖有效)
            model.nMifengJuanSideNum = result.dwMifengJuanSideCount;                          //ID:42 卷边个数(铁盖有效)
            model.nMifengJuanSideArea = result.dwMifengJuanSideArea;                           //ID:43 卷边单个面积(铁盖有效)

            //    内圈物理尺寸  - 取样自动计算
            model.fInnerLoopMaxRadius = (int)result.fInnerLoopMaxRadius * 2;                        //ID:51 内圈外直径
            model.fInnerLoopMinRadius = (int)result.fInnerLoopMinRadius * 2;                          //ID:52 内圈内直径
            model.fInnerLoopMidRadius = (int)result.fInnerLoopMinValidRadius * 2;                          //ID:53 内圈有效直径
            model.fInnerLoopRound = (int)result.fInnerLoopRound;                            //ID:54 内圈倒角大小
            model.fInnerLoopCenter = (int)result.fInnerLoopCenter;                         //ID:55 内圈同心度
            model.fInnerLoopHighLightMeanGray = (int)result.fHighLightInnerLoopMeanGray;                  //ID:57 内圈高亮度平均灰度
            model.fInnerLoopLowLightMeanGray = (int)result.fLowLightInnerLoopMeanGray;                   //ID:58 内圈低亮度平均灰度
            model.fInnerLoopMinX = (int)result.fInnerLoopMinCenterX;                                //ID:59 内圈中心X
            model.fInnerLoopMinY = (int)result.fInnerLoopMinCenterY;                                //ID:60 内圈中心Y

            //文字信息      - 取样自动计算
            //model.nInnerLoopTextGroupNum =(UInt32)result.fOuterLoopMaxRadius;  //ID:71 内圈文字组数
            //model.nInnerLoopAllTextNum = (UInt32)result.fOuterLoopMaxRadius;              //ID:72 内圈文字个数
            //model.nInnerLoopTextMinWidth = (UInt32)result.fOuterLoopMaxRadius;              //ID:73 内圈文字最小宽度
            //model.nInnerLoopTextMaxWidth = (UInt32)result.fOuterLoopMaxRadius;               //ID:74 内圈文字最大宽度
            //model.nInnerLoopTextMinHeight = (UInt32)result.fOuterLoopMaxRadius;               //ID:75 内圈文字最小高度
            //model.nInnerLoopTextMaxHeight = (UInt32)result.fOuterLoopMaxRadius;               //ID:76 内圈文字最大高度

            //model.nOuterLoopTextGroupNum = (UInt32)result.fOuterLoopMaxRadius;                 //ID:77 外圈文字组数
            //model.nOuterLoopAllTextNum = (UInt32)result.fOuterLoopMaxRadius;                 //ID:78 外圈文字个数
            //model.nOuterLoopTextMinWidth = (UInt32)result.fOuterLoopMaxRadius;                 //ID:79 外圈文字最小宽度
            //model.nOuterLoopTextMaxWidth = (UInt32)result.fOuterLoopMaxRadius;                 //ID:80 外圈文字最大宽度
            //model.nOuterLoopTextMinHeight = (UInt32)result.fOuterLoopMaxRadius;                //ID:81 外圈文字最小高度
            //model.nOuterLoopTextMaxHeight = (UInt32)result.fOuterLoopMaxRadius;                //ID:82 外圈文字最大高度

            model.nMifengTextGroupNum = (UInt32)result.fMifengTextGroupNum;                    //ID:83 密封圈文字组数
            model.nMifengAllTextNum = (UInt32)result.fMifengAllTextNum;                      //ID:84 密封圈文字个数
            model.nMifengTextMinWidth = (UInt32)result.fMifengTextMinWidth;                    //ID:85 密封圈文字最小宽度
            model.nMifengTextMaxWidth = (UInt32)result.fMifengTextMaxWidth;                    //ID:86 密封圈文字最大宽度
            model.nMifengTextMinHeight = (UInt32)result.fMifengTextMinHeigh;                   //ID:87 密封圈文字最小高度
            model.nMifengTextMaxHeight = (UInt32)result.fMifengTextMaxHeigh;                   //ID:88 密封圈文字最大高度
            // 开式轴承  - 取样自动计算
            model.dwBallNum = (int)result.dwBallNum;                               //ID:91 钢球数量(开式)
            model.dwDingNum = (int)result.dwDingNum;                               //ID:92 钉子数量(开式)

            if (model.modelAngle != null)
            {
                for (int i = 0; i < 4; i++)
                {
                    model.modelAngle[i] = result.modelAngle[i];
                    model.modelCol[i] = result.modelCol[i];
                    model.modelRow[i] = result.modelRow[i];
                }
            }
            model.textRegionEnableNum = result.textRegionEnableNum;
            if (model.findCircleRadius == null)
            {
                model.findCircleRadius = new float[Global.findCircleNum];
            }
            for (int i = 0; i < Global.findCircleNum; i++)
            {
                model.findCircleRadius[i] = result.findCircleRadius[i];
            }
        }

        private void btnSaveParam_Click(object sender, EventArgs e)
        {
            if (m_duanMianAB == 0)
            {
                GlobalVariable.configSetting.duanMianAParam.dwOuterLoopText = cmb19.SelectedIndex;
                GlobalVariable.configSetting.duanMianAParam.nWorkPieceType = cmb200.SelectedIndex;
                if (cmb211.SelectedIndex == 1)
                {
                    GlobalVariable.configSetting.duanMianAParam.bMifengText = true;
                }
                else
                {
                    GlobalVariable.configSetting.duanMianAParam.bMifengText = false;
                }
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextMode = cmb266.SelectedIndex;
                GlobalVariable.configSetting.duanMianAParam.dwInnerLoopText = cmb419.SelectedIndex;
                GlobalVariable.DuanMianAProcess.setParamInfo(GlobalVariable.configSetting.duanMianAParam);
            }
            else
            {
                GlobalVariable.configSetting.duanMianBParam.dwOuterLoopText = cmb19.SelectedIndex;
                GlobalVariable.configSetting.duanMianBParam.nWorkPieceType = cmb200.SelectedIndex;
                if (cmb211.SelectedIndex == 1)
                {
                    GlobalVariable.configSetting.duanMianBParam.bMifengText = true;
                }
                else
                {
                    GlobalVariable.configSetting.duanMianBParam.bMifengText = false;
                }
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextMode = cmb266.SelectedIndex;
                GlobalVariable.configSetting.duanMianBParam.dwInnerLoopText = cmb419.SelectedIndex;
                GlobalVariable.DuanMianBProcess.setParamInfo(GlobalVariable.configSetting.duanMianBParam);
            }
            string ErrorMsg = "";
            //bool bRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json", ref ErrorMsg);
            //File.Copy(FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json",
            //             FilePath.ParamSettingPath + "ParamBackup" + "\\Param.Json",
            //             true);
            //if (bRtn)
            //{
            //    GlobalVariable.resetParam();
            //    GlobalMethod.ShowMessage("保存成功");
            //}
            //else
            //{
            //    GlobalMethod.ShowMessage("保存失败，异常信息为" + ErrorMsg);
            //}

            string mainPath = FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json";
            string backupPath = FilePath.ParamSettingPath + "ParamBackup\\Param.Json";

            // 1. 写入主配置文件
            bool mRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, mainPath, ref ErrorMsg);

            if (mRtn)
            {
                // 2. 写入备份配置文件（同样使用 WriteJsonFile）
                bool backupRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, backupPath, ref ErrorMsg);

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

        private void btnSaveAll_Click(object sender, EventArgs e)
        {
            refreshModel();
            if (m_duanMianAB == 0)
            {
                GlobalVariable.configSetting.duanMianAModel = m_modelInfo;
            }
            else
            {
                GlobalVariable.configSetting.duanMianBModel = m_modelInfo;
            }
            saveCtrl_HXJC();

            string ErrorMsg = "";
            //bool bRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json", ref ErrorMsg);
            //File.Copy(FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json",
            //              FilePath.ParamSettingPath + "ParamBackup" + "\\Param.Json",
            //              true);
            //if (bRtn)
            //{
            //    bitmapShow_Main.Save(path_Main, ImageFormat.Bmp);
            //    bitmapShow_YaRu.Save(path_YaRu, ImageFormat.Bmp);
            //    bitmapShow_JuanBian.Save(path_JuanBian, ImageFormat.Bmp);
            //    bitmapShow_WenZi.Save(path_WenZi, ImageFormat.Bmp);
            //    for (int i = 0; i < 4; i++)
            //    {
            //        bitmapShow_XiangXian[i].Save(path_XiangXian[i], ImageFormat.Bmp);
            //    }
            //    GlobalVariable.resetParam();
            //    GlobalMethod.ShowMessage("保存成功");
            //}
            //else
            //{
            //    GlobalMethod.ShowMessage("保存失败，异常信息为" + ErrorMsg);
            //}

            string mainPath = FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json";
            string backupPath = FilePath.ParamSettingPath + "ParamBackup\\Param.Json";

            // 1. 写入主配置文件
            bool mRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, mainPath, ref ErrorMsg);

            if (mRtn)
            {
                // 2. 写入备份配置文件（同样使用 WriteJsonFile）
                bool backupRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, backupPath, ref ErrorMsg);
                bitmapShow_Main.Save(path_Main, ImageFormat.Bmp);
                bitmapShow_YaRu.Save(path_YaRu, ImageFormat.Bmp);
                bitmapShow_JuanBian.Save(path_JuanBian, ImageFormat.Bmp);
                bitmapShow_WenZi.Save(path_WenZi, ImageFormat.Bmp);
                for (int i = 0; i < 4; i++)
                {
                    bitmapShow_XiangXian[i].Save(path_XiangXian[i], ImageFormat.Bmp);
                }
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

        private double ScaleNumberX_Main = 0.0;
        private double ScaleNumberY_Main = 0.0;

        public void GetScaleNumber_Main(int x, int y)
        {
            ScaleNumberY_Main = y * 1.0 / pictureBoxMain.Height;
            ScaleNumberX_Main = x * 1.0 / pictureBoxMain.Width;
        }

        private void pictureBoxMain_MouseMove(object sender, MouseEventArgs e)
        {
            if (pictureBoxMain.Image == null)
            {
                return;
            }
            Bitmap bitmap = (Bitmap)pictureBoxMain.Image;

            int x = e.X;
            int y = e.Y;
            x = (int)(x * ScaleNumberX_Main);
            y = (int)(y * ScaleNumberY_Main);
            int ch = BitMapHelper.getPixelDot(x, y, bitmap);
            lblGray_Main.Text = "灰度：" + ch.ToString() + ";X = " + x.ToString() + ";Y = " + y.ToString();
        }

        private void DrawYuan_Main()
        {
            if (graphicsShow_Main == null)
            {
                return;
            }
            try
            {
                //外圆
                graphicsShow_Main.DrawEllipse(pen, (float)(m_modelInfo.fInnerLoopMinX - m_modelInfo.fOuterLoopMaxRadius / 2), (float)(m_modelInfo.fInnerLoopMinY - m_modelInfo.fOuterLoopMaxRadius / 2), m_modelInfo.fOuterLoopMaxRadius, m_modelInfo.fOuterLoopMaxRadius);
                graphicsShow_Main.DrawEllipse(pen, (float)(m_modelInfo.fInnerLoopMinX - m_modelInfo.fOuterLoopMinRadius / 2), (float)(m_modelInfo.fInnerLoopMinY - m_modelInfo.fOuterLoopMinRadius / 2), m_modelInfo.fOuterLoopMinRadius, m_modelInfo.fOuterLoopMinRadius);
                graphicsShow_Main.DrawEllipse(pen, (float)(m_modelInfo.fInnerLoopMinX - m_modelInfo.fOuterLoopMidRadius / 2), (float)(m_modelInfo.fInnerLoopMinY - m_modelInfo.fOuterLoopMidRadius / 2), m_modelInfo.fOuterLoopMidRadius, m_modelInfo.fOuterLoopMidRadius);
                //密封圈
                graphicsShow_Main.DrawEllipse(pen, (float)(m_modelInfo.fInnerLoopMinX - m_modelInfo.fMifengMaxRadius / 2), (float)(m_modelInfo.fInnerLoopMinY - m_modelInfo.fMifengMaxRadius / 2), m_modelInfo.fMifengMaxRadius, m_modelInfo.fMifengMaxRadius);
                graphicsShow_Main.DrawEllipse(pen, (float)(m_modelInfo.fInnerLoopMinX - m_modelInfo.fMifengMinRadius / 2), (float)(m_modelInfo.fInnerLoopMinY - m_modelInfo.fMifengMinRadius / 2), m_modelInfo.fMifengMinRadius, m_modelInfo.fMifengMinRadius);
                //内圈
                graphicsShow_Main.DrawEllipse(pen, (float)(m_modelInfo.fInnerLoopMinX - m_modelInfo.fInnerLoopMaxRadius / 2), (float)(m_modelInfo.fInnerLoopMinY - m_modelInfo.fInnerLoopMaxRadius / 2), m_modelInfo.fInnerLoopMaxRadius, m_modelInfo.fInnerLoopMaxRadius);
                graphicsShow_Main.DrawEllipse(pen, (float)(m_modelInfo.fInnerLoopMinX - m_modelInfo.fInnerLoopMinRadius / 2), (float)(m_modelInfo.fInnerLoopMinY - m_modelInfo.fInnerLoopMinRadius / 2), m_modelInfo.fInnerLoopMinRadius, m_modelInfo.fInnerLoopMinRadius);
                graphicsShow_Main.DrawEllipse(pen, (float)(m_modelInfo.fInnerLoopMinX - m_modelInfo.fInnerLoopMidRadius / 2), (float)(m_modelInfo.fInnerLoopMinY - m_modelInfo.fInnerLoopMidRadius / 2), m_modelInfo.fInnerLoopMidRadius, m_modelInfo.fInnerLoopMidRadius);
            }
            catch
            {
            }
            pictureBoxMain.Image = bitmapShow_Main;
        }

        #endregion 主界面相关

        #region 压入区域有关

        private void LoadParam_YaRu()
        {
            txt213.Text = waijing_YaRu.ToString();
            txt214.Text = neijing_YaRu.ToString();

            if (graphicsShow_Main != null)
            {
                graphicsShow_YaRu.DrawImage(allPicture[1], 0, 0);
                DrawTwoYuan_YaRu();
                pictureBox_YaRu.Image = bitmapShow_YaRu;
            }
        }

        private void btnD1_YaRu_Click(object sender, EventArgs e)
        {
            neijing_YaRu -= 1;
            txt214.Text = neijing_YaRu.ToString();
        }

        private void btnD2_YaRu_Click(object sender, EventArgs e)
        {
            waijing_YaRu -= 1;
            txt213.Text = waijing_YaRu.ToString();
        }

        private void btnA1_YaRu_Click(object sender, EventArgs e)
        {
            neijing_YaRu += 1;
            txt214.Text = neijing_YaRu.ToString();
        }

        private void btnA2_YaRu_Click(object sender, EventArgs e)
        {
            waijing_YaRu += 1;
            txt213.Text = waijing_YaRu.ToString();
        }

        private void btnSave_YaRu_Click(object sender, EventArgs e)
        {
            if (m_duanMianAB == 0)
            {
                GlobalVariable.configSetting.duanMianAParam.dwMifengYaRuOuterOffset = waijing_YaRu;
                GlobalVariable.configSetting.duanMianAParam.dwMifengYaRuInnerOffset = neijing_YaRu;
                GlobalVariable.DuanMianAProcess.setParamInfo(GlobalVariable.configSetting.duanMianAParam);
            }
            else
            {
                GlobalVariable.configSetting.duanMianBParam.dwMifengYaRuOuterOffset = waijing_YaRu;
                GlobalVariable.configSetting.duanMianBParam.dwMifengYaRuInnerOffset = neijing_YaRu;
                GlobalVariable.DuanMianBProcess.setParamInfo(GlobalVariable.configSetting.duanMianBParam);
            }
            string ErrorMsg = "";
            //bool bRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json", ref ErrorMsg);
            //File.Copy(FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json",
            //             FilePath.ParamSettingPath + "ParamBackup" + "\\Param.Json",
            //             true);
            //if (bRtn)
            //{
            //    GlobalVariable.resetParam();
            //    GlobalMethod.ShowMessage("保存成功");
            //}
            //else
            //{
            //    GlobalMethod.ShowMessage("保存失败，异常信息为" + ErrorMsg);
            //}

            string mainPath = FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json";
            string backupPath = FilePath.ParamSettingPath + "ParamBackup\\Param.Json";

            // 1. 写入主配置文件
            bool mRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, mainPath, ref ErrorMsg);

            if (mRtn)
            {
                // 2. 写入备份配置文件（同样使用 WriteJsonFile）
                bool backupRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, backupPath, ref ErrorMsg);

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

        private void DrawTwoYuan_YaRu()
        {
            if (graphicsShow_YaRu == null)
            {
                return;
            }
            graphicsShow_YaRu.DrawImage(allPicture[1], 0, 0);
            float d1 = m_modelInfo.fOuterLoopMinRadius + waijing_YaRu * 2;
            float d2 = m_modelInfo.fMifengMaxRadius + neijing_YaRu * 2;
            graphicsShow_YaRu.DrawEllipse(pen, (float)(m_modelInfo.fInnerLoopMinX - d1 / 2), (float)(m_modelInfo.fInnerLoopMinY - d1 / 2), (float)d1, (float)d1);
            graphicsShow_YaRu.DrawEllipse(pen, (float)(m_modelInfo.fInnerLoopMinX - d2 / 2), (float)(m_modelInfo.fInnerLoopMinY - d2 / 2), (float)d2, (float)d2);
            pictureBox_YaRu.Image = bitmapShow_YaRu;
        }

        private void txt214_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = neijing_YaRu.ToString();
                return;
            }
            neijing_YaRu = i;
            DrawTwoYuan_YaRu();
        }

        private void txt213_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = waijing_YaRu.ToString();
                return;
            }
            waijing_YaRu = i;
            DrawTwoYuan_YaRu();
        }

        #endregion 压入区域有关

        #region 卷边区域

        private void LoadParam_JuanBian()
        {
            txt276.Text = waijing_JuanBian.ToString();
            txt277.Text = neijing_JuanBian.ToString();
            if (bitmapShow_JuanBian != null)
            {
                graphicsShow_JuanBian.DrawImage(allPicture[3], 0, 0);
                DrawTwoYuan_JuanBian();
                pictureBox_JuanBian.Image = bitmapShow_JuanBian;
            }
        }

        private void btnD1_JuanBian_Click(object sender, EventArgs e)
        {
            neijing_JuanBian -= 1;
            txt277.Text = neijing_JuanBian.ToString();
        }

        private void btnD2_JuanBian_Click(object sender, EventArgs e)
        {
            waijing_JuanBian -= 1;
            txt276.Text = waijing_JuanBian.ToString();
        }

        private void btnA1_JuanBian_Click(object sender, EventArgs e)
        {
            neijing_JuanBian += 1;
            txt277.Text = neijing_JuanBian.ToString();
        }

        private void btnA2_JuanBian_Click(object sender, EventArgs e)
        {
            waijing_JuanBian += 1;
            txt276.Text = waijing_JuanBian.ToString();
        }

        private void btnSave_JuanBian_Click(object sender, EventArgs e)
        {
            if (m_duanMianAB == 0)
            {
                GlobalVariable.configSetting.duanMianAParam.dwMifengJuanSideRegionOuterOffset = waijing_JuanBian;
                GlobalVariable.configSetting.duanMianAParam.dwMifengJuanSideRegionInnerOffset = neijing_JuanBian;
                GlobalVariable.DuanMianAProcess.setParamInfo(GlobalVariable.configSetting.duanMianAParam);
            }
            else
            {
                GlobalVariable.configSetting.duanMianBParam.dwMifengJuanSideRegionOuterOffset = waijing_JuanBian;
                GlobalVariable.configSetting.duanMianBParam.dwMifengJuanSideRegionInnerOffset = neijing_JuanBian;
                GlobalVariable.DuanMianBProcess.setParamInfo(GlobalVariable.configSetting.duanMianBParam);
            }
            string ErrorMsg = "";
            //bool bRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json", ref ErrorMsg);
            //File.Copy(FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json",
            //             FilePath.ParamSettingPath + "ParamBackup" + "\\Param.Json",
            //             true);
            //if (bRtn)
            //{
            //    GlobalVariable.resetParam();
            //    GlobalMethod.ShowMessage("保存成功");
            //}
            //else
            //{
            //    GlobalMethod.ShowMessage("保存失败，异常信息为" + ErrorMsg);
            //}

            string mainPath = FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json";
            string backupPath = FilePath.ParamSettingPath + "ParamBackup\\Param.Json";

            // 1. 写入主配置文件
            bool mRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, mainPath, ref ErrorMsg);

            if (mRtn)
            {
                // 2. 写入备份配置文件（同样使用 WriteJsonFile）
                bool backupRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, backupPath, ref ErrorMsg);

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

        private void txt277_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = neijing_JuanBian.ToString();
                return;
            }
            neijing_JuanBian = i;
            DrawTwoYuan_JuanBian();
        }

        private void txt276_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = waijing_JuanBian.ToString();
                return;
            }
            waijing_JuanBian = i;
            DrawTwoYuan_JuanBian();
        }

        private void DrawTwoYuan_JuanBian()
        {
            if (graphicsShow_JuanBian == null)
            {
                return;
            }
            graphicsShow_JuanBian.DrawImage(allPicture[3], 0, 0);
            float d1 = m_modelInfo.fOuterLoopMinRadius + waijing_JuanBian * 2;
            float d2 = m_modelInfo.fMifengMaxRadius + neijing_JuanBian * 2;
            graphicsShow_JuanBian.DrawEllipse(pen, (float)(m_modelInfo.fInnerLoopMinX - d1 / 2), (float)(m_modelInfo.fInnerLoopMinY - d1 / 2), (float)d1, (float)d1);
            graphicsShow_JuanBian.DrawEllipse(pen, (float)(m_modelInfo.fInnerLoopMinX - d2 / 2), (float)(m_modelInfo.fInnerLoopMinY - d2 / 2), (float)d2, (float)d2);
            pictureBox_JuanBian.Image = bitmapShow_JuanBian;
        }

        #endregion 卷边区域

        #region 文字区域

        private void LoadParam_WenZi()
        {
            ckb233.Checked = paramInfo.bMifengTextRegionCheckOne;
            group233.Enabled = ckb233.Checked;
            ckb238.Checked = paramInfo.bMifengTextRegionCheckTwo;
            group238.Enabled = ckb238.Checked;
            ckb243.Checked = paramInfo.bMifengTextRegionCheckThree;
            group243.Enabled = ckb243.Checked;
            ckb248.Checked = paramInfo.bMifengTextRegionCheckFour;
            group248.Enabled = ckb248.Checked;
            ckb253.Checked = paramInfo.bMifengNoTextRegionCheck;
            group253.Enabled = ckb253.Checked;
            txt254.Text = paramInfo.dwMifengNoTextStartAngle.ToString();
            txt255.Text = paramInfo.dwMifengNoTextEndAngle.ToString();
            txt256.Text = paramInfo.dwMifengNoTextOuterOffset.ToString();
            txt257.Text = paramInfo.dwMifengNoTextInnerOffset.ToString();
            ckb258.Checked = paramInfo.bMifengKeyTextReconCheck;
            group258.Enabled = ckb258.Checked;
            txt259.Text = paramInfo.dwMifengKeyTextStartAngle.ToString();
            txt260.Text = paramInfo.dwMifengKeyTextEndAngle.ToString();
            txt261.Text = paramInfo.dwMifengKeyTextRegionOuterOffset.ToString();
            txt262.Text = paramInfo.dwMifengKeyTextRegionInnerOffset.ToString();
            numericUpDown1.Value = paramInfo.dwMifengTextRegionOneStartAngle;
            numericUpDown2.Value = paramInfo.dwMifengTextRegionOneEndAngle;
            numericUpDown3.Value = paramInfo.dwMifengTextRegionOneOuterOffset;
            numericUpDown4.Value = paramInfo.dwMifengTextRegionOneInnerOffset;

            numericUpDown5.Value = paramInfo.dwMifengTextRegionTwoStartAngle;
            numericUpDown6.Value = paramInfo.dwMifengTextRegionTwoEndAngle;
            numericUpDown7.Value = paramInfo.dwMifengTextRegionTwoOuterOffset;
            numericUpDown8.Value = paramInfo.dwMifengTextRegionTwoInnerOffset;

            numericUpDown9.Value = paramInfo.dwMifengTextRegionThreeStartAngle;
            numericUpDown10.Value = paramInfo.dwMifengTextRegionThreeEndAngle;
            numericUpDown11.Value = paramInfo.dwMifengTextRegionThreeOuterOffset;
            numericUpDown12.Value = paramInfo.dwMifengTextRegionThreeInnerOffset;

            numericUpDown13.Value = paramInfo.dwMifengTextRegionFourStartAngle;
            numericUpDown14.Value = paramInfo.dwMifengTextRegionFourEndAngle;
            numericUpDown15.Value = paramInfo.dwMifengTextRegionFourOuterOffset;
            numericUpDown16.Value = paramInfo.dwMifengTextRegionFourInnerOffset;
            nud_TextUp.Value = paramInfo.textUp;
            nud_TextDown.Value = paramInfo.textDown;

            if (graphicsShow_WenZi != null)
            {
                drawAllArea_WenZi();
            }
        }

        private void btnChangePicture_WenZi_Click(object sender, EventArgs e)
        {
        }

        private void btnCreateModel_WenZi_Click(object sender, EventArgs e)
        {
            if (m_duanMianAB == 0)
            {
                GlobalVariable.DuanMianAProcess.Sample(allPicture[2], 2);
            }
            else
            {
                GlobalVariable.DuanMianBProcess.Sample(allPicture[2], 2);
            }
        }

        private void btnSaveParam_WenZi_Click(object sender, EventArgs e)
        {
            if (m_duanMianAB == 0)
            {
                GlobalVariable.configSetting.duanMianAParam.bMifengTextRegionCheckOne = paramInfo.bMifengTextRegionCheckOne;
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextRegionOneStartAngle = paramInfo.dwMifengTextRegionOneStartAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextRegionOneEndAngle = paramInfo.dwMifengTextRegionOneEndAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextRegionOneOuterOffset = paramInfo.dwMifengTextRegionOneOuterOffset;
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextRegionOneInnerOffset = paramInfo.dwMifengTextRegionOneInnerOffset;
                GlobalVariable.configSetting.duanMianAParam.bMifengTextRegionCheckTwo = paramInfo.bMifengTextRegionCheckTwo;
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextRegionTwoStartAngle = paramInfo.dwMifengTextRegionTwoStartAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextRegionTwoEndAngle = paramInfo.dwMifengTextRegionTwoEndAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextRegionTwoOuterOffset = paramInfo.dwMifengTextRegionTwoOuterOffset;
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextRegionTwoInnerOffset = paramInfo.dwMifengTextRegionTwoInnerOffset;
                GlobalVariable.configSetting.duanMianAParam.bMifengTextRegionCheckThree = paramInfo.bMifengTextRegionCheckThree;
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextRegionThreeStartAngle = paramInfo.dwMifengTextRegionThreeStartAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextRegionThreeEndAngle = paramInfo.dwMifengTextRegionThreeEndAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextRegionThreeOuterOffset = paramInfo.dwMifengTextRegionThreeOuterOffset;
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextRegionThreeInnerOffset = paramInfo.dwMifengTextRegionThreeInnerOffset;
                GlobalVariable.configSetting.duanMianAParam.bMifengTextRegionCheckFour = paramInfo.bMifengTextRegionCheckFour;
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextRegionFourStartAngle = paramInfo.dwMifengTextRegionFourStartAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextRegionFourEndAngle = paramInfo.dwMifengTextRegionFourEndAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextRegionFourOuterOffset = paramInfo.dwMifengTextRegionFourOuterOffset;
                GlobalVariable.configSetting.duanMianAParam.dwMifengTextRegionFourInnerOffset = paramInfo.dwMifengTextRegionFourInnerOffset;
                GlobalVariable.configSetting.duanMianAParam.bMifengNoTextRegionCheck = paramInfo.bMifengNoTextRegionCheck;
                GlobalVariable.configSetting.duanMianAParam.dwMifengNoTextStartAngle = paramInfo.dwMifengNoTextStartAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengNoTextEndAngle = paramInfo.dwMifengNoTextEndAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengNoTextOuterOffset = paramInfo.dwMifengNoTextOuterOffset;
                GlobalVariable.configSetting.duanMianAParam.dwMifengNoTextInnerOffset = paramInfo.dwMifengNoTextInnerOffset;
                GlobalVariable.configSetting.duanMianAParam.bMifengKeyTextReconCheck = paramInfo.bMifengKeyTextReconCheck;
                GlobalVariable.configSetting.duanMianAParam.dwMifengKeyTextStartAngle = paramInfo.dwMifengKeyTextStartAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengKeyTextEndAngle = paramInfo.dwMifengKeyTextEndAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengKeyTextRegionOuterOffset = paramInfo.dwMifengKeyTextRegionOuterOffset;
                GlobalVariable.configSetting.duanMianAParam.dwMifengKeyTextRegionInnerOffset = paramInfo.dwMifengKeyTextRegionInnerOffset;
                GlobalVariable.configSetting.duanMianAParam.textDown = paramInfo.textDown;
                GlobalVariable.configSetting.duanMianAParam.textUp = paramInfo.textUp;
                GlobalVariable.DuanMianAProcess.setParamInfo(GlobalVariable.configSetting.duanMianAParam);
            }
            else
            {
                GlobalVariable.configSetting.duanMianBParam.bMifengTextRegionCheckOne = paramInfo.bMifengTextRegionCheckOne;
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextRegionOneStartAngle = paramInfo.dwMifengTextRegionOneStartAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextRegionOneEndAngle = paramInfo.dwMifengTextRegionOneEndAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextRegionOneOuterOffset = paramInfo.dwMifengTextRegionOneOuterOffset;
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextRegionOneInnerOffset = paramInfo.dwMifengTextRegionOneInnerOffset;
                GlobalVariable.configSetting.duanMianBParam.bMifengTextRegionCheckTwo = paramInfo.bMifengTextRegionCheckTwo;
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextRegionTwoStartAngle = paramInfo.dwMifengTextRegionTwoStartAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextRegionTwoEndAngle = paramInfo.dwMifengTextRegionTwoEndAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextRegionTwoOuterOffset = paramInfo.dwMifengTextRegionTwoOuterOffset;
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextRegionTwoInnerOffset = paramInfo.dwMifengTextRegionTwoInnerOffset;
                GlobalVariable.configSetting.duanMianBParam.bMifengTextRegionCheckThree = paramInfo.bMifengTextRegionCheckThree;
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextRegionThreeStartAngle = paramInfo.dwMifengTextRegionThreeStartAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextRegionThreeEndAngle = paramInfo.dwMifengTextRegionThreeEndAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextRegionThreeOuterOffset = paramInfo.dwMifengTextRegionThreeOuterOffset;
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextRegionThreeInnerOffset = paramInfo.dwMifengTextRegionThreeInnerOffset;
                GlobalVariable.configSetting.duanMianBParam.bMifengTextRegionCheckFour = paramInfo.bMifengTextRegionCheckFour;
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextRegionFourStartAngle = paramInfo.dwMifengTextRegionFourStartAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextRegionFourEndAngle = paramInfo.dwMifengTextRegionFourEndAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextRegionFourOuterOffset = paramInfo.dwMifengTextRegionFourOuterOffset;
                GlobalVariable.configSetting.duanMianBParam.dwMifengTextRegionFourInnerOffset = paramInfo.dwMifengTextRegionFourInnerOffset;
                GlobalVariable.configSetting.duanMianBParam.bMifengNoTextRegionCheck = paramInfo.bMifengNoTextRegionCheck;
                GlobalVariable.configSetting.duanMianBParam.dwMifengNoTextStartAngle = paramInfo.dwMifengNoTextStartAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengNoTextEndAngle = paramInfo.dwMifengNoTextEndAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengNoTextOuterOffset = paramInfo.dwMifengNoTextOuterOffset;
                GlobalVariable.configSetting.duanMianBParam.dwMifengNoTextInnerOffset = paramInfo.dwMifengNoTextInnerOffset;
                GlobalVariable.configSetting.duanMianBParam.bMifengKeyTextReconCheck = paramInfo.bMifengKeyTextReconCheck;
                GlobalVariable.configSetting.duanMianBParam.dwMifengKeyTextStartAngle = paramInfo.dwMifengKeyTextStartAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengKeyTextEndAngle = paramInfo.dwMifengKeyTextEndAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengKeyTextRegionOuterOffset = paramInfo.dwMifengKeyTextRegionOuterOffset;
                GlobalVariable.configSetting.duanMianBParam.dwMifengKeyTextRegionInnerOffset = paramInfo.dwMifengKeyTextRegionInnerOffset;
                GlobalVariable.configSetting.duanMianBParam.textDown = paramInfo.textDown;
                GlobalVariable.configSetting.duanMianBParam.textUp = paramInfo.textUp;
                GlobalVariable.DuanMianBProcess.setParamInfo(GlobalVariable.configSetting.duanMianBParam);
            }
            string ErrorMsg = "";
            //bool bRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json", ref ErrorMsg);
            //File.Copy(FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json",
            //              FilePath.ParamSettingPath + "ParamBackup" + "\\Param.Json",
            //              true);
            //if (bRtn)
            //{
            //    GlobalVariable.resetParam();
            //    GlobalMethod.ShowMessage("保存成功");
            //}
            //else
            //{
            //    GlobalMethod.ShowMessage("保存失败，异常信息为" + ErrorMsg);
            //}

            string mainPath = FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json";
            string backupPath = FilePath.ParamSettingPath + "ParamBackup\\Param.Json";

            // 1. 写入主配置文件
            bool mRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, mainPath, ref ErrorMsg);

            if (mRtn)
            {
                // 2. 写入备份配置文件（同样使用 WriteJsonFile）
                bool backupRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, backupPath, ref ErrorMsg);

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

        private void drawLittleArea_WenZi(UInt32 startAngle, UInt32 endAngle, int outJing, int innerJing)
        {
            try
            {
                float d1 = m_modelInfo.fMifengMaxRadius + outJing * 2;
                float d2 = m_modelInfo.fMifengMinRadius + innerJing * 2;
                float sweepAngle = 0;
                if (startAngle <= endAngle)
                {
                    sweepAngle = endAngle - startAngle;
                }
                else
                {
                    sweepAngle = endAngle + 360 - startAngle;
                }

                graphicsShow_WenZi.DrawArc(pen, (float)(m_modelInfo.fInnerLoopMinX - d1 / 2), (float)(m_modelInfo.fInnerLoopMinY - d1 / 2), d1, d1, 360 - endAngle, sweepAngle);
                graphicsShow_WenZi.DrawArc(pen, (float)(m_modelInfo.fInnerLoopMinX - d2 / 2), (float)(m_modelInfo.fInnerLoopMinY - d2 / 2), d2, d2, 360 - endAngle, sweepAngle);
                PointF start1 = new PointF();
                start1.X = m_modelInfo.fInnerLoopMinX + (float)Math.Cos((360 - startAngle) * Math.PI / 180) * d1 / 2;
                start1.Y = m_modelInfo.fInnerLoopMinY + (float)Math.Sin((360 - startAngle) * Math.PI / 180) * d1 / 2;
                PointF start2 = new PointF();
                start2.X = m_modelInfo.fInnerLoopMinX + (float)Math.Cos((360 - startAngle) * Math.PI / 180) * d2 / 2;
                start2.Y = m_modelInfo.fInnerLoopMinY + (float)Math.Sin((360 - startAngle) * Math.PI / 180) * d2 / 2;
                graphicsShow_WenZi.DrawLine(pen, start1, start2);

                PointF end1 = new PointF();
                end1.X = m_modelInfo.fInnerLoopMinX + (float)Math.Cos((360 - endAngle) * Math.PI / 180) * d1 / 2;
                end1.Y = m_modelInfo.fInnerLoopMinY + (float)Math.Sin((360 - endAngle) * Math.PI / 180) * d1 / 2;
                PointF end2 = new PointF();
                end2.X = m_modelInfo.fInnerLoopMinX + (float)Math.Cos((360 - endAngle) * Math.PI / 180) * d2 / 2;
                end2.Y = m_modelInfo.fInnerLoopMinY + (float)Math.Sin((360 - endAngle) * Math.PI / 180) * d2 / 2;
                graphicsShow_WenZi.DrawLine(pen, end1, end2);
            }
            catch
            {
            }
        }

        private void drawAllArea_WenZi()
        {
            if (graphicsShow_WenZi == null)
            {
                return;
            }
            graphicsShow_WenZi.DrawImage(allPicture[2], 0, 0);
            if (ckb233.Checked)
            {
                drawLittleArea_WenZi(paramInfo.dwMifengTextRegionOneStartAngle, paramInfo.dwMifengTextRegionOneEndAngle, paramInfo.dwMifengTextRegionOneOuterOffset, paramInfo.dwMifengTextRegionOneInnerOffset);
            }
            if (ckb238.Checked)
            {
                drawLittleArea_WenZi(paramInfo.dwMifengTextRegionTwoStartAngle, paramInfo.dwMifengTextRegionTwoEndAngle, paramInfo.dwMifengTextRegionTwoOuterOffset, paramInfo.dwMifengTextRegionTwoInnerOffset);
            }
            if (ckb243.Checked)
            {
                drawLittleArea_WenZi(paramInfo.dwMifengTextRegionThreeStartAngle, paramInfo.dwMifengTextRegionThreeEndAngle, paramInfo.dwMifengTextRegionThreeOuterOffset, paramInfo.dwMifengTextRegionThreeInnerOffset);
            }
            if (ckb248.Checked)
            {
                drawLittleArea_WenZi(paramInfo.dwMifengTextRegionFourStartAngle, paramInfo.dwMifengTextRegionFourEndAngle, paramInfo.dwMifengTextRegionFourOuterOffset, paramInfo.dwMifengTextRegionFourInnerOffset);
            }
            if (ckb253.Checked)
            {
                drawLittleArea_WenZi(paramInfo.dwMifengNoTextStartAngle, paramInfo.dwMifengNoTextEndAngle, paramInfo.dwMifengNoTextOuterOffset, paramInfo.dwMifengNoTextInnerOffset);
            }
            if (ckb258.Checked)
            {
                drawLittleArea_WenZi(paramInfo.dwMifengKeyTextStartAngle, paramInfo.dwMifengKeyTextEndAngle, paramInfo.dwMifengKeyTextRegionOuterOffset, paramInfo.dwMifengKeyTextRegionInnerOffset);
            }
            pictureBox_WenZi.Image = bitmapShow_WenZi;
        }

        #region textBox和checkBox的事件，发生改变则重新画图

        private void ckb233_CheckedChanged(object sender, EventArgs e)
        {
            paramInfo.bMifengTextRegionCheckOne = ckb233.Checked;
            group233.Enabled = ckb233.Checked;
            drawAllArea_WenZi();
        }

        private void ckb238_CheckedChanged(object sender, EventArgs e)
        {
            paramInfo.bMifengTextRegionCheckTwo = ckb238.Checked;
            group238.Enabled = ckb238.Checked;
            drawAllArea_WenZi();
        }

        private void ckb243_CheckedChanged(object sender, EventArgs e)
        {
            paramInfo.bMifengTextRegionCheckThree = ckb243.Checked;
            group243.Enabled = ckb243.Checked;
            drawAllArea_WenZi();
        }

        private void ckb248_CheckedChanged(object sender, EventArgs e)
        {
            paramInfo.bMifengTextRegionCheckFour = ckb248.Checked;
            group248.Enabled = ckb248.Checked;
            drawAllArea_WenZi();
        }

        private void ckb258_CheckedChanged(object sender, EventArgs e)
        {
            paramInfo.bMifengKeyTextReconCheck = ckb258.Checked;
            group258.Enabled = ckb258.Checked;
            drawAllArea_WenZi();
        }

        private void ckb253_CheckedChanged(object sender, EventArgs e)
        {
            paramInfo.bMifengNoTextRegionCheck = ckb253.Checked;
            group253.Enabled = ckb253.Checked;
            drawAllArea_WenZi();
        }

        private void txt259_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengKeyTextStartAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengKeyTextStartAngle = (uint)i;
            drawAllArea_WenZi();
        }

        private void txt260_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengKeyTextEndAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengKeyTextEndAngle = (uint)i;
            drawAllArea_WenZi();
        }

        private void txt261_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengKeyTextRegionOuterOffset.ToString();
                return;
            }
            paramInfo.dwMifengKeyTextRegionOuterOffset = i;
            drawAllArea_WenZi();
        }

        private void txt262_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengKeyTextRegionInnerOffset.ToString();
                return;
            }
            paramInfo.dwMifengKeyTextRegionInnerOffset = i;
            drawAllArea_WenZi();
        }

        private void txt254_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengNoTextStartAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengNoTextStartAngle = (uint)i;
            drawAllArea_WenZi();
        }

        private void txt255_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengNoTextEndAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengNoTextEndAngle = (uint)i;
            drawAllArea_WenZi();
        }

        private void txt256_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengNoTextOuterOffset.ToString();
                return;
            }
            paramInfo.dwMifengNoTextOuterOffset = i;
            drawAllArea_WenZi();
        }

        private void txt257_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengNoTextInnerOffset.ToString();
                return;
            }
            paramInfo.dwMifengNoTextInnerOffset = i;
            drawAllArea_WenZi();
        }

        private void txt234_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengTextRegionOneStartAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengTextRegionOneStartAngle = (uint)i;
            drawAllArea_WenZi();
        }

        private void txt235_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengTextRegionOneEndAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengTextRegionOneEndAngle = (uint)i;
            drawAllArea_WenZi();
        }

        private void txt236_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengTextRegionOneOuterOffset.ToString();
                return;
            }
            paramInfo.dwMifengTextRegionOneOuterOffset = i;
            drawAllArea_WenZi();
        }

        private void txt237_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengTextRegionOneInnerOffset.ToString();
                return;
            }
            paramInfo.dwMifengTextRegionOneInnerOffset = i;
            drawAllArea_WenZi();
        }

        private void txt239_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengTextRegionTwoStartAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengTextRegionTwoStartAngle = (uint)i;
            drawAllArea_WenZi();
        }

        private void txt240_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengTextRegionTwoEndAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengTextRegionTwoEndAngle = (uint)i;
            drawAllArea_WenZi();
        }

        private void txt241_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengTextRegionTwoOuterOffset.ToString();
                return;
            }
            paramInfo.dwMifengTextRegionTwoOuterOffset = i;
            drawAllArea_WenZi();
        }

        private void txt242_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengTextRegionTwoInnerOffset.ToString();
                return;
            }
            paramInfo.dwMifengTextRegionTwoInnerOffset = i;
            drawAllArea_WenZi();
        }

        private void txt244_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengTextRegionThreeStartAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengTextRegionThreeStartAngle = (uint)i;
            drawAllArea_WenZi();
        }

        private void txt245_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengTextRegionThreeEndAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengTextRegionThreeEndAngle = (uint)i;
            drawAllArea_WenZi();
        }

        private void txt246_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengTextRegionThreeOuterOffset.ToString();
                return;
            }
            paramInfo.dwMifengTextRegionThreeOuterOffset = i;
            drawAllArea_WenZi();
        }

        private void txt247_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengTextRegionThreeInnerOffset.ToString();
                return;
            }
            paramInfo.dwMifengTextRegionThreeInnerOffset = i;
            drawAllArea_WenZi();
        }

        private void txt249_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengTextRegionFourStartAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengTextRegionFourStartAngle = (uint)i;
            drawAllArea_WenZi();
        }

        private void txt250_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengTextRegionFourEndAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengTextRegionFourEndAngle = (uint)i;
            drawAllArea_WenZi();
        }

        private void txt251_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengTextRegionFourOuterOffset.ToString();
                return;
            }
            paramInfo.dwMifengTextRegionFourOuterOffset = i;
            drawAllArea_WenZi();
        }

        private void txt252_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengTextRegionFourInnerOffset.ToString();
                return;
            }
            paramInfo.dwMifengTextRegionFourInnerOffset = i;
            drawAllArea_WenZi();
        }

        #region 加减按钮

        private void btnD17_WenZi_Click(object sender, EventArgs e)
        {
            if (paramInfo.dwMifengKeyTextStartAngle == 0)
            {
                paramInfo.dwMifengKeyTextStartAngle = 360;
            }
            paramInfo.dwMifengKeyTextStartAngle--;
            txt259.Text = paramInfo.dwMifengKeyTextStartAngle.ToString();
        }

        private void btnA17_WenZi_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengKeyTextStartAngle++;
            if (paramInfo.dwMifengKeyTextStartAngle == 360)
            {
                paramInfo.dwMifengKeyTextStartAngle = 0;
            }
            txt259.Text = paramInfo.dwMifengKeyTextStartAngle.ToString();
        }

        private void btnD18_WenZi_Click(object sender, EventArgs e)
        {
            if (paramInfo.dwMifengKeyTextEndAngle == 0)
            {
                paramInfo.dwMifengKeyTextEndAngle = 360;
            }
            paramInfo.dwMifengKeyTextEndAngle--;
            txt260.Text = paramInfo.dwMifengKeyTextEndAngle.ToString();
        }

        private void btnA18_WenZi_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengKeyTextEndAngle++;
            if (paramInfo.dwMifengKeyTextEndAngle == 360)
            {
                paramInfo.dwMifengKeyTextEndAngle = 0;
            }
            txt260.Text = paramInfo.dwMifengKeyTextEndAngle.ToString();
        }

        private void btnD19_WenZi_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengKeyTextRegionOuterOffset--;
            txt261.Text = paramInfo.dwMifengKeyTextRegionOuterOffset.ToString();
        }

        private void btnA19_WenZi_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengKeyTextRegionOuterOffset++;
            txt261.Text = paramInfo.dwMifengKeyTextRegionOuterOffset.ToString();
        }

        private void btnD20_WenZi_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengKeyTextRegionInnerOffset--;
            txt262.Text = paramInfo.dwMifengKeyTextRegionInnerOffset.ToString();
        }

        private void btnA20_WenZi_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengKeyTextRegionInnerOffset++;
            txt262.Text = paramInfo.dwMifengKeyTextRegionInnerOffset.ToString();
        }

        private void btnD21_WenZi_Click(object sender, EventArgs e)
        {
            if (paramInfo.dwMifengNoTextStartAngle == 0)
            {
                paramInfo.dwMifengNoTextStartAngle = 360;
            }
            paramInfo.dwMifengNoTextStartAngle--;
            txt254.Text = paramInfo.dwMifengNoTextStartAngle.ToString();
        }

        private void btnA21_WenZi_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengNoTextStartAngle++;
            if (paramInfo.dwMifengNoTextStartAngle == 360)
            {
                paramInfo.dwMifengNoTextStartAngle = 0;
            }
            txt254.Text = paramInfo.dwMifengNoTextStartAngle.ToString();
        }

        private void btnD22_WenZi_Click(object sender, EventArgs e)
        {
            if (paramInfo.dwMifengNoTextEndAngle == 0)
            {
                paramInfo.dwMifengNoTextEndAngle = 360;
            }
            paramInfo.dwMifengNoTextEndAngle--;
            txt255.Text = paramInfo.dwMifengNoTextEndAngle.ToString();
        }

        private void btnA22_WenZi_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengNoTextEndAngle++;
            if (paramInfo.dwMifengNoTextEndAngle == 360)
            {
                paramInfo.dwMifengNoTextEndAngle = 0;
            }
            txt255.Text = paramInfo.dwMifengNoTextEndAngle.ToString();
        }

        private void btnD23_WenZi_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengNoTextOuterOffset--;
            txt256.Text = paramInfo.dwMifengNoTextOuterOffset.ToString();
        }

        private void btnA23_WenZi_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengNoTextOuterOffset++;
            txt256.Text = paramInfo.dwMifengNoTextOuterOffset.ToString();
        }

        private void btnD24_WenZi_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengNoTextInnerOffset--;
            txt257.Text = paramInfo.dwMifengNoTextInnerOffset.ToString();
        }

        private void btnA24_WenZi_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengNoTextInnerOffset++;
            txt257.Text = paramInfo.dwMifengNoTextInnerOffset.ToString();
        }

        #endregion 加减按钮

        #endregion textBox和checkBox的事件，发生改变则重新画图

        #endregion 文字区域

        #region 象限光区域

        private void LoadParam_XiangXinaGuang()
        {
            txt324.Text = paramInfo.dwMifengSubRegionOuterOffset.ToString();
            txt325.Text = paramInfo.dwMifengSubRegionInnerOffset.ToString();
            txt326.Text = paramInfo.dwMifengRegionOneStartAngle.ToString();
            txt327.Text = paramInfo.dwMifengRegionOneEndAngle.ToString();
            txt328.Text = paramInfo.dwMifengRegionTwoStartAngle.ToString();
            txt329.Text = paramInfo.dwMifengRegionTwoEndAngle.ToString();
            txt330.Text = paramInfo.dwMifengRegionThreeStartAngle.ToString();
            txt331.Text = paramInfo.dwMifengRegionThreeEndAngle.ToString();
            txt332.Text = paramInfo.dwMifengRegionFourStartAngle.ToString();
            txt333.Text = paramInfo.dwMifengRegionFourEndAngle.ToString();
            if (graphicsShow_XiangXian[0] != null)
            {
                drawAllArea_XiangXian();
            }
        }

        private void btnSave_XiangXian_Click(object sender, EventArgs e)
        {
            if (m_duanMianAB == 0)
            {
                GlobalVariable.configSetting.duanMianAParam.dwMifengSubRegionOuterOffset = paramInfo.dwMifengSubRegionOuterOffset;
                GlobalVariable.configSetting.duanMianAParam.dwMifengSubRegionInnerOffset = paramInfo.dwMifengSubRegionInnerOffset;
                GlobalVariable.configSetting.duanMianAParam.dwMifengRegionOneStartAngle = paramInfo.dwMifengRegionOneStartAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengRegionOneEndAngle = paramInfo.dwMifengRegionOneEndAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengRegionTwoStartAngle = paramInfo.dwMifengRegionTwoStartAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengRegionTwoEndAngle = paramInfo.dwMifengRegionTwoEndAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengRegionThreeStartAngle = paramInfo.dwMifengRegionThreeStartAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengRegionThreeEndAngle = paramInfo.dwMifengRegionThreeEndAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengRegionFourStartAngle = paramInfo.dwMifengRegionFourStartAngle;
                GlobalVariable.configSetting.duanMianAParam.dwMifengRegionFourEndAngle = paramInfo.dwMifengRegionFourEndAngle;
                GlobalVariable.DuanMianAProcess.setParamInfo(GlobalVariable.configSetting.duanMianAParam);
            }
            else
            {
                GlobalVariable.configSetting.duanMianBParam.dwMifengSubRegionOuterOffset = paramInfo.dwMifengSubRegionOuterOffset;
                GlobalVariable.configSetting.duanMianBParam.dwMifengSubRegionInnerOffset = paramInfo.dwMifengSubRegionInnerOffset;
                GlobalVariable.configSetting.duanMianBParam.dwMifengRegionOneStartAngle = paramInfo.dwMifengRegionOneStartAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengRegionOneEndAngle = paramInfo.dwMifengRegionOneEndAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengRegionTwoStartAngle = paramInfo.dwMifengRegionTwoStartAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengRegionTwoEndAngle = paramInfo.dwMifengRegionTwoEndAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengRegionThreeStartAngle = paramInfo.dwMifengRegionThreeStartAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengRegionThreeEndAngle = paramInfo.dwMifengRegionThreeEndAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengRegionFourStartAngle = paramInfo.dwMifengRegionFourStartAngle;
                GlobalVariable.configSetting.duanMianBParam.dwMifengRegionFourEndAngle = paramInfo.dwMifengRegionFourEndAngle;
                GlobalVariable.DuanMianBProcess.setParamInfo(GlobalVariable.configSetting.duanMianBParam);
            }
            string ErrorMsg = "";
            //bool bRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json", ref ErrorMsg);
            //File.Copy(FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json",
            //             FilePath.ParamSettingPath + "ParamBackup" + "\\Param.Json",
            //             true);
            //if (bRtn)
            //{
            //    GlobalVariable.resetParam();
            //    GlobalMethod.ShowMessage("保存成功");
            //}
            //else
            //{
            //    GlobalMethod.ShowMessage("保存失败，异常信息为" + ErrorMsg);
            //}

            string mainPath = FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json";
            string backupPath = FilePath.ParamSettingPath + "ParamBackup\\Param.Json";

            // 1. 写入主配置文件
            bool mRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, mainPath, ref ErrorMsg);

            if (mRtn)
            {
                // 2. 写入备份配置文件（同样使用 WriteJsonFile）
                bool backupRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, backupPath, ref ErrorMsg);

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

        private void drawOnePictureArea(int index, UInt32 startAngle, UInt32 endAngle, int outJing, int innerJing)
        {
            try
            {
                float d1 = m_modelInfo.fMifengMaxRadius + outJing * 2;
                float d2 = m_modelInfo.fMifengMinRadius + innerJing * 2;
                float sweepAngle = 0;
                if (startAngle <= endAngle)
                {
                    sweepAngle = endAngle - startAngle;
                }
                else
                {
                    sweepAngle = endAngle + 360 - startAngle;
                }

                graphicsShow_XiangXian[index].DrawArc(pen, (float)(m_modelInfo.fInnerLoopMinX - d1 / 2), (float)(m_modelInfo.fInnerLoopMinY - d1 / 2), (float)d1, (float)d1, 360 - endAngle, sweepAngle);
                graphicsShow_XiangXian[index].DrawArc(pen, (float)(m_modelInfo.fInnerLoopMinX - d2 / 2), (float)(m_modelInfo.fInnerLoopMinY - d2 / 2), (float)d2, (float)d2, 360 - endAngle, sweepAngle);
                PointF start1 = new PointF();
                start1.X = m_modelInfo.fInnerLoopMinX + (float)Math.Cos((360 - startAngle) * Math.PI / 180) * d1 / 2;
                start1.Y = m_modelInfo.fInnerLoopMinY + (float)Math.Sin((360 - startAngle) * Math.PI / 180) * d1 / 2;
                PointF start2 = new PointF();
                start2.X = m_modelInfo.fInnerLoopMinX + (float)Math.Cos((360 - startAngle) * Math.PI / 180) * d2 / 2;
                start2.Y = m_modelInfo.fInnerLoopMinY + (float)Math.Sin((360 - startAngle) * Math.PI / 180) * d2 / 2;
                graphicsShow_XiangXian[index].DrawLine(pen, start1, start2);

                PointF end1 = new PointF();
                end1.X = m_modelInfo.fInnerLoopMinX + (float)Math.Cos((360 - endAngle) * Math.PI / 180) * d1 / 2;
                end1.Y = m_modelInfo.fInnerLoopMinY + (float)Math.Sin((360 - endAngle) * Math.PI / 180) * d1 / 2;
                PointF end2 = new PointF();
                end2.X = m_modelInfo.fInnerLoopMinX + (float)Math.Cos((360 - endAngle) * Math.PI / 180) * d2 / 2;
                end2.Y = m_modelInfo.fInnerLoopMinY + (float)Math.Sin((360 - endAngle) * Math.PI / 180) * d2 / 2;
                graphicsShow_XiangXian[index].DrawLine(pen, end1, end2);

                graphicsShow_XiangXian[index].DrawArc(pen, (float)(m_modelInfo.fInnerLoopMinX - d1 / 2), (float)(m_modelInfo.fInnerLoopMinY - d1 / 2), (float)d1, (float)d1, 360 - endAngle + 180, sweepAngle);
                graphicsShow_XiangXian[index].DrawArc(pen, (float)(m_modelInfo.fInnerLoopMinX - d2 / 2), (float)(m_modelInfo.fInnerLoopMinY - d2 / 2), (float)d2, (float)d2, 360 - endAngle + 180, sweepAngle);
                start1.X = m_modelInfo.fInnerLoopMinX + (float)Math.Cos(((360 - startAngle) + 180) * Math.PI / 180) * d1 / 2;
                start1.Y = m_modelInfo.fInnerLoopMinY + (float)Math.Sin(((360 - startAngle) + 180) * Math.PI / 180) * d1 / 2;
                start2.X = m_modelInfo.fInnerLoopMinX + (float)Math.Cos(((360 - startAngle) + 180) * Math.PI / 180) * d2 / 2;
                start2.Y = m_modelInfo.fInnerLoopMinY + (float)Math.Sin(((360 - startAngle) + 180) * Math.PI / 180) * d2 / 2;
                graphicsShow_XiangXian[index].DrawLine(pen, start1, start2);

                end1.X = m_modelInfo.fInnerLoopMinX + (float)Math.Cos(((360 - endAngle) + 180) * Math.PI / 180) * d1 / 2;
                end1.Y = m_modelInfo.fInnerLoopMinY + (float)Math.Sin(((360 - endAngle) + 180) * Math.PI / 180) * d1 / 2;
                end2.X = m_modelInfo.fInnerLoopMinX + (float)Math.Cos(((360 - endAngle) + 180) * Math.PI / 180) * d2 / 2;
                end2.Y = m_modelInfo.fInnerLoopMinY + (float)Math.Sin(((360 - endAngle) + 180) * Math.PI / 180) * d2 / 2;
                graphicsShow_XiangXian[index].DrawLine(pen, end1, end2);
            }
            catch
            {
            }
        }

        private void drawAllArea_XiangXian()
        {
            if (graphicsShow_XiangXian[0] == null)
            {
                return;
            }

            graphicsShow_XiangXian[0].DrawImage(allPicture[6], 0, 0);
            drawOnePictureArea(0, paramInfo.dwMifengRegionOneStartAngle, paramInfo.dwMifengRegionOneEndAngle, paramInfo.dwMifengSubRegionOuterOffset, paramInfo.dwMifengSubRegionInnerOffset);
            pictureBox_XiangXian0.Image = bitmapShow_XiangXian[0];

            graphicsShow_XiangXian[1].DrawImage(allPicture[7], 0, 0);
            drawOnePictureArea(1, paramInfo.dwMifengRegionTwoStartAngle, paramInfo.dwMifengRegionTwoEndAngle, paramInfo.dwMifengSubRegionOuterOffset, paramInfo.dwMifengSubRegionInnerOffset);
            pictureBox_XiangXian1.Image = bitmapShow_XiangXian[1];

            graphicsShow_XiangXian[2].DrawImage(allPicture[8], 0, 0);
            drawOnePictureArea(2, paramInfo.dwMifengRegionThreeStartAngle, paramInfo.dwMifengRegionThreeEndAngle, paramInfo.dwMifengSubRegionOuterOffset, paramInfo.dwMifengSubRegionInnerOffset);
            pictureBox_XiangXian2.Image = bitmapShow_XiangXian[2];

            graphicsShow_XiangXian[3].DrawImage(allPicture[9], 0, 0);
            drawOnePictureArea(3, paramInfo.dwMifengRegionFourStartAngle, paramInfo.dwMifengRegionFourEndAngle, paramInfo.dwMifengSubRegionOuterOffset, paramInfo.dwMifengSubRegionInnerOffset);
            pictureBox_XiangXian3.Image = bitmapShow_XiangXian[3];
        }

        #region 数据变动，需要重新画图

        private void btnD1_XiangXian1_Click(object sender, EventArgs e)
        {
            if (paramInfo.dwMifengRegionOneStartAngle == 0)
            {
                paramInfo.dwMifengRegionOneStartAngle = 360;
            }
            paramInfo.dwMifengRegionOneStartAngle--;
            txt326.Text = paramInfo.dwMifengRegionOneStartAngle.ToString();
        }

        private void btnD2_XiangXian1_Click(object sender, EventArgs e)
        {
            if (paramInfo.dwMifengRegionOneEndAngle == 0)
            {
                paramInfo.dwMifengRegionOneEndAngle = 360;
            }
            paramInfo.dwMifengRegionOneEndAngle--;
            txt327.Text = paramInfo.dwMifengRegionOneEndAngle.ToString();
        }

        private void btnA1_XiangXian1_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengRegionOneStartAngle++;
            if (paramInfo.dwMifengRegionOneStartAngle == 360)
            {
                paramInfo.dwMifengRegionOneStartAngle = 0;
            }
            txt326.Text = paramInfo.dwMifengRegionOneStartAngle.ToString();
        }

        private void btnA2_XiangXian1_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengRegionOneEndAngle++;
            if (paramInfo.dwMifengRegionOneEndAngle == 360)
            {
                paramInfo.dwMifengRegionOneEndAngle = 0;
            }
            txt327.Text = paramInfo.dwMifengRegionOneEndAngle.ToString();
        }

        private void btnD1_XiangXian2_Click(object sender, EventArgs e)
        {
            if (paramInfo.dwMifengRegionTwoStartAngle == 0)
            {
                paramInfo.dwMifengRegionTwoStartAngle = 360;
            }
            paramInfo.dwMifengRegionTwoStartAngle--;
            txt328.Text = paramInfo.dwMifengRegionTwoStartAngle.ToString();
        }

        private void btnD2_XiangXian2_Click(object sender, EventArgs e)
        {
            if (paramInfo.dwMifengRegionTwoEndAngle == 0)
            {
                paramInfo.dwMifengRegionTwoEndAngle = 360;
            }
            paramInfo.dwMifengRegionTwoEndAngle--;
            txt329.Text = paramInfo.dwMifengRegionTwoEndAngle.ToString();
        }

        private void btnA1_XiangXian2_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengRegionTwoStartAngle++;
            if (paramInfo.dwMifengRegionTwoStartAngle == 360)
            {
                paramInfo.dwMifengRegionTwoStartAngle = 0;
            }
            txt328.Text = paramInfo.dwMifengRegionTwoStartAngle.ToString();
        }

        private void btnA2_XiangXian2_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengRegionTwoEndAngle++;
            if (paramInfo.dwMifengRegionTwoEndAngle == 360)
            {
                paramInfo.dwMifengRegionTwoEndAngle = 0;
            }
            txt329.Text = paramInfo.dwMifengRegionTwoEndAngle.ToString();
        }

        private void btnD1_XiangXian3_Click(object sender, EventArgs e)
        {
            if (paramInfo.dwMifengRegionThreeStartAngle == 0)
            {
                paramInfo.dwMifengRegionThreeStartAngle = 360;
            }
            paramInfo.dwMifengRegionThreeStartAngle--;
            txt330.Text = paramInfo.dwMifengRegionThreeStartAngle.ToString();
        }

        private void btnD2_XiangXian3_Click(object sender, EventArgs e)
        {
            if (paramInfo.dwMifengRegionThreeEndAngle == 0)
            {
                paramInfo.dwMifengRegionThreeEndAngle = 360;
            }
            paramInfo.dwMifengRegionThreeEndAngle--;
            txt331.Text = paramInfo.dwMifengRegionThreeEndAngle.ToString();
        }

        private void btnA1_XiangXian3_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengRegionThreeStartAngle++;
            if (paramInfo.dwMifengRegionThreeStartAngle == 360)
            {
                paramInfo.dwMifengRegionThreeStartAngle = 0;
            }
            txt330.Text = paramInfo.dwMifengRegionThreeStartAngle.ToString();
        }

        private void btnA2_XiangXian3_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengRegionThreeEndAngle++;
            if (paramInfo.dwMifengRegionThreeEndAngle == 360)
            {
                paramInfo.dwMifengRegionThreeEndAngle = 0;
            }
            txt331.Text = paramInfo.dwMifengRegionThreeEndAngle.ToString();
        }

        private void btnD1_XiangXian4_Click(object sender, EventArgs e)
        {
            if (paramInfo.dwMifengRegionFourStartAngle == 0)
            {
                paramInfo.dwMifengRegionFourStartAngle = 360;
            }
            paramInfo.dwMifengRegionFourStartAngle--;
            txt332.Text = paramInfo.dwMifengRegionFourStartAngle.ToString();
        }

        private void btnD2_XiangXian4_Click(object sender, EventArgs e)
        {
            if (paramInfo.dwMifengRegionFourEndAngle == 0)
            {
                paramInfo.dwMifengRegionFourEndAngle = 360;
            }
            paramInfo.dwMifengRegionFourEndAngle--;
            txt333.Text = paramInfo.dwMifengRegionFourEndAngle.ToString();
        }

        private void btnA1_XiangXian4_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengRegionFourStartAngle++;
            if (paramInfo.dwMifengRegionFourStartAngle == 360)
            {
                paramInfo.dwMifengRegionFourStartAngle = 0;
            }
            txt332.Text = paramInfo.dwMifengRegionFourStartAngle.ToString();
        }

        private void btnA2_XiangXian4_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengRegionFourEndAngle++;
            if (paramInfo.dwMifengRegionFourEndAngle == 360)
            {
                paramInfo.dwMifengRegionFourEndAngle = 0;
            }
            txt333.Text = paramInfo.dwMifengRegionFourEndAngle.ToString();
        }

        private void btnD1_XiangXianAll_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengSubRegionOuterOffset--;
            txt324.Text = paramInfo.dwMifengSubRegionOuterOffset.ToString();
        }

        private void btnD2_XiangXianAll_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengSubRegionInnerOffset--;
            txt325.Text = paramInfo.dwMifengSubRegionInnerOffset.ToString();
        }

        private void btnA1_XiangXianAll_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengSubRegionOuterOffset++;
            txt324.Text = paramInfo.dwMifengSubRegionOuterOffset.ToString();
        }

        private void btnA2_XiangXianAll_Click(object sender, EventArgs e)
        {
            paramInfo.dwMifengSubRegionInnerOffset++;
            txt325.Text = paramInfo.dwMifengSubRegionInnerOffset.ToString();
        }

        private void txt326_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengRegionOneStartAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengRegionOneStartAngle = (uint)i;
            if (graphicsShow_XiangXian[0] == null)
            {
                return;
            }
            graphicsShow_XiangXian[0].DrawImage(allPicture[6], 0, 0);
            drawOnePictureArea(0, paramInfo.dwMifengRegionOneStartAngle, paramInfo.dwMifengRegionOneEndAngle, paramInfo.dwMifengSubRegionOuterOffset, paramInfo.dwMifengSubRegionInnerOffset);
            pictureBox_XiangXian0.Image = bitmapShow_XiangXian[0];
        }

        private void txt327_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengRegionOneEndAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengRegionOneEndAngle = (uint)i;
            if (graphicsShow_XiangXian[0] == null)
            {
                return;
            }
            graphicsShow_XiangXian[0].DrawImage(allPicture[6], 0, 0);
            drawOnePictureArea(0, paramInfo.dwMifengRegionOneStartAngle, paramInfo.dwMifengRegionOneEndAngle, paramInfo.dwMifengSubRegionOuterOffset, paramInfo.dwMifengSubRegionInnerOffset);
            pictureBox_XiangXian0.Image = bitmapShow_XiangXian[0];
        }

        private void txt328_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengRegionTwoStartAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengRegionTwoStartAngle = (uint)i;
            if (graphicsShow_XiangXian[0] == null)
            {
                return;
            }
            graphicsShow_XiangXian[1].DrawImage(allPicture[7], 0, 0);
            drawOnePictureArea(1, paramInfo.dwMifengRegionTwoStartAngle, paramInfo.dwMifengRegionTwoEndAngle, paramInfo.dwMifengSubRegionOuterOffset, paramInfo.dwMifengSubRegionInnerOffset);
            pictureBox_XiangXian1.Image = bitmapShow_XiangXian[1];
        }

        private void txt329_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengRegionTwoEndAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengRegionTwoEndAngle = (uint)i;
            if (graphicsShow_XiangXian[0] == null)
            {
                return;
            }
            graphicsShow_XiangXian[1].DrawImage(allPicture[7], 0, 0);
            drawOnePictureArea(1, paramInfo.dwMifengRegionTwoStartAngle, paramInfo.dwMifengRegionTwoEndAngle, paramInfo.dwMifengSubRegionOuterOffset, paramInfo.dwMifengSubRegionInnerOffset);
            pictureBox_XiangXian1.Image = bitmapShow_XiangXian[1];
        }

        private void txt330_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengRegionThreeStartAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengRegionThreeStartAngle = (uint)i;
            if (graphicsShow_XiangXian[0] == null)
            {
                return;
            }
            graphicsShow_XiangXian[2].DrawImage(allPicture[8], 0, 0);
            drawOnePictureArea(2, paramInfo.dwMifengRegionThreeStartAngle, paramInfo.dwMifengRegionThreeEndAngle, paramInfo.dwMifengSubRegionOuterOffset, paramInfo.dwMifengSubRegionInnerOffset);
            pictureBox_XiangXian2.Image = bitmapShow_XiangXian[2];
        }

        private void txt331_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengRegionThreeEndAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengRegionThreeEndAngle = (uint)i;
            if (graphicsShow_XiangXian[0] == null)
            {
                return;
            }
            graphicsShow_XiangXian[2].DrawImage(allPicture[8], 0, 0);
            drawOnePictureArea(2, paramInfo.dwMifengRegionThreeStartAngle, paramInfo.dwMifengRegionThreeEndAngle, paramInfo.dwMifengSubRegionOuterOffset, paramInfo.dwMifengSubRegionInnerOffset);
            pictureBox_XiangXian2.Image = bitmapShow_XiangXian[2];
        }

        private void txt332_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengRegionFourStartAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengRegionFourStartAngle = (uint)i;
            if (graphicsShow_XiangXian[0] == null)
            {
                return;
            }
            graphicsShow_XiangXian[3].DrawImage(allPicture[9], 0, 0);
            drawOnePictureArea(3, paramInfo.dwMifengRegionFourStartAngle, paramInfo.dwMifengRegionFourEndAngle, paramInfo.dwMifengSubRegionOuterOffset, paramInfo.dwMifengSubRegionInnerOffset);
            pictureBox_XiangXian3.Image = bitmapShow_XiangXian[3];
        }

        private void txt333_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengRegionFourEndAngle.ToString();
                return;
            }
            if (i >= 360 || i < 0)
            {
                i = 0;
                txt.Text = i.ToString();
            }
            paramInfo.dwMifengRegionFourEndAngle = (uint)i;
            if (graphicsShow_XiangXian[0] == null)
            {
                return;
            }
            graphicsShow_XiangXian[3].DrawImage(allPicture[9], 0, 0);
            drawOnePictureArea(3, paramInfo.dwMifengRegionFourStartAngle, paramInfo.dwMifengRegionFourEndAngle, paramInfo.dwMifengSubRegionOuterOffset, paramInfo.dwMifengSubRegionInnerOffset);
            pictureBox_XiangXian3.Image = bitmapShow_XiangXian[3];
        }

        private void txt324_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengSubRegionOuterOffset.ToString();
                return;
            }
            paramInfo.dwMifengSubRegionOuterOffset = i;
            drawAllArea_XiangXian();
        }

        private void txt325_TextChanged(object sender, EventArgs e)
        {
            int i = 0;
            TextBox txt = (TextBox)sender;
            if (!int.TryParse(txt.Text, out i))
            {
                txt.Text = paramInfo.dwMifengSubRegionInnerOffset.ToString();
                return;
            }
            paramInfo.dwMifengSubRegionInnerOffset = i;
            drawAllArea_XiangXian();
        }

        #endregion 数据变动，需要重新画图

        #endregion 象限光区域

        #region 环形检测

        private void initComb_HXJC()
        {
            combPicSel.Items.Clear();
            for (int i = 0; i < 13; i++)
            {
                combPicSel.Items.Add("图片" + (i + 1).ToString());
            }
            combPicSel.SelectedIndex = 0;
            combHXSel.Items.Clear();
            for (int i = 0; i < Global.ringRegionNum; i++)
            {
                combHXSel.Items.Add("检测环" + (i + 1).ToString());
            }
            combHXSel.SelectedIndex = 0;


            findCirclePicSel.Items.Clear();
            for (int i = 0; i < 13; i++)
            {
                findCirclePicSel.Items.Add("图片" + (i + 1).ToString());
            }
            findCirclePicSel.SelectedIndex = 0;
            findCircle.Items.Clear();
            for (int i = 0; i < Global.findCircleNum; i++)
            {
                findCircle.Items.Add("找圆" + (i + 1).ToString());
            }
            findCircle.SelectedIndex = 0;

            pic_box.Dock = DockStyle.Fill;
            pic_box.showImg(null);
            grpb_PicBox.Controls.Add(pic_box);

            initCenCtrl(m_resultInfo.fOuterLoopMaxCenterX, m_resultInfo.fOuterLoopMaxCenterY);
        }

        private void initCenCtrl(double dx, double dy)
        {
            nud_CenX.Value = Convert.ToDecimal(dx);
            nud_CenY.Value = Convert.ToDecimal(dy);
        }
        private void saveFindCircleParam()
        {
            if (iFindCircleIndex < 0)
                return;
            if (m_duanMianAB == 0)
            {
                GlobalVariable.configSetting.duanMianAParam.circles[iFindCircleIndex].imageIndex = findCircle.SelectedIndex;
                GlobalVariable.configSetting.duanMianAParam.circles[iFindCircleIndex].Radius = (int)findCircleRadius.Value;
                GlobalVariable.configSetting.duanMianAParam.circles[iFindCircleIndex].length = (int)findCircleLength.Value;
                GlobalVariable.configSetting.duanMianAParam.circles[iFindCircleIndex].width = (int)findCircleWidth.Value;
                GlobalVariable.configSetting.duanMianAParam.circles[iFindCircleIndex].measureNum = (int)findCircleNum.Value;
                GlobalVariable.configSetting.duanMianAParam.circles[iFindCircleIndex].measureThreshold = (int)findCircleThreshold.Value;
                GlobalVariable.configSetting.duanMianAParam.circles[iFindCircleIndex].transition = (int)findCirclePolar.SelectedIndex;
                GlobalVariable.configSetting.duanMianAParam.circles[iFindCircleIndex].measureSelect = (int)findCircleLineSelect.SelectedIndex;
                GlobalVariable.configSetting.duanMianAParam.circles[iFindCircleIndex].enable = findCircleEnable.Checked;
            }else if (m_duanMianAB == 1)
            {
                GlobalVariable.configSetting.duanMianBParam.circles[iFindCircleIndex].imageIndex = findCircle.SelectedIndex;
                GlobalVariable.configSetting.duanMianBParam.circles[iFindCircleIndex].Radius = (int)findCircleRadius.Value;
                GlobalVariable.configSetting.duanMianBParam.circles[iFindCircleIndex].length = (int)findCircleLength.Value;
                GlobalVariable.configSetting.duanMianBParam.circles[iFindCircleIndex].width = (int)findCircleWidth.Value;
                GlobalVariable.configSetting.duanMianBParam.circles[iFindCircleIndex].measureNum = (int)findCircleNum.Value;
                GlobalVariable.configSetting.duanMianBParam.circles[iFindCircleIndex].measureThreshold = (int)findCircleThreshold.Value;
                GlobalVariable.configSetting.duanMianBParam.circles[iFindCircleIndex].transition = (int)findCirclePolar.SelectedIndex;
                GlobalVariable.configSetting.duanMianBParam.circles[iFindCircleIndex].measureSelect = (int)findCircleLineSelect.SelectedIndex;
                GlobalVariable.configSetting.duanMianBParam.circles[iFindCircleIndex].enable = findCircleEnable.Checked;
            }
        }
        private void saveCtrl_HXJC()
        {
            if (iCurHXIndex < 0)
                return;
            if (m_duanMianAB == 0)
            {
                GlobalVariable.configSetting.duanMianAParam.rings[iCurHXIndex].imageIndex = combPicSel.SelectedIndex;
                GlobalVariable.configSetting.duanMianAParam.rings[iCurHXIndex].MaxRadius = (float)nud_MaxRad.Value;
                GlobalVariable.configSetting.duanMianAParam.rings[iCurHXIndex].MinRadius = (float)nud_MinRad.Value;
                GlobalVariable.configSetting.duanMianAParam.rings[iCurHXIndex].maxGray = (int)nud_MaxGray.Value;
                GlobalVariable.configSetting.duanMianAParam.rings[iCurHXIndex].minGray = (int)nud_MinGray.Value;
                GlobalVariable.configSetting.duanMianAParam.rings[iCurHXIndex].maxArea = (int)nud_MaxArea.Value;
                GlobalVariable.configSetting.duanMianAParam.rings[iCurHXIndex].minArea = (int)nud_MinArea.Value;
                GlobalVariable.configSetting.duanMianAParam.rings[iCurHXIndex].startAngle = (int)nud_SrtAng.Value;
                GlobalVariable.configSetting.duanMianAParam.rings[iCurHXIndex].endAngle = (int)nud_EndAng.Value;
                GlobalVariable.configSetting.duanMianAParam.rings[iCurHXIndex].isFullCircle = ckbFullCircle.Checked;
                GlobalVariable.configSetting.duanMianAParam.rings[iCurHXIndex].enable = chkEnable.Checked;
                GlobalVariable.configSetting.duanMianAParam.floatParams[9 + iCurHXIndex] = (float)referPoint.SelectedIndex;
                GlobalVariable.configSetting.duanMianAParam.floatParams[25 + iCurHXIndex] = (float)referRadius.SelectedIndex;
            }
            else if (m_duanMianAB == 1)
            {
                GlobalVariable.configSetting.duanMianBParam.rings[iCurHXIndex].imageIndex = combPicSel.SelectedIndex;
                GlobalVariable.configSetting.duanMianBParam.rings[iCurHXIndex].MaxRadius = (float)nud_MaxRad.Value;
                GlobalVariable.configSetting.duanMianBParam.rings[iCurHXIndex].MinRadius = (float)nud_MinRad.Value;
                GlobalVariable.configSetting.duanMianBParam.rings[iCurHXIndex].maxGray = (int)nud_MaxGray.Value;
                GlobalVariable.configSetting.duanMianBParam.rings[iCurHXIndex].minGray = (int)nud_MinGray.Value;
                GlobalVariable.configSetting.duanMianBParam.rings[iCurHXIndex].maxArea = (int)nud_MaxArea.Value;
                GlobalVariable.configSetting.duanMianBParam.rings[iCurHXIndex].minArea = (int)nud_MinArea.Value;
                GlobalVariable.configSetting.duanMianBParam.rings[iCurHXIndex].startAngle = (int)nud_SrtAng.Value;
                GlobalVariable.configSetting.duanMianBParam.rings[iCurHXIndex].endAngle = (int)nud_EndAng.Value;
                GlobalVariable.configSetting.duanMianBParam.rings[iCurHXIndex].isFullCircle = ckbFullCircle.Checked;
                GlobalVariable.configSetting.duanMianBParam.rings[iCurHXIndex].enable = chkEnable.Checked;
                GlobalVariable.configSetting.duanMianBParam.floatParams[9 + iCurHXIndex] = (float)referPoint.SelectedIndex;
                GlobalVariable.configSetting.duanMianBParam.floatParams[25 + iCurHXIndex] = (float)referRadius.SelectedIndex;
            }
        }
        private void refreshReferRadius(int suferSelect,int PointSelect, int radiusSelect)
        {
            Surface_Result_Info resultInfo;

            if (suferSelect == 0)
            {
                resultInfo = GlobalVariable.duanMianAAutoFlow.info;
            }
            else
            {
                resultInfo = GlobalVariable.duanMianBAutoFlow.info;
            }
            if (PointSelect == 1)
            {
                if (radiusSelect == 0)
                {
                    m_referenceRadius = 0;
                }
                else if (radiusSelect == 1)
                {
                    m_referenceRadius = resultInfo.fInnerLoopMaxRadius;
                }
                else if (radiusSelect == 2)
                {
                    m_referenceRadius = resultInfo.fInnerLoopMinValidRadius;
                }
                else if (radiusSelect == 3)
                {
                    m_referenceRadius = resultInfo.fInnerLoopMinRadius;
                }
                else
                {
                    m_referenceRadius = 0;
                }
            }
            else
            {
                if (radiusSelect == 0)
                {
                    m_referenceRadius = 0;
                }
                else if (radiusSelect == 1)
                {
                    m_referenceRadius = resultInfo.fOuterLoopMaxRadius;
                }
                else if (radiusSelect == 2)
                {
                    m_referenceRadius = resultInfo.fOuterLoopMaxValidRadius;
                }
                else if (radiusSelect == 3)
                {
                    m_referenceRadius = resultInfo.fOuterLoopMinRadius;
                }
                else
                {
                    m_referenceRadius = 0;
                }
            }
            //if (suferSelect == 0)
            //{
            //    if (PointSelect == 1)
            //    {
            //        if (radiusSelect == 0)
            //        {
            //            m_referenceRadius = 0;
            //        }
            //        else if (radiusSelect == 1)
            //        {
            //            m_referenceRadius = resultInfo.fInnerLoopMaxRadius;
            //        }
            //        else if (radiusSelect == 2)
            //        {
            //            m_referenceRadius = resultInfo.fInnerLoopMinValidRadius;
            //        }
            //        else if (radiusSelect == 3)
            //        {
            //            m_referenceRadius = resultInfo.fInnerLoopMinRadius;
            //        }
            //        else
            //        {
            //            m_referenceRadius = 0;
            //        }
            //    }
            //    else
            //    {
            //        if (radiusSelect == 0)
            //        {
            //            m_referenceRadius = 0;
            //        }
            //        else if (radiusSelect == 1)
            //        {
            //            m_referenceRadius = resultInfo.fOuterLoopMaxRadius;
            //        }
            //        else if (radiusSelect == 2)
            //        {
            //            m_referenceRadius = resultInfo.fOuterLoopMaxValidRadius;
            //        }
            //        else if (radiusSelect == 3)
            //        {
            //            m_referenceRadius = resultInfo.fOuterLoopMinRadius;
            //        }
            //        else
            //        {
            //            m_referenceRadius = 0;
            //        }
            //    }
            //}
            //else
            //{
            //    if (PointSelect == 1)
            //    {
            //        if (radiusSelect == 0)
            //        {
            //            m_referenceRadius = 0;
            //        }
            //        else if (radiusSelect == 1)
            //        {
            //            m_referenceRadius = GlobalVariable.duanMianBAutoFlow.info.fInnerLoopMaxRadius;
            //        }
            //        else if (radiusSelect == 2)
            //        {
            //            m_referenceRadius = GlobalVariable.duanMianBAutoFlow.info.fInnerLoopMinValidRadius;
            //        }
            //        else if (radiusSelect == 3)
            //        {
            //            m_referenceRadius = GlobalVariable.duanMianBAutoFlow.info.fInnerLoopMinRadius;
            //        }
            //        else
            //        {
            //            m_referenceRadius = 0;
            //        }
            //    }
            //    else
            //    {
            //        if (radiusSelect == 0)
            //        {
            //            m_referenceRadius = 0;
            //        }
            //        else if (radiusSelect == 1)
            //        {
            //            m_referenceRadius = GlobalVariable.duanMianBAutoFlow.info.fOuterLoopMaxRadius;
            //        }
            //        else if (radiusSelect == 2)
            //        {
            //            m_referenceRadius = GlobalVariable.duanMianBAutoFlow.info.fOuterLoopMaxValidRadius;
            //        }
            //        else if (radiusSelect == 3)
            //        {
            //            m_referenceRadius = GlobalVariable.duanMianBAutoFlow.info.fOuterLoopMinRadius;
            //        }
            //        else
            //        {
            //            m_referenceRadius = 0;
            //        }
            //    }
            //}
        }
        private void initCtrl_HXJC()
        {
            int iHX = combHXSel.SelectedIndex;
            if(m_duanMianAB == 0)
            {
                m_paramAB = GlobalVariable.configSetting.duanMianAParam;
            }
            else
            {
                m_paramAB = GlobalVariable.configSetting.duanMianBParam;
            }
            combPicSel.SelectedIndex = m_paramAB.rings[iHX].imageIndex;
            nud_MaxRad.Value = Convert.ToDecimal(m_paramAB.rings[iHX].MaxRadius);
            nud_MinRad.Value = Convert.ToDecimal(m_paramAB.rings[iHX].MinRadius);
            nud_MaxGray.Value = m_paramAB.rings[iHX].maxGray;
            nud_MinGray.Value = m_paramAB.rings[iHX].minGray;
            nud_MaxArea.Value = m_paramAB.rings[iHX].maxArea;
            nud_MinArea.Value = m_paramAB.rings[iHX].minArea;
            nud_SrtAng.Value = m_paramAB.rings[iHX].startAngle;
            nud_EndAng.Value = m_paramAB.rings[iHX].endAngle;
            ckbFullCircle.Checked = m_paramAB.rings[iHX].isFullCircle;
            chkEnable.Checked = m_paramAB.rings[iHX].enable;
            referPoint.SelectedIndex = Convert.ToInt32(m_paramAB.floatParams[9 + iHX]);
            referRadius.SelectedIndex = Convert.ToInt32(m_paramAB.floatParams[25 + iHX]);
            int PointSelect = referPoint.SelectedIndex;
            int radiusSelect = referRadius.SelectedIndex;
            refreshReferRadius(m_duanMianAB, PointSelect, radiusSelect);
            //if (m_duanMianAB == 0)
            //{
            //    combPicSel.SelectedIndex = GlobalVariable.configSetting.duanMianAParam.rings[iHX].imageIndex;
            //    nud_MaxRad.Value = Convert.ToDecimal(GlobalVariable.configSetting.duanMianAParam.rings[iHX].MaxRadius);
            //    nud_MinRad.Value = Convert.ToDecimal(GlobalVariable.configSetting.duanMianAParam.rings[iHX].MinRadius);
            //    nud_MaxGray.Value = GlobalVariable.configSetting.duanMianAParam.rings[iHX].maxGray;
            //    nud_MinGray.Value = GlobalVariable.configSetting.duanMianAParam.rings[iHX].minGray;
            //    nud_MaxArea.Value = GlobalVariable.configSetting.duanMianAParam.rings[iHX].maxArea;
            //    nud_MinArea.Value = GlobalVariable.configSetting.duanMianAParam.rings[iHX].minArea;
            //    nud_SrtAng.Value = GlobalVariable.configSetting.duanMianAParam.rings[iHX].startAngle;
            //    nud_EndAng.Value = GlobalVariable.configSetting.duanMianAParam.rings[iHX].endAngle;
            //    ckbFullCircle.Checked = GlobalVariable.configSetting.duanMianAParam.rings[iHX].isFullCircle;
            //    chkEnable.Checked = GlobalVariable.configSetting.duanMianAParam.rings[iHX].enable;
            //    referPoint.SelectedIndex = Convert.ToInt32(GlobalVariable.configSetting.duanMianAParam.floatParams[9 + iHX]);
            //    referRadius.SelectedIndex = Convert.ToInt32(GlobalVariable.configSetting.duanMianAParam.floatParams[25 + iHX]);
            //    int PointSelect = referPoint.SelectedIndex;
            //    int radiusSelect = referRadius.SelectedIndex;
            //    refreshReferRadius(m_duanMianAB, PointSelect, radiusSelect);
            //}
            //else if (m_duanMianAB == 1)
            //{
            //    combPicSel.SelectedIndex = GlobalVariable.configSetting.duanMianBParam.rings[iHX].imageIndex;
            //    nud_MaxRad.Value = Convert.ToDecimal(GlobalVariable.configSetting.duanMianBParam.rings[iHX].MaxRadius);
            //    nud_MinRad.Value = Convert.ToDecimal(GlobalVariable.configSetting.duanMianBParam.rings[iHX].MinRadius);
            //    nud_MaxGray.Value = GlobalVariable.configSetting.duanMianBParam.rings[iHX].maxGray;
            //    nud_MinGray.Value = GlobalVariable.configSetting.duanMianBParam.rings[iHX].minGray;
            //    nud_MaxArea.Value = GlobalVariable.configSetting.duanMianBParam.rings[iHX].maxArea;
            //    nud_MinArea.Value = GlobalVariable.configSetting.duanMianBParam.rings[iHX].minArea;
            //    nud_SrtAng.Value = GlobalVariable.configSetting.duanMianBParam.rings[iHX].startAngle;
            //    nud_EndAng.Value = GlobalVariable.configSetting.duanMianBParam.rings[iHX].endAngle;
            //    ckbFullCircle.Checked = GlobalVariable.configSetting.duanMianBParam.rings[iHX].isFullCircle;
            //    chkEnable.Checked = GlobalVariable.configSetting.duanMianBParam.rings[iHX].enable;
            //    referPoint.SelectedIndex = Convert.ToInt32(GlobalVariable.configSetting.duanMianBParam.floatParams[9 + iHX]);
            //    referRadius.SelectedIndex = Convert.ToInt32(GlobalVariable.configSetting.duanMianBParam.floatParams[25 + iHX]);
            //    int PointSelect = referPoint.SelectedIndex;
            //    int radiusSelect = referRadius.SelectedIndex;
            //    refreshReferRadius(m_duanMianAB, PointSelect, radiusSelect);
            //}
        }

        private void initFindCircleParam()
        {
            int iHX = findCircle.SelectedIndex;
            if (m_duanMianAB == 0)
            {
                m_paramAB = GlobalVariable.configSetting.duanMianAParam;
            }
            else
            {
                m_paramAB = GlobalVariable.configSetting.duanMianBParam;
            }
            findCirclePicSel.SelectedIndex = m_paramAB.circles[iHX].imageIndex;
            findCircleRadius.Value = Convert.ToDecimal(m_paramAB.circles[iHX].Radius);
            findCircleLength.Value = m_paramAB.circles[iHX].length;
            findCircleWidth.Value = m_paramAB.circles[iHX].width;
            findCircleNum.Value = m_paramAB.circles[iHX].measureNum;
            findCircleThreshold.Value = m_paramAB.circles[iHX].measureThreshold;
            findCirclePolar.SelectedIndex = m_paramAB.circles[iHX].transition;
            findCircleLineSelect.SelectedIndex = m_paramAB.circles[iHX].measureSelect;
            findCircleEnable.Checked = m_paramAB.circles[iHX].enable;
        }

        private void combHXSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            saveCtrl_HXJC();
            initCtrl_HXJC();
            iCurHXIndex = cb.SelectedIndex;
            showHXPic();
        }
        private void findCircle_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            saveFindCircleParam();
            initFindCircleParam();
            iFindCircleIndex = cb.SelectedIndex;
        }

        private void zoomPic()
        {
            pic_box.zoomAll();
        }
        private void showHXPic()
        {
            int ipic = combPicSel.SelectedIndex;
            Bitmap bmp0 = allPicture[ipic];
            if (bmp0 == null)
                return;

            //if (nud_CenX.Value < 0)
            //{
            //    nud_CenX.Value = bmp0.Width / 2;
            //}
            //if (nud_CenY.Value < 0)
            //{
            //    nud_CenY.Value = bmp0.Height / 2;
            //}

            float fCenX = (float)nud_CenX.Value;
            float fCenY = (float)nud_CenY.Value;
            float fMaxRad = (float)nud_MaxRad.Value + m_referenceRadius;
            float fMinRad = (float)nud_MinRad.Value + m_referenceRadius;
            int iSrtAng = (int)nud_SrtAng.Value;
            int iEndAng = (int)nud_EndAng.Value;
            int lineWidth = (int)LineWidth.Value;
            bool isFullCircle = ckbFullCircle.Checked;
            Bitmap bmp = new Bitmap(bmp0.Width, bmp0.Height);
            Graphics grp = Graphics.FromImage(bmp);
            grp.DrawImage(bmp0, new Point(0, 0));
            if (isFullCircle)
            {
                grp.DrawEllipse(new Pen(Color.Green, lineWidth), fCenX - fMaxRad, fCenY - fMaxRad, fMaxRad * 2, fMaxRad * 2);
                grp.DrawEllipse(new Pen(Color.Green, lineWidth), fCenX - fMinRad, fCenY - fMinRad, fMinRad * 2, fMinRad * 2);
            }
            else
            {
                if (iEndAng - iSrtAng > 0 && fMaxRad > 0 && fMinRad > 0)
                {
                    grp.DrawArc(new Pen(Color.Green, lineWidth), fCenX - fMaxRad, fCenY - fMaxRad, fMaxRad * 2, fMaxRad * 2, -iEndAng, iEndAng - iSrtAng);
                    grp.DrawArc(new Pen(Color.Green, lineWidth), fCenX - fMinRad, fCenY - fMinRad, fMinRad * 2, fMinRad * 2, -iEndAng, iEndAng - iSrtAng);
                }

                PointF start1 = new PointF();
                start1.X = fCenX + (float)Math.Cos(-iSrtAng * Math.PI / 180) * fMaxRad;
                start1.Y = fCenY + (float)Math.Sin(-iSrtAng * Math.PI / 180) * fMaxRad;
                PointF start2 = new PointF();
                start2.X = fCenX + (float)Math.Cos(-iSrtAng * Math.PI / 180) * fMinRad;
                start2.Y = fCenY + (float)Math.Sin(-iSrtAng * Math.PI / 180) * fMinRad;
                grp.DrawLine(new Pen(Color.Green, 2), start1, start2);

                PointF end1 = new PointF();
                end1.X = fCenX + (float)Math.Cos(-iEndAng * Math.PI / 180) * fMaxRad;
                end1.Y = fCenY + (float)Math.Sin(-iEndAng * Math.PI / 180) * fMaxRad;
                PointF end2 = new PointF();
                end2.X = fCenX + (float)Math.Cos(-iEndAng * Math.PI / 180) * fMinRad;
                end2.Y = fCenY + (float)Math.Sin(-iEndAng * Math.PI / 180) * fMinRad;
                grp.DrawLine(new Pen(Color.Green, lineWidth), end1, end2);
            }
            float radiusCircle = (float)nud_MaxRad.Value;
            if(showCircle.Checked)
            {
                grp.DrawEllipse(new Pen(Color.Blue, lineWidth), fCenX - radiusCircle, fCenY - radiusCircle, radiusCircle * 2, radiusCircle * 2);
            }

            pic_box.showImg(bmp);
        }

        private void nud_CenX_ValueChanged(object sender, EventArgs e)
        {
            showHXPic();
        }

        private void ckbFullCircle_CheckedChanged(object sender, EventArgs e)
        {
            showHXPic();
        }

        private void combPicSel_SelectedIndexChanged(object sender, EventArgs e)
        {
            showHXPic();
        }

        #endregion 环形检测

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value > 360)
            {
                numericUpDown1.Value = numericUpDown1.Value % 360;
            }
            if (numericUpDown1.Value < 0)
            {
                numericUpDown1.Value = numericUpDown1.Value % 360 + 360;
            }
            paramInfo.dwMifengTextRegionOneStartAngle = (uint)numericUpDown1.Value;
            drawAllArea_WenZi();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown2.Value > 360)
            {
                numericUpDown2.Value = numericUpDown2.Value % 360;
            }
            if (numericUpDown2.Value < 0)
            {
                numericUpDown2.Value = numericUpDown2.Value % 360 + 360;
            }
            paramInfo.dwMifengTextRegionOneEndAngle = (uint)numericUpDown2.Value;
            drawAllArea_WenZi();
        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            paramInfo.dwMifengTextRegionOneOuterOffset = (int)numericUpDown3.Value;
            drawAllArea_WenZi();
        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {
            paramInfo.dwMifengTextRegionOneInnerOffset = (int)numericUpDown4.Value;
            drawAllArea_WenZi();
        }

        private void numericUpDown5_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown5.Value > 360)
            {
                numericUpDown5.Value = numericUpDown5.Value % 360;
            }
            if (numericUpDown5.Value < 0)
            {
                numericUpDown5.Value = numericUpDown5.Value % 360 + 360;
            }
            paramInfo.dwMifengTextRegionTwoStartAngle = (uint)numericUpDown5.Value;
            drawAllArea_WenZi();
        }

        private void numericUpDown6_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown6.Value > 360)
            {
                numericUpDown6.Value = numericUpDown6.Value % 360;
            }
            if (numericUpDown6.Value < 0)
            {
                numericUpDown6.Value = numericUpDown6.Value % 360 + 360;
            }
            paramInfo.dwMifengTextRegionTwoEndAngle = (uint)numericUpDown6.Value;
            drawAllArea_WenZi();
        }

        private void numericUpDown7_ValueChanged(object sender, EventArgs e)
        {
            paramInfo.dwMifengTextRegionTwoOuterOffset = (int)numericUpDown7.Value;
            drawAllArea_WenZi();
        }

        private void numericUpDown8_ValueChanged(object sender, EventArgs e)
        {
            paramInfo.dwMifengTextRegionTwoInnerOffset = (int)numericUpDown8.Value;
            drawAllArea_WenZi();
        }

        private void numericUpDown9_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown9.Value > 360)
            {
                numericUpDown9.Value = numericUpDown9.Value % 360;
            }
            if (numericUpDown9.Value < 0)
            {
                numericUpDown9.Value = numericUpDown9.Value % 360 + 360;
            }
            paramInfo.dwMifengTextRegionThreeStartAngle = (uint)numericUpDown9.Value;
            drawAllArea_WenZi();
        }

        private void numericUpDown10_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown10.Value > 360)
            {
                numericUpDown10.Value = numericUpDown10.Value % 360;
            }
            if (numericUpDown10.Value < 0)
            {
                numericUpDown10.Value = numericUpDown10.Value % 360 + 360;
            }
            paramInfo.dwMifengTextRegionThreeEndAngle = (uint)numericUpDown10.Value;
            drawAllArea_WenZi();
        }

        private void numericUpDown11_ValueChanged(object sender, EventArgs e)
        {
            paramInfo.dwMifengTextRegionThreeOuterOffset = (int)numericUpDown11.Value;
            drawAllArea_WenZi();
        }

        private void numericUpDown12_ValueChanged(object sender, EventArgs e)
        {
            paramInfo.dwMifengTextRegionThreeInnerOffset = (int)numericUpDown12.Value;
            drawAllArea_WenZi();
        }

        private void numericUpDown13_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown13.Value > 360)
            {
                numericUpDown13.Value = numericUpDown13.Value % 360;
            }
            if (numericUpDown13.Value < 0)
            {
                numericUpDown13.Value = numericUpDown13.Value % 360 + 360;
            }
            paramInfo.dwMifengTextRegionFourStartAngle = (uint)numericUpDown13.Value;
            drawAllArea_WenZi();
        }

        private void numericUpDown14_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown14.Value > 360)
            {
                numericUpDown14.Value = numericUpDown14.Value % 360;
            }
            if (numericUpDown14.Value < 0)
            {
                numericUpDown14.Value = numericUpDown14.Value % 360 + 360;
            }
            paramInfo.dwMifengTextRegionFourEndAngle = (uint)numericUpDown14.Value;
            drawAllArea_WenZi();
        }

        private void numericUpDown15_ValueChanged(object sender, EventArgs e)
        {
            paramInfo.dwMifengTextRegionFourOuterOffset = (int)numericUpDown15.Value;
            drawAllArea_WenZi();
        }

        private void numericUpDown16_ValueChanged(object sender, EventArgs e)
        {
            paramInfo.dwMifengTextRegionFourInnerOffset = (int)numericUpDown16.Value;
            drawAllArea_WenZi();
        }

        private void nud_TextDown_ValueChanged(object sender, EventArgs e)
        {
            if (nud_TextDown.Value > 255)
            {
                nud_TextDown.Value = nud_TextDown.Value % 255;
            }
            if (nud_TextDown.Value < 0)
            {
                nud_TextDown.Value = nud_TextDown.Value % 255 + 255;
            }
            paramInfo.textDown = (int)nud_TextDown.Value;
            if (paramInfo.textDown > paramInfo.textUp)
            {
                paramInfo.textUp = paramInfo.textDown;
                nud_TextUp.Value = paramInfo.textUp;
            }
        }

        private void nud_TextUp_ValueChanged(object sender, EventArgs e)
        {
            if (nud_TextUp.Value > 255)
            {
                nud_TextUp.Value = nud_TextUp.Value % 255;
            }
            if (nud_TextUp.Value < 0)
            {
                nud_TextUp.Value = nud_TextUp.Value % 255 + 255;
            }
            paramInfo.textUp = (int)nud_TextUp.Value;
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            m_editEnable = !m_editEnable;

            txt1.Enabled = m_editEnable;
            txt2.Enabled = m_editEnable;
            txt3.Enabled = m_editEnable;
            txt4.Enabled = m_editEnable;
            txt5.Enabled = m_editEnable;
            txt7.Enabled = m_editEnable;
            txt8.Enabled = m_editEnable;

            txt21.Enabled = m_editEnable;
            txt22.Enabled = m_editEnable;
            txt23.Enabled = m_editEnable;
            txt24.Enabled = m_editEnable;
            txt25.Enabled = m_editEnable;
            txt27.Enabled = m_editEnable;
            txt28.Enabled = m_editEnable;
            txt29.Enabled = m_editEnable;
            txt41.Enabled = m_editEnable;
            txt42.Enabled = m_editEnable;
            txt43.Enabled = m_editEnable;

            txt51.Enabled = m_editEnable;
            txt52.Enabled = m_editEnable;
            txt53.Enabled = m_editEnable;
            txt54.Enabled = m_editEnable;
            txt55.Enabled = m_editEnable;
            txt57.Enabled = m_editEnable;
            txt58.Enabled = m_editEnable;

            txt71.Enabled = m_editEnable;
            txt72.Enabled = m_editEnable;
            txt73.Enabled = m_editEnable;
            txt74.Enabled = m_editEnable;
            txt75.Enabled = m_editEnable;
            txt76.Enabled = m_editEnable;

            txt77.Enabled = m_editEnable;
            txt78.Enabled = m_editEnable;
            txt79.Enabled = m_editEnable;
            txt80.Enabled = m_editEnable;
            txt81.Enabled = m_editEnable;
            txt82.Enabled = m_editEnable;

            txt83.Enabled = m_editEnable;
            txt84.Enabled = m_editEnable;
            txt85.Enabled = m_editEnable;
            txt86.Enabled = m_editEnable;
            txt87.Enabled = m_editEnable;
            txt88.Enabled = m_editEnable;

            txt91.Enabled = m_editEnable;
            txt92.Enabled = m_editEnable;
        }

        private void groupBox1_Leave(object sender, EventArgs e)
        {
            refreshModel();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            refreshCenterPoint();
        }

        private void referRadius_SelectedIndexChanged(object sender, EventArgs e)
        {
            int PointSelect = referPoint.SelectedIndex;
            int radiusSelect = referRadius.SelectedIndex;
            refreshReferRadius(m_duanMianAB, PointSelect, radiusSelect);
        }

        private void refreshCenterPoint()
        {
            if (m_duanMianAB == 0)
            {
                if (referPoint.SelectedIndex == 1)
                {
                    initCenCtrl(m_resultInfo.fInnerLoopMaxCenterX, m_resultInfo.fInnerLoopMaxCenterY);
                }
                else
                {
                    initCenCtrl(m_resultInfo.fOuterLoopMinCenterX, m_resultInfo.fOuterLoopMinCenterY);
                }
            }
            else
            {
                if (referPoint.SelectedIndex == 1)
                {
                    initCenCtrl(GlobalVariable.duanMianBAutoFlow.info.fInnerLoopMaxCenterX, GlobalVariable.duanMianBAutoFlow.info.fInnerLoopMaxCenterY);
                }
                else
                {
                    initCenCtrl(GlobalVariable.duanMianBAutoFlow.info.fOuterLoopMinCenterX, GlobalVariable.duanMianBAutoFlow.info.fOuterLoopMinCenterY);
                }
            }
        }

        private void showCircle_CheckedChanged(object sender, EventArgs e)
        {
            showHXPic();
        }

        private void LineWidth_ValueChanged(object sender, EventArgs e)
        {
            showHXPic();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }
    }
}