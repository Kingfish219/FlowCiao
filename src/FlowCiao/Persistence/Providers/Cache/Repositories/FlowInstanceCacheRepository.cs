using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    internal sealed class FlowInstanceCacheRepository : FlowCiaoCacheRepository, IFlowInstanceRepository
    {
        public FlowInstanceCacheRepository(FlowHub flowHub) : base(flowHub)
        {
        }

        public async Task<List<FlowInstance>> Get(Guid id = default, Guid flowId = default)
        {
            return await Task.Run(() =>
            {
                var db = GetDbConnection();
                var result = (from o in db.FlowExecutions
                              where (flowId == default || o.Flow.Id.Equals(flowId))
                              && (id == default || o.Id.Equals(id))
                              select o).ToList();

                return result;
            });
        }

        public Task<List<FlowInstance>> Get(Guid flowId = default)
        {
            return Task.Run(() =>
            {
                var db = GetDbConnection();
                var result = (from o in db.FlowExecutions
                    where flowId == default || o.Flow.Id.Equals(flowId)
                    select o).ToList();

                return result;
            });
        }

        public Task<FlowInstance> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> Modify(FlowInstance entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await FlowHub.ModifyFlowInstance(entity);

            return entity.Id;
        }

        public async Task<Guid> Update(FlowInstance entity)
        {
            await FlowHub.ModifyFlowInstance(entity);

            return entity.Id;
        }
    }
}
