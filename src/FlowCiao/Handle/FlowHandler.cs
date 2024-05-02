using FlowCiao.Interfaces;
using FlowCiao.Models;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services;

namespace FlowCiao.Handle
{
    internal abstract class FlowHandler : IFlowHandler
    {
        internal IFlowHandler NextHandler { get; private set; }
        internal IFlowHandler PreviousHandler { get; private set; }

        protected readonly IFlowRepository FlowRepository;
        protected readonly FlowService FlowService;

        internal FlowHandler(IFlowRepository flowRepository, FlowService flowStepManager)
        {
            FlowRepository = flowRepository;
            FlowService = flowStepManager;
        }

        public void SetNextHandler(IFlowHandler handler)
        {
            NextHandler = handler;
        }

        public void SetPreviousHandler(IFlowHandler handler)
        {
            PreviousHandler = handler;
        }

        public abstract FlowResult Handle(FlowStepContext flowStepContext);
        public abstract FlowResult RollBack(FlowStepContext flowStepContext);
    }
}
