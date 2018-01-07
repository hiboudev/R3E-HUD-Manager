using da2mvc.core.events;
using da2mvc.core.view;
using R3EHUDManager.application.events;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
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

namespace R3EHUDManager_wpf.screen.view
{
    /// <summary>
    /// Logique d'interaction pour ZoomView.xaml
    /// </summary>
    public partial class ZoomView : UserControl, IEventDispatcher, IView
    {
        public event EventHandler MvcEventHandler;
        public event EventHandler Disposed;

        public static readonly int EVENT_ZOOM_LEVEL_CHANGED = EventId.New();
        private bool holdChangeEvents = false;

        public ZoomView()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            fitWindow.Checked += CheckedChange;
            fitHeight.Checked += CheckedChange;
        }

        internal void SetZoomLevel(ZoomLevel zoomLevel)
        {
            holdChangeEvents = true;

            if (zoomLevel == ZoomLevel.FIT_TO_HEIGHT)
                fitHeight.IsChecked = true;
            else
                fitWindow.IsChecked = true;

            holdChangeEvents = false;
        }

        private void CheckedChange(object sender, RoutedEventArgs e)
        {
            if (holdChangeEvents || !((RadioButton)sender).IsChecked == true) return;

            if (fitWindow.IsChecked == true)
                DispatchEvent(new IntEventArgs(EVENT_ZOOM_LEVEL_CHANGED, (int)ZoomLevel.FIT_TO_WINDOW));
            else
                DispatchEvent(new IntEventArgs(EVENT_ZOOM_LEVEL_CHANGED, (int)ZoomLevel.FIT_TO_HEIGHT));
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
