using FlowCiao.Handle.Handlers;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services.Interfaces;
using FlowCiao.UnitTests.TestUtils.Handlers;
using Moq;

namespace FlowCiao.UnitTests.Handle.Handlers;

public class TriggerHandlerTests
{
    private readonly IFlowRepository _flowRepository;
    private readonly IFlowService _flowService;
    
    public TriggerHandlerTests()
    {
        var mockFlowRepository = new Mock<IFlowRepository>();
        var mockFlowService = new Mock<IFlowService>();

        _flowRepository = mockFlowRepository.Object;
        _flowService = mockFlowService.Object;
    }
    
    [Fact]
    public void Handle_ShouldWork()
    {
        var testHandler = new TestHandler(_flowRepository, _flowService);
        var handler = new TriggerHandler(_flowRepository, _flowService);
        handler.SetNextHandler(testHandler);
        
        var context = new FlowStepContext
        {
            FlowInstanceStepDetail = new FlowInstanceStepDetail()
        };

        var result = handler.Handle(context);
        
        Assert.Equal("completed", result.Status);
    }
    
    [Fact]
    public void Handle_ShouldBeValid()
    {
        var testHandler = new TestHandler(_flowRepository, _flowService);
        var handler = new TriggerHandler(_flowRepository, _flowService);
        handler.SetNextHandler(testHandler);
        
        var context = new FlowStepContext
        {
            FlowInstanceStepDetail = new FlowInstanceStepDetail()
        };

        handler.Handle(context);
        
        Assert.True(context.FlowInstanceStepDetail.IsCompleted);
    }
}