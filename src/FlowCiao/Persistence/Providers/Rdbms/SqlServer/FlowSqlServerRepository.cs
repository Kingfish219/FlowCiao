using System.Threading.Tasks;
using FlowCiao.Models.Core;
using Microsoft.EntityFrameworkCore;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer
{
    public class FlowSqlServerRepository
    {
        protected FlowCiaoDbContext FlowCiaoDbContext { get; }

        protected FlowSqlServerRepository(FlowCiaoDbContext flowCiaoDbContext)
        {
            FlowCiaoDbContext = flowCiaoDbContext;
        }

        protected async Task<int> CreateAsync<TEntity>(TEntity entity) where TEntity : BaseEntity
        {
            entity.Id = default;
            FlowCiaoDbContext.Entry(entity).State = EntityState.Unchanged;
            await FlowCiaoDbContext.Set<TEntity>().AddAsync(entity);
            var result = await FlowCiaoDbContext.SaveChangesAsync();
            FlowCiaoDbContext.Entry(entity).State = EntityState.Detached;

            return result;
        }
            
        protected async Task<int> CreateNavigationAsync<TEntity>(TEntity entity) where TEntity : BaseNavigationEntity
        {
            FlowCiaoDbContext.Entry(entity).State = EntityState.Unchanged;
            await FlowCiaoDbContext.Set<TEntity>().AddAsync(entity);
            var result = await FlowCiaoDbContext.SaveChangesAsync();
            FlowCiaoDbContext.Entry(entity).State = EntityState.Detached;

            return result;
        }

        protected async Task<int> UpdateAsync<TEntity>(TEntity entity, TEntity existed = null)
            where TEntity : BaseEntity
        {
            entity.Id = existed.Id;
            FlowCiaoDbContext.Entry(entity).State = EntityState.Modified;
            FlowCiaoDbContext.Set<TEntity>().Update(entity);
            var result = await FlowCiaoDbContext.SaveChangesAsync();
            FlowCiaoDbContext.Entry(entity).State = EntityState.Detached;

            return result;
        }
    }
}