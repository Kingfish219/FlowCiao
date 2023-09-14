using System;
using System.Threading.Tasks;
using FlowCiao.Models.Flow;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    public class ActionCacheRepository : FlowCacheRepository, IActionRepository
    {
        public ActionCacheRepository(FlowHub smartFlowHub) : base(smartFlowHub)
        {
        }

        public async Task<Guid> Modify(ProcessAction entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await FlowHub.ModifyAction(entity);

            return entity.Id;
        }
    }
}
