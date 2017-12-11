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
using R3EHUDManager.profile.command;

namespace R3EHUDManager.profile.view
{
    class ProfileMenuMediator : BaseMediator
    {
        public ProfileMenuMediator(SelectedProfileModel selectedProfile)
        {
            RegisterEventListener(typeof(ProfileCollectionModel), ProfileCollectionModel.EVENT_PROFILE_ADDED, OnProfileAdded);
            RegisterEventListener(typeof(ProfileCollectionModel), ProfileCollectionModel.EVENT_PROFILE_REMOVED, OnProfileRemoved);

            RegisterEventListener(typeof(SelectedProfileModel), SelectedProfileModel.EVENT_SELECTION_CHANGED, OnProfileSelected);
            RegisterEventListener(typeof(SelectedProfileModel), SelectedProfileModel.EVENT_SELECTION_CLEARED, OnProfileUnselected);

            RegisterEventListener(typeof(SaveProfileCommand), SaveProfileCommand.EVENT_PROFILE_CHANGES_SAVED, OnProfileSaved);
        }

        private void OnProfileRemoved(BaseEventArgs args)
        {
            ((ProfileMenuView)View).RemoveItem(((ProfileEventArgs)args).Profile.Id);
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

        private void OnProfileAdded(BaseEventArgs args)
        {
            ((ProfileMenuView)View).AddProfiles(((ProfileCollectionEventArgs)args).ModifiedProfiles);
        }
    }
}
