using System;
using SmartFlow.Models;

namespace SmartFlow.Persistence.Interfaces
{
    public interface IEntityRepository
    {
        ProcessResult Create(ProcessEntity entity);
        ProcessResult ChangeState(ProcessEntity entity, Guid newStateId);
    }
}
