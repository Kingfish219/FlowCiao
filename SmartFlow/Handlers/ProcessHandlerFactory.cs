using System.Collections.Generic;
using SmartFlow.Interfaces;
using SmartFlow.Models;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Handlers
{
    public class ProcessHandlerFactory
    {
        private readonly IProcessRepository _processRepository;
        private readonly IProcessStepService _processStepService;

        public ProcessHandlerFactory(IProcessRepository processRepository
            , IProcessStepService processStepService)
        {
            _processRepository = processRepository;
            _processStepService = processStepService;
        }

        internal Queue<WorkflowHandler> BuildHandlers(
            ProcessStepContext processStepContext
            )
        {
            return BuildDefaultHandlers(processStepContext, _processRepository, _processStepService);
        }

        private Queue<WorkflowHandler> BuildDefaultHandlers(
             ProcessStepContext processStepContext
            , IProcessRepository processRepository
            , IProcessStepService processStepManager)
        {
            var actionHandler = new ActionHandler(processRepository, processStepManager);
            var actionActivityHandler = new ActionActivityHandler(processRepository, processStepManager);
            var transitionHandler = new TransitionHandler(processRepository, processStepManager);
            var transitionActivityHandler = new TransitionActivityHandler(processRepository, processStepManager);
            var stateActivityHandler = new StateActivityHandler(processRepository, processStepManager);
            var processStepFinalizerHandler = new ProcessStepFinalizerHandler(processRepository, processStepManager);

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
