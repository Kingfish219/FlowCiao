using System;
using FlowCiao.Persistence.Providers.Rdbms.SqlServer;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao.Models
{
    public class FlowSettings
    {
        private readonly IServiceCollection _serviceCollection;
        public bool PersistFlow { get; private set; }
        private FlowSqlServerPersistenceSettings PersistenceSettings { get; set; }

        public FlowSettings(IServiceCollection serviceCollection)
        {
            _serviceCollection = serviceCollection;
        }
        
        public FlowSettings Persist(Action<FlowSqlServerPersistenceSettings> settings)
        {
            PersistFlow = true;
            PersistenceSettings = new FlowSqlServerPersistenceSettings(_serviceCollection);
            settings(PersistenceSettings);
            
            return this;
        }
        
        internal void MigrateIfRequired(IServiceScope serviceScope)
        {
            if (!PersistFlow)
            {
                return;
            }
            
            PersistenceSettings.Migrate(serviceScope);
        }
    }
}
