using System;
using System.Windows.Forms;

namespace YiRongMachine
{
    public enum UserAutority
    {
        Operater,
        Admin,
        Vendor,
    }

    public partial class LoginForm : Form
    {
        public static Action UserManageAction;

        //程序加载时的初始应用权限
        private static UserAutority _userAuthority = UserAutority.Operater;

        public static UserAutority UserAuthority            //建立一个property，为了expose theValue
        {
            get
            {
                return _userAuthority;
            }
            set
            {
                _userAuthority = value;
                if (UserManageAction != null)
                {
                    UserManageAction();
                }
            }
        }

        public LoginForm()
        {
            InitializeComponent();
            SetStyle(
                ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.ResizeRedraw
                | ControlStyles.Selectable
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.SupportsTransparentBackColor,
                true);
            this.UpdateStyles();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            combUser.Items.Add("操作员");
            combUser.Items.Add("管理员");
            combUser.Items.Add("厂商");
            combUser.SelectedIndex = 0;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string strUserName = "";
            if (combUser.SelectedIndex == 0)
            {
                strUserName = "Operator";
            }
            else if (combUser.SelectedIndex == 1)
            {
                strUserName = "Admin";
            }
            else
            {
                strUserName = "Vendor";
            }
            string strPassword = txbPassword.Text;
            string strIniPassword = IniHelper.IniReadString("Password", strUserName, "", FilePath.UserPasswordPath);
            if (strUserName == "Vendor")
            {
                string strMinute = DateTime.Now.ToString("mm");
                strIniPassword = strIniPassword + strMinute;
            }

            if (strPassword != strIniPassword)
            {
                txbPassword.Text = "";
                GlobalMethod.ShowMessage("密码不正确，请重新输入");
                return;
            }
            if (combUser.SelectedIndex == 0)
            {
                UserAuthority = UserAutority.Operater;
            }
            else if (combUser.SelectedIndex == 1)
            {
                UserAuthority = UserAutority.Admin;
            }
            else
            {
                UserAuthority = UserAutority.Vendor;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.btnOk_Click(sender, e);
            }
        }

        private void lblSoftName_MouseDown(object sender, MouseEventArgs e)
        {
            UIHelper.ReleaseCapture();
            UIHelper.PostMessage(this.Handle, UIHelper.WM_SYSCOMMAND, UIHelper.SC_MOVE + UIHelper.HTCAPTION, 0);
        }

        private void combUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            txbPassword.Text = "";
            txbPassword.Focus();
        }

        /// <summary>
        /// 获得当前权限的名称：操作员, 工程师, 管理员
        /// </summary>
        /// <returns></returns>
        public static string GetUserName()
        {
            if (UserAuthority == UserAutority.Operater)
            {
                return "操作员";
            }
            else if (UserAuthority == UserAutority.Admin)
            {
                return "管理员";
            }
            else
            {
                return "厂商";
            }
        }
    }
}