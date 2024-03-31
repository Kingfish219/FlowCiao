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
            return await DbContext.Flows.SingleOrDefaultAsync(a =>
                (a.Id == default || a.Id == id) &&
                (string.IsNullOrWhiteSpace(key) || a.Key.ToLower().Equals(key.ToLower())));
        }

        public async Task<List<Flow>> Get()
        {
            return await DbContext.Flows.ToListAsync();
        }

        public async Task<Guid> Modify(Flow entity)
        {
            var existed = await GetByKey(entity.Id, entity.Key);
            if (existed != null)
            {
                DbContext.Flows.Update(entity);
            }
            else
            {
                await DbContext.Flows.AddAsync(entity);
            }
            
            await DbContext.SaveChangesAsync();

            return entity.Id;
        }
    }
}