using SmartFlow.Core.Models;

namespace SmartFlow.Core
{
    internal interface IProcessHandler
    {
        void SetNextHandler(IProcessHandler handler);
        void SetPreviousHandler(IProcessHandler handler);
        ProcessResult Handle(ProcessStepContext processStepContext);
        ProcessResult RollBack(ProcessStepContext processStepContext);
    }
}
