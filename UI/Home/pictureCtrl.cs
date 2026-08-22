using System.Drawing;
using System.Windows.Forms;

namespace YiRongMachine
{
    public partial class pictureCtrl : UserControl
    {
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
        private string m_strInfo;

        public pictureCtrl()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void showImg(Image img)
        {
            if (img != null)
                m_Img = img;
            this.Invalidate();
            //zoomAll();
        }

        public void zoomAll()
        {
            if (m_Img == null)
                return;

            try
            {
                float fscl1 = (float)this.Width / (float)m_Img.Width;
                float fscl2 = (float)this.Height / (float)m_Img.Height;
                float fscl = fscl1 < fscl2 ? fscl1 : fscl2;
                m_factor = new PointF(fscl, fscl);

                float fx = (this.Width - m_Img.Width * fscl) / 2.0f / fscl;
                float fy = (this.Height - m_Img.Height * fscl) / 2.0f / fscl;

                m_ptOri = new PointF(fx, fy);
                this.Invalidate();
            }
            catch
            {
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                PointF ptOri = toGuiPt(new PointF(0, 0));
                if (m_Img != null)
                {
                    e.Graphics.DrawImage(m_Img, new Rectangle((int)ptOri.X, (int)ptOri.Y, (int)(m_Img.Width * m_factor.X), (int)(m_Img.Height * m_factor.Y)),
                    new Rectangle(0, 0, m_Img.Width, m_Img.Height), GraphicsUnit.Pixel);
                }
                if (m_strInfo != null)
                {
                    e.Graphics.DrawString(m_strInfo, new Font("宋体", 15), new SolidBrush(Color.Red), new PointF(10, 10));
                }
            }
            catch
            {
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                m_bMidBtnDown = true;
                doPan(PanType.PAN_SRT, e.Location);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle)
            {
                m_bMidBtnDown = false;
                doPan(PanType.PAN_END, e.Location);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (m_Img != null)
            {
                PointF pt = toGraphPt(e.Location);
                if (pt.X >= 0 && pt.X <= m_Img.Width && pt.Y >= 0 && pt.Y <= m_Img.Height)
                {
                    int ch = BitMapHelper.getPixelDot((int)pt.X, (int)pt.Y, (Bitmap)m_Img);
                    m_strInfo = "灰度:" + ch.ToString() + "  X:" + ((int)(pt.X)).ToString() + "  Y:" + ((int)(pt.Y)).ToString();
                    this.Invalidate();
                }
            }

            if (m_bMidBtnDown)
            {
                doPan(PanType.PAN_MOV, e.Location);
                this.Invalidate();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                zoomAtPoint(1.0F / m_dScl, e.Location);
            }
            else
            {
                zoomAtPoint(m_dScl, e.Location);
            }
            this.Invalidate();
        }

        private float toGraphX(float dx)
        {
            return dx / m_factor.X - m_ptOri.X;
        }

        private float toGraphY(float dy)
        {
            return dy / m_factor.Y - m_ptOri.Y;
        }

        private PointF toGraphPt(PointF pt)
        {
            return new PointF(toGraphX(pt.X), toGraphY(pt.Y));
        }

        private float toGuiX(float dx)
        {
            return (dx + m_ptOri.X) * m_factor.X;
        }

        private float toGuiY(float dy)
        {
            return (dy + m_ptOri.Y) * m_factor.Y;
        }

        private PointF toGuiPt(PointF pt)
        {
            return new PointF(toGuiX(pt.X), toGuiY(pt.Y));
        }

        private static bool bPan = false;
        private static PointF ptOriOld;
        private static PointF ptPanS;  //视图移动时的起点
        private static PointF ptPanM;  //视图移动中的坐标

        private void doPan(PanType pt, PointF point)
        {
            switch (pt)
            {
                case PanType.PAN_SRT:
                    bPan = true;
                    ptPanS = point;
                    ptPanM = point;
                    ptOriOld = m_ptOri;
                    break;

                case PanType.PAN_MOV:
                    if (bPan)
                    {
                        ptPanM = point;
                        ptPanM = new PointF(ptPanS.X - ptPanM.X, ptPanS.Y - ptPanM.Y);
                        PointF ptOriOld0 = new PointF(ptPanM.X / m_factor.X, ptPanM.Y / m_factor.Y);
                        m_ptOri.X = ptOriOld.X - ptOriOld0.X;
                        m_ptOri.Y = ptOriOld.Y - ptOriOld0.Y;
                    }
                    break;

                case PanType.PAN_END:
                    if (bPan)
                    {
                        bPan = false;
                        ptPanM = point;
                        ptPanM = new PointF(ptPanS.X - ptPanM.X, ptPanS.Y - ptPanM.Y);
                        PointF ptOriOld0 = new PointF(ptPanM.X / m_factor.X, ptPanM.Y / m_factor.Y);
                        m_ptOri.X = ptOriOld.X - ptOriOld0.X;
                        m_ptOri.Y = ptOriOld.Y - ptOriOld0.Y;
                    }
                    break;

                default:
                    break;
            }
        }

        private void zoomAtPoint(float zoomRate, PointF pt)
        {
            PointF ptN = toGraphPt(pt);

            m_factor.X /= zoomRate;
            m_factor.Y /= zoomRate;

            ptN = toGuiPt(ptN);
            ptN = new PointF((ptN.X - pt.X), (ptN.Y - pt.Y));
            PointF ptOri0 = new PointF(ptN.X / m_factor.X, ptN.Y / m_factor.Y);
            m_ptOri.X -= ptOri0.X;
            m_ptOri.Y -= ptOri0.Y;
        }
    }
}