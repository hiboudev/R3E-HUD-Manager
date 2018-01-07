using da2mvc.core.events;
using da2mvc.framework.collection.view;
using R3EHUDManager.application.events;
using R3EHUDManager.background.model;
using R3EHUDManager_wpf.application.view;
using System;
using System.Collections.Generic;

namespace R3EHUDManager_wpf.background.view
{
    /// <summary>
    /// Logique d'interaction pour BackgroundManagerView.xaml
    /// </summary>
    public partial class BackgroundManagerView : ModalWindow, IEventDispatcher, ICollectionView<BackgroundModel>
    {
        public event EventHandler MvcEventHandler;
        public event EventHandler Disposed;

        public static readonly int EVENT_DELETE_BACKGROUND = EventId.New();
        List<string> names = new List<string>();
        private Dictionary<string, int> ids = new Dictionary<string, int>();

        public BackgroundManagerView() : base("Manage backgrounds")
        {
            InitializeComponent();
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
            foreach (string name in names)
            {
                list.Items.Add(name);
            }
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
            button.IsEnabled = false;
            button.Click += OnDeleteClicked;
            list.SelectionChanged += OnSelectionChanged;
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            DispatchEvent(new IntEventArgs(EVENT_DELETE_BACKGROUND, ids[list.SelectedItem.ToString()]));
        }

        private void OnSelectionChanged(object sender, EventArgs e)
        {
            button.IsEnabled = list.SelectedItem != null;
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
