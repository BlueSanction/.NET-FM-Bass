using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TSkin
{
    public static class GraphicsPathHelper
    {
        /// <summary>
        /// 建立带有圆角样式的路径。
        /// </summary>
        /// <param name="rect">用来建立路径的矩形。</param>
        /// <param name="_radius">圆角的大小。</param>
        /// <param name="style">圆角的样式。</param>
        /// <param name="correction">是否把矩形长宽减 1,以便画出边框。</param>
        /// <returns>建立的路径。</returns>
        public static GraphicsPath CreatePath(
            Rectangle rect, int radius, bool correction)
        {
            GraphicsPath path = new GraphicsPath();
            int radiusCorrection = correction ? 1 : 0;
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(
                rect.Right - radius - radiusCorrection,
                rect.Y,
                radius,
                radius,
                270,
                90);
            path.AddArc(
                rect.Right - radius - radiusCorrection,
                rect.Bottom - radius - radiusCorrection,
                radius,
                radius, 0, 90);
            path.AddArc(
                rect.X,
                rect.Bottom - radius - radiusCorrection,
                radius,
                radius,
                90,
                90);
            path.CloseFigure();
            return path;
        }
    }
}