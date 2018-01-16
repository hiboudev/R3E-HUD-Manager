using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.profile.model
{
    class ProfileFactory
    {
        private static int idCounter = 0;

        public static ProfileModel NewProfileModel(string name, int backgroundId, string hudFilePath, int motecId)
        {
            return new ProfileModel(++idCounter, name, backgroundId, hudFilePath, motecId);
        }

        public static ProfileModel NewProfileModel(int databaseId, string name, int backgroundId, string hudFilePath, int motecId)
        {
            if (databaseId > idCounter) idCounter = databaseId;

            return new ProfileModel(databaseId, name, backgroundId, hudFilePath, motecId);
        }
    }
}
