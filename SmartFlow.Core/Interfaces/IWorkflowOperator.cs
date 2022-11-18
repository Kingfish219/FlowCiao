using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFlow.Core.Db;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;

namespace SmartFlow.Core
{
    public interface IWorkflowOperator
    {
        Task<ProcessResult> AdvanceAsync(Entity entity,
               ProcessUser user,
               ProcessStepInput input,
               IEntityRepository entityRepository,
               IEntityCreateHistory entityCreateHistory,
               EntityCommandType commandType = EntityCommandType.Update,
               Dictionary<string, object> parameters = null
        );
    }
}
