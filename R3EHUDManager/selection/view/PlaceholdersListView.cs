using System;
using System.Linq;
using System.Windows.Forms;
using R3EHUDManager.placeholder.model;
using da2mvc.core.events;
using R3EHUDManager.application.events;
using System.Drawing;
using da2mvc.framework.collection.view;
using System.Collections.Generic;

namespace R3EHUDManager.selection.view
{
    class PlaceholdersListView : Panel, IEventDispatcher, ICollectionView<PlaceholderModel>
    {
        private ListBox list;
        private bool bypassSelectedEvent = false;
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_PLACEHOLDER_SELECTED = EventId.New();
        private Dictionary<string, int> ids = new Dictionary<string, int>();

        public PlaceholdersListView()
        {
            InitializeUI();
        }

        public void Add(PlaceholderModel[] models)
        {
            foreach (PlaceholderModel model in models.OrderBy(x => x.Name))
            {
                list.Items.Add(model.Name);
                ids.Add(model.Name, model.Id);
            }
        }

        public void Remove(PlaceholderModel[] models)
        {
            foreach (PlaceholderModel model in models)
            {
                list.Items.Remove(model.Name);
                ids.Remove(model.Name);
            }
        }

        public void Clear()
        {
            list.Items.Clear();
            ids.Clear();
        }

        internal void SelectPlaceholder(string name)
        {
            SelectListItem(name);
        }

        internal void UnselectPlaceholder()
        {
            SelectListItem(null);
        }

        private void InitializeUI()
        {
            Label title = new Label()
            {
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Height = 15,
                Text = "Placeholders",
                Dock = DockStyle.Top,
                Margin = new Padding(Margin.Left, Margin.Top, Margin.Right, 4),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.LightGray,
            };

            list = new ListBox
            {
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill,
                BackColor = Color.WhiteSmoke,
            };

            list.SelectedIndexChanged += OnSelectedIndexChanged;

            Controls.Add(list);
            Controls.Add(title);
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (bypassSelectedEvent) return;

            DispatchEvent(new IntEventArgs(EVENT_PLACEHOLDER_SELECTED, ids[list.SelectedItem.ToString()]));
        }

        private void SelectListItem(string name)
        {
            bypassSelectedEvent = true;
            list.SelectedItem = name;
            bypassSelectedEvent = false;
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
