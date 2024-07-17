using FlowCiao.Builder;
using FlowCiao.IntegrationTests.Fixtures;
using FlowCiao.Interfaces.Builder;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Interfaces.Services;
using FlowCiao.Models.Core;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao.IntegrationTests.Services
{
    public class FlowServiceTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly IFlowRepository _flowRepository;
        public FlowServiceTests(ServiceProviderFixture serviceProviderFixture)
        {
            _flowRepository = serviceProviderFixture.ServiceProvider.GetService<IFlowRepository>();
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
            var result = await _flowRepository.Modify(flow);

            Assert.NotEqual(default, result);
        }

        [Fact]
        public async Task GetAsync_ShouldWork()
        {
            var result = await _flowRepository.Get();

            Assert.NotNull(result);
            Assert.Contains("flowKey", result.Select(x => x.Key));
        }

        [Fact]
        public async Task GetByKeyAsync_ShouldWork()
        {
            var result = await _flowRepository.GetByKey(key: "flowKey");

            Assert.NotEqual(default, result.Id);
        }
    }
}
