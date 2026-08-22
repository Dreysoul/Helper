using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace YiRongMachine
{
    public class UIHelper
    {
        #region 拖动窗体

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;

        #endregion 拖动窗体

        /// <summary>
        /// 是否是有效控件：控件是否为空，是否已经被释放了
        /// </summary>
        /// <param name="control">控件的实例化对象</param>
        /// <returns></returns>
        public static bool IsValidControl(Control control)
        {
            if (null == control || control.IsDisposed)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 增加tab页
        /// </summary>
        /// <param name="tb"></param>
        /// <param name="tp"></param>
        /// <param name="tpName"></param>
        /// <param name="form"></param>
        /// <param name="page"></param>
        public static void AddTabPage(TabControl tb, TabPage tp, UserControl form, ref int page)
        {
            if (!IsValidControl(tb) ||
                !IsValidControl(tp) ||
                !IsValidControl(form))
            {
                return;
            }

            form.Dock = DockStyle.Fill;
            tb.TabPages[page].Controls.Add(form);
            page++;
        }

        public static void AddTabPage(TabPage tp, UserControl form)
        {
            if (!IsValidControl(tp) ||
                !IsValidControl(form))
            {
                return;
            }

            form.Dock = DockStyle.Fill;
            tp.Controls.Add(form);
        }

        /// <summary>
        /// 往RichTextBox中增加数据
        /// </summary>
        /// <param name="rtbMsgShow">RichTextBox的实例化对象</param>
        /// <param name="msg">要发送的内容</param>
        public static void AddAppendText(RichTextBox rtbMsgShow, string msg)
        {
            try
            {
                if (!IsValidControl(rtbMsgShow))
                {
                    return;
                }
                rtbMsgShow.AppendText(DateTime.Now.ToString("HH:mm:ss:fff ") + msg);
                rtbMsgShow.Focus();
                rtbMsgShow.AppendText("\r\n");
                LogHelper.AddCommLog(msg);
            }
            catch (Exception e)
            {
                LogHelper.AddCommLog(e.Message);
            }
        }

        /// <summary>
        /// 设置DGV常用属性，可以改变内容值
        /// </summary>
        /// <param name="DGV">DGV的实例化对象</param>
        public static void SetDGVFormat_Change(DataGridView DGV, int MinRowHeight)
        {
            DGV.MultiSelect = false;  //不允许多行
            DGV.EnableHeadersVisualStyles = false;
            DGV.RowHeadersVisible = false;//去除前面的空白列
            DGV.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control; //行头背景
            DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //设置表头（居中显示）
            DGV.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //禁止用户改变列宽
            DGV.AllowUserToResizeColumns = false;
            //禁止用户改变行高
            DGV.AllowUserToResizeRows = false;
            //禁止用户改变列头的高度
            DGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //用户调整
            DGV.AllowUserToAddRows = false;      //不显示最后一行
            //列自适应
            DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //行最小高度
            DGV.RowTemplate.MinimumHeight = MinRowHeight;
        }

        /// <summary>
        /// 设置DGV常用属性，只是显示
        /// </summary>
        /// <param name="DGV"></param>
        /// <param name="MinRowHeight"></param>
        public static void SetDGVFormat_JustShow(DataGridView DGV, int MinRowHeight)
        {
            DGV.ReadOnly = true;  //仅读
            DGV.MultiSelect = false;  //不允许多行
            DGV.EnableHeadersVisualStyles = false;
            DGV.RowHeadersVisible = false;//去除前面的空白列
            DGV.ColumnHeadersDefaultCellStyle.BackColor = SystemColors.Control; //行头背景
            DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; //设置表头（居中显示）
            DGV.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True;
            //禁止用户改变行高
            DGV.AllowUserToResizeRows = false;
            //禁止用户改变列头的高度
            DGV.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            //用户调整
            DGV.AllowUserToAddRows = false;      //不显示最后一行
            //每一列都是填充
            DGV.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            //行最小高度
            DGV.RowTemplate.MinimumHeight = MinRowHeight;
            //禁止用户排序
            for (int i = 0; i < DGV.Columns.Count; i++)
            {
                DGV.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        /// <summary>
        /// ToolStip的格式
        /// </summary>
        /// <param name="toolTip1"></param>
        public static void SetToolTipFormat(ToolTip toolTip1)
        {
            toolTip1.InitialDelay = 200;
            toolTip1.ReshowDelay = 300;
            toolTip1.IsBalloon = true;
        }

        public static void RichTestBoxAppendTextColorful(RichTextBox rtBox, string addtext, Color color)
        {
            rtBox.SelectionStart = rtBox.TextLength;
            rtBox.SelectionLength = 0;
            rtBox.SelectionColor = color;
            rtBox.AppendText(addtext);
            //rtBox.SelectionColor = rtBox.ForeColor;
        }

        public static void PaintControl(Control control, Panel panel, int ColumnNum, int RowNum)
        {
            for (int i = 0; i < ColumnNum * RowNum; i++)
            {
                int j = 10 / ColumnNum;
                int k = 10 % ColumnNum;
                control.Location = new Point(k * 180, j * 50);
                control.Size = new Size(180, 50);
                panel.Controls.Add(control);
            }
        }

        public static void GeiAllControls(Control con, List<Control> l)
        {
            foreach (Control item in con.Controls)
            {
                l.Add(item);
                if (item.Controls.Count > 0)
                {
                    GeiAllControls(item, l);
                }
            }
        }

        private static string text_temp = "";
        private static Regex obj = new Regex(@"^(-?\d+)(\.\d+)?$");

        public static void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if ((!(char.IsNumber(e.KeyChar) || (e.KeyChar == '.') || (e.KeyChar == '-') || (e.KeyChar == 8)))
                || ((e.KeyChar == '.') && (tb.Text.Contains('.')))
                || ((e.KeyChar == '-') && (tb.Text.Contains('-')))
               )
            {
                e.Handled = true;
            }
        }

        public static void textBox_Enter(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            text_temp = tb.Text;
        }

        public static void textBox_Leave(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (!obj.IsMatch(tb.Text))
            {
                tb.Text = text_temp;
            }
        }
    }
}