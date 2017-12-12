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
    /**
     * For virtual mirror and position bar.
     */
    class BarRule : IResizeRule
    {
        public SizeF GetSize(Size referenceScreenSize, Size targetScreenSize, Size placeholderOriginalSize, bool isTripleScreen)
        {
            if (isTripleScreen)
                targetScreenSize = new Size(targetScreenSize.Width / 3, targetScreenSize.Height);

            double widthFactor = (double)targetScreenSize.Width / referenceScreenSize.Width;
            double heightFactor = Math.Sqrt(targetScreenSize.Width * targetScreenSize.Height) / Math.Sqrt(referenceScreenSize.Width * referenceScreenSize.Height);

            return new SizeF(
                (float)(widthFactor * placeholderOriginalSize.Width),
                (float)heightFactor * placeholderOriginalSize.Height
                );
        }
    }

    /**
     * For regular placeholders.
     */
    class RegularRule : IResizeRule
    {
        public SizeF GetSize(Size referenceScreenSize, Size targetScreenSize, Size placeholderOriginalSize, bool isTripleScreen)
        {
            if (isTripleScreen)
                targetScreenSize = new Size(targetScreenSize.Width / 3, targetScreenSize.Height);

            double factor = Math.Sqrt(targetScreenSize.Width * targetScreenSize.Height) / Math.Sqrt(referenceScreenSize.Width * referenceScreenSize.Height);

            return new SizeF(
                (float)(factor * placeholderOriginalSize.Width),
                (float)(factor * placeholderOriginalSize.Height)
                );
        }
    }
}
