using System.Linq;
using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace FlowCiao.Persistence.Providers;

public class FlowCiaoDbContext : DbContext
{
    public FlowCiaoDbContext(DbContextOptions<FlowCiaoDbContext> contextOptions) : base(contextOptions)
    {
    }

    public DbSet<Flow> Flows { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<Trigger> Triggers { get; set; }
    public DbSet<Transition> Transitions { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<FlowExecution> FlowExecutions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Flow>()
            .Property(x => x.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();
        
        modelBuilder.Entity<Activity>()
            .Property(x => x.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();
        
        modelBuilder.Entity<Trigger>()
            .Property(x => x.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();
        
        modelBuilder.Entity<Transition>()
            .Property(x => x.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();
        
        modelBuilder.Entity<State>()
            .Property(x => x.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();
        
        modelBuilder.Entity<FlowExecution>()
            .Property(x => x.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();
        
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