using System;
using System.IO;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class ParamSettingForm : Form
    {
        private SideABParamControl duanMianAParamControl;
        private SideOutParamControl waiYuanAParamControl;
        private SideABParamControl duanMianBParamControl;

        public ParamSettingForm()
        {
            InitializeComponent();

            duanMianAParamControl = new SideABParamControl(0);
            waiYuanAParamControl = new SideOutParamControl();
            duanMianBParamControl = new SideABParamControl(1);

            UIHelper.AddTabPage(tabPageDuanMianA, duanMianAParamControl);
            UIHelper.AddTabPage(tabPageWaiYuanA, waiYuanAParamControl);
            UIHelper.AddTabPage(tabPageDuanMianB, duanMianBParamControl);

            if (GlobalVariable.iWorkStation == 2)
            {
                tabControl1.TabPages.Remove(tabControl1.TabPages[1]);
            }
        }

      private void btnSave_Click(object sender, EventArgs e)
        {
              // 创建主配置文件夹
            if (!Directory.Exists(FilePath.ParamSettingPath + GlobalVariable.configname))
            {
                Directory.CreateDirectory(FilePath.ParamSettingPath + GlobalVariable.configname);
            }
            if(!Directory.Exists(FilePath.ParamSettingPath + "ParamBackup"))
            {
                Directory.CreateDirectory(FilePath.ParamSettingPath + "ParamBackup");
            }
            string ErrorMsg = "";
            bool bRtn = false;

            // 1. 保存端面A参数
            bRtn = duanMianAParamControl.SaveParam(ref ErrorMsg);
            if (!bRtn)
            {
                MessageBox.Show("端面A参数保存失败，异常信息为" + ErrorMsg);
                return;
            }
           
            // 2. 保存外圆A参数
            bRtn = waiYuanAParamControl.SaveParam(ref ErrorMsg);
            if (!bRtn)
            {
                MessageBox.Show("外圆A参数保存失败，异常信息为" + ErrorMsg);
                return;
            }

            // 3. 保存端面B参数
            bRtn = duanMianBParamControl.SaveParam(ref ErrorMsg);
            if (!bRtn)
            {
                MessageBox.Show("端面B参数保存失败，异常信息为" + ErrorMsg);
                return;
            }

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
    }
}