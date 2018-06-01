using R3EHUDManager.coordinates;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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
                case PlaceholderName.INPUT_METER:
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

    public interface IResizeRule
    {
        Size GetSize(R3ePoint modelSize, Size referenceScreenSize, Size targetScreenSize, Size placeholderReferenceSize, bool isTripleScreen);
    }
    /**
     * For virtual mirror and position bar.
     */
    public class BarRule : IResizeRule
    {
        public Size GetSize(R3ePoint modelSize, Size referenceScreenSize, Size targetScreenSize, Size placeholderReferenceSize, bool isTripleScreen)
        {
            if (isTripleScreen)
                targetScreenSize = new Size(targetScreenSize.Width / 3, targetScreenSize.Height);
            else
                targetScreenSize = new Size(targetScreenSize.Width, targetScreenSize.Height);

            double widthFactor = targetScreenSize.Width / referenceScreenSize.Width;
            double heightFactor = Math.Sqrt(targetScreenSize.Width * targetScreenSize.Height) / Math.Sqrt(referenceScreenSize.Width * referenceScreenSize.Height);

            return new Size(
                 modelSize.X * widthFactor * placeholderReferenceSize.Width,
                 modelSize.Y * heightFactor * placeholderReferenceSize.Height
                );
        }
    }

    /**
     * For regular placeholders.
     */
    public class RegularRule : IResizeRule
    {
        public Size GetSize(R3ePoint modelSize, Size referenceScreenSize, Size targetScreenSize, Size placeholderReferenceSize, bool isTripleScreen)
        {
            if (isTripleScreen)
                targetScreenSize = new Size(targetScreenSize.Width / 3, targetScreenSize.Height);
            else
                targetScreenSize = new Size(targetScreenSize.Width, targetScreenSize.Height);

            double factor = Math.Sqrt(targetScreenSize.Width * targetScreenSize.Height) / Math.Sqrt(referenceScreenSize.Width * referenceScreenSize.Height);

            return new Size(
                modelSize.X * factor * placeholderReferenceSize.Width,
                modelSize.Y * factor * placeholderReferenceSize.Height
                );
        }
    }

    /**
     * For FFB meter.
     */
    public class FFBRule : IResizeRule
    {
        public Size GetSize(R3ePoint modelSize, Size referenceScreenSize, Size targetScreenSize, Size placeholderReferenceSize, bool isTripleScreen)
        {
            if (isTripleScreen)
                targetScreenSize = new Size(targetScreenSize.Width / 3, targetScreenSize.Height);
            else
                targetScreenSize = new Size(targetScreenSize.Width, targetScreenSize.Height);

            double factor = Math.Sqrt(targetScreenSize.Width * targetScreenSize.Height) / Math.Sqrt(referenceScreenSize.Width * referenceScreenSize.Height);

            return new Size(
                modelSize.X * factor * placeholderReferenceSize.Width * (isTripleScreen ? 3 : 1),
                modelSize.Y * factor * placeholderReferenceSize.Height
                );
        }
    }
}
