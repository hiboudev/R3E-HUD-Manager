using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.database
{
    class R3eDatabase
    {
        private const string DB_ARGS = @"Data Source=_r3e-data\r3e-database.sqlite";

        public Car[] GetCars(int[] ids)
        {
            var cars = new List<Car>();
            var classes = new Dictionary<int, string>();

            using (SQLiteConnection db = new SQLiteConnection(DB_ARGS))
            {
                db.Open();

                using (SQLiteDataReader reader = new SQLiteCommand("SELECT * FROM classes", db).ExecuteReader())
                {
                    while (reader.Read())
                    {
                        classes.Add(reader.GetInt32(0), reader.GetString(1));
                    }
                }

                using (SQLiteDataReader reader = new SQLiteCommand("SELECT * FROM cars", db).ExecuteReader())
                {
                    while (reader.Read())
                    {
                        if (ids.Contains(reader.GetInt32(0)))
                            cars.Add(new Car(reader.GetInt32(0), reader.GetString(1), classes[reader.GetInt32(2)]));
                    }
                }

                db.Close();
            }

            return cars.ToArray();
        }

        private void NoQuery(string request, SQLiteConnection db)
        {
            new SQLiteCommand(request, db).ExecuteNonQuery();
        }
    }

    class Car
    {
        public Car(int id, string name, string className)
        {
            Id = id;
            Name = name;
            ClassName = className;
        }

        public int Id { get; }
        public string Name { get; }
        public string ClassName { get; }
    }
}
