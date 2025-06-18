using System.Data.Common;

namespace HqNotenverwaltung.Contracts
{
    public interface IDbCommandsSchoolyearQuery
    {
        abstract DbCommand GetDays(DbConnection connection, string table, int schoolyear);
        abstract DbCommand GetSchoolyears(DbConnection connection);
        abstract DbCommand GetSchoolyearSemestered(DbConnection connection, int schoolyear);
        abstract DbCommand UpsertDay(DbConnection connection, int id, string table, int schoolyear, DateOnly day, string remark);
        abstract DbCommand UpsertSchoolyear(DbConnection connection, int schoolyear, int semestered);
    }
}