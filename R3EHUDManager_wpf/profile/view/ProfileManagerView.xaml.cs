using da2mvc.core.events;
using da2mvc.framework.application.view;
using da2mvc.framework.collection.view;
using R3EHUDManager.application.events;
using R3EHUDManager.profile.model;
using R3EHUDManager_wpf.application.view;
using System;
using System.Collections.Generic;

namespace R3EHUDManager_wpf.profile.view
{
    /// <summary>
    /// Logique d'interaction pour ProfileManagerView.xaml
    /// </summary>
    public partial class ProfileManagerView : ModalWindow, IEventDispatcher, ICollectionView<ProfileModel>
    {
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_DELETE_PROFILE = EventId.New();
        public event EventHandler Disposed;

        private Dictionary<string, int> ids = new Dictionary<string, int>();
        List<string> names = new List<string>();

        public ProfileManagerView()
        {
            InitializeComponent();
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
            foreach (string name in names)
            {
                list.Items.Add(name);
            }
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
            button.IsEnabled = false;
            list.SelectionChanged += OnSelectionChanged;
            button.Click += OnDeleteClicked;
        }

        private void OnDeleteClicked(object sender, EventArgs e)
        {
            DispatchEvent(new IntEventArgs(EVENT_DELETE_PROFILE, ids[list.SelectedItem.ToString()]));
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
