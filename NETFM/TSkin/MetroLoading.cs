using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Drawing.Drawing2D;

namespace TSkin
{
    public partial class MetroLoading : Control
    {
        List<int> Cr = null;
        public MetroLoading()
        {
            SetStyle(
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.UserPaint |
                ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor,
                true);
            Cr = new List<int>();
            Cr.Add(-10);
            Cr.Add(-60);
            Cr.Add(-110);
            Cr.Add(-160);
        }
        protected override void Dispose(bool disposing)
        {
            Stop();
            base.Dispose(disposing);
        }
        private Color _co = Color.White;
        [Description("圆角颜色"), Category("TSkin"), DefaultValue(typeof(Color), "White")]
        public Color Color { get { return _co; } set { _co = value; } }
        Thread Tim = null;
        /// <summary>
        /// 获取动画状态
        /// </summary>
        public bool State = false;
        /// <summary>
        /// 启动动画
        /// </summary>
        public void Start()
        {
            if (!State)
            {
                jiz = false;
                Cr[0] = -10;
                Cr[1] = -60;
                Cr[2] = -110;
                Cr[3] = -160;
                if (Tim != null)
                {
                    try
                    {
                        Tim.Abort(); Tim = null;
                    }
                    catch { }
                }
                State = true;
                Tim = new Thread(GG);
                Tim.Start();
            }
        }
        /// <summary>
        /// 停止动画
        /// </summary>
        public void Stop()
        {
            if (State)
            { 
            Cr[0] = -10;
            Cr[1] = -60;
            Cr[2] = -110;
            Cr[3] = -160; 
            try
            {
                State = false;
                Tim.Abort(); Tim = null;
            }
            catch { }
            }
        }
        bool jiz = false;
        /// <summary>
        /// 等最后一个动画结束停止
        /// </summary>
        public void Stops()
        {
            if (State)
            {
                jiz = true;
            }
        }
        private void GG() {
            while (true)
            {
                Cr[0] = Getint(Cr[0]);
                Cr[1] = Getint1(Cr[1]);
                Cr[2] = Getint2(Cr[2]);
                Cr[3] = Getint3(Cr[3]);
                Thread.Sleep(10);
                this.Invalidate();
            }
        }
        int ji1 = 1;
        int ji2 = 1;
        int ji3 = 1;
        int ji4 = 1;
        private int Getint(int j)
        {
            int Wi = ClientRectangle.Width;
            int Wis = Wi / 2 - 50;
            if (j > Wi)
            { return -1100; }
            else if (j > Wis && j < Wi - Wis)
            {
                if (ji1!= 1)
                {
                    ji1--;
                }
                return j += ji1;
            }
            else
            {
                if (ji1!=5)
                {
                    ji1++;
                }
                return j += ji1;
            }
        }
        private int Getint1(int j)
        {
            int Wi = ClientRectangle.Width;
            int Wis = Wi / 2 - 50;
            if (j > Wi)
            { return -1100; }
            else if (j > Wis && j < Wi - Wis)
            {
                if (ji2!= 1)
                {
                    ji2--;
                }
                return j += ji2;
            }
            else
            {
                if (ji2!=5)
                {
                    ji2++;
                }
                return j += ji2;
            }
        }
        private int Getint2(int j)
        {
            int Wi = ClientRectangle.Width;
            int Wis = Wi / 2 - 50;
            if (j > Wi)
            { return -1100; }
            else if (j > Wis && j < Wi - Wis)
            {
                if (ji3 != 1)
                {
                    ji3--;
                }
                return j += ji3;
            }
            else
            {
                if (ji3!=5)
                {
                    ji3++;
                }
                return j += ji3;
            }
        }
        private int Getint3(int j)
        {
            int Wi = ClientRectangle.Width;
            int Wis = Wi / 2 - 50;
            if (j > Wi)
            {
                Cr[0]=-10;
                Cr[1] = -60;
                Cr[2] = -110;
                if (jiz)
                {
                    jiz = false;
                    try
                    {
                        State = false;
                        Tim.Abort(); Tim = null;
                    }
                    catch { } 
                }
                return -160;
            }
            else if (j > Wis && j < Wi - Wis)
            {
                if (ji4 !=1)
                {
                    ji4--;
                }
                return j += ji4;
            }
            else
            {
                if (ji4!=5)
                {
                    ji4++;
                }
                return j += ji4;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                if (Tim!=null)
                {
                    //抗锯齿
                    e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

                    using (var bmp = new Bitmap(ClientRectangle.Width, 5))
                    {
                        //缓冲绘制
                        using (Graphics bufferGraphics = Graphics.FromImage(bmp))
                        {
                            //抗锯齿
                            bufferGraphics.SmoothingMode = SmoothingMode.HighQuality;
                            foreach (int dot in Cr)
                            {
                                var rect = new RectangleF(
                                    new PointF(dot - 2, 0),
                                    new SizeF(4, 4));

                                bufferGraphics.FillEllipse(new SolidBrush(_co),
                                    rect);
                            }
                        }

                        //贴图
                        e.Graphics.DrawImage(bmp, new PointF(0, 0));
                    } //bmp disposed
                }
            }
            catch { }
            base.OnPaint(e);
        }
    }
}
