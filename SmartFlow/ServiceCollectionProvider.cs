﻿using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SmartFlow.Builders;
using SmartFlow.Exceptions;
using SmartFlow.Handlers;
using SmartFlow.Models;
using SmartFlow.Models.Flow;
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
            var smartFlowSettings = new SmartFlowSettings();
            settings.Invoke(smartFlowSettings);
            services.AddSingleton(smartFlowSettings);

            AddRepositories(services, smartFlowSettings);
            AddServices(services);
            services.AddTransient<ISmartFlowBuilder, SmartFlowBuilder>();
            services.AddSingleton<ISmartFlowOperator, SmartFlowOperator>();
            
            return services;
        }

        private static void AddRepositories(IServiceCollection services, SmartFlowSettings smartFlowSettings)
        {
            if (smartFlowSettings.PersistFlow)
            {
                AddSqlServerRepositories(services, smartFlowSettings);
            }
            else
            {
                AddCacheRepositories(services);
            }
        }

        private static void AddCacheRepositories(IServiceCollection services)
        {
            var smartFlowHub = new SmartFlowHub();
            smartFlowHub.Initiate(new List<Process>(),
                new List<ProcessExecution>(),
                new List<State>(),
                new List<Transition>(),
                new List<Activity>(),
                new List<ProcessAction>());

            services.AddSingleton(smartFlowHub);
            services.AddTransient<ITransitionRepository, TransitionCacheRepository>();
            services.AddTransient<IStateRepository, StateCacheRepository>();
            services.AddTransient<IActionRepository, ActionCacheRepository>();
            services.AddTransient<IActivityRepository, ActivityCacheRepository>();
            services.AddTransient<ILogRepository, LogCacheRepository>();
            services.AddTransient<IProcessExecutionRepository, ProcessExecutionCacheRepository>();
            services.AddTransient<IProcessRepository, ProcessCacheRepository>();
        }

        private static void AddSqlServerRepositories(IServiceCollection services, SmartFlowSettings smartFlowSettings)
        {
            DapperHelper.EnsureMappings();

            var migration = new DbMigrationManager(smartFlowSettings);
            if (!migration.MigrateUp())
            {
                throw new SmartFlowPersistencyException("Migration failed");
            }

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
