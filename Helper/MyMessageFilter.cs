using System.Windows.Forms;

namespace YiRongMachine
{
    //IMessageFilter接口为微软底层系统接口
    public class MyMessageFilter : IMessageFilter
    {
        #region IMessageFilter 成员

        public bool PreFilterMessage(ref Message m)
        {
            //如果检测到有鼠标或则键盘的消息 可添加其他消息ID如触摸屏的点击事件ID
            const int WM_KEYDOWN = 0x0100;
            //const int WM_MOUSEMOVE = 0x0200;  //鼠标移动
            const int WM_LBUTTONDOWN = 0x0201;
            const int WM_RBUTTONDOWN = 0x0204;
            switch (m.Msg)
            {
                case WM_KEYDOWN:
                //case WM_MOUSEMOVE:
                case WM_LBUTTONDOWN:
                case WM_RBUTTONDOWN:
                    UserManager._dtLastOperate = System.DateTime.Now;
                    break;
            }
            return false;
        }

        #endregion IMessageFilter 成员
    }
}