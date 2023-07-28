using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartFlow.Models;

namespace SmartFlow.Persistence.Interfaces
{
    public interface IProcessExecutionRepository
    {
        Task<List<ProcessExecution>> Get(Guid id = default, Guid processId = default);
        Task<Guid> Modify(ProcessExecution entity);
    }
}
