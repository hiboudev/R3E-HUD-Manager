using da2MVC.events;
using R3EHUDManager.background.model;
using R3EHUDManager.graphics;
using R3EHUDManager.location.model;
using R3EHUDManager.screen.events;
using R3EHUDManager.screen.view;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.screen.model
{
    class ScreenModel: EventDispatcher
    {
        public BackgroundModel Background { get; private set; }

        //public Size Dimension { get; private set; }
        //public decimal AspectRatio { get; private set; }
        public ScreenLayoutType Layout {
            get {
                if (overridedLayout) return overrideLayout;
                if (Background != null) return Background.Layout;
                return ScreenLayoutType.SINGLE;
            }
        }
        public ZoomLevel ZoomLevel { get; private set; } = ZoomLevel.FIT_TO_WINDOW;

        public const string EVENT_BACKGROUND_CHANGED = "backgroundChanged";
        public const string EVENT_TRIPLE_SCREEN_CHANGED = "tripleScreenChanged";
        public const string EVENT_ZOOM_LEVEL_CHANGED = "zoomLevelChanged";

        private Image bitmap;
        private LocationModel locationModel;
        private bool overridedLayout = false;
        private ScreenLayoutType overrideLayout;

        public ScreenModel(LocationModel locationModel)
        {
            this.locationModel = locationModel;
        }

        public void SetBackground(BackgroundModel background)
        {
            overridedLayout = false;

            Background = background;

            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }

            string dirPath = locationModel.GetGraphicBasePath(background.DirectoryType);

            bitmap = GraphicalAsset.GetNoCache(Path.Combine(dirPath, Background.FileName));
            //Dimension = bitmap.PhysicalDimension.ToSize();
            //UpdateAspectRatio();

            DispatchEvent(new ScreenModelEventArgs(EVENT_BACKGROUND_CHANGED, this));
        }

        public void SetLayout(ScreenLayoutType layout)
        {
            overrideLayout = layout;
            overridedLayout = true;

            //UpdateAspectRatio();

            DispatchEvent(new ScreenModelEventArgs(EVENT_TRIPLE_SCREEN_CHANGED, this));
        }

        public void SetZoomLevel(ZoomLevel zoomLevel)
        {
            ZoomLevel = zoomLevel;
            DispatchEvent(new ScreenModelEventArgs(EVENT_ZOOM_LEVEL_CHANGED, this));
        }

        //private void UpdateAspectRatio()
        //{
        //    // TODO Manage screens with different resolutions
        //    if (Layout == ScreenLayoutType.TRIPLE)
        //        AspectRatio = ((decimal)Dimension.Width / Dimension.Height) / 3;
        //    else
        //        AspectRatio = (decimal)Dimension.Width / Dimension.Height;
        //}

        public Image GetBackgroundImage()
        {
            return bitmap;
        }
    }
}
