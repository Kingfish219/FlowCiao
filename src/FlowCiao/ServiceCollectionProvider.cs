﻿using System;
using System.Collections.Generic;
using FlowCiao.Builder;
using FlowCiao.Builder.Serialization;
using FlowCiao.Builder.Serialization.Serializers;
using FlowCiao.Handle;
using FlowCiao.Interfaces;
using FlowCiao.Interfaces.Builder;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Interfaces.Services;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;
using FlowCiao.Operators;
using FlowCiao.Persistence.Providers.Cache;
using FlowCiao.Persistence.Providers.Cache.Repositories;
using FlowCiao.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao
{
    public static class ServiceCollectionProvider
    {
        /// <summary>
        /// Adds FlowCiao and its required components to your application
        /// </summary>
        public static IServiceCollection AddFlowCiao(this IServiceCollection services,
            Action<FlowSettings> settings = null)
        {
            var flowSettings = new FlowSettings(services);
            settings?.Invoke(flowSettings);
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
            services.AddScoped<IFlowSerializerHelper, FlowSerializerHelper>();
            services.AddScoped<IFlowJsonSerializer, FlowJsonSerializer>();
        }

        public static void UseFlowCiao(this IApplicationBuilder applicationBuilder)
        {
            using var scope = applicationBuilder.ApplicationServices.GetRequiredService<IServiceScopeFactory>()
                .CreateScope();
            var flowSettings = scope.ServiceProvider.GetService<FlowSettings>();
            if (flowSettings.PersistFlow)
            {
                flowSettings.Migrate(scope);
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
                new List<FlowInstance>(),
                new List<State>(),
                new List<Transition>(),
                new List<Activity>(),
                new List<Trigger>());

            services.AddSingleton(flowHub);
            services.AddScoped<ITransitionRepository, TransitionCacheRepository>();
            services.AddScoped<IStateRepository, StateCacheRepository>();
            services.AddScoped<ITriggerRepository, TriggerCacheRepository>();
            services.AddScoped<IActivityRepository, ActivityCacheRepository>();
            services.AddScoped<IFlowInstanceRepository, FlowInstanceCacheRepository>();
            services.AddScoped<IFlowRepository, FlowCacheRepository>();
        }

        private static void AddServices(IServiceCollection services)
        {
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<TriggerService>();
            services.AddScoped<ITransitionService, TransitionService>();
            services.AddScoped<IStateService, StateService>();
            services.AddScoped<FlowInstanceService>();
            services.AddScoped<FlowHandlerFactory>();
            services.AddScoped<IFlowService, FlowService>();
        }
    }
}