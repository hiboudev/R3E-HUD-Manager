using da2mvc.framework.collection.model;
using da2mvc.framework.collection.view;
using R3EHUDManager.location.events;
using R3EHUDManager.location.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.location.view
{
    class R3eDirectoryMenuMediator : CollectionMediator<CollectionModel<R3eDirectoryModel>, R3eDirectoryModel, R3eDirectoryMenuView>
    {
        public R3eDirectoryMenuMediator()
        {
            HandleEvent<SelectedR3eDirectoryModel, SelectedR3eDirectoryEventArgs>(SelectedR3eDirectoryModel.EVENT_DIRECTORY_CHANGED, OnDirectoryChanged);
        }

        private void OnDirectoryChanged(SelectedR3eDirectoryEventArgs args)
        {
            View.SetSelectedItem(args.Selection.Directory.Id);
        }
    }
}
