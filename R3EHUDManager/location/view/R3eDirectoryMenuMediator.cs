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

namespace R3EHUDManager.location.view
{
    class R3eDirectoryMenuMediator : BaseMediator
    {
        public R3eDirectoryMenuMediator()
        {
            RegisterEventListener(typeof(R3eDirectoryCollectionModel), R3eDirectoryCollectionModel.EVENT_COLLECTION_FILLED, OnCollectionFilled);
            RegisterEventListener(typeof(SelectedR3eDirectoryModel), SelectedR3eDirectoryModel.EVENT_DIRECTORY_CHANGED, OnDirectoryChanged);
        }

        private void OnDirectoryChanged(BaseEventArgs args)
        {
            ((R3eDirectoryMenuView)View).SetSelectedItem(((SelectedR3eDirectoryEventArgs)args).Selection.Directory.Id);
        }

        private void OnCollectionFilled(BaseEventArgs args)
        {
            R3eDirectoryCollectionModel collection = ((R3eDirectoryCollectionEventArgs)args).Collection;
            
            ((R3eDirectoryMenuView)View).Visible = collection.Directories.Count > 1;

            List<ContextMenuViewItem> items = new List<ContextMenuViewItem>();
            foreach (var directory in collection.Directories)
                items.Add(new ContextMenuViewItem(directory.Id, directory.Name));

            ((R3eDirectoryMenuView)View).AddItems(items);
        }
    }
}
