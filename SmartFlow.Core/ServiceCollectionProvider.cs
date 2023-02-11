using Microsoft.Extensions.DependencyInjection;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using SmartFlow.Core.Services;
using System;
using SmartFlow.Core.Builders;
using SmartFlow.Core.Operators;
using SmartFlow.Core.Persistence.SqlServer;
using SmartFlow.Core.Exceptions;
using SmartFlow.Core.Repositories.Interfaces;

namespace SmartFlow.Core
{
    public static class ServiceCollectionProvider
    {
        public static IServiceCollection AddSmartFlow(this IServiceCollection services, Action<SmartFlowSettings> settings)
        {
            DapperHelper.EnsureMappings();

            var smartFlowSettings = new SmartFlowSettings();
            settings.Invoke(smartFlowSettings);

            services.AddSingleton(smartFlowSettings);
            services.AddSingleton<ISmartFlowOperator, SmartFlowOperator>();
            AddRepositories(services);
            AddServices(services);
            services.AddTransient<ISmartFlowBuilder, SmartFlowBuilder>();

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
            services.AddTransient<IProcessRepository, ProcessRepository>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<ProcessService>();
            services.AddTransient<ActivityService>();
            services.AddTransient<TransitionService>();
            services.AddTransient<StateService>();
        }
    }
}
