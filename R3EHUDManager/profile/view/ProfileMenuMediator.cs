using da2mvc.core.view;
using R3EHUDManager.profile.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.core.events;
using R3EHUDManager.profile.events;
using R3EHUDManager.contextmenu.view;
using R3EHUDManager.profile.command;
using da2mvc.framework.model;
using da2mvc.framework.model.events;

namespace R3EHUDManager.profile.view
{
    class ProfileMenuMediator : BaseMediator
    {
        public ProfileMenuMediator(SelectedProfileModel selectedProfile)
        {
            RegisterEventListener(typeof(CollectionModel<ProfileModel>), CollectionModel<ProfileModel>.EVENT_ITEMS_ADDED, OnProfileAdded);
            RegisterEventListener(typeof(CollectionModel<ProfileModel>), CollectionModel<ProfileModel>.EVENT_ITEMS_REMOVED, OnProfileRemoved);

            RegisterEventListener(typeof(SelectedProfileModel), SelectedProfileModel.EVENT_SELECTION_CHANGED, OnProfileSelected);
            RegisterEventListener(typeof(SelectedProfileModel), SelectedProfileModel.EVENT_SELECTION_CLEARED, OnProfileUnselected);

            RegisterEventListener(typeof(SaveProfileCommand), SaveProfileCommand.EVENT_PROFILE_CHANGES_SAVED, OnProfileSaved);
        }
        
        private void OnProfileAdded(BaseEventArgs args)
        {
             ((ProfileMenuView)View).AddProfiles(((CollectionEventArgs<ProfileModel>)args).ChangedItems);
        }

        private void OnProfileRemoved(BaseEventArgs args)
        {
            foreach (var profile in ((CollectionEventArgs<ProfileModel>)args).ChangedItems)
                ((ProfileMenuView)View).RemoveItem(profile.Id);
        }

        private void OnProfileUnselected(BaseEventArgs args)
        {
            ((ProfileMenuView)View).UnselectProfile();
        }

        private void OnProfileSaved(BaseEventArgs args)
        {
            ((ProfileMenuView)View).UpdateProfile(((ProfileEventArgs)args).Profile);
        }

        private void OnProfileSelected(BaseEventArgs args)
        {
            ((ProfileMenuView)View).SelectProfile(((ProfileEventArgs)args).Profile);
        }
    }
}
