using da2mvc.core.events;
using da2mvc.core.injection;
using R3EHUDManager.application.events;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using R3EHUDManager.profile.model;
using R3EHUDManager.background.model;
using R3EHUDManager.graphics;
using da2mvc.framework.collection.model;
using da2mvc.framework.menubutton.view;

namespace R3EHUDManager.profile.view
{
    class ProfileMenuView : MenuButtonView<ProfileModel>
    {
        public const string EVENT_CREATE_NEW_PROFILE = "createNewProfile";
        public const string EVENT_SAVE_PROFILE = "saveProfile";

        private ToolStripMenuItem itemSaveProfile;
        private readonly CollectionModel<BackgroundModel> backgroundCollection;

        public ProfileMenuView(CollectionModel<BackgroundModel> backgroundCollection)
        {
            Width = 170;
            this.backgroundCollection = backgroundCollection;
        }

        protected override string Title => "Profile";

        protected override List<ToolStripMenuItem> GetBuiltInItems()
        {
            var list = new List<ToolStripMenuItem>();

            itemSaveProfile = new ToolStripMenuItem("<Save profile>");
            itemSaveProfile.Click += OnSaveProfileClicked;
            itemSaveProfile.Enabled = false;

            ToolStripMenuItem itemSaveToNewProfile = new ToolStripMenuItem("<Save to new profile>");
            itemSaveToNewProfile.Click += OnSaveToNewProfileClicked;

            ToolStripMenuItem manageProfiles = new ToolStripMenuItem("<Manage profiles>");
            manageProfiles.Click += OnManageProfileClicked;

            list.Add(itemSaveProfile);
            list.Add(itemSaveToNewProfile);
            list.Add(manageProfiles);

            return list;
        }

        protected override ToolStripMenuItem ModelToItem(ProfileModel model)
        {
            var item = base.ModelToItem(model);
            BackgroundModel background = backgroundCollection.Get(model.BackgroundId);
            item.Image = GraphicalAsset.GetLayoutIcon(background.Layout);
            return item;
        }

        internal void SelectProfile(ProfileModel profile)
        {
            if(SetSelectedItem(profile.Id))
                itemSaveProfile.Enabled = true;

            itemSaveProfile.Text = $"<Save profile '{profile.Name}'>";
        }

        internal void UnselectProfile()
        {
            SetSelectedItem(null);
            itemSaveProfile.Enabled = false;
            itemSaveProfile.Text = "<Save profile>";
        }

        internal void UpdateProfile(ProfileModel profile)
        {
            // TODO Add update management to abstract class?
            foreach(ToolStripMenuItem item in ContextMenuStrip.Items)
            {
                if ((int)item.Tag == profile.Id)
                {
                    BackgroundModel background = backgroundCollection.Get(profile.BackgroundId);
                    item.Image = GraphicalAsset.GetLayoutIcon(background.Layout);
                    break;
                }
            }
        }

        private void OnSaveToNewProfileClicked(object sender, EventArgs e)
        {
            var newProfileDialog = Injector.GetInstance<PromptNewProfileView>();

            if(newProfileDialog.ShowDialog() == DialogResult.OK)
            {
                DispatchEvent(new StringEventArgs(EVENT_CREATE_NEW_PROFILE, newProfileDialog.ProfileName));
            }

            newProfileDialog.Dispose();
        }

        private void OnSaveProfileClicked(object sender, EventArgs e)
        {
            DispatchEvent(new BaseEventArgs(EVENT_SAVE_PROFILE));
        }

        private void OnManageProfileClicked(object sender, EventArgs e)
        {
            var profileDialog = Injector.GetInstance<ProfileManagerView>();

            if (profileDialog.ShowDialog() == DialogResult.OK)
            {
            }

            profileDialog.Dispose();
        }
    }
}
