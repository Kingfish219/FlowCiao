using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    internal sealed class FlowCacheRepository : FlowCiaoCacheRepository, IFlowRepository
    {
        public FlowCacheRepository(FlowHub flowHub) : base(flowHub)
        {
        }

        public Task<List<Flow>> Get()
        {
            return Task.Run(() =>
            {
                var db = GetDbConnection();
                
                return db.Flows;
            });
        }

        public async Task<Flow> GetByKey(Guid id = default, string key = default)
        {
            await Task.CompletedTask;

            return FlowHub.Flows.SingleOrDefault(a =>
                (a.Id == default || a.Id == id) &&
                (string.IsNullOrWhiteSpace(key) || a.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)));
        }

        public async Task<Guid> Modify(Flow entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            var flows = await GetByKey(entity.Id, entity.Key);
            if (flows is not null)
            {
                await FlowHub.DeleteFlow(entity);
            }

            await FlowHub.ModifyFlow(entity);

            return entity.Id;
        }
    }
}
