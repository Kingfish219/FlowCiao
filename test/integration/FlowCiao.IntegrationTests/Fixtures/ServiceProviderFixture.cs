using FlowCiao.Models;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao.IntegrationTests.Fixtures;

public class ServiceProviderFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; }

    public ServiceProviderFixture()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddFlowCiao(settings =>
        {
            settings
            .Persist(persistenceSettings =>
            {
                persistenceSettings.UseSqlServer("TrustServerCertificate=True;Integrated Security=True;Initial Catalog=FlowCiaoStudio;Data Source=.");
            });
        });
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
}