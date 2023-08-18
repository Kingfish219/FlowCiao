﻿using SmartFlow.Models.Flow;
using System;
using System.Threading.Tasks;

namespace SmartFlow.Persistence.Interfaces
{
    public interface IActionRepository
    {
        Task<Guid> Modify(ProcessAction entity);
    }
}