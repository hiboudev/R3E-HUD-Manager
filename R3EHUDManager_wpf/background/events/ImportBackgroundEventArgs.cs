using da2mvc.core.events;
using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace R3EHUDManager.background.events
{
    class ImportBackgroundEventArgs : BaseEventArgs
    {
        public ImportBackgroundEventArgs(int eventId, string name, ScreenLayoutType layout, Rect cropArea, string filePath) : base(eventId)
        {
            Name = name;
            Layout = layout;
            FilePath = filePath;
            CropArea = cropArea;
        }

        public string Name { get; }
        public ScreenLayoutType Layout { get; }
        public string FilePath { get; }
        public Rect CropArea { get; }
    }
}
