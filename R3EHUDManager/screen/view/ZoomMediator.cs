using da2mvc.view;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using da2mvc.events;
using R3EHUDManager.screen.events;

namespace R3EHUDManager.screen.view
{
    class ZoomMediator : BaseMediator
    {
        private ScreenModel screenModel;

        public ZoomMediator(ScreenModel screenModel)
        {
            RegisterEventListener(typeof(ScreenModel), ScreenModel.EVENT_ZOOM_LEVEL_CHANGED, OnZoomLevelChanged);
            this.screenModel = screenModel;
        }

        private void OnZoomLevelChanged(BaseEventArgs args)
        {
            ((ZoomView)View).SetZoomLevel(((ScreenModelEventArgs)args).ScreenModel.ZoomLevel);
        }

        protected override void ViewInitialized()
        {
            ((ZoomView)View).SetZoomLevel(screenModel.ZoomLevel);
            screenModel = null;
        }
    }
}
