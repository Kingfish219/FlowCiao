using SmartFlow.Core.Db;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartFlow.Core.Operators
{
    public interface ISmartFlowOperator
    {
        bool RegisterFlow<TFlow>() where TFlow : ISmartFlow, new();
        ProcessResult Execute(ISmartFlow process);
        Task<ProcessResult> ExecuteAsync(ISmartFlow process);
        Task<ProcessResult> AdvanceAsync(ProcessEntity entity,
            ProcessUser user,
            ProcessStepInput input,
            IEntityRepository entityRepository,
            EntityCommandType commandType = EntityCommandType.Update,
            Dictionary<string, object> parameters = null
        );
    }
}
