using System;
using FlowCiao.Models;

namespace FlowCiao.Persistence.Interfaces
{
    public interface IEntityRepository
    {
        ProcessResult Create(ProcessEntity entity);
        ProcessResult ChangeState(ProcessEntity entity, Guid newStateId);
    }
}
