using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFlow.Core.Db;
using SmartFlow.Core.Models;

namespace SmartFlow.Core
{
    public interface IProcessOperator
    {
        ProcessResult Start(Process process);

        Task<ProcessResult> AdvanceAsync(ProcessEntity entity,
               ProcessUser user,
               ProcessStepInput input,
               IEntityRepository entityRepository,
               EntityCommandType commandType = EntityCommandType.Update,
               Dictionary<string, object> parameters = null
        );
    }
}
