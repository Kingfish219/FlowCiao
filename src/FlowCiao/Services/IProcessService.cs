using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;

namespace FlowCiao.Services
{
    public interface IProcessService
    {
        Task<Guid> Modify(Process process);
        Task<List<Process>> Get(Guid processId = default, string key = default);
        ProcessExecutionStep GenerateProcessStep(Process process, State state);
        Task<ProcessExecution> Finalize(ProcessExecution processExecution);
    }
}
