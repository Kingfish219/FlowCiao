using System.Reflection;
using DbUp;
using DbUp.Helpers;
using SmartFlow.Models;

namespace SmartFlow.Persistence.SqlServer
{
    public class DbMigrationManager
    {
        private readonly string _connectionString;
        private const string ScriptsPath = @"SmartFlow.Core.Persistence.SqlServer.Migration";

        public DbMigrationManager(SmartFlowSettings settings)
        {
            _connectionString = settings.ConnectionString;
        }

        public bool MigrateUp()
        {
            EnsureDatabase.For.SqlDatabase(_connectionString);

            var upgrader =
                DeployChanges.To
                    .SqlDatabase(_connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly()
                        , s => s.StartsWith(ScriptsPath))
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
