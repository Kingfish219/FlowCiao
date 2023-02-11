
using SmartFlow.Core.Models;

namespace SmartFlow.Core
{
    public interface IProcessStepService
    {
        ProcessExecutionStep GenerateProcessStep(Process process, State state);
        //ProcessExecutionStep GetActiveProcessStep(Guid userId, ProcessEntity entity);
        //ProcessExecutionStep InitializeActiveProcessStep(Process process);
        //ProcessExecutionStep InitializeActiveProcessStep(Guid userId, ProcessEntity entity, bool initializeFromFirstState);
        ProcessResult FinalizeActiveProcessStep(ProcessExecutionStep processStep);
    }
}