using R3EHUDManager.background.model;
using R3EHUDManager.screen.model;
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

                SQLiteCommand command = new SQLiteCommand("begin", db);
                command.ExecuteNonQuery();

                string sql = "CREATE TABLE backgrounds (id INT UNIQUE, name TEXT, fileName TEXT, directoryType INT, isBuiltIn INT, layoutType INT);" +
                    "CREATE TABLE config (key TEXT unique, value BLOB);";

                command = new SQLiteCommand(sql, db);
                command.ExecuteNonQuery();

                sql = $"INSERT INTO config (key, value) VALUES ('dbVersion', {VERSION})";

                command = new SQLiteCommand(sql, db);
                command.ExecuteNonQuery();

                command = new SQLiteCommand("end", db);
                command.ExecuteNonQuery();

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
                new SQLiteCommand("begin", db).ExecuteNonQuery();
                upgrader.Upgrade(this, db);
                new SQLiteCommand("end", db).ExecuteNonQuery();
                db.Close();
            }
        }

        public void AddBackground(BackgroundModel background)
        {
            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();
                SQLiteCommand command;

                string request = "INSERT INTO backgrounds (id, name, fileName, directoryType, isBuiltIn, layoutType) VALUES " +
                    $"({background.Id}, '{background.Name}', '{background.FileName}', {(int)background.DirectoryType}, {Convert.ToInt32(background.IsBuiltInt)}, {(int)background.Layout})";

                command = new SQLiteCommand(request, db);
                command.ExecuteNonQuery();

                db.Close();
            }
        }

        internal void DeleteBackground(BackgroundModel background)
        {
            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();
                SQLiteCommand command;

                string request = $"DELETE FROM backgrounds WHERE id = {background.Id}";

                command = new SQLiteCommand(request, db);
                command.ExecuteNonQuery();

                db.Close();
            }
        }

        public List<BackgroundModel> GetAllBackgrounds()
        {
            List<BackgroundModel> backgrounds = new List<BackgroundModel>();

            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();
                SQLiteCommand command;

                string request = "SELECT * from backgrounds";
                command = new SQLiteCommand(request, db);

                using (SQLiteDataReader reader = command.ExecuteReader())
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
    }
}
