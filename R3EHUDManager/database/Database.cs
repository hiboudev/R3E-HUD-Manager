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

        public void Initialize(string path)
        {
            dbArgs = $"Data Source={path}";

            if (File.Exists(path))
                return;

            SQLiteConnection.CreateFile(path);

            using (SQLiteConnection db = new SQLiteConnection(dbArgs))
            {
                db.Open();

                string sql = "create table backgrounds (id int unique, name text, filePath text);";

                SQLiteCommand command = new SQLiteCommand(sql, db);
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

                string request = "INSERT INTO backgrounds (id, name, filePath) VALUES " +
                    $"({background.Id}, '{background.Name}', '{background.FilePath}')";

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
                        backgrounds.Add(BackgroundFactory.NewBackgroundModel(reader.GetInt32(0), reader.GetString(1), reader.GetString(2)));
                    }
                    reader.Close();
                }

                db.Close();
            }

            return backgrounds;
        }
    }
}
