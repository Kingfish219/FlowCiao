using SmartFlow.Core.Db;
using SmartFlow.Core.Models;
using System;
using System.Linq;

namespace SmartFlow.Core.Handlers
{
    internal class EntityHandler : WorkflowHandler
    {
        private readonly Func<Entity, ProcessResult> _command;

        public EntityHandler(IProcessRepository processRepository
            , IProcessStepManager processStepManager
            , Func<Entity, ProcessResult> command) : base(processRepository, processStepManager)
        {
            _command = command;
        }

        //internal EntityHandler(IProcessRepository processRepository, Func<Entity, ProcessResult> command) : base(processRepository)
        //{
        //    _command = command;
        //}

        public override ProcessResult Handle(ProcessStepContext processStepContext)
        {
            try
            {
                processStepContext.ProcessStep.Entity.LastStatus = processStepContext.ProcessStep.TransitionActions.FirstOrDefault().Transition.CurrentStateId;
                var result = _command(processStepContext.ProcessStep.Entity);
                if (result.Status != ProcessResultStatus.Completed)
                {
                    return result;
                }

                return NextHandler.Handle(processStepContext);
            }
            catch (Exception exception)
            {
                return new ProcessResult
                {
                    Status = ProcessResultStatus.Failed,
                    Message = exception.Message
                };
            }
        }

        public override ProcessResult RollBack(ProcessStepContext processStepContext)
        {
            return PreviousHandler?.RollBack(processStepContext) ?? new ProcessResult
            {
                Status = ProcessResultStatus.Failed
            };
        }
    }
}
