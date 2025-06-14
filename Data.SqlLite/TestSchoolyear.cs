using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HqNotenverwaltung.Data.SqlLite
{
    public class TestSchoolyear
    {
        public TestSchoolyear()
        {
        }

        public async Task startup()
        {
            Debug.WriteLine("Preparing To create");
            Debug.Flush();

            var services = new ServiceCollection();
            services.AddSingleton(database => new SQLiteDbManager("Schoolyear.sqlite"));
            services.AddTransient<RepositorySchoolyear>();

            var provider = services.BuildServiceProvider();

            var schoolyearRepo = provider.GetRequiredService<RepositorySchoolyear>();

            Debug.WriteLine("Starting To create");
            Debug.Flush();
            await schoolyearRepo.CreateTableAsync();
//            Thread.Sleep(2000);
            Debug.WriteLine("Starting Seed");
            Debug.Flush();

            await schoolyearRepo.Seed();
            Debug.WriteLine("Created Database!!!");
            Debug.Flush();
        }

    }
}
