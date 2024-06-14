using FlowCiao.Handle;
using FlowCiao.Models;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services.Interfaces;

namespace FlowCiao.UnitTests.TestUtils.Handlers;

internal class TestHandler : FlowHandler
{
    public TestHandler(IFlowRepository flowRepository, IFlowService flowStepManager) : base(flowRepository, flowStepManager)
    {
    }

    public override FlowResult Handle(FlowStepContext flowStepContext)
    {
        return new FlowResult();
    }

    public override FlowResult RollBack(FlowStepContext flowStepContext)
    {
        return new FlowResult();
    }
}