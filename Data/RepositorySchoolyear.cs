using HqNotenverwaltung.Contracts;
using HqNotenverwaltung.Data.SqlLite;
using HqNotenverwaltung.Data.Tools;
using HqNotenverwaltung.Model;
using HqNotenverwaltung.ViewModel;
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
        private List<int> schoolyears = [];
        private readonly IDbManager dbManager;
        private readonly SchoolyearQueries schoolyearQueries;
        private readonly SchooldayQueries schooldayQeries;
        private ModelSchoolyear modelSchoolyear = new();

        public List<int> Schoolyears
        {
            get
            {
                //TODO: GetSchoolYearsAsync in CTOR and on Updating Schoolyears Delete GetSchoolYears() here
                GetSchoolYears();
                return schoolyears;
            }
        }

        public ModelSchoolyear ActiveSchoolYear { get => modelSchoolyear; }
        public string Server { get; private set; } = "Data";
        public string Database { get; private set; } = "Schoolyear";

        public RepositorySchoolyear(IDbManager dbManager)
        {
            this.dbManager = dbManager ?? throw new ArgumentNullException(nameof(dbManager), "Database manager cannot be null.");  
            this.schoolyearQueries = new SchoolyearQueries(dbManager);  
            this.schooldayQeries = new SchooldayQueries(dbManager);
        }

        public void Connect(string server, string database)
        {
            Server = server;
            Database = database;
            dbManager.ConectionsStringUpdate(Server, Database);
            dbManager.CreateTables();
        }

        public async Task ConnectAsync(string server, string database)
        {
            Server = server;
            Database = database;
            dbManager.ConectionsStringUpdate(Server, Database);
            await dbManager.CreateTablesAsync();
        }
        public async Task UpsertSchoolyearAsync(int schoolyear, int semestered) { await schoolyearQueries.UpsertSchoolyearAsync(schoolyear, semestered); }
        public async Task GetSchoolyearAsync(int schoolyear)
        {
            using DbConnection connection = await dbManager.GetConnectionAsync();
            if (connection.State != System.Data.ConnectionState.Open) { throw new InvalidOperationException("Database connection is not open."); }
            var commandCreate = dbManager.CommandsSchoolyearQuery;
            modelSchoolyear.StartYear = schoolyear;
            modelSchoolyear.Semestered = (EnumSemestered)await schoolyearQueries.GetSchoolyearSemesteredAsync(connection, commandCreate, schoolyear);
            modelSchoolyear.DateStart = await schooldayQeries.GetSchooldaysAsync(connection, commandCreate, schoolyear, "DaysStart");
            modelSchoolyear.DateEnd = await schooldayQeries.GetSchooldaysAsync(connection, commandCreate, schoolyear, "DaysEnd");
            modelSchoolyear.DaysFree = await schooldayQeries.GetSchooldaysAsync(connection, commandCreate, schoolyear, "DaysFree");
        }

        private void GetSchoolYears() { schoolyears = schoolyearQueries.GetSchoolYears(); }
        private async Task GetSchoolYearsAsync() { schoolyears = await schoolyearQueries.GetSchoolYearsAsync(); }



        public void Dispose()
        {
            dbManager.Dispose();
            schoolyears.Clear();
        }
    }
}
