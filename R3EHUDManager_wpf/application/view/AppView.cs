using da2mvc.core.injection;
using R3EHUDManager_wpf.utils;
using System;
using System.Windows;
using System.Windows.Controls;

namespace R3EHUDManager_wpf.application.view
{
    public class AppView : DockPanel
    {
        private Type viewType;

        public AppView()
        {
            DataContext = this;
        }

        public Type ViewType
        {
            get
            {
                return viewType;
            }
            set
            {
                if (viewType != null)
                    throw new Exception("View is already set.");

                viewType = value;

                if (WpfUtils.IsInDesignMode()) Mappings.Initialize();

                Children.Add((UIElement)Injector.GetInstance(viewType));
            }
        }
    }
}
