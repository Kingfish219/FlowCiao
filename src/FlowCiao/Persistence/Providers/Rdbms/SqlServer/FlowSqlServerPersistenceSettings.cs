using System;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Persistence.Providers.Rdbms.SqlServer.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer;

public class FlowSqlServerPersistenceSettings
{
    private readonly IServiceCollection _serviceCollection;

    public FlowSqlServerPersistenceSettings(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }

    public void UseSqlServer(string connectionString)
    {
        _serviceCollection.AddDbContext<FlowCiaoDbContext>(options =>
            options.UseSqlServer(connectionString,
                x => x.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "FlowCiao")));

        _serviceCollection.AddTransient<ITransitionRepository, TransitionRepository>();
        _serviceCollection.AddTransient<IStateRepository, StateRepository>();
        _serviceCollection.AddTransient<ITriggerRepository, TriggerRepository>();
        _serviceCollection.AddTransient<IActivityRepository, ActivityRepository>();
        _serviceCollection.AddTransient<IFlowInstanceRepository, FlowInstanceRepository>();
        _serviceCollection.AddTransient<IFlowRepository, FlowRepository>();
    }
    
    public void UseInMemoryDatabase()
    {
        _serviceCollection.AddDbContext<FlowCiaoDbContext>(options =>
            options.UseInMemoryDatabase(Guid.NewGuid().ToString()));

        _serviceCollection.AddTransient<ITransitionRepository, TransitionRepository>();
        _serviceCollection.AddTransient<IStateRepository, StateRepository>();
        _serviceCollection.AddTransient<ITriggerRepository, TriggerRepository>();
        _serviceCollection.AddTransient<IActivityRepository, ActivityRepository>();
        _serviceCollection.AddTransient<IFlowInstanceRepository, FlowInstanceRepository>();
        _serviceCollection.AddTransient<IFlowRepository, FlowRepository>();
    }

    internal void Migrate(IServiceScope serviceScope)
    {
        var context = serviceScope.ServiceProvider.GetService<FlowCiaoDbContext>();
        FlowCiaoDbInitializer.Initialize(context);
    }
}