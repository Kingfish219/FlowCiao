using System.Reflection;
using DbUp;
using DbUp.Helpers;
using FlowCiao.Models;

namespace FlowCiao.Persistence.Providers.SqlServer
{
    public class DbMigrationManager
    {
        private readonly string _connectionString;
        private const string ScriptsPath = "FlowCiao.Persistence.SqlServer.Migration";

        public DbMigrationManager(FlowSettings settings)
        {
            _connectionString = settings.PersistenceSettings.ConnectionString;
        }

        public bool MigrateUp()
        {
            EnsureDatabase.For.SqlDatabase(_connectionString);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(_connectionString)
                    // .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly()
                    //     , s => s.StartsWith(ScriptsPath))
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .WithTransaction()
                    .JournalTo(new NullJournal())
                    .LogToConsole()
                    .Build();

            upgrader.TryConnect(out var errMessage);

            if (!string.IsNullOrEmpty(errMessage))
            {
                return false;
            }

            //if (upgrader.GetScriptsToExecute().Count == 0) return true;
            var result = upgrader.PerformUpgrade();

            return result.Successful;
        }
    }
}
