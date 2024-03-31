using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    public class TriggerCacheRepository : FlowCacheRepository, ITriggerRepository
    {
        public TriggerCacheRepository(FlowHub flowHub) : base(flowHub)
        {
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
