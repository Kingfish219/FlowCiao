using System.Reflection;
using DbUp;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Db
{
    public class DbMigrationManager
    {
        private readonly string _connectionString;
        private const string ScriptsPath = @"SmartFlow.Core.Db.SqlServer.Migration";

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
