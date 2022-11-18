
using SmartFlow.Core.Models;
using System;

namespace SmartFlow.Core.Db
{
    public interface IEntityRepository
    {
        ProcessResult Create(Entity entity);
        ProcessResult ChangeState(Entity entity, Guid newStateId);
    }
}
