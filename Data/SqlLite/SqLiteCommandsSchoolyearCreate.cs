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
                    Startyear INTEGER INTEGER PRIMARY KEY
                   ,Semestered Integer NOT NULL
                );";
            return cmd;
        }
        public SqliteCommand CreateTableDaysStart(SqliteConnection connection)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS DaysStart (
                    Id INTEGER PRIMARY KEY
                   ,Schoolyear INTEGER NOT NULL
                   ,Day DATE 
                   ,Remark TEXT
                   ,FOREIGN KEY (Schoolyear) REFERENCES Schoolyear(Startyear)
                );";
            return cmd;
        }
        public SqliteCommand CreateTableDaysEnd(SqliteConnection connection)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS DaysEnd (
                    Id INTEGER PRIMARY KEY
                   ,Day DATE 
                   ,Schoolyear INTEGER NOT NULL
                   ,Remark TEXT
                   ,FOREIGN KEY (Schoolyear) REFERENCES Schoolyear(Startyear)
                );";
            return cmd;
        }

        public SqliteCommand CreateTableDaysFree(SqliteConnection connection)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText =
                @"CREATE TABLE IF NOT EXISTS DaysFree (
                    Id INTEGER PRIMARY KEY
                   ,Day DATE 
                   ,Schoolyear INTEGER NOT NULL
                   ,Remark TEXT
                   ,FOREIGN KEY (Schoolyear) REFERENCES Schoolyear(Startyear)
                );";
            return cmd;
        }

    }
}
