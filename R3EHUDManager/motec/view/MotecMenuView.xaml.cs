using da2mvc.core.events;
using da2mvc.framework.collection.view;
using R3EHUDManager.application.events;
using R3EHUDManager.motec.model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace R3EHUDManager.motec.view
{
    /// <summary>
    /// Logique d'interaction pour MotecListView.xaml
    /// </summary>
    public partial class MotecMenuView : UserControl, ICollectionView<MotecModel>, IEventDispatcher
    {
        public event EventHandler Disposed;
        public event EventHandler MvcEventHandler;
        public static readonly int EVENT_MOTEC_SELECTED = EventId.New();

        public MotecMenuView()
        {
            InitializeComponent();
            InitializeUI();
        }

        public void Add(MotecModel[] models)
        {
            foreach(var model in models)
            {
                list.Items.Add(model);
            }
        }

        public void Remove(MotecModel[] models)
        {
            foreach (var model in models)
            {
                list.Items.Remove(model);
            }
        }

        public void Clear()
        {
            list.Items.Clear();
        }

        public void SelectMotec(int value)
        {
            foreach(MotecModel motec in list.Items)
            {
                if (motec.Id == value)
                {
                    list.SelectedItem = motec;
                    break;
                }
            }
        }

        private void InitializeUI()
        {
            list.SelectionChanged += OnSelectionChanged;
        }

        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            MotecModel motec = (MotecModel)list.SelectedItem;
            DispatchEvent(new IntEventArgs(EVENT_MOTEC_SELECTED, motec.Id));
        }

        private void OnRequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            //Allows the keyboard to bring the items into view as expected:
            if (Keyboard.IsKeyDown(Key.Down) || Keyboard.IsKeyDown(Key.Up))
                return;
            if (((ComboBoxItem)e.TargetObject).Content == list.SelectedItem)
                return;

            e.Handled = true;
        }

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }

        public void DispatchEvent(BaseEventArgs args)
        {
            MvcEventHandler?.Invoke(this, args);
        }
    }
}
