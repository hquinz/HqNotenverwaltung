
using Microsoft.Data.Sqlite;

namespace HqNotenverwaltung.Data.SqlLite
{
    internal class SqLiteCommandsSchoolyearQuery
    {
        public SqliteCommand GetSchoolyears(SqliteConnection connection)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = @"SELECT Startyear FROM Schoolyear;";
            return cmd;
        }
    }
}
