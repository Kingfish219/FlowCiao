using System;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    public class TransitionCacheRepository : FlowCiaoCacheRepository, ITransitionRepository
    {
        public TransitionCacheRepository(FlowHub flowHub) : base(flowHub)
        {
        }

        public async Task<Transition> GetById(Guid id)
        {
            await Task.CompletedTask;
            
            return FlowHub.Transitions.SingleOrDefault(s => s.Id == id);
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
