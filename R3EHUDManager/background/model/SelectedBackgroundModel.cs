using da2MVC.events;
using R3EHUDManager.background.events;
using R3EHUDManager.graphics;
using R3EHUDManager.location.model;
using R3EHUDManager.placeholder.events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.background.model
{
    class SelectedBackgroundModel : EventDispatcher
    {
        private Image bitmap;
        public Size Dimension { get; private set; }
        public decimal AspectRatio { get; private set; }
        public BackgroundModel Selection { get; private set; }

        public const string EVENT_BACKGROUND_CHANGED = "backgroundChanged";
        private readonly LocationModel locationModel;

        public SelectedBackgroundModel(LocationModel locationModel)
        {
            this.locationModel = locationModel;
        }

        public void SelectBackground(BackgroundModel background)
        {
            Selection = background;

            if(bitmap != null)
            {
                bitmap.Dispose();
                bitmap = null;
            }

            string dirPath = locationModel.GetGraphicBasePath(background.DirectoryType);

            bitmap = GraphicalAsset.GetNoCache(Path.Combine(dirPath, Selection.FileName));
            Dimension = bitmap.PhysicalDimension.ToSize();
            AspectRatio = (decimal)Dimension.Width / Dimension.Height;

            DispatchEvent(new SelectedBackgroundEventArgs(EVENT_BACKGROUND_CHANGED, this));
        }

        public Image GetBackground (/*Size appScreenSize*/)
        {
            return new Bitmap(bitmap);
        }
    }
}
