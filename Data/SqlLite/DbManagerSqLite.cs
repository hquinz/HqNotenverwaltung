using HqNotenverwaltung.Contracts;
using Microsoft.Data.Sqlite;
using System.Data.Common;
using System.IO;
using System.Windows.Input;

namespace HqNotenverwaltung.Data.SqlLite;

public class SQLiteDbManager : IDbManager
{
    private string _connectionString = "";
    private SqliteConnection? _connection;

    #region Properties
    public string Server { get; private set; } = "Data";
    public string Database { get; private set; }
    public DbConnection? Connection { get { return _connection; } }

    public IDbCommandsSchoolyearQuery CommandsSchoolyearQuery => new SqLiteCommandsSchoolyearQuery();
    #endregion

    #region ctors
    public SQLiteDbManager() : this("Schoolyear.sqlite") { }
    public SQLiteDbManager(string server, string name) : this(name) { Server = server;}
    public SQLiteDbManager(string name) { Database = name; }
    #endregion
    #region Connectionshandling
    public void ConectionsStringUpdate()
    {
        _connection?.Dispose();
        _connection = null; 
        var _database = Database.EndsWith(".sqlite") ? Database : $"{Database}.sqlite";
        var databasePath = Path.Combine(Server, _database);
        Directory.CreateDirectory(Server);
        _connectionString = $"Data Source={databasePath}";
    }
    public void ConectionsStringUpdate(string name)
    {
        Database = name;
        ConectionsStringUpdate();
    }
    public void ConectionsStringUpdate(string server, string name)
    {
        Server = server;
        Database = name;
        ConectionsStringUpdate();
    }
    public async Task<DbConnection> GetConnectionAsync()
    {
        if (_connectionString == "") { ConectionsStringUpdate(); }
        if (_connection == null)
        {
            _connection = new SqliteConnection(_connectionString);
            await _connection.OpenAsync();

            using var pragma = _connection.CreateCommand();
            pragma.CommandText = "PRAGMA foreign_keys = ON;";
            await pragma.ExecuteNonQueryAsync();

        }
        else
        {
            await _connection.OpenAsync();
        }

        return _connection;
    }
    public DbConnection GetConnection()
    {
        if (_connectionString == "") { ConectionsStringUpdate(); }
        if (_connection == null)
        {
            _connection = new SqliteConnection(_connectionString);
            _connection.Open();

            using var pragma = _connection.CreateCommand();
            pragma.CommandText = "PRAGMA foreign_keys = ON;";
            pragma.ExecuteNonQuery();

        }
        else
        {
            _connection.Open();
        }

        return _connection;
    }
    public async Task CloseConnectionAsync()
    {
        if (_connection != null) { await _connection.CloseAsync(); }
    }
    public void CloseConnection() { _connection?.Close(); }
    #endregion
    #region CreateTables
    public void CreateTables()
    {
        using var connection = GetConnection() as SqliteConnection ?? throw new InvalidOperationException("Unable to connect database.");
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

    public async Task CreateTablesAsync()
    {
        using var connection = await GetConnectionAsync() as SqliteConnection ?? throw new InvalidOperationException("Unable to connect database.");
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
    #endregion
    #region Commands
    public void ExecuteNonQuery(DbCommand command)
    {
        if (_connection == null) throw new InvalidOperationException("Connection is not established.");
        var _command = command as SqliteCommand ?? throw new ArgumentNullException(nameof(command), "Command cannot be null.");
        _command.Connection ??= _connection;
        _command.ExecuteNonQuery();
    }
    public async Task ExecuteNonQueryAsync(DbCommand command)
    {
        if (_connection == null) throw new InvalidOperationException("Connection is not established.");
        var _command = command as SqliteCommand ?? throw new ArgumentNullException(nameof(command), "Command cannot be null.");
        _command.Connection ??= _connection;
        await _command.ExecuteNonQueryAsync();
    }
    public DbDataReader ExecuteQuery(DbCommand command)
    {
        if (_connection == null) throw new InvalidOperationException("Connection is not established.");
        var _command = command as SqliteCommand ?? throw new ArgumentNullException(nameof(command), "Command cannot be null.");
        _command.Connection ??= _connection;
        return _command.ExecuteReader();
    }

    public async Task<DbDataReader> ExecuteQueryAsync(DbCommand command)
    {
        if (_connection == null) throw new InvalidOperationException("Connection is not established.");
        var _command = command as SqliteCommand ?? throw new ArgumentNullException(nameof(command), "Command cannot be null.");
        _command.Connection ??= _connection;
        return await _command.ExecuteReaderAsync();    
    }


    #endregion
    public void Dispose()
    {

        if (_connection != null)
        {
            _connection.Close();
            _connection.Dispose();
            _connection = null;
        }
    }
}
