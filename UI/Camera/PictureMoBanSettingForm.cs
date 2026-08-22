using System;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class ModelSettingForm : Form
    {
        private SideABZhuJieMian duanMianAModelForm = new SideABZhuJieMian(0);
        private SideOutZhuJieMian waiYuanAModelForm = new SideOutZhuJieMian();
        private MultiZhuJieMian daoJiaoAModelForm = new MultiZhuJieMian(0);
        private MultiZhuJieMian neiKongModelForm = new MultiZhuJieMian(1);
        private SideABZhuJieMian duanMianBModelForm = new SideABZhuJieMian(1);
        private MultiZhuJieMian waiYuanBModelForm = new MultiZhuJieMian(2);
        private MultiZhuJieMian daoJiaoBModelForm = new MultiZhuJieMian(3);

        public ModelSettingForm()
        {
            InitializeComponent();

            UIHelper.AddTabPage(tabDuanMianAModel, duanMianAModelForm);
            UIHelper.AddTabPage(tabWaiYuanAModel, waiYuanAModelForm);
            UIHelper.AddTabPage(tabDaoJiaoAModel, daoJiaoAModelForm);
            UIHelper.AddTabPage(tabNeiKongModel, neiKongModelForm);
            UIHelper.AddTabPage(tabDuanMianBModel, duanMianBModelForm);
            UIHelper.AddTabPage(tabWaiYuanBModel, waiYuanBModelForm);
            UIHelper.AddTabPage(tabDaoJiaoBModel, daoJiaoBModelForm);
            if (GlobalVariable.iWorkStation == 2)
            {
                tabControl1.TabPages.Remove(tabControl1.TabPages[6]);
                tabControl1.TabPages.Remove(tabControl1.TabPages[5]);
                tabControl1.TabPages.Remove(tabControl1.TabPages[3]);
                tabControl1.TabPages.Remove(tabControl1.TabPages[2]);
                tabControl1.TabPages.Remove(tabControl1.TabPages[1]);
            }
            if (GlobalVariable.iWorkStation == 5)
            {
                tabControl1.TabPages.Remove(tabControl1.TabPages[6]);
                tabControl1.TabPages.Remove(tabControl1.TabPages[2]);
            }
        }

        private void PictureMoBanSettingForm_Load(object sender, EventArgs e)
        {
            //if (GlobalVariable.machineSetting.MachineType == (int)MachineType.FourCamera)
            //{
            //    tabDaoJiaoAModel.Parent = tabControl1;
            //}
            //else
            //{
            //    tabDaoJiaoAModel.Parent = null;
            //}
        }
    }
}