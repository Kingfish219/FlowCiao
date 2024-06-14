using System.Collections.Generic;
using FlowCiao.Handle.Handlers;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Services;

namespace FlowCiao.Handle
{
    internal class FlowHandlerFactory
    {
        private readonly IFlowRepository _flowRepository;
        private readonly FlowService _flowService;
        private readonly FlowInstanceService _flowInstanceService;

        public FlowHandlerFactory(IFlowRepository flowRepository
            , FlowService flowService,
            FlowInstanceService flowInstanceService)
        {
            _flowRepository = flowRepository;
            _flowService = flowService;
            _flowInstanceService = flowInstanceService;
        }

        internal Queue<FlowHandler> BuildHandlers()
        {
            return BuildDefaultHandlers(_flowRepository, _flowService, _flowInstanceService);
        }

        private Queue<FlowHandler> BuildDefaultHandlers(IFlowRepository flowRepository,
            FlowService flowService,
            FlowInstanceService flowInstanceService)
        {
            var triggerHandler = new TriggerHandler(flowRepository, flowService);
            var triggerActivityHandler = new TriggerActivityHandler(flowRepository, flowService);
            var transitionHandler = new TransitionHandler(flowRepository, flowService);
            var transitionActivityHandler = new TransitionActivityHandler(flowRepository, flowService);
            var stateActivityHandler = new StateActivityHandler(flowRepository, flowService);
            var flowStepFinalizerHandler = new FlowStepFinalizerHandler(flowRepository, flowService, flowInstanceService);

            triggerHandler.SetNextHandler(triggerActivityHandler);

            triggerActivityHandler.SetNextHandler(transitionHandler);
            triggerActivityHandler.SetPreviousHandler(triggerHandler);

            transitionHandler.SetNextHandler(transitionActivityHandler);
            transitionHandler.SetPreviousHandler(triggerActivityHandler);

            transitionActivityHandler.SetNextHandler(stateActivityHandler);
            transitionActivityHandler.SetPreviousHandler(transitionHandler);

            stateActivityHandler.SetNextHandler(flowStepFinalizerHandler);
            stateActivityHandler.SetPreviousHandler(transitionActivityHandler);

            flowStepFinalizerHandler.SetPreviousHandler(stateActivityHandler);

            var queue = new Queue<FlowHandler>();

            queue.Enqueue(triggerHandler);
            queue.Enqueue(triggerActivityHandler);
            queue.Enqueue(transitionHandler);
            queue.Enqueue(transitionActivityHandler);
            queue.Enqueue(stateActivityHandler);
            queue.Enqueue(flowStepFinalizerHandler);

            return queue;
        }
    }
}
