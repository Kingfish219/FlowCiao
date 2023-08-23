using System;
using System.Threading.Tasks;
using SmartFlow.Models.Flow;
using SmartFlow.Persistence.Interfaces;

namespace SmartFlow.Persistence.Providers.Cache.Repositories
{
    public class ActionCacheRepository : SmartFlowCacheRepository, IActionRepository
    {
        public ActionCacheRepository(SmartFlowHub smartFlowHub) : base(smartFlowHub)
        {
        }

        public async Task<Guid> Modify(ProcessAction entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await SmartFlowHub.InsertAction(entity);

            return entity.Id;
        }
    }
}
