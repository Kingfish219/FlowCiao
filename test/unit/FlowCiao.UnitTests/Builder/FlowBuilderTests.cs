using FlowCiao.Builder;
using FlowCiao.Interfaces;
using FlowCiao.Interfaces.Builder;
using FlowCiao.Interfaces.Services;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.UnitTests.TestUtils.Flows;
using Moq;

namespace FlowCiao.UnitTests.Builder;

public class FlowBuilderTests
{
    private readonly IFlowBuilder _flowBuilder;
    
    public FlowBuilderTests()
    {
        var mockFlowService = new Mock<IFlowService>();
        mockFlowService
            .Setup(service => service.Modify(It.IsAny<Flow>()))
            .Returns(Task.FromResult(Guid.NewGuid()));
        
        var mockStateService = new Mock<IStateService>();
        mockStateService
            .Setup(service => service.Modify(It.IsAny<State>()))
            .Returns(Task.FromResult(new FuncResult<Guid>(true, data: Guid.NewGuid())));
        
        var mockTransitionService = new Mock<ITransitionService>();
        mockTransitionService
            .Setup(service => service.Modify(It.IsAny<Transition>()))
            .Returns(Task.FromResult(new FuncResult<Guid>(true, data: Guid.NewGuid())));
        
        var mockFlowJsonSerializer = new Mock<IFlowJsonSerializer>();

        _flowBuilder = new FlowBuilder(
            mockFlowService.Object,
            mockStateService.Object,
            mockTransitionService.Object,
            mockFlowJsonSerializer.Object
        );
    }
    
    [Fact]
    public void Build_ShouldWork()
    {
        var result = _flowBuilder.Build<SamplePhoneFlow>();
        
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task BuildAsync_ShouldWork()
    {
        var result = await _flowBuilder.BuildAsync<SamplePhoneFlow>();
        
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task BuildAsync_ShouldReturnValidFlow()
    {
        var samplePhoneFlow = new SamplePhoneFlow();
        
        var result = await _flowBuilder.BuildAsync<SamplePhoneFlow>();
        
        Assert.Equal(samplePhoneFlow.Key, result.Key);
        Assert.Single(result.InitialStates);
        Assert.Equal(4, result.States.Count);
        Assert.Equal(5, result.Transitions.Count);
        Assert.Equal(5, result.Triggers.Count);
    }
}