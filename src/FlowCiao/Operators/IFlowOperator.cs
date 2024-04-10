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
        Task<FlowResult> FireAsync(FlowInstance flowInstance, int triggerCode, Dictionary<object, object> data = null);
        Task<FlowResult> CiaoAndFireAsync(string flowKey, int trigger, Dictionary<object, object> data = null);
        Task<FlowInstance> Ciao(Flow flow);
        Task<FlowInstance> Ciao(string flowKey);
    }
}
