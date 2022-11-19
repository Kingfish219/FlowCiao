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
            , ProcessStepContext processStepContext
            , Func<Entity, ProcessResult> command) : base(processRepository, processStepManager, processStepContext)
        {
            _command = command;
        }

        //internal EntityHandler(IProcessRepository processRepository, Func<Entity, ProcessResult> command) : base(processRepository)
        //{
        //    _command = command;
        //}

        public override ProcessResult Handle()
        {
            try
            {
                ProcessStepContext.ProcessStep.Entity.LastStatus = ProcessStepContext.ProcessStep.TransitionActions.FirstOrDefault().Transition.CurrentStateId;
                var result = _command(ProcessStepContext.ProcessStep.Entity);
                if (result.Status != ProcessResultStatus.Completed)
                {
                    return result;
                }

                return NextHandler.Handle();
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

        public override ProcessResult RollBack()
        {
            throw new NotImplementedException();
        }
    }
}
