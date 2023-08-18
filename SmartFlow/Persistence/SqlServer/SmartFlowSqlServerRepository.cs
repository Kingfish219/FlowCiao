using SmartFlow.Models;
using System.Data;

namespace SmartFlow.Persistence.SqlServer
{
    public class SmartFlowSqlServerRepository
    {
        private readonly SmartFlowPersistanceSettings _settings;

        public SmartFlowSqlServerRepository(SmartFlowSettings smartFlowSettings)
        {
            _settings = smartFlowSettings.PersistanceSettings;
        }

        protected IDbConnection GetDbConnection()
        {
            return _settings.GetDbConnection();
        }
    }
}
