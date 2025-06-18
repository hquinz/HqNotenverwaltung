using HqNotenverwaltung.Contracts;
using HqNotenverwaltung.Model;
using HqNotenverwaltung.ViewModel;
using System.Data.Common;
using System.Windows.Documents;

namespace HqNotenverwaltung.Data.Tools
{
    class SchoolyearQueries
    {
        private readonly IDbManager dbManager;

        public SchoolyearQueries(IDbManager dbManager) => this.dbManager = dbManager;

        public List<int> GetSchoolYears() 
        {
            List<int> _schoolyears = [];

            using DbConnection connection = dbManager.GetConnection();  
            if (connection.State != System.Data.ConnectionState.Open) { throw new InvalidOperationException("Database connection is not open."); }
            var commandCreate = dbManager.CommandsSchoolyearQuery;
            using var reader = dbManager.ExecuteQuery(commandCreate.GetSchoolyears(connection));
            while (reader.Read()) { _schoolyears.Add(reader.GetInt32(0)); }
            return _schoolyears;
        }

        public async Task<List<int>> GetSchoolYearsAsync()
        {
            List<int> schoolyears = [];
            using DbConnection connection = await dbManager.GetConnectionAsync();
            if (connection.State != System.Data.ConnectionState.Open) { throw new InvalidOperationException("Database connection is not open."); }
            var commandCreate = dbManager.CommandsSchoolyearQuery;
            using var reader = await dbManager.ExecuteQueryAsync(commandCreate.GetSchoolyears(connection));
            while (await reader.ReadAsync()) { schoolyears.Add(reader.GetInt32(0)); }
            return schoolyears;
        }
        public async Task<int> GetSchoolyearSemesteredAsync(int schoolyear)
        {
            using DbConnection connection = await dbManager.GetConnectionAsync();
            if (connection.State != System.Data.ConnectionState.Open) { throw new InvalidOperationException("Database connection is not open."); }
            return await GetSchoolyearSemesteredAsync(connection, schoolyear);
        }
        public async Task<int> GetSchoolyearSemesteredAsync(DbConnection connection, int schoolyear)
        {
            var _commandCreate = dbManager.CommandsSchoolyearQuery;
            return await GetSchoolyearSemesteredAsync(connection, _commandCreate, schoolyear);
        }
        public async Task<int> GetSchoolyearSemesteredAsync(DbConnection connection, IDbCommandsSchoolyearQuery commandCreate, int schoolyear)
        {
            using var reader = await dbManager.ExecuteQueryAsync(commandCreate.GetSchoolyearSemestered(connection, schoolyear));
            await reader.ReadAsync();
            return reader.GetInt32(0);
        }



        public async Task UpsertSchoolyearAsync(int schoolyear, int semestered)
        {
            using var connection = await dbManager.GetConnectionAsync();
            if (connection.State != System.Data.ConnectionState.Open) { throw new InvalidOperationException("Database connection is not open."); }
            var command = dbManager.CommandsSchoolyearQuery.UpsertSchoolyear(connection, schoolyear, semestered);
            await dbManager.ExecuteNonQueryAsync(command);
        }


    }
}
