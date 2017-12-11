﻿using da2mvc.events;
using da2mvc.injection;
using R3EHUDManager.application.events;
using R3EHUDManager.contextmenu.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using R3EHUDManager.profile.model;
using R3EHUDManager.background.model;
using R3EHUDManager.graphics;

namespace R3EHUDManager.profile.view
{
    class ProfileMenuView : AbstractContextMenuView
    {
        public const string EVENT_CREATE_NEW_PROFILE = "createNewProfile";
        public const string EVENT_SAVE_PROFILE = "saveProfile";

        private ToolStripMenuItem itemSaveProfile;
        private readonly BackgroundCollectionModel backgroundCollection;

        public ProfileMenuView(BackgroundCollectionModel backgroundCollection) : base("Profile")
        {
            Width = 170;
            this.backgroundCollection = backgroundCollection;
        }

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

        internal void AddProfiles(ProfileModel[] profiles)
        {
            List<ContextMenuViewItem> items = new List<ContextMenuViewItem>();

            foreach (ProfileModel profile in profiles)
            {
                BackgroundModel background = backgroundCollection.Get(profile.BackgroundId);
                items.Add(new ContextMenuViewItem(profile.Id, profile.Name, GraphicalAsset.GetLayoutIcon(background.Layout)));
            }

            AddItems(items);
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
            var newProfileDialog = (PromptNewProfileView)Injector.GetInstance(typeof(PromptNewProfileView));

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
            var profileDialog = (ProfileManagerView)Injector.GetInstance(typeof(ProfileManagerView));

            if (profileDialog.ShowDialog() == DialogResult.OK)
            {
            }

            profileDialog.Dispose();
        }
    }
}
