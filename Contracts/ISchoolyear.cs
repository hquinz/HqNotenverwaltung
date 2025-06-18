
using HqNotenverwaltung.Model;

namespace HqNotenverwaltung.Contracts
{
    public interface ISchoolyear : IDisposable
    {
        string Server { get; }
        string Database { get; }
        List<int> Schoolyears { get; }
        ModelSchoolyear ActiveSchoolYear { get;}
        void Connect(string server, string database);
        Task ConnectAsync(string server, string database);
        Task UpsertSchoolyearAsync(int schoolyear, int semestered);
        Task GetSchoolyearAsync(int  schoolyear);

    }
}