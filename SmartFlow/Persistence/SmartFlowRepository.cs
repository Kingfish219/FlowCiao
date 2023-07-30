using SmartFlow.Models;
using System.Data;

namespace SmartFlow.Persistence
{
    public class SmartFlowRepository
    {
        private readonly SmartFlowPersistanceSettings _settings;

        public SmartFlowRepository(SmartFlowSettings smartFlowSettings)
        {
            _settings = smartFlowSettings.PersistanceSettings;
        }

        protected IDbConnection GetDbConnection()
        {
            return _settings.GetDbConnection();
        }
    }
}
