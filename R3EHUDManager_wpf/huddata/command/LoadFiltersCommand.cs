using da2mvc.core.command;
using R3EHUDManager.database;
using R3EHUDManager.huddata.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.huddata.command
{
    class LoadFiltersCommand : ICommand
    {
        private readonly PlaceholderBlackListModel blackList;
        private readonly Database database;

        public LoadFiltersCommand(PlaceholderBlackListModel blackList, Database database)
        {
            this.blackList = blackList;
            this.database = database;
        }

        public void Execute()
        {
            blackList.InitializeFilters(database.GetPlaceholderFilters());
        }
    }
}
