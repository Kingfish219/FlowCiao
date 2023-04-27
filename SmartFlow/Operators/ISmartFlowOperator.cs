using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFlow.Interfaces;
using SmartFlow.Models;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Operators
{
    public interface ISmartFlowOperator
    {
        Task<bool> RegisterFlow<TFlow>(TFlow smartFlow) where TFlow : Process, new();
        Task<ProcessResult> ExecuteAsync(ISmartFlow process);
        Task<ProcessResult> AdvanceAsync(ProcessEntity entity,
            ProcessUser user,
            ProcessStepInput input,
            IEntityRepository entityRepository,
            EntityCommandType commandType = EntityCommandType.Update,
            Dictionary<string, object> parameters = null
        );

        ProcessResult Fire(string smartFlowKey, int action, Dictionary<string, object> data = null);
    }
}
