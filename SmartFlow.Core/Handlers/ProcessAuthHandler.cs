
using SmartFlow.Core.Db;
using SmartFlow.Core.Models;
using System;

namespace SmartFlow.Core.Handlers
{
    internal class ProcessAuthHandler : WorkflowHandler
    {
        public ProcessAuthHandler(IProcessRepository processRepository
            , IProcessStepManager processStepManager
            , ProcessStepContext processStepContext) : base(processRepository, processStepManager, processStepContext)
        {
        }

        public override ProcessResult Handle()
        {
            try
            {
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
            return PreviousHandler.RollBack();
        }
    }
}
