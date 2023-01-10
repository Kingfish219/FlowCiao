using Microsoft.Extensions.DependencyInjection;
using SmartFlow.Core.Db;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using SmartFlow.Core.Services;
using System;
using SmartFlow.Core.Builders;

namespace SmartFlow.Core
{
    public static class ServiceCollectionProvider
    {
        public static IServiceCollection AddSmartFlow(this IServiceCollection services, Action<SmartFlowSettings> settings)
        {
            DapperHelper.EnsureMappings();

            var smartFlowSettings = new SmartFlowSettings();
            settings.Invoke(smartFlowSettings);

            services.AddSingleton(settings);
            services.AddTransient<IProcessRepository, ProcessRepository>();
            services.AddTransient<ProcessService, ProcessService>();
            services.AddTransient<TransitionRepository, TransitionRepository>();
            services.AddTransient<IStateMachineBuilder, StateMachineBuilder>();
            services.AddTransient<ISmartFlowOperator, SmartFlowOperator>();

            return services;
        }
    }
}
