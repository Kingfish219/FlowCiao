using FlowCiao.IntegrationTests.Fixtures;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using FlowCiao.Models.Core;

namespace FlowCiao.IntegrationTests.Services
{
    public class StateServiceTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly IStateService _stateService;
        private readonly IFlowService _flowService;

        public StateServiceTests(ServiceProviderFixture serviceProviderFixture)
        {
            _stateService = serviceProviderFixture.ServiceProvider.GetService<IStateService>();
            _flowService = serviceProviderFixture.ServiceProvider.GetService<IFlowService>();
        }

        [Fact]
        public async Task ModifyAsync_ShouldWork()
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
            var flowId = await _flowService.Modify(flow);
            if (flowId == default)
            {
                Assert.Fail("modified flow failed");
            }

            flow.Id = flowId;
            
            var state = flow.States.First();
            state.Description = "test modified state at:" + DateTime.Now;
            state.Flow = flow;
            state.FlowId = flowId;
            
            var result = await _stateService.Modify(state);

            Assert.NotEqual(default, result);

        }

    }
}

