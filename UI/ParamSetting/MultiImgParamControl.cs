using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class MultiImgParamControl : UserControl
    {
        public MultiImgParamControl(int iType)
        {
            InitializeComponent();
        }

        public bool SaveParam(ref string ErrorMsg)
        {
            return true;
        }
    }
}