using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class GongNengSettingForm : Form
    {
        private GeneralSettingForm generalSettingForm = new GeneralSettingForm();
        private VendorSettingForm vendorSettingForm = new VendorSettingForm();

        public GongNengSettingForm()
        {
            InitializeComponent();

            UIHelper.AddTabPage(tabPage1, generalSettingForm);
            UIHelper.AddTabPage(tabPage3, vendorSettingForm);

            SetToolButtonEnableByAuthority();
        }

        private void SetToolButtonEnableByAuthority()
        {
            if (LoginForm.UserAuthority == UserAutority.Operater)
            {
                tabPage1.Parent = null;
                tabPage2.Parent = null;
                tabPage3.Parent = null;
            }
            else if (LoginForm.UserAuthority == UserAutority.Admin)
            {
                tabPage1.Parent = null;
                tabPage2.Parent = tabControl1;
                tabPage3.Parent = null;
            }
            else if (LoginForm.UserAuthority == UserAutority.Vendor)
            {
                tabPage1.Parent = tabControl1;
                tabPage2.Parent = tabControl1;
                tabPage3.Parent = tabControl1;
            }
        }

        private void GongNengSettingForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            generalSettingForm.stopTimer();
        }
    }
}