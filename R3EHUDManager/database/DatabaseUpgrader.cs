using R3EHUDManager.screen.model;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace R3EHUDManager.database
{
    class DatabaseUpgrader
    {
        public bool IsUpgradeNeeded(Database database)
        {
            return database.GetVersion() != Database.VERSION;
        }

        public void Upgrade(Database database, SQLiteConnection connection)
        {
            if (database.GetVersion() == 1)
                UpgradeVersion1(connection);

            new SQLiteCommand(
                $"UPDATE config SET value = {Database.VERSION} WHERE key = 'dbVersion'"
                , connection).ExecuteNonQuery();
        }

        private void UpgradeVersion1(SQLiteConnection connection)
        {
            new SQLiteCommand(
                $"ALTER TABLE backgrounds ADD COLUMN layoutType INTEGER DEFAULT {(int)ScreenLayoutType.SINGLE}"
                , connection).ExecuteNonQuery();
        }
    }
}
