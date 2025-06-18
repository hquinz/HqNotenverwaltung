using HqNotenverwaltung.Contracts;
using HqNotenverwaltung.Model;
using System.Data.Common;
using System.Diagnostics;

namespace HqNotenverwaltung.Data.Tools
{
    class SchooldayQueries
    {
        private readonly IDbManager dbManager;

        public SchooldayQueries(IDbManager dbManager) => this.dbManager = dbManager;

        public async Task<List<ModelSchooldaySpecial>> GetSchooldaysAsync(int schoolyear, string table)
        {
            using DbConnection connection = await dbManager.GetConnectionAsync();
            if (connection.State != System.Data.ConnectionState.Open) { throw new InvalidOperationException("Database connection is not open."); }
            return await GetSchooldaysAsync(connection, schoolyear, table);
        }
        public async Task<List<ModelSchooldaySpecial>> GetSchooldaysAsync(DbConnection connection, int schoolyear, string table)
        {
            var _commandCreate = dbManager.CommandsSchoolyearQuery;
            return await GetSchooldaysAsync(connection, _commandCreate, schoolyear, table);
        }
        public async Task<List<ModelSchooldaySpecial>> GetSchooldaysAsync(DbConnection connection, IDbCommandsSchoolyearQuery commandCreate, int schoolyear, string table)
        {
            List<ModelSchooldaySpecial> _schooldays = [];
            using var reader = await dbManager.ExecuteQueryAsync(commandCreate.GetDays(connection, table, schoolyear));
            while (await reader.ReadAsync())
            {
                var _day = new ModelSchooldaySpecial
                {
                    Id = reader.GetInt32(0),
                    Date = reader.GetFieldValue<DateOnly>(1),
                    Remark = reader.GetString(2)
                };
                _schooldays.Add(_day);
            }
            return _schooldays;
        }

    }
}
