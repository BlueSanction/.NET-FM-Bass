using System;
using System.Runtime.InteropServices;
using System.Drawing;

/// <summary>
/// Wind32API声明
/// </summary>
namespace TSkin
{
    public class Win32
    {
        public const int WM_CONTEXTMENU = 0x007B;
        public const int GWL_EXSTYLE = -20;
        public const int WS_EX_TRANSPARENT = 0x00000020;
        public const int WS_EX_LAYERED = 0x00080000;

        [StructLayout(LayoutKind.Sequential)]
        public struct Size
        {
            public Int32 cx;
            public Int32 cy;

            public Size(Int32 x, Int32 y)
            {
                cx = x;
                cy = y;
            }
        }

        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        public struct BLENDFUNCTION
        {
            public byte BlendOp;
            public byte BlendFlags;
            public byte SourceConstantAlpha;
            public byte AlphaFormat;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public Int32 x;
            public Int32 y;

            public Point(Int32 x, Int32 y)
            {
                this.x = x;
                this.y = y;
            }
        }

        public const byte AC_SRC_OVER = 0;
        public const Int32 ULW_ALPHA = 2;
        public const byte AC_SRC_ALPHA = 1;

        /// <summary>
        /// 从左到右显示
        /// </summary>
        public const Int32 AW_HOR_POSITIVE = 0x00000001;
        /// <summary>
        /// 从右到左显示
        /// </summary>
        public const Int32 AW_HOR_NEGATIVE = 0x00000002;
        /// <summary>
        /// 从上到下显示
        /// </summary>
        public const Int32 AW_VER_POSITIVE = 0x00000004;
        /// <summary>
        /// 从下到上显示
        /// </summary>
        public const Int32 AW_VER_NEGATIVE = 0x00000008;
        /// <summary>
        /// 若使用了AW_HIDE标志，则使窗口向内重叠，即收缩窗口；否则使窗口向外扩展，即展开窗口
        /// </summary>
        public const Int32 AW_CENTER = 0x00000010;
        /// <summary>
        /// 隐藏窗口，缺省则显示窗口
        /// </summary>
        public const Int32 AW_HIDE = 0x00010000;
        /// <summary>
        /// 激活窗口。在使用了AW_HIDE标志后不能使用这个标志
        /// </summary>
        public const Int32 AW_ACTIVATE = 0x00020000;
        /// <summary>
        /// 使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略
        /// </summary>
        public const Int32 AW_SLIDE = 0x00040000;
        /// <summary>
        /// 透明度从高到低
        /// </summary>
        public const Int32 AW_BLEND = 0x00080000;

        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr GetWindowDC(IntPtr handle);
        /// <summary>
        /// 执行动画
        /// </summary>
        /// <param name="whnd">控件句柄</param>
        /// <param name="dwtime">动画时间</param>
        /// <param name="dwflag">动画组合名称</param>
        /// <returns>bool值，动画是否成功</returns>
        [DllImport("user32")]
        public static extern bool AnimateWindow(IntPtr whnd, int dwtime, int dwflag);

        /// <summary>
        /// <para>该函数将指定的消息发送到一个或多个窗口。</para>
        /// <para>此函数为指定的窗口调用窗口程序直到窗口程序处理完消息再返回。</para>
        /// <para>而函数PostMessage不同，将一个消息寄送到一个线程的消息队列后立即返回。</para>
        /// return 返回值 : 指定消息处理的结果，依赖于所发送的消息。
        /// </summary>
        /// <param name="hWnd">要接收消息的那个窗口的句柄</param>
        /// <param name="Msg">消息的标识符</param>
        /// <param name="wParam">具体取决于消息</param>
        /// <param name="lParam">具体取决于消息</param>
        [DllImport("User32.dll", CharSet = CharSet.Auto, EntryPoint = "SendMessageA")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("gdi32.dll")]
        public static extern int CreateRoundRectRgn(int x1, int y1, int x2, int y2, int x3, int y3);

        [DllImport("user32.dll")]
        public static extern int SetWindowRgn(IntPtr hwnd, int hRgn, Boolean bRedraw);

        [DllImport("user32", EntryPoint = "GetWindowLong")]
        public static extern int GetWindowLong(
            IntPtr hwnd, int nIndex);

        [DllImport("user32.dll")]
        public static extern int SetWindowLong(
            IntPtr hwnd, int nIndex, int dwNewLong);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr CreateCompatibleDC(IntPtr hDC);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr SelectObject(IntPtr hDC, IntPtr hObj);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteDC(IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int DeleteObject(IntPtr hObj);

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern int UpdateLayeredWindow(IntPtr hwnd, IntPtr hdcDst, ref Point pptDst, ref Size psize, IntPtr hdcSrc, ref Point pptSrc, Int32 crKey, ref BLENDFUNCTION pblend, Int32 dwFlags);

        [DllImport("gdi32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr ExtCreateRegion(IntPtr lpXform, uint nCount, IntPtr rgnData);

        /// <summary>
        /// 两个API函数,其作用是:指定窗口的指定位置插入插入符.
        /// </summary>
        /// <param name="hwnd">窗口句柄</param>
        /// <param name="hBitmap">插入符的样式,可以为NULL</param>
        /// <param name="nWidth">插入符的宽度</param>
        /// <param name="nHeight">插入符的高度</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "CreateCaret")]
        public static extern int CreateCaret(IntPtr hwnd, int hBitmap, int nWidth, int nHeight);

        /// <summary>
        /// 显示插入符
        /// </summary>
        /// <param name="hWnd">窗口句柄</param>
        /// <returns></returns>
        [DllImport("user32.dll", EntryPoint = "ShowCaret")]
        public static extern bool ShowCaret(IntPtr hWnd);

        [DllImport("User32.dll", EntryPoint = "SetCaretPos")]
        public static extern bool SetCaretPos(int x, int y);



        public const int WM_MOUSEMOVE = 0x0200;
        public const int WM_LBUTTONDOWN = 0x0201;
        public const int WM_LBUTTONUP = 0x0202;
        public const int WM_RBUTTONDOWN = 0x0204;
        public const int WM_LBUTTONDBLCLK = 0x0203;

        public const int WM_MOUSELEAVE = 0x02A3;



        public const int WM_PAINT = 0x000F;
        public const int WM_ERASEBKGND = 0x0014;

        public const int WM_PRINT = 0x0317;

        //const int EN_HSCROLL       =   0x0601;
        //const int EN_VSCROLL       =   0x0602;

        public const int WM_HSCROLL = 0x0114;
        public const int WM_VSCROLL = 0x0115;


        public const int EM_GETSEL = 0x00B0;
        public const int EM_LINEINDEX = 0x00BB;
        public const int EM_LINEFROMCHAR = 0x00C9;

        public const int EM_POSFROMCHAR = 0x00D6;



        [DllImport("USER32.DLL", EntryPoint = "PostMessage")]
        public static extern bool PostMessage(IntPtr hwnd, uint msg,
            IntPtr wParam, IntPtr lParam);

        /*
            BOOL PostMessage(          HWND hWnd,
                UINT Msg,
                WPARAM wParam,
                LPARAM lParam
                );
        */

        // Put this declaration in your class   //IntPtr
        [DllImport("USER32.DLL", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int msg, IntPtr wParam,
            IntPtr lParam);




        [DllImport("USER32.DLL", EntryPoint = "GetCaretBlinkTime")]
        public static extern uint GetCaretBlinkTime();




        const int WM_PRINTCLIENT = 0x0318;

        const long PRF_CHECKVISIBLE = 0x00000001L;
        const long PRF_NONCLIENT = 0x00000002L;
        const long PRF_CLIENT = 0x00000004L;
        const long PRF_ERASEBKGND = 0x00000008L;
        const long PRF_CHILDREN = 0x00000010L;
        const long PRF_OWNED = 0x00000020L;

        /*  Will clean this up later doing something like this
        enum  CaptureOptions : long
        {
            PRF_CHECKVISIBLE= 0x00000001L,
            PRF_NONCLIENT	= 0x00000002L,
            PRF_CLIENT		= 0x00000004L,
            PRF_ERASEBKGND	= 0x00000008L,
            PRF_CHILDREN	= 0x00000010L,
            PRF_OWNED		= 0x00000020L
        }
        */


        public static bool CaptureWindow(System.Windows.Forms.Control control,
                                ref System.Drawing.Bitmap bitmap)
        {
            //This function captures the contents of a window or control

            Graphics g2 = Graphics.FromImage(bitmap);

            //PRF_CHILDREN // PRF_NONCLIENT
            int meint = (int)(PRF_CLIENT | PRF_ERASEBKGND); //| PRF_OWNED ); //  );
            System.IntPtr meptr = new System.IntPtr(meint);

            System.IntPtr hdc = g2.GetHdc();
            Win32.SendMessage(control.Handle, Win32.WM_PRINT, hdc, meptr);

            g2.ReleaseHdc(hdc);
            g2.Dispose();

            return true;

        }

    }
}