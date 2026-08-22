using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class CameraDebugForm : Form
    {
        public bool startSign = false;
        public bool selectChange = false;
        public string indexWorkStation = "";
        public int m_indexWorkStation = 0;
        public int m_PictureIndex = 0;
        private int plcErrorCode = 0;

        IAreaScanCameraHelper[] m_cameraHelper = { GlobalVariable.DuanMianACameraHelper,
                                                     GlobalVariable.WaiYuanACameraHelper,
                                                     GlobalVariable.DaoJiaoACameraHelper,
                                                     GlobalVariable.NeiKongCameraHelper,
                                                     GlobalVariable.DuanMianBCameraHelper,
                                                     GlobalVariable.WaiYuanBCameraHelper,
                                                     GlobalVariable.DaoJiaoBCameraHelper};
        CameraPhotoSetting[] m_photoSetting = { GlobalVariable.configSetting.duanMianACameraPhotoSetting,
                                                  GlobalVariable.configSetting.waiYuanACameraPhotoSetting,
                                                  GlobalVariable.configSetting.daoJiaoACameraPhotoSetting,
                                                  GlobalVariable.configSetting.neiKongCameraPhotoSetting,
                                                  GlobalVariable.configSetting.duanMianBCameraPhotoSetting,
                                                  GlobalVariable.configSetting.waiYuanBCameraPhotoSetting,
                                                  GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting };

          CameraPhotoSetting[] m_photoSettingTmp =  { GlobalVariable.TmpConfigSetting.duanMianACameraPhotoSetting,
                                                      GlobalVariable.TmpConfigSetting.waiYuanACameraPhotoSetting,
                                                      GlobalVariable.TmpConfigSetting.daoJiaoACameraPhotoSetting,
                                                      GlobalVariable.TmpConfigSetting.neiKongCameraPhotoSetting,
                                                      GlobalVariable.TmpConfigSetting.duanMianBCameraPhotoSetting,
                                                      GlobalVariable.TmpConfigSetting.waiYuanBCameraPhotoSetting,
                                                      GlobalVariable.TmpConfigSetting.daoJiaoBCameraPhotoSetting};

        public CameraDebugForm()
        {
            InitializeComponent();
            startSign = false;
            Thread th = new Thread(takeImageThread);
            th.IsBackground = true;
            th.Start();
            lblGray.Parent = picBox;
            lblGray.BackColor = Color.White;
        }

        private void CameraDebugForm_Load(object sender, EventArgs e)
        {
            if (GlobalVariable.iWorkStation == 2)
            {
                cmbStationCheck.Items.Add("端面A");
                cmbStationCheck.Items.Add("端面B");
            }
           else if(GlobalVariable.iWorkStation == 5)
            {
                cmbStationCheck.Items.Add("端面A");
                cmbStationCheck.Items.Add("外圆A");
                cmbStationCheck.Items.Add("内孔");
                cmbStationCheck.Items.Add("端面B");
                cmbStationCheck.Items.Add("外圆B");
            }

            else
            {
                cmbStationCheck.Items.Add("端面A");
                cmbStationCheck.Items.Add("外圆A");
                cmbStationCheck.Items.Add("倒角A");
                cmbStationCheck.Items.Add("内孔");
                cmbStationCheck.Items.Add("端面B");
                cmbStationCheck.Items.Add("外圆B");
                cmbStationCheck.Items.Add("倒角B");
            }
            cmbStationCheck.SelectedIndex = 0;

            trackBarLight1.Minimum = 0;
            trackBarLight1.Maximum = 255;
            trackBarLight1.SmallChange = 10;
            trackBarLight1.TickFrequency = 100;
            trackBarLight2.Minimum = 0;
            trackBarLight2.Maximum = 255;
            trackBarLight2.SmallChange = 10;
            trackBarLight2.TickFrequency = 100;
            trackBarLight3.Minimum = 0;
            trackBarLight3.Maximum = 255;
            trackBarLight3.SmallChange = 10;
            trackBarLight3.TickFrequency = 100;
            trackBarLight4.Minimum = 0;
            trackBarLight4.Maximum = 255;
            trackBarLight4.SmallChange = 10;
            trackBarLight4.TickFrequency = 100;
            trackBarLight5.Minimum = 0;
            trackBarLight5.Maximum = 255;
            trackBarLight5.SmallChange = 10;
            trackBarLight5.TickFrequency = 100;
            trackBarLight6.Minimum = 0;
            trackBarLight6.Maximum = 255;
            trackBarLight6.SmallChange = 10;
            trackBarLight6.TickFrequency = 100;
            trackBarLight7.Minimum = 0;
            trackBarLight7.Maximum = 255;
            trackBarLight7.SmallChange = 10;
            trackBarLight7.TickFrequency = 100;

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

        #region 工位和图片位置选项卡

        private void cmbStationCheck_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectChange = true;
            indexWorkStation = cmbStationCheck.Text;
            SetControlEnable(indexWorkStation);
            startSign = false;
            btnTakePicture.Text = "开始抓拍";
            groupBox2.Enabled = true;
            #region
            if (indexWorkStation == "端面A")
            {
                m_indexWorkStation = 0;
            }
            else if (indexWorkStation == "外圆A")
            {
                m_indexWorkStation = 1;
            }
            else if (indexWorkStation == "倒角A")
            {
                m_indexWorkStation = 2;
            }
            else if (indexWorkStation == "内孔")
            {
                m_indexWorkStation = 3;
            }
            else if (indexWorkStation == "端面B")
            {
                m_indexWorkStation = 4;
            }
            else if (indexWorkStation == "外圆B")
            {
                m_indexWorkStation = 5;
            }
            else if (indexWorkStation == "倒角B")
            {
                m_indexWorkStation = 6;
            }
            #endregion
            if (m_indexWorkStation == 0|| m_indexWorkStation == 4)
            {
                groupBox1.Enabled = true;
                cmbPictureCheck.Visible = true;
                picLab.Visible = true;

                cmbPictureCheck.Items.Clear();
                cmbPictureCheck.Items.Add("1:背光图片");
                cmbPictureCheck.Items.Add("2:高环光图片");
                cmbPictureCheck.Items.Add("3:盖子文字图片");
                cmbPictureCheck.Items.Add("4:高位低角度环光高位图片");
                cmbPictureCheck.Items.Add("5:盖子区域图片");
                cmbPictureCheck.Items.Add("6:其他功能图片");
                cmbPictureCheck.Items.Add("7:右上光图片");
                cmbPictureCheck.Items.Add("8:左上光图片");
                cmbPictureCheck.Items.Add("9:左下光图片");
                cmbPictureCheck.Items.Add("10:右下光图片");
                cmbPictureCheck.Items.Add("11:低角度环光低位图片");
                cmbPictureCheck.Items.Add("12:内圈区域四图片");
                cmbPictureCheck.Items.Add("13:外圈区域四图片");
                cmbPictureCheck.SelectedIndex = 0;
                ShowDataByLightCMDAB(m_photoSetting[m_indexWorkStation].exposure[0], m_photoSetting[m_indexWorkStation].lightsCmd[0]);
                
            }else
            {
                m_PictureIndex = 0;
                groupBox1.Enabled = false;
                cmbPictureCheck.Visible = false;
                picLab.Visible = false;
            }
            nud_Gain.Value = (int)m_photoSetting[m_indexWorkStation].gain[0];
            nud_ExposureTime.Value = (int)m_photoSetting[m_indexWorkStation].exposure[0];
            txtWidthMax.Text = 5000.ToString();
            txtHeightMax.Text = 2500.ToString();
            txtWidthOffset.Text = m_photoSetting[m_indexWorkStation].OffsetX.ToString();
            txtWidthTrue.Text = m_photoSetting[m_indexWorkStation].ROIWidthTrue.ToString();
            txtHeightOffset.Text = m_photoSetting[m_indexWorkStation].OffsetY.ToString();
            txtHeightTrue.Text = m_photoSetting[m_indexWorkStation].ROIHeihgtTrue.ToString();
            txtHeightOffset.Enabled = true;
            txtXSSpeed.Enabled = false;
            if (m_indexWorkStation == 1)
            {
                txtHeightOffset.Enabled = false;
                txtXSSpeed.Enabled = true;
                txtXSSpeed.Text = GlobalVariable.configSetting.waiYuanACameraPhotoSetting.xsSpeed.ToString();
            }
            #region
            /*
            if (indexWorkStation == "端面A")
            {
                groupBox1.Enabled = true;
                groupBox2.Visible = true;
                panel1.Visible = true;
                btnSaveROI.Visible = true;
                btnTakePicture.Visible = true;
                cmbPictureCheck.Visible = true;
                picLab.Visible = true;

                cmbPictureCheck.Items.Clear();
                cmbPictureCheck.Items.Add("1:背光图片");
                cmbPictureCheck.Items.Add("2:高环光图片");
                cmbPictureCheck.Items.Add("3:盖子文字图片");
                cmbPictureCheck.Items.Add("4:高位低角度环光高位图片");
                cmbPictureCheck.Items.Add("5:盖子区域图片");
                cmbPictureCheck.Items.Add("6:其他功能图片");
                cmbPictureCheck.Items.Add("7:右上光图片");
                cmbPictureCheck.Items.Add("8:左上光图片");
                cmbPictureCheck.Items.Add("9:左下光图片");
                cmbPictureCheck.Items.Add("10:右下光图片");
                cmbPictureCheck.Items.Add("11:低角度环光低位图片");
                cmbPictureCheck.Items.Add("12:内圈区域四图片");
                cmbPictureCheck.Items.Add("13:外圈区域四图片");
                cmbPictureCheck.SelectedIndex = 0;
                ShowDataByLightCMDAB(GlobalVariable.configSetting.duanMianACameraPhotoSetting.exposure[0], GlobalVariable.configSetting.duanMianACameraPhotoSetting.lightsCmd[0]);
                nud_Gain.Value = (int)GlobalVariable.configSetting.duanMianACameraPhotoSetting.gain[0];
                txtWidthMax.Text = 5000.ToString();
                txtWidthOffset.Text = GlobalVariable.configSetting.duanMianACameraPhotoSetting.OffsetX.ToString();
                txtWidthTrue.Text = GlobalVariable.configSetting.duanMianACameraPhotoSetting.ROIWidthTrue.ToString();
                txtHeightMax.Text = 2500.ToString();
                txtHeightOffset.Text = GlobalVariable.configSetting.duanMianACameraPhotoSetting.OffsetY.ToString();
                txtHeightTrue.Text = GlobalVariable.configSetting.duanMianACameraPhotoSetting.ROIHeihgtTrue.ToString();

                txtHeightOffset.Enabled = true;
            }
            else if (indexWorkStation == "外圆A")
            {
                m_PictureIndex = 0;
                groupBox1.Enabled = false;
                cmbPictureCheck.Visible = false;
                picLab.Visible = false;

                txtWidthMax.Text = 2500.ToString();
                txtWidthOffset.Text = GlobalVariable.configSetting.waiYuanACameraPhotoSetting.OffsetX.ToString();
                txtWidthTrue.Text = GlobalVariable.configSetting.waiYuanACameraPhotoSetting.ROIWidthTrue.ToString();
                txtHeightMax.Text = 5000.ToString();
                txtHeightOffset.Text = GlobalVariable.configSetting.waiYuanACameraPhotoSetting.OffsetY.ToString();
                txtHeightTrue.Text = GlobalVariable.configSetting.waiYuanACameraPhotoSetting.ROIHeihgtTrue.ToString();
                txtXSSpeed.Text = GlobalVariable.configSetting.waiYuanACameraPhotoSetting.xsSpeed.ToString();
                nud_ExposureTime.Value = (int)GlobalVariable.configSetting.waiYuanACameraPhotoSetting.exposure[0];
                nud_Gain.Value = (int)GlobalVariable.configSetting.waiYuanACameraPhotoSetting.gain[0];
            }
            else if (indexWorkStation == "倒角A")
            {
                m_PictureIndex = 0;
                groupBox1.Enabled = false;
                cmbPictureCheck.Visible = false;
                picLab.Visible = false;

                txtWidthMax.Text = 2500.ToString();
                txtWidthOffset.Text = GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.OffsetX.ToString();
                txtWidthTrue.Text = GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.ROIWidthTrue.ToString();
                txtHeightMax.Text = 5000.ToString();
                txtHeightOffset.Text = GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.OffsetY.ToString();
                txtHeightTrue.Text = GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.ROIHeihgtTrue.ToString();
                nud_ExposureTime.Value = (int)GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.exposure[0];
                nud_Gain.Value = (int)GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.gain[0];
            }
            else if (indexWorkStation == "内孔")
            {
                m_PictureIndex = 0;
                groupBox1.Enabled = false;
                cmbPictureCheck.Visible = false;
                picLab.Visible = false;

                txtWidthMax.Text = 2500.ToString();
                txtWidthOffset.Text = GlobalVariable.configSetting.neiKongCameraPhotoSetting.OffsetX.ToString();
                txtWidthTrue.Text = GlobalVariable.configSetting.neiKongCameraPhotoSetting.ROIWidthTrue.ToString();
                txtHeightMax.Text = 5000.ToString();
                txtHeightOffset.Text = GlobalVariable.configSetting.neiKongCameraPhotoSetting.OffsetY.ToString();
                txtHeightTrue.Text = GlobalVariable.configSetting.neiKongCameraPhotoSetting.ROIHeihgtTrue.ToString();
                nud_ExposureTime.Value = (int)GlobalVariable.configSetting.neiKongCameraPhotoSetting.exposure[0];
                nud_Gain.Value = (int)GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.gain[0];
            }
            else if (indexWorkStation == "端面B")
            {
                groupBox1.Enabled = true;
                groupBox2.Visible = true;
                panel1.Visible = true;
                btnSaveROI.Visible = true;
                btnTakePicture.Visible = true;
                cmbPictureCheck.Visible = true;
                picLab.Visible = true;

                cmbPictureCheck.Items.Clear();
                cmbPictureCheck.Items.Add("1:背光图片");
                cmbPictureCheck.Items.Add("2:高环光图片");
                cmbPictureCheck.Items.Add("3:盖子文字图片");
                cmbPictureCheck.Items.Add("4:高位低角度环光高位图片");
                cmbPictureCheck.Items.Add("5:盖子区域图片");
                cmbPictureCheck.Items.Add("6:其他功能图片");
                cmbPictureCheck.Items.Add("7:右上光图片");
                cmbPictureCheck.Items.Add("8:左上光图片");
                cmbPictureCheck.Items.Add("9:左下光图片");
                cmbPictureCheck.Items.Add("10:右下光图片");
                cmbPictureCheck.Items.Add("11:低角度环光低位图片");
                cmbPictureCheck.Items.Add("12:内圈区域四图片");
                cmbPictureCheck.Items.Add("13:外圈区域四图片");
                cmbPictureCheck.SelectedIndex = 0;
                ShowDataByLightCMDAB(GlobalVariable.configSetting.duanMianBCameraPhotoSetting.exposure[0], GlobalVariable.configSetting.duanMianBCameraPhotoSetting.lightsCmd[0]);
                nud_Gain.Value = (int)GlobalVariable.configSetting.duanMianBCameraPhotoSetting.gain[0];
                txtWidthMax.Text = 5000.ToString();
                txtWidthOffset.Text = GlobalVariable.configSetting.duanMianBCameraPhotoSetting.OffsetX.ToString();
                txtWidthTrue.Text = GlobalVariable.configSetting.duanMianBCameraPhotoSetting.ROIWidthTrue.ToString();
                txtHeightMax.Text = 2500.ToString();
                txtHeightOffset.Text = GlobalVariable.configSetting.duanMianBCameraPhotoSetting.OffsetY.ToString();
                txtHeightTrue.Text = GlobalVariable.configSetting.duanMianBCameraPhotoSetting.ROIHeihgtTrue.ToString();
                txtHeightOffset.Enabled = true;
            }
            else if (indexWorkStation == "外圆B")
            {
                m_PictureIndex = 0;
                groupBox1.Enabled = false;
                cmbPictureCheck.Visible = false;
                picLab.Visible = false;

                txtWidthMax.Text = 2500.ToString();
                txtWidthOffset.Text = GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.OffsetX.ToString();
                txtWidthTrue.Text = GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.ROIWidthTrue.ToString();
                txtHeightMax.Text = 5000.ToString();
                txtHeightOffset.Text = GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.OffsetY.ToString();
                txtHeightTrue.Text = GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.ROIHeihgtTrue.ToString();
                nud_ExposureTime.Value = (int)GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.exposure[0];
                nud_Gain.Value = (int)GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.gain[0];
            }
            else if (indexWorkStation == "倒角B")
            {
                m_PictureIndex = 0;
                groupBox1.Enabled = false;
                cmbPictureCheck.Visible = false;
                picLab.Visible = false;

                txtWidthMax.Text = 2500.ToString();
                txtWidthOffset.Text = GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.OffsetX.ToString();
                txtWidthTrue.Text = GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.ROIWidthTrue.ToString();
                txtHeightMax.Text = 5000.ToString();
                txtHeightOffset.Text = GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.OffsetY.ToString();
                txtHeightTrue.Text = GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.ROIHeihgtTrue.ToString();
                nud_ExposureTime.Value = (int)GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.exposure[0];
                nud_Gain.Value = (int)GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.gain[0];
            }
            */
            #endregion
        }

        private void cmbPictureCheck_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectChange = true;
            m_PictureIndex = cmbPictureCheck.SelectedIndex;
            if (indexWorkStation == "端面A")
            {
                ShowDataByLightCMDAB(GlobalVariable.configSetting.duanMianACameraPhotoSetting.exposure[m_PictureIndex], GlobalVariable.configSetting.duanMianACameraPhotoSetting.lightsCmd[m_PictureIndex]);
                nud_Gain.Value = (int)GlobalVariable.configSetting.duanMianACameraPhotoSetting.gain[m_PictureIndex];
                if (m_PictureIndex == 0)
                {
                    groupBox2.Enabled = true;
                }
                else
                {
                    groupBox2.Enabled = false;
                }
            }
            else if (indexWorkStation == "端面B")
            {
                ShowDataByLightCMDAB(GlobalVariable.configSetting.duanMianBCameraPhotoSetting.exposure[m_PictureIndex], GlobalVariable.configSetting.duanMianBCameraPhotoSetting.lightsCmd[m_PictureIndex]);
                nud_Gain.Value = (int)GlobalVariable.configSetting.duanMianBCameraPhotoSetting.gain[m_PictureIndex];
                if (m_PictureIndex == 0)
                {
                    groupBox2.Enabled = true;
                }
                else
                {
                    groupBox2.Enabled = false;
                }
            }
        }

        /// <summary>
        /// 通过曝光值，光源的CMD指令显示于界面上
        /// </summary>
        /// <param name="cmd"></param>
        public void ShowDataByLightCMDAB(double exposure, string cmd)
        {
            if (cmd == null)
            {
                return;
            }
            nud_ExposureTime.Value = (int)exposure;
            string[] str = cmd.Split('#');
            int light1 = Convert.ToInt32(str[0].Substring(3));
            txtLight1.Text = light1.ToString();
            trackBarLight1.Value = light1;
            if (light1 > 0)
            {
                ckbLight1.Checked = true;
                txtLight1.Enabled = true;
                trackBarLight1.Enabled = true;
            }
            else
            {
                ckbLight1.Checked = false;
                txtLight1.Enabled = false;
                trackBarLight1.Enabled = false;
            }

            int light2 = Convert.ToInt32(str[1].Substring(3));
            txtLight2.Text = light2.ToString();
            trackBarLight2.Value = light2;
            if (light2 > 0)
            {
                ckbLight2.Checked = true;
                txtLight2.Enabled = true;
                trackBarLight2.Enabled = true;
            }
            else
            {
                ckbLight2.Checked = false;
                txtLight2.Enabled = false;
                trackBarLight2.Enabled = false;
            }

            int light3 = Convert.ToInt32(str[2].Substring(3));
            txtLight3.Text = light3.ToString();
            trackBarLight3.Value = light3;
            if (light3 > 0)
            {
                ckbLight3.Checked = true;
                txtLight3.Enabled = true;
                trackBarLight3.Enabled = true;
            }
            else
            {
                ckbLight3.Checked = false;
                txtLight3.Enabled = false;
                trackBarLight3.Enabled = false;
            }

            int light4 = Convert.ToInt32(str[3].Substring(3));
            txtLight4.Text = light4.ToString();
            trackBarLight4.Value = light4;
            if (light4 > 0)
            {
                ckbLight4.Checked = true;
                txtLight4.Enabled = true;
                trackBarLight4.Enabled = true;
            }
            else
            {
                ckbLight4.Checked = false;
                txtLight4.Enabled = false;
                trackBarLight4.Enabled = false;
            }

            int light5 = Convert.ToInt32(str[4].Substring(3));
            txtLight5.Text = light5.ToString();
            trackBarLight5.Value = light5;
            if (light5 > 0)
            {
                ckbLight5.Checked = true;
                txtLight5.Enabled = true;
                trackBarLight5.Enabled = true;
            }
            else
            {
                ckbLight5.Checked = false;
                txtLight5.Enabled = false;
                trackBarLight5.Enabled = false;
            }

            int light6 = Convert.ToInt32(str[5].Substring(3));
            txtLight6.Text = light6.ToString();
            trackBarLight6.Value = light6;
            if (light6 > 0)
            {
                ckbLight6.Checked = true;
                txtLight6.Enabled = true;
                trackBarLight6.Enabled = true;
            }
            else
            {
                ckbLight6.Checked = false;
                txtLight6.Enabled = false;
                trackBarLight6.Enabled = false;
            }

            int light7 = Convert.ToInt32(str[6].Substring(3));
            txtLight7.Text = light7.ToString();
            trackBarLight7.Value = light7;
            if (light7 > 0)
            {
                ckbLight7.Checked = true;
                txtLight7.Enabled = true;
                trackBarLight7.Enabled = true;
            }
            else
            {
                ckbLight7.Checked = false;
                txtLight7.Enabled = false;
                trackBarLight7.Enabled = false;
            }
        }

        /// <summary>
        /// AB面可调整光源，内外圈暂时不需要调整
        /// </summary>
        /// <param name="index"></param>
        public void SetControlEnable(string WorkStation)
        {
            if (WorkStation == "端面A" || WorkStation == "端面B")
            {
                txtLight1.Enabled = true;
                txtLight2.Enabled = true;
                txtLight3.Enabled = true;
                txtLight4.Enabled = true;
                txtLight5.Enabled = true;
                txtLight6.Enabled = true;
                trackBarLight1.Enabled = true;
                trackBarLight2.Enabled = true;
                trackBarLight3.Enabled = true;
                trackBarLight4.Enabled = true;
                trackBarLight5.Enabled = true;
                trackBarLight6.Enabled = true;
                ckbLight1.Enabled = true;
                ckbLight2.Enabled = true;
                ckbLight3.Enabled = true;
                ckbLight4.Enabled = true;
                ckbLight5.Enabled = true;
                ckbLight6.Enabled = true;
            }
            else
            {
                txtLight1.Enabled = false;
                txtLight2.Enabled = false;
                txtLight3.Enabled = false;
                txtLight4.Enabled = false;
                txtLight5.Enabled = false;
                txtLight6.Enabled = false;
                trackBarLight1.Enabled = false;
                trackBarLight2.Enabled = false;
                trackBarLight3.Enabled = false;
                trackBarLight4.Enabled = false;
                trackBarLight5.Enabled = false;
                trackBarLight6.Enabled = false;
                ckbLight1.Enabled = false;
                ckbLight2.Enabled = false;
                ckbLight3.Enabled = false;
                ckbLight4.Enabled = false;
                ckbLight5.Enabled = false;
                ckbLight6.Enabled = false;
            }
        }

        #endregion 工位和图片位置选项卡

        #region 开始抓拍

        private void btnTakePicture_Click(object sender, EventArgs e)
        {
            if (startSign)
            {
                startSign = false;
                btnTakePicture.Text = "开始抓拍";
                groupBox2.Enabled = true;
            }
            else
            {
                startSign = true;
                btnTakePicture.Text = "停止抓拍";
                groupBox2.Enabled = false;
            }

            if (indexWorkStation == "端面A")
            {
            }
            else if (indexWorkStation == "外圆A")
            {
                GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.WaiYuanA_TXG, startSign);
                GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.WaiYuanA_TZG, startSign);
            }
            else if (indexWorkStation == "倒角A")
            {
                GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DaoJiaoA_TXG, startSign);
            }
            else if (indexWorkStation == "内孔")
            {
                GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiKong_BG, startSign);
            }
            else if (indexWorkStation == "端面B")
            {
            }
            else if (indexWorkStation == "外圆B")
            {
                GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.WaiYuanB_TXG, startSign);
            }
            else if (indexWorkStation == "倒角B")
            {
                GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.DaoJiaoB_TXG, startSign);
            }
        }

        private void ShowPicture(Bitmap bitmap, int index)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    picBox.Image = bitmap;
                }));
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 拍照线程
        /// </summary>
        private void takeImageThread()
        {
            string ErrorMsg = "";
            Bitmap bitmap = new Bitmap(1600, 1600);
            int index = 0;

            while (true)
            {
                //IAreaScanCameraHelper[] cameraHelper = { GlobalVariable.DuanMianACameraHelper,
                //                                     GlobalVariable.WaiYuanACameraHelper,
                //                                     GlobalVariable.DaoJiaoACameraHelper,
                //                                     GlobalVariable.NeiKongCameraHelper,
                //                                     GlobalVariable.DuanMianBCameraHelper,
                //                                     GlobalVariable.WaiYuanBCameraHelper,
                //                                     GlobalVariable.DaoJiaoBCameraHelper};
                //CameraPhotoSetting[] photoSetting = { GlobalVariable.configSetting.duanMianACameraPhotoSetting,
                //                                  GlobalVariable.configSetting.waiYuanACameraPhotoSetting,
                //                                  GlobalVariable.configSetting.daoJiaoACameraPhotoSetting,
                //                                  GlobalVariable.configSetting.neiKongCameraPhotoSetting,
                //                                  GlobalVariable.configSetting.duanMianBCameraPhotoSetting,
                //                                  GlobalVariable.configSetting.waiYuanBCameraPhotoSetting,
                //                                  GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting};

                Thread.Sleep(10);
                if (!startSign)
                {
                    continue;
                }
                    
                #region
                //if (index == 0)
                //{
                //    LogHelper.AddCommLog(GlobalVariable.configSetting.duanMianACameraPhotoSetting.lightsCmd[m_PictureIndex]);
                //    GlobalVariable.lightHelper[0].OperateLight(GlobalVariable.configSetting.duanMianACameraPhotoSetting.lightsCmd[m_PictureIndex]);
                //    GlobalVariable.DuanMianACameraHelper.SetCameraExposureTime(GlobalVariable.configSetting.duanMianACameraPhotoSetting.exposure[m_PictureIndex], ref ErrorMsg);
                //    GlobalVariable.DuanMianACameraHelper.SetCameraGain(GlobalVariable.configSetting.duanMianACameraPhotoSetting.gain[m_PictureIndex], ref ErrorMsg);
                //}
                //else if (index == 1)
                //{
                //    GlobalVariable.WaiYuanACameraHelper.SetCameraExposureTime(GlobalVariable.configSetting.waiYuanACameraPhotoSetting.exposure[0], ref ErrorMsg);
                //    GlobalVariable.WaiYuanACameraHelper.SetCameraGain(GlobalVariable.configSetting.duanMianACameraPhotoSetting.gain[0], ref ErrorMsg);
                //}
                //else if (index == 2)
                //{
                //    GlobalVariable.DaoJiaoACameraHelper.SetCameraExposureTime(GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.exposure[0], ref ErrorMsg);
                //    GlobalVariable.DaoJiaoACameraHelper.SetCameraGain(GlobalVariable.configSetting.duanMianACameraPhotoSetting.gain[0], ref ErrorMsg);
                //}
                //else if (index == 3)
                //{
                //    GlobalVariable.NeiKongCameraHelper.SetCameraExposureTime(GlobalVariable.configSetting.neiKongCameraPhotoSetting.exposure[0], ref ErrorMsg);
                //    GlobalVariable.NeiKongCameraHelper.SetCameraGain(GlobalVariable.configSetting.duanMianACameraPhotoSetting.gain[0], ref ErrorMsg);
                //}
                //else if (index == 4)
                //{
                //    LogHelper.AddCommLog(GlobalVariable.configSetting.duanMianBCameraPhotoSetting.lightsCmd[m_PictureIndex]);
                //    GlobalVariable.lightHelper[1].OperateLight(GlobalVariable.configSetting.duanMianBCameraPhotoSetting.lightsCmd[m_PictureIndex]);
                //    GlobalVariable.DuanMianBCameraHelper.SetCameraExposureTime(GlobalVariable.configSetting.duanMianBCameraPhotoSetting.exposure[m_PictureIndex], ref ErrorMsg);
                //    GlobalVariable.DuanMianBCameraHelper.SetCameraGain(GlobalVariable.configSetting.duanMianACameraPhotoSetting.gain[m_PictureIndex], ref ErrorMsg);
                //}
                //else if (index == 5)
                //{
                //    GlobalVariable.WaiYuanBCameraHelper.SetCameraExposureTime(GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.exposure[0], ref ErrorMsg);
                //    GlobalVariable.WaiYuanBCameraHelper.SetCameraGain(GlobalVariable.configSetting.duanMianACameraPhotoSetting.gain[0], ref ErrorMsg);
                //}
                //else if (index == 6)
                //{
                //    GlobalVariable.DaoJiaoBCameraHelper.SetCameraExposureTime(GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.exposure[0], ref ErrorMsg);
                //    GlobalVariable.DaoJiaoBCameraHelper.SetCameraGain(GlobalVariable.configSetting.duanMianACameraPhotoSetting.gain[0], ref ErrorMsg);
                //}
                #endregion
                if (m_indexWorkStation == 0 ||m_indexWorkStation ==4 )
                {
                    //LogHelper.AddCommLog(photoSetting[m_indexWorkStation].lightsCmd[m_PictureIndex]);
                    int i = 0;
                    if(m_indexWorkStation ==0)
                    {
                        i = 0;
                    }
                    else
                    {
                        i = 1;
                    }
                    GlobalVariable.lightHelper[i].OperateLight(m_photoSetting[m_indexWorkStation].lightsCmd[m_PictureIndex]);
                    m_cameraHelper[m_indexWorkStation].SetCameraExposureTime(m_photoSetting[m_indexWorkStation].exposure[m_PictureIndex], ref ErrorMsg);
                    m_cameraHelper[m_indexWorkStation].SetCameraGain(m_photoSetting[m_indexWorkStation].gain[m_PictureIndex], ref ErrorMsg);
                }
                else
                {
                    m_cameraHelper[m_indexWorkStation].SetCameraExposureTime(m_photoSetting[m_indexWorkStation].exposure[0], ref ErrorMsg);
                    m_cameraHelper[m_indexWorkStation].SetCameraGain(m_photoSetting[m_indexWorkStation].gain[0], ref ErrorMsg);
                }
                if(m_indexWorkStation==1)
                {
                    m_cameraHelper[m_indexWorkStation].SetCameraROI(m_photoSetting[m_indexWorkStation].ROIWidthTrue
                        , m_photoSetting[m_indexWorkStation].ROIHeihgtTrue, m_photoSetting[m_indexWorkStation].OffsetX, m_photoSetting[m_indexWorkStation].OffsetY,false);
                }
                else
                {
                    m_cameraHelper[m_indexWorkStation].SetCameraROI(m_photoSetting[m_indexWorkStation].ROIWidthTrue
                        , m_photoSetting[m_indexWorkStation].ROIHeihgtTrue, m_photoSetting[m_indexWorkStation].OffsetX, m_photoSetting[m_indexWorkStation].OffsetY);
                }
                m_cameraHelper[m_indexWorkStation].Start();

                while (true)
                {
                    m_cameraHelper[m_indexWorkStation].TakeCameraImage(ref bitmap, ref ErrorMsg);
                    Thread.Sleep(3);
                    if (!startSign || selectChange)
                    {
                        selectChange = false;
                        m_cameraHelper[m_indexWorkStation].Stop();
                        break;
                    }

                    Bitmap bitmapCopy = new Bitmap(bitmap);
                    if (m_indexWorkStation == 1)
                        BitMapHelper.RotateBitmap(ref bitmapCopy);
                    GetScaleNumber(bitmapCopy.Width, bitmapCopy.Height);
                    ShowPicture(bitmapCopy, m_indexWorkStation);
                }
            }
        }

        //SA0000#SB0000#SC0000#SD0000#SE0000#SF0200#
        /// <summary>
        /// 获得光源的CMD指令
        /// </summary>
        /// <returns></returns>
        public string GetStringLightCMDAB()
        {
            string cmd = "";
            //背光
            if (ckbLight1.Checked)
            {
                cmd += "S01" + GetCorrectDataFromNumber(txtLight1.Text.Trim()) + "#";
            }
            else
            {
                cmd += "S01000#";
            }

            if (ckbLight2.Checked)
            {
                cmd += "S02" + GetCorrectDataFromNumber(txtLight2.Text.Trim()) + "#";
            }
            else
            {
                cmd += "S02000#";
            }

            if (ckbLight3.Checked)
            {
                cmd += "S10" + GetCorrectDataFromNumber(txtLight3.Text.Trim()) + "#";
            }
            else
            {
                cmd += "S10000#";
            }

            if (ckbLight4.Checked)
            {
                cmd += "S07" + GetCorrectDataFromNumber(txtLight4.Text.Trim()) + "#";
            }
            else
            {
                cmd += "S07000#";
            }

            if (ckbLight5.Checked)
            {
                cmd += "S08" + GetCorrectDataFromNumber(txtLight5.Text.Trim()) + "#";
            }
            else
            {
                cmd += "S08000#";
            }

            if (ckbLight6.Checked)
            {
                cmd += "S09" + GetCorrectDataFromNumber(txtLight6.Text.Trim()) + "#";
            }
            else
            {
                cmd += "S09000#";
            }

            if (ckbLight7.Checked)
            {
                cmd += "S03" + GetCorrectDataFromNumber(txtLight7.Text.Trim()) + "#";
            }
            else
            {
                cmd += "S03000#";
            }
            return cmd;
        }

        public string GetCorrectDataFromNumber(string text)
        {
            int d = Convert.ToInt32(text);
            if (d < 10)
            {
                return "00" + text;
            }
            else if (d >= 10 && d < 100)
            {
                return "0" + text;
            }
            else
            {
                return text;
            }
        }

        private double ScaleNumberX = 0.0;
        private double ScaleNumberY = 0.0;

        public void GetScaleNumber(int x, int y)
        {
            ScaleNumberY = y * 1.0 / picBox.Height;
            ScaleNumberX = x * 1.0 / picBox.Width;
        }

        #endregion 开始抓拍

        #region 2个保存按钮事件

        private void btnSaveROI_Click(object sender, EventArgs e)
        {
            bool bRtn = false;
            int pictureIndex = cmbPictureCheck.SelectedIndex;

            try
            {
                Convert.ToDouble(txtWidthMax.Text);
                Convert.ToDouble(txtWidthOffset.Text);
                Convert.ToDouble(txtWidthTrue.Text);
                Convert.ToDouble(txtHeightMax.Text);
                Convert.ToDouble(txtHeightOffset.Text);
                Convert.ToDouble(txtHeightTrue.Text);
                if (indexWorkStation == "外圆A")
                    Convert.ToDouble(txtXSSpeed.Text);
            }
            catch
            {
                GlobalMethod.ShowMessage("您所输入的数字有误，请重新输入");
                return;
            }
            if(indexWorkStation == "外圆A") 
            {
                m_photoSettingTmp[m_indexWorkStation].xsSpeed = (short)Convert.ToDouble(txtXSSpeed.Text);
                m_photoSetting[m_indexWorkStation].xsSpeed = (short)Convert.ToDouble(txtXSSpeed.Text);
            }
            m_photoSetting[m_indexWorkStation].ROIWidthMax = (short)Convert.ToDouble(txtWidthMax.Text);
            m_photoSetting[m_indexWorkStation].OffsetX = (short)Convert.ToDouble(txtWidthOffset.Text);
            m_photoSetting[m_indexWorkStation].ROIWidthTrue = (short)Convert.ToDouble(txtWidthTrue.Text);
            m_photoSetting[m_indexWorkStation].ROIHeightMax = (short)Convert.ToDouble(txtHeightMax.Text);
            m_photoSetting[m_indexWorkStation].OffsetY = (short)Convert.ToDouble(txtHeightOffset.Text);
            m_photoSetting[m_indexWorkStation].ROIHeihgtTrue = (short)Convert.ToDouble(txtHeightTrue.Text);

            m_photoSettingTmp[m_indexWorkStation].ROIWidthMax = (short)Convert.ToDouble(txtWidthMax.Text);
            m_photoSettingTmp[m_indexWorkStation].OffsetX = (short)Convert.ToDouble(txtWidthOffset.Text);
            m_photoSettingTmp[m_indexWorkStation].ROIWidthTrue = (short)Convert.ToDouble(txtWidthTrue.Text);
            m_photoSettingTmp[m_indexWorkStation].ROIHeightMax = (short)Convert.ToDouble(txtHeightMax.Text);
            m_photoSettingTmp[m_indexWorkStation].OffsetY = (short)Convert.ToDouble(txtHeightOffset.Text);
            m_photoSettingTmp[m_indexWorkStation].ROIHeihgtTrue = (short)Convert.ToDouble(txtHeightTrue.Text);
            if (m_indexWorkStation == 1)
            {
                bRtn = GlobalVariable.WaiYuanACameraHelper.SetXSSpeed(GlobalVariable.configSetting.waiYuanACameraPhotoSetting.xsSpeed);
                if (!bRtn)
                {
                    GlobalMethod.ShowMessage("设置外圆A相机线扫速度设置错误");
                    return;
                }
                bRtn = m_cameraHelper[m_indexWorkStation].SetCameraROI(
                    m_photoSetting[m_indexWorkStation].ROIWidthTrue,
                    m_photoSetting[m_indexWorkStation].ROIHeihgtTrue,
                    m_photoSetting[m_indexWorkStation].OffsetX,
                    m_photoSetting[m_indexWorkStation].OffsetY,false);
            }
            else
            {
                bRtn = m_cameraHelper[m_indexWorkStation].SetCameraROI(
                    m_photoSetting[m_indexWorkStation].ROIWidthTrue,
                    m_photoSetting[m_indexWorkStation].ROIHeihgtTrue,
                    m_photoSetting[m_indexWorkStation].OffsetX,
                    m_photoSetting[m_indexWorkStation].OffsetY);
            }

            if (!bRtn)
            {
                GlobalMethod.ShowMessage("设置相机ROI错误");
                return;
            }

            #region
            /*
            if (indexWorkStation == "端面A")
            {
                GlobalVariable.configSetting.duanMianACameraPhotoSetting.ROIWidthMax = (short)Convert.ToDouble(txtWidthMax.Text);
                GlobalVariable.configSetting.duanMianACameraPhotoSetting.OffsetX = (short)Convert.ToDouble(txtWidthOffset.Text);
                GlobalVariable.configSetting.duanMianACameraPhotoSetting.ROIWidthTrue = (short)Convert.ToDouble(txtWidthTrue.Text);
                GlobalVariable.configSetting.duanMianACameraPhotoSetting.ROIHeightMax = (short)Convert.ToDouble(txtHeightMax.Text);
                GlobalVariable.configSetting.duanMianACameraPhotoSetting.OffsetY = (short)Convert.ToDouble(txtHeightOffset.Text);
                GlobalVariable.configSetting.duanMianACameraPhotoSetting.ROIHeihgtTrue = (short)Convert.ToDouble(txtHeightTrue.Text);
                bRtn = GlobalVariable.DuanMianACameraHelper.SetCameraROI(
                    GlobalVariable.configSetting.duanMianACameraPhotoSetting.ROIWidthTrue,
                    GlobalVariable.configSetting.duanMianACameraPhotoSetting.ROIHeihgtTrue,
                    GlobalVariable.configSetting.duanMianACameraPhotoSetting.OffsetX,
                    GlobalVariable.configSetting.duanMianACameraPhotoSetting.OffsetY);
                if (!bRtn)
                {
                    GlobalMethod.ShowMessage("设置B面相机ROI错误，可能是您所设置的偏移值过大");
                    return;
                }
            }
            else if (indexWorkStation == "外圆A")
            {
                GlobalVariable.configSetting.waiYuanACameraPhotoSetting.ROIWidthMax = (short)Convert.ToDouble(txtWidthMax.Text);
                GlobalVariable.configSetting.waiYuanACameraPhotoSetting.OffsetX = (short)Convert.ToDouble(txtWidthOffset.Text);
                GlobalVariable.configSetting.waiYuanACameraPhotoSetting.ROIWidthTrue = (short)Convert.ToDouble(txtWidthTrue.Text);
                GlobalVariable.configSetting.waiYuanACameraPhotoSetting.ROIHeightMax = (short)Convert.ToDouble(txtHeightMax.Text);
                GlobalVariable.configSetting.waiYuanACameraPhotoSetting.OffsetY = (short)Convert.ToDouble(txtHeightOffset.Text);
                GlobalVariable.configSetting.waiYuanACameraPhotoSetting.ROIHeihgtTrue = (short)Convert.ToDouble(txtHeightTrue.Text);
                GlobalVariable.configSetting.waiYuanACameraPhotoSetting.xsSpeed = (short)Convert.ToDouble(txtXSSpeed.Text);
                bRtn = GlobalVariable.WaiYuanACameraHelper.SetCameraROI(
                    GlobalVariable.configSetting.waiYuanACameraPhotoSetting.ROIWidthTrue,
                    GlobalVariable.configSetting.waiYuanACameraPhotoSetting.ROIHeihgtTrue,
                    GlobalVariable.configSetting.waiYuanACameraPhotoSetting.OffsetX,
                    GlobalVariable.configSetting.waiYuanACameraPhotoSetting.OffsetY, false);
                if (!bRtn)
                {
                    GlobalMethod.ShowMessage("设置外圆A相机ROI错误，可能是您所设置的偏移值过大");
                    return;
                }
                bRtn = GlobalVariable.WaiYuanACameraHelper.SetXSSpeed(GlobalVariable.configSetting.waiYuanACameraPhotoSetting.xsSpeed);
                if (!bRtn)
                {
                    GlobalMethod.ShowMessage("设置外圆A相机线扫速度设置错误");
                    return;
                }
            }
            else if (indexWorkStation == "倒角A")
            {
                GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.ROIWidthMax = (short)Convert.ToDouble(txtWidthMax.Text);
                GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.OffsetX = (short)Convert.ToDouble(txtWidthOffset.Text);
                GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.ROIWidthTrue = (short)Convert.ToDouble(txtWidthTrue.Text);
                GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.ROIHeightMax = (short)Convert.ToDouble(txtHeightMax.Text);
                GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.OffsetY = (short)Convert.ToDouble(txtHeightOffset.Text);
                GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.ROIHeihgtTrue = (short)Convert.ToDouble(txtHeightTrue.Text);
                bRtn = GlobalVariable.DaoJiaoACameraHelper.SetCameraROI(
                    GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.ROIWidthTrue,
                    GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.ROIHeihgtTrue,
                    GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.OffsetX,
                    GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.OffsetY);
                if (!bRtn)
                {
                    GlobalMethod.ShowMessage("设置倒角A相机ROI错误，可能是您所设置的偏移值过大");
                    return;
                }
            }
            else if (indexWorkStation == "内孔")
            {
                GlobalVariable.configSetting.neiKongCameraPhotoSetting.ROIWidthMax = (short)Convert.ToDouble(txtWidthMax.Text);
                GlobalVariable.configSetting.neiKongCameraPhotoSetting.OffsetX = (short)Convert.ToDouble(txtWidthOffset.Text);
                GlobalVariable.configSetting.neiKongCameraPhotoSetting.ROIWidthTrue = (short)Convert.ToDouble(txtWidthTrue.Text);
                GlobalVariable.configSetting.neiKongCameraPhotoSetting.ROIHeightMax = (short)Convert.ToDouble(txtHeightMax.Text);
                GlobalVariable.configSetting.neiKongCameraPhotoSetting.OffsetY = (short)Convert.ToDouble(txtHeightOffset.Text);
                GlobalVariable.configSetting.neiKongCameraPhotoSetting.ROIHeihgtTrue = (short)Convert.ToDouble(txtHeightTrue.Text);
                bRtn = GlobalVariable.NeiKongCameraHelper.SetCameraROI(
                    GlobalVariable.configSetting.neiKongCameraPhotoSetting.ROIWidthTrue,
                    GlobalVariable.configSetting.neiKongCameraPhotoSetting.ROIHeihgtTrue,
                    GlobalVariable.configSetting.neiKongCameraPhotoSetting.OffsetX,
                    GlobalVariable.configSetting.neiKongCameraPhotoSetting.OffsetY);
                if (!bRtn)
                {
                    GlobalMethod.ShowMessage("设置内孔相机ROI错误，可能是您所设置的偏移值过大");
                    return;
                }
            }
            else if (indexWorkStation == "端面B")
            {
                GlobalVariable.configSetting.duanMianBCameraPhotoSetting.ROIWidthMax = (short)Convert.ToDouble(txtWidthMax.Text);
                GlobalVariable.configSetting.duanMianBCameraPhotoSetting.OffsetX = (short)Convert.ToDouble(txtWidthOffset.Text);
                GlobalVariable.configSetting.duanMianBCameraPhotoSetting.ROIWidthTrue = (short)Convert.ToDouble(txtWidthTrue.Text);
                GlobalVariable.configSetting.duanMianBCameraPhotoSetting.ROIHeightMax = (short)Convert.ToDouble(txtHeightMax.Text);
                GlobalVariable.configSetting.duanMianBCameraPhotoSetting.OffsetY = (short)Convert.ToDouble(txtHeightOffset.Text);
                GlobalVariable.configSetting.duanMianBCameraPhotoSetting.ROIHeihgtTrue = (short)Convert.ToDouble(txtHeightTrue.Text);
                bRtn = GlobalVariable.DuanMianBCameraHelper.SetCameraROI(
                    GlobalVariable.configSetting.duanMianBCameraPhotoSetting.ROIWidthTrue,
                    GlobalVariable.configSetting.duanMianBCameraPhotoSetting.ROIHeihgtTrue,
                    GlobalVariable.configSetting.duanMianBCameraPhotoSetting.OffsetX,
                    GlobalVariable.configSetting.duanMianBCameraPhotoSetting.OffsetY);
                if (!bRtn)
                {
                    GlobalMethod.ShowMessage("设置端面B相机ROI错误，可能是您所设置的偏移值过大");
                    return;
                }
            }
            else if (indexWorkStation == "外圆B")
            {
                GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.ROIWidthMax = (short)Convert.ToDouble(txtWidthMax.Text);
                GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.OffsetX = (short)Convert.ToDouble(txtWidthOffset.Text);
                GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.ROIWidthTrue = (short)Convert.ToDouble(txtWidthTrue.Text);
                GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.ROIHeightMax = (short)Convert.ToDouble(txtHeightMax.Text);
                GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.OffsetY = (short)Convert.ToDouble(txtHeightOffset.Text);
                GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.ROIHeihgtTrue = (short)Convert.ToDouble(txtHeightTrue.Text);
                bRtn = GlobalVariable.WaiYuanBCameraHelper.SetCameraROI(
                    GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.ROIWidthTrue,
                    GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.ROIHeihgtTrue,
                    GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.OffsetX,
                    GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.OffsetY);
                if (!bRtn)
                {
                    GlobalMethod.ShowMessage("设置外圆B相机ROI错误，可能是您所设置的偏移值过大");
                    return;
                }
            }
            else if (indexWorkStation == "倒角B")
            {
                GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.ROIWidthMax = (short)Convert.ToDouble(txtWidthMax.Text);
                GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.OffsetX = (short)Convert.ToDouble(txtWidthOffset.Text);
                GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.ROIWidthTrue = (short)Convert.ToDouble(txtWidthTrue.Text);
                GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.ROIHeightMax = (short)Convert.ToDouble(txtHeightMax.Text);
                GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.OffsetY = (short)Convert.ToDouble(txtHeightOffset.Text);
                GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.ROIHeihgtTrue = (short)Convert.ToDouble(txtHeightTrue.Text);
                bRtn = GlobalVariable.DaoJiaoBCameraHelper.SetCameraROI(
                    GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.ROIWidthTrue,
                    GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.ROIHeihgtTrue,
                    GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.OffsetX,
                    GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.OffsetY);
                if (!bRtn)
                {
                    GlobalMethod.ShowMessage("设置倒角B相机ROI错误，可能是您所设置的偏移值过大");
                    return;
                }
            }
            */
            #endregion
            string ErrorMsg = "";
            refreshConfigSetting();

            string mainPath = FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json";
            string backupPath = FilePath.ParamSettingPath + "ParamBackup\\Param.Json";

            // 1. 写入主配置文件
            bool mRtn = JsonHelper.WriteJsonFile(GlobalVariable.configSetting, mainPath, ref ErrorMsg);

            if (mRtn)
            {
                // 2. 写入备份配置文件（同样使用 WriteJsonFile）
                bool backupRtn = JsonHelper.WriteJsonFile(GlobalVariable.TmpConfigSetting, backupPath, ref ErrorMsg);

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

        private void updateParam()
        {
            selectChange = true;
            if(m_indexWorkStation == 0|| m_indexWorkStation == 4)
            {
                m_photoSetting[m_indexWorkStation].lightsCmd[m_PictureIndex] = GetStringLightCMDAB();
            }
            m_photoSetting[m_indexWorkStation].exposure[m_PictureIndex] = Convert.ToDouble(nud_ExposureTime.Value);
            #region
            /*
            if (stationIndex == 0)
            {
                GlobalVariable.configSetting.duanMianACameraPhotoSetting.exposure[pictureIndex] = Convert.ToDouble(nud_ExposureTime.Value);
                GlobalVariable.configSetting.duanMianACameraPhotoSetting.lightsCmd[pictureIndex] = GetStringLightCMDAB();
            }
            else if (stationIndex == 1)
            {
                GlobalVariable.configSetting.waiYuanACameraPhotoSetting.exposure[0] = Convert.ToDouble(nud_ExposureTime.Value);
            }
            else if (stationIndex == 2)
            {
                GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.exposure[0] = Convert.ToDouble(nud_ExposureTime.Value);
            }
            else if (stationIndex == 3)
            {
                GlobalVariable.configSetting.neiKongCameraPhotoSetting.exposure[0] = Convert.ToDouble(nud_ExposureTime.Value);
            }
            else if (stationIndex == 4)
            {
                GlobalVariable.configSetting.duanMianBCameraPhotoSetting.exposure[pictureIndex] = Convert.ToDouble(nud_ExposureTime.Value);
                GlobalVariable.configSetting.duanMianBCameraPhotoSetting.lightsCmd[pictureIndex] = GetStringLightCMDAB();
            }
            else if (stationIndex == 5)
            {
                GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.exposure[0] = Convert.ToDouble(nud_ExposureTime.Value);
            }
            else if (stationIndex == 6)
            {
                GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.exposure[0] = Convert.ToDouble(nud_ExposureTime.Value);
            }
            */
            #endregion
        }

        private void btnSaveLight_Click(object sender, EventArgs e)
        {
            string ErrorMsg = "";
            bool bRtn = false;
            int pictureIndex = cmbPictureCheck.SelectedIndex;
            m_photoSetting[m_indexWorkStation].exposure[pictureIndex] = Convert.ToDouble(nud_ExposureTime.Value);
            //m_photoSetting[m_indexWorkStation].gain[pictureIndex] = Convert.ToDouble(nud_Gain.Value);
            if(m_indexWorkStation == 0||m_indexWorkStation == 4)
            {
                m_photoSetting[m_indexWorkStation].lightsCmd[m_PictureIndex] = GetStringLightCMDAB();
            }
            #region
            /*
            if (indexWorkStation == "端面A")
            {
                GlobalVariable.configSetting.duanMianACameraPhotoSetting.exposure[pictureIndex] = Convert.ToDouble(nud_ExposureTime.Value);
                GlobalVariable.configSetting.duanMianACameraPhotoSetting.lightsCmd[pictureIndex] = GetStringLightCMDAB();
                GlobalVariable.configSetting.duanMianACameraPhotoSetting.gain[pictureIndex] = Convert.ToDouble(nud_Gain.Value);
            }
            else if (indexWorkStation == "外圆A")
            {
                GlobalVariable.configSetting.waiYuanACameraPhotoSetting.exposure[pictureIndex] = Convert.ToDouble(nud_ExposureTime.Value);
                GlobalVariable.configSetting.waiYuanACameraPhotoSetting.gain[pictureIndex] = Convert.ToDouble(nud_Gain.Value);
            }
            else if (indexWorkStation == "倒角A")
            {
                GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.exposure[pictureIndex] = Convert.ToDouble(nud_ExposureTime.Value);
                GlobalVariable.configSetting.daoJiaoACameraPhotoSetting.gain[pictureIndex] = Convert.ToDouble(nud_Gain.Value);
            }
            else if (indexWorkStation == "内孔")
            {
                GlobalVariable.configSetting.neiKongCameraPhotoSetting.exposure[pictureIndex] = Convert.ToDouble(nud_ExposureTime.Value);
                GlobalVariable.configSetting.neiKongCameraPhotoSetting.gain[pictureIndex] = Convert.ToDouble(nud_Gain.Value);
            }
            else if (indexWorkStation == "端面B")
            {
                GlobalVariable.configSetting.duanMianBCameraPhotoSetting.exposure[pictureIndex] = Convert.ToDouble(nud_ExposureTime.Value);
                GlobalVariable.configSetting.duanMianBCameraPhotoSetting.lightsCmd[pictureIndex] = GetStringLightCMDAB();
                GlobalVariable.configSetting.duanMianBCameraPhotoSetting.gain[pictureIndex] = Convert.ToDouble(nud_Gain.Value);
            }
            else if (indexWorkStation == "外圆B")
            {
                GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.exposure[pictureIndex] = Convert.ToDouble(nud_ExposureTime.Value);
                GlobalVariable.configSetting.waiYuanBCameraPhotoSetting.gain[pictureIndex] = Convert.ToDouble(nud_Gain.Value);
            }
            else if (indexWorkStation == "倒角B")
            {
                GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.exposure[pictureIndex] = Convert.ToDouble(nud_ExposureTime.Value);
                GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting.gain[pictureIndex] = Convert.ToDouble(nud_Gain.Value);
            }
            */
            #endregion
            refreshConfigSetting();

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

        #endregion 2个保存按钮事件

        #region TXT ROI实际宽高的事件

        private void txtWidthTrue_TextChanged(object sender, EventArgs e)
        {
            double d = 0;
            try
            {
                d = Convert.ToDouble(txtWidthTrue.Text);
                txtWidthTrue.BackColor = SystemColors.Window;
            }
            catch
            {
                d = 0;
                txtWidthTrue.Text = d.ToString();
                txtWidthTrue.BackColor = Color.Red;
                return;
            }
            //if (d > Convert.ToDouble(txtWidthMax.Text))
            //{
            //    txtWidthTrue.Text = txtWidthMax.Text;
            //}
            //else
            {
                //if (d % 16 != 0)
                //{
                //    d = d - d % 16;
                //}
                txtWidthTrue.Text = d.ToString();
                txtWidthTrue.BackColor = SystemColors.Window;
            }
        }

        private void txtHeightTrue_TextChanged(object sender, EventArgs e)
        {
            double d = 0;
            try
            {
                d = Convert.ToDouble(txtHeightTrue.Text);
            }
            catch
            {
                d = 0;
                txtHeightTrue.BackColor = Color.Red;
                txtHeightTrue.Text = d.ToString();
                return;
            }
            //if (d > Convert.ToDouble(txtHeightMax.Text))
            //{
            //    d = 0;
            //    txtHeightTrue.Text = d.ToString();
            //    txtHeightTrue.BackColor = Color.Red;
            //}
            //else
            {
                txtHeightTrue.BackColor = SystemColors.Window;
            }
        }

        #endregion TXT ROI实际宽高的事件

        #region 滑块的事件

        private void trackBarExposure_Scroll(object sender, EventArgs e)
        {
            //if (index == 3)
            //{
            //    if (trackBarExposure.Value < 10)
            //    {
            //        trackBarExposure.Value = 10;
            //    }
            //}
            //else
            //{
            //    if (trackBarExposure.Value < 50)
            //    {
            //        trackBarExposure.Value = 50;
            //    }
            //}

            //txtExposureTime.Text = trackBarExposure.Value.ToString();
            //updateParam();
        }

        private void trackBarLight1_Scroll(object sender, EventArgs e)
        {
            txtLight1.Text = trackBarLight1.Value.ToString();
            updateParam();
        }

        private void trackBarLight2_Scroll(object sender, EventArgs e)
        {
            txtLight2.Text = trackBarLight2.Value.ToString();
            updateParam();
        }

        private void trackBarLight3_Scroll(object sender, EventArgs e)
        {
            txtLight3.Text = trackBarLight3.Value.ToString();
            updateParam();
        }

        private void trackBarLight4_Scroll(object sender, EventArgs e)
        {
            txtLight4.Text = trackBarLight4.Value.ToString();
            updateParam();
        }

        private void trackBarLight5_Scroll(object sender, EventArgs e)
        {
            txtLight5.Text = trackBarLight5.Value.ToString();
            updateParam();
        }

        private void trackBarLight6_Scroll(object sender, EventArgs e)
        {
            txtLight6.Text = trackBarLight6.Value.ToString();
            updateParam();
        }

        private void trackBarLight7_Scroll(object sender, EventArgs e)
        {
            txtLight7.Text = trackBarLight7.Value.ToString();
            updateParam();
        }

        #endregion 滑块的事件

        #region checkbox的事件

        private void ckbLight1_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbLight1.Checked)
            {
                trackBarLight1.Enabled = true;
                txtLight1.Enabled = true;
            }
            else
            {
                trackBarLight1.Enabled = false;
                txtLight1.Enabled = false;
            }
            updateParam();
        }

        private void ckbLight2_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbLight2.Checked)
            {
                trackBarLight2.Enabled = true;
                txtLight2.Enabled = true;
            }
            else
            {
                trackBarLight2.Enabled = false;
                txtLight2.Enabled = false;
            }
            updateParam();
        }

        private void ckbLight3_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbLight3.Checked)
            {
                trackBarLight3.Enabled = true;
                txtLight3.Enabled = true;
            }
            else
            {
                trackBarLight3.Enabled = false;
                txtLight3.Enabled = false;
            }
            updateParam();
        }

        private void ckbLight4_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbLight4.Checked)
            {
                trackBarLight4.Enabled = true;
                txtLight4.Enabled = true;
            }
            else
            {
                trackBarLight4.Enabled = false;
                txtLight4.Enabled = false;
            }
            updateParam();
        }

        private void ckbLight5_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbLight5.Checked)
            {
                trackBarLight5.Enabled = true;
                txtLight5.Enabled = true;
            }
            else
            {
                trackBarLight5.Enabled = false;
                txtLight5.Enabled = false;
            }
            updateParam();
        }

        private void ckbLight6_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbLight6.Checked)
            {
                trackBarLight6.Enabled = true;
                txtLight6.Enabled = true;
            }
            else
            {
                trackBarLight6.Enabled = false;
                txtLight6.Enabled = false;
            }
            updateParam();
        }

        private void ckbLight7_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbLight7.Checked)
            {
                trackBarLight7.Enabled = true;
                txtLight7.Enabled = true;
            }
            else
            {
                trackBarLight7.Enabled = false;
                txtLight7.Enabled = false;
            }
            updateParam();
        }

        #endregion checkbox的事件

        #region textbox的事件

        private void txtLight1_TextChanged(object sender, EventArgs e)
        {
            double d = 0.0;
            if (double.TryParse(txtLight1.Text, out d))
            {
                if (d < 0 || d > 255)
                {
                    txtLight1.Text = "100";
                }
            }
            else
            {
                txtLight1.Text = "100";
            }
            updateParam();
        }

        private void txtLight2_TextChanged(object sender, EventArgs e)
        {
            double d = 0.0;
            if (double.TryParse(txtLight2.Text, out d))
            {
                if (d < 0 || d > 255)
                {
                    txtLight2.Text = "100";
                }
            }
            else
            {
                txtLight2.Text = "100";
            }
            updateParam();
        }

        private void txtLight3_TextChanged(object sender, EventArgs e)
        {
            double d = 0.0;
            if (double.TryParse(txtLight3.Text, out d))
            {
                if (d < 0 || d > 255)
                {
                    txtLight3.Text = "100";
                }
            }
            else
            {
                txtLight3.Text = "100";
            }
            updateParam();
        }

        private void txtLight4_TextChanged(object sender, EventArgs e)
        {
            double d = 0.0;
            if (double.TryParse(txtLight4.Text, out d))
            {
                if (d < 0 || d > 255)
                {
                    txtLight4.Text = "100";
                }
            }
            else
            {
                txtLight4.Text = "100";
            }
            updateParam();
        }

        private void txtLight5_TextChanged(object sender, EventArgs e)
        {
            double d = 0.0;
            if (double.TryParse(txtLight5.Text, out d))
            {
                if (d < 0 || d > 255)
                {
                    txtLight5.Text = "100";
                }
            }
            else
            {
                txtLight5.Text = "100";
            }
            updateParam();
        }

        private void txtLight6_TextChanged(object sender, EventArgs e)
        {
            double d = 0.0;
            if (double.TryParse(txtLight6.Text, out d))
            {
                if (d < 0 || d > 255)
                {
                    txtLight6.Text = "100";
                }
            }
            else
            {
                txtLight6.Text = "100";
            }
            updateParam();
        }

        private void txtLight7_TextChanged(object sender, EventArgs e)
        {
            double d = 0.0;
            if (double.TryParse(txtLight7.Text, out d))
            {
                if (d < 0 || d > 255)
                {
                    txtLight7.Text = "100";
                }
            }
            else
            {
                txtLight7.Text = "100";
            }
            updateParam();
        }

        #endregion textbox的事件

        private void picBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (picBox.Image == null)
            {
                return;
            }
            Bitmap bitmap = (Bitmap)picBox.Image;

            int x = e.X;
            int y = e.Y;
            x = (int)(x * ScaleNumberX);
            y = (int)(y * ScaleNumberY);
            int ch = BitMapHelper.getPixelDot(x, y, bitmap);
            lblGray.Text = "灰度：" + ch.ToString() + ";X = " + x.ToString() + ";Y = " + y.ToString();
        }

        private void CameraDebugForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            startSign = false;
            string cmd = "SA0000#SB0000#SC0000#SD0000#SE0000#SF0000#";
            //GlobalVariable.lightHelper[0].OperateLight(cmd);
            //GlobalVariable.lightHelper[1].OperateLight(cmd);
        }

        private void nud_ExposureTime_ValueChanged(object sender, EventArgs e)
        {
            updateParam();
        }

        private void nud_Gain_ValueChanged(object sender, EventArgs e)
        {
            //selectChange = true;
            m_photoSetting[m_indexWorkStation].gain[m_PictureIndex] = Convert.ToDouble(nud_Gain.Value);
        }

        private void refreshConfigSetting()
        {
            GlobalVariable.configSetting.duanMianACameraPhotoSetting = m_photoSetting[0];
            GlobalVariable.configSetting.waiYuanACameraPhotoSetting = m_photoSetting[1];
            GlobalVariable.configSetting.daoJiaoACameraPhotoSetting = m_photoSetting[2];
            GlobalVariable.configSetting.neiKongCameraPhotoSetting = m_photoSetting[3];
            GlobalVariable.configSetting.duanMianBCameraPhotoSetting = m_photoSetting[4];
            GlobalVariable.configSetting.waiYuanBCameraPhotoSetting = m_photoSetting[5];
            GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting= m_photoSetting[6];
        }
    }
}