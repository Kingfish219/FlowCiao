using System;
using System.Threading.Tasks;
using FlowCiao.Models.Flow;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    public class ActivityCacheRepository : FlowCacheRepository, IActivityRepository
    {
        public ActivityCacheRepository(FlowHub flowHub) : base(flowHub)
        {
        }

        public async Task<Guid> Modify(Activity entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await FlowHub.ModfiyActivity(entity);

            return entity.Id;
        }
    }
}
