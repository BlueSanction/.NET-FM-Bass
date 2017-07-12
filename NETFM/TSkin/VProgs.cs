using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace TSkin.Controls
{
    public partial class VProgs : Control
    {
        public VProgs()
        {
            //实例化创建Image图像
            this.SetStyle(
               ControlStyles.AllPaintingInWmPaint |
               ControlStyles.OptimizedDoubleBuffer |
               ControlStyles.ResizeRedraw |
               ControlStyles.Selectable |
               ControlStyles.DoubleBuffer |
               ControlStyles.SupportsTransparentBackColor |
               ControlStyles.UserPaint, true);
            this.SetStyle(ControlStyles.Opaque, false);
            base.BackColor = Color.Transparent;
            this.UpdateStyles();
        }


        public delegate void ScrollValueEventHandler(object sender, double e);

        public event ScrollValueEventHandler ScrollValueChange;

        protected virtual void ScrollValue(double e)
        {
            if (ScrollValueChange != null&&e!=_value)
                ScrollValueChange(this, GetV(e));
        }
        private double GetV(double value)
        {
            if (value < 0)
            {
                return  0;
            }
            else if (value > 30)
            {
                return  30;
            }
            else
            {
                return value;
            }
        }

        Color _BackColor = Color.Silver;
        /// <summary>
        /// 背景色
        /// </summary>
        [Description("进度条整体背景色")]
        public Color DM_BackColor
        {
            get { return _BackColor; }
            set { _BackColor = value; this.Invalidate(); }
        }

        Color _BlockColor = Color.DodgerBlue;
        /// <summary>
        /// 进度颜色
        /// </summary>
        [Description("进度条到达的进度背景色")]
        public Color DM_BlockColor
        {
            get { return _BlockColor; }
            set { _BlockColor = value; this.Invalidate(); }
        }

        double _value;
        [Description("当前进度")]
        public double DM_Value
        {
            get { return _value; }
            set
            {

                _value = GetV(value);
                this.Invalidate();
            }
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //设置高质量,低速度呈现平滑程度
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            Rectangle rect = new Rectangle(ClientRectangle.X, ClientRectangle.Y + 11, ClientRectangle.Width, ClientRectangle.Height - 22);

            double wd = Width;
            ///绘制背景色
            g.FillRectangle(new SolidBrush(_BackColor), rect);

            int jj = Convert.ToInt32(wd * (DM_Value / 30.0));
            if (jj > Width - 6) { jj = Width-6; }
            //绘制进度
            g.FillRectangle(new SolidBrush(_BlockColor), new Rectangle(rect.X, rect.Y,jj, rect.Height));

            ret = new Rectangle(jj, 0, 6, Height);
            //绘制圆柱
            g.FillPath(new SolidBrush(DM_BlockColor), GetRoundRectangle(ret, 4));
            //g.FillEllipse(new SolidBrush(DM_BlockColor), ret);
        }

        private GraphicsPath GetRoundRectangle(Rectangle rectangle, int r)
        {
            int l = 2 * r;
            // 把圆角矩形分成八段直线、弧的组合，依次加到路径中 
            GraphicsPath gp = new GraphicsPath();
            gp.AddLine(new Point(rectangle.X + r, rectangle.Y), new Point(rectangle.Right - r, rectangle.Y));
            gp.AddArc(new Rectangle(rectangle.Right - l, rectangle.Y, l, l), 270F, 90F);

            gp.AddLine(new Point(rectangle.Right, rectangle.Y + r), new Point(rectangle.Right, rectangle.Bottom - r));
            gp.AddArc(new Rectangle(rectangle.Right - l, rectangle.Bottom - l, l, l), 0F, 90F);

            gp.AddLine(new Point(rectangle.Right - r, rectangle.Bottom), new Point(rectangle.X + r, rectangle.Bottom));
            gp.AddArc(new Rectangle(rectangle.X, rectangle.Bottom - l, l, l), 90F, 90F);

            gp.AddLine(new Point(rectangle.X, rectangle.Bottom - r), new Point(rectangle.X, rectangle.Y + r));
            gp.AddArc(new Rectangle(rectangle.X, rectangle.Y, l, l), 180F, 90F);
            return gp;
        }
        Rectangle ret;
        
       public bool IsMouseDown = false;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            IsMouseDown = true;
            base.OnMouseDown(e);
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            IsMouseDown = false;
            base.OnMouseUp(e);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (ret != null)
            {
                if (ret.Contains(e.Location))
                {
                    this.Cursor = Cursors.Hand;
                }
                else
                {
                    if (!IsMouseDown)
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            if (IsMouseDown)
            {
                double jj = (Convert.ToDouble(e.X - 3) / Convert.ToDouble(Width)) * 30.0;
                ScrollValue(jj);
                DM_Value = jj;
            }
            base.OnMouseMove(e);
        }
        
    }
}
