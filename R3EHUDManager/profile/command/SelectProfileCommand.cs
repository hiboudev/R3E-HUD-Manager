using da2mvc.command;
using R3EHUDManager.application.events;
using R3EHUDManager.contextmenu.events;
using R3EHUDManager.profile.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.profile.command
{
    class SelectProfileCommand : ICommand
    {
        private readonly ContextMenuEventArgs args;
        private readonly ProfileCollectionModel collection;
        private readonly SelectedProfileModel selection;

        public SelectProfileCommand(ContextMenuEventArgs args, ProfileCollectionModel collection, SelectedProfileModel selection)
        {
            this.args = args;
            this.collection = collection;
            this.selection = selection;
        }

        public void Execute()
        {
            selection.SelectProfile(collection.Get(args.ItemId));
        }
    }
}
