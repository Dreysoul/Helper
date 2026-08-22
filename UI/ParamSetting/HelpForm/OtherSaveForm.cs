using System;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class OtherSaveForm : Form
    {
        public string name = "";

        public OtherSaveForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                GlobalMethod.ShowMessage("请输入字符");
                return;
            }
            name = textBox1.Text;
            DialogResult = DialogResult.OK;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}