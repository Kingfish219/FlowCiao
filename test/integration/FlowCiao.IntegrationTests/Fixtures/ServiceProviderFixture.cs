using FlowCiao.Models;
using FlowCiao.Persistence.Providers.Rdbms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao.IntegrationTests.Fixtures;

public class ServiceProviderFixture : IDisposable
{
    private readonly string _connectionString =
        "TrustServerCertificate=True;Integrated Security=True;Initial Catalog=FlowCiao_Test;Data Source=.";

    public ServiceProvider ServiceProvider { get; }

    public ServiceProviderFixture()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddFlowCiao(settings =>
        {
            settings
                .Persist(persistenceSettings => { persistenceSettings.UseSqlServer(_connectionString); });
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