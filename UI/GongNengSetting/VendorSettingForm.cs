using System;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class VendorSettingForm : UserControl
    {
        private string ErrorMsg = "";

        public VendorSettingForm()
        {
            InitializeComponent();
        }

        private void VendorSettingForm_Load(object sender, EventArgs e)
        {
            cmbMachineType.Items.Clear();
            cmbMachineType.Items.Add("四个相机");
            cmbMachineType.Items.Add("三个相机");
            cmbMachineType.SelectedIndex = 0;
            LoadForm();
        }

        public void LoadForm()
        {
            cmbMachineType.SelectedIndex = GlobalVariable.machineSetting.MachineType;
        }

        private bool SaveForm(ref string ErrorMsg)
        {
            GlobalVariable.machineSetting.MachineType = cmbMachineType.SelectedIndex;
            return JsonHelper.WriteJsonFile(GlobalVariable.machineSetting, FilePath.VendorSettingPath, ref ErrorMsg);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("请再确认是否保存厂商参数，确认即保存参数后关闭软件！！", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                bool bRtn = SaveForm(ref ErrorMsg);
                if (bRtn)
                {
                    Environment.Exit(0);
                }
                else
                {
                    GlobalMethod.ShowMessage(ErrorMsg);
                }
            }
        }
    }
}