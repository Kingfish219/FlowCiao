using SmartFlow.Models;
using SmartFlow.Persistence.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartFlow.Persistence.Cache.Repositories
{
    internal class ProcessExecutionCacheRepository : SmartFlowCacheRepository, IProcessExecutionRepository
    {
        public ProcessExecutionCacheRepository(SmartFlowHub smartFlowHub) : base(smartFlowHub)
        {
        }

        public async Task<List<ProcessExecution>> Get(Guid id = default, Guid processId = default)
        {
            var db = GetDbConnection();
            var result = (from o in db.ProcessExecutions
                          where (processId == default || o.Process.Id.Equals(processId))
                          && (id == default || o.Id.Equals(id))
                          select o).ToList();

            return result;
        }

        public Task<Guid> Modify(ProcessExecution entity)
        {
            throw new NotImplementedException();
        }
    }
}
