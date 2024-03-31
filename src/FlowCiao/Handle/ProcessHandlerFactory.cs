using System.Collections.Generic;
using FlowCiao.Handle.Handlers;
using FlowCiao.Persistence.Interfaces;
using FlowCiao.Services;

namespace FlowCiao.Handle
{
    public class ProcessHandlerFactory
    {
        private readonly IProcessRepository _processRepository;
        private readonly IProcessService _processService;
        private readonly ProcessExecutionService _processExecutionService;

        public ProcessHandlerFactory(IProcessRepository processRepository
            , IProcessService processService,
            ProcessExecutionService processExecutionService)
        {
            _processRepository = processRepository;
            _processService = processService;
            _processExecutionService = processExecutionService;
        }

        internal Queue<WorkflowHandler> BuildHandlers()
        {
            return BuildDefaultHandlers(_processRepository, _processService, _processExecutionService);
        }

        private Queue<WorkflowHandler> BuildDefaultHandlers(IProcessRepository processRepository,
            IProcessService processService,
            ProcessExecutionService processExecutionService)
        {
            var triggerHandler = new TriggerHandler(processRepository, processService);
            var triggerActivityHandler = new TriggerActivityHandler(processRepository, processService);
            var transitionHandler = new TransitionHandler(processRepository, processService);
            var transitionActivityHandler = new TransitionActivityHandler(processRepository, processService);
            var stateActivityHandler = new StateActivityHandler(processRepository, processService);
            var processStepFinalizerHandler = new ProcessStepFinalizerHandler(processRepository, processService, processExecutionService);

            triggerHandler.SetNextHandler(triggerActivityHandler);

            triggerActivityHandler.SetNextHandler(transitionHandler);
            triggerActivityHandler.SetPreviousHandler(triggerHandler);

            transitionHandler.SetNextHandler(transitionActivityHandler);
            transitionHandler.SetPreviousHandler(triggerActivityHandler);

            transitionActivityHandler.SetNextHandler(stateActivityHandler);
            transitionActivityHandler.SetPreviousHandler(transitionHandler);

            stateActivityHandler.SetNextHandler(processStepFinalizerHandler);
            stateActivityHandler.SetPreviousHandler(transitionActivityHandler);

            processStepFinalizerHandler.SetPreviousHandler(stateActivityHandler);

            var queue = new Queue<WorkflowHandler>();

            queue.Enqueue(triggerHandler);
            queue.Enqueue(triggerActivityHandler);
            queue.Enqueue(transitionHandler);
            queue.Enqueue(transitionActivityHandler);
            queue.Enqueue(stateActivityHandler);
            queue.Enqueue(processStepFinalizerHandler);

            return queue;
        }
    }
}
