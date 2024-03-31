using System;
using System.Collections.Generic;
using FlowCiao.Builders;
using FlowCiao.Exceptions;
using FlowCiao.Handle;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;
using FlowCiao.Operators;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Persistence.Providers.Cache;
using FlowCiao.Persistence.Providers.Cache.Repositories;
using FlowCiao.Persistence.Providers.SqlServer;
using FlowCiao.Persistence.Providers.SqlServer.Repositories;
using FlowCiao.Services;
using Microsoft.Extensions.DependencyInjection;
using FlowCacheRepository = FlowCiao.Persistence.Providers.Cache.Repositories.FlowCacheRepository;

namespace FlowCiao
{
    public static class ServiceCollectionProvider
    {
        public static IServiceCollection AddFlowCiao(this IServiceCollection services,
            Action<FlowSettings> settings)
        {
            try
            {
                var flowSettings = new FlowSettings();
                settings.Invoke(flowSettings);
                services.AddSingleton(flowSettings);

                AddRepositories(services, flowSettings);
                AddServices(services);
                services.AddTransient<IFlowBuilder, FlowBuilder>();
                services.AddSingleton<IFlowOperator, FlowOperator>();
            
                return services;
            }
            catch (Exception ex)
            {
                return services;
            }
        }

        private static void AddRepositories(IServiceCollection services, FlowSettings flowSettings)
        {
            if (flowSettings.PersistFlow)
            {
                AddSqlServerRepositories(services, flowSettings);
            }
            else
            {
                AddCacheRepositories(services);
            }
        }

        private static void AddCacheRepositories(IServiceCollection services)
        {
            var flowHub = new FlowHub();
            flowHub.Initiate(new List<Flow>(),
                new List<FlowExecution>(),
                new List<State>(),
                new List<Transition>(),
                new List<Activity>(),
                new List<Trigger>());

            services.AddSingleton(flowHub);
            services.AddTransient<ITransitionRepository, TransitionCacheRepository>();
            services.AddTransient<IStateRepository, StateCacheRepository>();
            services.AddTransient<ITriggerRepository, TriggerCacheRepository>();
            services.AddTransient<IActivityRepository, ActivityCacheRepository>();
            services.AddTransient<IFlowExecutionRepository, FlowExecutionCacheRepository>();
            services.AddTransient<IFlowRepository, FlowCacheRepository>();
        }

        private static void AddSqlServerRepositories(IServiceCollection services, FlowSettings flowSettings)
        {
            DapperHelper.EnsureMappings();

            var migration = new DbMigrationManager(flowSettings);
            if (!migration.MigrateUp())
            {
                throw new FlowCiaoPersistencyException("Migration failed");
            }

            services.AddTransient<ITransitionRepository, TransitionRepository>();
            services.AddTransient<IStateRepository, StateRepository>();
            services.AddTransient<ITriggerRepository, TriggerRepository>();
            services.AddTransient<IActivityRepository, ActivityRepository>();
            services.AddTransient<IFlowExecutionRepository, FlowExecutionRepository>();
            services.AddTransient<IFlowRepository, FlowRepository>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddTransient<ActivityService>();
            services.AddTransient<TransitionService>();
            services.AddTransient<StateService>();
            services.AddTransient<IFlowService, FlowService>();
            services.AddTransient<FlowExecutionService>();
            services.AddTransient<FlowHandlerFactory>();
            services.AddTransient<FlowExecutionService>();
        }
    }
}
