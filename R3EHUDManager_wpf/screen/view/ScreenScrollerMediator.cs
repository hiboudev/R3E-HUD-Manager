using da2mvc.core.view;
using R3EHUDManager.screen.events;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager_wpf.screen.view
{
    class ScreenScrollerMediator : BaseMediator<ScreenScrollerView>
    {
        public ScreenScrollerMediator()
        {
            HandleEvent<ScreenModel, ScreenModelEventArgs>(ScreenModel.EVENT_ZOOM_LEVEL_CHANGED, OnZoomLevelChanged);
        }

        private void OnZoomLevelChanged(ScreenModelEventArgs args)
        {
            View.SetZoomLevel(args.ScreenModel.ZoomLevel);
        }
    }
}
