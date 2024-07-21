using FlowCiao.IntegrationTests.Fixtures;
using FlowCiao.IntegrationTests.TestUtils.Flows;
using FlowCiao.IntegrationTests.TestUtils.Models;
using FlowCiao.Interfaces.Builder;
using FlowCiao.Operators;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao.IntegrationTests.Operators;

[Collection("Sequential")]
public class FlowOperatorTests : IClassFixture<ServiceProviderFixture>
{
    private readonly IFlowOperator _flowOperator;
    private readonly IFlowBuilder _flowBuilder;

    public FlowOperatorTests(ServiceProviderFixture serviceProviderFixture)
    {
        _flowOperator = serviceProviderFixture.ServiceProvider.GetService<IFlowOperator>();
        _flowBuilder = serviceProviderFixture.ServiceProvider.GetService<IFlowBuilder>();
    }

    [Fact]
    public async Task CiaoAsync_ShouldWork()
    {
        var flow = _flowBuilder.Build<SamplePhoneFlow>();

        var result = await _flowOperator.Ciao(flow);

        Assert.NotNull(result);
        Assert.NotEqual(default, result.Id);
    }

    [Fact]
    public async Task CiaoAndTriggerAsync_ShouldWork()
    {
        _flowBuilder.Build<SamplePhoneFlow>();

        var result = await _flowOperator.CiaoAndTriggerAsync("phone", Triggers.Ring.Code);

        Assert.Equal("completed", result.Status);
        Assert.NotEqual(default, result.InstanceId);
    }
}