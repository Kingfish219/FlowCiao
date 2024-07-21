using FlowCiao.IntegrationTests.Fixtures;
using FlowCiao.IntegrationTests.TestUtils.Flows;
using FlowCiao.Interfaces.Services;
using FlowCiao.Models.Core;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao.IntegrationTests.Services
{
    [Collection("Sequential")]
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
            var result = await _flowService.Modify(Sample.Flow);

            Assert.NotEqual(default, result);
        }

        [Fact]
        public async Task GetAsync_ShouldWork()
        {
            await _flowService.Modify(Sample.Flow);

            var result = await _flowService.Get();

            Assert.NotNull(result);
            Assert.Contains("flowKey", result.Select(x => x.Key));
        }

        [Fact]
        public async Task GetByKeyAsync_ShouldWork()
        {
            await _flowService.Modify(Sample.Flow);

            var result = await _flowService.GetByKey(key: "flowKey");

            Assert.NotEqual(default, result.Id);
        }
    }
}