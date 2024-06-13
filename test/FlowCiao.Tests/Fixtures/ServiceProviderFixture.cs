using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao.Tests.Fixtures;

public class ServiceProviderFixture : IDisposable
{
    public ServiceProvider ServiceProvider { get; }

    public ServiceProviderFixture()
    {
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddFlowCiao();
        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    public void Dispose()
    {
        ServiceProvider.Dispose();
    }
}