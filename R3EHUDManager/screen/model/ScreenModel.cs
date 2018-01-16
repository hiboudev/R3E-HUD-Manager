using da2mvc.core.events;
using R3EHUDManager.background.model;
using R3EHUDManager.graphics;
using R3EHUDManager.location.model;
using R3EHUDManager.screen.events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace R3EHUDManager.screen.model
{
    public class ScreenModel : EventDispatcher
    {
        public static readonly int EVENT_BACKGROUND_CHANGED = EventId.New();
        public static readonly int EVENT_ZOOM_LEVEL_CHANGED = EventId.New();

        public BackgroundModel Background { get; private set; }
        public ZoomLevel ZoomLevel { get; private set; } = ZoomLevel.FIT_TO_WINDOW;

        private BitmapSource bitmap;
        private LocationModel locationModel;
        private readonly GraphicalAssetFactory assetsFactory;

        public ScreenLayoutType Layout
        {
            get
            {
                if (Background != null) return Background.Layout;
                return ScreenLayoutType.SINGLE;
            }
        }

        public ScreenModel(LocationModel locationModel, GraphicalAssetFactory assetsFactory)
        {
            this.locationModel = locationModel;
            this.assetsFactory = assetsFactory;
        }

        public void SetBackground(BackgroundModel background)
        {
            Background = background;

            string dirPath = locationModel.GetGraphicBasePath(background.DirectoryType);

            bitmap = assetsFactory.GetNoCache(Path.Combine(dirPath, Background.FileName));

            DispatchEvent(new ScreenModelEventArgs(EVENT_BACKGROUND_CHANGED, this));
        }

        public void SetZoomLevel(ZoomLevel zoomLevel)
        {
            ZoomLevel = zoomLevel;
            DispatchEvent(new ScreenModelEventArgs(EVENT_ZOOM_LEVEL_CHANGED, this));
        }

        public BitmapSource GetBackgroundImage()
        {
            return bitmap;
        }
    }
}
