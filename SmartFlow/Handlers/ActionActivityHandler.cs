using System;
using SmartFlow.Interfaces;
using SmartFlow.Models;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Handlers
{
    internal class ActionActivityHandler : WorkflowHandler
    {
        public ActionActivityHandler(IProcessRepository processRepository
            , IProcessStepService processStepManager) : base(processRepository, processStepManager)
        {
        }

        public override ProcessResult Handle(ProcessStepContext processStepContext)
        {
            try
            {
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
