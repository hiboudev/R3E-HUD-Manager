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

        public override bool Equals(object obj)
        {
            return obj is R3ePoint && ((R3ePoint)obj).X == X && ((R3ePoint)obj).Y == Y;
        }
    }
}
