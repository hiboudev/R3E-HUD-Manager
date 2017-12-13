using da2mvc.core.view;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.core.events;
using R3EHUDManager.screen.events;

namespace R3EHUDManager.screen.view
{
    class ZoomMediator : BaseMediator<ZoomView>
    {
        private ScreenModel screenModel;

        public ZoomMediator(ScreenModel screenModel)
        {
            HandleEvent<ScreenModel, ScreenModelEventArgs>(ScreenModel.EVENT_ZOOM_LEVEL_CHANGED, OnZoomLevelChanged);
            this.screenModel = screenModel;
        }

        private void OnZoomLevelChanged(ScreenModelEventArgs args)
        {
            View.SetZoomLevel(args.ScreenModel.ZoomLevel);
        }

        protected override void ViewInitialized()
        {
            View.SetZoomLevel(screenModel.ZoomLevel);
            screenModel = null;
        }
    }
}
