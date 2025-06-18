using HqNotenverwaltung.Data.SqlLite;
using System.Data.Common;

namespace HqNotenverwaltung.Contracts
{
    public interface IDbManager : IDisposable
    {
        DbConnection? Connection { get; }
        string Database { get; }
        string Server { get; }

        IDbCommandsSchoolyearQuery CommandsSchoolyearQuery { get; }

        void CloseConnection();
        Task CloseConnectionAsync();
        void ConectionsStringUpdate();
        void ConectionsStringUpdate(string name);
        void ConectionsStringUpdate(string server, string name);
        DbConnection GetConnection();
        Task<DbConnection> GetConnectionAsync();
        void CreateTables();
        Task CreateTablesAsync();
        void ExecuteNonQuery(DbCommand command);
        Task ExecuteNonQueryAsync(DbCommand command);
        DbDataReader ExecuteQuery(DbCommand command);
        Task<DbDataReader> ExecuteQueryAsync(DbCommand command);

    }
}