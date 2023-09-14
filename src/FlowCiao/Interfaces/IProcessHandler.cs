using FlowCiao.Models;

namespace FlowCiao.Interfaces
{
    internal interface IProcessHandler
    {
        void SetNextHandler(IProcessHandler handler);
        void SetPreviousHandler(IProcessHandler handler);
        ProcessResult Handle(ProcessStepContext processStepContext);
        ProcessResult RollBack(ProcessStepContext processStepContext);
    }
}
