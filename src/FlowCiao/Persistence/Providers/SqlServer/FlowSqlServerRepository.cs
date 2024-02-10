using System.Data;
using FlowCiao.Models;

namespace FlowCiao.Persistence.Providers.SqlServer
{
    public class FlowSqlServerRepository
    {
        private readonly FlowPersistenceSettings _settings;

        public FlowSqlServerRepository(FlowSettings flowSettings)
        {
            _settings = flowSettings.PersistenceSettings;
        }

        protected IDbConnection GetDbConnection()
        {
            return _settings.GetDbConnection();
        }
    }
}
