using HqNotenverwaltung.Contracts;
using HqNotenverwaltung.Model;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using System.Diagnostics.Contracts;


namespace HqNotenverwaltung.Data.SqlLite
{
    internal class RepositorySchoolyear(SQLiteDbManager dbCon)            
    {

        private readonly SQLiteDbManager _dbManager = dbCon;

        public async Task CreateTableAsync()
        {
            using var connection = await _dbManager.GetOpenConnectionAsync();
            var create = new SqLiteCommandsSchoolyearCreate();
            var cmd = create.CreateTableSchoolyear(connection);
            await cmd.ExecuteNonQueryAsync();
            cmd = create.CreateTableDaysStart(connection);
            await cmd.ExecuteNonQueryAsync();
            cmd = create.CreateTableDaysEnd(connection);
            await cmd.ExecuteNonQueryAsync();
            cmd = create.CreateTableDaysFree(connection);
            await cmd.ExecuteNonQueryAsync();
        }
        public void CreateTable()
        {
            using var connection = _dbManager.GetOpenConnection();
            var create = new SqLiteCommandsSchoolyearCreate();
            var cmd = create.CreateTableSchoolyear(connection);
            cmd.ExecuteNonQuery();
            cmd = create.CreateTableDaysStart(connection);
            cmd.ExecuteNonQuery();
            cmd = create.CreateTableDaysEnd(connection);
            cmd.ExecuteNonQuery();
            cmd = create.CreateTableDaysFree(connection);
            cmd.ExecuteNonQuery();

        }
 
        public async Task Seed()
        {
            using var connection = await _dbManager.GetOpenConnectionAsync();
            var create = new SqLiteCommandsSchoolyearQuery();
            var cmd = create.UpsertSchoolyear(connection, 25, 0);
            await cmd.ExecuteNonQueryAsync();
            cmd = create.UpsertDay(connection, 1, "DaysStart", 25, new DateOnly(2025, 09, 01), "Schulstart");
            await cmd.ExecuteNonQueryAsync();
            cmd = create.UpsertDay(connection, 2, "DaysStart", 25, new DateOnly(2025, 11, 01), "Schulstart 4AFME");
            await cmd.ExecuteNonQueryAsync();
            cmd = create.UpsertDay(connection, 1, "DaysEnd", 25, new DateOnly(2026, 02, 02), "Semster");
            await cmd.ExecuteNonQueryAsync();
            cmd = create.UpsertDay(connection, 2, "DaysEnd", 25, new DateOnly(2026, 05, 20), "Ende Maturaklassen");
            await cmd.ExecuteNonQueryAsync();
            cmd = create.UpsertDay(connection, 3, "DaysEnd", 25, new DateOnly(2026, 07, 03), "Ende Schuljahr");
            await cmd.ExecuteNonQueryAsync();
        }
    }
}
