using FlowCiao.Handle.Handlers;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Interfaces.Services;
using FlowCiao.Models.Execution;
using FlowCiao.UnitTests.TestUtils.Handlers;
using Moq;

namespace FlowCiao.UnitTests.Handle.Handlers;

public class TransitionHandlerTests
{
    private readonly IFlowRepository _flowRepository;
    private readonly IFlowService _flowService;
    
    public TransitionHandlerTests()
    {
        var mockFlowRepository = new Mock<IFlowRepository>();
        var mockFlowService = new Mock<IFlowService>();

        _flowRepository = mockFlowRepository.Object;
        _flowService = mockFlowService.Object;
    }
    
    [Fact]
    public void Handle_ShouldFail()
    {
        var testHandler = new TestHandler(_flowRepository, _flowService);
        var handler = new TransitionHandler(_flowRepository, _flowService);
        handler.SetNextHandler(testHandler);
        
        var context = new FlowStepContext
        {
            FlowInstanceStepDetail = new FlowInstanceStepDetail()
        };
        
        var result = handler.Handle(context);
        
        Assert.Equal("failed", result.Status);
    }
}