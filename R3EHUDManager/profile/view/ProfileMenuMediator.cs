using da2mvc.core.view;
using R3EHUDManager.profile.model;
using da2mvc.core.events;
using R3EHUDManager.profile.events;
using R3EHUDManager.profile.command;
using da2mvc.framework.collection.events;
using da2mvc.framework.collection.model;

namespace R3EHUDManager.profile.view
{
    class ProfileMenuMediator : BaseMediator<ProfileMenuView>
    {
        public ProfileMenuMediator(SelectedProfileModel selectedProfile)
        {
            HandleEvent< CollectionModel < ProfileModel > , CollectionEventArgs <ProfileModel>>(CollectionModel<ProfileModel>.EVENT_ITEMS_ADDED, OnProfileAdded);
            HandleEvent<CollectionModel<ProfileModel>, CollectionEventArgs<ProfileModel>>(CollectionModel<ProfileModel>.EVENT_ITEMS_REMOVED, OnProfileRemoved);

            HandleEvent<SelectedProfileModel, ProfileEventArgs>(SelectedProfileModel.EVENT_SELECTION_CHANGED, OnProfileSelected);
            HandleEvent<SelectedProfileModel, ProfileEventArgs>(SelectedProfileModel.EVENT_SELECTION_CLEARED, OnProfileUnselected);

            HandleEvent<SaveProfileCommand, ProfileEventArgs>(SaveProfileCommand.EVENT_PROFILE_CHANGES_SAVED, OnProfileSaved);
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
