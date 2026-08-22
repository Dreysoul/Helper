using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class FrmErrCode : Form
    {
        public FrmErrCode()
        {
            InitializeComponent();

            InitCtrlDuanMianA();
            InitCtrlWaiYuanA();
            InitCtrlDaoJiaoA();
            InitCtrlNeiKong();
            InitCtrlDuanMianB();
            InitCtrlWaiYuanB();
            InitCtrlDaoJiaoB();
        }

        public void InitCtrlDuanMianA()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));

            int id = 0;
            foreach (KeyValuePair<string, int> kvp in GlobalVariable.duanMianAErrCodeStatistic)
            {
                id++;
                dt.Rows.Add(id, kvp.Key, kvp.Value);
            }
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView1.ReadOnly = true; // 设置只读，防止用户修改数据
        }

        public void InitCtrlWaiYuanA()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));

            int id = 0;
            foreach (KeyValuePair<string, int> kvp in GlobalVariable.waiYuanAErrCodeStatistic)
            {
                id++;
                dt.Rows.Add(id, kvp.Key, kvp.Value);
            }
            dataGridView2.DataSource = dt;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView2.ReadOnly = true; // 设置只读，防止用户修改数据
        }

        public void InitCtrlDaoJiaoA()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));

            int id = 0;
            foreach (KeyValuePair<string, int> kvp in GlobalVariable.daoJiaoAErrCodeStatistic)
            {
                id++;
                dt.Rows.Add(id, kvp.Key, kvp.Value);
            }
            dataGridView3.DataSource = dt;
            dataGridView3.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView3.ReadOnly = true; // 设置只读，防止用户修改数据
        }

        public void InitCtrlNeiKong()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));

            int id = 0;
            foreach (KeyValuePair<string, int> kvp in GlobalVariable.neiKongErrCodeStatistic)
            {
                id++;
                dt.Rows.Add(id, kvp.Key, kvp.Value);
            }
            dataGridView4.DataSource = dt;
            dataGridView4.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView4.ReadOnly = true; // 设置只读，防止用户修改数据
        }

        public void InitCtrlDuanMianB()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));

            int id = 0;
            foreach (KeyValuePair<string, int> kvp in GlobalVariable.duanMianBErrCodeStatistic)
            {
                id++;
                dt.Rows.Add(id, kvp.Key, kvp.Value);
            }
            dataGridView5.DataSource = dt;
            dataGridView5.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView5.ReadOnly = true; // 设置只读，防止用户修改数据
        }

        public void InitCtrlWaiYuanB()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));

            int id = 0;
            foreach (KeyValuePair<string, int> kvp in GlobalVariable.waiYuanBErrCodeStatistic)
            {
                id++;
                dt.Rows.Add(id, kvp.Key, kvp.Value);
            }
            dataGridView6.DataSource = dt;
            dataGridView6.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView6.ReadOnly = true; // 设置只读，防止用户修改数据
        }

        public void InitCtrlDaoJiaoB()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));

            int id = 0;
            foreach (KeyValuePair<string, int> kvp in GlobalVariable.daoJiaoBErrCodeStatistic)
            {
                id++;
                dt.Rows.Add(id, kvp.Key, kvp.Value);
            }
            dataGridView7.DataSource = dt;
            dataGridView7.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView7.ReadOnly = true; // 设置只读，防止用户修改数据
        }
    }
}