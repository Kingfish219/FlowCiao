using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FlowCiao.Persistence.Providers.Rdbms;

public class FlowCiaoDbContextFactory : IDesignTimeDbContextFactory<FlowCiaoDbContext>
{
    public FlowCiaoDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<FlowCiaoDbContext> dbContextOptionsBuilder =
            new();
        dbContextOptionsBuilder.UseSqlServer("Password=Abc1234;TrustServerCertificate=True;Persist Security Info=True;User ID=sa;Initial Catalog=FlowCiao;Data Source=.");
        
        return new FlowCiaoDbContext(dbContextOptionsBuilder.Options);
    }
}
