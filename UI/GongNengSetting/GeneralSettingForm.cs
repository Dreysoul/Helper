using System;
using System.Linq;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class GeneralSettingForm : UserControl
    {
        private string ErrorMsg = "";

        public GeneralSettingForm()
        {
            InitializeComponent();
        }

        private void GeneralSettingForm_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }

        public void LoadForm()
        {
            ckbbUseLog.Checked = GlobalVariable.generalSetting.bUseLog;
            txtSleepTime.Text = GlobalVariable.generalSetting.sleepTime.ToString();
        }

        private bool SaveForm(ref string ErrorMsg)
        {
            GlobalVariable.generalSetting.bUseLog = ckbbUseLog.Checked;
            GlobalVariable.generalSetting.sleepTime = Convert.ToInt32(txtSleepTime.Text);
            return JsonHelper.WriteJsonFile(GlobalVariable.generalSetting, FilePath.GeneralSettingPath, ref ErrorMsg);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool bRtn = SaveForm(ref ErrorMsg);
            if (bRtn)
            {
                HomeForm.home.ShowLogPage();
                MessageBox.Show("保存成功");
            }
            else
            {
                GlobalMethod.ShowMessage(ErrorMsg);
            }
        }

        private void chkOutput01_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox pchk = (CheckBox)(sender);
            string order = pchk.Name.Substring(pchk.Name.Length - 2, 2);
            int index = int.Parse(order) - 1;
            GlobalVariable.ioBoardHelper.setOutput(index, pchk.Checked);
        }

        public void stopTimer()
        {
            timer1.Enabled = false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = 0; i < 16; i++)
            {
                bool bRet = GlobalVariable.ioBoardHelper.readInput(i);
                Control[] chk = this.Controls.Find("chkInput" + (i + 1).ToString("D2"), false);
                if (chk.Count() >= 1)
                {
                    CheckBox pb = (chk[0]) as CheckBox;
                    pb.Checked = bRet;
                }
            }
        }
    }
}