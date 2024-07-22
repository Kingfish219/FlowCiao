using System;
using System.Threading.Tasks;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Models.Core;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    internal sealed class TriggerCacheRepository : FlowCiaoCacheRepository, ITriggerRepository
    {
        public TriggerCacheRepository(FlowHub flowHub) : base(flowHub)
        {
        }

        public Task<Trigger> GetByKey(int code, Guid transitionId)
        {
            throw new NotImplementedException();
        }

        public Task<Trigger> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> Modify(Trigger entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await FlowHub.ModifyTrigger(entity);

            return entity.Id;
        }
    }
}
