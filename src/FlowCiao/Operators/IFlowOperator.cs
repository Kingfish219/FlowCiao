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
        Task<FlowResult> FireAsync(Guid flowInstanceId, int triggerCode, Dictionary<object, object> data = null);
        Task<FlowResult> FireAsync(FlowExecution flowExecution, int triggerCode, Dictionary<object, object> data = null);
        Task<FlowResult> Fire(string key, int trigger, Dictionary<object, object> data = null);
        Task<FlowExecution> Ciao(Flow flow);
        Task<FlowExecution> Ciao(string flowKey);
        Task<State> GetFLowState(string key);
    }
}
