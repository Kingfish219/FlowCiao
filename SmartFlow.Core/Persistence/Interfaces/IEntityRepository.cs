using System;
using SmartFlow.Core.Models;

namespace SmartFlow.Core.Persistence.Interfaces
{
    public interface IEntityRepository
    {
        ProcessResult Create(ProcessEntity entity);
        ProcessResult ChangeState(ProcessEntity entity, Guid newStateId);
    }
}
