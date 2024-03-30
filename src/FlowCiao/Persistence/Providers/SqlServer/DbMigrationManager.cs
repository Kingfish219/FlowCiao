using System.Reflection;
using DbUp;
using DbUp.Helpers;
using FlowCiao.Models;

namespace FlowCiao.Persistence.Providers.SqlServer
{
    public class DbMigrationManager
    {
        private readonly string _connectionString;
        private const string ScriptsPath = "FlowCiao.Persistence.Providers.SqlServer.Migration";

        public DbMigrationManager(FlowSettings settings)
        {
            _connectionString = settings.PersistenceSettings.ConnectionString;
        }

        public bool MigrateUp()
        {
            EnsureDatabase.For.SqlDatabase(_connectionString);

            var result = UpgradeSchema();
            if (!result)
            {
                return false;
            }

            result = UpgradeIfNeeded();
            if (!result)
            {
                return false;
            }
            
            result = UpgradeAlways();

            return result;
        }

        private bool UpgradeSchema()
        {
            var upgradeEngine =
                DeployChanges.To
                    .SqlDatabase(_connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly()
                        , s => s.StartsWith(ScriptsPath + "._00"))
                    .WithTransaction()
                    .LogToConsole()
                    .Build();

            upgradeEngine.TryConnect(out var errMessage);

            return string.IsNullOrEmpty(errMessage) && upgradeEngine.PerformUpgrade().Successful;
        }
        
        private bool UpgradeIfNeeded()
        {
            var upgradeEngine =
                DeployChanges.To
                    .SqlDatabase(_connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly()
                        , s => !s.StartsWith(ScriptsPath + "._02") && !s.StartsWith(ScriptsPath + "._00"))
                    .WithTransaction()
                    .JournalToSqlTable("FlowCiao", "SchemaVersions")
                    .LogToConsole()
                    .Build();

            upgradeEngine.TryConnect(out var errMessage);

            if (!string.IsNullOrEmpty(errMessage))
            {
                return false;
            }

            if (!upgradeEngine.IsUpgradeRequired())
            {
                return true;
            }

            var result = upgradeEngine.PerformUpgrade();
            
            return result.Successful;
        }
        
        private bool UpgradeAlways()
        {
            var upgradeEngine =
                DeployChanges.To
                    .SqlDatabase(_connectionString)
                    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly()
                        , s => s.StartsWith(ScriptsPath + "._02"))
                    .WithTransaction()
                    .JournalTo(new NullJournal())
                    .LogToConsole()
                    .Build();

            upgradeEngine.TryConnect(out var errMessage);

            return string.IsNullOrEmpty(errMessage) && upgradeEngine.PerformUpgrade().Successful;
        }
    }
}
