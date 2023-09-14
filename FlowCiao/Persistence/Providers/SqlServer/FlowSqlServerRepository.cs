using System.Data;
using FlowCiao.Models;

namespace FlowCiao.Persistence.Providers.SqlServer
{
    public class FlowSqlServerRepository
    {
        private readonly FlowPersistanceSettings _settings;

        public FlowSqlServerRepository(FlowSettings smartFlowSettings)
        {
            _settings = smartFlowSettings.PersistanceSettings;
        }

        protected IDbConnection GetDbConnection()
        {
            return _settings.GetDbConnection();
        }
    }
}
