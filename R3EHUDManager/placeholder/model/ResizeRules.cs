using R3EHUDManager.screen.view;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.model
{
    interface IResizeRule
    {
        decimal GetResizeRatio(Size referenceScreenSize, Size targetScreenSize, Size placeholderOriginalSize);
    }

    class MotecResizeRule : IResizeRule
    {

        /**
         * Approximation for FFB and Motec, a bit strange...
         */
        public decimal GetResizeRatio(Size referenceScreenSize, Size targetScreenSize, Size placeholderOriginalSize)
        {
            // Screen ratio reference
            decimal Rr = (decimal)referenceScreenSize.Width / referenceScreenSize.Height;
            // Screen ratio target
            decimal Rt = (decimal)targetScreenSize.Width / targetScreenSize.Height;
            // Screen width reference
            decimal Wr = referenceScreenSize.Width;
            // Screen width target
            decimal Wt = targetScreenSize.Width;
            // Ratio object
            decimal Ro = (decimal)placeholderOriginalSize.Width / placeholderOriginalSize.Height;

            return (Rr * (1 / Ro) + Ro * (1 / Rt)) * (1m / 2) * (Wt / Wr);
        }
    }

    /**
     * Doesn't display items properly if screen ratio is different than 16/9.
     */
    class NoRatioResizeRule : IResizeRule
    {
        public decimal GetResizeRatio(Size referenceScreenSize, Size targetScreenSize, Size placeholderOriginalSize)
        {
            // Screen width reference
            decimal Hr = referenceScreenSize.Height;
            // Screen width target
            decimal Ht = targetScreenSize.Height;

            return (Ht / Hr);
        }
    }
}
