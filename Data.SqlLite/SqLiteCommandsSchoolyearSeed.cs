using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqNotenverwaltung.Data.SqlLite
{
    internal class SqLiteCommandsSchoolyearSeed
    {
          public SqliteCommand SeedTableSchoolyear(SqliteConnection connection)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText =
                @"INSERT INTO Schoolyear (Startyear,Semestered)
                   VALUES (25,0);";

            return cmd;
        }

        public SqliteCommand GetSchoolyearId(SqliteConnection connection)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText =
                @"SELECT Id FROM Schoolyear WHERE  Startyear =25;";

            return cmd;
        }

        public SqliteCommand SeedTableDaysStart(SqliteConnection connection, int schoolyearId, DateOnly day, string remark)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO DaysStart (Day, SchoolyearId, Remark) VALUES ($day, $schoolyeartId, $remark)";
            cmd.Parameters.AddWithValue("$day", day);
            cmd.Parameters.AddWithValue("$schoolyeartId", schoolyearId);
            cmd.Parameters.AddWithValue("$remark", remark);
            return cmd;
        }
        public SqliteCommand SeedTableDaysEnd(SqliteConnection connection, int schoolyearId, DateOnly day, string remark)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = "INSERT INTO DaysEnd (Day, SchoolyearId, Remark) VALUES ($day, $schoolyeartId, $remark)";
            cmd.Parameters.AddWithValue("$day", day);
            cmd.Parameters.AddWithValue("$schoolyeartId", schoolyearId);
            cmd.Parameters.AddWithValue("$remark", remark);
            return cmd;
        }

    }
}
