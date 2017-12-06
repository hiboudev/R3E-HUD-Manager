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

        public static BackgroundModel NewBackgroundModel(string name, string filePath)
        {
            return new BackgroundModel(++idCounter, name, filePath);
        }

        public static BackgroundModel NewBackgroundModel(int databaseId, string name, string filePath)
        {
            if (databaseId > idCounter) idCounter = databaseId;

            return new BackgroundModel(databaseId, name, filePath);
        }
    }
}
