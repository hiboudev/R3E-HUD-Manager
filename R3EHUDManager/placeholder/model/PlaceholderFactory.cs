using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.model
{
    class PlaceholderFactory
    {
        private static int idCounter = 0;

        public static PlaceholderModel NewPlaceholder(string name)
        {
            return new PlaceholderModel(++idCounter, name)
            {
                ResizeRule = GetResizeRule(name),
            };
        }

        private static IResizeRule GetResizeRule(string name)
        {
            switch (name)
            {
                case PlaceholderName.APEXHUNT_DISPLAY:
                case PlaceholderName.CAR_STATUS:
                case PlaceholderName.DRIVER_NAME_TAGS:
                case PlaceholderName.FFB_GRAPH:
                case PlaceholderName.FLAGS:
                case PlaceholderName.MINI_MOTEC:
                case PlaceholderName.MOTEC:
                case PlaceholderName.TRACK_MAP:
                    return new RegularRule();

                case PlaceholderName.VIRTUAL_MIRROR:
                case PlaceholderName.POSITION_BAR:
                    return new BarRule();

                default:
                    return new RegularRule();
            }
        }
    }
}
