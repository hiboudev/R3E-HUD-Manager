using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.placeholder.model
{
    class PlaceholderFactory
    {
        public static PlaceholderModel NewPlaceholder(string name)
        {
            return new PlaceholderModel(name)
            {
                ResizeRule = new NoRatioResizeRule()
            };
        }
    }
}
