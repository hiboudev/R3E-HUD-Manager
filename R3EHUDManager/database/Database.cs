using R3EHUDManager.background.model;
using R3EHUDManager.placeholder.model;
using R3EHUDManager.profile.model;
using R3EHUDManager.screen.model;
using R3EHUDManager.userpreferences.model;
using R3EHUDManager.utils;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.database
{
    class Database
    {
        private string dbArgs;
        public const int VERSION = 3;

        public void Initialize(string path)
        {
            dbArgs = $"Data Source={path}";

            if (File.Exists(path))
                return;

            SQLiteConnection.CreateFile(path);


            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();

                NoQuery("begin", db);

                NoQuery(
                    "CREATE TABLE backgrounds (id INT UNIQUE, name TEXT, fileName TEXT, directoryType INT, isBuiltIn INT, layoutType INT);" +
                    "CREATE TABLE config (key TEXT unique, value BLOB);" +
                    "CREATE TABLE profiles (id INT UNIQUE, name TEXT, backgroundId INT, fileName TEXT);" +
                    "CREATE TABLE placeholderFilter (name TEXT UNIQUE, isFiltered INT);" +
                    "CREATE TABLE userPreferences (type INT UNIQUE, value BLOB);"
                    , db);

                NoQuery(
                    $"INSERT INTO config (key, value) VALUES ('dbVersion', {VERSION})"
                    , db);

                Dictionary<string, bool> values = new Dictionary<string, bool>
                {
                    { PlaceholderName.APEXHUNT_DISPLAY, true },
                    { PlaceholderName.CAR_STATUS, true },
                    { PlaceholderName.DRIVER_NAME_TAGS, true },
                    { PlaceholderName.FFB_GRAPH, false },
                    { PlaceholderName.FLAGS, true },
                    { PlaceholderName.MINI_MOTEC, true },
                    { PlaceholderName.MOTEC, false },
                    { PlaceholderName.POSITION_BAR, false },
                    { PlaceholderName.TRACK_MAP, false },
                    { PlaceholderName.VIRTUAL_MIRROR, false }
                };

                foreach (KeyValuePair<string, bool> keyValue in values)
                    NoQuery(
                        $"INSERT INTO placeholderFilter (name, isFiltered) VALUES ('{keyValue.Key}', {Convert.ToInt32(keyValue.Value)});"
                        , db);

                NoQuery(
                    $"INSERT INTO userPreferences (type, value) VALUES ({(int)PreferenceType.PROMPT_OUTSIDE_PLACEHOLDER}, {(int)OutsidePlaceholdersPrefType.PROMPT});"
                    , db);

                NoQuery(
                    $"INSERT INTO userPreferences (type, value) VALUES ({(int)PreferenceType.USER_WATCHED_PRESENTATION}, {0});"
                    , db);

                NoQuery(
                    $"INSERT INTO userPreferences (type, value) VALUES ({(int)PreferenceType.LAST_PROFILE}, {-1});"
                    , db);

                NoQuery("end", db);

                db.Close();
            }
        }

        public int GetVersion()
        {
            int dbVersion = -1;

            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();

                using (SQLiteDataReader reader = new SQLiteCommand("SELECT value FROM config WHERE key = 'dbVersion'", db).ExecuteReader())
                {
                    while (reader.Read())
                    {
                        dbVersion = reader.GetInt32(0);
                    }
                }
                db.Close();
            }

            return dbVersion;
        }

        public void Upgrade(DatabaseUpgrader upgrader)
        {
            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();

                NoQuery("begin", db);
                upgrader.Upgrade(this, db);
                NoQuery("end", db);

                db.Close();
            }
        }

        public void GetPreferences(UserPreferencesModel model)
        {
            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();
                using (SQLiteDataReader reader = new SQLiteCommand("SELECT * from userPreferences", db).ExecuteReader())
                {
                    while (reader.Read())
                    {
                        switch ((PreferenceType)reader.GetInt32(0))
                        {
                            case PreferenceType.PROMPT_OUTSIDE_PLACEHOLDER:
                                model.PromptOutsidePlaceholders = (OutsidePlaceholdersPrefType)reader.GetInt32(1);
                                break;
                            case PreferenceType.USER_WATCHED_PRESENTATION:
                                model.UserWatchedPresentation = Convert.ToBoolean(reader.GetInt32(1));
                                break;
                            case PreferenceType.LAST_PROFILE:
                                model.LastProfileId = reader.GetInt32(1);
                                break;
                        }
                    }
                    reader.Close();
                }
                db.Close();
            }
        }

        internal void SaveLastProfilePref(int profileId)
        {
            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();
                NoQuery("begin", db);

                NoQuery(
                        $"UPDATE userPreferences SET value = {profileId} WHERE type = {(int)PreferenceType.LAST_PROFILE};"
                        , db);


                NoQuery("end", db);
                db.Close();
            }
        }

        internal void SaveOutsidePlaceholdersPref(OutsidePlaceholdersPrefType prefValue)
        {
            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();
                NoQuery("begin", db);

                NoQuery(
                        $"UPDATE userPreferences SET value = {(int)prefValue} WHERE type = {(int)PreferenceType.PROMPT_OUTSIDE_PLACEHOLDER};"
                        , db);


                NoQuery("end", db);
                db.Close();
            }
        }

        internal void SaveUserWatchedPresentationPref(bool prefValue)
        {
            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();
                NoQuery("begin", db);

                NoQuery(
                        $"UPDATE userPreferences SET value = {Convert.ToInt32(prefValue)} WHERE type = {(int)PreferenceType.USER_WATCHED_PRESENTATION};"
                        , db);


                NoQuery("end", db);
                db.Close();
            }
        }

        public Dictionary<string, bool> GetPlaceholderFilters()
        {
            Dictionary<string, bool> filters = new Dictionary<string, bool>();

            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();

                using (SQLiteDataReader reader = new SQLiteCommand("SELECT * from placeholderFilter", db).ExecuteReader())
                {
                    while (reader.Read())
                    {
                        filters.Add(reader.GetString(0), Convert.ToBoolean(reader.GetInt32(1)));
                    }
                    reader.Close();
                }

                db.Close();
            }

            return filters;
        }

        public void UpdatePlaceholderFilters(Dictionary<string, bool> filters)
        {
            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();
                NoQuery("begin", db);

                foreach (KeyValuePair<string, bool> keyValue in filters)
                    NoQuery(
                        $"UPDATE placeholderFilter SET isFiltered = {Convert.ToInt32(keyValue.Value)} WHERE name = '{keyValue.Key}';"
                        , db);

                NoQuery("end", db);
                db.Close();
            }
        }

        public void AddProfile(ProfileModel profile)
        {
            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();

                NoQuery("INSERT INTO profiles (id, name, backgroundId, fileName) VALUES " +
                    $"({profile.Id}, '{StringUtils.ToDatabaseUserString(profile.Name)}', {profile.BackgroundId}, '{profile.FileName}')"
                    , db);

                db.Close();
            }
        }

        internal void UpdateProfile(ProfileModel profile)
        {
            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();

                NoQuery(
                    $"UPDATE profiles SET name = '{StringUtils.ToDatabaseUserString(profile.Name)}', backgroundId = {profile.BackgroundId}, fileName = '{profile.FileName}'" +
                    $"WHERE id = {profile.Id};"
                    , db);

                db.Close();
            }
        }

        internal void DeleteProfile(ProfileModel profile)
        {
            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();

                NoQuery(
                    $"DELETE FROM profiles WHERE id = {profile.Id}"
                    , db);

                db.Close();
            }
        }

        public List<ProfileModel> GetAllProfiles()
        {
            List<ProfileModel> profiles = new List<ProfileModel>();

            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();

                using (SQLiteDataReader reader = new SQLiteCommand("SELECT * from profiles", db).ExecuteReader())
                {
                    while (reader.Read())
                    {
                        profiles.Add(ProfileFactory.NewProfileModel(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetInt32(2),
                            reader.GetString(3)));
                    }
                    reader.Close();
                }

                db.Close();
            }

            return profiles;
        }

        public void AddBackground(BackgroundModel background)
        {
            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();

                NoQuery("INSERT INTO backgrounds (id, name, fileName, directoryType, isBuiltIn, layoutType) VALUES " +
                    $"({background.Id}, '{StringUtils.ToDatabaseUserString(background.Name)}', '{background.FileName}', {(int)background.DirectoryType}, {Convert.ToInt32(background.IsBuiltInt)}, {(int)background.Layout})"
                    , db);

                db.Close();
            }
        }

        internal void DeleteBackground(BackgroundModel background)
        {
            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();

                NoQuery($"DELETE FROM backgrounds WHERE id = {background.Id}"
                    , db);

                db.Close();
            }
        }

        public List<BackgroundModel> GetAllBackgrounds()
        {
            List<BackgroundModel> backgrounds = new List<BackgroundModel>();

            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();

                using (SQLiteDataReader reader = new SQLiteCommand("SELECT * from backgrounds", db).ExecuteReader())
                {
                    while (reader.Read())
                    {
                        backgrounds.Add(BackgroundFactory.NewBackgroundModel(
                            reader.GetInt32(0),
                            reader.GetString(1),
                            reader.GetString(2),
                            (BaseDirectoryType)reader.GetInt32(3),
                            Convert.ToBoolean(reader.GetInt32(4)),
                            (ScreenLayoutType)reader.GetInt32(5)));
                    }
                    reader.Close();
                }

                db.Close();
            }

            return backgrounds;
        }

        private void NoQuery(string request, SQLiteConnection db)
        {
            new SQLiteCommand(request, db).ExecuteNonQuery();
        }
    }
}
