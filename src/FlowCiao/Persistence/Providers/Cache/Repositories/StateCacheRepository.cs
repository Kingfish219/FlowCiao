using System;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    public class StateCacheRepository : FlowCiaoCacheRepository, IStateRepository
    {
        public StateCacheRepository(FlowHub flowHub) : base(flowHub)
        {
        }

        public async Task<State> GetById(Guid id)
        {
            await Task.CompletedTask;

            return FlowHub.States.SingleOrDefault(s => s.Id == id);
        }

        public async Task<State> GetByKey(int code, Guid flowId)
        {
            await Task.CompletedTask;

            return FlowHub.States.SingleOrDefault(s => s.Code == code && s.FlowId == flowId);
        }

        public async Task<Guid> Modify(State entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await FlowHub.ModifyState(entity);

            return entity.Id;
        }
    }
}