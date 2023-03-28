using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Persistence.Interfaces
{
    public interface IProcessExecutionRepository
    {
        Task<List<ProcessExecution>> Get(Guid id = default, Guid processId = default);
        Task<Guid> Modify(ProcessExecution entity);
    }
}
