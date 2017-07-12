using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace TSkin
{

    public partial class Main : Form
    {
        //绘制层

        private MainForm skin;
        public Main()
        {
            InitializeComponent();
            //减少闪烁

            SetStyles();
        }
        #region 减少闪烁
        private void SetStyles()
        {
            SetStyle(
                ControlStyles.UserPaint |
                ControlStyles.AllPaintingInWmPaint |
                ControlStyles.OptimizedDoubleBuffer |
                //ControlStyles.ResizeRedraw |
                ControlStyles.DoubleBuffer, true);
            //强制分配样式重新应用到控件上
            UpdateStyles();
            base.AutoScaleMode = AutoScaleMode.None;
        }
        #endregion

        #region 变量属性

        //不显示FormBorderStyle属性
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FormBorderStyle FormBorderStyle
        {
            get { return base.FormBorderStyle; }
            set { base.FormBorderStyle = FormBorderStyle.None; }
        }

        private bool _yin = true;
        [Category("Skin")]
        [Description("窗体是否有阴影")]
        [DefaultValue(typeof(bool), "true")]
        public bool Yin
        {
            get { return _yin; }
            set
            {

                if (_yin != value)
                {
                    _yin = value;
                    if (_yin)
                    {
                        if (skin == null)
                        {
                            skin = new MainForm(this, _CanResize);
                            skin.Show(this); SetRs();
                        }
                    }
                    else
                    {
                        try
                        {
                                skin.Close();
                                skin.Dispose();
                            skin = null;
                        }
                        catch
                        {
                        }
                    }
                }
            }
        }

        private bool _CanResize = false;
        [Category("Skin")]
        [Description("窗体是否可以改变大小")]
        [DefaultValue(typeof(bool), "false")]
        public bool CanResize
        {
            get { return _CanResize; }
            set
            {
                if (_CanResize != value)
                {
                    _CanResize = value; SetRs();
                }
            }
        }
        private void SetRs() {
            try
            {
                if (_CanResize)
                {
                    skin.panel1.Cursor = Cursors.SizeWE;
                    skin.panel2.Cursor = Cursors.SizeWE;
                    skin.panel3.Cursor = Cursors.SizeNS;
                    skin.panel4.Cursor = Cursors.SizeNS;
                    skin.panel5.Cursor = Cursors.SizeNWSE;
                    skin.panel6.Cursor = Cursors.SizeNWSE;
                    skin.panel7.Cursor = Cursors.SizeNESW;
                    skin.panel8.Cursor = Cursors.SizeNESW;
                }
                else
                {
                    skin.panel1.Cursor = Cursors.Default;
                    skin.panel2.Cursor = Cursors.Default;
                    skin.panel3.Cursor = Cursors.Default;
                    skin.panel4.Cursor = Cursors.Default;
                    skin.panel5.Cursor = Cursors.Default;
                    skin.panel6.Cursor = Cursors.Default;
                    skin.panel7.Cursor = Cursors.Default;
                    skin.panel8.Cursor = Cursors.Default;
                }
            }
            catch { }
        }
        #endregion

        #region 重载事件

        protected override void OnVisibleChanged(EventArgs e)
        {
            try
            {
                if (Visible)
                {
                    //启用窗口淡入淡出
                    if (!DesignMode)
                    {
                        //淡入特效
                        Win32.AnimateWindow(this.Handle, 100, Win32.AW_BLEND | Win32.AW_ACTIVATE);
                    }
                    if (_yin)
                    {
                        if (!DesignMode && skin == null)
                        {
                            skin = new MainForm(this, _CanResize);
                            skin.Show(this); SetRs();
                        }
                    }
                }
                else
                {
                    Win32.AnimateWindow(this.Handle, 70, Win32.AW_BLEND | Win32.AW_HIDE);
                }
                base.OnVisibleChanged(e);
            }
            catch { }
        }

        //窗体关闭时
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            //先关闭阴影窗体
            if (_yin)
            {
                if (skin != null)
                {
                    skin.Close();
                }
            }
            //在Form_FormClosing中添加代码实现窗体的淡出
            Win32.AnimateWindow(this.Handle, 150, Win32.AW_BLEND | Win32.AW_HIDE);
        }
        

        ////移动窗体
        //protected override void OnMouseDown(MouseEventArgs e)
        //{
        //    if (CanMove)
        //    {
        //        //释放鼠标焦点捕获
        //        Win32.ReleaseCapture();
        //        //向当前窗体发送拖动消息
        //        Win32.SendMessage(this.Handle, 0x0112, 0xF011, 0);
        //        base.OnMouseUp(e);
        //    }
        //    base.OnMouseDown(e);
        //}
        #endregion

        #region 允许点击任务栏最小化
        protected override CreateParams CreateParams
        {
            get
            {
                const int WS_MINIMIZEBOX = 0x00020000;  // Winuser.h中定义
                CreateParams cp = base.CreateParams;
                cp.Style = cp.Style | WS_MINIMIZEBOX;   // 允许最小化操作
                return cp;
            }
        }
        #endregion



        //const int Guying_HTLEFT = 10;
        //const int Guying_HTRIGHT = 11;
        //const int Guying_HTTOP = 12;
        //const int Guying_HTTOPLEFT = 13;
        //const int Guying_HTTOPRIGHT = 14;
        //const int Guying_HTBOTTOM = 15;
        //const int Guying_HTBOTTOMLEFT = 0x10;
        //const int Guying_HTBOTTOMRIGHT = 17;
    }
}
