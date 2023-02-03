using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFlow.Core.Db;
using SmartFlow.Core.Interfaces;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Operators
{
    public class SmartFlowOperator : ISmartFlowOperator
    {
        private readonly List<ISmartFlow> _flows;

        public SmartFlowOperator()
        {
            _flows = new List<ISmartFlow>();
        }

        public Task<bool> RegisterFlow<TFlow>(TFlow smartFlow) where TFlow : ISmartFlow, new()
        {
            _flows.Add(smartFlow);

            return Task.FromResult(true);
        }

        public Task<ProcessResult> ExecuteAsync(ISmartFlow process)
        {
            throw new NotImplementedException();
        }

        public Task<ProcessResult> AdvanceAsync(ProcessEntity entity, ProcessUser user, ProcessStepInput input, IEntityRepository entityRepository,
            EntityCommandType commandType = EntityCommandType.Update, Dictionary<string, object> parameters = null)
        {
            throw new NotImplementedException();
        }
    }
}
