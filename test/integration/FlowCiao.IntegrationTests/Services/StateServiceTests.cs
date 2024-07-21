using FlowCiao.IntegrationTests.Fixtures;
using FlowCiao.IntegrationTests.TestUtils.Flows;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using FlowCiao.Models.Core;

namespace FlowCiao.IntegrationTests.Services
{
    [Collection("Sequential")]
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
            var flow = Sample.Flow;
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