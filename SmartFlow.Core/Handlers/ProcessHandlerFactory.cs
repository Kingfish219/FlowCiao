using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using System.Collections.Generic;

namespace SmartFlow.Core.Handlers
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
            var processStepFinalizerHandler = new ProcessStepFinalizerHandler(processRepository, processStepManager);
            var stateActivityHandler = new StateActivityHandler(processRepository, processStepManager);

            actionHandler.SetNextHandler(actionActivityHandler);

            actionActivityHandler.SetNextHandler(transitionHandler);
            actionActivityHandler.SetPreviousHandler(actionHandler);

            transitionHandler.SetNextHandler(transitionActivityHandler);
            transitionHandler.SetPreviousHandler(actionActivityHandler);

            transitionActivityHandler.SetNextHandler(processStepFinalizerHandler);
            transitionActivityHandler.SetPreviousHandler(transitionHandler);

            processStepFinalizerHandler.SetNextHandler(stateActivityHandler);
            processStepFinalizerHandler.SetPreviousHandler(transitionActivityHandler);

            var queue = new Queue<WorkflowHandler>();

            //if (processStepContext.EntityCommandType == EntityCommandType.Create)
            //{
            //    var entityCommandHandler = new EntityHandler(processRepository, processStepManager, entityRepository.Create);
            //    entityCommandHandler.SetNextHandler(actionHandler);
            //    actionHandler.SetPreviousHandler(entityCommandHandler);
            //    queue.Enqueue(entityCommandHandler);
            //}

            queue.Enqueue(actionHandler);
            queue.Enqueue(actionActivityHandler);
            queue.Enqueue(transitionHandler);
            queue.Enqueue(transitionActivityHandler);
            queue.Enqueue(processStepFinalizerHandler);
            queue.Enqueue(stateActivityHandler);

            return queue;
        }
    }
}
