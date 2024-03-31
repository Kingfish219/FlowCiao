using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    internal class FlowCacheRepository : Cache.FlowCacheRepository, IFlowRepository
    {
        public FlowCacheRepository(FlowHub flowHub) : base(flowHub)
        {
        }

        public async Task<Guid> Modify(Flow entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            var flows = await Get(entity.Id, entity.Key);
            if (flows is not null)
            {
                await FlowHub.DeleteFlow(entity);
            }

            await FlowHub.ModifyFlow(entity);

            return entity.Id;
        }

        public async Task<List<Flow>> Get(Guid flowId = default, string key = null)
        {
            return await Task.Run(() =>
            {
                var db = GetDbConnection();
                var result = (from o in db.Flows
                              where (string.IsNullOrWhiteSpace(key) || o.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                              && (flowId == default || o.Id.Equals(flowId))
                              select o).ToList();

                return result;
            });
        }
    }
}
