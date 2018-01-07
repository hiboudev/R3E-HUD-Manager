using da2mvc.core.command;
using R3EHUDManager.database;
using R3EHUDManager.huddata.events;
using R3EHUDManager.huddata.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.huddata.command
{
    class UpdateFiltersCommand : ICommand
    {
        private readonly FiltersChangedEventArgs args;
        private readonly PlaceholderBlackListModel blacklist;
        private readonly Database database;

        public UpdateFiltersCommand(FiltersChangedEventArgs args, PlaceholderBlackListModel blacklist, Database database)
        {
            this.args = args;
            this.blacklist = blacklist;
            this.database = database;
        }

        public void Execute()
        {
            foreach (KeyValuePair<string, bool> keyValue in args.Filters)
                blacklist.SetFilter(keyValue.Key, !keyValue.Value);

            database.UpdatePlaceholderFilters(blacklist.Filters);
        }
    }
}
