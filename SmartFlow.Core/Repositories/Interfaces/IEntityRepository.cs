using SmartFlow.Core.Models;
using System;

namespace SmartFlow.Core.Repositories
{
    public interface IEntityRepository
    {
        ProcessResult Create(ProcessEntity entity);
        ProcessResult ChangeState(ProcessEntity entity, Guid newStateId);
    }
}
