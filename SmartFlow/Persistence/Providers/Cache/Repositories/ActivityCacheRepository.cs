using System;
using System.Threading.Tasks;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Persistence.Providers.Cache.Repositories
{
    public class ActivityCacheRepository : SmartFlowCacheRepository, IActivityRepository
    {
        public ActivityCacheRepository(SmartFlowHub smartFlowHub) : base(smartFlowHub)
        {
        }

        public async Task<Guid> Modify(Activity entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await SmartFlowHub.InsertActivity(entity);

            return entity.Id;
        }
    }
}
