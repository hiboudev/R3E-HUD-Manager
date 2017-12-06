using da2mvc.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.background.events
{
    class ImportBackgroundEventArgs : BaseEventArgs
    {
        public ImportBackgroundEventArgs(string eventName, string name, string filePath) : base(eventName)
        {
            Name = name;
            FilePath = filePath;
        }

        public string Name { get; }
        public string FilePath { get; }
    }
}
