using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using R3EHUDManager.placeholder.model;
using System.Diagnostics;
using da2mvc.events;
using R3EHUDManager.application.events;

namespace R3EHUDManager.selection.view
{
    class PlaceholdersListView : Panel, IEventDispatcher
    {
        private ListBox list;
        private bool bypassSelectedEvent = false;
        public event EventHandler MvcEventHandler;
        public const string EVENT_PLACEHOLDER_SELECTED = "placeholderSelected";


        public PlaceholdersListView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            list = new ListBox
            {
                BorderStyle = BorderStyle.None,
                Dock = DockStyle.Fill
            };

            list.SelectedIndexChanged += OnSelectedIndexChanged;

            Controls.Add(list);
        }

        private void OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (bypassSelectedEvent) return;

            DispatchEvent(new StringEventArgs(EVENT_PLACEHOLDER_SELECTED, list.SelectedItem.ToString()));
        }

        internal void SetPlaceholders(List<PlaceholderModel> placeHolders)
        {
            list.Items.Clear();

            foreach(PlaceholderModel model in placeHolders)
            {
                list.Items.Add(model.Name);
            }
        }

        internal void SelectPlaceholder(string name)
        {
            SelectListItem(name);
        }

        internal void UnselectPlaceholder()
        {
            SelectListItem(null);
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
