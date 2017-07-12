using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;


namespace TSkin
{
    public partial class MainForm : Form
    {
        //控件层
        private Main Main;
        //带参构造
        public MainForm(Main main,bool nc)
        {
            //将控制层传值过来
            this.Main = main;
            InitializeComponent();
            //置顶窗体
            TopMost = Main.TopMost;
            Main.BringToFront();
            //是否在任务栏显示
            ShowInTaskbar = false;
            //无边框模式
            FormBorderStyle = FormBorderStyle.None;
            //设置绘图层显示位置
            this.Location = new Point(Main.Location.X - 10, Main.Location.Y - 10);
            //设置大小
            Width = Main.Width + 20;
            Height = Main.Height + 20;
            //绘图层窗体移动
            Main.LocationChanged += new EventHandler(Main_LocationChanged);
            Main.SizeChanged += new EventHandler(Main_SizeChanged);
            Main.VisibleChanged += new EventHandler(Main_VisibleChanged);
            //还原任务栏右键菜单
            //CommonClass.SetTaskMenu(Main);
            //加载背景
            SetBits();
            //窗口鼠标穿透效果
            //CanPenetrate();
        }
        #region 还原任务栏右键菜单
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cParms = base.CreateParams;
                cParms.ExStyle |= 0x00080000; // WS_EX_LAYERED
                return cParms;
            }
        }
        public class CommonClass
        {
            [DllImport("user32.dll", EntryPoint = "GetWindowLong", CharSet = CharSet.Auto)]
            static extern int GetWindowLong(HandleRef hWnd, int nIndex);
            [DllImport("user32.dll", EntryPoint = "SetWindowLong", CharSet = CharSet.Auto)]
            static extern IntPtr SetWindowLong(HandleRef hWnd, int nIndex, int dwNewLong);
            public const int WS_SYSMENU = 0x00080000;
            public const int WS_MINIMIZEBOX = 0x20000;
            public static void SetTaskMenu(Form form)
            {
                int windowLong = (GetWindowLong(new HandleRef(form, form.Handle), -16));
                SetWindowLong(new HandleRef(form, form.Handle), -16, windowLong | WS_SYSMENU | WS_MINIMIZEBOX);
            }
        }
        #endregion

        #region 减少闪烁
        private void SetStyles()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer, true);
            //强制分配样式重新应用到控件上
            UpdateStyles();
            base.AutoScaleMode = AutoScaleMode.None;
        }
        #endregion

        #region 控件层相关事件
        //移动主窗体时
        void Main_LocationChanged(object sender, EventArgs e)
        {
            Location = new Point(Main.Left - 10, Main.Top - 10);
        }

        //主窗体大小改变时
        void Main_SizeChanged(object sender, EventArgs e)
        {
            //设置大小
            Width = Main.Width + 20;
            Height = Main.Height + 20;
            SetBits();
        }

        //主窗体显示或隐藏时
        void Main_VisibleChanged(object sender, EventArgs e)
        {
            this.Visible = Main.Visible;
        }
        #endregion

        //#region 使窗口有鼠标穿透功能
        ///// <summary>
        ///// 使窗口有鼠标穿透功能
        ///// </summary>
        //private void CanPenetrate()
        //{
        //    int intExTemp = Win32.GetWindowLong(this.Handle, Win32.GWL_EXSTYLE);
        //    int oldGWLEx = Win32.SetWindowLong(this.Handle, Win32.GWL_EXSTYLE, Win32.WS_EX_TRANSPARENT | Win32.WS_EX_LAYERED);
        //}
        //#endregion

        #region 不规则无毛边方法
        public void SetBits()
        {
            //绘制绘图层背景
            Bitmap bitmap = new Bitmap(Main.Width + 20, Main.Height + 20);
            Rectangle _BacklightLTRB = new Rectangle(25, 25, 25, 25);//窗体光泽重绘边界
            Graphics g = Graphics.FromImage(bitmap);
            g.SmoothingMode = SmoothingMode.HighQuality; //高质量
            g.PixelOffsetMode = PixelOffsetMode.HighQuality; //高像素偏移质量

            Bitmap ps = NETFM.Properties.Resources.bgs;
            ImageDrawRect.DrawRect(g, ps, ClientRectangle, Rectangle.FromLTRB(_BacklightLTRB.X, _BacklightLTRB.Y, _BacklightLTRB.Width, _BacklightLTRB.Height), 1, 1);

            //Color _BorderColor = Color.DodgerBlue;
            //g.DrawLine(new Pen(_BorderColor), 10,10, Width-10, 10);
            //g.DrawLine(new Pen(_BorderColor), 10, 10, 10, Height-10);
            //g.DrawLine(new Pen(_BorderColor),10, Height - 10, Width-10, Height - 10);
            //g.DrawLine(new Pen(_BorderColor), Width -10, 10, Width - 10, Height-10);

            if (!Bitmap.IsCanonicalPixelFormat(bitmap.PixelFormat) || !Bitmap.IsAlphaPixelFormat(bitmap.PixelFormat))
                throw new ApplicationException("图片必须是32位带Alhpa通道的图片。");
            IntPtr oldBits = IntPtr.Zero;
            IntPtr screenDC = Win32.GetDC(IntPtr.Zero);
            IntPtr hBitmap = IntPtr.Zero;
            IntPtr memDc = Win32.CreateCompatibleDC(screenDC);

            try
            {
                Win32.Point topLoc = new Win32.Point(Left, Top);
                Win32.Size bitMapSize = new Win32.Size(Width, Height);
                Win32.BLENDFUNCTION blendFunc = new Win32.BLENDFUNCTION();
                Win32.Point srcLoc = new Win32.Point(0, 0);

                hBitmap = bitmap.GetHbitmap(Color.FromArgb(0));
                oldBits = Win32.SelectObject(memDc, hBitmap);

                blendFunc.BlendOp = Win32.AC_SRC_OVER;
                blendFunc.SourceConstantAlpha = Byte.Parse("255");
                blendFunc.AlphaFormat = Win32.AC_SRC_ALPHA;
                blendFunc.BlendFlags = 0;

                Win32.UpdateLayeredWindow(Handle, screenDC, ref topLoc, ref bitMapSize, memDc, ref srcLoc, 0, ref blendFunc, Win32.ULW_ALPHA);
            }
            catch { }
            finally
            {
                if (hBitmap != IntPtr.Zero)
                {
                    Win32.SelectObject(memDc, oldBits);
                    Win32.DeleteObject(hBitmap);
                }
                Win32.ReleaseDC(IntPtr.Zero, screenDC);
                Win32.DeleteDC(memDc);
            }
        }
        #endregion
        
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        //改变窗体大小
        private void Con(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
            if (Main.CanResize)
            {
                //Main.LocationChanged -= new EventHandler(Main_LocationChanged);
                //Main.SizeChanged -= new EventHandler(Main_SizeChanged);
                Panel d = (Panel)sender;
                ReleaseCapture();
                SendMessage(Main.Handle, 0x0112, Ons(d.Tag.ToString()), 0);
                //Main.Location = Location;
                //Main.Size = Size;
                //Main.LocationChanged += new EventHandler(Main_LocationChanged);
                //Main.SizeChanged += new EventHandler(Main_SizeChanged);
            }
            }
        }
        private int Ons(string a)
        {
            switch (a)
            {
                case "1": return 0xF001;
                case "2": return 0xF002;
                case "3": return 0xF003;
                case "4": return 0xF004;
                case "5": return 0xF005;
                case "6": return 0xF006;
                case "7": return 0xF007;
                case "8": return 0xF008;
            }
            return 0xF010 + 0x0002;
        }
    }
}
