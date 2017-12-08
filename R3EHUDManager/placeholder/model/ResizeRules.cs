using R3EHUDManager.screen.view;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.model
{
    interface IResizeRule
    {
        SizeF GetSize(Size referenceScreenSize, Size targetScreenSize, Size placeholderOriginalSize, bool isTripleScreen);
    }

    // TODO not actually taking into account ratio and need comparaisons with R3E display.

    /**
     * For virtual mirror and position bar. Not accurate actually.
     */
    class BarRule : IResizeRule
    {
        public SizeF GetSize(Size referenceScreenSize, Size targetScreenSize, Size placeholderOriginalSize, bool isTripleScreen)
        {
            float widthFactor = isTripleScreen ? 1f / 3 : 1;

            return new SizeF(
                widthFactor * placeholderOriginalSize.Width * targetScreenSize.Width / referenceScreenSize.Width,
                (float)placeholderOriginalSize.Height * targetScreenSize.Height / referenceScreenSize.Height
                );
        }
    }

    /**
     * For regular placeholders.
     */
    class HeightRule : IResizeRule
    {
        public SizeF GetSize(Size referenceScreenSize, Size targetScreenSize, Size placeholderOriginalSize, bool isTripleScreen)
        {
            float ratio = (float)targetScreenSize.Height / referenceScreenSize.Height;

            return new SizeF(
                ratio * placeholderOriginalSize.Width,
                ratio * placeholderOriginalSize.Height
                );
        }
    }



    //class MotecResizeRule : IResizeRule
    //{

    //    /**
    //     * Approximation for FFB and Motec, a bit strange...
    //     */
    //    public decimal GetResizeRatio(Size referenceScreenSize, Size targetScreenSize, Size placeholderOriginalSize)
    //    {
    //        // Screen ratio reference
    //        decimal Rr = (decimal)referenceScreenSize.Width / referenceScreenSize.Height;
    //        // Screen ratio target
    //        decimal Rt = (decimal)targetScreenSize.Width / targetScreenSize.Height;
    //        // Screen width reference
    //        decimal Wr = referenceScreenSize.Width;
    //        // Screen width target
    //        decimal Wt = targetScreenSize.Width;
    //        // Ratio object
    //        decimal Ro = (decimal)placeholderOriginalSize.Width / placeholderOriginalSize.Height;

    //        return (Rr * (1 / Ro) + Ro * (1 / Rt)) * (1m / 2) * (Wt / Wr);
    //    }
    //}
}
