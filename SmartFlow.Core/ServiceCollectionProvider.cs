using Microsoft.Extensions.DependencyInjection;
using SmartFlow.Core.Db;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using SmartFlow.Core.Services;
using System;
using SmartFlow.Core.Builders;
using SmartFlow.Core.Operators;

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
            migration.MigrateUp();

            return services;
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddTransient<TransitionRepository, TransitionRepository>();
            services.AddTransient<StateRepository, StateRepository>();
            services.AddTransient<ActionRepository, ActionRepository>();
            services.AddTransient<ISmartFlowRepository, ProcessRepository>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<SmartFlowService, SmartFlowService>();
        }
    }
}
