using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Models.Core;

namespace FlowCiao.Operators
{
    public interface IFlowOperator
    {
        Task<ProcessResult> Fire(string key, int action, Dictionary<string, object> data = null);
        Task<State> GetFLowState(string key);
    }
}
