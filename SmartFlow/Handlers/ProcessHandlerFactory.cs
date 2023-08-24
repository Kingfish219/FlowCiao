using System.Collections.Generic;
using SmartFlow.Models;
using SmartFlow.Persistence.Interfaces;
using SmartFlow.Services;

namespace SmartFlow.Handlers
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
            var actionHandler = new ActionHandler(processRepository, processService);
            var actionActivityHandler = new ActionActivityHandler(processRepository, processService);
            var transitionHandler = new TransitionHandler(processRepository, processService);
            var transitionActivityHandler = new TransitionActivityHandler(processRepository, processService);
            var stateActivityHandler = new StateActivityHandler(processRepository, processService);
            var processStepFinalizerHandler = new ProcessStepFinalizerHandler(processRepository, processService, processExecutionService);

            actionHandler.SetNextHandler(actionActivityHandler);

            actionActivityHandler.SetNextHandler(transitionHandler);
            actionActivityHandler.SetPreviousHandler(actionHandler);

            transitionHandler.SetNextHandler(transitionActivityHandler);
            transitionHandler.SetPreviousHandler(actionActivityHandler);

            transitionActivityHandler.SetNextHandler(processStepFinalizerHandler);
            transitionActivityHandler.SetPreviousHandler(transitionHandler);

            stateActivityHandler.SetNextHandler(processStepFinalizerHandler);
            stateActivityHandler.SetNextHandler(transitionActivityHandler);

            processStepFinalizerHandler.SetPreviousHandler(stateActivityHandler);

            var queue = new Queue<WorkflowHandler>();

            queue.Enqueue(actionHandler);
            queue.Enqueue(actionActivityHandler);
            queue.Enqueue(transitionHandler);
            queue.Enqueue(transitionActivityHandler);
            queue.Enqueue(stateActivityHandler);
            queue.Enqueue(processStepFinalizerHandler);

            return queue;
        }
    }
}
