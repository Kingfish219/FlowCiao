using FlowCiao.IntegrationTests.Fixtures;
using FlowCiao.IntegrationTests.TestUtils.Flows;
using FlowCiao.Interfaces;
using FlowCiao.Interfaces.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace FlowCiao.IntegrationTests.Builder
{
    public class FlowBuilderTests : IClassFixture<ServiceProviderFixture>
    {
        private readonly IFlowBuilder _flowBuilder;
    
        public FlowBuilderTests(ServiceProviderFixture serviceProviderFixture)
        {
            _flowBuilder = serviceProviderFixture.ServiceProvider.GetService<IFlowBuilder>();
        }
    
        [Fact]
        public void Build_ShouldWork()
        {
            var result = _flowBuilder.Build<SamplePhoneFlow>();
        
            Assert.NotEqual(result.Id, default);
        }
    
        [Fact]
        public async Task BuildAsync_ShouldWork()
        {
            var result = await _flowBuilder.BuildAsync<SamplePhoneFlow>();
        
            Assert.NotEqual(result.Id, default);
        }
    
        [Fact]
        public async Task BuildAsync_ShouldReturnValidFlow()
        {
            var samplePhoneFlow = new SamplePhoneFlow();
        
            var result = await _flowBuilder.BuildAsync<SamplePhoneFlow>();
        
            Assert.NotEqual(result.Id, default);
            Assert.Equal(samplePhoneFlow.Key, result.Key);
            Assert.Single(result.InitialStates);
            Assert.Equal(4, result.States.Count);
            Assert.Equal(5, result.Transitions.Count);
            Assert.Equal(5, result.Triggers.Count);
        }
    }
}