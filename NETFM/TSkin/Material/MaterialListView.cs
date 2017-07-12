using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MaterialSkin.Controls
{
    public class MaterialListView : ListView 
    {

       public enum MouseStates
        {
            HOVER,
            DOWN,
            OUT
        }

		[Browsable(false)]
		public MouseStates MouseState { get; set; }
		[Browsable(false)]
		public Point MouseLocation { get; set; }

		public MaterialListView()
		{
			GridLines = false;
			FullRowSelect = true;
			HeaderStyle = ColumnHeaderStyle.Nonclickable;
			View = View.Details;
			OwnerDraw = true;
			ResizeRedraw = true;
			BorderStyle = BorderStyle.None;
			SetStyle(ControlStyles.DoubleBuffer | ControlStyles.OptimizedDoubleBuffer, true);

			//Fix for hovers, by default it doesn't redraw
			//TODO: should only redraw when the hovered line changed, this to reduce unnecessary redraws
			MouseLocation = new Point(-1, -1);
			MouseState = MouseStates.OUT;
			MouseEnter += delegate
			{
				MouseState = MouseStates.HOVER;
			}; 
			MouseLeave += delegate
			{
				MouseState = MouseStates.OUT; 
				MouseLocation = new Point(-1, -1);
				Invalidate();
			};
			MouseDown += delegate { MouseState = MouseStates.DOWN; };
			MouseUp += delegate{ MouseState = MouseStates.HOVER; };
			MouseMove += delegate(object sender, MouseEventArgs args)
			{
				MouseLocation = args.Location;
				Invalidate();
			};
        } 
        public event EventHandler HScroll;
        public event EventHandler VScroll;
        const int WM_HSCROLL = 0x0114;
        const int WM_VSCROLL = 0x0115;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HSCROLL)
            {
                OnHScroll(this, null);
            }
            else if (m.Msg == WM_VSCROLL)
            {
                OnVScroll(this, null);
            }
            base.WndProc(ref m);
        }
        virtual protected void OnHScroll(object sender, EventArgs e)
        {
            if (HScroll != null)
                HScroll(this, e);
        }
        virtual protected void OnVScroll(object sender, EventArgs e)
        {
            if (VScroll != null)
                VScroll(this, e);
        }

        public Font ROBOTO_MEDIUM_12 = new Font("Microsoft JhengHei", 12f);
        public Font ROBOTO_MEDIUM_10 = new Font("Microsoft JhengHei", 10f);

        private Color BACKGROUND_DARK = Color.White;//背景色
        public Brush TEXT_WHITE = new SolidBrush(Color.Black);//标题颜色

        private Brush FLAT_BUTTON_BACKGROUND_PRESSED_DARK_BRUSH = new SolidBrush(Color.FromArgb(15, Color.Black));//点击颜色

        private Color DIVIDERS_WHITE = Color.FromArgb(31, 255, 255, 255);

        private Brush PRIMARY_TEXT_WHITE_BRUSH = new SolidBrush(Color.Black);//内容字颜色
        protected override void OnDrawColumnHeader(DrawListViewColumnHeaderEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(BACKGROUND_DARK), new Rectangle(e.Bounds.X, e.Bounds.Y, Width, e.Bounds.Height));
            e.Graphics.DrawString(e.Header.Text,
                ROBOTO_MEDIUM_10,
                TEXT_WHITE,
                new Rectangle(e.Bounds.X + ITEM_PADDING, e.Bounds.Y + ITEM_PADDING, e.Bounds.Width - ITEM_PADDING * 2, e.Bounds.Height - ITEM_PADDING * 2),
                getStringFormat());
        }

        private const int ITEM_PADDING = 12;
        protected override void OnDrawItem(DrawListViewItemEventArgs e)
        {
            try
            {
                //We draw the current line of items (= item with subitems) on a temp bitmap, then draw the bitmap at once. This is to reduce flickering.
                var b = new Bitmap(e.Item.Bounds.Width, e.Item.Bounds.Height);
                var g = Graphics.FromImage(b);

                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                //always draw default background
                g.FillRectangle(new SolidBrush(BACKGROUND_DARK), new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));

                if (e.State.HasFlag(ListViewItemStates.Selected))
                {
                    //selected background
                    g.FillRectangle(FLAT_BUTTON_BACKGROUND_PRESSED_DARK_BRUSH, new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
                }
                else if (e.Bounds.Contains(MouseLocation) && MouseState == MouseStates.HOVER)
                {
                    //hover background
                    g.FillRectangle(FLAT_BUTTON_BACKGROUND_PRESSED_DARK_BRUSH, new Rectangle(new Point(e.Bounds.X, 0), e.Bounds.Size));
                }


                //Draw seperator
                g.DrawLine(new Pen(DIVIDERS_WHITE), e.Bounds.Left, 0, e.Bounds.Right, 0);

                foreach (ListViewItem.ListViewSubItem subItem in e.Item.SubItems)
                {
                    int ji = ITEM_PADDING;
                    //Draw text
                    //if (subItem.ForeColor == Color.Red)
                    //{
                    //    g.DrawImage(TSkin.Properties.Resources.play, new Rectangle(subItem.Bounds.Location.X, 5, 35, 35)); ji = 30;
                    //}
                    //else if (subItem.ForeColor == Color.Blue)
                    //{
                    //    g.DrawImage(TSkin.Properties.Resources.shi, new Rectangle(subItem.Bounds.Location.X, 6, 32, 35)); ji = 30;
                    //}
                    g.DrawString(subItem.Text, ROBOTO_MEDIUM_10, PRIMARY_TEXT_WHITE_BRUSH,
                                     new Rectangle(subItem.Bounds.Location.X + ji, ITEM_PADDING, subItem.Bounds.Width - 2 * ITEM_PADDING, subItem.Bounds.Height - 2 * ITEM_PADDING),
                                     getStringFormat());
                }

                e.Graphics.DrawImage((Image)b.Clone(), e.Item.Bounds.Location);
                g.Dispose();
                b.Dispose();
            }
            catch { }
        }

        private StringFormat getStringFormat()
        {
            return new StringFormat
            {
                FormatFlags = StringFormatFlags.LineLimit,
                Trimming = StringTrimming.EllipsisCharacter,
                Alignment = StringAlignment.Near,
                LineAlignment = StringAlignment.Center
            };
        }

        protected override void OnCreateControl()
        {
            base.OnCreateControl();

            //This is a hax for the needed padding.
            //Another way would be intercepting all ListViewItems and changing the sizes, but really, that will be a lot of work
            //This will do for now.
            Font = new Font(ROBOTO_MEDIUM_12.FontFamily, 24);
        }
    }
}
