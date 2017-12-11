using da2mvc.view;
using R3EHUDManager.profile.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.events;
using R3EHUDManager.profile.events;
using R3EHUDManager.contextmenu.view;

namespace R3EHUDManager.profile.view
{
    class ProfileMenuMediator : BaseMediator
    {
        public ProfileMenuMediator()
        {
            RegisterEventListener(typeof(ProfileCollectionModel), ProfileCollectionModel.EVENT_PROFILE_ADDED, OnProfileAdded);
            RegisterEventListener(typeof(SelectedProfileModel), SelectedProfileModel.EVENT_SELECTION_CHANGED, OnProfileSelected);
        }

        private void OnProfileSelected(BaseEventArgs args)
        {
            ((ProfileMenuView)View).SetSelectedItem(((ProfileEventArgs)args).Profile.Id);
        }

        private void OnProfileAdded(BaseEventArgs args)
        {
            var view = (ProfileMenuView)View;
            var profiles = ((ProfileCollectionEventArgs)args).ModifiedProfiles;

            List<ContextMenuViewItem> items = new List<ContextMenuViewItem>();

            foreach (ProfileModel profile in profiles)
                items.Add(new ContextMenuViewItem(profile.Id, profile.Name));

            view.AddItems(items);
        }
    }
}
