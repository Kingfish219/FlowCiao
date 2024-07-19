using FlowCiao.IntegrationTests.Fixtures;
using FlowCiao.Interfaces.Services;
using FlowCiao.Models.Core;
using FlowCiao.Operators;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao.IntegrationTests.Operators;

public class FlowOperatorTests : IClassFixture<ServiceProviderFixture>
{
    private readonly IFlowOperator _flowOperator;
    private readonly IFlowService _flowService;
    
    public FlowOperatorTests(ServiceProviderFixture serviceProviderFixture)
    {
        _flowOperator = serviceProviderFixture.ServiceProvider.GetService<IFlowOperator>();
        _flowService = serviceProviderFixture.ServiceProvider.GetService<IFlowService>();
    }

    [Fact]
    public async Task CiaAsync_shouldWork()
    {
        var flow = new Flow
        {
            Key = "flowKey",
            Name = "flowName",
            IsActive = true,
            CreatedAt = DateTime.Now,
            Transitions = new List<Transition>
            {
                new Transition
                {
                    From = new State(1, "State1") { IsInitial = true },
                    To = new State(2, "State2"),
                    Triggers = new List<Trigger> { new Trigger(1, "Trigger1") }
                }
            },
            States = new List<State>
            {
                new State(1, "State1") { IsInitial = true },
                new State(2, "State2")
            }
        };

        var resultFlow = await _flowService.Modify(flow);

        flow.Id = resultFlow;
        
        var result = await _flowOperator.Ciao(flow);
        
        Assert.NotNull(result);
        Assert.NotEqual(default, result.Id);
    }

    [Fact]
    public async Task CiaoAndTriggerAsync_ShouldWork()
    {
        var flow = new Flow
        {
            Key = "flowKey",
            Name = "flowName",
            IsActive = true,
            CreatedAt = DateTime.Now,
            Transitions = new List<Transition>
            {
                new Transition
                {
                    From = new State(1, "State1") { IsInitial = true },
                    To = new State(2, "State2"),
                    Triggers = new List<Trigger> { new Trigger(1, "Trigger1") }
                }
            },
            States = new List<State>
            {
                new State(1, "State1") { IsInitial = true },
                new State(2, "State2")
            }
        };

        var resultFlow = await _flowService.Modify(flow);

        flow.Id = resultFlow;

        var result = await _flowOperator.CiaoAndTriggerAsync("flowKey",1);
        
       Assert.Equal("completed", result.Status);
       Assert.NotEqual(default, result.InstanceId);
    }
}