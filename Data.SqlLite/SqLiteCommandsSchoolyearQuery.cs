
using Microsoft.Data.Sqlite;

namespace HqNotenverwaltung.Data.SqlLite
{
    internal class SqLiteCommandsSchoolyearQuery
    {
        #region Schoolyear
        public SqliteCommand GetSchoolyears(SqliteConnection connection)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT Startyear FROM Schoolyear;";
            return cmd;
        }

        public SqliteCommand UpsertSchoolyear(SqliteConnection connection, int schoolyear, int semestered)
        {
            var cmd = connection.CreateCommand();
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
        public SqliteCommand GetDays(SqliteConnection connection, string table, int schoolyear)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = $"SELECT Id, Day, Remark FROM {table}";
            cmd.CommandText += @"WHERE  Schoolyear = $schoolyear);";
            cmd.Parameters.AddWithValue("$schoolyear", schoolyear);
            return cmd;
        }

        public SqliteCommand UpsertDay(SqliteConnection connection, int id, string table, int schoolyear, DateOnly day, string remark)
        {
            var cmd = connection.CreateCommand();
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
