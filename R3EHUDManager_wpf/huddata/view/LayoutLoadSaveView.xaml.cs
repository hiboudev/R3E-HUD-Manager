using da2mvc.core.events;
using da2mvc.core.view;
using R3EHUDManager.graphics;
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

namespace R3EHUDManager.huddata.view
{
    /// <summary>
    /// Logique d'interaction pour LayoutLoadSaveView.xaml
    /// </summary>
    public partial class LayoutLoadSaveView : UserControl, IView, IEventDispatcher
    {
        public event EventHandler Disposed;
        public event EventHandler MvcEventHandler;

        public static readonly int EVENT_SAVE_CLICKED = EventId.New();
        public static readonly int EVENT_RELOAD_CLICKED = EventId.New();
        public static readonly int EVENT_RELOAD_DEFAULT_CLICKED = EventId.New();

        public LayoutLoadSaveView()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            apply.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_SAVE_CLICKED));
            reloadR3e.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_RELOAD_CLICKED));
            reloadOriginal.Click += (sender, args) => DispatchEvent(new BaseEventArgs(EVENT_RELOAD_DEFAULT_CLICKED));
        }

        internal void SetSaveStatus(bool isSaved)
        {
            if (isSaved)
                apply.FontStyle = AppFontStyles.SAVED_FONT_STYLE;
            else
                apply.FontStyle = AppFontStyles.UNSAVED_FONT_STYLE;
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
