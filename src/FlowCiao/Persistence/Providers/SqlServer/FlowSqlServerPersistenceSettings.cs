using System.Data;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Persistence.Providers.SqlServer.Repositories;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao.Persistence.Providers.SqlServer;

public class FlowSqlServerPersistenceSettings
{
    private readonly IServiceCollection _serviceCollection;
    public string ConnectionString { get; private set; }

    public FlowSqlServerPersistenceSettings(IServiceCollection serviceCollection)
    {
        _serviceCollection = serviceCollection;
    }
        
    public void UseSqlServer(string connectionString)
    {
        ConnectionString = connectionString;
            
        _serviceCollection.AddDbContext<FlowCiaoDbContext>(options =>
            options.UseSqlServer(ConnectionString));

        _serviceCollection.AddTransient<ITransitionRepository, TransitionRepository>();
        _serviceCollection.AddTransient<IStateRepository, StateRepository>();
        _serviceCollection.AddTransient<ITriggerRepository, TriggerRepository>();
        _serviceCollection.AddTransient<IActivityRepository, ActivityRepository>();
        _serviceCollection.AddTransient<IFlowExecutionRepository, FlowExecutionRepository>();
        _serviceCollection.AddTransient<IFlowRepository, FlowRepository>();
    }

    public IDbConnection GetDbConnection()
    {
        return new SqlConnection(ConnectionString);
    }
}