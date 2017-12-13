﻿using da2mvc.core.events;
using da2mvc.framework.model;
using R3EHUDManager.application.events;
using R3EHUDManager.application.view;
using R3EHUDManager.profile.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.profile.view
{
    class ProfileManagerView : BaseModalForm, IEventDispatcher
    {
        private ListBox list;
        private Button deleteButton;
        private Dictionary<string, int> ids = new Dictionary<string, int>();
        public event EventHandler MvcEventHandler;
        public const string EVENT_DELETE_PROFILE = "deleteProfile";

        public ProfileManagerView(CollectionModel<ProfileModel> collectionModel):base("Manage profiles")
        {
            InitializeUI();

            List<string> names = new List<string>();

            foreach (ProfileModel profile in collectionModel.Items)
            {
                list.Items.Add(profile.Name);
                ids.Add(profile.Name, profile.Id);
            }

            names.Sort((x, y) => string.Compare(x, y));
            list.Items.AddRange(names.ToArray());
        }

        internal void RemoveProfile(ProfileModel model)
        {
            list.Items.Remove(model.Name);
            ids.Remove(model.Name);
        }

        private void InitializeUI()
        {
            Size = new System.Drawing.Size(300, 300);
            Padding = new Padding(0);

            list = new ListBox()
            {
                Dock = DockStyle.Fill,
            };
            list.SelectedIndexChanged += OnSelectionChanged;

            deleteButton = new Button()
            {
                Text = "Delete profile",
                Dock = DockStyle.Bottom,
                Enabled = false,
            };
            deleteButton.Click += OnDeleteClicked;

            Controls.Add(list);
            Controls.Add(deleteButton);
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            DispatchEvent(new IntEventArgs(EVENT_DELETE_PROFILE, ids[list.SelectedItem.ToString()]));
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            deleteButton.Enabled = list.SelectedItem != null;
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
