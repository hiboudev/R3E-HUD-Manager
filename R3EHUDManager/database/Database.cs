using R3EHUDManager.background.model;
using R3EHUDManager.profile.model;
using R3EHUDManager.screen.model;
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
        public const int VERSION = 2;

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

                // V2 added backgrounds.layoutType
                // V2 added table profiles
                NoQuery(
                    "CREATE TABLE backgrounds (id INT UNIQUE, name TEXT, fileName TEXT, directoryType INT, isBuiltIn INT, layoutType INT);" +
                    "CREATE TABLE config (key TEXT unique, value BLOB);" +
                    "CREATE TABLE profiles (id INT UNIQUE, name TEXT, backgroundId INT, fileName TEXT);"
                    , db);

                NoQuery(
                    $"INSERT INTO config (key, value) VALUES ('dbVersion', {VERSION})"
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

        public void AddProfile(ProfileModel profile)
        {
            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();

                NoQuery("INSERT INTO profiles (id, name, backgroundId, fileName) VALUES " +
                    $"({profile.Id}, '{StringUtils.ToDatabaseUserString(profile.Name)}', {profile.BackgroundId}, '{profile.fileName}')"
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
                    $"UPDATE profiles SET name = '{StringUtils.ToDatabaseUserString(profile.Name)}', backgroundId = {profile.BackgroundId}, fileName = '{profile.fileName}'" +
                    $"WHERE id = {profile.Id};"
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
