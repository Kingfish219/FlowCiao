using FlowCiao.Models.Core;
using FlowCiao.Models.Execution;
using Microsoft.EntityFrameworkCore;

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
}