using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.background.model
{
    class BackgroundFactory
    {
        private static int idCounter = 0;

        public static BackgroundModel NewBackgroundModel(string name, string filePath, BaseDirectoryType directoryType, bool IsBuiltInt, ScreenLayout layout)
        {
            return new BackgroundModel(++idCounter, name, filePath, directoryType, IsBuiltInt, layout);
        }

        public static BackgroundModel NewBackgroundModel(int databaseId, string name, string filePath, BaseDirectoryType directoryType, bool IsBuiltInt, ScreenLayout layout)
        {
            if (databaseId > idCounter) idCounter = databaseId;

            return new BackgroundModel(databaseId, name, filePath, directoryType, IsBuiltInt, layout);
        }
    }
}
