using System;
using SmartFlow.Core.Db;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Handlers
{
    internal class ActionActivityHandler : WorkflowHandler
    {
        public ActionActivityHandler(IProcessRepository processRepository
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
            throw new NotImplementedException();
        }
    }
}
