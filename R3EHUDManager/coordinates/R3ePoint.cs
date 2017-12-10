using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.coordinates
{
    class R3ePoint
    {
        public double X { get; }
        public double Y { get; }

        public R3ePoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        internal R3ePoint Clone()
        {
            return new R3ePoint(X, Y);
        }

        public bool Equals(R3ePoint point)
        {
            return point.X == X && point.Y == Y;
        }
    }
}
