
using SmartFlow.Core.Db;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;
using System;

namespace SmartFlow.Core.Handlers
{
    internal class ProcessAuthHandler : WorkflowHandler
    {
        internal ProcessAuthHandler(IProcessRepository processRepository) : base(processRepository)
        {

        }

        public override ProcessResult Handle(ProcessStep processStep, ProcessUser user, ProcessStepContext processStepContext)
        {
            try
            {
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
            return PreviousHandler.RollBack(processStep);
        }
    }
}
