﻿using R3EHUDManager.placeholder.model;
using R3EHUDManager.screen.model;
using R3EHUDManager.userpreferences.model;
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
            switch (database.GetVersion())
            {
                case 1:
                    UpgradeVersion1To2(connection);
                    goto case 2;
                case 2:
                    UpgradeVersion2To3(connection);
                    goto case 3;
                case 3:
                    UpgradeVersion3To4(connection);
                    goto case 4;
                case 4:
                    UpgradeVersion4To5(connection);
                    break;
            }

            new SQLiteCommand(
                $"UPDATE config SET value = {Database.VERSION} WHERE key = 'dbVersion'"
                , connection).ExecuteNonQuery();
        }

        private void UpgradeVersion1To2(SQLiteConnection connection)
        {
            new SQLiteCommand(
                $"ALTER TABLE backgrounds ADD COLUMN layoutType INTEGER DEFAULT {(int)ScreenLayoutType.SINGLE}"
                , connection).ExecuteNonQuery();

            new SQLiteCommand(
                    "CREATE TABLE profiles (id INT UNIQUE, name TEXT, backgroundId INT, fileName TEXT);"
                    , connection).ExecuteNonQuery();
        }

        private void UpgradeVersion2To3(SQLiteConnection connection)
        {
            new SQLiteCommand(
                "CREATE TABLE placeholderFilter (name TEXT UNIQUE, isFiltered INT);"
                , connection).ExecuteNonQuery();

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
                new SQLiteCommand(
                    $"INSERT INTO placeholderFilter (name, isFiltered) VALUES ('{keyValue.Key}', {Convert.ToInt32(keyValue.Value)});"
                    , connection).ExecuteNonQuery();


            new SQLiteCommand(
                "CREATE TABLE userPreferences (type INT UNIQUE, value BLOB);"
                , connection).ExecuteNonQuery();

            new SQLiteCommand(
                $"INSERT INTO userPreferences (type, value) VALUES ({(int)PreferenceType.PROMPT_OUTSIDE_PLACEHOLDER}, {(int)OutsidePlaceholdersPrefType.PROMPT});"
                , connection).ExecuteNonQuery();

            new SQLiteCommand(
                $"INSERT INTO userPreferences (type, value) VALUES ({(int)PreferenceType.PROMPT_SAVE_PROFILE_LAYOUT_CHANGE}, {1});"
                , connection).ExecuteNonQuery();

            new SQLiteCommand(
                $"INSERT INTO userPreferences (type, value) VALUES ({(int)PreferenceType.PROMPT_SAVE_PROFILE_APP_EXIT}, {1});"
                , connection).ExecuteNonQuery();

            new SQLiteCommand(
                $"INSERT INTO userPreferences (type, value) VALUES ({(int)PreferenceType.PROMPT_APPLY_LAYOUT_LAYOUT_CHANGE}, {1});"
                , connection).ExecuteNonQuery();

            new SQLiteCommand(
                $"INSERT INTO userPreferences (type, value) VALUES ({(int)PreferenceType.PROMPT_APPLY_LAYOUT_APP_EXIT}, {1});"
                , connection).ExecuteNonQuery();

            new SQLiteCommand(
                $"INSERT INTO userPreferences (type, value) VALUES ({(int)PreferenceType.USER_WATCHED_PRESENTATION}, {0});"
                , connection).ExecuteNonQuery();

            new SQLiteCommand(
                $"INSERT INTO userPreferences (type, value) VALUES ({(int)PreferenceType.LAST_PROFILE}, {-1});"
                , connection).ExecuteNonQuery();

            new SQLiteCommand(
                $"INSERT INTO userPreferences (type, value) VALUES ({(int)PreferenceType.USE_INVARIANT_CULTURE}, {0});"
                , connection).ExecuteNonQuery();
        }

        private void UpgradeVersion3To4(SQLiteConnection connection)
        {
            new SQLiteCommand(
                $"ALTER TABLE profiles ADD COLUMN motecId INTEGER DEFAULT {1}"
                , connection).ExecuteNonQuery();
        }

        private void UpgradeVersion4To5 (SQLiteConnection connection)
        {
            new SQLiteCommand(
                $"INSERT INTO placeholderFilter (name, isFiltered) VALUES ('{PlaceholderName.INPUT_METER}', {Convert.ToInt32(false)});"
                , connection).ExecuteNonQuery();
        }
    }
}
