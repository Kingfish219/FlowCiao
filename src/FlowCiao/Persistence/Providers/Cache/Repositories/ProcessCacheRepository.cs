using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Models.Flow;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    internal class ProcessCacheRepository : FlowCacheRepository, IProcessRepository
    {
        public ProcessCacheRepository(FlowHub flowHub) : base(flowHub)
        {
        }

        public async Task<Guid> Modify(Process entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            var process = await Get(entity.Id, entity.Key);
            if (process is not null)
            {
                await FlowHub.DeleteProcess(entity);
            }

            await FlowHub.ModifyProcess(entity);

            return entity.Id;
        }

        public async Task<List<Process>> Get(Guid processId = default, string key = null)
        {
            return await Task.Run(() =>
            {
                var db = GetDbConnection();
                var result = (from o in db.Processes
                              where (string.IsNullOrWhiteSpace(key) || o.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                              && (processId == default || o.Id.Equals(processId))
                              select o).ToList();

                return result;
            });
        }
    }
}
