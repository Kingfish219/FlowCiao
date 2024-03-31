using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    public class TriggerCacheRepository : FlowCiaoCacheRepository, ITriggerRepository
    {
        public TriggerCacheRepository(FlowHub flowHub) : base(flowHub)
        {
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
