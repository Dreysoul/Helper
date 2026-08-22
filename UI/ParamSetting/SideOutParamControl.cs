using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class SideOutParamControl : UserControl
    {
        public SideOutParamControl()
        {
            InitializeComponent();
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

        public void LoadParam()
        {
            #region 1

            ckb1.Checked = GlobalVariable.configSetting.waiYuanAParam.bAutoSearchRegion;
            txt2.Text = GlobalVariable.configSetting.waiYuanAParam.dwSampleRegionMinMeanGray.ToString();
            txt3.Text = GlobalVariable.configSetting.waiYuanAParam.dwValidRegionStartPosOffset.ToString();
            txt4.Text = GlobalVariable.configSetting.waiYuanAParam.dwValidRegionGrayThresOffset.ToString();
            txt5.Text = GlobalVariable.configSetting.waiYuanAParam.dwValidRegionSmoothLen.ToString();
            txt6.Text = GlobalVariable.configSetting.waiYuanAParam.dwValidRegionUpOffset.ToString();
            txt7.Text = GlobalVariable.configSetting.waiYuanAParam.dwValidRegionDownOffset.ToString();
            txt8.Text = GlobalVariable.configSetting.waiYuanAParam.dwBlackValidRegionUpOffset.ToString();
            txt9.Text = GlobalVariable.configSetting.waiYuanAParam.dwBlackValidRegionDownOffset.ToString();
            txt17.Text = GlobalVariable.configSetting.waiYuanAParam.dwValidRegionWidthOffset.ToString();
            txt18.Text = GlobalVariable.configSetting.waiYuanAParam.dwValidRegionHeightOffset.ToString();
            txt21.Text = GlobalVariable.configSetting.waiYuanAParam.dwHighBlackBlobGrayOffset.ToString();
            txt22.Text = GlobalVariable.configSetting.waiYuanAParam.dwHighBlackBlobMinArea.ToString();
            txt23.Text = GlobalVariable.configSetting.waiYuanAParam.dwAllHighBlackBlockMinArea.ToString();
            txt26.Text = GlobalVariable.configSetting.waiYuanAParam.dwLowBlackBlobGrayOffset.ToString();
            txt27.Text = GlobalVariable.configSetting.waiYuanAParam.dwLowBlackBlobMinArea.ToString();
            txt28.Text = GlobalVariable.configSetting.waiYuanAParam.dwHighWhiteBlobGrayOffset.ToString();
            txt29.Text = GlobalVariable.configSetting.waiYuanAParam.dwHighWhiteBlobMinArea.ToString();
            txt30.Text = GlobalVariable.configSetting.waiYuanAParam.dwLowWhiteBlobGrayOffset.ToString();
            txt31.Text = GlobalVariable.configSetting.waiYuanAParam.dwLowWhiteBlobMinArea.ToString();

            txt32.Text = GlobalVariable.configSetting.waiYuanAParam.dwBlackBlockJindu.ToString();
            txt33.Text = GlobalVariable.configSetting.waiYuanAParam.dwBlackBlockMinArea.ToString();
            txt34.Text = GlobalVariable.configSetting.waiYuanAParam.dwLowBlackDynGrayOffset.ToString();
            txt35.Text = GlobalVariable.configSetting.waiYuanAParam.dwLowBlackMeanLen.ToString();
            txt36.Text = GlobalVariable.configSetting.waiYuanAParam.dwLowBlackContLength.ToString();
            txt37.Text = GlobalVariable.configSetting.waiYuanAParam.dwMiddleRegionBlackBlockOffset.ToString();
            txt38.Text = GlobalVariable.configSetting.waiYuanAParam.dwMiddleRegionBlackBlockMinArea.ToString();

            txt50.Text = GlobalVariable.configSetting.waiYuanAParam.dwMoHengValidRegionUpOffset.ToString();
            txt51.Text = GlobalVariable.configSetting.waiYuanAParam.dwMoHengValidRegionDownOffset.ToString();
            txt54.Text = GlobalVariable.configSetting.waiYuanAParam.fMohengGraySmoothLen.ToString();
            txt55.Text = GlobalVariable.configSetting.waiYuanAParam.fMohengGrayMinOffset.ToString();
            txt56.Text = GlobalVariable.configSetting.waiYuanAParam.fMohengGrayMinArea.ToString();
            txt57.Text = GlobalVariable.configSetting.waiYuanAParam.fMohengGrayMinDistance.ToString();
            txt58.Text = GlobalVariable.configSetting.waiYuanAParam.fMohengGraySingleMinLen.ToString();
            txt59.Text = GlobalVariable.configSetting.waiYuanAParam.fMohengGraySumMinLen.ToString();
            txt60.Text = GlobalVariable.configSetting.waiYuanAParam.dwMohengMeanGrayMinOffset.ToString();

            txt68.Text = GlobalVariable.configSetting.waiYuanAParam.dwLieHengJingdu.ToString("f4");
            txt69.Text = GlobalVariable.configSetting.waiYuanAParam.dwLieHengFactor.ToString("f4");
            txt70.Text = GlobalVariable.configSetting.waiYuanAParam.dwLieHengMinGrayOffset.ToString();
            txt71.Text = GlobalVariable.configSetting.waiYuanAParam.dwLieHengMaxGrayOffset.ToString();
            txt72.Text = GlobalVariable.configSetting.waiYuanAParam.dwLieHengMinDist.ToString();
            txt73.Text = GlobalVariable.configSetting.waiYuanAParam.dwLieHengHeight.ToString();
            txt84.Text = GlobalVariable.configSetting.waiYuanAParam.dwMeanGrayMaxOffset.ToString();
            txt85.Text = GlobalVariable.configSetting.waiYuanAParam.dwMeanGrayMinOffset.ToString();
            txt201.Text = GlobalVariable.configSetting.waiYuanAParam.dwRoundRegionBlackBlockJindu.ToString("f4");
            txt203.Text = GlobalVariable.configSetting.waiYuanAParam.dwUpRoundRegionUpOffset.ToString();
            txt204.Text = GlobalVariable.configSetting.waiYuanAParam.dwUpRoundRegionDownOffset.ToString();
            txt205.Text = GlobalVariable.configSetting.waiYuanAParam.dwUpRoundRegionBlackBlockGrayOffset.ToString();
            txt206.Text = GlobalVariable.configSetting.waiYuanAParam.dwUpRoundRegionBlackBlockMinArea.ToString();
            txt207.Text = GlobalVariable.configSetting.waiYuanAParam.dwUpRoundRegionMeanGrayMaxOffset.ToString();
            txt208.Text = GlobalVariable.configSetting.waiYuanAParam.dwUpRoundRegionMeanGrayMinOffset.ToString();
            txt209.Text = GlobalVariable.configSetting.waiYuanAParam.dwDownRoundRegionUpOffset.ToString();
            txt210.Text = GlobalVariable.configSetting.waiYuanAParam.dwDownRoundRegionDownOffset.ToString();

            txt211.Text = GlobalVariable.configSetting.waiYuanAParam.dwDownRoundRegionBlackBlockGrayOffset.ToString();
            txt212.Text = GlobalVariable.configSetting.waiYuanAParam.dwDownRoundRegionBlackBlockMinArea.ToString();
            txt213.Text = GlobalVariable.configSetting.waiYuanAParam.dwDownRoundRegionMeanGrayMaxOffset.ToString();
            txt214.Text = GlobalVariable.configSetting.waiYuanAParam.dwDownRoundRegionMeanGrayMinOffset.ToString();
            txt215.Text = GlobalVariable.configSetting.waiYuanAParam.dwUpRoundHeighMaxOffsetRatio.ToString("f4");
            txt216.Text = GlobalVariable.configSetting.waiYuanAParam.dwDownRoundHeighMaxOffsetRatio.ToString("f4");

            textBox1.Text = GlobalVariable.configSetting.waiYuanAParam.upRoundBlackRegionMaxGray.ToString();
            textBox2.Text = GlobalVariable.configSetting.waiYuanAParam.upRoundBlackRegionMinArea.ToString();
            textBox3.Text = GlobalVariable.configSetting.waiYuanAParam.downRoundBlackRegionMaxGray.ToString();
            textBox4.Text = GlobalVariable.configSetting.waiYuanAParam.downRoundBlackRegionMinArea.ToString();
            #endregion 1

            #region 2

            txt301.Text = GlobalVariable.configSetting.waiYuanAParam.dwBlackImageMohengUpDownOffset.ToString();
            txt302.Text = GlobalVariable.configSetting.waiYuanAParam.dwBlackImageMohengLeftRightOffset.ToString();
            txt303.Text = GlobalVariable.configSetting.waiYuanAParam.dwBlackImageMohengMeanLen.ToString();
            txt303_2.Text = GlobalVariable.configSetting.waiYuanAParam.dwBlackImageMohengGrayOffset.ToString();
            txt304.Text = GlobalVariable.configSetting.waiYuanAParam.dwBlackImageMohengFilterMinLen.ToString();
            txt305.Text = GlobalVariable.configSetting.waiYuanAParam.dwBlackImageMohengMinDistance.ToString();
            txt306.Text = GlobalVariable.configSetting.waiYuanAParam.dwBlackImageMohengMinLen.ToString();
            txt307.Text = GlobalVariable.configSetting.waiYuanAParam.fZhenWenJindu.ToString("f4");
            txt308.Text = GlobalVariable.configSetting.waiYuanAParam.dwZhenWenFilterMinArea.ToString();
            txt309.Text = GlobalVariable.configSetting.waiYuanAParam.dwZhenWenErrMinArea.ToString();
            txt310.Text = GlobalVariable.configSetting.waiYuanAParam.dwZhenWenErrCount.ToString();
            txt311.Text = GlobalVariable.configSetting.waiYuanAParam.fCaErrJindu.ToString("f4");
            txt312.Text = GlobalVariable.configSetting.waiYuanAParam.dwCaFilterMinArea.ToString();
            txt313.Text = GlobalVariable.configSetting.waiYuanAParam.dwCaErrMinArea.ToString();
            textBox5.Text = GlobalVariable.configSetting.waiYuanAParam.wideMoHenMinWidth.ToString();
            textBox6.Text = GlobalVariable.configSetting.waiYuanAParam.wideMoHenMinOffset.ToString();
            textBox7.Text = GlobalVariable.configSetting.waiYuanAParam.wideMoHenClosingRadius.ToString();

            textBox8.Text = GlobalVariable.configSetting.waiYuanAParam.intParams[0].ToString();
            textBox9.Text = GlobalVariable.configSetting.waiYuanAParam.intParams[4].ToString();
            textBox10.Text = GlobalVariable.configSetting.waiYuanAParam.intParams[3].ToString();
            textBox11.Text = GlobalVariable.configSetting.waiYuanAParam.intParams[2].ToString();
            textBox12.Text = GlobalVariable.configSetting.waiYuanAParam.intParams[1].ToString();
            textBox13.Text = GlobalVariable.configSetting.waiYuanAParam.intParams[7].ToString();
            #endregion 2

            #region flag

            ckbFlag1.Checked = GlobalVariable.configSetting.waiYuanAFlag.CalBlackBlockEnable;
            ckbFlag2.Checked = GlobalVariable.configSetting.waiYuanAFlag.FFTOuterSideImageEnable;
            ckbFlag3.Checked = GlobalVariable.configSetting.waiYuanAFlag.FindHighBlackBlockEnable;
            ckbFlag4.Checked = GlobalVariable.configSetting.waiYuanAFlag.FindLowBlackBlockEnable;
            ckbFlag5.Checked = GlobalVariable.configSetting.waiYuanAFlag.FindLowBlackBlockByMeanImageEnable;
            ckbFlag6.Checked = GlobalVariable.configSetting.waiYuanAFlag.FindHighWhiteBlockEnable;
            ckbFlag7.Checked = GlobalVariable.configSetting.waiYuanAFlag.FindLowWhiteBlockEnable;
            ckbFlag8.Checked = GlobalVariable.configSetting.waiYuanAFlag.FindOuterLoopBlackMohengEnable;
            ckbFlag9.Checked = GlobalVariable.configSetting.waiYuanAFlag.FindOuterLoopWhiteMohengEnable;
            ckbFlag10.Checked = GlobalVariable.configSetting.waiYuanAFlag.FindOuterLoopCrackEnable;
            ckbFlag11.Checked = GlobalVariable.configSetting.waiYuanAFlag.FindOuterLoopBackImageMoHengEnable;
            ckbFlag12.Checked = GlobalVariable.configSetting.waiYuanAFlag.FindOuterLoopVerBlackCaShangEnable;
            ckbFlag13.Checked = GlobalVariable.configSetting.waiYuanAFlag.FindOuterLoopZhenWenEnable;
            checkBox1.Checked = GlobalVariable.configSetting.waiYuanAFlag.upRoundBlackRegionEnable;
            checkBox2.Checked = GlobalVariable.configSetting.waiYuanAFlag.downRoundBlackRegionEnable;
            checkBox3.Checked = GlobalVariable.configSetting.waiYuanAFlag.wideMoHenEnable;
            checkBox4.Checked = GlobalVariable.configSetting.waiYuanAFlag.flags[0];
            #endregion flag
        }

        public bool SaveParam(ref string ErrorMsg)
        {
            #region 1

            GlobalVariable.configSetting.waiYuanAParam.bAutoSearchRegion = ckb1.Checked;
            GlobalVariable.configSetting.waiYuanAParam.dwSampleRegionMinMeanGray = Convert.ToInt32(txt2.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwValidRegionStartPosOffset = Convert.ToInt32(txt3.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwValidRegionGrayThresOffset = Convert.ToInt32(txt4.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwValidRegionSmoothLen = Convert.ToInt32(txt5.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwValidRegionUpOffset = Convert.ToInt32(txt6.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwValidRegionDownOffset = Convert.ToInt32(txt7.Text);

            GlobalVariable.configSetting.waiYuanAParam.dwBlackValidRegionUpOffset = Convert.ToInt32(txt8.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwBlackValidRegionDownOffset = Convert.ToInt32(txt9.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwValidRegionWidthOffset = Convert.ToInt32(txt17.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwValidRegionHeightOffset = Convert.ToInt32(txt18.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwHighBlackBlobGrayOffset = Convert.ToInt32(txt21.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwHighBlackBlobMinArea = Convert.ToUInt32(txt22.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwAllHighBlackBlockMinArea = Convert.ToUInt32(txt23.Text);

            GlobalVariable.configSetting.waiYuanAParam.dwLowBlackBlobGrayOffset = Convert.ToInt32(txt26.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwLowBlackBlobMinArea = Convert.ToUInt32(txt27.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwHighWhiteBlobGrayOffset = Convert.ToInt32(txt28.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwHighWhiteBlobMinArea = Convert.ToUInt32(txt29.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwLowWhiteBlobGrayOffset = Convert.ToInt32(txt30.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwLowWhiteBlobMinArea = Convert.ToUInt32(txt31.Text);

            GlobalVariable.configSetting.waiYuanAParam.dwBlackBlockJindu = (float)Convert.ToDouble(txt32.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwBlackBlockMinArea = Convert.ToUInt32(txt33.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwLowBlackDynGrayOffset = Convert.ToInt32(txt34.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwLowBlackMeanLen = Convert.ToUInt32(txt35.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwLowBlackContLength = Convert.ToUInt32(txt36.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwMiddleRegionBlackBlockOffset = Convert.ToUInt32(txt37.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwMiddleRegionBlackBlockMinArea = Convert.ToUInt32(txt38.Text);

            GlobalVariable.configSetting.waiYuanAParam.dwMoHengValidRegionUpOffset = Convert.ToInt32(txt50.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwMoHengValidRegionDownOffset = Convert.ToInt32(txt51.Text);
            GlobalVariable.configSetting.waiYuanAParam.fMohengGraySmoothLen = Convert.ToInt32(txt54.Text);
            GlobalVariable.configSetting.waiYuanAParam.fMohengGrayMinOffset = Convert.ToInt32(txt55.Text);
            GlobalVariable.configSetting.waiYuanAParam.fMohengGrayMinArea = Convert.ToInt32(txt56.Text);
            GlobalVariable.configSetting.waiYuanAParam.fMohengGrayMinDistance = Convert.ToInt32(txt57.Text);
            GlobalVariable.configSetting.waiYuanAParam.fMohengGraySingleMinLen = Convert.ToInt32(txt58.Text);
            GlobalVariable.configSetting.waiYuanAParam.fMohengGraySumMinLen = Convert.ToInt32(txt59.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwMohengMeanGrayMinOffset = Convert.ToInt32(txt60.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwLieHengJingdu = (float)Convert.ToDouble(txt68.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwLieHengFactor = (float)Convert.ToDouble(txt69.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwLieHengMinGrayOffset = Convert.ToUInt32(txt70.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwLieHengMaxGrayOffset = Convert.ToUInt32(txt71.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwLieHengMinDist = Convert.ToUInt32(txt72.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwLieHengHeight = Convert.ToUInt32(txt73.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwMeanGrayMaxOffset = Convert.ToInt32(txt84.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwMeanGrayMinOffset = Convert.ToInt32(txt85.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwRoundRegionBlackBlockJindu = (float)Convert.ToDouble(txt201.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwUpRoundRegionUpOffset = Convert.ToInt32(txt203.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwUpRoundRegionDownOffset = Convert.ToInt32(txt204.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwUpRoundRegionBlackBlockGrayOffset = Convert.ToInt32(txt205.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwUpRoundRegionBlackBlockMinArea = Convert.ToUInt32(txt206.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwUpRoundRegionMeanGrayMaxOffset = Convert.ToInt32(txt207.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwUpRoundRegionMeanGrayMinOffset = Convert.ToInt32(txt208.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwDownRoundRegionUpOffset = Convert.ToInt32(txt209.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwDownRoundRegionDownOffset = Convert.ToInt32(txt210.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwDownRoundRegionBlackBlockGrayOffset = Convert.ToInt32(txt211.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwDownRoundRegionBlackBlockMinArea = Convert.ToUInt32(txt212.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwDownRoundRegionMeanGrayMaxOffset = Convert.ToInt32(txt213.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwDownRoundRegionMeanGrayMinOffset = Convert.ToInt32(txt214.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwUpRoundHeighMaxOffsetRatio = (float)Convert.ToDouble(txt215.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwDownRoundHeighMaxOffsetRatio = (float)Convert.ToDouble(txt216.Text);

            GlobalVariable.configSetting.waiYuanAParam.upRoundBlackRegionMaxGray = Convert.ToInt32(textBox1.Text);
            GlobalVariable.configSetting.waiYuanAParam.upRoundBlackRegionMinArea = Convert.ToInt32(textBox2.Text);
            GlobalVariable.configSetting.waiYuanAParam.downRoundBlackRegionMaxGray = Convert.ToInt32(textBox3.Text);
            GlobalVariable.configSetting.waiYuanAParam.downRoundBlackRegionMinArea = Convert.ToInt32(textBox4.Text);


            #endregion 1

            #region 2

            GlobalVariable.configSetting.waiYuanAParam.dwBlackImageMohengUpDownOffset = Convert.ToInt32(txt301.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwBlackImageMohengLeftRightOffset = Convert.ToInt32(txt302.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwBlackImageMohengMeanLen = Convert.ToInt32(txt303.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwBlackImageMohengGrayOffset = Convert.ToInt32(txt303_2.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwBlackImageMohengFilterMinLen = Convert.ToInt32(txt304.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwBlackImageMohengMinDistance = Convert.ToInt32(txt305.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwBlackImageMohengMinLen = Convert.ToInt32(txt306.Text);

            GlobalVariable.configSetting.waiYuanAParam.fZhenWenJindu = (float)Convert.ToDouble(txt307.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwZhenWenFilterMinArea = Convert.ToInt32(txt308.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwZhenWenErrMinArea = Convert.ToInt32(txt309.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwZhenWenErrCount = Convert.ToInt32(txt310.Text);
            GlobalVariable.configSetting.waiYuanAParam.fCaErrJindu = (float)Convert.ToDouble(txt311.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwCaFilterMinArea = Convert.ToInt32(txt312.Text);
            GlobalVariable.configSetting.waiYuanAParam.dwCaErrMinArea = Convert.ToInt32(txt313.Text);
            GlobalVariable.configSetting.waiYuanAParam.wideMoHenMinWidth = Convert.ToInt32(textBox5.Text);
            GlobalVariable.configSetting.waiYuanAParam.wideMoHenMinOffset = Convert.ToInt32(textBox6.Text);
            GlobalVariable.configSetting.waiYuanAParam.wideMoHenClosingRadius = Convert.ToInt32(textBox7.Text);


            GlobalVariable.configSetting.waiYuanAParam.intParams[0] = Convert.ToInt32(textBox8.Text);
            GlobalVariable.configSetting.waiYuanAParam.intParams[4] = Convert.ToInt32(textBox9.Text);
            GlobalVariable.configSetting.waiYuanAParam.intParams[3] = Convert.ToInt32(textBox10.Text);
            GlobalVariable.configSetting.waiYuanAParam.intParams[2] = Convert.ToInt32(textBox11.Text);
            GlobalVariable.configSetting.waiYuanAParam.intParams[1] = Convert.ToInt32(textBox12.Text);
            GlobalVariable.configSetting.waiYuanAParam.intParams[7] = Convert.ToInt32(textBox13.Text);

            #endregion 2

            #region flag

            GlobalVariable.configSetting.waiYuanAFlag.CalBlackBlockEnable = ckbFlag1.Checked;
            GlobalVariable.configSetting.waiYuanAFlag.FFTOuterSideImageEnable = ckbFlag2.Checked;
            GlobalVariable.configSetting.waiYuanAFlag.FindHighBlackBlockEnable = ckbFlag3.Checked;
            GlobalVariable.configSetting.waiYuanAFlag.FindLowBlackBlockEnable = ckbFlag4.Checked;
            GlobalVariable.configSetting.waiYuanAFlag.FindLowBlackBlockByMeanImageEnable = ckbFlag5.Checked;
            GlobalVariable.configSetting.waiYuanAFlag.FindHighWhiteBlockEnable = ckbFlag6.Checked;
            GlobalVariable.configSetting.waiYuanAFlag.FindLowWhiteBlockEnable = ckbFlag7.Checked;
            GlobalVariable.configSetting.waiYuanAFlag.FindOuterLoopBlackMohengEnable = ckbFlag8.Checked;
            GlobalVariable.configSetting.waiYuanAFlag.FindOuterLoopWhiteMohengEnable = ckbFlag9.Checked;
            GlobalVariable.configSetting.waiYuanAFlag.FindOuterLoopCrackEnable = ckbFlag10.Checked;
            GlobalVariable.configSetting.waiYuanAFlag.FindOuterLoopBackImageMoHengEnable = ckbFlag11.Checked;
            GlobalVariable.configSetting.waiYuanAFlag.FindOuterLoopVerBlackCaShangEnable = ckbFlag12.Checked;
            GlobalVariable.configSetting.waiYuanAFlag.FindOuterLoopZhenWenEnable = ckbFlag13.Checked;
            GlobalVariable.configSetting.waiYuanAFlag.upRoundBlackRegionEnable = checkBox1.Checked;
            GlobalVariable.configSetting.waiYuanAFlag.downRoundBlackRegionEnable = checkBox2.Checked;
            GlobalVariable.configSetting.waiYuanAFlag.wideMoHenEnable = checkBox3.Checked;
            GlobalVariable.configSetting.waiYuanAFlag.flags[0] = checkBox4.Checked;
            #endregion flag

            return true;
        }

        #region 最大最小默认值

        private void txt2_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "110";
                return;
            }
        }

        private void txt3_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "0";
                return;
            }
        }

        private void txt4_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "50";
                return;
            }
        }

        private void txt5_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "500";
                return;
            }
        }

        private void txt6_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "5";
                return;
            }
        }

        private void txt7_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "-5";
                return;
            }
        }

        private void txt17_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "40";
                return;
            }
        }

        private void txt18_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "15";
                return;
            }
        }

        private void txt8_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "2";
                return;
            }
        }

        private void txt9_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "-2";
                return;
            }
        }

        private void txt32_Leave(object sender, EventArgs e)
        {
            double ccc = 0;
            TextBox t = sender as TextBox;
            if (!double.TryParse(t.Text, out ccc))
            {
                t.Text = "0.4";
                return;
            }
        }

        private void txt33_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "80";
                return;
            }
        }

        private void txt37_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "5";
                return;
            }
        }

        private void txt38_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "20";
                return;
            }
        }

        private void txt21_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "50";
                return;
            }
        }

        private void txt22_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "80";
                return;
            }
        }

        private void txt23_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "100";
                return;
            }
        }

        private void txt34_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "20";
                return;
            }
        }

        private void txt35_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "20";
                return;
            }
        }

        private void txt36_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "100";
                return;
            }
        }

        private void txt26_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "30";
                return;
            }
        }

        private void txt27_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "100";
                return;
            }
        }

        private void txt28_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "-50";
                return;
            }
        }

        private void txt29_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "200";
                return;
            }
        }

        private void txt30_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "-30";
                return;
            }
        }

        private void txt31_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "1000";
                return;
            }
        }

        private void txt84_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "50";
                return;
            }
        }

        private void txt85_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "50";
                return;
            }
        }

        private void txt201_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "2";
                return;
            }
        }

        private void txt203_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "2";
                return;
            }
        }

        private void txt204_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "-2";
                return;
            }
        }

        private void txt205_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "30";
                return;
            }
        }

        private void txt206_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "250";
                return;
            }
        }

        private void txt207_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "50";
                return;
            }
        }

        private void txt208_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "50";
                return;
            }
        }

        private void txt215_Leave(object sender, EventArgs e)
        {
            double ccc = 0;
            TextBox t = sender as TextBox;
            if (!double.TryParse(t.Text, out ccc))
            {
                t.Text = "0.4";
                return;
            }
        }

        private void txt209_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "2";
                return;
            }
        }

        private void txt210_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "-2";
                return;
            }
        }

        private void txt211_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "30";
                return;
            }
        }

        private void txt212_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "250";
                return;
            }
        }

        private void txt213_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "50";
                return;
            }
        }

        private void txt214_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "50";
                return;
            }
        }

        private void txt216_Leave(object sender, EventArgs e)
        {
            double ccc = 0;
            TextBox t = sender as TextBox;
            if (!double.TryParse(t.Text, out ccc))
            {
                t.Text = "0.4";
                return;
            }
        }

        private void txt50_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "15";
                return;
            }
        }

        private void txt51_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "-15";
                return;
            }
        }

        private void txt69_Leave(object sender, EventArgs e)
        {
            double ccc = 0;
            TextBox t = sender as TextBox;
            if (!double.TryParse(t.Text, out ccc))
            {
                t.Text = "3";
                return;
            }
        }

        private void txt70_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "2";
                return;
            }
        }

        private void txt71_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "8";
                return;
            }
        }

        private void txt72_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "20";
                return;
            }
        }

        private void txt73_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "100";
                return;
            }
        }

        private void txt301_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "15";
                return;
            }
        }

        private void txt302_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "10";
                return;
            }
        }

        private void txt303_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "20";
                return;
            }
        }

        private void txt303_2_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "8";
                return;
            }
        }

        private void txt304_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "25";
                return;
            }
        }

        private void txt305_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "150";
                return;
            }
        }

        private void txt306_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "800";
                return;
            }
        }

        private void txt307_Leave(object sender, EventArgs e)
        {
            double ccc = 0;
            TextBox t = sender as TextBox;
            if (!double.TryParse(t.Text, out ccc))
            {
                t.Text = "1.2";
                return;
            }
        }

        private void txt308_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "80";
                return;
            }
        }

        private void txt309_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "500";
                return;
            }
        }

        private void txt310_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "6";
                return;
            }
        }

        private void txt311_Leave(object sender, EventArgs e)
        {
            double ccc = 0;
            TextBox t = sender as TextBox;
            if (!double.TryParse(t.Text, out ccc))
            {
                t.Text = "0.7";
                return;
            }
        }

        private void txt312_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "30";
                return;
            }
        }

        private void txt313_Leave(object sender, EventArgs e)
        {
            int ccc = 0;
            TextBox t = sender as TextBox;
            if (!int.TryParse(t.Text, out ccc))
            {
                t.Text = "300";
                return;
            }
        }

        #endregion 最大最小默认值
    }
}