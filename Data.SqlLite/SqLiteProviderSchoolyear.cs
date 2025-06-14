using HqNotenverwaltung.Contracts;
using HqNotenverwaltung.Model;
using Microsoft.Extensions.DependencyInjection;


namespace HqNotenverwaltung.Data.SqlLite

{
    internal class SqLiteProviderSchoolyear : ISchoolyear
    {
        private List<int> _schoolyears = [];

        public List<int> Schoolyears
        {
            get
            {
                getSchoolYears();
                return _schoolyears;
            }
        }

        public ModelSchoolyear ActiveSchoolYear { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Server { get; private set; } = "Data";
        public string Database { get; private set; } = "Schoolyear.sqlite";

        private ServiceCollection services = new();
        private ServiceProvider provider;
        private SQLiteDbManager dbManager;
        private RepositorySchoolyear schoolyearRepo;

        public SqLiteProviderSchoolyear()
        {
            services.AddSingleton(database => new SQLiteDbManager());
            services.AddTransient<RepositorySchoolyear>();
            provider = services.BuildServiceProvider();
            dbManager = provider.GetRequiredService<SQLiteDbManager>();
            schoolyearRepo = provider.GetRequiredService<RepositorySchoolyear>();

        }
        public void Connect(string server, string database)
        {
            Server = server;
            Database = database;
            dbManager.ConectionsStringUpdate(Server, Database);
            schoolyearRepo.CreateTable();
        }

        public async Task ConnectAsync(string server, string database)
        {
            Server = server;
            Database = database;
            dbManager.ConectionsStringUpdate(Server,Database);
            await schoolyearRepo.CreateTableAsync();
//            await schoolyearRepo.Seed();  
        }

        private void getSchoolYears()
        {
            using var connection = dbManager.GetOpenConnection();
            var create = new SqLiteCommandsSchoolyearQuery();
            var cmd = create.GetSchoolyears(connection);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                _schoolyears.Add(reader.GetInt32(0));
            }

        }
        public void Dispose()
        {
            provider.Dispose();
        }

    }
}
