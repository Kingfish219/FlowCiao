using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FlowCiao.Models.Execution;
using FlowCiao.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.Repositories
{
    public class FlowExecutionRepository : FlowSqlServerRepository, IFlowExecutionRepository
    {
        public FlowExecutionRepository(FlowCiaoDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<FlowExecution>> Get(Guid flowId = default)
        {
            return await FlowCiaoDbContext.FlowExecutions
                .AsNoTracking()
                .Where(e => flowId == default || e.Flow.Id == flowId)
                .ToListAsync();
        }

        public async Task<FlowExecution> GetById(Guid id)
        {
            return await FlowCiaoDbContext.FlowExecutions.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Guid> Modify(FlowExecution entity)
        {
            var existed = await GetById(entity.Id);
            if (existed != null)
            {
                FlowCiaoDbContext.FlowExecutions.Update(entity);
            }
            else
            {
                await FlowCiaoDbContext.FlowExecutions.AddAsync(entity);
            }
            
            await FlowCiaoDbContext.SaveChangesAsync();

            return entity.Id;
        }
    }
}