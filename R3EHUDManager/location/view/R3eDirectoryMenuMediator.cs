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
    class R3eDirectoryMenuMediator : BaseMediator
    {
        public R3eDirectoryMenuMediator()
        {
            RegisterEventListener(typeof(CollectionModel<R3eDirectoryModel>), CollectionModel<R3eDirectoryModel>.EVENT_ITEMS_ADDED, OnCollectionFilled);
            RegisterEventListener(typeof(SelectedR3eDirectoryModel), SelectedR3eDirectoryModel.EVENT_DIRECTORY_CHANGED, OnDirectoryChanged);
        }

        private void OnDirectoryChanged(BaseEventArgs args)
        {
            ((R3eDirectoryMenuView)View).SetSelectedItem(((SelectedR3eDirectoryEventArgs)args).Selection.Directory.Id);
        }

        private void OnCollectionFilled(BaseEventArgs args)
        {
            CollectionModel<R3eDirectoryModel> collection = ((CollectionEventArgs<R3eDirectoryModel>)args).Collection;
            
            ((R3eDirectoryMenuView)View).Visible = collection.Items.Count > 1;

            List<ContextMenuViewItem> items = new List<ContextMenuViewItem>();
            foreach (var directory in collection.Items)
                items.Add(new ContextMenuViewItem(directory.Id, directory.Name));

            ((R3eDirectoryMenuView)View).AddItems(items);
        }
    }
}
