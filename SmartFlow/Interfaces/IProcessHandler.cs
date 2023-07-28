using SmartFlow.Models;

namespace SmartFlow.Interfaces
{
    internal interface IProcessHandler
    {
        void SetNextHandler(IProcessHandler handler);
        void SetPreviousHandler(IProcessHandler handler);
        ProcessResult Handle(ProcessStepContext processStepContext);
        ProcessResult RollBack(ProcessStepContext processStepContext);
    }
}
