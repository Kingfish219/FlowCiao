using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;

namespace FlowCiao.Operators
{
    public interface IFlowOperator
    {
        Task<ProcessResult> FireAsync(Guid processInstanceId, int action, Dictionary<string, object> data = null);
        Task<ProcessResult> Fire(string key, int action, Dictionary<string, object> data = null);
        Task<ProcessExecution> Instantiate(Process process);
        Task<ProcessExecution> Instantiate(string flowKey);
        Task<State> GetFLowState(string key);
    }
}
