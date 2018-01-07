using da2mvc.core.view;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Logique d'interaction pour ScreenScrollerView.xaml
    /// </summary>
    public partial class ScreenScrollerView : UserControl, IView
    {
        private readonly ScreenView screenView;
        private bool holdScrollEvent;
        public event EventHandler Disposed;

        public ScreenScrollerView(ScreenView screenView)
        {
            this.screenView = screenView;
            InitializeComponent();
            InitializeUI();
        }

        internal void SetZoomLevel(ZoomLevel zoomLevel)
        {
            switch (zoomLevel)
            {
                case ZoomLevel.FIT_TO_WINDOW:
                    scrollBar.Scroll -= OnScrollBarScroll;
                    screenView.ScreenAreaChanged -= OnScreenAreaChanged;
                    SizeChanged -= OnSizeChanged;
                    scrollBar.Visibility = Visibility.Collapsed;
                    break;

                case ZoomLevel.FIT_TO_HEIGHT:
                    scrollBar.Scroll += OnScrollBarScroll;
                    screenView.ScreenAreaChanged += OnScreenAreaChanged;
                    SizeChanged += OnSizeChanged;
                    scrollBar.Visibility = Visibility.Visible;
                    scrollBar.UpdateLayout();
                    UpdateScrollBar();
                    break;
            }
        }

        private void InitializeUI()
        {
            scrollBar.Visibility = Visibility.Collapsed;
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateScrollBar();
        }

        private void OnScreenAreaChanged(object sender, EventArgs e)
        {
            UpdateScrollBar();
        }

        private void UpdateScrollBar()
        {
            if (RenderSize.Width == 0 || screenView == null) return;

            holdScrollEvent = true;

            scrollBar.Minimum = 0;
            scrollBar.Maximum = (screenView.ScreenArea.Width + 2 * ScreenView.MARGIN) - RenderSize.Width;
            scrollBar.Track.ViewportSize = (screenView.ScreenArea.Width + 2 * ScreenView.MARGIN);

            holdScrollEvent = false;

            scrollBar.Visibility = scrollBar.IsEnabled ? Visibility.Visible : Visibility.Hidden;
        }

        private void OnScrollBarScroll(object sender, ScrollEventArgs e)
        {
            if (holdScrollEvent) return;

            screenView.Scroll(e.NewValue);
        }

        public void Dispose()
        {
            Disposed?.Invoke(this, EventArgs.Empty);
        }
    }
}
