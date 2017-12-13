using da2mvc.core.view;
using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.core.events;
using R3EHUDManager.contextmenu.view;
using R3EHUDManager.location.events;
using da2mvc.framework.model;
using da2mvc.framework.model.events;

namespace R3EHUDManager.location.view
{
    class R3eDirectoryMenuMediator : BaseMediator<R3eDirectoryMenuView>
    {
        public R3eDirectoryMenuMediator()
        {
            HandleEvent< CollectionModel < R3eDirectoryModel > , CollectionEventArgs <R3eDirectoryModel>>(CollectionModel<R3eDirectoryModel>.EVENT_ITEMS_ADDED, OnCollectionFilled);
            HandleEvent<SelectedR3eDirectoryModel, SelectedR3eDirectoryEventArgs>(SelectedR3eDirectoryModel.EVENT_DIRECTORY_CHANGED, OnDirectoryChanged);
        }

        private void OnDirectoryChanged(SelectedR3eDirectoryEventArgs args)
        {
            View.SetSelectedItem(args.Selection.Directory.Id);
        }

        private void OnCollectionFilled(CollectionEventArgs<R3eDirectoryModel> args)
        {
            View.Visible = args.Collection.Items.Count > 1;

            List<ContextMenuViewItem> items = new List<ContextMenuViewItem>();
            foreach (var directory in args.Collection.Items)
                items.Add(new ContextMenuViewItem(directory.Id, directory.Name));

            View.AddItems(items);
        }
    }
}
