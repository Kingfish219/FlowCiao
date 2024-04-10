using System;
using System.Threading.Tasks;
using FlowCiao.Models.Core;
using FlowCiao.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.Repositories
{
    public class TriggerRepository : FlowSqlServerRepository, ITriggerRepository
    {
        public TriggerRepository(FlowCiaoDbContext dbContext) : base(dbContext) { }

        public async Task<Trigger> GetById(Guid id)
        {
            return await FlowCiaoDbContext.Triggers
                .AsNoTracking()
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Guid> Modify(Trigger entity)
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
    }
}
