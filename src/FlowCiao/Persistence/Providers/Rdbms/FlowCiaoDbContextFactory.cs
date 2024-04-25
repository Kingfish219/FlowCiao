using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace FlowCiao.Persistence.Providers.Rdbms;

internal class FlowCiaoDbContextFactory : IDesignTimeDbContextFactory<FlowCiaoDbContext>
{
    public FlowCiaoDbContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<FlowCiaoDbContext> dbContextOptionsBuilder = new();
        dbContextOptionsBuilder.UseSqlServer(
            "Password=Abc1234;TrustServerCertificate=True;Persist Security Info=True;User ID=sa;Initial Catalog=FlowCiao;Data Source=.");
        dbContextOptionsBuilder.ConfigureWarnings(w => w.Throw(RelationalEventId.MultipleCollectionIncludeWarning));

        return new FlowCiaoDbContext(dbContextOptionsBuilder.Options);
    }
}