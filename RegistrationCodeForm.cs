using System;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class RegistrationCodeForm : Form
    {
        public RegistrationCodeForm()
        {
            InitializeComponent();
        }

        private void RegistrationCodeForm_Load(object sender, EventArgs e)
        {
            txtMachineCode.Text = SecurityHelper.GetMD5(SecurityHelper.GetMachineCodeString());
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            bool b = RegistrationCodeFunction.Check(txtTargetCode.Text.Trim());
            if (b)
            {
                RegistrationCodeFunction.Record(txtTargetCode.Text.Trim());
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                MessageBox.Show("校验失败");
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}