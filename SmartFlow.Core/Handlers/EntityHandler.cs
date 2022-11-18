using SmartFlow.Core.Db;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;
using System;
using System.Linq;

namespace SmartFlow.Core.Handlers
{
    internal class EntityHandler : WorkflowHandler
    {
        private readonly Func<Entity, ProcessResult> _command;

        internal EntityHandler(IProcessRepository processRepository, Func<Entity, ProcessResult> command) : base(processRepository)
        {
            _command = command;
        }

        public override ProcessResult Handle(ProcessStep processStep, ProcessUser user, ProcessStepContext processStepContext)
        {
            try
            {
                processStep.Entity.LastStatus = processStep.TransitionActions.FirstOrDefault().Transition.CurrentStateId;
                var result = _command(processStep.Entity);
                if (result.Status != ProcessResultStatus.Completed)     
                {
                    return result;
                }

                return NextHandler.Handle(processStep,user, processStepContext);
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

        public override ProcessResult RollBack(ProcessStep processStep)
        {
            throw new NotImplementedException();
        }
    }
}
