using BearingInspection;
using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class FrmResultInfo : Form
    {
        public FrmResultInfo()
        {
            InitializeComponent();
            refreshData();
        }

        public void refreshData()
        {
            InitCtrlDuanMianA();
            InitCtrlDuanMianB();
            if (GlobalVariable.iWorkStation != 2)
            {
                InitCtrlWaiYuanA();
                InitCtrlDaoJiaoA();
                InitCtrlNeiKong();
                InitCtrlWaiYuanB();
                InitCtrlDaoJiaoB();
            }
        }

        public void InitCtrlDuanMianA()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Value", typeof(string));

            int id = 0;
            foreach (FieldInfo field in typeof(Surface_Result_Info).GetFields())
            {
                if (field.FieldType.IsArray)
                {
                    Array arr = (Array)field.GetValue(GlobalVariable.duanMianAAutoFlow.info);
                    if (arr == null)
                        continue;

                    id++;
                    string str = "";
                    foreach (var item in arr)
                    {
                        str += item.ToString();
                        str += "\r\n";
                    }
                    dt.Rows.Add(id, field.Name, str);
                }
                else
                {
                    id++;
                    dt.Rows.Add(id, field.Name, field.GetValue(GlobalVariable.duanMianAAutoFlow.info));
                }
            }

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
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
            foreach (FieldInfo field in typeof(OuterSide_Result_Info).GetFields())
            {
                id++;
                dt.Rows.Add(id, field.Name, field.GetValue(GlobalVariable.waiYuanAAutoFlow.info));
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
            foreach (FieldInfo field in typeof(MultiImg_Result_Info).GetFields())
            {
                id++;
                dt.Rows.Add(id, field.Name, field.GetValue(GlobalVariable.daoJiaoAAutoFlow.info));
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
            foreach (FieldInfo field in typeof(OuterSide_Result_Info).GetFields())
            {
                id++;
                dt.Rows.Add(id, field.Name, field.GetValue(GlobalVariable.neiKongAutoFlow.info));
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
            foreach (FieldInfo field in typeof(Surface_Result_Info).GetFields())
            {
                if (field.FieldType.IsArray)
                {
                    Array arr = (Array)field.GetValue(GlobalVariable.duanMianBAutoFlow.info);
                    if (arr == null)
                        continue;

                    id++;
                    string str = "";
                    foreach (var item in arr)
                    {
                        str += item.ToString();
                        str += "\r\n";
                    }
                    dt.Rows.Add(id, field.Name, str);
                }
                else
                {
                    id++;
                    dt.Rows.Add(id, field.Name, field.GetValue(GlobalVariable.duanMianBAutoFlow.info));
                }
            }

            dataGridView5.DataSource = dt;
            dataGridView5.Columns[2].DefaultCellStyle.WrapMode = DataGridViewTriState.True;
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
            foreach (FieldInfo field in typeof(MultiImg_Result_Info).GetFields())
            {
                id++;
                dt.Rows.Add(id, field.Name, field.GetValue(GlobalVariable.waiYuanBAutoFlow.info));
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
            foreach (FieldInfo field in typeof(MultiImg_Result_Info).GetFields())
            {
                id++;
                dt.Rows.Add(id, field.Name, field.GetValue(GlobalVariable.daoJiaoBAutoFlow.info));
            }

            dataGridView7.DataSource = dt;
            dataGridView7.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dataGridView7.ReadOnly = true; // 设置只读，防止用户修改数据
        }
    }
}