using System;
using System.Windows.Forms;

namespace YiRongMachine
{
    public enum MsgType
    {
        Message = 0,   //消息
        Retry = 1,     //重试
        Choose = 2,    //选择
    }

    public partial class AlarmDialog : Form
    {
        public DateTime starttime;
        public string alarmmsg = "";
        public bool IsRetry = false;

        public AlarmDialog(string AlarmMsg, MsgType msgtype)
        {
            InitializeComponent();
            alarmmsg = AlarmMsg;
            //在窗体中显示报警信息
            btnAlarmMsg.Text = AlarmMsg;

            if (msgtype == MsgType.Message)
            {
                btnNG.Visible = false;
                btnRestart.Visible = false;
                return;
            }
            else if (msgtype == MsgType.Retry)
            {
                btnOK.Visible = false;
                btnNG.Visible = false;
            }
            else if (msgtype == MsgType.Choose)
            {
                btnOK.Visible = false;
            }

            //将线程暂停
            GlobalVariable.pcState = PCState.Pause;
            //HomeForm.home.lblStatus.Text = "暂停中";
            //HomeForm.home.lblStatus.BackColor = Color.Yellow;
            //数据：开始时间、结束时间,持续时间，报警信息,解决方案,是否是PLC报警
            starttime = DateTime.Now;//.ToString("yyyy/MM/dd HH:mm:ss");
        }

        private void btnRestart_Click(object sender, EventArgs e)
        {
            IsRetry = true;
            this.DialogResult = DialogResult.Retry;
            SaveAlarmLog(IsRetry);
        }

        private void btnAlarmMsg_Click(object sender, EventArgs e)
        {
            //这一个按钮里不需要任何方法，作用是显示报警信息
        }

        private void btnNG_Click(object sender, EventArgs e)
        {
            IsRetry = false;
            this.DialogResult = DialogResult.Ignore;
            SaveAlarmLog(IsRetry);
        }

        /// <summary>
        /// 将报警信息保存进入CSV中
        /// </summary>
        /// <param name="IsRetry"></param>
        private void SaveAlarmLog(bool IsRetry)
        {
            DateTime endtime = DateTime.Now;
            TimeSpan AlarmInterval = endtime - starttime;
            string continuetime = (AlarmInterval.TotalMilliseconds / 1000).ToString("f0");
            string solution = "";
            if (IsRetry)
            {
                solution = "人工按下重试按钮";
            }
            else
            {
                solution = "人工按下NG按钮";
            }
            string ErrorMsg = "";
            string[] log = { starttime.ToString("yyyy/MM/dd HH:mm:ss"), endtime.ToString("yyyy/MM/dd HH:mm:ss"), continuetime, alarmmsg, solution, "否" };
            CSVHelper.WriteCSVLog(log, FilePath.AlarmLogPath, ref ErrorMsg);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}