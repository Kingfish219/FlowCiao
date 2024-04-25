using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.Repositories
{
    public class FlowInstanceRepository : FlowSqlServerRepository, IFlowInstanceRepository
    {
        public FlowInstanceRepository(FlowCiaoDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<FlowInstance>> Get(Guid flowId = default)
        {
            return await FlowCiaoDbContext.FlowInstances
                .AsNoTracking()
                .Include(fi => fi.Flow)
                .Where(e => flowId == default || e.Flow.Id == flowId)
                .ToListAsync();
        }

        public async Task<FlowInstance> GetById(Guid id)
        {
            return await FlowCiaoDbContext.FlowInstances
                .AsNoTracking()
                .Include(fi => fi.Flow)
                    .ThenInclude(f => f.Transitions)
                        .ThenInclude(t => t.Triggers)
                .Include(fi => fi.Flow)
                    .ThenInclude(f => f.Transitions)
                        .ThenInclude(t => t.From)
                .Include(fi => fi.Flow)
                    .ThenInclude(f => f.Transitions)
                        .ThenInclude(t => t.To)
                .AsSplitQuery()
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Guid> Modify(FlowInstance entity)
        {
            var existed = await GetById(entity.Id);
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

        public async Task<Guid> Update(FlowInstance entity)
        {
            await UpdateAsync(entity);

            return entity.Id;
        }
    }
}