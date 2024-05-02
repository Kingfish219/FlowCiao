using System.Linq;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;
using Microsoft.EntityFrameworkCore;

namespace FlowCiao.Persistence.Providers.Rdbms;

internal class FlowCiaoDbContext : DbContext
{
    public FlowCiaoDbContext(DbContextOptions<FlowCiaoDbContext> contextOptions) : base(contextOptions)
    {
    }

    public DbSet<Flow> Flows { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Trigger> Triggers { get; set; }
    public DbSet<Transition> Transitions { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<FlowInstance> FlowInstances { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("FlowCiao");

        var cascadeFKs = modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetForeignKeys())
            .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

        foreach (var fk in cascadeFKs)
        {
            fk.DeleteBehavior = DeleteBehavior.NoAction;
        }
        
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FlowCiaoDbContext).Assembly);
    }
}