using SmartFlow.Models;
using SmartFlow.Models.Flow;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFlow.Interfaces
{
    public interface IProcessService
    {
        Task<Guid> Modify(Process process);
        Task<List<Process>> Get(Guid processId = default, string key = default);
        ProcessExecutionStep GenerateProcessStep(Process process, State state);
        Task<ProcessExecution> Finalize(ProcessExecution processExecution);
    }
}
