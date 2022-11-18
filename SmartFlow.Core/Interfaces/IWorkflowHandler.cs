
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;

namespace SmartFlow.Core
{
    internal interface IWorkflowHandler
    {
        void SetNextHandler(IWorkflowHandler handler);
        //ProcessResult Handle(ProcessStep processStep);
        ProcessResult Handle(ProcessStep processStep, ProcessUser user, ProcessStepContext processStepContext);
        ProcessResult RollBack(ProcessStep processStep);
    }
}
