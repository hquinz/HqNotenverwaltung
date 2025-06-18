using System.Data.Common;
using Microsoft.Data.Sqlite;
using HqNotenverwaltung.Contracts;

namespace HqNotenverwaltung.Data.SqlLite
{
    internal class SqLiteCommandsSchoolyearQuery : IDbCommandsSchoolyearQuery
    {
        #region Schoolyear
        public DbCommand GetSchoolyears(DbConnection connection)
        {
            var sqliteConnection = connection as SqliteConnection ?? throw new ArgumentNullException(nameof(connection), "DbConnection cannot be null.");
            var cmd = sqliteConnection.CreateCommand();
            cmd.CommandText = @"SELECT Startyear FROM Schoolyear;";
            return cmd;
        }

        public DbCommand UpsertSchoolyear(DbConnection connection, int schoolyear, int semestered)
        {
            var sqliteConnection = connection as SqliteConnection ?? throw new ArgumentNullException(nameof(connection), "DbConnection cannot be null.");
            var cmd = sqliteConnection.CreateCommand();
            cmd.CommandText =
                @"INSERT INTO Schoolyear (Startyear,Semestered) 
                    VALUES ($schoolyear, $semestered)
                    ON CONFLICT (Startyear) DO UPDATE SET Semestered=excluded.Semestered;";
            cmd.Parameters.AddWithValue("$schoolyear", schoolyear);
            cmd.Parameters.AddWithValue("$semestered", semestered);
            return cmd;
        }
        #endregion
        #region Days
        public DbCommand GetDays(DbConnection connection, string table, int schoolyear)
        {
            var sqliteConnection = connection as SqliteConnection ?? throw new ArgumentNullException(nameof(connection), "DbConnection cannot be null.");
            var cmd = sqliteConnection.CreateCommand();
            cmd.CommandText = $"SELECT Id, Day, Remark FROM {table}";
            cmd.CommandText += @" WHERE  Schoolyear = $schoolyear;";
            cmd.Parameters.AddWithValue("$schoolyear", schoolyear);
            return cmd;
        }

        public DbCommand UpsertDay(DbConnection connection, int id, string table, int schoolyear, DateOnly day, string remark)
        {
            var sqliteConnection = connection as SqliteConnection ?? throw new ArgumentNullException(nameof(connection), "DbConnection cannot be null.");
            var cmd = sqliteConnection.CreateCommand();
            cmd.CommandText = $"INSERT INTO {table} (Id, Schoolyear, Day, Remark)";
            cmd.CommandText += @"VALUES ($id, $schoolyear, $day, $remark)
                                ON CONFLICT (Id) DO UPDATE SET 
                                    Schoolyear=excluded.Schoolyear,
                                    Remark=excluded.Remark;";
            cmd.Parameters.AddWithValue("$id", id);
            cmd.Parameters.AddWithValue("$schoolyear", schoolyear);
            cmd.Parameters.AddWithValue("$day", day);
            cmd.Parameters.AddWithValue("$remark", remark);
            return cmd;
        }
        #endregion
    }
}
