using System;
using System.IO;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class LoadConfigForm : Form
    {
        public string configName = "";

        public LoadConfigForm()
        {
            InitializeComponent();
            btnOK.Enabled = false;
            btnLoad.Enabled = false;
        }

        private void LoadConfigForm_Load(object sender, EventArgs e)
        {
            textBox1.Enabled = false;
            UIHelper.SetDGVFormat_Change(dgvConfigName, 30);
            //string[] path = Directory.GetDirectories(Application.StartupPath + "\\Param");
            DirectoryInfo di = new DirectoryInfo(Application.StartupPath + "\\Param");
            DirectoryInfo[] dd = di.GetDirectories();
            for (int i = 0; i < dd.Length; i++)
            {
                dgvConfigName.Rows.Add();
                dgvConfigName.Rows[i].Cells[0].Value = dd[i].Name;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            configName = textBox1.Text;
            DialogResult = DialogResult.OK;
        }

        private void dgvConfigName_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnOK.Enabled = true;
            btnLoad.Enabled = true;
            textBox1.Text = dgvConfigName.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            configName = textBox1.Text;
            DialogResult = DialogResult.Yes;
        }
    }
}