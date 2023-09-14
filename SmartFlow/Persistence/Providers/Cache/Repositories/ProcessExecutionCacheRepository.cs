using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Models;
using FlowCiao.Persistence.Interfaces;

namespace FlowCiao.Persistence.Providers.Cache.Repositories
{
    internal class ProcessExecutionCacheRepository : FlowCacheRepository, IProcessExecutionRepository
    {
        public ProcessExecutionCacheRepository(FlowHub smartFlowHub) : base(smartFlowHub)
        {
        }

        public async Task<List<ProcessExecution>> Get(Guid id = default, Guid processId = default)
        {
            return await Task.Run(() =>
            {
                var db = GetDbConnection();
                var result = (from o in db.ProcessExecutions
                              where (processId == default || o.Process.Id.Equals(processId))
                              && (id == default || o.Id.Equals(id))
                              select o).ToList();

                return result;
            });
        }

        public async Task<Guid> Modify(ProcessExecution entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }

            await FlowHub.ModifyProcessExecution(entity);

            return entity.Id;
        }
    }
}
