using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartFlow.Builders;
using SmartFlow.Exceptions;
using SmartFlow.Handlers;
using SmartFlow.Interfaces;
using SmartFlow.Models;
using SmartFlow.Operators;
using SmartFlow.Persistence;
using SmartFlow.Persistence.Interfaces;
using SmartFlow.Persistence.SqlServer;
using SmartFlow.Persistence.SqlServer.Repositories;
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

            AddRepositories(services);
            AddServices(services);
            services.AddScoped<ISmartFlowBuilder, SmartFlowBuilder>();
            services.AddSingleton<ISmartFlowOperator, SmartFlowOperator>();
            services.AddSingleton<SmartFlowHub>();
            
            if (smartFlowSettings.PersistFlow)
            {
                services.AddDbContext<SmartFlowDbContext>(options =>
                    options.UseSqlServer(smartFlowSettings.ConnectionString)
                );

                var migration = new DbMigrationManager(smartFlowSettings);
                if (!migration.MigrateUp())
                {
                    throw new SmartFlowPersistencyException("Migration failed");
                }
            }

            return services;
        }

        private static void AddRepositories(IServiceCollection services)
        {
            services.AddScoped<TransitionRepository>();
            services.AddScoped<StateRepository>();
            services.AddScoped<ActionRepository>();
            services.AddScoped<ActivityRepository>();
            services.AddScoped<LogRepository>();
            services.AddScoped<IProcessExecutionRepository, ProcessExecutionRepository>();
            services.AddScoped<IProcessRepository, ProcessRepository>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<ActivityService>();
            services.AddScoped<TransitionService>();
            services.AddScoped<StateService>();
            services.AddScoped<IProcessService, ProcessService>();
            services.AddScoped<ProcessExecutionService>();
            services.AddScoped<ProcessHandlerFactory>();
            services.AddScoped<ProcessExecutionService>();
        }
    }
}
