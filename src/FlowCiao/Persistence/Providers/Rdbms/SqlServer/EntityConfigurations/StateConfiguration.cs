using FlowCiao.Models.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace FlowCiao.Persistence.Providers.Rdbms.SqlServer.EntityConfigurations;

internal class StateConfiguration : IEntityTypeConfiguration<State>
{
    public void Configure(EntityTypeBuilder<State> builder)
    {
        builder.ToTable("State")
            .Property(x => x.Id)
            .HasValueGenerator<SequentialGuidValueGenerator>();
        
        builder.HasMany(e => e.Activities)
            .WithMany(e => e.States)
            .UsingEntity<StateActivity>(
                l => l.HasOne(e => e.Activity).WithMany(e => e.StateActivities).HasForeignKey(e=>e.ActivityId),
                r => r.HasOne(e => e.State).WithMany(e => e.StateActivities).HasForeignKey(e=>e.StateId));
    }
}