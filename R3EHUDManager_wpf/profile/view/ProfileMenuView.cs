using da2mvc.core.events;
using da2mvc.core.injection;
using R3EHUDManager.application.events;
using System;
using System.Collections.Generic;
using R3EHUDManager.profile.model;
using R3EHUDManager.background.model;
using R3EHUDManager.graphics;
using da2mvc.framework.collection.model;
using da2mvc.framework.menubutton.view;
using System.Drawing;
using System.Windows.Controls;
using System.Windows;
using R3EHUDManager_wpf.profile.view;

namespace R3EHUDManager.profile.view
{
    class ProfileMenuView : MenuButtonView<ProfileModel>
    {
        public static readonly int EVENT_CREATE_NEW_PROFILE = EventId.New();
        public static readonly int EVENT_SAVE_PROFILE = EventId.New();

        private MenuItem itemSaveProfile;
        private readonly CollectionModel<BackgroundModel> backgroundCollection;

        public ProfileMenuView(CollectionModel<BackgroundModel> backgroundCollection)
        {
            Width = 170;
            this.backgroundCollection = backgroundCollection;
        }

        protected override string Title => "Profile";

        internal bool HasSelection { get; private set; }

        protected override List<MenuItem> GetBuiltInItems()
        {
            var list = new List<MenuItem>();

            itemSaveProfile = new MenuItem()
            {
                Header = "<Save profile>",
            };
            itemSaveProfile.Click += OnSaveProfileClicked;
            itemSaveProfile.IsEnabled = false;

            MenuItem itemSaveToNewProfile = new MenuItem()
            {
                Header = "<Save to new profile>",
            };
            itemSaveToNewProfile.Click += OnSaveToNewProfileClicked;

            MenuItem manageProfiles = new MenuItem()
            {
                Header = "<Manage profiles>",
            };
            manageProfiles.Click += OnManageProfileClicked;

            list.Add(itemSaveProfile);
            list.Add(itemSaveToNewProfile);
            list.Add(manageProfiles);

            return list;
        }

        protected override MenuItem ModelToItem(ProfileModel model)
        {
            var item = base.ModelToItem(model);
            BackgroundModel background = backgroundCollection.Get(model.BackgroundId);
            item.Icon = new Image() { Source = GraphicalAsset.GetLayoutIcon(background.Layout) };
            return item;
        }

        internal void SelectProfile(ProfileModel profile)
        {
            if (SetSelectedItem(profile.Id))
            {
                HasSelection = true;
                itemSaveProfile.IsEnabled = true;
            }
            else
                HasSelection = false;

            itemSaveProfile.Header = $"<Save profile '{profile.Name}'>";
        }

        internal void UnselectProfile()
        {
            SetSelectedItem(null);
            HasSelection = false;
            itemSaveProfile.IsEnabled = false;
            itemSaveProfile.Header = "<Save profile>";
        }

        internal void UpdateProfile(ProfileModel profile)
        {
            // TODO Add update management to abstract class?
            foreach (MenuItem item in ContextMenu.Items)
            {
                if ((int)item.Tag == profile.Id)
                {
                    BackgroundModel background = backgroundCollection.Get(profile.BackgroundId);
                    item.Icon = new Image() { Source = GraphicalAsset.GetLayoutIcon(background.Layout) };
                    break;
                }
            }
        }

        internal void SetSaveStatus(bool isSaved)
        {
            if (isSaved)
            {
                FontStyle = AppFontStyles.SAVED_FONT_STYLE;
                itemSaveProfile.FontStyle = AppFontStyles.SAVED_FONT_STYLE;
                itemSaveProfile.IsEnabled = false;
            }
            else
            {
                FontStyle = AppFontStyles.UNSAVED_FONT_STYLE;
                itemSaveProfile.FontStyle = AppFontStyles.UNSAVED_FONT_STYLE;
                itemSaveProfile.IsEnabled = true;
            }
        }

        private void OnSaveToNewProfileClicked(object sender, EventArgs e)
        {
            var newProfileDialog = Injector.GetInstance<ProfileCreationView>();

            if (newProfileDialog.ShowDialog() == true)
            {
                DispatchEvent(new StringEventArgs(EVENT_CREATE_NEW_PROFILE, newProfileDialog.ProfileName));
            }

            //newProfileDialog.Dispose();
        }

        private void OnSaveProfileClicked(object sender, EventArgs e)
        {
            DispatchEvent(new BaseEventArgs(EVENT_SAVE_PROFILE));
        }

        private void OnManageProfileClicked(object sender, EventArgs e)
        {
            var profileDialog = Injector.GetInstance<ProfileManagerView>();

            profileDialog.ShowDialog();

            profileDialog.Dispose();
        }
    }
}
