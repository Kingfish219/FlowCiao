using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Models.Core;

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
                (id == default || a.Id == id) &&
                (string.IsNullOrWhiteSpace(key) || a.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase)) &&
                a.IsActive
            );
        }

        public async Task<Guid> Modify(Flow entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await FlowHub.ModifyFlow(entity);

            return entity.Id;
        }

        public async Task<bool> Delete(Flow flow)
        {
            await Task.CompletedTask;
            FlowHub.Flows.RemoveAll(f => f.Id == flow.Id);

            return true;
        }
    }
}