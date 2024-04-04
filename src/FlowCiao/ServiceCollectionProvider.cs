using System;
using System.Collections.Generic;
using FlowCiao.Builder;
using FlowCiao.Builder.Serialization;
using FlowCiao.Builder.Serialization.Serializers;
using FlowCiao.Handle;
using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;
using FlowCiao.Operators;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Persistence.Providers.Cache;
using FlowCiao.Persistence.Providers.Cache.Repositories;
using FlowCiao.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao
{
    public static class ServiceCollectionProvider
    {
        public static IServiceCollection AddFlowCiao(this IServiceCollection services,
            Action<FlowSettings> settings)
        {
            var flowSettings = new FlowSettings(services);
            settings.Invoke(flowSettings);
            services.AddSingleton(flowSettings);

            AddRepositories(services, flowSettings);
            AddServices(services);
            AddInfra(services);

            return services;
        }

        private static void AddInfra(IServiceCollection services)
        {
            services.AddScoped<IFlowBuilder, FlowBuilder>();
            services.AddScoped<IFlowStepBuilder, FlowStepBuilder>();
            services.AddScoped<IFlowOperator, FlowOperator>();
            services.AddScoped<FlowSerializerHelper>();
            services.AddScoped<FlowJsonSerializer>();
        }

        public static void UseFlowCiao(this IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            var flowSettings = scope.ServiceProvider.GetService<FlowSettings>();
            if (flowSettings.PersistFlow)
            {
                flowSettings.MigrateIfRequired(scope);
            }
        }

        private static void AddRepositories(IServiceCollection services, FlowSettings flowSettings)
        {
            if (!flowSettings.PersistFlow)
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
            services.AddScoped<ITransitionRepository, TransitionCacheRepository>();
            services.AddScoped<IStateRepository, StateCacheRepository>();
            services.AddScoped<ITriggerRepository, TriggerCacheRepository>();
            services.AddScoped<IActivityRepository, ActivityCacheRepository>();
            services.AddScoped<IFlowExecutionRepository, FlowExecutionCacheRepository>();
            services.AddScoped<IFlowRepository, FlowCacheRepository>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<ActivityService>();
            services.AddScoped<TransitionService>();
            services.AddScoped<StateService>();
            services.AddScoped<FlowExecutionService>();
            services.AddScoped<FlowHandlerFactory>();
            services.AddScoped<FlowService>();
        }
    }
}