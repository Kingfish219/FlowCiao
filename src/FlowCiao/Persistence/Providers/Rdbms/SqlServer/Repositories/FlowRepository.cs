﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Interfaces.Persistence;
using FlowCiao.Models.Core;
using Microsoft.EntityFrameworkCore;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.Repositories
{
    internal sealed class FlowRepository : FlowSqlServerRepository, IFlowRepository
    {
        public FlowRepository(FlowCiaoDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Flow> GetByKey(Guid id = default, string key = default)
        {
            return await FlowCiaoDbContext.Flows
                .Include(f => f.Transitions)
                .ThenInclude(t => t.From)
                .ThenInclude(s => s.Activities)
                .Include(f => f.Transitions)
                .ThenInclude(t => t.To)
                .ThenInclude(s => s.Activities)
                .Include(f => f.Transitions)
                .ThenInclude(t => t.Activities)
                .Include(f => f.Transitions)
                .ThenInclude(t => t.Triggers)
                .Include(f => f.Transitions)
                .Include(f => f.States)
                .AsNoTracking()
                .AsSplitQuery()
                .SingleOrDefaultAsync(a =>
                    (id == default || a.Id == id) &&
                    (string.IsNullOrWhiteSpace(key) || a.Key.ToLower().Equals(key.ToLower())) &&
                    a.IsActive
                );
        }

        public async Task<List<Flow>> Get()
        {
            return await FlowCiaoDbContext.Flows
                .Include(f => f.Transitions)
                .ThenInclude(t => t.From)
                .ThenInclude(s => s.Activities)
                .Include(f => f.Transitions)
                .ThenInclude(t => t.To)
                .ThenInclude(s => s.Activities)
                .Include(f => f.Transitions)
                .ThenInclude(t => t.Activities)
                .Include(f => f.Transitions)
                .ThenInclude(t => t.Triggers)
                .Include(f => f.Transitions)
                .Include(f => f.States)
                .AsNoTracking()
                .AsSplitQuery()
                .ToListAsync();
        }

        public async Task<Guid> Modify(Flow entity)
        {
            var existed = await GetByKey(entity.Id, entity.Key);
            if (existed != null)
            {
                await UpdateAsync(entity, existed);
            }
            else
            {
                await CreateAsync(entity);
            }

            return entity.Id;
        }
    }
}