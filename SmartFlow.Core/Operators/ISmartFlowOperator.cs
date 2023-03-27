using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;
using SmartFlow.Core.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFlow.Core.Operators
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
