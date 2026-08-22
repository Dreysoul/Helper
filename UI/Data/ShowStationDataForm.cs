using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class ShowStationDataForm : Form
    {
        int index = 0;
        public ShowStationDataForm(int StationNumber)
        {
            InitializeComponent();
            index = StationNumber;
        }

        private void ShowStationDataForm_Load(object sender, EventArgs e)
        {
            LoadInfo();
        }

        private void LoadInfo()
        {
            int ng = 0;
            if (index == 0)
            {
                ng = GlobalVariable.totalDataCollect.SideANGNumber;
            }
            else if (index == 1)
            {
                ng = GlobalVariable.totalDataCollect.SideBNGNumber;
            }
            else if (index == 2)
            {
                ng = GlobalVariable.totalDataCollect.SideInNGNumber;
            }
            else if (index == 3)
            {
                ng = GlobalVariable.totalDataCollect.SideOutNGNumber;
            }

            lblTitle.Text = string .Format ("当前是工位{0}的数据统计，该工位一共NG了{1}个产品",index ,ng );
            UIHelper.SetDGVFormat_JustShow(dgvDedectType,30);

            for (int i = 0; i < 21; i++)
            {
                dgvDedectType.Rows.Add();
            }
            dgvDedectType.Rows[0].Cells[0].Value = "半径过大";
            dgvDedectType.Rows[1].Cells[0].Value = "半径过小";
            dgvDedectType.Rows[2].Cells[0].Value = "无法找到半径";
            dgvDedectType.Rows[3].Cells[0].Value = "轴承盖文字错误";
            dgvDedectType.Rows[4].Cells[0].Value = "划痕检测";
            dgvDedectType.Rows[5].Cells[0].Value = "锈斑检测";
            dgvDedectType.Rows[6].Cells[0].Value = "坑点检测";
            dgvDedectType.Rows[7].Cells[0].Value = "磕碰检测";
            dgvDedectType.Rows[8].Cells[0].Value = "盒子压位检测";
            dgvDedectType.Rows[9].Cells[0].Value = "亮线检测";
            dgvDedectType.Rows[10].Cells[0].Value = "金属盖太暗";
            dgvDedectType.Rows[11].Cells[0].Value = "金属盖按压错误";

            dgvDedectType.Rows[12].Cells[0].Value = "外圈区域";
            dgvDedectType.Rows[13].Cells[0].Value = "端盖区域";
            dgvDedectType.Rows[14].Cells[0].Value = "内圈区域";

            dgvDedectType.Rows[15].Cells[0].Value = "高光源";
            dgvDedectType.Rows[16].Cells[0].Value = "低侧光";
            dgvDedectType.Rows[17].Cells[0].Value = "左上光";
            dgvDedectType.Rows[18].Cells[0].Value = "右上光";
            dgvDedectType.Rows[19].Cells[0].Value = "左下光";
            dgvDedectType.Rows[20].Cells[0].Value = "内右下光";

            dgvDedectType.Rows[0].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].OverRadius.ToString ();
            dgvDedectType.Rows[1].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].TooSmallRadius.ToString ();
            dgvDedectType.Rows[2].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].UnFindRadius.ToString ();
            dgvDedectType.Rows[3].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].TextError;
            dgvDedectType.Rows[4].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].Scratch;
            dgvDedectType.Rows[5].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].Rust ;
            dgvDedectType.Rows[6].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].SandHole ;
            dgvDedectType.Rows[7].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].Bump ;
            dgvDedectType.Rows[8].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].CapUnPressed;
            dgvDedectType.Rows[9].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].BrightLine;
            dgvDedectType.Rows[10].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].MetalCapTooDark;
            dgvDedectType.Rows[11].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].MetalCapPressError;

            dgvDedectType.Rows[12].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].OuterRing;
            dgvDedectType.Rows[13].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].CapRing;
            dgvDedectType.Rows[14].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].InterRing;

            dgvDedectType.Rows[15].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].HighLight;
            dgvDedectType.Rows[16].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].LowSideLight;
            dgvDedectType.Rows[17].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].LeftTopLight;
            dgvDedectType.Rows[18].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].RightTopLight;
            dgvDedectType.Rows[19].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].LeftBotLight;
            dgvDedectType.Rows[20].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].RightBotLight;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            GlobalVariable.nowadayTotalInfo[index].Clear();
            dgvDedectType.Rows[0].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].OverRadius.ToString();
            dgvDedectType.Rows[1].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].TooSmallRadius.ToString();
            dgvDedectType.Rows[2].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].UnFindRadius.ToString();
            dgvDedectType.Rows[3].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].TextError;
            dgvDedectType.Rows[4].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].Scratch;
            dgvDedectType.Rows[5].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].Rust;
            dgvDedectType.Rows[6].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].SandHole;
            dgvDedectType.Rows[7].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].Bump;
            dgvDedectType.Rows[8].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].CapUnPressed;
            dgvDedectType.Rows[9].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].BrightLine;
            dgvDedectType.Rows[10].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].MetalCapTooDark;
            dgvDedectType.Rows[11].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].MetalCapPressError;

            dgvDedectType.Rows[12].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].OuterRing;
            dgvDedectType.Rows[13].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].CapRing;
            dgvDedectType.Rows[14].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].InterRing;

            dgvDedectType.Rows[15].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].HighLight;
            dgvDedectType.Rows[16].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].LowSideLight;
            dgvDedectType.Rows[17].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].LeftTopLight;
            dgvDedectType.Rows[18].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].RightTopLight;
            dgvDedectType.Rows[19].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].LeftBotLight;
            dgvDedectType.Rows[20].Cells[1].Value = GlobalVariable.nowadayTotalInfo[index].RightBotLight;
        }
    }
}
