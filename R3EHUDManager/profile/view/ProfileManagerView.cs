using da2mvc.core.events;
using da2mvc.framework.collection.view;
using R3EHUDManager.application.events;
using R3EHUDManager.application.view;
using R3EHUDManager.profile.model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace R3EHUDManager.profile.view
{
    class ProfileManagerView : BaseModalForm, IEventDispatcher, ICollectionView<ProfileModel>
    {
        private ListBox list;
        private Button deleteButton;
        private Dictionary<string, int> ids = new Dictionary<string, int>();
        List<string> names = new List<string>();
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_DELETE_PROFILE = EventId.New();

        public ProfileManagerView():base("Manage profiles")
        {
            InitializeUI();
        }

        public void Add(ProfileModel[] models)
        {
            foreach (ProfileModel profile in models)
            {
                names.Add(profile.Name);
                ids.Add(profile.Name, profile.Id);
            }

            names.Sort((x, y) => string.Compare(x, y));
            list.Items.AddRange(names.ToArray());
        }

        public void Remove(ProfileModel[] models)
        {
            foreach (ProfileModel profile in models)
            {
                list.Items.Remove(profile.Name);
                ids.Remove(profile.Name);
                list.Items.Remove(profile.Name);
            }
        }

        public void Clear()
        {
            list.Items.Clear();
            ids.Clear();
            list.Items.Clear();
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
