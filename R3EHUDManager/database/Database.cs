using R3EHUDManager.background.model;
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
        private const int dbVersion = 1;

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

                string sql = "CREATE TABLE backgrounds (id INT UNIQUE, name TEXT, fileName TEXT, directoryType INT, isBuiltIn INT);" +
                    "CREATE TABLE config (key TEXT unique, value BLOB);";

                command = new SQLiteCommand(sql, db);
                command.ExecuteNonQuery();

                sql = $"INSERT INTO config (key, value) VALUES ('dbVersion', {dbVersion})";

                command = new SQLiteCommand(sql, db);
                command.ExecuteNonQuery();

                command = new SQLiteCommand("end", db);
                command.ExecuteNonQuery();

                db.Close();
            }
        }

        public void AddBackground(BackgroundModel background)
        {
            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();
                SQLiteCommand command;

                string request = "INSERT INTO backgrounds (id, name, fileName, directoryType, isBuiltIn) VALUES " +
                    $"({background.Id}, '{background.Name}', '{background.FileName}', {(int)background.DirectoryType}, {Convert.ToInt32(background.IsBuiltInt)})";

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
                            reader.GetInt32(0), reader.GetString(1), reader.GetString(2), (BaseDirectoryType)reader.GetInt32(3), Convert.ToBoolean(reader.GetInt32(4)), new screen.model.ScreenLayout(screen.model.ScreenLayoutType.SINGLE, 1000, 1000, 1000))); // TODO temp
                    }
                    reader.Close();
                }

                db.Close();
            }

            return backgrounds;
        }
    }
}
