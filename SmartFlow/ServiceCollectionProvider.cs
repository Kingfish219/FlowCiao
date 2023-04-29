using System;
using Microsoft.Extensions.DependencyInjection;
using SmartFlow.Builders;
using SmartFlow.Exceptions;
using SmartFlow.Handlers;
using SmartFlow.Interfaces;
using SmartFlow.Models;
using SmartFlow.Operators;
using SmartFlow.Persistence.Interfaces;
using SmartFlow.Persistence.SqlServer;
using SmartFlow.Persistence.SqlServer.Repositories;
using SmartFlow.Services;

namespace SmartFlow
{
    public static class ServiceCollectionProvider
    {
        public static IServiceCollection AddSmartFlow(this IServiceCollection services, Action<SmartFlowSettings> settings)
        {
            DapperHelper.EnsureMappings();

            var smartFlowSettings = new SmartFlowSettings();
            settings.Invoke(smartFlowSettings);

            services.AddSingleton(smartFlowSettings);
            AddRepositories(services);
            AddServices(services);
            services.AddTransient<ISmartFlowBuilder, SmartFlowBuilder>();
            services.AddSingleton<ISmartFlowOperator, SmartFlowOperator>();

            var migration = new DbMigrationManager(smartFlowSettings);
            if (!migration.MigrateUp())
            {
                throw new SmartFlowPersistencyException("Migration failed");
            }

            return services;
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddTransient<TransitionRepository>();
            services.AddTransient<StateRepository>();
            services.AddTransient<ActionRepository>();
            services.AddTransient<ActivityRepository>();
            services.AddTransient<LogRepository>();
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
