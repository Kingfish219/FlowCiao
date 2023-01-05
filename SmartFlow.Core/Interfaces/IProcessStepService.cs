
using SmartFlow.Core.Models;
using System;

namespace SmartFlow.Core
{
    internal interface IProcessStepService
    {
        ProcessStep GetActiveProcessStep(Guid userId, ProcessEntity entity);
        ProcessStep InitializeActiveProcessStep(Guid userId, ProcessEntity entity, bool initializeFromFirstState);
        ProcessResult FinalizeActiveProcessStep(ProcessStep processStep);
    }
}