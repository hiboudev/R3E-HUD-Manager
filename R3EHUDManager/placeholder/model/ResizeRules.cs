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
    internal class ResizeRule
    {
        public static IResizeRule Get(string placeholderName)
        {
            switch (placeholderName)
            {
                case PlaceholderName.APEXHUNT_DISPLAY:
                case PlaceholderName.CAR_STATUS:
                case PlaceholderName.DRIVER_NAME_TAGS:
                case PlaceholderName.FLAGS:
                case PlaceholderName.MINI_MOTEC:
                case PlaceholderName.MOTEC:
                case PlaceholderName.TRACK_MAP:
                    return new RegularRule();

                case PlaceholderName.VIRTUAL_MIRROR:
                case PlaceholderName.POSITION_BAR:
                    return new BarRule();

                case PlaceholderName.FFB_GRAPH:
                    return new FFBRule();

                default:
                    return new RegularRule();
            }
        }
    }

    interface IResizeRule
    {
        // TODO take Model.Size into account here and not everywhere else.
        SizeF GetSize(Size referenceScreenSize, Size targetScreenSize, Size placeholderOriginalSize, bool isTripleScreen);
    }
    /**
     * For virtual mirror and position bar.
     */
    class BarRule : IResizeRule
    {
        public SizeF GetSize(Size referenceScreenSize, Size targetScreenSize, Size placeholderOriginalSize, bool isTripleScreen)
        {
            SizeF targetScreenSizeF;

            if (isTripleScreen)
                targetScreenSizeF = new SizeF((float)targetScreenSize.Width / 3, targetScreenSize.Height);
            else
                targetScreenSizeF = new SizeF(targetScreenSize.Width, targetScreenSize.Height);

            double widthFactor = (double)targetScreenSizeF.Width / referenceScreenSize.Width;
            double heightFactor = Math.Sqrt(targetScreenSizeF.Width * targetScreenSizeF.Height) / Math.Sqrt(referenceScreenSize.Width * referenceScreenSize.Height);

            return new SizeF(
                (float)widthFactor * placeholderOriginalSize.Width,
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
            SizeF targetScreenSizeF;

            if (isTripleScreen)
                targetScreenSizeF = new SizeF((float)targetScreenSize.Width / 3, targetScreenSize.Height);
            else
                targetScreenSizeF = new SizeF(targetScreenSize.Width, targetScreenSize.Height);

            double factor = Math.Sqrt(targetScreenSizeF.Width * targetScreenSizeF.Height) / Math.Sqrt(referenceScreenSize.Width * referenceScreenSize.Height);

            return new SizeF(
                (float)(factor * placeholderOriginalSize.Width),
                (float)(factor * placeholderOriginalSize.Height)
                );
        }
    }

    /**
     * For FFB meter.
     */
    class FFBRule : IResizeRule
    {
        public SizeF GetSize(Size referenceScreenSize, Size targetScreenSize, Size placeholderOriginalSize, bool isTripleScreen)
        {
            SizeF targetScreenSizeF;

            if (isTripleScreen)
                targetScreenSizeF = new SizeF((float)targetScreenSize.Width / 3, targetScreenSize.Height);
            else
                targetScreenSizeF = new SizeF(targetScreenSize.Width, targetScreenSize.Height);

            double factor = Math.Sqrt(targetScreenSizeF.Width * targetScreenSizeF.Height) / Math.Sqrt(referenceScreenSize.Width * referenceScreenSize.Height);

            return new SizeF(
                (float)(factor * placeholderOriginalSize.Width) * (isTripleScreen ? 3 : 1),
                (float)(factor * placeholderOriginalSize.Height)
                );
        }
    }
}
