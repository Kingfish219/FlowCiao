using FlowCiao.Models;
using FlowCiao.Persistence.Providers.Rdbms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao.IntegrationTests.Fixtures;

public class ServiceProviderFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; }

    public ServiceProviderFixture()
    {
        var serviceCollection = new ServiceCollection();
        
        // var configuration = new ConfigurationBuilder()
        //     .AddJsonFile("appsettings.json")
        //     .Build();
        var configuration = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .Build();


        var connectionString = configuration.GetConnectionString("FlowCiao_Test");

        serviceCollection.AddFlowCiao(settings =>
        {
            settings
                .Persist(persistenceSettings => { persistenceSettings.UseSqlServer(connectionString); });
        });
        ServiceProvider = serviceCollection.BuildServiceProvider();

        using var scope = ServiceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<FlowCiaoDbContext>();
        context.Database.Migrate();
    }

    public void Dispose()
    {
        using (var scope = ServiceProvider.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<FlowCiaoDbContext>();
            context.Database.EnsureDeleted();
        }

        ServiceProvider.Dispose();
    }
}