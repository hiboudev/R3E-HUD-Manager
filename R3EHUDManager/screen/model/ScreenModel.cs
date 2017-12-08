using da2MVC.events;
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

namespace R3EHUDManager.screen.model
{
    class ScreenModel: EventDispatcher
    {
        public bool IsTripleScreen { get; private set; }
        public BackgroundModel Background { get; private set; }

        public Size Dimension { get; private set; }
        public decimal AspectRatio { get; private set; }

        public const string EVENT_BACKGROUND_CHANGED = "backgroundChanged";
        public const string EVENT_TRIPLE_SCREEN_CHANGED = "tripleScreenChanged";

        private Image bitmap;
        private LocationModel locationModel;

        public ScreenModel(LocationModel locationModel)
        {
            this.locationModel = locationModel;
        }

        public void SetBackground(BackgroundModel background)
        {
            Background = background;

            if (bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }

            string dirPath = locationModel.GetGraphicBasePath(background.DirectoryType);

            bitmap = GraphicalAsset.GetNoCache(Path.Combine(dirPath, Background.FileName));
            Dimension = bitmap.PhysicalDimension.ToSize();
            UpdateAspectRatio();

            DispatchEvent(new ScreenModelEventArgs(EVENT_BACKGROUND_CHANGED, this));
        }

        public void SetTripleScreen(bool value)
        {
            IsTripleScreen = value;

            UpdateAspectRatio();

            DispatchEvent(new ScreenModelEventArgs(EVENT_TRIPLE_SCREEN_CHANGED, this));
        }

        private void UpdateAspectRatio()
        {
            // TODO Manage screens with different resolutions
            if (IsTripleScreen)
                AspectRatio = ((decimal)Dimension.Width / Dimension.Height) / 3;
            else
                AspectRatio = (decimal)Dimension.Width / Dimension.Height;
        }

        public Image GetBackgroundImage()
        {
            // TODO error management loading
            return new Bitmap(bitmap);
        }
    }
}
