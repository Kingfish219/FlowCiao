﻿using FlowCiao.IntegrationTests.Fixtures;
using FlowCiao.IntegrationTests.TestUtils.Flows;
using FlowCiao.Interfaces.Services;
using FlowCiao.Models.Core;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao.IntegrationTests.Services;

[Collection("Sequential")]
public class TransitionServiceTests : IClassFixture<ServiceProviderFixture>
{
    private readonly ITransitionService _transitionService;
    private readonly IFlowService _flowService;
    private readonly IStateService _stateService;

    public TransitionServiceTests(ServiceProviderFixture serviceProviderFixture)
    {
        _transitionService = serviceProviderFixture.ServiceProvider.GetService<ITransitionService>();
        _flowService = serviceProviderFixture.ServiceProvider.GetService<IFlowService>();
        _stateService = serviceProviderFixture.ServiceProvider.GetService<IStateService>();
    }

    [Fact]
    public async Task ModifyAsync_ShouldWork()
    {
        var flow = Sample.Flow;
        var flowId = await _flowService.Modify(flow);
        if (flowId == default)
        {
            Assert.Fail("Flow Modified Failed");
        }

        flow.Id = flowId;

        var fromState = flow.States.First();
        fromState.Flow = flow;
        fromState.FlowId = flowId;
        var fromStateResult = await _stateService.Modify(fromState);
        fromState.Id = fromStateResult.Data;
        if (fromState.Id == default)
        {
            Assert.Fail("Getting Flow State Failed");
        }

        var toState = flow.States.Last();
        toState.Flow = flow;
        toState.FlowId = flowId;
        var toStateResult = await _stateService.Modify(fromState);
        toState.Id = toStateResult.Data;
        if (toState.Id == default)
        {
            Assert.Fail("Getting Flow State Failed");
        }

        var transition = flow.Transitions.First();
        transition.Flow = flow;
        transition.FlowId = flowId;
        transition.From = fromState;
        transition.FromId = fromState.Id;
        transition.To = toState;
        transition.ToId = toState.Id;
        transition.Name = "transition 1 -" + DateTime.Now.ToShortDateString();

        var result = await _transitionService.Modify(transition);

        Assert.NotEqual(default, result);
    }
}