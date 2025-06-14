using Microsoft.Data.Sqlite;

namespace HqNotenverwaltung.Data.SqlLite
{
    internal class SqLiteCommandsSchoolyearCreate 
    {
        public SqliteCommand CreateTableSchoolyear(SqliteConnection connection)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS Schoolyear (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT
                   ,Startyear INTEGER NOT NULL
                   ,Semestered Integer NOT NULL
                );";

            return cmd;
        }
        public SqliteCommand CreateTableDaysStart(SqliteConnection connection)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS DaysStart (
                    Day DATE PRIMARY KEY
                   ,SchoolyearId INTEGER NOT NULL
                   ,Remark TEXT
                   ,FOREIGN KEY (SchoolyearId) REFERENCES Schoolyear(Id)
                );";

            return cmd;
        }
        public SqliteCommand CreateTableDaysEnd(SqliteConnection connection)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS DaysEnd (
                    Day DATE PRIMARY KEY
                   ,SchoolyearId INTEGER NOT NULL
                   ,Remark TEXT
                   ,FOREIGN KEY (SchoolyearId) REFERENCES Schoolyear(Id)
                );";

            return cmd;
        }

        public SqliteCommand CreateTableDaysFree(SqliteConnection connection)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS DaysFree (
                    Day DATE PRIMARY KEY
                   ,SchoolyearId INTEGER NOT NULL
                   ,Cause TEXT
                   ,FOREIGN KEY (SchoolyearId) REFERENCES Schoolyear(Id)
                );";

            return cmd;
        }

    }
}
