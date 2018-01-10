using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.huddata.model
{
    public class PlaceholderBlackListModel
    {
        public Dictionary<string, bool> Filters { get; private set; } = new Dictionary<string, bool>();

        public void InitializeFilters(Dictionary<string, bool> filters)
        {
            Filters = filters;
        }

        public void SetFilter(string placeholderName, bool isFiltered)
        {
            Filters[placeholderName] = isFiltered;
        }

        public bool IsFiltered(string placeholderName)
        {
            return Filters[placeholderName];
        }
    }
}
