using BearingInspection;
using SvPatMax;
using System;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class HomeForm : UserControl
    {
        public static HomeForm home = null;
        Thread th_DuanMianA_Ctrl;
        Thread th_DuanMianA_Calc;
        Thread th_DuanMianA_Show;

        Thread th_WaiYuanA_Ctrl;
        Thread th_WaiYuanA_Calc;
        Thread th_WaiYuanA_Show;

        Thread th_DaoJiaoA_Ctrl;
        Thread th_DaoJiaoA_Calc;
        Thread th_DaoJiaoA_Show;

        Thread th_NeiKong_Ctrl;
        Thread th_NeiKong_Calc;
        Thread th_NeiKong_Show;

        Thread th_DuanMianB_Ctrl;
        Thread th_DuanMianB_Calc;
        Thread th_DuanMianB_Show;

        Thread th_WaiYuanB_Ctrl;
        Thread th_WaiYuanB_Calc;
        Thread th_WaiYuanB_Show;

        Thread th_DaoJiaoB_Ctrl;
        Thread th_DaoJiaoB_Calc;
        Thread th_DaoJiaoB_Show;
        public HomeForm()
        {
            InitializeComponent();
            CreateAllThread();
            SetStyle(
            ControlStyles.OptimizedDoubleBuffer
            | ControlStyles.ResizeRedraw
            | ControlStyles.Selectable
            | ControlStyles.AllPaintingInWmPaint
            | ControlStyles.UserPaint
            | ControlStyles.SupportsTransparentBackColor,
            true);
            this.UpdateStyles();
            home = this;
        }

        /// <summary>
        /// 显示的图片界面，0=SideA,1=SideB,2=SideIn,3=SideOut
        /// </summary>
        public ShowPictureControl duanMianAPicForm = new ShowPictureControl(0);
        public ShowPictureControl waiYuanAPicForm = new ShowPictureControl(1);
        public ShowPictureControl daoJiaoAPicForm = new ShowPictureControl(2);
        public ShowPictureControl neiKongPicForm = new ShowPictureControl(3);
        public ShowPictureControl duanMianBPicForm = new ShowPictureControl(4);
        public ShowPictureControl waiYuanBPicForm = new ShowPictureControl(5);
        public ShowPictureControl daoJiaoBPicForm = new ShowPictureControl(6);
        public ShowPictureControl neiKongBPicForm = new ShowPictureControl(7);
        private ModelSettingForm modelSettingForm = new ModelSettingForm();

        private void HomeForm_Load(object sender, EventArgs e)
        {
            if (GlobalVariable.iWorkStation == 2)
            {
                tableMain.Controls.Remove(tableAll);
                tableAll.Dispose();

                TableLayoutPanel tableNew = new TableLayoutPanel();
                tableNew.Dock = DockStyle.Fill;
                tableNew.Name = "tableNew";
                tableNew.RowCount = 1;
                tableNew.ColumnCount = 2;
                // 设置列宽比例
                tableNew.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
                tableNew.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
                // 设置行高
                tableNew.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
                duanMianAPicForm.Dock = DockStyle.Fill;
                tableNew.Controls.Add(duanMianAPicForm, 0, 0);
                duanMianBPicForm.Dock = DockStyle.Fill;
                tableNew.Controls.Add(duanMianBPicForm, 1, 0);
                tableMain.Controls.Add(tableNew, 0, 0);
            }
            else if(GlobalVariable.iWorkStation == 5)
            {
                tableMain.Controls.Remove(tableAll);
                tableAll.Dispose();
                TableLayoutPanel tableNew = new TableLayoutPanel();
                tableNew.Dock = DockStyle.Fill;
                tableNew.Name = "tableNew";
                tableNew.RowCount = 2;
                tableNew.ColumnCount = 3;
                // 设置列宽比例
                tableNew.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
                tableNew.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
                tableNew.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
                // 设置行高
                tableNew.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
                tableNew.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
                duanMianAPicForm.Dock = DockStyle.Fill;
                tableNew.Controls.Add(duanMianAPicForm, 0, 0);
                waiYuanAPicForm.Dock = DockStyle.Fill;
                tableNew.Controls.Add(waiYuanAPicForm, 1, 0);
                tableNew.SetColumnSpan(waiYuanAPicForm, 2);
                //neiKongPicForm.Dock = DockStyle.Fill;
                //tableNew.Controls.Add(neiKongPicForm, 2, 0);
                neiKongPicForm.Dock = DockStyle.Fill;
                tableNew.Controls.Add(neiKongPicForm, 0, 1);
                duanMianBPicForm.Dock = DockStyle.Fill;
                tableNew.Controls.Add(duanMianBPicForm, 1, 1);
                waiYuanBPicForm.Dock = DockStyle.Fill;
                tableNew.Controls.Add(waiYuanBPicForm, 2, 1);
                tableMain.Controls.Add(tableNew, 0, 0);
            }
            else if (GlobalVariable.iWorkStation == 8)
            {
                tableMain.Controls.Remove(tableAll);
                tableAll.Dispose();
                TableLayoutPanel tableNew = new TableLayoutPanel();
                tableNew.Dock = DockStyle.Fill;
                tableNew.Name = "tableNew";
                tableNew.RowCount = 2;
                tableNew.ColumnCount = 4;
                // 设置列宽比例
                tableNew.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
                tableNew.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
                tableNew.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
                tableNew.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
                // 设置行高
                tableNew.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
                tableNew.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
                duanMianAPicForm.Dock = DockStyle.Fill;
                tableNew.Controls.Add(duanMianAPicForm, 0, 0);
                waiYuanAPicForm.Dock = DockStyle.Fill;
                tableNew.Controls.Add(waiYuanAPicForm, 1, 0);
                neiKongPicForm.Dock = DockStyle.Fill;
                tableNew.Controls.Add(neiKongPicForm, 2, 0);
                duanMianBPicForm.Dock = DockStyle.Fill;
                tableNew.Controls.Add(duanMianBPicForm, 0, 1);
                waiYuanBPicForm.Dock = DockStyle.Fill;
                tableNew.Controls.Add(waiYuanBPicForm, 1, 1);
                tableMain.Controls.Add(tableNew, 0, 0);
            }
            else
            {
                ShowPictureControl[] picForm = { duanMianAPicForm, waiYuanAPicForm, daoJiaoAPicForm, neiKongPicForm, duanMianBPicForm, waiYuanBPicForm, daoJiaoBPicForm };
                Panel[] panel = { panel1, panel2, panel3, panel4, panel5, panel6, panel7 };
                for (int i = 0; i < picForm.Length; i++)
                {
                    picForm[i].Dock = DockStyle.Fill;
                    panel[i].Controls.Add(picForm[i]);
                }
            }

            timer1.Interval = 100;
            timer1.Enabled = true;
            ConfigStatus.Text = GlobalVariable.configname;
            VersionStatus.Text = GlobalVariable.version;
            PLCStatus.Text = "     ";
            LoadHome();

            LoginForm.UserManageAction += SetToolButtonEnableByAuthority;
            LoginForm.UserAuthority = UserAutority.Vendor;
            groupCt.Visible = true;

            //tabWaiYuanB.Parent = null;
            //tabPageNG.Parent = null;
            //tabDuanMianB.Parent = null;
            //tabDaoJiaoB.Parent = null;
            //tabPageThrow.Parent = null;
            // if (GlobalVariable.generalSetting.bUseLog)
            // {
            //}
            // else
            // {
            //     groupCt.Visible = false;
            //  }
        }

        private void LoadHome()
        {
            UIHelper.SetDGVFormat_JustShow(dgvDataStatistic, 30);
            dgvDataStatistic.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
            string[] Name1 = new string[] { "端面A", "外圆A", "倒角A", "内孔", "端面B", "外圆B", "倒角B", "总数" };
            for (int i = 0; i < Name1.Length; i++)
            {
                dgvDataStatistic.Rows.Add();
                dgvDataStatistic.Rows[i].Cells[0].Value = Name1[i];
                dgvDataStatistic.Rows[i].Cells[1].Value = 0.ToString();
                dgvDataStatistic.Rows[i].Cells[2].Value = "0%";
            }
            if (GlobalVariable.iWorkStation == 2)
            {
                dgvDataStatistic.Rows[1].Visible = false;
                dgvDataStatistic.Rows[2].Visible = false;
                dgvDataStatistic.Rows[3].Visible = false;
                dgvDataStatistic.Rows[5].Visible = false;
                dgvDataStatistic.Rows[6].Visible = false;
            }
           

            //UIHelper.SetDGVFormat_JustShow(dgvCT, 20);
            //string[] Name2 = new string[] { "A拍照", "A算法", "A结果", "B拍照", "B算法", "B结果", "内圈拍照", "内圈算法", "内圈结果", "外圈拍照", "外圈算法", "外圈结果", };
            //for (int i = 0; i < Name2.Length ; i++)
            //{
            //    dgvCT.Rows.Add();
            //    dgvCT.Rows[i].Cells[0].Value = Name2[i];
            //    dgvCT.Rows[i].Cells[1].Value = 0.ToString();
            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                #region 设备连接状态

                if (GlobalVariable.bPLCConnect)
                {
                    PLCStatus.BackColor = Color.Lime;
                }
                else
                {
                    PLCStatus.BackColor = Color.Red;
                }

                #endregion 设备连接状态

                #region 在界面上显示各种Log记录

                #region 运行日志

                if (LogHelper.Que_DuanMianALog.Count > 0)
                {
                    lock (LogHelper.Lock_DuanMianALog)
                    {
                        string msg = LogHelper.Que_DuanMianALog.Dequeue();
                        //txtRunLog.Focus();
                        txtDuanMianALog.Text = txtDuanMianALog.Text + msg + "\r\n";
                        if (txtDuanMianALog.Text.Length > 10000)
                        {
                            txtDuanMianALog.Text = txtDuanMianALog.Text.Substring(txtDuanMianALog.Text.Length - 10000, 10000);
                        }

                        txtDuanMianALog.Select(txtDuanMianALog.Text.Length, 0);
                        txtDuanMianALog.ScrollToCaret();
                    }
                }

                #endregion 运行日志

                #region 运行日志

                if (LogHelper.Que_WaiYuanALog.Count > 0)
                {
                    lock (LogHelper.Lock_WaiYuanALog)
                    {
                        string msg = LogHelper.Que_WaiYuanALog.Dequeue();
                        //txtRunLog.Focus();
                        txtWaiYuanALog.Text = txtWaiYuanALog.Text + msg + "\r\n";
                        if (txtWaiYuanALog.Text.Length > 10000)
                        {
                            txtWaiYuanALog.Text = txtWaiYuanALog.Text.Substring(txtWaiYuanALog.Text.Length - 10000, 10000);
                        }

                        txtWaiYuanALog.Select(txtWaiYuanALog.Text.Length, 0);
                        txtWaiYuanALog.ScrollToCaret();
                    }
                }

                #endregion 运行日志

                #region 运行日志

                if (LogHelper.Que_DaoJiaoALog.Count > 0)
                {
                    lock (LogHelper.Lock_DaoJiaoALog)
                    {
                        string msg = LogHelper.Que_DaoJiaoALog.Dequeue();
                        //txtRunLog.Focus();
                        txtDaoJiaoALog.Text = txtDaoJiaoALog.Text + msg + "\r\n";
                        if (txtDaoJiaoALog.Text.Length > 10000)
                        {
                            txtDaoJiaoALog.Text = txtDaoJiaoALog.Text.Substring(txtDaoJiaoALog.Text.Length - 10000, 10000);
                        }

                        txtDaoJiaoALog.Select(txtDaoJiaoALog.Text.Length, 0);
                        txtDaoJiaoALog.ScrollToCaret();
                    }
                }

                #endregion 运行日志

                #region 运行日志

                if (LogHelper.Que_NeiKongLog.Count > 0)
                {
                    lock (LogHelper.Lock_NeiKongLog)
                    {
                        string msg = LogHelper.Que_NeiKongLog.Dequeue();
                        //txtRunLog.Focus();
                        txtNeiKongLog.Text = txtNeiKongLog.Text + msg + "\r\n";
                        if (txtNeiKongLog.Text.Length > 10000)
                        {
                            txtNeiKongLog.Text = txtNeiKongLog.Text.Substring(txtNeiKongLog.Text.Length - 10000, 10000);
                        }

                        txtNeiKongLog.Select(txtNeiKongLog.Text.Length, 0);
                        txtNeiKongLog.ScrollToCaret();
                    }
                }

                #endregion 运行日志

                #region 运行日志

                if (LogHelper.Que_DuanMianBLog.Count > 0)
                {
                    lock (LogHelper.Lock_DuanMianBLog)
                    {
                        string msg = LogHelper.Que_DuanMianBLog.Dequeue();
                        //txtPLCLog.Focus();
                        txtDuanMianBLog.Text = txtDuanMianBLog.Text + msg + "\r\n";
                        if (txtDuanMianBLog.Text.Length > 10000)
                        {
                            txtDuanMianBLog.Text = txtDuanMianBLog.Text.Substring(txtDuanMianBLog.Text.Length - 10000, 10000);
                        }

                        txtDuanMianBLog.Select(txtDuanMianBLog.Text.Length, 0);
                        txtDuanMianBLog.ScrollToCaret();
                    }
                }

                #endregion 运行日志

                #region 运行日志

                if (LogHelper.Que_WaiYuanBLog.Count > 0)
                {
                    lock (LogHelper.Lock_WaiYuanBLog)
                    {
                        string msg = LogHelper.Que_WaiYuanBLog.Dequeue();
                        //txtCCDLog.Focus();
                        txtWaiYuanBLog.Text = txtWaiYuanBLog.Text + msg + "\r\n";
                        if (txtWaiYuanBLog.Text.Length > 10000)
                        {
                            txtWaiYuanBLog.Text = txtWaiYuanBLog.Text.Substring(txtWaiYuanBLog.Text.Length - 10000, 10000);
                        }

                        txtWaiYuanBLog.Select(txtWaiYuanBLog.Text.Length, 0);
                        txtWaiYuanBLog.ScrollToCaret();
                    }
                }

                #endregion 运行日志

                #region 运行日志

                if (LogHelper.Que_DaoJiaoBLog.Count > 0)
                {
                    lock (LogHelper.Lock_DaoJiaoBLog)
                    {
                        string msg = LogHelper.Que_DaoJiaoBLog.Dequeue();
                        //txtSFCLog.Focus();
                        txtDaoJiaoBLog.Text = txtDaoJiaoBLog.Text + msg + "\r\n";
                        if (txtDaoJiaoBLog.Text.Length > 10000)
                        {
                            txtDaoJiaoBLog.Text = txtDaoJiaoBLog.Text.Substring(txtDaoJiaoBLog.Text.Length - 10000, 10000);
                        }

                        txtDaoJiaoBLog.Select(txtDaoJiaoBLog.Text.Length, 0);
                        txtDaoJiaoBLog.ScrollToCaret();
                    }
                }

                #endregion 运行日志

                #region 运行日志

                if (LogHelper.Que_CommLog.Count > 0)
                {
                    lock (LogHelper.Lock_CommLog)
                    {
                        string msg = LogHelper.Que_CommLog.Dequeue();
                        //txtNGLog.Focus();
                        txtCommLog.Text = txtCommLog.Text + msg + "\r\n";
                        if (txtCommLog.Text.Length > 10000)
                        {
                            txtCommLog.Text = txtCommLog.Text.Substring(txtCommLog.Text.Length - 10000, 10000);
                        }

                        txtCommLog.Select(txtCommLog.Text.Length, 0);
                        txtCommLog.ScrollToCaret();
                    }
                }

                #endregion 运行日志

                #endregion 在界面上显示各种Log记录

                //dgvCT.Rows[0].Cells[1].Value = GlobalVariable.ctCalculate[0].TakePicture;
                //dgvCT.Rows[1].Cells[1].Value = GlobalVariable.ctCalculate[0].Calculate;
                //dgvCT.Rows[2].Cells[1].Value = GlobalVariable.ctCalculate[0].WriteResult;
                //dgvCT.Rows[3].Cells[1].Value = GlobalVariable.ctCalculate[1].TakePicture;
                //dgvCT.Rows[4].Cells[1].Value = GlobalVariable.ctCalculate[1].Calculate;
                //dgvCT.Rows[5].Cells[1].Value = GlobalVariable.ctCalculate[1].WriteResult;
                //dgvCT.Rows[6].Cells[1].Value = GlobalVariable.ctCalculate[2].TakePicture;
                //dgvCT.Rows[7].Cells[1].Value = GlobalVariable.ctCalculate[2].Calculate;
                //dgvCT.Rows[8].Cells[1].Value = GlobalVariable.ctCalculate[2].WriteResult;
                //dgvCT.Rows[9].Cells[1].Value = GlobalVariable.ctCalculate[3].TakePicture;
                //dgvCT.Rows[10].Cells[1].Value = GlobalVariable.ctCalculate[3].Calculate;
                //dgvCT.Rows[11].Cells[1].Value = GlobalVariable.ctCalculate[3].WriteResult;

                timeStatus.Text = DateTime.Now.ToString("HH:mm");
            }
            catch (Exception ex)
            {
                LogHelper.AddCommLog("HomeUIForm中Timer产生异常，为:" + ex.Message);
            }
        }

        private void SetToolButtonEnableByAuthority()
        {
            if (LoginForm.UserAuthority == UserAutority.Operater)
            {
                btnOtherSave.Enabled = false;
                btnLoadSetting.Enabled = false;
                btnParamSetting.Enabled = false;
                btnModelSetting.Enabled = false;
                btnXiangJiSetting.Enabled = false;
                btnHardwareSetting.Enabled = false;
                btnGongNengSetting.Enabled = false;
                authorityStatus.Text = "操作员";
            }
            else if (LoginForm.UserAuthority == UserAutority.Admin)
            {
                btnOtherSave.Enabled = true;
                btnLoadSetting.Enabled = true;
                btnParamSetting.Enabled = true;
                btnModelSetting.Enabled = true;
                btnXiangJiSetting.Enabled = true;
                btnHardwareSetting.Enabled = true;
                btnGongNengSetting.Enabled = true;
                authorityStatus.Text = "管理员";
            }
            else if (LoginForm.UserAuthority == UserAutority.Vendor)
            {
                btnOtherSave.Enabled = true;
                btnLoadSetting.Enabled = true;
                btnParamSetting.Enabled = true;
                btnModelSetting.Enabled = true;
                btnXiangJiSetting.Enabled = true;
                btnHardwareSetting.Enabled = true;
                btnGongNengSetting.Enabled = true;
                authorityStatus.Text = "厂商";
            }
        }

        public void ShowData(TotalDataCollect totaldata, TotalBears count)
        {
            Invoke(new Action(() =>
            {
                dgvDataStatistic.Rows[0].Cells[1].Value = totaldata.DuanMianANGNumber;
                dgvDataStatistic.Rows[1].Cells[1].Value = totaldata.WaiYuanANGNumber;
                dgvDataStatistic.Rows[2].Cells[1].Value = totaldata.DaoJiaoANGNumber;
                dgvDataStatistic.Rows[3].Cells[1].Value = totaldata.NeiKongNGNumber;
                dgvDataStatistic.Rows[4].Cells[1].Value = totaldata.DuanMianBNGNumber;
                dgvDataStatistic.Rows[5].Cells[1].Value = totaldata.WaiYuanBNGNumber;
                dgvDataStatistic.Rows[6].Cells[1].Value = totaldata.DaoJiaoBNGNumber;
                dgvDataStatistic.Rows[7].Cells[1].Value = count.DuanMianACount;
                if (count.DuanMianACount == 0)
                {
                    dgvDataStatistic.Rows[0].Cells[2].Value = 0;
                }
                else
                {
                    dgvDataStatistic.Rows[0].Cells[2].Value = ((double)100 * totaldata.DuanMianANGNumber / count.DuanMianACount).ToString("f2") + "%";
                }
                if (count.WaiYuanACount == 0)
                {
                    dgvDataStatistic.Rows[1].Cells[2].Value = 0;
                }
                else
                {
                    dgvDataStatistic.Rows[1].Cells[2].Value = ((double)100 * totaldata.WaiYuanANGNumber / count.WaiYuanACount).ToString("f2") + "%";
                }
                if (count.DaoJiaoACount == 0)
                {
                    dgvDataStatistic.Rows[2].Cells[2].Value = 0;
                }
                else
                {
                    dgvDataStatistic.Rows[2].Cells[2].Value = ((double)100 * totaldata.DaoJiaoANGNumber / count.DaoJiaoACount).ToString("f2") + "%";
                }
                if (count.NeiKongCount == 0)
                {
                    dgvDataStatistic.Rows[3].Cells[2].Value = 0;
                }
                else
                {
                    dgvDataStatistic.Rows[3].Cells[2].Value = ((double)100 * totaldata.NeiKongNGNumber / count.NeiKongCount).ToString("f2") + "%";
                }
                if (count.DuanMianBCount == 0)
                {
                    dgvDataStatistic.Rows[4].Cells[2].Value = 0;
                }
                else
                {
                    dgvDataStatistic.Rows[4].Cells[2].Value = ((double)100 * totaldata.DuanMianBNGNumber / count.DuanMianBCount).ToString("f2") + "%";
                }
                if (count.WaiYuanBCount == 0)
                {
                    dgvDataStatistic.Rows[5].Cells[2].Value = 0;
                }
                else
                {
                    dgvDataStatistic.Rows[5].Cells[2].Value = ((double)100 * totaldata.WaiYuanBNGNumber / count.WaiYuanBCount).ToString("f2") + "%";
                }
                if (count.DaoJiaoBCount == 0)
                {
                    dgvDataStatistic.Rows[6].Cells[2].Value = 0;
                }
                else
                {
                    dgvDataStatistic.Rows[6].Cells[2].Value = ((double)100 * totaldata.DaoJiaoBNGNumber / count.DaoJiaoBCount).ToString("f2") + "%";
                }
            }));
        }

        public void ChangeConfigName(string name)
        {
            ConfigStatus.Text = name;
        }

        private void clearErrorCodeStatistic()
        {
            GlobalVariable.duanMianAErrCodeStatistic.Clear();
            GlobalVariable.waiYuanAErrCodeStatistic.Clear();
            GlobalVariable.daoJiaoAErrCodeStatistic.Clear();
            GlobalVariable.neiKongErrCodeStatistic.Clear();
            GlobalVariable.duanMianBErrCodeStatistic.Clear();
            GlobalVariable.waiYuanBErrCodeStatistic.Clear();
            GlobalVariable.daoJiaoBErrCodeStatistic.Clear();
            if (File.Exists(FilePath.duanMianAStatisticPath))
            {
                File.Delete(FilePath.duanMianAStatisticPath);
            }
            if (File.Exists(FilePath.waiYuanAStatisticPath))
            {
                File.Delete(FilePath.waiYuanAStatisticPath);
            }
            if (File.Exists(FilePath.daoJiaoAStatisticPath))
            {
                File.Delete(FilePath.daoJiaoAStatisticPath);
            }
            if (File.Exists(FilePath.neiKongStatisticPath))
            {
                File.Delete(FilePath.neiKongStatisticPath);
            }
            if (File.Exists(FilePath.duanMianBStatisticPath))
            {
                File.Delete(FilePath.duanMianBStatisticPath);
            }
            if (File.Exists(FilePath.waiYuanBStatisticPath))
            {
                File.Delete(FilePath.waiYuanBStatisticPath);
            }
            if (File.Exists(FilePath.daoJiaoBStatisticPath))
            {
                File.Delete(FilePath.daoJiaoBStatisticPath);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clearErrorCodeStatistic();

            GlobalVariable.totalDataCollect.DuanMianANGNumber = 0;
            GlobalVariable.totalDataCollect.WaiYuanANGNumber = 0;
            GlobalVariable.totalDataCollect.DaoJiaoANGNumber = 0;
            GlobalVariable.totalDataCollect.NeiKongNGNumber = 0;
            GlobalVariable.totalDataCollect.DuanMianBNGNumber = 0;
            GlobalVariable.totalDataCollect.WaiYuanBNGNumber = 0;
            GlobalVariable.totalDataCollect.DaoJiaoBNGNumber = 0;

            GlobalVariable.totalBears.DuanMianACount = 0;
            GlobalVariable.totalBears.WaiYuanACount = 0;
            GlobalVariable.totalBears.DaoJiaoACount = 0;
            GlobalVariable.totalBears.NeiKongCount = 0;
            GlobalVariable.totalBears.DuanMianBCount = 0;
            GlobalVariable.totalBears.WaiYuanBCount = 0;
            GlobalVariable.totalBears.DaoJiaoBCount = 0;

            ShowData(GlobalVariable.totalDataCollect, GlobalVariable.totalBears);
        }

        public void ShowLogPage()
        {
            if (GlobalVariable.generalSetting.bUseLog)
            {
                tabDuanMianA.Parent = tabControl2;
                tabWaiYuanA.Parent = tabControl2;
                tabDaoJiaoA.Parent = tabControl2;
                tabNeiKong.Parent = tabControl2;
                groupCt.Visible = true;
            }
            else
            {
                tabDuanMianA.Parent = null;
                tabWaiYuanA.Parent = null;
                tabDaoJiaoA.Parent = null;
                tabNeiKong.Parent = null;
                groupCt.Visible = false;
            }
        }

        private void btnLoadSetting_Click(object sender, EventArgs e)
        {
            //if(GlobalVariable.pcState == PCState.Run)
            //{
            //    MessageBox.Show("请先停止测试");
            //    return;
            //}
            string ErrorMsg = "";
            LoadConfigForm l = new LoadConfigForm();
            var ret = l.ShowDialog();
            if (ret == DialogResult.OK)
            {
                GlobalVariable.configname = l.configName;
                ConfigStatus.Text = GlobalVariable.configname;
                IniHelper.IniWriteString("Password", "CurrentConfig", GlobalVariable.configname, FilePath.UserPasswordPath);
                ChangeConfigName(GlobalVariable.configname);
                bool bRtn = JsonHelper.ReadJsonFile(FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json", ref GlobalVariable.configSetting, ref ErrorMsg);
                GlobalVariable.DuanMianAProcess.setTypeInfo("Param//" + GlobalVariable.configname);
                GlobalVariable.DuanMianAProcess.updateModel();
                GlobalVariable.DuanMianBProcess.setTypeInfo("Param//" + GlobalVariable.configname);
                GlobalVariable.DuanMianBProcess.updateModel();
                GlobalVariable.resetParam();
                if (!bRtn)
                {
                    GlobalMethod.ShowMessage("加载配置文件失败");
                    return;
                }
            }
            else if (ret == DialogResult.Yes)
            {
                bool bRtn = JsonHelper.ReadJsonFile(FilePath.ParamSettingPath + GlobalVariable.configname + "\\Param.Json", ref GlobalVariable.TmpConfigSetting, ref ErrorMsg);

                GlobalVariable.configSetting.waiYuanACameraPhotoSetting = GlobalVariable.TmpConfigSetting.waiYuanACameraPhotoSetting;
                GlobalVariable.configSetting.waiYuanAFlag = GlobalVariable.TmpConfigSetting.waiYuanAFlag;
                GlobalVariable.configSetting.waiYuanAParam = GlobalVariable.TmpConfigSetting.waiYuanAParam;
                GlobalVariable.configSetting.waiYuanAModel = GlobalVariable.TmpConfigSetting.waiYuanAModel;
                //GlobalVariable.configSetting.SideOutExtra = GlobalVariable.TmpConfigSetting.SideOutExtra;
                //GlobalVariable.WaiYuanAProcess.setROI(GlobalVariable.configSetting.SideOutExtra.LeftX, GlobalVariable.configSetting.SideOutExtra.TopY, GlobalVariable.configSetting.SideOutExtra.RightX, GlobalVariable.configSetting.SideOutExtra.DownY);


                GlobalVariable.configSetting.daoJiaoACameraPhotoSetting = GlobalVariable.TmpConfigSetting.daoJiaoACameraPhotoSetting;
                GlobalVariable.configSetting.daoJiaoAFlag = GlobalVariable.TmpConfigSetting.daoJiaoAFlag;
                GlobalVariable.configSetting.daoJiaoAParam = GlobalVariable.TmpConfigSetting.daoJiaoAParam;
                
                GlobalVariable.configSetting.waiYuanBCameraPhotoSetting = GlobalVariable.TmpConfigSetting.waiYuanBCameraPhotoSetting;
                GlobalVariable.configSetting.waiYuanBFlag = GlobalVariable.TmpConfigSetting.waiYuanBFlag;
                GlobalVariable.configSetting.waiYuanBParam = GlobalVariable.TmpConfigSetting.waiYuanBParam; 


                GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting = GlobalVariable.TmpConfigSetting.daoJiaoBCameraPhotoSetting;
                GlobalVariable.configSetting.daoJiaoBFlag = GlobalVariable.TmpConfigSetting.daoJiaoBFlag;
                GlobalVariable.configSetting.daoJiaoBParam = GlobalVariable.TmpConfigSetting.daoJiaoBParam;

                GlobalVariable.configSetting.neiKongCameraPhotoSetting = GlobalVariable.TmpConfigSetting.neiKongCameraPhotoSetting;
                GlobalVariable.configSetting.neiKongFlag = GlobalVariable.TmpConfigSetting.neiKongFlag;
                GlobalVariable.configSetting.neiKongParam = GlobalVariable.TmpConfigSetting.neiKongParam;
                GlobalVariable.resetParam();

                bRtn = JsonHelper.ReadJsonFile(FilePath.ParamSettingPath + "ParamBackup\\Param.Json", ref GlobalVariable.TmpConfigSetting, ref ErrorMsg);
                if (bRtn)
                {

                    GlobalVariable.configSetting.waiYuanACameraPhotoSetting = GlobalVariable.TmpConfigSetting.waiYuanACameraPhotoSetting;
                    GlobalVariable.configSetting.waiYuanAFlag = GlobalVariable.TmpConfigSetting.waiYuanAFlag;
                    GlobalVariable.configSetting.waiYuanAParam = GlobalVariable.TmpConfigSetting.waiYuanAParam;
                    GlobalVariable.configSetting.waiYuanAModel = GlobalVariable.TmpConfigSetting.waiYuanAModel;
                    //GlobalVariable.configSetting.SideOutExtra = GlobalVariable.TmpConfigSetting.SideOutExtra;
                   // GlobalVariable.WaiYuanAProcess.setROI(GlobalVariable.configSetting.SideOutExtra.LeftX, GlobalVariable.configSetting.SideOutExtra.TopY, GlobalVariable.configSetting.SideOutExtra.RightX, GlobalVariable.configSetting.SideOutExtra.DownY);

                    GlobalVariable.configSetting.daoJiaoACameraPhotoSetting = GlobalVariable.TmpConfigSetting.daoJiaoACameraPhotoSetting;
                    GlobalVariable.configSetting.daoJiaoAFlag = GlobalVariable.TmpConfigSetting.daoJiaoAFlag;
                    GlobalVariable.configSetting.daoJiaoAParam = GlobalVariable.TmpConfigSetting.daoJiaoAParam;

                    GlobalVariable.configSetting.waiYuanBCameraPhotoSetting = GlobalVariable.TmpConfigSetting.waiYuanBCameraPhotoSetting;
                    GlobalVariable.configSetting.waiYuanBFlag = GlobalVariable.TmpConfigSetting.waiYuanBFlag;
                    GlobalVariable.configSetting.waiYuanBParam = GlobalVariable.TmpConfigSetting.waiYuanBParam;


                    GlobalVariable.configSetting.daoJiaoBCameraPhotoSetting = GlobalVariable.TmpConfigSetting.daoJiaoBCameraPhotoSetting;
                    GlobalVariable.configSetting.daoJiaoBFlag = GlobalVariable.TmpConfigSetting.daoJiaoBFlag;
                    GlobalVariable.configSetting.daoJiaoBParam = GlobalVariable.TmpConfigSetting.daoJiaoBParam;

                    GlobalVariable.configSetting.neiKongCameraPhotoSetting = GlobalVariable.TmpConfigSetting.neiKongCameraPhotoSetting;
                    GlobalVariable.configSetting.neiKongFlag = GlobalVariable.TmpConfigSetting.neiKongFlag;
                    GlobalVariable.configSetting.neiKongParam = GlobalVariable.TmpConfigSetting.neiKongParam;
                    GlobalVariable.resetParam();
                }
            }
        }

        private void btnOtherSave_Click(object sender, EventArgs e)
        {
            //if (GlobalVariable.pcState == PCState.Run)
            //{
            //    MessageBox.Show("请先停止测试");
            //    return;
            //}

            OtherSaveForm o = new OtherSaveForm();
            if (o.ShowDialog() == DialogResult.OK)
            {
                bool bRtn = FileHelper.CopyDirectory(FilePath.ParamSettingPath + GlobalVariable.configname, FilePath.ParamSettingPath + o.name);
                if (bRtn)
                {
                    GlobalVariable.configname = o.name;
                    ConfigStatus.Text = GlobalVariable.configname;
                    GlobalMethod.ShowMessage("另存成功");
                }
                else
                {
                    GlobalMethod.ShowMessage("另存失败");
                }
            }
        }

        private void btnGongNengSetting_Click(object sender, EventArgs e)
        {
            //if (GlobalVariable.pcState == PCState.Run)
            //{
            //    MessageBox.Show("请先停止测试");
            //    return;
            //}

            GongNengSettingForm form = new GongNengSettingForm();
            form.ShowDialog();
        }

        private void btnParamSetting_Click(object sender, EventArgs e)
        {
            //if (GlobalVariable.pcState == PCState.Run)
            //{
            //    MessageBox.Show("请先停止测试");
            //    return;
            //}

            ParamSettingForm form = new YiRongMachine.ParamSettingForm();
            form.ShowDialog();
        }

        private void btnModelSetting_Click(object sender, EventArgs e)
        {
            //if (GlobalVariable.pcState == PCState.Run)
            //{
            //    MessageBox.Show("请先停止测试");
            //    return;
            //}

            modelSettingForm.ShowDialog();
        }

        private void btnXiangJiSetting_Click(object sender, EventArgs e)
        {
            //if (GlobalVariable.pcState == PCState.Run)
            //{
            //    MessageBox.Show("请先停止测试");
            //    return;
            //}

            CameraDebugForm form = new CameraDebugForm();
            form.ShowDialog();
        }

        private void btnHardwareSetting_Click(object sender, EventArgs e)
        {
            //if (GlobalVariable.pcState == PCState.Run)
            //{
            //    MessageBox.Show("请先停止测试");
            //    return;
            //}

            HardwareSettingForm form = new HardwareSettingForm();
            form.ShowDialog();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.pcState == PCState.Run)
            {
                MessageBox.Show("请先停止测试");
                return;
            }

            LoginForm l = new LoginForm();
            l.ShowDialog();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            //if (GlobalVariable.pcState == PCState.Error)
            //{
            //    return;
            //}
            if(GlobalVariable.iWorkStation == 2)
            {
                GlobalVariable.duanMianAAutoFlow.init();
                GlobalVariable.duanMianBAutoFlow.init();
            }
            else if(GlobalVariable.iWorkStation == 5)
            {
                GlobalVariable.duanMianAAutoFlow.init();
                GlobalVariable.duanMianBAutoFlow.init();
                GlobalVariable.waiYuanAAutoFlow.init();
                GlobalVariable.neiKongAutoFlow.init();
                GlobalVariable.waiYuanBAutoFlow.init();
            }
            else
            {
                GlobalVariable.duanMianAAutoFlow.init();
                GlobalVariable.duanMianBAutoFlow.init();
                GlobalVariable.waiYuanAAutoFlow.init();
                GlobalVariable.daoJiaoAAutoFlow.init();
                GlobalVariable.neiKongAutoFlow.init();
                GlobalVariable.waiYuanBAutoFlow.init();
                GlobalVariable.daoJiaoBAutoFlow.init();
            }

            GlobalVariable.resetParam();

            GlobalVariable.lightHelper[0].setL();
            GlobalVariable.lightHelper[1].setL();

            GlobalVariable.pcState = PCState.Run;
            lblStatus.Text = "正在运行中";
            lblStatus.BackColor = Color.Lime;
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (GlobalVariable.pcState == PCState.Error)
            {
                return;
            }
            GlobalVariable.pcState = PCState.Pause;
            lblStatus.Text = "暂停中";
            lblStatus.BackColor = Color.Yellow;

            GlobalVariable.duanMianAAutoFlow.init();
            GlobalVariable.duanMianBAutoFlow.init();

            if (GlobalVariable.iWorkStation != 2)
            {
                GlobalVariable.waiYuanAAutoFlow.init();
                GlobalVariable.daoJiaoAAutoFlow.init();
                GlobalVariable.neiKongAutoFlow.init();
                GlobalVariable.waiYuanBAutoFlow.init();
                GlobalVariable.daoJiaoBAutoFlow.init();
            }

            GlobalVariable.resetParam();

            GlobalVariable.lightHelper[0].setL();
            GlobalVariable.lightHelper[1].setL();
        }

        private void btnErrCode_Click(object sender, EventArgs e)
        {
            FrmErrCode frm = new FrmErrCode();
            frm.ShowDialog();
        }

        private void btnResultInfo_Click(object sender, EventArgs e)
        {
            FrmResultInfo frm = new FrmResultInfo();
            frm.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form = new Form1();
            form.ShowDialog();
        }

        private void CreateAllThread()
        {
            if(GlobalVariable.iWorkStation == 5)
            {
                th_DuanMianA_Ctrl = new Thread(GlobalVariable.duanMianAAutoFlow.CreateControlThread);
                th_DuanMianA_Ctrl.IsBackground = true;
                th_DuanMianA_Ctrl.Start();
                th_DuanMianA_Calc = new Thread(GlobalVariable.duanMianAAutoFlow.CreateCalculateThread);
                th_DuanMianA_Calc.IsBackground = true;
                th_DuanMianA_Calc.Start();
                th_DuanMianA_Show = new Thread(GlobalVariable.duanMianAAutoFlow.CreateShowThread);
                th_DuanMianA_Show.IsBackground = true;
                th_DuanMianA_Show.Start();

                th_WaiYuanA_Ctrl = new Thread(GlobalVariable.waiYuanAAutoFlow.CreateControlThread);
                th_WaiYuanA_Ctrl.IsBackground = true;
                th_WaiYuanA_Ctrl.Start();
                th_WaiYuanA_Calc = new Thread(GlobalVariable.waiYuanAAutoFlow.CreateCalculateThread);
                th_WaiYuanA_Calc.IsBackground = true;
                th_WaiYuanA_Calc.Start();
                th_WaiYuanA_Show = new Thread(GlobalVariable.waiYuanAAutoFlow.CreateShowThread);
                th_WaiYuanA_Show.IsBackground = true;
                th_WaiYuanA_Show.Start();

                th_NeiKong_Ctrl = new Thread(GlobalVariable.neiKongAutoFlow.CreateControlThread);
                th_NeiKong_Ctrl.IsBackground = true;
                th_NeiKong_Ctrl.Start();
                th_NeiKong_Calc = new Thread(GlobalVariable.neiKongAutoFlow.CreateCalculateThread);
                th_NeiKong_Calc.IsBackground = true;
                th_NeiKong_Calc.Start();
                th_NeiKong_Show = new Thread(GlobalVariable.neiKongAutoFlow.CreateShowThread);
                th_NeiKong_Show.IsBackground = true;
                th_NeiKong_Show.Start();

                th_DuanMianB_Ctrl = new Thread(GlobalVariable.duanMianBAutoFlow.CreateControlThread);
                th_DuanMianB_Ctrl.IsBackground = true;
                th_DuanMianB_Ctrl.Start();
                th_DuanMianB_Calc = new Thread(GlobalVariable.duanMianBAutoFlow.CreateCalculateThread);
                th_DuanMianB_Calc.IsBackground = true;
                th_DuanMianB_Calc.Start();
                th_DuanMianB_Show = new Thread(GlobalVariable.duanMianBAutoFlow.CreateShowThread);
                th_DuanMianB_Show.IsBackground = true;
                th_DuanMianB_Show.Start();

                th_WaiYuanB_Ctrl = new Thread(GlobalVariable.waiYuanBAutoFlow.CreateControlThread);
                th_WaiYuanB_Ctrl.IsBackground = true;
                th_WaiYuanB_Ctrl.Start();
                th_WaiYuanB_Calc = new Thread(GlobalVariable.waiYuanBAutoFlow.CreateCalculateThread);
                th_WaiYuanB_Calc.IsBackground = true;
                th_WaiYuanB_Calc.Start();
                th_WaiYuanB_Show = new Thread(GlobalVariable.waiYuanBAutoFlow.CreateShowThread);
                th_WaiYuanB_Show.IsBackground = true;
                th_WaiYuanB_Show.Start();
            }
            else
            {
                th_DuanMianA_Ctrl = new Thread(GlobalVariable.duanMianAAutoFlow.CreateControlThread);
                th_DuanMianA_Ctrl.IsBackground = true;
                th_DuanMianA_Ctrl.Start();
                th_DuanMianA_Calc = new Thread(GlobalVariable.duanMianAAutoFlow.CreateCalculateThread);
                th_DuanMianA_Calc.IsBackground = true;
                th_DuanMianA_Calc.Start();
                th_DuanMianA_Show = new Thread(GlobalVariable.duanMianAAutoFlow.CreateShowThread);
                th_DuanMianA_Show.IsBackground = true;
                th_DuanMianA_Show.Start();

                th_WaiYuanA_Ctrl = new Thread(GlobalVariable.waiYuanAAutoFlow.CreateControlThread);
                th_WaiYuanA_Ctrl.IsBackground = true;
                th_WaiYuanA_Ctrl.Start();
                th_WaiYuanA_Calc = new Thread(GlobalVariable.waiYuanAAutoFlow.CreateCalculateThread);
                th_WaiYuanA_Calc.IsBackground = true;
                th_WaiYuanA_Calc.Start();
                th_WaiYuanA_Show = new Thread(GlobalVariable.waiYuanAAutoFlow.CreateShowThread);
                th_WaiYuanA_Show.IsBackground = true;
                th_WaiYuanA_Show.Start();

                th_DaoJiaoA_Ctrl = new Thread(GlobalVariable.daoJiaoAAutoFlow.CreateControlThread);
                th_DaoJiaoA_Ctrl.IsBackground = true;
                th_DaoJiaoA_Ctrl.Start();
                th_DaoJiaoA_Calc = new Thread(GlobalVariable.daoJiaoAAutoFlow.CreateCalculateThread);
                th_DaoJiaoA_Calc.IsBackground = true;
                th_DaoJiaoA_Calc.Start();
                th_DaoJiaoA_Show = new Thread(GlobalVariable.daoJiaoAAutoFlow.CreateShowThread);
                th_DaoJiaoA_Show.IsBackground = true;
                th_DaoJiaoA_Show.Start();

                th_NeiKong_Ctrl = new Thread(GlobalVariable.neiKongAutoFlow.CreateControlThread);
                th_NeiKong_Ctrl.IsBackground = true;
                th_NeiKong_Ctrl.Start();
                th_NeiKong_Calc = new Thread(GlobalVariable.neiKongAutoFlow.CreateCalculateThread);
                th_NeiKong_Calc.IsBackground = true;
                th_NeiKong_Calc.Start();
                th_NeiKong_Show = new Thread(GlobalVariable.neiKongAutoFlow.CreateShowThread);
                th_NeiKong_Show.IsBackground = true;
                th_NeiKong_Show.Start();

                th_DuanMianB_Ctrl = new Thread(GlobalVariable.duanMianBAutoFlow.CreateControlThread);
                th_DuanMianB_Ctrl.IsBackground = true;
                th_DuanMianB_Ctrl.Start();
                th_DuanMianB_Calc = new Thread(GlobalVariable.duanMianBAutoFlow.CreateCalculateThread);
                th_DuanMianB_Calc.IsBackground = true;
                th_DuanMianB_Calc.Start();
                th_DuanMianB_Show = new Thread(GlobalVariable.duanMianBAutoFlow.CreateShowThread);
                th_DuanMianB_Show.IsBackground = true;
                th_DuanMianB_Show.Start();

                th_WaiYuanB_Ctrl = new Thread(GlobalVariable.waiYuanBAutoFlow.CreateControlThread);
                th_WaiYuanB_Ctrl.IsBackground = true;
                th_WaiYuanB_Ctrl.Start();
                th_WaiYuanB_Calc = new Thread(GlobalVariable.waiYuanBAutoFlow.CreateCalculateThread);
                th_WaiYuanB_Calc.IsBackground = true;
                th_WaiYuanB_Calc.Start();
                th_WaiYuanB_Show = new Thread(GlobalVariable.waiYuanBAutoFlow.CreateShowThread);
                th_WaiYuanB_Show.IsBackground = true;
                th_WaiYuanB_Show.Start();

                th_DaoJiaoB_Ctrl = new Thread(GlobalVariable.daoJiaoBAutoFlow.CreateControlThread);
                th_DaoJiaoB_Ctrl.IsBackground = true;
                th_DaoJiaoB_Ctrl.Start();
                th_DaoJiaoB_Calc = new Thread(GlobalVariable.daoJiaoBAutoFlow.CreateCalculateThread);
                th_DaoJiaoB_Calc.IsBackground = true;
                th_DaoJiaoB_Calc.Start();
                th_DaoJiaoB_Show = new Thread(GlobalVariable.daoJiaoBAutoFlow.CreateShowThread);
                th_DaoJiaoB_Show.IsBackground = true;
                th_DaoJiaoB_Show.Start();
            }
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //bool bret = GlobalVariable.ioBoardHelper.readInput((int)IOBoardHelper.enInputType.NeiQuanB_CS_QD);
            //if (bret || GlobalVariable.bNeiQuanBRunManual)
            //{
            //    GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiQuanB_TXG, false);
            //    GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiQuanB_PZ_WC, false);
            //    GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiQuanB_OK, false);
            //    GlobalVariable.ioBoardHelper.setOutput((int)IOBoardHelper.enOutputType.NeiQuanB_NG, false);
            //}
            //Application.Restart();
        }

        private void PLCStatus_Click(object sender, EventArgs e)
        {
      
        }
    }
}