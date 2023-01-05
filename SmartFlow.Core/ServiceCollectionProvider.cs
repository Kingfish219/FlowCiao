using Microsoft.Extensions.DependencyInjection;
using SmartFlow.Core.Db;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using SmartFlow.Core.Services;
using System;

namespace SmartFlow.Core
{
    public static class ServiceCollectionProvider
    {
        public static IServiceCollection AddSmartFlow(this IServiceCollection services, Action<SmartFlowSettings> settings)
        {
            var smartFlowSettings = new SmartFlowSettings();
            settings.Invoke(smartFlowSettings);

            services.AddSingleton(settings);
            services.AddTransient<IProcessRepository, ProcessRepository>();
            services.AddTransient<ProcessService, ProcessService>();
            services.AddTransient<TransitionRepository, TransitionRepository>();

            return services;
        }
    }
}
