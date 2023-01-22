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

            services.AddSingleton(settings);
            services.AddTransient<ISmartFlowRepository, ProcessRepository>();
            services.AddTransient<SmartFlowService, SmartFlowService>();
            services.AddTransient<TransitionRepository, TransitionRepository>();
            services.AddTransient<ISmartFlowBuilder, SmartFlowBuilder>();
            services.AddTransient<IStateMachineOperator, StateMachineOperator>();

            return services;
        }
    }
}
