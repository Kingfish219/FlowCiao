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
                persistenceSettings.UseSqlServer("Password=Abc1234;TrustServerCertificate=True;Persist Security Info=True;User ID=sa;Initial Catalog=FlowCiao;Data Source=(LocalDb)\\MSSQLLocalDB");
            });
        });
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
}