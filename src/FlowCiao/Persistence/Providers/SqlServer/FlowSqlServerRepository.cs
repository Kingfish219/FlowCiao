
namespace FlowCiao.Persistence.Providers.SqlServer
{
    public class FlowSqlServerRepository
    {
        protected FlowCiaoDbContext DbContext { get; }

        protected FlowSqlServerRepository(FlowCiaoDbContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
