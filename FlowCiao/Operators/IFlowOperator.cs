using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Models.Flow;

namespace FlowCiao.Operators
{
    public interface IFlowOperator
    {
        Task<ProcessResult> Fire(string flowKey, int action, Dictionary<string, object> data = null);
        Task<State> GetFLowState(string flowKey);
    }
}
