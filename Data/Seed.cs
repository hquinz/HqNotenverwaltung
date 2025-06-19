using HqNotenverwaltung.Contracts;
using System.Data.Common;


namespace HqNotenverwaltung.Data
{
    class Seed
    {
        private readonly IDbManager _dbManager;

        public Seed(IDbManager dbmanager)
        {
            {
                _dbManager = dbmanager ?? throw new ArgumentNullException(nameof(dbmanager), "Database manager cannot be null.");
            }
        }

        public async Task DoAsync(IDbCommandsSchoolyearQuery create)
        {
            using DbConnection connection = await _dbManager.GetConnectionAsync();
            if (connection.State != System.Data.ConnectionState.Open) { throw new InvalidOperationException("Database connection is not open."); }
            var commandCreate = _dbManager.CommandsSchoolyearQuery;
            await _dbManager.ExecuteNonQueryAsync(commandCreate.UpsertSchoolyear(connection, 24, 0));
            await _dbManager.ExecuteNonQueryAsync(commandCreate.UpsertDay(connection, 1, "DaysStart", 24, new DateOnly(2024, 09, 09), "Schulstart"));
            await _dbManager.ExecuteNonQueryAsync(commandCreate.UpsertDay(connection, 2, "DaysStart", 24, new DateOnly(2024, 11, 18), "Schulstart 4AFME"));
            await _dbManager.ExecuteNonQueryAsync(commandCreate.UpsertDay(connection, 1, "DaysEnd"  , 24, new DateOnly(2025, 02, 17), "Semster"));
            await _dbManager.ExecuteNonQueryAsync(commandCreate.UpsertDay(connection, 2, "DaysEnd"  , 24, new DateOnly(2025, 05, 02), "Ende Maturaklassen"));
            await _dbManager.ExecuteNonQueryAsync(commandCreate.UpsertDay(connection, 3, "DaysEnd"  , 24, new DateOnly(2025, 07, 03), "Ende Schuljahr"));
        }


    }
}
