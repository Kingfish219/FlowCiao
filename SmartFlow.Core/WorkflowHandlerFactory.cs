using SmartFlow.Core.Db;
using SmartFlow.Core.Handlers;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using System.Collections.Generic;

namespace SmartFlow.Core
{
    internal static class WorkflowHandlerFactory
    {
        internal static Queue<WorkflowHandler> BuildHandlers(
            EntityCommandType commandType,
            Entity entity,
            IProcessRepository processRepository,
            IProcessStepManager processStepManager,
            IEntityRepository entityRepository,
            LogRepository logRepository,
            ProcessStepInput input,
            ProcessUser user,
            string connectionString
            )
        {

            return BuildDefaultHandlers(commandType, entity, processRepository, processStepManager, entityRepository, logRepository, input, user, connectionString);
        }

        private static Queue<WorkflowHandler> BuildDefaultHandlers(
            EntityCommandType commandType
            , Entity entity
            , IProcessRepository processRepository
            , IProcessStepManager processStepManager
            , IEntityRepository entityRepository
            , LogRepository logRepository
            , ProcessStepInput input
            , ProcessUser user
            , string connectionString)
        {
            var actionHandler = new ActionHandler(processRepository, input.ActionCode, logRepository);
            var actionActivityHandler = new ActionActivityHandler(processRepository);
            var transitionHandler = new TransitionHandler(processRepository, entityRepository, input.ActionCode, connectionString, logRepository);
            var transitionActivityHandler = new TransitionActivityHandler(processRepository, input.ActionCode, connectionString, logRepository);
            var processStepFinalizerHandler = new ProcessStepFinalizerHandler(processRepository, processStepManager, input.ActionCode, entity, user.Id);
            var stateActivityHandler = new StateActivityHandler(processRepository, input.ActionCode, connectionString, logRepository);

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

            if (commandType == EntityCommandType.Create)
            {
                var entityCommandHandler = new EntityHandler(processRepository, entityRepository.Create);
                entityCommandHandler.SetNextHandler(actionHandler);
                actionHandler.SetPreviousHandler(entityCommandHandler);
                queue.Enqueue(entityCommandHandler);
            }

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
