using FlowCiao.Handle.Handlers;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services;
using FlowCiao.UnitTests.Fixtures;
using FlowCiao.UnitTests.Fixtures.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao.UnitTests.Handle.Handlers;

public class TriggerHandlerTests : IClassFixture<ServiceProviderFixture>
{
    private readonly IFlowRepository _flowRepository;
    private readonly FlowService _flowService;
    
    public TriggerHandlerTests(ServiceProviderFixture serviceProviderFixture)
    {
        _flowRepository = serviceProviderFixture.ServiceProvider.GetService<IFlowRepository>();
        _flowService = serviceProviderFixture.ServiceProvider.GetService<FlowService>();
    }
    
    [Fact]
    public void Handle_ShouldWork()
    {
        var testHandler = new TestHandler(_flowRepository, _flowService);
        var handler = new TriggerHandler(_flowRepository, _flowService);
        handler.SetNextHandler(testHandler);
        
        var context = new FlowStepContext
        {
            FlowInstanceStepDetail = new FlowInstanceStepDetail()
        };

        var result = handler.Handle(context);
        
        Assert.Equal("completed", result.Status);
    }
}