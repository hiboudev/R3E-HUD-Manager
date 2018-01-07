using da2mvc.core.events;
using R3EHUDManager.huddata.events;
using R3EHUDManager.huddata.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager_wpf.application.view;
using System;
using System.Collections.Generic;

namespace R3EHUDManager_wpf.huddata.view
{
    /// <summary>
    /// Logique d'interaction pour PlaceholderBlacklistView.xaml
    /// </summary>
    public partial class PlaceholderBlacklistView : ModalWindow, IEventDispatcher
    {
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_FILTERS_CHANGED = EventId.New();

        public PlaceholderBlacklistView(PlaceholderBlackListModel blacklistModel) :base("Filtered placeholders")
        {
            InitializeComponent();
            InitializeUI(blacklistModel);
        }

        private void InitializeUI(PlaceholderBlackListModel blacklistModel)
        {
            foreach (string placeholderName in PlaceholderName.GetAll())
            {
                ListItem item = new ListItem(placeholderName, !blacklistModel.IsFiltered(placeholderName));
                list.Items.Add(item);
            }

            applyButton.Click += OnOkClicked;
        }

        private void OnOkClicked(object sender, EventArgs e)
        {
            DispatchEvent(new FiltersChangedEventArgs(EVENT_FILTERS_CHANGED, GetFilters()));
            DialogResult = true;
        }

        private Dictionary<string, bool> GetFilters()
        {
            Dictionary<string, bool> filters = new Dictionary<string, bool>();
            foreach (ListItem item in list.Items)
            {
                filters.Add(item.Name, item.IsChecked);
            }
            return filters;
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }

    class ListItem
    {
        public ListItem(string name, bool isChecked)
        {
            Name = name;
            IsChecked = isChecked;
        }

        public string Name { get; }
        public bool IsChecked { get; set; }
    }
}
