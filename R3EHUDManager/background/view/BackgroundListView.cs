using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using R3EHUDManager.background.model;
using da2mvc.events;
using R3EHUDManager.application.events;

namespace R3EHUDManager.background.view
{
    class BackgroundListView : Panel, IEventDispatcher
    {
        private ComboBox list;
        private Dictionary<string, int> ids = new Dictionary<string, int>();

        public event EventHandler MvcEventHandler;
        public const string EVENT_BACKGROUND_SELECTED = "backgroundSelected";

        public BackgroundListView()
        {
            InitializeUI();
        }

        private void InitializeUI()
        {
            AutoSize = true;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;

            list = new ComboBox()
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
            };

            list.SelectionChangeCommitted += OnSelectionChanged;

            Controls.Add(list);
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            int id = ids[list.SelectedItem.ToString()];
            DispatchEvent(new IntEventArgs(EVENT_BACKGROUND_SELECTED, id));
        }

        internal void AddBackground(BackgroundModel background)
        {
            list.Items.Add(background.Name);
            ids.Add(background.Name, background.Id);
        }

        internal void SetBackgrounds(List<BackgroundModel> backgrounds)
        {
            list.Items.Clear();
            ids.Clear();

            foreach(BackgroundModel background in backgrounds)
                AddBackground(background);
        }

        internal void SelectBackground(BackgroundModel selection)
        {
            list.SelectedItem = selection.Name;
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
