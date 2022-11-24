using SmartFlow.Core.Models;

namespace SmartFlow.Core
{
    internal interface IWorkflowHandler
    {
        void SetNextHandler(IWorkflowHandler handler);
        void SetPreviousHandler(IWorkflowHandler handler);
        ProcessResult Handle(ProcessStepContext processStepContext);
        ProcessResult RollBack(ProcessStepContext processStepContext);
    }
}
