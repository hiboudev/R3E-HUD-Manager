using da2mvc.core.events;
using da2mvc.framework.collection.view;
using R3EHUDManager.application.events;
using R3EHUDManager.application.view;
using R3EHUDManager.background.model;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace R3EHUDManager.background.view
{
    class BackgroundManagerView : BaseModalForm, IEventDispatcher, ICollectionView<BackgroundModel>
    {
        private ListBox list;
        private Button deleteButton;
        public event EventHandler MvcEventHandler;
        public const string EVENT_DELETE_BACKGROUND = "deleteBackground";
        List<string> names = new List<string>();
        private Dictionary<string, int> ids = new Dictionary<string, int>();

        public BackgroundManagerView():base("Manage backgrounds")
        {
            InitializeUI();
        }

        public void Add(BackgroundModel[] models)
        {
            foreach (var background in models)
            {
                // Don't change/delete the built-in background.
                if (background.IsBuiltInt) continue;

                names.Add(background.Name);
                ids.Add(background.Name, background.Id);
            }

            names.Sort((x, y) => string.Compare(x, y));
            list.Items.AddRange(names.ToArray());
        }

        public void Remove(BackgroundModel[] models)
        {
            foreach (var background in models)
            {
                if (background.IsBuiltInt) continue;

                names.Remove(background.Name);
                ids.Remove(background.Name);
                list.Items.Remove(background.Name);
            }
        }

        public void Clear()
        {
            names.Clear();
            ids.Clear();
            list.Items.Clear();
        }

        internal void RemoveBackground(BackgroundModel model)
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
                Text = "Delete background",
                Dock = DockStyle.Bottom,
                Enabled = false,
            };
            deleteButton.Click += OnDeleteClicked;

            Controls.Add(list);
            Controls.Add(deleteButton);
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            DispatchEvent(new IntEventArgs(EVENT_DELETE_BACKGROUND, ids[list.SelectedItem.ToString()]));
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
