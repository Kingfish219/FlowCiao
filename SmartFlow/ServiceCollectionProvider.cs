using System;
using Microsoft.Extensions.DependencyInjection;
using SmartFlow.Builders;
using SmartFlow.Exceptions;
using SmartFlow.Handlers;
using SmartFlow.Interfaces;
using SmartFlow.Models;
using SmartFlow.Operators;
using SmartFlow.Persistence.Interfaces;
using SmartFlow.Persistence.Providers.Cache;
using SmartFlow.Persistence.Providers.Cache.Repositories;
using SmartFlow.Persistence.Providers.SqlServer;
using SmartFlow.Persistence.Providers.SqlServer.Repositories;
using SmartFlow.Services;

namespace SmartFlow
{
    public static class ServiceCollectionProvider
    {
        public static IServiceCollection AddSmartFlow(this IServiceCollection services,
            Action<SmartFlowSettings> settings)
        {
            DapperHelper.EnsureMappings();

            var smartFlowSettings = new SmartFlowSettings();
            settings.Invoke(smartFlowSettings);
            services.AddSingleton(smartFlowSettings);

            AddCacheRepositories(services);
            AddServices(services);
            services.AddTransient<ISmartFlowBuilder, SmartFlowBuilder>();
            services.AddSingleton<ISmartFlowOperator, SmartFlowOperator>();
            
            if (smartFlowSettings.PersistFlow)
            {
                var migration = new DbMigrationManager(smartFlowSettings);
                if (!migration.MigrateUp())
                {
                    throw new SmartFlowPersistencyException("Migration failed");
                }
            }

            return services;
        }

        private static void AddCacheRepositories(IServiceCollection services)
        {
            var smartFlowHub = new SmartFlowHub();
            smartFlowHub.Initiate(new System.Collections.Generic.List<Models.Flow.Process>(),
                new System.Collections.Generic.List<ProcessExecution>());

            services.AddSingleton(smartFlowHub);
            services.AddTransient<ITransitionRepository, TransitionCacheRepository>();
            services.AddTransient<IStateRepository, StateCacheRepository>();
            services.AddTransient<IActionRepository, ActionCacheRepository>();
            services.AddTransient<IActivityRepository, ActivityCacheRepository>();
            services.AddTransient<ILogRepository, LogCacheRepository>();
            services.AddTransient<IProcessExecutionRepository, ProcessExecutionCacheRepository>();
            services.AddTransient<IProcessRepository, ProcessCacheRepository>();
        }

        private static void AddSqlServerRepositories(IServiceCollection services)
        {
            services.AddTransient<ITransitionRepository, TransitionRepository>();
            services.AddTransient<IStateRepository, StateRepository>();
            services.AddTransient<IActionRepository, ActionRepository>();
            services.AddTransient<IActivityRepository, ActivityRepository>();
            services.AddTransient<ILogRepository, LogRepository>();
            services.AddTransient<IProcessExecutionRepository, ProcessExecutionRepository>();
            services.AddTransient<IProcessRepository, ProcessRepository>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<ActivityService>();
            services.AddTransient<TransitionService>();
            services.AddTransient<StateService>();
            services.AddTransient<IProcessService, ProcessService>();
            services.AddTransient<ProcessExecutionService>();
            services.AddTransient<ProcessHandlerFactory>();
            services.AddTransient<ProcessExecutionService>();
        }
    }
}
