using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowCiao.Persistence.Providers.SqlServer.Repositories
{
    public class TriggerRepository : FlowSqlServerRepository, ITriggerRepository
    {
        public TriggerRepository(FlowCiaoDbContext dbContext) : base(dbContext) { }

        public async Task<Trigger> GetById(Guid id)
        {
            return await DbContext.Triggers.SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Guid> Modify(Trigger entity)
        {
            var existed = await GetById(entity.Id);
            if (existed != null)
            {
                DbContext.Triggers.Update(entity);
            }
            else
            {
                await DbContext.Triggers.AddAsync(entity);
            }
            
            await DbContext.SaveChangesAsync();

            return entity.Id;
        }
    }
}
