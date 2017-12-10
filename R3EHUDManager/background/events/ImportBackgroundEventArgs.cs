using da2mvc.events;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.background.events
{
    class ImportBackgroundEventArgs : BaseEventArgs
    {

        public ImportBackgroundEventArgs(string eventName, string name, ScreenLayoutType layout, Rectangle cropArea, string filePath) : base(eventName)
        {
            Name = name;
            Layout = layout;
            FilePath = filePath;
            CropArea = cropArea;
        }

        public string Name { get; }
        public ScreenLayoutType Layout { get; }
        public string FilePath { get; }
        public Rectangle CropArea { get; }
    }
}
