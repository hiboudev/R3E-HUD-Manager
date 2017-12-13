using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using R3EHUDManager.placeholder.model;
using System.Diagnostics;
using da2mvc.core.events;
using R3EHUDManager.application.events;
using System.Drawing;
using System.Reflection;
using da2mvc.framework.view;

namespace R3EHUDManager.selection.view
{
    class PlaceholdersListView : Panel, IEventDispatcher, ICollectionView<PlaceholderModel>
    {
        private ListBox list;
        private bool bypassSelectedEvent = false;
        public event EventHandler MvcEventHandler;
        public const string EVENT_PLACEHOLDER_SELECTED = "placeholderSelected";


        public PlaceholdersListView()
        {
            InitializeUI();
        }

        public void Add(PlaceholderModel[] models)
        {
            foreach (PlaceholderModel model in models.OrderBy(x => x.Name))
                list.Items.Add(model.Name);
        }

        public void Remove(PlaceholderModel[] models)
        {
            foreach (PlaceholderModel model in models)
                list.Items.Remove(model.Name);
        }

        public void Clear()
        {
            list.Items.Clear();
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

            DispatchEvent(new StringEventArgs(EVENT_PLACEHOLDER_SELECTED, list.SelectedItem.ToString()));
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
