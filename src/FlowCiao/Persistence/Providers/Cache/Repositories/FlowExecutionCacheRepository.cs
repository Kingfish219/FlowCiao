using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    internal class FlowExecutionCacheRepository : Cache.FlowCacheRepository, IFlowExecutionRepository
    {
        public FlowExecutionCacheRepository(FlowHub flowHub) : base(flowHub)
        {
        }

        public async Task<List<FlowExecution>> Get(Guid id = default, Guid flowId = default)
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

        public async Task<Guid> Modify(FlowExecution entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await FlowHub.ModifyFlowExecution(entity);

            return entity.Id;
        }
    }
}
