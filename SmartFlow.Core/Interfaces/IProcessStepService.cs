
using SmartFlow.Core.Models;
using System;

namespace SmartFlow.Core
{
    public interface IProcessStepService
    {
        ProcessExecutionStep GetActiveProcessStep(Guid userId, ProcessEntity entity);
        ProcessExecutionStep InitializeActiveProcessStep(Process process);
        ProcessExecutionStep InitializeActiveProcessStep(Guid userId, ProcessEntity entity, bool initializeFromFirstState);
        ProcessResult FinalizeActiveProcessStep(ProcessExecutionStep processStep);
    }
}