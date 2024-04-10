using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.Repositories
{
    public class FlowRepository : FlowSqlServerRepository, IFlowRepository
    {
        public FlowRepository(FlowCiaoDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<Flow> GetByKey(Guid id = default, string key = default)
        {
            return await FlowCiaoDbContext.Flows
                .AsNoTracking()
                .SingleOrDefaultAsync(a =>
                    (id == default || a.Id == id) &&
                    (string.IsNullOrWhiteSpace(key) || a.Key.ToLower().Equals(key.ToLower())));
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
                .Include(f => f.Triggers)
                .AsNoTracking()
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