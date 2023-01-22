
using SmartFlow.Core.Models;
using System;

namespace SmartFlow.Core.Db
{
    public interface IEntityRepository
    {
        ProcessResult Create(ProcessEntity entity);
        ProcessResult ChangeState(ProcessEntity entity, Guid newStateId);
    }
}
