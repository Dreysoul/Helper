using System;
using System.Drawing;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class ShowPictureControl : UserControl
    {
        /// <summary>
        /// 0=SideA,1=SideB,2=SideIn,3=SideOut
        /// </summary>
        private int index;

        /// <summary>
        /// 缩放倍数
        /// </summary>
        private double ScaleNumberX = 0;

        private double ScaleNumberY = 0;

        private int plcErrorCode = 0;
        private SolidBrush brush = new SolidBrush(Color.Red);
        private Font wordFont = new Font("宋体", 15);
        private Mutex mutex = new Mutex();
        private int m_iCameraNumber = 0;
        private pictureCtrl pic_box = new pictureCtrl();

        private enum PanType
        {
            PAN_SRT,
            PAN_MOV,
            PAN_END
        };

        private Image m_Img = null;
        private bool m_bMidBtnDown = false;
        private PointF m_ptOri = new PointF(0, 0);
        private PointF m_factor = new PointF(1, 1);
        private float m_dScl = 1.05F;

        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem toolStripMenuItem1;
        private ToolStripMenuItem toolStripMenuItem2;
        private ToolStripMenuItem toolStripMenuItem3;

        public ShowPictureControl(int CameraNumber)
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            //string path = System.IO.Directory.GetCurrentDirectory() + "\\" + (11).ToString() +".bmp";
            //Image img = Image.FromFile(path);
            pic_box.Dock = DockStyle.Fill;
            pic_box.showImg(null);
            picb.Controls.Add(pic_box);

            m_iCameraNumber = CameraNumber;
            // 初始化ContextMenuStrip和菜单项
            contextMenuStrip1 = new ContextMenuStrip();
            toolStripMenuItem1 = new ToolStripMenuItem("适应窗口");
            toolStripMenuItem1.Click += delegate (object sender, EventArgs e)
            {
                pic_box.zoomAll();
            };
            toolStripMenuItem2 = new ToolStripMenuItem("手动运行");
            toolStripMenuItem2.Click += delegate (object sender, EventArgs e)
            {
                switch (m_iCameraNumber)
                {
                    case 0:
                        GlobalVariable.bDuanMianARunManual = true;
                        GlobalVariable.bDuanMianAParamUpdate = true;
                        break;

                    case 1:
                        GlobalVariable.bWaiYuanARunManual = true;
                        GlobalVariable.bWaiYuanAParamUpdate = true;
                        break;

                    case 2:
                        GlobalVariable.bDaoJiaoARunManual = true;
                        GlobalVariable.bDaoJiaoAParamUpdate = true;
                        break;

                    case 3:
                        GlobalVariable.bNeiKongRunManual = true;
                        GlobalVariable.bNeiKongParamUpdate = true;
                        break;

                    case 4:
                        GlobalVariable.bDuanMianBRunManual = true;
                        GlobalVariable.bDuanMianBParamUpdate = true;
                        break;

                    case 5:
                        GlobalVariable.bWaiYuanBRunManual = true;
                        GlobalVariable.bWaiYuanBParamUpdate = true;
                        break;

                    case 6:
                        GlobalVariable.bDaoJiaoBRunManual = true;
                        GlobalVariable.bDaoJiaoBParamUpdate = true;
                        break;
                    case 7:
                        GlobalVariable.bNeiKongBRunManual = true;
                        GlobalVariable.bNeiKongBParamUpdate = true;
                        break;
                    default:
                        break;
                }
            };
            toolStripMenuItem3 = new ToolStripMenuItem("保存照片");
            toolStripMenuItem3.Click += delegate (object sender, EventArgs e)
            {
                if (GlobalVariable.pcState == PCState.Run)
                {
                    MessageBox.Show("请先停止运行");
                    return;
                }
                Bitmap[] allPicture = null;
                switch (m_iCameraNumber)
                {
                    case 0:
                        allPicture = GlobalVariable.duanMianAAutoFlow.bitmapArray;
                        break;

                    case 1:
                        allPicture = GlobalVariable.waiYuanAAutoFlow.bitmapArray;
                        break;

                    case 2:
                        allPicture = GlobalVariable.daoJiaoAAutoFlow.bitmapArray;
                        break;

                    case 3:
                        allPicture = GlobalVariable.neiKongAutoFlow.bitmapArray;
                        break;

                    case 4:
                        allPicture = GlobalVariable.duanMianBAutoFlow.bitmapArray;
                        break;

                    case 5:
                        allPicture = GlobalVariable.waiYuanBAutoFlow.bitmapArray;
                        break;

                    case 6:
                        allPicture = GlobalVariable.daoJiaoBAutoFlow.bitmapArray;
                        break;

                    case 7:
                        allPicture = GlobalVariable.neiKongBAutoFlow.bitmapArray;
                        break;
                    default:
                        break;
                }
                if (allPicture == null)
                {
                    MessageBox.Show("没有需要保存的照片");
                    return;
                }
                string ErrorMsg = "";
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                //打开的文件选择对话框上的标题
                saveFileDialog.Title = "请选择文件";
                //设置文件类型
                saveFileDialog.Filter = "所有文件(*.*)|*.*";
                //按下确定选择的按钮
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //获得文件路径
                    string localFilePath = saveFileDialog.FileName.ToString();
                    bool bRtn = GlobalMethod.SavePicture(allPicture, localFilePath, ref ErrorMsg);
                    if (bRtn)
                    {
                        GlobalMethod.ShowMessage("导出图片成功");
                    }
                    else
                    {
                        GlobalMethod.ShowMessage("导出图片失败，错误信息为" + ErrorMsg);
                    }
                }
            };
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { toolStripMenuItem1, toolStripMenuItem2, toolStripMenuItem3 });
            this.ContextMenuStrip = contextMenuStrip1;

            string[] title = { "端面A", "外圆A", "倒角A", "内孔", "端面B", "外圆B", "倒角B","内孔B" };
            lblTitle.Text = title[CameraNumber];
            //string path = System.IO.Directory.GetCurrentDirectory() + "\\" + (CameraNumber + 11).ToString() +".bmp";
            //m_Img = Image.FromFile(path);

            System.Timers.Timer tSrt = new System.Timers.Timer(1000);
            tSrt.AutoReset = false;
            tSrt.Interval = 3500;
            tSrt.Elapsed += delegate (object sender, ElapsedEventArgs e)
            {
                pic_box.zoomAll();
            };
            tSrt.Start();
        }

        public void showResult(Image img, string result)
        {
            mutex.WaitOne();
            pic_box.showImg(img);
            pic_box.zoomAll();
            mutex.ReleaseMutex();
            ShowResult(result);
        }

        public void ShowResult(string result)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    lblResult.Text = result;
                    if (result == "OK")
                        lblResult.BackColor = Color.Lime;
                    else
                        lblResult.BackColor = Color.Red;
                }));
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 改变触发标签的颜色
        /// </summary>
        /// <param name="color"></param>
        public void ChangeSignStartColor(Color color)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    lblSignStart.BackColor = color;
                }));
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 改变图像标签的颜色
        /// </summary>
        /// <param name="color"></param>
        public void ChangeSignPictureColor(Color color)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    lblSignPicture.BackColor = color;
                }));
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 改变相机标签的颜色
        /// </summary>
        /// <param name="color"></param>
        public void ChangeSignCameraColor(Color color)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    lblSignCamera.BackColor = color;
                }));
            }
            catch (Exception)
            {
                return;
            }
        }

        /// <summary>
        /// 显示错误信息
        /// </summary>
        /// <param name="strErrorMsg"></param>
        public void ShowErrorMsg(string strErrorMsg)
        {
            try
            {
                Invoke(new Action(() =>
                {
                    txtShowError.Text = strErrorMsg;
                }));
            }
            catch (Exception)
            {
                return;
            }
        }

        private void ckbSaveNG_CheckedChanged(object sender, EventArgs e)
        {
            GlobalVariable.generalSetting.bSaveNG[m_iCameraNumber] = ckbSaveNG.Checked;
            string ErrorMsg = "";
            JsonHelper.WriteJsonFile(GlobalVariable.generalSetting, FilePath.GeneralSettingPath, ref ErrorMsg);
        }

        private void ckbForbidden_CheckedChanged(object sender, EventArgs e)
        {
            if (ckbForbidden.Checked)
            {
                GlobalVariable.bForbidden[m_iCameraNumber] = true;
            }
            else
            {
                GlobalVariable.bForbidden[m_iCameraNumber] = false;
            }
        }
    }
}