using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DMSkin
{
    /// <summary>
    /// 关于 GDI+ 绘图的辅助类
    /// </summary>
    public class DrawHelper
    {
        #region RendererBackground 渲染背景图片，使背景图片不失真
        
        /// <summary>
        /// 渲染背景图片,使背景图片不失真
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="backgroundImage"></param>
        /// <param name="method"></param>
        public static void RendererBackground(Graphics g, Rectangle rect, Image backgroundImage, bool method)
        {
            if (!method)
            {
                g.DrawImage(backgroundImage, new Rectangle(rect.X + 0, rect.Y, 5, rect.Height), 0, 0, 5, backgroundImage.Height, GraphicsUnit.Pixel);
                g.DrawImage(backgroundImage, new Rectangle(rect.X + 5, rect.Y, rect.Width - 10, rect.Height), 5, 0, backgroundImage.Width - 10, backgroundImage.Height, GraphicsUnit.Pixel);
                g.DrawImage(backgroundImage, new Rectangle(rect.X + rect.Width - 5, rect.Y, 5, rect.Height), backgroundImage.Width - 5, 0, 5, backgroundImage.Height, GraphicsUnit.Pixel);
            }
            else
            {
                DrawHelper.RendererBackground(g, rect, 5, backgroundImage);
            }
        }

        /// <summary>
        /// 渲染背景图片,使背景图片不失真
        /// </summary>
        /// <param name="g"></param>
        /// <param name="rect"></param>
        /// <param name="cut"></param>
        /// <param name="backgroundImage"></param>
        public static void RendererBackground(Graphics g, Rectangle rect, int cut, Image backgroundImage)
        {
            //左上角
            g.DrawImage(backgroundImage, new Rectangle(rect.X, rect.Y, cut, cut), 0, 0, cut, cut, GraphicsUnit.Pixel);
            //上边
            g.DrawImage(backgroundImage, new Rectangle(rect.X + cut, rect.Y, rect.Width - cut * 2, cut), cut, 0, backgroundImage.Width - cut * 2, cut, GraphicsUnit.Pixel);
            //右上角
            g.DrawImage(backgroundImage, new Rectangle(rect.X + rect.Width - cut, rect.Y, cut, cut), backgroundImage.Width - cut, 0, cut, cut, GraphicsUnit.Pixel);
            //左边
            g.DrawImage(backgroundImage, new Rectangle(rect.X, rect.Y + cut, cut, rect.Height - cut * 2), 0, cut, cut, backgroundImage.Height - cut*2, GraphicsUnit.Pixel);
            //左下角
            g.DrawImage(backgroundImage, new Rectangle(rect.X, rect.Y + rect.Height - cut, cut, cut), 0, backgroundImage.Height - cut, cut, cut, GraphicsUnit.Pixel);
            //右边
            g.DrawImage(backgroundImage, new Rectangle(rect.X + rect.Width - cut, rect.Y + cut, cut, rect.Height - cut * 2), backgroundImage.Width - cut, cut, cut, backgroundImage.Height - cut*2, GraphicsUnit.Pixel);
            //右下角
            g.DrawImage(backgroundImage, new Rectangle(rect.X + rect.Width - cut, rect.Y + rect.Height - cut, cut, cut), backgroundImage.Width - cut, backgroundImage.Height - cut, cut, cut, GraphicsUnit.Pixel);
            //下边
            g.DrawImage(backgroundImage, new Rectangle(rect.X + cut, rect.Y + rect.Height - cut, rect.Width - cut * 2, cut), cut, backgroundImage.Height - cut, backgroundImage.Width - cut * 2, cut, GraphicsUnit.Pixel);
            //平铺中间
            g.DrawImage(backgroundImage, new Rectangle(rect.X + cut, rect.Y + cut, rect.Width - cut * 2, rect.Height - cut * 2), cut, cut, backgroundImage.Width - cut * 2, backgroundImage.Height - cut * 2, GraphicsUnit.Pixel);
        }

        #endregion
    }
}
