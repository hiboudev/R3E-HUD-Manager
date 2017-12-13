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
    class ProfileMenuMediator : BaseMediator<ProfileMenuView>
    {
        public ProfileMenuMediator(SelectedProfileModel selectedProfile)
        {
            RegisterEventListener< CollectionModel < ProfileModel > , CollectionEventArgs <ProfileModel>>(CollectionModel<ProfileModel>.EVENT_ITEMS_ADDED, OnProfileAdded);
            RegisterEventListener<CollectionModel<ProfileModel>, CollectionEventArgs<ProfileModel>>(CollectionModel<ProfileModel>.EVENT_ITEMS_REMOVED, OnProfileRemoved);

            RegisterEventListener<SelectedProfileModel, ProfileEventArgs>(SelectedProfileModel.EVENT_SELECTION_CHANGED, OnProfileSelected);
            RegisterEventListener<SelectedProfileModel, ProfileEventArgs>(SelectedProfileModel.EVENT_SELECTION_CLEARED, OnProfileUnselected);

            RegisterEventListener<SaveProfileCommand, ProfileEventArgs>(SaveProfileCommand.EVENT_PROFILE_CHANGES_SAVED, OnProfileSaved);
        }
        
        private void OnProfileAdded(CollectionEventArgs<ProfileModel> args)
        {
             View.AddProfiles(args.ChangedItems);
        }

        private void OnProfileRemoved(CollectionEventArgs<ProfileModel> args)
        {
            foreach (var profile in args.ChangedItems)
                View.RemoveItem(profile.Id);
        }

        private void OnProfileUnselected(BaseEventArgs args)
        {
            View.UnselectProfile();
        }

        private void OnProfileSaved(ProfileEventArgs args)
        {
            View.UpdateProfile(args.Profile);
        }

        private void OnProfileSelected(ProfileEventArgs args)
        {
            View.SelectProfile(args.Profile);
        }
    }
}
