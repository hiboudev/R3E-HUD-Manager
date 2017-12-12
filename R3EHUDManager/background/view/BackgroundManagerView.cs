using da2mvc.core.events;
using R3EHUDManager.application.events;
using R3EHUDManager.background.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace R3EHUDManager.background.view
{
    class BackgroundManagerView : Form, IEventDispatcher
    {
        private ListBox list;
        private Button deleteButton;
        private Dictionary<string, int> ids = new Dictionary<string, int>();
        public event EventHandler MvcEventHandler;
        public const string EVENT_DELETE_BACKGROUND = "deleteBackground";

        public BackgroundManagerView(BackgroundCollectionModel collectionModel)
        {
            InitializeUI();

            List<string> names = new List<string>();

            foreach (var background in collectionModel.Backgrounds)
            {
                // Don't change/delete the built-in background.
                if (background.IsBuiltInt) continue;

                names.Add(background.Name);
                ids.Add(background.Name, background.Id);
            }

            names.Sort((x, y) => string.Compare(x, y));
            list.Items.AddRange(names.ToArray());
        }

        internal void RemoveBackground(BackgroundModel model)
        {
            list.Items.Remove(model.Name);
            ids.Remove(model.Name);
        }

        private void InitializeUI()
        {
            Text = "Manage backgrounds";
            StartPosition = FormStartPosition.CenterParent;

            Size = new System.Drawing.Size(300, 300);

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
