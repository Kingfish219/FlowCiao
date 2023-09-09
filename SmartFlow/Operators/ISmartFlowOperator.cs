using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFlow.Models;
using SmartFlow.Models.Flow;

namespace SmartFlow.Operators
{
    public interface ISmartFlowOperator
    {
        Task<ProcessResult> Fire(string smartFlowKey, int action, Dictionary<string, object> data = null);
        Task<State> GetFLowState(string smartFlowKey);
    }
}
