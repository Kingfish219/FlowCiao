using FlowCiao.IntegrationTests.Fixtures;
using FlowCiao.Interfaces.Services;
using FlowCiao.Models.Core;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao.IntegrationTests.Services
{
    public class FlowServiceTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly IFlowService _flowService;

        public FlowServiceTests(ServiceProviderFixture serviceProviderFixture)
        {
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
            var result = await _flowService.Modify(flow);

            Assert.NotEqual(default, result);
        }

        [Fact]
        public async Task GetAsync_ShouldWork()
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

            var result = await _flowService.Get();

            Assert.NotNull(result);
            Assert.Contains("flowKey", result.Select(x => x.Key));
        }

        [Fact]
        public async Task GetByKeyAsync_ShouldWork()
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

            var result = await _flowService.GetByKey(key: "flowKey");

            Assert.NotEqual(default, result.Id);
        }
    }
}