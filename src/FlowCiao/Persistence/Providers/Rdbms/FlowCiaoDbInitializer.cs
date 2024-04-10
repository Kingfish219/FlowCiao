using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FlowCiao.Persistence.Providers.Rdbms;

internal static class FlowCiaoDbInitializer
{
    public static void Initialize(FlowCiaoDbContext context)
    {
        context.Database.EnsureCreated();
        
        if (context.Database.GetPendingMigrations().Any())
        {
            context.Database.Migrate();
        }
    }
}