using System;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Models.Core;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    internal sealed class TransitionCacheRepository : FlowCiaoCacheRepository, ITransitionRepository
    {
        public TransitionCacheRepository(FlowHub flowHub) : base(flowHub)
        {
        }

        public async Task<Transition> GetById(Guid id)
        {
            await Task.CompletedTask;
            
            return FlowHub.Transitions.SingleOrDefault(s => s.Id == id);
        }

        public async Task<Transition> GetByKey(Guid flowId, Guid fromStateId, Guid toStateId)
        {
            await Task.CompletedTask;
            
            return FlowHub.Transitions.SingleOrDefault(a => a.FlowId == flowId && a.FromId == fromStateId && a.ToId == toStateId);
        }

        public async Task<Transition> GetByKey(string flowKey, Guid fromStateId, Guid toStateId)
        {
            await Task.CompletedTask;
            
            return FlowHub.Transitions.SingleOrDefault(a => a.Flow.Key == flowKey && a.FromId == fromStateId && a.ToId == toStateId);
        }

        public async Task<Guid> Modify(Transition entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await FlowHub.ModifyTransition(entity);

            return entity.Id;
        }
    }
}
