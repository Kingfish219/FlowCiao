
namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer
{
    public class FlowSqlServerRepository
    {
        protected FlowCiaoDbContext FlowCiaoDbContext { get; }

        protected FlowSqlServerRepository(FlowCiaoDbContext flowCiaoDbContext)
        {
            FlowCiaoDbContext = flowCiaoDbContext;
        }
    }
}
