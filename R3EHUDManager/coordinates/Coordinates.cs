using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.coordinates
{
    class Coordinates
    {
        public static R3ePoint ToR3e(Point point, Size size)
        {
            double x = ((double)point.X / size.Width) * 2 - 1;
            double y = ((double)(size.Height - point.Y) / size.Height) * 2 - 1;

            return new R3ePoint(x, y);
        }

        public static Point FromR3e(R3ePoint point, Size size)
        {
            double x = size.Width * (point.X + 1) / 2;
            double y = size.Height - size.Height * (point.Y + 1) / 2;
            return new Point((int)Math.Round(x), (int)Math.Round(y));
        }
    }
}
