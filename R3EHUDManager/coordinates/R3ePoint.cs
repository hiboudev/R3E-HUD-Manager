using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.coordinates
{
    class R3ePoint
    {
        public R3ePoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; }
        public double Y { get; }

        internal R3ePoint Clone()
        {
            return new R3ePoint(X, Y);
        }
    }
}
