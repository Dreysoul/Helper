using System;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class HardwareSettingForm : Form
    {
        public HardwareSettingForm()
        {
            InitializeComponent();
        }

        private void HardwareSettingForm1_Load(object sender, EventArgs e)
        {
            txtDuanMianACameraSN.Text = GlobalVariable.hardwareSetting.DuanMianACameraSn;
            txtWaiYuanACameraSN.Text = GlobalVariable.hardwareSetting.WaiYuanACameraSn;
            txtDaoJiaoACameraSN.Text = GlobalVariable.hardwareSetting.DaoJiaoACameraSn;
            txtNeiKongCameraSN.Text = GlobalVariable.hardwareSetting.NeiKongCameraSn;
            txtDuanMianBCameraSN.Text = GlobalVariable.hardwareSetting.DuanMianBCameraSn;
            txtWaiYuanBCameraSN.Text = GlobalVariable.hardwareSetting.WaiYuanBCameraSn;
            txtDaoJiaoBCameraSN.Text = GlobalVariable.hardwareSetting.DaoJiaoBCameraSn;
            txtLight1Com.Text = GlobalVariable.hardwareSetting.light1Com;
            txtLight2Com.Text = GlobalVariable.hardwareSetting.light2Com;

            if (GlobalVariable.iWorkStation == 2)
            {
                lblWaiYuanASN.Visible = false;
                txtWaiYuanACameraSN.Visible = false;
                lblDaoJiaoASN.Visible = false;
                txtDaoJiaoACameraSN.Visible = false;
                lblNeiKongSN.Visible = false;
                txtNeiKongCameraSN.Visible = false;
                lblWaiYuanBSN.Visible = false;
                txtWaiYuanBCameraSN.Visible = false;
                lblDaoJiaoBSN.Visible = false;
                txtDaoJiaoBCameraSN.Visible = false;
            }
        }

        private bool SaveHardwareSetting(ref string ErrorMsg)
        {
            bool bRtn = false;
            try
            {
                GlobalVariable.hardwareSetting.DuanMianACameraSn = txtDuanMianACameraSN.Text;
                GlobalVariable.hardwareSetting.WaiYuanACameraSn = txtWaiYuanACameraSN.Text;
                GlobalVariable.hardwareSetting.DaoJiaoACameraSn = txtDaoJiaoACameraSN.Text;
                GlobalVariable.hardwareSetting.NeiKongCameraSn = txtNeiKongCameraSN.Text;
                GlobalVariable.hardwareSetting.DuanMianBCameraSn = txtDuanMianBCameraSN.Text;
                GlobalVariable.hardwareSetting.WaiYuanBCameraSn = txtWaiYuanBCameraSN.Text;
                GlobalVariable.hardwareSetting.DaoJiaoBCameraSn = txtDaoJiaoBCameraSN.Text;
                GlobalVariable.hardwareSetting.light1Com = txtLight1Com.Text;
                GlobalVariable.hardwareSetting.light2Com = txtLight2Com.Text;
                bRtn = JsonHelper.WriteJsonFile(GlobalVariable.hardwareSetting, FilePath.HardwareSettingPath, ref ErrorMsg);
                return bRtn;
            }
            catch (Exception e)
            {
                ErrorMsg = e.Message;
                return false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string ErrorMsg = "";
            bool bRtn = SaveHardwareSetting(ref ErrorMsg);
            if (bRtn)
            {
                GlobalMethod.ShowMessage("保存成功");
            }
            else
            {
                GlobalMethod.ShowMessage("保存失败，错误信息为" + ErrorMsg);
            }

            string[] cameraSN = {  GlobalVariable.hardwareSetting.DuanMianACameraSn,
                                   GlobalVariable.hardwareSetting.WaiYuanACameraSn,
                                   GlobalVariable.hardwareSetting.DaoJiaoACameraSn,
                                   GlobalVariable.hardwareSetting.NeiKongCameraSn,
                                   GlobalVariable.hardwareSetting.DuanMianBCameraSn,
                                   GlobalVariable.hardwareSetting.WaiYuanBCameraSn,
                                   GlobalVariable.hardwareSetting.DaoJiaoBCameraSn };
            IAreaScanCameraHelper[] cameraHelper = { GlobalVariable.DuanMianACameraHelper,
                                                     GlobalVariable.WaiYuanACameraHelper,
                                                     GlobalVariable.DaoJiaoACameraHelper,
                                                     GlobalVariable.NeiKongCameraHelper,
                                                     GlobalVariable.DuanMianBCameraHelper,
                                                     GlobalVariable.WaiYuanBCameraHelper,
                                                     GlobalVariable.DaoJiaoBCameraHelper};
            for (int i = 0; i < 7; i++)
            {
                cameraHelper[i].CameraClose();
                cameraHelper[i].CameraOpen(cameraSN[i]);
            }
        }
    }
}