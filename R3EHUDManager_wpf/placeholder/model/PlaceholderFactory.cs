using da2mvc.core.injection;
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
            PlaceholderModel placeholder = Injector.GetInstance<PlaceholderModel>();
            placeholder.Id = ++idCounter;
            placeholder.Name = name;
            placeholder.ResizeRule = ResizeRule.Get(name);
            return placeholder;
        }
    }
}
