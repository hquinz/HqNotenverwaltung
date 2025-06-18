using HqNotenverwaltung.Contracts;
using HqNotenverwaltung.Data.SqlLite;
using HqNotenverwaltung.Model;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqNotenverwaltung.Data
{
    class RepositorySchoolyear : ISchoolyear
    {
        private List<int> _schoolyears = [];
        private readonly IDbManager _dbManager;

        public List<int> Schoolyears
        {
            get
            {
                //TODO: GetSchoolYearsAsync in CTOR and on Updating Schoolyears Delete GetSchoolYears() here
                GetSchoolYears();
                return _schoolyears;
            }
        }

        public ModelSchoolyear ActiveSchoolYear { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Server { get; private set; } = "Data";
        public string Database { get; private set; } = "Schoolyear.sqlite";

        public RepositorySchoolyear(IDbManager dbManager)
        {
            _dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager), "Database manager cannot be null.");  
        }

        public void Connect(string server, string database)
        {
            Server = server;
            Database = database;
            _dbManager.ConectionsStringUpdate(Server, Database);
            _dbManager.CreateTables();
        }

        public async Task ConnectAsync(string server, string database)
        {
            Server = server;
            Database = database;
            _dbManager.ConectionsStringUpdate(Server, Database);
            await _dbManager.CreateTablesAsync();
        }
        public async Task UpsertSchoolyearAsync(int schoolyear, int semestered)
        {
            using DbConnection connection = await _dbManager.GetConnectionAsync();
            if (connection.State != System.Data.ConnectionState.Open) { throw new InvalidOperationException("Database connection is not open."); }
            var commandCreate = _dbManager.CommandsSchoolyearQuery;
            await _dbManager.ExecuteNonQueryAsync(commandCreate.UpsertSchoolyear(connection, schoolyear, semestered));
        }
        public async Task GetSchoolyear(int schoolyear)
        {
            using DbConnection connection = await _dbManager.GetConnectionAsync();
            if (connection.State != System.Data.ConnectionState.Open) { throw new InvalidOperationException("Database connection is not open."); }
            var commandCreate = _dbManager.CommandsSchoolyearQuery;
            using var reader = await _dbManager.ExecuteQueryAsync(commandCreate.GetDays(connection, "DaysStart", schoolyear));
            //TODO: Implement this method properly (GetSchoolyear)
            while (await reader.ReadAsync()) { Debug.WriteLine(reader.GetDateTime(0)); }
            throw new NotImplementedException("fxxx, Got to get the schoolyear");

        }

        private void GetSchoolYears()
        {
            using DbConnection connection = _dbManager.GetConnection();
            if (connection.State != System.Data.ConnectionState.Open) { throw new InvalidOperationException("Database connection is not open."); }
            var commandCreate = _dbManager.CommandsSchoolyearQuery;
            using var reader = _dbManager.ExecuteQuery(commandCreate.GetSchoolyears(connection));
            while (reader.Read()) { _schoolyears.Add(reader.GetInt32(0)); }
        }
        private async Task GetSchoolYearsAsync()
        {
            using DbConnection connection = await _dbManager.GetConnectionAsync();
            if (connection.State != System.Data.ConnectionState.Open) { throw new InvalidOperationException("Database connection is not open."); }
            var commandCreate = _dbManager.CommandsSchoolyearQuery;
            using var reader = await _dbManager.ExecuteQueryAsync(commandCreate.GetSchoolyears(connection));
            while (await reader.ReadAsync()) { _schoolyears.Add(reader.GetInt32(0)); }
        }



        public void Dispose()
        {
            _dbManager.Dispose();
            _schoolyears.Clear();
        }
    }
}
