using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Xml.Linq;
using Microsoft.Data.Sqlite;

namespace HqNotenverwaltung.Data.SqlLite;

public class SQLiteDbManager : IDisposable
{
    private string _connectionString ="";
    private SqliteConnection? _connection;

    public string Server { get; private set; } = "Data";
    public string Database { get; private set; }
    public SqliteConnection? Connection { get { return _connection; } }

    public SQLiteDbManager() : this("Schoolyear.sqlite") { }
    public SQLiteDbManager(string server, string name): this(name) 
    {
        Server = server;
    }
    public SQLiteDbManager(string name)
    {
        Database = name;
    }

    public void ConectionsStringUpdate()
    {
        _connection?.Dispose();
        var databasePath = Path.Combine(Server, Database);
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

    public async Task<SqliteConnection> GetOpenConnectionAsync()
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
    public async Task CloseConnectionAsync()
    {
        if (_connection != null) { await _connection.CloseAsync(); }
    }

    public SqliteConnection GetOpenConnection()
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

    public void CloseConnection() {_connection?.Close();}

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
